using UnityEngine;
using System.Collections;

public class NewJCReceiver : EventReceiver {
	
	public NewJCReceiver()
	{
		
	}
	
	protected override void OnEvent(EventTypeDefine p_e,object p_param)
	{
		GuideData data = p_param as GuideData;
		switch(p_e)
		{
		//点击副本
		case EventTypeDefine.Click_FB:		
			DBUIController.mDBUIInstance.OnBtnMainViewID(SQYMainController.CLICK_FuBen);
			break;
		//*选择第一大章
		case EventTypeDefine.Click_FirstChapter:
			foreach(Chapter c in Core.Data.dungeonsManager.ChapterList.Values)
			{
				DBUIController.mDBUIInstance.SetViewStateWithData(RUIType.EMViewState.S_CityFloor, c);
                UIMiniPlayerController.Instance.SetActive (false);
				break;
			}
			break;
		//点击执行
		case EventTypeDefine.Click_RunButton:			
			CityFloorData.Instance.ClickFloorItem(Core.Data.dungeonsManager.FloorList[ CityFloorData.Instance.currCity.toFightFloorId ],true);
			break;
		//点击收取
		case EventTypeDefine.Click_ReceiveButton:
			GameObject.Find("pbUIFloorReward(Clone)").GetComponent<UIFloorReward>().OnColse();	
			City CurFightCity = CityFloorData.Instance.currCity;//Core.Data.dungeonsManager.CityList[Core.Data.dungeonsManager.fightCityId];
			//60105
			CurFightCity.toFightFloorId =CurFightCity.config.floorID[CurFightCity.config.floorID.Length-1] ;
			//60104
			Core.Data.dungeonsManager.lastFloorId = CurFightCity.toFightFloorId-1;
			//直接通过当前City的所有小关
			foreach(int floorid in CurFightCity.config.floorID)
			{
				Floor floor = Core.Data.dungeonsManager.FloorList[floorid];
				if(!floor.isBoss)
				{
					floor.curProgress = floor.config.wave;
				}			
			}
			
			CityFloorData.Instance.SelectCity(CurFightCity);
			CityFloorData.Instance.UpdateFloorItemsState();
			break;
		//跳转BOSS战
		case EventTypeDefine.Click_GoToBossFight:		
			break;
		//点击执行,进入BOSS战
		case EventTypeDefine.Click_RunBossFight:
			CityFloorData.Instance.ClickFloorItem(Core.Data.dungeonsManager.FloorList[ CityFloorData.Instance.currCity.toFightFloorId ],true);
			Core.Data.guideManger.HideGuide();
			break;
	    //播放战斗
		case EventTypeDefine.Play_FightAnimation:
			Core.Data.guideManger.HideGuide();
			Time.timeScale = 1;
			break;
		//点击返回，<一键>从副本返回游戏主界面
		case EventTypeDefine.Exit_FBtoMainScene:
			if(UICityFloorManager.Instance != null)
			UICityFloorManager.Instance.ClickBack();
			if(SQYChapterController.Instance != null)
			SQYChapterController.Instance.OnBtnBack();
			break;
		//点击返回,从副本主界面返回主界面
		case EventTypeDefine.Exit_MainFB:
			if(JCPVEMainController.Instance != null)
			JCPVEMainController.Instance.OnBtnClick("BackButton");
			break;
		//点击返回,从副本主界面返回主界面，并重置3D场景位置
		case EventTypeDefine.Click_TeamToMainUI2:
			if(JCPVEMainController.Instance != null)
			JCPVEMainController.Instance.OnBtnClick("BackButton");
			Core.Data.guideManger.SetMainSceensDefaultPostion();
			break;		
		 //点击任意地方后，强制返回主界面
		case EventTypeDefine.Click_AnyWhereToMainScene:
			UICityFloorManager.Instance.ClickBack();
			SQYChapterController.Instance.OnBtnBack();
			break;
		//点击神龙
		case EventTypeDefine.Click_ShenLong:
			FinalTrialMgr.GetInstance().m_NowTaskId = Core.Data.guideManger.LastTaskID;
			DBUIController.mDBUIInstance.OnBtnMainViewID(SQYMainController.CLICK_ShenLong);
			break;
		//点击6号龙珠
		case EventTypeDefine.Click_SixStarBall:
			//Debug.Log(UIShenLongManager.Instance.GetDragonBallButton(5).name);
			UIShenLongManager.Instance.GetDragonBallButton(5).qiangDuoDragonBall();
			break;
		//固定只推送一个机器人，点击抢夺按钮
		case EventTypeDefine.Click_RobSixStarBall:
			if(QiangDuoPanelScript.Instance.ListCell.Count>0)
			QiangDuoPanelScript.Instance.ListCell[0].onFight();
			Core.Data.guideManger.HideGuide();
			break;
		//返回龙珠主界面
		case EventTypeDefine.Click_BackToShengLongMain:
			QiangDuoPanelScript.Instance.OnBtnClose();
			break;
		//点击神龙合成按钮
		case EventTypeDefine.Click_CallOfDragon:
			UIShenLongManager.Instance.clickCallDragon();
			break;
		//选择奥义
		case EventTypeDefine.Click_ChooseAoYi:
			UIShenLongManager.Instance.ClickYaoYi1();
			break;
		//召唤神龙以后点击确定
		case EventTypeDefine.Click_YesAtDragonUI:
			UIShenLongManager.Instance.BtnSelectedAoYiOk();
			break;
		//和占卜婆婆说话<跳转打弗利萨>
		case EventTypeDefine.Click_FightWithFulisa:
			Core.Data.guideManger.HideGuide();			

			Object obj = PrefabLoader.loadFromUnPack("Ban/FeatureWar", false);
			GameObject go = GameObject.Instantiate(obj) as GameObject;
			RED.AddChild(go, DBUIController.mDBUIInstance._bottomRoot);

			AsyncTask.QueueOnMainThread( ()=> { SendBattleRequest(); }, 1F);

			break;
	    //点击穿戴奥义
		case EventTypeDefine.Click_EquipAoYi:
			UIDragonAltar d = UIShenLongManager.Instance.dragonAltar;
			if(d !=null && d.aoYiSlotList.Count>0)
			d.aoYiSlotList[0].OnClicked();
			break;
		//选择第一个奥义
		case EventTypeDefine.Click_SelectedFirstAoYi:
			if(UIShenLongManager.Instance.dragonAltar!=null && UIShenLongManager.Instance.dragonAltar.selectAoYiAlert!= null && UIShenLongManager.Instance.dragonAltar.selectAoYiAlert.List_AoYiSlots.Count>0)
			UIShenLongManager.Instance.dragonAltar.selectAoYiAlert.List_AoYiSlots[0].OnClicked();
			break;
	    //点击确定穿戴这个奥义
		case EventTypeDefine.Click_SureEquipAoYi:
			if(UIShenLongManager.Instance.dragonAltar!=null && UIShenLongManager.Instance.dragonAltar.selectAoYiAlert!= null )
				UIShenLongManager.Instance.dragonAltar.selectAoYiAlert.OnSelectAoYi();
			break;
		    //关闭组合技能显示板	
		case EventTypeDefine.Click_CloseCombinationSkillPanel:
			if(CombinationSkillPanelScript.Instance != null)
			    CombinationSkillPanelScript.Instance.DestroyPanel();
			break;
			//获得一个1级红宝石
		case EventTypeDefine.Add_GetLv1RedGem:
		{
			ItemOfReward[] item = new ItemOfReward[]{new ItemOfReward()};
			item[0].num = 1;
			item[0].ppid = -21;
			item[0].pid = 120201;
			Core.Data.itemManager.AddRewardToBag(item[0]);
			GetRewardSucUI.OpenUI(item,Core.Data.stringManager.getString(5047));
		}
		    break;
			 //点击返回，返回主界面(并设置主界面位置)
		case EventTypeDefine.Click_BackTo3DMain:
			TeamUI.mInstance.m_teamView.OnBtnBack ();
			Core.Data.guideManger.SetMainSceensDefaultPostion();
			break;
			//打开装备锻造屋
		case EventTypeDefine.Click_OpenFrogingSystem:
			FrogingSystem.ForgingRoomUI.OpenUI ();
			DBUIController.mDBUIInstance.HiddenFor3D_UI ();
			break;
			//打开宝石合成界面
		case EventTypeDefine.Click_OpenGemMosaic:
			FrogingSystem.ForgingRoomUI.Instance.GoTo(FrogingSystem.ForgingPage.Forging_Mosaic);
			break;
			//点击添加按钮
		case EventTypeDefine.Click_AddEquipment:
			FrogingSystem.ForgingRoomUI.Instance.InlaySystem.ButtonClick("Equipment");
			break;
		case EventTypeDefine.Click_LeftGemSlot:
			FrogingSystem.ForgingRoomUI.Instance.InlaySystem.ButtonClick("Btn_Hole5");
			break;
			//任务弱引导(点击了主线任务按钮)
		case EventTypeDefine.Click_MainLineTask:
			UITask.Instance.OnBtnClick("Btn_MainLine");
			break;
			//任务弱引导(点击了Go按钮)
		case EventTypeDefine.Click_TaskGoBtn:
			UITask.Instance.OnBtnClick("Btn_Jump");
			break;
			//显示太阳和布尔玛
		case EventTypeDefine.Click_ShowSunAndBuErMa:
			if(DBUIController.mDBUIInstance!=null)
			{
				DBUIController.mDBUIInstance.ShowFor2D_UI();
			    DBUIController.mDBUIInstance.StartCoroutine("CheckSunState");
			}
			break;
		case EventTypeDefine.Add_GetJinDouYun:  //龟仙人送金箍棒
			{
			    Core.Data.guideManger.HideGuide();
			     ChAnimUI5.OpenUI(DBUIController.mDBUIInstance._TopRoot).OnFinished = () =>
				{
				    if( Core.Data.itemManager.GetBagItem(-10) == null)
					{
						ItemOfReward[] item = new ItemOfReward[]{new ItemOfReward()};
						item[0].num = 1;
						item[0].ppid = -10;
					    item[0].lv = 1;
						item[0].pid = 45108;
						Core.Data.itemManager.AddRewardToBag(item[0]);
						GetRewardSucUI.OpenUI(item,Core.Data.stringManager.getString(5047));
					}
			     };
			}
			break;
		case EventTypeDefine.Click_JuQingFB:
			if(JCPVEMainController.Instance != null)
			JCPVEMainController.Instance.OnBtnClick("PVEType_Plot");
			break;
		case EventTypeDefine.Click_FirstGuanKa:
		    {
			    Core.Data.guideManger.HideGuide();
				Core.Data.usrManager.UserConfig.cartoon = 1;
				if(JCPVEPlotController.Instance != null )
			    {
				     Core.Data.guideManger.HideGuide();
					 JCPVEPlotController.Instance.OnBuildingClick("60101");		        
			    }			
		    }
			break;
		case EventTypeDefine.Click_FightDesButton:
             if(JCPVEPlotDes.Instance != null)
				JCPVEPlotDes.Instance.OnBtnClick("Btn_Fight");
			break;
		case EventTypeDefine.Click_FightButton:
            if(FightRoleSelectPanel.Instance != null)
				FightRoleSelectPanel.Instance.OnBtnClick("Btn_Fight");
			Core.Data.guideManger.uiGuide.HideGuide();
			Core.Data.temper.TempTouch = true;
			//Core.Data.guideManger.uiGuide.hand.gameObject.SetActive(false);
			break;
		case EventTypeDefine.Click_SecondGuanKa:
		    {
				if(JCPVEPlotController.Instance != null )
					JCPVEPlotController.Instance.OnBuildingClick("60102");
		    }
			break;
		case EventTypeDefine.Click_ExitPlotFB:
			{
			    if(JCPVEPlotController.Instance != null)
				JCPVEPlotController.Instance.OnBtnClick("Btn_Close");
			}
			break;
		case EventTypeDefine.Click_ThirdGuaKa:
			if(JCPVEPlotController.Instance != null )
					JCPVEPlotController.Instance.OnBuildingClick("60103");
			break;
		case EventTypeDefine.Click_FourthGuaKa:
			if(JCPVEPlotController.Instance != null )
					JCPVEPlotController.Instance.OnBuildingClick("60104");
			break;
		case EventTypeDefine.Click_SelectSecondRoleAtFightPanel:
			if(FightRoleSelectPanel.Instance != null)
			    FightRoleSelectPanel.Instance.OnBtnClick("all_1");
			break;	
		case EventTypeDefine.Click_SelectFristRoleAtFightPanel:
			if(FightRoleSelectPanel.Instance != null)
			{
			    FightRoleSelectPanel.Instance.OnBtnClick("all_0");
				AsyncTask.QueueOnMainThread(Core.Data.guideManger.AutoRUN,0.5f);
			}
			break;
		case EventTypeDefine.Click_OpenTaskPage1:   //第一次打开任务面板
			GetTaskList.count = 0;
			UITask.Open(UITaskType.MainLine);
			break;
		case EventTypeDefine.Click_OpenTaskPage2:   //第二次打开任务面板
			GetTaskList.count = 1;
			//UITask.Open(UITaskType.MainLine);
			TopMenuUI.mInstance.OnBtnTask();
			break;
		case EventTypeDefine.Click_CloseTaskPage:   //关闭任务面板
			UITask.Instance.OnBtnClick("Btn_Close");
			break;
		case EventTypeDefine.Click_GetTaskRewardButton:  //领取奖励按钮
			UITask.Instance.OnBtnClick("Btn_GetReward");
			Core.Data.guideManger.HideGuide();
			break;
		case EventTypeDefine.Click_JuQingFB_Special: //重要: 点击剧情副本，但这里是特殊处理<因为这里会影响下一步到底是点第四关还是点第五关(默认是第四关)>
			if(Core.Data.newDungeonsManager.lastFloorId == 60104)
			{
				GuideData nextData = null;
				if(Core.Data.guideManger.ConfigData.TryGetValue(data.ID+1,out nextData))
				{
					nextData.ZoomX = 1f;
					nextData.ZoomY = 1f;
					nextData.MaskX = -182f;
					nextData.MaskY = -220f;
					nextData.TaskID = 155;
				}
			}
			if(JCPVEMainController.Instance != null)
				JCPVEMainController.Instance.OnBtnClick("PVEType_Plot");
			break;
		case EventTypeDefine.Click_FifthGuaKa://点击第五关
			if(JCPVEPlotController.Instance != null )
				JCPVEPlotController.Instance.OnBuildingClick("60105");
			break;
		}



#if NewGuide
		NewUIGuide uiGuide = NewUIGuide.Instance;
#else
		UIGuide uiGuide = UIGuide.Instance;
#endif

		if(data.AutoNext == 1)
		{
			if(data.Dialogue == "null")
				uiGuide.CompleteShelter();
			else
				uiGuide.HideGuide();
		}
		
	}
	
	
	void SendBattleRequest() 
    {
		ComLoading.Open();
        HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
        task.AppendCommonParam(RequestType.FIGHT_FULISA, new BattleParam(Core.Data.playerManager.PlayerID, -1, 0, 0));
        task.ErrorOccured += HttpResp_Error;
        task.afterCompleted += BattleResponseFUC;
        task.DispatchToRealHandler();
    }
		
	void HttpResp_Error (BaseHttpRequest request, string error) 
    {
		ComLoading.Close();
	}
	
	void JumpToBattleView() 
    {
        BattleToUIInfo.From = RUIType.EMViewState.MainView;
		Core.Data.temper.CitySence = 4;
		Core.SM.beforeLoadLevel(Application.loadedLevelName, SceneName.GAME_BATTLE);
        AsyncLoadScene.m_Instance.LoadScene(SceneName.GAME_BATTLE);
    }
	
	void BattleResponseFUC (BaseHttpRequest request, BaseResponse response) 
    {
        ComLoading.Close();
        if(response != null)
        {
            if(response.status!=BaseResponse.ERROR)
            {
                BattleResponse r = response as BattleResponse;
				if(r != null && r.data != null && r.data.reward != null)Core.Data.playerManager.RTData.curVipLevel = r.data.sync.vip;

				r.data.battleData.rsty = null;
				r.data.battleData.rsmg = null;
                Core.Data.temper.warBattle = r.data;
				Core.Data.temper.currentBattleType = TemporyData.BattleType.FightWithFulisa;
                //跳转至Ban 的场景
                JumpToBattleView();
            }
            else
            {
                RED.LogError (response.errorCode.ToString());
            }
        }
    }
	
	
	
	
	
	
	
}
