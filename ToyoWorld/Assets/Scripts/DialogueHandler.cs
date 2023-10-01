using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueHandler : MonoBehaviour
{
    [TextArea]
    public List<string> dialogueText = new List<string>();

    [SerializeField] int lettersPerSecond;

    public DialogueManager dialogManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator ChatDialog(string dialog)
    {
        dialogManager.panelText.text = "";
        foreach (var letter in dialog.ToCharArray())
        {
            dialogManager.panelText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator ChatSetDialog()
    {
        dialogManager.OpenPanel();
        for(int i = 0; i < dialogueText.Count; i++)
        {
            dialogManager.panelText.text = "";
            foreach (var letter in dialogueText[i].ToCharArray())
            {
                dialogManager.panelText.text += letter;
                yield return new WaitForSeconds(1f / lettersPerSecond);
            }
            yield return new WaitForSeconds(1f);
            if(Input.GetKey(KeyCode.Escape))
            {
                break;
            }
        }
        dialogManager.ClosePanel();
    }
}
