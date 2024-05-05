using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleHud : MonoBehaviour
{
    new public TextMeshProUGUI name;
    public TextMeshProUGUI level;
    public TextMeshProUGUI status;
    public HPBar hpSlider;

    [SerializeField] private Color psnColor;
    [SerializeField] private Color brnColor;
    [SerializeField] private Color slpColor;
    [SerializeField] private Color parColor;
    [SerializeField] private Color frzColor;

    Pokemon pokemon;

    Dictionary<ConditionID, Color> statusColors;

    public void SetData(Pokemon pokemon)
    {
        this.pokemon = pokemon;

        name.text = pokemon.PokemonSO.name;
        level.text = $"Lvl {pokemon.Level}";
        hpSlider.SetupHpBar(pokemon.MaxHp, pokemon.Hp);

        statusColors = new Dictionary<ConditionID, Color>()
        {
            {ConditionID.psn, psnColor},
            {ConditionID.brn, brnColor},
            {ConditionID.slp, slpColor},
            {ConditionID.par, parColor},
            {ConditionID.frz, frzColor}
        };

        SetStatusText();
        pokemon.OnStatusChanged += SetStatusText;
    }

    private void SetStatusText()
    {
        if (pokemon.Status == null)
        {
            status.text = "";
        }
        else
        {
            status.text = $"{pokemon.Status.Id}".ToUpper();
            status.color = statusColors[pokemon.Status.Id];
        }
    }

    public IEnumerator UpdateHP()
    {
        if (pokemon.HpChanged)
        {
            yield return hpSlider.SetCurrentHP(pokemon.Hp);
            pokemon.HpChanged = false;
        }
    }
}
