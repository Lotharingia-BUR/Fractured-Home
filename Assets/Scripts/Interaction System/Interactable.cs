using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class Interactable : MonoBehaviour
{
    // Unity event to run on interaction
    public GameplayEvent onInteractedEvent;

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) { return; }
        if (PauseModeManager.Instance.pauseMode != PauseMode.Unpaused) { return; }

        OnClicked();
    }

    public void Click()
    {
        if (EventSystem.current.IsPointerOverGameObject()) { return; }
        if (PauseModeManager.Instance.pauseMode != PauseMode.Unpaused) { return; }

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
            StartCoroutine(onInteractedEvent.Run());
        }
    }
}
