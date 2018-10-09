using UnityEngine;
using System.Collections;
using FrogingSystem;
public class GemRecastingSystemUI_Logic : MonoBehaviour {
	
	public GemRecastingSystemUI_View view{get;set;}
	Equipment SelectedEqudata=null;
	public int LockSlotCount{get;set;}
	void Start () 
	{
	    view=gameObject.GetComponent<GemRecastingSystemUI_View>();
		view.ButtonClick+=ButtonClick;
	}
	
	void ButtonClick(GameObject btn)
	{
		if(btn.name.Contains("Btn_Hole"))
		{			
			GemRecastingHoleInfo holeinfo=btn.GetComponent<GemRecastingHoleInfo>();
			holeinfo.AutoLockOrUnLock();
		}
		else if(btn.name=="Equipment")
		{
			ClearLastSelected();
			ForgingRoomUI.Instance.Visible=false;
			/*打开背包
			 * */
			DBUIController.mDBUIInstance.SetViewState (RUIType.EMViewState.S_Bag, RUIType.EMBoxType.SELECT_EQUIPMENT_RECAST);
		}
		else if(btn.name=="Btn_Recast")
		{
			if(SelectedEqudata != null)
			SendRecastMsg();
		}
		else if(btn.name=="Btn_Describe")
		{
			DescribeMessageBox.Open(view.TEXT(9005)+view.TEXT(9004),view.TEXT(9018));
		}
	}
	
	
	public void SelectEquipment(Equipment equdata)
	{
		view.Lab_Add.enabled=false;
		LockSlotCount = 0;
		SelectedEqudata=equdata;
		view.SetSelectedEquipment(equdata.ConfigEquip.ID.ToString());
		EquipSlot[] slots=equdata.RtEquip.slot;
		view.ShowHoles(slots);
		view.SetNeedStone(10);
	}
	
	public void Quit()
	{
		ClearLastSelected();
	}
	
	/*向服务器发送重铸数据
	 * */
	void SendRecastMsg()
	{
		if (Core.Data.playerManager.RTData.curStone < int.Parse(view.Lab_stone.text)) {
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(35006));
			return;
		}

		Send_GemRecastSystem param = new Send_GemRecastSystem ();
		param.gid = Core.Data.playerManager.PlayerID;
		param.eqid = SelectedEqudata.ID;
		param.locks=view.GetLockAarry();
		
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.GEM_RECASTING, param);

		task.ErrorOccured += testHttpResp_Error;
		task.afterCompleted += testHttpResp_UI;

		//then you should dispatch to a real handler
		task.DispatchToRealHandler ();
		ComLoading.Open();
	}
	
	void testHttpResp_UI (BaseHttpRequest req, BaseResponse response)
	{
		ComLoading.Close();
		if (response.status != BaseResponse.ERROR) 
		{	
			HttpRequest rq = req as HttpRequest;
			if (rq.Type == RequestType.GEM_RECASTING)
			{	
				view.ShowHoles(SelectedEqudata.RtEquip.slot);
//				UIMiniPlayerController.Instance.freshPlayerInfoView();
				DBUIController.mDBUIInstance.RefreshUserInfo();

                //talkingData add by wxl 
                GemRecastResponse resp = response as GemRecastResponse;
                if(resp.data != null){
                    Core.Data.ActivityManager.OnPurchaseVirtualCurrency (ActivityManager.ForgingType,1,Mathf.Abs(resp.data.stone));
                }

			}
		}
	}
	
	void testHttpResp_Error(BaseHttpRequest request, string error)
	{
		ConsoleEx.DebugLog ("---- Http Resp - Error has ocurred.(at GemSyntheticSystemUI_Logic:)" + error);
	}
	
	/*如果再锁定一个,是否所有的插槽都锁定
	 * */
    public bool IsSoltsWillAllLock
	{
		get
		{
			if(LockSlotCount+1>=SelectedEqudata.RtEquip.slot.Length)
				return true;
			return false;
		}
	}
	void ClearLastSelected()
	{
		LockSlotCount=0;
		SelectedEqudata=null;
		view.ClearGemInlayViewPanel();
		view.Lab_Add.enabled=true;
	}
	
}
