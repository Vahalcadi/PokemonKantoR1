using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    public PokemonSO pokemonSo;
    public int level;
    [SerializeField]bool isPlayerUnit;

    public Pokemon Pokemon { get; set; }

    public void Setup()
    {
        Pokemon = new Pokemon(pokemonSo, level);

        if (isPlayerUnit)
            GetComponent<Image>().sprite = Pokemon.pokemonSO.backSprite;
        else
            GetComponent<Image>().sprite = Pokemon.pokemonSO.frontSprite;
    }
}
