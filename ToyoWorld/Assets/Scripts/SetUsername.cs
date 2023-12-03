using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetUsername : MonoBehaviour
{
    public TextMeshProUGUI usernameInput;
    public TextMeshProUGUI usernameGUI;

    // Start is called before the first frame update
    void Start()
    {
        usernameGUI.text = PlayerPrefs.GetString("Username");
        gameObject.SetActive(!GameController.instance.setName);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetName()
    {
        PlayerPrefs.SetString("Username", usernameInput.text.ToString());
        usernameGUI.text = PlayerPrefs.GetString("Username");
        GameController.instance.setName = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
