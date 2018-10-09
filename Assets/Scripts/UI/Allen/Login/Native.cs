using UnityEngine;
using System.Collections;
using Framework;
using System.Runtime.InteropServices;

public class Native : MonoBehaviour 
{
	private static Native _instance;
	public static Native mInstace
	{
		get
		{
			return _instance;
		}
	}
		
	//所有第三方SDK调用的方法
	public const string LOGIN_FUN = "startLogin";						//登录(必须接)
	public const string SWITCH_ACCOUNT_FUN = "SwitchAccount";			//切换账号(必须接)
	public const string PAY_FUN = "startPay";							//支付(必须接)
	public const string QUIT_FUN = "Quit";								//退出(必须接)
	public const string FANGCHENMI_FUN = "AntiAddictionQuery";			//防沉迷查询(必须接)
	public const string REAL_NAME_REGISTE = "RealNameRegister";			//实名注册(必须接)
	public const string BBS_FUN = "";									//游戏论坛（可选）
	public const string CUSTOM_FUN = "";								//客服中心（可选）
	public const string LOGOUT_FUN = "logoutSpade";                     //登出SDK游戏

	//---- 黑桃专有的方法 ----
	public const string CHECK_GAME = "CheckGame";                       //检查游戏能否进入
	public const string ENTER_GAME = "EnterGame";                       //进入游戏的一个统计

	//安卓native方法
	public const string START_GPS = "StartGPS";							//获取gps位置

	//第三方
	public IGetUniqueID m_thridParty;

	void Awake() {
		if(Core.SM != null && Core.SM.isReLogin) {
			Destroy(gameObject);
		} else {
			_instance = this;
			m_thridParty = new GetUniqueIDFactory().createInstance();
		}
	}
		
	//登录第三方成功
	void LoginThridPartySuc(string strAutoCode)
	{
		RED.Log ("Receive ::LoginThridPartySuc");
		m_thridParty.LoginSuc (strAutoCode);
	}

	//取消登录
	void LoginCacel(string strAutoCode)
	{
		RED.Log ("Receive :: Login third party failure");
		m_thridParty.LoginCacel (strAutoCode);
	}

	void PayResultCallBack(string state)
	{
		RED.Log ("Receive ::PayResultCallBack：： " + state);
		m_thridParty.PayResultCallback (state);
	}

	/// <summary>
	/// 登出第三方SDK
	/// </summary>
	void LogoutThridParty(string code) {
		RED.Log ("Receive ::LogoutThridPart.");
		m_thridParty.SwitchAccount();
	}

	/// <summary>
	/// 登出游戏
	/// </summary>
	void QuitGame(string code) {
		RED.Log ("Receive ::QuitGame.");
		m_thridParty.Quit();
	}

	#if UNITY_ANDROID
	public void StartGPS() {
		RED.Log ("启动本地gps服务::StartGPS");
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call(Native.START_GPS);
	}
	
	#elif UNITY_IPHONE 
	//导出以后将在xcode项目中生成这个fun的注册，
	//这样就可以在xocde代码中实现这个fun的事件。
	[DllImport("__Internal")]
	private static extern void _StartGPS ();
	public  void StartGPS ()
	{
		//调用xcode中的 _StartGPS ()方法，
		//方法中的内容须要我们自己来添加
		_StartGPS ();

	}
	
	[DllImport("__Internal")]
	private static extern void _StopGPS ();
	public  void StopGPS ()
	{

		//调用xcode中的 _StopGPS ()方法，
		//方法中的内容须要我们自己来添加
		_StopGPS ();
	}
    #endif

	public void GetGPSLocation(string strLocation)
	{
		RED.Log ("得到本地位置：：" + strLocation);
		if (RadarTeamUI.mInstace != null)
		{
			RadarTeamUI.mInstace.SetGPSLocation (strLocation);
		}
	}
}
