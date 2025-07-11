using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;

[CreateAssetMenu(fileName = "DialogueVariablesInterface", menuName = "Scriptable Objects/DialogueVariablesInterface")]
public class DialogueVariablesInterface : ScriptableObject
{
    public DictionaryOfStringAndBool globalBools;
    public DictionaryOfStringAndInt globalInts;
    public DictionaryOfStringAndFloat globalFloats;

    public void SaveValues()
    {
        DialogueVariables.instance.globalBools = globalBools;
        DialogueVariables.instance.globalInts = globalInts;
        DialogueVariables.instance.globalFloats = globalFloats;
        DialogueVariables.instance.SaveValues();
    }

    public void LoadValues()
    {
        globalBools = DialogueVariables.instance.globalBools;
        globalInts = DialogueVariables.instance.globalInts;
        globalFloats = DialogueVariables.instance.globalFloats;
    }
}


[CustomEditor(typeof(DialogueVariablesInterface)), CanEditMultipleObjects]
public class DialogueVariablesInterfaceEditor : Editor
{
    private bool _boolsToggle;
    private bool _intsToggle;
    private bool _floatsToggle;

    public override void OnInspectorGUI()
    {
        var obj = (DialogueVariablesInterface)target;

        if (obj.globalBools == null) { obj.globalBools = new DictionaryOfStringAndBool(); }
        if (obj.globalInts == null) { obj.globalInts = new DictionaryOfStringAndInt(); }
        if (obj.globalFloats == null) { obj.globalFloats = new DictionaryOfStringAndFloat(); }

        _boolsToggle = EditorGUILayout.Foldout(_boolsToggle, "Global Booleans", true);

        if (_boolsToggle)
        {
            DictionaryOfStringAndBool temp = new DictionaryOfStringAndBool();
            foreach (var kvPair in obj.globalBools)
            {
                EditorGUILayout.BeginHorizontal();
                string key = EditorGUILayout.TextField(kvPair.Key);
                bool value = EditorGUILayout.Toggle(kvPair.Value);

                if (!EditorGUILayout.LinkButton("Remove"))
                {
                    temp.Add(key, value);
                }
                EditorGUILayout.EndHorizontal();
            }

            obj.globalBools = temp;

            if (EditorGUILayout.LinkButton("Add Variable"))
            {
                obj.globalBools.Add("", false);
            }
        }

        EditorGUILayout.Space();

        _intsToggle = EditorGUILayout.Foldout(_intsToggle, "Global Integers", true);

        if (_intsToggle)
        {
            DictionaryOfStringAndInt temp = new DictionaryOfStringAndInt();
            foreach (var kvPair in obj.globalInts)
            {
                EditorGUILayout.BeginHorizontal();
                string key = EditorGUILayout.TextField(kvPair.Key);
                int value = EditorGUILayout.IntField(kvPair.Value);

                if (!EditorGUILayout.LinkButton("Remove"))
                {
                    temp.Add(key, value);
                }
                EditorGUILayout.EndHorizontal();
            }

            obj.globalInts = temp;

            if (EditorGUILayout.LinkButton("Add Variable"))
            {
                obj.globalInts.Add("", 0);
            }
        }

        EditorGUILayout.Space();

        _floatsToggle = EditorGUILayout.Foldout(_floatsToggle, "Global Floats", true);

        if (_floatsToggle)
        {
            DictionaryOfStringAndFloat temp = new DictionaryOfStringAndFloat();
            foreach (var kvPair in obj.globalFloats)
            {
                EditorGUILayout.BeginHorizontal();
                string key = EditorGUILayout.TextField(kvPair.Key);
                float value = EditorGUILayout.FloatField(kvPair.Value);

                if (!EditorGUILayout.LinkButton("Remove"))
                {
                    temp.Add(key, value);
                }
                EditorGUILayout.EndHorizontal();
            }

            obj.globalFloats = temp;

            if (EditorGUILayout.LinkButton("Add Variable"))
            {
                obj.globalFloats.Add("", 0f);
            }
        }

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        int totalCount = obj.globalBools.Count + obj.globalInts.Count + obj.globalFloats.Count;
        if (EditorGUILayout.LinkButton("Save") && totalCount > 0)
        {
            obj.SaveValues();
        }
        if (EditorGUILayout.LinkButton("Load"))
        {
            obj.LoadValues();
        }
        EditorGUILayout.EndHorizontal();
    }
}
#endif