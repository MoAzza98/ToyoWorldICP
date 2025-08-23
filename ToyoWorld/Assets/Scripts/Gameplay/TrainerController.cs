using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrainerController : Interactable
{
    [SerializeField] float moneyReward = 100;
    [SerializeField] List<string> dialogueBeforeBattle;
    [SerializeField] List<string> dialogueAfterBattle;
    [SerializeField] GameObject closeUpCam;


    bool battleLost = false;

    public ToyoParty Party { get; private set; }

    private void Awake()
    {
        Party = GetComponent<ToyoParty>();
    }

    private void Update()
    {

        if (!isInteracting && playerInRange && GameController.i.StateMachine.CurrentState == FreeRoamState.i)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (!battleLost)
                {
                    OnInteractionStarted();
                    playerInRange = false;
                    StartCoroutine(StartBattle());
                }
                else
                {
                    StartCoroutine(ShowAfterBattleDialogue());
                }

            }
        }
    }

    IEnumerator StartBattle()
    {
        // rotate towards player
        RotateTowardsTarget(PlayerController.i.transform);

        // cut camera to cose-up of trainer
        Camera.main.transform.position = transform.position + Vector3.up * 2f + transform.forward * -2f;

        closeUpCam?.SetActive(true);

        yield return DialogueState.i.ShowDialogueLines(dialogueBeforeBattle, exitCurrState: true);

        PlayerController.i.GetComponent<CharacterController>().enabled = false;
        PlayerController.i.transform.position = transform.position + transform.forward * 6f;
        PlayerController.i.GetComponent<CharacterController>().enabled = true;

        closeUpCam?.SetActive(false);

        BattleState.i.StartTrainerBattle(PlayerController.i, this);
    }

    IEnumerator ShowAfterBattleDialogue()     
    {
        var dirToTarget = RotateTowardsTarget(PlayerController.i.transform);
        PlayerController.i.ResetTargetRotation();

        OnInteractionStarted();
        yield return DialogueState.i.ShowDialogueLines(dialogueAfterBattle, exitCurrState: true);
        OnInteractionEnded();
    }

    public void OnBattleLost()
    {
        battleLost = true;
        StartCoroutine(HandleBattleLostDialogues());
    }

    IEnumerator HandleBattleLostDialogues()   
    {
        Wallet.i.AddMoney(moneyReward);
        yield return DialogueState.i.ShowDialogue("You defeated the trainer in battle!");
        yield return DialogueState.i.ShowDialogue($"You received ${moneyReward} for winning.");
        OnInteractionEnded();
    }
}
