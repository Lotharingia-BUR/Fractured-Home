using UnityEngine;

// Inventory Item class
// Scriptable Object that represents an item that can be in the player's inventory
[CreateAssetMenu(fileName = "InventoryItem", menuName = "Scriptable Objects/InventoryItem")]
public class InventoryItem : ScriptableObject
{
    //The unique ID associated with this item
    public string id;

    //The sprite to display in the inventory UI
    public Sprite sprite;

    public event System.Action onDestroyEvent;
}
