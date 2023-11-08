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
    [SerializeField] Text statusText;
    [SerializeField] Image statusImg;

    [SerializeField] Color psnColour;
    [SerializeField] Color dspColour;
    [SerializeField] Color brnColour;
    [SerializeField] Color parColour;
    [SerializeField] Color frzColour;
    [SerializeField] Color slpColour;

    Toyo _toyo;
    Dictionary<ConditionID, Color> statusColours;

    public void SetData(Toyo toyo)
    {
        _toyo = toyo;
        nameText.text = toyo.Base.ToyoName;
        Debug.Log("Name is: " + toyo.Base.ToyoName);
        levelText.text = "Lvl. " + toyo.Level;
        hpBar.SetHP((float) toyo.HP/toyo.MaxHP);

        statusColours = new Dictionary<ConditionID, Color>()
        {
            {ConditionID.PSN, psnColour},
            {ConditionID.SLP, slpColour},
            {ConditionID.BRN, brnColour},
            {ConditionID.PAR, parColour},
            {ConditionID.FRZ, frzColour},
            {ConditionID.DSP, dspColour}
        };

        SetStatusText();
        _toyo.OnStatusChanged += SetStatusText;
    }

    void SetStatusText()
    {
        if(_toyo.Status == null)
        {
            statusText.text = "";
            statusImg.gameObject.SetActive(false);
        }
        else
        {
            statusText.text = _toyo.Status.ID.ToString().ToUpper();
            statusImg.gameObject.SetActive(true);
            statusImg.color = statusColours[_toyo.Status.ID];
        }
    }

    public IEnumerator UpdateHP()
    {
        if(_toyo.HpChanged)
        {
            yield return hpBar.SetHPSmooth((float)_toyo.HP / _toyo.MaxHP);
            _toyo.HpChanged = false;
        }
    }
}
