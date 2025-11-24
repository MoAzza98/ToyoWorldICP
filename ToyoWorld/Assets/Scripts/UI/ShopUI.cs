using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ShopUI : SelectionUI<ButtonImageSlot>
{
    [SerializeField] ShopSlotUI shopSlotUI;
    [SerializeField] GameObject shopSlotList;
    [SerializeField] TMP_Text descriptionText;

    List<ShopSlotUI> shopSlots = new List<ShopSlotUI>();
    List<ItemBase> _items = new List<ItemBase>();

    public void SetData(List<ItemBase> items)
    {
        _items = items;
        shopSlots = new List<ShopSlotUI>();

        // clear existing items
        foreach (Transform child in shopSlotList.transform)
            Destroy(child.gameObject);

        foreach (var item in items)
        {
            var newSlot = Instantiate(shopSlotUI, shopSlotList.transform);
            newSlot.SetData(item);
            shopSlots.Add(newSlot);
        }

        SetItems(shopSlots.Select(s => s.GetComponent<ButtonImageSlot>()).ToList());
    }

    public override void UpdateSelectionInUI()
    {
        base.UpdateSelectionInUI();

        if (_items != null)
            descriptionText.text = _items[selectedItem].Description;
    }
}
