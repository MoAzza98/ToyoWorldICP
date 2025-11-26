using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveSelectionUI : SelectionUI<ButtonSlot>
{
    [SerializeField] List<ButtonSlot> moveSlots;
    [SerializeField] Color noPPColor = Color.red;

    List<Move> _moves = new List<Move>();

    public void SetMoves(List<Move> moves)
    {
        _moves = moves;

        for (int i = 0; i < moveSlots.Count; i++)
        {
            if (i < moves.Count)
            {
                moveSlots[i].gameObject.SetActive(true);
                moveSlots[i].SetText(moves[i].Base.Name);
            }
            else
            {
                moveSlots[i].gameObject.SetActive(false);
            }
        }

        SetItems(moveSlots.Take(moves.Count).ToList());
    }

    public override void UpdateSelectionInUI()
    {
        base.UpdateSelectionInUI();

        for (int i = 0; i < _moves.Count; i++)
        {
            if (_moves[i].PP == 0)
                moveSlots[i].SetTextColor(noPPColor);
        }
    }
}
