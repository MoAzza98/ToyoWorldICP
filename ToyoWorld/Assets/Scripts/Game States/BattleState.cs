using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleActions { Move, SwitchToyo, UseItem, Run }

public class BattleState : State<GameController>
{
    [SerializeField] BattleHUD battleHudPrefab;

    public Toyo PlayerToyo { get; private set; }
    public Toyo EnemyToyo { get; private set; }

    public int SelectedMove { get; set; }
    public BattleActions SelectedAction { get; set; }

    public StateMachine<BattleState> StateMachine { get; private set; }

    public static BattleState i { get; private set; }
    private void Awake()
    {
        i = this;
        StateMachine = new StateMachine<BattleState>(this);
    }

    public void StartState(Toyo playerToyo, Toyo wildToyo)
    {
        PlayerToyo = playerToyo;
        EnemyToyo = wildToyo;

        GameController.i.StateMachine.Push(this);
    }

    GameController gc;
    public override void Enter(GameController owner)
    {
        gc = owner;

        PlayerController.i.CanThrowPokeball = false;
        StartCoroutine(StartWildBattle());
    }

    IEnumerator StartWildBattle()
    {
        ShowBattleHUDs();
        yield return DialogueState.i.ShowDialogue($"{EnemyToyo.Base.Name} is keeping it's guard up...");

        StateMachine.ChangeState(ActionSelectionState.i);
    }

    void ShowBattleHUDs()
    {
        if (PlayerToyo.BattleHUD == null)
            PlayerToyo.BattleHUD = Instantiate(battleHudPrefab, PlayerToyo.Model.transform);

        if (EnemyToyo.BattleHUD == null)
            EnemyToyo.BattleHUD = Instantiate(battleHudPrefab, EnemyToyo.Model.transform);

        PlayerToyo.ShowHUD();
        EnemyToyo.ShowHUD();
    }

    public override void Execute()
    {
        StateMachine.Execute();
    }

    public override void Exit()
    {
        PlayerController.i.CanThrowPokeball = true;
    }
}
