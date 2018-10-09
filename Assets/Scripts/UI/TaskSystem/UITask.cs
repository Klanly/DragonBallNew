using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum UITaskType
{
	EveryData,
	MainLine,
	None,
}

/*任务系统UI逻辑层
 * */
public class UITask : MonoBehaviour {
	
	public UITask_View _view;
	public static UITask  Instance;
	
	UITaskType _curTaskType = UITaskType.EveryData;

	//当前选中的每日任务
	[HideInInspector]
	public  UITaskCell CurSelected_EveryDayCell;
	//当前选中的主线任务
	[HideInInspector]
	public  UITaskCell CurSelected_MainLineCell;
	
	public UIGrid uiGrid;
	
	List<TaskData> DayTaskList;	
	List<TaskData> MainTaskList;
	
	private TaskManager tm;
	
	public TweenScale tweenscale;

	public System.Action exitCallBack;
	
	
	//当前任务种类
	UITaskType curTaskType 
	{
	    get{return _curTaskType;}
		set
		{
			_curTaskType = value;
			if(_curTaskType != UITaskType.None)
			Core.Data.taskManager.LastSelectedType = (int)_curTaskType;
		}
	}
	
	
	void Awake()
	{
		if(Instance == null) Instance = this;
		if(_view == null)_view = GetComponent<UITask_View>();		
		DayTaskList = new List<TaskData>();
		MainTaskList = new List<TaskData>();
		tm = Core.Data.taskManager;
	}
	
	void Start ()
	{
	    CreateElements(10);		
		tweenscale.onFinished.Add(new EventDelegate(UIInit));
		tweenscale.PlayForward();
	}
	
	void UIInit()
	{		
		SendTaskListRequest();
	}
	
	public void UIDestory()
	{
		if (exitCallBack != null)
			exitCallBack ();

		Destroy(gameObject);
		Instance = null;
	}
	
	
	//发送任务列表请求
	void SendTaskListRequest()
	{
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.Task_List, new Send_TaskList(Core.Data.playerManager.PlayerID));

		task.ErrorOccured =(BaseHttpRequest b, string error)=>
		{
			ComLoading.Close();
			if(Core.Data.guideManger.isGuiding)
			     Core.Data.guideManger.AutoRUN();
		};
		task.afterCompleted += OnListRequestFinished;

		task.DispatchToRealHandler ();
		ComLoading.Open();
	}
	//服务器任务列表返回
	void OnListRequestFinished(BaseHttpRequest request, BaseResponse response)
	{
		ComLoading.Close();
		
		if(Core.Data.guideManger.isGuiding) Core.Data.guideManger.AutoRUN();

		if (response.status != BaseResponse.ERROR) 
		{	
			TaskListResponse res = response as TaskListResponse;
			foreach(TaskDataResponse data in res.data.tasks)
			{
				TaskData tdata = tm[data.id];
				if(tdata != null)
				{
					tdata.curProgress = data.progress;
					tdata.Progress = data.condition;
					if(tdata.Type == 0)
						DayTaskList.Add(tdata);
					else if(tdata.Type == 1)
						MainTaskList.Add(tdata);
				}
				else
				{
					Debug.LogError("Not find Task["+data.id.ToString()+"]");
				}
			}			
		}

		//如果某种任务完成,优先显示那一类
		if(isHaveCompleteTask(ref MainTaskList) )
			Core.Data.taskManager.LastSelectedType = 1;
		if(isHaveCompleteTask(ref DayTaskList) )
			Core.Data.taskManager.LastSelectedType = 0;
		
		if(curTaskType == UITaskType.None)
		curTaskType =(UITaskType)Core.Data.taskManager.LastSelectedType;
			
		//每日任务列表可能为空,但主线任务列表永远不会为空

		if(DayTaskList.Count == 0)
		{
			_view.SetBtnBright(0,1);
			curTaskType = UITaskType.MainLine;
		}
		
		RefreshTaskList();

	}
	
	

	bool isSendGetRwewarding = false;
	//发送任务奖励请求
	void SendGetRewardRequest()
	{
		if(isSendGetRwewarding || _view.curTaskData == null) return;
		isSendGetRwewarding = true;

		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.Task_Reward, new Send_TaskReward(Core.Data.playerManager.PlayerID,_view.curTaskData.ID));

		task.ErrorOccured =(BaseHttpRequest b, string error)=>{ComLoading.Close(); isSendGetRwewarding = false; SQYAlertViewMove.CreateAlertViewMove("error:"+error);};
		task.afterCompleted += OnListGetRewardFinished;

		task.DispatchToRealHandler ();
		ComLoading.Open();
	}
	//领取任务奖励返回
	void OnListGetRewardFinished(BaseHttpRequest request, BaseResponse response)
	{
		isSendGetRwewarding = false;
		ComLoading.Close();
		_view.ShowGetRewardFinger(false);
		
		if (response.status != BaseResponse.ERROR) 
		{	
			TaskData data = null;
			TaskRewardResponse res = response as TaskRewardResponse;
			if(res == null || res.data == null || res.data.award == null)
			{
				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(9124));
				return;
			}
			GetRewardSucUI.OpenUI(res.data.award,Core.Data.stringManager.getString(5047));
			AddRewardToBag(res.data.award);
			
			HttpRequest req = request as HttpRequest;
			Send_TaskReward param = req.ParamMem as Send_TaskReward;
			if(res.data.nextTask != null)
			{
				data = tm[res.data.nextTask.id];
				if(data != null)
				{
					data.curProgress =  res.data.nextTask.progress;
					data.Progress = res.data.nextTask.condition;		
				}
				int index = 0;
				if(curTaskType == UITaskType.EveryData)
				{
					 index = GetTaskIndex(ref DayTaskList,param.taskId);
					 if(index > -1)
						DayTaskList[index] = data;
				}
				else if(curTaskType == UITaskType.MainLine)
				{
					 index = GetTaskIndex(ref MainTaskList,param.taskId);
					 if(index > -1)
						MainTaskList[index] = data;
				}
			}
			else
			{			
				if(curTaskType == UITaskType.EveryData)
				{
					DeleteTask(ref DayTaskList,param.taskId);
				}
				else if(curTaskType == UITaskType.MainLine)
				{
					DeleteTask(ref MainTaskList,param.taskId);
				}
			}
			//刷新当前列表
			if(curTaskType == UITaskType.EveryData)
			{
				if(DayTaskList.Count == 0)
				{
					_view.SetBtnBright(0,1);
					curTaskType = UITaskType.MainLine;
				}
			}
			
			if(data == null)
			   RefreshTaskList();
			else
			   RefreshTaskList(data.ID);
			
			//刷新任务按钮提示
//			Core.Data.taskManager.isHaveTaskComplete =  isHaveCompleteTask(ref DayTaskList) ||  isHaveCompleteTask(ref MainTaskList) ;
//			UITaskBtnState.Refresh();
			
			_view.ObjectPool[0].isSelected = true;
		
			if(FightRoleSelectPanel.Instance == null)
				DBUIController.mDBUIInstance.RefreshUserInfo ();
		}

	}
	
	
	bool isHaveCompleteTask(ref List<TaskData> list)
	{
		foreach(TaskData data in list)
		{
			if(data.curProgress == data.Progress)
				return true;
		}
		return false;
	}
	
	
	
	//删除任务
	public void DeleteTask(ref List<TaskData> TaskList,int TaskID)
	{
		TaskData temp = null;
		foreach(TaskData data in TaskList)
		{
			if(data.ID == TaskID)
			{
				temp = data;
				break;
			}
		}
		TaskList.Remove(temp);
	}
	
	//搜索任务在链表中的下标
	public int GetTaskIndex(ref List<TaskData> TaskList,int TaskID)
	{
		for(int i= 0;i<TaskList.Count; i++)
		{
			if(TaskList[i].ID == TaskID)
			{
				return i;
			}
		}
		return -1;
	}
	
	//如果type = UITaskType.None,按玩家最近一次的选择显示
	public static UITask Open(UITaskType type = UITaskType.None, System.Action callBack =  null)
	{
		if(Instance == null)
		{
		   	Object prefab = PrefabLoader.loadFromPack("LS/MissionSys/MissionSysRoot");
			if(prefab != null)
			{
				GameObject obj = Instantiate(prefab) as GameObject;
				RED.AddChild(obj, DBUIController.mDBUIInstance._TopRoot);
				obj.transform.localScale = Vector3.one;
				obj.transform.localPosition = Vector3.zero;
				obj.transform.localEulerAngles = Vector3.zero;
				Instance = obj.GetComponent<UITask>();	
			}
		}
		else
		{
			if(!Instance.gameObject.activeSelf)
			Instance.gameObject.SetActive(true);
			Instance.UIInit();
		}
		Instance.exitCallBack = callBack;
		//if(type != UITaskType.None)
		Instance.curTaskType = type;
		return Instance;
	}
	
	
	void OnBtnClick(GameObject btn)
	{
		if(btn != null) OnBtnClick(btn.name);
	}
	
	/*按钮事件处理
	 * */
	public void OnBtnClick(string btnName)
	{
		switch(btnName)
		{
		//主线任务
		case "Btn_MainLine":
			if(curTaskType == UITaskType.MainLine) return;
			curTaskType = UITaskType.MainLine;
			//Debug.Log("Btn_MainLine");
		
			RefreshTaskList();
			break;
	    //每日任务
		case "Btn_EveryData":
			if(curTaskType == UITaskType.EveryData) return;
			curTaskType = UITaskType.EveryData;
			//Debug.Log("Btn_EveryData");
			
		   RefreshTaskList();
			break;
		//领取奖励	
		case "Btn_GetReward":
			SendGetRewardRequest();
			break;
		//关闭
		case "Btn_Close":
			UIDestory();
			break;
		case "Btn_Jump":
			TaskJump();
			break;
		}
	}
	
	//刷新任务列表
	void RefreshTaskList(int SelectedID = -1)
	{
		_view.isHaveMainTask.enabled = isHaveCompleteTask(ref MainTaskList);
		if(_view.isHaveMainTask.enabled) CurSelected_MainLineCell = null;

		_view.isHaveDataTask.enabled =  isHaveCompleteTask(ref DayTaskList);
		if(_view.isHaveDataTask.enabled) CurSelected_EveryDayCell = null;

		if(curTaskType == UITaskType.MainLine)
		     _view.ShowTaskList(ref MainTaskList,SelectedID);
		else if(curTaskType == UITaskType.EveryData)
		     _view.ShowTaskList(ref DayTaskList,SelectedID);
		
		_view.SetBtnBright((int)curTaskType);

		//刷新任务按钮提示
		Core.Data.taskManager.isHaveTaskComplete =  _view.isHaveMainTask.enabled || _view.isHaveDataTask.enabled; //isHaveCompleteTask(ref DayTaskList) ||  isHaveCompleteTask(ref MainTaskList) ;
		UITaskBtnState.Refresh();
	}
	
	
	public void CreateElements(int num)
	{
		if(_view.ObjectPool == null) _view.ObjectPool = new List<UITaskCell>();
		Object prefab = PrefabLoader.loadFromPack("LS/MissionSys/pbLSMissionCell");
		int start = _view.ObjectPool.Count;	
		int end = start+num;
		float cellHeight = uiGrid.cellHeight;
		for(int i=start ;i< end; i++)
		{
			GameObject obj = Instantiate(prefab) as GameObject;
			obj.transform.parent = uiGrid.transform;
			obj.transform.localScale = Vector3.one;
			obj.transform.localPosition = new Vector3(0, -cellHeight*i ,0);
			_view.ObjectPool.Add(obj.GetComponent<UITaskCell>());
			obj.SetActive(false);
		}
	}
	
	

	
	//添加奖励到背包中
	void AddRewardToBag(ItemOfReward[] p)
	{
		ItemManager im = Core.Data.itemManager;
		foreach(ItemOfReward item in p)
		{
			im.AddRewardToBag(item);

            Core.Data.ActivityManager.setOnReward(item  ,ActivityManager.DAYTASK);
		}
	}
	
	void TaskJump()
	{
		TaskData data = _view.curTaskData;
	 	Core.Data.taskManager.JumpTaskID = data.ID;
		bool isDestroy = true;
		
		
		if(data == null) return;
				
		switch(data.TASKTYPE)
		{
		case 0:
		case 12:
			//DBUIController.mDBUIInstance.OpenFB(data.Count,TaskJumpBack);
			DBUIController.mDBUIInstance.OpenFB(data.Count);
			break;
		case 13:
		case 14:
		case 15:
			//SQYTeamController.mInstance.ShowTeamView(TaskJumpBack);
			DBUIController.mDBUIInstance.HiddenFor3D_UI ();
			TeamUI.OpenUI ();
			break;
		case 1:
			//FinalTrialMgr.GetInstance().InterfaceCreateScript(TrialEnum.TrialType_TianXiaDiYi,TaskJumpBack);
			FinalTrialMgr.GetInstance().InterfaceCreateScript(TrialEnum.TrialType_TianXiaDiYi,null);
			break;
		case 2:
			//FinalTrialMgr.GetInstance().InterfaceCreateScript(TrialEnum.TrialType_ShaLuChoose,TaskJumpBack);
			FinalTrialMgr.GetInstance().InterfaceCreateScript(TrialEnum.TrialType_ShaLuChoose,null);
			break;
		case 3:
			//FinalTrialMgr.GetInstance().InterfaceCreateScript(TrialEnum.TrialType_PuWuChoose,TaskJumpBack);
			FinalTrialMgr.GetInstance().InterfaceCreateScript(TrialEnum.TrialType_PuWuChoose,null);
			break;
		case 4:
			//FinalTrialMgr.GetInstance().InterfaceCreateScript(TrialEnum.TrialType_QiangDuoGold,TaskJumpBack);
			FinalTrialMgr.GetInstance().InterfaceCreateScript(TrialEnum.TrialType_QiangDuoGold,null);
			break;
		case 5:
			//DBUIController.mDBUIInstance.JumpToDragon(TaskJumpBack);
			DBUIController.mDBUIInstance.JumpToDragon(null);
			break;
		case 16:
			
			//UIDragonMallMgr.GetInstance().OpenUI(ShopItemType.HotSale,TaskJumpBack,true);	
			UIDragonMallMgr.GetInstance().OpenUI(ShopItemType.HotSale,null,true);	
			break;
		case 17:		
			//获得6星武者
			//TrainingRoomUI.OpenUI (ENUM_TRAIN_TYPE.MonsterEvolve, null, TaskJumpBack);
			if (TrainingRoomUI.IsTrainingRoomUnLock ())
			{
				TrainingRoomUI.OpenUI (ENUM_TRAIN_TYPE.MonsterEvolve);
				DBUIController.mDBUIInstance.HiddenFor3D_UI ();
			}
			else
				isDestroy = false;

			break;
		case 18:	
			if (TrainingRoomUI.IsTrainingRoomUnLock ()) 
			{
				//获得觉武者  
				//TrainingRoomUI.OpenUI(ENUM_TRAIN_TYPE.HeCheng, null, TaskJumpBack);
				TrainingRoomUI.OpenUI (ENUM_TRAIN_TYPE.HeCheng);
				DBUIController.mDBUIInstance.HiddenFor3D_UI ();
			}
			else
				isDestroy = false;
			break;
		case 19:
			//获得神武者
			//TrainingRoomUI.OpenUI(ENUM_TRAIN_TYPE.HeCheng, null, TaskJumpBack);
			if (TrainingRoomUI.IsTrainingRoomUnLock()) 
			{
				TrainingRoomUI.OpenUI (ENUM_TRAIN_TYPE.HeCheng);
				DBUIController.mDBUIInstance.HiddenFor3D_UI ();
			}
			else
				isDestroy = false;
			break;
		}
		
		if(isDestroy)
		Destroy(gameObject);
	}
	
	void TaskJumpBack2()
	{
		Core.Data.taskManager.JumpTaskID = -1;
	    UITask.Open();
	}
	
	void ResetTaskUI()
	{
		foreach( UITaskCell cell in _view.ObjectPool)
		{
			if(cell.gameObject.activeSelf)
				cell.gameObject.SetActive(false);
		}
		_view.ClearPanel();
	}
	
}
