using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] TMP_Text nameTxt;
    [SerializeField] TMP_Text levelTxt;
    [SerializeField] ProgressBar hpBar;

    Toyo _toyo;
    public void SetData(Toyo pokemon)
    {
        if (_toyo != null)
            _toyo.OnHPChanged -= SetHP;

        _toyo = pokemon;

        nameTxt.text = pokemon.Base.Name;
        levelTxt.text = "Lv. " + pokemon.Level;
        hpBar.SetProgress((float)pokemon.Hp / pokemon.MaxHp);

        pokemon.OnHPChanged += SetHP;
    }

    void SetHP()
    {
        StartCoroutine(hpBar.SetProgressSmooth((float)_toyo.Hp / _toyo.MaxHp));
    }

    public ProgressBar HPBar => hpBar;
}
