using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Tasks/Catch Toyo Task")]
public class CatchToyoTask : QuestTask
{
    [SerializeField] ToyoBase toyoToCatch;

    public override void RegisterEvents(QuestTaskState taskState)
    {
        Pokeball.OnToyoCaptured += (toyo) =>
        {
            if (toyoToCatch != null)
            {
                if (toyo.Base == toyoToCatch)
                    taskState.currentAmount++;
            }
            else
                taskState.currentAmount++;
        };
    }

    public override void UnregisterEvents(QuestTaskState taskState)
    {
        // TODO: store delegate reference from register and remove them from here
    }
}
