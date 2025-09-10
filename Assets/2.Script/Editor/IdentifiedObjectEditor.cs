using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(IdentifiedObject), true)]
public class IdentifiedObjectEditor : OdinEditor
{
    #region 1-8
    private SerializedProperty iconProperty;
    private SerializedProperty idProperty;
    private SerializedProperty codeNameProperty;
    private SerializedProperty displayNameProperty;
    private SerializedProperty descriptionProperty;

    private ReorderableList categories;

    private GUIStyle textAreaStyle;

    private readonly Dictionary<string, bool> isFoldoutExpandedesByTitle = new();
    #endregion

    #region 1-9
    protected virtual void OnEnable()
    {
        GUIUtility.keyboardControl = 0;

        iconProperty = serializedObject.FindProperty("icon");
        idProperty = serializedObject.FindProperty("id");
        codeNameProperty = serializedObject.FindProperty("codeName");
        displayNameProperty = serializedObject.FindProperty("displayName");
        descriptionProperty = serializedObject.FindProperty("description");

    }

    private void StyleSetup()
    {
        if (textAreaStyle == null)
        {
            textAreaStyle = new(EditorStyles.textArea);
            textAreaStyle.wordWrap = true;
        }
    }

    protected bool DrawFoldoutTitle(string text)
        => CustomEditorUtility.DrawFoldoutTitle(isFoldoutExpandedesByTitle, text);
    #endregion

    public override void OnInspectorGUI()
    {
        StyleSetup();

        serializedObject.Update();

        if (DrawFoldoutTitle("Infomation"))
        {
            EditorGUILayout.BeginHorizontal("HelpBox");
            {
                iconProperty.objectReferenceValue = EditorGUILayout.ObjectField(GUIContent.none, iconProperty.objectReferenceValue,
                    typeof(Sprite), false, GUILayout.Width(65));

                EditorGUILayout.BeginVertical();
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        GUI.enabled = false;
                        EditorGUILayout.PrefixLabel("ID");
                        EditorGUILayout.PropertyField(idProperty, GUIContent.none);
                        GUI.enabled = true;
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUI.BeginChangeCheck();
                    var prevCodeName = codeNameProperty.stringValue;
                    EditorGUILayout.DelayedTextField(codeNameProperty);
                    if (EditorGUI.EndChangeCheck())
                    {
                        var assetPath = AssetDatabase.GetAssetPath(target);
                        var newName = $"{codeNameProperty.stringValue}";

                        serializedObject.ApplyModifiedProperties();

                        var message = AssetDatabase.RenameAsset(assetPath, newName);
                        if (string.IsNullOrEmpty(message))
                            target.name = newName;
                        else
                            codeNameProperty.stringValue = prevCodeName;
                    }

                    GUI.enabled = false;
                    EditorGUILayout.PropertyField(displayNameProperty);
                    GUI.enabled = true;
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginVertical("HelpBox");
            {
                GUI.enabled = false;
                EditorGUILayout.LabelField("Description");
                descriptionProperty.stringValue = EditorGUILayout.TextArea(descriptionProperty.stringValue,
                    textAreaStyle, GUILayout.Height(60));
                GUI.enabled = true;
            }
            EditorGUILayout.EndVertical();
        }

        if (DrawFoldoutTitle("Setting"))
        {
            base.OnInspectorGUI();
        }

        serializedObject.ApplyModifiedProperties();
    }

    protected bool DrawRemovableLevelFoldout(SerializedProperty datasProperty, SerializedProperty targetProperty,
        int targetIndex, bool isDrawRemoveButton)
    {
        bool isRemoveButtonClicked = false;

        EditorGUILayout.BeginHorizontal();
        {
            GUI.color = Color.green;
            var level = targetProperty.FindPropertyRelative("level").intValue;
            targetProperty.isExpanded = EditorGUILayout.Foldout(targetProperty.isExpanded, $"Level {level}");
            GUI.color = Color.white;

            if (isDrawRemoveButton)
            {
                GUI.color = Color.red;
                if (GUILayout.Button("x", EditorStyles.miniButton, GUILayout.Width(20)))
                {
                    isRemoveButtonClicked = true;
                    datasProperty.DeleteArrayElementAtIndex(targetIndex);
                }
                GUI.color = Color.white;
            }
        }
        EditorGUILayout.EndHorizontal();

        return isRemoveButtonClicked;
    }

    protected void DrawAutoSortLevelProperty(SerializedProperty datasProperty, SerializedProperty levelProperty,
        int index, bool isEditable)
    {
        if (!isEditable)
        {
            GUI.enabled = false;
            EditorGUILayout.PropertyField(levelProperty);
            GUI.enabled = true;
        }
        else
        {
            EditorGUI.BeginChangeCheck();
            var prevValue = levelProperty.intValue;
            EditorGUILayout.DelayedIntField(levelProperty);
            if (EditorGUI.EndChangeCheck())
            {
                if (levelProperty.intValue <= 1)
                    levelProperty.intValue = prevValue;
                else
                {
                    for (int i = 0; i < datasProperty.arraySize; i++)
                    {
                        if (index == i)
                            continue;

                        var element = datasProperty.GetArrayElementAtIndex(i);
                        if (element.FindPropertyRelative("level").intValue == levelProperty.intValue)
                        {
                            levelProperty.intValue = prevValue;
                            break;
                        }
                    }

                    if (levelProperty.intValue != prevValue)
                    {
                        for (int moveIndex = 1; moveIndex < datasProperty.arraySize; moveIndex++)
                        {
                            if (moveIndex == index)
                                continue;

                            var element = datasProperty.GetArrayElementAtIndex(moveIndex).FindPropertyRelative("level");
                            if (levelProperty.intValue < element.intValue || moveIndex == (datasProperty.arraySize - 1))
                            {
                                datasProperty.MoveArrayElement(index, moveIndex);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
