using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{

    [SerializeField] BattleSystem battleSystem;
    public GameObject player;
    [SerializeField] Toyo wildToyo;

    [SerializeField] public MapArea mapArea;

    private PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        mapArea = FindAnyObjectByType<MapArea>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
