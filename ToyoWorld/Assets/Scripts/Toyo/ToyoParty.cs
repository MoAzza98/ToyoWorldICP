using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public Toyo GetHealthyToyo()
    {
        return toyos.Where(x => x.Hp > 0).FirstOrDefault();
    }

    public List<Toyo> Toyos => toyos; 
}
