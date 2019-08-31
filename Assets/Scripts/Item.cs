using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {

    new public string name = "New Item";
    public string description = "Item description";
    public Sprite icon = null;

    public GameObject instance;

    /*
    public virtual void Use()
    {

        Debug.Log("Using " + name);

        HandManager.instance.Equip(this);

        RemoveFromInventory();

    }

    public void RemoveFromInventory()
    {

        Inventory.instance.RemoveWhenUsed(this);

    }
    */

}
