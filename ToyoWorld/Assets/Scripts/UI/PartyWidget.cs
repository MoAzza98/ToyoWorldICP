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

    int selectedToyo = 0;
    public Toyo SelectedToyo => playerParty.Toyos[selectedToyo];

    ToyoParty playerParty;
    private void Start()
    {
        playerParty = PlayerController.i.GetComponent<ToyoParty>();

        UpdateSelectionInUI();
        centerSlot.color = Color.white;
    }

    private void Update()
    {
        float prevSelection = selectedToyo;

        if (Input.GetKeyDown(KeyCode.Q))
            selectedToyo = GetPrevToyoIndex(selectedToyo);
        else if (Input.GetKeyDown(KeyCode.E))
            selectedToyo = GetNextToyoIndex(selectedToyo);

        if (selectedToyo != prevSelection)
        {
            UpdateSelectionInUI();
        }
    }

    public void UpdateSelectionInUI()
    {
        var toyo = playerParty.Toyos[selectedToyo];

        nameTxt.text = toyo.Base.Name;
        lvlTxt.text = toyo.Level.ToString();
        centerSlot.sprite = toyo.Base.Sprite;

        int nextToyoIndex = GetNextToyoIndex(selectedToyo);
        int prevToyoIndex = GetPrevToyoIndex(selectedToyo);

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

    int GetNextToyoIndex(int currIndex)
    {
        return (currIndex + 1) % playerParty.Toyos.Count;
    }

    int GetPrevToyoIndex(int currIndex)
    {
        return currIndex > 0 ? currIndex - 1 : playerParty.Toyos.Count - 1;
    }
}
