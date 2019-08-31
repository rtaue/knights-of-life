using UnityEngine;

public class Interactable : MonoBehaviour {

    public SphereCollider triggerArea;
    public Transform interactionTransform;

    public SpriteRenderer interactionSign;
    public Sprite interactionIcon;

    public float radius = 3f;

    private void Update()
    {

        triggerArea.radius = radius;
        triggerArea.center = interactionTransform.localPosition;

    }

    public virtual void Interact ()
    {

        // This method is meant to be overwritten;
        Debug.Log("Player interacting with " + transform.name);

    }

    public virtual void DeathInteract()
    {

        // This method is meant to be overwritten;
        Debug.Log("Death interacting with " + transform.name);

    }

    public virtual void VictimInteract()
    {

        // This method is meant to be overwritten;
        Debug.Log("Victim interacting with " + transform.name);

    }

}
