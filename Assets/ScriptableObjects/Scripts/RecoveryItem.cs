using UnityEngine;

[CreateAssetMenu]
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
        if (revive || maxRevive)
        {
            if (pokemon.Hp > 0)
                return false;

            if(revive)
                pokemon.IncreaseHp(pokemon.MaxHp / 2);
            else
                pokemon.IncreaseHp(pokemon.MaxHp);

            pokemon.CureStatus();

            return true;
        }

        if (pokemon.Hp <= 0)
            return false;

        if (restoreMaxHp || hpAmount > 0)
        {
            if (pokemon.Hp == pokemon.MaxHp)
                return false;

            if (restoreMaxHp)
                pokemon.IncreaseHp(pokemon.MaxHp);
            else
                pokemon.IncreaseHp(hpAmount);
        }

        if (recoverAllStatus || status != ConditionID.none)
        {
            if(pokemon.Status == null)
                return false;

            if (recoverAllStatus)        
                pokemon.CureStatus();
            else
            {
                if (pokemon.Status.Id == status)
                    pokemon.CureStatus();
                else
                    return false;
            }
        }

        if (restoreMaxPp || ppAmount > 0)
        {

            if (restoreMaxPp)
                pokemon.moves.ForEach(m => m.IncreasePP(m.moveSO.pp));
            else
                pokemon.moves.ForEach(m => m.IncreasePP(ppAmount));
        }
        

        return true;
    }
}
