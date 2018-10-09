using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SuperSocket.ClientEngine;

/// <summary>
/// Activity net controller  with Socket.
/// </summary>

public class ActivityNetController
{
	static ActivityNetController instance;

	public static ActivityNetController GetInstance ()
	{
		if (instance == null) {
			instance = new ActivityNetController ();
		}
		return instance;
	}

	public const int NextAtkTime = 51;
	public const int needDiaNum = 350;
	public const int needDiaAtk = 20;
	public const int stoneAddBuff = 50;
	public const int coinAddBuff = 100000;
	//月卡充值
	public const int monthRewardStone = 120;
	public const int stoneId = 110052;
	public const int buqian = 50;
	public long TimeLeft;
	public long dinnerLeft;
	public static List<int> stateList;
	//	public static int tempHonorGiftId;
	public GameObject CurCtrl;
	public SockBossAtkListData TempAtkData;
	public static int actTipNum;
	public static int tempColNum;
	//public static List<int> levelRewardList = new List<int> ();
	public static int lvRState = 1;
	public static int timeArange = 0;
	public static bool isActSKTConnect = false;
	public List<long> dinnerTime = new List<long> (4);
	/// <summary>
	/// 0:没等待 只是登录 1：等待武者节日  2：等待魔王来袭
	/// </summary>
	public static int curWaitState = 0;
	public System.Action finishMethod = null;
	public static System.Action ReconnectContinue = null;
	public System.Action callBackToRadarGroup = null;
	public List<DailyGiftItemClass> dailyGiftList = new List<DailyGiftItemClass> ();
	public  List<DailyGiftItemClass> sevenDailyGiftList = null;
	private Dictionary<int,bool> reqback = null;
	public static int curSevenGetIndex = 0;
	public static int UnGotGiftNum = 5;
	private  int CurRewardAct = 3;

    public int ACTION_CURRENT_USERSATAE = 20000; 

	public static bool isInActivity = false;

	public static void ShowDebug (string tip)
	{
		SQYAlertViewMove.CreateAlertViewMove (tip);
	}

	#region 登录    1001

	public  void SendLoginMSGPrepare (string playerId, string tiId)
	{
		Core.NetEng.SockEngine.OnLogin (new System.Net.DnsEndPoint (Core.SM.curServer.active_ip, Core.SM.curServer.active_port));

		SocketTask task = new SocketTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.SOCK_LOGIN, new SockLoginParam (Core.Data.playerManager.PlayerID, tiId, Core.SM.curServer.sid));
		task.ErrorOccured = SocketResp_Error;
		task.afterCompleted = SocketResp_UI;
		
		task.DispatchToRealHandler ();
		ReceiveActTime ();
	}

	public void SendLoginMSG (string playerId, string tiId)
	{
		MessageMgr.GetInstance ().CloseWorldChatSocket ();
		AsyncTask.QueueOnMainThread (() => {
			SendLoginMSGPrepare (playerId, tiId);
		}, 0.5f);
	}

	public void OutRadarLoginMSG (string tId, System.Action loginAndRadar = null)
	{
		MessageMgr.GetInstance ().CloseWorldChatSocket ();
		callBackToRadarGroup = loginAndRadar;
		AsyncTask.QueueOnMainThread (() => {
			SendLoginMSGPrepare (Core.Data.playerManager.PlayerID, tId);
		}, 0.5f);

	}

	void ShowLogIn (SockLoginData data)
	{
		if (data.retCode == 0) {
			RED.Log ("can not log in ");
		} else {
			isActSKTConnect = true;

			if (curWaitState == 1)
				UIWXLActivityMainController.Instance.OnBtnFestival ();
			else if (curWaitState == 2)
				UIWXLActivityMainController.Instance.OnBtnMonsterCome ();
			else if (curWaitState == 3)
				UIWXLActivityMainController.Instance.OnBtnGroupWar ();
		}
	}

	#region 攻击boss   1002

	public  void sendAttackBoss (int type)
	{
		if (type == 1) {
			if (Core.Data.playerManager.Stone < needDiaAtk) {
				ShowDebug (Core.Data.stringManager.getString (7310));
				ComLoading.Close ();
				return;
			} 
		}
		SocketTask task = new SocketTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.SOCK_ATTACKBOSS, new SockAttackBossParam (Core.Data.playerManager.PlayerID, type));
		task.ErrorOccured = SocketResp_Error;
		task.afterCompleted = BattleResponseInNet;
		task.DispatchToRealHandler ();
		ComLoading.Open ();
	}

	public void BattleResponseInNet (BaseSocketRequest request, BaseResponse response)
	{


		if (response != null) {
			if (response.status != BaseResponse.ERROR) {
				SocketRequest req = request as SocketRequest;
				SockAttackBossParam reqParam = req.ParamMem as SockAttackBossParam;

				//付费
				if (reqParam.type == 1) {
					Core.Data.playerManager.ReduceCoin (-50, 2);
					//talkingData add by wxl 
					Core.Data.ActivityManager.OnPurchaseVirtualCurrency (ActivityManager.MonsterComeBackType, 1, 50);
				}
				SockAttackBossResponse sockAttack = response as SockAttackBossResponse;
				if (sockAttack.data.battleData.isPay == 1)
					sockAttack.data.battleData.rsty = null;
				sockAttack.data.battleData.rsmg = null;
				Core.Data.temper.warBattle = sockAttack.data;
				Core.Data.temper.currentBattleType = TemporyData.BattleType.WorldBossWar;
				UIActMonsterComeController.TimeLeft = NextAtkTime;
				Core.Data.temper.WorldPoint = 200;
				JumpToBattleView ();
				
			} else {
				ShowHttpError (response.errorCode);
			}
		
		}

		UIActMonsterComeController.Instance.isNormalAtk = true;
		ComLoading.Close ();
	}

	void JumpToBattleView ()
	{
		BattleToUIInfo.From = RUIType.EMViewState.S_MosterCome;
		Core.SM.beforeLoadLevel (Application.loadedLevelName, SceneName.GAME_BATTLE);
		AsyncLoadScene.m_Instance.LoadScene (SceneName.GAME_BATTLE);
	}

	#endregion

	#region  登出boss 房间     1003

	public void LogOutBoss ()
	{
		SocketTask task = new SocketTask (ThreadType.MainThread, TaskResponse.Igonre_Response);
		task.AppendCommonParam (RequestType.SOCK_LOGOUTBOSS, new SockLogOutBossParam (Core.Data.playerManager.PlayerID));
		task.ErrorOccured = SocketResp_Error;
		task.afterCompleted = SocketResp_UI;
		task.DispatchToRealHandler ();
	}

	#endregion

	#region 更新排行榜血量排行榜  1004

	public void GetPoint (SockBossAtkListData data)
	{
		TempAtkData = data;

		if (UIActMonsterComeController.Instance != null)
			UIActMonsterComeController.Instance.UpdateList (data);

	}

	/// <summary>
	/// 攻击世界boss排行榜和血量   1004
	/// </summary>
	public void ReceiveBloodRank ()
	{
		SocketTask task = new SocketTask (ThreadType.MainThread, TaskResponse.Donot_Send);

		task.AppendCommonParam (RequestType.SOCK_GETBLOOD_ATKLIST, new BaseRequestParam ());

		task.ErrorOccured = SocketResp_Error;
		task.afterCompleted = SocketResp_UI;

		task.DispatchToRealHandler ();

	}

	#region 进入世界boss   1005

	public void LoginBoss (System.Action callBack)
	{
		ReconnectContinue = callBack;
		ComLoading.Open ();
		SocketTask task = new SocketTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.SOCK_LOGINBOSS, new SockLoginBossParam (Core.Data.playerManager.PlayerID));
		task.ErrorOccured = SocketResp_Error;
		task.afterCompleted = LoginBossPanel;
		task.DispatchToRealHandler ();
		ReceiveActTime ();

	}

	public void LoginBossPanel (BaseSocketRequest request, BaseResponse response)
	{
		SockLoginBossResponse sockLogBoss = response as SockLoginBossResponse;
		SockLoginBossData data  = sockLogBoss.data;
		if (data.retCode != 1) {
       
			ShowDebug (Core.Data.stringManager.getString (data.retCode));
			ComLoading.Close ();
			return;
			//		ShowDebug(" can not log in ");
		} else {
			UIActMonsterComeController.Instance.init (data);
			Core.Data.temper.WorldBoss_lv = data.bossLev;
			Core.Data.temper.WorldBoss_Att = data.bossHp;
			//	Debug.Log (" attack  time  =  " + Core.TimerEng.curTime);
			curWaitState = 0;
		}
	}

	#endregion

	#endregion

	#region 活动状态      1006

	//1006  活动时间   通用   目前只获取  魔王来袭的 时间
	public void ReceiveActTime ()
	{
		SocketTask task = new SocketTask (ThreadType.MainThread, TaskResponse.Donot_Send);

		task.AppendCommonParam (RequestType.SOCK_GET_ACTTIME, new BaseRequestParam ());

		task.ErrorOccured = SocketResp_Error;
		task.afterCompleted = SocketResp_UI;

		task.DispatchToRealHandler ();
		ReceiveBloodRank ();

	}

	public void WriteActivityTime (SockGetActTimeData timeData)
	{
		if (UIWXLActivityMainController.Instance != null)
			UIWXLActivityMainController.Instance.Refresh ();
		if (timeData != null) {
			Core.Data.ActivityManager.SetActState (ActivityManager.monsterType, timeData.bossState.ToString ());
			this.TimerCountingAtkBoss (long.Parse (timeData.nowTime), long.Parse (timeData.time), 1);
            
			Core.Data.ActivityManager.SetActState (ActivityManager.festivalType, timeData.wuzheState.ToString ());
		} 

		if (UIWXLActivityMainController.Instance != null) {
			if (UIWXLActivityMainController.Instance.gameObject.activeInHierarchy == true) {
				UIWXLActivityMainController.Instance.Refresh ();
			}
		}
	}

	public void ShowActState (SockGetActivityStateData stateData)
	{
		ReceiveBloodRank ();
	}

	/// <summary>
	/// 活动状态 1007
	/// </summary>
    /*	public void ReceiveActState ()
	{
		SocketTask task = new SocketTask (ThreadType.MainThread, TaskResponse.Donot_Send);

		task.AppendCommonParam (RequestType.SOCK_ACTIVITYSTATE, new BaseRequestParam ());

		task.ErrorOccured = SocketResp_Error;
		task.afterCompleted = SocketResp_UI;

		task.DispatchToRealHandler ();
	}
*/
	#endregion

	#region  加  buff   1008

	public void Addpower (int type)
	{
		if (type == 0) {
			if (Core.Data.playerManager.Coin < coinAddBuff) {
				ShowDebug (Core.Data.stringManager.getString (6028));
				return;
			}
		} else if (type == 1) {
			if (Core.Data.playerManager.Stone < stoneAddBuff) {
				ShowDebug (Core.Data.stringManager.getString (7310));
				return;
			}
		}

		if (ActivityManager.buyLeftTimes < 1) {
			ShowDebug (Core.Data.stringManager.getString (40002));
			return;
		}
		SocketTask task = new SocketTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.SOCK_ADDPOWER, new SockAddPowerParam (Core.Data.playerManager.PlayerID, type));
		task.ErrorOccured = SocketResp_Error;
		task.afterCompleted = SocketResp_UI;
		task.DispatchToRealHandler ();
	}

	/// <summary>
	/// 1008 反应
	/// </summary>
	/// <param name="data">Data.</param>
	public void AddPower_Back (SockAddPowerData data)
	{
		if (data.retCode == 1) {
			ActivityManager.moneyType = data.type;
			ActivityManager.AddPecent ();
			Core.Data.ActivityManager.OnPurchaseVirtualCurrency (ActivityManager.MonsterComeBackType, 1, stoneAddBuff);

			// UIMiniPlayerController.Instance.freshPlayerInfoView ();
			DBUIController.mDBUIInstance.RefreshUserInfo ();
			ShowDebug (Core.Data.stringManager.getString (7321));
		}		
	}

	#endregion

	#endregion

	#region 兑换商品    1009

	/// <summary>
	///  兑换商品
	/// </summary>
	public void SendChargeItem (int goodid)
	{
		ComLoading.Open ();
		//	tempHonorGiftId = goodid;
		SocketTask task = new SocketTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.SOCK_HONORBUYITEM, new SockbuyItemParam (Core.Data.playerManager.PlayerID, goodid));
		task.ErrorOccured = SocketResp_Error;
		task.afterCompleted = ChargeItemBack;
		task.DispatchToRealHandler ();
	}

	public void ChargeItemBack ( BaseSocketRequest request, BaseResponse response    )
	{
		 SockBuyItemResponse buyItem = response as SockBuyItemResponse;
		SockHonorBuyItemData data = buyItem.data;
		SocketRequest sR = request as SocketRequest;
		if(buyItem.data != null){
			SockbuyItemParam tParam = sR.ParamMem as SockbuyItemParam;
			if (data.retCode == 1) {
				HonorItemData tItem = Core.Data.ActivityManager.getHonorItemById (tParam.goodId);
				ItemOfReward[] iOR = new ItemOfReward[]{ data.p };
				if (tItem != null) {
					Debug.Log (" tItem .cost " + tItem.count );
					UIActMonsterComeController.Instance.ShowHonorItem (iOR, tItem.cost);
				}
			}
		}
		ComLoading.Close ();
	}

	#endregion

	#region 武者的节日    1010

	public void  SendLoginFestival (System.Action callBack)
	{
		ReconnectContinue = callBack;
		ComLoading.Open ();
		SocketTask task = new SocketTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.SOCK_LOGINFESTIVAL, new SockLoginFestivalParam (Core.Data.playerManager.PlayerID));
		task.ErrorOccured = SocketResp_Error;
		task.afterCompleted = ShowLogInFestival;
		task.DispatchToRealHandler ();
		GetScoreRankList ();
	}

	void ShowLogInFestival (BaseSocketRequest request, BaseResponse response)
	{
		SockLoginFestivalResponse enterFResponse = response as SockLoginFestivalResponse;
		if (enterFResponse.data.retCode != 1) {
			ShowDebug (Core.Data.stringManager.getString (enterFResponse.data.retCode));
			WXLActivityFestivalController.Instance.On_Back ();
		} else {
			curWaitState = 0;
//			enterFResponse.data.goodId = 110022;
			WXLActivityFestivalController.Instance.Init (enterFResponse.data);
		}
		ComLoading.Close ();
	}
	//1011
	public void  BuyLotteryInFestival (int type)
	{
      
		SocketTask task = new SocketTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.SOCK_BUYLOTTERY, new SockBuyLotteryParam (Core.Data.playerManager.PlayerID, type));
		task.ErrorOccured = SocketResp_Error;
		task.afterCompleted = SocketResp_UI;
		task.DispatchToRealHandler ();
		ComLoading.Open ();
	}

	public void  BuyLotteryInFestivalBack (SockFestivalBugLotteryData data)
	{
      
		if (data.retCode == 1) {
			ItemOfReward[] tItem = new ItemOfReward[1]{ data.p };
			WXLActivityFestivalController.Instance.ShowReward (tItem);
			//talkingdata add by wxl 
			if (data.stone != 0) {
				Core.Data.ActivityManager.OnPurchaseVirtualCurrency (ActivityManager.FestivalPurchaseType, 1, Mathf.Abs (data.stone));
			}
			// refresh 
			WXLActivityFestivalController.Instance.ShowScore (data);
		} else {
			ShowDebug (Core.Data.stringManager.getString (data.retCode));
		}
		ComLoading.Close ();
	}
	//1012   玩家退出武者的节日
	public void  SendLogOutFestival ()
	{

		SocketTask task = new SocketTask (ThreadType.MainThread, TaskResponse.Igonre_Response);
		task.AppendCommonParam (RequestType.SOCK_LOGOUTFESTIVAL, new SockLogOutFestivalParam (Core.Data.playerManager.PlayerID));
		task.ErrorOccured = SocketResp_Error;
		task.afterCompleted = SocketResp_UI;
		task.DispatchToRealHandler ();
        
	}
	//1013 获取积分排行榜
	public void GetScoreRankList ()
	{

		SocketTask task = new SocketTask (ThreadType.MainThread, TaskResponse.Donot_Send);
		task.AppendCommonParam (RequestType.SOCK_GETSCORERANKLIST, new SockGetScoreRankListParam ());
		task.ErrorOccured = SocketResp_Error;
		task.afterCompleted = SocketResp_UI;
		task.DispatchToRealHandler ();
      
	}

	public void GetScoreRankListBack (SockRankListData data)
	{
		ComLoading.Close ();
		if (data != null) {
			if (data.pointList.Count == 0)
				return;
		}
		if(WXLActivityFestivalController.Instance!= null)
			if(data!= null)
				WXLActivityFestivalController.Instance.RefreshRank (data.pointList);
	}

	#endregion

	#region   淘宝商城   编号 3

	/// <summary>
	///  淘宝商城     541  
	/// </summary>
	public void TaobaoRequest (string num)
	{
		HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.GIVE_TAOBAOCDKEY, new TaobaoParam (Core.Data.playerManager.PlayerID, num));
		task.afterCompleted += BackTaobaoRequest;
		task.ErrorOccured += AllErrorBack;
		task.DispatchToRealHandler ();
	}

	/// <summary>
	/// 返回错误码
	/// </summary>
	/// <param name="request">Request.</param>
	/// <param name="response">Response.</param>
	public void BackTaobaoRequest (BaseHttpRequest request, BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR) {
			TaobaoResponse TResponse = response as TaobaoResponse;

			if (TResponse.status == 1) {
			} 
		} else {
			TaobaoResponse TResponse = response as TaobaoResponse;
			if (TResponse.status == 2) {
				ShowHttpError (TResponse.errorCode);				
			}
		}
	}

	#endregion

	#region 签到    112 编号 4

	/// <summary>
	/// 获取签到状态
	/// </summary>
	public void SignDateStateRequest ()
	{
		HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.GET_SIGNDATE_STATE, new SignDateParam (Core.Data.playerManager.PlayerID));
		task.afterCompleted += BackSignDateState;
		task.ErrorOccured += AllErrorBack;
		task.DispatchToRealHandler ();
	}

	public void BackSignDateState (BaseHttpRequest request, BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR) {
			SignDateStateResponse SDResponse = response as SignDateStateResponse;
			
			UIDateSignController.Instance.Init (SDResponse);
			if (Core.Data.guideManger.isGuiding)
				Core.Data.guideManger.AutoRUN ();
		}
	}

	/// <summary>
	/// 签到    {"act":113,"status":1,"data":{"p":[{"ppid":170,"pid":10015,"num":2,"lv":1,"ep":0,"ak":0,"df":0}],"stone":0}}
	/// </summary>
	public void SignDayRequest (int Today)
	{
		HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.SIGNDAY, new SignParam (Core.Data.playerManager.PlayerID, Today));
		task.afterCompleted += BackSignDay;
		task.ErrorOccured += AllErrorBack;
		task.DispatchToRealHandler ();
	}

	public void BackSignDay (BaseHttpRequest request, BaseResponse response)
	{
		ComLoading.Close ();

		SignDayResponse SResponse = response as SignDayResponse;

       
		if (response != null && response.status != BaseResponse.ERROR) {


			//if(request != null )
			if (SResponse.data.stone != 0) {
				//补签
				UIDateSignController.Instance.BackBuQian (SResponse);

				//talking data add by wxl 
				Core.Data.ActivityManager.OnPurchaseVirtualCurrency (ActivityManager.SignDayType, 1, SResponse.data.stone);
			
				NoticeManager.pairtms++;
				NoticeManager.signtms++;
			} else {
				//正常签
				UIDateSignController.Instance.BackUINormalSignDay (SResponse);
				Core.Data.ActivityManager.SetSignState ("0");
				NoticeManager.signtms++;
			}
            
			SQYMainController.mInstance.UpdateBagTip ();
		} else {
			ShowDebug (Core.Data.stringManager.getNetworkErrorString(response.errorCode));
		}

		if (UIWXLActivityMainController.Instance != null) {
			UIWXLActivityMainController.Instance.Refresh ();
		}
		UIDateSignController.canClick = true;

		DBUIController.mDBUIInstance.RefreshUserInfo ();
	}

	public void AllErrorBack (BaseHttpRequest request, string error)
	{
		ShowDebug (error);
	}

	public void ShowHttpError (int errorCode)
	{
		ShowDebug (Core.Data.stringManager.getNetworkErrorString (errorCode));
	}

	#endregion

	#region    抽宝箱

	public void GetTreasureBoxState ()
	{
		HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.GETTREASURESTATE, new GetTreasureStateParam (int.Parse (Core.Data.playerManager.PlayerID)));
		task.afterCompleted += BackGetTreasureState;
		task.ErrorOccured += AllErrorBack;
		task.DispatchToRealHandler ();
	}

	public void BackGetTreasureState (BaseHttpRequest request, BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR) {
			GetTreasureStateResponse GTResponse = response as GetTreasureStateResponse;
			TreasureBoxController.Instance.ShowBoxDoubleState (GTResponse);
		}
	}

	/// <summary>
	/// Opens the treasure box.
	/// </summary>
	/// <param name="type">Type.</param>
	/// <param name="openType">OpenType 是打开的方式  1.钥匙 2 钻石.</param>
	public void OpenTreasureBox (int type, int openType)
	{
		HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.OPENTREASUREBOX, new OpenTreasureBoxParam (int.Parse (Core.Data.playerManager.PlayerID), type, openType));
		task.afterCompleted += BackGotTreasure;
		task.ErrorOccured += ChestError;
		task.DispatchToRealHandler ();
	}

	public void BackGotTreasure (BaseHttpRequest request, BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR) {
			GetTresureResponse GTResponse = response as GetTresureResponse;
			HttpRequest hrequest = request as HttpRequest;

			OpenTreasureBoxParam reqPara = hrequest.ParamMem  as OpenTreasureBoxParam;
			if (reqPara.type == 2) {
				int needMoney = 0;

				if (reqPara.boxType == 1) {
					needMoney = TreasureBoxController.CuNeedNum;
				} else if (reqPara.boxType == 2) {
					needMoney = TreasureBoxController.AgNeedNum;
				} else if (reqPara.boxType == 3) {
					needMoney = TreasureBoxController.AuNeedNum;
				}
				Core.Data.playerManager.ReduceCoin (needMoney * -1, 2);
				if (needMoney != 0)
					Core.Data.ActivityManager.OnPurchaseVirtualCurrency (ActivityManager.TreasureBoxType, 1, needMoney);

			}
			DBUIController.mDBUIInstance.RefreshUserInfo ();
            
			TreasureBoxController.Instance.ShowReward (GTResponse.data.p);

			if (GTResponse != null) { 
				if (GTResponse.data.p [0].pid != 110060) {
//					Core.Data.itemManager.addItem (GTResponse.data.p);
//
//                    Core.Data.gemsManager.addItem(GTResponse);
//
//                    Core.Data.monManager.addItem(GTResponse);
//
//                    Core.Data.EquipManager.addItem(GTResponse);
//                    Core.Data.soulManager.addItem(GTResponse);
//
					for (int i = 0; i < GTResponse.data.p.Length; i++)
						Core.Data.itemManager.AddRewardToBag (GTResponse.data.p[i]);

					if (TreasureBoxController.doubleNum [reqPara.boxType - 1] > 1)
						TreasureBoxController.doubleNum [reqPara.boxType - 1] = 1;
				} else {
					if (TreasureBoxController.doubleNum [reqPara.boxType - 1] == 0) {
						TreasureBoxController.doubleNum [reqPara.boxType - 1] += 2;
					} else if (TreasureBoxController.doubleNum [reqPara.boxType - 1] > 0) {
						TreasureBoxController.doubleNum [reqPara.boxType - 1]++;
					} else {
						TreasureBoxController.doubleNum [reqPara.boxType - 1] = 2;
					}
				}
			}

		} else {
			TreasureBoxController.Instance.BackBtn.isEnabled = true;
			TreasureBoxController.Instance.againBtn.isEnabled = false;
			ShowHttpError (response.errorCode);
		}
	}

	void ChestError(BaseHttpRequest request, string error){
		TreasureBoxController.Instance.BackBtn.isEnabled = true;
		TreasureBoxController.Instance.againBtn.isEnabled = false;
	}

	#endregion

	#region 请求获得赌博类型    赌博界面用   820

	public void GetGambleStateList (System.Action finalM = null)
	{
		HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.GET_GAMBLESTATE, new GetGambleParam (Core.Data.playerManager.PlayerID));
		task.afterCompleted += BackGetGambleState;
		task.ErrorOccured += GambleError;
		finishMethod = finalM;
		task.DispatchToRealHandler ();

	}

	public void BackGetGambleState (BaseHttpRequest request, BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR) {
			GetGambleStateResponse getGamebleResp = response as GetGambleStateResponse;
			if (getGamebleResp != null) {
				if (finishMethod != null) {
					UIGambleController.CreateGamblePanel (finishMethod, getGamebleResp);
				}
			}
		} else if(response.status == BaseResponse.ERROR){
			ShowDebug (Core.Data.stringManager.getNetworkErrorString( response.errorCode));
		}
		ComLoading.Close();
	}
	public void GambleError(BaseHttpRequest request, string error){
		ComLoading.Close();
	}

	#endregion

	//关闭 tcp
	public  void closeAllenTcp ()
	{
		SocketTask task = new SocketTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCmdParam (InternalRequestType.SHUT_DOWN);
		task.DispatchToRealHandler ();

		AsyncTask.QueueOnMainThread (
			() => {
				Core.NetEng.SockEngine.OnLogin (new System.Net.DnsEndPoint (Core.SM.curServer.chat_ip, Core.SM.curServer.chat_port));
				MessageMgr.GetInstance ().SendWorldChatLogin ();
			}, 
			0.2f);
		isActSKTConnect = false;
	}

	/// <summary>
	/// socket UI 层反应
	/// </summary>
	/// <param name="request">Request.</param>
	/// <param name="response">Response.</param>
	void SocketResp_UI (BaseSocketRequest request, BaseResponse response)
	{
		AsyncTask.QueueOnMainThread (() => {
			if (response.status != BaseResponse.ERROR) {
				SocketRequest sR = request as SocketRequest;

				switch (sR.Type) {
				case RequestType.SOCK_LOGIN:
					SockLoginResponse sockLogD = response as SockLoginResponse;
					this.ShowLogIn (sockLogD.data);
					isInActivity = true;
					if (callBackToRadarGroup != null) {
						callBackToRadarGroup ();
						callBackToRadarGroup = null;
					}
					break;
				case RequestType.SOCK_LOGINBOSS:
					
//					SockLoginBossResponse sockLogBoss = response as SockLoginBossResponse;
					//this.LoginBossPanel (sockLogBoss.data);
					break;
				case RequestType.SOCK_ATTACKBOSS:
					this.BattleResponseInNet (sR, response);
					break;

				case RequestType.SOCK_GETBLOOD_ATKLIST:
					SockUpdateAtkBloodListResponse sockGetList = response as SockUpdateAtkBloodListResponse;
					this.GetPoint (sockGetList.data);
					break;
				case RequestType.SOCK_LOGOUTBOSS:
                    // SockLogOutAttackBossResponse sockLogOutBoss = response as SockLogOutAttackBossResponse;
					break;
				case  RequestType.SOCK_ACTIVITYSTATE:
					SockActivityStateResponse sockState = response as SockActivityStateResponse;
					ShowActState (sockState.data);
					break;
				case  RequestType.SOCK_GET_ACTTIME:
					SockGetActTimeResponse actTime = response as SockGetActTimeResponse;
					WriteActivityTime (actTime.data);
					break;

				case RequestType.SOCK_ADDPOWER:
					SockAddPowerResponse addPower = response as SockAddPowerResponse;
					AddPower_Back (addPower.data);
					break;
				case RequestType.SOCK_HONORBUYITEM:
//					SockBuyItemResponse buyItem = response as SockBuyItemResponse;
//					this.ChargeItemBack (buyItem.data);
					break;
				case RequestType.SOCK_LOGINFESTIVAL:

					//	this.ShowLogInFestival (response);
					break;	
				case RequestType.SOCK_BUYLOTTERY:
					SockFestivalBuyLotteryResponse buyLotteryResponse = response as SockFestivalBuyLotteryResponse;
					BuyLotteryInFestivalBack (buyLotteryResponse.data);

					break;	
				case RequestType.SOCK_LOGOUTFESTIVAL:
                    //  SockLogOutFestivalResponse QuitFResponse = response as SockLogOutFestivalResponse;

					break;	
				case RequestType.SOCK_GETSCORERANKLIST:
					SockGetRankListResponse SockRankListResponse = response as SockGetRankListResponse;
					GetScoreRankListBack (SockRankListResponse.data);
					break;
				}
			} else {
				ShowDebug (Core.Data.stringManager.getString (response.errorCode + 30000));
				ComLoading.Close ();
			}
		});

	}

	void SocketResp_Error (BaseSocketRequest request, string error)
	{
		ConsoleEx.DebugLog ("---- Socket Resp - Error has ocurred." + error);
	}

	public long MonsterRespectTime ()
	{
		return TimeLeft;
	}

	#region 计时器

	/// <summary>
	/// Timers the counting.
	/// </summary>
	/// <param name="sTime">开始时间.</param>
	/// <param name="endTime">截止时间.</param>
	/// <param name="interval">时间间隔  = 1 秒.</param>
	void TimerCountingAtkBoss (long sTime, long DeadTime, int interval)
	{
		//Core.TimerEng.OnLogin (sTime);

		TimerTask task = new TimerTask (Core.TimerEng.curTime, Core.TimerEng.curTime + DeadTime, interval);
		//All this methods is running in the background
		task.onEventBegin += eventBegin;
		task.onEventEnd += eventEnd;
		task.onEvent += (TimerTask t) => {
			TimeLeft = t.leftTime;
		};
		task.DispatchToRealHandler ();
	
	}

	void eventBegin (TimerTask task)
	{
		RED.Log ("Timer Engine : = on event begin.");
	}

	void eventEnd (TimerTask task)
	{
		task = null;

		//TimerTask AttackBoss = null;
	}

	#endregion

//	public void RefreshState ()
//	{
//		//获取 活跃 数量
//		List<string> actStateList = Core.Data.ActivityManager.actInfoList;
//		int tNum = 0;
//		for (int i = 0; i < actStateList.Count; i++) {
//			if (actStateList [i] == "2") {
//				tNum++;
//			}
//		}
//		activeNum = tNum;
//	}

	public void SetDinnerTimerState ()
	{
		//this.RefreshState ();
		List<DinnerInfo> dinnerInfo = Core.Data.ActivityManager.listDinnerInfoList;
		try {
			for (int i = 0; i < dinnerInfo.Count; i++) {
				dinnerTime.Add (dinnerInfo [i].start);
				dinnerTime.Add (dinnerInfo [i].end);
			}

			for (int i = 0; i < dinnerTime.Count; i++) {
				if (Core.TimerEng.curTime < dinnerTime [i]) {
					SetDinnerTimer (dinnerTime [i], 1);
					break;
				}
			}      
		} catch (Exception ex) {
			ConsoleEx.Write (" error :" + ex.ToString ());
		}
	}

	void SetDinnerTimer (long endTime, int interval)
	{

		TimerTask task = new TimerTask (Core.TimerEng.curTime, endTime, interval);
		task.onEventBegin += eventDinnerBegin;
		task.onEventEnd += eventDinnerEnd;

		task.onEvent += (TimerTask t) => {
			dinnerLeft = t.leftTime;
		};
		task.DispatchToRealHandler ();
	}

	void eventDinnerBegin (TimerTask task)
	{
	}

	void eventDinnerEnd (TimerTask task)
	{
		for (int i = 0; i < dinnerTime.Count; i++) {
			if (Core.TimerEng.curTime < dinnerTime [i]) {
				SetDinnerTimer (dinnerTime [i], 1);
				this.CommonShiftUI ();
				return;
			}
		}
	}
	//转换 状态   目前只用于外部  不干涉 内部
	public void CommonShiftUI ()
	{
		List<DinnerInfo> dinnerInfo = Core.Data.ActivityManager.listDinnerInfoList;
		int timeNum = -1;
		if (dinnerInfo != null) {
			if (Core.TimerEng.curTime > dinnerTime [dinnerTime.Count - 1]) {
				ActivityManager.isOpen = 2;
			} else {
				for (int i = 0; i < dinnerTime.Count; i++) {
					if (Core.TimerEng.curTime < dinnerTime [i]) {
						timeNum = i;
						break;
					}
				}
                
				switch (timeNum) {
				case 0:
					ActivityManager.isOpen = 2;
					Core.Data.ActivityManager.SetDailyGiftState (ActivityManager.dinnerType, "2");
					break;
				case 1:
					ActivityManager.isOpen = 1;
					Core.Data.ActivityManager.SetDailyGiftState (ActivityManager.dinnerType, "1");
					break;
				case 2:
					ActivityManager.isOpen = 2;
					Core.Data.ActivityManager.SetDailyGiftState (ActivityManager.dinnerType, "2");
					break;
				case 3:
					ActivityManager.isOpen = 1;
					Core.Data.ActivityManager.SetDailyGiftState (ActivityManager.dinnerType, "1");
					break;
				default:
					break;
				}
			}
         
		}
	}

	#region 雷达组队功能

	public void RegisterRararMsgs ()
	{
		//注册更新队伍列表消息
		SocketTask task = new SocketTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.SOCK_GETROOMLIST, new SockGetRoomListParam ());
		task.afterCompleted = BackGetRoomList;
		task.ErrorOccured = SockErrorBack;
		Core.NetEng.SockEngine.RegisterSocketTask (task);

		//注册进入队伍消息
		task = new SocketTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.SOCK_LOGININROOM, new SockLoginRoomParam (0));
		task.afterCompleted = BackLoginRoom;
		task.ErrorOccured = SockErrorBack;
		Core.NetEng.SockEngine.RegisterSocketTask (task);

		//注册退出队伍消息
		task = new SocketTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.SOCK_LOGGOUTROOM, new SockLogOutRoomParam ());
		task.afterCompleted = BackLogoutRoom;
		task.ErrorOccured = SockErrorBack;
		Core.NetEng.SockEngine.RegisterSocketTask (task);

		//注册开始战斗消息
		task = new SocketTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.SOCK_GPSFIGHT, new  SockFightParam (1));
		task.afterCompleted = BackStartFight;
		task.ErrorOccured = SockErrorBack;
		Core.NetEng.SockEngine.RegisterSocketTask (task);

		//注册战斗结束消息
		task = new SocketTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.SOCK_FIGHTCOMPLETE, new SockCompleteFightParam ());
		task.afterCompleted = BackFightComplete;
		task.ErrorOccured = SockErrorBack;
		Core.NetEng.SockEngine.RegisterSocketTask (task);
	}
	//加入gps活动
	public void LoginGPSWar ()
	{
		ComLoading.Open ();
		SocketTask task = new SocketTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.SOCK_LOGINGPSWAR, new SockLoginGPSWarParam (Core.Data.playerManager.RTData.curTeam.teamAttack, Core.Data.playerManager.RTData.curTeam.teamDefend));
		task.afterCompleted = BackLoginGPSWar;
		task.ErrorOccured = SockErrorBack;
		task.DispatchToRealHandler ();
	}

	void BackLoginGPSWar (BaseSocketRequest request, BaseResponse response)
	{
		SockLoginGPSWarResponse resp = response as SockLoginGPSWarResponse;
		if (resp.data.retCode == 1) {
			Core.Data.gpsWarManager.JoinActSuc (resp);
			RadarTeamUI.mInstace.JoinActSuc (resp);
		} else {
			ShowDebug (Core.Data.stringManager.getString (resp.data.retCode));
		}

		curWaitState = 0;
		ComLoading.Close ();
	}
	//获取gps房间列表
	public void GetRoomList ()
	{
		SocketTask task = new SocketTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.SOCK_GETROOMLIST, new SockGetRoomListParam ());
		task.afterCompleted = BackGetRoomList;
		task.ErrorOccured = SockErrorBack;
		task.DispatchToRealHandler ();
	}

	void BackGetRoomList (BaseSocketRequest request, BaseResponse response)
	{
		SockGetRoomListResponse resp = response as SockGetRoomListResponse;
		if (resp.data != null && resp.data.retCode == 1) {
			Core.Data.gpsWarManager.DealUpdateRoomListRS (resp);
			if (RadarTeamUI.mInstace != null) {
				RadarTeamUI.mInstace.DealUpdateRoomListRS (resp);
			}
		} else {
			ShowDebug (Core.Data.stringManager.getString (resp.data.retCode));
		}
	}
	//加入gps房间
	public void LoginRoom (int roomId)
	{
		SocketTask task = new SocketTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.SOCK_LOGININROOM, new SockLoginRoomParam (roomId));
		task.afterCompleted = BackLoginRoom;
		task.ErrorOccured = SockErrorBack;
		task.DispatchToRealHandler ();
		ComLoading.Open ();
	}

	void BackLoginRoom (BaseSocketRequest request, BaseResponse response)
	{
		SockLoginRoomResponse resp = response as SockLoginRoomResponse;
		if (resp.data != null && resp.data.retCode == 1) {
			Core.Data.gpsWarManager.DealJoinRoomRS (resp);
			RadarTeamUI.mInstace.DealJoinRoomRS (resp);
		} else {
			ShowDebug (Core.Data.stringManager.getString (resp.data.retCode));
		}
		ComLoading.Close ();
	}
	//退出房间
	public void LogOutRoom ()
	{
		SocketTask task = new SocketTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.SOCK_LOGGOUTROOM, new SockLogOutRoomParam ());
		task.afterCompleted = BackLogoutRoom;
		task.ErrorOccured = SockErrorBack;
		task.DispatchToRealHandler ();
	}

	void BackLogoutRoom (BaseSocketRequest request, BaseResponse response)
	{
		SockLogOutRoomResponse resp = response as SockLogOutRoomResponse;
		if (RadarTeamUI.mInstace != null) {
			RadarTeamUI.mInstace.ExitRoomSuc (resp);
		}
	}
	//创建房间
	public void CreatRoom ()
	{
		SocketTask task = new SocketTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.SOCK_CREATROOM, new SockCreatRoomParam ());
		task.afterCompleted = BackCreatRoom;
		task.ErrorOccured = SockErrorBack;
		task.DispatchToRealHandler ();
	}

	void BackCreatRoom (BaseSocketRequest request, BaseResponse response)
	{
		SockCreatRoomResponse resp = response as SockCreatRoomResponse;
		if (resp != null && resp.data.retCode == 1) {
			Core.Data.gpsWarManager.CreateRoomSuc (resp);
			if (RadarTeamUI.mInstace != null) {
				RadarTeamUI.mInstace.CreateRoomSuc (resp);
			}
		}
	}
	//开始战斗
	public void StartGPSFight (int lv)
	{
		SocketTask task = new SocketTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.SOCK_GPSFIGHT, new  SockFightParam (lv));
		task.afterCompleted = BackStartFight;
		task.ErrorOccured = SockErrorBack;
		task.DispatchToRealHandler ();
		ComLoading.Open ();
	}

	void BackStartFight (BaseSocketRequest request, BaseResponse response)
	{
		SockFightResponse resp = response as SockFightResponse;
		if (resp.data != null && resp.data.retCode == 1) {
			Core.Data.gpsWarManager.StartWarSuc (resp);

			if (resp.data.battle.reward != null && resp.data.battle.reward.p != null) {
				for (int i = 0; i < resp.data.battle.reward.p.Length; i++) {
					Core.Data.itemManager.AddRewardToBag (resp.data.battle.reward.p [i]);
					Core.Data.ActivityManager.setOnReward (resp.data.battle.reward.p [i], ActivityManager.GPSTYPE);//yangchenguang统计获取砖石的功能

				}
			}

			if (resp.data.battle.radarReward != null && resp.data.battle.radarReward.p != null && Core.Data.playerManager.PlayerID == resp.data.battle.radarReward.user_id.ToString ()) {
				for (int i = 0; i < resp.data.battle.radarReward.p.Length; i++) {
					Core.Data.itemManager.AddRewardToBag (resp.data.battle.radarReward.p [i]);
					Core.Data.ActivityManager.setOnReward (resp.data.battle.reward.p [i], ActivityManager.GPSTYPE);//yangchenguang统计获取砖石的功能
				}
			}

			if (RadarTeamUI.mInstace != null) {
				RadarTeamUI.mInstace.StartWarSuc (resp);
			}
		} else {
			ShowDebug (Core.Data.stringManager.getString (resp.data.retCode));
		}
		ComLoading.Close ();
	}
	//战斗结束
	public void FightComplete ()
	{
		SocketTask task = new SocketTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.SOCK_FIGHTCOMPLETE, new SockCompleteFightParam ());
		task.afterCompleted = BackFightComplete;
		task.ErrorOccured = SockErrorBack;
		task.DispatchToRealHandler ();
	}

	void BackFightComplete (BaseSocketRequest request, BaseResponse response)
	{
		SockFightCompleteResponse resp = response as SockFightCompleteResponse;
		Core.Data.gpsWarManager.DealExitGPSWarRS (resp);
		if (RadarTeamUI.mInstace != null) {
			RadarTeamUI.mInstace.DealExitWarRS ();
		}
	}
	//退出活动
	public void LogOutAct ()
	{
		SocketTask task = new SocketTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.SOCK_LOGOUTGPSWAR, new SockLogOutGPSWarParam ());
		task.afterCompleted = BackLogOutAct;
		task.ErrorOccured = SockErrorBack;
		task.DispatchToRealHandler ();
	}

	void BackLogOutAct (BaseSocketRequest request, BaseResponse response)
	{
		Core.Data.gpsWarManager.ExitActSuc ();
		if (RadarTeamUI.mInstace != null) {
			RadarTeamUI.mInstace.ExitActSuc ();
		}
	}
	//同步经纬度
	public void SynchronizationLocation (float tLongitude, float tLatitude)
	{
		SocketTask task = new SocketTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.SOCK_SYNCLOCATION, new SockSyncLocationParam (tLongitude, tLatitude));
		task.afterCompleted = BackSyneLocation;
		task.ErrorOccured = SockErrorBack;
		task.DispatchToRealHandler ();
	}

	void BackSyneLocation (BaseSocketRequest request, BaseResponse response)
	{
		Core.Data.gpsWarManager.SyncGPSLocationSuc ();
		if (RadarTeamUI.mInstace != null) {
			RadarTeamUI.mInstace.DealSyncGPSSucRS ();
		}
	}
	//发送同步房间消息
	public void SendSyncRoomRQ ()
	{
		SocketTask task = new SocketTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.SOCK_SYNCROOM, new SockSyncRoomParam ());
		task.afterCompleted = BackSyncRoom;
		task.ErrorOccured = SockErrorBack;
		task.DispatchToRealHandler ();
	}

	void BackSyncRoom (BaseSocketRequest request, BaseResponse response)
	{
		SockSyncRoomResponse resp = response as SockSyncRoomResponse;
		Core.Data.gpsWarManager.SyncRoomSuc (resp);
		if (RadarTeamUI.mInstace != null) {
			RadarTeamUI.mInstace.DealSyncRoomSucRS ();
		}
	}

	public void SockErrorBack (BaseSocketRequest request, string str)
	{
		ComLoading.Close ();
		ShowDebug (str);
	}

	#endregion

	#region   断线重联

	public void BackToActMainPanel ()
	{
		if (ReconnectContinue != null) {
			if (WXLActivityFestivalController.Instance != null || UIActMonsterComeController.Instance != null)
				ReconnectContinue ();
		}
		if (UIWXLActivityMainController.Instance != null) {
			UIWXLActivityMainController.Instance.OnBtnClick ();
		}
		//this.SendLoginMSGPrepare (Core.Data.playerManager.PlayerID, null);
	}

	#endregion

	#region  获取天神赐福 604

	public void GotGodRewardList ()
	{
		HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.GET_GODGIFTLIST, new GotGodGiftListParam (int.Parse (Core.Data.playerManager.PlayerID)));
		task.afterCompleted += BackGotGodReward;
		task.ErrorOccured += AllErrorBack;
		task.DispatchToRealHandler ();
		ComLoading.Open ();

	}

	public void BackGotGodReward (BaseHttpRequest request, BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR) {
			GotGodGiftListResponse resp = response as GotGodGiftListResponse;
			if (resp.data != null) {
				if (RollGambleController.Instance != null) {
					RollGambleController.Instance.InitData (resp.data);
                    Core.Data.ActivityManager.SaveRollGambleList(resp.data.awardList);

				}
			}
		} else if (response.status == BaseResponse.ERROR) {
			ShowHttpError (response.errorCode);
            if (Core.Data.ActivityManager.GetRollGamebleList() != null)
            {
				//            List<int[]> awardList = Core.Data.ActivityManager.GetRollGamebleList();
                RollGambleController.Instance.InitData (null);
            }
//			RollGambleController.Instance.btnRoll.isEnabled = true;
		}
		ComLoading.Close ();
	}

	public void BuyGodReward ()
	{
		HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.GET_GODGIFT, new BuyGodGiftParam (int.Parse (Core.Data.playerManager.PlayerID)));
		task.afterCompleted += BackBuyGodReward;
		task.ErrorOccured += AllErrorBack;
		task.DispatchToRealHandler ();
		ComLoading.Open ();
	}

	public void BackBuyGodReward (BaseHttpRequest request, BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR) {
			GotGodGiftResponse resp = response as  GotGodGiftResponse;
			if (resp.data != null) {
				if (RollGambleController.Instance != null) {
					RollGambleController.Instance.GotGodGiftRefresh (resp);
                    if (resp.data.nextAward != null)
                    {
                        Core.Data.ActivityManager.SaveRollGambleList(resp.data.nextAward.awardList);
                    }
                    else
                    {
                        Core.Data.ActivityManager.SaveRollGambleList(null);
                    }
                    //talkind data add by wxl 
					if (resp != null) {
						if (resp.data.award != null) {
							double result = (double)resp.data.award [0].num + (double)resp.data.stone;
							Core.Data.ActivityManager.OnPurchaseVirtualCurrency (ActivityManager.HappyRollType, 1, result);

						} 
					}
				}
			}
		} else if (response.status == BaseResponse.ERROR) {
			ShowHttpError (response.errorCode);
            RollGambleController.Instance.InitData (null);
		}
		ComLoading.Close ();
	}

	#endregion

	#region 首充 状态

	public void GetFirstChargeStateRequest ()
    {
        GetSevenRewardListParam param = new GetSevenRewardListParam(int.Parse(Core.Data.playerManager.PlayerID));
        HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
        task.AppendCommonParam(RequestType.GET_FIRSTCHARGESTATE, param);
        task.ErrorOccured += AllErrorBack;
        task.afterCompleted += BackGetFirstChargeStateData;
        task.DispatchToRealHandler();
	}

	void BackGetFirstChargeStateData (BaseHttpRequest request, BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR) {
			GetFirstChargeStateResponse resp = response as GetFirstChargeStateResponse;
			if (resp.data != null) {

				//判断是否过3天

				int canGain = resp.data.canGainFirstAward;

				if (canGain == 0) {
					if (Core.Data.playerManager.FirstPurchaseReward	== 0) {
						canGain = 2;
					}
				} 

				Core.Data.rechargeDataMgr._canGainFirstAward = canGain;

				Core.Data.temper.PurStatus = canGain;



				billingCD bill = billingCD.BillCountDown;
				if(bill != null) bill.NetworkCallBack(canGain);
				else {
					ConsoleEx.DebugLog("Bill UI is NULL", ConsoleEx.RED);
				}

				List<ItemOfReward> iorList = new List<ItemOfReward> ();

				for (int i = 0; i < resp.data.awards.Count; i++) {
					ItemOfReward ior = new ItemOfReward ();
					ior.pid = resp.data.awards [i] [0];
					ior.num = resp.data.awards [i] [1];
					ior.lv = 1;
					iorList.Add (ior);
					if(ior.getCurType() == ConfigDataType.Monster)
					{
						if(ior.pid != 19998 && ior.pid != 19999)
						{
							Core.Data.rechargeDataMgr.NPCId = ior.pid;
						}

					}
				}
				Core.Data.ActivityManager.SaveListReward (iorList.ToArray ());
				if (UIFirstRechargePanel.GetInstance () != null) {
					UIFirstRechargePanel.GetInstance ().ShowBtnlabel ();
				}


				if (DBUIController.mDBUIInstance._mainViewCtl != null) {
					DBUIController.mDBUIInstance._mainViewCtl.ArrangeLeftBtnPos ();
					
				}
			}
		} else {
			ShowDebug (Core.Data.stringManager.getNetworkErrorString (response.errorCode));
		}
	}

	public void GetFirstChargeGiftRequest ()
	{
		ComLoading.Open ();
		GetSevenRewardListParam param = new GetSevenRewardListParam (int.Parse (Core.Data.playerManager.PlayerID));
		HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.GET_FIRSTCHARGEGIFT, param);
		task.ErrorOccured += AllErrorBack;
		task.afterCompleted += BackGetFirstChargeGiftData;
		task.DispatchToRealHandler ();
	}

	void BackGetFirstChargeGiftData (BaseHttpRequest request, BaseResponse response)
	{
		if (response != null) {
			if (response.status != BaseResponse.ERROR) {
				GetFirstChargeGiftResponse resp =  response as GetFirstChargeGiftResponse;
				if (resp != null && resp.data != null) {
					GetRewardSucUI.OpenUI (resp.data.award,Core.Data.stringManager.getString(5047));
					DBUIController.mDBUIInstance.RefreshUserInfo ();
					Core.Data.rechargeDataMgr._canGainFirstAward = 2;
					Core.Data.rechargeDataMgr.CloseAll();
					SQYMainController.mInstance.ArrangeLeftBtnPos();
				}
			}else{
				ShowDebug (Core.Data.stringManager.getNetworkErrorString (response.errorCode));
			}
		}
		ComLoading.Close ();
	}

	#endregion

	#region 获取7天奖励

	public void GetSevenDayRewardData ()
	{
		ComLoading.Open ();
		GetSevenRewardListParam param = new GetSevenRewardListParam (int.Parse (Core.Data.playerManager.PlayerID));
		HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.SEVENDAYREWARD, param);
		task.ErrorOccured += AllErrorBack;
		task.afterCompleted += BackGetSevenDayRewardData;
		task.DispatchToRealHandler ();
        
	}

	void BackGetSevenDayRewardData (BaseHttpRequest request, BaseResponse response)
	{
       
		if (response != null) {
			if (response.status != BaseResponse.ERROR) {
				SevenDaysListResponse sevenres = response as SevenDaysListResponse;
				Core.Data.ActivityManager.SaveSevenDayData (sevenres.data);
				GetGiftPanelController.Instance.SetSevenRewardDetail (sevenres.data);
//                sevenDailyGiftList = new List<DailyGiftItemClass> ();
//                if (dailyGiftList != null) {
//                    curSevenGetIndex = sevenres.data.index;
//                    for (int j = 0; j < sevenres.data.awads.Length; j++) {
//                        DailyGiftItemClass giftClass = new DailyGiftItemClass ();
//                        giftClass.id = sevenres.data.awads [j].day;
//                        if (sevenres.data.canGain == true) {
//                            if (giftClass.id == curSevenGetIndex + 1)
//                                giftClass.canGet = true;
//                            else
//                                giftClass.canGet = false;
//                        } else {
//                            giftClass.canGet = sevenres.data.canGain;
//                        }
//
//                        giftClass.curItemType = DailyGiftItemClass.dailyItemType.sevenGiftType;
//                        List<ItemOfReward> tIorList = new List<ItemOfReward> ();
//                       
//                        for (int i = 0; i < sevenres.data.awads [j].reward.Count; i++) {
//                            ItemOfReward itemOR = new ItemOfReward ();
//                            itemOR.pid = sevenres.data.awads [j].reward [i] [0];
//                            itemOR.num = sevenres.data.awads [j].reward [i] [1];
//                            tIorList.Add (itemOR);
//                        }
//
//                        giftClass.giftReward = tIorList;
//
//                        if (sevenres.data.awads [j].day > curSevenGetIndex) {
//                            sevenDailyGiftList.Add (giftClass);
//                        }
//                    }
//                    //      SaveKey ((int)DailyGiftItemClass.dailyItemType.sevenGiftType, true);
//                }
			}
		}
		ComLoading.Close ();
	}

	#endregion

	//已经没用
	/*
      public void GetSevenDayReward ()
    {
        ComLoading.Open ();
        ReceiveSevenRewardParam param = new ReceiveSevenRewardParam (int.Parse (Core.Data.playerManager.PlayerID));
        HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
        task.AppendCommonParam (RequestType.SEVENDAYREWARD_BUY, param);

        task.ErrorOccured += AllErrorBack;
        task.afterCompleted += BackGetSevenDayReward;
        task.DispatchToRealHandler ();
       
    }

    void BackGetSevenDayReward (BaseHttpRequest request, BaseResponse response)
    {
        if (response != null) {
            if (response.status != BaseResponse.ERROR) {
                SevenDaysBuyResponse mbuyres = response as SevenDaysBuyResponse;
    
//                curSevenGetIndex++;
//
//                SevenDaysListData sData = Core.Data.ActivityManager.GetSevenData ();
//                sData.canGain = false;
//                if (sData.index < 7)
//                    sData.index++;
//                Core.Data.ActivityManager.SaveSevenDayData(sData);
//
//                //DailyGiftController.Instance.SimpleRefresh (DailyGiftItemClass.dailyItemType.sevenGiftType);
//              
//                GetRewardSucUI.OpenUI (mbuyres.data.p, Core.Data.stringManager.getString (5047));
//                Core.Data.ActivityManager.SetDailyGiftState (ActivityManager.sevenDayType,"2");

            } else {
                ShowHttpError (response.errorCode);
            }
        }
        ComLoading.Close ();
    }
*/

	#region 等级奖励

	//等级奖励状态请求     type  = 1  外部 请求状态不直接刷新     type =0， 内部 请求状态 需要直接刷新
	public void LevelGiftStateRequest (int type)
	{
		lvRState = type;
		HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.LEVELGIFTSTATE, new LevelGiftStateParam (Core.Data.playerManager.PlayerID));
		task.afterCompleted += BackLevelGiftStateRequest;
		task.ErrorOccured += AllErrorBack;
		task.DispatchToRealHandler ();
		ComLoading.Open ();
	}

	public void BackLevelGiftStateRequest (BaseHttpRequest request, BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR) {
			LevelRewardStateResponse LRSResponse = response as LevelRewardStateResponse;

			if (LRSResponse.data != null) { 
				//levelRewardList.Clear ();
				for (int i = 0; i < LRSResponse.data.Length; i++) {
					//		levelRewardList.Add (LRSResponse.data [i]);
					Core.Data.ActivityManager.SaveGotLvReward (LRSResponse.data[i]);
				}
			} 
//			else {
//				levelRewardList = null;
//			}
			if (lvRState == 0) {//内部
				GetGiftPanelController.Instance.InitLevelRewardData ();
				/*      int tLv = Core.Data.playerManager.Lv;
                int tCount = tLv % 5;
                int curGetLv = 5;
                bool canget = false;
                if (tLv > 5) {
                    tCount = (tLv - tCount) / 5;
                }
                List<ItemOfReward > RewardList = new List<ItemOfReward> ();
                if (levelRewardList != null) {
                    if (levelRewardList.Count <= tCount) {

                        curGetLv = BackIndex (levelRewardList);
                        if (Core.Data.playerManager.Lv >= curGetLv) {
                            canget = true;
                        } else
                            canget = false;

                        if (curGetLv != -1) {
                       
                            LevelUpRewardData lvData = Core.Data.ActivityManager.GetRewardData (curGetLv);
                            for (int i = 0; i < lvData.reward.Count; i++) {
                                ItemOfReward tIor = new ItemOfReward ();
                                tIor.pid = lvData.reward [i] [0];
                                tIor.num = lvData.reward [i] [1];
                                RewardList.Add (tIor);
                            }
                        }
                    } else {
                        canget = false;
                     
                    }
                } else {

                    if (Core.Data.playerManager.Lv >= curGetLv)
                        canget = true;
                    else
                        canget = false;
                    LevelUpRewardData lvData = Core.Data.ActivityManager.GetRewardData (curGetLv);
                    for (int i = 0; i < lvData.reward.Count; i++) {
                        ItemOfReward tIor = new ItemOfReward ();
                        tIor.pid = lvData.reward [i] [0];
                        tIor.num = lvData.reward [i] [1];
                        RewardList.Add (tIor);
                    }
                }
                if (dailyGiftList != null) {
                    DailyGiftItemClass giftClass = new DailyGiftItemClass ();
                    giftClass.id = curGetLv;
                    giftClass.curItemType = DailyGiftItemClass.dailyItemType.levelGiftType;
                    giftClass.canGet = canget;
                    giftClass.giftReward = RewardList;
                    dailyGiftList.Add (giftClass);
                }*/
				//  SaveKey ((int)DailyGiftItemClass.dailyItemType.levelGiftType, true);
			} else {
				this.CheckUnGotGift ();
			}

		}
		ComLoading.Close ();
	}

	public static int BackIndex (List<int> tlist)
	{
		int tCount = (int)((float)Core.Data.playerManager.Lv / 5.0f);
		for (int i = 0; i < tCount; i++) {
			if (!tlist.Contains ((i + 1) * 5)) {
				return (i + 1) * 5;
			}
		}
		if (Core.Data.playerManager.Lv <= 120) {
			return (tCount + 1) * 5;
		}
		return -1;
	}

	/// <summary>
	///判断是否领取状态
	/// </summary>
	public static void CheckUnGotGift ()
	{
		List<int> levelRewardList = Core.Data.ActivityManager.GetGotLvReward ();

		if (levelRewardList == null || levelRewardList.Count == 0) {
			return;
		}

		UnGotGiftNum = BackIndex (levelRewardList);
		if (levelRewardList.Count < Core.Data.playerManager.Lv / 5) {
			Core.Data.ActivityManager.SetDailyGiftState (ActivityManager.lvRewardType, "1");
		} else {
			Core.Data.ActivityManager.SetDailyGiftState (ActivityManager.lvRewardType, "2");
		}

	}
	//获取等级奖励 请求
	public void GotLevelGiftRequest (int num)
	{
		ComLoading.Open ();
		tempColNum = num;
		HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.LEVELGIFT_REQUEST, new GetLevelGiftParam (Core.Data.playerManager.PlayerID, num));
		task.afterCompleted += BackGotLevelGift;
		task.ErrorOccured += AllErrorBack;
		task.DispatchToRealHandler ();
	}

	/// <summary>
	/// 获取等级奖励 back
	/// </summary>
	public void BackGotLevelGift (BaseHttpRequest request, BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR) {
			GetLevelRewardResponse GLRResoponse = response as GetLevelRewardResponse;
			HttpRequest httpRequest = request as HttpRequest;

//			if (levelRewardList == null) {
//
//				levelRewardList = new List<int> ();
//				levelRewardList.Add (tempColNum);
//			} else {   
//				levelRewardList.Add (tempColNum);
//			}
			Core.Data.ActivityManager.SaveGotLvReward (tempColNum);
			CheckUnGotGift ();
			BaseRequestParam param = httpRequest.ParamMem;
			GetLevelGiftParam glvGift = param as GetLevelGiftParam;

			string tTitle = string.Format (Core.Data.stringManager.getString (7318), glvGift.lv.ToString ());
			GetRewardSucUI.OpenUI (GLRResoponse.data, tTitle);

			GetGiftPanelController.Instance.InitLevelRewardData ();

			//    DailyGiftController.Instance.SimpleRefresh (DailyGiftItemClass.dailyItemType.levelGiftType);
		} else {
			ShowHttpError (response.errorCode);
		}
		ComLoading.Close ();
	}

	#endregion

	#region    吃大餐    Http request    800 - 801    编号 4

	public void StartDinnerTimeState ()
	{
		HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.HAVEDINNER_STATE, new HavingDinnerStateParam (Core.Data.playerManager.PlayerID));
		task.afterCompleted += BackStartDinnerTime;
		task.ErrorOccured += AllErrorBack;
		task.DispatchToRealHandler ();
	}

	public void BackStartDinnerTime (BaseHttpRequest request, BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR) {
			HaveDinnerStateResponse dinnerStateResponse = response as HaveDinnerStateResponse;
			if (dinnerStateResponse.data.dinner.isopen == true) {
				//活动开启
				if (dinnerStateResponse.data.stat == false) {        
					//吃完
					Core.Data.ActivityManager.SetDailyGiftState (ActivityManager.dinnerType, "2");
				} else {
					Core.Data.ActivityManager.SetDailyGiftState (ActivityManager.dinnerType, "1");
				}
			} else {
				Core.Data.ActivityManager.SetDailyGiftState (ActivityManager.dinnerType, "2");
			}

			if (dailyGiftList != null) {
				DailyGiftItemClass giftClass = new DailyGiftItemClass ();

				giftClass.curItemType = DailyGiftItemClass.dailyItemType.dinnerType;
				//利用id  记录 是否开启  和  关闭    id = 2 是 没吃  1吃了 
				if (dinnerStateResponse.data.dinner.isopen) {
					if (dinnerStateResponse.data.stat) {
						giftClass.canGet = true;
						giftClass.id = 2;
					} else {
						giftClass.canGet = false;
						giftClass.id = 1;
					}
				} else {
					giftClass.canGet = false;
					if (dinnerStateResponse.data.stat)
						giftClass.id = 2;
					else
						giftClass.id = 1;
				}
				giftClass.giftReward = null;
				dailyGiftList.Add (giftClass);
			}
			SaveKey ((int)DailyGiftItemClass.dailyItemType.dinnerType, true);
		}
	}

	public void GetVipGift ()
	{
		ComLoading.Open ();
		HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.GETVIPGIFT, new GetVipGiftParam (int.Parse (Core.Data.playerManager.PlayerID)));
		task.afterCompleted = BackGetVipGift;
		task.DispatchToRealHandler ();
	}

	void BackGetVipGift (BaseHttpRequest request, BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR) {
			GetVipGiftResponse res = response as GetVipGiftResponse;
			GetRewardSucUI.OpenUI (res.data.award, Core.Data.stringManager.getString (5047));
			//Core.Data.ActivityManager.SetDailyGiftState (ActivityManager.vipLibaoType, "2");
			//Core.Data.ActivityManager.SetActState (ActivityManager.vipLibaoType, "2");
			GetGiftPanelController.Instance.SimpleRefresh (DailyGiftItemClass.dailyItemType.vipGiftType);
		} else if (response.status == BaseResponse.ERROR) {
			ShowDebug (Core.Data.stringManager.getNetworkErrorString (response.errorCode));
		}
		ComLoading.Close ();
	}

	/// <summary>
	///  吃拉面    801  
	/// </summary>
	public void EatDinnerRequest ()
	{
		ComLoading.Open ();
		HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.HAVEDINNER, new HavingDinnerParam (Core.Data.playerManager.PlayerID));
		task.afterCompleted += BackEatDinnerRequest;
		task.ErrorOccured += AllErrorBack;
		task.DispatchToRealHandler ();

	}

	public void BackEatDinnerRequest (BaseHttpRequest request, BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR) {
			HaveDinnerResponse eatDinner = response as HaveDinnerResponse;
			if (eatDinner.status == 1) {
				Core.Data.ActivityManager.SetDailyGiftState (ActivityManager.dinnerType, "2");
				ShowDebug (Core.Data.stringManager.getString (7386));
				GetGiftPanelController.Instance.SimpleRefresh (DailyGiftItemClass.dailyItemType.dinnerType, false);
			} 
		} else {
			ShowHttpError (response.errorCode);
		}
		DBUIController.mDBUIInstance.RefreshUserInfo ();
		ComLoading.Close ();
	}

	#endregion

	#region 检测所有 请求的返回    包括： 大餐，月卡 和 vip礼包

	public void GetAllRewardData ()
	{
		ComLoading.Open ();
		dailyGiftList = new List<DailyGiftItemClass> ();
		reqback = new Dictionary<int, bool> ();
		CurRewardAct = 2;
		StartDinnerTimeState ();
		GetMonthStateRequest ();
		//	SetMonthItem ();
//		if (Core.Data.ActivityManager.GetMonthStateData () == null ) {
//			if (Core.Data.ActivityManager.isMonthRequest == false) {
//				GetMonthStateRequest ();
//			} else {
//				CurRewardAct--;
//				SetMonthItem ();
//			}
//		} else {
//			CurRewardAct--;
//
//		}
//		if (Core.Data.playerManager.RTData.curVipLevel > 0 && Core.Data.ActivityManager.GetDailyGiftState (ActivityManager.vipLibaoType) == "1") {
//			CheckVipGift ();
//		}
		//	} else {
//			CurRewardAct--;
//		}

	}

	void CheckAllBack ()
	{
		int tR = 0;

		foreach (int keyNum  in reqback.Keys) {
			if (reqback.ContainsKey (keyNum)) {
				bool tb = false;
				if (reqback.TryGetValue (keyNum, out tb)) {
					if (tb == true) {
						tR++;
					}
				}
			}
		}

		if (tR >= CurRewardAct) {
			if (GetGiftPanelController.Instance != null) {
				GetGiftPanelController.Instance.InitGiftGroupData ();
				reqback.Clear ();
			}
		}
	}

	void SaveKey (int keyI, bool valueB)
	{
		if (reqback != null) {
			if (reqback.ContainsKey (keyI)) {
				reqback [keyI] = valueB;
			} else {
				reqback.Add (keyI, valueB);
			}
			CheckAllBack ();
		}
	}

	#region   获取月卡状态  94     (充值状态  应该在充值之后 结束界面 重新请求)
	//领取月卡 请求   95
	public void GetMonthGiftRequest ()
	{
		ComLoading.Open ();
		GetPayCntParam param = new GetPayCntParam (Core.Data.playerManager.PlayerID);
		HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.GET_MONTHGIFT, param);
		task.afterCompleted += BackGetMonthGiftRequest;
		task.DispatchToRealHandler ();
	}

	void BackGetMonthGiftRequest (BaseHttpRequest request, BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR) {
			GetMonthGiftResponse tdata = response as GetMonthGiftResponse;
			if (tdata != null) {
				string tTitle = Core.Data.stringManager.getString (7369);
				GetRewardSucUI.OpenUI (tdata.data, tTitle);
			
				MonthGiftData tData =  Core.Data.ActivityManager.GetMonthStateData ();
				if (tdata != null) {
					if (tData.lastDay > 0) {
						tData.lastDay--;
						tData.canGain = 0;
					}
				}
				if (GetGiftPanelController.Instance != null) {
					GetGiftPanelController.Instance.SimpleRefresh (DailyGiftItemClass.dailyItemType.monthGiftType);
					GetGiftPanelController.Instance.ShowTipCtrl ();
				}
				Core.Data.ActivityManager.SetDailyGiftState (ActivityManager.monthGiftType, "2");
				Core.Data.ActivityManager.SaveMonthStateData (tData);
				DBUIController.mDBUIInstance.RefreshUserInfo ();
			}
		} else {
			ShowHttpError (response.errorCode);
		}
		ComLoading.Close ();
	}


	public void GetMonthStateRequest ()
	{
		GetPayCntParam param = new GetPayCntParam (Core.Data.playerManager.PlayerID);
		HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.GET_MONTHSTATE, param);
		task.afterCompleted += BackMonthStateRequest;
		task.DispatchToRealHandler ();
	}

	void BackMonthStateRequest (BaseHttpRequest request, BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR) {
			GetMonthGiftStateResponse tdata = response as GetMonthGiftStateResponse;
			Core.Data.ActivityManager.SaveMonthStateData (tdata.data);
			if (tdata.data != null) {
				Core.Data.ActivityManager.SetDailyGiftState (ActivityManager.monthGiftType, tdata.data.canGain.ToString ());
			} else {
				Core.Data.ActivityManager.SetDailyGiftState (ActivityManager.monthGiftType, "2");
			}
			SetMonthItem (tdata.data);
		} else {
			ShowHttpError (response.errorCode);
		}
	}

	//在月卡 界面中 生成 item
	void SetMonthItem( MonthGiftData mData = null){
		if (GetGiftPanelController.Instance != null) {
			MonthGiftData tdata = mData;
			if (mData == null) {
				if (Core.Data.ActivityManager.GetMonthStateData () != null) {
					tdata = Core.Data.ActivityManager.GetMonthStateData ();
				}
			}
			DailyGiftItemClass giftClass = new DailyGiftItemClass ();
			giftClass.curItemType = DailyGiftItemClass.dailyItemType.monthGiftType;


			if (tdata != null) {//买过
				giftClass.id = 1;
				if (tdata.canGain == 1)
					giftClass.canGet = true;
				else
					giftClass.canGet = false;

				giftClass.otherPara = tdata.lastDay;

				ItemOfReward itemOR = new ItemOfReward ();
				itemOR.pid = stoneId;
				itemOR.num = monthRewardStone;
				giftClass.giftReward = new List<ItemOfReward> ();
				giftClass.giftReward.Add (itemOR);
				dailyGiftList.Add (giftClass);
				//	Core.Data.ActivityManager.SetDailyGiftState (ActivityManager.monthGiftType, tdata.canGain.ToString ());

			} else {//没买过
				giftClass.id = 2;
				ItemOfReward itemOR = new ItemOfReward ();
				itemOR.pid = stoneId;
				itemOR.num = monthRewardStone;
				giftClass.giftReward = new List<ItemOfReward> ();
				giftClass.giftReward.Add (itemOR);
				dailyGiftList.Add (giftClass);
			}
			SaveKey ((int)DailyGiftItemClass.dailyItemType.monthGiftType, true);
			ComLoading.Close ();
		}
	}
	#endregion

	/*	void CheckVipGift ()
	{
		if (Core.Data.playerManager.RTData.curVipLevel > 0 && Core.Data.ActivityManager.GetDailyGiftState (ActivityManager.vipLibaoType) == "1") {
			//初始化 vip礼包
			if (dailyGiftList != null) {
				DailyGiftItemClass giftClass = new DailyGiftItemClass ();
				giftClass.curItemType = DailyGiftItemClass.dailyItemType.vipGiftType;

				if (Core.Data.ActivityManager.GetDailyGiftState (ActivityManager.vipLibaoType) == "1") {
					giftClass.canGet = true;
				} else {
					giftClass.canGet = false;
				}
				ItemOfReward iOR = new ItemOfReward ();
				iOR.pid = 110031;
				iOR.num = 1;
				giftClass.giftReward = new List<ItemOfReward> ();
				giftClass.giftReward.Add (iOR);
				dailyGiftList.Add (giftClass);
			}
			SaveKey ((int)DailyGiftItemClass.dailyItemType.vipGiftType, true);
		} else {
			//  for  test 
			DailyGiftItemClass giftClass = new DailyGiftItemClass ();
			giftClass.curItemType = DailyGiftItemClass.dailyItemType.vipGiftType;
			giftClass.canGet = false;
			ItemOfReward iOR = new ItemOfReward ();
			iOR.pid = 110031;
			iOR.num = 1;
			giftClass.giftReward = new List<ItemOfReward> ();
			giftClass.giftReward.Add (iOR);
			dailyGiftList.Add (giftClass);
		}
		ComLoading.Close ();

	}*/

	#endregion

    #region  发送状态  新手引导步骤 

    public void SendCurrentUserState (int guideId)
	{
        if (Core.Data.guideManger.isGuiding)
        {
            if (Core.Data.playerManager.RTData != null)
            {
                SendCurUserStateParam param = new SendCurUserStateParam(int.Parse(Core.Data.playerManager.PlayerID), guideId, Core.Data.playerManager.Lv);
                HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
                task.AppendCommonParam(RequestType.CURRENT_USERSATAE, param);
                task.DispatchToRealHandler();
            }
        }
	}

	public void SendUserDeviceInfo ()
	{
		ModelTypeData deviceId = new ModelTypeData (HardwareInfo.mInstace.deviceId);
		ModelTypeData deviceNamed = new ModelTypeData (HardwareInfo.mInstace.deviceName);
		ModelTypeData graphicDN = new ModelTypeData (HardwareInfo.mInstace.graphicsDeviceName);
		ModelTypeData osD = new ModelTypeData (HardwareInfo.mInstace.operatingSystem);
		ModelTypeData systemMSize = new ModelTypeData (HardwareInfo.mInstace.systemMemorySize);
		ModelTypeData graphicMSize = new ModelTypeData (HardwareInfo.mInstace.graphicsMemorySize);

		ModelTypeData[] modeType = new ModelTypeData[6] {
			deviceId,
			deviceNamed,
			graphicDN,
			osD,
			systemMSize,
			graphicMSize
		};
		//	Debug.Log ( " d n" + deviceNamed.type + "  gD = " +graphicDN.type + "  osD =" + osD.type  + " count = "+ modeType.Length);
		SendUserDeviceInfoParam param = new SendUserDeviceInfoParam (modeType);
		HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.SEND_USERDEVICE, param);
		task.DispatchToRealHandler ();
	}

	#endregion

	#region 激活码
	public void SendActivationCodeRequest(string m_key)
	{
		ActivationCodeParam param = new ActivationCodeParam(int.Parse(Core.Data.playerManager.PlayerID), m_key);
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.ACTIVATION_CODE, param);
		task.afterCompleted += BackActivationCodeRequest;
		task.DispatchToRealHandler();
	}

	void BackActivationCodeRequest(BaseHttpRequest request, BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR)
		{
			UsePropResponse resp = response as UsePropResponse;
			if(resp.data == null)
			{
				return;
			}
			
			ItemOfReward[] rewards = resp.data.p;
			GetRewardSucUI.OpenUI(rewards, Core.Data.stringManager.getString(5097));
		}
		else if(response != null && response.status == BaseResponse.ERROR)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
		}
	}

	#endregion


	#region
	public void SendGetAllStatus(){
		GetSysStatusParam param = new GetSysStatusParam(int.Parse(Core.Data.playerManager.PlayerID));
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.GetSYSTEMSTATUES, param);
		task.afterCompleted += GetSystemStatuesRequest;
		task.DispatchToRealHandler();
	}

	void GetSystemStatuesRequest(BaseHttpRequest request, BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR)
		{
			GetSystemStatusData resp = response as GetSystemStatusData;
			if (LuaTest.Instance != null) {
				if (resp.getSysStatus == null) {
					return;
				} else {
					SysStatus status = resp.getSysStatus;
					LuaTest.SetLuaStatus (status);
					if (UIWXLActivityMainController.Instance != null) {
						UIWXLActivityMainController.Instance.Refresh ();
					}
				}
			}
		}
		else if(response != null && response.status == BaseResponse.ERROR)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
		}
	}
	#endregion



	#region   龙珠银行
	public void SendDragonBankMsg(){
		GetDragonBankParam param = new GetDragonBankParam(Core.Data.playerManager.PlayerID);
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.DRAGONBANKMESSAGE, param);
		task.afterCompleted += GetDragonBankRequest;
		task.DispatchToRealHandler();
		ComLoading.Open ();
	}

	void GetDragonBankRequest(BaseHttpRequest request, BaseResponse response){
		if (response != null && response.status != BaseResponse.ERROR) {
			GetDragonBankResponse resp = response as GetDragonBankResponse;
			if (resp.data != null) {
				HttpRequest req = request as HttpRequest;
				if (req.Type == RequestType.DRAGONBANKSAVEMSG) {
					GetSaveMoneyParam tParam = req.ParamMem as GetSaveMoneyParam;
					Core.Data.playerManager.RTData.curStone -= tParam.stone;
				}

				if (DragonBankController.Instance != null) {
					DragonBankController.Instance.InitData (resp.data);
				}

				DBUIController.mDBUIInstance.RefreshUserInfo ();
			}
		} else if (response.status == BaseResponse.ERROR) {
			ShowDebug (Core.Data.stringManager.getNetworkErrorString(response.errorCode));
		}
		ComLoading.Close ();
	}

	public void SaveMoneyInBank(int stoneNum){
		GetSaveMoneyParam param = new GetSaveMoneyParam(Core.Data.playerManager.PlayerID,stoneNum);
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.DRAGONBANKSAVEMSG, param);
		task.afterCompleted += GetDragonBankRequest;
		task.DispatchToRealHandler();
		ComLoading.Open ();
	}

	public void ReceiveMoneyInBank(){
		GetDragonBankParam param = new GetDragonBankParam(Core.Data.playerManager.PlayerID);
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.RECEIVEMONEY, param);
		task.afterCompleted += ReceiveMoneyRequest;
		task.DispatchToRealHandler();
		ComLoading.Open ();
	}

	void ReceiveMoneyRequest(BaseHttpRequest request, BaseResponse response){
		if (response != null && response.status != BaseResponse.ERROR) {
			ReceiveBankMoney resp = response as ReceiveBankMoney;
			if (resp.data != null) {
				Core.Data.playerManager.RTData.curStone += resp.data.stone;
				SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(7518));
				SendDragonBankMsg ();
			}
			DBUIController.mDBUIInstance.RefreshUserInfo ();
		}else if(response.status == BaseResponse.ERROR){
			ShowDebug (Core.Data.stringManager.getNetworkErrorString(response.errorCode));

		}
		ComLoading.Close ();
	}



//	void GetSaveMoneyRequest(BaseHttpRequest request, BaseResponse response){
//		if (response != null && response.status != BaseResponse.ERROR) {
//			GetDragonBankResponse resp = response as GetDragonBankResponse;
//			if (resp.data != null) {
//				if (DragonBankController.Instance != null) {
//					DragonBankController.Instance.InitData (resp.data );
//				}
//			}
//		}
//	}


	#endregion

}
