using Boom.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks.Triggers;

public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] int lettersPerSecond;

    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject moveDetails;
    [SerializeField] GameObject backButton;

    [SerializeField] List<TextMeshProUGUI> actionTexts;
    [SerializeField] List<TextMeshProUGUI> moveTexts;
    [SerializeField] List<Button> moveButtons;

    [SerializeField] TextMeshProUGUI ppText;
    [SerializeField] TextMeshProUGUI typeText;

    [SerializeField] BattleSystem bSystem;
    BattleState checkState;
    public MoveButton moveBtn { get; set; }

    private bool actionOpen = false;
    private bool moveOpen = false;

    private void Start()
    {
        
    }

    public void SetDialog(string text)
    {
        dialogText.text = text;
    }

    public IEnumerator TypeDialog(string dialog)
    {
        checkState = bSystem.state;
        dialogText.text = "";
        foreach(var letter in dialog.ToCharArray())
        {
            if(checkState != bSystem.state)
            {
                dialogText.text = "";
                break;
            }
            else
            {
                dialogText.text += letter;
                yield return new WaitForSeconds(1f / lettersPerSecond);
            }

        }
        yield return new WaitForSeconds(1f);
    }

    public void GoToActionSelector()
    {
        moveSelector.SetActive(false);
        actionSelector.SetActive(true);
        moveDetails.SetActive(false);
        backButton.SetActive(false);
    }

    public void GoToMoveSelector()
    {
        actionSelector.SetActive(false);
        moveSelector.SetActive(true);
        moveDetails.SetActive(true);
        backButton.SetActive(true);
    }

    public void EnableBackSelector(bool isEnabled)
    {
        backButton.SetActive(isEnabled);
    }

    public void EnableMoveDetails(bool isEnabled)
    {
        moveDetails.SetActive(isEnabled);
    }

    public void EnableActionSelector(bool isEnabled)
    {
        actionSelector.SetActive(isEnabled);
    }

    public void EnableMoveSelector(bool isEnabled)
    {
        moveSelector.SetActive(isEnabled);
    }

    private void SetCurrentAction()
    {

    }

    public void SetMoveNames(List<Move> moves)
    {
        Debug.Log("Setting moves");
        for(int i = 0; i < moveTexts.Count; i++)
        {
            if(i < moves.Count)
            {
                moveTexts[i].text = moves[i].Base.name;
            }
            else
            {
                moveTexts[i].text = "-";
            }
        }
    }

    public void SetButtonMoves(List<Move> moves)
    {
        for (int i = 0; i < moveButtons.Count; i++)
        {
            if (i < moves.Count)
            {
                MoveButton hasMove = moveButtons[i].gameObject.GetComponent<MoveButton>();
                if (hasMove != null)
                {
                    Destroy(moveButtons[i].gameObject.GetComponent<MoveButton>());
                }
                moveBtn = moveButtons[i].gameObject.AddComponent<MoveButton>();
                moveBtn.buttonMove = moves[i];
                Debug.Log($"{moves[i].Base.name} has been added to a button number:" + i +
                    $" {moveBtn.buttonMove.Base.name} should be the same.");
            }
            else
            {
                break;
            }
        }
    }

    void OpenPartyScreen()
    {

    }

    public void UpdateMoveSelction(Move move)
    {
        ppText.text = $"Energy {move.PP}/{move.Base.Pp}";
        typeText.text = $"{move.Base.ToyoType}";
    }

    public void SetMoveInfo()
    {

    }

}
