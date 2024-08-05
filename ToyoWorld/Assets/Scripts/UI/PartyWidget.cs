using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyWidget : MonoBehaviour
{
    [SerializeField] TMP_Text nameTxt;
    [SerializeField] TMP_Text lvlTxt;
    [SerializeField] Image centerSlot;
    [SerializeField] Image leftSlot;
    [SerializeField] Image rightSlot;

    bool showItems = false;

    int selectedToyo = 0;
    int selectedItem = 0;
    public Toyo SelectedToyo => playerParty.Toyos[selectedToyo];
    public ItemBase SelectedItem => toyoBallSlots.Count > 0? toyoBallSlots[selectedItem].Item : null;

    ToyoParty playerParty;
    Inventory inventory;
    List<ItemSlot> toyoBallSlots;
    private void Start()
    {
        playerParty = PlayerController.i.GetComponent<ToyoParty>();
        inventory = Inventory.GetInventory();
        toyoBallSlots = inventory.ToyoballSlots;

        UpdateSelectionInUI();
        centerSlot.color = Color.white;

        playerParty.OnPartyUpdated += UpdateSelectionInUI;
        inventory.OnUpdated += UpdateSelectionInUI;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            ToggleShowingItemsOrParty();

        if (showItems)
        {
            selectedItem = Mathf.Clamp(selectedItem, 0, toyoBallSlots.Count);

            float prevSelection = selectedItem;

            if (Input.GetKeyDown(KeyCode.Q))
                selectedItem = GetPrevIndex(selectedItem, toyoBallSlots.Count);
            else if (Input.GetKeyDown(KeyCode.E))
                selectedItem = GetNextIndex(selectedItem, toyoBallSlots.Count);

            if (selectedItem != prevSelection)
                UpdateSelectionInUI();
        }
        else
        {
            float prevSelection = selectedItem;

            if (Input.GetKeyDown(KeyCode.Q))
                selectedToyo = GetPrevIndex(selectedToyo, playerParty.Toyos.Count);
            else if (Input.GetKeyDown(KeyCode.E))
                selectedToyo = GetNextIndex(selectedToyo, playerParty.Toyos.Count);

            if (selectedToyo != prevSelection)
                UpdateSelectionInUI();
        }
    }

    public void UpdateSelectionInUI()
    {
        if (showItems)
        {
            if (toyoBallSlots.Count == 0)
            {
                lvlTxt.text = "";
                nameTxt.text = "";
                centerSlot.sprite = null;
                centerSlot.color = new Color(1, 1, 1, 0);
            }

            var itemSlot = toyoBallSlots[selectedItem];

            lvlTxt.text = itemSlot.Item.Name;
            nameTxt.text = "x" + itemSlot.Count;
            centerSlot.sprite = itemSlot.Item.Icon;
            centerSlot.color = Color.white;

            int nextItemIndex = GetNextIndex(selectedItem, toyoBallSlots.Count);
            int prevItemIndex = GetPrevIndex(selectedItem, toyoBallSlots.Count);

            if (toyoBallSlots.Count == 1)
            {
                prevItemIndex = -1;
                nextItemIndex = -1;
            }
            else if (prevItemIndex == nextItemIndex)
                nextItemIndex = -1;

            if (prevItemIndex != -1)
            {
                leftSlot.sprite = toyoBallSlots[prevItemIndex].Item.Icon;
                leftSlot.color = Color.white;
            }
            else
                leftSlot.color = new Color(1, 1, 1, 0);

            if (nextItemIndex != -1)
            {
                rightSlot.sprite = toyoBallSlots[nextItemIndex].Item.Icon;
                rightSlot.color = Color.white;
            }
            else
                rightSlot.color = new Color(1, 1, 1, 0);
        }
        else
        {
            var toyo = playerParty.Toyos[selectedToyo];

            nameTxt.text = toyo.Base.Name;
            lvlTxt.text = "Lv. " + toyo.Level.ToString();
            centerSlot.sprite = toyo.Base.Sprite;

            int nextToyoIndex = GetNextIndex(selectedToyo, playerParty.Toyos.Count);
            int prevToyoIndex = GetPrevIndex(selectedToyo, playerParty.Toyos.Count);

            if (playerParty.Toyos.Count == 1)
            {
                prevToyoIndex = -1;
                nextToyoIndex = -1;
            }
            else if (prevToyoIndex == nextToyoIndex)
                nextToyoIndex = -1;

            if (prevToyoIndex != -1)
            {
                leftSlot.sprite = playerParty.Toyos[prevToyoIndex].Base.Sprite;
                leftSlot.color = Color.white;
            }
            else
                leftSlot.color = new Color(1, 1, 1, 0);

            if (nextToyoIndex != -1)
            {
                rightSlot.sprite = playerParty.Toyos[nextToyoIndex].Base.Sprite;
                rightSlot.color = Color.white;
            }
            else
                rightSlot.color = new Color(1, 1, 1, 0);
        }
    }

    int GetNextIndex(int currIndex, int totalCount)
    {
        return (currIndex + 1) % totalCount;
    }

    int GetPrevIndex(int currIndex, int totalCount)
    {
        return currIndex > 0 ? currIndex - 1 : totalCount - 1;
    }

    public void ToggleShowingItemsOrParty()
    {
        showItems = !showItems;
        UpdateSelectionInUI();
    }

    public bool IsShowingItems => showItems;
}
