using UnityEngine;
using UnityEngine.Events;

public class PointAndClickObject : Interactable
{
    public Transform objectDestinationNode;

    public PointAndClickCharacterController[] pointAndClickCharacters;

    public bool hasLockedDialogue = false;

    public InventoryItem itemKey;
    public UnityEvent onUnlockedEvent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void OnClicked()
    {
        foreach (var pc in pointAndClickCharacters)
        {
            pc.SetDestination(this);
        }
    }

    private void ObjectReached()
    {
        base.OnClicked();
    }

    public void Unlock()
    {
        onUnlockedEvent.Invoke();
    }
}
