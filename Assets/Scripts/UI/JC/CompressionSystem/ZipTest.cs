using UnityEngine;
using System.Collections;
using Compression;
using DeCompression;
public class ZipTest : MonoBehaviour {

	void Start () {
	
	}
	
	void OnGUI()
	{
		if(GUI.Button(new Rect(400,130,100,50),"ZipClass"))
		{
			string []FileProperties=new string[2];    
			//待压缩文件目录   
			FileProperties[0]=@"/Users/jiangcheng/Desktop/ziptest";
			//压缩后的目标文件  
			FileProperties[1]=@"/Users/jiangcheng/Desktop/a.zip";  
			ZipClass Zc=new ZipClass();  
			Zc.ZipFileMain(FileProperties); 
			
			//Debug.Log(DeviceInfo.StreamingPath);
//			ZipClass Zc=new ZipClass();  
//			Zc.ZipFile(@"/Users/jiangcheng/Desktop/ziptest",@"/Users/jiangcheng/Desktop/a.zip",6,2048);
		}
		
		if(GUI.Button(new Rect(400,200,100,50),"UnZipClass"))
		{
			string ConfigPath = System.IO.Path.Combine(DeviceInfo.StreamingPath,"Config.zip");
			//string ConfigMD5 = MessageDigest_Algorithm.getFileMd5Hash(ConfigPath);
			Debug.Log( Core.Data.guideManger.getBasePath() );

			string []FileProperties=new string[2]; 
			//待解压的文件   
			FileProperties[0]=ConfigPath;
			//解压后放置的目标目录    
			FileProperties[1]= Core.Data.guideManger.getBasePath();		
			UnZipClass UnZc=new UnZipClass();   
			UnZc.UnZip(FileProperties);
		}
		
		if(GUI.Button(new Rect(400,270,100,50),"GetMD5"))
		{
		    string ConfigPath = System.IO.Path.Combine(DeviceInfo.StreamingPath,"Config.zip");
			string ConfigMD5 = MessageDigest_Algorithm.getFileMd5Hash(ConfigPath);
			Debug.Log(ConfigMD5);
		}
		
		if(GUI.Button(new Rect(400,340,100,50),"DownLoad"))
		{
			 string ConfigPath = System.IO.Path.Combine(DeviceInfo.StreamingPath,"Config.zip");
			 string ConfigMD5 = MessageDigest_Algorithm.getFileMd5Hash(ConfigPath);
			 test_DownloadResource(ConfigMD5);
		}
	}
	
	
	
	#region MyRegion
	void test_DownloadResource (string md5) 
	{
		HttpDownloadTask task = new HttpDownloadTask(ThreadType.MainThread, TaskResponse.Igonre_Response);
		
		string url = @"http://192.168.1.110:10000/ball/www/image/60101-3.jpg";
		string fn = "60101-3.jpg";
		string path = Core.Data.guideManger.getBasePath();
		long size = 103234;
		task.AppendDownloadParam(url, fn, path, md5, size);

		task.DownloadStart += (string durl) => { ConsoleEx.DebugLog("Download starts and url = " + durl); };
		 
		task.taskCompeleted += () => { ConsoleEx.DebugLog("Download success."); };
		task.afterCompleted += testDownloadResp;
		task.ErrorOccured += (s,t) => { ConsoleEx.DebugLog("Download failure.");};

		task.Report += (cur,total) => {ConsoleEx.DebugLog("current Bytes = " + cur + ", total Bytes = " + total);};

		task.DispatchToRealHandler();
	}
	
	public void testDownloadResp(BaseHttpRequest t, BaseResponse r) 
	{
		
	}
	
	#endregion
	
	
}
