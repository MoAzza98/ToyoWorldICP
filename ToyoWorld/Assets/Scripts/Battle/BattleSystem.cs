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

    public BattleState state { get; set; }
    int currentAction;

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

        yield return new WaitForSeconds(1f);

        PlayerAction();

    }

    public void PlayerAction()
    {
        state = BattleState.PLAYERACTION;
        StartCoroutine(bDialogue.TypeDialog($"Choose an action."));
        bDialogue.GoToActionSelector();
    }

    public void PlayerMove()
    {
        state = BattleState.PLAYERMOVE;
        bDialogue.GoToMoveSelector();
    }

    private void HandleActionSelection()
    {
        if(Input.GetKey(KeyCode.DownArrow)) {
            if(currentAction < 1)
            {
                ++currentAction;
            }
        }
    }
}
