using System.Collections.Generic;
using UnityEngine;

public class Pokemon
{
    public PokemonSO pokemonSO { get; set; }
    public int level { get; set; }

    public int hp;
    public List<Move> moves { get; set; }

    public Pokemon(PokemonSO pokemonSO, int level)
    {
        this.pokemonSO = pokemonSO;
        this.level = level;
        hp = MaxHp;
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
        get { return Mathf.FloorToInt((pokemonSO.maxHp * level) / 100) + 10; }
    }

    public int Attack
    {
        get { return Mathf.FloorToInt((pokemonSO.attack * level) / 100) + 5; }
    }

    public int Defence
    {
        get { return Mathf.FloorToInt((pokemonSO.defence * level) / 100) + 5; }
    }

    public int SpAttack
    {
        get { return Mathf.FloorToInt((pokemonSO.specialAttack * level) / 100) + 5; }
    }

    public int SpDefence
    {
        get { return Mathf.FloorToInt((pokemonSO.specialDefence * level) / 100) + 5; }
    }

    public int Speed
    {
        get { return Mathf.FloorToInt((pokemonSO.speed * level) / 100) + 5; }
    }

    public DamageDetails TakeDamage(Move move, Pokemon attacker)
    {
        float critical = 1;
        if (Random.value * 100 <= 6.25f)
            critical = 1.5f;

        float typeEffectiveness = TypeChart.GetEffectiveness(move.moveSO.type, pokemonSO.type1) * TypeChart.GetEffectiveness(move.moveSO.type, pokemonSO.type2);

        var damageDetails = new DamageDetails()
        {
            TypeEffectiveness = typeEffectiveness,
            Critical = critical,
            Fainted = false
        };
        float modifiers = Random.Range(0.85f, 1f) * typeEffectiveness * critical;
        
        float a = (2 * attacker.level + 10) / 250f;
        
        float d;

        if (move.moveSO.isSpecial)
            d = a * move.moveSO.power * ((float)attacker.SpAttack / SpDefence) + 2;
        else
            d = a * move.moveSO.power * ((float)attacker.Attack / Defence) + 2;

        int damage = Mathf.FloorToInt(d * modifiers);
        Debug.Log(damage);

        hp -= damage;

        if (hp <= 0)
        {
            hp = 0;
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
