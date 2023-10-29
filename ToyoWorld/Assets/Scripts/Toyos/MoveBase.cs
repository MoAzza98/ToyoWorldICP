using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Move", menuName ="Toyo/Create new move")]
public class MoveBase : ScriptableObject
{
    [SerializeField] string moveName;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] ToyoType toyoType;
    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] int pp;
    [SerializeField] MoveCategory category;
    [SerializeField] MoveEffects effects;

    [SerializeField] GameObject effectPrefab;
    [SerializeField] MoveEffectDestination _moveDestination;
    [SerializeField] MoveTarget _moveTarget;

    public string MoveName
    {
        get { return moveName; }
    }
    public string Description
    {
        get { return description; }
    }

    public ToyoType ToyoType
    {
        get { return toyoType; }
    }

    public int Power
    {
        get { return power; }
    }

    public int Accuracy
    {
        get { return accuracy; }
    }

    public int Pp
    {
        get { return pp; }
    }

    public GameObject EffectPrefab
    {
        get { return effectPrefab; }
    }

    public MoveEffects Effects
    {
        get { return effects; }
    }

    public MoveEffectDestination _MoveDestinaiton
    {
        get { return _moveDestination; }
    }

    public MoveCategory Category
    {
        get { return category; }
    }

    public MoveTarget MoveTarget 
    { 
        get { return _moveTarget; } 
    }
}

[System.Serializable]
public class MoveEffects
{
    [SerializeField] List<StatBoost> boosts;

    public List<StatBoost> Boosts
    {
        get { return boosts; }
    }
}

[System.Serializable]
public class StatBoost
{
    public Stat stat;
    public int boost;
}

//Determines where the VFX effect is spawned
public enum MoveEffectDestination
{
    Self,
    Enemy,
    Projectile
}

public enum MoveCategory
{
    Physical,
    Special,
    Status
}

//Who should a move effect, like stat boosting moves
public enum MoveTarget
{
    Self,
    Foe
}
