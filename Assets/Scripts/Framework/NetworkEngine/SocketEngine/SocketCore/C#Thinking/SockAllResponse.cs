using System;
using Framework;
using fastJSON;
using System.Collections.Generic;
using AW.Battle;

/*
 * 
 * 所有的服务器响应格式的定义都在这里。
 * 
 */
#region 登陆  1001
public class SockLoginData
{
	/// <summary>
	///  返回值  0 lose  1 win 
	/// </summary>
	public int retCode;
	//0 : lose ,  1 : win
}

[Serializable]
public class SockLoginResponse : BaseResponse
{
	public SockLoginData data;

	public SockLoginResponse ()
	{ 
	}

	public override void handleResponse ()
	{
		base.handleResponse ();

	}
}
#endregion
#region 攻击世界boss   1002


[Serializable]
public class SockAttackBossResponse : BaseResponse
{
	//public SockAttackBossData data;
	public BattleSequence data;

	public SockAttackBossResponse ()
	{
	}

	public override void handleResponse ()
	{
		//decompress the string and then json.deserize it
		if (data != null ) {
            if (!string.IsNullOrEmpty (data.battleData.rsty)) {
                string compressed = data.battleData.rsty;
                string json = DeflateEx.Decompress (compressed);

                data.battleData.classType = JSON.Instance.ToObject (json, typeof(List<short>)) as List<short>;
            }
		}

		if (data != null && data.battleData.classType != null && data.battleData.rsmg != null) {
			string compressed = data.battleData.rsmg;
			if (!string.IsNullOrEmpty (compressed)) {
				string[] jsons = DeflateEx.Decompress (compressed).Split ('@');
				if (jsons != null) {
					int length = jsons.Length;

					if (length == data.battleData.classType.Count) {
						data.battleData.info = new List<CMsgHeader> ();

						for (int i = 0; i < length; ++i) {
							Type type = CMsgHeader.Table [data.battleData.classType [i]];
							data.battleData.info.Add (JSON.Instance.ToObject (jsons [i], type) as CMsgHeader);
							//	data.info.Add( JsonFx.Json.JsonReader.Deserialize(jsons[i], type) as War.CMsgHeader);
						}

					}

				}
			}
		}



	}
}
#endregion
#region 退出世界boss房间   1003
public class SockLogOutAttackBossData
{
	//string playerId;   //0 :lose  1 win
}

[Serializable]
public class SockLogOutAttackBossResponse : BaseResponse
{
	public SockLogOutAttackBossData data;

	public SockLogOutAttackBossResponse ()
	{
	}

	public override void handleResponse ()
	{
		base.handleResponse ();
	}
}
#endregion
#region 攻击世界boss排行榜和血量（1004）
public class UserAtkBossInfo
{
	public int userId;
	public string userName;
	public int hurt;
}

public class SockBossAtkListData
{
	/// <summary>
	/// 实时的伤害
	/// </summary>
	public int attCur;
	/// <summary>
    /// boss当前的hp 也就是 伤害
    /// 
	/// </summary>
	public int bossCurHp;
	public int userPoint;
	public int isKill;
	public long killId;
	public string killName;
	public List<UserAtkBossInfo> attStrList;
	//需要转换
}

[Serializable]
public class SockUpdateAtkBloodListResponse : BaseResponse
{
	public SockBossAtkListData data;

	public SockUpdateAtkBloodListResponse ()
	{
	}

	public override void handleResponse ()
	{
		base.handleResponse ();
	}
}
#endregion
#region 登陆boss room    1005
public class SockLoginBossData
{
	/// <summary>
	/// 玩家ID
	/// </summary>
	//	public String playerId;   //0 : lose ,  1 : win
	public int retCode;
	public int bossId;
	public int bossHp;
	public int bossLev;
	public int buyTimes;
	public int reliveTime;
	public int add;
}

[Serializable]
public class SockLoginBossResponse : BaseResponse
{
	public SockLoginBossData data;

	public SockLoginBossResponse ()
	{
	}

	public override void handleResponse ()
	{
		base.handleResponse ();

	}
}
#endregion
#region 获取活动时间   1006
public class SockGetActTimeData
{
	/// <summary>
	/// 活动状态   0 关闭  1开启
	/// </summary>
	public int state;
	/// <summary>
	/// 活动类型  0 世界boss    1：武者节日
	/// </summary>
	public int type;
	/// <summary>
	/// 现在时间
	/// </summary>
	public string nowTime;
	/// <summary>
	/// 结束时间
	/// </summary>
	public string time;

    public int bossState;
    public int wuzheState;
}

[Serializable]
public class SockGetActTimeResponse : BaseResponse
{
	public SockGetActTimeData data;

	public SockGetActTimeResponse ()
	{
	}

	public override void handleResponse ()
	{
		base.handleResponse ();

	}
}
#endregion
#region 活动状态    1007
public class SockGetActivityStateData
{
	public List<int> state;
}

[Serializable]
public class SockActivityStateResponse:BaseResponse
{
	public SockGetActivityStateData data;

	public SockActivityStateResponse ()
	{
	
	}

	public override void handleResponse ()
	{
		base.handleResponse ();

	}
}
#endregion
#region  花钱加成 1008
public class SockAddPowerData
{
	public int retCode;
	//0成功 ， 1 失败
	//public int  retCold;
	public int type;
    public int count;
//	public  int diaNum = 50;
//	public  int coinNum = 100000;
}

public class SockAddPowerResponse:BaseResponse
{
	public SockAddPowerData data;

	public SockAddPowerResponse ()
	{
	}

	public override void handleResponse ()
	{


	}
}
#endregion
#region  兑换商品 1009
//public  class HonorItemData
//{
//	public int rank;
//	public int goodId;
//	public int count;
//	public int cost;
//	public string name;
//}

public class SockHonorBuyItemData
{
	public int retCode;
	public  ItemOfReward p;
 }

public class SockBuyItemResponse:BaseResponse
{
	public SockHonorBuyItemData data;

	public SockBuyItemResponse ()
	{
	}

	public override void handleResponse ()
	{


	}
}
#endregion
#region 关闭socket
public class SockCloseData
{
}

[Serializable]
public class SockCloseResponse : BaseResponse
{
	public SockCloseData data;

	public SockCloseResponse ()
	{
	}

	public override void handleResponse ()
	{
		base.handleResponse ();
	}
}
#endregion
#region  武者的节日    1010---1012
//   登录武者的节日   1010
public class SockLoginFestivalData
{
	public int retCode;
	public string freeTime;
	public string nowTime;
	public int times;
	public int jifen;
	public string startTime;
	public string endTime;
	public int goodId;
	public int goodNum;
	public int wzTimes;

    public int stoneTimes;
}

[Serializable]
public class SockLoginFestivalResponse : BaseResponse
{
	public SockLoginFestivalData data;

	public SockLoginFestivalResponse ()
	{
	}

	public override void handleResponse ()
	{
		base.handleResponse ();
	}
}

//public class FestivalGiftInfo
//{
//	public int ppid;
//	public int pid;
//	public int num;
//	public short lv;
//	public int ep;
//	public short at;
//	public int ak;
//	public int df;
//}
//  1011   进行抽奖
public class SockFestivalBugLotteryData
{
	public int retCode;
	public ItemOfReward p;
	public int type;
	public int jifen;
	public string freeTime;
    public int times;
    public int stoneTimes;
    public int stone;
	public int wzTimes;
}

[Serializable]
public class SockFestivalBuyLotteryResponse : BaseResponse
{
	public SockFestivalBugLotteryData data;

	public SockFestivalBuyLotteryResponse ()
	{
	}

	public override void handleResponse ()
	{
		base.handleResponse ();
	}
}
//   退出武者的节日   1012
public class SockLogOutFestivalData
{
	public int retCode;
}

[Serializable]
public class SockLogOutFestivalResponse : BaseResponse
{
	public SockLogOutFestivalData data;

	public SockLogOutFestivalResponse ()
	{
	}

	public override void handleResponse ()
	{
		base.handleResponse ();
	}
}
//积分排行榜    1013
public class SockFRankItem
{
	public int userId;
	public string userName;
	public int point;
}

public class SockRankListData
{
	/// <summary>
	/// 积分排行榜
	/// </summary>
	public List<SockFRankItem> pointList;
}

[Serializable]
public class SockGetRankListResponse : BaseResponse
{
	public SockRankListData data;

	public SockGetRankListResponse ()
	{
	}

	public override void handleResponse ()
	{
		base.handleResponse ();
	}
}
#endregion

#region  聊天
public class SockWorldChatResponse : BaseResponse
{
	public SockWorldChatData data;
	
	public SockWorldChatResponse ()
	{
	}
	
	public override void handleResponse ()
	{
		base.handleResponse ();
	}
}

public class SockWorldChatData
{
	public int chatCountPerDay;
	public string content ;
	public string roleName;
	public long roleid;
	public int rolelv;
	public long time;
	public long iconid;
}



#endregion

#region  登录聊天服务器
public class SockLoginWorldChatResponse : BaseResponse
{
	public SockLoginWorldChatData data;
	
	public SockLoginWorldChatResponse ()
	{
	}
	
	public override void handleResponse ()
	{
		base.handleResponse ();
	}
}

public class SockLoginWorldChatData
{
	public int retCode;
	public int roleid;
	public int chatCountPerDay;
	public SockWorldChatData[] contentList;

}

#endregion


#region  雷达协议返回
public class SockLoginGPSWarResponse : BaseResponse
{
	public LoginGPSData data;
}

public class LoginGPSData
{
	public int retCode;
	public int maxTimes;
	public int nowTimes;
}

//获取房间列表 1202
public class SockGetRoomListResponse:BaseResponse
{
	public UpdateRoomListData data;
}

public class UpdateRoomListData
{
	/// <summary>
	///  1 添加房间 2 更新房间 3 删除房间
	/// </summary>
	public int type;
	public int retCode;

	public SockRoomInfo[] rooms;
}

//进入房间
public class SockLoginRoomResponse:BaseResponse
{
	public GPSRoomData data;
    public SockLoginRoomResponse(){
    }
}
	
//房间详细信息
public class GPSRoomData
{
	public GPSRoomInfo room;
	public RoomMembersData[] members;
	public int retCode;

	//队伍容量
	public int totalMember
	{
		get
		{
			return 2;
		}
	}

	//队伍当前人数
	public int validMember
	{
		get
		{
			if (members == null)
			{
				return 0;
			}
			if (members.Length == 0)
			{
				return 0;
			}

			int count = 0;
			for (int i = 0; i < members.Length; i++)
			{
				if (members [i] != null)
				{
					count++;
				}
			}
			return count;
		}
	}
}

//房间列表信息
public class SockRoomInfo{
	public int roomId;
	public int joinState;
	public int roomMaxMemberNum;
	public int nowRoomMemberNum; 
	public string masterName;
	public int masterLevel;
}
	
//房间基本信息
public class GPSRoomInfo
{
	public int roomId;
	public int roomState;
	public int roomMaxMemberNum;
	public int nowRoomMemberNum; 
}

//队员信息
	public class RoomMembersData{
	public string memberName;
	public int memberState;
	public int memberType;
	public int iconId;
	public int memberLv;
	public int atk;
	public int def;
	public int memberId;
	public int zoneId;
}

//退出房间
public class SockLogOutRoomResponse:BaseResponse{

	public ExitRoomData data;
    public SockLogOutRoomResponse()
	{
    }
}

public class ExitRoomData
{
	public int roomId;
	public int retCode;
	public int memberId;
	public int zoneId;
}

//创建房间
public class SockCreatRoomResponse:BaseResponse{

	public GPSRoomData data;
}

//GPS战斗
public class SockFightResponse:BaseResponse
{
	public GPSWarRespData data;
    public SockFightResponse()
	{
    }

	public override void handleResponse ()
	{
		if (data == null || data.retCode != 1)
        {
            return;
        }

		//decompress the string and then json.deserize it
		if (data != null && !string.IsNullOrEmpty (data.battle.battleData.rsty)) {
			string compressed = data.battle.battleData.rsty;
			string json = DeflateEx.Decompress (compressed);
			
			data.battle.battleData.classType = JSON.Instance.ToObject (json, typeof(List<short>)) as List<short>;
			
		}
		
		if (data != null && data.battle.battleData.classType != null && data.battle.battleData.rsmg != null) {
			string compressed = data.battle.battleData.rsmg;
			if (!string.IsNullOrEmpty (compressed)) {
				string[] jsons = DeflateEx.Decompress (compressed).Split ('@');
				if (jsons != null) {
					int length = jsons.Length;
					
					if (length == data.battle.battleData.classType.Count) {
						data.battle.battleData.info = new List<CMsgHeader> ();
						
						for (int i = 0; i < length; ++i) {
							Type type = CMsgHeader.Table [data.battle.battleData.classType [i]];
							data.battle.battleData.info.Add (JSON.Instance.ToObject (jsons [i], type) as CMsgHeader);
							//	data.info.Add( JsonFx.Json.JsonReader.Deserialize(jsons[i], type) as War.CMsgHeader);
						}
						
					}
					
				}
			}
		}
	}
}

public class GPSWarRespData
{
	public BattleSequence battle;		    //战斗数据
	public int battleId;					//关卡ID
	public int retCode;
}

//战斗结束
public class SockFightCompleteResponse:BaseResponse
{
	public GPSWarEndData data;
	public SockFightCompleteResponse()
	{
	}
}

public class GPSWarEndData
{
	public int memberId;			//退出战斗的memberId
	public int zoneId;
}

//退出活动
public class SockLogOutGPSWarResponse:BaseResponse
{
    public SockLogOutGPSWarResponse(){

    }
}

//同步经纬度
public class SockSyncLocationResponse:BaseResponse
{
    public SockSyncLocationResponse(){

    }
}

public class SockSyncRoomResponse:BaseResponse
{
	public GPSRoomData data;
	public SockSyncRoomResponse(){
		
	}
}


#endregion



