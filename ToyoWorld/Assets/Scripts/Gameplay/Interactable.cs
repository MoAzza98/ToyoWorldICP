using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] protected GameObject overlayTxt;
    protected bool playerInRange = false;
    protected bool isInteracting = false;

    protected void OnInteractionStarted()
    {
        isInteracting = true;
        overlayTxt.SetActive(false);

        // PlayerController.i.EnablePartyWidget(false);
    }

    protected void OnInteractionEnded()
    {
        isInteracting = false;
        if (playerInRange)
            overlayTxt.SetActive(true);

        // PlayerController.i.EnablePartyWidget(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (!isInteracting)
                overlayTxt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            overlayTxt.SetActive(false);
        }
    }

    public Vector3 RotateTowardsTarget(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        direction.y = 0; // Keep the rotation on the horizontal plane
        transform.forward = direction.normalized;
        target.forward = -direction.normalized;

        return direction;
    }
}
