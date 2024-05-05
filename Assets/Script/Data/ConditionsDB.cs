using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConditionID
{
    none,
    psn,
    brn,
    slp,
    par,
    frz
}

public class ConditionsDB
{
    public static void Initialise()
    {
        foreach (var kvp in Conditions)
        {
            var conditionId = kvp.Key;
            var condition = kvp.Value;

            condition.Id = conditionId;
        }
    }

    public static Dictionary<ConditionID, Condition> Conditions { get; set; } = new Dictionary<ConditionID, Condition>()
    {
        { 
            ConditionID.psn,
            new Condition
            { 
                Name = "Poison", 
                StartMessage = "has been poisoned", 
                OnAfterTurn = (Pokemon pokemon) => 
                {
                    pokemon.UpdateHp(pokemon.MaxHp / 8);
                    pokemon.StatusChanges.Enqueue($"{pokemon.PokemonSO.name} is hurt due to poison");
                } 
            }         
        },


        {
            ConditionID.brn,
            new Condition
            {
                Name = "Burn",
                StartMessage = "has been burned",
                OnAfterTurn = (Pokemon pokemon) =>
                {
                    pokemon.UpdateHp(pokemon.MaxHp / 16);
                    pokemon.StatusChanges.Enqueue($"{pokemon.PokemonSO.name} is hurt due to burn");
                }
            }
        },


        {
            ConditionID.par,
            new Condition
            {
                Name = "Paralize",
                StartMessage = "has been paralized",
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if(Random.Range(1, 5) == 1)
                    {
                        pokemon.StatusChanges.Enqueue($"{pokemon.PokemonSO.name} is paralized and can't move"); 
                        return false;
                    }

                    return true;
                }
            }
        },


        {
            ConditionID.frz,
            new Condition
            {
                Name = "Freeze",
                StartMessage = "has been frozen",
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if(Random.Range(1, 5) == 1)
                    {
                        pokemon.CureStatus();
                        pokemon.StatusChanges.Enqueue($"{pokemon.PokemonSO.name} is not frozen anymore");
                        return true;
                    }

                    return false;
                }
            }
        },


        {
            ConditionID.slp,
            new Condition
            {
                Name = "Sleep",
                StartMessage = "has fallen asleep",
                OnStart = (Pokemon pokemon) =>
                {
                    pokemon.StatusTime = Random.Range(1, 4);
                    Debug.Log($"Will be asleep for {pokemon.StatusTime} moves");
                },
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if(pokemon.StatusTime <= 0)
                    {
                        pokemon.CureStatus();
                        pokemon.StatusChanges.Enqueue($"{pokemon.PokemonSO.name} woke up");
                        return true;
                    }

                    pokemon.StatusTime--;
                    pokemon.StatusChanges.Enqueue($"{pokemon.PokemonSO.name} is sleeping");
                    return false;
                }
            }
        }
    };
}
