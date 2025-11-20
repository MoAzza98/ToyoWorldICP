using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    [SerializeField] bool autoCheckForCompletion = true;
    [SerializeField] float completionCheckTimeInterval = 1;

    public List<Quest> activeQuests = new List<Quest>();
    public List<Quest> completedQuests = new List<Quest>();

    float timer = 0f;

    public static QuestController i { get; private set; }
    private void Awake()
    {
        i = this;
    }

    public void AddQuest(QuestData questData)
    {
        var quest = new Quest(questData);
        activeQuests.Add(quest);
    }

    public Quest GetActiveQuest(QuestData questData)
    {
        return activeQuests.FirstOrDefault(q => q.Data.name == questData.name);
    }

    public Quest GetCompletedQuest(QuestData questData)
    {
        return completedQuests.FirstOrDefault(q => q.Data.name == questData.name);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > completionCheckTimeInterval)
        {
            timer = 0f;
            CheckForQuestCompletion();
        }
    }

    void CheckForQuestCompletion()
    {
        var newlyCompletedQuests = activeQuests.Where(q => q.IsComplete()).ToList();
        foreach (var quest in newlyCompletedQuests)
        {
            activeQuests.Remove(quest);
            Wallet.i.AddMoney(quest.Data.Reward);
            completedQuests.Add(quest);
        }
    }

    public void LoadQuestData(QuestJournalSaveData saveData)
    {
        activeQuests = saveData.ActiveQuests.Select(q => new Quest(q)).ToList();
        completedQuests = saveData.CompletedQuests.Select(q => new Quest(q)).ToList();
    }

    public QuestJournalSaveData GetSaveData() 
    {
        return new QuestJournalSaveData()
        {
            ActiveQuests = activeQuests.Select(q => q.GetSaveData()).ToList(),
            CompletedQuests = completedQuests.Select(q => q.GetSaveData()).ToList()
        };
    }
}

[System.Serializable]
public class QuestJournalSaveData
{
    public List<QuestSaveData> ActiveQuests;
    public List<QuestSaveData> CompletedQuests;
}
