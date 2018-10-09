using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace UEditor {
    public class CreateAssetBundleForXmlVersion
    {
        public static void Execute(UnityEditor.BuildTarget target)
        {
            string SavePath = AssetBundlesCreatorEx.GetPlatformPath(target);
            Object obj = AssetDatabase.LoadAssetAtPath(SavePath + "VersionNum/VersionNum.xml", typeof(Object));
            BuildPipeline.BuildAssetBundle(obj, null, SavePath + "VersionNum/VersionNum.assetbundle", BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.DeterministicAssetBundle, target);

            AssetDatabase.Refresh();
        }

        static string ConvertToAssetBundleName(string ResName)
        {
            return ResName.Replace('/', '.');
        }

    }
}