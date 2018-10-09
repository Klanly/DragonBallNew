using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class UIBigWheel : MonoBehaviour {
	
	public List<BigWheelReward> wheels_object = new List<BigWheelReward>();
	private List<BigWheelReward> wheels = new List<BigWheelReward>();
	
	private BigWheelReward  LastSelected;
	
	public static UIBigWheel Instance;
	
	public UILabel Lab_Refresh;
	public UILabel Lab_Btn_Refresh;
	public UISprite Spr_Btn_Refresh;
	
	public UILabel Lab_Btn_Use;
	public UILabel Lab_NeedStone;
	public UISprite Spr_Btn_Use;
	
	private int totalCount;
	private int flushCount;
	
	public BoxCollider Btn_Back;
	
	public UILabel Title;
 
	void Start () {
		
		Title.text = Core.Data.stringManager.getString(9071);
		Lab_Btn_Refresh.text = Core.Data.stringManager.getString(6004);
		Lab_Btn_Use.text = Core.Data.stringManager.getString(9072);
		//wheels.RemoveAt(0);
		//wheels[0].isEnable = false;
		SendListMsg(RequestType.GET_BIGWHEEL_LIST);
	}
	
	//发送列表请求 or 重置大转盘
	void SendListMsg(RequestType requestType)
	{
		ComLoading.Open();
		HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(requestType, new BigWheelListParam(Core.Data.playerManager.PlayerID));
		
		task.ErrorOccured =(BaseHttpRequest request, string error)=>
		{
			ComLoading.Close();
			SQYAlertViewMove.CreateAlertViewMove(error);
		};
		task.afterCompleted =(BaseHttpRequest requset, BaseResponse response)=>
		{
			ComLoading.Close();
			if (response.status != BaseResponse.ERROR)
			{
				wheels.Clear();
				wheels.AddRange(wheels_object.ToArray());

				BigWheelListResponse res = response as BigWheelListResponse;
				if(res.data != null && res.data.awardList.Count > 0)
				{
					ShowBigWheelList(res.data.awardList);
					flushCount = res.data.flushCount;
					totalCount = res.data.totalCount;
					ShowResetButton();
					ShowUseButton(res.data.needStone);
					if(wheels.Count > 0)
					{
						foreach(BigWheelReward rewardObj in wheels)
						{
							if(rewardObj.isSelected)
								rewardObj.isSelected = false;
						}
						WheelIndex = 0;
						LastSelected = wheels[0];
						LastSelected.isSelected = true;
					}
				}
				else
				{
					SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(9118));
				}
			}
			else
			{
				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
			}
		} ;
	
		task.DispatchToRealHandler ();
	}
	
	
	//显示重置按钮
	void ShowResetButton()
	{
		Lab_Refresh.text = flushCount.ToString()+"/"+totalCount.ToString();
		SetResetButtonEnable(totalCount - flushCount > 0);
	}
	
	void SetResetButtonEnable(bool Value)
	{
		if(Value)
		{
		   Lab_Btn_Refresh.color = new Color(0.95f,0.843f,0.745f,1f);
			Spr_Btn_Refresh.color = new Color(1f,1f,1f,1f);
			Spr_Btn_Refresh.transform.parent.GetComponent<BoxCollider>().enabled = true;
		}
		else
		{
			Lab_Btn_Refresh.color = new Color(1f,1f,1f,1f);
			Spr_Btn_Refresh.color = new Color(0,0,0,1f);
			Spr_Btn_Refresh.transform.parent.GetComponent<BoxCollider>().enabled = false;
		}
	}
	
	
	
	
	//显示摇奖按钮
	void ShowUseButton(int needStone)
	{
		Lab_NeedStone.text = needStone.ToString();
		SetUseButtonEnable(wheels.Count > 0);
	}
	
	void SetUseButtonEnable(bool Value)
	{
		if(Value)
		{
		    Lab_Btn_Use.color = new Color(0.95f,0.843f,0.745f,1f);
			Spr_Btn_Use.color = new Color(1f,1f,1f,1f);
			Spr_Btn_Use.transform.parent.GetComponent<BoxCollider>().enabled = true;
		}
		else
		{
			Lab_Btn_Use.color = new Color(1f,1f,1f,1f);
			Spr_Btn_Use.color = new Color(0,0,0,1f);
			Spr_Btn_Use.transform.parent.GetComponent<BoxCollider>().enabled = false;
		}
	}
	
	
	//显示大转盘列表
	void ShowBigWheelList(List<int[]> awardList)
	{
		List<BigWheelReward> removeList = new List<BigWheelReward>();
		for(int i=0;i<wheels.Count;i++)
		{
			wheels[i].SetData(awardList[i]);
			if(awardList[i][2] == 0)
				removeList.Add(wheels[i]);
		}
		foreach(BigWheelReward cell in removeList)
			wheels.Remove(cell);
		
	}
	
	
	public static UIBigWheel OpenUI()
	{
		Object prefab = PrefabLoader.loadFromPack("JC/UIBigWheel");
		if(prefab != null)
		{
			GameObject obj = Instantiate(prefab) as GameObject;
			RED.AddChild(obj, DBUIController.mDBUIInstance._bottomRoot);
			obj.transform.localScale = Vector3.one;
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localEulerAngles = Vector3.zero;
			Instance = obj.GetComponent<UIBigWheel>();			
		}
		return Instance;
	}
	
	
    private float TimeInterval = 0.3f;
	private float TimeCount = 0;
	private int WheelIndex = 0;
//	private int SpeedIndex = 0;
	bool isRUN = false;
	bool UpOrDown = true;
	
	private bool isSTOP = false;
	float MaxSpeedTime = 0;
	
	
	/*最终要停的位置*/
	int NeedID = 0;
	void Update ()
	{
	    if(!isRUN) return ;
	     if(TimeCount < TimeInterval)
		{
			TimeCount += Time.deltaTime;
			return;
		}
		TimeCount = 0;
		
		if(WheelIndex >= wheels.Count)WheelIndex = 0;
		
		
		if(UpOrDown)
		{
			if(TimeInterval  > 0.04f)
			{
				
			   TimeInterval -= 0.02f;
			   TimeInterval = float.Parse( TimeInterval.ToString("f2"));
				MaxSpeedTime = Time.time;
			}
			else
			{
				if(Time.time - MaxSpeedTime >1f)
				{					
					 //开始降速
				     isSTOP = true;
					 int downCount = (int)((0.3f-0.04f)/0.02f);
					 int wheelsCount = wheels.Count;
					
					 STOPIndex = Mathf.Abs( NeedID - downCount ) % wheelsCount  ;
					 if(STOPIndex != 0)
					 STOPIndex =wheelsCount - Mathf.Abs( NeedID - downCount ) % wheelsCount  ;
					 //Debug.Log("downCount="+downCount.ToString()+"    NeedID="+NeedID.ToString()+"     STOPIndex="+STOPIndex.ToString());
				}
			}
		}
		else
		{
			TimeInterval += 0.02f;
			if(TimeInterval >= 0.3f)
			{
				isRUN = false;
				UpOrDown = true;
				Invoke("OnBigWheelAnimationFinished",0.5f);
			}

				TimeInterval = float.Parse( TimeInterval.ToString("f2"));
			    
				//Debug.Log("-----TimeInterval=------------"+TimeInterval.ToString() + "   WheelIndex"+WheelIndex.ToString() ) ;		
		}
		
		
		
		
		
		if(WheelIndex < wheels.Count)
		{
			    wheels[WheelIndex].isSelected = true;
				if(LastSelected != null)
				LastSelected.isSelected = false;
				LastSelected = wheels[WheelIndex];
			
		}		
		
		if(isSTOP)
		{
			if(WheelIndex == STOPIndex)
			{
				UpOrDown = false;
				isSTOP = false;
			}
		}
		
		WheelIndex++;
		
	}
	
	
	void UIInformationSure()
	{
		UIDragonMallMgr.GetInstance().SetRechargeMainPanelActive();
	}
	
	
	int STOPIndex = 0;
	
	void OnBtnClick(GameObject btn)
	{
		switch(btn.name)
		{
		case "Btn_Use":
			//如果钻石不够
			//Debug.Log("Stone="+ Lab_NeedStone.text);
			if(Core.Data.playerManager.RTData.curStone <  (int)System.Convert.ToInt32(Lab_NeedStone.text)  )
			{
			   UIInformation.GetInstance().SetInformation(Core.Data.stringManager.getString(9021),Core.Data.stringManager.getString(5030),UIInformationSure);
			}
			else
			SendUseWheelMsg();
			break;
		case "Btn_Refresh":
			SendListMsg(RequestType.RESET_BIGWHEEL_LIST);
			break;
		case "Btn_Back":
			DBUIController.mDBUIInstance.ShowFor2D_UI();
			Destroy(gameObject);
			Instance = null;
			
			break;
		}
		
	}
	
	
	
	//发送使用大转盘消息
	UseBigWheelResponse res = null;
	void SendUseWheelMsg()
	{
		ComLoading.Open();
		SetUseButtonEnable(false);
		SetResetButtonEnable(false);
		Btn_Back.enabled = false;
		
		HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.USE_BIGWHEEL, new BigWheelListParam(Core.Data.playerManager.PlayerID));
		
		task.ErrorOccured =(BaseHttpRequest request, string error)=> {ComLoading.Close();Btn_Back.enabled = true;};
		task.afterCompleted =(BaseHttpRequest requset, BaseResponse response)=>
		{
			
		    if (response != null && response.status != BaseResponse.ERROR)
            {
				ComLoading.Close();
				res = response as UseBigWheelResponse;

				if(res.data != null )
				NeedID = GetRewardIndex(res.data.p[0]);
				else
				{
					SQYAlertViewMove.CreateAlertViewMove("Server Data error [p=null]");
					Btn_Back.enabled = true;
					return;
				}
				if(wheels.Count == 1)
				{				
					wheels[NeedID].isSelected = true;
					if(LastSelected != null)
					LastSelected.isSelected = false;
				    OnBigWheelAnimationFinished();			
				}
				else
				isRUN = true;
				AddRewardToBag(res.data.p);
				
				
				Core.Data.playerManager.RTData.curStone += res.data.stone;
	            //talking data add by wxl 
	            Core.Data.ActivityManager.OnPurchaseVirtualCurrency(ActivityManager.BigWheelType,1,Mathf.Abs(res.data.stone));
	
//				UIMiniPlayerController.Instance.freshPlayerInfoView ();
			    DBUIController.mDBUIInstance.RefreshUserInfo ();
			}
		} ;
	
		task.DispatchToRealHandler ();
	}
	
	//获取获得的奖励在链表中的下标
	public int GetRewardIndex(ItemOfReward _item)
	{
		for(int i=0;i<wheels.Count;i++)
		{
			if(wheels[i].gameObject.name == _item.pid.ToString())
				return i;
		}
		return -1;
	}
	
	
	void OnBigWheelAnimationFinished()
	{
		Btn_Back.enabled = true;
		GetRewardSucUI.OpenUI(res.data.p,Core.Data.stringManager.getString(5047));
		wheels[NeedID].isEnable = false;
		wheels.RemoveAt(NeedID);
		ShowUseButton(res.data.nStone);
		ShowResetButton();
	}
	
	
	//添加奖励到背包中
	void AddRewardToBag(ItemOfReward[] p)
	{
		ItemManager im = Core.Data.itemManager;
		foreach(ItemOfReward item in p)
		{
			im.AddRewardToBag(item);

            Core.Data.ActivityManager.setOnReward(item  ,ActivityManager.XINGYUNWHEEL);

		}
	}
	
}
