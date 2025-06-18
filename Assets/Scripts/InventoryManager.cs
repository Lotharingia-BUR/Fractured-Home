using System.Collections.Generic;
using UnityEngine;

// Inventory manager class
// Stores and manages all of the items in the player's inventory
public class InventoryManager : Manager<InventoryManager>
{
    public List<InventoryItem> inventory { get; private set; }

    public InventoryUIItem[] inventorySlots;

    public event System.Action inventoryChanged;

    protected override void Initialize()
    {
        base.Initialize();

        inventoryChanged += UpdateItemSlots;

        inventory = new List<InventoryItem>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void UpdateItemSlots()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (i < inventory.Count)
            {
                inventorySlots[i].item = inventory[i];
                inventorySlots[i].SetVisibility(true);
            }
            else
            {
                inventorySlots[i].item = null;
                inventorySlots[i].SetVisibility(false);
            }
        }
    }

    private void OnInventoryChanged()
    {
        inventoryChanged?.Invoke();
    }

    public void AddItem(InventoryItem inItem)
    {
        inventory.Add(inItem);

        Debug.Log(inItem.id + " added to inventory");

        OnInventoryChanged();
    }

    public void RemoveItem(string itemID)
    {
        foreach (InventoryItem item in inventory)
        {
            if (item.id == itemID)
            {
                inventory.Remove(item);
                return;
            }
        }

        OnInventoryChanged();
    }

    public void RemoveItem(InventoryItem inItem)
    {
        foreach (InventoryItem item in inventory)
        {
            if (item.id == inItem.id)
            {
                inventory.Remove(item);
                return;
            }
        }

        OnInventoryChanged();
    }

    public bool HasItem(string itemID)
    {
        return !inventory.TrueForAll((InventoryItem item) => item.id != itemID);
    }

    public bool HasItem(InventoryItem inItem)
    {
        return !inventory.TrueForAll((InventoryItem item) => item.id != inItem.id);
    }
}
