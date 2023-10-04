using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberUI : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;

    Toyo _toyo;

    public void SetData(Toyo toyo)
    {
        _toyo = toyo;

        nameText.text = toyo.Base.ToyoName;
        levelText.text = "Lvl " + toyo.Level;
        hpBar.SetHP((float)toyo.HP/toyo.MaxHP);
    }
}
