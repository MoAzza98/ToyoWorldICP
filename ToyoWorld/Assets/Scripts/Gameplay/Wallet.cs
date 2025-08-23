using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField] float startingMoney = 500;

    public static Wallet i { get; private set; }

    private void Awake()
    {
        i = this;
        if (Money == default(float))
            Money = startingMoney;
    }

    public event Action OnMoneyChanged;

    public float Money { get; private set; }

    // add money to wallet
    public void AddMoney(float amount)
    {
        Money += amount;
        OnMoneyChanged?.Invoke();
    }

    // take money from wallet, return true if successful, false if not enough money
    public bool TakeMoney(float amount)
    {
        if (Money >= amount)
        {
            Money -= amount;
            OnMoneyChanged?.Invoke();
            return true;
        }
        return false;
    }

    public void SetMoney(float amount)
    {
        Money = amount;
        OnMoneyChanged?.Invoke();
    }
}
