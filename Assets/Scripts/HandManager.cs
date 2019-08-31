using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour {

    #region Singleton

    public static HandManager instance;

    private void Awake()
    {

        instance = this;

    }

    #endregion

    Inventory inventory;

    public Transform[] handOpt = new Transform[2];
    public Transform hand;

    public Transform itemOnWorld;

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public Item currentItem;

    private void Start()
    {

        inventory = Inventory.instance;

        if (MainData.isPlayerMale)
        {

            hand = handOpt[0];

        }
        else
        {

            hand = handOpt[1];

        }

    }

    public void Equip(Item newItem)
    {

        Item oldItem = null;

        if(currentItem != null)
        {

            oldItem = currentItem;

            inventory.Add(oldItem);

        }

        currentItem = newItem;

        currentItem.instance.transform.SetParent(hand);
        currentItem.instance.transform.localPosition = Vector3.zero;
        currentItem.instance.SetActive(true);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();

    }

    public void Remove()
    {

        currentItem.instance.transform.SetParent(itemOnWorld);

        SphereCollider sCollider = currentItem.instance.GetComponentInChildren<SphereCollider>();
        if (sCollider)
            sCollider.enabled = true;

        BoxCollider bCollider = currentItem.instance.GetComponent<BoxCollider>();
        if (bCollider)
            bCollider.enabled = true;

        Rigidbody rdb = currentItem.instance.GetComponent<Rigidbody>();
        if (rdb)
            rdb.isKinematic = false;

        currentItem = null;

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();

    }

    public void RemoveWhenUsed()
    {

        currentItem = null;

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();

    }

}
