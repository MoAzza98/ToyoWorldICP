using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toyo
{
    ToyoBase _base;
    int level;

    public int HP { get; set; }

    public List<Move> Moves { get; set; }

    public Toyo(ToyoBase tBase, int tLevel)
    {
        _base = tBase;
        level = tLevel;
        HP = _base.MaxHP;

        Moves = new List<Move>();
        foreach(var move in _base.LearnableMoves)
        {
            if(move.Level <= level)
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
        get { return Mathf.FloorToInt((_base.Attack * level) / 100f) + 5; }
    }

    public int Defence
    {
        get { return Mathf.FloorToInt((_base.Attack * level) / 100f) + 5; }
    }

    public int SpAtk
    {
        get { return Mathf.FloorToInt((_base.Attack * level) / 100f) + 5; }
    }

    public int SpDef
    {
        get { return Mathf.FloorToInt((_base.Attack * level) / 100f) + 5; }
    }

    public int Speed
    {
        get { return Mathf.FloorToInt((_base.Attack * level) / 100f) + 5; }
    }

    public int MaxHP
    {
        get { return Mathf.FloorToInt((_base.Attack * level) / 100f) + 10; }
    }
}
