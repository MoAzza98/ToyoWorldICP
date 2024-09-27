using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    [SerializeField] GameObject overlayTxt;
    bool playerInRange = false;
    Transform cam;

    private void Start()
    {
        cam = Camera.main.transform;
    }

    private void Update()
    {
        overlayTxt.transform.forward = cam.forward;

        if (playerInRange && Input.GetKeyDown(KeyCode.Return) && GameController.i.StateMachine.CurrentState == FreeRoamState.i)
            StartCoroutine(OpenBox());
    }

    private void OnTriggerEnter(Collider other)
    {
        playerInRange = true;
        overlayTxt.gameObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        playerInRange = false;
        overlayTxt.gameObject.SetActive(false);
    }

    IEnumerator OpenBox()
    {
        yield return null;
        GameController.i.StateMachine.Push(StorageState.i);
    }
}
