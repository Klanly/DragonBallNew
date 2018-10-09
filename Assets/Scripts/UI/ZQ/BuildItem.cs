using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BuildItem : MonoBehaviour 
{
	private BuildGetUI m_getUI;					//
	private BuildTipUI m_tipUI;					//提示UI
	public ProduceMoneyTip showMoneyTip; 
//	private BuildWorkUI m_workUI;
	private GameObject m_fire;					//
	[HideInInspector]
	public bool m_bRobTipOpened = false;		//抢夺提示是否打开过
	private Transform m_meat;
	private bool m_bIsAnimPlaying = false;

	private Transform m_firePar;

//	private const string BASE_PREFAB_PATH = "Build/Prefab/";
	private const string BASE_PREFAB_PATH = "Build_New/";
	private const string FIRE = "UI_FIRE";
	private const string FIRE_PATH = "810001_1/BoneRoot";
	public GameObject m_Lighting = null;

	public int buildNum
	{
		get 
		{
			string[] strText = this.name.Split ('_');
			return int.Parse (strText [0]);
		}
	}

	public int buildID
	{
		get 
		{
			string[] strText = this.name.Split ('_');
			if(strText.Length == 2)
			{
				return int.Parse (strText [1]);
			}
			return 0;
		}
	}

	public Building m_buildData
	{
		get
		{
			Building data = Core.Data.BuildingManager.GetBuildFromBagByNum(buildNum);
			if(data == null)
			{
				RED.Log("  id   " + buildID);
				RED.Log("  num   " + buildNum);
			}

			return data;
		}
	}

	void Start()
	{
		InitUI ();
		UpdateBuildState();
		if (m_buildData.config.build_kind == BaseBuildingData.BUILD_KIND_PRODUCE
		   || m_buildData.config.build_kind == BaseBuildingData.BUILD_KIND_BATTLE)
		{
			InvokeRepeating ("CheckBuildGetState", 0, 1);
		}
		if (m_buildData.config.build_kind == BaseBuildingData.BUILD_KIND_PRODUCE) {
			showMoneyTip = transform.GetComponentInChildren<ProduceMoneyTip> ();
			showMoneyTip.gameObject.SetActive (false);
		}


		if (buildNum == BaseBuildingData.Q_phD)
		{
			VipInfoData vip = Core.Data.vipManager.GetVipInfoData (Core.Data.playerManager.RTData.curVipLevel);
	
			if (vip != null)
			{
				bool isVisib = vip.iqshow > 0 && Core.Data.playerManager.RTData.curLevel >= m_buildData.config.limitLevel;
				RED.SetActive (isVisib, this.gameObject);
			}
			else
			{
				RED.SetActive (false, this.gameObject);
			}
		}
	}


	public void CheckLighting(){
		if (m_Lighting == null) {
			if (m_buildData.config.id == BaseBuildingData.BUILD_FRAGMENT) {
				FragmentLighting tLight = gameObject.transform.GetComponentInChildren<FragmentLighting> ();
				if (tLight != null)
					m_Lighting = tLight.gameObject;
				else {
					m_Lighting = gameObject.transform.FindChild ("820010_1").FindChild("BuildingLight").gameObject;
				}
			}
		}
	}

		
	void CheckBuildGetState()
	{
		if (m_bIsAnimPlaying)
		{
			return;
		}

		if(m_buildData.config.build_kind == BaseBuildingData.BUILD_KIND_PRODUCE
			|| m_buildData.config.build_kind == BaseBuildingData.BUILD_KIND_BATTLE) 
		{
			if (m_buildData.fTime <= DateHelper.UnixTimeStampToDateTime (Core.TimerEng.curTime)) 
			{
				m_buildData.RTData.dur  = 0;
				UpdateBuildState ();
			}
		}
	}

	void InitUI()
	{
		if(m_buildData != null)
		{
			this.name = m_buildData.config.ID + "_" + m_buildData.RTData.id.ToString();
		}
	}

	void Clear()
	{
		for(int i = 0; i < transform.childCount; i++)
		{
			Destroy(transform.GetChild(i).gameObject);
		}

		m_getUI = null;
//		m_workUI = null;
		m_tipUI = null;
		showMoneyTip = null;
	}


	void UpdateRobFire()
	{
		if (m_buildData.config.build_kind == BaseBuildingData.BUILD_KIND_PRODUCE)
		{
			if (m_buildData.RTData.openType != 0 && m_buildData.RTData.robc > 0 && m_fire == null)
			{
				UnityEngine.Object firePrefab = PrefabLoader.loadFromPack (BASE_PREFAB_PATH + FIRE, false);
				m_fire = Instantiate (firePrefab) as GameObject;
				RED.AddChild (m_fire, this.gameObject, new Vector3 (0.4f, 1.6f, 0.1f));
				if (m_meat == null)
				{
					m_meat = transform.FindChild ("830001_1/Object171");
				}
				if (m_meat != null)
				{
					RED.SetActive (false, m_meat.gameObject);
				}
			}
		}
	}

	void CreateBuild()
	{
		Clear();

		string strPath = GetPrefabName();
		UnityEngine.Object obj = PrefabLoader.loadFromPack(BASE_PREFAB_PATH + strPath, false);

		if(obj == null)
		{
			RED.LogWarning(BASE_PREFAB_PATH + strPath + "is not find");
			return;
		}

		GameObject build = Instantiate(obj) as GameObject;
		build.name = strPath;
		RED.AddChild(build, this.gameObject);
	
		build.transform.localPosition = Vector3.zero;

		if(m_buildData != null)
		{
			this.name = m_buildData.config.ID + "_" + m_buildData.RTData.id.ToString();
		}

//		if(m_buildData.config.build_kind == BaseBuildingData.BUILD_KIND_PRODUCE)
//		{
//			if(m_buildData.RTData.robc > 0)
//			{
//				UnityEngine.Object firePrefab = PrefabLoader.loadFromPack(BASE_PREFAB_PATH + FIRE, false);
//				m_fire = Instantiate(firePrefab) as GameObject;
//				RED.AddChild(m_fire, this.gameObject, new Vector3(0.4f, 1.6f, 0.1f));
//				if (m_meat == null)
//				{
//					m_meat = transform.FindChild ("830001_1/Object171");
//				}
//				if (m_meat != null)
//				{
//					RED.SetActive (false, m_meat.gameObject);
//				}
//			}
//		}

		UpdateRobFire ();

		CheckLighting ();
		if (m_buildData.config.build_kind != BaseBuildingData.BUILD_KIND_NORMAL)
		{
			m_getUI = transform.GetComponentInChildren<BuildGetUI>();
			if (m_getUI != null)
			{
				m_getUI.InitUI (m_buildData, this);
			}
			else
			{
				RED.LogWarning ("build get ui not find  " + m_buildData.config.id);
			}
		}


		m_tipUI = transform.GetComponentInChildren<BuildTipUI>();

		if(m_tipUI != null)
		{
			m_tipUI.InitName(m_buildData);
		}
	}
		
	string GetPrefabName()
	{
		string strPath =  m_buildData.config.ID + "_1";
		return strPath;
	}
	
	private int GetStep()
	{
		int step = 1; 
		switch(m_buildData.config.build_kind)
		{
			case BaseBuildingData.BUILD_KIND_NORMAL:
				step = 1;
				break;
			case BaseBuildingData.BUILD_KIND_BATTLE:
			case BaseBuildingData.BUILD_KIND_PRODUCE:
				if(m_buildData.RTData.lv < 4)
				{
					step = 1;
				}
				else if(m_buildData.RTData.lv >= 4 && m_buildData.RTData.lv <7 )
				{
					step = 2;
				}
				else if(m_buildData.RTData.lv >= 7)
				{
					step =3;
				}
				break;
		}
		return step;
	}

	public void UpdateBuildState()
	{
		string strPath = GetPrefabName ();
		if (!this.transform.FindChild (strPath))
		{
			CreateBuild ();
		}
		else
		{
			if(m_getUI != null)
			{
				m_getUI.InitUI(m_buildData, this);
			}

			if (buildNum == BaseBuildingData.Q_phD)
			{
				VipInfoData vip = Core.Data.vipManager.GetVipInfoData (Core.Data.playerManager.RTData.curVipLevel);
				if (vip != null)
				{
					bool isVisib = vip.iqshow > 0 && Core.Data.playerManager.RTData.curLevel >= m_buildData.config.limitLevel;
					RED.SetActive (isVisib, this.gameObject);
				}
				else
				{
					RED.SetActive (false, this.gameObject);
				}
			}

			m_tipUI = transform.GetComponentInChildren<BuildTipUI>();

			UpdateRobFire ();

			if(m_tipUI != null)
			{
				m_tipUI.InitName(m_buildData);
			}

			if(m_buildData.config.build_kind == BaseBuildingData.BUILD_KIND_BATTLE)
			{
				if(m_firePar == null)
				{
					m_firePar = transform.FindChild(FIRE_PATH);
				}
				if(m_firePar != null)
				{
					RED.SetActive(m_buildData.RTData.dur > 0, m_firePar.gameObject);
				}
			}

			CheckLighting ();
		}
	}
		
	public void OnTouchUp()
	{
		this.transform.localScale = Vector3.one;
	}

	private void OnClick()
	{
		if(!Core.Data.temper.TempTouch)return;
		
		if(!DBUIController.mDBUIInstance.m_bCanClick)
		{
			return;
		}

		if (m_bIsAnimPlaying)
		{
			return;
		}
        Core.Data.soundManager.BtnPlay(ButtonType.Confirm);// yangchenguang 点击建筑音效

		ClickBuild ();
	}


	public void ShowRobUI()
	{
		m_bRobTipOpened = true;

		float leftRate = 1.0f - (float)(m_buildData.RTData.robRate / 100);
		int leftCoin = (int)(m_buildData.config.GetCoin * leftRate);
		int leftStone = (int)(m_buildData.config.GetStone * leftRate);
		RobUI.OpenUI (Core.Data.stringManager.getString (5215), (int)m_buildData.config.GetCoin, leftCoin, (int)m_buildData.config.GetStone, leftStone);
	}

	public void ClickBuild()
	{

		if (Core.Data.playerManager.RTData.curLevel < m_buildData.config.limitLevel && m_buildData.config.limitLevel >= 0)
		{
			string strText = Core.Data.stringManager.getString (6054);
			strText = strText.Replace ("#", m_buildData.config.limitLevel.ToString());
			SQYAlertViewMove.CreateAlertViewMove (strText);
			return;
		}

		if (m_buildData.config.build_kind == BaseBuildingData.BUILD_KIND_PRODUCE)
		{
			if ( m_buildData.RTData.openType != 0 && m_buildData.RTData.robc > 0 && !m_bRobTipOpened)
			{
				ShowRobUI ();
				return;
			}
		}

		if (m_buildData.config.id == BaseBuildingData.BUILD_ZHAOMU) {
			if (Core.Data.BuildingManager.ZhaoMuUnlock) {
				ZhaoMuUI.OpenUI ();
				DBUIController.mDBUIInstance.HiddenFor3D_UI ();
			} else {
				string strText = Core.Data.stringManager.getString (9111);
				strText = string.Format (strText, RED.GetChineseNum (4));
				SQYAlertViewMove.CreateAlertViewMove (strText);
			}
		} else if (m_buildData.config.id == BaseBuildingData.BUILD_XUNLIAN) {
			TrainingRoomUI.OpenUI ();
			DBUIController.mDBUIInstance.HiddenFor3D_UI ();
		} else if (m_buildData.config.id == BaseBuildingData.BUILD_YELIAN) {
			FrogingSystem.ForgingRoomUI.OpenUI ();
			DBUIController.mDBUIInstance.HiddenFor3D_UI ();
		} else if (m_buildData.config.id == BaseBuildingData.BUILD_TREE) {
			string strText = Core.Data.stringManager.getString (5116);
			SQYAlertViewMove.CreateAlertViewMove (strText);
			return;
		} else if (m_buildData.config.id == BaseBuildingData.BUILD_SHOP) {
			SecretShopMgr.GetInstance ().SetSecretShop (true, 1);
			DBUIController.mDBUIInstance.HiddenFor3D_UI ();
		} else if (m_buildData.config.id == BaseBuildingData.Q_phD) {
			SecretShopMgr.GetInstance ().SetSecretShop (true, 2);
			DBUIController.mDBUIInstance.HiddenFor3D_UI ();
		} else if (m_buildData.config.id == BaseBuildingData.BUILD_MailBox) {
			MailBox.OpenUI (1);
			DBUIController.mDBUIInstance.HiddenFor3D_UI ();
		} else if (m_buildData.config.id == BaseBuildingData.BUILD_FUBEN) {
			DBUIController.mDBUIInstance.OnBtnMainViewID (SQYMainController.CLICK_FuBen);
		} else if (m_buildData.config.id == BaseBuildingData.BUILD_CHALLENGE) {
			DBUIController.mDBUIInstance.OnBtnMainViewID (SQYMainController.CLICK_DuoBao);
		} else if (m_buildData.config.id == BaseBuildingData.BUILD_FRAGMENT) {
			DBUIController.mDBUIInstance.SetViewState (RUIType.EMViewState.S_Bag, RUIType.EMBoxType.LOOK_MonFrag);
		}  else if (m_buildData.config.build_kind == BaseBuildingData.BUILD_KIND_PRODUCE) {
			Debug.Log ("  build_kind  " + m_buildData.config.build_kind);
			UISourceBuilding.OpenUI (m_buildData);
		}else   {
			BuildingLvUpNew.OpenUI (m_buildData);
			//BuildLvlUpUI.OpenUI(m_buildData);
		}
	}

	public void OnTouchDown()
	{
		this.transform.localScale = Vector3.one * 1.1f;
	}

	void SendGetCoinOrEnergy()
	{
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.BUILD_GET, new BuildGetParam(Core.Data.playerManager.PlayerID, m_buildData.RTData.id));

		task.ErrorOccured += testHttpResp_Error;
		task.afterCompleted += testHttpResp_UI;

		task.DispatchToRealHandler();

		ComLoading.Open ();
	}


	void testHttpResp_UI (BaseHttpRequest request, BaseResponse reponse)
	{
		ComLoading.Close ();
		if (reponse.status != BaseResponse.ERROR) 
		{
			HttpRequest req = request as HttpRequest;
			switch(req.Type)
			{
				case RequestType.BUILD_GET:
				{
					BuildOperateResponse resp = reponse as BuildOperateResponse;
					int[] getMoney = new int[2]{ resp.data.coin, resp.data.stone };
					GetProductionSuc();
					this.ShowEffect (getMoney);
					break;
				}
//				case RequestType.BUILD_CREATED:
//				{
//					BuildCreateSuc();
//					break;
//				}
			}
		} 
		else 
		{
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getNetworkErrorString(reponse.errorCode));
		}
	}

	public void GetProductionSuc()
	{
		m_getUI.GetSuccess();
		m_getUI.UpdateUI(m_buildData);
		UpdateBuildState();
		DBUIController.mDBUIInstance._playerViewCtl.freshPlayerInfoView ();
		m_buildData.RTData.robc = 0;
		if(m_fire != null)
		{
			Destroy(m_fire);
			m_fire = null;
			if (m_meat == null)
			{
				m_meat = transform.FindChild ("830001_1/Object171");
			}
			if (m_meat != null)
			{
				RED.SetActive (true, m_meat.gameObject);
			}
		}
	}

	public void UpdateOpenProduction(){
		m_getUI.UpdateUI(m_buildData);
		DBUIController.mDBUIInstance._playerViewCtl.freshPlayerInfoView ();
	}

//	public void BuildCreateSuc()
//	{
//		DBUIController.mDBUIInstance._playerViewCtl.freshPlayerInfoView ();
//		Invoke("UpdateBuildState", 2.5f);
//		Invoke("PlayCreateBuildSound", 2.5f);
//		m_workUI.PlayAnim();
//		if(Core.Data.guideManger.isGuiding)
//		{
//			UIGuide.Instance.DelayAutoRun(3);
//		}
//		Core.Data.soundManager.SoundFxPlay(SoundFx.FX_BuildingLevelUp);
//		StartCoroutine ("StarPlayAnim", 3f);
//	}

	public void BuildUpgradeSuc()
	{
		DBUIController.mDBUIInstance._playerViewCtl.freshPlayerInfoView ();
		Clear();
		CreateWorkdingUI();
		//m_workUI.PlayAnim();
		Invoke("UpdateBuildState", 3f);
		Invoke("PlayCreateBuildSound", 3f);
		Core.Data.soundManager.SoundFxPlay(SoundFx.FX_BuildingLevelUp);
		StartCoroutine ("StarPlayAnim", 3f);
	}


	IEnumerator StarPlayAnim(float time)
	{
		m_bIsAnimPlaying = true;
		yield return new WaitForSeconds (time);
		m_bIsAnimPlaying = false;
	}

	public void CheckAnimPlaying()
	{
		if (m_bIsAnimPlaying)
		{
			StopAllCoroutines ();
			CreateBuild ();
			m_bIsAnimPlaying = false;
		}
	}

	void PlayCreateBuildSound()
	{
		Core.Data.soundManager.SoundFxPlay(SoundFx.FX_Building_Done);
	}
	
	void testHttpResp_Error (BaseHttpRequest request, string error)
	{
		ComLoading.Close();
		ConsoleEx.DebugLog ("---- Http Resp - Error has ocurred." + error);
	}

	void SetChildShow(bool bShow)
	{
		for(int i = 0; i < transform.childCount; i++)
		{
			RED.SetActive (bShow, transform.GetChild (i).gameObject);
		}
	}

	public void OnClickGet()
	{
		if(!DBUIController.mDBUIInstance.m_bCanClick)
		{
			return;
		}
			
		SendGetCoinOrEnergy();
		Core.Data.soundManager.SoundFxPlay(SoundFx.FX_CoinFromBuilding);
	}

	public void CreateWorkdingUI()
	{
		UnityEngine.Object prefab = PrefabLoader.loadFromPack(BASE_PREFAB_PATH + "Smoke", false);
		if (prefab != null)
		{
			GameObject obj = Instantiate (prefab) as GameObject;
			RED.AddChild(obj, this.gameObject);
//			m_workUI = obj.GetComponent<BuildWorkUI>();
		}
		else
		{
			RED.LogWarning(BASE_PREFAB_PATH +  "Smoke" + "not find");
		}
	}

	public void ShowEffect(int[] tInt){
		//int[] tInt = new int[2]{10000,300};
		showMoneyTip.InitInfo (tInt);
	}




}
	
