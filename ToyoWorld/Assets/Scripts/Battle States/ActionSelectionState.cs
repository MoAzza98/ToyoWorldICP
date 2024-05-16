using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelectionState : State<BattleState>
{
    [SerializeField] GameObject actionSelectionUI;

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
    }

    public override void Execute()
    {
        if (Input.GetButton("Action"))
        {
            bs.SelectedAction = BattleActions.Move;
            bs.StateMachine.Push(MoveSelectionState.i, true);
        }
        else if (Input.GetButton("Back"))
        {
            // Run
        }
    }

    public override void Exit()
    {
        actionSelectionUI.SetActive(false);
    }
}
