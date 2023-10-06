using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatBubble : MonoBehaviour
{
    private GameObject chatPanel;
    private TextMeshProUGUI chatText;

    public static void Create(string Text)
    {
        Transform chatBubbleTransform = Instantiate(GameAssets.i.pfChatBubble, GameAssets.i.playerCanvas.transform);

        chatBubbleTransform.GetComponent<ChatBubble>().Setup(Text);
    }

    private void Awake()
    {
        chatPanel = transform.Find("ChatPanel").gameObject;
        chatText = transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Setup("Hello world!");
    }

    private void Setup(string text)
    {
        chatText.SetText(text);
    }
}
