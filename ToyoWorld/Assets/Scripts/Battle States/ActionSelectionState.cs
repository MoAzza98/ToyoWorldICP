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

        EnableUI();
    }

    public override void Execute()
    {
        actionSelectionUI.HandleUpdate();

        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(SwitchToyo());
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
        DisableUI();
    }

    void EnableUI()
    {
        actionSelectionUI.gameObject.SetActive(true);
        actionSelectionUI.OnSelected += OnSelected;
        actionSelectionUI.OnBack += OnBack;
        partyWidget.gameObject.SetActive(true);
        controlUI?.gameObject.SetActive(true);
    }

    void DisableUI()
    {
        actionSelectionUI.OnSelected -= OnSelected;
        actionSelectionUI.OnBack -= OnBack;
        actionSelectionUI.gameObject.SetActive(false);
        partyWidget.gameObject.SetActive(false);
        controlUI?.gameObject.SetActive(false);
    }

    IEnumerator SwitchToyo()
    {
        DisableUI();

        if (partyWidget.SelectedToyo == bs.PlayerToyo)
        {
            yield return DialogueState.i.ShowDialogue($"You can't switch with the same toyo!");
            EnableUI();
            yield break;
        }
        if (partyWidget.SelectedToyo.Hp <= 0)
        {
            yield return DialogueState.i.ShowDialogue($"You can't send out a fainted toyo!");
            EnableUI();
            yield break;
        }

        yield return DialogueState.i.ShowDialogue($"Do you want to send out {partyWidget.SelectedToyo.Base.Name}?", choices: new List<string>() { "Yes", "No" });
        if (DialogueState.i.SelectedChoice == 0)
        {
            // Yes
            bs.SelectedAction = BattleActions.SwitchToyo;
            bs.SelectedToyo = partyWidget.SelectedToyo;
            bs.StateMachine.ChangeState(RunTurnState.i);
        }
        else
        {
            // No
            EnableUI();
        }
    }
}
