using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : ScriptableObject
{
    [SerializeField] string name;
    [SerializeField] string description;
    [SerializeField] Sprite icon;
    [SerializeField] GameObject overworldModel;
    [SerializeField] float price;
    [SerializeField] bool isSellable;
    [SerializeField] ItemRarity rarity;

    public virtual string Name => name;
    public string Description => description;
    public Sprite Icon => icon;
    public GameObject OverworldModel => overworldModel;

    public float Price => price;
    public bool IsSellable => isSellable;

    public ItemRarity Rarity => rarity;

    public virtual bool Use(Toyo toyo)
    {
        return false;
    }

    public virtual bool IsReusable => false;

    public virtual bool CanUseInBattle => true;
    public virtual bool CanUseOutsideBattle => true;
}

public enum ItemRarity { Common, Uncommon, Rare, Legendary }
