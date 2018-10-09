using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;

public class FloorTextureManager
{
    
	public static string localUrl = DeviceInfo.ArtPath;
#if TEST
    public static string downloadUrl = "http://192.168.1.110:10000/ball/www/image";
#else
	public static string downloadUrl = "http://42.121.116.36/ball/www/image";
#endif

//	#if Google
//	public static string downloadNewUrl = "http://cdn.myappblog.net/image-jp";
//	#else
	public static string downloadNewUrl = "http://cdn.myappblog.net/image";
//	#endif
    public const string localPath = "Cartoon/";
    private static List<string> localFirstChapter = new List<string>();

    private static List<string> existName = new List<string>();
    private static FloorTextureManager instance;
    public static FloorTextureManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new FloorTextureManager();

            }
            return instance;
        }
    }

    static void SaveLocalFirst(){
        localFirstChapter = new List<string>();
        if(localFirstChapter != null && localFirstChapter.Count == 0 ){
            int startChapterId = 60101;
            for(int i=0;i<16;i++ ){
                string tStr_1 = (startChapterId +i)+ "-1.jpg";
                string tStr_2 = (startChapterId +i)+ "-2.jpg";
                // ConsoleEx.Write(tStr_1 ,"yellow");
                localFirstChapter.Add(tStr_1);
                localFirstChapter.Add(tStr_2);
            }
        }
    }

    //检测是否 再  第一章中 
    public static bool CheckIsExist(string picName){

        SaveLocalFirst();
        if(!string.IsNullOrEmpty(picName)){
          
            if (localFirstChapter.Contains(picName))
            {
                return true;
            }
            else
                return false;
        }
        return false;
    }
    
    static void AddExistName(string _name)
    {
        _name = GetFileName(_name);
        if(!string.IsNullOrEmpty(_name) && !CheckExist(_name))
        {
            existName.Add(_name);
        }
    }

    public static void SaveTexture(string _name, byte[] data)
    {
        if(! CheckExist(_name))
        {
            SaveTextureData sd = new SaveTextureData();
            sd.name = _name;
            sd.data = data;
            Thread thread = new Thread(new ParameterizedThreadStart(FloorTextureManager.ThreadSave));
            thread.Start(sd);
            thread.Join();
        }
    }


    public class SaveTextureData
    {
        public string name;
        public byte[] data;
    }

    static void ThreadSave(object data)
    {
        if(data != null)
        {
            SaveTextureData sd = data as SaveTextureData;
            if(sd != null)
            {
                try
                {
                    File.WriteAllBytes (Path.Combine(localUrl, GetFileName(sd.name)), sd.data);
                    AddExistName(sd.name);
                }
                catch(IOException e)
                {
                    UnityEngine.Debug.Log(e);
                }
            }
        }
    }
    
    public static bool CheckExist(string _name)
    {

        SaveLocalFirst();

        if (localFirstChapter.Contains(_name))
        {
            //       Debug.Log("   local first contains ");
            return true;
        }
        return existName.Contains(GetFileName(_name));
    }
    
    public static void LoadExist()
    {
        ProcessDirectory(localUrl);
    }

    static void ProcessDirectory(string targetDirectory) 
    {
        // Process the list of files found in the directory.
        string [] fileEntries = Directory.GetFiles(targetDirectory);
        foreach(string fileName in fileEntries)
            ProcessFile(fileName);
//        
//        // Recurse into subdirectories of this directory.
//        string [] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
//        foreach(string subdirectory in subdirectoryEntries)
//            ProcessDirectory(subdirectory);
    }

    static void ProcessFile(string fileName)
    {
        string ret = GetFileName(fileName);
        if(! string.IsNullOrEmpty(ret))
        {
            AddExistName(ret);
        }
    }

    public static string GetFileName(string fullName)
    {
        string fileName = "";
        if(! string.IsNullOrEmpty(fullName))
        {
            if(fullName.IndexOf(Path.DirectorySeparatorChar) != -1)
            {
                fileName = fullName.Substring(fullName.LastIndexOf(Path.DirectorySeparatorChar) + 1);
            }
            else
            {
                fileName = fullName;
            }
        }
        return fileName;
    }


}
