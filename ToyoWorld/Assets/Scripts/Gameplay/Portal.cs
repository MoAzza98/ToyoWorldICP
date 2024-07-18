using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] int sceneToLoad = -1;
    [SerializeField] DestinationIdentifier destinationPortal;
    [SerializeField] Transform spawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(SwitchScene());
        }
    }

    IEnumerator SwitchScene()
    {
        DontDestroyOnLoad(gameObject);

        //GameController.Instance.PauseGame(true);
        yield return Fader.i.FadeIn(0.5f);

        yield return SceneManager.LoadSceneAsync(sceneToLoad);

        var destPortal = FindObjectsOfType<Portal>().First(x => x != this && x.destinationPortal == this.destinationPortal);
        var spawnPos = destPortal.spawnPoint.position;

        // Find ground and to place the player
        float yPos = spawnPos.y;
        var rayOrigin = spawnPos + Vector3.up * 0.1f;
        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 2f, GameLayers.i.GroundLayer))
            yPos = hit.point.y;

        PlayerController.i.transform.position = new Vector3(spawnPos.x, yPos, spawnPos.z);
        Physics.SyncTransforms();

        yield return Fader.i.FadeOut(0.5f);
        //GameController.Instance.PauseGame(false);

        Destroy(gameObject);
    }

    public Transform SpawnPoint => spawnPoint;
}

public enum DestinationIdentifier { A, B, C, D, E }
