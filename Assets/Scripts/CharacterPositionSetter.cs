using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CharacterPositionSetter : MonoBehaviour
{
    public Transform dylan;
    public Transform casper;

    [HideInInspector] public string[] sourceScenes;
    [HideInInspector] public Transform[] dylanPositions;
    [HideInInspector] public Transform[] casperPositions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < sourceScenes.Length; i++)
        {
            if (SceneManagerComponent.Instance.prevScene == sourceScenes[i])
            {
                dylan.position = dylanPositions[i].position;
                casper.position = casperPositions[i].position;

                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}


[CustomEditor(typeof(CharacterPositionSetter)), CanEditMultipleObjects]
public class CharacterPositionSetterEditor : Editor
{
    private bool _conditionsToggle;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var obj = (CharacterPositionSetter)target;

        _conditionsToggle = EditorGUILayout.Foldout(_conditionsToggle, "Positions", true);

        if (_conditionsToggle)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Scene Name", GUILayout.MinWidth(85f));
            EditorGUILayout.LabelField("Dylan", GUILayout.MinWidth(40f));
            EditorGUILayout.LabelField("Casper", GUILayout.MinWidth(100f));
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();

            var tempScenes = new List<string>();
            var tempDylanPositions = new List<Transform>();
            var tempCasperPositions = new List<Transform>();

            for (int i = 0; i < obj.sourceScenes.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();

                string scene = EditorGUILayout.TextField(obj.sourceScenes[i]);
                Transform dylanPos = (Transform)EditorGUILayout.ObjectField(obj.dylanPositions[i], typeof(Transform), true);
                Transform casperPos = (Transform)EditorGUILayout.ObjectField(obj.casperPositions[i], typeof(Transform), true);

                if (!EditorGUILayout.LinkButton("Remove"))
                {
                    tempScenes.Add(scene);
                    tempDylanPositions.Add(dylanPos);
                    tempCasperPositions.Add(casperPos);
                }

                EditorGUILayout.EndHorizontal();
            }

            if (EditorGUILayout.LinkButton("Add Scene"))
            {
                tempScenes.Add("");
                tempDylanPositions.Add(null);
                tempCasperPositions.Add(null);
            }

            if (!tempScenes.ToArray().SequenceEqual(obj.sourceScenes) || !tempDylanPositions.ToArray().SequenceEqual(obj.dylanPositions) || !tempCasperPositions.ToArray().SequenceEqual(obj.casperPositions))
            {
                obj.sourceScenes = tempScenes.ToArray();
                obj.dylanPositions = tempDylanPositions.ToArray();
                obj.casperPositions = tempCasperPositions.ToArray();

                EditorUtility.SetDirty(obj);
            }
        }
    }
}