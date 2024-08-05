using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ItemCategory { Items, Toyoballs, Tms }

public class Inventory : MonoBehaviour
{
    [SerializeField] List<ItemSlot> slots;
    [SerializeField] List<ItemSlot> toyoballSlots;
    [SerializeField] List<ItemSlot> tmSlots;

    List<List<ItemSlot>> allSlots;

    public List<ItemSlot> ToyoballSlots => toyoballSlots;

    public event Action OnUpdated;

    private void Awake()
    {
        allSlots = new List<List<ItemSlot>>() { slots, toyoballSlots, tmSlots };
    }

    public static List<string> ItemCategories { get; set; } = new List<string>()
    {
        "ITEMS", "POKEBALLS", "TMs & HMs"
    };

    public List<ItemSlot> GetSlotsByCategory(int categoryIndex)
    {
        return allSlots[categoryIndex];
    }

    public ItemBase GetItem(int itemIndex, int categoryIndex)
    {
        var currenSlots = GetSlotsByCategory(categoryIndex);
        return currenSlots[itemIndex].Item;
    }

    public ItemBase UseItem(int itemIndex, Toyo selectedToyo, int selectedCategory)
    {
        var item = GetItem(itemIndex, selectedCategory);
        return UseItem(item, selectedToyo);
    }

    public ItemBase UseItem(ItemBase item, Toyo selectedToyo)
    {
        bool itemUsed = item.Use(selectedToyo);
        if (itemUsed)
        {
            if (!item.IsReusable)
                RemoveItem(item);

            return item;
        }

        return null;
    }

    public void AddItem(ItemBase item, int count = 1)
    {
        int category = (int)GetCategoryFromItem(item);
        var currentSlots = GetSlotsByCategory(category);

        var itemSlot = currentSlots.FirstOrDefault(slot => slot.Item == item);
        if (itemSlot != null)
        {
            itemSlot.Count += count;
        }
        else
        {
            currentSlots.Add(new ItemSlot()
            {
                Item = item,
                Count = count
            });
        }

        OnUpdated?.Invoke();
    }

    public int GetItemCount(ItemBase item)
    {
        int category = (int)GetCategoryFromItem(item);
        var currentSlots = GetSlotsByCategory(category);

        var itemSlot = currentSlots.FirstOrDefault(slot => slot.Item == item);

        if (itemSlot != null)
            return itemSlot.Count;
        else
            return 0;
    }

    public void RemoveItem(ItemBase item, int countToRemove = 1)
    {
        int category = (int)GetCategoryFromItem(item);
        var currentSlots = GetSlotsByCategory(category);

        var itemSlot = currentSlots.First(slot => slot.Item == item);
        itemSlot.Count -= countToRemove;
        if (itemSlot.Count == 0)
            currentSlots.Remove(itemSlot);

        OnUpdated?.Invoke();
    }

    public bool HasItem(ItemBase item)
    {
        int category = (int)GetCategoryFromItem(item);
        var currentSlots = GetSlotsByCategory(category);

        return currentSlots.Exists(slot => slot.Item == item);
    }

    ItemCategory GetCategoryFromItem(ItemBase item)
    {
        //if (item is RecoveryItem || item is EvolutionItem)
        //    return ItemCategory.Items;
        //else
        if (item is ToyoballItem)
            return ItemCategory.Toyoballs;
        else
            return ItemCategory.Items;
    }

    public static Inventory GetInventory()
    {
        return FindObjectOfType<PlayerController>().GetComponent<Inventory>();
    }
}

[Serializable]
public class ItemSlot
{
    [SerializeField] ItemBase item;
    [SerializeField] int count;

    public ItemSlot()
    {

    }

    //public ItemSlot(ItemSaveData saveData)
    //{
    //    item = ItemDB.GetObjectByName(saveData.name);
    //    count = saveData.count;
    //}

    //public ItemSaveData GetSaveData()
    //{
    //    var saveData = new ItemSaveData()
    //    {
    //        name = item.name,
    //        count = count
    //    };

    //    return saveData;
    //}

    public ItemBase Item {
        get => item;
        set => item = value;
    }
    public int Count {
        get => count;
        set => count = value;
    }
}
