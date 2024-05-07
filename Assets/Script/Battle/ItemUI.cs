using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Image image;
    private string description;
    [SerializeField] private Image arrowPointer;
    private TextMeshProUGUI descriptionBox;

    private void Start()
    {
        arrowPointer.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        arrowPointer.enabled = true;
        descriptionBox.text = description;
    }

    public void SetData(ItemSlot itemSlot)
    {
        nameText.text = itemSlot.Item.name;
        countText.text = $"x {itemSlot.Count}";
        image.sprite = itemSlot.Item.icon;
        description = itemSlot.Item.description;
    }

    public void SetDescriptionBox(TextMeshProUGUI descriptionBox)
    {
        this.descriptionBox = descriptionBox;
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
