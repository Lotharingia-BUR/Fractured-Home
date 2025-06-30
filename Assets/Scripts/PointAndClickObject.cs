using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PointAndClickObject : Interactable
{
    public Transform objectDestinationNode;

    public PointAndClickCharacterController[] pointAndClickCharacters;

    public bool hasLockedDialogue = false;

    public InventoryItem itemKey;
    public UnityEvent onUnlockedEvent;
    public UnityEvent onInteractUnlockedEvent;

    private bool _isUnlocked = false;

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
        if (_isUnlocked)
        {
            if (onInteractUnlockedEvent == null)
            {
                Debug.LogWarning($"No unlocked interaction event has been assigned to {gameObject.name}");
            }
            else
            {
                onInteractUnlockedEvent.Invoke();
            }
        }
        else
        {
            base.OnClicked();
        }
    }

    public void Unlock()
    {
        onUnlockedEvent.Invoke();
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(PointAndClickObject)), CanEditMultipleObjects]
public class PointAndClickObjectEditor : Editor
{
    private SerializedProperty _interactLockedEvent;
    private SerializedProperty _interactUnlockedEvent;
    private SerializedProperty _unlockedEvent;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


    }
}
#endif