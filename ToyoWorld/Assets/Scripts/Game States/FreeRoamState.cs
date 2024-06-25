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
    [SerializeField] GameObject gameControls;

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
        player.CanThrowPokeball = true;
        partyWidget.gameObject.SetActive(true);
        gameControls?.SetActive(true);

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
        player.CanThrowPokeball = false;

        moveSwitchButton.onClick.RemoveAllListeners();
    }

    public override void OnGainedFocus()
    {
        moveSwitchButton.gameObject.SetActive(true);
        partyWidget.gameObject.SetActive(true);
        gameControls?.SetActive(true);
        player.CanThrowPokeball = true;
    }

    public override void OnLostFocus()
    {
        moveSwitchButton.gameObject.SetActive(false);
        gameControls?.SetActive(false);
        player.CanThrowPokeball = false;
    }

    void GoToMoveSwitchState()
    {
        if (gc.StateMachine.CurrentState == this)
            gc.StateMachine.Push(MoveSwitchingState.i);
    }
}
