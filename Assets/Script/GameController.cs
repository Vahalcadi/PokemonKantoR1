using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    FreeRoam,
    Battle
}

public class GameController : MonoBehaviour
{
    [SerializeField] private Bushes Bushes;
    [SerializeField] private BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;
    GameState state;

    private void Start()
    {
        Bushes.OnEncountered += StartBattle;
        battleSystem.OnBattleOver += EndBattle;
    }

    // Update is called once per frame
    void Update()
    {
        if(state == GameState.FreeRoam)
            InputManager.Instance.OnEnable();
        else
            InputManager.Instance.OnDisable();
    }
    private void StartBattle()
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        var playerParty = PlayerManager.instance.PlayerParty;
        var wildPokemon = FindAnyObjectByType<MapArea>().GetComponent<MapArea>().GetRandomWildPokemon();

        battleSystem.StartBattle(playerParty,wildPokemon);
    }

    private void EndBattle(bool won)
    {
        state = GameState.FreeRoam;
        worldCamera.gameObject.SetActive(true);
        battleSystem.gameObject.SetActive(false);
        PlayerManager.instance.Player.engagedInCombat = false;
    }
}
