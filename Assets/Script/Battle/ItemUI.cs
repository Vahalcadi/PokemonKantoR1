using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image arrowPointer;

    private void Start()
    {
        arrowPointer.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        arrowPointer.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        arrowPointer.enabled = false;
    }

    private void OnDisable()
    {
        arrowPointer.enabled = false;
    }
}
