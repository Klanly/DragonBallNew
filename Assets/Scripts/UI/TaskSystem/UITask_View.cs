using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*任务系统UI显示层
 * */
public class UITask_View : MonoBehaviour {

	//对象池
	public List<UITaskCell> ObjectPool;
	public UIButton[] Spr_Btns;
	public UILabel[] Lab_Btns;
	public UISprite[] Spr_BtnsSgin;
	
	public UILabel MissionName;
	public UILabel MissionDescription;
	public UILabel CompleteCondition;
	
	public UIScrollView leftScrollView;
	
	public TaskReward[] rewards;
	
	public UILabel rewardDes;
	
	public UISprite Btn_RewardBg;
    
	public UILabel Lab_RewardTxt;
	
    [HideInInspector]
	public TaskData curTaskData;
	
	
	public GameObject Btn_Go;
	
	public GameObject FingerWeakGuide;
	
	public UISprite isHaveDataTask;

	public UISprite isHaveMainTask;

	void Start () {
	
	}
	
	/*显示右边具体任务描述
	 * */
	public void ShowRightPanel(TaskData data)
	{
		curTaskData = data;
		MissionName.text = data.Title;
		MissionDescription.text = data.Content;

        if (data.Count > 60000)
        {
            NewFloor nfd =  Core.Data.newDungeonsManager.FloorList[data.Count];
            if (nfd != null)
            {
                CompleteCondition.text = data.Require.Replace("{}", nfd.config.name) + " [ffff00](" + data.curProgress.ToString() + "/" + data.Progress.ToString() + ")[-]";
            }
        }
        else
        {
            CompleteCondition.text = data.Require.Replace("{}", data.Count.ToString()) + " [ffff00](" + data.curProgress.ToString() + "/" + data.Progress.ToString() + ")[-]";
        }
		
		
		string str_content = "";
		for(int i = 0; i< rewards.Length ; i++)
		{
			UITaskRewardData rewardData =  null;
			if(i < data.Reward_ItemID.Count )
			{
				rewardData = new UITaskRewardData();	
				rewardData.Reward_ItemID = data.Reward_ItemID[i][0];
				rewardData.Reward_ItemCount = data.Reward_ItemID[i][1];
			}
			rewards[i].SetTaskReward(rewardData);
			if(rewardData != null)
				str_content+= rewardData.Reward_ItemCount.ToString() + "[-]" + Core.Data.stringManager.getString(6011) +rewards[i].Lab_Name.text;
		}
            // reward.SetTaskReward(data);
		/*
		 * {"ID":9066,"txt":"随机获得{}颗宝石"}
			{"ID":9067,"txt":"获得{}钻石"}
			{"ID":9068,"txt":"随机获得{}名武者的魂魄"}
		 * */
        rewardDes.text = Core.Data.stringManager.getString(9079)  + "[ffff00]"  + str_content ;

		SetGetRewardButtonEnable(data.curProgress == data.Progress);
		
		switch (data.TASKTYPE)
		{
		case 7:
		case 8:
		case 9:
		case 6:
		case  11:
			Btn_Go.SetActive(false);
			break;
		default:
			Btn_Go.SetActive(false); // yangchenguang  隐藏GO按钮
			break;
		}
		
	}
	
	public void ClearPanel()
	{
		curTaskData = null;
		MissionName.text = "";
		MissionDescription.text = "";
		CompleteCondition.text = "";
		rewardDes.text = "";
		for(int i = 0; i< rewards.Length ; i++)
		{
			rewards[i].SetTaskReward(null);
		}
		//reward.SetTaskReward(null);
		SetGetRewardButtonEnable(false);
	}
	
	/*将完成的任务放在最上面*/
	public bool PutCompleteToFront(ref List<TaskData> list)
	{
		bool isHaveTaskComplete = false;
		List<int> tempIndex = new List<int>();
		for(int i= 0;i<list.Count;i++)
		{
			if(list[i].curProgress == list[i].Progress)
			{
				isHaveTaskComplete = true;
				tempIndex.Add(i);
			}
		}
		foreach(int index in tempIndex)
		{
			TaskData data = list[index];
			list.RemoveAt(index);
			list.Insert(0,data);
		}
		return isHaveTaskComplete;
	}
	
	
	/*显示任务列表
	 * */
	public void ShowTaskList(ref List<TaskData> list,int selectedid = -1)
	{
//		UITask.Instance.CurSelected_EveryDayCell = null;
//		UITask.Instance.CurSelected_MainLineCell = null;

		bool isComplete = PutCompleteToFront(ref list);
		
		int listcount = list.Count;
		int cha = listcount - ObjectPool.Count;
		if(cha > 0) UITask.Instance.CreateElements(cha);
		
		for(int i = 0; i < ObjectPool.Count; i++)
		{
			if(i<listcount)
			{
				TaskData tdata = list[i];
				if(!ObjectPool[i].gameObject.activeSelf) ObjectPool[i].gameObject.SetActive(true);
				ObjectPool[i].name = tdata.ID.ToString();
				ObjectPool[i].SetCell(tdata);	
				if(!isComplete)
				{
					if(selectedid == -1)
					{
						if( tdata.Type == 0 && tdata.ID == Core.Data.taskManager.LastDayTaskID  || tdata.Type == 1 && tdata.ID == Core.Data.taskManager.LastMainTaskID )
							ObjectPool[i].isSelected = true;
					}
					else
					{
						//selectedid !=-1 说明指定了某任务为选中状态
						if(tdata.ID == selectedid)  
							ObjectPool[i].isSelected = true;
					}	
				}
				ObjectPool[i].Lab_Title.color =tdata.curProgress == tdata.Progress ? new Color(0,1f,0,1f):new Color(1f,0.8f,0,1f);
			}
			else
			{
				if(ObjectPool[i].gameObject.activeSelf) ObjectPool[i].gameObject.SetActive(false);
			}
		}
		
		if(list.Count == 0)
		{
			ClearPanel();
			return;
		} 
		
		int type = list[0].Type;
		SetBtnBright(type);
		
		
		if(type == 0)
		{
			if(UITask.Instance.CurSelected_EveryDayCell == null)
				ObjectPool[0].isSelected = true;
		}
		else
		{
			if(UITask.Instance.CurSelected_MainLineCell == null)
				ObjectPool[0].isSelected = true;
		}
		
		SpringPanel.Begin(leftScrollView.gameObject,new Vector3(0,120f,0),20f);
	}
	
	//state 1亮     2不可用
	public void SetBtnBright(int type,int state = 0)
	{
		if(state == 0)
		{
			Spr_Btns[type].GetComponent<BoxCollider>().enabled = true;
			Spr_Btns[type].normalSprite = "Symbol 31";
			Spr_Btns[1-type].normalSprite = "Symbol 32";
			//Lab_Btns[type].color = new Color(1f,1f,0,1f);
			//Lab_Btns[1-type].color = new Color(0.953f,0.843f,0.745f,1);
			if(Spr_BtnsSgin[type].enabled) Spr_BtnsSgin[type].enabled = false;
		}
		else
		{
			Spr_Btns[type].GetComponent<BoxCollider>().enabled = false;
			UISprite sprite = Spr_Btns[type].tweenTarget.GetComponent<UISprite>();
			if( sprite != null )
				sprite.color = new Color(0,0,0,1f);
			//Lab_Btns[type].color = new Color(1f,1f,1f,1f);
			Spr_BtnsSgin[type].enabled = true;
		}
	}
	
	
	public void SetGetRewardButtonEnable(bool Value)
	{
		Btn_RewardBg.transform.parent.GetComponent<BoxCollider>().enabled = Value;
		Lab_RewardTxt.text = Core.Data.stringManager.getString (7388);

		if(Value)
		{
			Btn_RewardBg.color = new Color(1f,1f,1f,1f);
			//Lab_RewardTxt.color = new Color(1f,0.796f,0.149f,1f);			
			if(!Core.Data.guideManger.isGuiding && Core.Data.playerManager.Lv < Core.Data.guideManger.TaskSystemWeekGuideTiggerLevel)
			{
				FingerWeakGuide.SetActive(true);
			}
		}
		else
		{
			FingerWeakGuide.SetActive(false);
			Btn_RewardBg.color = new Color(0,0,0,1f);
			Lab_RewardTxt.color = new Color(1f,1f,1f,1f);
		}
	}
	
	public void ShowGetRewardFinger(bool Value)
	{
		if(FingerWeakGuide.activeSelf != Value)
		FingerWeakGuide.SetActive(Value);
	}
	
	
}

