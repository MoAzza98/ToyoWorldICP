using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MoveButton : MonoBehaviour, IPointerEnterHandler
{
    public Move buttonMove;
    public Button yourButton;
    public BattleDialogBox battleDialogBox;
    public BattleSystem battleSystem;

    void Start()
    {
        yourButton = GetComponent<Button>();
        battleDialogBox = GetComponentInParent<BattleDialogBox>();
        battleSystem = GameObject.FindAnyObjectByType<BattleSystem>().GetComponent<BattleSystem>();
        yourButton.onClick.AddListener(UseMove);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        battleDialogBox.UpdateMoveSelction(buttonMove);
    }

    public void UseMove()
    {
        battleSystem.UseCurrentMove(buttonMove);
    }
}
