using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum BattleActions { Move, SwitchToyo, UseItem, Run }

public class BattleState : State<GameController>
{
    [SerializeField] BattleHUD battleHudPrefab;

    public Toyo PlayerToyo { get; set; }
    public Toyo EnemyToyo { get; set; }
    public ToyoParty PlayerParty { get; private set; }

    public bool IsBattleOver { get; private set; }

    public int SelectedMove { get; set; }
    public BattleActions SelectedAction { get; set; }
    public Toyo SelectedToyo { get; set; }

    public StateMachine<BattleState> StateMachine { get; private set; }

    public static BattleState i { get; private set; }
    private void Awake()
    {
        i = this;
        StateMachine = new StateMachine<BattleState>(this);
    }

    public void StartState(ToyoParty playerParty, Toyo playerToyo, Toyo wildToyo)
    {
        PlayerParty = playerParty;
        PlayerToyo = playerToyo;
        EnemyToyo = wildToyo;
        IsBattleOver = false;

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

    public void ShowBattleHUDs()
    {
        if (PlayerToyo.BattleHUD == null)
            PlayerToyo.BattleHUD = Instantiate(battleHudPrefab, PlayerToyo.Model.transform);

        if (EnemyToyo.BattleHUD == null)
            EnemyToyo.BattleHUD = Instantiate(battleHudPrefab, EnemyToyo.Model.transform);

        PlayerToyo.ShowHUD();
        EnemyToyo.ShowHUD();
    }

    public void BattleOver(bool won)
    {
        IsBattleOver = true;
        GameController.i.StateMachine.Pop();

        //PlayerParty.Pokemons.ForEach(p => p.OnBattleOver());
        //OnBattleOver(won);
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
