using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using System;

public class BuildingLvUpNew : RUIMonoBehaviour
{
	private Building m_data;
	public UIButton btn_Open;
	public UIButton btn_Update;
	public UILabel lbl_Desp;
	public UILabel lbl_UpdateMoney;
	public UILabel lbl_NeedLv;
	public UISprite sp_Coin;
	public UILabel lbl_GetBtn;
	public UILabel title_Get;

	#region 作战

	public UILabel lbl_TitleName;
	public UILabel lbl_TitleLv;
	public UILabel lbl_Atk;
	public UILabel lbl_Def;
	public UILabel lbl_Rate;
	public UILabel lbl_OpenMoney;
	public GameObject battleObj;
	private bool isOpen = false;

	Vector3 timePos  = new Vector3(-100,-211,0);
	Vector3 norPos  = new Vector3(-45,-211,0);
	#endregion

	#region 产出

	public UILabel lbl_ProduceCoin;
	public UILabel lbl_ProduceStone;
	public GameObject produceObj;

	#endregion

	public void Awake ()
	{
		_mInstance = this;
	}

	private static BuildingLvUpNew _mInstance;

	public static BuildingLvUpNew mInstance {
		get {
			return _mInstance;
		}
	}

	public static void OpenUI (Building buildData)
	{
		if (_mInstance == null) {
			UnityEngine.Object prefab = PrefabLoader.loadFromPack ("ZQ/BuildLvlUpNewUI");
			if (prefab != null) {
				GameObject obj = Instantiate (prefab) as GameObject;
				RED.AddChild (obj, DBUIController.mDBUIInstance._TopRoot);
				_mInstance = obj.GetComponent<BuildingLvUpNew> ();
				_mInstance.m_data = buildData;
				RED.TweenShowDialog (obj);
			}
		} else {
			_mInstance.SetShow (true);
			_mInstance.m_data = buildData;
			_mInstance.InitUI ();
		}
	}

	void Start ()
	{
		InitUI ();
	}

	void InitUI ()
	{
		if (m_data != null) {
			if (m_data.RTData.dur > 0) {
				InvokeRepeating ("TimerCounting", 0, 1f);
				btn_Open.isEnabled = false;
				isOpen = false;                                      

			} else {
				m_data.RTData.dur  = 0;
				btn_Open.isEnabled = true;
				isOpen = true;
			}
		}
		this.Refresh ();
	}

	public void SetShow (bool bShow)
	{
		RED.SetActive (bShow, this.gameObject);
	}

	public void Refresh ()
	{
		if (m_data != null) {
			StringManager tSM = Core.Data.stringManager;
			BuildingManager tBM = Core.Data.BuildingManager;

			if (m_data.config.build_kind == BaseBuildingData.BUILD_KIND_BATTLE) {
				battleObj.SetActive (true);
				produceObj.SetActive (false);
				sp_Coin.gameObject.SetActive (true);
				StringBuilder sAtk = new StringBuilder (tSM.getString (5103));
				sAtk.Append ("+");
				sAtk.Append (m_data.config.GetAtk.ToString());
				sAtk.Append ("%");
				lbl_Atk.text = sAtk.ToString ();
				StringBuilder sDef = new StringBuilder (tSM.getString (5104));
				sDef.Append ("+");
				sDef.Append (m_data.config.GetDef.ToString());

				sDef.Append ("%");
				lbl_Def.text = sDef.ToString ();

				StringBuilder sRate = new StringBuilder (tSM.getString (7400));
				sRate.Append ("+");
				sRate.Append ( m_data.config.GetRate.ToString());
				sRate.Append ("%");
				lbl_Rate.text = sRate.ToString ();

				StringBuilder sName = new StringBuilder (m_data.config.name);

				StringBuilder sCurBuildingLv = new StringBuilder (m_data.RTData.lv.ToString());

				sCurBuildingLv.Append (tSM.getString (7147)); 

				lbl_TitleName.text = sName.ToString ();
				lbl_TitleLv.text = sCurBuildingLv.ToString ();
				lbl_OpenMoney.text = m_data.config.OpenBattleCostCoin.ToString ();
				lbl_OpenMoney.transform.localPosition = norPos;

				BaseBuildingData tNextBuildData = tBM.GetConfigByBuildLv (m_data.config.id,m_data.config.Lv + 1);
				if (tNextBuildData != null) {
					lbl_NeedLv.text = string.Format (tSM.getString (7401), tNextBuildData.limitLevel.ToString());
					lbl_UpdateMoney.text = tNextBuildData.UpCostCoin.ToString (); 
				}

				lbl_GetBtn.text = tSM.getString (7402);
				lbl_Desp.text = tSM.getString (7403);


				if (isOpen == false) {
					title_Get.text = tSM.getString (7405);
					sp_Coin.gameObject.SetActive (false);
				} else {
					title_Get.text = tSM.getString (7367);
				}

			} else if (m_data.config.build_kind == BaseBuildingData.BUILD_KIND_PRODUCE) {
				battleObj.SetActive (false);
				produceObj.SetActive (true);
				sp_Coin.gameObject.SetActive (false);
				float leftRate = 100.0f - m_data.RTData.robc ;

				int coin = (int)(m_data.config.GetCoin * leftRate / 100.0f);
				lbl_ProduceCoin.text = coin.ToString ();

				int stone =(int) (m_data.config.GetStone * leftRate / 100.0f);
				lbl_ProduceStone.text = stone.ToString ();

				StringBuilder sName = new StringBuilder (m_data.config.name);

				StringBuilder sCurBuildingLv = new StringBuilder (m_data.RTData.lv.ToString());
				sCurBuildingLv.Append (tSM.getString (7147)); 

				lbl_TitleName.text = sName.ToString ();
				lbl_TitleLv.text = sCurBuildingLv.ToString ();

				BaseBuildingData tNextBuildData = tBM.GetConfigByBuildLv (m_data.config.id,m_data.config.Lv + 1);
				if (tNextBuildData != null) {
					lbl_NeedLv.text = string.Format (tSM.getString (7401), tNextBuildData.limitLevel.ToString());
					lbl_UpdateMoney.text = tNextBuildData.UpCostCoin.ToString ();
				}

				lbl_OpenMoney.text = "00:00:00";
				lbl_OpenMoney.transform.localPosition = timePos;
				lbl_GetBtn.text = tSM.getString (25098);
				lbl_Desp.text = tSM.getString (7404);
				title_Get.text = tSM.getString (7405);
			}

			//能否升级   解锁等级 
			btn_Update.isEnabled = Core.Data.BuildingManager.CanLvlUp (m_data.config.id, m_data.RTData.lv +1) ;
		}
	}

	void OnClose ()
	{
		Destroy (this.gameObject);
	}

	void OnReceiveBtn ()
	{
		if (isOpen == true) {
			isOpen = false;
			btn_Open.isEnabled = isOpen;
			this.SendGetNowMsg ();
		}

	}

	void OnOpenBattleBtn ()
	{
		if (isOpen == true) {
			isOpen = false;
			btn_Open.isEnabled = isOpen;
			this.SendOpenBattleBuildMsg ();

		}

	}

	void UpBtn ()
	{
		if (m_data.config.build_kind == BaseBuildingData.BUILD_KIND_BATTLE) {
			OnOpenBattleBtn ();
		} else if (m_data.config.build_kind == BaseBuildingData.BUILD_KIND_PRODUCE) {
			OnReceiveBtn ();
		}
	}

	void OnUpdateBuildingBtn ()
	{
		BaseBuildingData nextLv = Core.Data.BuildingManager.GetConfigByBuildLv (m_data.RTData.num, m_data.RTData.lv + 1);
		if (nextLv == null) {
			RED.LogWarning (m_data.RTData.id + " not find next level build info!   " + m_data.RTData.lv);
			return;
		}

		if (nextLv.UpCostCoin != 0 && Core.Data.playerManager.RTData.curCoin < nextLv.UpCostCoin) {
			JCRestoreEnergyMsg.OpenUI (ItemManager.COIN_PACKAGE, ItemManager.COIN_BOX, 2);
			return;
		} else if (nextLv.UpCostStone != 0 && Core.Data.playerManager.RTData.curStone < nextLv.UpCostStone) {
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (35006));
			return;
		}

		//talkingdata add by wxl 
		if (nextLv.UpCostStone != 0) {
			Core.Data.ActivityManager.OnPurchaseVirtualCurrency (ActivityManager.BuildOpenType, 1, nextLv.UpCostStone);
		}


		btn_Update.isEnabled = false;
		HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.BUILD_UPGRADE, new  BuildUpgradeParam (Core.Data.playerManager.PlayerID, m_data.RTData.id));
		task.ErrorOccured += testHttpResp_Error;
		task.afterCompleted += testHttpResp_UI;
		task.DispatchToRealHandler ();
		ComLoading.Open ();

	}

	void testHttpResp_UI (BaseHttpRequest request, BaseResponse reponse)
	{
		ComLoading.Close ();
		if (reponse.status != BaseResponse.ERROR) {
			HttpRequest req = request as HttpRequest;
			switch (req.Type) {
			case RequestType.BUILD_UPGRADE:
				Core.Data.playerManager.RTData.curTeam.QianliXunlianMember ();
				m_data = Core.Data.BuildingManager.GetBuildFromBagByID (m_data.RTData.id);
				Refresh ();
				BuildScene.mInstance.BuildUpgradeSuc (m_data.RTData.id);
				SetShow (false);
				break;
			case RequestType.BUILD_GET:

				BuildGetParam param = req.ParamMem as BuildGetParam;
				//	BuildOperateResponse tResp = reponse as BuildOperateResponse;
				SetShow (false);
				BuildScene.mInstance.GetProductionSuc (param.bid);
				BuildScene.mInstance.UpdateBuildById (m_data.RTData.id);
				break;
			case RequestType.BATTLE_BUILD_OPEN:
				Core.Data.playerManager.RTData.curTeam.QianliXunlianMember ();
				string strText = Core.Data.stringManager.getString (5113);
				strText = string.Format (strText, m_data.config.name);
				SQYAlertViewMove.CreateAlertViewMove (strText);
				InitUI ();
				BuildScene.mInstance.UpdateBuildById (m_data.RTData.id);
				break;
			}
			DBUIController.mDBUIInstance.RefreshUserInfo ();
		} else {

			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getNetworkErrorString (reponse.errorCode));
			btn_Update.isEnabled = Core.Data.BuildingManager.CanLvlUp (m_data.config.id, m_data.RTData.lv +1) ;
		}

	}

	void testHttpResp_Error (BaseHttpRequest request, string error)
	{
		ComLoading.Close ();
		ConsoleEx.DebugLog ("---- Http Resp - Error has ocurred." + error);
	}

	void TimerCounting ()
	{
		if (m_data.config.build_kind == BaseBuildingData.BUILD_KIND_PRODUCE) {
			string leftTimeTxt = "00:00:00";
			if (m_data.RTData.dur > 0) {
				TimeSpan span = m_data.fTime - DateHelper.UnixTimeStampToDateTime (Core.TimerEng.curTime);
				if (span.TotalSeconds > 0) {
					DateTime leftTime = Convert.ToDateTime (span.Hours.ToString () + ":" + span.Minutes.ToString () + ":" + span.Seconds.ToString ());
					leftTimeTxt = leftTime.ToLongTimeString ();
					lbl_OpenMoney.text = leftTimeTxt;
					lbl_OpenMoney.transform.localPosition = timePos;
				}
			} else {
				this.FinishCounting ();
			}
		} else if (m_data.config.build_kind == BaseBuildingData.BUILD_KIND_BATTLE) {
			string leftString = "00:00:00";
			if (m_data.RTData.dur > 0) {
				TimeSpan span = m_data.fTime - DateHelper.UnixTimeStampToDateTime (Core.TimerEng.curTime);
				if (span.TotalSeconds > 0) {
					DateTime leftTime = Convert.ToDateTime (span.Hours.ToString () + ":" + span.Minutes.ToString () + ":" + span.Seconds.ToString ());
					leftString = leftTime.ToLongTimeString ();
					lbl_OpenMoney.text = leftString;
					lbl_OpenMoney.transform.localPosition = timePos;
				}
			} else {
				this.FinishCounting ();
			}
		}
	}

	void FinishCounting(){
		CancelInvoke ("TimerCounting");
		btn_Open.isEnabled = true;
		isOpen = true;
		m_data.RTData.dur = 0;

		if (m_data.config.build_kind == BaseBuildingData.BUILD_KIND_BATTLE) {
			title_Get.text =  Core.Data.stringManager.getString (7367);
			sp_Coin.gameObject.SetActive (true);
			this.Refresh ();
		} else if (m_data.config.build_kind == BaseBuildingData.BUILD_KIND_PRODUCE) {
			title_Get.text = Core.Data.stringManager.getString (25098);
			lbl_OpenMoney.text = "00:00:00";
			lbl_OpenMoney.transform.localPosition = timePos;
		}
	}

	void SendOpenBattleBuildMsg ()
	{
		int tCoin = m_data.config.OpenBattleCostCoin;
		if (m_data.config.build_kind == BaseBuildingData.BUILD_BATTLE) {
			if (Core.Data.playerManager.RTData.curCoin < tCoin) {
				JCRestoreEnergyMsg.OpenUI (ItemManager.COIN_PACKAGE, ItemManager.COIN_BOX, 2);
				return;
			}
		} 
		HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.BATTLE_BUILD_OPEN, new  BattleBuildOpenParam (Core.Data.playerManager.PlayerID, m_data.RTData.id, 1));
		task.ErrorOccured += testHttpResp_Error;
		task.afterCompleted += testHttpResp_UI;
		task.DispatchToRealHandler ();

		ComLoading.Open ();
	}

	public void SendGetNowMsg ()
	{
		HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.BUILD_GET, new  BuildGetParam (Core.Data.playerManager.PlayerID, m_data.RTData.id));
		task.ErrorOccured += testHttpResp_Error;
		task.afterCompleted += testHttpResp_UI;
		task.DispatchToRealHandler ();

		ComLoading.Open ();
	}
}
