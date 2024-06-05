using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSlot : MonoBehaviour, ISelectableItem, IPointerEnterHandler
{
    [SerializeField] Color activeBgColor;
    [SerializeField] Color activeTxtColor;

    int index = 0;
    Action<int> onHover;
    Action<int> onClicked;

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

    public void Init(int index, Action<int> onClicked=null, Action<int> onHover=null)
    {
        this.index = index;
        this.onClicked = onClicked;
        this.onHover = onHover;

        if (button != null && onClicked != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => onClicked?.Invoke(this.index));
        }
    }

    public void SetSelected(bool selected)
    {
        if (!disabled)
            bgImage.color = (selected) ? activeBgColor : originalBgColor;

        if (!disabled)
            text.color = (selected) ? activeTxtColor : activeBgColor;
    }

    public void SetText(string txt)
    {
        text.text = txt;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameObject.activeInHierarchy && !disabled)
            onHover?.Invoke(index);
    }

    bool disabled;
    public bool Disabled {
        get => disabled;
        set {
            disabled = value;
            //text.color = (disabled) ? Color.gray : originalTxtColor;
            bgImage.color = (disabled) ? Color.gray : originalBgColor;
        }
    }
}
