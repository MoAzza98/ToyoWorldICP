using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreeRoamState : State<GameController>
{
    [SerializeField] PlayerController player;
    [SerializeField] PartyWidget partyWidget;
    [SerializeField] Button moveSwitchButton;

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
        partyWidget.gameObject.SetActive(true);

        moveSwitchButton.onClick.AddListener(GoToMoveSwitchState);
    }

    public override void Execute()
    {

        if (Input.GetKeyDown(KeyCode.T))
        {
            gc.StateMachine.Push(MoveSwitchingState.i);
        }
    }

    public override void Exit()
    {
        player.SetControl(false);

        moveSwitchButton.onClick.RemoveAllListeners();
    }

    public override void OnGainedFocus()
    {
        moveSwitchButton.gameObject.SetActive(true);
        partyWidget.gameObject.SetActive(true);
    }

    public override void OnLostFocus()
    {
        moveSwitchButton.gameObject.SetActive(false);
    }

    void GoToMoveSwitchState()
    {
        if (gc.StateMachine.CurrentState == this)
            gc.StateMachine.Push(MoveSwitchingState.i);
    }
}
