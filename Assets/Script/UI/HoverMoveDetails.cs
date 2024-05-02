using UnityEngine;
using UnityEngine.EventSystems;

public class HoverMoveDetails : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private BattleDialogBox dialogueBox;
    [SerializeField] private BattleUnit playerUnit;
    [SerializeField] private int indexInMoveSelector;

    public void OnPointerClick(PointerEventData eventData)
    {
        BattleSystem.instance.currentMove = indexInMoveSelector;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        dialogueBox.UpdateMoveSelection(playerUnit.Pokemon.moves[indexInMoveSelector]);
    }
}
