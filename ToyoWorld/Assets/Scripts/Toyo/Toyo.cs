using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Toyo
{
    [SerializeField] ToyoBase _base;
    [SerializeField] int level;

    public int Hp { get; set; }
    public List<Move> Moves { get; set; }

    public GameObject Model { get; set; }
    public BattleHUD BattleHUD { get; set; }

    public event Action OnHPChanged;

    public void Init()
    {
        Hp = MaxHp;

        // Generate the moves based on the level
        Moves = new List<Move>();
        foreach (var move in _base.LearnableMoves.OrderByDescending(m => m.Level))
        {
            if (move.Level <= level)
                Moves.Add(new Move(move.MoveBase));

            if (Moves.Count == 4)
                break;
        }
    }

    public void ShowHUD()
    {
        BattleHUD.gameObject.SetActive(true);
        BattleHUD.SetData(this);
    }

    public int Attack => Mathf.FloorToInt((_base.Attack * level) / 100) + 5;
    public int Defense => Mathf.FloorToInt((_base.Defense * level) / 100) + 5;
    public int SpAttack => Mathf.FloorToInt((_base.SpAttack * level) / 100) + 5;
    public int SpDefense => Mathf.FloorToInt((_base.SpDefense * level) / 100) + 5;
    public int Speed => Mathf.FloorToInt((_base.Speed * level) / 100) + 5;
    public int MaxHp => Mathf.FloorToInt((_base.MaxHp * level) / 100) + 10;

    public void TakeDamage(Move move, Toyo attacker)
    {
        float attack = move.Base.Category == MoveCategory.Physical ? attacker.Attack : attacker.SpAttack;
        float defense = move.Base.Category == MoveCategory.Physical ? Defense : SpDefense;

        float modifiers = UnityEngine.Random.Range(0.85f, 1f);
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attack / defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        DecreaseHP(damage);
    }

    void DecreaseHP(int damage)
    {
        Hp = Mathf.Clamp(Hp - damage, 0, MaxHp);
        OnHPChanged?.Invoke();
    }

    public Move GetRandomMove()
    {
        return Moves[UnityEngine.Random.Range(0, Moves.Count)];
    }

    public ToyoBase Base => _base;
    public int Level => level;
}
