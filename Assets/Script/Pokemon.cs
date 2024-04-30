using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokemon
{
    PokemonSO scriptableObject;
    int level;

    public Pokemon(PokemonSO scriptableObject, int level)
    {
        this.scriptableObject = scriptableObject;
        this.level = level;
    }

    public int MaxHp
    {
        get { return Mathf.FloorToInt((scriptableObject.maxHp * level) / 100) + 10; }
    }

    public int Attack
    {
        get { return Mathf.FloorToInt((scriptableObject.attack * level) / 100) + 5; }
    }

    public int Defence
    {
        get { return Mathf.FloorToInt((scriptableObject.defence * level) / 100) + 5; }
    }

    public int SpAttack
    {
        get { return Mathf.FloorToInt((scriptableObject.specialAttack * level) / 100) + 5; }
    }

    public int SpDefence
    {
        get { return Mathf.FloorToInt((scriptableObject.specialDefence * level) / 100) + 5; }
    }

    public int Speed
    {
        get { return Mathf.FloorToInt((scriptableObject.speed * level) / 100) + 5; }
    }
}
