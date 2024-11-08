using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ToyoStorageBoxes : MonoBehaviour, ISavable
{
    const int numberOfBoxes = 1;
    const int numberOfSlots = 30;

    Toyo[,] boxes = new Toyo[numberOfBoxes, numberOfSlots];

    public event Action OnUpdated;

    public void AddToyo(Toyo toyo, int boxIndex, int slotIndex)
    {
        boxes[boxIndex, slotIndex] = toyo;
        OnUpdated?.Invoke();
    }

    public void RemoveToyo(int boxIndex, int slotIndex)
    {
        if (boxes[boxIndex, slotIndex] != null)
        {
            boxes[boxIndex, slotIndex] = null;
            OnUpdated?.Invoke();
        }
    }

    public Toyo GetToyo(int boxIndex, int slotIndex)
    {
        return boxes[boxIndex, slotIndex];
    }

    public void AddToyoToEmptySlot(Toyo toyo)
    {
        for (int boxIndex = 0; boxIndex < numberOfBoxes; boxIndex++)
        {
            for (int slotIndex = 0; slotIndex < numberOfSlots; slotIndex++)
            {
                if (boxes[boxIndex, slotIndex] == null)
                {
                    boxes[boxIndex, slotIndex] = toyo;
                    OnUpdated?.Invoke();
                    return;
                }
            }
        }
    }

    public static ToyoStorageBoxes GetPlayerStorageBoxes()
    {
        return FindObjectOfType<PlayerController>().GetComponent<ToyoStorageBoxes>();
    }

    public object CaptureState()
    {
        var saveData = new BoxSaveData()
        {
            boxSlots = new List<BoxSlotSaveData>()
        };

        for (int boxIndex = 0; boxIndex < numberOfBoxes; boxIndex++)
        {
            for (int slotIndex = 0; slotIndex < numberOfSlots; slotIndex++)
            {
                var toyo = boxes[boxIndex, slotIndex];
                if (toyo != null)
                {
                    saveData.boxSlots.Add(new BoxSlotSaveData()
                    {
                        boxIndex = boxIndex,
                        slotIndex = slotIndex,
                        toyoData = toyo.GetSaveData()
                    });
                }
            }
        }

        return saveData;
    }

    public void RestoreState(object state)
    {
        var saveData = (state as BoxSaveData);

        for (int boxIndex = 0; boxIndex < numberOfBoxes; boxIndex++)
        {
            for (int slotIndex = 0; slotIndex < numberOfSlots; slotIndex++)
            {
                boxes[boxIndex, slotIndex] = null;
            }
        }

        if (saveData.boxSlots != null)
        {
            foreach (var bs in saveData.boxSlots)
            {
                boxes[bs.boxIndex, bs.slotIndex] = new Toyo(bs.toyoData);
            }
        }
    }
}

[System.Serializable]
public class BoxSaveData
{
    public List<BoxSlotSaveData> boxSlots;
}

[System.Serializable]
public class BoxSlotSaveData
{
    public int boxIndex;
    public int slotIndex;
    public ToyoSaveData toyoData;
}
