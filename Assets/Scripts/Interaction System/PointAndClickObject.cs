using UnityEngine;
using UnityEngine.Events;
using Unity.VisualScripting;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class PointAndClickObject : Interactable
{
    public Transform objectDestinationNode;

    private PointAndClickCharacterController _pointAndClickCharacter;

    public InventoryItem itemKey;
    public UnityEvent onUnlockedEvent;
    public UnityEvent onInteractUnlockedEvent;

    private bool _isUnlocked = false;

    void Awake()
    {
        PointAndClickObjectState savedState = PersistentObjectStateManager.Instance.GetObjectState(gameObject.name);
        if (savedState.isCollected)
        {
            Destroy(gameObject);
        }
        _isUnlocked = savedState.isUnlocked;
        if (_isUnlocked)
        {
            onUnlockedEvent.Invoke();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _pointAndClickCharacter = FindFirstObjectByType<PointAndClickCharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DestroyPNCObject(PointAndClickObject pncObject)
    {
        PersistentObjectStateManager.Instance.SaveObjectState(pncObject.gameObject.name, true, pncObject._isUnlocked);
        Destroy(pncObject.gameObject);
    }

    protected override void OnClicked()
    {
        _pointAndClickCharacter.SetDestination(this);
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
        PersistentObjectStateManager.Instance.SaveObjectState(gameObject.name, false, true);
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