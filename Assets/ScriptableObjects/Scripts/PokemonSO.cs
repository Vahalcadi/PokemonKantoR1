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
    BUG,
    GROUND,
    NONE
}

public enum Stat
{
    Attack,
    Defence,
    SpAttack,
    SpDefence,
    Speed,
    
    Accuracy,
    Evasion
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

public class TypeChart
{
    static float[][] chart =
    {
        //                           DRK,   FHT,   GHT,   PSY,    FAI,    DRG,    FIR,    GRA,    STE,    NOR,  ROCK,  ICE,   ElE,   FLY,   POI,   WAT,   BUG,   GRO 
        /*DARK*/ new float[]        {.5f,   .5f,    2,     2,     .5f,     1,      1,      1,      1,      1,     1,    1,     1,     1,     1,     1,     1,     1},
        /*FIGHTING*/ new float[]     {2,     1,     0,    .5f,    .5f,     1,      1,      1,      2,      2,     2,    2,     1,    .5f,   .5f,    1,    .5f,    1},
        /*GHOST*/ new float[]       {.5f,    1,     2,     2,      1,      1,      1,      1,      1,      0,     1,    1,     1,     1,     1,     1,     1,     1},
        /*PSYCHIC*/ new float[]      {0,     2,     1,     1,      1,      1,      1,      1,     .5f,     1,     1,    1,     1,     1,     2,     1,    .5f,    1},
        /*FAIRY*/ new float[]        {2,     2,     1,     1,      1,      2,     .5f,     1,     .5f,     1,     1,    1,     1,     1,    .5f,    1,     1,     1},
        /*DRAGON*/ new float[]       {1,     1,     1,     1,      0,      2,      1,      1,     .5f,     1,     1,    1,     1,     1,     1,     1,     1,     1},
        /*FIRE*/ new float[]         {1,     1,     1,     1,      1,     .5f,    .5f,     2,      2,      1,    .5f,   2,     1,     1,     1,    .5f,    2,     1},
        /*GRASS*/ new float[]        {1,     1,     1,     1,      1,     .5f,    .5f,    .5f,    .5f,     1,     2,    1,     1,    .5f,   .5f,    2,    .5f,    2},
        /*STEEL*/ new float[]        {1,     1,     1,     1,      2,      1,     .5f,     1,     .5f,     1,     2,    2,    .5f,    1,     1,    .5f,    1,     1},
        /*NORMAL*/ new float[]       {1,     1,     0,     1,      1,      1,      1,      1,     .5f,     1,    .5f,   1,     1,     1,     1,     1,     1,     1},
        /*ROCK*/ new float[]         {1,     1,     1,     1,      1,      1,      2,      1,     .5f,     1,     1,    2,     1,     2,    .5f,    1,     2,    .5f},
        /*ICE*/ new float[]          {1,     1,     1,     1,      1,      2,     .5f,     2,     .5f,     1,     1,   .5f,    1,     2,     1,    .5f,    1,     2},
        /*ELECTRIC*/ new float[]     {1,     1,     1,     1,      1,     .5f,     1,     .5f,     1,      1,     1,    1,    .5f,    2,     1,     2,     1,     0},
        /*FLYING*/ new float[]       {1,     2,    .5f,    1,      1,      1,      1,      2,     .5f,     1,     1,    1,    .5f,    1,     1,     1,     2,     1},
        /*POISON*/ new float[]       {1,     1,    .5f,    1,      2,      1,      1,      2,      0,      1,    .5f,   1,     1,     1,    .5f,    1,     1,    .5f},
        /*WATER*/ new float[]        {1,     1,     1,     1,      1,     .5f,     2,     .5f,     1,      1,     2,    1,     1,     1,     1,    .5f,    1,     2},
        /*BUG*/ new float[]          {2,    .5f,   .5f,    2,     .5f,     1,     .5f,     2,     .5f,     1,     1,    1,     1,    .5f,   .5f,    1,     1,     1},
        /*GROUND*/ new float[]       {1,     1,     1,     1,      1,      1,      2,     .5f,     2,      1,     2,    1,     2,     0,     2,     1,    .5f,    1},
    };

    public static float GetEffectiveness(Type attackType, Type defenceType)
    {
        if (attackType == Type.NONE || defenceType == Type.NONE)
            return 1;

        int row = (int)attackType;
        int col = (int)defenceType;

        return chart[row][col];
    }
}
