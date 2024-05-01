using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    DARK,
    FIGHTING,
    GHOST,
    PSYCHIC,
    FAIRY,
    DRAGON,
    FIRE,
    GRASS,
    STEEL,
    NORMAL,
    ROCK,
    ICE,
    ELECTRIC,
    FLYING,
    POISON,
    WATER,
    BUG
}

[CreateAssetMenu]
public class PokemonSO : ScriptableObject
{
    new public string name;

    public Sprite frontSprite;
    public Sprite backSprite;

    public Type type1;
    public Type type2;

    public int maxHp;
    public int attack;
    public int defence;
    public int specialAttack;
    public int specialDefence;
    public int speed;

    public List<LearnableMove> learnableMoves;

    
}

[System.Serializable]
public class LearnableMove
{
    public MovesSO moveSO;
    public int level;

}
