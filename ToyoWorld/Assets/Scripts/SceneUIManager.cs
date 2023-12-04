using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneUIManager : MonoBehaviour
{
    [SerializeField] GameObject mainmenu;
    [SerializeField] GameObject eventNotice;
    public bool isMainOpen { get; set; }
    bool isMouseLocked = true;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("ReadNotice") == 1)
        {
            eventNotice.SetActive(false);
        }

        if (GameController.instance.setName)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isMouseLocked = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !isMainOpen)
        {
            mainmenu.SetActive(true);
            isMainOpen = true;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && isMainOpen)
        {
            mainmenu.SetActive(false);
            isMainOpen = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (isMouseLocked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                isMouseLocked = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                isMouseLocked = true;
            }
        }
    }

    public void EventNoticeRead()
    {
        PlayerPrefs.SetInt("ReadNotice", 1);
    }
}
