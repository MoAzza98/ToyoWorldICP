using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveSelectionUI : SelectionUI<ButtonSlot>
{
    [SerializeField] List<ButtonSlot> moveSlots;

    public void SetMoves(List<Move> moves)
    {
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
}
