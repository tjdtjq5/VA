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

        string targetDir = UnityHelper.GetBuildPath(BuildTarget.Android.ToString());
        Directory.CreateDirectory(targetDir);

        UnityHelper.LogSerialize(SCENES);

        BuildAndroid(targetDir, SCENES, targetDir + "/" + appName);
    }

    private static void BuildAndroid(string targetDir, string[] scenes, string app_target)
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = scenes;
        buildPlayerOptions.locationPathName = app_target;
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = BuildOptions.CompressWithLz4;

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

    public static void PerformBuildIOS()
    {
        BuildOptions opt = BuildOptions.None; 
        PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;
        char sep = Path.DirectorySeparatorChar;
        string BUILD_TARGET_PATH = UnityHelper.GetBuildPath(BuildTarget.iOS.ToString());
        Directory.CreateDirectory(BUILD_TARGET_PATH);
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
        BuildReport res = BuildPipeline.BuildPlayer(scenes, target_path, build_target, build_options);
        if (res == null) { throw new Exception("BuildPlayer failure: " + res); }

        FileHelper.ProcessStart(target_path);
    }
}