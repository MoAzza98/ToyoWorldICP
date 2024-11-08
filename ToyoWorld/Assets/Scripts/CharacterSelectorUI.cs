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
    [SerializeField] Vector3 startingPosition;
    [SerializeField] Vector3 startingRotation;

    GameObject essentialObjects;

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
        GameController.i.EnableDefaultObjects();

        SceneManager.LoadScene(sceneIdToLoad);
    }
}
