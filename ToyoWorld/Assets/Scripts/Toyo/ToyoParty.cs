using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyoParty : MonoBehaviour
{
    [SerializeField] List<Toyo> toyos;

    private void Awake()
    {
        foreach (var toyo in toyos)
        {
            toyo.Init();
        }
    }

    public List<Toyo> Toyos => toyos; 
}
