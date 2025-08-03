using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrainerController : MonoBehaviour
{
    [SerializeField] GameObject overlayTxt;
    [SerializeField] string dialogueBeforeBattle = "Let's battle and see who is stronger!";
    [SerializeField] string dialogueAfterBattle = "I'll train hard and beat you one day!";
    [SerializeField] GameObject closeUpCam;


    bool battleLost = false;

    bool playerInRange = false; 

    public ToyoParty Party { get; private set; }

    private void Awake()
    {
        Party = GetComponent<ToyoParty>();
    }

    private void Update()
    {
        if (playerInRange && GameController.i.StateMachine.CurrentState == FreeRoamState.i)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (!battleLost)
                {
                    StartCoroutine(StartBattle());
                    playerInRange = false;
                    overlayTxt.SetActive(false);
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
        Vector3 direction = PlayerController.i.transform.position - transform.position;
        direction.y = 0; // Keep the rotation on the horizontal plane
        transform.forward = direction.normalized;
        PlayerController.i.transform.forward = -direction.normalized;

        // cut camera to cose-up of trainer
        Camera.main.transform.position = transform.position + Vector3.up * 2f + transform.forward * -2f;

        closeUpCam?.SetActive(true);

        yield return DialogueState.i.ShowDialogue(dialogueBeforeBattle);

        PlayerController.i.GetComponent<CharacterController>().enabled = false;
        PlayerController.i.transform.position = transform.position + transform.forward * 6f;
        PlayerController.i.GetComponent<CharacterController>().enabled = true;

        closeUpCam?.SetActive(false);

        BattleState.i.StartTrainerBattle(PlayerController.i, this);
    }

    IEnumerator ShowAfterBattleDialogue()     
    {
        overlayTxt.SetActive(false);
        yield return DialogueState.i.ShowDialogue(dialogueAfterBattle);
        overlayTxt.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
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

    public void SetBattleLost(bool lost)
    {
        battleLost = lost;
    }
}
