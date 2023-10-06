using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactRange = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            foreach(Collider collider in colliderArray)
            {
                if(collider.TryGetComponent(out InteractableItem interactableItem)){
                    interactableItem.Interact();
                }
            }
        }
    }

    public InteractableItem GetInteractableObject()
    {
        List<InteractableItem> interactableList = new List<InteractableItem>();
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out InteractableItem interactableItem))
            {
                interactableList.Add(interactableItem);
            }
        }

        InteractableItem closestInteract = null;
        foreach(InteractableItem interactableItem in interactableList)
        {
            if (closestInteract == null)
            {
                closestInteract = interactableItem;
            } else if(Vector3.Distance(transform.position, interactableItem.transform.position) < 
                Vector3.Distance(transform.position, closestInteract.transform.position))
            {
                //is closer
                closestInteract = interactableItem;
            }
        }

        return closestInteract;
    }
}
