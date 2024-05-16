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
    public override void Enter(BattleState owner)
    {
        bs = owner;

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
