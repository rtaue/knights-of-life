using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour {

    HandManager handManager;
    Inventory inventory;

    public Item item;
    public GameObject objItem;

    public string targetItemName;

	// Use this for initialization
	void Start () {

        handManager = HandManager.instance;
        inventory = Inventory.instance;

	}
	
	// Update is called once per frame
	void Update () {
		
        if (item != null)
        {

            objItem.SetActive(true);

        }
        else
        {

            objItem.SetActive(false);

        }

	}

    public void UseItemOnHand(Item newItem)
    {

        if (newItem.name == targetItemName)
        {

            item = newItem;

            item.instance.transform.SetParent(transform);
            item.instance.transform.localPosition = Vector3.zero;
            item.instance.gameObject.SetActive(false);

            handManager.RemoveWhenUsed();

            Debug.Log("Item used: " + item.name);

            return;

        }

        Debug.Log("Wrong item");

    }

    public void PickUpItem()
    {

        if (item != null)
        {

            Debug.Log("Picking up " + item.name);
            bool wasPickedUp = inventory.Add(item);

            if (wasPickedUp)
            {

                item.instance.transform.SetParent(inventory.inventoryParent);
                item.instance.transform.localPosition = Vector3.zero;

                item = null; 

            }
                

        }  

    }

    public void OnTriggerEnter(Collider other)
    {
        
        

        if (other.gameObject.CompareTag("Death"))
        {

            DeathController dController = other.GetComponent<DeathController>();
            if(dController != null)
            {

                if (item != null)
                {

                    Debug.Log("Death: There's already a item");
                                      
                }
                else if (dController.currentItem.name == targetItemName)
                {

                    Debug.Log("Death: Used item");

                    item = dController.currentItem;

                    item.instance.transform.SetParent(transform);
                    item.instance.transform.localPosition = Vector3.zero;
                    item.instance.gameObject.SetActive(false);

                    dController.currentItem = null;

                }
                else
                {

                    Debug.Log("Death: Wrong item");

                }


            }

        }

    }

    public void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("NPC"))
        {

            Vector3 dir = other.transform.position - this.transform.position;

            NPCController npcController = other.GetComponent<NPCController>();
            if (item && npcController != null && dir.magnitude < 2)
            {

                npcController.npcState = NPCController.NPCState.Dead;

            }

        }

    }

}
