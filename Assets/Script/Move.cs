using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public MovesSO moveSO { get; set; }
    public int pp { get; set; }

    public Move(MovesSO move)
    {
        this.moveSO = move;
        pp = move.pp;
    }
}
