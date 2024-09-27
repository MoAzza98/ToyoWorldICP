using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageState : State<GameController>
{
    [SerializeField] ToyoStorageUI storageUI;

    bool isMovingToyo = false;
    int selectedSlotToMove = 0;
    Toyo selectedToyoToMove = null;

    ToyoParty party;

    public static StorageState i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    private void Start()
    {
        party = PlayerController.i.PlayerParty;
    }

    GameController gc;
    public override void Enter(GameController owner)
    {
        gc = owner;

        storageUI.gameObject.SetActive(true);
        storageUI.SetDataInPartySlots();
        storageUI.SetDataInStorageSlots();

        storageUI.OnSelected += OnSlotSelected;
        storageUI.OnBack += OnBack;
    }

    public override void Execute()
    {
        storageUI.HandleUpdate();
    }

    public override void Exit()
    {
        storageUI.gameObject.SetActive(false);
        storageUI.OnSelected -= OnSlotSelected;
        storageUI.OnBack -= OnBack;
    }

    void OnSlotSelected(int slotIndex)
    {
        if (!isMovingToyo)
        {
            var pokemon = storageUI.TakeToyoFromSlot(slotIndex);
            if (pokemon != null)
            {
                isMovingToyo = true;
                selectedSlotToMove = slotIndex;
                selectedToyoToMove = pokemon;
            }
        }
        else
        {
            isMovingToyo = false;

            int firstSlotIndex = selectedSlotToMove;
            int secondSlotIndex = slotIndex;

            var secondToyo = storageUI.TakeToyoFromSlot(slotIndex);

            if (secondToyo == null && storageUI.IsPartySlot(firstSlotIndex) && party.Toyos.Count == 1)
                return;

            if (secondToyo == null && storageUI.IsPartySlot(firstSlotIndex) && storageUI.IsPartySlot(secondSlotIndex))
            {
                storageUI.PutToyoIntoSlot(selectedToyoToMove, selectedSlotToMove);

                storageUI.SetDataInStorageSlots();
                storageUI.SetDataInPartySlots();
                return;
            }

            storageUI.PutToyoIntoSlot(selectedToyoToMove, secondSlotIndex);

            if (secondToyo != null)
                storageUI.PutToyoIntoSlot(secondToyo, firstSlotIndex);

            party.Toyos.RemoveAll(p => p == null);
            party.PartyUpdated();

            storageUI.SetDataInStorageSlots();
            storageUI.SetDataInPartySlots();
        }
    }

    void OnBack()
    {
        if (isMovingToyo)
        {
            isMovingToyo = false;
            storageUI.PutToyoIntoSlot(selectedToyoToMove, selectedSlotToMove);
        }
        else
        {
            gc.StateMachine.Pop();
        }
    }
}
