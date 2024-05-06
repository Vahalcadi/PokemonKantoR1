using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    new public string name;
    public string description;
    public Sprite icon;

    public virtual bool Use(Pokemon pokemon)
    {
        return false;
    }
}
