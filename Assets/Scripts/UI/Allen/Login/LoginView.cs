using System;
using UnityEngine;
using System.Collections;
using Framework;
using System.Collections.Generic;
using AW.Data;
using Compression;
using DeCompression;
//打包前请先删除StreamingAssets下的Config文件夹

#region 登陆的状态和登陆的步骤

/// <summary>
/// 主要依据LoginStatus来决定能否跳到第二个场景
/// </summary>
[Flags]
internal enum LoginStatus {
	None         = 0x00,
	Config_Ready = 0x01,
	Login_Ready  = 0x02,
	All          = Config_Ready | Login_Ready,
}

/// <summary>
/// 这个主要控制登陆步骤（用户可能多次重试登陆）
/// </summary>
[Flags]
internal enum LoginStep {
	None                 = 0x00, //只有这个状态下是可以重试登陆的
	CheckConfig_Start    = 0x01,
	DownloadConfig_Start = 0x02,
	Download_OK          = 0x03,
	Login_OK             = 0x04,
	ReadConfig_OK        = 0x05,
	Download_ERROR       = 0x06,

	//只有Android设备有这个状态
	//登陆时拷贝配置表
	#if UNITY_ANDROID
	Prepare_Config       = 0x07,
	#endif
}

#endregion

internal static class StatusExtension {
	public static bool check( this LoginStatus flags, LoginStatus totest ) {
		return (flags & totest) == totest;
	}

	public static LoginStatus set(this LoginStatus flags, LoginStatus totest) {
		return flags | totest;
	}
}

public class LoginView : MonoBehaviour {

	// --------  UI ---------
	public UIButton BtnLogin;
	public UILabel TxtServer;
	//系统级别的公告
	public SpeakLoader SpeakerMgr;

	private LoginStatus status;
	private LoginStep step;
	private Server ChosenServer;
	private Server[] ListOfServer;
	private IGetUniqueID thirdParty;

	/// 
	/// 获取服务器列表是否成功
	/// 
	private bool bGetServerListOk = false;

	private static LoginView _instance;
	public static LoginView mInstance
	{
		get 
		{
			return _instance;
		}
	}


	//只有Android设备才有用，判定用户是否按了这个按钮
	#if UNITY_ANDROID && !UNITY_EDITOR
	private bool User_Click_LoginBtn = false;
	#endif

	public GameObject Content;
	public ConfigLoading configLoading;

	//added by zhangqiang to set game name by different chanels
	//游戏名称
	public UITexture m_picName;
	public UILabel m_txtVersion;

	IEnumerator SetVersionCode()
	{
		yield return new WaitForSeconds(1f);
		string strText = Core.Data.stringManager.getString (5266);
		strText = string.Format (strText, LuaTest.Instance.GetVersionCode);
		m_txtVersion.text = strText;

	}

	void Awake()
	{
		_instance = this;
	}

	void Start() {
		LoginIsNotReady();
		Core.SM.beforeLoadLevel(string.Empty, Application.loadedLevelName);
		Core.SM.afterLoadLevel(string.Empty, Application.loadedLevelName);
	
		Native.mInstace.m_thridParty.getUniqueId((t) => {
				SendGetThirdServerRQ();
			}
		);
			
		#if UNITY_ANDROID && !UNITY_EDITOR
		//准备好配表
		PrepareConfig();
		#endif

		string strPath = "texture/" + SoftwareInfo.gameName.ToString ();
		Texture img = PrefabLoader.loadFromPack (strPath) as Texture;
		if (img != null)
		{
			m_picName.mainTexture = img;
		}
		else
		{
			RED.LogWarning (strPath + "is not exist!");
		}
		if(m_txtVersion != null)
		{
			m_txtVersion.SafeText("");
		}

		StartCoroutine(SetVersionCode());
	}

	#region 网络命令

	/// <summary>
	/// ignore this routine, no more be called.
	/// </summary>
	void getPartitionServer() 
	{
		//如果已经不是在用户中心的状态下，忽略该命令
		if(HttpClient.IsStillInUserCenterMode() == false) return;

		ComLoading.Open ();
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		//HttpTask task = new HttpTask(ThreadType.MainThread); is the same as above line.

		task.AppendCommonParam(RequestType.GET_PARTITION_SERVER, new PartitionServerParam(SoftwareInfo.VersionCode, 1) );

		//task.ErrorOccured += HttpResp_Error;
		task.afterCompleted += HttpResp_UI;

		//then you should dispatch to a real handler
		task.DispatchToRealHandler();
	}


	//发送第三方登录获取服务器列表消息
	void SendGetThirdServerRQ() {
		//如果已经不是在用户中心的状态下，忽略该命令
		if(HttpClient.IsStillInUserCenterMode() == false) return;

		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);

		task.AppendCommonParam(RequestType.THIRD_GET_SERVER, new ThirdGetServerParam() );

		task.ErrorOccured += HttpResp_Error;
		task.afterCompleted += HttpResp_UI;

		//then you should dispatch to a real handler
		task.DispatchToRealHandler();
	}

	//检查更新配表
	void checkConfig() {
		#if UNITY_ANDROID
		if(step == LoginStep.Download_ERROR || step == LoginStep.None || step == LoginStep.Prepare_Config) {
		#else
		if(step == LoginStep.Download_ERROR || step == LoginStep.None) {
		#endif
			MobilePhoneInfo PhoneInfo = Core.Data.extensionManager.iPhoneInfo;
			if(PhoneInfo != null) {
				/*
				Debug.Log("appCurName="+PhoneInfo.appCurName);
				Debug.Log("PhoneInfo="+PhoneInfo.appCurVersion);
				Debug.Log("appCurVersionNum="+PhoneInfo.appCurVersionNum);
				Debug.Log("deviceName="+PhoneInfo.deviceName);
				Debug.Log("identifierNumber="+PhoneInfo.identifierNumber);
				Debug.Log("phoneVersion="+PhoneInfo.phoneVersion);
				Debug.Log("phoneModel="+PhoneInfo.phoneModel);		
				Debug.Log("userPhoneName="+PhoneInfo.userPhoneName);		
				Debug.Log("localPhoneModel="+PhoneInfo.localPhoneModel);
				Debug.Log("appCurVersionNum="+PhoneInfo.appCurVersionNum);
				*/
			}

			//开始检测要不要下载
			step = LoginStep.CheckConfig_Start;

			//MD5  --> config.zip
			string ConfigMD5 = "";	
			string ConfigPath = System.IO.Path.Combine(DeviceInfo.PersistRootPath,"Config.zip");
			if(System.IO.File.Exists(ConfigPath)) {
				ConfigMD5 = MessageDigest_Algorithm.getFileMd5Hash(ConfigPath);	
			}

			ConsoleEx.DebugLog(" ZIPFILE Path= " + ConfigPath +" md5= " + ConfigMD5);

			CMSHttpTask task = new CMSHttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
			task.AppendCommonParam(RequestType.UPDATE_RESOURCES, new CheckConfig((int)RequestType.UPDATE_RESOURCES,ConfigMD5,SoftwareInfo.VersionCode.ToString()) ,"http://114.215.183.29/gs/ms/index.php/configure/check");
			task.ErrorOccured += HttpResp_Error;
			task.afterCompleted += HttpResp_UI;
			//then you should dispatch to a real handler
			task.DispatchToRealHandler();
		} else if(step == LoginStep.ReadConfig_OK) {
			readCompleted();
		}

	}

	private string UniqueId;
	void Login() 
	{
		ComLoading.Open();
		#if CHECKCONFIG
		checkConfig();
		#else
		readLocalConfig();
		#endif
	}


	void SendLoginMsg() {
		if(ChosenServer != null) 
		{
			HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
			//HttpTask task = new HttpTask(ThreadType.MainThread); is the same as above line.

			task.AppendCommonParam(RequestType.LOGIN_GAME, new LoginParam(UniqueId, ChosenServer.sid.ToString()));

			task.afterCompleted += HttpResp_UI;
			//task.ErrorOccured += HttpResp_Error;

			//then you should dispatch to a real handler
			task.DispatchToRealHandler();
		}
	}


	void HttpResp_UI (BaseHttpRequest request, BaseResponse response) {

		ConsoleEx.DebugLog(" --- Http Resp - running in the main thread, UI purpose --" + response.GetType().ToString());
		if(response != null && response.status != BaseResponse.ERROR) 
		{
			HttpRequest myRequest = (HttpRequest)request;
			switch(myRequest.Type) 
			{
			case RequestType.GET_PARTITION_SERVER:
				//UI ...
				GetPartitionServerResponse ServerResp = response as GetPartitionServerResponse;
				showServerList(ServerResp);
				LoginIsReady();
				if(ServerResp != null && ServerResp.data != null) SpeakerMgr.autoShow(ServerResp.data.noticeTitle, ServerResp.data.noticeContent);
				break;
			case RequestType.THIRD_GET_SERVER:
				GetPartitionServerResponse resp = response as GetPartitionServerResponse;
				showServerList (resp);
				AccountData ad = Native.mInstace.m_thridParty.GetAccountData ();
				if (!string.IsNullOrEmpty (resp.data.platId)) {
					ad.uniqueId = resp.data.platId;
				}
				ad.token = resp.data.platToken;

				this.UniqueId = resp.data.token;
				LoginIsReady ();
				if(resp != null && resp.data != null) SpeakerMgr.autoShow(resp.data.noticeTitle, resp.data.noticeContent);
				break;

			case RequestType.UPDATE_RESOURCES: {
				//更新资源包
				ConfigResponse r = response as ConfigResponse;
				if(r!=null && r.result) {
					step = LoginStep.DownloadConfig_Start;
					Content.SetActive(false);
					configLoading.gameObject.SetActive(true);
					test_DownloadResource(r);
				} else {					
					Debug.Log("the Config.zip is The latest! Don't need Download.");
					step = LoginStep.Download_OK;
					readLocalConfig();
				}
			}
				break;
			case RequestType.LOGIN_GAME:
				ComLoading.Close ();

				status = status.set (LoginStatus.Login_Ready);
				JumpToGameView ();
				#if Spade
				SpadeIOSLogin spadeSdk = Native.mInstace.m_thridParty as SpadeIOSLogin;
				#if !UNITY_EDITOR
				spadeSdk.NotityLogin(ChosenServer);
				#endif
				#endif
                #if UNITY_IOS && !DEBUG
				// 添加 IOS 本地 push
				IOSLocalPush.getInstance ().notifyLoggedin ();
				#endif
				MessageMgr.GetInstance ().SendWorldChatLogin ();

                ///
                /// --------------- 登陆完成之后，设定日期改变，设定获得活动运营信息 -------
                ///
				if (Core.Data != null && Core.Data.playerManager != null && Core.Data.playerManager.RTData != null)
					Core.SM.recordDayChanged (Core.Data.playerManager.RTData.systemTime);

                if (Core.Data != null && Core.Data.HolidayActivityManager != null)
                    Core.Data.HolidayActivityManager.setHourChanged();

				Core.Data.rechargeDataMgr.SendHttpRequest ();
				break;
			}
		} else {
			ComLoading.Close ();

			//登陆超时了
			if(response.errorCode == 2000) {
				if(Core.Data != null && Core.Data.stringManager != null)
					SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(48), gameObject);

				///
				///  ---- 回滚用户中心
				///
				HttpClient.RevertToUserCenter();
				SendGetThirdServerRQ();
			} else {

				GetPartitionServerResponse ServerResp = response as GetPartitionServerResponse;
				if(ServerResp != null && ServerResp.data != null) SpeakerMgr.autoShow(ServerResp.data.noticeTitle, ServerResp.data.noticeContent);

				if(Core.Data != null && Core.Data.stringManager != null) {
					string word = Core.Data.stringManager.getString(response.errorCode);
					if(!string.IsNullOrEmpty(word)) SQYAlertViewMove.CreateAlertViewMove (word, gameObject);
				}

			}
		}
	}


	void HttpResp_Error (BaseHttpRequest request, string error) {
		ConsoleEx.DebugLog("---- Http Resp - Error has ocurred." + error);
		ComLoading.Close ();
		if(request != null && request is HttpRequest) {
			HttpRequest myRequest = (HttpRequest)request;
			switch(myRequest.Type) {
			case RequestType.GET_PARTITION_SERVER:
			case RequestType.THIRD_GET_SERVER:
//				SQYAlertViewMove.CreateAlertViewMove ("Network is broken down.", gameObject);
				SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(17), gameObject);
				break;
			case RequestType.LOGIN_GAME:
				if(!Content.activeSelf)Content.SetActive(true);
				break;
			}
		}

		if(request != null && request is ThirdPartyHttpRequest) {
			step = LoginStep.Download_ERROR;
		}

	}

	#endregion


	#region 下载zip包
	void test_DownloadResource (ConfigResponse res) 
	{
		configLoading.SetDescribe(Core.Data.stringManager.getString(9027) );
		configLoading.ShowLoading(0);

		HttpDownloadTask task = new HttpDownloadTask(ThreadType.MainThread, TaskResponse.Igonre_Response);

		string url = res.url;
		string fn = "Config.zip";
		string path = DeviceInfo.PersistRootPath;
		Debug.Log("path="+path);
		long size = res.size;
		task.AppendDownloadParam(url, fn, path, res.md5, size);

		task.DownloadStart += (string durl) => { ConsoleEx.DebugLog("Download starts and url = " + durl); };

		task.taskCompeleted += () => { ConsoleEx.DebugLog("Download success."); };
		task.afterCompleted += testDownloadResp;
		task.ErrorOccured += (s,t) => { step = LoginStep.Download_ERROR; ConsoleEx.DebugLog("Download failure.");};

		task.Report += (cur,total) => {
			ConsoleEx.DebugLog("current Bytes = " + cur + ", total Bytes = " + total);
			configLoading.ShowLoading( (float)cur/(float)total );
		};

		task.DispatchToRealHandler();
	}
	//下载完成
	public void testDownloadResp(BaseHttpRequest t, BaseResponse r) 
	{
		step = LoginStep.Download_OK;
		StartCoroutine(RunUnZipFile());
	}

	#endregion

	#region 解压zip包
	IEnumerator RunUnZipFile()
	{
		yield return new WaitForEndOfFrame();

		configLoading.SetDescribe(Core.Data.stringManager.getString(9028));
		configLoading.ShowLoading(0);
		string ConfigPath = System.IO.Path.Combine(DeviceInfo.PersistRootPath,"Config.zip");
		MessageDigest_Algorithm.getFileMd5Hash(ConfigPath);

		string[] FileProperties = new string[2];
		//待解压的文件   
		FileProperties[0] = ConfigPath;
		//解压后放置的目标目录    
		FileProperties[1] = DeviceInfo.PersistRootPath;
		UnZipClass UnZc = new UnZipClass();
		UnZc.UnZip(FileProperties);

		yield return new WaitForEndOfFrame();
		configLoading.ShowLoading(1);

		yield return new WaitForEndOfFrame();
		configLoading.gameObject.SetActive(false);
		//解压完成以后读表
		readLocalConfig();
	}
	#endregion


	#region read Configure files 

	void readLocalConfig() {
		#if UNITY_ANDROID && CHECKCONFIG
		ReLoadAudio();
		#endif

		///
		/// 注销再次登陆的时候，不用读配表
		///
		if(Core.SM != null && Core.SM.isReLogin) {
			readCompleted();
		} else {
			AsyncTask.RunAsync( () => { 
				Core.Data.readLocalConfig();
				readCompleted();
			} );
		}
	}

	#if UNITY_ANDROID

	//重新加载声音。。。因为Android的特殊性
	void ReLoadAudio() {
		Core.Data.soundManager.reloadConfig();
		Core.Data.soundManager.BGMPlay(SceneBGM.BGM_Login);
	}

	void PrepareConfig() {
		///  AndroidOrWebDataCopyOnInstall 点击按钮后会自动从 streamingAsset拷贝到 /packagename/files/Config/目录下
		///  但是如果是CHECKCONFIG，则初始的文件不是由AndroidOrWebDataCopyOnInstall来生成。是从服务器上下载下来。
		///  所以导致ReLoadAudio（）没有执行
		AndroidOrWebDataCopyOnInstall onInstall = new AndroidOrWebDataCopyOnInstall();
		onInstall.ApplicationOn(this, () => {
			step = LoginStep.Prepare_Config;
	#if !UNITY_EDITOR
		if(User_Click_LoginBtn) {
			onButtonClick();
		}
	#endif
		},
			() => { ReLoadAudio(); }
		);
	}

	#endif

	#if FB2
	List<string> allDownLoadSourceName = new List<string>(){"FB2_0.assetbundle","FB2_1.assetbundle","FB2_2.assetbundle","FB2_3.assetbundle","FB2_4.assetbundle","FB2_5.assetbundle"};

	int DownLoadIndex = 0;
	bool isHaveUpdate = false;
	#endif

	//配表都读完
	void readCompleted () 
	{
		status = status.set(LoginStatus.Config_Ready);
		step = LoginStep.ReadConfig_OK;
		AsyncTask.QueueOnMainThread( () => 
			{ 
				#if FB2
				//如果是第二版本的副本,需要登陆前额外下载副本的一些图片
				//Caching.CleanCache();
				StartCoroutine(DownFB2ImgSource(allDownLoadSourceName[DownLoadIndex]));
				#else
				SendLoginMsg();
				#endif
			});
	}
	#endregion

	#if FB2
	IEnumerator DownFB2ImgSource(string fileName)
	{		
		Debug.Log("->old:"+fileName+" v= "+Core.Data.sourceManager.getLocalNum(fileName).ToString()+"  ->new :"+fileName+" v= "+Core.Data.sourceManager.getNewNum(fileName).ToString());
		//只有当版本不同的时候才会去下载
		if(Core.Data.sourceManager.getLocalNum(fileName) != Core.Data.sourceManager.getNewNum(fileName))
		{		
			if(Content.activeSelf) Content.SetActive(false);
			isHaveUpdate = true;
			//Caching.CleanCache();
			string url = @"http://114.215.183.29/down/gameconf/1/"+fileName;
			int version = 1;
			WWW www = WWW.LoadFromCacheOrDownload(url,version);
			configLoading.SetDescribe(Core.Data.stringManager.getString(9031)+"["+fileName+"]");
			while(!www.isDone)
			{			
				configLoading.ShowLoading(www.progress);
				yield return 0;
			}
			www.assetBundle.Unload(true);
			www.Dispose();
			//添加一个下载记录到本地配置表
			Core.Data.sourceManager.AddDownloadRecord(new SouceData(fileName,Core.Data.sourceManager.getNewNum(fileName)) );
		}

		DownLoadIndex++;
		if(DownLoadIndex<allDownLoadSourceName.Count)
			StartCoroutine(DownFB2ImgSource(allDownLoadSourceName[DownLoadIndex]));
		else
		{
			if(isHaveUpdate)
			Core.Data.sourceManager.SaveToLocaldisk();
			SendLoginMsg();
		}

	}
	#endif

	#region UI Operation
	void LoginIsNotReady () {
		/*BtnLogin.enabled = false;
		BtnLogin.isEnabled = false;*/
		bGetServerListOk = false;
	}

	public void LoginIsReady () 
	{
		BtnLogin.enabled = true;
		BtnLogin.isEnabled = true;
		bGetServerListOk = true;
	}

	void showServerList (GetPartitionServerResponse resp) 
	{
		if(resp != null && resp.data != null && resp.data.sv != null) {

			if(resp.data.sv.Length == 0) 
			{
				 SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(9123),GameObject.Find("UI Root_top"));
				 return;
			}

			foreach(Server sv in resp.data.sv) {
				if(sv != null && sv.sid == resp.data.last) {
					ChosenServer = sv;
					break;
				}
			}
			if(ChosenServer == null) 
				ChosenServer = resp.data.sv[0];

			TxtServer.text = ChosenServer.name;

			ListOfServer = resp.data.sv;
			Core.SM.onServerSelected(ChosenServer, DeviceInfo.GUID);
		}
	}

	void selectServer(Server server) {
		ChosenServer = server;

		TxtServer.text = ChosenServer.name;
		Core.SM.onServerSelected(ChosenServer, DeviceInfo.GUID);
	}

	void JumpToGameView() 
	{
		if(status.check( LoginStatus.All )) 
		{
			if(!Core.Data.playerManager.RTData.IsRegister)
			{
				Core.SM.beforeLoadLevel(SceneName.LOGIN_SCENE, SceneName.GameMovie);
				Application.LoadLevel(SceneName.GameMovie);
				SmartRelease.SmartRemoveAsset_1();
			}
			else
			{
				Core.SM.beforeLoadLevel(SceneName.LOGIN_SCENE, SceneName.MAINUI);
				Application.LoadLevel(SceneName.MAINUI);
				SmartRelease.SmartRemoveAsset_1();
			}
		}
	}

	

	public void onButtonClick() 
	{
		AccountData ad = Native.mInstace.m_thridParty.GetAccountData ();
	#if Spade
		SpadeIOSLogin spadeSdk = Native.mInstace.m_thridParty as SpadeIOSLogin;
		bool retry = spadeSdk == null ? false : spadeSdk.OnCheckGameEnter();
		//如果没有登录，或者取消登录，打开第三方登录界面
		if (ad.loginStatus == ThirdLoginState.CancelLogin || ad.loginStatus == ThirdLoginState.Invalid || retry == false) {
			spadeSdk.getUniqueId (
				(t) => { SendGetThirdServerRQ ();}, false
			);
	#else
		//如果没有登录，或者取消登录，打开第三方登录界面
		if (ad.loginStatus == ThirdLoginState.CancelLogin || ad.loginStatus == ThirdLoginState.Invalid) {
			Native.mInstace.m_thridParty.getUniqueId (
				(t) => { SendGetThirdServerRQ ();}
			);
	#endif

		} 
		else if(ad.loginStatus == ThirdLoginState.LoginFinish)
		{

			///
			/// ---- 到目前为止，都是第三方登陆成功，自己的还没有初始化成功 -----
			///

			#if UNITY_ANDROID && !UNITY_EDITOR && !CHECKCONFIG
			if(step != LoginStep.Prepare_Config) {
				User_Click_LoginBtn = true;
				return;
			}
			#endif

			if (ChosenServer != null) {
				if (ChosenServer.status == Server.STATUS_STOP) {


					string word = Core.Data.stringManager.getString (21);
					if (string.IsNullOrEmpty (word)) {
						word = "Beta test is Over.\nThe public test is coming soon!";
					}
					SQYAlertViewMove.CreateAlertViewMove (word, gameObject);
					return;
                } else if(ChosenServer.status == Server.STATUS_FULL) {
                  //TODO :停服提示 yancg

                     string status_full = Core.Data.stringManager.getString (32002);
                    if(string.IsNullOrEmpty (status_full))
                    {
                          status_full = "Beta test is Over.\nThe public test is coming soon!";
                    }
                    SQYAlertViewMove.CreateAlertViewMove (status_full, gameObject);
                    return ;
               
                } 

			}
			if(bGetServerListOk)
				Login ();
			else {
				SendGetThirdServerRQ();
			}
		}
	}

	public void onChangeServer() 
	{
		AccountData ad = Native.mInstace.m_thridParty.GetAccountData ();
		if(bGetServerListOk && ad.loginStatus == ThirdLoginState.LoginFinish) {
			ServerList.Instance().onClick = selectServer;
			ServerList.Instance().setLastLoginServer(ChosenServer);
			ServerList.Instance().setServerList(ListOfServer);
		} else {
			onButtonClick();
		}
	}




	#endregion
   

}
