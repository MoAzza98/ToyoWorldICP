using DG.Tweening;
using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageState : State<GameController>
{
    [SerializeField] ToyoStorageUI storageUI;
    [SerializeField] ToyoParty party;

    bool isMovingToyo = false;
    int selectedSlotToMove = 0;
    Toyo selectedToyoToMove = null;

    

    public static StorageState i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    private void Start()
    {
        if (party == null)
            party = PlayerController.i.Party;
    }

    GameController gc;
    public override void Enter(GameController owner)
    {
        gc = owner;

        PlayerController.i.SetControl(false);

        storageUI.transform.localScale = Vector3.zero;
        storageUI.gameObject.SetActive(true);
        storageUI.transform.DOScale(Vector3.one, 0.3f);

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
        storageUI.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
        {
            storageUI.gameObject.SetActive(false);
            PlayerController.i.SetControl(true);
        });
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

            // Don't allow to move if the is ony one toyo left in the party
            if (secondToyo == null && storageUI.IsPartySlot(firstSlotIndex) && party.Toyos.Count == 1)
            {
                storageUI.PutToyoIntoSlot(selectedToyoToMove, selectedSlotToMove);

                storageUI.SetDataInStorageSlots();
                storageUI.SetDataInPartySlots();
                return;
            }

            // Moving to a differnt party slot
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

            storageUI.SetDataInStorageSlots();
            storageUI.SetDataInPartySlots();
        }
        else
        {
            gc.StateMachine.Pop();
        }
    }
}
