

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEngine;

public class SettingOptionWindow : EditorWindow
{
    static OptionFile optionFile = new OptionFile();
    static SecretOptionFile secretOptionFile = new SecretOptionFile();
    static Vector2 _windowSize;

    [MenuItem("Tool/Setting %q")]
    static void Open()
    {
        var window = GetWindow<SettingOptionWindow>();
        window.titleContent = new GUIContent() { text = "Setting" };
        window.minSize = _windowSize;
        window.Show();
    }

    bool _isExpandedOption;
    bool _isExpandedSecret;

    bool _linkedOptionFile;
    bool _linkedSecretFile;

    string _inputKey;
    string _inputValue;

    Dictionary<string, bool> _optionModifyFlag = new Dictionary<string, bool>();
    Dictionary<string, bool> _secretModifyFlag = new Dictionary<string, bool>();
         
    private void OnEnable()
    {
        _windowSize = new Vector2(600, 700);

        _linkedOptionFile = optionFile.Exist();
        _linkedSecretFile = secretOptionFile.Exist();

        _optionModifyFlag.Clear();
        _secretModifyFlag.Clear();
    }
    private void OnGUI()
    {
        EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
        {
            _isExpandedOption = CustomEditorUtility.DrawFoldoutTitle("OptionSetting", _isExpandedOption);

            if (_isExpandedOption)
            {
                OptionSetting();
            }

            EditorGUILayout.Space();

            _isExpandedSecret = CustomEditorUtility.DrawFoldoutTitle("SecretSetting", _isExpandedSecret);

            if (_isExpandedSecret) 
            {
                SecretSetting();
            }

            EditorGUILayout.Space();

            ServerURLSetting();
        }
        EditorGUILayout.EndVertical();
    }

    void OptionSetting()
    {
        // Path ?????? ????
        if (!_linkedOptionFile)
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.PrefixLabel($"Need Link Path : Option.txt");
            }
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.LabelField("Success Linked Option Path!");

                EditorGUILayout.BeginHorizontal();
                {
                    Color originColor = GUI.contentColor;

                    GUI.contentColor = Color.cyan;
                    EditorGUILayout.SelectableLabel(optionFile.FileName);

                    GUI.contentColor = originColor;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                List<string> optionKeys = optionFile.Keys();
                if (optionKeys != null)
                {
                    for (int i = 0; i < optionKeys.Count; i++)
                    {
                        string key = optionKeys[i];
                        object value = optionFile.Read<object>(key);
                        string valueStr = CSharpHelper.SerializeObject(value);
                        valueStr = RemoveSemi(valueStr);

                        bool isModify = _optionModifyFlag.ContainsKey(key) ? _optionModifyFlag[key] : false;

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

                                    SwitchOptionModifyFlag(key, true);
                                }

                                if (GUILayout.Button("P", GUILayout.Width(35)))
                                {
                                    string filePath = FileHelper.SelectFilePath(FileHelper.GetCurrentDirectory());
                                    if (!string.IsNullOrEmpty(filePath))
                                    {
                                        _inputValue = filePath;
                                        _inputValue = RemoveSemi(_inputValue);

                                        ChangeOption(key, valueStr);
                                    }
                                }
                            }
                            else
                            {
                                _inputKey = EditorGUILayout.TextArea(_inputKey, GUILayout.Width(150));
                                _inputValue = EditorGUILayout.TextArea(_inputValue);

                                if (GUILayout.Button("M", GUILayout.Width(35)))
                                {
                                    _inputValue = RemoveSemi(_inputValue);

                                    ChangeOption(key, valueStr);

                                    SwitchOptionModifyFlag(key, false);
                                }
                            }
                        }
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.Space();
                    }
                }

                if (GUILayout.Button("+"))
                {
                    int index = optionKeys != null ? optionKeys.Count : 0;
                    string key = $"NewOption_{index}";
                    optionFile.Add(key, $"Value");
                }
            }
            EditorGUILayout.EndVertical();
        }
    }
    void SecretSetting()
    {
        // Path ?????? ????
        if (!_linkedSecretFile)
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.PrefixLabel($"Need Link Path : Secret.txt");
            }
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.LabelField("Success Linked Secret Path!");

                EditorGUILayout.BeginHorizontal();
                {
                    Color originColor = GUI.contentColor;

                    GUI.contentColor = Color.cyan;
                    EditorGUILayout.SelectableLabel(secretOptionFile.FileName);

                    GUI.contentColor = originColor;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                List<string> secretKeys = secretOptionFile.Keys();
                if (secretKeys != null)
                {
                    for (int i = 0; i < secretKeys.Count; i++)
                    {
                        string key = secretKeys[i];
                        object value = secretOptionFile.Read<object>(key);
                        string valueStr = CSharpHelper.SerializeObject(value);
                        valueStr = RemoveSemi(valueStr);

                        bool isModify = _secretModifyFlag.ContainsKey(key) ? _secretModifyFlag[key] : false;

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

                                    SwitchSecretModifyFlag(key, true);
                                }
                                if (GUILayout.Button("P", GUILayout.Width(35)))
                                {
                                    string filePath = FileHelper.SelectFilePath(FileHelper.GetCurrentDirectory());
                                    if (!string.IsNullOrEmpty(filePath))
                                    {
                                        _inputValue = filePath;
                                        _inputValue = RemoveSemi(_inputValue);
                                        ChangeSecretOption(key, valueStr);
                                    }
                                }
                            }
                            else
                            {
                                _inputKey = EditorGUILayout.TextArea(_inputKey, GUILayout.Width(150));
                                _inputValue = EditorGUILayout.TextArea(_inputValue);

                                if (GUILayout.Button("M", GUILayout.Width(35)))
                                {
                                    _inputValue = RemoveSemi(_inputValue);

                                    ChangeSecretOption(key, valueStr);

                                    SwitchSecretModifyFlag(key, false);
                                }
                            }
                        }
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.Space();
                    }
                }

                if (GUILayout.Button("+"))
                {
                    int index = secretKeys != null ? secretKeys.Count : 0;
                    string key = $"NewOption_{index}";
                    secretOptionFile.Add(key, $"Value");
                }
            }
            EditorGUILayout.EndVertical();
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
                    ProjectSettingsEditor.IosUrlSchma(new string[] { "https" });
                }
            }
        }
        EditorGUILayout.EndHorizontal();

        GUI.backgroundColor = originColor;
    }

    void SwitchOptionModifyFlag(string key, bool flag)
    {
        if (flag)
        {
            _optionModifyFlag.Clear();
            _optionModifyFlag.Add(key, flag);
        }
        else
        {
            _optionModifyFlag.Clear();
        }
    }
    void SwitchSecretModifyFlag(string key, bool flag)
    {
        if (flag)
        {
            _secretModifyFlag.Clear();
            _secretModifyFlag.Add(key, flag);
        }
        else
        {
            _secretModifyFlag.Clear();
        }
    }

    string RemoveSemi(string path)
    {
        string result = path;
        result = result.Replace("\\\\\\", "");
        result = result.Replace("\\\\", "\\");
        result = result.Replace("\"", "");

        return result;
    }

    void ChangeOption(string originKey, string originValue)
    {
        if (originKey != _inputKey)
        {
            if (string.IsNullOrEmpty(_inputKey))
            {
                optionFile.Remove(originKey);
            }
            else
            {
                optionFile.Remove(originKey);
                optionFile.Add(_inputKey, CSharpHelper.AutoParse(_inputValue));
            }
        }
        else
        {
            if (_inputValue != originValue)
            {
                optionFile.Remove(originKey);
                optionFile.Add(_inputKey, CSharpHelper.AutoParse(_inputValue));
            }
        }
    }
    void ChangeSecretOption(string originKey, string originValue)
    {
        if (originKey != _inputKey)
        {
            if (string.IsNullOrEmpty(_inputKey))
            {
                secretOptionFile.Remove(originKey);
            }
            else
            {
                secretOptionFile.Remove(originKey);
                secretOptionFile.Add(_inputKey, CSharpHelper.AutoParse(_inputValue));
            }
        }
        else
        {
            if (_inputValue != originValue)
            {
                secretOptionFile.Remove(originKey);
                secretOptionFile.Add(_inputKey, CSharpHelper.AutoParse(_inputValue));
            }
        }
    }
}
