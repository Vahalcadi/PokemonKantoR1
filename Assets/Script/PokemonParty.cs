using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PokemonParty : MonoBehaviour
{
    [SerializeField] List<Pokemon> pokemons;

    public List<Pokemon> Pokemons { get { return pokemons; } }

    private void Start()
    {
        foreach (var pokemon in pokemons)
        {
            pokemon.InitialisePokemon();
        }
    }

    public Pokemon GetAlivePokemon()
    {
        return pokemons.Where(x => x.Hp > 0).FirstOrDefault();
    }
}
