using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using JsonFx.Json;

namespace UEditor {
    public class CreateMD5List
    {

        public static void Execute(UnityEditor.BuildTarget target)
        {
            // 获取平台标识 比如 IOS 、Mac
            string platform = AssetBundlesCreatorEx.GetPlatformName(target);
            Execute(platform);
            AssetDatabase.Refresh();
        }

        public static void Execute(string platform)
        {
            //FileMD5Item
            Dictionary<string, FileMD5Item> DicFileMD5 = new Dictionary<string, FileMD5Item>();
            FileMD5Col collection = new FileMD5Col();

            MD5CryptoServiceProvider md5Generator = new MD5CryptoServiceProvider();

            string dir = Path.Combine(DeviceInfo.PersistRootPath, "AssetBundles/" + platform); 

            foreach (string filePath in Directory.GetFiles(dir))
            {
                if (!filePath.EndsWith(".unity3d"))
                    continue;
                FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                byte[] hash = md5Generator.ComputeHash(file);
                string strMD5 = System.BitConverter.ToString(hash);
                string strSize = file.Length.ToString();
                Debug.Log("======"+strMD5+"/"+strSize);
                file.Close();

                string key = filePath.Substring(dir.Length + 1, filePath.Length - dir.Length - 1);

                if (DicFileMD5.ContainsKey(key) == false){
                    FileMD5Item item = new FileMD5Item();
                    item.FileName = key;
                    item.MD5 = strMD5;
                    item.Size = strSize;
                    DicFileMD5.Add(key, item);
                }else{
                    Debug.LogWarning("<Two File has the same name> name = " + filePath);
                }
            }

            string savePath = Path.Combine(DeviceInfo.PersistRootPath, "AssetBundles/") + platform + "/VersionNum";
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            string oldPath = savePath + "/VersionMD5-old.bytes";
            string curPath = savePath + "/VersionMD5.bytes";

            // 删除前一版的old数据
            if (File.Exists(oldPath))
                File.Delete(oldPath);

            // 如果之前的版本存在，则将其名字改为VersionMD5-old.xml
            if (File.Exists(curPath))
                File.Move(curPath, oldPath);

            // 读取旧版本的MD5, 以防止新版只是做了一部分的AssetBundle
            FileMD5Col oldCol = IO.LoadFromFile<FileMD5Col>(oldPath);
            if(oldCol != null) {
                foreach(FileMD5Item item in oldCol.fileInfo) {
                    if(!DicFileMD5.ContainsKey(item.FileName)) {
                        DicFileMD5.Add(item.FileName, item);
                    }
                }
            }

            //保存为VersionMD5
            foreach(KeyValuePair<string, FileMD5Item> pair in DicFileMD5) {
                FileMD5Item data = new FileMD5Item(pair.Value.FileName, pair.Value.MD5,pair.Value.Size);
                collection.addItem(data);
            }

            collection.End();
            IO.SaveToFile(collection, curPath);
        }
    }
}