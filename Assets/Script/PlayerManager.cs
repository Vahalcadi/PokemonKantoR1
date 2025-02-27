using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private TilemapPlayer player;
    [SerializeField] private PokemonParty playerParty;
    public static PlayerManager instance;

    private void Awake()
    {
        if(instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    public TilemapPlayer Player { get { return player; } }
    public PokemonParty PlayerParty { get { return playerParty; } }
}
