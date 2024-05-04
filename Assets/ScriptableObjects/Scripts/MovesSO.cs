using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MovesSO : ScriptableObject
{
    new public string name;

    [TextArea] public string description;

    public Type type;
    public int power;
    public int accuracy;
    public int pp;
    public MoveCategory moveCategory;
    public MoveEffects moveEffects;
    public MoveTarget target;
}

[System.Serializable]
public class MoveEffects
{
    [SerializeField] private List<StatBoost> boosts;

    public List<StatBoost> Boosts { get { return boosts; } }
}

[System.Serializable]
public class StatBoost
{
    public Stat stat;
    public int boost;
}

public enum MoveCategory
{
    Physical,
    Special,
    Status
}

public enum MoveTarget
{
    Foe,
    Self
}
