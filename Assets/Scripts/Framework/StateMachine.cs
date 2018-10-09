using System;
#if DEBUG
using System.Collections.Generic;
#endif
using UnityEngine;

public enum States
{
    ON_LAUNCHED,
    ON_LOCALREADY,
    //ON_NETREADY is standard and runable situation.
    ON_NETREADY,
    ON_DOWNLOADING,
    ON_QUIT,
    ON_PAUSE,
    ON_RESUME,
}

public class StateMachine {

	public RuntimePlatform rtPlat {
		get;
		private set;
	}

	public States curStates {
		get;
		private set;
	}

	public Server curServer {
		get;
		private set;
	}

	//游戏暂停时候的时间
	private DateTime pauseTime;

    /**
     * When Game is launced...
     */ 
    public void onGameLaunched() {
        curStates = States.ON_LAUNCHED;
		//define the current runtime platform.
		rtPlat = Application.platform;

		//We should initialize all necessery things.
		Core.Initialize();

		//Initialize the path on the main thread.
		string temp = null;
		temp = DeviceInfo.PersistRootPath;
		temp = DeviceInfo.StreamingPath;

		if(rtPlat != RuntimePlatform.OSXEditor && rtPlat != RuntimePlatform.WindowsEditor)
			FileUtility.createFolder(DeviceInfo.ConfigDownload);
		FileUtility.createFolder(DeviceInfo.ArtPath);
        temp = string.Empty;

		#if UNITY_ANDROID && !UNITY_EDITOR
		Android_QuitGame.Initialize ();
		#endif

		pauseTime = DateTime.Now;
    }

    /*
     * We must do some work to save game status or clean the memory.
     */ 
    public void onGameQuit(string curLevel) {
        curStates = States.ON_QUIT;
		Core.NetEng.SockEngine.Dispose();
		Core.TimerEng.Dispose();
    }
    /*
     * We must do some work to save game status or clean the memory.
     */
    public void onGamePause(string curLevel) {
		pauseTime = DateTime.Now;
		curStates = States.ON_PAUSE;   
        CloseCurrentSocket();
		Core.OnPause(); 
    }

    void CloseCurrentSocket(){
        SocketTask task = new SocketTask (ThreadType.MainThread, TaskResponse.Default_Response);
        task.AppendCmdParam (InternalRequestType.SHUT_DOWN);
        task.DispatchToRealHandler ();
    }

    void ReconnectActivitySocket(){
        AsyncTask.QueueOnMainThread (() => {
            ActivityNetController.GetInstance().SendLoginMSGPrepare (Core.Data.playerManager.PlayerID, null);
        }, 0.2f);


        if (WXLActivityFestivalController.Instance != null)
        {
             AsyncTask.QueueOnMainThread (() => {
                ActivityNetController.GetInstance().GetScoreRankList();
             }, 0.5f);
        }
    }

    void ReconnectChatSocket(){
        Core.NetEng.SockEngine.OnLogin (new System.Net.DnsEndPoint (Core.SM.curServer.chat_ip, Core.SM.curServer.chat_port));
        MessageMgr.GetInstance ().SendWorldChatLogin ();
    }

    public void onGameResume(string curLevel) {
        curStates = States.ON_RESUME;
        
		if(Application.loadedLevelName != SceneName.LOGIN_SCENE)
		{
			if (Core.NetEng.SockEngine.endPoint.Host == Core.SM.curServer.active_ip && Core.NetEng.SockEngine.endPoint.Port == Core.SM.curServer.active_port) {
				//activity 
				ReconnectActivitySocket();
			} else if ( Core.NetEng.SockEngine.endPoint.Host == Core.SM.curServer.chat_ip && Core.NetEng.SockEngine.endPoint.Port == Core.SM.curServer.chat_port){
				//chat 
				ReconnectChatSocket();
			}

			Core.TimerEng.OnResume();
			#region added by zhangqiang 
			TimeSpan span = DateTime.Now - pauseTime;
			if(span.TotalSeconds >= 10 && !Core.Data.guideManger.isDataSyncing)
			{
				ComLoading.Open();
				//同步招募屋1
				Core.Data.zhaomuMgr.bInit = false;
				Core.Data.zhaomuMgr.SendZhaomuStateMsg ();
				MailReveicer.Instance.SendFightMegRequest(100);

			}
			#endregion
		}

        curStates = States.ON_NETREADY;
    }

	/// <summary>
	/// On the download. we are going to download new resources.
	/// </summary>
    public void onDownload() {
        curStates = States.ON_DOWNLOADING;
        // do something....

    }

    /*
     * When All local configure file is ready.
     */ 
    public void onLocalGameDataIsReady() {
        curStates = States.ON_LOCALREADY;
    }

    /*
     * When Network data is ready.
     */ 
    public void onNetworkGameDataIsReady()
    {
        curStates = States.ON_NETREADY;
    }

    /* Game is going to reset
     */ 
    public void onGameReset(){
    }

	/// <summary>
	/// After Server is selected & user click "login" button
	/// </summary>
	/// <param name="ChosenServer">Chosen server.</param>
	public void onServerSelected(Server ChosenServer, string uniqueId) {
		//we here should set current selected server to the DataPersisteManager.
		curServer = ChosenServer;
		HttpClient.BaseUrl = curServer.url;

		//登陆后第一次要使DataPersistManager继续添加参数
		PathInfo param = new PathInfo();
		param.curServer = curServer.name;
		param.UniqueId = uniqueId;
		ICore ic = Core.DPM as ICore;
		ic.OnLogin(param);

		ic = Core.NetEng as ICore;
		//外网
		ic.OnLogin(new NewworkObj(Core.DPM, new System.Net.DnsEndPoint(ChosenServer.chat_ip, ChosenServer.chat_port)));
    }

    public void beforeLoadLevel(string curLevel, string nextLevel) {
		ConsoleEx.DebugLog("BeforeLoadLevel. curlevel = " + curLevel + "   next level = " + nextLevel );
		
		LastScenesName = curLevel;
		CurScenesName = nextLevel;
		
		if(curLevel == SceneName.GAME_BATTLE && nextLevel == SceneName.MAINUI) {
			TimeMgr.getInstance().setBaseLine(1.0f);
		}

		switch(nextLevel) {
		case SceneName.LOGIN_SCENE:
			Core.Data.soundManager.BGMPlay(SceneBGM.BGM_Login);
			break;
		case SceneName.GAME_BATTLE:
			Core.Data.soundManager.BGMPlay(SceneBGM.BGM_BATTLE);
			break;
		case SceneName.MAINUI:
			Core.Data.soundManager.BGMPlay(SceneBGM.BGM_GAMEUI);
			break;
        case SceneName.GameMovie:
			Core.Data.soundManager.BGMStop();
            break;
		}

    }

    public void afterLoadLevel(string preLevel, string curLevel) {
		ConsoleEx.DebugLog("AfterLoadLevel. curlevel = " + preLevel + "   next level = " + curLevel );
		
		GameObject go = PrefabLoader.loadFromUnPack("Allen/NetworkExceptionHandler", false) as GameObject;
		GameObject g = GameObject.Instantiate(go) as GameObject;

		var gobal = GameObject.FindGameObjectWithTag("Gobal");
		if(gobal != null) g.transform.parent = gobal.transform;

		go = null;
		gobal = null;

	}


	/// <summary>
	/// 开始记录下，第二天凌晨12点的时间。防止有部分用户会在晚上玩游戏之后不关闭游戏，导致服务器状态和客户端状态不一致
	/// 与此同时，也记录下当天晚上9点的时间。
	/// </summary>
	public void recordDayChanged(long sysTime) {

		//
		//------ 当天结束 -------
		//
		long LeftOfDayEnd = DateHelper.getLeftTimingbeforeDayChanged(sysTime);
		ConsoleEx.DebugLog("Start to recording Day Changed.. leaving seconds = " + LeftOfDayEnd, ConsoleEx.YELLOW);
		long now = Core.TimerEng.curTime;
		TimerTask task = new TimerTask(now, now + LeftOfDayEnd, 1);
		task.onEventEnd = OnDayChanged;
		task.DispatchToRealHandler();


		//
		// ------ 当天晚上9点 -------
		//
		long LeftOfNineNight = DateHelper.getNineNight(sysTime);
		ConsoleEx.DebugLog("Start to recording 21:00.. leaving seconds = " + LeftOfNineNight, ConsoleEx.YELLOW);
		if(LeftOfNineNight > 0) {
			TimerTask task2 = new TimerTask(now, now + LeftOfNineNight, 1);
			task2.onEventEnd = OnNineChanged;
			task2.DispatchToRealHandler();
		}
	}

	void OnDayChanged (TimerTask tTask) {
		///
		/// 新手引导的时候不能执行
		/// 
		if(Core.Data.guideManger.isGuiding) { 
			return;
		}

		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.MESSAGE_INFORMATION, new PlayerIDParam(Core.Data.playerManager.PlayerID, 100));
		task.afterCompleted = (BaseHttpRequest request, BaseResponse response) => { 
			recordDayChanged(Core.Data.playerManager.RTData.systemTime);  

			///
			/// -------- 弹出提醒用户签到 --------
			///
			if(CurScenesName == SceneName.MAINUI) {
				string content = Core.Data.stringManager.getString(55);
				string btn = Core.Data.stringManager.getString(5030);
				UIInformation.GetInstance().SetInformation(content,btn, DBUIController.mDBUIInstance.SyncBackToMainUI, DBUIController.mDBUIInstance.SyncBackToMainUI);
			}
		};
		task.ErrorOccured = (BaseHttpRequest request, string error) => { ConsoleEx.DebugLog("SendFightRequest is error!"); };
		task.DispatchToRealHandler();
	}

	void OnNineChanged(TimerTask tTask) {
		FinalTrialMgr.GetInstance ().SendFirstRansMsg ();
	}

	/// 
	/// 当用户注销的时候，这个必须被调用, 而且这个方法必须在跳场景之后
	/// 
	public void OnUnregister() {
		/// *********  注销的设定 ********
		mUnregister = true;
		///
		/// 清理所有的底层信息，
		///

		Core.TimerEng.deleteAllTask();
		AsyncTask.RemoveAllDelayedWork();

		///
		/// 网络地址必须连接到UserCenter
		///

		HttpClient.RevertToUserCenter();

		///
		/// 所有的数据必须全部同步
		///
		Core.Data.rechargeDataMgr.Unregister();
		Core.Data.Unregister();
		Core.Data.itemManager.ClearBagData();
		Core.Data.guideManger.Clear();
		Core.Data.sourceManager.Clear();
		FinalTrialMgr trialMgr = FinalTrialMgr.GetInstance();
		if(trialMgr != null)
			trialMgr.Unregister();

		HttpRequestFactory._sessionId = "empty";
		Core.Data.battleTeamMgr.Unregister();
		#if UNITY_EDITOR
		UniqueGUID.getInstance().Unregister();
		#endif

		///
		/// 清除邮件
		///
		if(MailReveicer.Instance != null) MailReveicer.Instance.DeleteMailReveicer();
		if(MessageMgr.GetInstance() != null)MessageMgr.GetInstance().UnRegister();
		System.GC.Collect();
	}

	private bool mUnregister = false;
	public bool isReLogin {
		get {
			return mUnregister;
		}
	}

	#region Add by jc
	public string LastScenesName{get;set;}
	public string CurScenesName{get;set;}
	#endregion
	
}

