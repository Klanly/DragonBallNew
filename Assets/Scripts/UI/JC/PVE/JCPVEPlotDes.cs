using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class JCPVEPlotDes : MonoBehaviour {

	public List<JCPVEPlotDesMonsterHead> ListDropReward = new List<JCPVEPlotDesMonsterHead>();
	
	public UILabel Lab_Energy;
	
	public UILabel Lab_EmenyAct;

	public UILabel lab_TeamSize;				//上阵武者人数

	public UILabel lab_PassTime;				//通关次数
	
	public UISprite Spr_Sgin;
	
	public static JCPVEPlotDes Instance = null;
	
	public StarsUI Stars;
	
	public UILabel Lab_Des;
	
	public UILabel Lab_Title;

	public UILabel lab_GetTip;

	public UIButton btn_Buy;					//购买通关次数
	public UIButton btn_saoDangs;				//扫荡十次
	public UIButton btn_saoDang;				//扫荡一次
	public UILabel lab_saoDangTip;				//扫荡提示
	public UISprite sp_saoDangBg;				//扫荡背景
	private GameObject m_saodangRoot;			//扫单root

    public UIButton btn_ShowPic;


    PVEDownloadCartoonController pveCartoonCtrl = null;


	NewFloor floordata = null;
		
	public static JCPVEPlotDes OpenUI(NewFloor floor)
	{
		if(Instance == null)
		{
			Object prefab = PrefabLoader.loadFromPack("JC/JCPVEPlotDes");
			if(prefab != null)
			{
				GameObject obj = Instantiate(prefab) as GameObject;
				RED.AddChild(obj, DBUIController.mDBUIInstance._PVERoot.gameObject);
				Instance = obj.GetComponent<JCPVEPlotDes>();				
				Instance.RefreshPanel(floor);
				DBUIController.mDBUIInstance._PVERoot.AddPage("JCPVEPlotDes",obj);
			}
		}		
		return Instance;
	}
	
	
	public static void SafeDelete()
	{
		if(Instance != null)
			Instance.Close();
	}
	
	int leftTime = 0;
	void RefreshPanel(NewFloor floor)
	{
		floordata = floor;
		ShowDropReward(floor);
	
		Stars.SetStar(floor.star);	

		ExploreConfigData explore = Core.Data.newDungeonsManager.GetExploreData (5);

		leftTime = floordata.config.times - floordata.passTimes;
		string strText = "";
		if(floor.config.isBoss > 0)
			strText = Core.Data.stringManager.getString(9079);
		else
			strText = Core.Data.stringManager.getString(9104);
		lab_GetTip.text = strText;

		if (Core.Data.newDungeonsManager.lastFloorId >= explore.openfloor)
		{
			RED.SetActive (true, btn_saoDangs.transform.parent.gameObject);
			CanSaoDang = ((floor.star >= floor.config.Sweep) && (leftTime > 0));
			Lab_Des.width = 800;
		}
		else
		{
			CanSaoDang = false;
			RED.SetActive (false, btn_saoDangs.transform.parent.gameObject);
			Lab_Des.width = 1000;
		}

		Lab_Des.text = "　　"+floor.config.Des;
		Lab_Title.text = floor.config.name;
		NeedEnergy = floor.config.NeedEnergy;
		FightType = floor.config.FightType;
		TeamSize = floor.config.TeamSize;


		lab_PassTime.text = "[f4cd8b]" + Core.Data.stringManager.getString (9105) + "[-]   " + leftTime.ToString() + "/" + floordata.config.times.ToString();

		if (LuaTest.Instance.OpenCartoon) {
			if (Core.Data.playerManager.Lv == 1 || !Core.Data.guideManger.isGuiding) {
				if (floordata != null) {
					if (floordata.config.ID - 1 == Core.Data.newDungeonsManager.lastFloorId) {
						if (Core.Data.usrManager.UserConfig.cartoon == 1) {
#if NewGuide
							if(!Core.Data.guideManger.isGuiding)
							pveCartoonCtrl = PVEDownloadCartoonController.CreateCartoonPanel (floordata.config, true);
#else
							pveCartoonCtrl = PVEDownloadCartoonController.CreateCartoonPanel (floordata.config, true);
#endif
						} else {
							pveCartoonCtrl = PVEDownloadCartoonController.CreateCartoonPanel (floordata.config);
						}
					} else {
						pveCartoonCtrl = PVEDownloadCartoonController.CreateCartoonPanel (floordata.config);
					}
				}
			}
		} else {
			if (Core.Data.playerManager.Lv == 1 && Core.Data.guideManger.isGuiding) {
				if (floordata != null) {
					if (floordata.config.ID - 1 == Core.Data.newDungeonsManager.lastFloorId) {
						pveCartoonCtrl = PVEDownloadCartoonController.CreateCartoonPanel (floordata.config, true);
					}
				}
			}
		}
//        PrepareCartoonPanel();

		RED.SetActive (leftTime <= 0, btn_Buy.gameObject);

		UILabel lab_saodang = btn_saoDangs.GetComponentInChildren<UILabel>();
		if(lab_saodang != null)
			lab_saodang.text = string.Format (Core.Data.stringManager.getString (9106), leftTime);   //{"ID":9106,"txt":"扫荡{0}次"}
	}
	
	//显示掉落奖励
	void ShowDropReward(NewFloor floor)
	{
		List<int[]> rewards = floor.config.Reward;
		int count = rewards.Count;
		for(int i=0; i<ListDropReward.Count; i++)
		{
			if(i<count)
			{
			    ListDropReward[i].gameObject.SetActive(true);
				ItemOfReward reward = new ItemOfReward();
				reward.pid = rewards[i][0];
				ListDropReward[i].SetData(reward);
			}
		}
		
		float startx = -275f;
		float starty = -290f;
		foreach( JCPVEPlotDesMonsterHead head in ListDropReward)
		{
			head.transform.localPosition = new Vector3(startx,starty,0);			
			startx+=head.CellWidth;
		}

	}

	int TeamSize
	{
		set
		{
			lab_TeamSize.text = "[f4cd8b]" + Core.Data.stringManager.getString (9093) + "[-]   " + value.ToString ();
		}
	}

	float NeedEnergy
	{
		set
		{
			//精力消耗  34
			Lab_Energy.text ="[f4cd8b]"+ Core.Data.stringManager.getString(9077)+"[-]        "+value.ToString();
		}
	}
	
	int FightType
	{
		set
		{
			//int PlayerCombatValue = value== 0?Core.Data.playerManager.RTData.curTeam.teamAttack : Core.Data.playerManager.RTData.curTeam.teamDefend;		
			Spr_Sgin.spriteName = value==0 ? "common-0008":"common-0010";
			string Contenet =  Core.Data.stringManager.getString(9078);
			Lab_EmenyAct.text = "[f4cd8b]"+ Contenet +"[-]        " /*+ PlayerCombatValue.ToString()*/;		
		}
	}

	public void Close()
	{
		Destroy(gameObject);
		Instance = null;
	}

	//是否解锁连续扫荡
	bool isUnLockSaoDangs = false;
	//是否可以扫荡
	public bool CanSaoDang
	{
		set
		{
			Transform SangDang_Obj = btn_saoDangs.transform.parent;
			SangDang_Obj.localPosition = new Vector3(405f,10f,0);

			btn_saoDang.isEnabled = value;

#region 连续扫荡功能是根据VIP等级解锁的
			isUnLockSaoDangs = Core.Data.playerManager.curVipLv >= Core.Data.vipManager.GetUnLockContinuousSweepingVIPLevel() && value;

			btn_saoDangs.GetComponent<UIButton>().enabled = isUnLockSaoDangs;
			btn_saoDangs.GetComponentInChildren<UISprite>().color = isUnLockSaoDangs ? new Color(1f,1f,1f,1f) : new Color(0,0,0,1f);
			btn_saoDangs.GetComponent<BoxCollider>().enabled = leftTime >0 ? true : false ;
#endregion

			if(leftTime > 0)
			{
			   RED.SetActive (!value, lab_saoDangTip.gameObject);
				if (!value)
				{
					ExploreConfigData explore = Core.Data.newDungeonsManager.GetExploreData (5);
					if (Core.Data.newDungeonsManager.lastFloorId >= explore.openfloor)
					{
						lab_saoDangTip.text = Core.Data.stringManager.getString (9097);    //{"ID":9097,"txt":"满星开启"}
					}
					else
					{
						string strText = Core.Data.stringManager.getString (9096);   //{"ID":9096,"txt":"第{0}章开启"}
						int num = floordata.BelongChapterID % 30000 / 100;
						strText = string.Format (strText, RED.GetChineseNum(num));
						lab_saoDangTip.text = strText;
					}
					sp_saoDangBg.height = 220;
				}
				else
				{
					sp_saoDangBg.height = 180;
					SangDang_Obj.localPosition = new Vector3(405f,48f,0);
				}
			}
			else 
			{
				//没有扫荡次数了
				RED.SetActive (false, lab_saoDangTip.gameObject);
				sp_saoDangBg.height = 180;
				SangDang_Obj.localPosition = new Vector3(405f,48f,0);
			}
		}
	} 
	
	void OnBtnClick(GameObject Btn)
	{
		OnBtnClick(Btn.name);
	}

	void OnClickReset()
	{
		VipInfoData vip = Core.Data.vipManager.GetVipInfoData (Core.Data.playerManager.curVipLv);

		if(floordata.resetTimes < vip.starttimes)
		{
			string strText = Core.Data.stringManager.getString (9108);

			strText = string.Format (strText, Core.Data.newDungeonsManager.GetResetFloorCost(floordata.resetTimes + 1), floordata.resetTimes);
			UIInformation.GetInstance ().SetInformation (strText, Core.Data.stringManager.getString (5030), BuyReset, null);
		}
		else
		{
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (5239));
		}
	}

	void BuyReset()
	{
		if (Core.Data.playerManager.RTData.curStone < 20)
		{
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(7310));
			return;
		}

		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.RESET_FLOOR, new  ResetFloorParam(Core.Data.playerManager.PlayerID, floordata.config.ID));

		task.ErrorOccured += testHttpResp_Error;
		task.afterCompleted += testHttpResp_UI;

		task.DispatchToRealHandler();

		ComLoading.Open ();
	}

	#region 网络返回

	void testHttpResp_UI (BaseHttpRequest request, BaseResponse response)
	{
		ComLoading.Close ();
		if (response.status != BaseResponse.ERROR) 
		{
			RefreshPanel (floordata); 
			DBUIController.mDBUIInstance.RefreshUserInfo ();
		} 
		else 
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
		}
	}
		
	void testHttpResp_Error (BaseHttpRequest request, string error)
	{
		ComLoading.Close();
		ConsoleEx.DebugLog ("---- Http Resp - Error has ocurred." + error);
		SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(36000));
	}

	#endregion
    public bool  isSaoDang = true  ;// 创建完扫荡界面才可以点击第二次 ycg 12 月 13日
    public void IsSaoDang()
    {
        isSaoDang = true ;
    }

	public void OnBtnClick(string BtnName)
	{
		int left = floordata.config.times - floordata.passTimes;

		switch(BtnName)
		{
		case "Btn_SaoDang1":
            if (isSaoDang)
            {
                isSaoDang= false ;
                SaoDang(1);
                Invoke("IsSaoDang" ,0.5f);

            }
			break;
		case "Btn_SaoDang10":
			if(isUnLockSaoDangs)
			    SaoDang (left);
			else
			{
				string content = Core.Data.stringManager.getString(9117);
				content = content.Replace("{}",Core.Data.vipManager.GetUnLockContinuousSweepingVIPLevel().ToString() );
				SQYAlertViewMove.CreateAlertViewMove(content);
			}
			break;
		case "Btn_Fight":
			if(Core.Data.playerManager.curJingLi < floordata.config.NeedEnergy)
			{
				JCRestoreEnergyMsg.OpenUI(110015,110016);
				return;
			}
			if (left <= 0)
			{
				//战斗次数已满
				SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(9107));
				return;
			}	
			FightRoleSelectPanel FPanel = FightRoleSelectPanel.OpenUI(floordata.config.TeamSize,SelectFightPanelType.STORY_BATTLE,  floordata.config.FightType);
			if(FPanel != null)
			{
				JCPVEPlotController.Instance.gameObject.SetActive(false);
				gameObject.SetActive(false);
				
				FPanel.CallBack_Fight = (int[] array,int teamID) => 
				{
					if(array.Length == 0)
					    SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(9092));
					else
					{
#if NewGuide
						if(Core.Data.guideManger.isGuiding)
						{
							FPanel.gameObject.SetActive(false);
							TopMenuUI.mInstance.gameObject.SetActive(false);
							DBUIController.mDBUIInstance.UIBackground.SetActive(false);
							DBUIController.mDBUIInstance.BottomCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
							//如果在新手引导状态下，播放一个动画
							if( floordata.config.ID == 60101)
							{
								GuideAnimation2D_2.OpenUI(DBUIController.mDBUIInstance._TopRoot.transform).OnFinished = () =>
								{
									JCPVEFightLogic.Instance.Fight(floordata.config.ID,array,teamID);
								};
							}
							else if( floordata.config.ID == 60102)
							{
								ChAnimUI3.OpenUI(DBUIController.mDBUIInstance._TopRoot.transform).OnFinished = () =>
								{
									JCPVEFightLogic.Instance.Fight(floordata.config.ID,array,teamID);
								};
							}
							else if( floordata.config.ID == 60103)
							{
								ChAnimUI4.OpenUI(DBUIController.mDBUIInstance._TopRoot.transform).OnFinished = () =>
								{
									JCPVEFightLogic.Instance.Fight(floordata.config.ID,array,teamID);
								};
							}
						}
						else
#endif
							JCPVEFightLogic.Instance.Fight(floordata.config.ID,array,teamID);
					}

				};


				FPanel.OnClose = () =>
				{
					JCPVEPlotController.Instance.gameObject.SetActive(true);
					gameObject.SetActive(true);
				};
			}
			break;
		}
	}

	//扫荡
	void SaoDang(int count)
	{
		if (count <= 0)
		{
			//战斗次数已满
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(9107));
			return;
		}

		if(Core.Data.playerManager.curJingLi < floordata.config.NeedEnergy * count)
		{
			JCRestoreEnergyMsg.OpenUI(110015,110016);
			return;
		}

	    ComLoading.Open();
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.SAODANG, new SaoDangParam(Core.Data.playerManager.PlayerID, floordata.config.ID , count) );
        task.ErrorOccured += HttpResp_Error;
        task.afterCompleted += SaoDangSucess;
        task.DispatchToRealHandler();
	}
	
	void SaoDangSucess(BaseHttpRequest request, BaseResponse response)
	{
		 ComLoading.Close();
		if (response.status != BaseResponse.ERROR)
		{
			SaoDangResponse r = response as SaoDangResponse;

			HttpRequest htp = request as HttpRequest;
			SaoDangParam param = htp.ParamMem as SaoDangParam;
			if (r != null && r.data != null)
			{
				BattleSequence[] bsquesce = r.data.sweepList;
				List<BattleSequence> list_data = new List<BattleSequence> ();
				foreach (BattleSequence data in bsquesce)
				{
					list_data.Add (data);
				}
				JCPVESaoDangPanel.OpenUI (list_data);

				//同步通过次数
				NewFloor floor = Core.Data.newDungeonsManager.GetFloorData (param.doorId);
				floor.passTimes = r.data.passCount;
				RefreshPanel (floor);
			}
		}
		else
		{
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getNetworkErrorString (response.errorCode));
		}
	}
	

    #region   add by wxl 


    void ShowCartoonBtn(){
        if (pveCartoonCtrl != null)
            pveCartoonCtrl.OpenCartoonPanel();
    }


//    void PrepareCartoonPanel(){
//        if (pveCartoonCtrl == null)
//        {
//            pveCartoonCtrl = PVEDownloadCartoonController.CreateCartoonPanel(floordata.config);
//        }else{
//            btn_ShowPic.GetComponent<RedUIDragObject>().target = pveCartoonCtrl.transform;
//            btn_ShowPic.GetComponent<RedUIDragObject>().dragMovement = Vector3.up;
//            btn_ShowPic.GetComponent<RedUIDragObject>().dragEffect = RedUIDragObject.DragEffect.Momentum;
//
//        }
//    }
    #endregion
	
	
	
//	 void SendBattleRequest() 
//    {
//		ComLoading.Open();
//		//added by zhangqiang ao rember level
//		if(Core.Data.playerManager.RTData.curTeam.validateMember == 0)
//		{
//			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(5031));
//			ComLoading.Close();
//			return;
//		}
//		
//		Core.Data.temper.mPreLevel = Core.Data.playerManager.RTData.curLevel;
//		Core.Data.temper.mPreVipLv = Core.Data.playerManager.RTData.curVipLevel;
//
//        HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
//		
//		int flag =0 ;
//		//检测该关卡是否是这一章节的最后一个关卡
//		bool isLastFloorOfChapter = false;
//
//		task.AppendCommonParam(RequestType.NEW_PVE_BOSSBATTLE, new ClientBattleParam(Core.Data.playerManager.PlayerID, floordata .config.ID, 0, 0, 0, 0,Core.Data.guideManger.isGuiding ? 1 : 0 ));
//
//        task.ErrorOccured += HttpResp_Error;
//        task.afterCompleted += BattleResponseFUC;
//        task.DispatchToRealHandler();
//    }
	
	
	
	/*点击BOSS关卡<执行>按钮后服务器返回的数据
	 * */
//    void BattleResponseFUC (BaseHttpRequest request, BaseResponse response) 
//    {
//        ComLoading.Close();
//        if(response != null)
//        {
//            TemporyData temp = Core.Data.temper;
//
//            if(response.status!=BaseResponse.ERROR)
//            {
//                BattleResponse r = response as BattleResponse;
//
//				ClientBattleResponse resp = response as ClientBattleResponse;
//
//				if(r != null) {
//					if(r != null && r.data != null && r.data.reward != null && r.data.sync != null) Core.Data.playerManager.RTData.curVipLevel = r.data.sync.vip;
//
//					r.data.battleData.rsty = null;
//					r.data.battleData.rsmg = null;
//                    temp.warBattle = r.data;
//
//                    temp.currentBattleType = TemporyData.BattleType.BossBattle;
//
//					HttpRequest req = request as HttpRequest;
//					BaseRequestParam param = req.ParamMem;
//					//BattleResponse res = response as BattleResponse;
//					BattleParam bp = param as BattleParam;
//					FloorData floorD =	Core.Data.dungeonsManager.getFloorData(bp.doorId);
//					if(r.data.battleData.iswin  == 1){
//						if(floorD != null)
//							Core.Data.ActivityManager.OnMissionComplete(floorD.name);
//					}else {
//						if(floorD != null)
//							Core.Data.ActivityManager.OnMissionFail(floorD.name);
//					}
//					if(bp.flag == 1){
//						//add by wxl 
//						Core.Data.ActivityManager.OnPurchaseVirtualCurrency(ActivityManager.ChapterType,1,10);
//					}
//				} 
//
//				if(resp != null) { 
//                    temp.currentBattleType = TemporyData.BattleType.BossBattle;
//                    temp.clientDataResp = resp;
//
//                    temp.Open_StepMode = true;
//                    temp.Open_LocalWarMode = true;
//
//					HttpRequest req = request as HttpRequest;
//					if(req != null) {
//						ClientBattleParam param = req.ParamMem as ClientBattleParam;
//						if(param != null)
//                            temp.clientReqParam = param;
//					}
//
//				}
//
//                //跳转至Ban 的场景
//                JumpToBattleView();
//            }
//            else
//            {
//				if(response.errorCode == 4002)
//				JCRestoreEnergyMsg.OpenUI(110015,110016);
//				else
//                ErrorDeal(response.errorCode);
//            }
//        }
//    }
//	
//	void JumpToBattleView() 
//    {
//		Core.Data.newDungeonsManager.curFightingFloor = floordata;
//        BattleToUIInfo.From = RUIType.EMViewState.S_CityFloor;
//		Core.Data.temper.CitySence = floordata.config.Scence;
//		Core.SM.beforeLoadLevel(Application.loadedLevelName, SceneName.GAME_BATTLE);
//        AsyncLoadScene.m_Instance.LoadScene(SceneName.GAME_BATTLE);
//    }
//	
//	


	void HttpResp_Error (BaseHttpRequest request, string error) 
    {
        ComLoading.Close();
    }



}
