using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCLines : MonoBehaviour
{
    [SerializeField] STMDialogueSample linesManager;
    [SerializeField] GameObject dialoguePanel;
    private Animator animator;

    [TextArea(5, 5)]
    public string[] npcLines;
    public string npcName;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLines()
    {
        if(animator != null)
        {
            animator.SetTrigger("Talk");
        }
        dialoguePanel.SetActive(true);
        linesManager.gameObject.SetActive(true);
        linesManager.lines = npcLines;
        linesManager.currentLine = 0;
        linesManager.Apply();
    }

    private void OnTriggerExit(Collider other)
    {
        dialoguePanel.SetActive(false);
        linesManager.gameObject.SetActive(false);
    }
}
