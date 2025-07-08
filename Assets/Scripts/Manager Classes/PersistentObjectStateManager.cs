using System.Collections.Generic;
using UnityEngine;

public class PersistentObjectStateManager : Manager<PersistentObjectStateManager>
{
    private Dictionary<string, PointAndClickObjectState> _objectStates;
    private Dictionary<string, TriggerObjectState> _triggerStates;

    protected override void Initialize()
    {
        base.Initialize();

        _objectStates = new Dictionary<string, PointAndClickObjectState>();
        _triggerStates = new Dictionary<string, TriggerObjectState>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SaveObjectState(string objectID, bool isCollected, bool isUnlocked)
    {
        PointAndClickObjectState objState = new PointAndClickObjectState
        {
            isCollected = isCollected,
            isUnlocked = isUnlocked
        };

        if (_objectStates.ContainsKey(objectID))
        {
            _objectStates[objectID] = objState;
        }
        else
        {
            _objectStates.Add(objectID, objState);
        }

        Debug.Log($"State of {objectID} saved as {objState.isCollected}, {objState.isUnlocked}");
    }

    public void SaveTriggerState(string objectID, bool wasTriggered)
    {
        TriggerObjectState objState = new TriggerObjectState
        {
            wasTriggered = wasTriggered
        };

        if (_triggerStates.ContainsKey(objectID))
        {
            _triggerStates[objectID] = objState;
        }
        else
        {
            _triggerStates.Add(objectID, objState);
        }

        Debug.Log($"State of {objectID} saved as {objState.wasTriggered}");
    }

    public PointAndClickObjectState GetObjectState(string objectID)
    {
        if (_objectStates.ContainsKey(objectID))
        {
            Debug.Log($"Loaded data for {objectID} as {_objectStates[objectID].isCollected}, {_objectStates[objectID].isUnlocked}");

            return _objectStates[objectID];
        }
        else
        {
            Debug.Log($"{objectID} has no saved data. Loaded default values");

            return new();
        }
    }

    public TriggerObjectState GetTriggerState(string objectID)
    {
        if (_objectStates.ContainsKey(objectID))
        {
            Debug.Log($"Loaded data for {objectID} as {_objectStates[objectID].isCollected}, {_objectStates[objectID].isUnlocked}");

            return _triggerStates[objectID];
        }
        else
        {
            Debug.Log($"{objectID} has no saved data. Loaded default values");

            return new();
        }
    }
}

[System.Serializable]
public struct PointAndClickObjectState
{
    public bool isCollected;
    public bool isUnlocked;

    public static bool operator ==(PointAndClickObjectState a, PointAndClickObjectState b)
    {
        return a.isCollected == b.isCollected && a.isUnlocked == b.isUnlocked;
    }

    public static bool operator !=(PointAndClickObjectState a, PointAndClickObjectState b)
    {
        return !(a == b);
    }
}

[System.Serializable]
public struct TriggerObjectState
{
    public bool wasTriggered;

    public static bool operator ==(TriggerObjectState a, TriggerObjectState b)
    {
        return a.wasTriggered == b.wasTriggered;
    }

    public static bool operator !=(TriggerObjectState a, TriggerObjectState b)
    {
        return !(a == b);
    }
}