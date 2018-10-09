using UnityEngine;
using System.Collections;
using System;
using UniLua;
using AW.Data;

public class LuaTest : MonoBehaviour {

	public static LuaTest Instance;

	//根据服务器赋值
	private static bool billing = false;
	public bool HasBilling {
		get {
			return billing;
		}
	}

	private static bool Push = false;
	public bool HasPush {
		get {
			return Push;
		}
	}

	private static bool activation = false;
	public bool ActivationCode {
		get {
			return activation;
		}
	}
	//开启漫画
	private static bool cartoon = false;
	public bool OpenCartoon{
		get {return cartoon; }
	}
	//首充
	private static  bool firstCharge = false;
	public bool OpenFirstCharge{
		get{ 
			return firstCharge;
		}
	}
	//下载礼包
	private static bool downAward = false;
	public bool OpenDownAward{
		get{ 
			return downAward;
		}
	}
	//七天
	private static bool sevenAward = false;
	public bool OpenSevenAward {
		get{ 
			return sevenAward;
		}
	}
	//等级
	private static bool levelAward = false;
	public bool OpenLevelAward{
		get{ 
			return levelAward;
		}
	}
	//命运转轮
	private static bool fateAward = false;
	public bool OpenFateAward {
		get{ 
			return fateAward;
		}
	}
	//武者节日
	private static bool festival = false;
	public bool OpenFestival{
		get{ 
			return festival;
		}
	}
	//	魔王来袭
	private static bool devil = false;
	public bool OpenDevil{
		get{ 
			return devil;
		}
	}
	//	签到奖励
	private static bool signAward = false;
	public bool OpenSign {
		get{ return signAward;}
	}
	//	vip奖励
	private static bool vipAward = false;
	public bool VipAward{
		get{ return vipAward;}
	}
	//雷达
	private static bool teamPlay = false;
	public bool OpenTeamPlay{
		get{return teamPlay;}
	}
	//聊天
	private static bool chat = false;
	public bool OpenChat{
		get{ return chat;}
	}
	//微信
	private static bool weixin = false;
	public bool OpenWeiXin{
		get{ return weixin;}
	}
	//招募  金币
	private static bool recruitCoinRole = false;
	public bool OpenCoinRecruit{
		get { return recruitCoinRole;}
	}
	private static bool recruitStoneRole = false;
	public bool OpenStoneRecruit{
		get{ return recruitStoneRole;}
	}

	//商店
	private static bool openStore =false;
	public bool OpenStore{
		get{ return openStore;}
	}

	//是否显示热卖
	private static bool hotStore = true;
	public bool HotStore{
		get{ return hotStore;}
	}

	//是否显示经验
	private static bool expStore = true;
	public bool ExpStore{
		get{ return expStore;}
	}

	//是否显示道具
	private static bool itemStore = true;
	public bool ItemStore{
		get{ return itemStore;}
	}

	//是否显示便捷购买
	private static bool convenientBuy = true;
	public bool ConvenientBuy{
		get{ return convenientBuy;}
    }
    
	//"openRecruitRoles":1   十连抽开关   "oepnStore":1 

	//版本号
	public static string versionCode = "";
	public string GetVersionCode
	{
		get{
			return versionCode;
		}
	}
	private static bool openGuess = false;
	public bool OpenGuess{
		get{ return openGuess;}
	}

	private static bool openLuckyWheel = false;
	public bool OpenLuckyWheel {
		get{ return openLuckyWheel;}
	}


	public static void fullfillByNetwork(BaseResponse response) {
		if(response != null && response.status != BaseResponse.ERROR) {
			LoginResponse resp = response as LoginResponse;
			if(resp != null && resp.data != null && resp.data.sysStatus != null) {
				SysStatus status = resp.data.sysStatus;
				billing =  status.opencharge == (short)1;
				activation = status.openActvity == (short)1;

				cartoon = status.openComic == (short)1;
				firstCharge = status.openFirstCash == (short)1;
				downAward = status.openDownAward == (short)1;
				sevenAward = status.openSevenDay == (short)1;
				levelAward = status.openLevelAward == (short)1;
				fateAward = status.openFateAward == (short)1;
				festival = status.openFeast == (short)1; 
				devil = status.openDevil == (short)1;
				signAward = status.openSign == (short)1;
				vipAward = status.openVipAward == (short)1;
				teamPlay = status.openTeamPlay == (short)1;
				chat = status.openChat == (short)1;
				weixin = status.openWeixin == (short)1;
				recruitStoneRole = status.openRecruitRoles == (short)1;
				recruitCoinRole = status.openRecruitRoles == (short)1;
				openStore = status.oepnStore == (short)1;
				openGuess = status.openGuess == (short)1;
				openLuckyWheel = status.openLuckWheel == (short)1;

			}
		}
	}


	/// 
	/// Lua的配置
	/// 
	public const string	LuaScriptFile = "main.lua";
	public const string LIB_NAME = "libluatest.cs";
	private ILuaState mlua;

	public static int OpenLib(ILuaState lua) // 库的初始化函数
	{
		var define = new NameFuncPair[]
		{
			new NameFuncPair("add", Add),
			new NameFuncPair("sub", Sub),
			new NameFuncPair("create", CreateUI),
		};

		lua.L_NewLib(define);
		return 1;
	}

	public static int Add(ILuaState lua)
	{
		var a = lua.L_CheckNumber( 1 ); // 第一个参数
		var b = lua.L_CheckNumber( 2 ); // 第二个参数
		var d = lua.L_CheckString( 3 );    // 第三个参数
		var f = lua.L_CheckNumber( 4 );    // 第四个参数
		var g = lua.L_CheckNumber( 5 );    // 第5个参数
		var h = lua.L_CheckNumber( 6 );    // 第6个参数
		var i = lua.L_CheckNumber( 7 );    // 第7个参数
		var c = a + b; // 执行加法操作
		lua.PushNumber( c ); // 将返回值入栈

		billing = a == 1;
		Push    = b == 1;
		versionCode = d;
		hotStore = f == 1;
		expStore = g == 1;
		itemStore = h == 1;
		convenientBuy = i == 1;
		ConsoleEx.DebugLog("billing = " + billing + "; Push = " + Push + "; VersionCode = " + versionCode + "; newStore = ");

		return 1; // 有一个返回值
	}
		

	public static int Sub(ILuaState lua)
	{
		var a = lua.L_CheckNumber( 1 ); // 第一个参数
		var b = lua.L_CheckNumber( 2 ); // 第二个参数
		var c = a - b; // 执行减法操作
		lua.PushNumber( c ); // 将返回值入栈
		return 1; // 有一个返回值
	}

	public static int CreateUI(ILuaState lua) {
		UnityEngine.Object obj = Resources.Load("CartoonPieces/1");
		GameObject go = UnityEngine.GameObject.Instantiate(obj) as GameObject;
		go.transform.position = Vector3.zero;
		return 0;
	}

	// Use this for initialization
	void Start () {
		if(Core.SM != null && Core.SM.isReLogin) {
			Destroy(gameObject);
		} else {
			Instance = this;
			#if !UNITY_EDITOR

			#if UNITY_ANDROID
			AndroidOrWebDataCopyOnInstall onInstall = new AndroidOrWebDataCopyOnInstall();
			onInstall.LuaOn(this, () => { Invoke("initialLua", 1f); } , null);
			#else
			Invoke("initialLua", 1f);
			#endif

			#else
			Invoke("initialLua", 0.5f);

			#endif
		}
	}

	void initialLua() {

		// 创建 Lua 虚拟机
		mlua = LuaAPI.NewState();

		// 加载基本库
		mlua.L_OpenLibs();

		mlua.L_RequireF( LuaTest.LIB_NAME    // 库的名字
			, LuaTest.OpenLib   // 库的初始化函数
			, false             // 不默认放到全局命名空间 (在需要的地方用require获取)
		);

		ConsoleEx.DebugLog("Lua Init Done", ConsoleEx.YELLOW);

		loadLuaFile();
	}

	void loadLuaFile() {
		var status = mlua.L_DoFile( LuaScriptFile );
		if( status != ThreadStatus.LUA_OK )
		{
			throw new Exception( mlua.ToString(-1) );
		}

		if( ! mlua.IsTable(-1) )
		{
			throw new Exception(
				"framework main's return value is not a table" );
		}

		mlua.Pop(1);
		ConsoleEx.DebugLog("load LuaFile Done", ConsoleEx.YELLOW);
	}

	public static void SetLuaStatus( SysStatus  status){
		if (status != null) {
			billing =  status.opencharge == (short)1;
			activation = status.openActvity == (short)1;

			cartoon = status.openComic == (short)1;
			firstCharge = status.openFirstCash == (short)1;
			downAward = status.openDownAward == (short)1;
			sevenAward = status.openSevenDay == (short)1;
			levelAward = status.openLevelAward == (short)1;
			fateAward = status.openFateAward == (short)1;
			festival = status.openFeast == (short)1; 
			devil = status.openDevil == (short)1;
			signAward = status.openSign == (short)1;
			vipAward = status.openVipAward == (short)1;
			teamPlay = status.openTeamPlay == (short)1;
			chat = status.openChat == (short)1;
			weixin = status.openWeixin == (short)1;
			recruitStoneRole = status.openRecruitRoles == (short)1;
			recruitCoinRole = status.openRecruitRoles == (short)1;
			openStore = status.oepnStore == (short)1;
		}
	}
	/**
	void OnGUI() {
		if(GUI.Button(new Rect(10, 10, 100, 100), "load lua file")) {
			loadLuaFile();
		}
	} **/

}
