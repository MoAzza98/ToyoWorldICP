using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSelectionState : State<BattleState>
{
    [SerializeField] MoveSelectionUI moveSelectionUI;

    public static MoveSelectionState i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    BattleState bs;
    public override void Enter(BattleState owner)
    {
        bs = owner;
        moveSelectionUI.gameObject.SetActive(true);
        moveSelectionUI.SetMoves(bs.PlayerToyo.Moves);
        moveSelectionUI.OnSelected += OnMoveSelected;
        moveSelectionUI.OnBack += OnBack;

        PlayerController.i.SetControl(false);
        PlayerController.i.FreezeCamera(true);
    }

    public override void Execute()
    {
        moveSelectionUI.HandleUpdate();
    }

    public override void Exit()
    {
        moveSelectionUI.gameObject.SetActive(false);
        moveSelectionUI.OnSelected -= OnMoveSelected;
        moveSelectionUI.OnBack -= OnBack;

        PlayerController.i.SetControl(true);
        PlayerController.i.FreezeCamera(false);
    }

    void OnMoveSelected(int selection)
    {
        bs.SelectedMove = selection;
        bs.StateMachine.ChangeState(RunTurnState.i);
        // Debug.Log($"Selected Move: {bs.PlayerPokemon.Moves[selection].Base.Name}");
    }

    void OnBack()
    {
        bs.StateMachine.Pop(true);
    }
}
