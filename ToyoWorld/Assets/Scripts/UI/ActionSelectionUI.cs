using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelectionUI : SelectionUI<ButtonSlot>
{
    [SerializeField] List<ButtonSlot> actionSlots;

    private void Start()
    {
        SetItems(actionSlots);
    }
}
