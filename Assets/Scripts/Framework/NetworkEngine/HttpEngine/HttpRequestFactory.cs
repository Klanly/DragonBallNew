using System;
using System.Collections.Generic;

/*
 * This is public for 
 */
public enum RequestType
{
	#region HTTP Request 
	GET_PARTITION_SERVER,				//普通登录获取服务器列表
	THIRD_GET_SERVER,					//第三方登录获取服务器列表
	LOGIN_GAME,
	THIRD_PARTY_LOGIN,					//第三方登录
	PAY,								//第三方支付
	QUERY_PAY_STATUS,					//查询订单状态
	PAYCOUNT,							//充值次数列表


	SKILL_UPGRADE,						//技能升级
	PVE_BATTLE,
	SM_PVE_BATTLE,
	UPDATE_RESOURCES,
	CHANGE_TEAM_MEMBER,
	CHANGE_EQUIPMENT,
	SELL_MONSTER,
	DECOMPOSE_MONSTER,
	EVOLVE_MONSTER,
	STRENGTHEN_MONSTER,
	STRENGTHEN_EQUIPMENT,
	SELL_EQUIPMENT,
	SELL_GEM,
	USE_PROP,
	SWAP_TEAM,
	SWAP_MONSTER_POS,
    BUY_ITEM,
	BUY_LOTTERY,


	TIANXIADIYI_BATTLE,
	GET_TIANXIADIYI_OPPONENTS,
	ZHANGONG_BUY_ITEM,
	REFRESH_ZHANGONG_BUY_ITEM,
	GET_ZHANGONG_BUY_ITEM_ID,

	GET_QIANGDUO_GOLD_OPPONENTS,
	QIANGDUO_GOLD_BUY_ITEM,
	GET_QIANGDUO_GOLD_ITEM_TOTAL,
	QIANGDUO_GOLD_BATTLE,

	GET_SU_DI_LIST,
	SU_DI_BATTLE,
	DELETE_SU_DI,
	ADD_SU_DI,

	CALL_DRAGON,

	LEARN_AOYI,
	EQUIP_AOYI,
	BUY_AOYI_SLOT,
	GET_QIANGDUO_DRAGONBALL_OPPONENTS,
	QIANGDUO_DRAGONBALL_BATTLE,
	BUY_MIANZHAN_TIME,

	GET_FRIEND_LIST,
	GET_FRIEND_REQUEST_LIST,
	GET_RECOMMEND_FRIEND_LIST,
	ADD_OR_DELETE_FRIEND,
	SEARCH_FRIEND,
	AGREE_OR_REFUSED_FRIEND,


	BUILD_CREATED,		//建造建筑
	BUILD_GET,			//建筑收钱
	BATTLE_BUILD_OPEN,	//开启作战建筑
	BUILD_UPGRADE,		//建筑升级

	CHANGE_USERINFO,	//改变用户信息
	RANDOM_NAME,		//随机用户昵称

	ZHAOMU_STATE,		//招募状态
	ZHAOMU,				//招募
	HECHENG,			//合成

    FINALTRIAL_ADDATR,
    FINALTRIAL_CHOOSEATR,
    FINALTRIAL_CHALLENGELIST,
    FINALTRIAL_DONGUENATR,
    FINALTRIAL_ISDONGUENATR,
    FINALTRIAL_GETRANK,
    FINALTRIAL_FIGHT,
	FINALTRIAL_LOGINDATA,
	FINALTRIAL_GETRANKINFO,

	FINALTRIAL_GETFINALLOGININFO,


	ATTR_SWAP,			//属性转换
	QIANLIXUNLIAN,		//潜力训练
	QIANLIRESET,		//潜力重置
	SOULHECHENG, 		//魂魄合成

    ANNOUNCE_INFO,

	HAVEDINNER,          //吃拉面
	HAVEDINNER_STATE,   //午餐状态
	GET_SIGNDATE_STATE,	//获取签到状态
	GIVE_TAOBAOCDKEY,   //淘宝cdkey

    MESSAGE_INFORMATION,     //战报
	MESSAGE_EMAIL,                   //邮件
	
	SIGNDAY,            //签到请求

	SEND_EMAIL ,     //发送留言
	
	GET_MAIL_ATTACHMENT,//GetReward attachment
	
	CHANGE_MAIL_STATE,      //改变邮件状态
	
	
	SECRETSHOP,
	SECRETSHOP_BUY,

	LEVELGIFTSTATE, // 获取 等级奖励状态
	LEVELGIFT_REQUEST, // 等级奖励 获取请求
	/*宝石合成
	 * */
	GEM_SYNTHETIC,
	/*宝石兑换
	 * */
	GEM_EXCHANGE,
	SEND_MESSAGE,
	/*宝石镶嵌或摘除
	 * */
	GEM_INLAY_REMOVE,
	/*宝石重铸
	 * */
	GEM_RECASTING,
	/*引导进度
	 * */
	GuideProgress,
	/*任务列表
	 * */
	Task_List,
	/*任务奖励
	 * */
	Task_Reward,
	
	BattleVideoPlayback,
	SingleBattleVideoPlayback,

	SEVENDAYREWARD,
	SEVENDAYREWARD_BUY,

    OPENTREASUREBOX,
    GETTREASURESTATE,
	VIPSHOPINFO,
	GETVIPGIFT,

	NEW_FINALTRIAL_ENTER,
	NEW_FINALTRIAL_ADDBUFFER,
	NEW_FINALTRIAL_MAP,
	NEW_FINALTRIAL_BATTLE,
	NEW_FINALTRIAL_MAP_NOTADD,
	NEW_FINALTRIAL_GETRANK,
	NEW_FINALTRIAL_GETRANKINFO,
	
	BATTLE_COMBO,					//战斗服务器

    DOWNLOAD_RECEOVE,
    DOWNLOAD_CHECK,
    DOWNLOAD_FINISH,

	MOVABLE_NOTICE,                 //主界面的公告
	
	GET_COMBO,          //从服务器获取玩家当前最大连击数
	
    GET_GAMBLESTATE,        //获取 赌博类型 list

	GET_CHALLENGE_RANK,
	GET_TIANXIADIYI_RANK,
	GET_TIANXIADIYI_RANKSINGLE,
    GET_CALLDRAGONISFINISH,

	GUAGUALE_STATUS,
	GUAGUALE,

    GET_GODGIFTLIST,
    GET_GODGIFT,
	
	RECHARGECOUNT,     //查询所购物品状态'

	GET_BIGWHEEL_LIST,    //大转盘显示列表
	USE_BIGWHEEL,     //使用大转盘
	RESET_BIGWHEEL_LIST,   //重置大转盘

	GET_AWARDACTIVITY, // 奖励活动
    GET_MONTHSTATE,    //月卡状态
    GET_MONTHGIFT ,//月礼包
	CURRENT_USERSATAE,   //向服务器发送 状态  10级  非UI
	SEND_USERDEVICE,    //向服务器发送 设备信息

	GET_FIRSTCHARGESTATE,
	GET_FIRSTCHARGEGIFT,
	NEW_PVE_BOSSBATTLE, //请求客户端的PVE战斗
	SETTLE_BOSSBATTLE,  //PVE战斗结束
	SETTLE_SHABU,       //沙鲁布欧的战斗结果
	SAODANG,     //扫荡
	SECRETSHOP_BUYSOULHERO,
	SYNC_PVE,			//同步PVE数据
	RESET_FLOOR,		//重置关卡挑战次数
	SYNC_MONEY,			//重置coin和stone

	BUY_PVEACT_FB,     //购买PVE活动副本次数(只有VIP才有这个资格)

	SYNC_CALLDRAGONTIME,

	NEW_FINALTRIAL_STATE,
	NEW_FINALTRIAL_CURDUNGEON,
	NEW_FINALTRIAL_TEAM,
    REFRESH_SECRETSHOP,
    BUY_ENERGY,
	ACTIVATION_CODE,
	
	GET_REVENGE_DATA,  //获取复仇信息

	RESETFINALTRIALVALUE,
	GETVIPLEVELREWARD,

    HOLIDAYACTIVITY,// 请求限时活动

	ACTIVESHOPITEM,
	ACTIVESHOPBUYITEM,

	CHECK_TEAMRANKINFO,
	GetSYSTEMSTATUES,
	ACTIVITYLIMITTIME,   //限时活动请求
	GETACTIVITYLIMITTIME,   //限时活动领取

	//龙珠银行
	DRAGONBANKMESSAGE,
	DRAGONBANKSAVEMSG,
	RECEIVEMONEY,	

	//不再使用的代码
	None,
	#endregion

	
	#region SOCKET Request
	SOCK_LOGIN,
	SOCK_CLOSE,
	SOCK_ATTACKBOSS,
	SOCK_LOGOUTBOSS,
	SOCK_LOGINBOSS,
	SOCK_GETBLOOD_ATKLIST,
	SOCK_GET_ACTTIME,
	SOCK_ACTIVITYSTATE,
	SOCK_ADDPOWER,
	SOCK_HONORBUYITEM,

	SOCK_LOGINFESTIVAL,
	SOCK_BUYLOTTERY,
	SOCK_LOGOUTFESTIVAL,
	SOCK_GETSCORERANKLIST,

	SOCK_WORLDCHATLOGIN,
	SOCK_WORLDCHAT,
    SOCK_LOGINGPSWAR,
    SOCK_GETROOMLIST,
    SOCK_LOGININROOM,
    SOCK_LOGGOUTROOM,
    SOCK_CREATROOM,
    SOCK_GPSFIGHT,
    SOCK_FIGHTCOMPLETE,
    SOCK_LOGOUTGPSWAR ,
    SOCK_SYNCLOCATION,
	SOCK_SYNCROOM,
	#endregion

	#region 本地数据
	FIGHT_FULISA,
	#endregion
}

public sealed class RelationShipReqAndResp {
	public Type respType;
	public RequestType requetType;
	public int requestAction;

	public RelationShipReqAndResp (RequestType rqType, int rqAction, Type respType) {
		this.requetType = rqType;
		this.requestAction = rqAction;
		this.respType = respType;
	}
}

public class HttpRequestFactory {

    //This will be fullfilled after game is launched.
    public static int swInfo;
	//This will be fullfilled after game is launched.
	public static int platformId;
	public static string _sessionId = "empty";
    public const int ACTION_DEFAULT = -1;

#if NewGuide
	public const int ACTION_LOGIN = -100;
#else
	public const int ACTION_LOGIN = 100;
#endif
	public const int ACTION_THRID_LOGIN = 90;					   //第三方登录
	public const int ACTION_THRID_PAY = 92;					  		//第三方支付下单(下单和查订单状态走的是不同的服务器，所以协议号相同不会有影响)
	public const int ACTION_QUERY_PAY_STATUS = 92;					//查看第三方订单状态
	public const int ACTION_RECHARGECOUNT = 93;                     //查询所购物品状态'
	public const int ACTION_GET_PAYCOUNT = 94;						//查看充值次数列表
    public const int ACTION_GETMONTHGIFT = 95;                          //领取月卡
	public const int ACTION_GET_FIRSTCHARGESTATE = 96;
	public const int ACTION_GET_FIRSTCHARGEGIFT = 97;

	public const int ACTION_CHG_TM = 102;
	public const int ACTION_PVE_BATTLE = 103;
	public const int ACTION_CHG_EQUIP = 104;
	public const int ACTION_SELL_MONSTER = 130;
	public const int ACTION_STRENGTHEN_MONSTER = 131;
	public const int ACTION_DECOMPOSE_MONSTER = 136;				//分解
	public const int ACTION_EVOLVE_MONSTER = 137;					//进化
	public const int ACTION_STRENGTHEN_EQUIP = 105;
	public const int ACTION_SELL_EQUIPMENT = 106;
	public const int ACTION_SELL_GEM = 203;
	public const int ACTION_USE_PROP = 107;
	public const int ACTION_SWAP_TEAM = 108;
    public const int ACTION_BUY_ITEM = 120;
	public const int ACTION_SWAP_MONSTER_POS = 132;
	public const int ACTION_UPDATE_RESOURCES = 1000;

	public const int ACTION_GET_SERVER = 1001;					//快速登录获取服务器列表
	public const int ACTION_THIRD_GET_SERVER = 1002;			//第三方登录获取服务器列表
	public const int ACTION_SKILL_UPGRADE = 139;				//技能升级

	public const int ACTION_GEM_SYNTHETIC = 200;
	public const int ACTION_GEM_EXCHANGE = 202;
	public const int ACTION_GEM_INLAY_REMOVE= 201;
	public const int ACTION_GEM_RECASTING = 204;

	public const int ACTION_ZHAOMU_STATE = 501;
	public const int ACTION_ZHAOMU = 502;			//招募
	public const int ACTION_HECHENG = 503;			//合成
	public const int ACTION_QIANLIXUNLIAN = 133;	//潜力训练
	public const int ACTION_ATTRSWAP = 134;			//属性变换
	public const int ACTION_SOULHECHENG = 135;		//魂魄合成
	public const int ACTION_QIANLIRESET = 138;		//潜力重置
	public const int ACTION_GETVIPLEVELREWARD = 548;

	public const int ACTION_CHANGE_USERINFO = 560;	//更改个人信息
	public const int ACTION_RANDOM_NAME = 900;		//随机个人信息

	//建筑操作
	public const int ACTION_BUILD_CREATE = 511; 
	public const int ACTION_BUILD_GET = 512;
	public const int ACTION_BUILD_UPGRADE = 513;
	public const int ACTION_BATTLE_BUILD_OPEN = 514;

    #region 夺宝界面
    public const int ACTION_GET_TIANXIADIYI_OPPONENTS = 300;//天下第一武道大会排行榜匹配列表
    public const int ACTION_ZHANGONG_BUY_ITEM = 3003;        //天下第一武道大会领取排行奖励或者战功兑换商品
	public const int ACTION_REFRESH_ZHANGONG_BUY_ITEM = 3004;
	public const int ACTION_TIANXIADIYI_BATTLE = 302;
    public const int ACTION_GET_ZHANGONG_BUY_ITEM_ID = 3002;//天下第一武道大会商城信息
	public const int ACTION_FINALTRIAL_GETFINALLOGININFO = 305;                    //夺宝界面
    #endregion

	public const int ACTION_GET_QIANGDUO_GOLD_OPPONENTS = 400;
	public const int ACTION_QIANGDUO_GOLD_BATTLE = 401;

	public const int ACTION_QIANGDUO_GOLD_BUY_ITEM = 403;
	public const int ACTION_GET_QIANGDUO_GOLD_ITEM_TOTAL = 402;

	public const int ACTION_GET_SU_DI_LIST = 141;  //宿敌列表请求
	public const int ACTION_DELETE_SU_DI = 142;
	public const int ACTION_ADD_SU_DI = 140;

	// 神龙召唤
	public const int ACTION_CALL_DRAGON = 150;

	public const int ACTION_LEARN_AOYI = 151;
	public const int ACTION_EQUIP_AOYI = 152;
    public const int ACTION_BUY_ENERGY =115;
	public const int ACTION_BUY_AOYI_SLOT = 156;
    public const int ACTION_GET_CALLDRAGONISFINISH = 157;
	public const int ACTION_GET_QIANGDUO_DRAGONBALL_OPPONENTS = 153;
	public const int ACTION_QIANGDUO_DRAGONBALL_BATTLE = 155;
	public const int ACTION_BUY_MIANZHAN_TIME = 154;
	public const int ACIION_SYNC_CALLDRAGONTIME = 158;

    public const int ACTION_MESSAGE_INFORMATION = 523;
	
	public const int ACTION_CHANGE_MAIL_STATE = 524;
	
	public const int ACTION_SEND_EMAIL = 525;
	
	public const int ACTION_GET_MAIL_ATTACHMENT = 526;
	
	public const int ACTION_MESSAGE_EMAIL = 527;
    public const int ACTION_REFRESH_SECRETSHOP = 3000;

	
	public const int ACTION_GET_FRIEND_LIST = 600;
	public const int ACTION_GET_FRIEND_REQUEST_LIST = 601;
	public const int ACTION_ADD_OR_DELETE_FRIEND = 603;
	public const int ACTION_SEARCH_FRIEND = 604;
	public const int ACTION_AGREE_OR_REFUSED_FRIEND = 602;


    public const int ACTION_DINNERSTATE = 800;
    public const int ACITON_EATDINNER = 801;
	public const int ACTION_GIVETAOBAOCDKEY = 541;
    public const int ACTION_LEVELGIFT_GET = 552;
    public const int ACTION_LEVELGIFTSTATE = 551;
    public const int ACITON_GET_SIGNDATESTATE = 112;
    public const int ACTION_SIGNDAY = 113;

    public const int ACTION_FINALTRIAL_ADDATR = 172;
    public const int ACTION_FINALTRIAL_CHOOSEATR = 173;
    public const int ACTION_FINALTRIAL_CHALLENGELIST = 170;
    public const int ACTION_FINALTRIAL_DONGUENATR = 174;
    public const int ACTION_FINALTRIAL_ISDONGUENATR = 175;
    public const int ACTION_CHECK_TEAMRANKINFO = 176;
    public const int ACTION_FINALTRIAL_FIGHT = 171;
	public const int ACTION_FINALTRIAL_GETRANKINFO = 177;
	public const int ACTION_FINALTRIAL_LOGINDATA = 114;


	public const int ACTION_NEW_FINALTRIAL_ENTER = 180;
	public const int ACTION_NEW_FINALTRIAL_ADDBUFFER = 181;
	public const int ACTION_NEW_FINALTRIAL_MAP = 182;
	public const int ACTION_NEW_FINALTRIAL_MAP_NOTADD = 183;
	public const int ACTION_NEW_FINALTRIAL_BATTLE = 184;
	public const int ACTION_NEW_FINALTRIAL_GETRANK = 185;
	public const int ACTION_NEW_FINALTRIAL_GETRANKINFO = 186;

    public const int ACTION_ANNOUNCE_INFO = 521;
	public const int ACTION_MOVABLE_NOTICE = 522;
	public const int ACTION_SECRETSHOP = 3001;
	public const int ACTION_SECRETSHOP_BUY = 543;

	public const int ACTION_SEND_MESSAGE = 9999;
	
	public const int ACTION_GUIDE_PROGRESS = 561;

	public const int ACTION_SINGLEBATTLEVIDEOPLAYBACK = 700;
	public const int ACTION_BATTLEVIDEOPLAYBACK = 701;

	public const int ACTION_SEVENDAYREWARD = 802;
	public const int ACTION_SEVENDAYREWARD_BUY = 803;

    public const int ACTION_OPENTREASUREBOX = 804;
    public const int ACTION_GETTREASURESTATE = 806;

	public const int ACTION_VIPSHOPINFO = 545;
	public const int ACTION_GETVIPGIFT = 805;
	public const int ACTION_BATTLE_COMBO = 807;
    public const int ACTION_DOWNLOAD_RECEOVE = 809;
    public const int ACTION_DOWNLOAD_CHECK = 810;
    public const int ACTION_DOWNLOAD_FINISH = 811;

	public const int ACTION_GETCOMBO = 808;
    public const int ACTION_GET_GAMBLESTATE = 820;

	public const int ACTION_GET_CHALLENGE_RANK = 404;
	public const int ACTION_GET_TIANXIADIYI_RANK = 306;
	public const int ACTION_GET_TIANXIADIYI_RANKSINGLE = 307;
	
	public const int ACTION_TASK_LIST = 812;
    public const int ACTION_TASK_REWARD = 813;

	public const int ACTION_GUAGUALE_STATUS = 606;
	public const int ACTION_GUAGUALE = 607;

    public const int ACTION_GODGIFT = 604;
    public const int ACTION_GOTGODGIFT = 605;

    public const int ACTION_GET_AWARDACTIVITY = 600;
	
	public const int ACTION_GET_BIGWHEEL_LIST = 601;
	public const int ACTION_USE_BIGWHEEL = 602;
	public const int ACTION_RESET_BIGWHEEL_LIST = 603;

	public const int ACTION_SECRETSHOP_BUYSOULHERO = 544;

	public const int ACTION_NEW_FINALTRIAL_STATE = 187;
	public const int ACTION_NEW_FINALTRIAL_CURDUNGEON = 188;
	public const int ACTION_NEW_FINALTRIAL_TEAM = 189;

	public const int ACTION_ACTIVATION_CODE = 821;

	public const int ACTION_RESETFINALTRIALVALUE = 191;


    public const int ACTION_HOLIDAYACTIVITY =799 ; // 限时活动

	public const int ACTION_ACTIVESHOPITEM = 547; // 活动商品
	public const int ACTION_ACTIVESHOPBUYITEM = 549; //活动商品购买
	
	#region 新的PVE战斗
	public const int ACTION_NEW_PVE_BATTLE_BOSS = 1001;
#if NewGuide
	public const int ACTION_SETTLE_BATTLE_BOSS = -1006;
#else
	public const int ACTION_SETTLE_BATTLE_BOSS = 1006;
#endif
	public const int ACTION_SETTLE_SHABU = 190;
	#endregion
	#region 扫荡
	public const int ACTION_SAODANG =1003;
	#endregion

	public const int ACTION_SYNCPVE = 1004;				//同步pve数据
	public const int ACTION_RESETFLOOR = 1005;			//重置关卡挑战次数
	public const int ACTION_SYNCMONEY = 562;			//同步钱(coin和stone)

	public const int ACTION_CURRENT_USERSATAE = 20000;
	public const int ACTION_SEND_USERDEVICE = 20001;

	public const int ACTION_GET_REVENGE_DATA = 406;

	public const int ACTION_BUY_PVEACT_FB = 1007;
	public const int ACTION_GET_SYSTEMSTATUES = 3007;

	public const int ACTION_ACTIVITYLIMITTIME = 3005;
	public const int ACTION_GETACTIVITYLIMITTIME = 3006;

	public const int ACTION_DRAGONBANKMESSAGE = 3008;
	public const int ACTION_DRAGONBANKSAVEMSG = 3009;
	public const int ACTION_RECEIVEMONEY = 3010;

	public static readonly Dictionary<RequestType, RelationShipReqAndResp> PreDefined = new Dictionary<RequestType, RelationShipReqAndResp>()
    {
		{ RequestType.UPDATE_RESOURCES,      new RelationShipReqAndResp(RequestType.UPDATE_RESOURCES,     ACTION_UPDATE_RESOURCES,    typeof(ConfigResponse))             },
		{ RequestType.LOGIN_GAME,            new RelationShipReqAndResp(RequestType.LOGIN_GAME,           ACTION_LOGIN,               typeof(LoginResponse))              },
		{ RequestType.THIRD_PARTY_LOGIN,     new RelationShipReqAndResp(RequestType.THIRD_PARTY_LOGIN,    ACTION_THRID_LOGIN,         typeof(LoginResponse))              },
		{ RequestType.PAY,    				 new RelationShipReqAndResp(RequestType.PAY,    			  ACTION_THRID_PAY,        	  typeof(PayResponse))             	  },
		{ RequestType.QUERY_PAY_STATUS,      new RelationShipReqAndResp(RequestType.QUERY_PAY_STATUS,     ACTION_QUERY_PAY_STATUS,    typeof(PayStatusResponse))          },
		{ RequestType.RECHARGECOUNT,         new RelationShipReqAndResp(RequestType.RECHARGECOUNT,        ACTION_RECHARGECOUNT,       typeof(PayCountResponse))          },


		{ RequestType.PVE_BATTLE,            new RelationShipReqAndResp(RequestType.PVE_BATTLE,           ACTION_PVE_BATTLE,          typeof(BattleResponse))             },
		{ RequestType.SM_PVE_BATTLE,         new RelationShipReqAndResp(RequestType.SM_PVE_BATTLE,        ACTION_PVE_BATTLE,          typeof(BattleResponse))             },
		{ RequestType.FIGHT_FULISA,          new RelationShipReqAndResp(RequestType.FIGHT_FULISA,         ACTION_PVE_BATTLE,          typeof(BattleResponse))             },
		{ RequestType.GET_PARTITION_SERVER,  new RelationShipReqAndResp(RequestType.GET_PARTITION_SERVER, ACTION_GET_SERVER,          typeof(GetPartitionServerResponse)) },
		{ RequestType.THIRD_GET_SERVER,  	 new RelationShipReqAndResp(RequestType.THIRD_GET_SERVER,     ACTION_THIRD_GET_SERVER,    typeof(GetPartitionServerResponse)) },

		{ RequestType.CHANGE_TEAM_MEMBER,    new RelationShipReqAndResp(RequestType.CHANGE_TEAM_MEMBER,   ACTION_CHG_TM,              typeof(ChangeTeamMemberResponse))   },
		{ RequestType.CHANGE_EQUIPMENT,      new RelationShipReqAndResp(RequestType.CHANGE_EQUIPMENT,     ACTION_CHG_EQUIP,           typeof(ChangeEquipmentResponse))    },
		{ RequestType.SELL_MONSTER,			 new RelationShipReqAndResp(RequestType.SELL_MONSTER,		  ACTION_SELL_MONSTER, 		  typeof(SellMonsterResponse))		  },
		{ RequestType.STRENGTHEN_MONSTER,	 new RelationShipReqAndResp(RequestType.STRENGTHEN_MONSTER,	  ACTION_STRENGTHEN_MONSTER,  typeof(StrengthenResponse))		  },
		{ RequestType.STRENGTHEN_EQUIPMENT,	 new RelationShipReqAndResp(RequestType.STRENGTHEN_EQUIPMENT, ACTION_STRENGTHEN_EQUIP,    typeof(StrengthEquipResponse))	  },
		{ RequestType.SELL_EQUIPMENT,	 	 new RelationShipReqAndResp(RequestType.SELL_EQUIPMENT,	  	  ACTION_SELL_EQUIPMENT,  	  typeof(SellEquipResponse))		  },
		{ RequestType.SELL_GEM,	 			 new RelationShipReqAndResp(RequestType.SELL_GEM,	  	  	  ACTION_SELL_GEM,  	      typeof(SellEquipResponse))		  },
		{ RequestType.DECOMPOSE_MONSTER,	 new RelationShipReqAndResp(RequestType.DECOMPOSE_MONSTER,	  ACTION_DECOMPOSE_MONSTER,   typeof(DecomposeMonsterResponse))	  },
		{ RequestType.EVOLVE_MONSTER,	     new RelationShipReqAndResp(RequestType.EVOLVE_MONSTER,	      ACTION_EVOLVE_MONSTER,      typeof(EvolveMonsterResponse))	  },

		{ RequestType.BUY_ITEM,         	 new RelationShipReqAndResp(RequestType.BUY_ITEM,       	  ACTION_BUY_ITEM,      	  typeof(BuyItemResponse))            },
		{ RequestType.USE_PROP,         	 new RelationShipReqAndResp(RequestType.USE_PROP,       	  ACTION_USE_PROP,      	  typeof(UsePropResponse))            },
		{ RequestType.SWAP_TEAM,         	 new RelationShipReqAndResp(RequestType.SWAP_TEAM,       	  ACTION_SWAP_TEAM,      	  typeof(SwapTeamResponse))           },
		{ RequestType.SWAP_MONSTER_POS,      new RelationShipReqAndResp(RequestType.SWAP_MONSTER_POS,     ACTION_SWAP_MONSTER_POS,    typeof(SwapMonsterPosResponse))     },
		{ RequestType.BUILD_CREATED,     	 new RelationShipReqAndResp(RequestType.BUILD_CREATED,    	  ACTION_BUILD_CREATE,    	  typeof( BuildOperateResponse ))     },
		{ RequestType.BUILD_GET,     		 new RelationShipReqAndResp(RequestType.BUILD_GET,    		  ACTION_BUILD_GET,    		  typeof( BuildOperateResponse ))     },
		{ RequestType.BATTLE_BUILD_OPEN,     new RelationShipReqAndResp(RequestType.BATTLE_BUILD_OPEN,    ACTION_BATTLE_BUILD_OPEN,   typeof( BuildOperateResponse ))     },
		{ RequestType.BUILD_UPGRADE,         new RelationShipReqAndResp(RequestType.BUILD_UPGRADE,    	  ACTION_BUILD_UPGRADE,    	  typeof( BuildOperateResponse ))     },
		{ RequestType.ZHAOMU,         		 new RelationShipReqAndResp(RequestType.ZHAOMU,    	  		  ACTION_ZHAOMU,    	      typeof( ZhaoMuResponse))    		  },
		{ RequestType.ZHAOMU_STATE,          new RelationShipReqAndResp(RequestType.ZHAOMU_STATE,    	  ACTION_ZHAOMU_STATE,    	  typeof( ZhaoMuStateResponse))    	  },
		{ RequestType.HECHENG,         		 new RelationShipReqAndResp(RequestType.HECHENG,    	  	  ACTION_HECHENG,    	      typeof( HeChengResponse))    		  },
		{ RequestType.QIANLIXUNLIAN,         new RelationShipReqAndResp(RequestType.QIANLIXUNLIAN,    	  ACTION_QIANLIXUNLIAN,    	  typeof( QianLiXunLianResponse ))    },
		{ RequestType.ATTR_SWAP,         	 new RelationShipReqAndResp(RequestType.ATTR_SWAP,    	  	  ACTION_ATTRSWAP,    	      typeof( AttrSwapResponse ))    	  },
		{ RequestType.CHANGE_USERINFO,       new RelationShipReqAndResp(RequestType.CHANGE_USERINFO,      ACTION_CHANGE_USERINFO,     typeof( ChangeUserInfoResponse ))   },
		{ RequestType.RANDOM_NAME,       	 new RelationShipReqAndResp(RequestType.RANDOM_NAME,      	  ACTION_RANDOM_NAME,         typeof( RandomNameResponse ))       },
		{ RequestType.SOULHECHENG,	 		 new RelationShipReqAndResp(RequestType.SOULHECHENG,	  	  ACTION_SOULHECHENG,  	      typeof( SoulHeChenResponse))		  },
		{ RequestType.QIANLIRESET,	 		 new RelationShipReqAndResp(RequestType.QIANLIRESET,	  	  ACTION_QIANLIRESET,  	      typeof( QianLiXunLianResponse))	  },
		{ RequestType.BATTLE_COMBO,	 		 new RelationShipReqAndResp(RequestType.BATTLE_COMBO,	  	  ACTION_BATTLE_COMBO,  	  typeof( BattleComboResponse))	  	  },
		{ RequestType.SKILL_UPGRADE,	 	 new RelationShipReqAndResp(RequestType.SKILL_UPGRADE,	  	  ACTION_SKILL_UPGRADE,  	  typeof( SkillUpgradeResponse))	  },
		{ RequestType.SYNC_PVE,	 	 		 new RelationShipReqAndResp(RequestType.SYNC_PVE,	  	  	  ACTION_SYNCPVE,  	  		  typeof( SyncPveResponse))	  		  },
		{ RequestType.RESET_FLOOR,	 	     new RelationShipReqAndResp(RequestType.RESET_FLOOR,	  	  ACTION_RESETFLOOR,  	  	  typeof( ResetFloorResponse))	  	  },
		{ RequestType.SYNC_MONEY,	 	     new RelationShipReqAndResp(RequestType.SYNC_MONEY,	  	  	  ACTION_SYNCMONEY,  	  	  typeof( SyncMoneyResponse))	  	  },

		{ RequestType.GET_TIANXIADIYI_OPPONENTS, new RelationShipReqAndResp(RequestType.GET_TIANXIADIYI_OPPONENTS, ACTION_GET_TIANXIADIYI_OPPONENTS,  typeof(GetTianXiaDiYiOpponentsResponse))},
        { RequestType.ZHANGONG_BUY_ITEM,         new RelationShipReqAndResp(RequestType.ZHANGONG_BUY_ITEM,         ACTION_ZHANGONG_BUY_ITEM,          typeof(ZhanGongBuyItemResponse))},
        { RequestType.GET_ZHANGONG_BUY_ITEM_ID,  new RelationShipReqAndResp(RequestType.GET_ZHANGONG_BUY_ITEM_ID,  ACTION_GET_ZHANGONG_BUY_ITEM_ID,   typeof(GetZhanGongBuyItemIDResponse))},
        { RequestType.TIANXIADIYI_BATTLE,        new RelationShipReqAndResp(RequestType.TIANXIADIYI_BATTLE,        ACTION_TIANXIADIYI_BATTLE,         typeof(BattleResponse))},

        { RequestType.GET_QIANGDUO_GOLD_OPPONENTS,  new RelationShipReqAndResp(RequestType.GET_QIANGDUO_GOLD_OPPONENTS, ACTION_GET_QIANGDUO_GOLD_OPPONENTS,    typeof(GetQiangDuoGoldOpponentsResponse))},
        { RequestType.QIANGDUO_GOLD_BUY_ITEM,       new RelationShipReqAndResp(RequestType.QIANGDUO_GOLD_BUY_ITEM,      ACTION_QIANGDUO_GOLD_BUY_ITEM,         typeof(QiangDuoGoldBuyItemResponse))},
        { RequestType.GET_QIANGDUO_GOLD_ITEM_TOTAL, new RelationShipReqAndResp(RequestType.GET_QIANGDUO_GOLD_ITEM_TOTAL,ACTION_GET_QIANGDUO_GOLD_ITEM_TOTAL,   typeof(GoldBuyItemBuyTotalResponse))},
        { RequestType.QIANGDUO_GOLD_BATTLE,         new RelationShipReqAndResp(RequestType.QIANGDUO_GOLD_BATTLE,        ACTION_QIANGDUO_GOLD_BATTLE,           typeof(BattleResponse))},

        { RequestType.ADD_SU_DI,      new RelationShipReqAndResp(RequestType.ADD_SU_DI, ACTION_ADD_SU_DI,            typeof(AddSuDiResponse))},
		{ RequestType.GET_SU_DI_LIST, new RelationShipReqAndResp(RequestType.GET_SU_DI_LIST, ACTION_GET_SU_DI_LIST,  typeof(GetSuDiResponse))},
        { RequestType.DELETE_SU_DI,   new RelationShipReqAndResp(RequestType.DELETE_SU_DI, ACTION_DELETE_SU_DI,      typeof(DeleteSuDiResponse))},


		{ RequestType.CALL_DRAGON,          new RelationShipReqAndResp(RequestType.CALL_DRAGON,         ACTION_CALL_DRAGON,        typeof(CallDragonResponse))    },
        { RequestType.LEARN_AOYI,           new RelationShipReqAndResp(RequestType.LEARN_AOYI,          ACTION_LEARN_AOYI ,        typeof(LearnAoYiResponse))     },
        { RequestType.EQUIP_AOYI,           new RelationShipReqAndResp(RequestType.EQUIP_AOYI,          ACTION_EQUIP_AOYI ,        typeof(EquipAoYiResponse))     },
		{ RequestType.GET_QIANGDUO_DRAGONBALL_OPPONENTS, new RelationShipReqAndResp(RequestType.GET_QIANGDUO_DRAGONBALL_OPPONENTS, ACTION_GET_QIANGDUO_DRAGONBALL_OPPONENTS , typeof(GetQiangDuoDragonBallOpponentsResponse))     },
        { RequestType.QIANGDUO_DRAGONBALL_BATTLE,        new RelationShipReqAndResp(RequestType.QIANGDUO_DRAGONBALL_BATTLE,        ACTION_QIANGDUO_DRAGONBALL_BATTLE ,        typeof(BattleResponse))     },

        { RequestType.FINALTRIAL_ADDATR,        new RelationShipReqAndResp(RequestType.FINALTRIAL_ADDATR,       ACTION_FINALTRIAL_ADDATR , typeof(FinalTrialAddAttributeResponse))     },
        { RequestType.FINALTRIAL_CHALLENGELIST, new RelationShipReqAndResp(RequestType.FINALTRIAL_CHALLENGELIST, ACTION_FINALTRIAL_CHALLENGELIST , typeof(FinalTrialChallengeListResponse))     },
        { RequestType.FINALTRIAL_CHOOSEATR,     new RelationShipReqAndResp(RequestType.FINALTRIAL_CHOOSEATR,    ACTION_FINALTRIAL_CHOOSEATR ,   typeof(FinalTrialChooseAttributeResponse))     },
        { RequestType.FINALTRIAL_DONGUENATR,    new RelationShipReqAndResp(RequestType.FINALTRIAL_DONGUENATR,   ACTION_FINALTRIAL_DONGUENATR ,  typeof(FinalTrialDungeonAddResponse))     },
//		{ RequestType.FINALTRIAL_GETRANK,       new RelationShipReqAndResp(RequestType.CHECK_TEAMRANKINFO,      ACTION_CHECK_TEAMRANKINFO ,     typeof(CheckFinalTrialRankResponse))     },
        { RequestType.FINALTRIAL_ISDONGUENATR,  new RelationShipReqAndResp(RequestType.FINALTRIAL_ISDONGUENATR, ACTION_FINALTRIAL_ISDONGUENATR, typeof(FinalTrialAddResponse))     },
        { RequestType.FINALTRIAL_FIGHT,         new RelationShipReqAndResp(RequestType.FINALTRIAL_FIGHT,        ACTION_FINALTRIAL_FIGHT ,       typeof(BattleResponse))     },
        { RequestType.FINALTRIAL_LOGINDATA,     new RelationShipReqAndResp(RequestType.FINALTRIAL_LOGINDATA,    ACTION_FINALTRIAL_LOGINDATA ,   typeof(FinalTrialShaLuBuOuAllResponse)) },
        { RequestType.BUY_MIANZHAN_TIME,        new RelationShipReqAndResp(RequestType.BUY_MIANZHAN_TIME,       ACTION_BUY_MIANZHAN_TIME ,      typeof(BuyMianZhanTimeResponse))     },
        { RequestType.BUY_AOYI_SLOT,            new RelationShipReqAndResp(RequestType.BUY_AOYI_SLOT,           ACTION_BUY_AOYI_SLOT ,          typeof(BuyAoYiSlotResponse))     },
        { RequestType.ANNOUNCE_INFO,            new RelationShipReqAndResp(RequestType.ANNOUNCE_INFO,           ACTION_ANNOUNCE_INFO ,          typeof(AnnouceResponse))     },
        { RequestType.GET_SIGNDATE_STATE, 		new RelationShipReqAndResp(RequestType.GET_SIGNDATE_STATE,		ACITON_GET_SIGNDATESTATE, 		typeof(SignDateStateResponse))      },
        { RequestType.HAVEDINNER_STATE, 		new RelationShipReqAndResp(RequestType.HAVEDINNER_STATE,		ACTION_DINNERSTATE, 		    typeof(HaveDinnerStateResponse))    },
        { RequestType.HAVEDINNER, 				new RelationShipReqAndResp(RequestType.HAVEDINNER,		 		ACITON_EATDINNER, 		 	    typeof(HaveDinnerResponse))     	},
		{ RequestType.GIVE_TAOBAOCDKEY, 		new RelationShipReqAndResp(RequestType.GIVE_TAOBAOCDKEY,		ACTION_GIVETAOBAOCDKEY, 		typeof(TaobaoResponse))     		},
        { RequestType.SIGNDAY,					new RelationShipReqAndResp(RequestType.SIGNDAY,					ACTION_SIGNDAY, 		 	    typeof(SignDayResponse))     		},
        //henry edit{ RequestType.GET_FRIEND_LIST,          new RelationShipReqAndResp(RequestType.GET_FRIEND_LIST,         ACTION_GET_FRIEND_LIST,         typeof(GetFriendListResponse))      },
        { RequestType.GET_FRIEND_REQUEST_LIST,  new RelationShipReqAndResp(RequestType.GET_FRIEND_REQUEST_LIST, ACTION_GET_FRIEND_REQUEST_LIST, typeof(GetFriendRequestListResponse))     },
        { RequestType.ADD_OR_DELETE_FRIEND,     new RelationShipReqAndResp(RequestType.ADD_OR_DELETE_FRIEND,    ACTION_ADD_OR_DELETE_FRIEND ,   typeof(AddOrDeleteFriendResponse))   },
        { RequestType.AGREE_OR_REFUSED_FRIEND,  new RelationShipReqAndResp(RequestType.AGREE_OR_REFUSED_FRIEND, ACTION_AGREE_OR_REFUSED_FRIEND, typeof(AgreeOrRefusedFriendResponse))     },
        { RequestType.SEARCH_FRIEND,            new RelationShipReqAndResp(RequestType.SEARCH_FRIEND,           ACTION_SEARCH_FRIEND,           typeof(SearchUserResponse))     },
        { RequestType.MESSAGE_INFORMATION,      new RelationShipReqAndResp(RequestType.MESSAGE_INFORMATION,     ACTION_MESSAGE_INFORMATION,     typeof(MessageInformationResponse))     },
		{ RequestType.MESSAGE_EMAIL,            new RelationShipReqAndResp(RequestType.MESSAGE_EMAIL,           ACTION_MESSAGE_EMAIL,           typeof(MegMailResponse))     },
		{ RequestType.CHANGE_MAIL_STATE,        new RelationShipReqAndResp(RequestType.CHANGE_MAIL_STATE,       ACTION_CHANGE_MAIL_STATE,       typeof(ChangeMailStateResponse))     },
		{ RequestType.SEND_EMAIL,               new RelationShipReqAndResp(RequestType.SEND_EMAIL,              ACTION_SEND_EMAIL,              typeof(ChangeMailStateResponse))     },
		{ RequestType.GET_MAIL_ATTACHMENT,      new RelationShipReqAndResp(RequestType.GET_MAIL_ATTACHMENT,     ACTION_GET_MAIL_ATTACHMENT,     typeof(GetMailAttachmentResponse))     },
        { RequestType.SECRETSHOP,               new RelationShipReqAndResp(RequestType.SECRETSHOP,              ACTION_SECRETSHOP,              typeof(SecretShopResponse))     },
        { RequestType.SECRETSHOP_BUY,           new RelationShipReqAndResp(RequestType.SECRETSHOP_BUY,          ACTION_SECRETSHOP_BUY ,         typeof(SecretShopBuyResponse))     },
        { RequestType.LEVELGIFTSTATE,  			new RelationShipReqAndResp(RequestType.LEVELGIFTSTATE ,			ACTION_LEVELGIFTSTATE,			typeof(LevelRewardStateResponse))	},
        { RequestType.LEVELGIFT_REQUEST, 		new RelationShipReqAndResp(RequestType.LEVELGIFT_REQUEST,		ACTION_LEVELGIFT_GET,			typeof(GetLevelRewardResponse))     },
        { RequestType.SEND_MESSAGE, 		    new RelationShipReqAndResp(RequestType.SEND_MESSAGE,		    ACTION_SEND_MESSAGE,			typeof(SendMessageResponse))     },
        { RequestType.GEM_SYNTHETIC,  			new RelationShipReqAndResp(RequestType.GEM_SYNTHETIC ,			ACTION_GEM_SYNTHETIC,			typeof(GemSyntheitcResponse))	},
        { RequestType.GEM_EXCHANGE,  			new RelationShipReqAndResp(RequestType.GEM_EXCHANGE ,			ACTION_GEM_EXCHANGE,			typeof(GemExChangeResponse))	},
        { RequestType.FINALTRIAL_GETRANKINFO,   new RelationShipReqAndResp(RequestType.FINALTRIAL_GETRANKINFO,	ACTION_FINALTRIAL_GETRANKINFO,	typeof(FinalTrialRankCheckInfoResponse))},
        { RequestType.GEM_INLAY_REMOVE,  	    new RelationShipReqAndResp(RequestType.GEM_INLAY_REMOVE ,	    ACTION_GEM_INLAY_REMOVE,		typeof(GemInlayRemoveResponse))	},
        { RequestType.GEM_RECASTING,  			new RelationShipReqAndResp(RequestType.GEM_RECASTING ,			ACTION_GEM_RECASTING,			typeof(GemRecastResponse))	},
		{ RequestType.GuideProgress,  			new RelationShipReqAndResp(RequestType.GuideProgress ,			ACTION_GUIDE_PROGRESS,			typeof(BaseResponse))	},
		{ RequestType.BattleVideoPlayback,  	new RelationShipReqAndResp(RequestType.BattleVideoPlayback ,	ACTION_BATTLEVIDEOPLAYBACK,		typeof(BattleVideoPlaybackResponse))	},
		{ RequestType.SingleBattleVideoPlayback,new RelationShipReqAndResp(RequestType.SingleBattleVideoPlayback,ACTION_SINGLEBATTLEVIDEOPLAYBACK,typeof(BattleVideoPlaybackSingleResponse))	},
		{ RequestType.SEVENDAYREWARD,  			new RelationShipReqAndResp(RequestType.SEVENDAYREWARD ,			ACTION_SEVENDAYREWARD,			typeof(SevenDaysListResponse))	},
        { RequestType.SEVENDAYREWARD_BUY,  	    new RelationShipReqAndResp(RequestType.SEVENDAYREWARD_BUY ,		ACTION_SEVENDAYREWARD_BUY,		typeof(SevenDaysBuyResponse))	},
        { RequestType.OPENTREASUREBOX,          new RelationShipReqAndResp(RequestType.OPENTREASUREBOX,         ACTION_OPENTREASUREBOX,         typeof(GetTresureResponse))     },
        { RequestType.VIPSHOPINFO,              new RelationShipReqAndResp(RequestType.VIPSHOPINFO,             ACTION_VIPSHOPINFO,             typeof(GetVipShopInfoResponse)) },
        { RequestType.GETVIPGIFT,               new RelationShipReqAndResp(RequestType.GETVIPGIFT,              ACTION_GETVIPGIFT,              typeof(GetVipGiftResponse))     },
        { RequestType.GETTREASURESTATE,         new RelationShipReqAndResp(RequestType.GETTREASURESTATE,        ACTION_GETTREASURESTATE,        typeof(GetTreasureStateResponse))    },
		{ RequestType.FINALTRIAL_GETFINALLOGININFO,    new RelationShipReqAndResp(RequestType.FINALTRIAL_GETFINALLOGININFO,     ACTION_FINALTRIAL_GETFINALLOGININFO, typeof(GetDuoBaoLoginInfoResponse))    },
		{ RequestType.NEW_FINALTRIAL_ENTER,            new RelationShipReqAndResp(RequestType.NEW_FINALTRIAL_ENTER,             ACTION_NEW_FINALTRIAL_ENTER,         typeof(NewFinalTrialEnterResponse))    },
		{ RequestType.NEW_FINALTRIAL_ADDBUFFER,        new RelationShipReqAndResp(RequestType.NEW_FINALTRIAL_ADDBUFFER,         ACTION_NEW_FINALTRIAL_ADDBUFFER,     typeof(NewFinalTrialAddBufferResponse))},
		{ RequestType.NEW_FINALTRIAL_MAP,              new RelationShipReqAndResp(RequestType.NEW_FINALTRIAL_MAP,               ACTION_NEW_FINALTRIAL_MAP,           typeof(NewFinalTrialMapResponse))    },
		{ RequestType.NEW_FINALTRIAL_MAP_NOTADD,       new RelationShipReqAndResp(RequestType.NEW_FINALTRIAL_MAP_NOTADD,        ACTION_NEW_FINALTRIAL_MAP_NOTADD,    typeof(NewFinalTrialMapResponse))    },
		{ RequestType.NEW_FINALTRIAL_BATTLE,           new RelationShipReqAndResp(RequestType.NEW_FINALTRIAL_BATTLE,            ACTION_NEW_FINALTRIAL_BATTLE,        typeof(NewFinalTrialFightResponse))  },
		{ RequestType.NEW_FINALTRIAL_GETRANK,          new RelationShipReqAndResp(RequestType.NEW_FINALTRIAL_GETRANK,           ACTION_NEW_FINALTRIAL_GETRANK,       typeof(GetNewFinalTrialRankResponse))},
		{ RequestType.CHECK_TEAMRANKINFO,      new RelationShipReqAndResp(RequestType.CHECK_TEAMRANKINFO,       ACTION_CHECK_TEAMRANKINFO,   typeof(GetNewFinalTrialRankCheckInfoResponse))    },
        { RequestType.DOWNLOAD_RECEOVE,          new RelationShipReqAndResp(RequestType.DOWNLOAD_RECEOVE,       ACTION_DOWNLOAD_RECEOVE,       typeof(GetDownloadReceoveResponse))   },
        { RequestType.DOWNLOAD_CHECK,            new RelationShipReqAndResp(RequestType.DOWNLOAD_CHECK,         ACTION_DOWNLOAD_CHECK,         typeof(GetDownloadCheckResponse))     },
        { RequestType.DOWNLOAD_FINISH,           new RelationShipReqAndResp(RequestType.DOWNLOAD_FINISH,        ACTION_DOWNLOAD_FINISH,        typeof(GetDownloadFinishResponse))    },
		{ RequestType.MOVABLE_NOTICE,            new RelationShipReqAndResp(RequestType.MOVABLE_NOTICE,         ACTION_MOVABLE_NOTICE,         typeof(MovableNoticeResponse))        },
		{ RequestType.GET_COMBO,                 new RelationShipReqAndResp(RequestType.GET_COMBO,        		ACTION_GETCOMBO,               typeof(ComboResponse))        },
        { RequestType.GET_GAMBLESTATE,           new RelationShipReqAndResp(RequestType.GET_GAMBLESTATE,        ACTION_GET_GAMBLESTATE,        typeof(GetGambleStateResponse))       },
		{ RequestType.GET_CHALLENGE_RANK,        new RelationShipReqAndResp(RequestType.GET_CHALLENGE_RANK,     ACTION_GET_CHALLENGE_RANK,     typeof(GetChallengeRankResponse))       },
		{ RequestType.GET_TIANXIADIYI_RANK,      new RelationShipReqAndResp(RequestType.GET_TIANXIADIYI_RANK,   ACTION_GET_TIANXIADIYI_RANK,   typeof(GetChallengeRankResponse))       },
        { RequestType.GET_CALLDRAGONISFINISH,    new RelationShipReqAndResp(RequestType.GET_CALLDRAGONISFINISH, ACTION_GET_CALLDRAGONISFINISH, typeof(GetCallDragonIsFinishedResponse))       },
		{ RequestType.Task_List,                 new RelationShipReqAndResp(RequestType.Task_List,              ACTION_TASK_LIST,              typeof(TaskListResponse))       },
		{ RequestType.Task_Reward,               new RelationShipReqAndResp(RequestType.Task_Reward,            ACTION_TASK_REWARD,            typeof(TaskRewardResponse))       },
		{ RequestType.GUAGUALE_STATUS,           new RelationShipReqAndResp(RequestType.GUAGUALE_STATUS,        ACTION_GUAGUALE_STATUS,        typeof(GuaGuaStatusResponse))       },
		{ RequestType.GUAGUALE,                  new RelationShipReqAndResp(RequestType.GUAGUALE,               ACTION_GUAGUALE,               typeof(GuaGuaLeResponse))       },
        { RequestType.GET_GODGIFTLIST,           new RelationShipReqAndResp(RequestType.GET_GODGIFTLIST,        ACTION_GODGIFT,                typeof(GotGodGiftListResponse))       },
        { RequestType.GET_GODGIFT,               new RelationShipReqAndResp(RequestType.GET_GODGIFT,            ACTION_GOTGODGIFT,             typeof(GotGodGiftResponse))       },
		{ RequestType.GET_BIGWHEEL_LIST,         new RelationShipReqAndResp(RequestType.GET_BIGWHEEL_LIST,      ACTION_GET_BIGWHEEL_LIST,      typeof(BigWheelListResponse))       },
		{ RequestType.USE_BIGWHEEL,              new RelationShipReqAndResp(RequestType.USE_BIGWHEEL,           ACTION_USE_BIGWHEEL,           typeof(UseBigWheelResponse))       },
		{ RequestType.RESET_BIGWHEEL_LIST,       new RelationShipReqAndResp(RequestType.RESET_BIGWHEEL_LIST,    ACTION_RESET_BIGWHEEL_LIST,    typeof(BigWheelListResponse))       },
        { RequestType.GET_AWARDACTIVITY,         new RelationShipReqAndResp(RequestType.GET_AWARDACTIVITY,      ACTION_GET_AWARDACTIVITY,      typeof(GetAwardActivity))   },
        { RequestType.CURRENT_USERSATAE,         new RelationShipReqAndResp(RequestType.CURRENT_USERSATAE,      ACTION_CURRENT_USERSATAE,	   typeof(SendResponse))},
		{ RequestType.GET_MONTHGIFT,             new RelationShipReqAndResp(RequestType.GET_MONTHGIFT,          ACTION_GETMONTHGIFT,           typeof(GetMonthGiftResponse))   },
		{ RequestType.SECRETSHOP_BUYSOULHERO,    new RelationShipReqAndResp(RequestType.SECRETSHOP_BUYSOULHERO, ACTION_SECRETSHOP_BUYSOULHERO, typeof(SecretShopBuyResponse))   },

		{ RequestType.GET_MONTHSTATE,            new RelationShipReqAndResp(RequestType.GET_MONTHSTATE,         ACTION_GET_PAYCOUNT,           typeof(GetMonthGiftStateResponse))   },
		{ RequestType.GET_FIRSTCHARGESTATE,      new RelationShipReqAndResp(RequestType.GET_FIRSTCHARGESTATE,   ACTION_GET_FIRSTCHARGESTATE,   typeof(GetFirstChargeStateResponse)) },
		{ RequestType.GET_FIRSTCHARGEGIFT,       new RelationShipReqAndResp(RequestType.GET_FIRSTCHARGEGIFT,    ACTION_GET_FIRSTCHARGEGIFT,    typeof(GetFirstChargeGiftResponse))  },
		{ RequestType.NEW_PVE_BOSSBATTLE,        new RelationShipReqAndResp(RequestType.NEW_PVE_BOSSBATTLE,     ACTION_NEW_PVE_BATTLE_BOSS,    typeof(ClientBattleResponse))  },
        { RequestType.SETTLE_BOSSBATTLE,         new RelationShipReqAndResp(RequestType.SETTLE_BOSSBATTLE,      ACTION_SETTLE_BATTLE_BOSS,     typeof(BattleResponse))  },
		{ RequestType.SETTLE_SHABU,              new RelationShipReqAndResp(RequestType.SETTLE_SHABU,           ACTION_SETTLE_SHABU,           typeof(ClientBTShaBuResponse))  },
		{ RequestType.SYNC_CALLDRAGONTIME,       new RelationShipReqAndResp(RequestType.SYNC_CALLDRAGONTIME,    ACIION_SYNC_CALLDRAGONTIME,    typeof(SyncCallDragonTimeResponse))  },
		{ RequestType.SEND_USERDEVICE,           new RelationShipReqAndResp(RequestType.SEND_USERDEVICE,        ACTION_SEND_USERDEVICE,        typeof(SendDeviceInfoResponse))  },
		{ RequestType.SAODANG,                   new RelationShipReqAndResp(RequestType.SAODANG,                ACTION_SAODANG,                typeof(SaoDangResponse))  },
		{ RequestType.NEW_FINALTRIAL_STATE,      new RelationShipReqAndResp(RequestType.NEW_FINALTRIAL_STATE,   ACTION_NEW_FINALTRIAL_STATE,   typeof(GetFinalTrialStateResponse))  },
		{ RequestType.NEW_FINALTRIAL_CURDUNGEON, new RelationShipReqAndResp(RequestType.NEW_FINALTRIAL_CURDUNGEON, ACTION_NEW_FINALTRIAL_CURDUNGEON, typeof(GetFinalTrialCurDungeonResponse))  },
		{ RequestType.NEW_FINALTRIAL_TEAM,       new RelationShipReqAndResp(RequestType.NEW_FINALTRIAL_TEAM,    ACTION_NEW_FINALTRIAL_TEAM,    typeof(ClientBattleResponse))  },
        { RequestType.REFRESH_SECRETSHOP ,       new RelationShipReqAndResp(RequestType.REFRESH_SECRETSHOP,     ACTION_REFRESH_SECRETSHOP,     typeof(RefreshSecretShopResponse))  },
        { RequestType.BUY_ENERGY,                new RelationShipReqAndResp(RequestType.BUY_ENERGY,             ACTION_BUY_ENERGY,             typeof(getJLData))  },
		{ RequestType.ACTIVATION_CODE,           new RelationShipReqAndResp(RequestType.ACTIVATION_CODE,        ACTION_ACTIVATION_CODE,        typeof(UsePropResponse))  },
		{ RequestType.GET_TIANXIADIYI_RANKSINGLE,new RelationShipReqAndResp(RequestType.GET_TIANXIADIYI_RANKSINGLE,ACTION_GET_TIANXIADIYI_RANKSINGLE,typeof(GetChallengeRankResponse))       },
		{ RequestType.GET_REVENGE_DATA,          new RelationShipReqAndResp(RequestType.GET_REVENGE_DATA,       ACTION_GET_REVENGE_DATA,       typeof(RevengeResponse))       },
		{ RequestType.RESETFINALTRIALVALUE,      new RelationShipReqAndResp(RequestType.RESETFINALTRIALVALUE,   ACTION_RESETFINALTRIALVALUE,   typeof(GetFinalTrialStateResponse))       },
		{ RequestType.GETVIPLEVELREWARD,         new RelationShipReqAndResp(RequestType.GETVIPLEVELREWARD,      ACTION_GETVIPLEVELREWARD,      typeof(GetVipLevelRewardResponse))       },
		{ RequestType.BUY_PVEACT_FB,             new RelationShipReqAndResp(RequestType.BUY_PVEACT_FB,          ACTION_BUY_PVEACT_FB,          typeof(SyncPveResponse))       },
        { RequestType.HOLIDAYACTIVITY,           new RelationShipReqAndResp(RequestType.HOLIDAYACTIVITY,        ACTION_HOLIDAYACTIVITY,        typeof(HolidayActivityResponse))       },
		{ RequestType.ACTIVESHOPITEM,           new RelationShipReqAndResp(RequestType.ACTIVESHOPITEM,        ACTION_ACTIVESHOPITEM,        typeof(ActiveShopItemResponse))       },
		{ RequestType.ACTIVESHOPBUYITEM,           new RelationShipReqAndResp(RequestType.ACTIVESHOPBUYITEM,        ACTION_ACTIVESHOPBUYITEM,        typeof(BuyItemResponse))       },
		{ RequestType.REFRESH_ZHANGONG_BUY_ITEM,           new RelationShipReqAndResp(RequestType.REFRESH_ZHANGONG_BUY_ITEM,        ACTION_REFRESH_ZHANGONG_BUY_ITEM,        typeof(RefreshZhangongShopItemResponse))       },
		{ RequestType.GetSYSTEMSTATUES,           new RelationShipReqAndResp(RequestType.GetSYSTEMSTATUES,        ACTION_GET_SYSTEMSTATUES,        typeof(GetSystemStatusData))       },
		{ RequestType.ACTIVITYLIMITTIME,           new RelationShipReqAndResp(RequestType.ACTIVITYLIMITTIME,        ACTION_ACTIVITYLIMITTIME,        typeof(NewActivityResponse))       },
		{ RequestType.DRAGONBANKMESSAGE,           new RelationShipReqAndResp(RequestType.DRAGONBANKMESSAGE,        ACTION_DRAGONBANKMESSAGE,        typeof(GetDragonBankResponse))       },
		{ RequestType.DRAGONBANKSAVEMSG,           new RelationShipReqAndResp(RequestType.DRAGONBANKSAVEMSG,        ACTION_DRAGONBANKSAVEMSG,        typeof(GetDragonBankResponse))		},
		{ RequestType.RECEIVEMONEY,           		new RelationShipReqAndResp(RequestType.RECEIVEMONEY,        	ACTION_RECEIVEMONEY,        	typeof(ReceiveBankMoney))		},
		//... to do....s
    };

	public static HttpRequest createHttpRequestInstance(RequestType type, BaseRequestParam reqParam,string urlAddress = ""){
		HttpRequest req = new HttpRequest(type, swInfo, Convert.ToString(platformId),_sessionId, urlAddress);
		if (reqParam != null && Enum.IsDefined(typeof(RequestType), type)) {

            RelationShipReqAndResp preDef = PreDefined[type];
            
            if (preDef != null) {
				req.setParameter(HttpRequest.ACTION, preDef.requestAction);
                req.appendPara(reqParam);
            }
            else {
                throw new DragonException("Dictionary PreDefined is not defined.");
            }
        } else {
            throw new DragonException(DragonException.Exception_Message[DragonException.INVALIDATE_ARGUMENT]);
        }
        return req;
    }

	public static RelationShipReqAndResp getRelationShip(RequestType type) {
		return PreDefined[type];
	}


}  