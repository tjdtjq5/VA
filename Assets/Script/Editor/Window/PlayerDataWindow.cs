using UnityEditor;
using UnityEngine;

public class PlayerDataWindow : EditorWindow
{
    static Vector2 _windowSize;
    float btnHeight = 35f;

    private Editor cachedEditor;
    private static PlayerDataClassSelecter playerDataClassSelecter;

    [MenuItem("Tool/PlayerData")]
    static void Open()
    {
        var window = GetWindow<PlayerDataWindow>();
        window.titleContent = new GUIContent() { text = "PlayerData" };
        window.minSize = _windowSize;
        window.Show();
    }
    private void OnEnable()
    {
        _windowSize = new Vector2(600, 700);
        playerDataClassSelecter = new PlayerDataClassSelecter();
    }
    private void OnDisable()
    {
        DestroyImmediate(cachedEditor);
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();
        {
            if (playerDataClassSelecter)
            {
                Editor.CreateCachedEditor(playerDataClassSelecter, null, ref cachedEditor);
                cachedEditor.OnInspectorGUI();
            }

            Color origin = GUI.color;
            GUI.color = Color.cyan;
            if (GUILayout.Button("Create", GUILayout.Height(btnHeight)))
            {
                OnClickCreate();
            }

            EditorGUILayout.Space(5);

            GUI.color = Color.red;
            if (GUILayout.Button("Delete", GUILayout.Height(btnHeight)))
            {
                OnClickDelete();
            }
            GUI.color = origin;

            EditorGUILayout.Space(20);

            EditorGUILayout.LabelField("Do This First\nServiceData -> RR -> Service -> Controllers", CustomEditorUtility.GetLabelStyle(15));
        }
        GUILayout.EndVertical();
    }

    void OnClickCreate()
    {
        if (!playerDataClassSelecter)
            return;
    }
    void OnClickDelete()
    {
        if (!playerDataClassSelecter)
            return;
    }
}