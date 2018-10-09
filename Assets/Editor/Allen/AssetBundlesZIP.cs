using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Compression;

public class AssetBundlesZIP {

	public static void BaleZIP(){
		string balePath = EditorTools.mExportFolderLocationFloder;
		if (!Directory.Exists(balePath)) {
			Directory.CreateDirectory(balePath);
		} 
		string savePath = EditorTools.mExportFolderLocationFloder + EditorTools.mGame+".ZIP";
		if(File.Exists(savePath)){
			File.Delete(savePath);
		}
		string []FileProperties=new string[2];   
		//待压缩文件目录   
		FileProperties[0] = balePath;
		//压缩后的目标文件  
		FileProperties[1] = savePath;  

		ZipClass Zc=new ZipClass();  
		Zc.ZipFileMain(FileProperties); 
	}
}
