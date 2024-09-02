using Boom;
using Candid.World.Models;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem i { get; private set; } 
    private void Awake()
    {
        i = this;
    }

    ToyoStorageBoxes storageBoxes;
    Inventory inventory;
    private void Start()
    {
        storageBoxes = ToyoStorageBoxes.GetPlayerStorageBoxes();
        inventory = Inventory.GetInventory();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Save();
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            EntityUtil.TryGetFieldAsText(BoomManager.Instance.PrincipalId, "save_data", "savedata", out var outVal, "None");
            Debug.Log(outVal);
            var saveData = JsonUtility.FromJson<SaveData>(outVal);

            StartCoroutine(Load(saveData));
        }
    }

    public void Save()
    {
        var saveData = new SaveData()
        {
            playerSaveData = PlayerController.i.CaptureState() as PlayerSaveData,
            boxData = storageBoxes.CaptureState() as BoxSaveData,
            inventoryData = inventory.CaptureState() as InventorySaveData
        };

        string saveDataJson = JsonUtility.ToJson(saveData);
        ExecuteAction(saveDataJson).Forget();

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

    public async UniTaskVoid ExecuteAction(string json)
    {
        //SECTION A: Set up arguments

        List<Field> fields = new()
        {
            new("savedata", json),
        };

        //SECTION B: Action execution

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

            return;
        }

        //SECTION D: At this point the action was successful, therefore we print the username change

        // logCoroutine = StartCoroutine(DisplayTempLog($"You have changed your username to: {newUsername}"));
    }
}

[System.Serializable]
public class SaveData
{
    public PlayerSaveData playerSaveData;
    public BoxSaveData boxData;
    public InventorySaveData inventoryData;
}
