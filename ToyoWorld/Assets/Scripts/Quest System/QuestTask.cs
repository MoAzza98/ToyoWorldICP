using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestTaskType { Amount, Boolean }

public class QuestTask : ScriptableObject
{
    [SerializeField] QuestTaskType taskType;
    [SerializeField] int requiredAmount = 1;

    public virtual void Init(QuestTaskState taskState) 
    {
        if (taskType == QuestTaskType.Amount)
        {
            taskState.requiredAmount = requiredAmount;
        }
    }

    public virtual void RegisterEvents(QuestTaskState tasktState) { }

    public virtual void UnregisterEvents(QuestTaskState tasktState) { }

    public virtual bool IsComplete(QuestTaskState taskState)
    {
        return taskType == QuestTaskType.Boolean ? taskState.complete : taskState.currentAmount >= requiredAmount;
    }
}
