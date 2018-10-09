using UnityEngine;
using System.Collections;
using System;
using System.IO;
using fastJSON;
using System.Collections.Generic;
using System.Text;

public class SouceData
{
	public string FileName;
	public int Num;
    public string Size;
	//wxl    0有错   1对的
	public int isError;
	public SouceData(){}
	public SouceData(string filename,int num,string size ,int isErr)
	{
		FileName = filename;
		Num = num;
        Size = size;
		isError = isErr;

	}
}

public class GameSourceData
{
	public string Game;
	public string Platform;
	public string Version;
	public SouceData[] Files;
}

public class SourceManager : Manager {
	
	
	public GameSourceData localSource;
	public GameSourceData newSource;
	private Dictionary<string,SouceData> LocalSouceData;
	private Dictionary<string,SouceData> NewSouceData;

	
	public SourceManager()
	{
		LocalSouceData = new Dictionary<string, SouceData>();
		NewSouceData = new Dictionary<string, SouceData>();
	}
	
	public override bool loadFromConfig ()
	{
        string OldConfigPath = Path.Combine(DeviceInfo.PersistRootPath,"VerRes.bytes");
		localSource = ReadSourceFile(OldConfigPath);
		if(localSource != null) {
			foreach(SouceData data in localSource.Files) {
				LocalSouceData.Add(data.FileName,data);
			}
		}
		string NewConfigPath = Path.Combine(DeviceInfo.PersistRootPath,"Config/VerRes.bytes");
		newSource = ReadSourceFile(NewConfigPath);
		if(newSource != null && newSource.Files != null)
		{
			foreach(SouceData data in newSource.Files)
				NewSouceData.Add(data.FileName,data);
		}

		return true;
	}
	

	
	//读取资源表(与其他表格式不一样,单独处理)
	public GameSourceData ReadSourceFile(string path)
    {		
        string encrpytStr = null;
		GameSourceData data = null;
		
		if(File.Exists(path))
		{	
	        try
	        {
	            // Create an instance of StreamReader to read from a file.
	            // The using statement also closes the StreamReader.
	            using (StreamReader sr = new StreamReader(path))
	            {
	                // Read and display lines from the file until the end of the file is reached.
	                encrpytStr = sr.ReadToEnd();
					data = JSON.Instance.ToObject(encrpytStr,typeof(GameSourceData)  ) as GameSourceData ;
					sr.Close();
	            }
	        }
	        catch (Exception e)
	        {
	            // Let the user know what went wrong.
				ConsoleEx.DebugLog(e.Message);
	        }
		}
        return data;
    }

    public bool getSouceExist(string souceName)
    {
        bool result = false;
        int localNum = getLocalNum(souceName);
        int newdNum = getNewNum(souceName);
		int newDError = getNewSourceError (souceName);
//		Debug.Log ( " source Name == " + souceName + "   local  num  = " + localNum  + "    new num  == "  + newdNum  +  "   in error  "+ getErrorNum(souceName));
		if(localNum != -1 && newdNum != -1 && localNum == newdNum && newDError != 0)
        {
            result = true;
        }
        else
        {
            result = false;
        }
        return result;
    }


	//模型文件是否存在
	public bool IsModelExist(int monID)
	{
		//先判断本地与没有
		UnityEngine.Object temp_R = ModelLoader.get3DModel(monID);
		if (temp_R == null)
		{
			//本地没有，在下载资源里找
			string prefabName = "pb" + monID.ToString () + ".unity3d";
			return getSouceExist (prefabName);
		}

		return true;
	}
	
    public string getSouceSize(string souceName){
        if(NewSouceData.ContainsKey(souceName))
            return NewSouceData[souceName].Size;
        else
            return "0";
    }

	//获取本地配表里 是否存在
	public int getLocalNum(string oldsouceName)
	{
		if(LocalSouceData.ContainsKey(oldsouceName))
		    return LocalSouceData[oldsouceName].Num;
		else
			return -1;
	}

	public int getNewNum(string newsouceName)
	{
		if(NewSouceData.ContainsKey(newsouceName))
		    return NewSouceData[newsouceName].Num;
		else
			return -1;
	}

	public int getNewSourceError(string newSourceName){
		SouceData tData = null;
		if(LocalSouceData.TryGetValue(newSourceName,out tData))
		{
			if(tData!= null)
				return tData.isError;
		}
		return 0;

	}

	
	//添加一个下载记录
	public void AddDownloadRecord(SouceData data) {
		if(!LocalSouceData.ContainsKey(data.FileName)) {
			LocalSouceData.Add(data.FileName,data);
        } else {
            LocalSouceData[data.FileName] = data;
        }
	}
	//添加一个下载记录,并立刻保存到本地磁盘
	public void AddDownloadRecordAndSaveToLocaldisk(SouceData data)
	{
		AddDownloadRecord(data);
		SaveToLocaldisk();
	}


	//保存到本地磁盘
    //全部覆盖
	public void SaveToLocaldisk()
	{
		if( localSource == null ) localSource = new GameSourceData();
		localSource.Game = newSource.Game;
		localSource.Platform = newSource.Platform;
		localSource.Version = newSource.Version;

        int count = LocalSouceData.Values.Count;
        localSource.Files = new SouceData[count];

        int i = 0;
        foreach(SouceData sd in LocalSouceData.Values) {
            localSource.Files[i ++] = sd;
        }

		string jsonStr = JSON.Instance.ToJSON(localSource);
        string localConfigPath = Path.Combine(DeviceInfo.PersistRootPath,"VerRes.bytes");

	    try
        {
            using (StreamWriter sr = new StreamWriter(localConfigPath))
            {
			    sr.Write(jsonStr);
				sr.Close();
            }
        } catch (Exception e) {
            ConsoleEx.DebugLog(e.Message);
        }
	}

//	//用于记录下载错误模型的配表		wxl 
//	public void SaveToLocalErrorDisk(){
//		
//	}


	/// <summary>
	/// 获取当前版本需要更新的模型	列表
	/// </summary>
	/// <returns>The update mode.</returns>
	public List<SouceData> GetUpdateModes() {
		List<SouceData> curModes = new List<SouceData>();
		foreach(KeyValuePair<string,SouceData> pair in NewSouceData){
			SouceData date = null;
			if(LocalSouceData.TryGetValue(pair.Key,out date)){
				if(NewSouceData[pair.Key].Num != date.Num){
					curModes.Add(date);
				}
			}else{
				curModes.Add(pair.Value);
			}
		}
		return curModes;
	}

	/// <summary>
	/// 当前的客户端需要更新3D模型资源吗
	/// </summary>
	/// <returns><c>true</c>, if client need update modles was done, <c>false</c> otherwise.</returns>
	/// change by jc    2014.11.21
	int isClientNeedDownload = -1;    //  -1 未初始化     0 :不需要下载    1:需要下载 
	//强制检测
	public bool DoClientNeedUpdateModles(bool CheckMandatory = false) 
	{
//		#region 假数据
//		return false;
//		#endregion

		if(isClientNeedDownload == -1 || CheckMandatory)
		{
			isClientNeedDownload = 0;
			foreach(KeyValuePair<string,SouceData> pair in NewSouceData)
			{
				SouceData date = null;
				if(LocalSouceData.TryGetValue(pair.Key,out date))
				{
					if (NewSouceData [pair.Key].Num != date.Num) {
						isClientNeedDownload = 1;	
						break;
					} 
					else {
						if (LocalSouceData [pair.Key].isError == 0) {
//							isClientNeedDownload = 1;	
						}
					}
				}
				else
				{
					//老配表里都没有这个项目
					Debug.Log (" null   data  =======    " + NewSouceData[pair.Key].FileName);
					isClientNeedDownload = 1;
					break;
				}	
			}
		}

		bool result = isClientNeedDownload == 0 ? false:true;
		return result;
	}

//	public bool DoClientNeedUpdateModles() {
//
//		bool requireDownload = true;
//
//		//在这些都不为空的情况下，判定资源是否需要下载？
//		if(Core.Data != null && Core.Data.playerManager != null && Core.Data.playerManager.RTData != null ) 
//		{
//			requireDownload = Core.Data.playerManager.RTData.downloadReawrd == 0;
//
//			if(requireDownload == false) 
//			{
//
//				foreach(KeyValuePair<string,SouceData> pair in NewSouceData)
//				{
//					SouceData date = null;
//					if(LocalSouceData.TryGetValue(pair.Key,out date))
//					{
//						if(NewSouceData[pair.Key].Num != date.Num)
//						{
//							requireDownload = true;
//							break;
//						}
//					}
//					else
//					{
//						//老配表里都没有这个项目
//						requireDownload = true;
//						break;
//					}
//				}
//
//			}
//		}
//
//		return requireDownload;
//	}

    public List<SouceData> GetLocalSouceData(){
        List<SouceData> curModes = new List<SouceData>();
        foreach (KeyValuePair<string,SouceData> pair in LocalSouceData)
        {
            curModes.Add(pair.Value);
        }
        return curModes;
    }

    /// <summary>
    /// 添加一个，并且写入数据
    /// </summary>
    /// <param name="data">Data.</param>
    public void AppendOne(SouceData data) {
        if(!LocalSouceData.ContainsKey(data.FileName)) {
            LocalSouceData.Add(data.FileName,data);
            SaveToLocalDiskAppend(JSON.Instance.ToJSON(data));
        } else {
            LocalSouceData[data.FileName] = data;
        }
    }

    //附加的方式写入文件
    //只支持AscII
    public static void SaveToLocalDiskAppend(string toBeWritten) {
        if(!string.IsNullOrEmpty(toBeWritten)) {
            toBeWritten = "," + toBeWritten + "]}";

            byte[] written = Encoding.ASCII.GetBytes(toBeWritten); 

            string localResConfigPath = Path.Combine(DeviceInfo.PersistRootPath,"VerRes.bytes");

            try {
                using(FileStream fs = File.Open(localResConfigPath, FileMode.Open)){
                    fs.Seek(-2, SeekOrigin.End);
                    fs.Write(written, 0, written.Length);
                    fs.Flush();
                }
            } catch(Exception e){
                ConsoleEx.DebugLog(e.Message);
            }
        }
    }

	public void Clear()
	{
		isClientNeedDownload = -1;
		Core.Data.playerManager.RTData.downloadReawrd = -1;
	}
}
