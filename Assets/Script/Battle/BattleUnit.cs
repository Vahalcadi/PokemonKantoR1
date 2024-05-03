using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] bool isPlayerUnit;
    [SerializeField] BattleHud hud;

    public BattleHud Hud { get { return hud; } }
    public bool IsPlayerUnit { get { return isPlayerUnit; } }
    public Pokemon Pokemon { get; set; }

    public void Setup(Pokemon pokemon)
    {
        Pokemon = pokemon;

        if (isPlayerUnit)
            GetComponent<Image>().sprite = Pokemon.PokemonSO.backSprite;
        else
            GetComponent<Image>().sprite = Pokemon.PokemonSO.frontSprite;

        hud.SetData(pokemon);
    }

    public Image GetImageComponent()
    {
        return GetComponent<Image>();
    }
}
