using UnityEngine;
using System.Collections;
using Framework;

public class UIDragonRechargeCell : RUIMonoBehaviour {

	public UISprite _icon;
	public UILabel _money;
	public UISprite _sale;
	public UILabel _tag;

	public UILabel Title1;
	public UILabel Title2;
	public UILabel Title3;
	public UILabel LimitNum;

	private int index;

	private RechargeData m_data = null;

	private string m_strOrderId;

	private readonly int QueryMaxCnt = 60;
	private int curQueryCnt = 0;

	public void OnShow(RechargeData data, int _BuyCount = -1)
	{
		LimitNum.text = "";
		m_data = data;
		_icon.spriteName = m_data.Iconid;
		_icon.MakePixelPerfect();
		_money.text = string.Format(Core.Data.stringManager.getString(25108), m_data.Price/100);
		Title1.text = m_data.Title;

		if(m_data.Recommend == 0)
		{
			_sale.gameObject.SetActive(false);
			_tag.text = "";
		}
		else if(m_data.Recommend == 1)
		{
			_sale.gameObject.SetActive(true);
			_tag.text = Core.Data.stringManager.getString(25106);
		}
		else if(m_data.Recommend == 2)
		{
			_sale.gameObject.SetActive(true);
			_tag.text = Core.Data.stringManager.getString(25105);
		}

		if(m_data.Type == 0)
		{
			Title3.text = m_data.Describe1;
			Title2.text = m_data.Describe;
			LimitNum.text = "";
		}
		else if(m_data.Type == 1)
		{
			Title3.text = "";
			string info = "";
			if(m_data.Double == 0)
			{
				if(m_data.Present2 != null && m_data.Present2.Count != 0 && m_data.Present2[0] != null && m_data.Present2[0].Length == 2)
				{
					Title2.text = string.Format(m_data.Describe, m_data.Present2[0][1]);
				}
				else
				{
					Title2.text = "";
				}
			}
			else
			{
				if(_BuyCount == -1)
				{
					if(m_data.Present1 != null && m_data.Present1.Count != 0 && m_data.Present1[0] != null && m_data.Present1[0].Length == 2)
					{
						info = string.Format(m_data.Describe, m_data.Present1[0][1]);
						info = info+ string.Format(Core.Data.stringManager.getString(25127), m_data.Double);
						Title2.text = info;
					}
					else
					{
						Title2.text = "";
					}
				}
				else if(_BuyCount < m_data.Double)
				{
					if(m_data.Present1 != null && m_data.Present1.Count != 0 && m_data.Present1[0] != null && m_data.Present1[0].Length == 2)
					{
						info = string.Format(m_data.Describe, m_data.Present1[0][1]);
						info = info + string.Format(Core.Data.stringManager.getString(25127), m_data.Double);
						Title2.text = info;
					}
					else
					{
						Title2.text = "";
					}
				}
				else
				{
					if(m_data.Present2 != null && m_data.Present2.Count != 0 && m_data.Present2[0] != null && m_data.Present2[0].Length == 2)
					{
						Title2.text = string.Format(m_data.Describe, m_data.Present2[0][1]);
					}
					else
					{
						Title2.text = "";
					}
				}

			}
		}

	}

	void OnClick()
	{

		#if NOBILLING
		SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(5213));
		#else

		LuaTest lua = LuaTest.Instance;
		if(lua != null) {

			if(lua.HasBilling) {
				AccountData ad = Native.mInstace.m_thridParty.GetAccountData ();
				if (ad.lType == LoginType.TYPE_THIRDPARTY){
					SendPayRQ ();
				}
			} else {
				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(5213));
			}

		} else {
			AccountData ad = Native.mInstace.m_thridParty.GetAccountData ();
			if (ad.lType == LoginType.TYPE_THIRDPARTY){
				SendPayRQ ();
			}
		}
		#endif
	}

	void OpenThirdPayUI(PayData data)
	{
		string strJson = string.Empty;
		#if QiHo360
		strJson = generateQihoo(data);
		#elif Spade
		strJson = generateSpade(data);
		#elif Google
//		strJson = generateGoogle(data);
		#endif

		Native.mInstace.m_thridParty.Pay (strJson);
	}

	#region 生成本地支付信息
	string generateQihoo(PayData data) {
		PayInfo info = new PayInfo ();
		AccountData ad = Native.mInstace.m_thridParty.GetAccountData ();

		info.productId = m_data.ID.ToString();
		info.productName = m_data.Title;
		info.accessToken = ad.token;
		info.appOrderId = data.orderId;
		info.appUserId = Core.Data.playerManager.PlayerID;
		info.appUserName = Core.Data.playerManager.RTData.nickName;
		info.exchangeRate = "10";
		info.moneyAmount = m_data.Price.ToString();

		info.notifyUri = ad.payCallback;
		info.qihooUserId = ad.uniqueId;

		return fastJSON.JSON.Instance.ToJSON (info);
	}

	#if Spade
	string generateSpade(PayData data) {
		HTPayInfo info = new HTPayInfo();
		AccountData ad = Native.mInstace.m_thridParty.GetAccountData ();

		info.price = m_data.Price;
		info.count = 1;
		info.productId   = m_data.ID.ToString();
		info.productName = string.IsNullOrEmpty(m_data.SpadeName) ? " " : m_data.SpadeName;
		info.productDes = string.IsNullOrEmpty(m_data.SpadeDes) ? " " : m_data.SpadeDes;
		info.serverId    = "";
		info.url         = ad.payCallback;
		info.appOrderId  = data.orderId;
		info.isPayMonth  = m_data.Type == 0;

		return fastJSON.JSON.Instance.ToJSON (info);
	}
	#endif

	#if Google
//	string generateGoogle(PayData data) {
//		HTPayInfo info = new HTPayInfo();
//		AccountData ad = Native.mInstace.m_thridParty.GetAccountData ();
//
//		info.price = m_data.Price;
//		info.count = 1;
//		info.productId   = m_data.ID.ToString();
//		info.productName = string.IsNullOrEmpty(m_data.SpadeName) ? " " : m_data.SpadeName;
//		info.productDes = string.IsNullOrEmpty(m_data.SpadeDes) ? " " : m_data.SpadeDes;
//		info.serverId    = "";
//		info.url         = ad.payCallback;
//		info.appOrderId  = data.orderId;
//		info.isPayMonth  = m_data.Type == 0;
//
//		return fastJSON.JSON.Instance.ToJSON (info);
//	}
//	#endif

	#endif
	#endregion
	//重复查询支付结果
	public void LoopQueryPayStatus()
	{
		try {
			InvokeRepeating ("QueryPayStatus", 0f, 2f);
		} catch(System.Exception ex) {
			Debug.Log(ex.ToString());
		}

	}
		
	//下订单
	void SendPayRQ()
	{
		ComLoading.Open ();
		RED.Log ("向服务器发送订单");
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);

		AccountData ad = Native.mInstace.m_thridParty.GetAccountData ();
		if (ad == null) 
		{
			RED.LogWarning ("第三方数据为null");
			return;
		}
        //talking data add by wxl 
		if (m_data != null && m_data.Present2 != null && m_data.Present2.Count != 0 && m_data.Present2[0] != null && m_data.Present2[0].Length > 1)
		{
            Core.Data.ActivityManager.OnChargeRequest (m_data.Title, m_data.ID.ToString(), m_data.Price, m_data.Present2[0][1]);
        }
		task.AppendCommonParam(RequestType.PAY, new PayParam(m_data.ID.ToString()));

		HttpRequest req = task.request as HttpRequest;
		req.Url = Core.SM.curServer.payUrl;

		task.ErrorOccured += HttpResp_Error;
		task.afterCompleted += HttpResp_UI;

		task.DispatchToRealHandler();
	}

	void QueryPayStatus()
	{
		//查询超过60次，就停止查询
		if (curQueryCnt > QueryMaxCnt)
		{
			CancelInvoke ();
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (5236));
			return;
		}

		curQueryCnt++;
		ComLoading.Open ();
		RED.Log ("向服务器发送查询订单状态消息");
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.QUERY_PAY_STATUS,  new QueryPayStatusParam(Core.Data.playerManager.PlayerID, m_strOrderId));

		task.ErrorOccured += HttpResp_Error;
		task.afterCompleted += HttpResp_UI;

		task.DispatchToRealHandler();
	}

	/*
	void OnGUI() {
		if(GUI.Button(new Rect(0, 0, 100, 100), "test")) {
			HttpRequest req = new HttpRequest(RequestType.QUERY_PAY_STATUS, 2001, "ios");
			string json = @"{""act"":1002,""data"":{""p"":[{""ppid"":0,""pid"":110052,""num"":120}],""billStatus"":1,""vipStatus"":{""userVip"":1,""totalCach"":600,""toNextLevle"":600,""curStone"":130}}}";
			PayStatusResponse resp = fastJSON.JSON.Instance.ToObject<PayStatusResponse>(json);
			HttpResp_UI(req, resp);
		}
	} */

	#region 网络返回
	void HttpResp_UI (BaseHttpRequest request, BaseResponse response) 
	{
		ComLoading.Close ();
		if(response != null && response.status != BaseResponse.ERROR) 
		{
			HttpRequest myRequest = (HttpRequest)request;
			switch(myRequest.Type) 
			{
			case RequestType.PAY:
				PayResponse payResp = response as PayResponse;
				if (!string.IsNullOrEmpty (payResp.data.orderId)) 
				{
					m_strOrderId = payResp.data.orderId;
					AccountData ad = Native.mInstace.m_thridParty.GetAccountData ();
					ad.payCallback = payResp.data.payCallback;
					UIDragonMallMgr.GetInstance ().mUIDragonRechargeMain.SetCurPayCell (this);
					OpenThirdPayUI (payResp.data);
				}
				else
				{
					RED.LogWarning ("订单号为null");
				}
				break;
            case RequestType.QUERY_PAY_STATUS:
                PayStatusResponse resp = response as PayStatusResponse;

				RED.Log ("收到订单状态：" + resp.data.billStatus);
				if (resp.data.billStatus == 1) 				//重置成功，展示物品
				{
					UIDragonMallMgr.GetInstance().mUIDragonRechargeMain.Refresh();
					CancelInvoke ();
					if (resp.data.p != null) {
						int length = resp.data.p.Length;
						for (int i = 0; i < length; i++) {
							Core.Data.itemManager.AddRewardToBag (resp.data.p[i]);
                            //talking data add by wxl 
                            ItemData itemData = Core.Data.itemManager.getItemData (resp.data.p[i].pid);
                            if (itemData != null) {
								if (m_data != null) {
									RechargeData tdata = Core.Data.rechargeDataMgr.GetRechargeData (m_data.ID);
									if (tdata != null && tdata.Present2 != null) {
										if(tdata.Present2.Count == 2 )
											Core.Data.ActivityManager.OnChargeSuccess (tdata.Title, tdata.ID.ToString (), tdata.Present2 [0] [1]);
									}
								}
                            }
						}
					}


					UIDragonMallMgr mallMgr = UIDragonMallMgr.GetInstance ();
					if(resp.data.vipStatus != null) {
						Core.Data.playerManager.RTData.curStone = resp.data.vipStatus.curStone;

						Core.Data.vipManager.vipStatus = resp.data.vipStatus;
						RED.Log ("left rmb：" + resp.data.vipStatus.toNextLevle);
						int next = resp.data.vipStatus.toNextLevle / 100;
						int total = resp.data.vipStatus.totalCach/100;
						mallMgr.SeViptPercent(next,total);
					}

					mallMgr.mUIDragonRechargeMain.SetCurPayCell (null);
					if(UIFirstRechargePanel.GetInstance() == null) {
						DBUIController.mDBUIInstance.RefreshUserInfo ();
					} else {
						DBUIController.mDBUIInstance.RefreshUserInfoWithoutShow ();
						UIMiniPlayerController.Instance.freshPlayerInfoView ();
					}


					GetRewardSucUI.OpenUI (resp.data.p, Core.Data.stringManager.getString (5210), true, () => {
						//点击OK后，出现Vip信息
						if(resp.data.vipStatus != null) {
							PlayerManager player = Core.Data.playerManager;
							int temp = player.RTData.curVipLevel;
							int now  = resp.data.vipStatus.userVip;
							player.RTData.curVipLevel = now;
							UIDragonMallMgr.GetInstance().VipRequest();
							if(now > temp) {
								LevelUpUIOther.OpenUI();
								LevelUpUIOther.mInstance.showVipUpdate(now);
								SQYMainController.mInstance.RefreshVipLv();
							}
						}
					} );
					//检测是否是 月卡
					if (m_data.ID == 1) 
						ActivityNetController.GetInstance ().GetMonthStateRequest ();
					//首充   2是已充已领   1是已充未领 
					if(LuaTest.Instance.OpenFirstCharge) {
						if (Core.Data.rechargeDataMgr._canGainFirstAward == 0 || Core.Data.rechargeDataMgr._canGainFirstAward == 1 || Core.Data.rechargeDataMgr._canGainFirstAward == -1)
						{
							ActivityNetController.GetInstance ().GetFirstChargeStateRequest ();
						}
					}
				}
				else if (resp.data.billStatus == 2) 		//充值成功，订单过期
				{
					CancelInvoke ();
				}
				break;
			}
		}
	}


		
	void HttpResp_Error (BaseHttpRequest request, string error) 
	{
		ComLoading.Close ();
		if(request != null && request is HttpRequest)
		{
			HttpRequest myRequest = (HttpRequest)request;
			switch(myRequest.Type)
			{
			case RequestType.PAY:
				SQYAlertViewMove.CreateAlertViewMove (error);
				break;
			}
		}
	}

	#endregion

}