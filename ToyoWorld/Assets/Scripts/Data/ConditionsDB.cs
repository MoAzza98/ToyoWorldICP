using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionsDB
{
    public static void Init()
    {
        foreach(var kvp in Conditions) 
        {
            var conditionId = kvp.Key;
            var condition = kvp.Value;

            condition.ID = conditionId;
        }
    }

    public static Dictionary<ConditionID, Condition> Conditions { get; private set; } = new Dictionary<ConditionID, Condition>()
    {
        {
            ConditionID.PSN, 
            new Condition()
            {
                Name = "Poison",
                StartMessage = "Has been poisoned.",
                OnAfterTurn = (Toyo toyo) =>
                {
                    toyo.UpdateHP(toyo.MaxHP/8);
                    toyo.StatusChanges.Enqueue($"{toyo.Base.name} was hurt by the poison.");
                }
            }
        
        },
        {
            ConditionID.DSP,
            new Condition()
            {
                Name = "Despair",
                StartMessage = "Was afflicted with despair.",
                OnAfterTurn = (Toyo toyo) =>
                {
                    //toyo.ApplyBoost();
                    toyo.StatusChanges.Enqueue($"{toyo.Base.name} was weakened by the despair.");
                }
            }

        },
        {
            ConditionID.BRN,
            new Condition()
            {
                Name = "Burn",
                StartMessage = "Has been burned.",
                OnAfterTurn = (Toyo toyo) =>
                {
                    toyo.UpdateHP(toyo.MaxHP/16);
                    toyo.StatusChanges.Enqueue($"{toyo.Base.name} was hurt by its burn.");
                }
            }
        },
        {
            ConditionID.PAR,
            new Condition()
            {
                Name = "Paralysis",
                StartMessage = "Was paralyzed.",
                OnBeforeMove = (Toyo toyo) =>
                {
                    if(Random.Range(1, 5) == 1)
                    {
                        toyo.StatusChanges.Enqueue($"{toyo.Base.name} was paralyzed and couldn't move!");
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            ConditionID.FRZ,
            new Condition()
            {
                Name = "Freeze",
                StartMessage = "Was frozen.",
                OnBeforeMove = (Toyo toyo) =>
                {
                    if(Random.Range(1, 5) == 1)
                    {
                        toyo.CureStatus();
                        toyo.StatusChanges.Enqueue($"{toyo.Base.name} thawed out of the ice!");
                        return true;
                    }
                    toyo.StatusChanges.Enqueue($"{toyo.Base.name} is frozen and can't move.");
                    return false;
                }
            }
        },
        {
            ConditionID.SLP,
            new Condition()
            {
                Name = "Sleep",
                StartMessage = "Fell asleep.",
                OnStart = (Toyo toyo) =>
                {
                    //sleep for a random number of turns
                    toyo.statusTime = Random.Range(1, 4);
                    Debug.Log($"Will sleep for {toyo.statusTime} turns");
                },
                OnBeforeMove = (Toyo toyo) =>
                {
                    if(toyo.statusTime <= 0)
                    {
                        toyo.CureStatus();
                        toyo.StatusChanges.Enqueue($"{toyo.Base.name} woke up!");
                        return true;
                    }
                    toyo.statusTime--;
                    toyo.StatusChanges.Enqueue($"{toyo.Base.name} is asleep.");
                    return false;
                }
            }
        },
    };
}

public enum ConditionID
{
    NONE,
    PSN,
    BRN,
    SLP,
    PAR,
    FRZ,
    DSP,
    TOX
}
