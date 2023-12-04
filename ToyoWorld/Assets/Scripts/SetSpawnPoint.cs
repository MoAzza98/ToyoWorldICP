using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSpawnPoint : MonoBehaviour
{
    Vector3 spawnPos;
    // Start is called before the first frame update
    void Awake()
    {
        spawnPos = transform.position;
        if(GameController.instance != null)
        {
            if(GameController.instance.level == 2 || GameController.instance.level == 3)
            {
                transform.position = GameController.instance.sceneDataLoader.playerPosition;
            }
            else
            {
                transform.position = spawnPos;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
