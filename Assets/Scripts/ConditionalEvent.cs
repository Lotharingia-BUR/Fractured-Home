using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
#endif

public class ConditionalEvent : MonoBehaviour
{
    public string triggerID;

    public KeyValuePair<string, PointAndClickObjectState>[] conditions = { };

    public GameplayEvent onConditionMetEvent;
    public GameplayEvent onReloadedEvent;

    [HideInInspector] public string[] conditionKeys = { };
    [HideInInspector] public PointAndClickObjectState[] conditionValues = { };

    private float k_checkInterval = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        conditions = new KeyValuePair<string, PointAndClickObjectState>[conditionKeys.Length];
        for (int i = 0; i < conditions.Length; i++)
        {
            var condition = new KeyValuePair<string, PointAndClickObjectState>(conditionKeys[i], conditionValues[i]);
            conditions[i] = condition;
        }

        if (triggerID == null || triggerID == string.Empty)
        {
            triggerID = gameObject.name;
        }

        TriggerObjectState state = PersistentObjectStateManager.Instance.GetTriggerState(triggerID);
        if (state.wasTriggered)
        {
            StartCoroutine(EventCoroutine(new GameplayEvent[] { onReloadedEvent }));
        }
        else
        {
            StartCoroutine(CheckConditions());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator CheckConditions()
    {
        bool conditionsMet = true;
        foreach (var condition in conditions)
        {
            PointAndClickObjectState objState = PersistentObjectStateManager.Instance.GetObjectState(condition.Key);
            if (condition.Value != objState)
            {
                conditionsMet = false;
                break;
            }
        }

        while (!conditionsMet)
        {
            yield return new WaitForSeconds(k_checkInterval);

            conditionsMet = true;
            foreach (var condition in conditions)
            {
                PointAndClickObjectState objState = PersistentObjectStateManager.Instance.GetObjectState(condition.Key);
                if (condition.Value != objState)
                {
                    conditionsMet = false;
                    break;
                }
            }
        }

        StartCoroutine(EventCoroutine(new GameplayEvent[] { onConditionMetEvent, onReloadedEvent }));
    }

    private IEnumerator EventCoroutine(GameplayEvent[] gEvents)
    {
        foreach (GameplayEvent e in gEvents)
        {
            StartCoroutine(e.Run());
        }

        foreach (GameplayEvent e in gEvents)
        {
            yield return new WaitUntil(() => !e.isRunning);
        }

        PersistentObjectStateManager.Instance.SaveTriggerState(triggerID, true);
        Destroy(this);
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(ConditionalEvent)), CanEditMultipleObjects]
public class ConditionalEventEditor : Editor
{
    private bool _conditionsToggle;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var obj = (ConditionalEvent)target;

        _conditionsToggle = EditorGUILayout.Foldout(_conditionsToggle, "Conditions", true);

        if (_conditionsToggle)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Object ID", GUILayout.MinWidth(85f));
            EditorGUILayout.LabelField("Collected?", GUILayout.MinWidth(40f));
            EditorGUILayout.LabelField("Unlocked?", GUILayout.MinWidth(100f));
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();

            var tempKeys = new List<string>();
            var tempValues = new List<PointAndClickObjectState>();

            for (int i = 0; i < obj.conditionKeys.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();

                string key = EditorGUILayout.TextField(obj.conditionKeys[i]);
                PointAndClickObjectState value = new PointAndClickObjectState
                {
                    isCollected = EditorGUILayout.Toggle(obj.conditionValues[i].isCollected),
                    isUnlocked = EditorGUILayout.Toggle(obj.conditionValues[i].isUnlocked)
                };

                if (!EditorGUILayout.LinkButton("Remove"))
                {
                    tempKeys.Add(key);
                    tempValues.Add(value);
                }

                EditorGUILayout.EndHorizontal();
            }

            if (EditorGUILayout.LinkButton("Add Condition"))
            {
                tempKeys.Add("");
                tempValues.Add(new());
            }

            if (!tempKeys.ToArray().SequenceEqual(obj.conditionKeys) || !tempValues.ToArray().SequenceEqual(obj.conditionValues))
            {
                obj.conditionKeys = tempKeys.ToArray();
                obj.conditionValues = tempValues.ToArray();

                EditorUtility.SetDirty(obj);
            }
        }
    }
}
#endif