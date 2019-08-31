using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    #region Singleton

    public static Inventory instance;

    private void Awake()
    {
        if (instance != null)
        {

            Debug.LogWarning("More than one instance of Inventory found!");
            return;

        }

        instance = this;
    }

    #endregion

    public Transform[] inventoryOpt = new Transform[2];
    public Transform inventoryParent;

    public Transform itemOnWorld;

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int space = 3;

    public List<Item> items = new List<Item>();

    private void Start()
    {
        
        if (MainData.isPlayerMale)
        {

            inventoryParent = inventoryOpt[0];

        }
        else
        {

            inventoryParent = inventoryOpt[1];

        }

    }

    public bool Add(Item item)
    {

        if (items.Count >= space)
        {

            Debug.Log("Not enough room.");
            return false;

        }

        items.Add(item);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();

        return true;

    }

    public void RemoveFromInventory (Item item)
    {

        item.instance.transform.SetParent(itemOnWorld);
        item.instance.SetActive(true);

        

        SphereCollider sCollider = item.instance.GetComponentInChildren<SphereCollider>();
        if (sCollider)
            sCollider.enabled = true;

        Collider collider = item.instance.GetComponent<MeshCollider>();
        if (collider)
            collider.enabled = true;

        Rigidbody rdb = item.instance.GetComponent<Rigidbody>();
        if (rdb)
            rdb.isKinematic = false;

        items.Remove(item);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();

    }

    
    public void RemoveWhenUsed(Item item)
    {

        items.Remove(item);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();

    }

}
