using DG.Tweening;
using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopState : State<GameController>
{
    [SerializeField] ShopUI shopUI;
    [SerializeField] Wallet playerWallet;

    // input
    public List<ItemBase> Items { get; set; }

    public static ShopState i { get; private set; }
    Inventory playerInventory;
    

    private void Awake()
    {
        i = this;
        playerInventory = Inventory.GetInventory();
    }

    GameController gc;
    public override void Enter(GameController owner)
    {
        gc = owner;

        PlayerController.i.SetControl(false);

        shopUI.transform.localScale = Vector3.zero;
        shopUI.gameObject.SetActive(true);
        shopUI.transform.DOScale(Vector3.one, 0.3f);

        shopUI.SetData(Items);

        shopUI.OnSelected += OnSlotSelected;
        shopUI.OnBack += OnBack;
    }

    public override void Execute()
    {
        shopUI.HandleUpdate();
    }

    public override void Exit()
    {
        shopUI.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
        {
            shopUI.gameObject.SetActive(false);
            PlayerController.i.SetControl(true);
        });

        shopUI.OnSelected -= OnSlotSelected;
        shopUI.OnBack -= OnBack;
    }

    public void OnSlotSelected(int selection)
    {
        var item = Items[selection];
        StartCoroutine(TryToBuyItem(item));
    }

    IEnumerator TryToBuyItem(ItemBase item)
    {
        yield return DialogueState.i.ShowDialogue($"Do you want to buy a {item.Name}", choices: new List<string>() { "Yes", "No" });

        if (DialogueState.i.SelectedChoice == 0)
        {
            playerInventory.AddItem(item);
            playerWallet.TakeMoney(item.Price);

            yield return DialogueState.i.ShowDialogue($"You bought {item.Name}!");
        }
    }

    public void OnBack()
    {
        gc.StateMachine.Pop();
    }
}
