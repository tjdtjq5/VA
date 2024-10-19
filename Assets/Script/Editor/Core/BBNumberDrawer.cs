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

        // label�� �������� position�� �������
        position = EditorGUI.PrefixLabel(position, label);

        // EditorGUI.indentLevel�� �鿩���� �ܰ踦 �����ϴ� ������
        // �鿩���� �ܰ迡 ���� property�� x��ǥ Ʋ���� ������ �־ �̸� ��������
        // �̴� �� ������ϴ°� �ƴ϶� CustomEditor�� �ۼ��ϴٺ��� ���� GUI���� ������ ��ǥ ������ ����Ű�� ��찡 ����
        // ������ �� ��Ȳ�̶� ������ ��ġ�� ã�Ƽ� �������ִ� ��
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
