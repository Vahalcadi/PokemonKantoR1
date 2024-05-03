using System;
using System.Collections;
using UnityEngine;

public enum BattleState
{
    Start,
    PlayerAction,
    PlayerMove,
    EnemyMove,
    Busy,
    PartyScreen
}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] private BattleUnit playerUnit;
    [SerializeField] private BattleHud playerHud;
    [SerializeField] private BattleUnit enemyUnit;
    [SerializeField] private BattleHud enemyHud;
    [SerializeField] private BattleDialogBox dialogBox;
    [SerializeField] private PartyScreen partyScreen;

    public event Action<bool> OnBattleOver;

    private BattleState state;

    public static BattleSystem instance;

    private PokemonParty playerParty;
    private Pokemon wildPokemon;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    public void StartBattle(PokemonParty playerParty, Pokemon wildPokemon)
    {
        this.playerParty = playerParty;
        this.wildPokemon = wildPokemon;
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        playerUnit.Setup(playerParty.GetAlivePokemon());
        playerHud.SetData(playerUnit.Pokemon);

        enemyUnit.Setup(wildPokemon);
        enemyHud.SetData(enemyUnit.Pokemon);

        partyScreen.Initialise();

        dialogBox.SetMoveNames(playerUnit.Pokemon.moves);

        dialogBox.SetDialog($"A wild {enemyUnit.Pokemon.PokemonSO.name} appear");
        yield return new WaitForSeconds(2);

        PlayerAction();
    }

    private void PlayerAction()
    {
        state = BattleState.PlayerAction;
        dialogBox.SetDialog("Choose an action");
        dialogBox.EnableActionSelector(true);
    }

    private void OpenPartyScreen()
    {
        state = BattleState.PartyScreen;
        partyScreen.SetPartyData(playerParty.Pokemons);
        partyScreen.gameObject.SetActive(true);
    }

    private void PlayerMove()
    {
        state = BattleState.PlayerMove;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogueText(false);
        dialogBox.EnableMoveSelector(true);
    }

    public void FightSelected()
    {
        if (state == BattleState.PlayerAction)
            PlayerMove();
    }
    public void BagSelected()
    {
        if (state == BattleState.PlayerAction)
            Debug.Log("accessing bag");
    }
    public void PokemonSelected()
    {
        if (state == BattleState.PlayerAction)
            OpenPartyScreen();
    }
    public void RunSelected()
    {
        if (state == BattleState.PlayerAction)
            Debug.Log("run");
    }
    public void GoBackToActionSelection()
    {
        partyScreen.gameObject.SetActive(false);
        dialogBox.EnableMoveSelector(false);
        dialogBox.EnableDialogueText(true);
        PlayerAction();
    }

    public void ConfirmMove(int indexOfMove)
    {
        if (state != BattleState.PlayerMove)
            return;

        dialogBox.EnableMoveSelector(false);
        dialogBox.EnableDialogueText(true);
        StartCoroutine(PerformPlayerMove(indexOfMove));
    }

    public void ConfirmPokemon(int indexOfPokemon)
    {
        if (state != BattleState.PartyScreen)
            return;

        var selectedMember = playerParty.Pokemons[indexOfPokemon];
        if (selectedMember.Hp <= 0)
        {
            partyScreen.SetMessageText("Pokemon is fainted");
            return;
        }
        if (selectedMember == playerUnit.Pokemon)
        {
            partyScreen.SetMessageText("Pokemon is already in combat");
            return;
        }

        partyScreen.gameObject.SetActive(false);
        state = BattleState.Busy;
        StartCoroutine(SwitchPokemon(selectedMember));
    }

    private IEnumerator SwitchPokemon(Pokemon newPokemon)
    {
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableMoveSelector(false);

        if (playerUnit.Pokemon.Hp > 0)
        {
            dialogBox.SetDialog($"Come back {playerUnit.Pokemon.PokemonSO.name}");
            yield return new WaitForSeconds(1f);
            playerUnit.GetImageComponent().enabled = false;
        }      

        playerUnit.Setup(newPokemon);
        playerHud.SetData(newPokemon);

        dialogBox.SetMoveNames(newPokemon.moves);

        dialogBox.SetDialog($"Go {newPokemon.PokemonSO.name}");
        yield return new WaitForSeconds(1f);
        playerUnit.GetImageComponent().enabled = true;

        yield return new WaitForSeconds(1);

        StartCoroutine(EnemyMove());
    }

    private IEnumerator PerformPlayerMove(int indexOfMove)
    {
        state = BattleState.Busy;

        var move = playerUnit.Pokemon.moves[indexOfMove];
        move.pp--;
        Debug.Log(indexOfMove);
        dialogBox.SetDialog($"{playerUnit.Pokemon.PokemonSO.name} used {move.moveSO.name}");
        yield return new WaitForSeconds(1.5f);

        var damageDetails = enemyUnit.Pokemon.TakeDamage(move, playerUnit.Pokemon);
        yield return enemyHud.UpdateHP();

        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            dialogBox.SetDialog($"{enemyUnit.Pokemon.PokemonSO.name} Fainted");
            yield return new WaitForSeconds(2);

            OnBattleOver(true);
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }

    private IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if (damageDetails.Critical > 1)
        {
            dialogBox.SetDialog("A critical Hit");
            Debug.Log("Critical");
            yield return new WaitForSeconds(1.5f);
        }

        if (damageDetails.TypeEffectiveness > 1)
        {
            dialogBox.SetDialog("It's super effective!");
            Debug.Log("Super effective");
            yield return new WaitForSeconds(1.5f);
        }
        else if (damageDetails.TypeEffectiveness < 1)
        {
            dialogBox.SetDialog("It's not very effective...");
            Debug.Log("Not Effective");
            yield return new WaitForSeconds(1.5f);
        }
            
    }

    private IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;
        var move = enemyUnit.Pokemon.GetRandomMove();
        move.pp--;

        dialogBox.SetDialog($"{enemyUnit.Pokemon.PokemonSO.name} used {move.moveSO.name}");
        yield return new WaitForSeconds(1.5f);

        var damageDetails = playerUnit.Pokemon.TakeDamage(move, enemyUnit.Pokemon);
        yield return playerHud.UpdateHP();

        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            dialogBox.SetDialog($"{playerUnit.Pokemon.PokemonSO.name} Fainted");
            yield return new WaitForSeconds(1);
            playerUnit.GetImageComponent().enabled = false;
            yield return new WaitForSeconds(1);

            var nextPokemon = playerParty.GetAlivePokemon();
            if (nextPokemon != null)
            {
                OpenPartyScreen();
            }
            else
                OnBattleOver(false);
        }
        else
        {
            PlayerAction();
        }
    }
}
