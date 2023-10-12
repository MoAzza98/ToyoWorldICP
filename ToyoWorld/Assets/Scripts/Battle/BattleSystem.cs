using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BattleState
{
    START,
    PLAYERACTION,
    PLAYERMOVE,
    ENEMYMOVE,
    BUSY,
    PARTYSCREEN
}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleHUD playerHUD;

    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHUD enemyHUD;

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
        if(state == BattleState.PLAYERACTION) {
            HandleActionSelection();
        }
    }

    public IEnumerator SetupBattle()
    {

        playerUnit.Setup(playerParty.GetHealthyToyo());
        playerHUD.SetData(playerUnit.Toyo);

        enemyUnit.Setup(wildToyo);
        enemyHUD.SetData(enemyUnit.Toyo);

        partyScreen.Init();

        bDialogue.SetMoveNames(playerUnit.Toyo.Moves);
        bDialogue.SetButtonMoves(playerUnit.Toyo.Moves);

        yield return StartCoroutine(bDialogue.TypeDialog($"A wild {enemyUnit.Toyo.Base.ToyoName} appeared!"));

        PlayerAction();

    }

    public void PlayerAction()
    {
        state = BattleState.PLAYERACTION;
        StartCoroutine(bDialogue.TypeDialog($"Choose an action."));
        partyScreen.gameObject.SetActive(false);
        bDialogue.GoToActionSelector();
    }

    IEnumerator PerformPlayerMove()
    {
        state = BattleState.BUSY;

        var move = currentMove;
        move.PP--;
        yield return bDialogue.TypeDialog($"{playerUnit.Toyo.Base.ToyoName} used {move.Base.MoveName}.");

        playerUnit.playerAnim.SetTrigger("Attacked");
        effects.CastEffect(move, effects.enemyPoint);

        var damageDetails = enemyUnit.Toyo.TakeDamage(move, playerUnit.Toyo);
        enemyUnit.enemyAnim.SetTrigger("isHurt");

        yield return enemyHUD.UpdateHP();
        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            enemyUnit.enemyAnim.SetTrigger("Fainted");

            yield return bDialogue.TypeDialog($"{enemyUnit.Toyo.Base.ToyoName} fainted.");

            yield return new WaitForSeconds(1f);

            yield return bDialogue.TypeDialog($"You win!");

            yield return new WaitForSeconds(1f);

            GameController.instance.UpdateControllerParty(this.playerParty);
            GameController.instance.EndBattle();
            GameController.instance.toyosDefeated++;
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
        move.PP--;
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

            playerUnit.playerAnim.SetTrigger("Fainted");

            yield return new WaitForSeconds(1f);

            Destroy(playerUnit.playerToyo.gameObject);

            var nextToyo = playerParty.GetHealthyToyo();
            if(nextToyo != null)
            {
                OpenPartyScreen();
            }
            else
            {
                yield return bDialogue.TypeDialog($"All your toyos fainted! You black out...");

                foreach(var member in GameController.instance.gcParty.ToyoPartyList)
                {
                    member.Init();
                }
                GameController.instance.EndBattle();

            }
        }
        else
        {
            PlayerAction();
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
        if(playerUnit.Toyo.HP < 0)
        {
            yield return bDialogue.TypeDialog($"Come back, {playerUnit.Toyo.Base.ToyoName}!");
            //play animation here
            yield return new WaitForSeconds(2f);

        }

        playerUnit.Setup(toyo);
        playerHUD.SetData(toyo);

        bDialogue.SetMoveNames(toyo.Moves);
        bDialogue.SetButtonMoves(toyo.Moves);

        yield return bDialogue.TypeDialog($"Go! {toyo.Base.ToyoName}!");

        StartCoroutine(EnemyMove());

    }

    public void OpenPartyScreen()
    {
        state = BattleState.PARTYSCREEN;
        partyScreen.SetPartyData(playerParty.ToyoPartyList);
        partyScreen.gameObject.SetActive(true);
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
