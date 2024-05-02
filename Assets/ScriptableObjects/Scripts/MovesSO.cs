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

    public bool isSpecial;
}
