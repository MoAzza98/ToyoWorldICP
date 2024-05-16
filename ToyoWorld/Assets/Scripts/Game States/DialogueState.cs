using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueState : State<GameController>
{
    [SerializeField] float lettersPerSecond = 30f;

    [SerializeField] GameObject dialogueBox;
    [SerializeField] TMP_Text dialogueText;

    public static DialogueState i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    public IEnumerator ShowDialogue(string text, bool exitCurrState=false)
    {
        GameController.i.StateMachine.Push(this, exitCurrState);

        dialogueBox.SetActive(true);
        
        dialogueText.text = "";
        foreach (var letter in text)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1/lettersPerSecond);
        }

        yield return new WaitForSeconds(1f);

        dialogueBox.SetActive(false);
        GameController.i.StateMachine.Pop(exitCurrState);
    }

    public override void Enter(GameController owner)
    {
        
    }

    public override void Execute()
    {
        
    }

    public override void Exit()
    {
        
    }
}
