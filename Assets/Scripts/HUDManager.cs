using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour {

    public GameObject messagePanel;
    public GameObject inventoryUI;

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {

            OpenInventory();

        }

    }

    public void OpenMessagePanel()
    {

        messagePanel.SetActive(!messagePanel.activeSelf);

    }

    public void OpenInventory()
    {

        inventoryUI.SetActive(!inventoryUI.activeSelf);

    }

}
