using UnityEngine;
using UnityEngine.Events;
using Unity.VisualScripting;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class PointAndClickObject : Interactable
{
    public Transform dylanDestinationNode;
    public Transform casperDestinationNode;

    private PointAndClickCharacterController _dylan;
    private PointAndClickCharacterController _casper;

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
        PointAndClickCharacterController[] pncCharas = FindObjectsByType<PointAndClickCharacterController>(FindObjectsSortMode.None);

        foreach (PointAndClickCharacterController chara in pncCharas)
        {
            if (chara.gameObject.name == "Human" || chara.gameObject.name == "Dylan")
            {
                _dylan = chara;
            }
            else if (chara.gameObject.name == "Cat" || chara.gameObject.name == "Casper")
            {
                _casper = chara;
            }
        }
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
        _dylan.SetDestination(this, dylanDestinationNode);
        _casper.SetDestination(this, casperDestinationNode);
    }

    private void ObjectReached(string senderName)
    {
        if (senderName != "Human" && senderName != "Dylan") { return; }

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
        _isUnlocked = true;
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