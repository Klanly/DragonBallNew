using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UEditor;

public class ExportAssetBundles {
	
	//导出场景资源包
	public static void ExportScenes(List<string> scenes) {
		UnityEngine.Debug.ClearDeveloperConsole();
		Caching.CleanCache(); 
		if (scenes.Count > 0) {
			int createdBundles = 0;
			int allBundles = scenes.Count;
			foreach (string pair in scenes) {
				string[] levels = { pair };
				
				DirectoryInfo dirInfo = new DirectoryInfo(pair);
				string sceneName = dirInfo.Name;
				sceneName = sceneName.Replace(".unity", EditorTools.mFileExtension);
				
				string path = EditorTools.mExportFolderLocationFloder;
				
				if (!Directory.Exists(path)) {
					Directory.CreateDirectory(path);
				} 
				
				BuildPipeline.BuildStreamedSceneAssetBundle(levels, path + sceneName, EditorTools.mBuildTarget);
				
				createdBundles++;
				
				Debug.Log("场景：   " + sceneName + "  导出完毕！  适用于：" + EditorTools.mBuildTarget + " 平台！");
			}
			if (createdBundles > 0) {
				Debug.Log("***本地 场景 成功打包： " + createdBundles + "/" + allBundles + " 场景资源!***");
			}
			
		} else {
			Debug.Log("没有资源可以打包!请选择资源！");
		}
		AssetDatabase.Refresh();
		
	}
	
	//导出单个文件资源包
	public static void ExportFileAssetBundles(Dictionary<string, Dictionary<string, List<string>>> bundles) {
		UnityEngine.Debug.ClearDeveloperConsole();
		Caching.CleanCache(); 
		if (bundles.Count > 0) {
			int createdBundles = 0;
			int allBundles = bundles.Count;
			foreach (KeyValuePair<string, Dictionary<string, List<string>>> bundle in bundles) {
				string bundleName = bundle.Key;
				bool result = true;
				List<Object> assets = new List<Object>();
				foreach (KeyValuePair<string, List<string>> bundleFolder in bundle.Value) {
					List<string> assetNamesInFolder = bundleFolder.Value;
					foreach (string assetName in assetNamesInFolder) {
						string folderName = assetName;
						int directoryStart = assetName.IndexOf("Assets");
						folderName = assetName.Remove(0, directoryStart);
						string thisFileName = Path.GetFileName(assetName);
						if (assetName.EndsWith(".prefab")) {
							string internalFilePath = folderName;
							//Debug.Log("internalFilePath1==================="+internalFilePath);
							GameObject prefab = AssetDatabase.LoadAssetAtPath(internalFilePath, typeof(GameObject)) as GameObject;
							assets.Add(prefab);
						} else if (!assetName.EndsWith(".meta") && thisFileName.IndexOf(".") != -1) {
							string internalFilePath = folderName;
							//Debug.Log("internalFilePath2==================="+internalFilePath);
							assets.Add(AssetDatabase.LoadAssetAtPath(internalFilePath, typeof(Object)));
						}
						if(EditorTools.mBuildType == EditorTools.BuildType.BT_ExportFiles){
							GetDependencies(bundleName,folderName,ref result);
						}
					}
					if(EditorTools.mBuildType == EditorTools.BuildType.BT_ExportFolders){
						GetDependencies(bundleName,assetNamesInFolder,ref result);
					}
				}
				if(!result){
					Debug.Log("bundleName : "+bundleName + "  文件没有变动!!!");
					continue;
				}
				//string path = Application.dataPath + EditorTools.m_sExportFolderLocationFloder;
				string path = EditorTools.mExportFolderLocationFloder;
				
				if (!Directory.Exists(path)) {
					Directory.CreateDirectory(path);
				} 
				
				if (!BuildAssetBundle(assets, path + bundleName)) {
					continue;
				} 
				Debug.Log("资源：  " + bundleName + "  导出完毕！  适用于：" + EditorTools.mBuildTarget + " 平台！");
				string MD5Hash = EditorTools.GetMD5Hash(path + bundleName);
				Debug.Log("MD5Hash:" + MD5Hash);
				createdBundles++;
			}
			if (createdBundles > 0) {
				Debug.Log("***本地 资源 成功打包： " + createdBundles + "/" + allBundles + " 场景资源!***");
			}
		} else {
			Debug.Log("没有资源可以打包!请选择资源！");
		}
		CreateMD5List.Execute(EditorTools.mBuildTarget);
		CampareMD5ToGenerateVersionNum.Execute(EditorTools.mBuildTarget);
		AssetDatabase.Refresh();
	}
	
	
	
	private static bool BuildAssetBundle(List<Object> toInclude, string bundlePath) {
		if (toInclude.Count > 0) {
			BuildAssetBundleOptions buildAssetOptions = BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets;
			if (!BuildPipeline.BuildAssetBundle(toInclude.ToArray()[0], toInclude.ToArray(), bundlePath, buildAssetOptions, EditorTools.mBuildTarget)) {
				Debug.Log("打包失败!请检查您选择资源！");
				return false; 
			}
		} else {
			Debug.Log("没有资源可以打包!请选择资源！");
			return false;
		}
		return true;
	}
	
	private static void GetDependencies(string bundleName , List<string> list,ref bool result){
        Dictionary<string, FileMD5Item> DicFileMD5 = new Dictionary<string, FileMD5Item>();
        Dictionary<string, Dictionary<string, FileMD5Item>> DicFileInfo = new Dictionary<string, Dictionary<string, FileMD5Item>>();
		foreach(string path in list){
			//Debug.Log("path:" + path);
			string MD5Hash = EditorTools.GetMD5Hash(path);
			//Debug.Log("MD5Hash:" + MD5Hash);
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            string strSize = file.Length.ToString();
            FileMD5Item item = new FileMD5Item(path,MD5Hash,strSize);
            DicFileMD5.Add(path,item);
		}
		DicFileInfo.Add(bundleName,DicFileMD5);
		result = FileChange(bundleName,DicFileInfo);
		if(result){
			SaveToFile(DicFileInfo);
		}
	}
	
	private static void GetDependencies(string bundleName , string bundlePath,ref bool result){
		
        Dictionary<string, FileMD5Item> DicFileMD5 = new Dictionary<string, FileMD5Item>();
        Dictionary<string, Dictionary<string, FileMD5Item>> DicFileInfo = new Dictionary<string, Dictionary<string, FileMD5Item>>();
		
		//Debug.Log("bundlePath : "+bundlePath);
		string[] dependencies = AssetDatabase.GetDependencies(new string[]{bundlePath});
		foreach(string path in dependencies){
			//			Debug.Log("path : "+path);
			//			if(path == bundlePath){
			//				continue;
			//			}
			string newPath = Application.dataPath.Replace("Assets", "") + path;
			//Debug.Log("newPath : "+ newPath);
			string MD5Hash = EditorTools.GetMD5Hash(newPath);
			//Debug.Log("MD5Hash:" + MD5Hash);
            FileStream file = new FileStream(newPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            string strSize = file.Length.ToString();
            FileMD5Item item = new FileMD5Item(path,MD5Hash,strSize);
            DicFileMD5.Add(path,item);
		}
		Debug.Log("bundleName : "+bundleName);
		DicFileInfo.Add(bundleName,DicFileMD5);
		result = FileChange(bundleName,DicFileInfo);
		if(result){
			SaveToFile(DicFileInfo);
		}
	}
	
    public static bool FileChange(string bundleName,Dictionary<string, Dictionary<string, FileMD5Item>> DicFileInfo){
		string curPath = EditorTools.mExportVersionFolderLocationFloder + "/" + EditorTools.mExportVersionFolderName;
		bool result = false;
		if(File.Exists(curPath)){
			PrefabInfo oldInfo = IO.LoadFromFile<PrefabInfo>(curPath);
			if(oldInfo != null){
                Dictionary<string, Dictionary<string, FileMD5Item>> FileInfos = new Dictionary<string, Dictionary<string, FileMD5Item>>();
				foreach(PrefabCol col in oldInfo.Files) {
                    Dictionary<string, FileMD5Item> DicFileMD5 = new Dictionary<string, FileMD5Item>();
					foreach(FileMD5Item item in col.FileInfo) {
						DicFileMD5.Add(item.FileName,item);
					}
					FileInfos.Add(col.FileName,DicFileMD5);
				}
				if(!FileInfos.ContainsKey(bundleName)){
					result = true;
				}else{
					foreach(PrefabCol col in oldInfo.Files) {
						//Debug.Log("col.FileName : "+col.FileName);
                        Dictionary<string, FileMD5Item> dic = null;
						if(DicFileInfo.TryGetValue(col.FileName,out dic)){
							if(dic.Count!=col.FileInfo.Length){
								Debug.Log(dic.Count+"/"+col.FileInfo.Length);
								Debug.Log("文件依赖的文件个数有变动!!");
								result = true;
							}else{
								foreach(FileMD5Item item in col.FileInfo) {
                                    FileMD5Item data = null;
                                    if(dic.TryGetValue(item.FileName, out data)){
                                        if(item.MD5 != data.MD5){
											Debug.Log(item.FileName);
											Debug.Log("文件依赖的文件有变动!!!");
											result = true;
										}
									}
								}
							}
						}
					}
				}
			}else{
				Debug.Log("333333!!!");
				result = true;
			}
		}else{
			Debug.Log("4444444!!!");
			result = true;
		}
		return result;
	}
	
    private static void SaveToFile(Dictionary<string, Dictionary<string, FileMD5Item>> DicFileInfo){
		string savePath = EditorTools.mExportVersionFolderLocationFloder;
		if (!Directory.Exists(savePath)){
			Directory.CreateDirectory(savePath);
		}
		string curPath = EditorTools.mExportVersionFolderLocationFloder + "/" + EditorTools.mExportVersionFolderName;
		
		PrefabInfo oldInfo = IO.LoadFromFile<PrefabInfo>(curPath);
		if(oldInfo != null){
			foreach(PrefabCol col in oldInfo.Files) {
                Dictionary<string, FileMD5Item> DicFileMD5 = new Dictionary<string, FileMD5Item>();
				foreach(FileMD5Item item in col.FileInfo) {
					if(!DicFileMD5.ContainsKey(item.FileName)) {
                        FileMD5Item data = new FileMD5Item(item.FileName, item.MD5,item.Size);
                        DicFileMD5.Add(item.FileName, data);
					}
				}
				if(!DicFileInfo.ContainsKey(col.FileName)){
					DicFileInfo.Add(col.FileName,DicFileMD5);
				}
				else{
					DicFileInfo[col.FileName] = DicFileMD5;
				}
			}
		}
		
		if (File.Exists(curPath)){
			File.Delete(curPath);
		}
		
		PrefabInfo prefabinfo = new PrefabInfo();
        foreach(KeyValuePair<string,Dictionary<string, FileMD5Item>> info in DicFileInfo){
			PrefabCol prefabcol = new PrefabCol(info.Key);
            foreach(KeyValuePair<string,FileMD5Item> pair in info.Value){
                FileMD5Item item = new FileMD5Item(pair.Value.FileName, pair.Value.MD5,pair.Value.Size);
                prefabcol.addItem(item);
			}
			prefabcol.End();
			prefabinfo.addItem(prefabcol);
		}
		prefabinfo.End();
		IO.SaveToFile(prefabinfo, curPath);
		
	}
}
