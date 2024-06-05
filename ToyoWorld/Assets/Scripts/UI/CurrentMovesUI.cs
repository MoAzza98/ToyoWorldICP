using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CurrentMovesUI : SelectionUI<ButtonSlot>
{
    [SerializeField] List<ButtonSlot> moveSlots;

    public void SetMoves(List<MoveBase> moves)
    {
        for (int i = 0; i < moveSlots.Count; i++)
        {
            if (i < moves.Count)
            {
                moveSlots[i].gameObject.SetActive(true);
                moveSlots[i].SetText(moves[i].Name);
            }
            else
            {
                moveSlots[i].gameObject.SetActive(true);
                moveSlots[i].SetText("-");
            }
        }

        SetItems(moveSlots.Take(moves.Count).ToList());
    }

    public void DisableSlot(bool selected, int slotIndex)
    {
        if (slotIndex < moveSlots.Count)
            moveSlots[slotIndex].Disabled = selected;
    }
}
