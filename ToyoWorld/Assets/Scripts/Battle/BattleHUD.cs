using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;

    public void SetData(Toyo toyo)
    {
        nameText.text = toyo.Base.ToyoName;
        Debug.Log("Name is: " + toyo.Base.ToyoName);
        levelText.text = "Lvl. " + toyo.Level;
        hpBar.SetHP((float) toyo.HP/toyo.MaxHP);
    }
}
