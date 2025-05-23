using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Interactable : MonoBehaviour
{
    public UnityEvent onInteractedEvent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
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
