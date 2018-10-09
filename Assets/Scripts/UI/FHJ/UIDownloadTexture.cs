using UnityEngine;
using System.Collections;
using System;

public class UIDownloadTexture
{
    public delegate void endLoadLocal(Texture tex);
    public endLoadLocal EndLoadLocal = null;
    public delegate void endDownload(Texture tex, string _name);
    public endDownload EndDownload = null;
    public System.Action<string> DownloadError = null;
    public delegate void downloadProgress(float progress);
    public downloadProgress DownloadProgress = null;

    public IEnumerator LoadLocal(string name)
    {
        bool isGo = false;
       
        if (FloorTextureManager.CheckIsExist(name))
        {
            string strName =(FloorTextureManager.localPath + name).Replace(".jpg","");
            Texture2D nTexture = Resources.Load(strName, typeof(Texture2D))as Texture2D;

            //  Debug.Log(" load texture " + strName);
            if (nTexture != null)
            {
                if (EndLoadLocal != null)
                {
                    EndLoadLocal(nTexture);
                }
                isGo = true;
                //     Debug.Log(" load  is  done ");
            }
        }
        if (!isGo)
        {
            string url = "file:///" + System.IO.Path.Combine(FloorTextureManager.localUrl, FloorTextureManager.GetFileName(name));
            Debug.Log(" Load local =============== " + name);
            WWW www = new WWW(url);
            //定义www为WWW类型并且等于所下载下来的WWW中内容。
		
            yield return www;
            //返回所下载的www的值
            if (string.IsNullOrEmpty(www.error))
            {
                if (EndLoadLocal != null)
                {
                    Debug.Log("local load is OK");
                    EndLoadLocal(www.texture);
                }
            }
        }
    }
    
    public IEnumerator Download(string name)
    {
		string url = System.IO.Path.Combine(FloorTextureManager.downloadNewUrl, FloorTextureManager.GetFileName(name));

        // Debug.Log("  down load    == "  + name  );
        WWW www = new WWW (url);
		//Debug.Log(url);
        //定义www为WWW类型并且等于所下载下来的WWW中内容。
        while(! www.isDone)
        {
            Progress(www);
            yield return 1;
        }
        //返回所下载的www的值
        if(string.IsNullOrEmpty(www.error))
        {   
            FloorTextureManager.SaveTexture(name, www.texture.EncodeToPNG());
            if(EndDownload != null)
            {
                EndDownload(www.texture, name);
            }
        }
        else
        {
            Debug.Log("DownloadError=>" + www.error +"   name  == "+ name);
            if(DownloadError != null)
            {
                DownloadError(name);
            }
        }
    }
    // 预下载
    public  IEnumerator PreDownload(string  nextName){

        if(Core.Data.usrManager.UserConfig.cartoon == 1){ //自动下载 
            if (!FloorTextureManager.CheckExist(nextName))
            {
                //   Debug.Log("   in preDownload   ======   " + nextName);
                string url = System.IO.Path.Combine(FloorTextureManager.downloadNewUrl, FloorTextureManager.GetFileName(nextName));
                WWW www = new WWW(url);
                yield return www;
                if (string.IsNullOrEmpty(www.error))
                {
                    FloorTextureManager.SaveTexture(nextName, www.texture.EncodeToPNG());
                }
                else
                {
                    Debug.LogError(" DownloadError  => " + nextName);
                }
            }

        }

	}

    public void Progress(WWW www)
    {
        if(DownloadProgress!=null && www != null)
        {
            DownloadProgress(www.progress);
        }
    }
}
