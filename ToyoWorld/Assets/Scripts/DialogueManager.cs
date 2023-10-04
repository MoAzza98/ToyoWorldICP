using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] public GameObject dialoguePanel;
    [SerializeField] public TextMeshProUGUI panelText;
    [SerializeField] public TextMeshProUGUI nameText;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClosePanel()
    {
        dialoguePanel.SetActive(false);
    }

    public void OpenPanel()
    {
        dialoguePanel.SetActive(true);
    }



}
