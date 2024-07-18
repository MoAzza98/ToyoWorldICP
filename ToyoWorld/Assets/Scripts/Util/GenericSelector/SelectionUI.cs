using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class SelectionUI<T> : MonoBehaviour where T : ISelectableItem
{
    List<T> _items;
    List<T> allItems;
    protected int selectedItem = 0;

    SelectionType selectionType;
    int gridWidth = 2;

    float selectionTimer = 0f;

    public event Action<int> OnSelected;
    public event Action OnBack;

    public void SetSelectionSettings(SelectionType selectionType, int gridWidth)
    {
        this.selectionType = selectionType;
        this.gridWidth = gridWidth;
    }

    public void SetItems(List<T> items)
    {
        this._items = items.Where(i => !i.Disabled).ToList();
        allItems = items;

        //allItems.ForEach(i => i.);

        for (int i = 0; i < _items.Count; i++)
            _items[i].Init(i, OnItemClicked, OnItemHovered);

        UpdateSelectionInUI();
    }

    public virtual void HandleUpdate()
    {
        UpdateSelectionTimer();

        int prevSelection = selectedItem;

        if (selectionType == SelectionType.List)
            HandleListSelection();
        else if (selectionType == SelectionType.Grid)
            HandleGridSelection();

        selectedItem = Mathf.Clamp(selectedItem, 0, _items.Count - 1);

        if (selectedItem != prevSelection)
            UpdateSelectionInUI();

        if (Input.GetButtonDown("Action"))
            OnSelected?.Invoke(selectedItem);
        else if (Input.GetButtonDown("Back"))
            OnBack?.Invoke();
    }

    void HandleListSelection()
    {
        float v = Input.GetAxisRaw("VerticalArrow");

        if (selectionTimer == 0f && Mathf.Abs(v) > 0.2f)
        {
            selectedItem += -(int)Mathf.Sign(v);

            selectionTimer = 0.2f;
        }
    }

    void HandleGridSelection()
    {
        float v = Input.GetAxisRaw("VerticalArrow");
        float h = Input.GetAxisRaw("HorizontalArrow");

        if (selectionTimer == 0 && (Mathf.Abs(v) > 0.2f || Mathf.Abs(h) > 0.2f))
        {
            if (Mathf.Abs(h) > Mathf.Abs(v))
                selectedItem += (int)Mathf.Sign(h);
            else
                selectedItem += -(int)Mathf.Sign(v) * gridWidth;


            selectionTimer = 0.2f;
        }
    }

    public virtual void UpdateSelectionInUI()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            _items[i].SetSelected(i == selectedItem);
        }
    }

    void UpdateSelectionTimer()
    {
        if (selectionTimer > 0)
            selectionTimer = Mathf.Clamp(selectionTimer - Time.deltaTime, 0, selectionTimer);
    }

    public void OnItemClicked(int index)
    {
        if (index >= _items.Count || _items[index].Disabled)
            return;

        int actualIndex = allItems.IndexOf(_items[index]);
        OnSelected?.Invoke(actualIndex);
    }

    public void OnItemHovered(int index)
    {
        if (index >= _items.Count || _items[index].Disabled)
            return;

        selectedItem = index;
        UpdateSelectionInUI();
    }

    public enum SelectionType { List, Grid }
}
