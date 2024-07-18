using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoxPartySlotUI : MonoBehaviour
{
    [SerializeField] TMP_Text nameTxt;
    [SerializeField] TMP_Text lvlTxt;
    [SerializeField] Image image;

    public void SetData(Toyo toyo)
    {
        nameTxt.text = toyo.Base.name;
        lvlTxt.text = "Lv. " + toyo.Level;
        image.sprite = toyo.Base.Sprite;
        image.color = new Color(255, 255, 255, 100);
    }

    public void ClearData()
    {
        nameTxt.text = "";
        lvlTxt.text = "";
        image.sprite = null;
        image.color = new Color(255, 255, 255, 0);
    }
}
