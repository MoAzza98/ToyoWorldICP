using UnityEngine;

public class SceneDataSaver : MonoBehaviour
{
    private GameObject player;
    private void Start()
    {
        
    }
    public void SaveSceneData()
    {
        player = FindAnyObjectByType<ThirdPersonMovement>().gameObject;
        // Save NPC dialogue state
        //bool hasTalkedToNPC = true;
        //PlayerPrefs.SetInt("HasTalkedToNPC", hasTalkedToNPC ? 1 : 0);

        // Save player position
        Vector3 playerPosition = player.transform.position;
        PlayerPrefs.SetFloat("PlayerPositionX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerPositionY", player.transform.position.y);
        PlayerPrefs.SetFloat("PlayerPositionZ", player.transform.position.z);

        Debug.Log("Saved: " + playerPosition);

        // Save other scene-related information
        // ...

        PlayerPrefs.Save();
    }
}