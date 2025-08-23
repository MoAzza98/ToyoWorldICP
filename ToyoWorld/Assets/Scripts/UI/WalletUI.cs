using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WalletUI : MonoBehaviour
{
    [SerializeField] TMP_Text moneyText;

    private void Start()
    {
        UpdateMoneyText();
        Wallet.i.OnMoneyChanged += UpdateMoneyText;
    }

    void UpdateMoneyText()
    {
        moneyText.text = $"${Wallet.i.Money}";
    }
}
