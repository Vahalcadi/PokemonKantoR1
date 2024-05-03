using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] bool isPlayerUnit;

    public Pokemon Pokemon { get; set; }

    public void Setup(Pokemon pokemon)
    {
        Pokemon = pokemon;

        if (isPlayerUnit)
            GetComponent<Image>().sprite = Pokemon.PokemonSO.backSprite;
        else
            GetComponent<Image>().sprite = Pokemon.PokemonSO.frontSprite;
    }

    public Image GetImageComponent()
    {
        return GetComponent<Image>();
    }
}
