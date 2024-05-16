using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pokemon/Create a new move")]
public class MoveBase : ScriptableObject
{
    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] ToyoType type;
    [SerializeField] MoveCategory category;

    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] int maxPP;

    public string Name => name;
    public string Description => description;

    public ToyoType Type => type;
    public MoveCategory Category => category;

    public int Power => power;
    public int Accuracy => accuracy;
    public int MaxPP => maxPP;
}

public enum MoveCategory { Physical, Special, Status }
