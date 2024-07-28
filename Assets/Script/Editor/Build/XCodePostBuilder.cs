#if UNITY_IOS
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

class XCodePostBuilder
{
    static SecretOptionFile secretFileTxt = new SecretOptionFile();

    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        if (target == BuildTarget.iOS)
        {
            // PlayerSettings
            {
                PlayerSettings.iOS.iOSUrlSchemes = new string[] { "https" };
            }

            // xcode
            {
                string projectPath = GetBuildPath() + "/Unity-iPhone.xcodeproj/project.pbxproj";

                PBXProject pbxProject = new PBXProject();
                pbxProject.ReadFromFile(projectPath);

                string pbxTarget = pbxProject.GetUnityFrameworkTargetGuid();
                pbxProject.SetBuildProperty(pbxTarget, "ENABLE_BITCODE", "NO");

                pbxProject.WriteToFile(projectPath);
            }

            // info.plist
            {
                string infoPlistPath = GetBuildPath() + "/Info.plist";

                PlistDocument plistDoc = new PlistDocument();
                plistDoc.ReadFromFile(infoPlistPath);
                if (plistDoc.root != null)
                {
                    //수출 규정 관련 문서 누락
                    plistDoc.root.SetBoolean("ITSAppUsesNonExemptEncryption", false);
                    plistDoc.WriteToFile(infoPlistPath);
                }
                else
                {
                    Debug.LogError("ERROR: Can't open " + infoPlistPath);
                }
            }
        }
    }

    private static string GetBuildPath()
    {
        return secretFileTxt.Read<string>("IOSBuildPath");
    }
}
#endif