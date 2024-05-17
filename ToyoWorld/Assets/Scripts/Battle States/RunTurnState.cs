using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RunTurnState : State<BattleState>
{
    public static RunTurnState i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    BattleState bs;
    Toyo playerToyo;
    Toyo enemyToyo;

    public override void Enter(BattleState owner)
    {
        bs = owner;

        playerToyo = bs.PlayerToyo;
        enemyToyo = bs.EnemyToyo;

        StartCoroutine(RunTurns());
    }

    IEnumerator RunTurns()
    {
        var battleActions = new List<BattleAction>();

        // Add a random move for the enemy
        var enemyAction = new BattleAction(bs.EnemyToyo, bs.PlayerToyo, bs.EnemyToyo.GetRandomMove());
        battleActions.Add(enemyAction);

        if (bs.SelectedAction == BattleActions.Move)
        {
            var playerAction = new BattleAction(bs.PlayerToyo, bs.EnemyToyo, bs.PlayerToyo.Moves[bs.SelectedMove]);
            battleActions.Add(playerAction);

            var sortedActions = battleActions.OrderByDescending(a => a.Source.Speed);
            foreach (var action in sortedActions)
            {
                yield return RunMove(action.Source, action.Target, action.Move);

                if (bs.IsBattleOver) yield break;
            }
        }

        bs.StateMachine.ChangeState(ActionSelectionState.i);
    }

    IEnumerator RunMove(Toyo source, Toyo target, Move move)
    {
        yield return DialogueState.i.ShowDialogue($"{source.Base.Name} used {move.Base.Name}");

        // Play animation, vfx, sfx of the move

        // Take damage
        target.TakeDamage(move, source);
        yield return target.BattleHUD.HPBar.WaitForUpdate();

        if (target.Hp <= 0)
        {
            yield return HandleToyoFainted(target);
        }
    }

    IEnumerator HandleToyoFainted(Toyo faintedToyo)
    {
        yield return DialogueState.i.ShowDialogue($"{faintedToyo.Base.Name} Fainted");
        // faintedToyo.PlayFaintAnimation();
        faintedToyo.Model.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        if (faintedToyo == enemyToyo)
        {
            bool battlWon = true;
            //if (isTrainerBattle)
            //    battlWon = trainerParty.GetHealthyPokemon() == null;

            //if (battlWon)
            //    AudioManager.i.PlayMusic(bs.BattleVictoryMusic);

            // Exp Gain
            int expYield = faintedToyo.Base.ExpYield;
            int enemyLevel = faintedToyo.Level;
            float trainerBonus = 1; /*(isTrainerBattle) ? 1.5f : 1f;*/

            int expGain = Mathf.FloorToInt((expYield * enemyLevel * trainerBonus) / 7);
            playerToyo.Exp += expGain;
            yield return DialogueState.i.ShowDialogue($"{playerToyo.Base.Name} gained {expGain} exp");
            // yield return playerUnit.Hud.SetExpSmooth();

            // Check Level Up
            while (playerToyo.CheckForLevelUp())
            {
                // playerUnit.Hud.SetLevel();
                yield return DialogueState.i.ShowDialogue($"{playerToyo.Base.Name} grew to level {playerToyo.Level}");

                // yield return playerUnit.Hud.SetExpSmooth(true);
            }

            //yield return new WaitForSeconds(1f);
        }

        yield return CheckForBattleOver(faintedToyo);
    }

    IEnumerator CheckForBattleOver(Toyo faintedToyo)
    {
        if (faintedToyo == playerToyo)
        {
            var nextPokemon = bs.PlayerParty.GetHealthyToyo();
            if (nextPokemon != null)
            {
                //yield return GameController.i.StateMachine.PushAndWait(PartyState.i);
                //yield return bs.SwitchPokemon(PartyState.i.SelectedPokemon);
            }
            else
                bs.BattleOver(false);
        }
        else
        {
            playerToyo.Model.SetActive(false);
            bs.BattleOver(true);
            //if (!isTrainerBattle)
            //{
            //    bs.BattleOver(true);
            //}
            //else
            //{
            //    var nextPokemon = trainerParty.GetHealthyPokemon();
            //    if (nextPokemon != null)
            //    {
            //        // use next pokemon in trainer party
            //    }
            //    else
            //        bs.BattleOver(true);
            //}
        }

        yield break;
    }
}

public class BattleAction
{
    public BattleAction(Toyo source, Toyo target, Move move)
    {
        Source = source;
        Target = target;
        Move = move;
    }

    public Toyo Source { get; set; }
    public Toyo Target { get; set; }
    public Move Move { get; set; }
}
