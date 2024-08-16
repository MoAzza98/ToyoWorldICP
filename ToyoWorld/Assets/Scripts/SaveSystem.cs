using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            var saveData = new SaveData()
            {
                playerPosition = transform.position
            };

            string saveDataJson = JsonUtility.ToJson(saveData);

            Debug.Log(saveDataJson);
        }
    }
}

[System.Serializable]
public class SaveData
{
    public Vector3 playerPosition;
}
