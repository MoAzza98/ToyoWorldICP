using UnityEngine;

public class SceneDataLoader : MonoBehaviour
{
    public Vector3 playerPosition;
    public void LoadSceneData()
    {
        // Load NPC dialogue state
        //bool hasTalkedToNPC = PlayerPrefs.GetInt("HasTalkedToNPC", 0) == 1;

        // Load player position
        float playerPositionX = PlayerPrefs.GetFloat("PlayerPositionX", 0f);
        float playerPositionY = PlayerPrefs.GetFloat("PlayerPositionY", 0f);
        float playerPositionZ = PlayerPrefs.GetFloat("PlayerPositionZ", 0f);

        playerPosition = new Vector3(playerPositionX, playerPositionY, playerPositionZ);

        // Load other scene-related information
        // ...

        // Use the loaded data to set the scene state
        // ...
    }
}