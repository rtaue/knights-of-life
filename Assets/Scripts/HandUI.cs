using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandUI : MonoBehaviour {

    public HandSlot slot;

    HandManager handManager;

    // Use this for initialization
    void Start()
    {

        handManager = HandManager.instance;
        handManager.onItemChangedCallback += UpdateUI;

    }

    void UpdateUI()
    {

        if (handManager.currentItem)
            slot.AddItem(handManager.currentItem);
        else
            slot.ClearSlot();

        Debug.Log("UPDATING UI");

    }
}
