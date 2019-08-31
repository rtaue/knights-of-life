using UnityEngine;

public class ItemPickup : Interactable {

    Inventory inventory;

    public Item item;
    public Collider itemCollider;
    public Rigidbody rdb;

    private void Start()
    {
        interactionSign.sprite = interactionIcon;
        interactionSign.enabled = false;

        inventory = Inventory.instance;

        item.instance = gameObject;

        if (!itemCollider)
        {

            if (GetComponent<MeshCollider>())
            {

                itemCollider = GetComponent<MeshCollider>();

            }
            else if (GetComponent<BoxCollider>())
            {

                itemCollider = GetComponent<BoxCollider>();

            }
            else if (GetComponent<SphereCollider>())
            {

                itemCollider = GetComponent<SphereCollider>();

            }
            else if (GetComponent<CapsuleCollider>())
            {

                itemCollider = GetComponent<CapsuleCollider>();

            }

        }

        if (!rdb)
        {

            rdb = GetComponent<Rigidbody>();

        }

    }

    public override void Interact()
    {
        base.Interact();

        PickUp();

    }

    void PickUp()
    {

        Debug.Log("Picking up " + item.name);
        bool wasPickedUp = inventory.Add(item);

        if (wasPickedUp)
        {

            TransferToInventory();

            DisableItem();

        }
            

    }

    void TransferToInventory()
    {

        transform.SetParent(inventory.inventoryParent);
        transform.localPosition = Vector3.zero;

    }

    void DisableItem()
    {

        interactionSign.enabled = false;

        base.triggerArea.enabled = false;
        itemCollider.enabled = false;
        rdb.isKinematic = true;

        gameObject.SetActive(false);

    }

    public void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Death"))
        {

            DeathController dController = other.GetComponent<DeathController>();
            if (dController != null && dController.dState == DeathController.DeathState.Search)
            {

                if (dController.currentItem == null)
                {

                    dController.currentItem = item;
                    transform.SetParent(dController.deathHand);
                    transform.localPosition = Vector3.zero;

                    base.triggerArea.enabled = false;
                    itemCollider.enabled = false;

                    rdb.isKinematic = true;

                    dController.dState = DeathController.DeathState.Flee;

                }

            }

        }

        if (other.gameObject.CompareTag("Player"))
        {

            Debug.Log(item.name + " focused by: " + other.gameObject.name);

            interactionSign.enabled = true;

            PlayerControllerCC player = other.gameObject.GetComponent<PlayerControllerCC>();
            if (player != null)
            {

                player.onInteracting = Interact;

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

            Debug.Log(item.name + " not focused by: " + other.gameObject.name);

            interactionSign.enabled = false;

            PlayerControllerCC player = other.gameObject.GetComponent<PlayerControllerCC>();
            if (player != null)
            {

                player.onInteracting -= Interact;

            }

        }

    }

}
