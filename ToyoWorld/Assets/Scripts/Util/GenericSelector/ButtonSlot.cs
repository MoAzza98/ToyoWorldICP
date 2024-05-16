using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSlot : MonoBehaviour, ISelectableItem
{
    [SerializeField] Color activeBgColor;
    [SerializeField] Color activeTxtColor;

    Color originalBgColor;
    Color originalTxtColor;

    Button button;
    Image bgImage;
    TMP_Text text;
    private void Awake()
    {
        button = GetComponent<Button>();
        bgImage = button.GetComponent<Image>();
        text = button.GetComponentInChildren<TMP_Text>();

        originalBgColor = bgImage.color;
        originalTxtColor = text.color;
    }

    public void SetSelected(bool selected)
    {
        bgImage.color = (selected) ? activeBgColor : originalBgColor;
        text.color = (selected) ? activeTxtColor : activeBgColor;
    }

    public void SetText(string txt)
    {
        text.text = txt;
    }
}
