using UnityEngine;

public class ManagerInterface : MonoBehaviour
{
    private DialogueManager _dialogue;
    private InventoryManager _inventory;
    private SceneManagerComponent _scene;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _dialogue = DialogueManager.Instance;
        _inventory = InventoryManager.Instance;
        _scene = SceneManagerComponent.Instance;

        _inventory.ResetState();
    }

    public void SetDialogue(TextAsset dialogue) => _dialogue.SetStory(dialogue);
    public void AddItemToInventory(InventoryItem item) => _inventory.AddItem(item);
    public void RemoveItemFromInventory(string itemID) => _inventory.RemoveItem(itemID);
    public void RemoveItemFromInventory(InventoryItem item) => _inventory.RemoveItem(item);
    public void LoadScene(string sceneName) => _scene.LoadScene(sceneName);
    public void LoadScene(int sceneID) => _scene.LoadScene(sceneID);
    public void FadeOut(float duration) => _scene.FadeOut(duration);
}
