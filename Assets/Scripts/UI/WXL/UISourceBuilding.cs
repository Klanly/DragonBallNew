using UnityEngine;
using System.Collections;
using System;

public class UISourceBuilding : RUIMonoBehaviour {

	private Building m_data;
	public BatteryOpenItem[] batteryItems;		//由上至下	3大  2中  1小 	
	public UIButton btn_Collect;
	public UILabel lbl_needNum;
	public UILabel lbl_btnTitle;
	public UISprite sp_money;
	bool needStone = false; 
	public int needStoneNum = 100;
	public readonly int baseItem = 110181;
	private static UISourceBuilding _mInstance;

	public static UISourceBuilding mInstance {
		get {
			return _mInstance;
		}
	}

	public static void OpenUI (Building buildData)
	{
		if (_mInstance == null) {
			UnityEngine.Object prefab = WXLLoadPrefab.GetPrefab (WXLPrefabsName.UISourceBuildingPanel);
			if (prefab != null) {
				GameObject obj = Instantiate (prefab) as GameObject;
				RED.AddChild (obj, DBUIController.mDBUIInstance._TopRoot);
				_mInstance = obj.GetComponent<UISourceBuilding> ();
				_mInstance.m_data = buildData;
				_mInstance.InitUI ();
				RED.TweenShowDialog (obj);
			}
		} else {
			_mInstance.SetShow (true);
			_mInstance.m_data = buildData;
			_mInstance.InitUI ();
		}
	}


	public void Awake ()
	{
		_mInstance = this;
	}

	void SetShow(bool bShow){
		RED.SetActive (bShow, this.gameObject);
	}

	public void InitUI(){
		if (m_data != null) {
			if (m_data.config.build_kind == BaseBuildingData.BUILD_KIND_PRODUCE) {
				this.InitData ();
			}

			this.Refresh ();
			if(!IsInvoking("SetBtnLbl")){
				InvokeRepeating ("SetBtnLbl",0,1);
			}
		}
	}


	public void SetData(Building tData){
		if (tData != null) {
			m_data = tData;
			this.InitUI ();
		}
	
	}

	void InitData(){
		for (int i = 0; i < batteryItems.Length; i++) {
			BatteryData bData = new BatteryData ();
			bData.batteryType = i + 1;
			if (m_data != null) {
				bData.buildingId = int.Parse(m_data.config.ID);
				bData.buildingPid = m_data.RTData.id;
				BaseBuildingData bbData =  Core.Data.BuildingManager.GetConfigByBuildLv (m_data.config.id, bData.batteryType);
				if (bbData != null) {
					bData.moneyNum = (int)bbData.GetCoin ;
					bData.moneyType = 1;
					bData.needBatteryNum = bbData.OpenProduceCostItemNum;
					if (bbData.OpenProduceCostItem == m_data.RTData.openType +baseItem ) {
						Debug.Log ("  need stone Num == " +  bbData.ForceGainCost);
						needStoneNum = bbData.ForceGainCost;
					}
				}
				if (bData.batteryType == m_data.RTData.openType) {
					bData.canOpen = false;

					if (bData.dur >= 0)
						bData.dur = m_data.RTData.dur;
					
				} else {
					bData.canOpen = false;
					bData.dur = -1;
				}

				if (m_data.RTData.openType != 0) {
					if (m_data.RTData.dur >= 0) {
						needStone = true;
					} else {
						needStone = false;
					}
				} else {
					bData.canOpen = true;
					needStone = false;
				}
			}
			batteryItems [i].SetItemValue ((object)bData);
		}


	}


	void SetBtnLbl(){
		if (needStone) {
			lbl_needNum.gameObject.SetActive (true);
			lbl_needNum.text = countStone ().ToString ();
			sp_money.gameObject.SetActive (true);
			lbl_btnTitle.transform.localPosition = Vector3.right * 43; 
		} else {
			lbl_needNum.gameObject.SetActive (false);
			sp_money.gameObject.SetActive (false);
			lbl_btnTitle.transform.localPosition = Vector3.zero;
		}
	}

	int countStone(){
		TimeSpan span = m_data.fTime - DateHelper.UnixTimeStampToDateTime (Core.TimerEng.curTime);
		int resultNeedMoney = 0;
		if (span.TotalSeconds > 0) {

			resultNeedMoney = (int)(  needStoneNum * span.TotalSeconds /  m_data.config.time  + 0.5f);
			if (resultNeedMoney == 0)
				resultNeedMoney = 1;
		} else if(span.TotalSeconds == 0) {
			needStone = false;
		}
		if (needStone == true) {
			return  resultNeedMoney ;
		}
		return 0;
	}
	//刷新 方法   匹配  id  
	void  Refresh(){
		for (int i = 0; i < batteryItems.Length; i++) {
			BatteryData bData =	batteryItems [i].ReturnValue () as BatteryData;
			if (bData != null) {
				if (m_data.RTData.openType != 0) {
					if (bData.batteryType == m_data.RTData.openType) {
						bData.fTime = m_data.fTime;
						if (bData.dur >= 0) {
							bData.dur = m_data.RTData.dur;
						}
					} else {
						bData.dur = -1;
					}
					bData.canOpen = false;
					btn_Collect.isEnabled = true;
//					batteryItems [i].SetItemValue (bData);
				} else {
					bData.buildingId = int.Parse(m_data.config.ID);
					bData.buildingPid = m_data.RTData.id;
					bData.fTime = new DateTime ();
					bData.dur = -1;
					bData.canOpen = true;
					btn_Collect.isEnabled = false;
				}
				batteryItems [i].SetItemValue (bData);
			} else {
				bData = new BatteryData ();
				bData.batteryType = (i + 1);
				bData.buildingId =  int.Parse(m_data.config.ID);
				bData.buildingPid = m_data.RTData.id;

				if (bData.batteryType == m_data.RTData.openType) {
					if (bData.dur >= 0) {
						bData.dur = m_data.RTData.dur;
					}
					bData.fTime = m_data.fTime;
					bData.canOpen = false;
				} else {
					bData.dur = -1;
					bData.canOpen = false;
					bData.fTime = new System.DateTime ();
				}
				batteryItems [i].SetItemValue (bData);
			}
		}

		this.SetBtnLbl ();
	}

	//收取方法
	public void CollectItem(){
		Debug.Log (" stone ==" + countStone());
		if (CanCollect () == true) {
			//收取
			if (countStone() > 0)
				this.SendGetNowMsg (1);
			else
				this.SendGetNowMsg (0);
		}
	}


	bool CanCollect(){
		//时间 不为零
//		if (m_data.RTData.dur > 0) {
//			return false;
//		}
//
		if (Core.Data.playerManager.RTData.curStone >= countStone ()) {
			return true;
		} else {
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(7310));
			return false;
		}
		//每个cell	能否收取
//		for (int i = 0; i < batteryItems.Length; i++) {
//			BatteryData bData =	batteryItems [i].ReturnValue () as BatteryData;
//			if (bData != null) {
//				if (bData.canOpen == false) {
//					return false;
//				}
//			}
//		}
//		return  true;

	}

	public void SendGetNowMsg (int moneytype)
	{
		HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.BUILD_GET, new  BuildGetParam (Core.Data.playerManager.PlayerID, m_data.RTData.id,moneytype));
		task.afterCompleted += CollectSourceRequest;
		task.DispatchToRealHandler ();
		ComLoading.Open ();
	}

	void CollectSourceRequest (BaseHttpRequest request, BaseResponse reponse)
	{
		ComLoading.Close ();
		if (reponse != null&& reponse.status != BaseResponse.ERROR) {
			HttpRequest req = request as HttpRequest;
			BuildGetParam param = req.ParamMem as BuildGetParam;
			//	BuildOperateResponse tResp = reponse as BuildOperateResponse;
			SetShow (false);
			BuildScene.mInstance.GetProductionSuc (param.bid);
			BuildScene.mInstance.UpdateBuildById (m_data.RTData.id);
			Building data = Core.Data.BuildingManager.GetBuildFromBagByNum(int.Parse(m_data.config.ID));
			m_data = data;
			this.InitUI ();
			DBUIController.mDBUIInstance.RefreshUserInfo ();
		} else if(reponse.status == BaseResponse.ERROR){
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getNetworkErrorString(reponse.errorCode));
			btn_Collect.isEnabled = false;
			needStone = false;
		}

	}



	public void OnClose ()
	{
		Destroy (this.gameObject);
	}



}
