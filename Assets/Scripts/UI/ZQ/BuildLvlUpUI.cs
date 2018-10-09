using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Collections.Generic;

public class BuildLvlUpUI : MonoBehaviour 
{
	public UILabel m_txtName;

    //	public UILabel m_txtCur;
    //	public UILabel m_txtCurDesp;

	public UILabel m_txtNext;
    //  public UILabel m_txtNextDesp;

	public UILabel m_txtMax;
	public UILabel m_txtMaxDesp;

	public UILabel m_txtCurLvl;
	public UILabel m_txtNextLvl;

    public UILabel lblCurLv;
    public UILabel lblNextLv;

    public UILabel lblEffectDesTop;
    public UILabel lblEffectDesBot;

    public UILabel lblEfTopCurNum;
    public UILabel lblEfTopNextNum;

    public UILabel lblEfBotCurNum;
    public UILabel lblEfBotNextNum;

    public UILabel lblProduceLeftTitle;
    public UILabel lblProduceRightTitle;

	public UILabel m_txtCoin;
	public UILabel m_txtLeftTime;
    public UILabel m_txtRightTime;
	public UILabel m_txtWorkTime;

	public UIButton m_btnCoin;
	public UIButton m_btnStone;
	public UIButton m_btnOK;

	public UISprite m_spCoin;
	public UISprite m_spBuild;

	public UIButton m_btnLvlUp;
	public GameObject m_objLock;

    public GameObject effectAreaTop;
    public GameObject effectAreaBot;
    public GameObject levelTitleArea;
    public GameObject maxLblArea;
    //右侧时间显示
    public GameObject rightTimeObj;
	
	public const string STONE = "common-0014";
	public const string COIN = "common-0013";

	private const string BTN_SEL = "Symbol 31";
	private const string BTN_UNSEL = "Symbol 32";

//	private Building m_data;
//	private int m_nType = 1;		//1 金币升级   2 钻石升级

//	private StringBuilder m_strBuild = new StringBuilder();

	private const float EFFECT_TIME_COIN = 1f;
	private const float EFFECT_TIME_STONE = 1.25f;

	#region UI
//    private int oneLineVLength =  250;
//    private Vector3 oneLinePosY = new Vector3(54,-178.3f,0);
//    private int twoLineVLength = 180;
//    private Vector3 twoLineProPosY = new Vector3(-60,-172f,0);
//    private Vector3 twoLineAtkPosY = new Vector3 (50,-172,0);
//    private Vector3 maxTitleTwoPos = new Vector3(-220,0,0);
//    private Vector3 maxDespTwoPos = new Vector3(-170,0,0);
//    private Vector3 maxTitleOnePos = new Vector3(-228,0,0);
//    private Vector3 maxDespOnePos = new Vector3(50,0,0);
//    private int maxDespTwoLength = 410;
//    private int maxDespOneLength = 250;
	public UILabel lblLevelTitle;
	public UISprite spLine;
	public UISprite[] bgNum;
//    private int bgAtkLength = 200;
//    private int bgProLength =320;
//    private Vector3 bgLevelObjAtkPos = new Vector3(155,-128,0);
//    private Vector3 bgLevelObjProPos = new Vector3(100,-129,0);
//    private Vector3 bgAreaTwoObjProPos = new Vector3(-24f,0,0);
//    private Vector3 bgAreaTwoObjAtkPos = new Vector3(30,0,0);
	public GameObject areaOneObj;
	public GameObject areaTwoObj;
	public GameObject levelObj;
    public GameObject levelLineTwo;
    public GameObject levelLineOne;
    public UILabel[] lblDesp;
//    private int lblDespProLength = 200;
//    private int lblDespAtkLength = 230;

//    private Vector3 lvLblTitleOnePos =  new Vector3(119,12,0);
//    private Vector3 effectLblTitleOnePos = new Vector3(-22,-200,0);
//    private Vector3 maxLblOnePos = new Vector3(0,-290,0);
//    private Vector3 vLevelTitlePos = new Vector3(-375,-131,0);

	#endregion


	public void Awake()
	{
		_mInstance = this;
	}

	private static BuildLvlUpUI _mInstance;
	public static BuildLvlUpUI mInstance
	{
		get
		{
			return _mInstance;
		}
	}

	void Start()
	{
		m_btnLvlUp.TextID = 5148;
//		InitUI();
	}

	public static void OpenUI(Building buildData)
	{
		if(_mInstance == null)
		{
			UnityEngine.Object prefab = PrefabLoader.loadFromPack("ZQ/BuildLvlUpUI");
			if(prefab != null)
			{
				GameObject obj = Instantiate(prefab) as GameObject;
				RED.AddChild(obj, DBUIController.mDBUIInstance._TopRoot);
				_mInstance = obj.GetComponent<BuildLvlUpUI>();
//				_mInstance.m_data = buildData;
                RED.TweenShowDialog(obj);
			}
		}
		else
		{
			_mInstance.SetShow(true);
//			_mInstance.m_data = buildData;
			_mInstance.InitUI();
		}
	}

	void InitUI()
	{
//		Building buildData = m_data;
//		int maxLvl = Core.Data.BuildingManager.GetBuildMaxLvl(buildData.config.id);
//		int nextLvl = buildData.RTData.lv + 1;
//		BaseBuildingData nextLv = Core.Data.BuildingManager.GetConfigByBuildLv (buildData.RTData.num, nextLvl);
//
//
//
//		m_txtName.text = buildData.config.name;
//		m_txtCurLvl.text = buildData.RTData.lv.ToString ();
//
//        lblCurLv.text = "Lv"+buildData.config.Lv.ToString ();   //wxl
//        lblLevelTitle.text  =Core.Data.stringManager.getString(5173);       //wxl
//
//		nextLvl = nextLvl >= maxLvl ? maxLvl : nextLvl;
//		if(nextLvl != maxLvl){
//			lblNextLv.text = "Lv"+(nextLvl).ToString ();    //wxl
//		}else{
//			lblNextLv.text = "LvMAX";
//		}
//
//		if (nextLv != null)
//		{
//			string strText = Core.Data.stringManager.getString (5091);
//			strText = string.Format (strText, nextLv.limitLevel);
//			m_txtNextLvl.text = strText;
//		}
//		else
//		{
//			m_txtNextLvl.text = Core.Data.stringManager.getString (5181);
//		}
//
//		if(Core.Data.playerManager.RTData.curVipLevel >= 10)
//		{
//			m_btnLvlUp.isEnabled = true;
//		}
//		else
//		{
//			m_btnLvlUp.isEnabled = Core.Data.BuildingManager.CanLvlUp (m_data.config.id, buildData.RTData.lv + 1);
//		}
//
//		RED.SetActive (!m_btnLvlUp.isEnabled, m_objLock);
//
//		int step = 1;
//		if (buildData.RTData.lv > 0 && buildData.RTData.lv <= 3)
//		{
//			step = 1;
//		}
//		else if (buildData.RTData.lv > 3 && buildData.RTData.lv <= 6)
//		{
//			step = 2;
//		}
//		else if (buildData.RTData.lv > 6 && buildData.RTData.lv <= 10)
//		{
//			step = 3;
//		}
//
//		m_spBuild.spriteName = buildData.RTData.num.ToString () + "_" + step.ToString();
//		m_spBuild.MakePixelPerfect ();
//
//
//
//		if (nextLv != null)
//		{
//			if (nextLv.up_cost[0]  != 0)
//			{
//
//				m_spCoin.spriteName = COIN;
//				m_txtCoin.text = nextLv.up_cost [0].ToString ();
//			}
//			else if (nextLv.up_cost [1] != 0)
//			{
//              
//				m_spCoin.spriteName = STONE;
//				m_txtCoin.text = nextLv.up_cost [1].ToString ();
//            }else 
//            {
//                // 如果上面俩个条件不满足就改成0金币 yangchenguang
//                m_spCoin.spriteName = COIN;
//                m_txtCoin.text = "0" ;
//            }
//		}
//		else
//		{
//			m_txtCoin.text = "";
//			m_spCoin.spriteName = "";
//		}
//		
//        //	m_txtCur.text = Core.Data.stringManager.getString (5040);
//		m_txtNext.text = "Lv"+nextLvl.ToString ();
//        m_txtMax.text = "Max:";
//		
//		if(m_data.config.build_kind == BaseBuildingData.BUILD_KIND_PRODUCE)
//		{
//            rightTimeObj.SetActive (true);      //wxl
//            //	m_txtCurDesp.text = buildData.config.Description;
//			BaseBuildingData baseBuild = Core.Data.BuildingManager.GetConfigByBuildLv (buildData.config.id, nextLvl);
//            //	m_txtNextDesp.text = baseBuild.Description;
//			
//            effectAreaTop.SetActive (true);
//            effectAreaBot.SetActive (false);
//            //  wxl 
////            if (m_data.config.GetTili != 0) {
////                lblProduceRightTitle.text = string.Format (Core.Data.stringManager.getString (5162), Core.Data.stringManager.getString (5039));
////                //  strText = Core.Data.stringManager.getString (5039) + ":";
////            } else {
////                lblProduceRightTitle.text = string.Format (Core.Data.stringManager.getString (5162), Core.Data.stringManager.getString (5038));
////                //  strText = Core.Data.stringManager.getString (5038) + ":";
////            }
//
//
//
//            lblProduceLeftTitle.text = string.Format (Core.Data.stringManager.getString (5162), Core.Data.stringManager.getString (5037));      //wxl  
//
//            lblEffectDesTop.text = Core.Data.BuildingManager.GetConfigByBuildLv (m_data.config.id, m_data.config.Lv).description.Replace ("@", "");
//
//            //如果 是产出建筑物  则 第一位时间  第二位  是产出  3时间  4 体力或者精力
//            if (GetBattleBuildRate (buildData.config) != null) {
//                for (int i = 0; i < GetBattleBuildRate (buildData.config).Count; i++) {
//                    if (i == 1)
//                        lblEfTopCurNum.text =  Core.Data.stringManager.getString(5161).Replace("@", GetBattleBuildRate (buildData.config)[i].ToString());
//                }
//            }
//            if (GetBattleBuildRate (baseBuild) != null) {
//                for (int i = 0; i < GetBattleBuildRate (baseBuild).Count; i++) {
//                    if (i == 1)
//                        lblEfTopNextNum.text = Core.Data.stringManager.getString(5161).Replace("@", GetBattleBuildRate (baseBuild)[i].ToString());
//                }
//            }
//            if (buildData.config.description.Contains ("，")) {
//                effectAreaTop.SetActive (true);
//                effectAreaBot.SetActive (true);
//                string[] tStr = buildData.config.description.Split ('，'); 
//
//                lblEffectDesTop.text = tStr [0].Replace ("@", "");
//                lblEffectDesBot.text = tStr [1].Replace ("@","");
//                lblEfBotCurNum.text =  Core.Data.stringManager.getString(5174).Replace("@", GetBattleBuildRate (buildData.config)[3].ToString());
//                lblEfBotNextNum.text = Core.Data.stringManager.getString(5174).Replace("@", GetBattleBuildRate (baseBuild)[3].ToString());
//
//            } else {
//                effectAreaTop.SetActive (true);
//                effectAreaBot.SetActive (false);
//            }
//
//
//            baseBuild = Core.Data.BuildingManager.GetConfigByBuildLv (buildData.config.id, maxLvl);
//            string[] strMaxDesp = baseBuild.description.Split ('，');
//            string tResult_1 = strMaxDesp [0].Replace ("@", GetBattleBuildRate (baseBuild) [1].ToString ());
//            string tResult_2 = strMaxDesp [1].Replace ("@",Core.Data.stringManager.getString(5174).Replace("@", GetBattleBuildRate (baseBuild)[3].ToString()));
//            m_txtMaxDesp.text = tResult_1 + "," + tResult_2; 
//
//
//			RED.SetActive (false, m_btnCoin.gameObject, m_btnStone.gameObject);
//            //wxl change in 8 5
//            m_txtMax.text = Core.Data.stringManager.getString (7360);
//            m_txtMaxDesp.text = Core.Data.stringManager.getString (7359);
//
//
//            m_btnOK.TextID = 5035;
//			m_btnOK.isEnabled = true;
//			m_txtWorkTime.text = "";
//		}
//		else if (buildData.config.build_kind == BaseBuildingData.BUILD_KIND_BATTLE)
//		{
//            rightTimeObj.SetActive (false);     //wxl
//			RED.SetActive (true, m_btnCoin.gameObject, m_btnStone.gameObject);
//			m_btnOK.TextID = 5107;
//			m_txtLeftTime.text = "";
//            m_txtRightTime.text = "";
//			m_txtWorkTime.text = Core.Data.stringManager.getString(5110);
//			m_btnCoin.TextID = 5108;
//			m_btnStone.TextID = 5109;
//			
//            lblProduceLeftTitle.text = Core.Data.stringManager.getString (5163);        //wxl
//        
//
//            //		m_txtCurDesp.text = GetBattleBuildDesp(buildData.config);
//            //下一等级
//            BaseBuildingData baseBuild = Core.Data.BuildingManager.GetConfigByBuildLv (buildData.config.id, nextLvl);
//
//            if (GetBattleBuildDesp(buildData.config).Contains ("，")) {
//                effectAreaTop.SetActive (true);
//                effectAreaBot.SetActive (true);
//          
//
//                string[] tStr = baseBuild.description.Split ('，'); 
//
//                lblEffectDesTop.text = tStr [0].Replace ("@%", "");
//                lblEffectDesBot.text = tStr [1].Replace ("@%","");
//                if (GetBattleBuildRate (buildData.config) != null) {
//                    for (int i = 0; i < GetBattleBuildRate (buildData.config).Count; i++) {
//                        if (i == 0)
//                            lblEfTopCurNum.text = GetBattleBuildRate (buildData.config) [i] + "%";
//                        if (i == 1)
//                            lblEfBotCurNum.text = GetBattleBuildRate (buildData.config) [i] + "%";
//                    }
//                }
//                if (GetBattleBuildRate (baseBuild) != null) {
//                    for (int i = 0; i < GetBattleBuildRate (baseBuild).Count; i++) {
//                        if (i == 0)
//                            lblEfTopNextNum.text = GetBattleBuildRate (baseBuild) [i] + "%";
//                        if (i == 1)
//                            lblEfBotNextNum.text = GetBattleBuildRate (baseBuild) [i] + "%";
//                    }
//                }
//            } else {
//
//                lblEffectDesTop.text = buildData.config.description.Replace("@%","");
//               
//                for (int i = 0; i < GetBattleBuildRate (buildData.config).Count; i++) {
//                    if(i == 0) lblEfTopCurNum.text = GetBattleBuildRate (buildData.config)[i] + "%";
//                }
//                for (int i = 0; i < GetBattleBuildRate (baseBuild).Count; i++) {
//                    if(i == 0) lblEfTopNextNum.text = GetBattleBuildRate (baseBuild)[i] + "%";
//                }
//
//                effectAreaBot.SetActive (false);
//            }
//
//
//            //        m_txtNextDesp.text = baseBuild.description.Replace ("@%", Core.Data.stringManager.getString (5160)); // GetBattleBuildDesp(baseBuild);
//			
//			baseBuild = Core.Data.BuildingManager.GetConfigByBuildLv (buildData.config.id, maxLvl);
//            if (baseBuild.description.Contains ("，")) {
//                string[] tDespNum = baseBuild.description.Split ('，');
//                m_txtMaxDesp.text = tDespNum [0].Replace ("@", GetBattleBuildRate (baseBuild) [0].ToString ()) + "," + tDespNum [1].Replace ("@", GetBattleBuildRate (baseBuild) [1].ToString ());
//
//            } else {
//                m_txtMaxDesp.text = GetBattleBuildDesp (baseBuild);
//            }
//          
//
//
//
//			if(m_nType == 1)
//			{
//				RED.SetBtnSprite(m_btnStone.gameObject, BTN_UNSEL);
//				RED.SetBtnSprite(m_btnCoin.gameObject, BTN_SEL);
//			}
//			else if(m_nType == 2)
//			{
//				RED.SetBtnSprite(m_btnCoin.gameObject, BTN_UNSEL);
//				RED.SetBtnSprite(m_btnStone.gameObject, BTN_SEL);
//			}
//
//			if(m_data.RTData.dur > 0)
//			{
//				m_txtWorkTime.text = "";
//				m_btnOK.isEnabled = false;
//			}
//			else
//			{
//				m_btnOK.isEnabled = true;
//			}
//		}
//		//转换ui格局
//		this.ShiftUIMode();


	}

	public void OnBtnClickOK()
	{
//		if (m_data.config.build_kind == BaseBuildingData.BUILD_KIND_BATTLE)
//		{
//			if(m_nType == 1)
//			{
//				if(Core.Data.playerManager.Coin <  m_data.config.OpenBattleCostCoin)
//				{
//                    JCRestoreEnergyMsg.OpenUI(ItemManager.COIN_PACKAGE,ItemManager.COIN_BOX,2);
//
//					//SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(35000));
//					return;
//				}
//
//				string strText = Core.Data.stringManager.getString(5114);
//				strText = string.Format(strText, m_data.config.OpenBattleCostCoin, Core.Data.stringManager.getString(5037));
//				UIInformation.GetInstance().SetInformation(strText, Core.Data.stringManager.getString(5030), SendOpenBattleBuildMsg, null);
//			}
//			else if(m_nType == 2)
//			{
//				if(Core.Data.playerManager.Stone < m_data.config.OpenBattleCostStone)
//				{
//                    SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(35006));
//					return;
//                }
////                else{
////                    // talkingdata add by wxl
////                    Core.Data.ActivityManager.OnPurchaseVirtualCurrency (ActivityManager.BuildOpenType, 1, m_data.config.OpenBattleCostStone);
////                }
//
//
//
//				string strText = Core.Data.stringManager.getString(5114);
//				strText = string.Format(strText, m_data.config.OpenBattleCostStone, Core.Data.stringManager.getString(5070));
//				UIInformation.GetInstance().SetInformation(strText, Core.Data.stringManager.getString(5030), SendOpenBattleBuildMsg, null);
//			}
//		}
//
//		else if (m_data.config.build_kind == BaseBuildingData.BUILD_KIND_PRODUCE)
//		{
//
//			string strText = Core.Data.stringManager.getString(5115);
//
//			float rate = m_data.config.rate[1] / (float)m_data.config.GetCoinCostTime;
//			float stone = rate * (float)m_data.RTData.dur [0];
//			stone += 0.5f;
//
//			if(Core.Data.playerManager.Stone < stone)
//			{
//                SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(35006));
//				return;
//            }else{
//                // talkingdata add by wxl
//                Core.Data.ActivityManager.OnPurchaseVirtualCurrency (ActivityManager.BuildOpenType, 1, stone);
//            }
//				
//			strText = string.Format (strText, (int)(stone));
//			UIInformation.GetInstance().SetInformation(strText, Core.Data.stringManager.getString(5030), SendGetNowMsg, null);
//		}
	}
//    //只管位置 
//    void ShiftUIMode(){
//		//作战建筑物
//		if(m_data.config.build_kind == BaseBuildingData.BUILD_KIND_BATTLE){
//            if (!m_data.config.description.Contains ("，")) {
//                effectAreaBot.SetActive (false);
//                levelTitleArea.transform.localPosition = lvLblTitleOnePos;
//                effectAreaTop.transform.localPosition = effectLblTitleOnePos;
//                maxLblArea.transform.localPosition = maxLblOnePos;
//                levelLineOne.SetActive (true);
//                levelLineTwo.SetActive (false);
//                lblLevelTitle.transform.localPosition = vLevelTitlePos;
//                m_txtMax.transform.localPosition = maxTitleOnePos;
//                m_txtMaxDesp.transform.localPosition = maxDespOnePos;
//                spLine.transform.localPosition = oneLinePosY;
//                spLine.width = oneLineVLength;
//              
//            } else {
//                m_txtMaxDesp.width = maxDespTwoLength;
//                spLine.transform.localPosition = twoLineProPosY;
//                m_txtMaxDesp.transform.localPosition = maxDespTwoPos;
//                spLine.transform.localPosition =  twoLineAtkPosY;
//                spLine.width = twoLineVLength;
//
//            }
//
//            for(int i=0;i<bgNum.Length;i++){
//                bgNum[i].width = bgAtkLength;
//            }
//			
//            for (int i = 0; i < lblDesp.Length; i++) {
//                lblDesp[i].width = lblDespAtkLength;
//            }
//			
//            areaOneObj.transform.localPosition = bgAreaTwoObjAtkPos;
//            areaTwoObj.transform.localPosition = bgAreaTwoObjAtkPos;
//			levelObj.transform.localPosition = bgLevelObjAtkPos;
//
//
//
//		}else{//生产建筑物
//            if (!m_data.config.description.Contains ("，")) {
//                effectAreaBot.SetActive (false);
//                levelTitleArea.transform.localPosition = lvLblTitleOnePos;
//                effectAreaTop.transform.localPosition = effectLblTitleOnePos;
//                maxLblArea.transform.localPosition = maxLblOnePos;
//                levelLineOne.SetActive (true);
//                levelLineTwo.SetActive (false);
//                lblLevelTitle.transform.localPosition = vLevelTitlePos;
//                m_txtMax.transform.localPosition = maxTitleOnePos;
//                m_txtMaxDesp.transform.localPosition = maxDespOnePos;
//                spLine.transform.localPosition = oneLinePosY;
//                spLine.width = oneLineVLength;
//                m_txtMaxDesp.width = maxDespOneLength;
//        
//            } else {
//                levelLineOne.SetActive (false);
//                levelLineTwo.SetActive (true);
//                effectAreaBot.SetActive (true);
//                m_txtMax.transform.localPosition = maxTitleTwoPos;
//                m_txtMaxDesp.transform.localPosition = maxDespTwoPos;
//                m_txtMaxDesp.width = maxDespTwoLength;
//                spLine.transform.localPosition =  twoLineProPosY;
//                spLine.width = twoLineVLength;
//                m_txtMaxDesp.width = maxDespTwoLength;
//              
//            }
//
//		
//            for(int i=0;i<bgNum.Length;i++){
//                bgNum[i].width = bgProLength;
//            }
//            for (int i = 0; i < lblDesp.Length; i++) {
//                lblDesp[i].width = lblDespProLength;
//            }
//			
//
//            areaOneObj.transform.localPosition = bgAreaTwoObjProPos;
//            areaTwoObj.transform.localPosition = bgAreaTwoObjProPos;
//			levelObj.transform.localPosition = bgLevelObjProPos;
//		}
//
//
//	}
//
//
//	void SendOpenBattleBuildMsg()
//	{
////		float rate = 20 / (float)m_data.config.CostCoin;
////		float stone = rate * (float)m_data.RTData.dur [0];
////		stone += 0.5f;
//        float coin = m_data.config.OpenBattleCostCoin;
//        float stone = m_data.config.OpenBattleCostStone;
//		Debug.Log("--==--=-=-=  stone = " + stone);
//
//        //1 金币开启
//		if(m_nType == 1) {
//            if (Core.Data.playerManager.RTData.curCoin < coin) {
//
//                JCRestoreEnergyMsg.OpenUI(ItemManager.COIN_PACKAGE,ItemManager.COIN_BOX,2);
//
//               // SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(35000));
//				return;
//			}
//        } else if(m_nType == 2) { //2 钻石开启
//            if (Core.Data.playerManager.RTData.curStone < stone) {
//                SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (35006));
//                return;
//            } else {
//                Core.Data.ActivityManager.OnPurchaseVirtualCurrency (ActivityManager.BuildOpenType, 1, stone);
//            }
//		} 
//		
//
//		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
//		task.AppendCommonParam(RequestType.BATTLE_BUILD_OPEN, new  BattleBuildOpenParam(Core.Data.playerManager.PlayerID, m_data.RTData.id, m_nType));
//		
//		task.ErrorOccured += testHttpResp_Error;
//		task.afterCompleted += testHttpResp_UI;
//		
//		task.DispatchToRealHandler();
//		ComLoading.Open ();
//	}
//
//	public void SendGetNowMsg()
//	{
//		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
//		task.AppendCommonParam(RequestType.BUILD_GET, new  BuildGetParam(Core.Data.playerManager.PlayerID, m_data.RTData.id, 2, 1));
//		
//		task.ErrorOccured += testHttpResp_Error;
//		task.afterCompleted += testHttpResp_UI;
//		
//		task.DispatchToRealHandler();
//
//		ComLoading.Open ();
//	}
//
//	void OnBtnClickStone()
//	{
//		m_nType = 2;
//		RED.SetBtnSprite(m_btnCoin.gameObject, BTN_UNSEL);
//		RED.SetBtnSprite(m_btnStone.gameObject, BTN_SEL);
//
//		InitUI();
//	}
//
//	void OnBtnClickCoin()
//	{
//		m_nType = 1;
//		RED.SetBtnSprite(m_btnStone.gameObject, BTN_UNSEL);
//		RED.SetBtnSprite(m_btnCoin.gameObject, BTN_SEL);
//
//		InitUI();
//	}
//
//	public void  OnBtnClickLvlUp()
//	{
//		if (m_data.config.build_kind == BaseBuildingData.BUILD_KIND_BATTLE)
//		{
//			if (m_data.RTData.dur [0] > 0)
//			{
//				UIInformation.GetInstance ().SetInformation (Core.Data.stringManager.getString (5151), Core.Data.stringManager.getString (5030), SendLvlUpMsg, null);
//				return;
//			}
//			else
//			{
//				SendLvlUpMsg ();
//			}
//		}
//		else
//		{
//			SendLvlUpMsg ();
//		}
//	}
//
//	void SendLvlUpMsg()
//	{
//
//		BaseBuildingData nextLv = Core.Data.BuildingManager.GetConfigByBuildLv (m_data.RTData.num, m_data.RTData.lv + 1);
//
//		if (nextLv == null)
//		{
//			RED.LogWarning (m_data.RTData.id +  " not find next level build info!   " + m_data.RTData.lv);
//			return;
//		}
//
//		if (nextLv.cost [0] != 0 && Core.Data.playerManager.RTData.curCoin < nextLv.cost[0])
//		{
//            JCRestoreEnergyMsg.OpenUI(ItemManager.COIN_PACKAGE,ItemManager.COIN_BOX,2);
//
//			//SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(35000));
//			return;
//		}
//		else if (nextLv.cost [1] != 0 && Core.Data.playerManager.RTData.curStone < nextLv.cost[1])
//		{
//            SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(35006));
//			return;
//		}
//
//
//        //talkingdata add by wxl 
//        if (nextLv.cost [1] != 0) {
//            Core.Data.ActivityManager.OnPurchaseVirtualCurrency (ActivityManager.BuildOpenType, 1, nextLv.cost[1]);
//        }
//        //RED.Log ("send build upgrade msg");
//		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
//		task.AppendCommonParam(RequestType.BUILD_UPGRADE, new  BuildUpgradeParam(Core.Data.playerManager.PlayerID, m_data.RTData.id));
//
//		task.ErrorOccured += testHttpResp_Error;
//		task.afterCompleted += testHttpResp_UI;
//
//		task.DispatchToRealHandler();
//
//		ComLoading.Open ();
//	}
//
//	public void OnBtnClickExit()
//	{
//		Destroy(this.gameObject);
//	}
//
	public void SetShow(bool bShow)
	{
		RED.SetActive (bShow, this.gameObject);
	}
//
//	void testHttpResp_UI (BaseHttpRequest request, BaseResponse reponse)
//	{
//		ComLoading.Close ();
//		if (reponse.status != BaseResponse.ERROR) 
//		{
//			HttpRequest req = request as HttpRequest;
//			switch(req.Type)
//			{
//				case RequestType.BUILD_UPGRADE:
//					Core.Data.playerManager.RTData.curTeam.QianliXunlianMember ();
//					m_data = Core.Data.BuildingManager.GetBuildFromBagByID (m_data.RTData.id);
//					InitUI ();
//					BuildScene.mInstance.BuildUpgradeSuc (m_data.RTData.id);
//					SetShow (false);
//					break;
//
//				case RequestType.BUILD_GET:
//					BuildGetParam param = req.ParamMem as BuildGetParam;
//					SetShow(false);
//					BuildScene.mInstance.GetProductionSuc(param.bid);
//					BuildScene.mInstance.UpdateBuildById (m_data.RTData.id);
//					break;
//
//				case RequestType.BATTLE_BUILD_OPEN:
//					Core.Data.playerManager.RTData.curTeam.QianliXunlianMember ();
//					string strText = Core.Data.stringManager.getString(5113);
//					strText = string.Format(strText, m_data.config.name);
//					SQYAlertViewMove.CreateAlertViewMove(strText);
//					InitUI();
//					BuildScene.mInstance.UpdateBuildById (m_data.RTData.id);
//					break;
//			}
//			DBUIController.mDBUIInstance.RefreshUserInfo();
//		} 
//		else 
//		{
//			if(reponse.errorCode == 5006)
//			{
//				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(35006));
//			}
//			else
//			{
//				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(5111));
//			}
//		}
//	}
//
//	void testHttpResp_Error (BaseHttpRequest request, string error)
//	{
//		ComLoading.Close ();
//		ConsoleEx.DebugLog ("---- Http Resp - Error has ocurred." + error);
//	}
//
//	public string GetBattleBuildDesp(BaseBuildingData data)
//	{
//		if(data == null)
//		{
//			return "";
//		}
//
//		if(data.build_kind == BaseBuildingData.BUILD_KIND_BATTLE)
//		{
//			float eff = 0;
//			foreach(int res in data.effect)
//			{
//				if(res != 0) 
//				{
//					eff = res;
//					break;
//				}
//			}
//			if(m_nType == 2)
//			{
//				eff = eff * EFFECT_TIME_STONE;
//			}
//				
//			string strText = data.description.Replace ("@", eff.ToString());
//			return strText;
//		}
//		return "";
//	}
//
//
//    public List<float> GetBattleBuildRate(BaseBuildingData data){
//        List<float> tList = new List<float>();
//        if(data == null)
//        {
//            return tList;
//        }
//
//        if (data.build_kind == BaseBuildingData.BUILD_KIND_BATTLE) {
//            float eff = 0;
//            for (int i = 0; i < data.effect.Length; i++) {
//                if (data.effect [i] != 0) {
//                    if (m_nType == 2) {
//                        eff = data.effect [i] * EFFECT_TIME_STONE;
//
//                    } else {
//                        eff = data.effect [i];
//                    }
//                    tList.Add (eff);
//                }
//            }
//            return tList;
//        } else if (data.build_kind == BaseBuildingData.BUILD_KIND_PRODUCE) {
//            for (int i = 0; i < data.effect.Length; i++) {
//                float eff = 0;
//                if (data.effect [i] != 0) {
//                    eff = data.effect [i];
//                    tList.Add (eff);
//                }
//
//            }
//            return tList;
//        }
//        return tList;
//    }
//
//
//
//
//
//	void Update()
//	{
//		if (m_data.config.build_kind == BaseBuildingData.BUILD_KIND_PRODUCE)
//		{
//            //	m_strBuild.Remove (0, m_strBuild.Length);
//			string leftTimeTxt = "00:00:00";
//			string rightTimeTxt = "00:00:00";
//            if (m_data.RTData.dur [0] > 0) {
//				TimeSpan span = m_data.fTime [0] - DateHelper.UnixTimeStampToDateTime(Core.TimerEng.curTime);
//                if (span.TotalSeconds > 0) {
//                    //	string strText = Core.Data.stringManager.getString (5037) + ":";
//                    DateTime leftTime = Convert.ToDateTime (span.Hours.ToString () + ":" + span.Minutes.ToString () + ":" + span.Seconds.ToString ());
//                    
//                    leftTimeTxt = leftTime.ToLongTimeString ();
//				}
//            } else {
//                leftTimeTxt = "00:00:00";
//            }
//
//            if (m_data.RTData.dur [1] > 0) {
//				TimeSpan span = m_data.fTime [1] - DateHelper.UnixTimeStampToDateTime(Core.TimerEng.curTime);
//                if (span.TotalSeconds > 0) {
//                    DateTime leftTime = Convert.ToDateTime (span.Hours.ToString () + ":" + span.Minutes.ToString () + ":" + span.Seconds.ToString ());
//                    rightTimeTxt = leftTime.ToLongTimeString ();
//                }
//            } else {
//                rightTimeTxt = "00:00:00";
//            }
//
//
//            if (!string.IsNullOrEmpty (leftTimeTxt)) {
//                m_txtLeftTime.text = leftTimeTxt;
//            }
//            if (!string.IsNullOrEmpty (rightTimeTxt)) {
//                m_txtRightTime.text = rightTimeTxt;
//            }
//		}
//		else if (m_data.config.build_kind == BaseBuildingData.BUILD_KIND_BATTLE)
//		{
//			string leftString = "00:00:00";
//            //m_strBuild.Remove (0, m_strBuild.Length);
//            if (m_data.RTData.dur [0] > 0) {
//				TimeSpan span = m_data.fTime [0] - DateHelper.UnixTimeStampToDateTime(Core.TimerEng.curTime);
//                if (span.TotalSeconds > 0) {
//                    DateTime leftTime = Convert.ToDateTime (span.Hours.ToString () + ":" + span.Minutes.ToString () + ":" + span.Seconds.ToString ());
//                    leftString = leftTime.ToLongTimeString ();
//                }
//            } else {
//                leftString = "00:00:00";
//            }
//            if (!string.IsNullOrEmpty (leftString.ToString ()))
//			{
//                m_txtLeftTime.text = leftString.ToString ();
//			}
//		}
//	}
}
