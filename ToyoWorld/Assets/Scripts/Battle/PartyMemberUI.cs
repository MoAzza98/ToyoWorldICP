using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberUI : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;
    [SerializeField] BattleSystem bSystem;
    Button button;

    Toyo _toyo;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SendToyoSwap);
    }

    public void SetData(Toyo toyo)
    {
        _toyo = toyo;

        nameText.text = toyo.Base.ToyoName;
        levelText.text = "Lvl " + toyo.Level;
        hpBar.SetHP((float)toyo.HP/toyo.MaxHP);
    }

    private void SendToyoSwap()
    {
        bSystem.SendToyo(this._toyo);
        Debug.Log("Switching");
    }
    
}
