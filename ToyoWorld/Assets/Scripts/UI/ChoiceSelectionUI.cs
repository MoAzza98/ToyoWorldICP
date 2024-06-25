using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceSelectionUI : SelectionUI<ButtonSlot>
{
    [SerializeField] ButtonSlot choiceSlotPrefab;

    List<ButtonSlot> choiceSlots;

    public void SetChoices(List<string> choices)
    {
        gameObject.SetActive(true);

        // Delete existing choices
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        choiceSlots = new List<ButtonSlot>();
        foreach (var choice in choices)
        {
            var choiceSlotObj = Instantiate(choiceSlotPrefab, transform);
            choiceSlotObj.name = choice;
            choiceSlotObj.SetText(choice);
            choiceSlots.Add(choiceSlotObj);
        }

        selectedItem = 0;

        SetItems(choiceSlots);
    }
}
