using System;
using System.Collections.Generic;

public class SocketRequestFactory
{
	//This will be fullfilled after game is launched.
	public static int swInfo;
	//This will be fullfilled after game is launched.
	public static int platformId;

	public const int ACTION_DEFAULT = -1;
	//login in socket  wxl 
	public const int ACTION_LOGIN = 1001;
	public const int ACTION_ATTACKBOSS = 1002;
	public const int ACTION_LOGOUTBOSS = 1003;
	public const int ACTION_GETBLOOD_ATKLIST = 1004;
	public const int ACTION_LOGINBOSS = 1005;

	public const int ACTION_CLOSE = 2001;
	public const int ACTION_GETACTTIME = 1006;
	public const int ACTION_ACTSTATE = 1007;
	public const int ACTION_ACTADDPOWER = 1008;
	public const int ACTION_HONORBUYITEM = 1009;
	public const int ACTION_LOGINFESTIVAL = 1010;
	public const int ACTION_BUYLOTTERY = 1011;
	public const int ACTION_LOGOUTFESTIVAL = 1012;
	public const int ACTION_GETSCORERANKLIST = 1013;

	public const int ACTION_WORLDCHATLOGIN = 1100;
	public const int ACTION_WORLDCHAT = 1102;
    //雷达组队
    public const int ACTION_LOGINGPSWAR = 1201;
    public const int ACTION_GETROOMLIST = 1202;
    public const int ACTION_LOGININROOM = 1203;
    public const int ACTION_LOGGOUTROOM = 1204;
    public const int ACTION_CREATROOM = 1205;
    public const int ACTION_GPSFIGHT = 1206;
    public const int ACTION_FIGHTCOMPLETE = 1207;
    public const int ACTION_LOGOUTGPSWAR = 1208;
    public const int ACTION_SYNCLOCATION = 1209;
	public const int ACTION_SYNCROOM = 1210;

	public static readonly Dictionary<RequestType, RelationShipReqAndResp> PreDefined = new Dictionary<RequestType, RelationShipReqAndResp>()
	{
		{ RequestType.SOCK_LOGIN,            new RelationShipReqAndResp(RequestType.SOCK_LOGIN,           	ACTION_LOGIN,               typeof(SockLoginResponse))              },
		{ RequestType.SOCK_CLOSE,            new RelationShipReqAndResp(RequestType.SOCK_CLOSE,          	ACTION_CLOSE,               typeof(SockCloseResponse))              },
		{RequestType.SOCK_GETBLOOD_ATKLIST,  new RelationShipReqAndResp(RequestType.SOCK_GETBLOOD_ATKLIST,     ACTION_GETBLOOD_ATKLIST,       typeof(SockUpdateAtkBloodListResponse))    		 },
		{ RequestType.SOCK_ATTACKBOSS,       new RelationShipReqAndResp(RequestType.SOCK_ATTACKBOSS,        ACTION_ATTACKBOSS,          typeof(SockAttackBossResponse))          },
		{ RequestType.SOCK_LOGOUTBOSS,       new RelationShipReqAndResp(RequestType.SOCK_LOGOUTBOSS,        ACTION_LOGOUTBOSS,          typeof(SockLogOutAttackBossResponse))    },
		{RequestType.SOCK_LOGINBOSS,         new RelationShipReqAndResp(RequestType.SOCK_LOGINBOSS,     	ACTION_LOGINBOSS,     	    typeof(SockLoginBossResponse))    		 },
		{RequestType.SOCK_GET_ACTTIME,     new RelationShipReqAndResp(RequestType.SOCK_GET_ACTTIME,     ACTION_GETACTTIME,        typeof(SockGetActTimeResponse))        },
		{RequestType.SOCK_ACTIVITYSTATE,     new RelationShipReqAndResp(RequestType.SOCK_ACTIVITYSTATE,     ACTION_ACTSTATE,           	typeof(SockActivityStateResponse))       },
		{RequestType.SOCK_ADDPOWER,          new RelationShipReqAndResp(RequestType.SOCK_ADDPOWER,          ACTION_ACTADDPOWER,         	typeof(SockAddPowerResponse))        },
		{RequestType.SOCK_HONORBUYITEM,      new RelationShipReqAndResp(RequestType.SOCK_HONORBUYITEM,     ACTION_HONORBUYITEM,         typeof(SockBuyItemResponse) )},

		{RequestType.SOCK_LOGINFESTIVAL,    	 new RelationShipReqAndResp(RequestType.SOCK_LOGINFESTIVAL,     ACTION_LOGINFESTIVAL,           	typeof(SockLoginFestivalResponse))       },
		{RequestType.SOCK_BUYLOTTERY,          new RelationShipReqAndResp(RequestType.SOCK_BUYLOTTERY,          ACTION_BUYLOTTERY,         	typeof(SockFestivalBuyLotteryResponse))        },
		{RequestType.SOCK_LOGOUTFESTIVAL,      new RelationShipReqAndResp(RequestType.SOCK_LOGOUTFESTIVAL,    ACTION_LOGOUTFESTIVAL,         typeof(SockLogOutFestivalResponse) )},
		{RequestType.SOCK_GETSCORERANKLIST,      new RelationShipReqAndResp(RequestType.SOCK_GETSCORERANKLIST,    ACTION_GETSCORERANKLIST,         typeof(SockGetRankListResponse) )},

		{RequestType.SOCK_WORLDCHATLOGIN,      new RelationShipReqAndResp(RequestType.SOCK_WORLDCHATLOGIN,    ACTION_WORLDCHATLOGIN,         typeof(SockLoginWorldChatResponse) )},
		{RequestType.SOCK_WORLDCHAT,      new RelationShipReqAndResp(RequestType.SOCK_WORLDCHAT,    ACTION_WORLDCHAT,         typeof(SockWorldChatResponse) )},

        {RequestType.SOCK_LOGINGPSWAR,      new RelationShipReqAndResp(RequestType.SOCK_LOGINGPSWAR,    ACTION_LOGINGPSWAR,         typeof(SockLoginGPSWarResponse))},
        {RequestType.SOCK_GETROOMLIST,      new RelationShipReqAndResp(RequestType.SOCK_GETROOMLIST,    ACTION_GETROOMLIST,         typeof(SockGetRoomListResponse))},
        {RequestType.SOCK_LOGININROOM,      new RelationShipReqAndResp(RequestType.SOCK_LOGININROOM,    ACTION_LOGININROOM,         typeof(SockLoginRoomResponse))},
        {RequestType.SOCK_LOGGOUTROOM,      new RelationShipReqAndResp(RequestType.SOCK_LOGGOUTROOM,    ACTION_LOGGOUTROOM,         typeof(SockLogOutRoomResponse))},
        {RequestType.SOCK_CREATROOM,      new RelationShipReqAndResp(RequestType.SOCK_CREATROOM,    ACTION_CREATROOM,         typeof(SockCreatRoomResponse))},
        {RequestType.SOCK_GPSFIGHT,      new RelationShipReqAndResp(RequestType.SOCK_GPSFIGHT,    ACTION_GPSFIGHT,         typeof(SockFightResponse))},
        {RequestType.SOCK_FIGHTCOMPLETE,      new RelationShipReqAndResp(RequestType.SOCK_FIGHTCOMPLETE,    ACTION_FIGHTCOMPLETE,         typeof(SockFightCompleteResponse))},
        {RequestType.SOCK_LOGOUTGPSWAR,      new RelationShipReqAndResp(RequestType.SOCK_LOGOUTGPSWAR,    ACTION_LOGOUTGPSWAR,         typeof(SockLogOutGPSWarResponse))},
        {RequestType.SOCK_SYNCLOCATION,      new RelationShipReqAndResp(RequestType.SOCK_SYNCLOCATION,    ACTION_SYNCLOCATION,         typeof(SockSyncLocationResponse))},
		{RequestType.SOCK_SYNCROOM,      new RelationShipReqAndResp(RequestType.SOCK_SYNCROOM,    ACTION_SYNCROOM,         typeof(SockSyncRoomResponse))},
		//... to do....
	};

	public static SocketRequest createHttpRequestInstance(RequestType type, BaseRequestParam reqParam){
		SocketRequest req = new SocketRequest(type, swInfo, Convert.ToString(platformId));
		if (reqParam != null && Enum.IsDefined(typeof(RequestType), type)) {

			RelationShipReqAndResp preDef = PreDefined[type];
			if (preDef != null) {
				req.setParameter(SocketRequest.ACTION, preDef.requestAction);
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


