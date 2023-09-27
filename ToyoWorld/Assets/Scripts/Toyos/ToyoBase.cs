using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Toyo", menuName ="Toyo/Create new toyo")]

public class ToyoBase : ScriptableObject
{
    //Toyo name
    [SerializeField] string toyoName;

    //Toyo description
    [TextArea]
    [SerializeField] string description;

    //Model prefab
    [SerializeField] GameObject toyoPrefab;

    //Type
    [SerializeField] ToyoType type1;
    [SerializeField] ToyoType type2;

    //Base stats
    [SerializeField] int maxHP;
    [SerializeField] int attack;
    [SerializeField] int defence;
    [SerializeField] int spAtk;
    [SerializeField] int spDef;
    [SerializeField] int speed;

    [SerializeField] List<LearnableMove> learnableMoves;

    public string ToyoName
    {
        get { return toyoName; }
    }

    public string Description
    {
        get { return description; }
    }

    public GameObject ToyoPrefab
    {
        get { return toyoPrefab; }
    }

    public ToyoType Type1
    {
        get { return type1; }
    }
    
    public ToyoType Type2
    {
        get { return type2; }
    }

    public int MaxHP
    {
        get { return maxHP; }
    }

    public int Attack
    {
        get { return attack; }
    }

    public int Defence
    {
        get { return defence; }
    }

    public int SpAtk
    {
        get { return spAtk; }
    }

    public int SpDef
    {
        get { return SpDef; }
    }

    public int Speed
    {
        get { return speed; }
    }

    public List<LearnableMove> LearnableMoves
    {
        get { return learnableMoves; }
    }

}

[System.Serializable]
public class LearnableMove
{
    [SerializeField] MoveBase moveBase;
    [SerializeField] int level;

    public MoveBase Base { get { return moveBase;} }
    public int Level { get { return level;} }
}

public enum ToyoType
{
    None,
    Normal, 
    Fire,
    Water,
    Electric,
    Grass,
    Digi,
    Cosmic,
    Rock,
    Dragon,
    Fairy,
    Ice,
    Dream,
    Dark,
    Beast
}
