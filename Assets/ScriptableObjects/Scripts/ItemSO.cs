using UnityEngine;

public enum ItemID
{
    Potion,
    MaxPotion,
    HyperPotion,
    Ether,
    MaxEther,
    Antidote,
    Revive,
    MaxRevive,
    MoomooMilk,
    SitrusBerry,
    Awakening
}

public class ItemSO : ScriptableObject
{
    public ItemID itemID;
    new public string name;
    [TextArea]
    public string description;
    public Sprite icon;

    public virtual bool Use(Pokemon pokemon)
    {
        return false;
    }
}
