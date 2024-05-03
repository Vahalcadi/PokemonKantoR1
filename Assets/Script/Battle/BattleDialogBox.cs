using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject actionSelector;
    [SerializeField] private GameObject moveSelector;
    [SerializeField] private GameObject moveDetails;

    //[SerializeField] private List<Button> actionButtons;
    [SerializeField] private List<TextMeshProUGUI> moveButtons;

    [SerializeField] private TextMeshProUGUI ppText;
    [SerializeField] private TextMeshProUGUI typeText;
    public void SetDialog(string dialogue)
    {
        dialogueText.text = dialogue;
    }

    public void EnableDialogueText(bool enabled)
    {
        dialogueText.enabled = enabled;
    }

    public void EnableActionSelector(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }
    public void EnableMoveSelector(bool enabled)
    {
        moveSelector.SetActive(enabled);
        moveDetails.SetActive(enabled);
    }

    public void SetMoveNames(List<Move> moves)
    {
        for (int i = 0; i < moveButtons.Count; i++)
        {
            if (i < moves.Count)
            {
                moveButtons[i].text = moves[i].moveSO.name;
                moveButtons[i].gameObject.SetActive(true);
            }
            else
                moveButtons[i].gameObject.SetActive(false);           
        }
    }

    public void UpdateMoveSelection(Move move)
    {
        ppText.text = $"PP {move.pp}/{move.moveSO.pp}";
        typeText.text = $"{move.moveSO.type}";
    }
}
