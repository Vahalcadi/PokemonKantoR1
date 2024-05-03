using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartyMemberUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    new public TextMeshProUGUI name;
    public Image frontSprite;
    public TextMeshProUGUI level;
    public HPBar hpSlider;

    [SerializeField] private Image arrowPointer;

    Pokemon pokemon;

    private void Start()
    {
        arrowPointer.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        arrowPointer.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        arrowPointer.enabled = false;
    }

    public void SetData(Pokemon pokemon)
    {
        this.pokemon = pokemon;
        frontSprite.sprite = pokemon.PokemonSO.frontSprite;
        name.text = pokemon.PokemonSO.name;
        level.text = $"Lvl {pokemon.Level}";
        hpSlider.SetupHpBar(pokemon.MaxHp, pokemon.Hp);

    }

    private void OnDisable()
    {
        arrowPointer.enabled = false;
    }
}
