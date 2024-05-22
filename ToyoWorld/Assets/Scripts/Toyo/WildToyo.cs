using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildToyo : MonoBehaviour
{
    [field: SerializeField] public Toyo Toyo { get; private set; }

    private void Start()
    {
        Toyo.Init();
        Toyo.SetModel(gameObject);
    }
}
