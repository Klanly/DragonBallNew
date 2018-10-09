using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Compression;
using DeCompression;

namespace UEditor {

    public class ZipCreator {

        [MenuItem("ZipCreator/ZipConfig")]
        static void ZipConfig () 
		{
            string []FileProperties=new string[2];    
			string Path = Application.streamingAssetsPath;
			Debug.Log("Path="+Path);
			
			//待压缩文件目录   
			FileProperties[0]=System.IO.Path.Combine(Path,"Config");
			//压缩后的目标文件  
			FileProperties[1]=System.IO.Path.Combine(Path,"Config.zip");
			ZipClass Zc=new ZipClass();  
			Zc.ZipFileMain(FileProperties); 
        }
		
        [MenuItem("ZipCreator/UnZipConfig")]
        static void UnZipConfig() 
		{
			string []FileProperties=new string[2]; 
			//待解压的文件   
			FileProperties[0]=System.IO.Path.Combine(Application.streamingAssetsPath,"Config.zip");
			//string ConfigPath = System.IO.Path.Combine(DeviceInfo.PersistRootPath,"Config.zip");
			//解压后放置的目标目录    
			FileProperties[1]= Application.streamingAssetsPath;		
			UnZipClass UnZc=new UnZipClass();   
			UnZc.UnZip(FileProperties);
        }
		
		[MenuItem("ZipCreator/ZipMD5")]
		static void ZipMD5() 
		{
			string ConfigPath = System.IO.Path.Combine(Application.streamingAssetsPath,"Config.zip");
			//string ConfigPath = System.IO.Path.Combine(DeviceInfo.PersistRootPath,"Config.zip");
			string ConfigMD5 = MessageDigest_Algorithm.getFileMd5Hash(ConfigPath);
			if(string.IsNullOrEmpty(ConfigMD5))
				EditorUtility.DisplayDialog("Config.zip MD5", "can't find Config.zip" , "OK");
			else
			    EditorUtility.DisplayDialog("Config.zip MD5", ConfigMD5 , "OK");
		}
    }
}