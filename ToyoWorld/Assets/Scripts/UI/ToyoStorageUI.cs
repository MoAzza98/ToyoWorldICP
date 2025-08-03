using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ToyoStorageUI : SelectionUI<ButtonImageSlot>
{
    [SerializeField] List<ButtonImageSlot> boxSlots;
    [SerializeField] Image movingToyoImage;

    List<BoxPartySlotUI> partySlots = new List<BoxPartySlotUI>();
    List<BoxStorageSlotUI> storageSlots = new List<BoxStorageSlotUI>();

    List<Image> boxSlotImages = new List<Image>();

    ToyoParty party;
    ToyoStorageBoxes storageBoxes;

    int totalColumns = 7;

    public int SelectedBox { get; private set; } = 0;

    private void Awake()
    {
        foreach (var boxSlot in boxSlots)
        {
            var storageSlot = boxSlot.GetComponent<BoxStorageSlotUI>();
            if (storageSlot != null)
            {
                storageSlots.Add(storageSlot);
            }
            else
            {
                partySlots.Add(boxSlot.GetComponent<BoxPartySlotUI>());
            }
        }

        party = PlayerController.i.Party;
        storageBoxes = ToyoStorageBoxes.GetPlayerStorageBoxes();

        boxSlotImages = boxSlots.Select(b => b.transform.GetChild(0).GetComponent<Image>()).ToList();
        movingToyoImage.gameObject.SetActive(false);
    }

    private void Start()
    {
        SetItems(boxSlots);
        SetSelectionSettings(SelectionType.Grid, totalColumns);
    }

    public void SetDataInPartySlots()
    {
        for (int i = 0; i < partySlots.Count; i++)
        {
            if (i < party.Toyos.Count)
                partySlots[i].SetData(party.Toyos[i]);
            else
                partySlots[i].ClearData();
        }
    }

    public void SetDataInStorageSlots()
    {
        for (int i = 0; i < storageSlots.Count; i++)
        {
            var pokemon = storageBoxes.GetToyo(SelectedBox, i);
            if (pokemon != null)
                storageSlots[i].SetData(pokemon);
            else
                storageSlots[i].ClearData();
        }
    }

    public override void UpdateSelectionInUI()
    {
        base.UpdateSelectionInUI();

        if (movingToyoImage.gameObject.activeSelf)
            movingToyoImage.transform.position = boxSlotImages[selectedItem].transform.position + Vector3.up * 140f;
    }

    public bool IsPartySlot(int slotIndex)
    {
        return slotIndex % totalColumns == 0;
    }

    public Toyo TakeToyoFromSlot(int slotIndex)
    {
        Toyo toyo;
        if (IsPartySlot(slotIndex))
        {
            int partyIndex = slotIndex / totalColumns;

            if (partyIndex >= party.Toyos.Count)
                return null;

            toyo = party.Toyos[partyIndex];
            party.Toyos[partyIndex] = null;
        }
        else
        {
            int boxSlotIndex = slotIndex - (slotIndex / totalColumns + 1);
            toyo = storageBoxes.GetToyo(SelectedBox, boxSlotIndex);
            storageBoxes.RemoveToyo(SelectedBox, boxSlotIndex);
        }

        movingToyoImage.sprite = boxSlotImages[slotIndex].sprite;
        movingToyoImage.transform.position = boxSlotImages[slotIndex].transform.position + Vector3.up * 50f;
        boxSlotImages[slotIndex].color = new Color(1, 1, 1, 0);
        movingToyoImage.gameObject.SetActive(true);

        return toyo;
    }

    public void PutToyoIntoSlot(Toyo pokemon, int slotIndex)
    {
        if (IsPartySlot(slotIndex))
        {
            int partyIndex = slotIndex / totalColumns;

            if (partyIndex >= party.Toyos.Count)
                party.Toyos.Add(pokemon);
            else
                party.Toyos[partyIndex] = pokemon;
        }
        else
        {
            int boxSlotIndex = slotIndex - (slotIndex / totalColumns + 1);
            storageBoxes.AddToyo(pokemon, SelectedBox, boxSlotIndex);
        }

        movingToyoImage.gameObject.SetActive(false);
    }
}
