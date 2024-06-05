using GDEUtils.StateMachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoveSwitchingState : State<GameController>
{
    [SerializeField] PartyWidget partyWidget;
    [SerializeField] GameObject moveSwitchingUI;
    [SerializeField] CurrentMovesUI currentMovesUI;
    [SerializeField] PossibleMovesUI possibleMovesUI;

    [SerializeField] TMP_Text nameTxt;
    [SerializeField] TMP_Text lvlTxt;
    [SerializeField] Image toyoImage;

    int selectedToyoIndex = 0;
    Toyo selectedToyo;

    List<MoveBase> currentMoves = new List<MoveBase>();
    List<MoveBase> possibleMoves = new List<MoveBase>();

    bool firstMoveSelected = false;
    int SelectedMoveType = 0;
    int selectedMoveIndex = 0;


    public static MoveSwitchingState i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    ToyoParty playerParty;
    private void Start()
    {
        
    }

    GameController gc;
    public override void Enter(GameController owner)
    {
        gc = owner;

        moveSwitchingUI.gameObject.SetActive(true);
        
        PlayerController.i.SetControl(false);
        PlayerController.i.FreezeCamera(true);
        partyWidget.gameObject.SetActive(false);

        playerParty = PlayerController.i.GetComponent<ToyoParty>();
        selectedToyo = playerParty.Toyos[selectedToyoIndex];

        currentMoves = selectedToyo.Moves.Select(m => m.Base).ToList();
        possibleMoves = selectedToyo.Base.LearnableMoves.Where(m => !currentMoves.Contains(m.MoveBase) && m.Level <= selectedToyo.Level).
            Select(m => m.MoveBase).ToList();
        UpdateMoveLists();

        currentMovesUI.OnSelected += OnCurrentMoveSelected;
        possibleMovesUI.OnSelected += OnPossibleMoveSelected;
    }

    public override void Execute()
    {
        int prevSelection = selectedToyoIndex;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            selectedToyoIndex = selectedToyoIndex > 0 ? selectedToyoIndex - 1 : playerParty.Toyos.Count - 1;
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            selectedToyoIndex = (selectedToyoIndex + 1) % playerParty.Toyos.Count;

        if (prevSelection != selectedToyoIndex)
        {
            UpdateMoveLists(true);
        }

        if (Input.GetButtonDown("Back"))
        {
            if (firstMoveSelected)
            {
                firstMoveSelected = false;

                possibleMovesUI.DisableSlot(false, selectedMoveIndex);
                currentMovesUI.DisableSlot(false, selectedMoveIndex);
                UpdateMoveLists();
            }
            else
            {
                gc.StateMachine.Pop();
            }
        } 
    }

    public override void Exit()
    {
        moveSwitchingUI.gameObject.SetActive(false);
        partyWidget.gameObject.SetActive(true);
        PlayerController.i.SetControl(true);
        PlayerController.i.FreezeCamera(false);
    }

    void UpdateMoveLists(bool updatePossibleMoves=false)
    {
        selectedToyo = playerParty.Toyos[selectedToyoIndex];
            
        nameTxt.text = selectedToyo.Base.Name;
        lvlTxt.text = "Lv. " + selectedToyo.Level;
        toyoImage.sprite = selectedToyo.Base.Sprite;
        toyoImage.color = Color.white;


        currentMoves = selectedToyo.Moves.Select(m => m.Base).ToList();

        if (updatePossibleMoves)
        {
            possibleMoves = selectedToyo.Base.LearnableMoves.Where(m => !currentMoves.Contains(m.MoveBase) && m.Level <= selectedToyo.Level).
                Select(m => m.MoveBase).ToList();
        }

        currentMovesUI.SetMoves(currentMoves);
        possibleMovesUI.SetMoves(possibleMoves);
    }

    void OnCurrentMoveSelected(int currSelectedMove)
    {
        if (!firstMoveSelected)
        {
            firstMoveSelected = true;
            selectedMoveIndex = currSelectedMove;
            SelectedMoveType = 0;

            currentMovesUI.DisableSlot(true, selectedMoveIndex);
        }
        else
        {
            if (SelectedMoveType == 0) return;

            // Swap the moves
            var tmp = playerParty.Toyos[selectedToyoIndex].Moves[currSelectedMove].Base;
            playerParty.Toyos[selectedToyoIndex].Moves[currSelectedMove] = new Move(possibleMoves[selectedMoveIndex]);
            possibleMoves[selectedMoveIndex] = tmp;
            firstMoveSelected = false;

            possibleMovesUI.DisableSlot(false, selectedMoveIndex);
            UpdateMoveLists();
        }
    }

    void OnPossibleMoveSelected(int currSelectedMove)
    {
        if (!firstMoveSelected)
        {
            firstMoveSelected = true;
            selectedMoveIndex = currSelectedMove;
            SelectedMoveType = 1;

            possibleMovesUI.DisableSlot(true, selectedMoveIndex);
        }
        else
        {
            if (SelectedMoveType == 1) return;

            var tmp = playerParty.Toyos[selectedToyoIndex].Moves[selectedMoveIndex].Base;
            playerParty.Toyos[selectedToyoIndex].Moves[selectedMoveIndex] = new Move(possibleMoves[currSelectedMove]);
            possibleMoves[currSelectedMove] = tmp;
            firstMoveSelected = false;

            currentMovesUI.DisableSlot(false, selectedMoveIndex);

            UpdateMoveLists();
        }
    }
}
