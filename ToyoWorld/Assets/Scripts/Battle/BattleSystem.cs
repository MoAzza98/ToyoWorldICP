using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;

public enum BattleState
{
    START,
    PLAYERACTION,
    PLAYERMOVE,
    ENEMYMOVE,
    BUSY
}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleHUD playerHUD;

    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHUD enemyHUD;

    [SerializeField] BattleDialogBox bDialogue;

    [SerializeField] EffectHandler effects;

    public BattleState state { get; set; }
    int currentAction;
    Move currentMove;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetupBattle());
    }

    // Update is called once per frame
    void Update()
    {
        if(state == BattleState.PLAYERACTION) {
            HandleActionSelection();
        }
    }

    public IEnumerator SetupBattle()
    {
        playerUnit.Setup();
        playerHUD.SetData(playerUnit.Toyo);

        enemyUnit.Setup();
        enemyHUD.SetData(enemyUnit.Toyo);

        bDialogue.SetMoveNames(playerUnit.Toyo.Moves);
        bDialogue.SetButtonMoves(playerUnit.Toyo.Moves);

        yield return StartCoroutine(bDialogue.TypeDialog($"A wild {enemyUnit.Toyo.Base.ToyoName} appeared!"));

        PlayerAction();

    }

    public void PlayerAction()
    {
        state = BattleState.PLAYERACTION;
        StartCoroutine(bDialogue.TypeDialog($"Choose an action."));
        bDialogue.GoToActionSelector();
    }

    IEnumerator PerformPlayerMove()
    {
        state = BattleState.BUSY;

        var move = currentMove;

        yield return bDialogue.TypeDialog($"{playerUnit.Toyo.Base.ToyoName} used {move.Base.MoveName}.");

        playerUnit.playerAnim.SetTrigger("Attacked");
        effects.CastEffect(move, effects.enemyPoint);

        var damageDetails = enemyUnit.Toyo.TakeDamage(move, playerUnit.Toyo);
        enemyUnit.enemyAnim.SetTrigger("isHurt");

        yield return enemyHUD.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            yield return bDialogue.TypeDialog($"{enemyUnit.Toyo.Base.ToyoName} fainted.");
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if (damageDetails.Critical > 1) 
        {
            yield return bDialogue.TypeDialog($"It's a critical hit!");
        }
        if (damageDetails.TypeEffectiveness > 1)
        {
            yield return bDialogue.TypeDialog($"It's super effective!");
        } else if (damageDetails.TypeEffectiveness < 1)
        {
            yield return bDialogue.TypeDialog($"It's not very effective.");
        }
    }

    IEnumerator EnemyMove()
    {
        state = BattleState.ENEMYMOVE;

        var move = enemyUnit.Toyo.GetRandomMove();

        yield return bDialogue.TypeDialog($"{enemyUnit.Toyo.Base.ToyoName} used {move.Base.MoveName}.");

        enemyUnit.enemyAnim.SetTrigger("Attacked");
        effects.CastEffect(move, effects.playerPoint);

        var damageDetails = playerUnit.Toyo.TakeDamage(move, enemyUnit.Toyo);
        playerUnit.playerAnim.SetTrigger("isHurt");

        yield return playerHUD.UpdateHP();
        yield return ShowDamageDetails(damageDetails);


        if (damageDetails.Fainted)
        {
            yield return bDialogue.TypeDialog($"{playerUnit.Toyo.Base.ToyoName} fainted.");
        }
        else
        {
            PlayerAction();
        }


    }

    public void PlayerMove()
    {
        state = BattleState.PLAYERMOVE;
        bDialogue.GoToMoveSelector();
    }

    private void HandleActionSelection()
    {
        
    }

    public void UseCurrentMove(Move move)
    {
        if(state == BattleState.PLAYERMOVE)
        {
            currentMove = move;
            bDialogue.EnableMoveSelector(false);
            bDialogue.EnableBackSelector(false);
            bDialogue.EnableMoveDetails(false);
            StartCoroutine(PerformPlayerMove());
        }
        else
        {
            return;
        }
    }

}
