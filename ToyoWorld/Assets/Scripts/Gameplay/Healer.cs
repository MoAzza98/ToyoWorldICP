using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : Interactable
{
    [SerializeField] List<string> dialogue;

    private void Update()
    {
        if (!isInteracting && playerInRange && GameController.i.StateMachine.CurrentState == FreeRoamState.i)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(StartHealing());
            }
        }
    }

    IEnumerator StartHealing()
    {
        OnInteractionStarted();

        RotateTowardsTarget(PlayerController.i.transform);
        PlayerController.i.ResetTargetRotation();

        var choices = new List<string>() { "Yes", "No" };

        PlayerController.i.EnablePartyWidget(false);

        yield return DialogueState.i.ShowDialogueLines(dialogue, exitCurrState: true, choices);

        if (DialogueState.i.SelectedChoice == 0) // Yes
        {
            yield return DialogueState.i.ShowDialogue("Let me take a look at your Toyos...", exitCurrState: false);
            // yield return new WaitForSeconds(1f);
            foreach (var toyo in PlayerController.i.Party.Toyos)
                toyo.HealFully();
            yield return DialogueState.i.ShowDialogue("All done! Your Toyos are fully healed.", exitCurrState: false);
        }
        else // No
        {
            yield return DialogueState.i.ShowDialogue("Alright, come back if you need my help.", exitCurrState: false);
        }

        OnInteractionEnded();
    }
}
