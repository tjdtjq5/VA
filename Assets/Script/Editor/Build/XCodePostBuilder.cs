#if UNITY_IOS
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

class XCodePostBuilder
{
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        if (target == BuildTarget.iOS)
        {
            XCode();

            PList();

            PodInit();

            PodInstall();
        }
    }

    private static void XCode()
    {
        string projectPath = GetBuildPath() + "/Unity-iPhone.xcodeproj/project.pbxproj";

        PBXProject pbxProject = new PBXProject();
        pbxProject.ReadFromFile(projectPath);

        string unityTarget = pbxProject.GetUnityFrameworkTargetGuid();
        string mainTarget = pbxProject.GetUnityMainTargetGuid();

        pbxProject.SetBuildProperty(unityTarget, "ENABLE_BITCODE", "NO");

        // StoreKit -> Write -> AddInAppPurchase
        pbxProject.AddFrameworkToProject(unityTarget, "StoreKit.framework", false);
        pbxProject.AddFrameworkToProject(mainTarget, "StoreKit.framework", false);
        pbxProject.WriteToFile(projectPath);

        // apple login Entitlements 
        var manager = new ProjectCapabilityManager(projectPath, "Entitlements.entitlements", null, pbxProject.GetUnityMainTargetGuid());

        // Compatibility
        manager.AddSignInWithApple();
        manager.AddGameCenter();
        manager.AddInAppPurchase();

        manager.WriteToFile();
    }

    private static void PList()
    {
        string infoPlistPath = GetBuildPath() + "/Info.plist";

        PlistDocument plistDoc = new PlistDocument();
        plistDoc.ReadFromFile(infoPlistPath);

        if (plistDoc.root != null)
        {
            //수출 규정 관련 문서 누락
            plistDoc.root.SetBoolean("ITSAppUsesNonExemptEncryption", false);

            // URL Scheme
            // var array = plistDoc.root.CreateArray("CFBundleURLTypes");
            // var urlDict = array.AddDict();
            // urlDict.SetString("CFBundleURLName", PlayerSettings.iPhoneBundleIdentifier);
            // urlDict.SetString("CFBundleTypeRole", "Viewer");
            // varurlInnerArray = urlDict.CreateArray("CFBundleURLSchemes");
            // urlInnerArray.AddString("https");

            plistDoc.WriteToFile(infoPlistPath);
        }
        else
        {
            Debug.LogError("ERROR: Can't open " + infoPlistPath);
        }
    }

    private static void PodInit()
    {
        FileHelper.RunCmd("/bin/bash", GetBuildPath(), "/opt/homebrew/bin/pod init");
    }

    private static void PodInstall()
    {
        FileHelper.RunCmd("/bin/bash", GetBuildPath(), "/opt/homebrew/bin/pod install");
    }

    private static string GetBuildPath()
    {
        return UnityHelper.GetBuildPath(BuildTarget.iOS.ToString());
    }
}
#endif