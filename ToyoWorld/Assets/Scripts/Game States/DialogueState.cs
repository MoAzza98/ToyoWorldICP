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

    [SerializeField] ChoiceSelectionUI choiceBoxUI;

    bool choiceBoxOpened = false;
    bool choiceSelected = false;
    public int SelectedChoice { get; private set; }

    public static DialogueState i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    public IEnumerator ShowDialogue(string text, bool exitCurrState=false, List<string> choices=null)
    {
        GameController.i.StateMachine.Push(this, exitCurrState);

        dialogueBox.SetActive(true);
        
        dialogueText.text = "";
        foreach (var letter in text)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1/lettersPerSecond);
        }

        if (choices != null && choices.Count > 0)
        {
            choiceBoxUI.SetChoices(choices);
            choiceBoxUI.OnSelected += OnChoiceSelected;

            choiceBoxOpened = true;
            choiceSelected = false;
            yield return new WaitUntil(() => choiceSelected == true);
        }

        if (!choiceSelected)
            yield return new WaitForSeconds(1f);

        dialogueBox.SetActive(false);
        GameController.i.StateMachine.Pop(exitCurrState);
    }

    public override void Enter(GameController owner)
    {
        
    }

    public override void Execute()
    {
        if (choiceBoxOpened)
            choiceBoxUI.HandleUpdate();
    }

    public override void Exit()
    {
        
    }

    void OnChoiceSelected(int selection)
    {
        choiceSelected = true;
        choiceBoxOpened = false;
        SelectedChoice = selection;

        choiceBoxUI.OnSelected -= OnChoiceSelected;
        choiceBoxUI.gameObject.SetActive(false);
    }
}
