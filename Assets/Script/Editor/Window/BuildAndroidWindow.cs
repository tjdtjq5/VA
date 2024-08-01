using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildAndroidWindow : EditorWindow
{
    static BuildOptionFile buildOptionFile = new BuildOptionFile();

    static bool _versionFoldout;
    static bool _bundleCodeFoldout;
    static bool _googleBuildFoldout;
    static bool _keyStorePasswordFoldout;
    static bool _serverUrlFoldout;
    static bool _logFoldout;

    static string _title = "Android Build";
    static string _keystorePassward;
    static Vector2 _logScrollPos;
    static bool _isGoogleBuild;

    // Build Option Key 
    static string _versionCodeValueFirst = "VersionCodeValueFirst";
    static string _versionCodeValueSecond = "VersionCodeValueSecond";
    static string _versionCodeValueThird = "VersionCodeValueThird";
    static string _bundleCodeValue = "BundleCodeValue";
    static string _buildLog = "BuildLog";
    static string _keyaliasName = "AliasName";
    static string _keystoreName = "KeystoreName";

    [MenuItem("Build/Android")]
    static void OpenAndroid()
    {
        var window = GetWindow<BuildAndroidWindow>();
        window.titleContent = new GUIContent() { text = _title };
        window.minSize = new Vector2(750, 750);
        window.Show();
    }
    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        {
            TitleGUI();

            EditorGUILayout.Space(4);

            VersionGUI();

            EditorGUILayout.Space(4);

            BundlecCodeGUI();

            EditorGUILayout.Space(4);

            GoogleBuildGUI();

            EditorGUILayout.Space(4);

            KeystorePasswardGUI();

            EditorGUILayout.Space(4);

            ServerURLSetting();

            EditorGUILayout.Space(4);

            BuildLogSetting();

            EditorGUILayout.Space(4);

            BuildGUI();
        }
        EditorGUILayout.EndVertical();
    }

    private void OnEnable()
    {
        _googleBuildFoldout = true;
        _keyStorePasswordFoldout = true;
    }

    void TitleGUI()
    {
        int versionCodeValueFirst = buildOptionFile.Read<int>(_versionCodeValueFirst);
        int versionCodeValueSecond = buildOptionFile.Read<int>(_versionCodeValueSecond);
        int versionCodeValueThird = buildOptionFile.Read<int>(_versionCodeValueThird);
        int bundleCode = buildOptionFile.Read<int>(_bundleCodeValue);

        string title = $"{_title} Version {versionCodeValueFirst}.{versionCodeValueSecond}.{versionCodeValueThird} ({bundleCode})";

        EditorGUILayout.LabelField(title, CustomEditorUtility.titleStyle);
    }

    void VersionGUI()
    {
        _versionFoldout = CustomEditorUtility.DrawFoldoutTitle("Version Setting", _versionFoldout);

        if (_versionFoldout)
        {
            EditorGUILayout.Space(4);

            EditorGUILayout.BeginHorizontal();
            {
                int versionCodeValueFirst = buildOptionFile.Read<int>(_versionCodeValueFirst);
                int versionCodeValueSecond = buildOptionFile.Read<int>(_versionCodeValueSecond);
                int versionCodeValueThird = buildOptionFile.Read<int>(_versionCodeValueThird);

                if (GUILayout.Button("<", GUILayout.Width(50), GUILayout.Height(35)))
                {
                    if (versionCodeValueFirst <= 0)
                        versionCodeValueFirst = 0;
                    else
                        versionCodeValueFirst--;

                    buildOptionFile.Add(_versionCodeValueFirst, versionCodeValueFirst);
                }

                EditorGUILayout.LabelField(versionCodeValueFirst.ToString(), CustomEditorUtility.GetMiddleLabel, GUILayout.Width(50), GUILayout.Height(35));

                if (GUILayout.Button(">", GUILayout.Width(50), GUILayout.Height(35)))
                {
                    versionCodeValueFirst++;
                    buildOptionFile.Add(_versionCodeValueFirst, versionCodeValueFirst);
                }

                EditorGUILayout.Space(4);

                if (GUILayout.Button("<", GUILayout.Width(50), GUILayout.Height(35)))
                {
                    if (versionCodeValueSecond <= 0)
                        versionCodeValueSecond = 0;
                    else
                        versionCodeValueSecond--;

                    buildOptionFile.Add(_versionCodeValueSecond, versionCodeValueSecond);
                }

                EditorGUILayout.LabelField(versionCodeValueSecond.ToString(), CustomEditorUtility.GetMiddleLabel, GUILayout.Width(50), GUILayout.Height(35));

                if (GUILayout.Button(">", GUILayout.Width(50), GUILayout.Height(35)))
                {
                    versionCodeValueSecond++;
                    buildOptionFile.Add(_versionCodeValueSecond, versionCodeValueSecond);
                }

                EditorGUILayout.Space(4);

                if (GUILayout.Button("<", GUILayout.Width(50), GUILayout.Height(35)))
                {
                    if (versionCodeValueThird <= 0)
                        versionCodeValueThird = 0;
                    else
                        versionCodeValueThird--;

                    buildOptionFile.Add(_versionCodeValueThird, versionCodeValueThird);
                }

                EditorGUILayout.LabelField(versionCodeValueThird.ToString(), CustomEditorUtility.GetMiddleLabel, GUILayout.Width(50), GUILayout.Height(35));

                if (GUILayout.Button(">", GUILayout.Width(50), GUILayout.Height(35)))
                {
                    versionCodeValueThird++;
                    buildOptionFile.Add(_versionCodeValueThird, versionCodeValueThird);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    void BundlecCodeGUI()
    {
        _bundleCodeFoldout = CustomEditorUtility.DrawFoldoutTitle("BundlecCode Setting", _bundleCodeFoldout);

        if (_bundleCodeFoldout)
        {
            EditorGUILayout.Space(4);
            EditorGUILayout.BeginHorizontal();
            {
                int bundleCode = buildOptionFile.Read<int>(_bundleCodeValue);

                if (GUILayout.Button("<", CustomEditorUtility.GetButtonStyle(50, 35, AnchorStyles.Left)))
                {
                    if (bundleCode <= 1)
                        bundleCode = 1;
                    else
                        bundleCode--;

                    buildOptionFile.Add(_bundleCodeValue, bundleCode);
                }
                GUILayout.FlexibleSpace();

                EditorGUILayout.LabelField(bundleCode.ToString(), CustomEditorUtility.GetMiddleLabel, GUILayout.Width(50), GUILayout.Height(35));

                GUILayout.FlexibleSpace();

                if (GUILayout.Button(">", CustomEditorUtility.GetButtonStyle(50, 35, AnchorStyles.Right)))
                {
                    bundleCode++;
                    buildOptionFile.Add(_bundleCodeValue, bundleCode);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    void GoogleBuildGUI()
    {
        _googleBuildFoldout = CustomEditorUtility.DrawFoldoutTitle("AAB", _googleBuildFoldout);

        if (_googleBuildFoldout)
        {
            EditorGUILayout.Space(4);

            EditorGUILayout.BeginHorizontal();
            {
                _isGoogleBuild = EditorGUILayout.ToggleLeft("GooglePlayBuild", _isGoogleBuild);
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    void KeystorePasswardGUI()
    {
        _keyStorePasswordFoldout = CustomEditorUtility.DrawFoldoutTitle("KeystorePassward Setting", _keyStorePasswordFoldout);

        if (_keyStorePasswordFoldout) 
        {
            EditorGUILayout.Space(4);

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Passward : ");

                EditorGUILayout.Space(4);

                _keystorePassward = EditorGUILayout.PasswordField(_keystorePassward, EditorStyles.textArea);
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    void ServerURLSetting()
    {
        _serverUrlFoldout = CustomEditorUtility.DrawFoldoutTitle("Server Url Setting", _serverUrlFoldout);

        if (_serverUrlFoldout)
        {
            EditorGUILayout.Space(4);

            ServerUrlType selectType = GameOptionManager.ServerUrlType;
            int len = Enum.GetValues(typeof(ServerUrlType)).Length;

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
    }

    void BuildLogSetting()
    {
        _logFoldout = CustomEditorUtility.DrawFoldoutTitle("Log", _logFoldout);

        if (_logFoldout)
        {
            EditorGUILayout.Space(4);

            BuildLogDatas BuildLogDatas = buildOptionFile.Read<BuildLogDatas>(_buildLog);
            string logData = "";
            if (BuildLogDatas != null)
            {
                List<BuildLogData> datas = BuildLogDatas.Datas;

                for (int i = 0; i < datas.Count; i++)
                {
                    logData += $"{datas[i].ToString()}\n\n";
                }
                logData = logData.Substring(0, logData.Length - 1);
            }

            EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true));
            {
                _logScrollPos = EditorGUILayout.BeginScrollView(_logScrollPos, GUILayout.MaxHeight(500));
                {
                    EditorGUILayout.LabelField(logData, CustomEditorUtility.GetLabelStyle(18), GUILayout.Width(CustomEditorUtility.GetScreenWidth), GUILayout.ExpandHeight(true));
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }
    }

    void BuildGUI()
    {
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Build", GUILayout.Height(40)))
        {
            int versionCodeValueFirst = buildOptionFile.Read<int>(_versionCodeValueFirst);
            int versionCodeValueSecond = buildOptionFile.Read<int>(_versionCodeValueSecond);
            int versionCodeValueThird = buildOptionFile.Read<int>(_versionCodeValueThird);

            string version = UnityHelper.GetVersionCode(versionCodeValueFirst, versionCodeValueSecond, versionCodeValueThird);
            int bundleCode = buildOptionFile.Read<int>(_bundleCodeValue);

            PlayerSettings.bundleVersion = version;
            PlayerSettings.Android.bundleVersionCode = bundleCode;

            PlayerSettings.Android.keyaliasName = buildOptionFile.Read(_keyaliasName).Replace("\"", "");
            PlayerSettings.Android.keystoreName = buildOptionFile.Read(_keystoreName).Replace("\"","");
            PlayerSettings.Android.keyaliasPass = _keystorePassward;
            PlayerSettings.Android.keystorePass = _keystorePassward;

            AutoBuilder.PerformBuildAOS();
        }
    }
}
