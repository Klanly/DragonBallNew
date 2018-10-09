using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Framework;
#if UNITY_IPHONE
using System.Runtime.InteropServices;
#endif

#if Spade
using fastJSON;
#endif

#region 快速登陆(机器码登录)
public class QuickLogin : IGetUniqueID
{
	private Action<AccountData> m_LoginCallback;
	private AccountData m_AccountData;

	public void getUniqueId (Action<AccountData> onThirdPartyNotify)
	{
		m_LoginCallback = onThirdPartyNotify;
		LoginSuc (DeviceInfo.GUID);
	}

	public void LoginSuc(string strMsg)
	{
		AccountData ad = new AccountData();

		ad.maket = Market.Invalid;
		#if UNITY_IPHONE
		ad.platform = Platform.IOS;
		#elif UNITY_ANDROID
		ad.platform = Platform.Android;
		#else
		ad.platform = Platform.Invalid;
		#endif
		ad.uniqueId = DeviceInfo.GUID;
		ad.lType = LoginType.TYPE_QUICK;
		ad.loginStatus = ThirdLoginState.LoginFinish;
		m_AccountData = ad;

		m_LoginCallback(ad);
	}

	//第三方取消登录
	public void LoginCacel(string strCode)
	{
	}

	//第三方切换账号
	public void SwitchAccount()
	{
	}

	//退出第三方
	public void Quit() {
		Application.Quit();
	}

	//支付
	public void Pay(string strMsg)
	{
	}

	//支付回调
	public void PayResultCallback(string state)
	{
	}

	// 实名注册
	public void RrealNameRegist(string strID)
	{
	}

	//防沉迷查询
	public void AntiAddictionQuery(string strToken, string strID)
	{
	}

	public AccountData GetAccountData()
	{
		return m_AccountData;
	}

	public QuickLogin () { }
}
#endregion

#region app_store登录
public class AppStoreLogin : IGetUniqueID
{
	private Action<AccountData> m_LoginCallback;
	private AccountData m_AccountData;

	public void getUniqueId (Action<AccountData> onThirdPartyNotify)
	{
		m_LoginCallback = onThirdPartyNotify;
		LoginSuc (DeviceInfo.GUID);
	}

	public void LoginSuc(string strMsg)
	{
		AccountData ad = new AccountData();
		ad.maket = Market.APP_STORE;
		ad.platform = Platform.IOS;
		ad.uniqueId = DeviceInfo.GUID;
		ad.lType = LoginType.TYPE_THIRDPARTY;
		ad.loginStatus = ThirdLoginState.LoginFinish;
		m_AccountData = ad;

		m_LoginCallback(ad);
	}

	//第三方取消登录
	public void LoginCacel(string strCode)
	{
	}

	//第三方切换账号
	public void SwitchAccount()
	{
	}

	//退出第三方
	public void Quit()
	{
	}

	//支付
	public void Pay(string strMsg)
	{
	}

	//支付回调
	public void PayResultCallback(string state)
	{
	}

	// 实名注册
	public void RrealNameRegist(string strID)
	{
	}

	//防沉迷查询
	public void AntiAddictionQuery(string strToken, string strID)
	{

	}

	public AccountData GetAccountData()
	{
		return m_AccountData;
	}

	public AppStoreLogin () { }
}
#endregion


#region 黑桃登陆

#if Spade

public class HTPayInfo {
	//价格
	public int price;
	//单位价格购买的数量
	public int count;
	//商品ID
	public string productId;
	//商品名字
	public string productName;
	//商品描述
	public string productDes;
	//服务器ID
	public string serverId;
	//支付回调地址
	public string url;
	//扩展字段，订单ID
	public string appOrderId;
	//是否为月卡
	public bool isPayMonth;
}

/// 
/// Android 平台独有的一些支付信息
/// 
public class HTPayExtend {
	/**服务器ID key*/
	public string cp_server_id;
	/**服务器名称 key*/
	public string cp_server_name;
	/**角色ID key*/
	public string role_id;
	/**角色名 key*/
	public string role_name;
	/**角色等级 key*/
	public int role_level;
	/**是否为新角色(新:1 旧:0) key*/
	public short is_new_role;
	/**用户余额 key*/
	public int user_balance;
	/**VIP等级 key*/
	public int vip_level;
	 
	//reserved /**公会、帮派 key*/
	public string user_party = null;
	//不是月卡
	public short  is_pay_month = 0;
	//reserved /**货币图片所在assets目录文件名 key*/
	public string coin_image_name = @"gold.png";
	//reserved /**自定义订单号<SDK->CP> key*/
	public string custom_order_number = null;

	public HTPayExtend () {
		Server curserver = Core.SM == null ? null : Core.SM.curServer;
		cp_server_id = curserver == null ? string.Empty : curserver.id.ToString();
		cp_server_name = curserver == null ? string.Empty : curserver.name;

		PlayerManager player = Core.Data.playerManager;
		if(player != null && player.RTData != null) {
			role_id = player.RTData.ID;
			role_name = player.RTData.nickName;
			role_level = player.RTData.curLevel;
			is_new_role = (short)0;
			user_balance = player.RTData.curStone;
			vip_level = player.RTData.curVipLevel;
		}
	}

	public string toJson() {
		return JSON.Instance.ToJSON(this);
	}
}

public class SpadeIOSLogin : IGetUniqueID
{

	/// <summary>
	/// loginSpade 用于登陆黑桃SDK
	/// CheckGame  用于检测登陆
	/// EnterGame  用于Unity登陆完成后的信息收集
	/// logoutSpade用于登出黑桃SDK
	/// </summary>
#if UNITY_IPHONE

	[DllImport("__Internal")]
	private static extern void loginSpade();
	[DllImport("__Internal")]
	private static extern bool CheckGame();
	[DllImport("__Internal")]
	private static extern void EnterGame(string serverId, string serverName, string roleId, string roleName, int level, bool isNew);
	[DllImport("__Internal")]
	private static extern void logoutSpade();
	[DllImport("__Internal")]
	private static extern void Purchase(int price, int count, string productId, string serverId, string productName, string productDes, string url, string ext);

#endif

	private const string CANCEL_DEF = "cancel";

	/// <summary>
	/// 用户信息
	/// </summary>
	class UserInfo {
		public string platformId;
		public string platformUserId;
		public string token;
		public string userId;
	}

	private Action<AccountData> m_LoginCallback;
	private AccountData m_AccountData;

	/// 
	/// 黑桃SDK注销的时候，会默认自动登陆，所以我要缓存下来下一次登陆的数据
	/// 这里的数据有3种情况： 
	/// 1. null, 说明要打开第三方登陆界面
	/// 2. cancel，说明注销的时候，或者是登陆的时候，点击取消。这个就不处理任何事情
	/// 3. json数据，说明有合理的数据
	private string cachedReloginJson = null;

	/// 
	/// 扩展黑桃的方法
	/// 
	public void getUniqueId (Action<AccountData> onThirdPartyNotify) {
		getUniqueId(onThirdPartyNotify, true);
	}

	public void getUniqueId (Action<AccountData> onThirdPartyNotify, bool callNative)
	{
		m_LoginCallback = onThirdPartyNotify;
		AccountData ad = new AccountData();

		ad.maket = Market.Spade;
#if UNITY_IPHONE
		ad.platform = Platform.IOS;
#elif UNITY_ANDROID
		ad.platform = Platform.Android;
#else
		ad.platform = Platform.Invalid;
#endif
		ad.lType = LoginType.TYPE_THIRDPARTY;
		ad.loginStatus = ThirdLoginState.Opened;
		m_AccountData  = ad;

		///
		///  如果是注销后再次登陆，则不应该发送请求
		///
		bool relogin = Core.SM.isReLogin;
		string test = string.IsNullOrEmpty(cachedReloginJson) ? "Empty" : cachedReloginJson;
		ConsoleEx.DebugLog(" **********  isRelogin === " + relogin + ". cachedReloginJson = " + test);
		if(relogin) {

#if UNITY_EDITOR
		nativeLogin();
#else
		if(!string.IsNullOrEmpty(cachedReloginJson)) {

			if(cachedReloginJson == CANCEL_DEF) {
				if(callNative) nativeLogin();
			} else {
				LoginSuc(cachedReloginJson);
				cachedReloginJson = null;
			}
		}
#endif

		} else {
			if(callNative) nativeLogin();
		}

	}

	//打开第三方SDK的登陆界面
	void nativeLogin() {
#if UNITY_EDITOR
		LoginSuc(@"{""platformId"":""168"", ""platformUserId"":""2314076"", ""token"":""eb3761a908ced44a79f1435042cbc68b"", ""userId"":""4284866""}");
#elif UNITY_IPHONE
		loginSpade();
#elif UNITY_ANDROID
		androidSpadeLogin();
#endif
	}

	//退出第三方
	public void tryLogout() {
#if Spade && !UNITY_EDITOR
#if UNITY_IPHONE
		logoutSpade();
#elif UNITY_ANDROID
		androidSpadeLogout();
#endif

#endif
	}

#if UNITY_ANDROID
	/// 
	/// 退出游戏，这个只有android才有
	/// 
	public void quitGame() {
		androidSpadeQuit();
	}
#endif

	//支付
	public void Pay(string strMsg)
	{
		HTPayInfo payInfo = JSON.Instance.ToObject<HTPayInfo>(strMsg);
		if(payInfo != null) {
			payInfo.price = payInfo.price / 100;
#if UNITY_IPHONE
			Purchase(payInfo.price, payInfo.count, payInfo.productId, payInfo.serverId, payInfo.productName, payInfo.productDes, payInfo.url, payInfo.appOrderId);
#elif UNITY_ANDROID
			androidSpadePay(payInfo);
#endif

		}
	}


	public bool OnCheckGameEnter() {
		bool canEnter = true;
#if !UNITY_EDITOR

		///检测能否进入游戏
#if UNITY_IPHONE
		canEnter = CheckGame();
#elif UNITY_ANDROID
		canEnter = androidCheckGame();
#endif

#endif
		return canEnter;
	}

	//通知第三方登陆成功
	public void NotityLogin(Server server) {
		if(server != null) {

			PlayerManager player = Core.Data.playerManager;
			if(player == null) return;

#if UNITY_IPHONE
			EnterGame(server.sid.ToString(), server.name, player.PlayerID, player.NickName, player.Lv, !player.RTData.IsRegister);
#elif UNITY_ANDROID
			AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
			jo.Call(Native.ENTER_GAME, server.sid.ToString(), server.name, player.PlayerID, player.NickName, player.Lv, !player.RTData.IsRegister);
#endif

		}
	}

	public AccountData GetAccountData()
	{
		return m_AccountData;
	}

	public SpadeIOSLogin () { }


#region Android的函数
#if UNITY_ANDROID
	void androidSpadeLogin() {
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call(Native.LOGIN_FUN);
	}

	void androidSpadePay(HTPayInfo payInfo) {
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		HTPayExtend extendPay = new HTPayExtend();
		extendPay.is_pay_month = payInfo.isPayMonth ? (short) 1 : (short) 0;
		string extend = extendPay.toJson();
		jo.Call("Purchase", payInfo.price, payInfo.count, payInfo.productId, payInfo.serverId, payInfo.productName, payInfo.productDes, payInfo.url, payInfo.appOrderId, extend);
	}

	void androidSpadeQuit() {
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call(Native.QUIT_FUN);
	}

	bool androidCheckGame() {
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		bool result = jo.Call<bool>(Native.CHECK_GAME);
		return result;
	}

	void androidSpadeLogout() {
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call(Native.LOGOUT_FUN);
	}
#endif
#endregion
	///
	/// ------------------------------- 回调函数 -----------------------------------
	///

	public void LoginSuc(string strMsg) {
		ConsoleEx.DebugLog("Login Sequence : " + strMsg);
		///
		/// ------ 黑桃SDK特别奇怪的逻辑，注销后默认自动登陆 -------
		///        所以注销的时候，会把登陆的结果发送到第一次开始游戏创建的Native里面，故我应该在收到注销回调的时候设置m_LoginCallback为空
		///
		if(m_LoginCallback == null) { 
			cachedReloginJson = strMsg;
			return; 
		}


		UserInfo user = JSON.Instance.ToObject<UserInfo>(strMsg);

		m_AccountData.uniqueId    = user.userId;
		m_AccountData.session     = user.token;
		m_AccountData.loginStatus = ThirdLoginState.LoginFinish;
		m_AccountData.extension   = user.platformId;

#if UNITY_EDITOR
		m_LoginCallback(m_AccountData);
#else
///检测能否进入游戏
#if UNITY_IPHONE
		if(CheckGame()) 
			m_LoginCallback(m_AccountData);
#elif UNITY_ANDROID
		if(androidCheckGame())
			m_LoginCallback(m_AccountData);
#endif

#endif

	}

	//第三方取消登录
	public void LoginCacel(string strCode) {
		RED.Log ("Spade login cancel");
		m_AccountData.loginStatus = ThirdLoginState.CancelLogin;
		if (LoginView.mInstance != null) {
			LoginView.mInstance.LoginIsReady ();
		}
		cachedReloginJson = CANCEL_DEF;
	}

	//第三方切换账号
	public void SwitchAccount() {
		///
		/// ----- 有可能第三方SDK会在看漫画或者战斗的场景退出我们的游戏 -----
		///
		if(Core.SM.CurScenesName == SceneName.MAINUI || Core.SM.CurScenesName == SceneName.GameMovie || Core.SM.CurScenesName == SceneName.GAME_BATTLE) {
			///
			/// ------ 黑桃SDK特别奇怪的逻辑，注销后默认自动登陆 -------
			///        所以注销的时候，会把登陆的结果发送到第一次开始游戏创建的Native里面，故我应该在收到注销回调的时候设置m_LoginCallback为空
			///
			m_LoginCallback = null;


			Core.SM.OnUnregister();

			Core.SM.beforeLoadLevel(Application.loadedLevelName, SceneName.LOGIN_SCENE);
			AsyncLoadScene.m_Instance.LoadScene(SceneName.LOGIN_SCENE);
		}
	}

	//验证之后正式推出
	public void Quit() {
		Application.Quit();
	}

	//支付回调
	public void PayResultCallback(string state)
	{
		string result = state.ToLower();
		if(result == "ok") { 
			//支付成功, 可以理解为回调告诉为成功，实际上仍需和游戏服务器同步信息
			UIDragonMallMgr.GetInstance ().mUIDragonRechargeMain.LoopQueryPayStatus ();
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(5209));
		} else { 
			//支付失败
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(5208));
		}
	}

	// 实名注册
	public void RrealNameRegist(string strID)
	{
	}

	//防沉迷查询
	public void AntiAddictionQuery(string strToken, string strID)
	{
	}


}
#endif

#endregion

#region 360登录
#if QiHo360 && UNITY_ANDROID && !UNITY_EDITOR
public class Qiho360Login : IGetUniqueID
{
	private Action<AccountData> m_LoginCallback;
	private AccountData m_AccountData;

	public void getUniqueId (Action<AccountData> onThirdPartyNotify)
	{
		m_LoginCallback = onThirdPartyNotify;
		LoginQiHo360 ();
	}
		
	void LoginQiHo360()
	{
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call(Native.LOGIN_FUN);
			
		AccountData ad = new AccountData();
		ad.maket = Market.QI_HOO;
		ad.platform = Platform.Android;
		ad.lType = LoginType.TYPE_THIRDPARTY;
		ad.loginStatus = ThirdLoginState.Opened;
		m_AccountData = ad;
	}

	//第三方切换账号
	public void SwitchAccount()
	{
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call(Native.SWITCH_ACCOUNT_FUN);
	}

	public void LoginSuc(string uniqueId)
	{
		RED.Log ("360登录成功");

		m_AccountData.maket = Market.QI_HOO;
		m_AccountData.platform = Platform.Android;
		m_AccountData.uniqueId = uniqueId;
		m_AccountData.lType = LoginType.TYPE_THIRDPARTY;
		m_AccountData.loginStatus = ThirdLoginState.LoginFinish;

		m_LoginCallback (m_AccountData);
	}

	//第三方取消登录
	public void LoginCacel(string strCode)
	{
		RED.Log ("360取消登录");
		m_AccountData.loginStatus = ThirdLoginState.CancelLogin;
		if (LoginView.mInstance != null) 
		{
			LoginView.mInstance.LoginIsReady ();
		}
	}

	//退出第三方
	public void Quit()
	{
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call(Native.QUIT_FUN);
	}

	//支付
	public void Pay(string strMsg)
	{
		RED.Log ("qiho360     pay  :" + strMsg);
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call(Native.PAY_FUN, strMsg);
	}

	public void PayResultCallback(string state)
	{
		RED.Log ("360支付结果回调：： " + state);
		// error_code 状态码： 0 支付成功， -1 支付取消， 1 支付失败， -2 支付进行中。
		switch (state) 
		{
		case "-1":
			break;
		case "1":
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(5208));
			break;
		case "0":
		case "-2":
			UIDragonMallMgr.GetInstance ().mUIDragonRechargeMain.LoopQueryPayStatus ();
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(5209));
			break;
		}
	}

	// 实名注册
	public void RrealNameRegist(string strID)
	{
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call(Native.REAL_NAME_REGISTE, m_AccountData.uniqueId);
	}

	//防沉迷查询
	public void AntiAddictionQuery(string strToken, string strID)
	{
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call(Native.FANGCHENMI_FUN, m_AccountData.token, m_AccountData.uniqueId);
	}
		
	public AccountData GetAccountData()
	{
		return m_AccountData;
	}
}

#endif
#endregion



#region google Play
#if Google && UNITY_ANDROID 
public class GoogleLogin : IGetUniqueID
{
	private Action<AccountData> m_LoginCallback;
	private AccountData m_AccountData;

	public void getUniqueId (Action<AccountData> onThirdPartyNotify)
	{
		m_LoginCallback = onThirdPartyNotify;
		LoginSuc (DeviceInfo.GUID);
	}

	public void LoginSuc(string strMsg)
	{
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call(Native.LOGIN_FUN);

		AccountData ad = new AccountData();
		ad.maket = Market.GOOGLEPLAY;
		ad.platform = Platform.Android;
		ad.lType = LoginType.TYPE_THIRDPARTY;
		ad.loginStatus = ThirdLoginState.Opened;
		m_AccountData = ad;

		m_LoginCallback(ad);
	}

	public void Pay(string strMsg)
	{
		RED.Log ("google     pay  :" + strMsg);
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call(Native.PAY_FUN, strMsg);
	}

	public void PayResultCallback(string state)
	{
		RED.Log ("google支付结果回调：： " + state);
		// error_code 状态码： 0 支付成功， -1 支付取消， 1 支付失败， -2 支付进行中。
		switch (state) 
		{
			case "-1":
			break;
			case "1":
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(5208));
			break;
			case "0":
			case "-2":
			UIDragonMallMgr.GetInstance ().mUIDragonRechargeMain.LoopQueryPayStatus ();
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(5209));
			break;
		}
	}

	public void LoginCacel(string strCode)
	{
	}

	//第三方切换账号
	public void SwitchAccount()
	{
	}

	//退出第三方
	public void Quit()
	{
	}

	// 实名注册
	public void RrealNameRegist(string strID)
	{
	}

	//防沉迷查询
	public void AntiAddictionQuery(string strToken, string strID)
	{

	}

	public AccountData GetAccountData()
	{
		return m_AccountData;
	}
}

#endif
#endregion


