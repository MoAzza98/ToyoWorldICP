using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
    [SerializeField] List<string> dialogue;

    private void Update()
    {
        if (!isInteracting && playerInRange && GameController.i.StateMachine.CurrentState == FreeRoamState.i)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(StartDialogue());
            }
        }
    }

    IEnumerator StartDialogue()
    {
        RotateTowardsTarget(PlayerController.i.transform);
        PlayerController.i.ResetTargetRotation();

        OnInteractionStarted();
        yield return DialogueState.i.ShowDialogueLines(dialogue, exitCurrState: true);
        OnInteractionEnded();
    }
}
