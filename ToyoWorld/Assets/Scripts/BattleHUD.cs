using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] TMP_Text nameTxt;
    [SerializeField] TMP_Text levelTxt;
    [SerializeField] ProgressBar hpBar;
    [SerializeField] ProgressBar expBar;

    Toyo _toyo;
    public void SetData(Toyo toyo)
    {
        if (_toyo != null)
        {
            _toyo.OnHPChanged -= SetHP;
            _toyo.OnLevelChanged -= SetLevel;
        }

        _toyo = toyo;

        nameTxt.text = toyo.Base.Name;
        SetLevel();
        hpBar.SetProgress((float)toyo.Hp / toyo.MaxHp);
        expBar?.SetProgress(_toyo.GetNormalizedExp());
        expBar?.gameObject.SetActive(false);
        

        toyo.OnHPChanged += SetHP;
        toyo.OnLevelChanged += SetLevel;
    }

    void SetHP()
    {
        StartCoroutine(hpBar.SetProgressSmooth((float)_toyo.Hp / _toyo.MaxHp));
    }
    
    void SetLevel()
    {
        levelTxt.text = "Lv. " + _toyo.Level;
    }

    public void SetExp(float value)
    {
        expBar.gameObject.SetActive(true);
        expBar.SetProgress(value);
    }

    public IEnumerator SetExpSmooth()
    {
        expBar.gameObject.SetActive(true);
        yield return expBar.SetProgressSmooth(_toyo.GetNormalizedExp());
    }

    public void DisableExpBar()
    {
        expBar.gameObject.SetActive(false);
    }

    public ProgressBar HPBar => hpBar;
}
