using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Create new pokeball")]
public class PokeballItem : ItemBase
{
    [SerializeField] float catchRateModfier = 1;
    [SerializeField] Pokeball pokeballModel;

    public override bool Use(Toyo toyo)
    {
        return true;
    }

    public float CatchRateModifier => catchRateModfier;
    public Pokeball PokeballModel => pokeballModel;
}
