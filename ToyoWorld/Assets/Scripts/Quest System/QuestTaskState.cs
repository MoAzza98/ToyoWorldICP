using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestTaskState
{
    public bool complete = false;
    public int currentAmount = 0;
    public int requiredAmount;
}
