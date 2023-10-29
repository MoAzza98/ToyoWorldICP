using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.VisualScripting.Member;

public enum BattleState
{
    START,
    ACTIONSELECTION,
    MOVESELECTION,
    PERFORMMOVE,
    BUSY,
    PARTYSCREEN,
    BATTLEOVER
}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;

    [SerializeField] BattleUnit enemyUnit;

    [SerializeField] BattleDialogBox bDialogue;
    [SerializeField] PartyScreen partyScreen;

    [SerializeField] EffectHandler effects;

    public BattleState state { get; set; }
    int currentAction;
    Move currentMove;

    private ToyoParty playerParty;
    private Toyo wildToyo;

    // Start is called before the first frame update
    private void Start()
    {
        //playerParty = GameController.instance.currentToyoParty;
        //wildToyo = GameController.instance.mapArea.GetRandomWildToyo();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameController.instance.state = GameState.Battle;
        ToyoParty tempParty = new ToyoParty();
        tempParty.ToyoPartyList = GameController.instance.playerParty.partyMembers;
        StartBattle(GameController.instance.gcParty, GameController.instance.mapArea.GetRandomWildToyo());

    }

    private void OnDestroy()
    {
        // Unsubscribe from the event or messaging system
    }

    public void StartBattle(ToyoParty playerParty, Toyo wildToyo)
    {
        this.playerParty = playerParty;
        this.wildToyo = wildToyo;
        StartCoroutine(SetupBattle());
    }

    // Update is called once per frame
    public void Update()
    {
        if(state == BattleState.ACTIONSELECTION) {
            HandleActionSelection();
        }
    }

    public IEnumerator SetupBattle()
    {

        playerUnit.Setup(playerParty.GetHealthyToyo());

        enemyUnit.Setup(wildToyo);

        partyScreen.Init();

        bDialogue.SetMoveNames(playerUnit.Toyo.Moves);
        bDialogue.SetButtonMoves(playerUnit.Toyo.Moves);

        yield return StartCoroutine(bDialogue.TypeDialog($"A wild {enemyUnit.Toyo.Base.ToyoName} appeared!"));

        ChooseFirstTurn();

    }

    void ChooseFirstTurn()
    {
        if(playerUnit.Toyo.Speed > enemyUnit.Toyo.Speed)
        {
            PlayerAction();
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }

    void BattleOver(bool won)
    {
        state = BattleState.BATTLEOVER;
        playerParty.ToyoPartyList.ForEach(p => p.OnBattleOver());
        StartCoroutine(OnBattleOver(won));
        GameController.instance.EndBattle();
    }

    public void PlayerAction()
    {
        state = BattleState.ACTIONSELECTION;
        StartCoroutine(bDialogue.TypeDialog($"Choose an action."));
        partyScreen.gameObject.SetActive(false);
        bDialogue.GoToActionSelector();
    }

    IEnumerator PlayerMove()
    {
        state = BattleState.PERFORMMOVE;

        var move = currentMove;
        move.PP--;

        yield return RunMove(playerUnit, enemyUnit, move);

        if(state == BattleState.PERFORMMOVE)
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
        state = BattleState.PERFORMMOVE;

        var move = enemyUnit.Toyo.GetRandomMove();

        yield return RunMove(enemyUnit, playerUnit, move);

        if (state == BattleState.PERFORMMOVE)
        {
            PlayerAction();
        }

    }

    void CheckForBattleOver(BattleUnit faintedUnit)
    {
        if (faintedUnit.IsPlayerUnit)
        {
            Debug.Log("Player fainted");
            var nextToyo = playerParty.GetHealthyToyo();
            Debug.Log(nextToyo);
            if (nextToyo != null)
            {
                Debug.Log("Opening screen");
                OpenPartyScreen(false);
            }
            else
            {
                //onbattleover method needs to be added here
                //yield return bDialogue.TypeDialog($"All your toyos fainted! You black out...");
                foreach (var member in GameController.instance.gcParty.ToyoPartyList)
                {
                    member.Init();
                }
                BattleOver(false);
            }
        }
        else
        {
            GameController.instance.UpdateControllerParty(this.playerParty);
            GameController.instance.toyosDefeated++;
            BattleOver(true);
        }
    }

    IEnumerator RunMove(BattleUnit sourceUnit, BattleUnit targetUnit, Move move)
    {
        move.PP--;
        yield return bDialogue.TypeDialog($"{sourceUnit.Toyo.Base.ToyoName} used {move.Base.MoveName}.");

        if (move.Base._MoveDestinaiton == MoveEffectDestination.Self)
        {
            effects.CastEffect(move, effects.playerPoint);
        }
        else
        {
            effects.CastEffect(move, effects.enemyPoint);
        }

        if (move.Base.Category == MoveCategory.Status)
        {
            sourceUnit.unitAnim.SetTrigger("Casted");
            yield return RunMoveEffects(move, sourceUnit.Toyo, targetUnit.Toyo);
        }
        else
        {
            var damageDetails = targetUnit.Toyo.TakeDamage(move, sourceUnit.Toyo);
            targetUnit.unitAnim.SetTrigger("isHurt");
            sourceUnit.unitAnim.SetTrigger("Attacked");
            yield return targetUnit.Hud.UpdateHP();
            yield return ShowDamageDetails(damageDetails);
        }

        if (targetUnit.Toyo.HP <= 0)
        {
            targetUnit.unitAnim.SetTrigger("Fainted");

            yield return bDialogue.TypeDialog($"{targetUnit.Toyo.Base.ToyoName} fainted.");

            yield return new WaitForSeconds(1f);

            CheckForBattleOver(targetUnit);
        }
    }

    IEnumerator RunMoveEffects(Move move, Toyo source, Toyo target)
    {
        var effects = move.Base.Effects;
        if (effects != null)
        {
            if (move.Base.MoveTarget == MoveTarget.Self)
            {
                source.ApplyBoost(effects.Boosts);
            }
            else
            {
                target.ApplyBoost(effects.Boosts);
            }
            yield return ShowStatusChanges(source);
            yield return ShowStatusChanges(target);
        }
    }

    IEnumerator OnBattleOver(bool didWin)
    {
        if (didWin)
        {
            yield return bDialogue.TypeDialog($"You won!");
        }
        else
        {
            yield return bDialogue.TypeDialog($"All your Toyos fainted, you black out!");
        }

        yield return new WaitForSeconds(3f);
    }

    IEnumerator ShowStatusChanges(Toyo toyo)
    {
        while(toyo.StatusChanges.Count > 0)
        {
            var message = toyo.StatusChanges.Dequeue();
            yield return bDialogue.TypeDialog(message);
        }
    }

    public void SendToyo(Toyo selectedToyo)
    {
        if(selectedToyo.HP <= 0)
        {
            partyScreen.SetMessageText("You can't send out a fainted Toyo");
            return;
        }
        if(selectedToyo == playerUnit.Toyo)
        {
            partyScreen.SetMessageText("You can't send out the same Toyo");
            return;
        }
        partyScreen.gameObject.SetActive(false);
        state = BattleState.BUSY;

        Destroy(playerUnit.playerToyo.gameObject);
        //Debug.Log(playerUnit.playerToyo.gameObject.name);

        StartCoroutine(SwitchToyo(selectedToyo));

    }

    public IEnumerator SwitchToyo(Toyo toyo)
    {
        bool currentToyoFainted = true;
        if(playerUnit.Toyo.HP > 0)
        {
            currentToyoFainted = false;
            yield return bDialogue.TypeDialog($"Come back, {playerUnit.Toyo.Base.ToyoName}!");
            //play animation here
            yield return new WaitForSeconds(2f);

        }

        playerUnit.Setup(toyo);

        bDialogue.SetMoveNames(toyo.Moves);
        bDialogue.SetButtonMoves(toyo.Moves);

        yield return bDialogue.TypeDialog($"Go! {toyo.Base.ToyoName}!");

        if (currentToyoFainted)
        {
            ChooseFirstTurn();
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }

    public void OpenPartyScreen(bool canLeave)
    {
        state = BattleState.PARTYSCREEN;
        partyScreen.SetPartyData(playerParty.ToyoPartyList);
        partyScreen.CanLeaveScreen(canLeave);
        partyScreen.gameObject.SetActive(true);
        Debug.Log("Should be active");
    }

    public void MoveSelection()
    {
        state = BattleState.MOVESELECTION;
        bDialogue.GoToMoveSelector();
    }

    private void HandleActionSelection()
    {
        
    }

    public void UseCurrentMove(Move move)
    {
        if(state == BattleState.MOVESELECTION)
        {
            currentMove = move;
            bDialogue.EnableMoveSelector(false);
            bDialogue.EnableBackSelector(false);
            bDialogue.EnableMoveDetails(false);
            StartCoroutine(PlayerMove());
        }
        else
        {
            return;
        }
    }

}
