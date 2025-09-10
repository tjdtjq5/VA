using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using XNode;
using XNodeEditor;

[CustomNodeEditor(typeof(DungeonNode))]
public class DungeonNodeEditor : NodeEditor
{
    public override int GetWidth() => 300;

    public override void OnHeaderGUI()
    {
        var targetAsNode = target as DungeonNode;
        string header = $"{targetAsNode.Stage} - {targetAsNode.Index} Room";
        GUILayout.Label(header, NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
    }

    public override void OnBodyGUI()
    {
        serializedObject.Update();

        float originalLabelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 120;

        DrawPreviousNodes();
        DrawRoom();

        var outputNode = target.GetPort("thisNode");
        NodeEditorGUILayout.PortField(GUIContent.none, outputNode);

        serializedObject.ApplyModifiedProperties();
    }
    void DrawPreviousNodes()
    {
        NodeEditorGUILayout.DynamicPortList("previousNodes", typeof(DungeonNode), serializedObject, NodePort.IO.Input, Node.ConnectionType.Override, onCreation: OnCreatePreviousStages);
    }

    private void OnCreatePreviousStages(ReorderableList list)
    {
        list.drawHeaderCallback = (Rect rect) => {
            EditorGUI.LabelField(rect, "Previous Nodes");
        };

        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
            var previousNodesProperty = serializedObject.FindProperty("previousNodes");
            if(previousNodesProperty == null || index >= previousNodesProperty.arraySize) return;

            var element = previousNodesProperty.GetArrayElementAtIndex(index);

            EditorGUI.PropertyField(rect, element, new GUIContent("Link Stage"));

            var port = target?.GetPort("previousNodes " + index);

            if (port.Connection != null && port.Connection.GetOutputValue() is not DungeonNode)
            {
                UnityHelper.Log_H($"Disconnect {port.Connection.GetOutputValue()}");
                port.Disconnect(port.Connection);
            }

            var inputNode = port.GetInputValue<DungeonNode>();

            if(inputNode != null)
            {
                element.objectReferenceValue = inputNode;
                
                // Set stage to inputNode's stage + 1
                var dungeonNode = target as DungeonNode;
                var stageProperty = serializedObject.FindProperty("stage");
                stageProperty.intValue = inputNode.Stage + 1;
            }

            var position = rect.position;
            position.x -= 37f;
            NodeEditorGUILayout.PortField(position, port);
        };
    }

    void DrawRoom()
    {
        var roomProperty = serializedObject.FindProperty("dungeonRoom");
        var room = roomProperty.objectReferenceValue as DungeonRoom;

        if (room?.Icon){
            EditorGUILayout.BeginHorizontal();

            GUILayout.Space(GetWidth() * 0.5f - 50f);

            var preview = AssetPreview.GetAssetPreview(room.Icon);
            GUILayout.Label(preview, GUILayout.Width(80), GUILayout.Height(80));

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.PropertyField(roomProperty, GUIContent.none);
    }
}
