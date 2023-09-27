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
}
