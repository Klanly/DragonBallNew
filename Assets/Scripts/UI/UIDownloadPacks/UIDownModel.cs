using UnityEngine;
using System.Collections;
using System;
using UObj = UnityEngine.Object;

public class UIDownModel : MonoBehaviour
{

    public UISlider m_Slider;
    public UILabel m_DownloadSizeLabel;
    public UILabel m_DownloadTimeLabel;
    public UILabel m_BtnDownLoadLabel;

    public delegate void  DelegateLoadFinished(AssetTask aTask);
    public event DelegateLoadFinished m_EventLoadFinished;

    public delegate void  DelegateCloes();
    public event DelegateCloes m_EventCloes;

    private bool m_IsFrist = true;
    private float m_FristTime = 0;
    private string m_CurBundleName;
    private float m_CurDownTime = 1.1f;

    public enum WinType
    {
        WT_None,
        WT_One,
        WT_Two
    }

    public WinType m_WinType = WinType.WT_One;

    private float m_DownloadModelSize = 0;
    private float m_DownloadTime = 0;
    private string color2 = "ffff02";
    private int m_DownloadModelID = -1;
    private string m_DownloadModelName;


    private string m_DownTimeStr;//预计下载时间
    private string m_SecondStr;//秒


    private static UIDownModel _instance;

    public static UIDownModel mInstance
    {
        get
        {
            return _instance;
        }
    }

    public void OnBtnDownloadModel()
    {
        if (m_DownloadModelID != -1)
        {
            DownloadModel(m_DownloadModelID);
        }
    }

    public static void CloesWin()
    {   
        if (mInstance != null)
        {
            Destroy(mInstance.gameObject);
            _instance = null;
        }
    }

    public void OnCloes()
    {   
        if (m_EventCloes != null)
        {
            m_EventCloes();
        }
        CloesWin();
    }

    public static void OpenDownLoadUI(int monID, DelegateLoadFinished callBack, Vector3 pos, DelegateCloes close = null, WinType type = WinType.WT_One)
    {
        OpenDownLoadUI(monID, callBack, close, type);
        _instance.transform.localPosition = pos;
    }

    public static void OpenDownLoadUI(int monID, DelegateLoadFinished callBack, DelegateCloes close = null, WinType type = WinType.WT_One)
    {
        if (_instance == null)
        {
            UIDownModel modeWin = UIDownModel.CreateWin(type);
            if (modeWin != null)
            {
                _instance = modeWin;
                if (type == WinType.WT_One)
                {
					if (TeamUI.mInstance != null)
                    {
						modeWin.SetWinRoot(TeamUI.mInstance.m_teamView.gameObject);
                    } else
                    {
                        modeWin.SetWinRoot(DBUIController.mDBUIInstance._TopRoot);
                    }
                } else
                {
                    modeWin.SetWinRoot(DBUIController.mDBUIInstance._TopRoot);
                }         
            }
        }
        _instance.SetDownloadModel(monID, callBack);
        _instance.m_EventCloes = close;
    }

    public static UIDownModel CreateWin(WinType type)
    {    
        UObj obj = null;
        switch (type)
        {
            case WinType.WT_One:
                obj = PrefabLoader.loadFromPack("UIDownloadPacks/UIDownModel");
                break;
            case WinType.WT_Two:
                obj = PrefabLoader.loadFromPack("UIDownloadPacks/UIDownMusha"); 
                break;
        }
        if (obj != null)
        {
            GameObject go = Instantiate(obj) as GameObject;
            UIDownModel uiDpw = go.GetComponent<UIDownModel>();
            return uiDpw;
        } 
        return null;
    }

    public void SetWinRoot(GameObject root)
    {
        gameObject.transform.parent = root.transform;
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.identity;
        gameObject.transform.localScale = Vector3.one;
    }

    public void SetDownloadModel(int id, DelegateLoadFinished LoadFinish)
    {
        m_DownTimeStr = Core.Data.stringManager.getString(90000);//预计下载时间
        m_SecondStr = Core.Data.stringManager.getString(90003);//秒
        m_DownloadModelID = id;
        string prefabName = "pb" + id;
        m_DownloadModelName = prefabName + ".unity3d";
        m_DownloadModelSize = float.Parse(Core.Data.sourceManager.getSouceSize(m_DownloadModelName));
        m_DownloadModelSize = m_DownloadModelSize / 1024/1024;
        string str1 = SetLabelColor(color2, "0");
        SetDownloadSizeLabel(str1 + "/" + m_DownloadModelSize.ToString("F") + "MB");

        m_Slider.value = 0;
        
        m_DownloadTime = m_DownloadModelSize * m_CurDownTime;
        string str = m_DownTimeStr + SetLabelColor(color2, m_DownloadTime.ToString("F")) + m_SecondStr;
        SetDownloadTimeLabel(str);

        m_EventLoadFinished = LoadFinish;

    }

    public void DownloadModel(int id)
    {
        string prefabName = "pb" + id;
        if (Core.Data.sourceManager.IsModelExist(id))
        {
            Debug.Log("prefabName : " + prefabName + "     Error!!!!!!!");
        } else
        {
            AssetTask aTask = new AssetTask(prefabName, typeof(UObj), LoadFinished);
            aTask.AppendCommonParam(id, null, AssetTask.loadType.Both_Download_loadlocal);
            aTask.reportProgress = functionprogress;
            aTask.LoadError = loadError;
            //再通过WWW加载
            aTask.DispatchToRealHandler();

        }
    }

    private void loadError(string error){
        Debug.Log("error : " + error);
        string str = Core.Data.stringManager.getString(90007);//资源不存在，敬请期待！！！！
        //SQYAlertViewMove alertView = 
		SQYAlertViewMove.CreateAlertViewMove(str);
        //alertView.gameObject.transform.localPosition = new Vector3(0,0,-800);
        m_DownloadModelID = -1;
        m_DownloadModelName = string.Empty;
        OnCloes();
    }

    private void LoadFinished(AssetTask aTask)
    {
        SouceData data = new SouceData();
        data.FileName = aTask.AssetBundleName;
        data.Num = Core.Data.sourceManager.getNewNum(aTask.AssetBundleName);
        data.Size = Core.Data.sourceManager.getSouceSize(aTask.AssetBundleName);
		data.isError = 1;
        Core.Data.sourceManager.AddDownloadRecordAndSaveToLocaldisk(data);
        //    Caching.CleanCache();
        if (m_EventLoadFinished != null)
        {
            m_EventLoadFinished(aTask);
        }

        m_DownloadModelID = -1;
        m_DownloadModelName = string.Empty;
        CloesWin();
    }

    private void functionprogress(float value)
    {

        if (value <= 0)
        {
            m_FristTime = Time.time;
        }
        if (m_IsFrist && value > 0)
        {
            m_CurDownTime = (Time.time - m_FristTime) * 60;
            //Debug.Log("m_CurDownTime : " + m_CurDownTime);
            m_IsFrist = false;
        } else if (value > 0 && value < 1)
        {
            if (Time.time - m_FristTime >= 0.2f)
            {
                float size = m_DownloadModelSize * value;
                string str1 = SetLabelColor(color2, size.ToString("F"));
                SetDownloadSizeLabel(str1 + "/" + m_DownloadModelSize.ToString("F") + "MB");
                m_DownloadTime = (m_DownloadModelSize - size ) * m_CurDownTime;

                string str = m_DownTimeStr + SetLabelColor(color2, m_DownloadTime.ToString("F")) + m_SecondStr;
                SetDownloadTimeLabel(str);
            }
        }
        //Debug.Log("value : "+value);
        m_Slider.value = value;
    }

    private void SetDownloadSizeLabel(string text)
    {
        m_DownloadSizeLabel.text = text;
    }

    private void SetDownloadTimeLabel(string text)
    {
        m_DownloadTimeLabel.text = text;
    }

    private void SetBtnDownLoadLabel(string text)
    {
        m_BtnDownLoadLabel.text = text;
    }

    private string SetLabelColor(string color, string content)
    {
        return "[" + color + "]" + content + "[-]";
    }

}
