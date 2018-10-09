using UnityEngine;
using System.Collections;
using System;
using System.Text;

public class ZhaoMuSubUI : MonoBehaviour 
{
	private readonly int[] TITLE_ID = {5240, 5241, 5242};									//标题ID
	private readonly int[] BUY_TIP = {5245, 5246, 5264, 5265, 5247, 5248};					//购买提示
	private readonly int[] MONEY = { 30000, 270000, 350, 3150, 350,350};						//花费钱数
	private readonly string STONE = "common-0014";											//钻石
	private readonly string COIN = "common-0013";											//金币
	private readonly string[] BG_NAME = { "gaojiyi", "gaojishi", "chaojiyi", "chaojishi", "maiwuqi", "maifangju"};

	public UILabel m_labTitle;								//标题									
	public UILabel[] m_labBuyTip;							//购买提示
	public UILabel[] m_labPrice;							//价格
	public UILabel[] m_labTime;								//剩余时间
	public UISprite[] m_spPrice;							//价格ICON
	public UISprite[] m_spBg;								//背景

	public UILabel m_labGetCnt;								//
	public UILabel m_labGetTip;
	public UILabel m_labGet;

	private int m_nCurType;									//当前大招募类型
	private int m_nCurSubType;								//当前小招募类型
	public readonly int ZHAOMU_BASE = 51000;				//招募基数
	private bool m_bShow = false;
    public UISprite Zhekou ;  // 折扣提示标

	public readonly int BAG_MAX = 4000;							//背包上限

	public UILabel m_lblTitle;
	public GameObject subObj;

	public void OpenUI(int type)
	{
		m_nCurType = type;
		InitUI ();
		SetShow (true);
	}

	void Start()
	{

		InitUI ();
		if (IsInvoking ("UpdateSubUI")) {
			CancelInvoke ("UpdateSubUI");
		}
		InvokeRepeating("UpdateSubUI", 0.0f, 1.0f);
	}

	/// 
	/// 优化UI，所以有些没有任何逻辑的代码，只是选择而已
	/// 
	void OptimizedUI(string leftName, string rightName) {
		m_spBg [0].spriteName = leftName;
		m_spBg [1].spriteName = rightName;

		if(leftName == "maiwuqi" || leftName == "chaojiyi") {
			m_spBg [0].transform.localRotation = Quaternion.Euler( new Vector3 (0f, 0f, 90f));
			m_spBg [0].width = 216;
			m_spBg [0].height = 186;
		} else {
			m_spBg [0].transform.localRotation = Quaternion.identity;
			m_spBg [0].width = 186;
			m_spBg [0].height = 216;
		}

		if(rightName == "maifangju" || rightName == "gaojishi") {
			m_spBg [1].transform.localRotation = Quaternion.Euler( new Vector3 (0f, 0f, 90f));
			m_spBg [1].width = 216;
			m_spBg [1].height = 186;
		} else {
			m_spBg [1].transform.localRotation = Quaternion.identity;
			m_spBg [1].width = 186;
			m_spBg [1].height = 216;
		}
	}

	void InitUI()
	{
        if (m_nCurType == 3)
        {
			m_lblTitle.gameObject.SetActive (false);
            Zhekou.gameObject.SetActive(false);
        }else
        {
            Zhekou.gameObject.SetActive(true);
        }



		m_labGetCnt.gameObject.SetActive (false);//text = "";
		m_labGet.gameObject.SetActive (false);// = "";

		//标题
		int titleID = TITLE_ID[m_nCurType - 1];
		m_labTitle.text = Core.Data.stringManager.getString (titleID);

		int leftPron = (m_nCurType - 1) * 2 + 1;
		int rigtPron = (m_nCurType - 1) * 2 + 2;

		int leftMoney = MONEY [leftPron - 1];
		int rightMoney = MONEY [rigtPron - 1];

		//价格
		m_labPrice [0].text = leftMoney.ToString();
		m_labPrice [1].text = rightMoney.ToString();

		int leftBuy = BUY_TIP [leftPron - 1];
		int rigtBuy = BUY_TIP [rigtPron - 1];

		//购买提示
		m_labBuyTip [0].text = Core.Data.stringManager.getString (leftBuy);
		m_labBuyTip [1].text = Core.Data.stringManager.getString (rigtBuy);

		//背景
		string leftName = BG_NAME [leftPron - 1];
		string rigtName = BG_NAME [rigtPron - 1];

		OptimizedUI(leftName, rigtName);
	
		//钱ICON
		if (m_nCurType == 1)
		{
			m_spPrice [0].spriteName = COIN;
			m_spPrice [1].spriteName = COIN;

			m_labGetTip.text = Core.Data.stringManager.getString (5250);
			m_labGet.text = Core.Data.stringManager.getString (5253);
			m_labGetCnt.text = "";

			Vector3 getTipPos = m_labGetTip.transform.localPosition;
			Vector3 getPos = m_labGetCnt.transform.localPosition;
			getTipPos.x = 20;
			getPos.x = 20;

			m_labGetTip.transform.localPosition = getTipPos;
			m_labGet.transform.localPosition = getPos;



			if (LuaTest.Instance.OpenStoneRecruit) {
				subObj.transform.localPosition = Vector3.down * 10;
				if(m_nCurType != 3)
					m_lblTitle.gameObject.SetActive (true);
				else 
					m_lblTitle.gameObject.SetActive (false);
				m_lblTitle.text = Core.Data.stringManager.getString (7416);
			} else {
				subObj.transform.localPosition = Vector3.zero;
				m_lblTitle.gameObject.SetActive (false);
			}

		}
		else
		{
			m_spPrice [0].spriteName = STONE;
			m_spPrice [1].spriteName = STONE;

			if (m_nCurType == 2)
			{
				ZhaoMuStateData data = Core.Data.zhaomuMgr.GetZhaomuState (ZHAOMU_BASE + 3);
				if (data != null)
				{
					m_labGetTip.text = Core.Data.stringManager.getString (5251);
					m_labGet.text = Core.Data.stringManager.getString (5252);
					m_labGetCnt.text = data.spGetCnt.ToString();

					Vector3 getTipPos = m_labGetTip.transform.localPosition;
					Vector3 getPos = m_labGetCnt.transform.localPosition;

//					getTipPos.x = 83;
					getPos.x = 83;
					
					m_labGetTip.transform.localPosition = getTipPos;
					m_labGet.transform.localPosition = getPos;
				}
			}
			else
			{
				m_labGetCnt.text = "";
				m_labGet.text = "";
				m_labGetTip.text = "";
			}

			if (LuaTest.Instance.OpenStoneRecruit) {
				subObj.transform.localPosition = Vector3.down * 10;
				if(m_nCurType != 3)
					m_lblTitle.gameObject.SetActive (true);
				else 
					m_lblTitle.gameObject.SetActive (false);
				m_lblTitle.text = Core.Data.stringManager.getString (7417);
			} else {
				subObj.transform.localPosition = Vector3.zero;
				m_lblTitle.gameObject.SetActive (false);
			}
		}
	}

	void UpdateSubUI()
	{
		ZhaoMuStateData[] data = new ZhaoMuStateData[1];

		switch (m_nCurType)
		{
			case 1:
				data[0] = Core.Data.zhaomuMgr.GetZhaomuState (ZHAOMU_BASE + 1);
				break;
			case 2:
				data[0] = Core.Data.zhaomuMgr.GetZhaomuState (ZHAOMU_BASE + 3);
				break;
//			case 3:
//				data[0] = Core.Data.zhaomuMgr.GetZhaomuState (ZHAOMU_BASE + 5);
//				data[1] = Core.Data.zhaomuMgr.GetZhaomuState (ZHAOMU_BASE + 6);
//				break;
		}
				
		for (int i = 0; i < data.Length; i++)
		{
			string strFreeCnt = Core.Data.stringManager.getString (5243);

			if (data [i] != null)
			{
				//免费
				if (Core.Data.zhaomuMgr.IsZhaomuFree(data[i].pron))
				{
					strFreeCnt = string.Format (strFreeCnt, data [i].freecount, data [i].totalcount);
					m_labTime[i].text = strFreeCnt;
					m_labPrice [i].text = Core.Data.stringManager.getString (7136);
				}
				else
				{
					if (data [i].freecount <= 0)
					{
						m_labTime [i].text = Core.Data.stringManager.getString (5249);
					}
					else
					{
						string strTime = Core.Data.stringManager.getString (5244);
						m_labTime [i].text = string.Format (strTime, GetTimeFormat (data[i].coolTime));
					}
					int money = data [i].pron - ZHAOMU_BASE - 1;
					m_labPrice [i].text = MONEY [money].ToString ();
				}
			}
			else
			{
				m_labTime[i].text = "";
			}
		}
	}

	public string GetTimeFormat(long seconds)
	{
		if (seconds <= 0)
			return "";

		TimeSpan span = DateTime.Now.AddSeconds(seconds) - DateTime.Now;
		DateTime leftTime = Convert.ToDateTime (span.Hours.ToString () + ":" + span.Minutes.ToString () + ":" + span.Seconds.ToString ());
		string strTime = leftTime.ToLongTimeString ();
		strTime = strTime.Remove(0,2);
		int hours = span.Hours + span.Days * 24;
		string strHours = hours >= 10 ? hours.ToString() :"0"+hours.ToString();
		return strHours + strTime;
	}

	public void SetShow(bool bShow)
	{
		m_bShow = bShow;
		RED.SetActive (bShow, this.gameObject);
	}

	public bool IsShow()
	{
		return m_bShow;
	}

	public void OnClickExit()
	{
		SetShow (false);
	}


	public void OnClickSubZhao(int sub)
	{
		m_nCurSubType = (m_nCurType - 1) * 2 + sub;
	
		if (!Core.Data.guideManger.isGuiding && !Core.Data.zhaomuMgr.IsZhaomuFree(ZHAOMU_BASE + m_nCurSubType))
		{
			int money = MONEY [m_nCurSubType - 1];
			if (m_nCurType == 1)
			{
				if (Core.Data.playerManager.RTData.curCoin < money)
				{
					JCRestoreEnergyMsg.OpenUI (110019, 110020, 2);
					return;
				}
			}
			else
			{
				if (Core.Data.playerManager.Stone < money)
				{
					SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (7310));
					return;
				}
			}
		}

		int equipCnt = Core.Data.EquipManager.GetEquipCount ();
		int monCnt = Core.Data.monManager.GetMonCnt ();
		string strText = Core.Data.stringManager.getString (5254);

		if (m_nCurType == 3)
		{
			if (equipCnt >= BAG_MAX)
			{
				strText = string.Format (strText, Core.Data.stringManager.getString (5082));
				SQYAlertViewMove.CreateAlertViewMove (strText);
				return;
			}
		}
		else if (m_nCurType == 2)
		{
			if (monCnt >= BAG_MAX)
			{
				strText = string.Format (strText, Core.Data.stringManager.getString (5081));
				SQYAlertViewMove.CreateAlertViewMove (strText);
				return;
			}
		}
		else
		{
			if (equipCnt >= BAG_MAX)
			{
				strText = string.Format (strText, Core.Data.stringManager.getString (5082));
				SQYAlertViewMove.CreateAlertViewMove (strText);
				return;
			}

			if (monCnt >= BAG_MAX)
			{
				strText = string.Format (strText, Core.Data.stringManager.getString (5081));
				SQYAlertViewMove.CreateAlertViewMove (strText);
				return;
			}
		}
		
		SendZhaomuMsg ();
	}

	void OnClickSubZhaomu(GameObject obj)
	{
		int sub = int.Parse (obj.name);
		OnClickSubZhao(sub);
	}
    bool isZhaoMuMsg = true  ;// yangcg  等消息返回来初始化UI以后才可以点第二次 12月15日
	public void SendZhaomuMsg()
	{

        if (!isZhaoMuMsg)
        {
            return ;
        }

        isZhaoMuMsg = false ;
		HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);

		int pool = ZHAOMU_BASE + m_nCurSubType;
		int flag = Core.Data.zhaomuMgr.IsZhaomuFree (pool) ? 1 : 2;

		ZhaoMuParam param = new ZhaoMuParam (Core.Data.playerManager.PlayerID, pool, flag);

		task.AppendCommonParam(RequestType.ZHAOMU, param);

		task.ErrorOccured += testHttpResp_Error;
		task.afterCompleted += testHttpResp_UI;

		task.DispatchToRealHandler ();

		ComLoading.Open ();
	}


	void ShowZhaomuReward(ItemOfReward[] p)
	{
		if(p == null || p[0] == null)
		{
			RED.LogWarning("zhaomu itemofreward is null");
			return;
		}
		switch (m_nCurSubType)
		{
			case 1:				//金币单抽
				if (p[0].getCurType () == ConfigDataType.Monster)
				{
					Monster mon = p [0].toMonster (Core.Data.monManager);
					if (mon.Star >= 3)
					{
						CradSystemFx.GetInstance ().SetCardSinglePanel (p,30000,true,false);
					}
					else
					{
						GetRewardSucUI.OpenUI (p, Core.Data.stringManager.getString (5047));
					}
				}
				else
				{
					GetRewardSucUI.OpenUI (p, Core.Data.stringManager.getString (5047));
				}
				break;
			case 3:				//钻石单抽
				if(CradSystemFx.GetInstance().mCardSystemPanel != null)
				{
					ComLoading.Open();
					CradSystemFx.GetInstance().SetCardSinglePanel(p,350);
				}
				else
				{
					if(Core.Data.guideManger.isGuiding)CradSystemFx.GetInstance().SetCardSinglePanel(p,350,true,true,false);
					else CradSystemFx.GetInstance().SetCardSinglePanel(p,350);
				}
				break;
			case 2:             //金币十连抽
				CradSystemFx.GetInstance ().SetCardPanel (p,270000,true,false);
				break;
			case 4:				//钻石十连抽
				CradSystemFx.GetInstance ().SetCardPanel (p,3150);
				break;
			case 5:
			case 6:				//装备单抽
				GetRewardSucUI.OpenUI (p, Core.Data.stringManager.getString (5047));
				break;
		}
	}
		
	void testHttpResp_UI (BaseHttpRequest request, BaseResponse reponse)
	{
		if (reponse.status != BaseResponse.ERROR) 
		{
			HttpRequest req = request as HttpRequest;

			if (req.Type == RequestType.ZHAOMU)
			{

				ZhaoMuResponse resp = reponse as ZhaoMuResponse;

				ShowZhaomuReward (resp.data.p);
				InitUI();
				DBUIController.mDBUIInstance._playerViewCtl.freshPlayerInfoView ();
				UIMiniPlayerController.Instance.freshPlayerInfoView ();

				Core.Data.soundManager.SoundFxPlay(SoundFx.FX_Diamonds, null);
				SQYMainController.mInstance.UpdateBagTip ();
			}
		} 
		else 
		{
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getNetworkErrorString(reponse.errorCode));

			if (reponse.errorCode  == 7018)
			{
				Core.Data.zhaomuMgr.bInit = false;
				Core.Data.zhaomuMgr.SendZhaomuStateMsg ();
			}
		}
        isZhaoMuMsg = true ;// yangcg  等消息返回来初始化UI以后才可以点第二次 12月15日
		ComLoading.Close ();
	}

	void testHttpResp_Error (BaseHttpRequest request, string error)
	{
		RED.Log ("---- Http Resp - Error has ocurred." + error);
		ComLoading.Close ();

	}
}
