using DG.Tweening;
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
    public ItemBase SelectedItem { get; set; }

    public int EscapeAttempts { get; set; }

    public StateMachine<BattleState> StateMachine { get; private set; }

    public GameObject genControlsUI;

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
        StartCoroutine(StartWildBattle());
    }

    IEnumerator StartWildBattle()
    {
        BoomServices.i.UserLostMatch().Forget();

        ShowBattleHUDs();
        yield return DialogueState.i.ShowDialogue($"{EnemyToyo.Base.Name} is keeping it's guard up...");

        EscapeAttempts = 0;

        StateMachine.ChangeState(ActionSelectionState.i);
    }

    public void ShowBattleHUDs()
    {
        if (PlayerToyo.BattleHUD == null)
            PlayerToyo.BattleHUD = Instantiate(battleHudPrefab, PlayerToyo.Model.transform);

        if (EnemyToyo.BattleHUD == null)
            EnemyToyo.BattleHUD = Instantiate(battleHudPrefab, EnemyToyo.Model.transform);

        PlayerToyo.ShowHUD(true);
        EnemyToyo.ShowHUD();
    }

    public void BattleOver(bool won)
    {
        IsBattleOver = true;
        PlayerToyo.Model.SetActive(false);
        //EnemyToyo.Model.SetActive(false);

        GameController.i.StateMachine.Pop();
        
        if (won)
            BoomServices.i.UserWonMatch().Forget();

        genControlsUI.SetActive(true);
        //PlayerParty.Pokemons.ForEach(p => p.OnBattleOver());
        //OnBattleOver(won);
    }

    public IEnumerator ThrowPokeball(PokeballItem pokeballItem)
    {

        yield return DialogueState.i.ShowDialogue($"Used {pokeballItem.Name.ToUpper()}!");

        // Spawn in player's hand
        var pokeball = Instantiate(pokeballItem.PokeballModel, PlayerController.i.animator.GetBoneTransform(HumanBodyBones.RightHand));
        pokeball.transform.localPosition = PlayerController.i.HandOffset;

        int shakeCount = TryToCatchToyo(EnemyToyo, pokeballItem);

        // Throw pokeball
        pokeball.ShakeCount = shakeCount;
        pokeball.LaunchToTarget(EnemyToyo.Model.transform.position + Vector3.up);

        yield return new WaitUntil(() => pokeball.CatchComplete);

        if (shakeCount == 4)
        {
            PlayerParty.AddToyo(EnemyToyo);
            BattleOver(true);
        }

        Destroy(pokeball.gameObject);
    }

    int TryToCatchToyo(Toyo enemyToyo, PokeballItem pokeballItem)
    {
        float a = (3 * enemyToyo.MaxHp - 2 * enemyToyo.Hp) * enemyToyo.Base.CatchRate * pokeballItem.CatchRateModifier /** ConditionsDB.GetStatusBonus(enemyToyo.Status)*/ / (3 * enemyToyo.MaxHp);

        if (a >= 255)
            return 4;

        float b = 1048560 / Mathf.Sqrt(Mathf.Sqrt(16711680 / a));

        int shakeCount = 0;
        while (shakeCount < 4)
        {
            if (UnityEngine.Random.Range(0, 65535) >= b)
                break;

            ++shakeCount;
        }

        return shakeCount;
    }

    public override void Execute()
    {
        StateMachine.Execute();
    }

    public override void Exit()
    {

    }
}
