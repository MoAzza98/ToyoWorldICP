using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonImageSlot : MonoBehaviour, ISelectableItem, IPointerEnterHandler
{
    [SerializeField] Color activeBgColor;

    int index = 0;
    Action<int> onHover;
    Action<int> onClicked;

    Color originalBgColor;
    Color originalTxtColor;

    Button button;
    Image bgImage;
    private void Awake()
    {
        button = GetComponent<Button>();
        bgImage = button.GetComponent<Image>();

        originalBgColor = bgImage.color;
    }

    public void Init(int index, Action<int> onClicked = null, Action<int> onHover = null)
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
            bgImage.color = (disabled) ? Color.gray : originalBgColor;
        }
    }
}
