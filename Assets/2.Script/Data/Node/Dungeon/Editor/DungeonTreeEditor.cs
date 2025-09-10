using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

[CustomEditor(typeof(DungeonTree))]
public class DungeonTreeEditor : OdinEditor
{
    private SerializedProperty graphProperty;

    protected override void OnEnable()
    {
        base.OnEnable();
        graphProperty = serializedObject.FindProperty("graph");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        if (graphProperty.objectReferenceValue is null)
        {
            var targetObject = serializedObject.targetObject;
            var newGraph = CreateInstance<DungeonTreeGraph>();
            newGraph.name = $"{targetObject.name} Graph";

            var path = AssetDatabase.GetAssetPath(targetObject);
            if (!string.IsNullOrEmpty(path))
            {
                AssetDatabase.AddObjectToAsset(newGraph, path);
                AssetDatabase.SaveAssets();
                graphProperty.objectReferenceValue = newGraph;
            }
        }

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(graphProperty);

        EditorGUILayout.Space();

        if (GUILayout.Button("Open Graph", GUILayout.Height(30)))
        {
            NodeEditorWindow.Open(graphProperty.objectReferenceValue as NodeGraph);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
