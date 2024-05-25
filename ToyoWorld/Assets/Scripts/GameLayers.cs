using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLayers : MonoBehaviour
{
    [field: SerializeField] public LayerMask GroundLayer { get; private set; }

    public static GameLayers i { get; private set; }
    private void Awake()
    {
        i = this;
    }
}
