using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    [SerializeField] List<Toyo> wildToyos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Toyo GetRandomWildToyo()
    {
        var wildToyo = wildToyos[Random.Range(0, wildToyos.Count)];
        wildToyo.Init();
        return wildToyo;
    }
}
