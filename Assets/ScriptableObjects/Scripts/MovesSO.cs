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
    public bool alwaysHits;
    public int pp;
    public int priority;
    public MoveCategory moveCategory;
    public MoveEffects moveEffects;
    public List<SecondaryEffects> secondaryEffects;
    public MoveTarget target;
}

[System.Serializable]
public class MoveEffects
{
    [SerializeField] private List<StatBoost> boosts;
    [SerializeField] private ConditionID status;

    public List<StatBoost> Boosts { get { return boosts; } }
    public ConditionID Status { get { return status; } }
}

[System.Serializable]
public class StatBoost
{
    public Stat stat;
    public int boost;
}

[System.Serializable]
public class SecondaryEffects : MoveEffects
{
    [SerializeField] int chance;
    [SerializeField] MoveTarget target;

    public int Chance { get { return chance; } }
    public MoveTarget Target { get { return target; } }
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
