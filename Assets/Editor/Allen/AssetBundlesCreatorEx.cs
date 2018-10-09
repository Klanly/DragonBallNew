// Builds an asset bundle from the selected objects in the project view,
// and changes the texture format using an AssetPostprocessor.
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace UEditor {
	public class AssetBundlesCreatorEx : EditorWindow {
		
		public static AssetBundlesCreatorEx window;
		public static UnityEditor.BuildTarget buildTarget = BuildTarget.StandaloneWindows;
		
		//        [MenuItem("AssetBundle Tools/AssetBundle/AssetBundle For Windows32", false, 1)]
		public static void ExecuteWindows32()
		{
			buildTarget = UnityEditor.BuildTarget.StandaloneWindows;
			if(CheckPlatform(buildTarget)){
				checkWindow();
				window.Show();
			}
		}
		
		//        [MenuItem("AssetBundle Tools/AssetBundle/AssetBundle For IPhone", false, 2)]
		public static void ExecuteIPhone()
		{
			buildTarget = UnityEditor.BuildTarget.iPhone;
			if(CheckPlatform(buildTarget)){
				checkWindow();
				window.Show();
			}
		}
		
		//        [MenuItem("AssetBundle Tools/AssetBundle/AssetBundle For Mac", false, 3)]
		public static void ExecuteMac()
		{
			buildTarget = UnityEditor.BuildTarget.StandaloneOSXIntel;
			if(CheckPlatform(buildTarget)) {
				checkWindow();
				window.Show();
			}
		}
		
		//        [MenuItem("AssetBundle Tools/AssetBundle/AssetBundle For Android", false, 4)]
		public static void ExecuteAndroid()
		{
			buildTarget = UnityEditor.BuildTarget.Android;
			if(CheckPlatform(buildTarget)) {
				checkWindow();
				window.Show();
			}
		}
		
		//        [MenuItem("AssetBundle Tools/AssetBundle/AssetBundle For WebPlayer", false, 5)]
		public static void ExecuteWebPlayer()
		{
			buildTarget = UnityEditor.BuildTarget.WebPlayer;
			if(CheckPlatform(buildTarget)) {
				checkWindow();
				window.Show();
			}
		}
		
		//        [MenuItem("AssetBundle Tools/AssetBundle/AssetBundle For WP", false, 6)]
		public static void ExecuteWinPhone() {
			buildTarget = UnityEditor.BuildTarget.WP8Player;
			if(CheckPlatform(buildTarget)) {
				checkWindow();
				window.Show();
			}
		}
		
		static void checkWindow() {
			if (window == null)
			{
				window = (AssetBundlesCreatorEx)GetWindow(typeof(AssetBundlesCreatorEx));
			}
		}
		
		void OnGUI()
		{
			if (GUI.Button(new Rect(10f, 10f, 200f, 50f), "Step(1) : Create AssetBundle"))
			{
				CreateAssetBundle.Execute(buildTarget);
				EditorUtility.DisplayDialog("", "Step (1) Completed", "OK");
			}
			
			if (GUI.Button(new Rect(10f, 80f, 200f, 50f), "Step(2) : Generate MD5"))
			{
				CreateMD5List.Execute(buildTarget);
				EditorUtility.DisplayDialog("", "Step (2) Completed", "OK");
			}
			
			if (GUI.Button(new Rect(10f, 150f, 200f, 50f), "Step(3) : Compare MD5"))
			{
				CampareMD5ToGenerateVersionNum.Execute(buildTarget);
				EditorUtility.DisplayDialog("", "Step (3) Completed", "OK");
			}
			
			if (GUI.Button(new Rect(10f, 220f, 200f, 50f), "Create in One Step"))
			{
				CreateAssetBundle.Execute(buildTarget);
				CreateMD5List.Execute(buildTarget);
				CampareMD5ToGenerateVersionNum.Execute(buildTarget);
				EditorUtility.DisplayDialog("", "Completed", "OK");
			}
		}
		
		public static string GetPlatformPath(UnityEditor.BuildTarget target)
		{
			string SavePath = string.Empty;
			switch (target)
			{
			case BuildTarget.StandaloneWindows:
				SavePath = Path.Combine(DeviceInfo.PersistRootPath, "AssetBundle/Windows32/");
				break;
			case BuildTarget.StandaloneWindows64:
				SavePath = Path.Combine(DeviceInfo.PersistRootPath, "AssetBundle/Windows64/");
				break;
			case BuildTarget.iPhone:
				SavePath = Path.Combine(DeviceInfo.PersistRootPath, "AssetBundle/IOS/");
				break;
			case BuildTarget.WP8Player:
				SavePath = Path.Combine(DeviceInfo.PersistRootPath, "AssetBundle/WP/");
				break;
			case BuildTarget.StandaloneOSXUniversal: case BuildTarget.StandaloneOSXIntel:
				SavePath = Path.Combine(DeviceInfo.PersistRootPath, "AssetBundle/Mac/");
				break;
			case BuildTarget.Android:
				SavePath = Path.Combine(DeviceInfo.PersistRootPath, "AssetBundle/Android/");
				break;
			case BuildTarget.WebPlayer:
				SavePath = Path.Combine(DeviceInfo.PersistRootPath, "AssetBundle/WebPlayer/");
				break;
			default:
				SavePath = Path.Combine(DeviceInfo.PersistRootPath, "AssetBundle/");
				break;
			}
			
			if (!Directory.Exists(SavePath))
				Directory.CreateDirectory(SavePath);
			
			return SavePath;
		}
		
		public static string GetPlatformName(UnityEditor.BuildTarget target)
		{
			string platform = "Windows32";
			switch (target)
			{
			case BuildTarget.StandaloneWindows:
				platform = "Windows32";
				break;
			case BuildTarget.StandaloneWindows64:
				platform = "Windows64";
				break;
			case BuildTarget.iPhone:
				platform = "IOS";
				break;
			case BuildTarget.StandaloneOSXIntel:
				platform = "Mac";
				break;
			case BuildTarget.StandaloneOSXUniversal:
				platform = "Mac";
				break;
			case BuildTarget.WP8Player:
				platform = "WP";
				break;
			case BuildTarget.Android:
				platform = "Android";
				break;
			case BuildTarget.WebPlayer:
				platform = "WebPlayer";
				break;
			default:
				break;
			}
			return platform;
		}
		
		public static bool CheckPlatform(UnityEditor.BuildTarget target)
		{
			if (EditorUserBuildSettings.activeBuildTarget != target)
			{
				EditorUtility.DisplayDialog("Target platform isn't the same as Active platform.\nPlease switch to the target platform firstly.", 
				                            "Current platform : " + EditorUserBuildSettings.activeBuildTarget + "\nTarget platform : " + target.ToString(), "OK");
				return false;
			}
			return true;
		}
	}
}