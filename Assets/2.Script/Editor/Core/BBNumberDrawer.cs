using System.Reflection;
using UnityEditor;
using UnityEngine;
using Shared.BBNumber;

[CustomPropertyDrawer(typeof(BBNumber))]
public class BBNumberDrawer : PropertyDrawer
{
    private static readonly FieldInfo significandField =
        typeof(BBNumber).GetField("significand", BindingFlags.Public | BindingFlags.Instance);
    private static readonly FieldInfo exponentField =
        typeof(BBNumber).GetField("exponent", BindingFlags.Public | BindingFlags.Instance);

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // 기존 SerializedProperty 접근 시도
        var significandProperty = property.FindPropertyRelative("significand");
        var exponentProperty = property.FindPropertyRelative("exponent");

        // FindPropertyRelative가 실패하면 Reflection으로 값 읽기/쓰기 준비
        bool useReflection = significandProperty == null || exponentProperty == null;

        // 현재 값 가져오기
        BBNumber value;
        if (useReflection)
        {
            // Unity 또는 Odin 모두 안전하게 접근 가능
            object target = property.serializedObject.targetObject;
            value = (BBNumber)fieldInfo.GetValue(target);
        }
        else
        {
            value = new BBNumber(significandProperty.doubleValue, exponentProperty.doubleValue);
        }

        // UI 시작
        position = EditorGUI.PrefixLabel(position, label);
        float adjust = EditorGUI.indentLevel * 20f;
        float halfWidth = (position.width * 0.46f) + adjust;

        EditorGUI.BeginChangeCheck();

        // significand
        var significandRect = new Rect(position.x - adjust, position.y, halfWidth - 2.5f, position.height);
        double significand = useReflection
            ? (double)significandField.GetValue(value)
            : significandProperty.doubleValue;

        significand = EditorGUI.DoubleField(significandRect, significand);

        // label 'E'
        var eRect = new Rect(significandRect.x + significandRect.width - adjust + 6.5f, position.y, 10f, position.height);
        EditorGUI.LabelField(eRect, "E");

        // exponent
        var exponentRect = new Rect(significandRect.x + significandRect.width - adjust + 25f, position.y, halfWidth, position.height);
        double exponent = useReflection
            ? (double)exponentField.GetValue(value)
            : exponentProperty.doubleValue;

        exponent = EditorGUI.DoubleField(exponentRect, exponent);

        // 값이 변경되었으면 갱신
        if (EditorGUI.EndChangeCheck())
        {
            // BBNumber 보정 (기존 로직 유지)
            BBNumber newValue = new BBNumber(significand, 0);
            newValue = BBNumber.Rebalance(newValue);

            if (useReflection)
            {
                // Reflection으로 필드에 직접 반영
                significandField.SetValueDirect(__makeref(newValue), newValue.Significand);
                exponentField.SetValueDirect(__makeref(newValue), newValue.Exponent);

                object target = property.serializedObject.targetObject;
                fieldInfo.SetValue(target, newValue);
            }
            else
            {
                // SerializedProperty 갱신
                significandProperty.doubleValue = newValue.Significand;
                exponentProperty.doubleValue = newValue.Exponent;
            }
        }

        EditorGUI.EndProperty();
    }
}
