using UnityEditor.Callbacks;
using UnityEditor;
using System.Collections.Generic;

public class BuildLog
{
    static BuildOptionFile buildOptionFile = new BuildOptionFile();
    static string _buildLog = "BuildLog";

    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        BuildLogDatas BuildLogDatas = buildOptionFile.Read<BuildLogDatas>(_buildLog);

        string title = $"{target}";

        if (target == BuildTarget.Android)
        {
            if (EditorUserBuildSettings.buildAppBundle)
                title += $".aab";
            else
                title += $".apk";
        }
        else if (target == BuildTarget.iOS)
            title += $".xcode";

        BuildLogDatas.Datas.Add(new BuildLogData(title, PlayerSettings.bundleVersion, PlayerSettings.Android.bundleVersionCode, GameOptionManager.ServerUrlType));
        buildOptionFile.Add(_buildLog, BuildLogDatas);
    }
}
