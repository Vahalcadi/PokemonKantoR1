using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject itemList;
    [SerializeField] ItemUI itemUI;
    [SerializeField] TextMeshProUGUI itemDescription;
    Inventory inventory;

    List<ItemUI> items;

    private void Awake()
    {
        inventory = Inventory.GetInventory();
    }

    private void Start()
    {
        UpdateItemList();
    }

    public void UseItem(int currentItemId, Pokemon pokemon)
    {
        inventory.UseItem(currentItemId, pokemon);
    }

    public void UpdateDescription(string description)
    {
        itemDescription.text = description;
    }

    private void UpdateItemList()
    {
        foreach (Transform child in itemList.transform)
            Destroy(child.gameObject);

        items = new List<ItemUI>();

        foreach (var itemSlot in inventory.Items)
        {
            var slotUI = Instantiate(itemUI, itemList.transform);
            slotUI.SetData(itemSlot);
            slotUI.SetDescriptionBox(itemDescription);
            items.Add(slotUI);
        }
    }
}
