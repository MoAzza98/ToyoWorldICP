using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatBubble : MonoBehaviour
{
    private string objectName { get; set; }
    private static TextMeshProUGUI nameText;
    private static GameObject nameFrame;

    private void Start()
    {
        nameFrame = GameObject.Find("Name");
        nameText = GameObject.Find("NameText").GetComponent<TextMeshProUGUI>();
    }

    public static void Setup(string name, bool isCharacter)
    {
        if (isCharacter)
        {
            nameFrame.SetActive(true);
            nameText.text = name;
        }
        else
        {
            nameFrame.SetActive(false);
        }
    }
}
