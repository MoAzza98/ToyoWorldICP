using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectorUI : MonoBehaviour
{
    [SerializeField] Button boyButton;
    [SerializeField] Button girlButton;
    [SerializeField] int sceneIdToLoad = 6;

    private void Start()
    {
        boyButton.onClick.AddListener(() => StartGame(0));
        girlButton.onClick.AddListener(() => StartGame(1));
    }

    void StartGame(int gender)
    {
        PlayerPrefs.SetInt("PlayerGender", gender);
        SceneManager.LoadScene(sceneIdToLoad);
    }
}
