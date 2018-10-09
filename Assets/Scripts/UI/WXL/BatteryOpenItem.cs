using UnityEngine;
using System.Collections;
using System;

public class BatteryOpenItem :MonoBehaviour,IItem
{
	public UISprite spIcon;
	public UISprite spMoneyType;
	public UILabel lblMoneyNum;
	public UILabel lblLeftNum;
	public UILabel lblLeftTime;
	public UIButton openBtn;
	BatteryData m_data;
	int baseID= 110181;
	public int itemId =0;

	#region IItem implementation

	public void SetItemValue (object obj)
	{
		m_data = obj as BatteryData;
		if (IsInvoking ("Refresh")) {
			CancelInvoke ("Refresh");
		}
		if (m_data != null) {
			itemId = baseID + m_data.batteryType;
			if (m_data.dur > 0) {
				InvokeRepeating ("Refresh", 0, 1);
			}
			else {
				this.Refresh ();	
			}
		} else {
			lblLeftTime.text = "--:--:--";
			openBtn.isEnabled = false;
			lblMoneyNum.text = "642000";
			lblLeftNum.text = "x10";
		}
	}

	public object ReturnValue ()
	{
		return (object)m_data;
	}

	public void Refresh ()
	{
		if (m_data != null) {
			if (m_data.moneyType == 1) {
				spMoneyType.spriteName = "common-0013";
			} else {
				spMoneyType.spriteName = "common-0014";
			}
			lblMoneyNum.text = m_data.moneyNum.ToString ();
			spIcon.spriteName = itemId.ToString ();
			lblLeftNum.text = "x" + Core.Data.itemManager.GetBagItemCount (itemId);
			string leftTimeTxt = "00:00:00";
			if (m_data.dur >= 0) {
				TimeSpan span = m_data.fTime - DateHelper.UnixTimeStampToDateTime (Core.TimerEng.curTime);
				if (span.TotalSeconds > 0) {
					DateTime leftTime = Convert.ToDateTime (span.Hours.ToString () + ":" + span.Minutes.ToString () + ":" + span.Seconds.ToString ());
					leftTimeTxt = leftTime.ToLongTimeString ();
//					lblLeftTime.text = leftTimeTxt;
				} 
				lblLeftTime.text = leftTimeTxt;
				openBtn.isEnabled = m_data.canOpen;

			} else {
				CancelInvoke ("Refresh");
				openBtn.isEnabled = m_data.canOpen;
				leftTimeTxt = "--:--:--";
				lblLeftTime.text = leftTimeTxt;
			}
		}
	}

	#endregion

	public 	void  OpenCollect ()
	{
		ItemData tItemData = Core.Data.itemManager.getItemData (itemId);
		string content = "";
		if (tItemData != null) {
			content = string.Format (Core.Data.stringManager.getString (7418), m_data.needBatteryNum.ToString (), tItemData.name);
		}
		UIInformation.GetInstance ().SetInformation (content,Core.Data.stringManager.getString(5030),SendOpenSourceBuildMsg);
//		this.SendOpenSourceBuildMsg ();

	}

	void SendOpenSourceBuildMsg ()
	{
		if (m_data != null) {
			if (Core.Data.itemManager.GetBagItemCount (itemId) < m_data.needBatteryNum) {
//				JCRestoreEnergyMsg.OpenUI (ItemManager.COIN_PACKAGE, ItemManager.COIN_BOX, 2);
				string alert =  string.Format (Core.Data.stringManager.getString (25174), Core.Data.itemManager.getItemData(itemId).name);
				SQYAlertViewMove.CreateAlertViewMove (alert);
				return;
			}
		} 
		HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
		Debug.Log (" battery type  ====  " + m_data.batteryType);
		task.AppendCommonParam (RequestType.BATTLE_BUILD_OPEN, new  BattleBuildOpenParam (Core.Data.playerManager.PlayerID, m_data.buildingPid, 1, m_data.batteryType));
		task.ErrorOccured += HttpErrorOccured;
		task.afterCompleted += SendOpenBuilding;
		task.DispatchToRealHandler ();

		ComLoading.Open ();
	}

	void HttpErrorOccured (BaseHttpRequest request, string error)
	{

	}

	void SendOpenBuilding (BaseHttpRequest request, BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR) {
			BuildOperateResponse buildResponse = response as BuildOperateResponse;
			if (buildResponse.data != null) {
//				Debug.Log ("   id resp =  " +buildResponse.data.id  + " data  id = " + m_data.buildingId  + "   pidddd  = " +  m_data.buildingPid);
				if (buildResponse.data.id == m_data.buildingPid) {

					if (buildResponse.data.dur > 0) {
						openBtn.isEnabled = false;
						m_data.canOpen = false;
						Building data = Core.Data.BuildingManager.GetBuildFromBagByNum(buildResponse.data.num);
						UISourceBuilding.mInstance.SetData (data);

						BuildScene.mInstance.BuildOpenUpdate(data.RTData.id);
//						this.Refresh ();
					}
				}
			}
		} else if (response.status == BaseResponse.ERROR) {
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getNetworkErrorString(response.errorCode));
			openBtn.isEnabled = true;
			m_data.canOpen = true;
		}
		ComLoading.Close ();
	}
}

public class BatteryData
{
//	public int id;
	//道具 id
	public int buildingId;
	//建筑物 配表ID
	public int buildingPid;
	//建筑的唯一标示
	public int moneyType;
	//1 金币  2 钻石
	public int batteryType;
	// 1小  2中  3大
	public long dur;
	//剩余时间
	public int moneyNum;
	//产量
	public DateTime fTime = new DateTime ();
	//到期时间
	public bool canOpen = false;
	//能否开启
	public int needBatteryNum;


}