using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : Interactable {

    Inventory inventory;
    DeathController dController;
    NPCController npcController;

    private Item item;
    public Item targetItem;
    public GameObject objItem;

    

    void Start()
    {

        inventory = Inventory.instance;
        dController = DeathController.instance;
        npcController = NPCController.instance;

        interactionSign.enabled = false;

    }

    void Update()
    {

        triggerArea.radius = radius;


        if (interactionSign.enabled)
            ChangeIcon();

        ItemActivation();

    }

    public override void Interact()
    {
        base.Interact();

        if (item == null)
        {

            UseItemOnInventory();

        }
        else if (item != null)
        {

            PickUpItem();

        }

    }

    public override void DeathInteract()
    {
        base.DeathInteract();

        if (item != null)
        {

            Debug.Log("Death: There's already a item");

        }
        else if (dController.currentItem.name == targetItem.name)
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

    public override void VictimInteract()
    {
        base.VictimInteract();

        if (item)
        {

            Debug.Log("Victim: AAAAAAAAAAAAAAAAAAAAA");
            npcController.npcState = NPCController.NPCState.Dead;

        }
        else
        {

            Debug.Log("Victim: Interacting safely");

        }

    }

    public void UseItemOnInventory()
    {

        for (int i = 0; i < inventory.items.Count; i++)
        {

            if (inventory.items[i].name == targetItem.name)
            {

                item = inventory.items[i];

                item.instance.transform.SetParent(transform);
                item.instance.transform.localPosition = Vector3.zero;
                item.instance.gameObject.SetActive(false);

                inventory.RemoveWhenUsed(inventory.items[i]);

                Debug.Log("Item used: " + item.name);

                return;

            }

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

    void ChangeIcon()
    {

        if (item == null)
        {

            interactionSign.sprite = targetItem.icon;

        }
        else
        {

            interactionSign.sprite = interactionIcon;

        }

    }

    void ItemActivation()
    {

        if (item != null)
        {

            objItem.SetActive(true);

        }
        else
        {

            objItem.SetActive(false);

        }

    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {

            Debug.Log(transform.name + " focused by: " + other.gameObject.name);

            interactionSign.enabled = true;

            PlayerControllerCC player = other.gameObject.GetComponent<PlayerControllerCC>();
            if (player != null)
            {

                player.onInteracting = Interact;

            }

        }

        if (other.gameObject.CompareTag("Death"))
        {

            DeathController dController = other.GetComponent<DeathController>();
            if (dController != null)
            {

                dController.onInteracting = DeathInteract;


            }

        }

        if (other.gameObject.CompareTag("NPC"))
        {

            NPCController npcController = other.GetComponent<NPCController>();
            if (npcController != null)
            {

                npcController.onInteracting = VictimInteract;

            }

        }

    }

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {

            PlayerControllerCC player = other.gameObject.GetComponent<PlayerControllerCC>();
            if (player != null)
            {
                player.onInteracting = null;
                player.onInteracting = Interact;

            }

        }

    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {

            Debug.Log(transform.name + " not focused by: " + other.gameObject.name);

            interactionSign.enabled = false;

            PlayerControllerCC player = other.gameObject.GetComponent<PlayerControllerCC>();
            if (player != null)
            {

                player.onInteracting -= Interact;

            }

        }

        if (other.gameObject.CompareTag("Death"))
        {

            DeathController dController = other.GetComponent<DeathController>();
            if (dController != null)
            {

                dController.onInteracting -= DeathInteract;


            }

        }

        if (other.gameObject.CompareTag("NPC"))
        {

            NPCController npcController = other.GetComponent<NPCController>();
            if (npcController != null)
            {

                npcController.onInteracting -= VictimInteract;

            }

        }

    }

}
