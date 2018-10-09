using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FrogingSystem;
public class GemInlaySystemUI_Logic : MonoBehaviour {
	
	
	GemInlaySystemUI_View _view = null;
	GemInlaySystemUI_View view 
	{
	     get
		{
			if(_view == null)
			{
			     _view = gameObject.GetComponent<GemInlaySystemUI_View>();
				 _view.Init();
			}
			return _view;
		}
	}
	
    public List<int> AllSelectedGems=new List<int>();
	/*最近点击的装备也的名字
	 * */
	string LastSelectedHoleButtonName="";
	
	Equipment SelectedEqudata=null;

	public Gems SelectedGemdata{get;set;}
	
	public GemInlaySystemMessageBox _box;
	
	enum SendServiceMessageType
	{
		None,
		SendInlayGem,
		SendRemoveGem,
	}
	SendServiceMessageType  CurMessage=SendServiceMessageType.None;
	
	void Start ()
	{
		//view=gameObject.GetComponent<GemInlaySystemUI_View>();
		view.ButtonClick+=ButtonClick;
		_box.ButtonClick+=MessageBoxButton;
	}
	
	public void ButtonClick(GameObject button)
	{
		ButtonClick(button.name,button);
	}
	
	public void ButtonClick(string BtnName,GameObject button = null)
	{
		if(BtnName.Contains("Btn_Hole"))
		{
			LastSelectedHoleButtonName=BtnName;
			GemHoleViewInfo TempButton = null;
			if(button == null) 
				button = GameObject.Find(BtnName);
			if(button != null)
			 TempButton = button.GetComponent<GemHoleViewInfo>();
			
			if(TempButton!=null)
			{
				if(TempButton.isHaveGem)
				{					
					_box.Open();
				}
				else
				{
					int count = 0;
					for (short i = 1; i < 6; i++)
					{
						List<Gems> list = Core.Data.gemsManager.GetGemsByStar (i, SplitType.Split_If_InTeam);
						if (list != null)
						{
							count += list.Count;
						}
					}
					if (count == 0)
					{
						if (count == 0)
						{
							if(LuaTest.Instance != null && !LuaTest.Instance.ConvenientBuy){;}
							else UIInformation.GetInstance().SetInformation(Core.Data.stringManager.getString(9019),Core.Data.stringManager.getString(5030),UIInformationSure);
							return;
						}
					}

					ForgingRoomUI.Instance.Visible=false;
			        DBUIController.mDBUIInstance.SetViewState (RUIType.EMViewState.S_Bag, RUIType.EMBoxType.SELECT_GEM_INLAY);
				}
			}
			else
			{
				RED.LogError("GemInlaySystemUI_Logic line 47 BUG");
			}
			
		}
		else if(BtnName =="Equipment")
		{
			ForgingRoomUI.Instance.Visible=false;
			/*打开背包
			 * */
			DBUIController.mDBUIInstance.SetViewState (RUIType.EMViewState.S_Bag, RUIType.EMBoxType.SELECT_EQUIPMENT_INLAY);
		}
		else if(BtnName =="Btn_Describe")
		{
			DescribeMessageBox.Open(view.TEXT(9003)+view.TEXT(9004),view.TEXT(9017));
		}
	}


	void UIInformationSure()
	{
		UIDragonMallMgr.GetInstance().OpenUI(ShopItemType.Item,ShopExit);		
	}

	void ShopExit()
	{
		UIMiniPlayerController.Instance.SetActive(true);
		if(gameObject != null)
			gameObject.SetActive(true);
	}

	public void SelectEquipment(Equipment equdata)
	{
		view.Lab_Add.enabled=false;
		SelectedEqudata=equdata;
		view.SetSelectedEquipment(equdata.ConfigEquip.ID.ToString());
		EquipSlot[] slots=equdata.RtEquip.slot;
		view.ShowHoles(slots);
		AllSelectedGems.Clear();
		
//		if(isHaveEffect(SelectedEqudata))
//		{		
//			view.ShowEquipEffect(SelectedEqudata);
//		}
//		else
//			view.SetEquipEffect("");
//		view.ShowEquipEffect(SelectedEqudata, Core.Data.EquipManager.isHaveEffect(SelectedEqudata));
		view.ShowEquipmentInfo(SelectedEqudata);
	}
	
		
	public void SelectGem(Gems gem)
	{
		AllSelectedGems.Add(gem.id);
		if(LastSelectedHoleButtonName!="")
		{
			 SelectedGemdata=gem;
			 CurMessage = SendServiceMessageType.SendInlayGem;
		}
	}
	
	void Update()
	{
		switch(CurMessage)
		{
		case SendServiceMessageType.SendInlayGem:
			
			/*向服务器发送镶嵌宝石信息
			 * */
			SendInlayMsg();
			CurMessage=SendServiceMessageType.None;
			break;
		case SendServiceMessageType.SendRemoveGem:
			/*向服务器发送摘除宝石信息
			 * */
			SendRemoveMsg();
			CurMessage=SendServiceMessageType.None;
			break;
		}
	}
	
	
	/*
	 * 向服务器发送镶嵌信息
	 * */
	void SendInlayMsg()
	{
		Send_GemInLaySystem param = new Send_GemInLaySystem ();
		param.gid = Core.Data.playerManager.PlayerID;
		param.eid = SelectedEqudata.ID;
		int soltindex=view.GetHoleId(LastSelectedHoleButtonName);
		param.slot = soltindex;
		param.gemId=SelectedGemdata.id;
		param.beUp=1;
		
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
    	task.AppendCommonParam(RequestType.GEM_INLAY_REMOVE, param);

		task.ErrorOccured += testHttpResp_Error;
		task.afterCompleted += testHttpResp_UI;
		task.DispatchToRealHandler ();
		ComLoading.Open();
	}
	
	void testHttpResp_UI (BaseHttpRequest req, BaseResponse response)
	{
		ComLoading.Close();
		/*只能主线程访问
		 * */
		Core.Data.EquipManager.InlayGemToEquipment(req,response);
		if (response.status != BaseResponse.ERROR) 
		{	
		    HttpRequest htReq = req as HttpRequest;
			Send_GemInLaySystem sendparam = htReq.ParamMem as Send_GemInLaySystem;
			if(sendparam.beUp==0)
			{
				/*摘除宝石成功
				 * */
				int SelectedSoltIndex=System.Convert.ToInt32(LastSelectedHoleButtonName.Substring(LastSelectedHoleButtonName.Length-1));				   			
			    view.SetHoleGem(SelectedSoltIndex,null);
				
				//view.ShowEquipEffect(SelectedEqudata,Core.Data.EquipManager.isHaveEffect(SelectedEqudata));
				view.ShowEquipmentInfo(SelectedEqudata);
			}
			else if(sendparam.beUp==1)
			{
				/*镶嵌宝石成功
				 * */
				int SelectedSoltIndex=System.Convert.ToInt32(LastSelectedHoleButtonName.Substring(LastSelectedHoleButtonName.Length-1));				   			
			    view.SetHoleGem(SelectedSoltIndex,SelectedGemdata.configData.anime2D);

				//view.ShowEquipEffect(SelectedEqudata,Core.Data.EquipManager.isHaveEffect(SelectedEqudata));
				view.ShowEquipmentInfo(SelectedEqudata);
			}
		}   
	}
	
	void testHttpResp_Error(BaseHttpRequest request, string error)
	{
		ConsoleEx.DebugLog ("---- Http Resp - Error has ocurred.(at GemSyntheticSystemUI_Logic:)" + error);
	}
	
	void MessageBoxButton(GameObject btn)
	{
		switch(btn.name)
		{
		case "Btn_Close":
			_box.Close();
			break;
		case "Btn_Remove":
			CurMessage = SendServiceMessageType.SendRemoveGem;
			_box.Close();
			break;
		case "Btn_Replace":
			ForgingRoomUI.Instance.Visible=false;
			DBUIController.mDBUIInstance.SetViewState (RUIType.EMViewState.S_Bag, RUIType.EMBoxType.SELECT_GEM_INLAY);
			_box.Close();
			break;
		}
	}
	
	/*向服务器发送移除宝石信息
	 * */
	void SendRemoveMsg()
	{
		Send_GemInLaySystem param = new Send_GemInLaySystem ();
		param.gid = Core.Data.playerManager.PlayerID;
		param.eid = SelectedEqudata.ID;
		int soltindex=view.GetHoleId(LastSelectedHoleButtonName);
		param.slot = soltindex;
		
		EquipSlot[] slots=SelectedEqudata.RtEquip.slot;
		param.gemId =slots[soltindex].mGem.id;
		
		param.beUp=0;
		
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
    	task.AppendCommonParam(RequestType.GEM_INLAY_REMOVE, param);

		task.ErrorOccured += testHttpResp_Error;
		task.afterCompleted += testHttpResp_UI;
		task.DispatchToRealHandler ();
		ComLoading.Open();
	}
	
	
	public void Quit()
	{
		ClearLastSelected();
	}
	
	/*清除最近的选择
	 * */
	public void ClearLastSelected()
	{
		
		LastSelectedHoleButtonName="";
		SelectedGemdata=null;
		SelectedEqudata=null;
		view.Lab_Add.enabled=true;
		view.ClearGemInlayViewPanel();
	}

	public bool IsEquipHasEffect()
	{
		return Core.Data.EquipManager.isHaveEffect (SelectedEqudata);
	}
}
