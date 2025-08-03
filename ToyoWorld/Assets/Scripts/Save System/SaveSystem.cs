using Boom;
using Candid.World.Models;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem i { get; private set; } 
    private void Awake()
    {
        i = this;
    }

    ToyoStorageBoxes storageBoxes;
    Inventory inventory;
    ToyoParty playerParty;

    private void Start()
    {
        storageBoxes = ToyoStorageBoxes.GetPlayerStorageBoxes();
        inventory = Inventory.GetInventory();
        playerParty = PlayerController.i.Party;

        inventory.OnUpdated += Save;
        storageBoxes.OnUpdated += Save;
        playerParty.OnPartyUpdated += Save;

        StartCoroutine(SaveScheduler());

        EntityUtil.TryGetFieldAsText(BoomManager.Instance.PrincipalId, "save_data", "savedata", out var outVal, "None");

        if (!string.IsNullOrEmpty(outVal))
        {
            var saveData = JsonUtility.FromJson<SaveData>(outVal);

            if (saveData.sceneId != 0 && saveData.sceneId != 1)
                LoadNormal(saveData);
        }
    }

    Queue<string> saveQueue = new Queue<string>();
    IEnumerator SaveScheduler()
    {
        saveQueue = new Queue<string>();

        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (isExecutingAction) continue;

            if (saveQueue.Count > 0)
            {
                Debug.Log("Executing save");

                var saveData = saveQueue.Dequeue();
                isExecutingAction = true;
                yield return new WaitForSeconds(1f);

                try
                {
                    ExecuteAction(saveData).Forget();
                }
                catch (Exception e)
                {
                    Debug.LogError("Save Exception - " + e.Message);
                }
            }
        }
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Y))
        //{
        //    Save();
        //}
        //else if (Input.GetKeyDown(KeyCode.U))
        //{
        //    EntityUtil.TryGetFieldAsText(BoomManager.Instance.PrincipalId, "save_data", "savedata", out var outVal, "None");
        //    Debug.Log(outVal);
        //    var saveData = JsonUtility.FromJson<SaveData>(outVal);

        //    StartCoroutine(Load(saveData));
        //}
    }

    public void Save()
    {
        var saveData = new SaveData()
        {
            sceneId = SceneManager.GetActiveScene().buildIndex,
            playerSaveData = PlayerController.i.CaptureState() as PlayerSaveData,
            boxData = storageBoxes.CaptureState() as BoxSaveData,
            inventoryData = inventory.CaptureState() as InventorySaveData
        };

        string saveDataJson = JsonUtility.ToJson(saveData);
        saveQueue = new Queue<string>();
        saveQueue.Enqueue(saveDataJson);

        Debug.Log(saveDataJson);
    }

    IEnumerator Load(SaveData saveData)
    {
        yield return Fader.i.FadeIn(0.5f);

        PlayerController.i.RestoreState(saveData.playerSaveData);
        storageBoxes.RestoreState(saveData.boxData);
        inventory.RestoreState(saveData.inventoryData);

        yield return Fader.i.FadeOut(0.5f);
    }

    void LoadNormal(SaveData saveData)
    {
        PlayerController.i.RestoreState(saveData.playerSaveData);
        storageBoxes.RestoreState(saveData.boxData);
        inventory.RestoreState(saveData.inventoryData);
    }

    bool isExecutingAction = false;
    public async UniTaskVoid ExecuteAction(string json)
    {
        //SECTION A: Set up arguments
        isExecutingAction = true;

        List<Field> fields = new()
        {
            new("savedata", json),
        };

        //SECTION B: Action execution

        //while (ActionUtil.ActionsInProcess("set_savedata"))
        //{
        //    await Task.Delay(1000);
        //}

        //Here we execute the action by passing the actionId we wantto execute.
        // actionLogText.text = $"Processing Action of id: \"{actionId}\" with arguments:\n{JsonConvert.SerializeObject(fields)}";
        var actionResult = await ActionUtil.ProcessAction("set_savedata", fields);

        //SECTION C: Error handling

        //Here we handle the errors
        bool isError = actionResult.IsErr;

        if (isError)
        {
            string errorMessage = actionResult.AsErr().content;

            Debug.LogError(errorMessage);
            // logCoroutine = StartCoroutine(DisplayTempLog(errorMessage));
            isExecutingAction = false;

            return;
        }

        //SECTION D: At this point the action was successful, therefore we print the username change

        // logCoroutine = StartCoroutine(DisplayTempLog($"You have changed your username to: {newUsername}"));

        isExecutingAction = false;
    }
}

[System.Serializable]
public class SaveData
{
    public int sceneId;
    public PlayerSaveData playerSaveData;
    public BoxSaveData boxData;
    public InventorySaveData inventoryData;
}
