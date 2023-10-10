using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneUIManager : MonoBehaviour
{
    bool isMouseLocked = true;
    // Start is called before the first frame update
    void Start()
    {
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
}
