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
    public int Exp { get; set; }
    public List<Move> Moves { get; set; }

    public Dictionary<Stat, int> Stats { get; private set; }

    public GameObject Model { get; set; }
    public BattleHUD BattleHUD { get; set; }
    public Animator Animator { get; private set; }

    public event Action OnHPChanged;
    public event Action OnLevelChanged;

    public void Init()
    {
        // Generate the moves based on the level
        Moves = new List<Move>();
        foreach (var move in _base.LearnableMoves.OrderByDescending(m => m.Level))
        {
            if (move.Level <= level)
                Moves.Add(new Move(move.MoveBase));

            if (Moves.Count == 4)
                break;
        }

        CalculateStats();
        Hp = MaxHp;
    }

    public void SetModel(GameObject model)
    {
        if (Model != null) return;
        
        Model = model;
        Animator = Model.GetComponent<Animator>();
        Animator.applyRootMotion = true;
    }

    public void ShowHUD()
    {
        BattleHUD.gameObject.SetActive(true);
        BattleHUD.SetData(this);
    }

    void CalculateStats()
    {
        Stats = new Dictionary<Stat, int>();
        Stats.Add(Stat.Attack, Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5);
        Stats.Add(Stat.Defense, Mathf.FloorToInt((Base.Defense * Level) / 100f) + 5);
        Stats.Add(Stat.SpAttack, Mathf.FloorToInt((Base.SpAttack * Level) / 100f) + 5);
        Stats.Add(Stat.SpDefense, Mathf.FloorToInt((Base.SpDefense * Level) / 100f) + 5);
        Stats.Add(Stat.Speed, Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5);

        int oldMaxHP = MaxHp;
        MaxHp = Mathf.FloorToInt((Base.MaxHp * Level) / 100f) + 10 + Level;

        if (oldMaxHP != 0)
            Hp += MaxHp - oldMaxHP;
    }

    int GetStat(Stat stat)
    {
        int statVal = Stats[stat];

        // Apply stat boost
        //int boost = StatBoosts[stat];
        //var boostValues = new float[] { 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f };

        //if (boost >= 0)
        //    statVal = Mathf.FloorToInt(statVal * boostValues[boost]);
        //else
        //    statVal = Mathf.FloorToInt(statVal / boostValues[-boost]);

        return statVal;
    }

    public int Attack => GetStat(Stat.Attack);
    public int Defense => GetStat(Stat.Defense);
    public int SpAttack => GetStat(Stat.SpAttack);
    public int SpDefense => GetStat(Stat.SpDefense);
    public int Speed => GetStat(Stat.Speed);
    public int MaxHp { get; private set; }

    public DamageDetails TakeDamage(Move move, Toyo attacker)
    {
        float critical = 1f;
        if (UnityEngine.Random.value * 100f <= 6.25f)
            critical = 2f;

        float type = TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type1) * TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type2);

        float attack = move.Base.Category == MoveCategory.Physical ? attacker.Attack : attacker.SpAttack;
        float defense = move.Base.Category == MoveCategory.Physical ? Defense : SpDefense;

        float modifiers = UnityEngine.Random.Range(0.85f, 1f) * type;
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attack / defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        DecreaseHP(damage);

        var damageDetails = new DamageDetails()
        {
            TypeEffectiveness = type,
            Critical = critical
        };

        return damageDetails;
    }

    public void PlayAnimation(string animName)
    {
        Animator.CrossFade(animName, 0.2f);
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

    public bool CheckAndHandleLevelUp()
    {
        if (Exp > Base.GetExpForLevel(level + 1))
        {
            ++level;
            OnLevelChanged?.Invoke();
            CalculateStats();
            return true;
        }

        return false;
    }

    public float GetNormalizedExp()
    {
        int currLevelExp = Base.GetExpForLevel(Level);
        int nextLevelExp = Base.GetExpForLevel(Level + 1);

        float normalizedExp = (float)(Exp - currLevelExp) / (nextLevelExp - currLevelExp);
        return Mathf.Clamp01(normalizedExp);
    }

    public ToyoBase Base => _base;
    public int Level => level;
}

public class DamageDetails
{
    public float Critical { get; set; }
    public float TypeEffectiveness { get; set; }
}

public enum Stat
{
    Attack,
    Defense,
    SpAttack,
    SpDefense,
    Speed,

    // These 2 are not actual stats, they're used to boost the moveAccuracy
    Accuracy,
    Evasion
}
