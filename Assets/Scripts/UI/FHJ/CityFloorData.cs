using UnityEngine;
using System.Collections.Generic;

public class CityFloorData
{
    private static CityFloorData _instance;
    public static CityFloorData Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new CityFloorData();
                _instance.Init();
            }
            return _instance;
        }
    }
	
	private bool IsGuideClick{get;set;}
	
    void Init()
    {
        FloorTextureManager.LoadExist();
    }

    UICityFloorManager cityFloorManager;
	
    public Chapter chapter{get; set;}

    public City currCity
    {
        get;
        protected set;
    }

    public Floor currFloor
    {
        get;
        protected set;
    }
    
    public List<City> citys = null;
    public List<Floor> floors = null;
	/*创建CityFloor所有元素
	 * */
    public void ShowCityFloorView(Chapter _chapter, GameObject root)
    {
		if(Core.Data.temper != null && Core.Data.playerManager.Lv  > Core.Data.temper.mPreLevel)
		{
			LevelUpUI.OpenUI();
			Core.Data.temper.mPreLevel = Core.Data.playerManager.Lv;
			Core.Data.temper.mPreVipLv = Core.Data.playerManager.curVipLv;
		}
		
        if(cityFloorManager == null)
        {
#if FB2
			//FhjLoadPrefab.GetPrefab("pbUICityFloorManager2")
			cityFloorManager =(UnityEngine.Object.Instantiate(FhjLoadPrefab.GetPrefab("pbUICityFloorManager2")) as GameObject )  .GetComponent<UICityFloorManager>();		
#else	 
			cityFloorManager = FhjLoadPrefab.GetPrefabClass<UICityFloorManager>();
#endif   
            cityFloorManager.OnClickedCityItem = ClickCityItem;
            cityFloorManager.OnClickedFloorItem = ClickFloorItem;
			cityFloorManager.onBossDesInfoClick = ClickBossDesInfo;
            Transform t = cityFloorManager.transform;
            t.parent = root.transform;
            t.localPosition = Vector3.back * 10;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
        }

        DungeonsManager dm = Core.Data.dungeonsManager;
        chapter = _chapter;
        if(chapter != null)
        {
            citys = new List<City>();
            foreach(int cityId in chapter.config.cityID)
            {
                citys.Add(dm.CityList[cityId]);
            }
        }
        cityFloorManager.UpdateChapterName(chapter.config.name);
        cityFloorManager.InitCityItems(_chapter.config.cityID);
		

		if(chapter.toFightCity !=null)
		{
			SelectCity(chapter.toFightCity);
		}
		else
		{
            SelectCity(dm.CityList[chapter.toFightCityId]);
		}
		
        UpdateCityItemsState();
        UpdateFloorItemsState();	
		
		
		
    }

    public bool SelectCity(City _city)
    {
		_city.isOpen = true;
		
		if(Core.Data.dungeonsManager.CityList.ContainsKey(_city.config.ID))
		_city.toFightFloorId = Core.Data.dungeonsManager.CityList[_city.config.ID].toFightFloorId;		
		
		
		//Debug.Log(">>>>>>>>>>>>>city["+_city.config.ID.ToString()+"].toFightFloorId="+_city.toFightFloorId.ToString());
		
        currCity = _city;
        DungeonsManager dm = Core.Data.dungeonsManager;
        floors = new List<Floor>();
        foreach(int floorId in currCity.config.floorID)
        {
            floors.Add(dm.FloorList[floorId]);
        }

		if(cityFloorManager != null)
		{
        	UpdateCityItemsState();
			int index = GetCityIndex(currCity);
			cityFloorManager.JumpToCity(index, citys.Count);
		}

        if(currCity.toFightFloorId == 0)
		{
            currCity.toFightFloorId = floors[0].config.ID;
//			Debug.Log("JC---->SelectCity---->"+currCity.toFightFloorId.ToString());
		}
		if(cityFloorManager != null)
		{
       	 	cityFloorManager.InitFloorItems(floors, currCity.toFightFloorId);
        	StartFloor(GetFloor(currCity.toFightFloorId));
		}

        return true;
    }

	int GetCityIndex(City ct)
	{
		for(int i = 0; i < citys.Count; i++)
		{
			if(ct.toFightFloorId == citys[i].toFightFloorId)
			{
				return i;
			}
		}
		return -1;
	}

    public int StartFloor(Floor _floor)
    {
        cityFloorManager.UpdateFloorName(_floor.config.name);
		
#if FB2
#else
		if(_floor.isBoss)
		cityFloorManager.ShowTex(_floor.config.ID.ToString() + "-1.jpg");
		else
        cityFloorManager.ShowTex(_floor.config.ID.ToString() + "-" +(_floor.curProgress+1).ToString()+".jpg");
#endif	
		
        return _floor.config.ID;
    }

    void UpdateCityItemsState()
    {
		//Debug.Log("currCityID="+currCity.config.ID.ToString()+"     maxCityId="+Core.Data.dungeonsManager.fightCityId.ToString()+"   LastFloorID="+Core.Data.dungeonsManager.lastFloorId.ToString());
		
        cityFloorManager.UpdateCityItems(currCity.config.ID, Core.Data.dungeonsManager.fightCityId);
    }
    
    public void UpdateFloorItemsState()
    {
		if(cityFloorManager != null)
		{
			cityFloorManager.UpdateFloorItems(Core.Data.dungeonsManager.lastFloorId,currCity.toFightFloorId);
		}
	
//		Debug.Log("jc=======>dungeonsManager.lastFloorId="+Core.Data.dungeonsManager.lastFloorId.ToString()+"    currCity.toFightFloorId="+currCity.toFightFloorId);
    }



	public void SyncCity(City city)
	{
		SelectCity(city);
		UpdateFloorItemsState();
	}

	
	/*点击标题组按钮
	 * */
    void ClickCityItem(City _data)
    {
        if(_data != currCity)
        {
            SelectCity(_data);
			//Debug.Log("====jc===>floors(list).count="+floors.Count+"    floorId="+currCity.toFightFloorId);
            cityFloorManager.InitFloorItems(floors, currCity.toFightFloorId);
            UpdateFloorItemsState();
        }
    }

    Floor GetFloor(int id)
    {
        if(floors != null)
        {
            foreach(Floor item in floors)
            {
                if(item.config.ID == id)
                {
                    return item;
                }
            }
            return floors[0];
        }
        return null;
    }
	
	
	void ClickBossDesInfo(int num)
	{
		switch(num/10000)
		{
		case 1:
			Monster m = new Monster ();
		    m.num = num;
			m.InitConfig();
			JCMonsterDesInfoUI.OpenUI( m,true);
			break;
		case 4:
			Equipment e = new Equipment();
			EquipData equip= Core.Data.EquipManager.getEquipConfig(num);
			e.ConfigEquip = equip;
			JCEquipmentDesInfoUI.OpenUI(e);
			break;
		}
		
	}
	
	
	Floor CurClickFloor = null;
	
	//isGuideClick = true:新手引导调用的
	
	public bool isCanClick = true;
    public void ClickFloorItem(Floor _data,bool isGuideClick = false)
    {
		CurClickFloor = _data;
		IsGuideClick = isGuideClick;
		//Debug.Log("Click to here!!!");
		if(Core.Data.playerManager.RTData.curJingLi < _data.config.needeEnergy) 
		{
			isCanClick = true;
			JCRestoreEnergyMsg.OpenUI(110015,110016);
		}
		else
		{
			    
			    currFloor = _data;
		        if(_data.config.isBoss > 0)
		        {
				    if(!_data.isfree && !isGuideClick)
				    {
					     if(Core.Data.playerManager.RTData.curStone < 10)
					     {
						    isCanClick = true;
						    UIInformation.GetInstance().SetInformation(Core.Data.stringManager.getString(9021),Core.Data.stringManager.getString(5030),UIInformationSure);
					     }
					     else
						    SendBattleRequest();
				    }
				    else
		            SendBattleRequest();
		        }
		        else
		        {
		            SendSmallBattleRequest();
		        }
		}
    }
	
	//打BOSS关宝石不足弹出窗,用户选择跳转充值
	void UIInformationSure()
	{
		UIDragonMallMgr.GetInstance().SetRechargeMainPanelActive();
	}
	

    void BattleRequest()
    {
        SendBattleRequest();
    }

    public delegate void runFloor(bool win);
    public runFloor RunFloor = null;

    void NextStep(bool win)
    {
        if(win)
        {
            GoNextFloor();
        }
    }

    public void GoNextFloor()
    {
		/*如果有额外的金币奖励，点击"收下"时会加上*/
		if(FBReword != null)
		{
			if(FBReword.reward.eco > 0)
			{
               // Debug.Log (" go next floor ");
				UIMiniPlayerController.Instance.freshPlayerInfoView();
			}
		}
        Next(currFloor);
        currFloor = null;
    }

    void ShowMag(string title, string msg, bool cancel = false)
    {
		UIInformation.GetInstance().SetInformation(msg, Core.Data.stringManager.getString(5030), ClickOKCallBak);
    }

	void ClickOKCallBak()
	{
		if(RunFloor != null)
		{
			RunFloor(true);
			RunFloor = null;
		}
	}

//    void AlertBehaviour(MDAlertBehaviour ab,MDBaseAlertView alert)
//    {
//        //int alertID = alert.alertID;
//        switch(ab)
//        {
//        case MDAlertBehaviour.CLICK_OK:
//            alert.hiddenView();
//            break;
//        case MDAlertBehaviour.DID_HIDDEN:
//            if(RunFloor != null)
//            {
//                RunFloor(true);
//                RunFloor = null;
//            }
//            GameObject.Destroy(alert.gameObject);
//            break;
//        }
//    }

    void ResetCurrCityState()
    {
        foreach(Floor item in floors)
        {
			//Debug.Log("=========="+item.config.ID+"===========");
            item.status = DungeonsData.STATUS_NEW;
            item.curProgress = 0;
        }
        floors[0].status = DungeonsData.STATUS_ONGOING;
		
		
        currCity.toFightFloorId = floors[0].config.ID;
		//Debug.Log("JC---->FUCN:ResetCurrCityState---->"+currCity.toFightFloorId.ToString());
			
        floors[floors.Count-1].status = DungeonsData.STATUS_CLEAR;
        floors[floors.Count-1].curProgress = floors[floors.Count-1].config.wave;

    }

    void NextCity()
    {
        int index = citys.IndexOf(currCity);
        currCity.status = DungeonsData.STATUS_CLEAR;
        if(index!=-1 && index<citys.Count-1)
        {
            SelectCity(citys[index+1]);
        }
        currCity.status = DungeonsData.STATUS_ONGOING;
        UpdateCityItemsState();
        UpdateFloorItemsState();
    }

    void NextFloor(Floor _data)
    {
        int index = floors.IndexOf(_data);
        Floor _floor = floors[index+1];
		if(!_floor.isBoss)
        _floor.curProgress = 0;
        currCity.toFightFloorId = _floor.config.ID;

		/*首次解锁BOSS关，会有一个特效*
		  * */
       
        if( cityFloorManager.bossItem.state == UIFloorItem.FloorItemState.Unlocked && Core.Data.dungeonsManager.FloorList[CityFloorData.Instance.currCity.toFightFloorId].isBoss == true)
	   {
            //Debug.Log(" boss ");
			cityFloorManager.onCityJumpFinished=()=>
			{
				cityFloorManager.bossItem.AddTweenAniTOType();
			};
	   }
        cityFloorManager.JumpToFloor(index+1);

        UpdateFloorItemsState();
		
    }
	
    public void Next(Floor _data)
    {
#if FB2

#else
		string imageName=_data.config.ID.ToString()+"-"+(_data.curProgress+1).ToString();
		if(_data.curProgress>=_data.config.wave)
		{
			int index = floors.IndexOf(_data);
            Floor _floor = floors[index+1];
			imageName = _floor.config.ID.ToString()+"-"+(_floor.curProgress+1).ToString();
		}
		imageName+=".jpg";
		//Debug.Log("--------"+imageName+"---------");
		cityFloorManager.ShowTex(imageName);
#endif
		
        cityFloorManager.UpdatePlayerInfo();

        if(_data.config.isBoss > 0)
        {
            if(_data.curProgress < _data.config.wave)
            {
                UpdateFloorItemsState();
            }
            else
            {
                //ResetCurrCityState();
                NextCity();
            }
        }
        else
        {
            if(_data.curProgress < _data.config.wave)
            {
                UpdateFloorItemsState();
            }
            else
            {
                NextFloor(_data);
            }
        }
    }


    
    
    void SendSmallBattleRequest()
    {
		ComLoading.Open();
		//added by zhangqiang ao rember level
		Core.Data.temper.mPreLevel = Core.Data.playerManager.RTData.curLevel;
		Core.Data.temper.mPreVipLv = Core.Data.playerManager.RTData.curVipLevel;

        HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
        //HttpTask task = new HttpTask(ThreadType.MainThread); is the same as above line.
        //fhj 请求网络 需要换ID
        task.AppendCommonParam(RequestType.SM_PVE_BATTLE, new BattleParam(Core.Data.playerManager.PlayerID, currFloor.config.ID, currCity.config.ID, chapter.config.ID));
        
        task.ErrorOccured += HttpResp_Error;
        task.afterCompleted += SmallBattleResponse;
        
        //then you should dispatch to a real handler
        task.DispatchToRealHandler();
    }
	
	/*副本奖励*/
	private BattleSequence FBReword = null;
    void SmallBattleResponse(BaseHttpRequest request, BaseResponse response)
    {
        ComLoading.Close();
		FBReword = null;
		if (response != null && response.status != BaseResponse.ERROR)
        {
			BattleResponse r = response as BattleResponse;		
			FBReword = r.data;
			cityFloorManager.ShowReward(FBReword, GoNextFloor);
			//如果是新手引导点击的
			if(IsGuideClick)Core.Data.guideManger.DelayAutoRun(1.8f);			
        }
        else
        {
			isCanClick = true;
			if(response.errorCode == 4002)
			JCRestoreEnergyMsg.OpenUI(110015,110016);
			else
            ErrorDeal(response.errorCode);
        }
		
		//UIMiniPlayerController.Instance.freshPlayerInfoView ();
		//DBUIController.mDBUIInstance.RefreshUserInfo ();
    }
	


    
    void SendBattleRequest() 
    {
		ComLoading.Open();
		//added by zhangqiang ao rember level
		if(Core.Data.playerManager.RTData.curTeam.validateMember == 0)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(5031));
			ComLoading.Close();
			return;
		}
		
		Core.Data.temper.mPreLevel = Core.Data.playerManager.RTData.curLevel;
		Core.Data.temper.mPreVipLv = Core.Data.playerManager.RTData.curVipLevel;

        HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);

		
		int flag =0 ;
		if(CurClickFloor!=null)flag = System.Convert.ToInt32( !CurClickFloor.isfree );
		
		
		//检测该关卡是否是这一章节的最后一个关卡
		bool isLastFloorOfChapter = Core.Data.dungeonsManager.checkLastCity(currCity.config.ID);

		task.AppendCommonParam(RequestType.NEW_PVE_BOSSBATTLE, new ClientBattleParam(Core.Data.playerManager.PlayerID, currFloor.config.ID, currCity.config.ID, chapter.config.ID, flag, isLastFloorOfChapter ? 1 : 0, Core.Data.guideManger.isGuiding ? 1 : 0 ));
        //task.AppendCommonParam(RequestType.PVE_BATTLE, new BattleParam(Core.Data.playerManager.PlayerID, currFloor.config.ID, currCity.config.ID, chapter.config.ID, flag, isLastFloorOfChapter ? 1 : 0, Core.Data.guideManger.isGuiding ? 1 : 0 ));
		
		//Debug.Log("send boss atrrick msg!");
        task.ErrorOccured += HttpResp_Error;
        task.afterCompleted += BattleResponseFUC;
        
        //then you should dispatch to a real handler
        task.DispatchToRealHandler();
    }
    
	
	/*点击BOSS关卡<执行>按钮后服务器返回的数据
	 * */
    void BattleResponseFUC (BaseHttpRequest request, BaseResponse response) 
    {
        ComLoading.Close();
        if(response != null)
        {
            TemporyData temp = Core.Data.temper;

            if(response.status!=BaseResponse.ERROR)
            {
                BattleResponse r = response as BattleResponse;

				ClientBattleResponse resp = response as ClientBattleResponse;

				if(r != null) {
					if(r != null && r.data != null && r.data.reward != null && r.data.sync != null) Core.Data.playerManager.RTData.curVipLevel = r.data.sync.vip;

					r.data.battleData.rsty = null;
					r.data.battleData.rsmg = null;
                    temp.warBattle = r.data;

                    temp.currentBattleType = TemporyData.BattleType.BossBattle;

					HttpRequest req = request as HttpRequest;
					BaseRequestParam param = req.ParamMem;
					//BattleResponse res = response as BattleResponse;
					BattleParam bp = param as BattleParam;
					FloorData floorD =	Core.Data.dungeonsManager.getFloorData(bp.doorId);
					if(r.data.battleData.iswin  == 1){
						if(floorD != null)
							Core.Data.ActivityManager.OnMissionComplete(floorD.name);
					}else {
						if(floorD != null)
							Core.Data.ActivityManager.OnMissionFail(floorD.name);
					}
					if(bp.flag == 1){
						//add by wxl 
						Core.Data.ActivityManager.OnPurchaseVirtualCurrency(ActivityManager.ChapterType,1,10);
					}
				} 

				if(resp != null) { 
                    temp.currentBattleType = TemporyData.BattleType.BossBattle;
                    temp.clientDataResp = resp;

					#if LOCAL_AUTO
                    temp.Open_StepMode = false;
					#else
					temp.Open_StepMode = true;
					#endif
                    temp.Open_LocalWarMode = true;

					HttpRequest req = request as HttpRequest;
					if(req != null) {
						ClientBattleParam param = req.ParamMem as ClientBattleParam;
						if(param != null)
                            temp.clientReqParam = param;
					}

				}

                //跳转至Ban 的场景
                JumpToBattleView();
            }
            else
            {
				isCanClick = true;
				if(response.errorCode == 4002)
				JCRestoreEnergyMsg.OpenUI(110015,110016);
				else
                ErrorDeal(response.errorCode);
            }
        }
    }

    void JumpToBattleView() 
    {
        BattleToUIInfo.From = RUIType.EMViewState.S_CityFloor;
		Core.Data.temper.CitySence = currFloor.config.cj;
		Core.SM.beforeLoadLevel(Application.loadedLevelName, SceneName.GAME_BATTLE);
        AsyncLoadScene.m_Instance.LoadScene(SceneName.GAME_BATTLE);
    }
    
    
    void HttpResp_Error (BaseHttpRequest request, string error) 
    {
		isCanClick = true;
        ComLoading.Close();
        //UnityEditor.EditorUtility.DisplayDialog("HttpResp_Error", error , "OK");
        ShowMag("HttpResp_Error", error);
        ConsoleEx.DebugLog("---- Http Resp - Error has ocurred." + error);
    }

    public void ErrorDeal(int errorID)
    {
       ShowMag("ErrorDeal", Core.Data.stringManager.getNetworkErrorString(errorID));
    }
}