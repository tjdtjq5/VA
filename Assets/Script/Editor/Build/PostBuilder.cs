#if UNITY_IOS
    using System.IO;
    using UnityEditor;
    using UnityEditor.Build;
    using UnityEditor.iOS.Xcode;
    using UnityEngine;

    class PostBuilder : IPostprocessBuild
    {
        public int callbackOrder { get { return 0; } }
    
        static string ProjectPath
        {
            get { return Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/')); }
        }
        public void OnPostprocessBuild(BuildTarget buildTarget, string pathToBuiltProject)
        {
            // Stop processing if targe is NOT iOS 
            if (buildTarget != BuildTarget.iOS) return;
            // Initialize PbxProject 
            var projectPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
            PBXProject pbxProject = new PBXProject();
            pbxProject.ReadFromFile(projectPath);
            string targetGuid = pbxProject.TargetGuidByName("Unity-iPhone");
            pbxProject.AddCapability(targetGuid, PBXCapabilityType.InAppPurchase);
            pbxProject.AddCapability(targetGuid, PBXCapabilityType.iCloud);
            pbxProject.AddCapability(targetGuid, PBXCapabilityType.GameCenter);
            pbxProject.AddCapability(targetGuid, PBXCapabilityType.PushNotifications);
    //이런 식으로 권한 추가해주면 되는데, 특이하게 푸시나 아이클라우드는 추가로 더 해줘야합니다.  (다음에 나옴 ) 
    
            pbxProject.SetBuildProperty(targetGuid, "DEVELOPMENT_TEAM", "애플 개발자 팀 아이디 ");
            var guid = pbxProject.FindFileGuidByProjectPath("Classes/UI/Keyboard.mm");
            var flags = pbxProject.GetCompileFlagsForFile(targetGuid, guid);
            flags.Add("-fno-objc-arc");
            pbxProject.SetCompileFlagsForFile(targetGuid, guid, flags);
            pbxProject.AddFrameworkToProject(targetGuid, "CloudKit.framework", false);
    
            // Apply settings 
            File.WriteAllText(projectPath, pbxProject.WriteToString());
            // Samlpe of editing Info.plist 
            var plistPath = Path.Combine(pathToBuiltProject, "Info.plist");
            var plist = new PlistDocument();
            plist.ReadFromFile(plistPath);
            // Add string setting 
            plist.root.SetBoolean("ITSAppUsesNonExemptEncryption", false); // 앱이 암호화를 쓰는지 제출하는 건데, 안하면 귀찮게 바로 테스트플라이트가 안올라가고 의미없는 설문을 더 해야하니 . 암호화를 특별나게 쓰고 있지만 않다면 이렇게 합시다. 
            // Add URL Scheme\
            // Apply editing settings to Info.plist
            var cap = plist.root.CreateArray("UIRequiredDeviceCapabilities");
            cap.AddString("gamekit");
            plist.WriteToFile(plistPath);
    
    
            //-----
    
            var file_name = "unity.entitlements"; // 이거없으면 푸시 안됩니다 
            var proj_path = projectPath;
            var proj = new PBXProject();
            proj.ReadFromFile(proj_path);
    
    
            // target_name = "Unity-iPhone"
            var target_name = PBXProject.GetUnityTargetName();
            var target_guid = proj.TargetGuidByName(target_name);
            var dst = pathToBuiltProject + "/" + target_name + "/" + file_name;
            try
            {
                File.WriteAllText(dst, entitlements);
                proj.AddFile(target_name + "/" + file_name, file_name);
                proj.AddBuildProperty(target_guid, "CODE_SIGN_ENTITLEMENTS", target_name + "/" + file_name);
                proj.WriteToFile(proj_path);
            }
            catch (IOException e)
            {
                Debug.Log("Could not copy entitlements. Probably already exists. " + e);
            }
    
            UnityEditor.iOS.Xcode.ProjectCapabilityManager pcm = new UnityEditor.iOS.Xcode.ProjectCapabilityManager(proj_path, dst, target_name);
            pcm.AddPushNotifications(false);
    
        }
    
        private const string entitlements = @"
         <?xml version=""1.0"" encoding=""UTF-8\""?>
         <!DOCTYPE plist PUBLIC ""-//Apple//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
         <plist version=""1.0"">
             <dict>
                 <key>aps-environment</key>
                 <string>production</string>
             </dict>
         </plist>";
    
    }
#endif