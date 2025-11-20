using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Create new quest")]
public class QuestData : ScriptableObject
{
    [SerializeField] string questName;
    [SerializeField] string description;
    [SerializeField] List<QuestTask> tasks = new List<QuestTask>();
    [SerializeField] float reward;


    public string QuestName => questName;
    public string Description => description;
    public IReadOnlyList<QuestTask> Tasks => tasks;
    public float Reward => reward;
}
