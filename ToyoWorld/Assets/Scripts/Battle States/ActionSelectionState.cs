using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelectionState : State<BattleState>
{
    [SerializeField] ActionSelectionUI actionSelectionUI;
    [SerializeField] GameObject controlUI;
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

        actionSelectionUI.gameObject.SetActive(true);
        actionSelectionUI.OnSelected += OnSelected;
        actionSelectionUI.OnBack += OnBack;

        partyWidget.gameObject.SetActive(true);
        controlUI?.gameObject.SetActive(true);
    }

    public override void Execute()
    {
        actionSelectionUI.HandleUpdate();

        if (Input.GetMouseButtonDown(0))
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
    }

    void OnSelected(int selection)
    {
        if (selection == 0)
        {
            // Move
            bs.SelectedAction = BattleActions.Move;
            bs.StateMachine.Push(MoveSelectionState.i, true);
        }
        else
        {
            // Run
            bs.SelectedAction = BattleActions.Run;
            bs.StateMachine.ChangeState(RunTurnState.i);
        }
    }

    void OnBack()
    {
        // Run
        bs.SelectedAction = BattleActions.Run;
        bs.StateMachine.ChangeState(RunTurnState.i);
    }

    public override void Exit()
    {
        actionSelectionUI.OnSelected -= OnSelected;
        actionSelectionUI.OnBack -= OnBack;
        actionSelectionUI.gameObject.SetActive(false);
        partyWidget.gameObject.SetActive(false);
        controlUI?.gameObject.SetActive(false);
    }
}
