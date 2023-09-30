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

    Toyo _toyo;

    public void SetData(Toyo toyo)
    {
        _toyo = toyo;
        nameText.text = toyo.Base.ToyoName;
        Debug.Log("Name is: " + toyo.Base.ToyoName);
        levelText.text = "Lvl. " + toyo.Level;
        hpBar.SetHP((float) toyo.HP/toyo.MaxHP);
    }

    public IEnumerator UpdateHP()
    {
        yield return hpBar.SetHPSmooth((float)_toyo.HP / _toyo.MaxHP);
    }
}
