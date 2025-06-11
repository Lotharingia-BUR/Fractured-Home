using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Interactable : MonoBehaviour
{
    // Unity event to run on interaction
    public UnityEvent onInteractedEvent;

    void OnMouseDown()
    {
        OnClicked();
    }

    protected virtual void OnClicked()
    {
        if (onInteractedEvent == null)
        {
            Debug.LogWarning($"No interaction event has been assigned to {gameObject.name}");
        }
        else
        {
            onInteractedEvent.Invoke();
        }
    }
}
