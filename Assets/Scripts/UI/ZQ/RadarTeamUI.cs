using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RadarTeamUI : MonoBehaviour 
{

	private static RadarTeamUI _instance;
	public static RadarTeamUI mInstace
	{
		get
		{
			return _instance;
		}
	}

	//标题
	public UISprite m_spTitle;
	//挑战次数
	public UILabel m_txtChgCount;
	//操作UI
	public RadarTeamOpeUI m_operateUI;
	//房间列表
	public RadarRoomListUI m_roomListUI;
	//关卡详情
	public RadarDespUI m_despUI;
	//GPS定位
	private GPSLocation m_gpsLocation;

	private bool m_bNeedExit;
	public GameObject despObj;
	public UILabel lblDesp;

	// 1 : 创建房间， 2 更新列表
	private int type = 0;

	public static void OpenUI()
	{
		if (_instance == null)
		{
			Object prefab = PrefabLoader.loadFromPack ("ZQ/RadarTeamUI");
			if (prefab != null)
			{
				GameObject obj = Instantiate (prefab) as GameObject;
				RED.AddChild (obj, DBUIController.mDBUIInstance._bottomRoot);
			}
		}
	}

	void Awake()
	{
		_instance = this;
	}

	void Start()
	{
		//界面初始化
		InitUI ();

		//注册消息
		ActivityNetController.GetInstance ().RegisterRararMsgs ();

		//发送加入活动请求
		SendJoinActRQ ();
	}

	void InitUI()
	{
		m_operateUI.SetShow (true);
		m_roomListUI.SetShow (false);
		m_despUI.SetShow (false);

		m_gpsLocation = GetComponent<GPSLocation> ();
		lblDesp.text = Core.Data.stringManager.getString (90026);
	}

	public void SetShow(bool bShow)
	{
		RED.SetActive (bShow, this.gameObject);
	}

	//发送加入活动请求
	void SendJoinActRQ()
	{
		ActivityNetController.GetInstance ().LoginGPSWar ();
	}

	public void JoinActSuc(SockLoginGPSWarResponse resp)
	{
		string strText = Core.Data.stringManager.getString(5187);
		strText = string.Format(strText, resp.data.nowTimes, resp.data.maxTimes);
		m_txtChgCount.text = strText;
		//发送同步房间信息请求
		SyncRoomRQ();
	}

	//发送退出活动
	public void SendExitActRQ()
	{
		ActivityNetController.GetInstance ().LogOutAct ();
		Core.Data.gpsWarManager.ExitActSuc ();
		ExitActSuc ();
	}

	public void ExitActSuc()
	{
		m_gpsLocation.StopGPS ();
		Destroy (this.gameObject);
		_instance = null;
		DBUIController.mDBUIInstance.ShowFor2D_UI ();
	}

	//发送GPS经纬度请求
	public void SendSyncGPSRQ(float jingdu, float weidu)
	{
		ActivityNetController.GetInstance ().SynchronizationLocation (jingdu, weidu);
	}

	public void DealSyncGPSSucRS()
	{
		if (type == 1)
		{
			SendCreateRoomRQ ();
		}

		else if(type == 2)
		{
			SendUpdateRoomListRQ ();
		}
		type = 0;
	}


	public void SendGetStutasRQ()
	{
		ComLoading.Open ();
		//发送取得专拍状态信息
        RouletteLogic.sendSer(GetRouletteStateSuc, GetRouletteSatteFail,0);
	}

	//取得转盘状态Suc
	void GetRouletteStateSuc(BaseHttpRequest req, BaseResponse resp)
	{
		ComLoading.Close ();
		GetAwardActivity re = resp as GetAwardActivity;
		ActivityManager.activityZPID = re.data.status.id; 

//		if (re.data.status.id == 0)
//		{
//			UIInformation.GetInstance ().SetInformation (Core.Data.stringManager.getString (5206), Core.Data.stringManager.getString (5030), OpenZhuanPanUI);
//		}
//		else
//		{
//			if(ActivityManager.activityZPID != 4)
//			{
//				SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(5207));
//				return;
//			}
//			else
//			{
				if (!Core.Data.gpsWarManager.SyncGPSSuc) {
					//启动GPS服务
					m_gpsLocation.StartCoroutine (m_gpsLocation.StartGPS ());
					type = 1;
				} 
				else 
				{
					SendCreateRoomRQ ();
				}
				
		//}
		//}
	}

	void OpenZhuanPanUI()
	{
		RouletteLogic.CreateRouleView();
		SendExitActRQ ();
	}

	//取得转盘状态fail
	void GetRouletteSatteFail(BaseHttpRequest req, string strCode)
	{
		ComLoading.Close ();
		SQYAlertViewMove.CreateAlertViewMove (strCode);
	}

	public void DealSyncRoomSucRS()
	{
		m_operateUI.UpdateUI();
		if (Core.Data.gpsWarManager.curRoom != null 
			&& Core.Data.gpsWarManager.m_nCurCnt >= Core.Data.gpsWarManager.m_nMaxCnt
			&& Core.Data.gpsWarManager.AmILeader()) 
		{
			SendExitRoomRQ (false);
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(5198));
			return;
		}
	}

	public void SyncRoomRQ()
	{
		ActivityNetController.GetInstance().SendSyncRoomRQ();
	}
		
	//发送创建房间请求
	public void SendCreateRoomRQ()
	{
		if(!Core.Data.gpsWarManager.LoginSuc)
		{
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (5200));
			return;
		}

		if (!Core.Data.gpsWarManager.SyncGPSSuc)
		{
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (5185));
			return;
		}

		if (Core.Data.gpsWarManager.m_nCurCnt >= Core.Data.gpsWarManager.m_nMaxCnt) 
		{
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (5198));
			return;
		}

		ActivityNetController.GetInstance ().CreatRoom ();
	}

	public void CreateRoomSuc(SockCreatRoomResponse resp)
	{
		m_operateUI.UpdateUI ();
	}

	//发送更新房间列表请求
	public void SendUpdateRoomListRQ()
	{
		if(!Core.Data.gpsWarManager.LoginSuc)
		{
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (5200));
			return;
		}

		if (!Core.Data.gpsWarManager.SyncGPSSuc) {
			//启动GPS服务
			m_gpsLocation.StartCoroutine (m_gpsLocation.StartGPS ());
			type = 2;
			return;
		} 

		ActivityNetController.GetInstance ().GetRoomList ();
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
					m_roomListUI.DelRoom (rooms [i]);
				}
				break;
			case 2:		//更新
				for (int i = 0; i < resp.data.rooms.Length; i++)
				{
					m_roomListUI.UpdateRoom (rooms [i]);
				}
				break;
			case 1:		//添加
				for (int i = 0; i < resp.data.rooms.Length; i++)
				{
					m_roomListUI.AddRoom (rooms [i]);
				}
				break;
		}
	}

	public void DealExitWarRS()
	{
		m_operateUI.UpdateUI();
	}

	//发送假如房间请求
	public void SendJoinRoomRQ(int roomId)
	{
		if (!Core.Data.gpsWarManager.SyncGPSSuc)
		{
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (5185));
			return;
		}
		ActivityNetController.GetInstance().LoginRoom(roomId);
	}
		
	//处理加入房间消息
	public void DealJoinRoomRS(SockLoginRoomResponse resp)
	{
		m_roomListUI.SetShow(false);
		m_operateUI.SetShow(true);
		m_operateUI.UpdateUI ();
	}

	//发送退出房间请求
	public void SendExitRoomRQ(bool needExit)
	{
		m_bNeedExit = needExit;
		ActivityNetController.GetInstance ().LogOutRoom ();
	}

	public void ExitRoomSuc(SockLogOutRoomResponse resp)
	{
		//如果是自己退出
		if (Core.Data.playerManager.PlayerID.Equals(resp.data.memberId.ToString()) && Core.SM.curServer.sid == resp.data.zoneId)
		{
			if (Core.Data.gpsWarManager.AmILeader ())
			{
				m_operateUI.SetShow (true);
				m_roomListUI.SetShow (false);
			}
			else
			{
				m_operateUI.SetShow (false);
				m_roomListUI.SetShow (true);
			}
			Core.Data.gpsWarManager.curRoom = null;
		}
		else
		{
			if (Core.Data.gpsWarManager.curRoom != null 
				&& Core.Data.gpsWarManager.curRoom.members != null
				&& Core.Data.gpsWarManager.curRoom.members.Length > 0)
			{
				for (int i = 0; i < Core.Data.gpsWarManager.curRoom.members.Length; i++)
				{
					if (Core.Data.gpsWarManager.curRoom.members [i].memberId == resp.data.memberId
					   && Core.Data.gpsWarManager.curRoom.members [i].zoneId == resp.data.zoneId)
					{
						if (Core.Data.gpsWarManager.curRoom.members [i].memberType == 1)
						{
							m_operateUI.SetShow (false);
							m_roomListUI.SetShow (true);
							
							SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (5197));

							Core.Data.gpsWarManager.curRoom = null;
						}
						else
						{
							Core.Data.gpsWarManager.curRoom.members [i] = null;
						}

						break;
					}
				}
			}
		}

		m_despUI.UpdateChallengeBtn();

		m_operateUI.UpdateUI ();

		if(m_bNeedExit)
		{
			RadarTeamUI.mInstace.SendExitActRQ ();
			m_bNeedExit = false;
		}
	}

	//开始战斗
	public void SendStartWarRQ(int lv)
	{
		ActivityNetController.GetInstance ().StartGPSFight (lv);
	}

	//
	public void StartWarSuc(SockFightResponse resp)
	{
		Core.Data.temper.warBattle = resp.data.battle;
		Core.Data.temper.currentBattleType = TemporyData.BattleType.GPSWar;
		GPSWarInfo info = Core.Data.gpsWarManager.GetGPSWarInfo (resp.data.battleId);
		if (info != null)
		{
			Core.Data.temper.CitySence = info.Cj;
		}
		else
		{
			RED.LogWarning (resp.data.battleId + " not find pgs war info");
		}

		JumpToBattleView ();
	}

	void JumpToBattleView() 
	{
		BattleToUIInfo.From = RUIType.EMViewState.S_CityFloor;
		//		Core.Data.temper.CitySence = currFloor.config.cj;
		Core.SM.beforeLoadLevel(Application.loadedLevelName, SceneName.GAME_BATTLE);
		AsyncLoadScene.m_Instance.LoadScene(SceneName.GAME_BATTLE);
	}

	void OnOpenDespBtn(){

		TweenScale.Begin (despObj,0.2f,Vector3.one);
		lblDesp.text = Core.Data.stringManager.getString (90026);
	}
	void OnCloseDespBtn(){
		TweenScale.Begin (despObj,0.2f,Vector3.zero);
	}

 
	public void SetGPSLocation(string strPos)
	{
//		RED.Log ("雷达组队，设置gps：： " + strPos);
		m_gpsLocation.SetGPSLocation (strPos);
	}

}
