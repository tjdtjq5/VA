using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class AutoBuilder
{
    static string[] SCENES = FindEnabledEditorScenes();

    public static void PerformBuildAOS()
    {
        string appName = "Android_" + PlayerSettings.Android.bundleVersionCode;
        if (EditorUserBuildSettings.buildAppBundle)
            appName += ".aab";
        else
            appName += ".apk";

        string targetDir = ProjectPath + "/" + "Build";
        Directory.CreateDirectory(targetDir);

        BuildAndroid(targetDir, SCENES, targetDir + "/" + appName);
    }

    private static void BuildAndroid(string targetDir, string[] scenes, string app_target)
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = scenes;
        buildPlayerOptions.locationPathName = app_target;
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = BuildOptions.None;

        var report = BuildPipeline.BuildPlayer(buildPlayerOptions);

        if (report.summary.result == BuildResult.Succeeded)
        {
            FileHelper.ProcessStart(targetDir);
        }
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
    }

    private static string GetArg(string name)
    {
        var args = System.Environment.GetCommandLineArgs();

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == name && args.Length > i + 1)
            {
                return args[i + 1];
            }
        }

        return null;
    }

    public static void PerformBuildIOS()
    {
        BuildOptions opt = BuildOptions.None; // 기본이 cpp
        PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK; // 시뮬레이터에서 돌리시려면 시뮬레이터 sdk 로 
                                                                 //PlayerSettings.bundleVersion = GetArg ("-BUNDLE_VERSION"); //todo
                                                                 //PlayerSettings.iOS.buildNumber = (GetArg ("-VERSION_CODE")); //todo
        char sep = Path.DirectorySeparatorChar;
        string BUILD_TARGET_PATH = ProjectPath + "/ios"; //ios 폴더로 뱉습니다. 
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