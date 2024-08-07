using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

public static class CustomEditorUtility
{
    public readonly static GUIStyle titleStyle;
    public readonly static GUIStyle middleTitleStyle;

    static CustomEditorUtility()
    {
        // ����Ƽ ���ο� ���ǵǾ��ִ� ShurikenModuleTitle Style�� Base�� ��
        titleStyle = new GUIStyle("ShurikenModuleTitle")
        {
            // ����Ƽ Default Label�� font�� ������
            font = new GUIStyle(EditorStyles.label).font,
            fontStyle = FontStyle.Bold,
            fontSize = 16,
            // title�� �׸� ������ ������ ��
            border = new RectOffset(15, 7, 4, 4),
            // ���̴� 26
            fixedHeight = 26f,
            // ���� Text�� ��ġ�� ������
            contentOffset = new Vector2(20f, -2f)
        };

        middleTitleStyle = new GUIStyle("ShurikenModuleTitle")
        {
            // ����Ƽ Default Label�� font�� ������
            font = new GUIStyle(EditorStyles.label).font,
            fontStyle = FontStyle.Bold,
            fontSize = 14,
            // title�� �׸� ������ ������ ��
            border = new RectOffset(15, 7, 4, 4),
            // ���̴� 26
            fixedHeight = 22f,
            // ���� Text�� ��ġ�� ������
            contentOffset = new Vector2(20f, -2f)
        };

        buttonStyle.margin = new RectOffset(0, 0, buttonStyle.margin.top, buttonStyle.margin.bottom);
    }

    public static bool DrawFoldoutTitle(string title, bool isExpanded, float space = 15f)
    {
        // space��ŭ �� ���� ���
        EditorGUILayout.Space(space);

        // titleStyle�� ������ ������ Inspector�󿡼� �ǹٸ� ��ġ�� ������
        var rect = GUILayoutUtility.GetRect(16f, titleStyle.fixedHeight, titleStyle);
        // TitleStyle�� �����Ų Box�� �׷���
        GUI.Box(rect, title, titleStyle);

        // ���� Editor�� Event�� ������
        // Editor Event�� ���콺 �Է�, GUI ���� �׸���(Repaint), Ű���� �Է� �� Editor �󿡼� �Ͼ�� ����
        var currentEvent = Event.current;
        // Toggle Button�� ��ġ�� ũ�⸦ ����
        // ��ġ�� ��� �׸� �ڽ��� ��ǥ���� ��¦ ������ �Ʒ�, �� Button�� ��, ��� ������ �� ���°� ��.
        var toggleRect = new Rect(rect.x + 4f, rect.y + 4f, 13f, 13f);

        // Event�� Repaint(=GUI�� �׸��� Ȥ�� �ٽ� �׸���)�� �ܼ��� foldout button�� ������
        if (currentEvent.type == EventType.Repaint)
            EditorStyles.foldout.Draw(toggleRect, false, false, isExpanded, false);
        // Event�� MouseDown�̰� mousePosition�� rect�� ����������(=Mouse Pointer�� ������ �׷��� Box�ȿ� ����) Click ����
        else if (currentEvent.type == EventType.MouseDown && rect.Contains(currentEvent.mousePosition))
        {
            isExpanded = !isExpanded;
            // Use �Լ��� ������� ������ ���� Event�� ó������ ���� ������ �ǴܵǾ� ���� ��ġ�� �ִ� �ٸ� GUI�� ���� ���۵� �� ����.
            // event ó���� ������ �׻� Use�� ���� event�� ���� ó���� ������ Unity�� �˷��ִ°� ����
            currentEvent.Use();
        }

        return isExpanded;
    }

    // FoldoutTitle�� �׸��� ���ÿ� ���ڷ� ���� Dictionary�� Expand ��Ȳ�� ������� ����
    public static bool DrawFoldoutTitle(IDictionary<string, bool> isFoldoutExpandedesByTitle, string title, float space = 15f)
    {
        if (!isFoldoutExpandedesByTitle.ContainsKey(title))
            isFoldoutExpandedesByTitle[title] = true;

        isFoldoutExpandedesByTitle[title] = DrawFoldoutTitle(title, isFoldoutExpandedesByTitle[title], space);
        return isFoldoutExpandedesByTitle[title];
    }

    public static void DrawUnderline(float height = 1f)
    {
        // ���������� �׸� GUI�� ��ġ�� ũ�� ������ ���� Rect ����ü�� ������
        var lastRect = GUILayoutUtility.GetLastRect();
        // rect�� y���� ���� GUI�� ���̸�ŭ ����(=��, y���� ���� GUI �ٷ� �Ʒ��� ��ġ�ϰ� ��)
        lastRect.y += lastRect.height;
        lastRect.height = height;
        // rect ���� �̿��ؼ� ������ ��ġ�� heightũ���� Box�� �׸�
        // height�� 1�̶�� ���� GUI �ٷ� �Ʒ��� ũ�Ⱑ 1�� Box, �� Line�� �׷����Ե�
        EditorGUI.DrawRect(lastRect, Color.gray);
    }

    public static void DrawLine(float width = 1f, float height = 1f)
    {
        var lastRect = GUILayoutUtility.GetLastRect();
        lastRect.width = width;
        lastRect.height = height;

        EditorGUI.DrawRect(lastRect, Color.gray);
    }

    public static float GetScreenWidth { get => EditorGUIUtility.currentViewWidth; }
    public static float GetScreenHeight { get => Screen.height * (GetScreenWidth / Screen.width); }

    #region Label

    public static GUIStyle GetMiddleLabel { get=> new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter }; }
    public static GUIStyle GetRightLabel { get=> new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleRight }; }
    public static GUIStyle GetLeftLabel { get=> new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft }; }
    public static GUIStyle GetLabelStyle(int fontSize)
    {
        return new GUIStyle(EditorStyles.helpBox) { fontSize = fontSize };
    }

    #endregion

    #region Text Area

    public static GUIStyle GetTextAreaSeretStyle
    {
        get
        {
            return new GUIStyle(GUI.skin.textArea) { };
        }
    }

    #endregion

    #region Button
    static GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
    public static GUIStyle GetButtonStyle(float width, float height, AnchorStyles anchorStyles)
    {
        GUIStyle buttonStyle = null;

        switch (anchorStyles)
        {
            case AnchorStyles.Left:
                buttonStyle = new GUIStyle(EditorStyles.miniButtonLeft);
                break;
            case AnchorStyles.Right:
                buttonStyle = new GUIStyle(EditorStyles.miniButtonRight);
                break;
            default:
                buttonStyle = new GUIStyle(EditorStyles.miniButtonMid);
                break;
        }

        buttonStyle.fixedWidth = width;
        buttonStyle.fixedHeight = height;

        return buttonStyle;
    }

    #endregion
}
