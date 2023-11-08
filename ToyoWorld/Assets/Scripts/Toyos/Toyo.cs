using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Toyo
{
    [SerializeField] ToyoBase _base;
    [SerializeField] int level;

    public int HP { get; set; }
    public List<Move> Moves { get; set; }
    public Dictionary<Stat, int> Stats { get; private set; }
    public Dictionary<Stat, int> StatBoosts { get; private set; }
    public Condition Status { get; private set; }
    public Queue<string> StatusChanges { get; private set; } = new Queue<string>();
    public bool HpChanged { get; set; }
    public int statusTime { get; set; }
    public event System.Action OnStatusChanged;

    public ToyoBase Base 
    { 
        get { return _base; }
    }

    public int Level
    {
        get { return level; }
    }

    public void Init()
    {

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
        CalculateStats();
        HP = MaxHP;
        ResetStatBoosts();
    }

    void CalculateStats()
    {
        Stats = new Dictionary<Stat, int>();
        Stats.Add(Stat.Attack, Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5);
        Stats.Add(Stat.Defence, Mathf.FloorToInt((Base.Defence * Level) / 100f) + 5);
        Stats.Add(Stat.SpAttack, Mathf.FloorToInt((Base.SpAtk * Level) / 100f) + 5);
        Stats.Add(Stat.SpDef, Mathf.FloorToInt((Base.SpDef * Level) / 100f) + 5);
        Stats.Add(Stat.Speed, Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5);

        MaxHP =  Mathf.FloorToInt((Base.MaxHP * Level) / 100f) + 10 + Level;
    }

    void ResetStatBoosts()
    {
        StatBoosts = new Dictionary<Stat, int>()
        {
            {Stat.Attack, 0},
            {Stat.Defence, 0},
            {Stat.SpAttack, 0},
            {Stat.SpDef, 0},
            {Stat.Speed, 0},

        };
    }

    public void ReInitMoves()
    {
        Moves = new List<Move>();
        foreach (var move in Base.LearnableMoves)
        {
            if (move.Level <= Level)
            {
                Moves.Add(new Move(move.Base));

                if (Moves.Count >= 4)
                {
                    break;
                }
            }
        }
    }

    /*
    public void FullRestore()
    {
        HP = MaxHP;
        foreach(var move in Moves)
        {
            move.PP = move.Base.Pp;
        }
    }*/

    int GetStat(Stat stat)
    {
        int statVal = Stats[stat];

        //Todo: apply statboosts

        int boost = StatBoosts[stat];
        var boostValue = new float[] { 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f };

        if (boost >= 0)
        {
            statVal = Mathf.FloorToInt(statVal * boostValue[boost]);
        }
        else
        {
            statVal = Mathf.FloorToInt(statVal / boostValue[-boost]);
        }

        return statVal;
    }

    public void ApplyBoost(List<StatBoost> statBoosts)
    {
        foreach (var statBoost in statBoosts)
        {
            var stat = statBoost.stat;
            var boost = statBoost.boost;

            StatBoosts[stat] = Mathf.Clamp(StatBoosts[stat] + boost, -6, 6);
            Debug.Log($"{stat} has been boosted to {StatBoosts[stat]}");

            if (boost >= 0)
            {
                StatusChanges.Enqueue($"{Base.name}'s {stat} rose!");
            }
            else
            {
                StatusChanges.Enqueue($"{Base.name}'s {stat} fell!");
            }
        }
    }

    public void OnBattleOver()
    {
        ResetStatBoosts();
    }

    public int Attack
    {
        get { return GetStat(Stat.Attack); }
    }

    public int Defence
    {
        get { return GetStat(Stat.Defence); }
    }

    public int SpAtk
    {
        get { return GetStat(Stat.SpAttack); }
    }

    public int SpDef
    {
        get { return GetStat(Stat.SpDef); }
    }

    public int Speed
    {
        get { return GetStat(Stat.Speed); }
    }

    public int MaxHP
    {
        get; private set;
    }

    public DamageDetails TakeDamage(Move move, Toyo attacker)
    {
        float critical = 1f;
        if(Random.value * 100f <= 6.25f)
        {
            critical = 2f;
        }

        float type = TypeChart.GetEffectiveness(move.Base.ToyoType, this.Base.Type1) * TypeChart.GetEffectiveness(move.Base.ToyoType, this.Base.Type2);

        var damageDetails = new DamageDetails()
        {
            TypeEffectiveness = type,
            Critical = critical,
            Fainted = false,
        };

        float attack = (move.Base.Category == MoveCategory.Special) ? attacker.SpAtk : attacker.Attack;
        float defence = (move.Base.Category == MoveCategory.Special) ? attacker.SpDef : attacker.Defence;

        float modifiers = Random.Range(0.85f, 1f) * type * critical;
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attack / defence) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        UpdateHP(damage);

        return damageDetails;
    }

    public void OnAfterTurn()
    {
        Status?.OnAfterTurn?.Invoke(this);
    }

    public bool OnBeforeMove()
    {
        if(Status?.OnBeforeMove != null)
        {
            return Status.OnBeforeMove(this);
        }
        return true;
    }

    public void SetStatus(ConditionID conditionId)
    {
        if(Status != null)
        {
            return;
        }

        Status = ConditionsDB.Conditions[conditionId];
        Status?.OnStart?.Invoke(this);
        StatusChanges.Enqueue($"{Base.ToyoName} {Status.StartMessage}");
        Debug.Log($"{Base.ToyoName} {Status.StartMessage}");
        OnStatusChanged?.Invoke();
    }

    public void CureStatus() 
    {
        Status = null;
        OnStatusChanged?.Invoke();
    }

    public void UpdateHP(int damage)
    {
        HP = Mathf.Clamp(HP - damage, 0, MaxHP);
        HpChanged = true;
    }

    public Move GetRandomMove()
    {
        int r = Random.Range(0, Moves.Count);

        return Moves[r];
    }
}

public class DamageDetails
{
    public bool Fainted { get; set; }
    public float Critical { get; set; }
    public float TypeEffectiveness { get; set; }
}
