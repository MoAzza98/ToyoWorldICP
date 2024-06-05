using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PossibleMovesUI : SelectionUI<ButtonSlot>
{
    [SerializeField] ButtonSlot moveSlotPf;
    [SerializeField] GameObject moveList;

    List<ButtonSlot> moveSlots = new List<ButtonSlot>();

    public void SetMoves(List<MoveBase> moves)
    {
        // Clear all the existing items
        foreach (Transform child in moveList.transform)
            Destroy(child.gameObject);

        moveSlots = new List<ButtonSlot>();
        foreach (var move in moves)
        {
            var moveSlot = Instantiate(moveSlotPf, moveList.transform);
            moveSlot.SetText(move.Name);

            //var rectTransform = moveSlot.GetComponent<RectTransform>();
            //rectTransform.rect.Set(rectTransform.rect.x, rectTransform.rect.y, rectTransform.rect.width, 130f);

            moveSlots.Add(moveSlot);
        }

        SetItems(moveSlots);
    }

    public void DisableSlot(bool selected, int slotIndex)
    {
        if (slotIndex < moveSlots.Count)
            moveSlots[slotIndex].Disabled = selected;
    }
}
