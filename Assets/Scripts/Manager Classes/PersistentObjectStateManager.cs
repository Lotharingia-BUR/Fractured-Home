using System.Collections.Generic;
using UnityEngine;

public class PersistentObjectStateManager : Manager<PersistentObjectStateManager>
{
    private Dictionary<string, PointAndClickObjectState> _objectStates;

    protected override void Initialize()
    {
        base.Initialize();

        _objectStates = new Dictionary<string, PointAndClickObjectState>();
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
}

public struct PointAndClickObjectState
{
    public bool isCollected;
    public bool isUnlocked;
}