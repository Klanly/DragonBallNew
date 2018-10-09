using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace UEditor {
    public class CreateAssetBundle
    {
        public static void Execute(UnityEditor.BuildTarget target)
        {
            string SavePath = AssetBundlesCreatorEx.GetPlatformPath(target);

            Object[] targets = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            // 当前选中的资源列表
            foreach (Object o in targets)
            {
                string path = AssetDatabase.GetAssetPath(o);

                // 过滤掉meta文件
//                if(path.Contains(".meta"))
//                    continue;

                path = SavePath + ConvertToAssetBundleName(path);
                path = path.Substring(0, path.LastIndexOf('.'));
                path += ".assetbundle";

                BuildPipeline.BuildAssetBundle(o, null, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets , target);
            }

            AssetDatabase.Refresh();
        }

        static string ConvertToAssetBundleName(string ResName)
        {
            return ResName.Replace('/', '.');
        }

    }
}
