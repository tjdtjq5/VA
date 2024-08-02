

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEngine;

public class SettingOptionWindow : EditorWindow
{
    static OptionFile optionFile = new OptionFile();
    static SecretOptionFile secretOptionFile = new SecretOptionFile();
    static BuildOptionFile buildFile = new BuildOptionFile();

    static IFileTxt[] _txtFiles;
    static Vector2 _windowSize;

    [MenuItem("Tool/Setting %q")]
    static void Open()
    {
        var window = GetWindow<SettingOptionWindow>();
        window.titleContent = new GUIContent() { text = "Setting" };
        window.minSize = _windowSize;
        window.Show();
    }

    Vector2 _scrollPos;
    float _scrollMinusHeight;

    bool[] _isExpanded;

    bool[] _isLinked;

    string _inputKey;
    string _inputValue;

    Dictionary<string, bool>[] _modifyFlag;
         
    private void OnEnable()
    {
        Init();

        _windowSize = new Vector2(600, 700);

        _scrollMinusHeight = 250;
    }
    private void OnGUI()
    {
        EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
        {
            for (int i = 0; i < _txtFiles.Length; i++)
            {
                string fileName = _txtFiles[i].FileName;
                _isExpanded[i] = CustomEditorUtility.DrawFoldoutTitle($"{fileName} Setting", _isExpanded[i]);

                if (_isExpanded[i])
                {
                    TxtFilesSetting(i);
                }

                EditorGUILayout.Space();
            }

            ServerURLSetting();
        }
        EditorGUILayout.EndVertical();
    }

    void Init()
    {
        _txtFiles = new IFileTxt[] { optionFile, secretOptionFile, buildFile };
        _isExpanded = new bool[_txtFiles.Length];
        _isLinked = new bool[_txtFiles.Length];
        _modifyFlag = new Dictionary<string, bool>[_txtFiles.Length];

        for (int i = 0; i < _txtFiles.Length; i++)
        {
            _isLinked[i] = _txtFiles[i].Exist();
        }

        for (int i = 0; i < _modifyFlag.Length; i++)
        {
            _modifyFlag[i] = new Dictionary<string, bool>();
            _modifyFlag[i].Clear();
        }
    }
    void TxtFilesSetting(int i)
    {
        string fileName = _txtFiles[i].FileName;
        bool isLinked = _isLinked[i];
        Dictionary<string, bool> modifyFlag = _modifyFlag[i];

        if (!isLinked)
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.PrefixLabel($"Need Link Path : {fileName}");
            }
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.Height(CustomEditorUtility.GetScreenHeight - _scrollMinusHeight));
            {
                EditorGUILayout.LabelField($"Success Linked {fileName} Path!");

                EditorGUILayout.BeginHorizontal();
                {
                    Color originColor = GUI.contentColor;

                    GUI.contentColor = Color.cyan;
                    EditorGUILayout.SelectableLabel(fileName);

                    GUI.contentColor = originColor;

                    if (GUILayout.Button("Link"))
                    {
                        string path = _txtFiles[i].GetFile();
                        FileHelper.ProcessStart(path);
                    }
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                List<string> keys = _txtFiles[i].Keys();
                if (keys != null)
                {
                    for (int j = 0; j < keys.Count; j++)
                    {
                        string key = keys[j];
                        object value = _txtFiles[i].Read<object>(key);
                        string valueStr = CSharpHelper.SerializeObject(value);
                        valueStr = CSharpHelper.RemoveSemi(valueStr);

                        bool isModify = modifyFlag.ContainsKey(key) ? modifyFlag[key] : false;

                        EditorGUILayout.BeginHorizontal();
                        {
                            if (!isModify)
                            {
                                EditorGUILayout.LabelField(key, EditorStyles.helpBox, GUILayout.Width(150));
                                EditorGUILayout.LabelField(valueStr, GUILayout.Width(CustomEditorUtility.GetScreenWidth - 400));

                                if (GUILayout.Button("M", GUILayout.Width(35)))
                                {
                                    _inputKey = key;
                                    _inputValue = valueStr;

                                    SwitchModifyFlag(i, key, true);
                                }

                                if (GUILayout.Button("P", GUILayout.Width(35)))
                                {
                                    string filePath = FileHelper.SelectFilePath(FileHelper.GetCurrentDirectory());
                                    if (!string.IsNullOrEmpty(filePath))
                                    {
                                        _inputValue = filePath;
                                        _inputValue = CSharpHelper.RemoveSemi(_inputValue);

                                        ChangeData(i, key, valueStr);
                                    }
                                }
                            }
                            else
                            {
                                _inputKey = EditorGUILayout.TextArea(_inputKey, GUILayout.Width(150));
                                _inputValue = EditorGUILayout.TextArea(_inputValue);

                                if (GUILayout.Button("M", GUILayout.Width(35)))
                                {
                                    _inputValue = CSharpHelper.RemoveSemi(_inputValue);

                                    ChangeData(i, key, valueStr);

                                    SwitchModifyFlag(i, key, false);
                                }
                            }
                        }
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.Space();
                    }
                }

                if (GUILayout.Button("+"))
                {
                    int index = keys != null ? keys.Count : 0;
                    string key = $"NewOption_{index}";
                    _txtFiles[i].Add(key, $"Value");
                }
            }
            EditorGUILayout.EndScrollView();
        }
    }
    void ServerURLSetting()
    {
        ServerUrlType selectType = GameOptionManager.ServerUrlType;
        int len = Enum.GetValues(typeof(ServerUrlType)).Length;

        EditorGUILayout.LabelField("Server Url", CustomEditorUtility.GetMiddleLabel);

        Color originColor = GUI.backgroundColor;

        EditorGUILayout.BeginHorizontal();
        {
            for (int i = 0; i < len; i++) 
            {
                ServerUrlType cType = (ServerUrlType)i;
                bool isSelect = cType == selectType;
                GUI.backgroundColor = isSelect ? Color.cyan : Color.white;

                if (GUILayout.Button($"{cType.ToString()}"))
                {
                    GameOptionManager.ChangeServerUrl(cType);
                }
            }
        }
        EditorGUILayout.EndHorizontal();

        GUI.backgroundColor = originColor;
    }

    void SwitchModifyFlag(int index, string key, bool flag)
    {
        if (flag)
        {
            _modifyFlag[index].Clear();
            _modifyFlag[index].Add(key, flag);
        }
        else
        {
            _modifyFlag[index].Clear();
        }
    }

    void ChangeData(int index, string originKey, string originValue)
    {
        if (originKey != _inputKey)
        {
            if (string.IsNullOrEmpty(_inputKey))
            {
                _txtFiles[index].Remove(originKey);
            }
            else
            {
                _txtFiles[index].Remove(originKey);
                _txtFiles[index].Add(_inputKey, CSharpHelper.AutoParse(_inputValue));
            }
        }
        else
        {
            if (_inputValue != originValue)
            {
                _txtFiles[index].Remove(originKey);
                _txtFiles[index].Add(_inputKey, CSharpHelper.AutoParse(_inputValue));
            }
        }
    }
}
