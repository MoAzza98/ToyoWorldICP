using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toyo
{
    public ToyoBase Base { get; set; }
    public int Level { get; set; }

    public int HP { get; set; }

    public List<Move> Moves { get; set; }

    public Toyo(ToyoBase tBase, int tLevel)
    {
        Base = tBase;
        Level = tLevel;
        HP = MaxHP; 

        Moves = new List<Move>();
        foreach(var move in Base.LearnableMoves)
        {
            if(move.Level <= Level)
            {
                Moves.Add(new Move(move.Base));

                if(Moves.Count >= 4)
                {
                    break;
                }
            }
        }
    }

    public int Attack
    {
        get { return Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5; }
    }

    public int Defence
    {
        get { return Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5; }
    }

    public int SpAtk
    {
        get { return Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5; }
    }

    public int SpDef
    {
        get { return Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5; }
    }

    public int Speed
    {
        get { return Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5; }
    }

    public int MaxHP
    {
        get { return Mathf.FloorToInt((Base.Attack * Level) / 100f) + 10; }
    }
}
