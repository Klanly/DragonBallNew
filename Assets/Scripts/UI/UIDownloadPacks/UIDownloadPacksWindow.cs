using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIDownloadPacksWindow : MonoBehaviour
{
	//当前的进度
    public UISlider m_Slider;
	//总进度
	public UISlider m_SLiderTotal;

    public GameObject m_DownloadBtn;
    public UILabel m_DownloadLabel;
    public UILabel m_DownloadTimeLabel;
    public UILabel m_DownloadSizeLabel;

    public GameObject m_ReceiveBtn;
    public GameObject m_CheckBackground;
    public GameObject m_CheckReceive;
	public UISprite Spr_Receive;

    public delegate void DelegateFinished(int type);

    public event DelegateFinished m_EventFinished;

    public enum BtnState
    {
        BS_None,
        BS_Down,
        BS_Stop,
        BS_Receive
    }

    private BtnState m_BtnState = BtnState.BS_None;

    private static UIDownloadPacksWindow _instance;

    public static UIDownloadPacksWindow m_Instance
    {
        get
        {
            return _instance;
        }
    }

    private delegate void DelegateDownloadModel(int index);

    private event DelegateDownloadModel m_EventDownloadModel;

    private bool m_IsFrist = true;
    private float m_FristTime = 0;
    private string m_CurBundleName;
    private float m_CurDownTime = 1.1f;

	/// <summary>
	/// True需要下载， False不需要下载
	/// </summary>
    private bool m_IsDownload = true;
    private List<SouceData> m_SouceDatas = new List<SouceData>();
    private int m_DownloadTime;
    private float m_SouceSizes;
    private float m_CurSouceSizes;

    private string m_DownTimeStr;//预计下载时间
    private string m_HourStr;//时
    private string m_MinuteStr;//分
    private string m_SecondStr;//秒
    private string m_DownloadStr;//下载
    private string m_StopStr;//暂停
    private string m_DownEndStr;//下载礼包已下载完毕 

	//本次需要下载模型的数量
	private int totalCount = 0;
	//当前下载的模型数量的进度
	private int curCount = 0;
	//当前下载总进度比例
	private float curTotalRatio = 0f;

	private SourceManager sourceMgr = null;

    public void Start()
    {
		StringManager strMgr = Core.Data.stringManager;
		sourceMgr     = Core.Data.sourceManager;

		m_DownTimeStr = strMgr.getString(90000);//预计下载时间
		m_HourStr     = strMgr.getString(90001);//时
		m_MinuteStr   = strMgr.getString(90002);//分
		m_SecondStr   = strMgr.getString(90003);//秒
		m_DownloadStr = strMgr.getString(90004);//下载
		m_StopStr     = strMgr.getString(90005);//暂停
		m_DownEndStr  = strMgr.getString(90006);//下载礼包已下载完毕
		//本次需要更新的模型列表
		m_SouceDatas  = sourceMgr.GetUpdateModes();

		totalCount    = m_SouceDatas.Count;
		curCount      = 0;
        
        if (m_SouceDatas.Count > 0)
        {
            UserConfigManager usr = Core.Data.usrManager;
            m_SouceSizes = usr.UserConfig.downAllSize;
			///
			///  记录下总包的大小，第一次打开这个界面才会赋初始值，
			///  也就是说这里面保存的一定是 首次所有需要下载的包的大小
			///
            if (m_SouceSizes <= 0)
            {
                m_SouceSizes = GetSouceSizes(m_SouceDatas); 
                m_SouceSizes = m_SouceSizes / 1024 / 1024;
                usr.UserConfig.downAllSize = m_SouceSizes;
                usr.save();
            }

			///
			/// --- 当前的下载资源的总量，也就是说每次下载成功后都会保存新值 ----
			///
            m_CurSouceSizes = usr.UserConfig.downCurSize;

            m_DownloadTime = (int)((m_SouceSizes-m_CurSouceSizes) * m_CurDownTime);
            string color = "ffff02";
            string str1 = SetLabelColor(color, m_CurSouceSizes.ToString("F"));
            SetDownloadSizeLabel(str1 + "/" + m_SouceSizes.ToString("F") + "MB");
            SetDownloadTimeLabel(GetDownloadTime(m_DownloadTime));
            m_IsDownload = true;
            m_BtnState = BtnState.BS_Down;
            SetDownloadLabel(m_DownloadStr);
            m_ReceiveBtn.SetActive(false);

            m_Slider.value      = 0;
			curTotalRatio       = m_CurSouceSizes / m_SouceSizes;
			m_SLiderTotal.value = curTotalRatio;

        } else {
            m_BtnState = BtnState.BS_Receive;
            m_Slider.value      = 1;
			m_SLiderTotal.value = 1;

            string color1 = "000000";
            string color2 = "ffff02";
            SetDownloadSizeLabel(SetLabelColor(color2, m_DownEndStr));
            SetDownloadLabel(m_DownloadStr);
            SetDownloadTimeLabel(SetLabelColor(color1, m_DownTimeStr+"0"+m_SecondStr));

				

			if(Core.Data.playerManager.RTData.downloadReawrd != 1)
               m_ReceiveBtn.SetActive(true);
			else
				m_ReceiveBtn.SetActive(false);
        }

		Spr_Receive.enabled = Core.Data.playerManager.RTData.downloadReawrd == 1;

    }

    public void OnDestroy()
    {
        m_EventDownloadModel -= DownloadModel;
    }

    public void SetDownloadLabel(string text)
    {
        m_DownloadLabel.text = text;
    }

    public void SetDownloadTimeLabel(string text)
    {
        m_DownloadTimeLabel.text = text;
    }

    public void SetDownloadSizeLabel(string text)
    {
        m_DownloadSizeLabel.text = text;
    }

    public static void OpenDownLoadUI(DelegateFinished callBack)
    {
        UIDownloadPacksWindow win = UIDownloadPacksWindow.CreateWin();
        if (_instance == null)
        {
            if (win != null)
            {
                _instance = win;
                win.SetWinRoot(DBUIController.mDBUIInstance._TopRoot);
            }
        }
        _instance.m_EventFinished = callBack;
    }

    public static UIDownloadPacksWindow CreateWin()
    {
        UnityEngine.Object obj = PrefabLoader.loadFromPack("UIDownloadPacks/UIDownloadPacks");
        if (obj != null)
        {
            GameObject go = Instantiate(obj) as GameObject;
            UIDownloadPacksWindow uiDpw = go.GetComponent<UIDownloadPacksWindow>();
            //DBUIController.mDBUIInstance._TopRoot
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

    public void OnDownloadBtn()
    {
        if (m_BtnState == BtnState.BS_Down || m_BtnState == BtnState.BS_Stop)
        {
            m_IsDownload = !m_IsDownload;
            if (m_IsDownload)  //不需要下载
            {
                m_EventDownloadModel -= DownloadModel;
                SetDownloadLabel(m_DownloadStr);
            } else {          //需要下载
				m_SouceDatas = sourceMgr.GetUpdateModes();

                if (m_SouceDatas.Count > 0)
                {
                    m_Index = 0;
                    DownloadModel(m_Index);
                    m_EventDownloadModel += DownloadModel;
                    SetDownloadLabel(m_StopStr);
                }
            }
        } else if (m_BtnState == BtnState.BS_Receive) {
            Debug.Log("BS_Receive");
        }
		
    }

    public void CheckAfterCompleted (BaseHttpRequest request, BaseResponse response){
        if (response != null && response.status != BaseResponse.ERROR) {
            GetDownloadCheckResponse download = response as GetDownloadCheckResponse;
            if (download.data.award.Count > 0) {
                List<ItemOfReward> Items = new List<ItemOfReward>();
                for (int i = 0; i < download.data.award.Count; i++) {
                    ItemOfReward item = new ItemOfReward();
                    item.ppid = i;
                    item.pid = download.data.award[i][0];
                    item.showpid = 0;
                    item.num = download.data.award[i][1];
                    Items.Add(item);
                }

                m_CheckReceive.GetComponent<UICheckWindow>().SetCheckItems(Items);
                m_CheckReceive.SetActive(true);
                m_CheckBackground.SetActive(true);
            }

		} else {
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(23));
		}
    }

    public void CheckAllErrorBack (BaseHttpRequest request, string error) {
		SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(7365));
		ConsoleEx.DebugLog (" --- CheckAllErrorBack : ---- "+error);
    }

    public void ReceiveAfterCompleted (BaseHttpRequest request, BaseResponse response) {
		ComLoading.Close();
        if (response != null && response.status != BaseResponse.ERROR)
        {
            GetDownloadReceoveResponse downloadReceove = response as GetDownloadReceoveResponse;
            if (downloadReceove.data.p.Length > 0)
            {
                GetRewardSucUI.OpenUI(downloadReceove.data.p,Core.Data.stringManager.getString(5047));
                //SQYMainController.mInstance.SetDownloadFinish();
				Core.Data.playerManager.RTData.downloadReawrd = 1;

				SQYMainController.mInstance.UpdateBagTip();

				//加入背包
				AddRewardToBag(downloadReceove.data.p);
            }
        }
        if (m_EventFinished != null)
        {
            m_EventFinished(1);
        }
        CloesWin();
    }

	//添加到背包中
	void AddRewardToBag(ItemOfReward[] p)
	{
		foreach(ItemOfReward item in p)
		{
			AddRewardToBag(item);
		}
	}

	void AddRewardToBag(ItemOfReward item)
	{	
		ConfigDataType type = DataCore.getDataType(item.pid);
		switch(type)
		{
		case ConfigDataType.Item:
			{
				Core.Data.itemManager.addItem(item);
				break;
			}		
		case ConfigDataType.Monster:
			{		 
				Monster monster = item.toMonster(Core.Data.monManager);
				if(monster!=null)
					Core.Data.monManager.AddMonter(monster);
				break;
			}	
		case ConfigDataType.Equip:
			{
				Core.Data.EquipManager.AddEquip(item);
				break;
			}			
		case ConfigDataType.Gems:
			{
				Core.Data.gemsManager.AddGems(item);
				break;
			}				
		case ConfigDataType.Frag:
			{
				Soul soul = item.toSoul(Core.Data.soulManager);
				if(soul != null)
					Core.Data.soulManager.AddSoul(soul);
				break;
			}		
		}
	}


    public void ReceiveAllErrorBack (BaseHttpRequest request, string error){
		ComLoading.Close();
        Debug.Log ("ReceiveAllErrorBack : "+error);
        if (m_EventFinished != null)
        {
            m_EventFinished(1);
        }
        CloesWin();
    }

    public void OnReceive() {
		ComLoading.Open();
        HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.DOWNLOAD_RECEOVE, new DownloadReceoveParam (int.Parse(Core.Data.playerManager.PlayerID), SoftwareInfo.VersionCode,0));
        task.afterCompleted += ReceiveAfterCompleted;
        task.ErrorOccured += ReceiveAllErrorBack;
        task.DispatchToRealHandler ();
    }

    public void OnCheck() {
        HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
        task.AppendCommonParam (RequestType.DOWNLOAD_CHECK, new DownloadCheckParam (int.Parse(Core.Data.playerManager.PlayerID)));
        task.afterCompleted += CheckAfterCompleted;
        task.ErrorOccured += CheckAllErrorBack;
        task.DispatchToRealHandler ();
    }

    public void OnCheckColes()
    {
//        TweenScale scale = m_CheckReceive.GetComponent<TweenScale>();
//        if (scale)
//        {
//            scale.PlayReverse();
//        }
        m_CheckReceive.SetActive(false);
        m_CheckBackground.SetActive(false);
    }

    public void OnCloseBtn()
    {
        m_EventDownloadModel -= DownloadModel;
        if (m_EventFinished != null)
        {
            m_EventFinished(-1);
        }
        CloesWin();
    }

    public static void CloesWin()
    {   
        if (m_Instance != null)
        {
			if(Core.Data.playerManager.RTData.downloadReawrd == 1 && !Core.Data.sourceManager.DoClientNeedUpdateModles(true))
			{
				SQYMainController.mInstance.m_Download.gameObject.SetActive(false);
				SQYMainController.mInstance.ArrangeLeftBtnPos();
			}
            Destroy(m_Instance.gameObject);
            _instance = null;
        }
    }

    public void DownloadModel(int index)
    {
        //Debug.Log("index : " + index);
        if (m_SouceDatas.Count > index)
        {
            m_CurBundleName = m_SouceDatas [index].FileName;
            m_IsFrist = true;
            m_FristTime = Time.time;
			if (sourceMgr.getSouceExist(m_SouceDatas [index].FileName))
            {
                Debug.Log("FileName : " + m_SouceDatas [index].FileName + " Errr!!!!!!!");
            } else {
                AssetTask aTask = new AssetTask(m_SouceDatas [index].FileName.Replace(".unity3d", ""), typeof(Object), FeatureRight);
                aTask.AppendCommonParam(m_SouceDatas [index].Num, null, AssetTask.loadType.Only_Download);
                aTask.reportProgress = functionprogress;
                aTask.LoadError = loadError;
                //再通过WWW加载
                aTask.DispatchToRealHandler();
            }

			m_SLiderTotal.value = curTotalRatio + (1 - curTotalRatio) * curCount * 1.0f / totalCount;
        } else {
            m_BtnState = BtnState.BS_Receive;
            m_Slider.value = 1;
			m_SLiderTotal.value = 1;

            string color1 = "000000";
            string color2 = "ffff02";
            SetDownloadSizeLabel(SetLabelColor(color2, m_DownEndStr));
            SetDownloadLabel(m_DownloadStr);
            SetDownloadTimeLabel(SetLabelColor(color1, m_DownTimeStr+"0"+m_SecondStr));

			if(Core.Data.playerManager.RTData.downloadReawrd != 1)
				m_ReceiveBtn.SetActive(true);
			else
				m_ReceiveBtn.SetActive(false);


        }
    }


	/// <summary>
	/// 一旦发生了错误，应该弹出一个提示框，让用户来决定是否重新下载
	/// </summary>
	/// <param name="error">Error.</param>
    private void loadError(string error) {
		string content = null, title = null;
		content = Core.Data.stringManager.getString(25);
		title = Core.Data.stringManager.getString(24);
		UIInformation.GetInstance().SetInformation(content, title, ()=> { DownloadModel(m_Index); } , OnDownloadBtn);
    }

    private int m_Index = 0;

    private void FeatureRight(AssetTask task)
    {
		float size = float.Parse(sourceMgr.getSouceSize(task.AssetBundleName)) / 1024 / 1024;
        m_CurSouceSizes += size;

        UserConfigManager usr = Core.Data.usrManager;
        usr.UserConfig.downCurSize = m_CurSouceSizes;
        usr.save();

        string color = "ffff02";
        string str1 = SetLabelColor(color, (m_CurSouceSizes).ToString("F"));
        SetDownloadSizeLabel(str1 + "/" + m_SouceSizes.ToString("F") + "MB");
        m_DownloadTime = (int)((m_SouceSizes - m_CurSouceSizes) * m_CurDownTime);
        SetDownloadTimeLabel(GetDownloadTime(m_DownloadTime));
        m_Index++;
		curCount++;

        if (m_EventDownloadModel != null) {
            m_EventDownloadModel(m_Index);
		} else{
			m_SLiderTotal.value = curTotalRatio + (1 - curTotalRatio) * curCount * 1.0f / totalCount;
		}



        SouceData data = new SouceData();
        data.FileName  = task.AssetBundleName;
		data.Num  = sourceMgr.getNewNum(task.AssetBundleName);
		data.Size = sourceMgr.getSouceSize(task.AssetBundleName);
		data.isError = 1;
		sourceMgr.AddDownloadRecordAndSaveToLocaldisk(data);
    }

	public void ForceJumpEnd(AssetTask task){
		m_Index++;
		curCount++;

		if (m_EventDownloadModel != null) {
			m_EventDownloadModel(m_Index);
		} else{
			m_SLiderTotal.value = curTotalRatio + (1 - curTotalRatio) * curCount * 1.0f / totalCount;
		}
		SouceData data = new SouceData();
		data.FileName  = task.AssetBundleName;
		data.Num  = 1;
		data.Size = sourceMgr.getSouceSize(task.AssetBundleName);
		data.isError = 0;
		sourceMgr.AddDownloadRecordAndSaveToLocaldisk(data);
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
        }
		else if (value > 0 && value < 1)
        {
            if (Time.time - m_FristTime >= 0.2f)
            {
				float size    = float.Parse(sourceMgr.getSouceSize(m_CurBundleName)) / 1024 / 1024;
                float curSize =  size * value;
                string color  = "ffff02";
                string str1   = SetLabelColor(color, (m_CurSouceSizes + curSize).ToString("F"));
                SetDownloadSizeLabel(str1 + "/" + m_SouceSizes.ToString("F") + "MB");
                m_DownloadTime= (int)((m_SouceSizes - (m_CurSouceSizes + curSize)) * m_CurDownTime);
                SetDownloadTimeLabel(GetDownloadTime(m_DownloadTime));
                m_FristTime   = Time.time;
            }
        }
        //Debug.Log("value : " + value);
        //预计下载时间10秒
        m_Slider.value = value;

    }


	/// <summary>
	/// 获取当前需要下载包的文件总大小
	/// </summary>
	/// <returns>The souce sizes.</returns>
	/// <param name="souceDatas">Souce datas.</param>
    private float GetSouceSizes(List<SouceData> souceDatas)
    {
        float sizes = 0f; 
		foreach (SouceData data in souceDatas) {
			try {
				float val = System.Convert.ToSingle(sourceMgr.getSouceSize(data.FileName));
				sizes += val;
			} catch(System.Exception ex) {
				ConsoleEx.DebugLog(ex.Message);
			}
		}
			
        return sizes;
    }

    private string GetDownloadTime(int time)
    {
        int hour;
        int minute;
        int second;
        second = time % 60;
        minute = ((time - second) / 60) % 60;
        hour = (time - second) / 3600;
        string color1 = "000000";
        string color2 = "ffff02";
        string strDownloadLabel = SetLabelColor(color1, m_DownTimeStr);
        string strHour = SetLabelColor(color2, "{0}") + SetLabelColor(color1, m_HourStr);
        string strMinute = SetLabelColor(color2, "{1}") + SetLabelColor(color1, m_MinuteStr);
        string strSecond = SetLabelColor(color2, "{2}") + SetLabelColor(color1, m_SecondStr);
        return strDownloadLabel + string.Format(strHour + strMinute + strSecond, hour, minute, second);
    }

    private string SetLabelColor(string color, string content)
    {
        return "[" + color + "]" + content + "[-]";
    }

}
