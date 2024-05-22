using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pokemon/Create a new pokemon")]
public class ToyoBase : ScriptableObject
{
    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] GameObject model;

    [SerializeField] ToyoType type1;
    [SerializeField] ToyoType type2;

    // Base Stats
    [SerializeField] int maxHp;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int spAttack;
    [SerializeField] int spDefense;
    [SerializeField] int speed;

    [SerializeField] int expYield;
    [SerializeField] GrowthRate growthRate;

    [SerializeField] List<LearnableMove> learnableMoves;

    public int GetExpForLevel(int level)
    {
        if (growthRate == GrowthRate.Fast)
        {
            return 4 * (level * level * level) / 5;
        }
        else if (growthRate == GrowthRate.MediumFast)
        {
            return level * level * level;
        }

        return -1;
    }

    public string Name => name;
    public string Description => description;
    public GameObject Model => model;

    public int MaxHp => maxHp;
    public int Attack => attack;
    public int Defense => defense;
    public int SpAttack => spAttack;
    public int SpDefense => spDefense;
    public int Speed => speed;

    public ToyoType Type1 => type1;
    public ToyoType Type2 => type2;

    public int ExpYield => expYield;

    public List<LearnableMove> LearnableMoves => learnableMoves;
}

public enum GrowthRate
{
    Fast, MediumFast
}

public enum ToyoType
{
    None,
    Normal,
    Fire,
    Water,
    Grass,
    Thunder,
    Beast,
    Space,
    Dragon,
    Earth,
    Air,
    Digi,
    Fairy,
    Time,
    Dark,
    Ice
}

[System.Serializable]
public class LearnableMove
{
    [SerializeField] MoveBase moveBase;
    [SerializeField] int level;

    public MoveBase MoveBase => moveBase;
    public int Level => level;
}

public class TypeChart
{
    static float[][] chart =
    {
         //                      Nor   Fir   Wat   Gra   Thu   Bea   Spa   Dra   Ear   Air   Dig   Fai   Tim   Dar   Ice   Dre
        /*Normal*/  new float[] {1f,   1f,   1f,   1f,   1f,   0.5f, 1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   0f},
        /*Fire*/    new float[] {1f,   0.5f, 0.5f, 2f,   1f,   1f,   1f,   1f,   1f,   2f,   1f,   1f,   1f,   1f,   2f,   1f},
        /*Water*/   new float[] {1f,   2f,   0.5f, 0.5f, 0.5f, 1f,   1f,   1f,   2f,   1f,   2f,   1f,   1f,   1f,   0.5f, 1f},
        /*Grass*/   new float[] {1f,   0.5f, 2f,   0.5f, 1f,   1f,   1f,   1f,   2f,   1f,   1f,   2f,   1f,   1f,   0.5f, 1f},
        /*Thunder*/ new float[] {1f,   1f,   2f,   0.5f, 1f,   1f,   1f,   0.5f, 0f,   1f,   2f,   1f,   1f,   1f,   2f,   1f},
        /*Beast*/   new float[] {2f,   0.5f, 1f,   1f,   1f,   2f,   1f,   0.5f, 1f,   1f,   1f,   1f,   2f,   1f,   1f,   1f},
        /*Space*/   new float[] {2f,   1f,   1f,   1f,   1f,   1f,   0f,   2f,   1f,   1f,   1f,   2f,   2f,   0.5f, 1f,   0.5f},
        /*Dragon*/  new float[] {1f,   1f,   1f,   1f,   0.5f, 2f,   1f,   2f,   1f,   2f,   1f,   2f,   1f,   1f,   1f,   2f},
        /*Earth*/   new float[] {1f,   2f,   0.5f, 0.5f, 2f,   1f,   1f,   1f,   1f,   0.5f, 2f,   1f,   1f,   1f,   1f,   0f},
        /*Air*/     new float[] {1f,   0.5f, 1f,   2f,   1f,   2f,   1f,   0.5f, 2f,   1f,   1f,   1f,   1f,   1f,   1f,   1f},
        /*Digi*/    new float[] {2f,   0.5f, 1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   2f,   1f,   1f,   1f,   1f},
        /*Fairy*/   new float[] {1f,   1f,   1f,   1f,   1f,   2f,   2f,   1f,   2f,   1f,   0f,   1f,   1f,   2f,   1f,   1f},
        /*Time*/    new float[] {1f,   1f,   1f,   1f,   1f,   1f,   2f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f},
        /*Dark*/    new float[] {1f,   1f,   1f,   1f,   1f,   2f,   2f,   1f,   1f,   1f,   1f,   0.5f, 1f,   1f,   1f,   2f},
        /*Ice*/     new float[] {1f,   1f,   2f,   1f,   1f,   1f,   1f,   2f,   1f,   1f,   1f,   1f,   1f,   1f,   0.5f, 1f},
        /*Dream*/   new float[] {1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   2f,   0.5f, 1f,   1f}
    };

    public static float GetEffectiveness(ToyoType attackType, ToyoType defendType)
    {
        if (attackType == ToyoType.None || defendType == ToyoType.None)
            return 1f;

        int row = (int)attackType - 1;
        int col = (int)defendType - 1;

        return chart[row][col];
    }
}
