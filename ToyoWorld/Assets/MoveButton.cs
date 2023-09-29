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

    void Start()
    {
        yourButton = GetComponent<Button>();
        battleDialogBox = GetComponentInParent<BattleDialogBox>();
        yourButton.onClick.AddListener(ShowButtonMove);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        battleDialogBox.UpdateMoveSelction(buttonMove);
    }

    public void ShowButtonMove()
    {
        Debug.Log(buttonMove.Base.Power);
    }


}
