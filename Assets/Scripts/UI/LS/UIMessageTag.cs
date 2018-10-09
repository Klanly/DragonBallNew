using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class UIMessageTag : MonoBehaviour {

	static UIMessageTag _this;
	
	public List<GameObject> list_btn = new List<GameObject>();
	
	public FightMegCellData data{get;set;}

	public UILabel Lab_Revenge;

	public UISprite Spr_Stone;

	public static UIMessageTag OpenUI(FightMegCellData data,bool WinOrLose,RevengeProgressData rpdata)
	{
		if(_this == null)
		{
			Object prefab = PrefabLoader.loadFromPack("LS/pbLSInformationTag") ;
	        if(prefab != null)
	        { 
				GameObject obj = Instantiate(prefab) as GameObject;
				 RED.AddChild(obj, DBUIController.mDBUIInstance._TopRoot);
				 
	             _this = obj.gameObject.GetComponent<UIMessageTag>();
				 _this.Typography(WinOrLose);
				 _this.data = data;
				//要花钻石复仇
				if(rpdata.needStone > 0)
				{
					_this.Spr_Stone.enabled = true;
					_this.Lab_Revenge.text = rpdata.needStone.ToString();
				}
				else 
				{
					//免费复仇
					_this.Spr_Stone.enabled = false;
					_this.Lab_Revenge.text = "[FFFF00]("+rpdata.curProgress.ToString()+"/"+rpdata.maxProgress.ToString()+")[-]";
				}
	        }
		}
		else
		{
			_this.gameObject.SetActive(true);
		}
		return _this;
	}


	bool CanClickBtn_1 = true;
	bool CanClickBtn_2 = true;
	bool CanClickBtn_3 = true;
	void OnBtnClick(GameObject btn)
	{
		switch(btn.name)
		{
		case "Close":
			Destroy(gameObject);
			break;
		case "Btn_0":
			//留言
			gameObject.SetActive(false);
			UIMessageMail.OpenUI(data.cgid, OnMsgBoardClose);
			break;
		case "Btn_1":
			//报仇
			if(CanClickBtn_1)
			{
				CanClickBtn_1 = false;
			    Revenge(data.id);
			}
			break;
        case "Btn_2":
			//添加宿敌
			if(CanClickBtn_2)
			{
				 CanClickBtn_2 = false;
			     SendAddBlackListRequest();
			}
			break;
		case "Btn_3":
			//战斗回放
			FinalTrialMgr.GetInstance().BattleVideoRequestSingle(data.videoId.ToString(),RUIType.EMViewState.S_MailBox);
			break;
		}
	}
	
	//复仇
	void Revenge(int fightMail_id)
	{
		RevengeProgressData rpData = Core.Data.playerManager.revengeData;

		if(rpData != null && Core.Data.playerManager.Stone < rpData.needStone)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(7310));
			return;
		}

		Core.Data.temper.Revenge_Name = data.cName;
		Core.Data.temper.Revenge_Lv = data.cLevel;
		//如果成功会跳转场景，只有失败了才能再次被点击
		FinalTrialMgr.GetInstance().qiangDuoGoldFightRequest(data.cgid,0,RUIType.EMViewState.S_MailBox,-1,1).OnqiangDuoGoldError = ()=>{CanClickBtn_1 =true; };
		//类型 1：抢夺龙珠，2：排行榜，3抢夺金币
	}
	
	
	//发送添加宿敌请求
	public void SendAddBlackListRequest()
    {
		ComLoading.Open();
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
        task.AppendCommonParam(RequestType.ADD_SU_DI, new AddSuDiParam(int.Parse(Core.Data.playerManager.PlayerID),data.cgid)); 

		task.afterCompleted += addSuDiCompleted;

		task.ErrorOccured =(BaseHttpRequest request, string error) =>{ CanClickBtn_2 = true;Debug.Log("SendADD_SU_DIRequest is error!");  };

		task.DispatchToRealHandler ();
    }
     
	public void addSuDiCompleted(BaseHttpRequest request, BaseResponse response)
	{
		CanClickBtn_2 = true;
	    ComLoading.Close();
		if (response.status != BaseResponse.ERROR) 
		{
			AddSuDiResponse addSuDiResponse = response as AddSuDiResponse;
			switch(addSuDiResponse.data)
			{
			case 0:
				SQYAlertViewMove.CreateAlertViewMove ( Core.Data.stringManager.getString(9049) );
				break;
			case 1:
				SQYAlertViewMove.CreateAlertViewMove ( Core.Data.stringManager.getString(9050) );
				break;
			case 2:
				SQYAlertViewMove.CreateAlertViewMove ( "addSuDiCompleted Error" );
				break;	
			}
		}
	}
	
	
	
	
	
	
	//留言板关闭
	void OnMsgBoardClose()
	{
		gameObject.SetActive(true);
	}
	
	
	//排版
	void Typography(bool WinOrLose)
	{
		
	    int jiange = WinOrLose?80:100;
		int count = list_btn.Count;
		// 150 50  - 50  -150 
		//  0     100     200       300  
		for(int i = 0;i<count; i++)
		{
			if(jiange == 100)
			{
				if(!list_btn[i].activeSelf)list_btn[i].SetActive(true);
			     list_btn[i].transform.localPosition=new Vector3(0, -(jiange*i- 150),0);
			}
			else
			{
				if(i ==0 || i== count-1)
				{
					if(!list_btn[i].activeSelf)list_btn[i].SetActive(true);
					 list_btn[i].transform.localPosition=new Vector3(0, (jiange*i- 120)*2/3,0);
				}
				else
				{
					if(list_btn[i].activeSelf)list_btn[i].SetActive(false);
				}
			}
		}

	}
	
	
}
