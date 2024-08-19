using Boom;
using Candid.World.Models;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            var saveData = new SaveData()
            {
                playerPosition = transform.position,
                playerRotation = transform.rotation
            };

            string saveDataJson = JsonUtility.ToJson(saveData);
            ExecuteAction(saveDataJson).Forget();

            Debug.Log(saveDataJson);
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            EntityUtil.TryGetFieldAsText(BoomManager.Instance.PrincipalId, "save_data", "savedata", out var outVal, "None");
            Debug.Log(outVal);
            var saveData = JsonUtility.FromJson<SaveData>(outVal);

            StartCoroutine(Load(saveData));
        }
    }

    IEnumerator Load(SaveData saveData)
    {
        yield return Fader.i.FadeIn(0.5f);

        transform.position = saveData.playerPosition;
        transform.rotation = saveData.playerRotation;
        Physics.SyncTransforms();

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
    public Vector3 playerPosition;
    public Quaternion playerRotation;
}
