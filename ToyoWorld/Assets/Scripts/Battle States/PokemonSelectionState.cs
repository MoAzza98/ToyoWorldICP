using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonSelectionState : State<BattleState>
{
    [SerializeField] PartyWidget partyWidget;

    public Toyo SelectedToyo { get; private set; }

    public static PokemonSelectionState i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    BattleState bs;
    public override void Enter(BattleState owner)
    {
        bs = owner;
        partyWidget.gameObject.SetActive(true);
    }

    public override void Execute()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (partyWidget.SelectedToyo.Hp <= 0)
            {
                StartCoroutine(DialogueState.i.ShowDialogue($"You can't send out a fainted toyo!"));
                return;
            }

            SelectedToyo = partyWidget.SelectedToyo;
            bs.StateMachine.Pop();
        }
    }

    public override void Exit()
    {
        partyWidget.gameObject.SetActive(false);
    }
}
