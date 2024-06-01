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

    [SerializeField] GameObject vfx;
    [SerializeField] Vector3 vfxOffset = new Vector3(0, 0, 1);

    public string Name => name;
    public string Description => description;

    public ToyoType Type => type;
    public MoveCategory Category => category;

    public int Power => power;
    public int Accuracy => accuracy;
    public int MaxPP => maxPP;

    public GameObject VFX => vfx;
    public Vector3 VFXOffset => vfxOffset;
}

public enum MoveCategory { Physical, Special, Status }
