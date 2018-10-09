using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GPSWarManager : Manager 
{
	//Key is Equip number
	private Dictionary<int, GPSWarInfo> m_dicConfig = null;

	/********************数据***********************/
	//房间列表
	private Dictionary<int, SockRoomInfo> m_dicRoomList;
	//当前房间
	public GPSRoomData curRoom;
	//是否已经加入活动
	private bool m_bLoginSuc;
	//是否同步经纬度成功
	private bool m_bSynGPSSuc;

	public int m_nCurCnt;
	public int m_nMaxCnt;

	public GPSWarManager()
	{
		m_dicConfig = new Dictionary<int, GPSWarInfo> ();
		m_dicRoomList = new Dictionary<int, SockRoomInfo> ();
		curRoom = null;
	}
		
	public override bool loadFromConfig () 
	{
		return base.readFromLocalConfigFile<GPSWarInfo>(ConfigType.PGSWAR, m_dicConfig);
	}

	public GPSWarInfo GetGPSWarInfo(int id)
	{
		if (m_dicConfig.ContainsKey (id))
		{
			return m_dicConfig [id];
		}

		RED.LogWarning (id + " not find gps war info");
		return null;
	}

	//房间列表
	public Dictionary<int, SockRoomInfo> roomList
	{
		get
		{
			return m_dicRoomList;
		}
	}
		
	//是否登录成功
	public bool LoginSuc
	{
		get
		{
			return m_bLoginSuc;
		}
	}

	//是否同步pgs信息 
	public bool SyncGPSSuc
	{
		get
		{
			return m_bSynGPSSuc;
		}
	}

	//加入活动成功
	public void JoinActSuc(SockLoginGPSWarResponse resp)
	{
		m_bLoginSuc = true;
		m_nCurCnt = resp.data.nowTimes;
		m_nMaxCnt = resp.data.maxTimes;
	}

	//退出活动成功
	public void ExitActSuc()
	{
		m_bLoginSuc = false;
		curRoom = null;
		m_dicRoomList.Clear ();
		m_bSynGPSSuc = false;
	}
		
	//同步经纬度成功
	public void SyncGPSLocationSuc()
	{
		m_bSynGPSSuc = true;
	}

	public void SyncRoomSuc(SockSyncRoomResponse resp)
	{
		if (resp.data.room == null || resp.data.members == null)
		{
			curRoom = null;
		}
		else
		{
			curRoom = resp.data;
		}

	}

	//创建房间成功
	public void CreateRoomSuc(SockCreatRoomResponse resp)
	{
		m_dicRoomList.Clear ();
		SockRoomInfo room = new SockRoomInfo ();
		room.roomId = resp.data.room.roomId;
		room.joinState = 1;
		room.roomMaxMemberNum = resp.data.room.roomMaxMemberNum;
		room.nowRoomMemberNum = resp.data.room.nowRoomMemberNum; 
		room.masterName = Core.Data.playerManager.RTData.nickName;
		room.masterLevel = Core.Data.playerManager.RTData.curLevel;

		m_dicRoomList.Add (resp.data.room.roomId, room);
		curRoom = resp.data;
	}


	//处理更新房间信息
	public void DealUpdateRoomListRS(SockGetRoomListResponse resp)
	{

		SockRoomInfo[] rooms = resp.data.rooms;
		switch (resp.data.type)
		{
			case 3:		//删除
				for (int i = 0; i < rooms.Length; i++)
				{
					if (m_dicRoomList.ContainsKey (rooms [i].roomId))
					{
						m_dicRoomList.Remove (rooms [i].roomId);
					}
				}
				break;
			case 2:		//更新
				for (int i = 0; i < resp.data.rooms.Length; i++)
				{
					if (m_dicRoomList.ContainsKey (rooms [i].roomId))
					{
						m_dicRoomList [rooms [i].roomId] = rooms [i];
					}
					else
					{
						m_dicRoomList.Add (rooms [i].roomId, rooms [i]);
					}
				}
				break;
			case 1:		//添加
				for (int i = 0; i < resp.data.rooms.Length; i++)
				{
					if (!m_dicRoomList.ContainsKey (rooms [i].roomId))
					{
						m_dicRoomList.Add (rooms [i].roomId, rooms [i]);
					}
					else
					{
						m_dicRoomList [rooms [i].roomId] = rooms [i];
					}
				}
				break;
		}
	}


	//处理加入房间消息
	public void DealJoinRoomRS(SockLoginRoomResponse resp)
	{
		curRoom = resp.data;
	}

	//发送退出房间请求
	public void SendExitRoomRQ()
	{
		ActivityNetController.GetInstance ().LogOutRoom ();
	}

	
	//自己是不是队长
	public bool AmILeader()
	{
		if (curRoom != null && curRoom.members != null && curRoom.members.Length > 0)
		{
			for (int i = 0; i < curRoom.members.Length; i++)
			{
				if (curRoom.members [i].memberId == int.Parse (Core.Data.playerManager.PlayerID)
					&& curRoom.members [i].zoneId == Core.SM.curServer.sid
					&& curRoom.members [i].memberType == 1)
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	//开始战斗成功
	public void StartWarSuc(SockFightResponse resp)
	{
		for (int i = 0; i < curRoom.members.Length; i++)
		{
			if(curRoom.members [i] != null)
			{
				curRoom.members [i].memberState = 2;
			}
		}
		curRoom.room.roomState = 2;
	}

	//退出战斗成功
	public void DealExitGPSWarRS(SockFightCompleteResponse resp)
	{
		if(curRoom != null)
		{
			curRoom.room.roomState = 1;
			for (int i = 0; i < curRoom.members.Length; i++)
			{
				if (curRoom.members [i].memberId == resp.data.memberId
					&& curRoom.members [i].zoneId == resp.data.zoneId)
				{
					curRoom.members [i].memberState = 1;

					break;
				}
			}
		}
	}
}

public class GPSWarInfo : UniqueBaseData
{
	public int Unlock_Lv;		//解锁等级
	public string Describe;		//描述
	public int[] Show_reward;	//客户端展示奖励
	public string Name;			//关卡名字
	public List<int[]> Boss;	//Boss及其等级
	public int Cj;				//战斗场景
}
