using System.Collections.Generic;
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

    public void InitialisePokemon()
    {   
        Hp = MaxHp;
        moves = new List<Move>();

        foreach (var move in pokemonSO.learnableMoves)
        {
            if (move.level <= level)
                moves.Add(new Move(move.moveSO));

            if (moves.Count >= 4)
                break;
        }
    }

    public int MaxHp
    {
        get { return Mathf.FloorToInt((PokemonSO.maxHp * Level) / 100) + 10; }
    }

    public int Attack
    {
        get { return Mathf.FloorToInt((PokemonSO.attack * Level) / 100) + 5; }
    }

    public int Defence
    {
        get { return Mathf.FloorToInt((PokemonSO.defence * Level) / 100) + 5; }
    }

    public int SpAttack
    {
        get { return Mathf.FloorToInt((PokemonSO.specialAttack * Level) / 100) + 5; }
    }

    public int SpDefence
    {
        get { return Mathf.FloorToInt((PokemonSO.specialDefence * Level) / 100) + 5; }
    }

    public int Speed
    {
        get { return Mathf.FloorToInt((PokemonSO.speed * Level) / 100) + 5; }
    }

    public DamageDetails TakeDamage(Move move, Pokemon attacker)
    {
        float critical = 1;
        if (Random.value * 100 <= 6.25f)
            critical = 1.5f;

        float typeEffectiveness = TypeChart.GetEffectiveness(move.moveSO.type, PokemonSO.type1) * TypeChart.GetEffectiveness(move.moveSO.type, PokemonSO.type2);

        var damageDetails = new DamageDetails()
        {
            TypeEffectiveness = typeEffectiveness,
            Critical = critical,
            Fainted = false
        };
        float modifiers = Random.Range(0.85f, 1f) * typeEffectiveness * critical;
        
        float a = (2 * attacker.Level + 10) / 250f;
        
        float d;

        if (move.moveSO.isSpecial)
            d = a * move.moveSO.power * ((float)attacker.SpAttack / SpDefence) + 2;
        else
            d = a * move.moveSO.power * ((float)attacker.Attack / Defence) + 2;

        int damage = Mathf.FloorToInt(d * modifiers);
        Debug.Log(damage);

        Hp -= damage;

        if (Hp <= 0)
        {
            Hp = 0;
            damageDetails.Fainted = true;
        }
        return damageDetails;
    }

    public Move GetRandomMove()
    {
        int r = Random.Range(0, moves.Count);
        return moves[r];
    }
}

public class DamageDetails
{
    public bool Fainted { get; set; }
    public float Critical { get; set; }
    public float TypeEffectiveness { get; set; }
}
