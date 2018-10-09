using UnityEngine;
using System.Collections;

public class QianLiXunLianUI : MonoBehaviour 
{
	public UILabel m_txtTitle;
	public UILabel m_txtAttack;
	public UILabel m_txtDefent;
	public UILabel m_txtZhufu;
	public UILabel m_txtZhuFuTitle;
	
	public UILabel m_txtTip;

	public UILabel m_txtCoin;
	public UILabel m_txtAttr;

	public UILabel m_txtQianLi;
	public UILabel m_txtChaoShenShui;

	public UILabel m_txtQianliVal;
	public UILabel m_txtChaoShenVal;

	public UIButton m_btnReset;
	public UIButton m_btnNormal;
	public UIButton m_btnSpecial;
	public UIButton m_btnDesp;

	private UILabel m_txtNormal;
	private UILabel m_txtSpecail;

	public UIButton m_btnAddAttack;
	public UIButton m_btnAddDefent;

	public Card3DUI m_3dCard;
	public UISlider m_sliderZhufu;


	private Monster m_data = null;

    public UISprite m_Star; // yangchenguang 
    public UISprite s_btnSpecial; 
	public UILabel lblPress;

	public readonly int NEEDCOIN = 3000;
	public readonly int NEEDSTONE = 10;

	int tempCurCoin = 0;
	int tempCurStone = 0;

	private float m_nZhufu
	{
		get
		{
			return Core.Data.playerManager.RTData.happy;
		}
		set
		{
			Core.Data.playerManager.RTData.happy = (Int32Fog)value;
		}
	}
	private int m_nType = 1;  //  1 普训  2  特训   

    private const string BTN_SEL = "Symbol 31";
    private const string BTN_UNSEL = "Symbol 32";

    private Color COLOR_SEL = Color.white; //new Color(1f,215f/255f,0);
	private Color COLOR_UNSEL = Color.white;


	void Start()
	{
		InitUI ();
		m_3dCard.InitUI();
		tempCurCoin = Core.Data.playerManager.Coin;
		tempCurStone = Core.Data.playerManager.Stone;
        //setBtnXing();
	}
    //初始化星星
    public void initXX()
    {
        if(m_Star ==null || m_Star.gameObject == null ) return  ; 

        StarMove sm =  m_Star.gameObject.GetComponent<StarMove>();
        sm.setBtnXing();
    }
    public void ClearXX()
    {
        if(m_Star ==null || m_Star.gameObject == null ) return  ; 

        StarMove sm =  m_Star.gameObject.GetComponent<StarMove>();
        sm.ClearS();
    }

	void InitUI()
	{
		m_txtTitle.text = Core.Data.stringManager.getString (5050);
		m_btnNormal.TextID = 5071;
		m_btnSpecial.TextID = 5072;
		m_btnDesp.TextID = 9004;

		m_txtZhuFuTitle.text = Core.Data.stringManager.getString (5073);
		m_btnAddAttack.TextID = 5074;
		m_btnAddDefent.TextID = 5075;
		lblPress.text =Core.Data.stringManager.getString(7415);

		if(m_nZhufu == 0)
		{
			m_sliderZhufu.alpha = 0;
		}
		else
		{
			m_sliderZhufu.alpha = 1;
		}
		m_sliderZhufu.value = m_nZhufu / 3f;
		m_txtZhufu.text = m_nZhufu.ToString() + "/3";

		m_txtNormal = m_btnNormal.GetComponentInChildren<UILabel>();
		m_txtSpecail = m_btnSpecial.GetComponentInChildren<UILabel>();

		FreshTypeUI ();

		m_txtChaoShenShui.text = Core.Data.stringManager.getString (5077);
		m_txtChaoShenVal.text = Core.Data.itemManager.GetBagItemCount(ItemManager.CHAOSHENSHUI).ToString();

		if (m_data == null)
		{
			m_3dCard.Del3DModel ();
			m_3dCard.InitUI();
			m_txtQianliVal.text = "0";
			m_txtTip.text = Core.Data.stringManager.getString (5057);
			m_txtAttack.text = "0";
			m_txtDefent.text = "0";

			m_txtTip.text = Core.Data.stringManager.getString (5057);
		}
		else
		{
			Debug.Log ("total  qianli  = " + m_data.totalQianli + "   cur  = " + m_data.curQianli + "   upstet  = " + m_data.RTData.uspt );
			m_txtQianliVal.text = m_data.curQianli.ToString();
			m_txtTip.text = "";
			float param = Core.Data.monManager.GetMonUpAtkParam (m_data.config.star, m_data.Star);
			int baseAtk = (int)(m_data.baseAttack * param);
			int chaclaAtk = (int)(m_data.curChakala_Atk * param);
			m_txtAttack.text = baseAtk.ToString() + " + [00ff00]" + chaclaAtk.ToString() + "[-]";


			param = Core.Data.monManager.GetMonUpDefParam (m_data.config.star, m_data.Star);
			int baseDef = (int)(m_data.baseDefend * param);
			int chaklaDef = (int)(m_data.curChakala_Def * param);
			m_txtDefent.text = baseDef.ToString () + " + [00ff00]" + chaklaDef.ToString () + "[-]";

			m_txtTip.text = "";
		}
		m_txtQianLi.text = Core.Data.stringManager.getString (5076);
		m_btnReset.TextID = 5142;
	}
   
	void FreshTypeUI()
	{
		if (m_nType == 1)
		{
			m_sliderZhufu.gameObject.SetActive(false);
			m_txtZhuFuTitle.gameObject.SetActive(false);
			string strText = Core.Data.stringManager.getString(5092);
			strText = string.Format(strText, NEEDCOIN);
			m_txtCoin.text = strText;

			string  strAttr = Core.Data.stringManager.getString(5078);
			strAttr = string.Format(strAttr, 1);
			m_txtAttr.text = strAttr;

//			RED.SetBtnSprite (m_btnNormal.gameObject, BTN_SEL);
			m_btnNormal.normalSprite = BTN_SEL;
			m_btnSpecial.normalSprite = BTN_UNSEL;
//			RED.SetBtnSprite (m_btnSpecial.gameObject, BTN_UNSEL);

			m_txtNormal.color = COLOR_SEL;
			m_txtSpecail.color = COLOR_UNSEL;

			m_3dCard.transform.localScale = Vector3.one * 0.85f;
			m_3dCard.transform.localPosition = new Vector3 (0, -60, -150);
		}
		else if (m_nType == 2)
		{
			m_sliderZhufu.gameObject.SetActive(true);
			m_txtZhuFuTitle.gameObject.SetActive(true);
			string strText = Core.Data.stringManager.getString(5093);
			strText = string.Format(strText, NEEDSTONE);
			m_txtCoin.text = strText;

			string  strAttr = Core.Data.stringManager.getString(5078);
			strAttr = string.Format(strAttr, "2~5");
			m_txtAttr.text = strAttr;

//			RED.SetBtnSprite (m_btnNormal.gameObject, BTN_UNSEL);
//			RED.SetBtnSprite (m_btnSpecial.gameObject, BTN_SEL);

			m_btnNormal.normalSprite = BTN_UNSEL;
			m_btnSpecial.normalSprite = BTN_SEL;

			m_txtNormal.color = COLOR_UNSEL;
			m_txtSpecail.color = COLOR_SEL;


			m_3dCard.transform.localScale = Vector3.one * 0.7f;
			m_3dCard.transform.localPosition = new Vector3 (0, -25, -150);
		}
	}

	public void OnClickMain()
	{
        m_Star.gameObject.GetComponent<StarMove>().ClearS();
		DBUIController.mDBUIInstance.SetViewState (RUIType.EMViewState.S_Bag, RUIType.EMBoxType.QIANLI_XUNLIAN);
		TrainingRoomUI.mInstance.SetShow (false);
	}

	void OnClickAddAtk()
	{
		if (m_data == null)
		{
			return;
		}

		if (IsValia (1))
		{
			SendQianLiXunLianMsg (1,1);
		}
	}

	void OnClickAddDefent()
	{
		if (m_data == null)
		{
			return;
		}

		if (IsValia (2))
		{
			SendQianLiXunLianMsg (2,1);
		}
	}


	bool IsValia(int target)
	{
		if (m_nType == 1)
		{

			if (target == 1) {
				if (Core.Data.playerManager.Coin < 3000 * (i_AtkCounting + 1)) {
					JCRestoreEnergyMsg.OpenUI (ItemManager.COIN_PACKAGE, ItemManager.COIN_BOX, 2);
					return false;
				}
			} else {
				if (Core.Data.playerManager.Coin < 3000 * (i_DefCounting+1)) {
					JCRestoreEnergyMsg.OpenUI (ItemManager.COIN_PACKAGE, ItemManager.COIN_BOX, 2);
					return false;
				}
			}
		}
		else if (m_nType == 2)
		{
			if (target == 1) {
				if (Core.Data.playerManager.Stone < 10 * (i_AtkCounting + 1)) {
					SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (35006));
					return false;
				}
			} else {
				if (Core.Data.playerManager.Stone < 10 * (i_DefCounting + 1)) {
					SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (35006));
					return false;
				}
			}
		}

		if (target == 1) {
			if (Core.Data.itemManager.GetBagItemCount (ItemManager.CHAOSHENSHUI) <= i_AtkCounting) {
				if (Core.Data.playerManager.RTData.curLevel < 10) {
					string strText = Core.Data.stringManager.getString (34060);
					SQYAlertViewMove.CreateAlertViewMove (strText);
				} else {
					UIInformation.GetInstance ().SetInformation (Core.Data.stringManager.getString (5204), Core.Data.stringManager.getString (5030), OpenDuiHuanUI, null);
				}
				return false;
			}
		} else {
			if (Core.Data.itemManager.GetBagItemCount (ItemManager.CHAOSHENSHUI) <= i_DefCounting) {
				if (Core.Data.playerManager.RTData.curLevel < 10) {
					string strText = Core.Data.stringManager.getString (34060);
					SQYAlertViewMove.CreateAlertViewMove (strText);
				} else {
					UIInformation.GetInstance ().SetInformation (Core.Data.stringManager.getString (5204), Core.Data.stringManager.getString (5030), OpenDuiHuanUI, null);
				}
				return false;
			}
		}

		if (target == 1) {
			if (m_data.curQianli <= i_AtkCounting) {
				SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (5117));
				return false;
			}
		} else {
			if (m_data.curQianli <= i_DefCounting) {
				SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (5117));
				return false;
			}
		}
		return true;
	}

	//打开兑换超神水页面
	void OpenDuiHuanUI()
	{
		//需要添加打开兑换
		TrainingRoomUI.mInstance.SetShow(false);
		FinalTrialMgr.GetInstance().OpenDuihuanRequest(OpenduihuanCallback, DuihuanFromType.Type_Zhaomu);
	}

	void OpenduihuanCallback()
	{
		TrainingRoomUI.mInstance.SetShow(true);

		if (m_data != null)
		{
			m_3dCard.Show3DCard (m_data, false);
		}
		m_txtChaoShenShui.text = Core.Data.stringManager.getString (5077);
		m_txtChaoShenVal.text = Core.Data.itemManager.GetBagItemCount(ItemManager.CHAOSHENSHUI).ToString();
	}

	void OnClickReset()
	{
		if (m_data == null)
		{
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(5141));
			return;
		}

		string strText = Core.Data.stringManager.getString (5140);
		strText = string.Format (strText, 500);
		UIInformation.GetInstance ().SetInformation (strText, Core.Data.stringManager.getString(5030), ResetQianli);
	}

	void OnClickDesp()
	{
		string strText = Core.Data.stringManager.getString (5168);
		string[] strTexts = strText.Split ('\n');
		TrainingRoomUI.mInstance.m_despUI.SetText (Core.Data.stringManager.getString (5131), strTexts);
	}

	void ResetQianli()
	{
		if(m_data.RTData.ChaKeLa_Attck == 0 && m_data.RTData.ChaKeLa_Defend == 0)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(5170));
			return;
		}

		if (Core.Data.playerManager.RTData.curStone < 500)
		{
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (35006));
			return;
		}

		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.QIANLIRESET, new QianLiResetParam(Core.Data.playerManager.PlayerID, m_data.pid));

		task.ErrorOccured += testHttpResp_Error;
		task.afterCompleted += testHttpResp_UI;

		//then you should dispatch to a real handler
		task.DispatchToRealHandler ();

	}

	void OnClickNormal()
	{
		if (m_nType == 1)
		{
			return;
		}
		m_nType = 1;
		FreshTypeUI ();
	}
	void OnClickSpecial()
	{


		if (m_nType == 2)
		{
			return;
		}

		m_nType = 2;
		FreshTypeUI ();
	}

	void OnClickBack()
	{
        StarMove sm3  =m_Star.gameObject.GetComponent<StarMove>();
        sm3.ClearS();
		m_data = null;
		InitUI ();
		SetShow (false);
		RED.SetActive (true, TrainingRoomUI.mInstance.m_mainTraining);

		if (TrainingRoomUI.mInstance.m_callBack != null)
		{
			TrainingRoomUI.mInstance.OnClickExit ();
		}
	}

	public void SetData(Monster mon)
	{
		m_data = mon;
		InitUI ();
		m_3dCard.Show3DCard (mon);
	}

	public void SetShow(bool bShow)
	{
		RED.SetActive (bShow, this.gameObject);
		if(bShow)
		{
			InitUI();
		}
	}


	void SendQianLiXunLianMsg(int target,int tNum)
	{
		if (m_nType == 1) {
			if (Core.Data.playerManager.RTData.curCoin < NEEDCOIN) {
				SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (35000));
				return;
			}
		} else {
			if (Core.Data.playerManager.RTData.curStone < NEEDSTONE) {
				SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (35006));
				return;
			}
		}
		ComLoading.Open ();
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.QIANLIXUNLIAN, new QianLiXunLianParam(Core.Data.playerManager.PlayerID, m_data.pid, m_nType, target,tNum));

		task.ErrorOccured += testHttpResp_Error;
		task.afterCompleted += testHttpResp_UI;

		//then you should dispatch to a real handler
		task.DispatchToRealHandler ();
	}

	#region 网络返回

	void testHttpResp_UI (BaseHttpRequest request, BaseResponse response)
	{
		if (response.status != BaseResponse.ERROR) 
		{
			HttpRequest req = request as HttpRequest;

			switch (req.Type)
			{
				case RequestType.QIANLIXUNLIAN:
					QianLiXunLianParam param = req.ParamMem as QianLiXunLianParam;

					QianLiXunLianResponse resp = response as QianLiXunLianResponse;
					string strText = Core.Data.stringManager.getString(5102);
				if(resp.data.stone != 0){
					//add by wxl talkingdata
					Core.Data.ActivityManager.OnPurchaseVirtualCurrency(ActivityManager.TraningType,1,Mathf.Abs(resp.data.stone));
				}	
				
					if(param.target == 1)
					{
						if(resp.data.ak == 0)
						{
							DoFail();
							return;
						}

						strText = string.Format(strText, resp.data.ak, Core.Data.stringManager.getString(5103));
					}
					else
					{
						if(resp.data.df == 0)
						{
							DoFail();

							return;
						}
						strText = string.Format(strText, resp.data.df, Core.Data.stringManager.getString(5104));
					}
					if(m_nZhufu >= 3)
					{
						m_nZhufu = 0;
						m_sliderZhufu.alpha = 0;
						m_sliderZhufu.value = m_nZhufu / 3f;
						m_txtZhufu.text = m_nZhufu.ToString() + "/3";
					}
					SQYAlertViewMove.CreateAlertViewMove(strText);

					m_data = Core.Data.monManager.getMonsterById(param.roleid);
					m_3dCard.Show3DCard(m_data,false);
					break;
				case RequestType.QIANLIRESET:
					strText = Core.Data.stringManager.getString (5143);
					SQYAlertViewMove.CreateAlertViewMove (strText);
					break;
			}
		} else {
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
		}
		ComLoading.Close ();
		Core.Data.playerManager.RTData.curTeam.QianliXunlianMember();
		DBUIController.mDBUIInstance.RefreshUserInfo();
		InitUI ();
	}


	void DoFail()
	{
		ComLoading.Close ();
		m_nZhufu ++;
		if(m_nZhufu > 3)
		{
			m_nZhufu = 0;
			m_sliderZhufu.alpha = 0;
		}
		else
		{
			m_sliderZhufu.alpha = 1;
		}
		SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(5101));




		m_sliderZhufu.value = m_nZhufu / 3f;
		m_txtZhufu.text = m_nZhufu.ToString() + "/3";



		 isAtkPress = false;
		 i_AtkCounting = 0;
		 i_UpdateCounting =0;

		 isDefPress = false;
		 i_DefCounting = 0;

	}

	void testHttpResp_Error (BaseHttpRequest request, string error)
	{
		ConsoleEx.DebugLog ("---- Http Resp - Error has ocurred." + error);
	}
	#endregion

	#region 批量训练
	bool isAtkPress = false;
	int i_AtkCounting = 0;
	int i_UpdateCounting =0;

	bool isDefPress = false;
	int i_DefCounting = 0;


	public void AddAttackPress(){
		if (m_data == null || isDefPress == true) {
			return;
		}
			
		i_AtkCounting = 1;
		isAtkPress = true;
		tempCurCoin = Core.Data.playerManager.Coin;
		tempCurStone = Core.Data.playerManager.Stone;
	}

	public void AddAttackRelease(){
		if (m_data == null || isAtkPress == false)
			return;
		isAtkPress = false;
		i_UpdateCounting = 0;
		//发送消息
		SendQianLiXunLianMsg (1,i_AtkCounting);
		tempCurCoin = Core.Data.playerManager.RTData.curCoin;
		i_AtkCounting = 0;

	}
	// 按住 计算 
	void CountintAtk(){
		if (isAtkPress) {
			i_AtkCounting++;
		}
	}

	void Update(){
		if (isAtkPress) {
			i_UpdateCounting++;
			if (i_UpdateCounting >= 50) {
				if (Time.frameCount % 5==0) {
					if (IsValia (1)) {
						i_AtkCounting++;
						CountingRefreshUI (1);
					}
					else {
						AddAttackRelease ();
					}
				}
			}
		}

		if (isDefPress) {
			i_UpdateCounting++;
			if (i_UpdateCounting >= 50) {
				if (Time.frameCount % 5==0) {
					if (IsValia (2)) {
						i_DefCounting++;
						CountingRefreshUI (2);
					}
					else {
						AddDefendRelease ();
					}
				}
			}
		}
	}


	/// <summary>
	/// 攻击防御
	/// </summary>
	/// <param name="target">攻击 / 防御.</param>
	void CountingRefreshUI(int target ){
		int tempTotal = Core.Data.itemManager.GetBagItemCount (ItemManager.CHAOSHENSHUI);
		if (m_nType == 1) {
			int totalCoin = Core.Data.playerManager.RTData.curCoin;
			if (tempCurCoin <= 0) {
				tempCurCoin = totalCoin;
			}
			if (tempCurCoin >= 3000) {
				if (target == 1) {
					tempCurCoin -= 3000; 
					m_txtChaoShenVal.text = (tempTotal - i_AtkCounting).ToString ();
				} else {
					tempCurCoin -= 3000; 
					m_txtChaoShenVal.text = (tempTotal - i_DefCounting).ToString ();
				}
			}
			UIMiniPlayerController.Instance.lab_coin.text = tempCurCoin.ToString();
		} else {

			int totalStone = Core.Data.playerManager.RTData.curStone;
			if (tempCurStone <= 0) {
				tempCurStone = totalStone;
			}
			if (tempCurStone >= 10) {
				if (target == 1) {
					tempCurStone -= 10;
					m_txtChaoShenVal.text = (tempTotal - i_AtkCounting).ToString ();
				} else {
					tempCurStone -= 10; 
					m_txtChaoShenVal.text = (tempTotal - i_DefCounting).ToString ();
				}
			}
			UIMiniPlayerController.Instance.lab_stone.text = tempCurStone.ToString();
		}

//		UIMiniPlayerController.Instance.freshPlayerInfoView ();
	}


	public void AddDefendPress(){
		if (m_data == null || isAtkPress == true) {
			return;
		}
		i_DefCounting = 1;
		isDefPress = true;

		tempCurCoin = Core.Data.playerManager.Coin;
		tempCurStone = Core.Data.playerManager.Stone;
	}

	public void AddDefendRelease(){
		if (m_data == null ||isDefPress == false)
			return;
		isDefPress = false;
		i_UpdateCounting = 0;
		//发送消息
		SendQianLiXunLianMsg (2,i_DefCounting);
		tempCurStone = Core.Data.playerManager.RTData.curStone;
		i_DefCounting = 0;
	}


	#endregion


}
