using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGoal : Quest.QuestGoal
{
    public int goalAmount;

    public override string GetDescription()
    {
        return $"Defeat {goalAmount} Toyos in battle!";
    }

    public override void Initialize()
    {
        base.Initialize();
        EventManager.Instance.AddListener<ToyoBattleEvent>(OnBattling);
    }

    private void OnBattling(ToyoBattleEvent eventInfo)
    {
        if(eventInfo.toyoBattleAmount == goalAmount)
        {
            CurrentAmount++;
            Evaluate();
        }
    }
}
