using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Tasks/Defeat Toyo Task")]
public class DefeatToyoTask : QuestTask
{
    [SerializeField] ToyoBase toyoToBeat;

    public override void RegisterEvents(QuestTaskState taskState)
    {
        BattleState.i.OnWildToyoFainted += (toyo) =>
        {
            if (toyoToBeat != null)
            {
                if (toyo.Base == toyoToBeat)
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
