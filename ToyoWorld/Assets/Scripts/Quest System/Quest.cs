using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public QuestData Data { get; set; }
    public List<QuestTaskState> TaskState { get; set; } = new List<QuestTaskState>();

    public Quest(QuestData data)
    {
        Data = data;
        foreach (var task in data.Tasks)
        {
            var taskState = new QuestTaskState();
            TaskState.Add(taskState);

            task.Init(taskState);
            task.RegisterEvents(taskState);
            task.UnregisterEvents(taskState);
        }
    }

    public bool IsComplete()
    {
        for (int i = 0; i < Data.Tasks.Count; i++)
        {
            if (!Data.Tasks[i].IsComplete(TaskState[i]))
            {
                return false;
            }
        }
        return true;
    }

    public Quest(QuestSaveData saveData)
    {
        Data = QuestDB.GetObjectByName(saveData.name);
        if (Data == null)
            Debug.LogError($"Missing quest in DB with name {saveData.name}");

        for (int i = 0; i < Data.Tasks.Count; i++)
        {
            var task = Data.Tasks[i];
            var taskState = new QuestTaskState();
            TaskState.Add(taskState);

            task.RegisterEvents(taskState);
            task.UnregisterEvents(taskState);
        }
    }

    public QuestSaveData GetSaveData()
    {
        var saveData = new QuestSaveData()
        {
            name = Data.name,
            taskStates = TaskState.GetRange(0, TaskState.Count - 1)
        };
        return saveData;
    }
}

[System.Serializable]
public class QuestSaveData
{
    public string name;
    public List<QuestTaskState> taskStates;
}
