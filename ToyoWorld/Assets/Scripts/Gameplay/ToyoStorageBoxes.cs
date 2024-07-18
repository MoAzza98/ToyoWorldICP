using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyoStorageBoxes : MonoBehaviour
{
    const int numberOfBoxes = 16;
    const int numberOfSlots = 30;

    Toyo[,] boxes = new Toyo[numberOfBoxes, numberOfSlots];

    public void AddToyo(Toyo toyo, int boxIndex, int slotIndex)
    {
        boxes[boxIndex, slotIndex] = toyo;
    }

    public void RemoveToyo(int boxIndex, int slotIndex)
    {
        boxes[boxIndex, slotIndex] = null;
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
                    return;
                }
            }
        }
    }

    public static ToyoStorageBoxes GetPlayerStorageBoxes()
    {
        return FindObjectOfType<PlayerController>().GetComponent<ToyoStorageBoxes>();
    }
}
