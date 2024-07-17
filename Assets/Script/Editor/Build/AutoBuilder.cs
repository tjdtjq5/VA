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
    static string APP_NAME = "앱 이름";
    static string TARGET_DIR;

    [MenuItem("Build/Android")]
    public static void PerformBuildAOS()
    {
        //  PlayerSettings.Android.bundleVersionCode = 동적으로 버전 코드 설정
        // PlayerSettings.bundleVersion = 동적으로 번들 버전 설정 
        APP_NAME = "Android" + PlayerSettings.Android.bundleVersionCode + ".apk";
        // 맘대로 하셔도 별 지장 없지만, 저는 분간을 위해서 이렇게 합니다. 
        TARGET_DIR = ProjectPath + "/" + "Build";
        Directory.CreateDirectory(TARGET_DIR); // 혹시 없을까봐 디렉토리 만들어줌 
                                               //PlayerSettings.Android.keyaliasName = 환경 변수로 세팅하길 추천합니다. 
                                               //PlayerSettings.Android.keystoreName = 환경 변수로 세팅하길 추천합니다. 
                                               // PlayerSettings.Android.keyaliasPass = 환경 변수로 세팅하길 추천합니다. 
                                               // PlayerSettings.Android.keystorePass = 환경 변수로 세팅하길 추천합니다. 
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        // 대략 설정해줍니다. 본인 필요에 맞춰서요 .
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
        // 이넘이니 여러개를 중첩할 수 있습니다. 
        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle; //저는 그래들로 합니다. 인터널로 하실 수도 있어요 
        EditorUserBuildSettings.androidBuildType = AndroidBuildType.Release; // 디벨롭이 필요하시면 디벨롭으로 하시면 됩니다. 
        BuildAndroid(SCENES, TARGET_DIR + "/" + APP_NAME, BuildTargetGroup.Android, BuildTarget.Android, BuildOptions.CompressWithLz4HC);
        // 이것도 필요하시면 커스텀 ! 
    }

    private static void BuildAndroid(string[] scenes, string app_target, BuildTargetGroup build_target_group, BuildTarget build_target, BuildOptions build_options)
    {
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        //현 세팅이 안드로이드 아니면 안드로이드로 바꿔줍니다. 만약 다시 원상태로 돌아오고 싶으면 PostBuild 활용하세요
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
    } // 귀찮고 시간 없어서 대강 만들어서 쓰고 있는 중인 함수 

    [MenuItem("Build/IOS")]
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