using System.Collections.Generic;
using UnityEngine;

// Inventory manager class
// Stores and manages all of the items in the player's inventory
public class InventoryManager : Manager<InventoryManager>
{
    public List<InventoryItem> inventory { get; private set; }

    protected override void Initialize()
    {
        base.Initialize();

        inventory = new List<InventoryItem>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddItem(InventoryItem inItem)
    {
        inventory.Add(inItem);

        Debug.Log(inItem.id + " added to inventory");
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
