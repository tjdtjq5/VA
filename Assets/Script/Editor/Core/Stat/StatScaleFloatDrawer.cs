using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(StatScaleFloat))]
public class StatScaleFloatDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var defaultValueProperty = property.FindPropertyRelative("defaultValue");
        var scaleStatProperty = property.FindPropertyRelative("scaleStat");

        // label�� �������� position�� �������
        position = EditorGUI.PrefixLabel(position, label);

        // EditorGUI.indentLevel�� �鿩���� �ܰ踦 �����ϴ� ������
        // �鿩���� �ܰ迡 ���� property�� x��ǥ Ʋ���� ������ �־ �̸� ��������
        // �̴� �� ������ϴ°� �ƴ϶� CustomEditor�� �ۼ��ϴٺ��� ���� GUI���� ������ ��ǥ ������ ����Ű�� ��찡 ����
        // ������ �� ��Ȳ�̶� ������ ��ġ�� ã�Ƽ� �������ִ� ��
        float adjust = EditorGUI.indentLevel * 15f;
        float halfWidth = (position.width * 0.5f) + adjust;

        var defaultValueRect = new Rect(position.x - adjust, position.y, halfWidth - 2.5f, position.height);
        defaultValueProperty.floatValue = EditorGUI.FloatField(defaultValueRect, GUIContent.none, defaultValueProperty.floatValue);

        var scaleStatRect = new Rect(defaultValueRect.x + defaultValueRect.width - adjust + 2.5f, position.y, halfWidth, position.height);
        scaleStatProperty.objectReferenceValue = EditorGUI.ObjectField(scaleStatRect, GUIContent.none, scaleStatProperty.objectReferenceValue, typeof(Stat), false);

        EditorGUI.EndProperty();
    }
}
