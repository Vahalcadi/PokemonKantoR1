using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Pokemon
{
    [SerializeField] private PokemonSO pokemonSO;
    [SerializeField] private int level;

    public PokemonSO PokemonSO { get { return pokemonSO; } }
    public int Level { get { return level; } }

    public int Hp { get; set; }
    public List<Move> moves { get; set; }
    public Move CurrentMove { get; set; }
    public Dictionary<Stat, int> Stats { get; private set; }
    public Dictionary<Stat, int> StatBoosts { get; private set; }
    public Condition Status { get; private set; }
    public int StatusTime { get; set; }

    public Queue<string> StatusChanges { get; private set; } = new Queue<string>();
    public bool HpChanged { get; set; }
    public event Action OnStatusChanged;

    public void InitialisePokemon()
    {
        moves = new List<Move>();

        foreach (var move in pokemonSO.learnableMoves)
        {
            if (move.level <= level)
                moves.Add(new Move(move.moveSO));

            if (moves.Count >= 4)
                break;
        }

        CalculateStats();
        Hp = MaxHp;

        ResetStatBoosts();
    }

    public void SetStatus(ConditionID conditionID)
    {
        if (Status != null)
            return;

        Status = ConditionsDB.Conditions[conditionID];
        Status?.OnStart?.Invoke(this);
        StatusChanges.Enqueue($"{pokemonSO.name} {Status.StartMessage}");
        OnStatusChanged?.Invoke();
    }

    public void CureStatus()
    {
        Status = null;
        OnStatusChanged?.Invoke();
    }

    public void DecreaseHp(int damage)
    {
        Hp = Mathf.Clamp(Hp - damage, 0, MaxHp);
        HpChanged = true;
    }

    public void IncreaseHp(int amount)
    {
        Hp = Mathf.Clamp(Hp + amount, 0, MaxHp);
        HpChanged = true;
    }

    private void ResetStatBoosts()
    {
        StatBoosts = new Dictionary<Stat, int>()
        {
            {Stat.Attack, 0},
            {Stat.Defence, 0},
            {Stat.SpAttack, 0},
            {Stat.SpDefence, 0},
            {Stat.Speed, 0},
            {Stat.Accuracy, 0},
            {Stat.Evasion, 0},
        };
    }

    private void CalculateStats()
    {
        Stats = new Dictionary<Stat, int>
        {
            { Stat.Attack, Mathf.FloorToInt((PokemonSO.attack * Level) / 100) + 5 },
            { Stat.Defence, Mathf.FloorToInt((PokemonSO.defence * Level) / 100) + 5 },
            { Stat.SpAttack, Mathf.FloorToInt((PokemonSO.specialAttack * Level) / 100) + 5 },
            { Stat.SpDefence, Mathf.FloorToInt((PokemonSO.specialDefence * Level) / 100) + 5 },
            { Stat.Speed, Mathf.FloorToInt((PokemonSO.speed * Level) / 100) + 5 }
        };

        MaxHp = Mathf.FloorToInt((PokemonSO.maxHp * Level) / 100) + 10 + Level;
    }

    private int GetStat(Stat stat)
    {
        int statVal = Stats[stat];

        int boost = StatBoosts[stat];
        var boostValues = new float[] { 1, 1.5f, 2, 2.5f, 3, 3.5f, 4 };

        if (boost >= 0)
            statVal = Mathf.FloorToInt(statVal * boostValues[boost]);
        else
            statVal = Mathf.FloorToInt(statVal / boostValues[-boost]);

        return statVal;
    }

    public void ApplyBoosts(List<StatBoost> statBoosts)
    {
        foreach (var statBoost in statBoosts)
        {
            var stat = statBoost.stat;
            var boost = statBoost.boost;

            StatBoosts[stat] = Mathf.Clamp(StatBoosts[stat] + boost, -6, 6);

            if (boost > 0)
                StatusChanges.Enqueue($"{pokemonSO.name}'s {stat} rose!");
            else
                StatusChanges.Enqueue($"{pokemonSO.name}'s {stat} fell!");

            Debug.Log($"{stat} has been boosted to {StatBoosts[stat]}");
        }
    }

    public int MaxHp { get; private set; }

    public int Attack
    {
        get { return GetStat(Stat.Attack); }
    }

    public int Defence
    {
        get { return GetStat(Stat.Defence); }
    }

    public int SpAttack
    {
        get { return GetStat(Stat.SpAttack); }
    }

    public int SpDefence
    {
        get { return GetStat(Stat.SpDefence); }
    }

    public int Speed
    {
        get { return GetStat(Stat.Speed); }
    }

    public DamageDetails TakeDamage(Move move, Pokemon attacker)
    {
        float critical = 1;
        if (UnityEngine.Random.value * 100 <= 6.25f)
            critical = 1.5f;

        float typeEffectiveness = TypeChart.GetEffectiveness(move.moveSO.type, PokemonSO.type1) * TypeChart.GetEffectiveness(move.moveSO.type, PokemonSO.type2);

        var damageDetails = new DamageDetails()
        {
            TypeEffectiveness = typeEffectiveness,
            Critical = critical,
            Fainted = false
        };
        float modifiers = UnityEngine.Random.Range(0.85f, 1f) * typeEffectiveness * critical;

        float a = (2 * attacker.Level + 10) / 250f;

        float d;

        if (move.moveSO.moveCategory == MoveCategory.Special)
            d = a * move.moveSO.power * ((float)attacker.SpAttack / SpDefence) + 2;
        else
            d = a * move.moveSO.power * ((float)attacker.Attack / Defence) + 2;

        int damage = Mathf.FloorToInt(d * modifiers);
        Debug.Log(damage);

        DecreaseHp(damage);

        return damageDetails;
    }

    public bool OnBeforeTurn()
    {
        if (Status?.OnBeforeMove != null)
            return Status.OnBeforeMove(this);

        return true;
    }

    public void OnAfterTurn()
    {
        Status?.OnAfterTurn?.Invoke(this);
    }

    public Move GetRandomMove()
    {
        var movesWithPP = moves.Where(x => x.pp > 0).ToList();

        int r = UnityEngine.Random.Range(0, movesWithPP.Count);
        return movesWithPP[r];
    }

    public void OnBattleOver()
    {
        ResetStatBoosts();
    }
}

public class DamageDetails
{
    public bool Fainted { get; set; }
    public float Critical { get; set; }
    public float TypeEffectiveness { get; set; }
}
