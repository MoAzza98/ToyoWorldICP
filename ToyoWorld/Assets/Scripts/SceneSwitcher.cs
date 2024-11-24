using Boom;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public int characterSelectionScene = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwapScene()
    {
        String outVal = null;
        try
        {
            EntityUtil.TryGetFieldAsText(BoomManager.Instance.PrincipalId, "save_data", "savedata", out outVal, null);
        }
        catch (Exception e)
        {
            Debug.LogError("Load Exception - " + e.Message);
            outVal = null;
        }
        Debug.Log(outVal);

        if (String.IsNullOrEmpty(outVal))
        {
            SceneManager.LoadScene(characterSelectionScene);
        }
        else
        {
            var saveData = JsonUtility.FromJson<SaveData>(outVal);

            if (saveData.sceneId != 0)
            {
                GameController.i.EnableDefaultObjects();
                SceneManager.LoadScene(saveData.sceneId);
            }
            else
                SceneManager.LoadScene(characterSelectionScene);
        }
    }

    public void NewGame()
    {
        SceneManager.LoadScene(characterSelectionScene);
    }
}
