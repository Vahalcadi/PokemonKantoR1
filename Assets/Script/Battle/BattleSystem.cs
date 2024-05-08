using System;
using System.Collections;
using UnityEngine;

public enum BattleState
{
    Start,
    ActionSelection,
    MoveSelection,
    RunningTurn,
    Busy,
    PartyScreen,
    Bag,
    BattleOver
}

public enum BattleAction
{
    Move,
    SwitchPokemon,
    UseItem,
    Run
}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] private BattleUnit playerUnit;
    [SerializeField] private BattleUnit enemyUnit;
    [SerializeField] private BattleDialogBox dialogBox;
    [SerializeField] private PartyScreen partyScreen;
    [SerializeField] private InventoryUI inventoryUI;

    private int escapeAttempts;

    public event Action<bool> OnBattleOver;

    private BattleState state;
    private BattleState? prevState;

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

        enemyUnit.Setup(wildPokemon);

        partyScreen.Initialise();

        dialogBox.SetMoveNames(playerUnit.Pokemon.moves);

        dialogBox.SetDialog($"A wild {enemyUnit.Pokemon.PokemonSO.name} appear");
        yield return new WaitForSeconds(2);

        escapeAttempts = 0;

        ActionSelection();
    }

    #region player actions
    private void ActionSelection()
    {
        state = BattleState.ActionSelection;
        dialogBox.SetDialog("Choose an action");
        dialogBox.EnableActionSelector(true);
    }

    private void OpenPartyScreen()
    {
        prevState = state;
        state = BattleState.PartyScreen;
        partyScreen.SetPartyData(playerParty.Pokemons);
        partyScreen.gameObject.SetActive(true);
    }

    private void OpenBagScreen()
    {
        state = BattleState.Bag;
        inventoryUI.gameObject.SetActive(true);
    }

    public void FightSelected()
    {
        if (state == BattleState.ActionSelection)
            MoveSelection();
    }

    public void BagSelected()
    {
        if (state == BattleState.ActionSelection)
            OpenBagScreen();
    }
    public void PokemonSelected()
    {
        if (state == BattleState.ActionSelection)
            OpenPartyScreen();

    }
    public void RunSelected()
    {
        if (state == BattleState.ActionSelection)
        {
            dialogBox.EnableActionSelector(false);
            dialogBox.EnableDialogueText(true);
            dialogBox.EnableMoveSelector(false);
            StartCoroutine(RunTurns(BattleAction.Run, -1, -1, -1));
        }
    }
    public void GoBackToActionSelection()
    {
        inventoryUI.gameObject.SetActive(false);
        partyScreen.gameObject.SetActive(false);
        dialogBox.EnableMoveSelector(false);
        dialogBox.EnableDialogueText(true);
        prevState = null;

        ActionSelection();
    }

    #endregion

    public void ConfirmItem(int itemId)
    {
        if (state != BattleState.Bag)
            return;

        dialogBox.EnableActionSelector(false);
        dialogBox.EnableMoveSelector(false);
        dialogBox.EnableDialogueText(true);
        StartCoroutine(RunTurns(BattleAction.UseItem, -1, -1, itemId));
    }

    #region player moves
    private void MoveSelection()
    {
        state = BattleState.MoveSelection;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogueText(false);
        dialogBox.EnableMoveSelector(true);
    }
    public void ConfirmMove(int indexOfMove)
    {
        if (state != BattleState.MoveSelection)
            return;

        if (playerUnit.Pokemon.moves[indexOfMove].pp <= 0)
            return;

        dialogBox.EnableMoveSelector(false);
        dialogBox.EnableDialogueText(true);
        StartCoroutine(RunTurns(BattleAction.Move, indexOfMove, -1, -1));
    }

    #endregion

    #region switch pokemons
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

        if (prevState == BattleState.ActionSelection)
            StartCoroutine(RunTurns(BattleAction.SwitchPokemon, -1, indexOfPokemon, -1));
        else
        {
            state = BattleState.Busy;
            StartCoroutine(SwitchPokemon(selectedMember));
        }

        prevState = null;
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

        dialogBox.SetMoveNames(newPokemon.moves);

        dialogBox.SetDialog($"Go {newPokemon.PokemonSO.name}");
        yield return new WaitForSeconds(1f);
        playerUnit.GetImageComponent().enabled = true;

        yield return new WaitForSeconds(1);

        state = BattleState.RunningTurn;
    }
    #endregion


    public IEnumerator RunTurns(BattleAction playerAction, int indexOfMove, int indexOfPokemon, int idOfItem)
    {
        state = BattleState.RunningTurn;

        if (playerAction == BattleAction.Move)
        {
            playerUnit.Pokemon.CurrentMove = playerUnit.Pokemon.moves[indexOfMove];
            enemyUnit.Pokemon.CurrentMove = enemyUnit.Pokemon.GetRandomMove();

            int playerMovePriority = playerUnit.Pokemon.CurrentMove.moveSO.priority;
            int enemyMovePriority = enemyUnit.Pokemon.CurrentMove.moveSO.priority;

            bool playerGoesFirst = true;



            if (enemyMovePriority > playerMovePriority)
                playerGoesFirst = false;

            else if (enemyMovePriority == playerMovePriority)
            {
                int playerPokemonSpeed = playerUnit.Pokemon.Speed;
                int enemyPokemonSpeed = enemyUnit.Pokemon.Speed;

                if (playerUnit.Pokemon.Status == ConditionsDB.Conditions[ConditionID.par])
                    playerPokemonSpeed = Mathf.FloorToInt(playerPokemonSpeed / 2);

                if (playerUnit.Pokemon.Status == ConditionsDB.Conditions[ConditionID.par])
                    enemyPokemonSpeed = Mathf.FloorToInt(enemyPokemonSpeed / 2);

                if (playerPokemonSpeed == enemyPokemonSpeed)
                    playerGoesFirst = UnityEngine.Random.Range(1, 3) == 1;
                else
                    playerGoesFirst = playerPokemonSpeed > enemyPokemonSpeed;
            }

            var firstUnit = (playerGoesFirst) ? playerUnit : enemyUnit;
            var secondUnit = (playerGoesFirst) ? enemyUnit : playerUnit;

            var secondPokemon = secondUnit.Pokemon;

            //first turn
            yield return RunMove(firstUnit, secondUnit, firstUnit.Pokemon.CurrentMove);
            yield return RunAfterTurn(firstUnit);
            if (state == BattleState.BattleOver)
                yield break;

            if (secondPokemon.Hp > 0)
            {
                //second turn
                yield return RunMove(secondUnit, firstUnit, secondUnit.Pokemon.CurrentMove);
                yield return RunAfterTurn(secondUnit);
            }

        }
        else
        {
            if (playerAction == BattleAction.SwitchPokemon)
            {
                var selectedPokemon = playerParty.Pokemons[indexOfPokemon];
                state = BattleState.Busy;
                yield return SwitchPokemon(selectedPokemon);
            }
            else if (playerAction == BattleAction.Run)
            {
                yield return TryToRun();
            }
            else if (playerAction == BattleAction.UseItem)
            {
                state = BattleState.Busy;
                bool itemUsed = inventoryUI.UseItem(idOfItem, playerUnit.Pokemon);
                inventoryUI.gameObject.SetActive(false);

                if (itemUsed)
                {
                    dialogBox.SetDialog($"Player successfully used an item on {playerUnit.Pokemon}");
                    yield return new WaitForSeconds(1.5f);
                    state = BattleState.RunningTurn;
                    yield return playerUnit.Hud.UpdateHP();
                }
                else
                {
                    dialogBox.SetDialog($"Player cannot use the item right now");
                    yield return new WaitForSeconds(1.5f);

                    ActionSelection();
                    yield break;
                }
            }
            var enemyMove = enemyUnit.Pokemon.GetRandomMove();
            yield return RunMove(enemyUnit, playerUnit, enemyMove);
            yield return RunAfterTurn(enemyUnit);
            if (state == BattleState.BattleOver)
                yield break;
        }

        if (state != BattleState.BattleOver)
            ActionSelection();
    }

    private bool CheckIfMoveHits(Move move, Pokemon source, Pokemon target)
    {
        if (move.moveSO.alwaysHits)
            return true;

        float moveAccuracy = move.moveSO.accuracy;

        int accuracy = source.StatBoosts[Stat.Accuracy];
        int evasion = target.StatBoosts[Stat.Evasion];

        var boostValues = new float[] { 1, 4 / 3, 5 / 3, 2, 7 / 3, 8 / 3, 3 };

        if (accuracy > 0)
            moveAccuracy *= boostValues[accuracy];
        else
            moveAccuracy /= boostValues[-accuracy];

        if (evasion > 0)
            moveAccuracy /= boostValues[evasion];
        else
            moveAccuracy *= boostValues[-evasion];

        return UnityEngine.Random.Range(1, 101) <= moveAccuracy;
    }


    private IEnumerator RunMove(BattleUnit sourceUnit, BattleUnit targetUnit, Move move)
    {
        bool canRunMove = sourceUnit.Pokemon.OnBeforeTurn();
        if (!canRunMove)
        {
            yield return ShowStatusChanges(sourceUnit.Pokemon);
            yield break;
        }
        yield return ShowStatusChanges(sourceUnit.Pokemon);

        move.pp--;
        dialogBox.SetDialog($"{sourceUnit.Pokemon.PokemonSO.name} used {move.moveSO.name}");
        yield return new WaitForSeconds(1.5f);

        if (CheckIfMoveHits(move, sourceUnit.Pokemon, targetUnit.Pokemon))
        {
            if (move.moveSO.moveCategory == MoveCategory.Status)
            {
                yield return RunMoveEffects(move.moveSO.moveEffects, sourceUnit.Pokemon, targetUnit.Pokemon, move.moveSO.target);
            }
            else
            {
                var damageDetails = targetUnit.Pokemon.TakeDamage(move, sourceUnit.Pokemon);
                yield return targetUnit.Hud.UpdateHP();
                yield return ShowDamageDetails(damageDetails);
            }

            if (move.moveSO.secondaryEffects != null && move.moveSO.secondaryEffects.Count > 0 && targetUnit.Pokemon.Hp > 0)
            {
                foreach (var effect in move.moveSO.secondaryEffects)
                {
                    var random = UnityEngine.Random.Range(1, 101);
                    if (random <= effect.Chance)
                    {
                        yield return RunMoveEffects(effect, sourceUnit.Pokemon, targetUnit.Pokemon, effect.Target);
                    }
                }
            }

            if (targetUnit.Pokemon.Hp <= 0)
            {
                dialogBox.SetDialog($"{targetUnit.Pokemon.PokemonSO.name} Fainted");
                yield return new WaitForSeconds(2);

                CheckForBattleOver(targetUnit);
            }


        }
        else
        {
            dialogBox.SetDialog($"{sourceUnit.Pokemon.PokemonSO.name}'s attack missed");
            yield return new WaitForSeconds(1.5f);
        }
    }

    IEnumerator RunAfterTurn(BattleUnit sourceUnit)
    {
        if (state == BattleState.BattleOver)
            yield break;

        yield return new WaitUntil(() => state == BattleState.RunningTurn);

        sourceUnit.Pokemon.OnAfterTurn();
        yield return ShowStatusChanges(sourceUnit.Pokemon);
        yield return sourceUnit.Hud.UpdateHP();
        if (sourceUnit.Pokemon.Hp <= 0)
        {
            dialogBox.SetDialog($"{sourceUnit.Pokemon.PokemonSO.name} Fainted");
            yield return new WaitForSeconds(2);

            CheckForBattleOver(sourceUnit);
        }
    }

    private IEnumerator RunMoveEffects(MoveEffects effects, Pokemon source, Pokemon target, MoveTarget moveTarget)
    {
        if (effects.Boosts != null)
        {
            if (moveTarget == MoveTarget.Self)
                source.ApplyBoosts(effects.Boosts);
            else
                target.ApplyBoosts(effects.Boosts);
        }

        if (effects.Status != ConditionID.none)
        {
            target.SetStatus(effects.Status);
        }

        yield return ShowStatusChanges(source);
        yield return ShowStatusChanges(target);
    }

    IEnumerator ShowStatusChanges(Pokemon pokemon)
    {
        while (pokemon.StatusChanges.Count > 0)
        {
            var message = pokemon.StatusChanges.Dequeue();
            dialogBox.SetDialog(message);
            yield return new WaitForSeconds(1.5f);
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


    private void BattleOver(bool won)
    {
        state = BattleState.BattleOver;
        playerParty.Pokemons.ForEach(p => p.OnBattleOver());
        enemyUnit.Pokemon.OnBattleOver();
        enemyUnit.Pokemon.CureStatus();
        OnBattleOver(won);
    }

    private void CheckForBattleOver(BattleUnit faintedUnit)
    {
        if (faintedUnit.IsPlayerUnit)
        {
            var nextPokemon = playerParty.GetAlivePokemon();
            if (nextPokemon != null)
            {
                OpenPartyScreen();
            }
            else
                BattleOver(false);
        }
        else
            BattleOver(true);
    }

    private IEnumerator TryToRun()
    {
        state = BattleState.Busy;

        escapeAttempts++;

        int playerSpeed = playerUnit.Pokemon.Speed;
        int enemySpeed = enemyUnit.Pokemon.Speed;

        if (enemySpeed < playerSpeed)
        {
            dialogBox.SetDialog($"Player ran away");
            yield return new WaitForSeconds(1.5f);
            BattleOver(true);
        }
        else
        {
            float f = (playerSpeed * 128) / enemySpeed + 30 * escapeAttempts;
            f = f % 256;

            if (UnityEngine.Random.Range(0, 256) < f)
            {
                dialogBox.SetDialog($"Player ran away");
                yield return new WaitForSeconds(1.5f);
                BattleOver(true);
            }
            else
            {
                dialogBox.SetDialog($"Player failed to escape!");
                yield return new WaitForSeconds(1.5f);
                state = BattleState.RunningTurn;
            }
        }
    }
}
