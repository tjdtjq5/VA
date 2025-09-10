using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;
#if UNITY_6000_0_OR_NEWER
using UnityEditor.Build.Profile;
using System.Linq;
#endif

namespace CodeStage.EditorCommon.Tools
{
	internal static class CSSceneTools
	{
		public static Scene[] GetOpenedScenes()
		{
			var openScenes = new Scene[SceneManager.sceneCount];
			for (var i = 0; i < SceneManager.sceneCount; i++)
			{
				openScenes[i] = SceneManager.GetSceneAt(i);
			}
			return openScenes;
		}

		public static string[] GetOpenedValidScenesPaths()
		{
			var openedScenes = GetOpenedScenes();
			var scenePaths = new List<string>();
			foreach (var scene in openedScenes)
			{
				if (scene.IsValid())
					scenePaths.Add(scene.path);
			}
			return scenePaths.ToArray();
		}

		public static string[] GetBuildProfilesScenesPaths()
		{
#if UNITY_6000_0_OR_NEWER
			var scenePaths = new HashSet<string>(GetBuildSettingsSceneList());
			var allBuildProfiles = AssetDatabase.FindAssets("t:BuildProfile");
			foreach (var buildProfileGuid in allBuildProfiles)
			{
				var buildProfile = AssetDatabase.LoadAssetAtPath<BuildProfile>(AssetDatabase.GUIDToAssetPath(buildProfileGuid));
				var overrideGlobalSceneListProperty = typeof(BuildProfile).GetProperty("overrideGlobalSceneList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
				var overrideGlobalSceneList = overrideGlobalSceneListProperty != null && (bool)overrideGlobalSceneListProperty.GetValue(buildProfile);
				if (overrideGlobalSceneList)
				{
					foreach (var overridenSceneListScene in buildProfile.scenes)
					{
						scenePaths.Add(overridenSceneListScene.path);
					}
				}
			}
			return scenePaths.ToArray();
#else
			return GetBuildSettingsSceneList();
#endif
		}

		private static string[] GetBuildSettingsSceneList()
		{
			var scenes = EditorBuildSettings.scenes;
            var scenePaths = new string[scenes.Length];
            for (var i = 0; i < scenes.Length; i++)
            	scenePaths[i] = scenes[i].path;
            return scenePaths;
		}
	}
}