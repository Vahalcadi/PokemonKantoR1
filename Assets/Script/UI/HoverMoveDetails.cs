using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverMoveDetails : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private BattleDialogBox dialogueBox;
    [SerializeField] private BattleUnit playerUnit;
    [SerializeField] private int indexInMoveSelector;
    public void OnPointerEnter(PointerEventData eventData)
    {
        try
        {
            dialogueBox.UpdateMoveSelection(playerUnit.Pokemon.moves[indexInMoveSelector]);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}
