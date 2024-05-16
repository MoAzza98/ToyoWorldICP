using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeRoamState : State<GameController>
{
    [SerializeField] PlayerController player;

    public static FreeRoamState i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    GameController gc;
    public override void Enter(GameController owner)
    {
        gc = owner;

        player.SetControl(true);
    }

    public override void Execute()
    {

        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(DialogueState.i.ShowDialogue("The dialogue system is working fine!", exitCurrState: true));
        }
    }

    public override void Exit()
    {
        player.SetControl(false);
    }
}
