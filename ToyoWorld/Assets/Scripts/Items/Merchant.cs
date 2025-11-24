using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Merchant : Interactable
{
    [SerializeField] List<MerchantItem> items;
    [SerializeField] List<string> dialogue;

    private void Update()
    {
        if (!isInteracting && playerInRange && GameController.i.StateMachine.CurrentState == FreeRoamState.i)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(OpenShop());
            }
        }
    }

    IEnumerator OpenShop()
    {
        RotateTowardsTarget(PlayerController.i.transform);
        PlayerController.i.ResetTargetRotation();

        OnInteractionStarted();
        yield return DialogueState.i.ShowDialogueLines(dialogue, exitCurrState: true);

        var validItems = items.Where(i => !i.ShowIfQuestCompleted).ToList();
        foreach (var item in items.Where(i => i.ShowIfQuestCompleted && i.Quest != null)) 
        {
            if (QuestController.i.completedQuests.Any(q => q.Data.QuestName == item.Quest.QuestName))
                validItems.Add(item);
        }

        ShopState.i.Items = validItems.Select(m => m.Item).ToList();

        yield return GameController.i.StateMachine.PushAndWait(ShopState.i);

        OnInteractionEnded();
    }
}

[System.Serializable]
public class MerchantItem
{
    public ItemBase Item;
    public bool ShowIfQuestCompleted;
    public QuestData Quest;
}
