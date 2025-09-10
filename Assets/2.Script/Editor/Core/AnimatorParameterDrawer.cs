using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(AnimatorParameter))]
public class AnimatorParameterDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var nameProperty = property.FindPropertyRelative("name");
        var typeProperty = property.FindPropertyRelative("type");

        position = EditorGUI.PrefixLabel(position, label);

        float adjust = EditorGUI.indentLevel * 15f;
        float leftWidth = (position.width * 0.15f) + adjust;
        float rightWidth = (position.width * 0.85f) + adjust;

        var typeRect = new Rect(position.x - adjust, position.y, leftWidth - 2.5f, position.height); ;
        int enumInt = System.Convert.ToInt32(EditorGUI.EnumPopup(typeRect, (AnimatorParameterType)typeProperty.enumValueIndex));
        typeProperty.enumValueIndex = enumInt;

        var nameRect = new Rect(typeRect.x + typeRect.width - adjust + 2.5f, position.y, rightWidth, position.height);
        nameProperty.stringValue = EditorGUI.TextField(nameRect, GUIContent.none, nameProperty.stringValue);

        EditorGUI.EndProperty();
    }
}
