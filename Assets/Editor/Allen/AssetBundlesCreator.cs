// Builds an asset bundle from the selected objects in the project view,
// and changes the texture format using an AssetPostprocessor.
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace UEditor {
	
	public class AssetBundlesCreator {
		
		//        [MenuItem("AssetBundle Tools/Build AssetBundle From Selection - Track dependencies")]
		static void ExportResourceTrack () {
			// Bring up save panel
			string path = EditorUtility.SaveFilePanel ("Save Resource", DeviceInfo.PersistRootPath, "New AssetBundle", "unity3d");
			if (path.Length != 0) {
				// Build the resource file from the active selection.
				Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
				
				#if UNITY_ANDROID
				BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, BuildTarget.Android );
				#elif UNITY_IPHONE
				BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, BuildTarget.iPhone );
				#elif UNITY_WP8
				BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, BuildTarget.WP8Player );
				#endif
				Selection.objects = selection;
			}
		}
		
		//        [MenuItem("AssetBundle Tools/Build AssetBundle Without Compressed")]
		static void ExportResourceNoCompressed() {
			// Bring up save panel
			string path = EditorUtility.SaveFilePanel ("Save Resource", DeviceInfo.PersistRootPath, "New AssetBundle", "unity3d");
			if (path.Length != 0) {
				// Build the resource file from the active selection.
				Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
				
				#if UNITY_ANDROID
				BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, BuildTarget.Android );
				#elif UNITY_IPHONE
				BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.iPhone );
				#elif UNITY_WP8
				BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, BuildTarget.WP8Player );
				#endif
				Selection.objects = selection;
			}
		}
		
		
		
		static void ExportResourceNoTrack () {
			// Bring up save panel
			string path = EditorUtility.SaveFilePanel ("Save Resource", DeviceInfo.PersistRootPath, "New AssetBundle", "unity3d");
			if (path.Length != 0) {
				// Build the resource file from the active selection.
				#if UNITY_ANDROID
				BuildPipeline.BuildAssetBundle(Selection.activeObject, Selection.objects, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, BuildTarget.Android);
				#elif UNITY_IPHONE
				BuildPipeline.BuildAssetBundle(Selection.activeObject, Selection.objects, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, BuildTarget.iPhone);
				#elif UNITY_WP8
				BuildPipeline.BuildAssetBundle(Selection.activeObject, Selection.objects, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, BuildTarget.WP8Player);
				#endif
				
			}
		}
		
		// Store current texture format for the TextureProcessor.
		public static TextureImporterFormat textureFormat;
		
		//[MenuItem("REDTools/Build AssetBundle From Selection - PVRTC_RGB2")]
		static void ExportResourceRGB2 () {
			textureFormat = TextureImporterFormat.PVRTC_RGB2;
			ExportResource();       
		}   
		
		//[MenuItem("REDTools/Build AssetBundle From Selection - PVRTC_RGB4")]
		static void ExportResourceRGB4 () {
			textureFormat = TextureImporterFormat.PVRTC_RGB4;
			ExportResource();
		}
		
		static void ExportResource () {
			// Bring up save panel.
			string path = EditorUtility.SaveFilePanel ("Save Resource", Application.persistentDataPath , "New Resource", "unity3d");
			
			if (path.Length != 0) {
				// Build the resource file from the active selection.
				Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
				
				foreach (object asset in selection) {
					string assetPath = AssetDatabase.GetAssetPath((UnityEngine.Object) asset);
					if (asset is Texture2D) {
						// Force reimport thru TextureProcessor.
						AssetDatabase.ImportAsset(assetPath);
					}
				}
				
				BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, BuildTarget.iPhone);
				Selection.objects = selection;
			}
		}
	}
}