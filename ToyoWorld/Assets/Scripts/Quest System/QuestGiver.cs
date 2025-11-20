using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : Interactable
{
    [SerializeField] QuestData questToStart;

    [Header("Dialogues")]
    [SerializeField] List<string> questStartDialogue;
    [SerializeField] List<string> questProgressDialogue;
    [SerializeField] List<string> questCompletedDialogue;

    private void Update()
    {
        if (!isInteracting && playerInRange && GameController.i.StateMachine.CurrentState == FreeRoamState.i)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(GiveQuest());
            }
        }
    }

    IEnumerator GiveQuest()
    {
        RotateTowardsTarget(PlayerController.i.transform);
        PlayerController.i.ResetTargetRotation();

        OnInteractionStarted();

        var completedQuest = QuestController.i.GetCompletedQuest(questToStart);
        if (completedQuest != null)
        {
            // Quest Already Completed
            yield return DialogueState.i.ShowDialogueLines(questCompletedDialogue, exitCurrState: true);
        }
        else
        {
            var activeQuest = QuestController.i.GetActiveQuest(questToStart);
            if (activeQuest != null)
            {
                // Quest Already Active
                yield return DialogueState.i.ShowDialogueLines(questProgressDialogue, exitCurrState: true);
            }
            else
            {
                // Start Quest
                yield return DialogueState.i.ShowDialogueLines(questStartDialogue, exitCurrState: true);
                QuestController.i.AddQuest(questToStart);
            }
        }

        OnInteractionEnded();
    }
}
