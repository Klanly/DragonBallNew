using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RUIType;
using System;

namespace RUIType
{
	public enum EMViewState
	{
		NONE = 0,
		MainView = 1,
		S_ShenLong = 3,
		S_FuBen = 4,
		S_QiangDuo = 5,
		S_Team_NoSelect = 6,
		S_Team_Select = 7,
		H_Bag = 8,
		S_Bag = 9,
		S_CityFloor = 10,
		HIDE_TEAM_VIEW = 11,
		S_ShangCheng = 12,
		S_HuoDong = 13,
		S_XiaoXi = 14,
		S_MosterCome = 15,
		S_QiangDuoDragonBall = 16,
		S_Friend = 17,
		S_WuZheXunLian = 18,
		S_SevenDaysReward = 19,
		S_MailBox = 20,
		S_GPSWar = 21,
	}
}
/// <summary>
/// 整个游戏UI层的控制类
/// </summary>
public partial class DBUIController : RUIMonoBehaviour
{
	public static int VC_MAIN_ID = 10;
	public static int VC_MAP_ID = 12;
	public static int VC_PLAYER_ID = 14;
	public static int VC_TEAM_ID = 16;
	public static int VC_BAG_ID = 18;
	public static int VC_CHAPTER_ID = 20;
	public GameObject _bottomRoot;
	public GameObject _TopRoot;
	public PVERoot _PVERoot;
	[HideInInspector]
	public bool m_bCanClick = false;
	public UIPanel blackMaskPanel;
	private GameObject blackMaskObj;
	[HideInInspector]
	public Transform _tran;
	[HideInInspector]
	public GameObject _gameObject;

	public static DBUIController mDBUIInstance = null;
	[HideInInspector]
	public SQYMainController _mainViewCtl = null;
	[HideInInspector]
	public SQYPlayerController _playerViewCtl = null;

	[HideInInspector]
	public SQYPetBoxController _petBoxCtl = null;
	SQYChapterController _chapterViewCtl = null;

	private readonly string[] STATIC_BTMUI = {"Camera", "UIBackground", "_PVERoot", "pbSQYMainController(Clone)",  "pbSQYPlayerController(Clone)", "BroadCastUI(Clone)"};
	private readonly string[] STATIC_TOPUI = {"Camera", "UIBlackMask"};

	[HideInInspector]
	public MainNoticeUI _noticeUI = null;

	#region Add by jc
	public JCPVEMainController _pveViewCtl = null;
	#endregion

	QiangDuoPanelScript _QiangDuoViewCtl = null;
	DuoBaoPanelScript _DuoBaoViewCtl = null;
	[HideInInspector]
	public bool IsFirstLogin_Vip = true;
	[HideInInspector]
	public UIDragonMallMain _UIDragonMallMain = null;
	[HideInInspector]
		//	public UIWXLActivityMainController _WXLActivityMainCtl = null;
	UIActMonsterComeController _MonsterComeCtrl = null;

	public UICamera BottomCamera = null;
	
	public UICamera TopCamera = null;
	
	//add by jc 
	//所有UI层的背景,共用的一张图片<当3D主场景被隐藏时显示，3D场景显示时隐藏>
	public GameObject UIBackground = null;

    // 战斗再来一次状态
    public static bool battleAgain = false;
    public static int  battleAgainID = 0  ;  
	//是否关闭Top层和Bottom层点击
	public bool UITopAndBottomTouch
	{
		set
		{
			if(value)
			{
				BottomCamera.eventReceiverMask = 1 << 8;
			}
			else
			{
				BottomCamera.eventReceiverMask = 1 << 14;
			}

		}

	}
	
	
	SQYPetBoxController petBoxView
	{
		get
		{
			if (_petBoxCtl == null)
			{
				_petBoxCtl = SQYPetBoxController.CreatePetBoxView ();
				RED.AddChild (_petBoxCtl.gameObject, _bottomRoot);
				_petBoxCtl.transform.localPosition = new Vector3 (-1140, 0, 0);
			}
			return _petBoxCtl;
		}
	}

	public SQYChapterController chapterView
	{
		get
		{
			if (_chapterViewCtl == null)
			{
				_chapterViewCtl = SQYChapterController.CreateChapterView ();
				RED.AddChild (_chapterViewCtl.gameObject, _bottomRoot);
			}
			return _chapterViewCtl;
		}
	}
	
	//新PVE系统
	public JCPVEMainController pveView
	{
		get
		{
			if (_pveViewCtl == null)
			{
				_pveViewCtl = JCPVEMainController.CreatePVEMainView ();
				RED.AddChild (_pveViewCtl.gameObject, _PVERoot.gameObject);
			}
			return _pveViewCtl;
		}
	}
	
	
	
	public QiangDuoPanelScript qiangDuoView
	{
		get
		{
			if (_QiangDuoViewCtl == null)
			{
				_QiangDuoViewCtl = QiangDuoPanelScript.CreateQiangDuoPanel ();
				RED.AddChild (_QiangDuoViewCtl.gameObject, _bottomRoot);
			}
			return _QiangDuoViewCtl;
		}
	}

	public DuoBaoPanelScript mDuoBaoView
	{
		get
		{
			if (_DuoBaoViewCtl == null)
			{
				_DuoBaoViewCtl = DuoBaoPanelScript.CreateQiangDuoPanel ();
				RED.AddChild (_DuoBaoViewCtl.gameObject, _bottomRoot);
				_DuoBaoViewCtl.SetActive (false);
			}
			return _DuoBaoViewCtl;
		}
	}

	public UIDragonMallMain mUIDragonMallMain
	{
		get
		{
			if (_UIDragonMallMain == null)
			{
				_UIDragonMallMain = DragonMallPanelScript.CreateShangChengPanel ();
				RED.AddChild (_UIDragonMallMain.gameObject, _bottomRoot);
			}
			return _UIDragonMallMain;
		}
	}

//	public UIWXLActivityMainController mActPanel
//	{
//		get
//		{ 
//			if (_WXLActivityMainCtl == null)
//			{
//				_WXLActivityMainCtl = UIWXLActivityMainController.CreateActivityMainPanel (_bottomRoot);
//				RED.AddChild (_WXLActivityMainCtl.gameObject, _bottomRoot);
//
//			} 
//			return _WXLActivityMainCtl;
//		}
//	}

	public UIActMonsterComeController MonsterComePanel
	{
		get
		{ 
			if (_MonsterComeCtrl == null)
			{
				_MonsterComeCtrl = UIActMonsterComeController.CreateMonsterPanel (ActivityItemType.mosterComeItem, _bottomRoot);
				RED.AddChild (MonsterComePanel.gameObject, _bottomRoot);
//            
//				UIActMonsterComeController.GetInstance().SendLoginMSG (Core.Data.playerManager.PlayerID,Core.Data.playerManager.PlayerID);
			}
			return _MonsterComeCtrl;
		}
	}

	void Awake ()
	{
		mDBUIInstance = this;
		_tran = transform;
		_gameObject = gameObject;

		SmartRelease.RemoveAsset_WhenGameUI();
       
	}

	void Start ()
	{
//		#region Add by jc 
//		//预先创建副本
//		pveView.SetActive(false);
//		#endregion


		UITopAndBottomTouch = Core.Data.temper.TempTouch;
		
		if (MailReveicer.Instance == null)
			MailReveicer.Create ();

		// 开启 统计数据 core
		Core.Data.ActivityManager.InitAccount ();

		LoadGameUI ();
		CheckJump ();

		RefreshUserInfoWithoutShow ();
	
		if (Core.SM.LastScenesName == SceneName.LOGIN_SCENE) {
			if (Core.Data.playerManager.RTData.IsRegister) {
				ConsoleEx.DebugLog ("isGuiding:" + Core.Data.guideManger.isGuiding.ToString (), ConsoleEx.YELLOW);
				if (Core.Data.guideManger.isGuiding) {
					Core.Data.guideManger.Init ();
					BuildScene.mInstance.sunCamera.Show_Key = false;
					m_bCanClick = true;
				} else {
					StartCoroutine ("CheckSunState");
				}
			}
		} else if (Core.SM.LastScenesName == SceneName.GameMovie)
		{

		} 
		else if (Core.SM.LastScenesName == SceneName.GAME_BATTLE)
		{
#if NewGuide
			if(Core.Data.temper.currentBattleType == TemporyData.BattleType.FightWithFulisa)
			{
				HiddenFor3D_UI(false);
				JCSubtitlesPanel.OpenUI().OnClose = () =>
				{
					ChAnimUI1.OpenUI(DBUIController.mDBUIInstance._TopRoot).OnFinished = () =>
					{
						ShowFor2D_UI();
						StartCoroutine("CheckSunState");
					};
				};		   
			}
#else
			//现在的引导第一次是从战斗场景过来的
			if (!Core.Data.playerManager.RTData.IsRegister) 
			{
				HiddenFor3D_UI (false);
				JCSubtitlesPanel.OpenUI ().OnClose = () => 
				{
					JCCreateRole.OpenUI ();
				};		   
			} 
#endif
			else
				SunMoveFinish ();
		}
		if (LuaTest.Instance.OpenFirstCharge == false)
			if (Core.Data.ActivityManager.firstIOR == null || Core.Data.rechargeDataMgr._canGainFirstAward == -1) {
				ActivityNetController.GetInstance ().GetFirstChargeStateRequest ();
			}
	
		//added by zhangqiang 
		//发送招募时间同步消息
		Core.Data.zhaomuMgr.SendZhaomuStateMsg ();

		if(Core.Data.vipManager.IsFirstLogin_Vip)
		{
			UIDragonMallMgr.GetInstance().VipRequest();
			if(Core.Data.vipManager.vipStatus != null)
			{
				UIDragonMallMgr.GetInstance().SeViptPercent(Core.Data.vipManager.vipStatus.toNextLevle / 100, Core.Data.vipManager.vipStatus.totalCach / 100);
			}

		}
		if (BuildScene.mInstance != null) {
			BuildScene.mInstance.CheckFragBuilding ();
		}
	}



 

		
	//检查太阳状态
	public IEnumerator CheckSunState ()
	{
		while (true)
		{
			if (_playerViewCtl != null && _mainViewCtl != null)
			{
				//设置太阳开关
				if (Core.Data.temper.currentBattleType == TemporyData.BattleType.None || Core.Data.temper.currentBattleType == TemporyData.BattleType.FightWithFulisa )
				{
					SetScaleUIHide ();
					BuildScene.mInstance.sunCamera.Show_Key = true;
					AsyncTask.QueueOnMainThread (() =>
					{
						ShowScaleUIAnim ();
					}, BuildScene.mInstance.sunCamera.LongTime);
				}
				else
				{
					SunMoveFinish ();
				}
				break;
			}
		}
		yield return null;
	}

	void SetScaleUIHide ()
	{
		if(_playerViewCtl != null)
		{
			RED.SetActive (false, _playerViewCtl.gameObject);
			_playerViewCtl.m_scaleRoot.transform.localScale = Vector3.one * 0.00001f;
		}

		if (_noticeUI != null)
		{
			RED.SetActive (false, _noticeUI.gameObject);
			_noticeUI.scaleBg.transform.localScale = Vector3.one * 0.00001f;
		}

		if(_mainViewCtl != null)
		{
			RED.SetActive (false, _mainViewCtl.gameObject);
			_mainViewCtl.ResetScale (0.00001f);
			_mainViewCtl.m_bCanRotate = false;
		}
	}

	void ShowScaleUIAnim ()
	{
		if(_playerViewCtl != null)
		{
			RED.SetActive (true, _playerViewCtl.gameObject);
			_playerViewCtl.WillShowUI (0.3f);
		}

		if (_noticeUI != null)
		{
			RED.SetActive (true, _noticeUI.gameObject);
			_noticeUI.WillShowUI (0.3f);
		}

		if(_mainViewCtl != null)
		{
			RED.SetActive (true, _mainViewCtl.gameObject);
			_mainViewCtl.willShowUI (0.3f);
		}

		AsyncTask.QueueOnMainThread (() => { SunMoveDelay(); }, 1.0f);
		//Invoke ("SunMoveDelay", 1);
	}

	void SunMoveFinish ()
	{
		BuildScene.mInstance.sunCamera.Show_Key = false;
		ShowNotice ();
		
		m_bCanClick = true;

		GuideManager guideMgr = Core.Data.guideManger;
		TemporyData temp      = Core.Data.temper;

		if(guideMgr.isGuiding && Core.Data.guideManger.SpecialGuideID > 0) {
			guideMgr.Init();
		}
		
		temp.currentBattleType = TemporyData.BattleType.None;

		///
		/// --------  走马灯  -----------
		///如果是新手引导的时候，则跳过这次网络请求，我们会在新手引导结束的时候添加网络请求
		if(!guideMgr.isGuiding) {
			//只有第一次进入“GameUI”场景的时候
			if(Core.SM.LastScenesName == SceneName.LOGIN_SCENE) {
				Core.Data.MainNoticeMgr.loadFromNetwork( ()=> {
					_noticeUI = MainNoticeUI.getInstance(_bottomRoot);
				});
			} else {
				/// -------- 每五分钟会同步一次数据 -----------
				///
				_noticeUI = MainNoticeUI.getInstance(_bottomRoot);
				if(!Core.Data.temper.getNoticeReady) {
					Core.Data.MainNoticeMgr.loadFromNetwork (null);
				}
			}
		}

		///
		///  新手引导完成的时候，能调用这个。
		///
		guideMgr.OnGuideFinished = () => { 
			_noticeUI = MainNoticeUI.getInstance(_bottomRoot);
			if(!Core.Data.temper.getNoticeReady) {
				Core.Data.MainNoticeMgr.loadFromNetwork (null);
			}
		};

	}

	void ShowNotice ()
	{
		if (!Core.Data.guideManger.isGuiding && Core.Data.temper.currentBattleType == TemporyData.BattleType.None)
		{   
			ShowEveryDayNotice ();
		}
	}
	//延迟到太阳跑到合适的位置
	void SunMoveDelay ()
	{
		SunMoveFinish ();
		Core.Data.guideManger.Init ();
		if(_mainViewCtl != null)
		{
			_mainViewCtl.m_bCanRotate = true;
			if(_mainViewCtl.m_objSubFuben != null)
			{
				_mainViewCtl.m_objSubFuben.transform.localPosition = Vector3.zero;
			}
		}
	}

	//判断 7天奖励 还是 每日签到
	public  void ShowEveryDayNotice ()
	{
		if (Core.Data.guideManger.isGuiding)
		{
			Debug.Log ("GUide   is not  over  ");
			return;
		}

		if (NoticeManager.openAnnounce == true)
		{
			WXLAcitvityFactory.CreatActivity (ActivityItemType.gonggaoItem, null);
			NoticeManager.openAnnounce = false;
		}
		else
		{
			if (NoticeManager.openSign == true)
			{
//				if (NoticeManager.firstShowState == 1)
//				{
//                    //	UISevenDayRewardMain.OpenUI ();
//				}
//				else
                    if (NoticeManager.firstShowState == 2)
				{
					WXLAcitvityFactory.CreatActivity (ActivityItemType.qiandaoItem, (object)_TopRoot);  
				}
				NoticeManager.openSign = false;
			}
		}
	}

	void OnDestroy ()
	{
		mDBUIInstance.dealloc ();
		_mainViewCtl.dealloc ();
		_playerViewCtl.dealloc ();
		_petBoxCtl.dealloc ();
		_chapterViewCtl.dealloc ();
				//_WXLActivityMainCtl.dealloc ();
	}

	void LoadGameUI ()
	{
		if (_mainViewCtl == null)
		{
			_mainViewCtl = SQYMainController.CreateMainView ();
			_mainViewCtl.vcID = VC_MAIN_ID;
			RED.AddChild (_mainViewCtl.gameObject, _bottomRoot);
		}
		
		if (_playerViewCtl == null)
		{
			_playerViewCtl = SQYPlayerController.CreatePlayerView ();
			_playerViewCtl.myPlayerViewBehaviour += this.OnBtnPlayerViewID;
			_playerViewCtl.vcID = VC_PLAYER_ID;
			RED.AddChild (_playerViewCtl.gameObject, _bottomRoot);
		}
	}

	public void InitBlackMask ()
	{
      
		UnityEngine.Object obj = WXLLoadPrefab.GetPrefab (WXLPrefabsName.UIBlackAlphaMask);
		if (obj != null)
		{
			GameObject go = Instantiate (obj) as GameObject;
			Transform goTrans = go.transform;
			go.transform.parent = blackMaskPanel.transform;
			go.transform.localPosition = Vector3.zero;
			goTrans.localScale = Vector3.one;
			blackMaskObj = go;
		}
	}

	public GameObject GetBlackMask ()
	{
		return blackMaskObj;
	}

	public void SetViewStateWithData (EMViewState vs, object obj)
	{
		SetViewState (vs, EMBoxType.NONE, obj);
	}

	public void SetViewState (EMViewState vs, EMBoxType bt = EMBoxType.NONE, object obj = null)
	{
		switch (vs)
		{
			case EMViewState.MainView:
				break;
			case EMViewState.S_ShenLong:
				DBUIController.mDBUIInstance.HiddenFor3D_UI ();
				UIShenLongManager.setShenLongManagerRoot (_bottomRoot);
				break;
			case EMViewState.S_FuBen:
#if NEWPVE

                UIMiniPlayerController.ElementShowArray = new bool[]{true,false,true,true,true};
			    if(!_PVERoot.gameObject.activeSelf)
				    _PVERoot.gameObject.SetActive(true);

                pveView.SetActive(true);
				TopMenuUI.OpenUI();
#else
			chapterView.SetActive (true);
#endif			
				DBUIController.mDBUIInstance.HiddenFor3D_UI ();
				break;
			case EMViewState.S_QiangDuo:
				UIMiniPlayerController.ElementShowArray = new bool[]{true,true,false,true,true};
				FinalTrialMgr.GetInstance ().getAllData ();
//				DBUIController.mDBUIInstance.HiddenFor3D_UI ();
				break;
			case EMViewState.S_Team_NoSelect:				
					TeamUI.OpenUI ();
					HiddenFor3D_UI (false);
				break;
			case EMViewState.H_Bag:
				petBoxView.viewWillHidden ();
				break;
			case EMViewState.S_Bag:
            
				petBoxView.viewWillShow ();
				DBUIController.mDBUIInstance.HiddenFor3D_UI ();
            
				break;
			case EMViewState.S_Team_Select:
				HiddenFor3D_UI (false);
				TeamUI.OpenUI ();
				break;
			case EMViewState.S_CityFloor:
			 //更新PVE系统Timer
			   JCPVETimerManager.Instance.AutoOpenPVESystemTimer();
			 
			    NewFloor floordata = Core.Data.newDungeonsManager.curFightingFloor;
			
			    if(floordata != null && Core.Data.temper.warBattle.battleData.iswin == 1)
				{				   
				    floordata.state = NewFloorState.Pass;
				    if(floordata.config.ID > Core.Data.newDungeonsManager.lastFloorId)
					{				
						int nextId = ++Core.Data.newDungeonsManager.lastFloorId ;
	
					     NewFloor nextfloordata = null;
					    if(Core.Data.newDungeonsManager.FloorList.TryGetValue(nextId+1,out nextfloordata))
						{
							nextfloordata.state = NewFloorState.Current;
						}
					}			
								
				}
			
			    SQYMainController.mInstance.OnBtnFuBen();
			 
         
			  bool isOpenDescribe = false;
			   if ("PVEType_Plot" == Core.Data.newDungeonsManager.curFightingFBType)
			   {					
					   if (DBUIController.battleAgain == true )
	                   {
	                       DBUIController.battleAgain= false ;
			                if(DBUIController.battleAgainID <= Core.Data.newDungeonsManager.lastFloorId)
			                {
			                      JCPVEPlotController.tempOpenFloorID = DBUIController.battleAgainID;
							      isOpenDescribe = true;
			                }
					 }
	            }
			
	            JCPVEMainController.Instance.OnBtnClick(Core.Data.newDungeonsManager.curFightingFBType);
				if(isOpenDescribe)
				    JCPVEPlotController.Instance.OnBuildingClick(DBUIController.battleAgainID.ToString());
                
				break;
			case EMViewState.HIDE_TEAM_VIEW:
				if (TeamUI.mInstance != null)
					TeamUI.mInstance.CloseUI ();
					break;
			case EMViewState.S_ShangCheng:
				UIDragonMallMgr.GetInstance ().OpenUINew (ShopItemType.HotSale);
				break;
			
			case EMViewState.S_HuoDong:
				if (UIWXLActivityMainController.Instance != null)
					UIWXLActivityMainController.Instance.SetActive (true);
				else
					UIWXLActivityMainController.CreateActivityMainPanel (DBUIController.mDBUIInstance._TopRoot);
				//mActPanel.SetActive (true);
					ActivityNetController.GetInstance ().SendLoginMSG (Core.Data.playerManager.PlayerID, null);
					break;
			case EMViewState.S_MosterCome:
				MonsterComePanel.SetActive (true);

				break;
			case EMViewState.S_XiaoXi:
				MessageMgr.GetInstance().SetInfoPanel(true);
				break;
			case EMViewState.S_SevenDaysReward:
				UISevenDayRewardMain.OpenUI ();
//			DBUIController.mDBUIInstance.HiddenFor3D_UI ();
				break;
			case EMViewState.S_Friend:
				{

					Core.Data.FriendManager.initFriendListInfo ();
					Core.Data.FriendManager.initSuDiListInfo ();
					Core.Data.FriendManager.initFriendRequestListInfo ();

					UIMainFriend.Instance.getFriendList ();
					UIMainFriend.Instance.setMainFriendRoot (_bottomRoot);
					UIMainFriend.Instance.gameObject.SetActive (false);
				}
				break;
		}

		if (bt != EMBoxType.NONE)
		{
			petBoxView.SetPetBoxType (bt);
		}
	}

	public void HiddenFor3D_UI (bool showMiniBar = true)
	{
		if (BuildScene.mInstance != null)
		{
			BuildScene.mInstance.CheckAllBuildAnim();
			BuildScene.mInstance.SetShow (false);
			AsyncLoadScene.m_Instance.SetFBS (SceneName.GAME_BATTLE);
		}

		if (_mainViewCtl != null)
		{
//			UIBackground.SetActive(true);
//			_mainViewCtl.SetActive (false);
			try 
			{
				RED.SetActive (false, _mainViewCtl.gameObject);
				RED.SetActive (true, UIBackground);
			}
			catch (Exception ex)
			{
				RED.LogWarning ("error in hiddenfor3d");
			}
		}
		if (_playerViewCtl != null)
		{
//			_playerViewCtl.SetActive (false);
			RED.SetActive (false, _playerViewCtl.gameObject);
		}

		if (MainNoticeUI.getInstance(_bottomRoot) != null)
		{
			MainNoticeUI.getInstance(_bottomRoot).SetShow (false);
		}

		if (showMiniBar)
		{
			UIMiniPlayerController.Instance.SetActive (true);
			UIMiniPlayerController.Instance.freshPlayerInfoView ();
		}
		else
		{
			UIMiniPlayerController.Instance.SetActive (false);
		}
	}

	public void ShowFor2D_UI (bool showMiniBar = false)
	{
		if (BuildScene.mInstance != null)
		{
			BuildScene.mInstance.SetShow (true);
			AsyncLoadScene.m_Instance.SetFBS (SceneName.MAINUI);
		}

		if (_mainViewCtl != null)
		{
			UIBackground.SetActive(false);
			_mainViewCtl.SetActive (true);
			_mainViewCtl.UpdateTeamTip();

            if (_mainViewCtl.gameObject.activeInHierarchy == true)
            {
                Core.Data.temper.deltaAtk = Core.Data.playerManager.RTData.curTeam.teamAttack- Core.Data.temper.tempTeamAtk ;
                Core.Data.temper.deltaDef =  Core.Data.playerManager.RTData.curTeam.teamDefend -  Core.Data.temper.tempTeamDef ;
//                Debug.Log(" delta atk = " + Core.Data.temper.deltaAtk );
//                Debug.Log(" delta def = " + Core.Data.temper.deltaDef );
                this._mainViewCtl.ShowUpdate();
            }
		}

		if (_playerViewCtl != null)
		{
			_playerViewCtl.SetActive (true);
		}

		if (_noticeUI != null)
		{
			_noticeUI.SetShow (true);
		}

		UIMiniPlayerController.Instance.SetActive (showMiniBar);
		SQYMainController.mInstance.UpdateBagTip ();
		SQYMainController.mInstance.UpdateDailyGiftTip ();
		WillShowMainUI();
	}



	/// <summary>
	/// 这个是刷新顶部UI的方法，
	/// </summary>
	public void RefreshUserInfo ()
	{
		_playerViewCtl.freshPlayerInfoView ();
		_mainViewCtl.RefreshUserInfo ();
		UIMiniPlayerController.Instance.freshPlayerInfoView ();
		UIMiniPlayerController.Instance.SetActive (!_playerViewCtl.gameObject.activeSelf);
	}
	//在某些情况下，我们无须使顶部UI显示出来的情况下更新
	public void RefreshUserInfoWithoutShow ()
	{
		_playerViewCtl.freshPlayerInfoView ();
		_mainViewCtl.RefreshUserInfo ();
	}

	void CheckJump ()
	{
		switch (BattleToUIInfo.From)
		{
			case EMViewState.NONE:
				break;
			case EMViewState.MainView:		   
				break;
			case EMViewState.S_QiangDuo:
				{
				   SetViewState (EMViewState.S_QiangDuo);
//				   FinalTrialDougoenType _type = FinalTrialMgr.GetInstance ().NowDougoenType;
				   FinalTrialMgr.GetInstance ().SetShaluBuouStatus(Core.Data.temper.warBattle.battleData.iswin);

			     	if (FinalTrialMgr.GetInstance ().NowEnum == TrialEnum.TrialType_ShaLuChoose)
					{
					     FinalTrialMgr.GetInstance().OpenNewMap(1);
					     mDuoBaoView.SetActive(false);
					}
					else if(FinalTrialMgr.GetInstance ().NowEnum == TrialEnum.TrialType_PuWuChoose)
					{
						FinalTrialMgr.GetInstance().OpenNewMap(2);
					    mDuoBaoView.SetActive(false);
					}

				}
				break;
			case EMViewState.S_HuoDong:
				{
					SetViewState (EMViewState.S_HuoDong);
				}
				break;
			case EMViewState.S_MosterCome:

				SetViewState (EMViewState.S_MosterCome);
				break;
			case EMViewState.S_QiangDuoDragonBall:
				{
				    SetViewState (EMViewState.S_QiangDuo);
					SetViewState (EMViewState.S_ShenLong);
				}
				break;
			case EMViewState.S_Friend:
				{
					SetViewState (EMViewState.S_Friend);
				}
				break;
			case EMViewState.S_Team_NoSelect:
				{
					SetViewState (EMViewState.S_Team_NoSelect);
					HiddenFor3D_UI (false);
				}
				break;
			case EMViewState.S_Bag:
				{
					SetViewState (EMViewState.S_Bag, EMBoxType.LOOK_Charator);
					HiddenFor3D_UI ();
				}
				break;
			case EMViewState.S_ShangCheng:
				{
					SetViewState (EMViewState.S_ShangCheng);
				}
				break;
			case EMViewState.S_MailBox:
				{
					MailBox.OpenUI(1);
				    DBUIController.mDBUIInstance.HiddenFor3D_UI ();
				}
			    break;
			case EMViewState.S_XiaoXi:
				MessageMgr.GetInstance().SetInfoPanel(true);
				break;

			case EMViewState.S_GPSWar:
				HiddenFor3D_UI ();
				RadarTeamUI.OpenUI ();
				break;

            default: //EMViewState.S_CityFloor

                WhichChange c = Core.Data.dungeonsManager.getChanged;
                if (c.ChapterChanged)
                {
                    SetViewState(EMViewState.S_FuBen);
                }
                else
                {
                    SetViewState(EMViewState.S_CityFloor, EMBoxType.NONE, CityFloorData.Instance.chapter);
                }
                if (GameObject.Find("pbUICityFloorManager(Clone)") != null)
                    DBUIController.mDBUIInstance.HiddenFor3D_UI();
                // add by wxl 
                PreDownload();

				if(Core.Data.temper != null && Core.Data.playerManager.Lv  <= Core.Data.temper.mPreLevel && Core.Data.temper.mPreLevel > 0)
				{
					Core.Data.deblockingBuildMgr.OpenLevelUpUnlock(false);
				}
				break;
		}
		
		
		//自动处理升级弹窗
		AutoDealLevelUpUI();

		BattleToUIInfo.From = EMViewState.NONE;
	}
	
	
	
	//自动处理升级弹窗
	public bool AutoDealLevelUpUI()
	{
		//战斗直接返回。升级提示
		if(Core.Data.temper != null && Core.Data.playerManager.Lv  > Core.Data.temper.mPreLevel && Core.Data.temper.mPreLevel > 0)
		{
			if(Core.Data.guideManger.SpecialGuideID == 0)
			{
				LevelUpUI.OpenUI();
				Core.Data.temper.mPreLevel = Core.Data.playerManager.Lv;
				Core.Data.temper.mPreVipLv = Core.Data.playerManager.curVipLv;
				return true;
			}		
		}		
		return false;
	}
	
	
	
	
	//直接跳转副本的某一关
	public bool JumpToFB (int floodID)
	{
		DungeonsManager dm = Core.Data.dungeonsManager;
		//Debug.Log("floodID="+floodID+"    dm.lastFloorId="+dm.lastFloorId);
		if (floodID > dm.lastFloorId)
			return false;
		int cityid = dm.reverseToCity (floodID).ID;
		int chapterid = dm.reverseToChapter (cityid).ID;
		Chapter c = (Chapter)dm.ChapterList [chapterid].Clone ();
		c.toFightCityId = cityid;
		c.toFightCity = new City ();
		c.toFightCity = dm.CityList [cityid];
		c.toFightCity.toFightFloorId = floodID;

		DBUIController.mDBUIInstance.SetViewStateWithData (RUIType.EMViewState.S_CityFloor, c);
		return true;
	}
	
	public void OpenFB(int ChapterIndex,System.Action backCall = null)
	{
		chapterView.SetActive (true);
		//下标从0下始
		DBUIController.mDBUIInstance.HiddenFor3D_UI ();
		SQYChapterController.OnExitBackCall += backCall;
		chapterView.SetChapterAtIndex(ChapterIndex -1);
	}

    /// <summary>
    /// 去神龙跳转
    /// </summary>
    /// <param name="callBack">Call back.</param>
    public void JumpToDragon(System.Action callBack){
        DBUIController.mDBUIInstance.HiddenFor3D_UI ();
        UIShenLongManager.Instance.DragonBallSetUp (callBack);
     
    }



	public void WillShowMainUI()
	{
		if (_mainViewCtl != null)
		{
			_mainViewCtl.willShowUI ();
		}
		if (_playerViewCtl != null)
		{
			_playerViewCtl.WillShowUI ();
		}
		if (_noticeUI != null)
		{
			_noticeUI.WillShowUI ();
		}
	}

	
	public void WillHideMainUI()
	{
		if(_mainViewCtl != null)
		{
			_mainViewCtl.willHideUI();
		}
		if(_playerViewCtl != null)
		{
			_playerViewCtl.WillHideUI();
		}
		if (_noticeUI != null)
		{
			_noticeUI.WillHideUI ();
		}
	}
	
	//跳到PVE系统功能( PVE类型,关卡ID[剧情] )
	public bool OpenFVE(PVEType type,int floorID = 60101)
	{
		if(type == PVEType.PVEType_Plot)
		{
			if(floorID <= Core.Data.newDungeonsManager.lastFloorId)
			JCPVEPlotController.tempOpenFloorID = floorID;
			else
				return false;
		}



		if(JCPVEPlotController.Instance != null)
		{
			if(FightRoleSelectPanel.Instance != null)
				FightRoleSelectPanel.Instance.OnBtnClick("Btn_Close");

			if(PVEDownloadCartoonController.Instance != null)
					JCPVEPlotController.Instance.OnBtnClick("Btn_Close");

			if(JCPVEPlotDes.Instance != null)
				JCPVEPlotController.Instance.OnBtnClick("Btn_Close");

			if(JCPVEPlotController.Instance != null)
//				JCPVEPlotController.Instance.OnBtnClick("Btn_Close");
			   JCPVEPlotController.Instance.OpenPVEWhenExist(floorID);
		}
		SQYMainController.mInstance.OnBtnFuBen();
		JCPVEMainController.Instance.OnBtnClick(type.ToString());
		return true;
	}


    // 预下载  wxl 
    void PreDownload(){
        if (Core.Data.usrManager.UserConfig.cartoon == 1)
        {
            UIDownloadTexture preLoad = new UIDownloadTexture();
            StartCoroutine(preLoad.PreDownload((Core.Data.newDungeonsManager.lastFloorId + 2) + "-1.jpg"));
            StartCoroutine(preLoad.PreDownload((Core.Data.newDungeonsManager.lastFloorId + 2) + "-2.jpg"));
        }
    }

	//同步数据的时候，返回主界面
	public void SyncBackToMainUI()
	{
		List<Transform> btmChild = new List<Transform> ();
		for (int i = 0; i < _bottomRoot.transform.childCount; i++)
		{
			btmChild.Add (_bottomRoot.transform.GetChild (i));
		}

		for (int i = 0; i < btmChild.Count; i++)
		{
			bool bfind = false;
			for (int j = 0; j < STATIC_BTMUI.Length; j++)
			{
				if (string.Equals (btmChild [i].name, STATIC_BTMUI[j]))
				{
					bfind = true;
					break;
				}
			}
			if (!bfind)
			{
				Destroy (btmChild [i].gameObject);
			}
		}

		List<Transform> topChild = new List<Transform> ();
		for (int i = 0; i < _TopRoot.transform.childCount; i++)
		{
			topChild.Add (_TopRoot.transform.GetChild (i));
		}

		for (int i = 0; i < topChild.Count; i++)
		{
			bool bfind = false;
			for (int j = 0; j < STATIC_TOPUI.Length; j++)
			{
				if (string.Equals (topChild [i].name, STATIC_TOPUI[j]))
				{
					bfind = true;
					break;
				}
			}
			if (!bfind)
			{
				Destroy (topChild [i].gameObject);
			}
		}

		GameObject obj = GameObject.Find ("ShowStage(Clone)");
		if (obj != null)
		{
			CRLuo_ShowStage Stage = obj.GetComponent<CRLuo_ShowStage> ();
			if (Stage != null)
				Stage.DeleteSelf ();
		}

		obj = GameObject.Find ("ShowStage_Card(Clone)");
		if (obj != null)
		{
			CRLuo_ShowStage Stage = obj.GetComponent<CRLuo_ShowStage> ();
			if (Stage != null)
				Stage.DeleteSelf ();
		}

		obj = GameObject.Find ("ShowMonsterStage(Clone)");
		if (obj != null)
		{
			CRLuo_ShowStage Stage = obj.GetComponent<CRLuo_ShowStage> ();
			if (Stage != null)
				Stage.DeleteSelf ();
		}

		//一键退各种抽蛋
		CradSystemFx.GetInstance().DeleteAllOneKey();
			
		//一键退出副本
		_PVERoot.ResetPVESystem();
		ShowFor2D_UI ();
	}
}
