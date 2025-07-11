using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[FilePath("Assets/Narrative/GlobalVariables.json", FilePathAttribute.Location.ProjectFolder)]
public class DialogueVariables : ScriptableSingleton<DialogueVariables>
{
    public DictionaryOfStringAndBool globalBools;
    public DictionaryOfStringAndInt globalInts;
    public DictionaryOfStringAndFloat globalFloats;

    public void SaveValues()
    {
        Save(true);
        Debug.Log("Saved to " + GetFilePath());
    }
}
#endif

//Class from this answer on the Unity Forums: https://discussions.unity.com/t/solved-how-to-serialize-dictionary-with-unity-serialization-system/71474/4
[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> keys = new List<TKey>();

    [SerializeField]
    private List<TValue> values = new List<TValue>();

    // save the dictionary to lists
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    // load dictionary from lists
    public void OnAfterDeserialize()
    {
        this.Clear();

        if (keys.Count != values.Count)
            throw new System.Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));

        for (int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
    }
}

[System.Serializable] public class DictionaryOfStringAndBool : SerializableDictionary<string, bool> { }

[System.Serializable] public class DictionaryOfStringAndInt : SerializableDictionary<string, int> { }

[System.Serializable] public class DictionaryOfStringAndFloat : SerializableDictionary<string, float> { }