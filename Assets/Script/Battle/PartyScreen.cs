using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PartyScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI message;
    PartyMemberUI[] partySlots;

    public void Initialise()
    {
        partySlots = GetComponentsInChildren<PartyMemberUI>();
    }

    public void SetPartyData(List<Pokemon> pokemons)
    {
        for(int i = 0; i < partySlots.Length; i++)
        {
            if(i < pokemons.Count)
                partySlots[i].SetData(pokemons[i]);
            else
                partySlots[i].gameObject.SetActive(false);
        }

        message.text = "Choose a pokemon";
    }

    public void SetMessageText(string message)
    {
        this.message.text = message;
    }


}
