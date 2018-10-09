using UnityEngine;
using UnityEditor;
using System.IO;
using System.Security.Cryptography;

public class EditorTools{
	
	#if UNITY_IPHONE
	public static BuildTarget mBuildTarget = BuildTarget.iPhone;
	#elif UNITY_ANDROID
	public static BuildTarget mBuildTarget = BuildTarget.Android;
	#else
	public static BuildTarget mBuildTarget = BuildTarget.StandaloneWindows;
	#endif
	public enum BuildType{
		BT_None,
		BT_ExportFiles,
		BT_ExportFolders,
	}
	public static BuildType mBuildType = BuildType.BT_None;
	
	//不同平台导出的文件夹名
	public static string GetRemoteFolder() {
		BuildTarget buildTarget = mBuildTarget;
		switch (buildTarget) {
		case BuildTarget.iPhone:
			return "IOS";
		case BuildTarget.Android:
			return "Android";
		}
		return "Editor";
	}
	
	public static string mExportFolderLocation = "/../AssetBundles/";
	public static string mExportFolderLocationFloder {
		get { 
			return Application.dataPath.Replace('\\', '/')+mExportFolderLocation+ GetRemoteFolder()+"/";
		} 
	}
	
	public static string mExportVersionFolderName = "PrefabInfos.bytes";
	public static string mExportVersionFolderLocation = "/VersionNum";
	public static string mExportVersionFolderLocationFloder {
		get { 
			return mExportFolderLocationFloder + mExportVersionFolderLocation;
		} 
	}
	
	public static string mGame = "dragonball";
	public static string mVersion = "1";
	
	//导出文件后缀名
	public static string mFileExtension = ".unity3d";
	//设置大小写
	public static bool mIsLowerCaseName = false;
	
	//BuildAssetBundleOptions
	public static bool mIsCollectDependencies = true;
	public static bool mIsCompleteAssets = true;
	public static bool mIsDisableWriteTypeTree = false;
	public static bool mIsDeterministicAssetBundle = false;
	public static bool mIsUncompressedAssetBundle = false;
	
	public static string GetMD5Hash(string filePath) {

		using (FileStream fs = new FileStream(filePath, FileMode.Open)) {
			int len = (int)fs.Length;
			byte[] data = new byte[len];
			fs.Read(data, 0, len);

			MD5 md5 = new MD5CryptoServiceProvider();
			byte[] md5data = md5.ComputeHash(data);
			md5.Clear();
			string fileMD5 = string.Empty;
			for (int i = 0; i < md5data.Length; i++) {
				fileMD5 += md5data[i].ToString("x").PadLeft(2, '0');
			}
			return fileMD5;
		}
	}
}
