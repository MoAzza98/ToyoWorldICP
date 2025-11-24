using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlotUI : MonoBehaviour
{
    [SerializeField] TMP_Text nameTxt;
    [SerializeField] TMP_Text priceTxt;
    [SerializeField] Image image;

    ItemBase _item;

    public void SetData(ItemBase item)
    {
        if (item != null)
        {
            _item = item;

            nameTxt.text = item.Name;
            priceTxt.text = "" + item.Price;
            image.sprite = item.Icon;
            image.color = new Color(255, 255, 255, 100);
        }
    }

    public void ClearData()
    {
        _item = null;

        nameTxt.text = "";
        priceTxt.text = "";
        image.sprite = null;
        image.color = new Color(255, 255, 255, 0);
    }
}
