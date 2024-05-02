using System;
using System.Collections;
using UnityEngine;

public enum BattleState
{
    Start,
    PlayerAction,
    PlayerMove,
    EnemyMove,
    Busy
}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] private BattleUnit playerUnit;
    [SerializeField] private BattleHud playerHud;
    [SerializeField] private BattleUnit enemyUnit;
    [SerializeField] private BattleHud enemyHud;
    [SerializeField] private BattleDialogBox dialogBox;

    public event Action<bool> OnBattleOver;

    private BattleState state;

    public static BattleSystem instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    public void StartBattle()
    {
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        playerUnit.Setup();
        playerHud.SetData(playerUnit.Pokemon);

        enemyUnit.Setup();
        enemyHud.SetData(enemyUnit.Pokemon);

        dialogBox.SetMoveNames(playerUnit.Pokemon.moves);

        dialogBox.SetDialog($"A wild {enemyUnit.pokemonSo.name} appear");
        yield return new WaitForSeconds(2);

        PlayerAction();
    }

    private void PlayerAction()
    {
        state = BattleState.PlayerAction;
        dialogBox.SetDialog("Choose an action");
        dialogBox.EnableActionSelector(true);
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

    public void ConfirmMove(int indexOfMove)
    {
        if (state != BattleState.PlayerMove)
            return;

        dialogBox.EnableMoveSelector(false);
        dialogBox.EnableDialogueText(true);
        StartCoroutine(PerformPlayerMove(indexOfMove));
    }

    private IEnumerator PerformPlayerMove(int indexOfMove)
    {
        state = BattleState.Busy;

        var move = playerUnit.Pokemon.moves[indexOfMove];
        move.pp--;
        Debug.Log(indexOfMove);
        dialogBox.SetDialog($"{playerUnit.Pokemon.pokemonSO.name} used {move.moveSO.name}");
        yield return new WaitForSeconds(1.5f);

        var damageDetails = enemyUnit.Pokemon.TakeDamage(move, playerUnit.Pokemon);
        yield return enemyHud.UpdateHP();

        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            dialogBox.SetDialog($"{enemyUnit.Pokemon.pokemonSO.name} Fainted");
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

        dialogBox.SetDialog($"{enemyUnit.Pokemon.pokemonSO.name} used {move.moveSO.name}");
        yield return new WaitForSeconds(1.5f);

        var damageDetails = playerUnit.Pokemon.TakeDamage(move, enemyUnit.Pokemon);
        yield return playerHud.UpdateHP();

        yield return ShowDamageDetails(damageDetails);

        if (damageDetails.Fainted)
        {
            dialogBox.SetDialog($"{playerUnit.Pokemon.pokemonSO.name} Fainted");
            yield return new WaitForSeconds(2);

            OnBattleOver(false);
        }
        else
        {
            PlayerAction();
        }
    }
}
