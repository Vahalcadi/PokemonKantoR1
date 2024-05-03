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

        name.text = pokemon.PokemonSO.name;
        level.text = $"Lvl {pokemon.Level}";
        hpSlider.SetupHpBar(pokemon.MaxHp, pokemon.Hp);

    }

    public IEnumerator UpdateHP()
    {
        yield return hpSlider.SetCurrentHP(pokemon.Hp);
    }
}
