using System.Collections;
using TMPro;
using UnityEngine;

public class BattleHud : MonoBehaviour
{
    new public TextMeshProUGUI name;
    public TextMeshProUGUI level;
    public HPBar hpSlider;

    Pokemon pokemon;

    public void SetData(Pokemon pokemon)
    {
        this.pokemon = pokemon;

        name.text = pokemon.pokemonSO.name;
        level.text = $"Lvl {pokemon.level}";
        hpSlider.SetupHpBar(pokemon.MaxHp, pokemon.hp);

    }

    public IEnumerator UpdateHP()
    {
        yield return hpSlider.SetCurrentHP(pokemon.hp);
    }
}
