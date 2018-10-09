using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class GuideManager : Manager
{
	//是否可以点击引导UI
	public bool CanClickGuideUI = true;
	//任务系统弱引导触发等级
	public int TaskSystemWeekGuideTiggerLevel = 10;
	
	/*最近的完成的任务ID    -1:从未做过    0:全部做完       Other:已完成到某个阶段
	 * */
	private int lastTaskID = 0;
	
	//我们还差一个龙珠就可以召唤神龙啦，看看其他玩家手里有没有。神龙对话后抢夺
#if NewGuide
	public  int ShowDialogueBeforeRob = 800007;
#else
	public  int ShowDialogueBeforeRob = 600007;
#endif

	//新手引导是否正在同步中
	public bool isDataSyncing = false;

	public int LastTaskID {
		get{return lastTaskID;}
		set
		{
//			if(value == 500005)
//			{
//				Debug.LogError("ddddddddd");
//			}
			lastTaskID = value;
		}
	}
	//触发隐藏引导时记得之前的任务进度
	public int OldLastTaskID{get;set;}
	
	public Dictionary<int,GuideData> ConfigData = null;
	public GuideData CurTask {get;set;}
	public GuideListener listener;

#if NewGuide
	public NewUIGuide uiGuide = null;
#else
	public UIGuide uiGuide = null;
#endif
	public bool isFristGuide {get;set;}

	public System.Action OnGuideFinished;


	//等级引导ID
	public int LevelGuideID{get;set;}
	//特殊引导ID
	public int SpecialGuideID{get;set;}

	void ParsingServerData(int ServerID)
	{
		if(ServerID != -1)
		{
			//等级引导ID
			LevelGuideID = (ServerID  & 0xFF ) * 100000 ;
			//特殊引导ID
			SpecialGuideID = (ServerID >> 8) * 1000;
		}
		else
		{
			LevelGuideID = -1;
			SpecialGuideID = 0;
		}
	}
	
	//转化服务器ID   301000    4864
	public int GetServerID(int GuideID)
	{
		int S = (GuideID / 100000);         
		int SS = (GuideID % 100000  /1000) << 8;
		return   S | SS;
	}

	public GuideManager () 
	{
		listener = new GuideListener();
		ConfigData = new Dictionary<int, GuideData>();
	}
	
	public override bool loadFromConfig () 
	{
		bool result = base.readFromLocalConfigFile<GuideData>(ConfigType.Guide, ConfigData);
		//DubugPrint();
		return result;
	}
	
	public override void fullfillByNetwork (BaseResponse response) 
	{
		if(response != null && response.status != BaseResponse.ERROR)
		{
			LoginResponse loginResp = response as LoginResponse;
			if(loginResp.data !=null)
			{
				//解析服务器字段
				ParsingServerData(loginResp.data.user.guide);

				lastTaskID = LevelGuideID;
				isFristGuide = false;
				if(LastTaskID == -1)
				{
					isFristGuide = true;
					LastTaskID = 100000;
				}
				else if(LastTaskID > 0) //走下一个点
					LastTaskID += 100000;
				
				LastTaskID = LastTaskID/100000*100000;

				//Debug.LogError("LastTaskID="+LastTaskID.ToString()+"     SpecialGuideID="+SpecialGuideID.ToString());

#if  NOGUIDE
				LastTaskID= 0;
#endif			
			}
		}
	}
	
	
	public void DubugPrint()
	{
		foreach(GuideData data in ConfigData.Values)
		{
			Debug.Log(data.ID.ToString()+"    "+data.Multi.Count.ToString()+"      "+ data.MaskType.ToString());
		}
	}
	
	
	/*自动执行下一个任务
	 * */
	public void AutoRUN ()
	{
		if(uiGuide == null ) return;

		GuideData LastTaskData = null;
		if(ConfigData.TryGetValue(LastTaskID, out LastTaskData))
		{
			if(LastTaskData != null && LastTaskData.JumpStep != "null") {
				string[] str = LastTaskData.JumpStep.Split('_');
				switch(str[1])
				{
					//无判定条件
				case "0":
					LastTaskID += (int)System.Convert.ToInt32(str[0]);
					AutoSendMsgAtLastTask();
					break;
					//是否玩家已穿戴了筋斗云
				case "1":
					if(Core.Data.EquipManager.IsEquiped(45108))
						LastTaskID += (int)System.Convert.ToInt32(str[0]);
					break;
					//是否已签到
//				case "2":
//					if(!ActivityManager.canGet)
//			             LastTaskID += (int)System.Convert.ToInt32(str[0]);
//					break;
				case "3":
					if(JCPVEPlotController.Instance == null)
			              LastTaskID += (int)System.Convert.ToInt32(str[0]);
					break;
				}
			}
		}
		
	    if(uiGuide != null && CheckGuideTrigger)
		{
			uiGuide.SetUI(CurTask);
		}
	}
	
	//自动在最后一个任务被点击的时候,发送同步信息
	public void AutoSendMsgAtLastTask()
	{
		if(isLastOfCurGuide)
		{
			Debug.Log("Guide is Finish and I should delete it's gameobject .");
			try 
			{
				if(uiGuide != null)
				{
					uiGuide.DestoryGuide();
					uiGuide = null;
					if(OnGuideFinished != null)
						OnGuideFinished();
				}
			}
			catch (System.Exception ex) 
			{
				Debug.LogError(ex.ToString());
			}

			SendRecastMsg();
		}
	}
	
	void SendRecastMsg(bool resend = false)
	{
		//如果当前走的是特殊条件引导
		Send_GuideFinish param = new Send_GuideFinish ();
		param.gid = Core.Data.playerManager.PlayerID;

#if NewGuide
		param.isNew = 1;
#endif


		if(OldLastTaskID > 0)
		{
			if(!resend)
			OldLastTaskID -=100000;			
			int temp = OldLastTaskID/100000*100000+LastTaskID/1000*1000;
			param.step = GetServerID( temp );
			SpecialGuideID = 0;
		}
		else
		{
			param.step = GetServerID( LastTaskID/100000*100000 );
		}
		param.sync = 100;

		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.GuideProgress, param);

		task.ErrorOccured += HttpResp_Error;
		task.afterCompleted += HttpRespSucess;
        task.DispatchToRealHandler ();

		isDataSyncing = true;
		
		ComLoading.Open(Core.Data.stringManager.getString(9126));
	}
	//同步数据异常
	void HttpResp_Error(BaseHttpRequest request, string error)
	{
		SendRecastMsg(true);
	}
	//同步数据成功
    public void HttpRespSucess(BaseHttpRequest request, BaseResponse response)
	{
		isDataSyncing = false;

		isFristGuide = false;
		ComLoading.Close();
		
		if(DBUIController.mDBUIInstance != null)
		{
			DBUIController.mDBUIInstance.RefreshUserInfoWithoutShow();
		}
		
		  if (response.status != BaseResponse.ERROR)
          {		
               HttpRequest rq = request as HttpRequest;
               Send_GuideFinish param = rq.ParamMem as Send_GuideFinish;

				//等级引导ID
			    int tempLevelGuideID = (param.step  & 0xFF ) * 100000 ;
				//特殊引导ID
			    int tempSpecialGuideID = (param.step  >> 8) * 100;

				if(tempSpecialGuideID == 0 )
				{
					if(tempLevelGuideID == 100000)		
				    {
					     LastTaskID = 200000;
					     Init();
				    }
#if NewGuide
					else if(tempLevelGuideID == 400000)
					{
					     LastTaskID = 500000;
					     Init ();
					}
#endif
				    else
					LastTaskID = tempLevelGuideID + 100000;
			    }
				else if(tempSpecialGuideID > 0)
				{
			        LastTaskID = tempLevelGuideID +100000;
					OldLastTaskID = 0;
				   
					if(Core.Data.AccountMgr.UserConfig.mPreLevel >0)
					{
						 Core.Data.temper.mPreLevel = Core.Data.AccountMgr.UserConfig.mPreLevel;
					     DBUIController.mDBUIInstance.AutoDealLevelUpUI();
					}

			    }
         }


	}	



	public void DelayInit(int time) {
		Thread.Sleep(time*1000);
		Init();
	}
	
    public void Init() 
	{		
		if(isGuiding)
		{		
			if(SpecialGuideID > 0)
			{				
#if NewGuide
				if(SpecialGuideID == 1000)
					SpecialGuideID = 0;
#endif
				if( TriggerHideGuide(SpecialGuideID) )
				   SpecialTaskProcessing(CurTask.ID);

			}
			else
			{	
				if(uiGuide == null)
				{
#if NewGuide
					uiGuide =  NewUIGuide.Instance;
#else
					uiGuide = UIGuide.Instance;
#endif
					AutoRUN();
					SpecialTaskProcessing(CurTask.ID);
					// ---- 统计数据 ----
		            string tStr = " guide = " + CurTask.ID;
		            Core.Data.ActivityManager.OnMissionBegin (tStr);			
				}
			}
		}
	}
	
	
	
	
	
	//触发缘配齐的引导(此功能独立)
	public void TriggerFateGuide()
	{
#if NewGuide
		uiGuide = NewUIGuide.Instance;
#else
		uiGuide = UIGuide.Instance;
#endif
		GuideData FateTask = new GuideData();
		FateTask.ArrowDir = 3;
		FateTask.AutoNext = 1;
		FateTask.Dialogue = Core.Data.stringManager.getString(9030);
		FateTask.MaskX = 0;
		FateTask.MaskY = -620f;
		FateTask.ZoomX = 1;
		FateTask.ZoomY = 1;
		FateTask.ID = -1;
		FateTask.Sound = 389;
		FateTask.TaskID = -1;
		FateTask.Operation = 0;
		
		CurTask = FateTask;
		uiGuide.SetUI(CurTask);
	}

	public void SetMainSceensDefaultPostion()
	{
		if(Allen_TouchRoll.Instance!=null)
		{
			Allen_TouchRoll.Instance.transform.localPosition = new Vector3(-14.03704f,-82.93933f,71.01974f);
			Allen_TouchRoll.Instance.transform.localRotation =  Quaternion.Euler(30f,0,30f);
		}	
	}
	



	//特殊任务处理
	void SpecialTaskProcessing(int TaskID)
	{
		switch(TaskID)
		{
		case 100001:
			SetMainSceensDefaultPostion();	
			break;
		case 200001:		
#if !NewGuide
			if(JCPVEPlotController.Instance == null)
			{
			if(SQYMainController.mInstance != null)
			   SQYMainController.mInstance.OnBtnFuBen();
			}
			break;
#endif
		case 300001:
		case 400001:
		case 500001:	
#if NewGuide
		case 600001:
		case 700001:
		case 800001:
#endif
		case 1001:
		case 2001:
			if(SQYMainController.mInstance != null)
			   SQYMainController.mInstance.OnBtnFuBen();
			if(JCPVEMainController.Instance != null)
				JCPVEMainController.Instance.OnBtnClick("PVEType_Plot");
			break;
#if !NewGuide
		case 600001:
				if (Core.SM.LastScenesName == SceneName.LOGIN_SCENE && JCPVEPlotController.Instance == null)
			    {
					if(SQYMainController.mInstance != null)
			           SQYMainController.mInstance.OnBtnFuBen();
				}
			break;
#endif
		case 3001:
			if(SQYMainController.mInstance != null)
			   SQYMainController.mInstance.OnBtnFuBen();
			if(JCPVEMainController.Instance != null)
				JCPVEMainController.Instance.OnBtnClick("PVEType_Skill");
			break;
		}
	}
	
	
	//触发隐藏引导(特殊条件引导触发逻辑)
	public bool TriggerHideGuide(int HideTaskID)
	{
		GuideData guidedata = null;
		bool result = false;
		result = ConfigData.TryGetValue(HideTaskID+1,out guidedata);
		if(result)
		{
			if(guidedata.Type == 1)
			{
				if(Core.Data.playerManager.RTData.curLevel >= guidedata.NeedLevel)
					result = true;
				else
					return false;
				OldLastTaskID = LastTaskID;
			    LastTaskID = HideTaskID;
#if NewGuide
				uiGuide =  NewUIGuide.Instance;
#else
				uiGuide = UIGuide.Instance;
#endif
				AutoRUN();				
			}
		}
		return result;
	}
 
	/*检测是否触发引导
	 * */
	bool CheckGuideTrigger
	{
		get
		{
			if(LastTaskID == 0)
				return false;
			
			CurTask = null;
			int TaskID = LastTaskID + 1;
			/*如果任务存在
			 * */
			if(ConfigData.ContainsKey(TaskID) && ConfigData[TaskID].Type != 1 && Core.Data.playerManager.RTData.curLevel == ConfigData[TaskID].NeedLevel)
			{				
				//主线任务必须当前等级和需求等级一致
				CurTask = ConfigData[TaskID];
				LastTaskID = TaskID;
				return true;
			}
			else if(ConfigData.ContainsKey(TaskID) && ConfigData[TaskID].Type == 1 && Core.Data.playerManager.RTData.curLevel >= ConfigData[TaskID].NeedLevel)
			{
				//弱引导等级>=需求等级就可以进行
				CurTask = ConfigData[TaskID];
				LastTaskID = TaskID;
				return true;
			}
			else
			{
				if(ConfigData.ContainsKey(TaskID) && ConfigData[TaskID].Type != 1)
				{
					/*可能是下一阶段的任务*/
					TaskID = LastTaskID/100000*100000+100000+1;
					if(ConfigData.ContainsKey(TaskID) && Core.Data.playerManager.RTData.curLevel == ConfigData[TaskID].NeedLevel)
				    {
						CurTask = ConfigData[TaskID];
					    LastTaskID = TaskID;
						return true;
				    }
				}
			}		
			return false;
		}
	}
	
	/*当前是否正在新手教学(是否可以触发新手教学)    <特殊触发的引导不在此列>
	 * */
	public bool isGuiding
	{
		get
		{	
             if(LastTaskID == 0)
				return false;
			if(!Core.Data.playerManager.RTData.IsRegister)
				return false;
			
			if(SpecialGuideID > 0)
				return true;
			
			int TaskID = LastTaskID + 1;
			/*如果任务存在
			 * */
			GuideData tempData = null;
			ConfigData.TryGetValue(TaskID,out tempData);

			if(tempData !=null && tempData.Type !=1 && Core.Data.playerManager.RTData.curLevel == tempData.NeedLevel)
			{
				return true;
			}
			else if(tempData != null && tempData.Type ==1 && Core.Data.playerManager.RTData.curLevel >= tempData.NeedLevel)
			{
				return true;
			}
			else
			{
				if(tempData !=null && tempData.Type !=1)
				{
					/*可能是下一阶段的任务*/
					TaskID = LastTaskID/100000*100000+100000+1;
					if(ConfigData.ContainsKey(TaskID) && Core.Data.playerManager.RTData.curLevel == ConfigData[TaskID].NeedLevel)
				    {
						return true;
				    }
				}
			}	
			
			return false;
		}
	}
	
	
	//是否是当前引导的最后一个
	public bool isLastOfCurGuide
	{
		get
		{
			if(LastTaskID == 0)
				return false;
			int TaskID = LastTaskID + 1;
			return !ConfigData.ContainsKey(TaskID);
		}
	}
	
	
	public bool isAskServer
	{
		get
		{
			GuideData task = null;
			if(ConfigData.TryGetValue(LastTaskID,out task))
			{
				if(task.RequestServer == 1)
				    return true;
				else
					return false;
			}
			else
				return false;
		}
	}

	
	public void Clear()
	{
		lastTaskID = 0;
		UIGuide.SafeDestroy();
	} 

	public void DelayAutoRun(float second,bool pause = false, System.Action callback = null)
	{
#if NewGuide
		AsyncTask.QueueOnMainThread( AutoRUN ,second);
#else
		uiGuide.DelayAutoRun(second,pause,callback);
#endif
	}


	public void HideGuide()
	{
		uiGuide.HideGuide();
	}

}
