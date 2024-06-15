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
                if (action.Source.Hp > 0)
                {
                    yield return RunMove(action.Source, action.Target, action.Move);
                    if (bs.IsBattleOver) yield break;
                }
            }
        }
        else 
        {
            if (bs.SelectedAction == BattleActions.SwitchToyo)
            {
                yield return SwitchToyo(bs.SelectedToyo);
            }
            else if (bs.SelectedAction == BattleActions.Run)
            {
                yield return TryToEscape();
                if (bs.IsBattleOver) yield break;
            }

            // Enemy Move
            enemyAction.Target = bs.PlayerToyo;     // Player toyo will change after switching
            yield return RunMove(enemyAction.Source, enemyAction.Target, enemyAction.Move);
        }


        if (bs.IsBattleOver) yield break;

        bs.StateMachine.ChangeState(ActionSelectionState.i);
    }

    IEnumerator RunMove(Toyo source, Toyo target, Move move)
    {
        source.PlayAnimation("attack");

        yield return DialogueState.i.ShowDialogue($"{source.Base.Name} used {move.Base.Name}");

        // Play animation, vfx, sfx of the move
        if (move.Base.VFX != null)
        {
            var targetTransform = target.Model.transform;
            var localOffset = move.Base.VFXOffset;
            var posOffset = targetTransform.forward * localOffset.z + targetTransform.right * localOffset.x + targetTransform.up * localOffset.y;

            var vfxObj = Instantiate(move.Base.VFX);
            
            vfxObj.transform.position = target.Collider.bounds.center + posOffset;
            vfxObj.transform.rotation = Quaternion.LookRotation(-targetTransform.forward);

            StartCoroutine(AsyncUtil.RunAfterTime(10, () => Destroy(vfxObj)));
        }

        // Take damage
        var damageDetails = target.TakeDamage(move, source);
        yield return target.BattleHUD.HPBar.WaitForUpdate();
        yield return ShowDamageDetails(damageDetails);

        if (target.Hp <= 0)
        {
            yield return HandleToyoFainted(target);
        }
    }

    public IEnumerator SwitchToyo(Toyo newToyo)
    {
        if (playerToyo.Hp > 0)
        {
            yield return DialogueState.i.ShowDialogue($"Come back {playerToyo.Base.Name}");
        }
        playerToyo.Model.SetActive(false);

        yield return DialogueState.i.ShowDialogue($"Go {newToyo.Base.Name}!");

        ToyoParty.SpawnModel(newToyo, playerToyo.Model.transform.position, playerToyo.Model.transform.rotation);
        playerToyo = newToyo;
        bs.PlayerToyo = playerToyo;

        bs.ShowBattleHUDs();
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
            yield return playerToyo.BattleHUD.SetExpSmooth();

            // Check Level Up
            while (playerToyo.CheckAndHandleLevelUp())
            {
                // playerUnit.Hud.SetLevel();
                yield return DialogueState.i.ShowDialogue($"{playerToyo.Base.Name} grew to level {playerToyo.Level}");

                playerToyo.BattleHUD.SetExp(0);     // Level changed so start from zero
                yield return playerToyo.BattleHUD.SetExpSmooth();
            }

            //playerToyo.BattleHUD.DisableExpBar();
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
                yield return bs.StateMachine.PushAndWait(PokemonSelectionState.i);
                yield return SwitchToyo(PokemonSelectionState.i.SelectedToyo);
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
                playerToyo.Model.SetActive(false);
                bs.BattleOver(false);
            }
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
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

    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if (damageDetails.Critical > 1f)
            yield return DialogueState.i.ShowDialogue("A critical hit!");

        if (damageDetails.TypeEffectiveness == 0)
            yield return DialogueState.i.ShowDialogue("It doesn't have any effect!");
        else if (damageDetails.TypeEffectiveness > 1f)
            yield return DialogueState.i.ShowDialogue("It's super effective!");
        else if (damageDetails.TypeEffectiveness < 1f)
            yield return DialogueState.i.ShowDialogue("It's not very effective!");
    }

    IEnumerator TryToEscape()
    {

        //if (isTrainerBattle)
        //{
        //    yield return dialogBox.TypeDialog($"You can't run from trainer battles!");
        //    yield break;
        //}

        ++bs.EscapeAttempts;

        int playerSpeed = playerToyo.Speed;
        int enemySpeed = enemyToyo.Speed;

        if (enemySpeed < playerSpeed)
        {
            yield return DialogueState.i.ShowDialogue("Ran away safely!");
            bs.BattleOver(false);
        }
        else
        {
            float f = (playerSpeed * 128) / enemySpeed + 30 * bs.EscapeAttempts;
            f = f % 256;

            if (UnityEngine.Random.Range(0, 256) < f)
            {
                yield return DialogueState.i.ShowDialogue("Ran away safely!");
                bs.BattleOver(false);
            }
            else
            {
                yield return DialogueState.i.ShowDialogue("Can't escape!");
            }
        }
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
