using UnityEngine;
using UnityEngine.Events;

public class PointAndClickObject : Interactable
{
    public Transform dylanDestinationNode;
    public Transform casperDestinationNode;

    private PointAndClickCharacterController _dylan;
    private PointAndClickCharacterController _casper;

    public InventoryItem itemKey;
    public GameplayEvent onUnlockedEvent;
    public GameplayEvent onInteractUnlockedEvent;

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
            StartCoroutine(onUnlockedEvent.Run());
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
        if (PauseModeManager.Instance.pauseMode != PauseMode.Unpaused) { return; }

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
                StartCoroutine(onInteractUnlockedEvent.Run());
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
        StartCoroutine(onUnlockedEvent.Run());
    }
}