using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent
{
    public string eventDescription;
}

public class ToyoBattleEvent: GameEvent
{
    public int toyoBattleAmount;

    public ToyoBattleEvent (int num)
    {
        toyoBattleAmount = num;
    }
}
