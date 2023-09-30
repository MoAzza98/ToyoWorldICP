using Cysharp.Threading.Tasks;
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

public class TypeChart
{
    static float[][] chart =
    {                     //nor fir wat ele grs dig cos rck dra fai ice drm drk bst
        /*nor*/ new float[]{1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 0f, 1f, 0.5f},
        /*fir*/ new float[]{1f,0.5f,0.5f,1f,2f, 1f, 1f, 1f, 1f, 1f, 2f, 1f, 1f, 1f},
        /*wat*/ new float[]{1f,2f,0.5f,0.5f,0.5f,2f,1f, 2f, 1f, 1f,0.5f,1f, 1f, 1f},
        /*ele*/ new float[]{1f, 1f, 2f, 1f,0.5f, 2f, 1f,0f,0.5f,1f, 2f, 1f, 1f, 1f },
        /*grs*/ new float[]{1f,0.5f,2f, 1f,0.5f, 1f, 1f,2f, 1f, 2f,0.5f, 1f, 1f, 1f },
        /*dig*/ new float[]{2f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 2f, 1f, 1f, 1f, 1f },
        /*cos*/ new float[]{2f, 1f, 1f, 1f, 1f, 1f, 0f, 1f, 2f, 2f, 1f,0.5f,0.5f, 1f },
        /*rck*/ new float[]{1f, 2f,0.5f,2f,0.5f,2f, 1f, 1f, 1f, 1f, 1f, 0f, 1f, 1f },
        /*dra*/ new float[]{1f, 1f, 1f,0.5f, 1f, 1f, 1f,1f,2f, 2f, 1f, 2f, 1f, 2f },
        /*fai*/ new float[]{1f, 1f, 1f, 1f, 1f, 0f, 2f, 2f, 1f, 1f, 1f, 1f, 2f, 2f },
        /*ice*/ new float[]{1f,0.5f,2f, 1f, 1f, 1f, 1f, 1f, 2f, 1f,0.5f, 1f, 1f, 1f },
        /*drm*/ new float[]{1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 2f, 1f, 1f,0.5f, 1f },
        /*drk*/ new float[]{1f, 1f, 1f, 1f, 1f, 1f, 2f, 1f, 1f,0.5f,2f, 1f, 1f, 2f },
        /*bst*/ new float[]{2f,0.5f, 1f, 1f, 1f, 1f, 1f, 1f,0.5f, 1f, 1f, 1f, 1f,2f }
    };

    public static float GetEffectiveness(ToyoType attackType, ToyoType defenceType)
    {
        if(attackType == ToyoType.None || defenceType == ToyoType.None) return 1;

        int row = (int)attackType - 1;
        int col = (int)defenceType - 1;

        return chart[row][col];
    }


}
