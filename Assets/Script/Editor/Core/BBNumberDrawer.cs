using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(BBNumber))]
public class BBNumberDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        var significandProperty = property.FindPropertyRelative("significand");
        var exponentProperty = property.FindPropertyRelative("exponent");

        // label을 기준으로 position을 만들어줌
        position = EditorGUI.PrefixLabel(position, label);

        // EditorGUI.indentLevel은 들여쓰기 단계를 설정하는 변수로
        // 들여쓰기 단계에 따라서 property의 x좌표 틀어짐 문제가 있어서 이를 조정해줌
        // 이는 꼭 해줘야하는게 아니라 CustomEditor를 작성하다보면 여러 GUI들이 얽혀서 좌표 문제를 일으키는 경우가 있음
        // 지금이 그 상황이라 적절한 수치를 찾아서 조정해주는 것
        float adjust = EditorGUI.indentLevel * 20f;
        float halfWidth = (position.width * 0.46f) + adjust;

        var significandRect = new Rect(position.x - adjust, position.y, halfWidth - 2.5f, position.height);
        significandProperty.doubleValue = EditorGUI.DoubleField(significandRect, GUIContent.none, significandProperty.doubleValue);

        var eRect = new Rect(significandRect.x + significandRect.width - adjust + 6.5f, position.y, 10f, position.height);
        EditorGUI.LabelField(eRect, "E");
        
        var exponentRect = new Rect(significandRect.x + significandRect.width - adjust + 25f, position.y, halfWidth, position.height);
        exponentProperty.doubleValue = EditorGUI.DoubleField(exponentRect, GUIContent.none, exponentProperty.doubleValue);

        EditorGUI.EndProperty();
    }
}
