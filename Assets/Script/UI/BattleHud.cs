using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    new public TextMeshProUGUI name;
    public TextMeshProUGUI level;
    public Slider hpSlider;

    public void SetData(Pokemon pokemon)
    {
        name.text = pokemon.pokemonSO.name;
        level.text = $"Lvl {pokemon.level}";
        hpSlider.maxValue = pokemon.MaxHp;
        hpSlider.value = pokemon.hp;
    }
}
