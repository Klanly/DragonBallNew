using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

namespace UEditor {

    public class CampareMD5ToGenerateVersionNum {

        public static void Execute(UnityEditor.BuildTarget target)
        {
            string platform = AssetBundlesCreatorEx.GetPlatformName(target);
            Execute(platform);
            AssetDatabase.Refresh();
        }

        // 对比对应版本目录下的VersionMD5和VersionMD5-old，得到最新的版本号文件VersionNum.xml
        public static void Execute(string platform)
        {
            // 读取新旧MD5列表
            string newVersionMD5 = Path.Combine(DeviceInfo.PersistRootPath, "AssetBundles/" + platform + "/VersionNum/VersionMD5.bytes");
            string oldVersionMD5 = Path.Combine(DeviceInfo.PersistRootPath, "AssetBundles/" + platform + "/VersionNum/VersionMD5-old.bytes");

            Dictionary<string, string> dicNewMD5Info = ReadMD5File(newVersionMD5);
            Dictionary<string, string> dicOldMD5Info = ReadMD5File(oldVersionMD5);

            // 读取版本号记录文件VersinNum.xml
            string oldVersionNum = Path.Combine(DeviceInfo.PersistRootPath, "AssetBundles/" + platform + "/VersionNum.bytes");
            Dictionary<string, VersionNumItem> dicVersionNumInfo = ReadVersionNumFile(oldVersionNum);

            // 对比新旧MD5信息，并更新版本号，即对比dicNewMD5Info&&dicOldMD5Info来更新dicVersionNumInfo
            foreach (KeyValuePair<string, string> newPair in dicNewMD5Info)
            {
                // 旧版本中有
                if (dicOldMD5Info.ContainsKey(newPair.Key))
                {
                    // MD5一样，则不变
                    // MD5不一样，则+1
                    // 容错：如果新旧MD5都有，但是还没有版本号记录的，则直接添加新纪录，并且将版本号设为1
                    if (dicVersionNumInfo.ContainsKey(newPair.Key) == false)
                    {
                        VersionNumItem item = new VersionNumItem();
                        item.FileName = dicVersionNumInfo[newPair.Key].FileName;
                        item.Num = 1;
                        item.Size = dicVersionNumInfo[newPair.Key].Size;
                        dicVersionNumInfo.Add(newPair.Key, item);
                    }
                    else if (newPair.Value != dicOldMD5Info[newPair.Key])
                    {
                        VersionNumItem item = new VersionNumItem();
                        item.FileName = dicVersionNumInfo[newPair.Key].FileName;
                        item.Num = dicVersionNumInfo[newPair.Key].Num + 1;
                        item.Size = dicVersionNumInfo[newPair.Key].Size;
                        dicVersionNumInfo[newPair.Key] = item;
                    }
                }
                else // 旧版本中没有，则添加新纪录，并=1
                {
                    if(!dicVersionNumInfo.ContainsKey(newPair.Key)){
                        VersionNumItem item = new VersionNumItem();
                        item.FileName = newPair.Key;
                        item.Num = 1;
                        string newPath = Path.Combine(DeviceInfo.PersistRootPath, "AssetBundles/" + platform + "/" + newPair.Key);
                        FileStream file = new FileStream(newPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                        string size = file.Length.ToString();
                        item.Size = size;
                        dicVersionNumInfo.Add(newPair.Key, item);
                    }
                }
            }
            // 不可能出现旧版本中有，而新版本中没有的情况，原因见生成MD5List的处理逻辑

            // 存储最新的VersionNum.xml
            SaveVersionNumFile(dicVersionNumInfo, oldVersionNum);
        }

        static Dictionary<string, string> ReadMD5File(string fileName)
        {
            Dictionary<string, string> DicMD5 = new Dictionary<string, string>();

            // 如果文件不存在，则直接返回
            if (File.Exists(fileName) == false)
                return DicMD5;

            FileMD5Col col = IO.LoadFromFile<FileMD5Col>(fileName);
            if(col != null) {
                foreach(FileMD5Item item in col.fileInfo) {
                    DicMD5.Add(item.FileName, item.MD5);
                }
            }

            return DicMD5;
        }

        static Dictionary<string, VersionNumItem> ReadVersionNumFile(string fileName)
        {
            Dictionary<string, VersionNumItem> DicVersionNum = new Dictionary<string, VersionNumItem>();

            // 如果文件不存在，则直接返回
            if (File.Exists(fileName) == false)
                return DicVersionNum;

            VersionNum col = IO.LoadFromFile<VersionNum>(fileName);

            if(col != null) {
				foreach(VersionNumItem item in col.Files) {
					DicVersionNum.Add(item.FileName, item);
                }
            }

            return DicVersionNum;
        }

        static void SaveVersionNumFile(Dictionary<string, VersionNumItem> data, string savePath)
        {
			VersionNum ver = new VersionNum(EditorTools.mGame,EditorTools.GetRemoteFolder(),EditorTools.mVersion);
            foreach(KeyValuePair<string, VersionNumItem> pair in data) {
                ver.addItem(pair.Value);
            }
            ver.End();

            string newSavedPath = Path.Combine(DeviceInfo.PersistRootPath, "Assets/StreamingAssets/Config/VerRes.bytes"); 
			IO.SaveToFile(ver, newSavedPath);
            IO.SaveToFile(ver, savePath);
        }

    }
}
