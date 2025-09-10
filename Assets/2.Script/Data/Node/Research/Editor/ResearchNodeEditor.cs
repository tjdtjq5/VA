using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using XNode;
using XNodeEditor;

[CustomNodeEditor(typeof(ResearchNode))]
public class ResearchNodeEditor : NodeEditor
{
    public override int GetWidth() => 300;

    public override void OnHeaderGUI()
    {
        var targetAsNode = target as ResearchNode;
        string header = $"{targetAsNode.Floor} - {targetAsNode.Index} Research";
        GUILayout.Label(header, NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
    }

    public override void OnBodyGUI()
    {
        serializedObject.Update();

        float originalLabelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 120;

        DrawPreviousNodes();
        Draw();

        var outputNode = target.GetPort("thisNode");
        NodeEditorGUILayout.PortField(GUIContent.none, outputNode);

        serializedObject.ApplyModifiedProperties();
    }

    void DrawPreviousNodes()
    {
        NodeEditorGUILayout.DynamicPortList("previousNodes", typeof(ResearchNode), serializedObject, NodePort.IO.Input, Node.ConnectionType.Override, onCreation: OnCreatePreviousStages);
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

            if (port.Connection != null && port.Connection.GetOutputValue() is not ResearchNode)
            {
                UnityHelper.Log_H($"Disconnect {port.Connection.GetOutputValue()}");
                port.Disconnect(port.Connection);
            }

            var inputNode = port.GetInputValue<ResearchNode>();

            if(inputNode != null)
            {
                element.objectReferenceValue = inputNode;
                
                // Set stage to inputNode's stage + 1
                var researchNode = target as ResearchNode;
                var floorProperty = serializedObject.FindProperty("floor");
                floorProperty.intValue = inputNode.Floor + 1;
            }

            var position = rect.position;
            position.x -= 37f;
            NodeEditorGUILayout.PortField(position, port);
        };
    }

    void Draw()
    {
        var researchProperty = serializedObject.FindProperty("research");
        var research = researchProperty.objectReferenceValue as Research;

        if (research?.Icon){
            EditorGUILayout.BeginHorizontal();

            GUILayout.Space(GetWidth() * 0.5f - 50f);

            var preview = AssetPreview.GetAssetPreview(research.Icon);
            GUILayout.Label(preview, GUILayout.Width(80), GUILayout.Height(80));

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.PropertyField(researchProperty, GUIContent.none);
    }
}
