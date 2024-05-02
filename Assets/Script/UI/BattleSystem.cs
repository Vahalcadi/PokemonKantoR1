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

    private BattleState state;
    public int currentMove;

    public static BattleSystem instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

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
        if (state == BattleState.PlayerAction)
            PlayerMove();
    }

    public void ConfirmMove()
    {
        if (state != BattleState.PlayerMove)
            return;

        dialogBox.EnableMoveSelector(false);
        dialogBox.EnableDialogueText(true);
        StartCoroutine(PerformPlayerMove());
    }

    private IEnumerator PerformPlayerMove()
    {
        state = BattleState.Busy;

        var move = playerUnit.Pokemon.moves[currentMove];
        dialogBox.SetDialog($"{playerUnit.Pokemon.pokemonSO.name} used {move.moveSO.name}");
        yield return new WaitForSeconds(1.5f);

        bool isFainted = enemyUnit.Pokemon.TakeDamage(move, playerUnit.Pokemon);
        yield return enemyHud.UpdateHP();

        if (isFainted)
        {
            dialogBox.SetDialog($"{enemyUnit.Pokemon.pokemonSO.name} Fainted");
            yield return new WaitForSeconds(2);
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }

    private IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;
        var move = enemyUnit.Pokemon.GetRandomMove();

        dialogBox.SetDialog($"{enemyUnit.Pokemon.pokemonSO.name} used {move.moveSO.name}");
        yield return new WaitForSeconds(1.5f);

        bool isFainted = playerUnit.Pokemon.TakeDamage(move, enemyUnit.Pokemon);
        yield return playerHud.UpdateHP();

        if (isFainted)
        {
            dialogBox.SetDialog($"{playerUnit.Pokemon.pokemonSO.name} Fainted");
            yield return new WaitForSeconds(2);
        }
        else
        {
            PlayerAction();
        }
    }
}
