using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class AutoBuilder : ScriptableObject
{
    static string[] SCENES = FindEnabledEditorScenes();
    static string APP_NAME = "�� �̸�";
    static string TARGET_DIR;

    [MenuItem("Build/Android")]
    public static void PerformBuildAOS()
    {
        //  PlayerSettings.Android.bundleVersionCode = �������� ���� �ڵ� ����
        // PlayerSettings.bundleVersion = �������� ���� ���� ���� 
        APP_NAME = "Android" + PlayerSettings.Android.bundleVersionCode + ".apk";
        // ����� �ϼŵ� �� ���� ������, ���� �а��� ���ؼ� �̷��� �մϴ�. 
        TARGET_DIR = ProjectPath + "/" + "Build";
        Directory.CreateDirectory(TARGET_DIR); // Ȥ�� ������� ���丮 ������� 
                                               //PlayerSettings.Android.keyaliasName = ȯ�� ������ �����ϱ� ��õ�մϴ�. 
                                               //PlayerSettings.Android.keystoreName = ȯ�� ������ �����ϱ� ��õ�մϴ�. 
                                               // PlayerSettings.Android.keyaliasPass = ȯ�� ������ �����ϱ� ��õ�մϴ�. 
                                               // PlayerSettings.Android.keystorePass = ȯ�� ������ �����ϱ� ��õ�մϴ�. 
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        // �뷫 �������ݴϴ�. ���� �ʿ信 ���缭�� .
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
        // �̳��̴� �������� ��ø�� �� �ֽ��ϴ�. 
        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle; //���� �׷���� �մϴ�. ���ͳη� �Ͻ� ���� �־�� 
        EditorUserBuildSettings.androidBuildType = AndroidBuildType.Release; // �𺧷��� �ʿ��Ͻø� �𺧷����� �Ͻø� �˴ϴ�. 
        BuildAndroid(SCENES, TARGET_DIR + "/" + APP_NAME, BuildTargetGroup.Android, BuildTarget.Android, BuildOptions.CompressWithLz4HC);
        // �̰͵� �ʿ��Ͻø� Ŀ���� ! 
    }

    private static void BuildAndroid(string[] scenes, string app_target, BuildTargetGroup build_target_group, BuildTarget build_target, BuildOptions build_options)
    {
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        //�� ������ �ȵ���̵� �ƴϸ� �ȵ���̵�� �ٲ��ݴϴ�. ���� �ٽ� �����·� ���ƿ��� ������ PostBuild Ȱ���ϼ���
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = scenes;
        buildPlayerOptions.locationPathName = app_target;
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = build_options;
        var report = BuildPipeline.BuildPlayer(buildPlayerOptions);

        FileHelper.ProcessStart(TARGET_DIR);
    }

    private static string[] FindEnabledEditorScenes()
    {
        List<string> EditorScenes = new List<string>();

        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            EditorScenes.Add(scene.path);
        }

        return EditorScenes.ToArray();
    }

    static string ProjectPath
    {
        get { return Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/')); }
    } // ������ �ð� ��� �밭 ���� ���� �ִ� ���� �Լ� 

    [MenuItem("Build/IOS")]
    public static void PerformBuildIOS()
    {

        BuildOptions opt = BuildOptions.None; // �⺻�� cpp
        PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK; // �ùķ����Ϳ��� �����÷��� �ùķ����� sdk �� 
                                                                 //PlayerSettings.bundleVersion = GetArg ("-BUNDLE_VERSION"); //todo
                                                                 //PlayerSettings.iOS.buildNumber = (GetArg ("-VERSION_CODE")); //todo
        char sep = Path.DirectorySeparatorChar;
        string BUILD_TARGET_PATH = ProjectPath + "/ios"; //ios ������ ����ϴ�. 
        Directory.CreateDirectory(BUILD_TARGET_PATH);
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
        try
        {
            BuildIOS(SCENES, BUILD_TARGET_PATH, BuildTarget.iOS, opt);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    static void BuildIOS(string[] scenes, string target_path,
        BuildTarget build_target, BuildOptions build_options)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, build_target);
        BuildReport res = BuildPipeline.BuildPlayer(scenes, target_path, build_target, build_options);
        if (res == null) { throw new Exception("BuildPlayer failure: " + res); }

        FileHelper.ProcessStart(target_path);
    }
}