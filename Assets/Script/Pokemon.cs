using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokemon
{
    public PokemonSO pokemonSO { get; set; }
    public int level { get; set; }

    public int hp;
    public List<Move> moves {  get; set; }

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
}
