using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelectionState : State<BattleState>
{
    [SerializeField] GameObject actionSelectionUI;
    [SerializeField] GameObject genControlsUI;
    [SerializeField] PartyWidget partyWidget;

    public static ActionSelectionState i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    BattleState bs;
    public override void Enter(BattleState owner)
    {
        bs = owner;
        actionSelectionUI.SetActive(true);
        partyWidget.gameObject.SetActive(true);
        genControlsUI.gameObject.SetActive(false);
    }

    public override void Execute()
    {
        if (Input.GetButtonDown("Action"))
        {
            bs.SelectedAction = BattleActions.Move;
            bs.StateMachine.Push(MoveSelectionState.i, true);
        }
        else if (Input.GetMouseButtonDown(0))
        {
            if (partyWidget.SelectedToyo == bs.PlayerToyo)
            {
                StartCoroutine(DialogueState.i.ShowDialogue($"You can't switch with the same toyo!"));
                return;
            }
            if (partyWidget.SelectedToyo.Hp <= 0)
            {
                StartCoroutine(DialogueState.i.ShowDialogue($"You can't send out a fainted toyo!"));
                return;
            }

            bs.SelectedAction = BattleActions.SwitchToyo;
            bs.SelectedToyo = partyWidget.SelectedToyo;
            bs.StateMachine.ChangeState(RunTurnState.i);
        }
        else if (Input.GetButtonDown("Back"))
        {
            // Run
            bs.SelectedAction = BattleActions.Run;
            bs.StateMachine.ChangeState(RunTurnState.i);
        }
    }

    public override void Exit()
    {
        actionSelectionUI.SetActive(false);
        partyWidget.gameObject.SetActive(false);
    }
}
