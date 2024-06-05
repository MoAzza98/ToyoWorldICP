using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectableItem
{
    void Init(int index, Action<int> onClicked = null, Action<int> onHover = null);
    void SetSelected(bool selected);

    bool Disabled { get; set; }
}
