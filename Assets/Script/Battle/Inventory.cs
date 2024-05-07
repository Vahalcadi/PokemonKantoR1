using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<ItemSlot> items;
    public List<ItemSlot> Items => items;

    public event Action OnUpdated;

    public ItemSO UseItem(int itemIndex, Pokemon selectedPokemon)
    {
        var item = items.Find(item => item.Index == itemIndex).Item;
        bool itemUsed = item.Use(selectedPokemon);

        if (itemUsed)
        {
            RemoveItem(item);
            return item;
        }
        return null;
    }

    public void RemoveItem(ItemSO item)
    {
        var itemSlot = items.First(slot => slot.Item == item);
        itemSlot.Count--;

        if (itemSlot.Count == 0)
            items.Remove(itemSlot);

        OnUpdated?.Invoke();
    }

    public static Inventory GetInventory()
    {
        return FindObjectOfType<TilemapPlayer>().GetComponent<Inventory>();
    }
}

[System.Serializable]
public class ItemSlot
{
    [SerializeField] ItemSO item;
    [SerializeField] int count;
    public ItemSO Item => item;
    public int Count { get { return count; } set { count = value; } }
    public int Index => (int)item.itemID;
}
