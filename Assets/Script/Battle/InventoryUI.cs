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

        inventory.OnUpdated += UpdateItemList;
    }

    private void Start()
    {
        UpdateItemList();
    }

    public bool UseItem(int currentItemId, Pokemon pokemon)
    {
        var item = inventory.UseItem(currentItemId, pokemon);

        if (item != null)
            return true;

        return false;
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
