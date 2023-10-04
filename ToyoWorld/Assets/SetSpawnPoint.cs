using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSpawnPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        if(GameController.instance != null)
        {
            transform.position = GameController.instance.sceneDataLoader.playerPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
