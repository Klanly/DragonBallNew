using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public enum MailBoxShowTab
{
	Invalid = 0,			//无效
	Email = 1,				//邮件
	BattleMsg = 2,			//战报
}

public class MailBox : MonoBehaviour {

	public static MailBox _mInstance;
	
	public UIGrid uiguide;
	
	public MailBox_View _view;
	
	private int Comfrom = 0;
	private MailBoxShowTab showTab = MailBoxShowTab.Invalid;
	
	static string LastBtnClickName = "Btn_msg";
	
	//Comfrom   0:从界面返回    1:从战斗场景返回(回放或者复仇)
	public static MailBox OpenUI(int Comfrom = 0, MailBoxShowTab directShow = MailBoxShowTab.Invalid)
	{
		Object prefab = PrefabLoader.loadFromPack("JC/MailBox");
		if(prefab != null)
		{
			GameObject obj = Instantiate(prefab) as GameObject;
			RED.AddChild(obj, DBUIController.mDBUIInstance._bottomRoot);
			obj.transform.localScale = Vector3.one;
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localEulerAngles = Vector3.zero;
			_mInstance = obj.GetComponent<MailBox>();
			_mInstance.Comfrom = Comfrom;
			_mInstance.showTab = directShow;
			_mInstance.Init();
			
		}
		return _mInstance;
	}
	
	void Init()
	{
		
	}
	
	
	void Start()
	{
		
		_view.SetNewSgin(MailReveicer.Instance.mailState);
		//对象池默认十个元素
		CreateCellObject(10);
		
		if(MailReveicer.Instance.mailState == MailState.newMsg)
		{
			OnBtnClick("Btn_msg");
		}
		else if(MailReveicer.Instance.mailState == MailState.newFight)
		{
			OnBtnClick("Btn_fight");
		}
		else
		{
			if (showTab == MailBoxShowTab.Invalid)
			{
				if (Comfrom == 0)
					OnBtnClick ("Btn_msg");
				else if (Comfrom == 1)
					OnBtnClick (LastBtnClickName);
			}
			else
			{
				if (showTab == MailBoxShowTab.Email)
					OnBtnClick ("Btn_msg");
				else if (showTab == MailBoxShowTab.BattleMsg)
					OnBtnClick("Btn_fight");
			}
		}
	}
	
	void OnBtnClick(GameObject btn)
	{
		OnBtnClick(btn.name);
	}
	
	
	string oldClickBtnName = "";
	void OnBtnClick(string btnName)
	{
		switch(btnName)
		{
		case "Btn_close":
			FinalTrialMgr.GetInstance().jumpTo = TrialEnum.None;
			DBUIController.mDBUIInstance.ShowFor2D_UI (false);
			Destroy(gameObject);
			break;
		case "Btn_fight":
			if(oldClickBtnName == btnName) return;
			oldClickBtnName = btnName;		

			LastBtnClickName = btnName;

			_view.ShowFight();
			ChangFightStateRead();			
			break;
		case "Btn_msg":
			if(oldClickBtnName == btnName) return;
			oldClickBtnName = btnName;
			
			_view.SetNewSgin(MailReveicer.Instance.mailState);
			
			LastBtnClickName = btnName;
			_view.ShowMsg();
			break;
		}
	}
	
	//默认创建元素
    public void CreateCellObject(int num)
    {
        GameObject obj1 = PrefabLoader.loadFromPack("LS/pbLSInformationCell") as GameObject ;
        if(obj1 != null)
        { 
            for(int i=0; i<num; i++)
            {
                GameObject go = NGUITools.AddChild (uiguide.gameObject, obj1);
                UIMessageCell mm = go.gameObject.GetComponent<UIMessageCell>();
                mm.gameObject.SetActive(false);
				_view.ListCells.Add(mm);
            }
        }
    }
	

	//设置所有的战报为已读状态
	void ChangFightStateRead()
	{
		MailState state =MailReveicer.Instance.mailState;
		if(state == MailState.newFight || state == MailState.AllNew)
		{
			//Debug.Log("SendFightAllReadRequest");
			SendFightAllReadRequest();
		}
	}
	//发送所有战报为已读状态
	void SendFightAllReadRequest()
	{
		ComLoading.Open();
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		ChangeMailStateParam param = new ChangeMailStateParam();
		param.gid = Core.Data.playerManager.PlayerID;
		List<FightMegCellData> list_fight = MailReveicer.Instance.list_fight;
		List<int> list_ids = new List<int>();
		foreach(FightMegCellData data in list_fight)
			list_ids.Add(data.id);
		param.ids = list_ids.ToArray();
		param.type = 1;
		param.msgType = 1;
        task.AppendCommonParam(RequestType.CHANGE_MAIL_STATE, param);       
        task.afterCompleted = (BaseHttpRequest request, BaseResponse response) => 
		{
			ComLoading.Close();
			if (response.status != BaseResponse.ERROR) 
		    {
			   ChangeMailStateResponse res = response as ChangeMailStateResponse;
			   //Debug.Log(res.data);
			   if(res.data)
				{
					//操作成功
					if(MailReveicer.Instance.mailState == MailState.AllNew) 
					    MailReveicer.Instance.mailState = MailState.newMsg;
					else if(MailReveicer.Instance.mailState == MailState.newFight)
					    MailReveicer.Instance.mailState = MailState.None;
					//_view.SetNewSgin(MailReveicer.Instance.mailState);
				}
				
			}
			
		};
		task.ErrorOccured =(BaseHttpRequest request, string error) =>{ ComLoading.Close();Debug.Log("SendFightAllReadRequest is error!");  };
        task.DispatchToRealHandler();
	}
}
