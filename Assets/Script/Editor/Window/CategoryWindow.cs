using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CategoryWindow : EditorWindow
{
    [MenuItem("Tool/Category %w")]
    static void Open()
    {
        var window = GetWindow<CategoryWindow>();
        window.titleContent = new GUIContent() { text = "Category" };
        window.minSize = _windowSize;

        window.Show();
    }

    static Vector2 _windowSize;

    int _toolbarMinWidth;
    int _toolbarHeight;
    Vector2 _toolbarScrollviewPos;

    string _categoryTitle;
    string _tempCategoryTitle;
    int _categoryToolbarIndex;
    string[] _categoryNames;

    int _functionWidth;
    string _changeCategoryName;

    string _typeTitleFormat;
    string _tempTypeTitle;
    Vector2 _typeScrollviewPos;
    int _typeBtnHeight;

    string _changeTypeName;

    Dictionary<string, string> _selectType = new Dictionary<string, string>();

    private void OnEnable()
    {
        _windowSize = new Vector2(800, 700);

        _toolbarMinWidth = 200;
        _toolbarHeight = 20;

        _categoryTitle = "Category";
        _tempCategoryTitle = "NewCategory_";
        _changeCategoryName = "";

        _functionWidth = 220;

        _typeTitleFormat = "{0}Type";
        _tempTypeTitle = "NewType_";
        _typeBtnHeight = 35;

        _changeTypeName = "";
    }
    private void OnGUI()
    {
        CategoryToolbarSetting();

        EditorGUILayout.Space(4f);
        CustomEditorUtility.DrawUnderline();
        EditorGUILayout.Space(4f);

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(_functionWidth), GUILayout.ExpandHeight(true));
            {
                CategoryFunctionSetting();

                EditorGUILayout.Space(4f);
                CustomEditorUtility.DrawUnderline();
                EditorGUILayout.Space(4f);

                CategoryTypeSetting();
                EditorGUILayout.Space(4f);
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(_functionWidth), GUILayout.ExpandHeight(true));
            {
                TypeFuncSetting();

                EditorGUILayout.Space(4f);
                CustomEditorUtility.DrawUnderline();
                EditorGUILayout.Space(4f);

                TypeObjectSetting();
                EditorGUILayout.Space(4f);
            }
            EditorGUILayout.EndVertical();
        }


        EditorGUILayout.EndHorizontal();
    }

    void CategoryToolbarSetting()
    {
        bool existCategory = EnumPacketFormat.Exist(_categoryTitle);

        if (!existCategory)
        {
            _categoryNames = null;

            if (GUILayout.Button("Add New Category", GUILayout.ExpandWidth(true)))
            {
                AddCategory(_tempCategoryTitle + 1);
            }
        }
        else
        {
            _categoryNames = EnumPacketFormat.GetValues(_categoryTitle).ToArray();

            float screenWidth = _windowSize.x - 20;
            float toolbarSize = screenWidth / (float)(_categoryNames.Length + 1);
            toolbarSize = Mathf.Max(toolbarSize, _toolbarMinWidth);
            float btnSize = toolbarSize;
            toolbarSize = toolbarSize * _categoryNames.Length;

            EditorGUILayout.BeginHorizontal(GUILayout.Height(_toolbarHeight + 15));
            {
                _toolbarScrollviewPos = EditorGUILayout.BeginScrollView(_toolbarScrollviewPos);
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        _categoryToolbarIndex = GUILayout.Toolbar(_categoryToolbarIndex, _categoryNames, GUILayout.Width(toolbarSize), GUILayout.Height(_toolbarHeight));
                        if (GUILayout.Button("+", GUILayout.Width(btnSize), GUILayout.Height(_toolbarHeight)))
                        {
                            AddCategory($"{_tempCategoryTitle}{_categoryNames.Length + 1}");
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
    void CategoryFunctionSetting()
    {
        if (_categoryNames == null || _categoryNames.Length < 1)
        {
            return;
        }

        if (_categoryNames.Length - 1 < _categoryToolbarIndex)
        {
            return;
        }

        string selectCategoryName = _categoryNames[_categoryToolbarIndex];

        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.LabelField(selectCategoryName, CustomEditorUtility.titleStyle);
            EditorGUILayout.Space(15f);

            Color originColor = GUI.backgroundColor;

            _changeCategoryName = EditorGUILayout.TextArea(_changeCategoryName);

            GUI.backgroundColor = Color.green;
            if (GUILayout.Button($"ChangeName"))
            {
                ChangeCategory(selectCategoryName, _changeCategoryName);
            }

            EditorGUILayout.Space(15f);
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button($"Delete"))
            {
                RemoveCategory(selectCategoryName);
            }

            GUI.backgroundColor = originColor;
        }
        EditorGUILayout.EndVertical();
    }
    void CategoryTypeSetting()
    {
        if (_categoryNames == null || _categoryNames.Length < 1)
        {
            return;
        }

        if (_categoryNames.Length - 1 < _categoryToolbarIndex)
        {
            return;
        }

        string selectCategoryName = _categoryNames[_categoryToolbarIndex];
        string selectType = GetSelectType(selectCategoryName);

        string typeEnumName = GetTypeEnumName(selectCategoryName);
        _typeScrollviewPos = EditorGUILayout.BeginScrollView(_typeScrollviewPos);
        {
            List<string> typeList = EnumPacketFormat.GetValues(typeEnumName);

            if (typeList != null && typeList.Count >= 1)
            {
                for (int i = 0; i < typeList.Count; i++)
                {
                    Color originColor = GUI.backgroundColor;

                    if (selectType.Equals(typeList[i]))
                    {
                        GUI.backgroundColor = Color.cyan;
                    }

                    if (GUILayout.Button($"{typeList[i]}", GUILayout.Height(_typeBtnHeight)))
                    {
                        SetSelectType(selectCategoryName, typeList[i]);
                    }

                    GUI.backgroundColor = originColor;
                }
            }

            if (GUILayout.Button($"+", GUILayout.Height(_typeBtnHeight)))
            {
                // Type 추가 버튼
                int typeCount = typeList != null ? typeList.Count : 0;
                AddType(selectCategoryName, $"{_tempTypeTitle}{typeCount}");
            }
        }
        EditorGUILayout.EndScrollView();

    }

    void TypeFuncSetting()
    {
        if (_categoryNames == null || _categoryNames.Length < 1)
        {
            return;
        }

        if (_categoryNames.Length - 1 < _categoryToolbarIndex)
        {
            return;
        }

        string selectCategoryName = _categoryNames[_categoryToolbarIndex];
        string selectType = GetSelectType(selectCategoryName);

        if (string.IsNullOrEmpty(selectType))
        {
            return;
        }

        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.LabelField(selectType, CustomEditorUtility.titleStyle);
            EditorGUILayout.Space(15f);

            Color originColor = GUI.backgroundColor;

            _changeTypeName = EditorGUILayout.TextArea(_changeTypeName);

            GUI.backgroundColor = Color.green;
            if (GUILayout.Button($"ChangeName"))
            {
                ChangeType(selectCategoryName, selectType, _changeTypeName);
            }

            EditorGUILayout.Space(15f);
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button($"Delete"))
            {
                RemoveType(selectCategoryName, selectType);
            }

            GUI.backgroundColor = originColor;
        }
        EditorGUILayout.EndVertical();
    }
    void TypeObjectSetting()
    {

    }

    void AddCategory(string categoryName)
    {
        bool exist = EnumPacketFormat.Exist(_categoryTitle);
        if (!exist)
        {
            EnumPacketFormat.Create(_categoryTitle, new List<string>() { categoryName });
        }
        else
        {
            EnumPacketFormat.AddValue(_categoryTitle, categoryName);
        }

        _categoryNames = EnumPacketFormat.GetValues(_categoryTitle).ToArray();
        _categoryToolbarIndex = _categoryNames.Length - 1;
    }
    void RemoveCategory(string categoryName)
    {
        List<string> categoryList = EnumPacketFormat.GetValues(_categoryTitle);
        if (categoryList == null || categoryList.Count < 1)
        {
            UnityHelper.LogError_H($"CategoryWindow RemoveCategory Not Exist Category\ncategoryName : {categoryName}");
            return;
        }

        EnumPacketFormat.RemoveValue(_categoryTitle, categoryName);

        categoryList = EnumPacketFormat.GetValues(_categoryTitle);

        if (categoryList != null && categoryList.Count >= 1)
        {
            _categoryNames = categoryList.ToArray();
        }
    }
    void ChangeCategory(string originName, string nextName)
    {
        if (string.IsNullOrEmpty(nextName))
        {
            UnityHelper.LogError_H($"CategoryWindow ChangeCategory nextName Null Error\nextName : {nextName}");
            return;
        }

        List<string> categoryList = EnumPacketFormat.GetValues(_categoryTitle);
        if (categoryList.Contains(nextName))
        {
            UnityHelper.LogError_H($"CategoryWindow ChangeCategory Already Exist Error\nextName : {nextName}");
            return;
        }

        EnumPacketFormat.ModifyValue(_categoryTitle, originName, nextName);
    }

    string GetTypeEnumName(string categoryName)
    {
        return CSharpHelper.Format_H(_typeTitleFormat, categoryName);
    }
    void AddType(string categoryName, string typeName)
    {
        string enumName = GetTypeEnumName(categoryName);
        bool exist = EnumPacketFormat.Exist(enumName);

        if (!exist)
        {
            EnumPacketFormat.Create(enumName, new List<string>() { typeName });
        }
        else
        {
            EnumPacketFormat.AddValue(enumName, typeName);
        }
    }
    void RemoveType(string categoryName, string typeName)
    {
        string enumName = GetTypeEnumName(categoryName);
        List<string> typeList = EnumPacketFormat.GetValues(enumName);
        if (typeList == null || typeList.Count < 1)
        {
            UnityHelper.LogError_H($"CategoryWindow RemoveType Not Exist Type\nTypeName : {typeName}");
            return;
        }

        EnumPacketFormat.RemoveValue(enumName, typeName);
    }
    void ChangeType(string categoryName, string originName, string nextName)
    {
        string enumName = GetTypeEnumName(categoryName);

        if (string.IsNullOrEmpty(nextName))
        {
            UnityHelper.LogError_H($"CategoryWindow ChangeType nextName Null Error\nnextName : {nextName}");
            return;
        }

        List<string> typeList = EnumPacketFormat.GetValues(enumName);
        if (typeList.Contains(nextName))
        {
            UnityHelper.LogError_H($"CategoryWindow ChangeCategory Already Exist Error\nnextName : {nextName}");
            return;
        }

        EnumPacketFormat.ModifyValue(enumName, originName, nextName);
    }
    void SetSelectType(string categoryName, string typeName)
    {
        if (_selectType.ContainsKey(categoryName))
        {
            _selectType[categoryName] = typeName;
        }
        else
        {
            _selectType.Add(categoryName, typeName);
        }
    }
    string GetSelectType(string categoryName)
    {
        if (_selectType.ContainsKey(categoryName))
        {
            return _selectType[categoryName];
        }
        else
        {
            return "";
        }
    }
}
