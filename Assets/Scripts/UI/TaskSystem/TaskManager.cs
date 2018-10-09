using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TaskManager : Manager {

	public Dictionary<int,TaskData> ConfigData = null;

	List<int> TaskPromptList = new List<int>();
	
	//当前选中每日任务ID
	public int LastDayTaskID;
	//当前选中主线任务ID
	public int LastMainTaskID;
	//当前选中的任务类型
	public int LastSelectedType = 1;
	
	public int JumpTaskID = -1;
	
	public bool isHaveTaskComplete;
	
	public TaskManager()
	{
		ConfigData = new Dictionary<int, TaskData>();
	}
	
    public override bool loadFromConfig () 
	{
		bool result = base.readFromLocalConfigFile<TaskData>(ConfigType.Task,ConfigData);
		//DebugPrint();
		return result;
	}
	
	public void DebugPrint()
	{
		foreach(TaskData data in ConfigData.Values)
			Debug.LogError(data.ID.ToString());
	}
	
	
	public TaskData this[int TaskID]
	{
		get
		{
			TaskData data = null;
			ConfigData.TryGetValue(TaskID,out data);
			return data;
		}
		set
		{
			TaskData data = null;
			ConfigData.TryGetValue(TaskID,out data);
			if(data != null) data = value;
		}
	}
	

	
	//任务完成
	public void Complete(int[] TaskArray,int Act)
	{
		isHaveTaskComplete = true;
		UITaskBtnState.Refresh();
		
		if(Act == 100) return;
		foreach(int TaskID in TaskArray)
		{
			if(isFightTask(TaskID))
			{
				//添加到任务提示列表
				TaskPromptList.Add(TaskID);
			}
			else
			{
				//直接显示出来
				ShowTaskPromptWord(TaskID);
			}
		}
	}
	
	//是否延迟弹窗
	bool isFightTask(int TaskId)
	{
		TaskData data = this[TaskId];
		if(data == null) return false;
	    switch(data.TASKTYPE)
		{
		case 0:
		case 1:
		case 2:
		case 3:
		case 4:
		case 5:
		case 6:
		case 11:
		case 12:
		case 16:
			return true;
		}
		return false;
	}
	
	//显示任务提示文字
	void ShowTaskPromptWord(int TaskID, GameObject root = null)
	{
		string title ="[00ff00]["+Core.Data.stringManager.getString(9070)+"]" + TaskID.ToString() + Core.Data.stringManager.getString(10001) + "[-]";
		TaskData data = this[TaskID];

		if(data != null)
			title = "[00ff00]["+Core.Data.stringManager.getString(9070)+"]" + data.Title + Core.Data.stringManager.getString(10001) + "[-]";

		if(root != null) {
			SQYAlertViewMove.CreateAlertViewMove(title, root,1);
		} else {
            SQYAlertViewMove.CreateAlertViewMove(title,null ,1);
		}
			
	}


	/// <summary>
	/// 这个任务完成的提示，不能在非GameUI场景里调用
	/// </summary>
//	public void AutoShowPromptWord() 
//	{
//		foreach(int task in TaskPromptList)
//			ShowTaskPromptWord(task, Root);
//		TaskPromptList.Clear();	
//	}

	/// <summary>
	/// 这个任务完成的提示，在非GameUI场景里调用
	/// </summary>
	/// <param name="Root">Root.</param>
	public void AutoShowPromptWord(GameObject Root = null) 
	{
		foreach(int task in TaskPromptList)
			ShowTaskPromptWord(task, Root);
		TaskPromptList.Clear();		
	}

}
