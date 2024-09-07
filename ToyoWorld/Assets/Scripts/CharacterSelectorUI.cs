using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectorUI : MonoBehaviour
{
    [SerializeField] Button boyButton;
    [SerializeField] Button girlButton;
    [SerializeField] GameObject essentialObjects;
    [SerializeField] int sceneIdToLoad = 6;
    [SerializeField] Vector3 startingPosition;
    [SerializeField] Vector3 startingRotation;

    private void Start()
    {
        boyButton.onClick.AddListener(() => StartGame(0));
        girlButton.onClick.AddListener(() => StartGame(1));
    }

    void StartGame(int gender)
    {
        PlayerPrefs.SetInt("PlayerGender", gender);

        //PlayerController.i.transform.position = startingPosition;
        //PlayerController.i.transform.rotation = Quaternion.Euler(startingRotation);
        essentialObjects.SetActive(true);

        SceneManager.LoadScene(sceneIdToLoad);
    }
}
