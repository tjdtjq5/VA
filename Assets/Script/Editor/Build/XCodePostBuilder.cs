#if UNITY_IOS
using AppleAuth.Editor;
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
            //{
            //    PlayerSettings.iOS.iOSUrlSchemes = new string[] { "slitz95", "https", "http" };
            //}

            // xcode
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
                manager.AddSignInWithAppleWithCompatibility(pbxProject.GetUnityFrameworkTargetGuid());
                manager.AddGameCenter();
                manager.AddInAppPurchase();
                manager.AddAssociatedDomains(new string[] { $"applinks:slits95.com", $"applinks:www.slits95.com" });

                manager.WriteToFile();

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

                    // URL Scheme
                    var array = plistDoc.root.CreateArray("CFBundleURLTypes");

                    var urlDict = array.AddDict();
                    urlDict.SetString("CFBundleURLName", null);
                    urlDict.SetString("CFBundleTypeRole", "Viewer");
                    var urlInnerArray = urlDict.CreateArray("CFBundleURLSchemes");
                    urlInnerArray.AddString("slitz95");

                    urlDict = array.AddDict();
                    urlDict.SetString("CFBundleURLName", PlayerSettings.iPhoneBundleIdentifier);
                    urlDict.SetString("CFBundleTypeRole", "Viewer");
                    urlInnerArray = urlDict.CreateArray("CFBundleURLSchemes");
                    urlInnerArray.AddString("https");

                    urlDict = array.AddDict();
                    urlDict.SetString("CFBundleURLName", PlayerSettings.iPhoneBundleIdentifier);
                    urlDict.SetString("CFBundleTypeRole", "Viewer");
                    urlInnerArray = urlDict.CreateArray("CFBundleURLSchemes");
                    urlInnerArray.AddString("http");
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