using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryItem : ItemSO
{
    [Header("HP")]
    [SerializeField] int hpAmount;
    [SerializeField] bool restoreMaxHp;

    [Header("PP")]
    [SerializeField] int ppAmount;
    [SerializeField] bool restoreMaxPp;

    [Header("Status Conditions")]
    [SerializeField] ConditionID status;
    [SerializeField] bool recoverAllStatus;

    [Header("Revive")]
    [SerializeField] bool revive;
    [SerializeField] bool maxRevive;

    public override bool Use(Pokemon pokemon)
    {
        if (hpAmount > 0)
        {
            if(pokemon.Hp == pokemon.MaxHp)
                return false;

            pokemon.IncreaseHp(hpAmount);
        }

        return true;
    }
}
