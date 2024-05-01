using System.Collections;
using System.Collections.Generic;
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

    private BattleState state;
    int currentAction;

    private void Start()
    {
        StartCoroutine(SetupBattle());
    }

    private void Update()
    {
        /*if (state == BattleState.PlayerAction)
        {
            //HandleActionSelection();
        }*/
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
        PlayerMove();
    }
}
