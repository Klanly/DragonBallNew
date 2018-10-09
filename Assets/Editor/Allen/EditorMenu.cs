using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UEditor;

public class EditorMenu : EditorWindow {
	public static EditorMenu mMenu;


    [MenuItem("AssetBundle Tools/CleanCache/CleanCache")]
    public static void CleanCache() {
        EditorRemoveModel.CleanCache();
    }

    [MenuItem("AssetBundle Tools/CleanCache/MoveModels")]
    public static void RemoveModel() {
		DeleteCG3D();
		DeleteCG2D();
        EditorRemoveModel.RemoveModel();
	}

    [MenuItem("AssetBundle Tools/Export")]
    public static void Export() {
      if(mMenu == null){
          mMenu = GetWindow(typeof(EditorMenu)) as EditorMenu;
      }
      mMenu.Show();
    }

	[MenuItem("AssetBundle Tools/BaleZIP")]
	public static void BaleZIP(){
		AssetBundlesZIP.BaleZIP();
	}

	[MenuItem("AssetBundle Tools/ExportFiles")]
	public static void ExportFiles() {
		Dictionary<string, Dictionary<string, List<string>>> bundlesToBuild = new Dictionary<string, Dictionary<string, List<string>>>();
		
		//Object[] objs = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
		Object[] objs = Selection.objects;
		foreach (Object objFolder in objs) {
			string directoryInAsset = AssetDatabase.GetAssetOrScenePath(objFolder);
			directoryInAsset = directoryInAsset.Replace("Assets", "");
			string directoryName = Application.dataPath + directoryInAsset;
			if (!File.Exists(directoryName))
				continue;
			
			DirectoryInfo dirInfo = new DirectoryInfo(directoryName);
			string bundleNameToBuild = dirInfo.Name;
			if (bundleNameToBuild.IndexOf(".") > 0) {
				bundleNameToBuild = bundleNameToBuild.Substring(0, bundleNameToBuild.IndexOf("."));
			}
			if (EditorTools.mIsLowerCaseName) {
				bundleNameToBuild = bundleNameToBuild.ToLower();
			}
			bundleNameToBuild = bundleNameToBuild + EditorTools.mFileExtension;
			
			//GetDependencies(bundleNameToBuild , objFolder);
			
			string folderNameToBuild = dirInfo.Parent.FullName;
			folderNameToBuild = folderNameToBuild.Replace("\\", "/");
			
			List<string> assetNamesToBuild = new List<string>();
			assetNamesToBuild.Add(directoryName);
			
			Dictionary<string, List<string>> directoryToBuild = new Dictionary<string, List<string>>();
			directoryToBuild.Add(folderNameToBuild, assetNamesToBuild);
			
			bundlesToBuild.Add(bundleNameToBuild, directoryToBuild);
		}
		EditorTools.mBuildType = EditorTools.BuildType.BT_ExportFiles;
		ExportAssetBundles.ExportFileAssetBundles(bundlesToBuild);
	}

	[MenuItem("AssetBundle Tools/ExportFolders")]
	public static void ExportFolders() {
		Dictionary<string, Dictionary<string, List<string>>> bundlesToBuild = new Dictionary<string, Dictionary<string, List<string>>>();
		Object[] objs = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
		foreach (Object objFolder in objs) {
			string directoryInAsset = AssetDatabase.GetAssetOrScenePath(objFolder);
			directoryInAsset = directoryInAsset.Replace("Assets", "");
			string directoryName = Application.dataPath + directoryInAsset;
			Debug.Log("dir=" + directoryName);
			
			if (!Directory.Exists(directoryName)) {
				continue;
			}
			
			DirectoryInfo dirInfo = new DirectoryInfo(directoryName);
			string bundleNameToBuild = dirInfo.Name;
			if (bundleNameToBuild.IndexOf(".") > 0) {
				bundleNameToBuild = bundleNameToBuild.Substring(0, bundleNameToBuild.IndexOf("."));
			}
			if (EditorTools.mIsLowerCaseName) {
				bundleNameToBuild = bundleNameToBuild.ToLower();
			}
			bundleNameToBuild = bundleNameToBuild + EditorTools.mFileExtension;
			Dictionary<string, List<string>> directoryToBuild = GetFolderAssetBundles(directoryName);
			bundlesToBuild.Add(bundleNameToBuild, directoryToBuild);
		}
		EditorTools.mBuildType = EditorTools.BuildType.BT_ExportFolders;
		ExportAssetBundles.ExportFileAssetBundles(bundlesToBuild);
	}
	
	private static Dictionary<string, List<string>> GetFolderAssetBundles(string directoryName) {
		string[] assetPaths = Directory.GetFiles(directoryName);
		//string[] assetNames = Directory.GetFiles(directoryName, "*", SearchOption.AllDirectories);
		List<string> assetPathsToBuild = new List<string>();
		foreach (string assetPath in assetPaths) {
			string path = assetPath.Replace("\\", "/");
			if(assetPath.EndsWith(".meta")){
				continue;
			}
			//Debug.Log("assetPath==========" + path);
			assetPathsToBuild.Add(path);
		}
		
		Dictionary<string, List<string>> directoryToBuild = new Dictionary<string, List<string>>();
		directoryToBuild.Add(directoryName, assetPathsToBuild);
		
		return directoryToBuild;
	}

	private static void ExportFiles(Object[] objs) {
		Dictionary<string, Dictionary<string, List<string>>> bundlesToBuild = new Dictionary<string, Dictionary<string, List<string>>>();
		
		//Object[] objs = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
		//Object[] objs = Selection.objects;
		foreach (Object objFolder in objs) {
			string directoryInAsset = AssetDatabase.GetAssetOrScenePath(objFolder);
			directoryInAsset = directoryInAsset.Replace("Assets", "");
			string directoryName = Application.dataPath + directoryInAsset;
			if (!File.Exists(directoryName))
				continue;
			
			DirectoryInfo dirInfo = new DirectoryInfo(directoryName);
			string bundleNameToBuild = dirInfo.Name;
			if (bundleNameToBuild.IndexOf(".") > 0) {
				bundleNameToBuild = bundleNameToBuild.Substring(0, bundleNameToBuild.IndexOf("."));
			}
			if (EditorTools.mIsLowerCaseName) {
				bundleNameToBuild = bundleNameToBuild.ToLower();
			}
			bundleNameToBuild = bundleNameToBuild + EditorTools.mFileExtension;
			
			//GetDependencies(bundleNameToBuild , objFolder);
			
			string folderNameToBuild = dirInfo.Parent.FullName;
			folderNameToBuild = folderNameToBuild.Replace("\\", "/");
			
			List<string> assetNamesToBuild = new List<string>();
			assetNamesToBuild.Add(directoryName);
			
			Dictionary<string, List<string>> directoryToBuild = new Dictionary<string, List<string>>();
			directoryToBuild.Add(folderNameToBuild, assetNamesToBuild);
			
			bundlesToBuild.Add(bundleNameToBuild, directoryToBuild);
		}
		EditorTools.mBuildType = EditorTools.BuildType.BT_ExportFiles;
		ExportAssetBundles.ExportFileAssetBundles(bundlesToBuild);
	}
	
	private static void ExportFolders(Object[] objs) {
		Dictionary<string, Dictionary<string, List<string>>> bundlesToBuild = new Dictionary<string, Dictionary<string, List<string>>>();
		Dictionary<string, List<string>> directoryToBuild = new Dictionary<string, List<string>>();
		List<string> assetNamesToBuild = new List<string>();
		int index = 0;
		string bundleNameToBuild = string.Empty;
		foreach (Object objFolder in objs) {
			string directoryInAsset = AssetDatabase.GetAssetOrScenePath(objFolder);
			directoryInAsset = directoryInAsset.Replace("Assets", "");
			string directoryName = Application.dataPath + directoryInAsset;
			if (!File.Exists(directoryName))
				continue;
			if(index == 0){
				DirectoryInfo dirInfo = new DirectoryInfo(directoryName);
				bundleNameToBuild = dirInfo.Name;
				if (bundleNameToBuild.IndexOf(".") > 0) {
					bundleNameToBuild = bundleNameToBuild.Substring(0, bundleNameToBuild.IndexOf("."));
				}
				bundleNameToBuild = bundleNameToBuild + "Folders";
				if (EditorTools.mIsLowerCaseName) {
					bundleNameToBuild = bundleNameToBuild.ToLower();
				}
				bundleNameToBuild = bundleNameToBuild + EditorTools.mFileExtension;
			}
			assetNamesToBuild.Add(directoryName);
			index++;
		}
		Debug.Log("bundleNameToBuild : "+bundleNameToBuild);
		directoryToBuild.Add(bundleNameToBuild, assetNamesToBuild);
		bundlesToBuild.Add(bundleNameToBuild, directoryToBuild);
		
		EditorTools.mBuildType = EditorTools.BuildType.BT_ExportFolders;
		ExportAssetBundles.ExportFileAssetBundles(bundlesToBuild);
	}

	void OnGUI(){
		if (GUI.Button(new Rect(10f, 10f, 200f, 50f), "Step(1) : ExportFiles")){
			ExportFiles(Selection.objects);
			if(mMenu == null){
				mMenu = GetWindow(typeof(EditorMenu)) as EditorMenu;
			}
			mMenu.Close();
		}
		
		if (GUI.Button(new Rect(10f, 80f, 200f, 50f), "Step(2) : ExportFolders")){
			ExportFolders(Selection.objects);
			if(mMenu == null){
				mMenu = GetWindow(typeof(EditorMenu)) as EditorMenu;
			}
			mMenu.Close();
		}
	}

	/// <summary>
	/// 删除2D片头动画的资源
	/// </summary>
	static void DeleteCG2D() {
		string[] PathList = {
			Application.dataPath + "/Resources/China/Pack/JC/CG2D.meta",
			Application.dataPath + "/Resources/China/Pack/JC/CG2D.prefab",
		};

		foreach(var path in PathList) {
			try {
				if(File.Exists(path)) {
					File.Delete(path);
				}
			} catch(IOException ex) {
				Debug.Log(ex.Message);
			} 
		}

		AssetDatabase.Refresh();
	}

	/// <summary>
	/// 删除3D片头动画的资源
	/// </summary>
	static void DeleteCG3D() {
		string[] PathList = {
			Application.dataPath + "/Resources/China/Pack/JC/CG3D.meta",
			Application.dataPath + "/Resources/China/Pack/JC/CG3D.prefab",
		};

		foreach(var path in PathList) {
			try {
				if(File.Exists(path)) {
					File.Delete(path);
				}
			} catch(IOException ex) {
				Debug.Log(ex.Message);
			} 
		}

		AssetDatabase.Refresh();
	}

}
