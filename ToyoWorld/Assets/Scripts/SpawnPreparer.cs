using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPreparer : MonoBehaviour
{
    [SerializeField] Vector3 spawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNewSceneSpawn()
    {
        GameController.instance.sceneDataLoader.playerPosition = spawnPosition;
    }
}
