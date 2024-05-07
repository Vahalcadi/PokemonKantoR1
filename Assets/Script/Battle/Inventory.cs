using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<ItemSlot> items;
    public List<ItemSlot> Items => items;

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
    public int Count => count;
    public int Index => (int)item.itemID;
}
