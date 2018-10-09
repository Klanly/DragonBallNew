using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
public class SQYMainController : MDBaseViewController 
{
	
	#region button index 
	public const int CLICK_HuoDong = 1;
	public const int CLICK_MONSTER = 2;
	public const int CLICK_HaoYou = 3;
	public const int CLICK_BeiBao = 4;
	public const int CLICK_ShenLong = 5;
	public const int CLICK_ShangCheng = 6;
	public const int CLICK_XiaoXi = 7;
	public const int CLICK_ZHENRONG = 8;
	public const int CLICK_JiaYuan = 9;
	public const int CLICK_FuBen = 10;
	public const int CLICK_DuoBao = 11;
	public const int CLICK_GongHui = 12;
	public const int CLICK_QiTianJiangLi = 13;
	public const int CLICK_RECHARGE = 14;

    public const int CLICK_RollAct = 15;
    public const int CLICK_GodGift= 16;
    public const int CLICK_SuperGift = 17;
    public const int CLICK_HappyScratch = 18;
    public const int CLICK_RadarGroup = 19;
	public const int CLICK_FIRSTRECHARGE = 20;
	public const int CLICK_ACTIVITY = 21;
	public const int CLICK_DragonBank = 22;
	#endregion

	public const float TWEENSCALE_TIME = 0.1F;

	[HideInInspector]
	public bool m_bCanRotate = false;							// 副本的雷达指针是否可以旋转

	private static SQYMainController _mInstance;
	public static SQYMainController mInstance
	{
		get
		{
			return _mInstance;
		}
	}
	
	public UIGrid BtnGrid;

	public TweenScale m_scaleHead;				//头像
	public TweenScale m_Download;				//下载
	public TweenScale m_DailySign;				//每日签到
	public TweenScale m_FirstCharge;			//首冲
	public TweenScale m_scaleBtnRoot;			//挑战 + 副本
	public TweenScale m_ActBtn;					//微信
	public TweenScale m_NewActBtn;				//限时活动

	public TweenScale m_scaleShop;				//商城
	public TweenScale m_scaleChat;				//聊天
	public RightMenuUI m_menuUI;				//右边按钮

	public Spin m_objSubFuben;					//副本动画
	public BtnChallengeEffect m_btnChallenge;	//挑战动画
	public BtnShowChatEffect m_btnChatEffect;	//聊天动画

	public UISprite m_spHead;
	public UILabel m_txtName;
	public UILabel m_txtLevel;
	public UILabel m_txtVip;

	public GameObject vipNone;
	public GameObject vipReal;
	public UISprite mVipicon;
	public UILabel mViplv;

	public UILabel atkLabel;					//攻击值
	public UILabel defLabel;					//防御值

	public UISlider m_expSlider;				//经验

    List<LabelEffect> LELabels;

	private BtnWheelCtrller m_btnWheel;			//命运转轮按钮
	public UISprite sp_Tip;
	public UILabel m_NameLabel;
	public UISprite sp_ActivityTip;
	void Awake() 
	{
		_mInstance = this;
        LELabels = new List<LabelEffect>();
	}

	void OnEnable()
	{
#if SPLIT_MODEL
		//是否领取下载奖励(0 下载未领取，   1 下载已经领取)
		if( Core.Data.playerManager.RTData.downloadReawrd != 1)
		{
			bool show = Core.Data.sourceManager.DoClientNeedUpdateModles();
			RED.SetActive(show, m_Download.gameObject);
			ArrangeLeftBtnPos();
			//检测一下当前的状态
			SetDownloadFinish();
		}
#endif
	}


	// Use this for initialization
	void Start () 
	{
		//m_btnWheel = m_scaleWheel.GetComponent<BtnWheelCtrller>();

		RefreshUserInfo();
        //m_Download.gameObject.SetActive(false);
#if SPLIT_MODEL
//		bool show = Core.Data.sourceManager.DoClientNeedUpdateModles();
//		RED.SetActive(show, m_Download.gameObject);


//		if(Core.Data.playerManager.RTData.curLevel >= 10)
//		{
//			m_scaleWheel.transform.localPosition = Vector3.up * -100;
//			m_Download.transform.localPosition = Vector3.up * -200;
//		}
//		else
//		{
//			m_scaleWheel.transform.localPosition = Vector3.up * -200;
//			m_Download.transform.localPosition = Vector3.up * -100;
//		}
#endif
		ResetScale (1.0f);
		SetChatAnimEnable (false);
		m_bCanRotate = true;
		BtnGrid.Reposition ();
		UpdateTeamTip ();
		UpdateBagTip ();
		UpdateDailySign ();
		ObserveRouletteState ();
		//	ObserveFirstCharge ();
	}

    public void SetDownloadFinish(bool isClick = false) 
	{
		if(Core.Data.guideManger.isGuiding) return;
        HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.DOWNLOAD_FINISH, new DownloadFinishParam (int.Parse(Core.Data.playerManager.PlayerID), SoftwareInfo.VersionCode,0));
		task.afterCompleted = (BaseHttpRequest request, BaseResponse response) =>
		{
			if (response != null && response.status != BaseResponse.ERROR)
			{
				GetDownloadFinishResponse FinishResponse = response as GetDownloadFinishResponse;
				if (FinishResponse.data.status >= 0)
				{
					int state = Core.Data.playerManager.RTData.downloadReawrd;
					Core.Data.playerManager.RTData.downloadReawrd = FinishResponse.data.status;
					//bool show = Core.Data.sourceManager.DoClientNeedUpdateModles();
//					bool show = Core.Data.playerManager.RTData.downloadReawrd != 1;
//					RED.SetActive(show, m_Download.gameObject);
					if(Core.Data.playerManager.RTData.downloadReawrd == 0)
					{
						RED.SetActive(true, m_Download.gameObject);
						BtnGrid.Reposition();
					}
						

					if(state == -1 && isClick)
						UIDownloadPacksWindow.OpenDownLoadUI(LoadFinish);
				} 
				else
				{
					RED.LogWarning("FinishResponse.data.status Error!!!!!");
				}
			}
		};
        task.ErrorOccured += FinishAllErrorBack;
        task.DispatchToRealHandler ();
    }
		

    public void FinishAllErrorBack (BaseHttpRequest request, string error){
        Debug.Log ("ReceiveAllErrorBack : "+error);
    }

	#region added by zhangqiang to update main ui
	//更新主界面
	public void UpdateUI()
	{
		UpdateTeamTip ();
		UpdateBagTip ();
		UpdateDailySign ();
		UpdateDailyGiftTip ();
		UpdateActGiftTip ();
		UpdateTopTip ();
		ArrangeLeftBtnPos ();
		RefreshVipLv ();
		ObserveRouletteState ();
		RefreshUserInfo ();
		if(billingCD.BillCountDown != null)
			billingCD.BillCountDown.NetworkCallBack (Core.Data.temper.PurStatus);
	}
	#endregion
		
	public void RefreshUserInfo()
	{
//		Debug.Log (" head id " +Core.Data.playerManager.RTData.headID.ToString() );
		AtlasMgr.mInstance.SetHeadSprite(m_spHead, Core.Data.playerManager.RTData.headID.ToString());
		m_scaleHead.transform.FindChild ("userHead").GetComponent<UIButton> ().normalSprite = Core.Data.playerManager.RTData.headID.ToString();

		m_txtName.text = Core.Data.playerManager.NickName;
		m_txtLevel.text = Core.Data.playerManager.Lv.ToString();
		m_txtVip.text = "VIP" + Core.Data.playerManager.curVipLv.ToString();

		atkLabel.text = Core.Data.playerManager.RTData.curTeam.teamAttack.ToString();
		defLabel.text = Core.Data.playerManager.RTData.curTeam.teamDefend.ToString();

		float res = (float)Core.Data.playerManager.curExp / (float)Core.Data.playerManager.nextLvExp;
		if(res < 0.1f)
		{
			res = 0.1f;
		}
		if(m_expSlider != null)
		{
			m_expSlider.value = res;
		}
			

		if(Core.Data.playerManager.curVipLv <= 0)
		{
			vipNone.gameObject.SetActive(true);
			vipReal.gameObject.SetActive(false);
		}
		else
		{
			vipNone.gameObject.SetActive(false);
			vipReal.gameObject.SetActive(true);

			mViplv.text = Core.Data.playerManager.curVipLv.ToString();
			if (Core.Data.playerManager.curVipLv < 4) {
				mVipicon.spriteName = "common-2008";
			} else if (Core.Data.playerManager.curVipLv > 3 && Core.Data.playerManager.curVipLv < 8) {
				mVipicon.spriteName = "common-2009";
			} else if (Core.Data.playerManager.curVipLv > 7 && Core.Data.playerManager.curVipLv < 12) {
				mVipicon.spriteName = "common-2007";
			} else {
				mVipicon.spriteName = "common-2109";
			}
			mVipicon.MakePixelPerfect ();
		}



		//更新商城解锁状态
		if (Core.Data.BuildingManager.ZhaoMuUnlock)
		{
			SetBtnEnable (m_scaleShop.gameObject, true);
		}
		else
		{
			SetBtnEnable (m_scaleShop.gameObject, false);
		}


		if (LuaTest.Instance.OpenFirstCharge) {
			int canGain = Core.Data.rechargeDataMgr._canGainFirstAward;

			if (canGain == 1) {//已充值  未领取
				RED.SetActive (true, m_FirstCharge.gameObject);
			} else if (canGain == 2) {//已充值  已领取
				RED.SetActive (false, m_FirstCharge.gameObject);
			} else if (canGain == 0) {  //未充值
				RED.SetActive (true, m_FirstCharge.gameObject);
			} else if (canGain == -1) {
				ActivityNetController.GetInstance ().GetFirstChargeStateRequest ();
				//RED.SetActive (false, m_FirstCharge.gameObject);
			}
		} else {
			RED.SetActive (false, m_FirstCharge.gameObject);
		} 

		SetSignDay ();
		ChatEnter ();
		SetLeftShop ();
		ArrangeNewActivity ();

		ArrangeWeiXin ();

        BtnGrid.Reposition ();
		BtnGrid.repositionNow = true;

		if (Core.Data.playerManager.RTData.curJingLi <= 5)
		{
			//挑战动画
			if (Core.Data.playerManager.curTiLi > 0 && Core.Data.playerManager.RTData.curLevel >= 10)
			{
				m_btnChallenge.enabled = true;
			}
			else
			{
				m_btnChallenge.enabled = false;
			}
		}
		else
		{
			m_btnChallenge.enabled = false;
		}

		#if SPLIT_MODEL
		//是否领取下载奖励(0 下载未领取，   1 下载已经领取)
			bool show = Core.Data.sourceManager.DoClientNeedUpdateModles();
			RED.SetActive(show, m_Download.gameObject);
			//检测一下当前的状态
//			SetDownloadFinish();
		#endif



			
		BtnGrid.Reposition ();
	}

	//设置按钮是否可用
	void SetBtnEnable(GameObject obj, bool enable)
	{
		Color clr = enable ? Color.white : Color.black;
		UISprite[] arry = obj.GetComponentsInChildren<UISprite>();
		if (arry != null)
		{
			for (int i = 0; i < arry.Length; i++)
			{
				arry [i].color = clr;
			}
		}
		UISprite sp = obj.GetComponent<UISprite>();
		if (sp != null)
		{
			sp.color = clr;
		}
	}

    /// <summary>
    /// 展示 队伍攻击防御力 
    /// </summary>
    public void ShowUpdate(){
        string tDeltaAtk = "";
        string tDeltaDef = "";
        float tTime = 2f;
        float tHeight = 15;
        if (Core.Data.temper.deltaAtk > 0)
        {
            tDeltaAtk = "+" + Core.Data.temper.deltaAtk;
            LabelEffect tLE =  LabelEffect.CreateLabelEffect(tDeltaAtk,tHeight, tTime, LabelEffect.lightGreen, atkLabel.gameObject.transform.localPosition + Vector3.right * 80,atkLabel.transform.parent,atkLabel.depth);
            LELabels.Add (tLE);
        }
        else if(Core.Data.temper.deltaAtk < 0)
        {   tDeltaAtk =  Core.Data.temper.deltaAtk.ToString();
            LabelEffect tLE1 = LabelEffect.CreateLabelEffect(tDeltaAtk, tHeight, tTime, Color.red, atkLabel.gameObject.transform.localPosition + Vector3.right * 80,atkLabel.transform.parent,atkLabel.depth);
            LELabels.Add (tLE1);
        }
        if (Core.Data.temper.deltaDef > 0)
        {
            tDeltaDef = "+" + Core.Data.temper.deltaDef;
            LabelEffect tLE2 =LabelEffect.CreateLabelEffect(tDeltaDef, tHeight, tTime, LabelEffect.lightGreen, atkLabel.gameObject.transform.localPosition + Vector3.right * 80,defLabel.transform.parent,defLabel.depth);
            LELabels.Add (tLE2);
        }
        else if((Core.Data.temper.deltaDef < 0))
        {   tDeltaDef =  Core.Data.temper.deltaDef.ToString();
            LabelEffect tLE3 = LabelEffect.CreateLabelEffect(tDeltaDef, tHeight, tTime, Color.red, atkLabel.gameObject.transform.localPosition + Vector3.right * 80,defLabel.transform.parent,defLabel.depth);
            LELabels.Add (tLE3);
        }

        Core.Data.temper.deltaAtk = 0;
        Core.Data.temper.deltaDef = 0;
            
    }


	public void WillToMainView(int clickIdx)
	{
		DBUIController.mDBUIInstance.OnBtnMainViewID(clickIdx);
	}

	public void OnBtnMonster(){WillToMainView(CLICK_MONSTER);}
	public void OnBtnHaoYou(){WillToMainView(CLICK_HaoYou);}
	public void OnBtnBaiBao(){WillToMainView(CLICK_BeiBao);}
	public void OnBtnShenLong(){WillToMainView(CLICK_ShenLong);}
	public void OnBtnShangChang(){WillToMainView(CLICK_ShangCheng);}
	public void OnBtnXiaoXi(){WillToMainView(CLICK_XiaoXi);}
	public void OnBtnJiaYuan(){WillToMainView(CLICK_JiaYuan);}
	public void OnBtnFuBen(){WillToMainView(CLICK_FuBen);}
	public void OnBtnDuoBao(){WillToMainView(CLICK_DuoBao);}
	public void OnBtnGongHui(){WillToMainView(CLICK_GongHui);}
	public void OnBtnSevenDayReward(){WillToMainView(CLICK_QiTianJiangLi);}
	public void OnBtnNewActivity(){WillToMainView(CLICK_ACTIVITY);}


	public void OnBtnGoDailySign(){
		WXLAcitvityFactory.CreatActivity (ActivityItemType.qiandaoItem,DBUIController.mDBUIInstance._TopRoot);
	}


    public void OnDownloadPacks()
	{
		//---------------------
		//------下载礼包被点击，打开下载窗口 <说明当前客户端需要下载，但不知道玩家是否已领取礼包 >
		//---------------------
		if(Core.Data.playerManager.RTData.downloadReawrd == -1)
		{
			//检测一下当前的状态
			SetDownloadFinish(true);
		}
		else
            UIDownloadPacksWindow.OpenDownLoadUI(LoadFinish);
    }
	
	public void OnBtnTask()
	{
		UITask.Open();
	}

	//点击命运转盘按钮
	void OnBtnWheel()
	{
		switch (m_btnWheel.actState)
		{
        case 0:
            DBUIController.mDBUIInstance.OnBtnMainViewID(CLICK_RollAct);
		    break;
        case 1:
            DBUIController.mDBUIInstance.OnBtnMainViewID(CLICK_SuperGift);
    		break;
        case 2:
            DBUIController.mDBUIInstance.OnBtnMainViewID(CLICK_GodGift);
		    break;
		case 3:
            DBUIController.mDBUIInstance.OnBtnMainViewID(CLICK_HappyScratch);
			break;
		case 4:
            DBUIController.mDBUIInstance.OnBtnMainViewID(CLICK_RadarGroup);
			break;
		}
        DBUIController.mDBUIInstance.HiddenFor3D_UI(true);
	}

	//更新装盘状态
	public void UpdateWheelState(int state)
	{
		m_btnWheel.UpdateState (state);
	}

    private void LoadFinish(int type){
//        Debug.Log("type : "+type);
		UpdateBagTip();
    }

	public void OnBtnHuoDong(){
		WillToMainView(CLICK_HuoDong);
	}
	public void OnBtnZhenRong()
	{
		WillToMainView(CLICK_ZHENRONG);
	}

	public void OnBtnFirstCharge(){
		WillToMainView (CLICK_FIRSTRECHARGE);
	}
		
	/// <summary>
	/// 弹入设置界面
	/// </summary>
#region 第一次请求服务器combo次数 add by jc
	static bool isFristOpenClient = true;
	public void GoToOption()
	{
		if(isFristOpenClient)
			SendMsg();
		else
		    UIOptionController.CreatOptionCtrl(DBUIController.mDBUIInstance._TopRoot);
	}
	
	void SendMsg()
	{
		ComLoading.Open();
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
        task.AppendCommonParam(RequestType.GET_COMBO, new GetComboParam(Core.Data.playerManager.PlayerID) );
        
		task.ErrorOccured = (BaseHttpRequest b, string error) => { ConsoleEx.DebugLog ("______Error =" + error.ToString());  ComLoading.Close();};
        task.afterCompleted = (BaseHttpRequest request, BaseResponse response) =>
		{
			ComLoading.Close();
			if (response.status != BaseResponse.ERROR) {
				isFristOpenClient = false;
				ComboResponse resp = response as ComboResponse;

				RTPlayer player = Core.Data.playerManager.RTData;

				player.TotalCombo = resp.data.combo.total;
				player.TotalGambleWin = resp.data.gamble.win;
				player.TotalGambleLose = resp.data.gamble.lose;

				UIOptionController.CreatOptionCtrl(DBUIController.mDBUIInstance._TopRoot);
			}
			else
				SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getNetworkErrorString(response.errorCode));
		};
        
        task.DispatchToRealHandler();
	}
	
#endregion

	
	public static SQYMainController CreateMainView()
	{
		UnityEngine.Object obj = PrefabLoader.loadFromPack("SQY/pbSQYMainController");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			SQYMainController mv = go.GetComponent<SQYMainController>();
			return mv;
		}
		return null;
	}

	public void willShowUI(float time = TWEENSCALE_TIME)
	{
		if (!this.gameObject.activeInHierarchy)
		{
			return;
		}

		AnimationCurve anim = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(0.4f, 1.3f, 0.5f, 0.5f), new Keyframe(1f, 1f, 1f, 0f));

		m_scaleChat.animationCurve = anim;
		m_scaleBtnRoot.animationCurve = anim;
		m_scaleHead.animationCurve = anim;
		m_scaleShop.animationCurve = anim;
		m_DailySign.animationCurve = anim;
		m_FirstCharge.animationCurve = anim;
        #if SPLIT_MODEL
        m_Download.animationCurve = anim;
        #endif
		

		TweenScale.Begin(m_scaleChat.gameObject, time, Vector3.one);
		TweenScale.Begin(m_scaleBtnRoot.gameObject , time, Vector3.one);
		TweenScale.Begin(m_scaleHead.gameObject, time, Vector3.one);
		TweenScale.Begin(m_scaleShop.gameObject, time, Vector3.one);
		TweenScale.Begin(m_DailySign.gameObject, time, Vector3.one);
		TweenScale.Begin (m_FirstCharge.gameObject,time,Vector3.one);
		TweenScale.Begin (m_ActBtn.gameObject,time,Vector3.one);
		TweenScale.Begin (m_NewActBtn.gameObject,time,Vector3.one);

        #if SPLIT_MODEL
        TweenScale.Begin(m_Download.gameObject, time, Vector3.one);
        #endif


		m_menuUI.willShowUI (time);
	}

	public void willHideUI()
	{
		if (!this.gameObject.activeInHierarchy)
		{
			return;
		}

		AnimationCurve anim = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));
	
		m_scaleChat.animationCurve = anim;
		m_scaleBtnRoot.animationCurve = anim;
		m_scaleHead.animationCurve = anim;
		m_scaleShop.animationCurve = anim;
		m_DailySign.animationCurve = anim;
		m_FirstCharge.animationCurve = anim;
		#if SPLIT_MODEL
		m_Download.animationCurve = anim;
		#endif

		TweenScale.Begin(m_scaleChat.gameObject, TWEENSCALE_TIME, Vector3.one *0.0001f);
		TweenScale.Begin(m_scaleBtnRoot.gameObject, TWEENSCALE_TIME, Vector3.one *0.0001f);
		TweenScale.Begin(m_scaleHead.gameObject, TWEENSCALE_TIME, Vector3.one *0.0001f);
		TweenScale.Begin(m_scaleShop.gameObject, TWEENSCALE_TIME, Vector3.one *0.0001f);
		TweenScale.Begin(m_DailySign.gameObject, TWEENSCALE_TIME, Vector3.one *0.0001f);
		TweenScale.Begin (m_FirstCharge.gameObject,TWEENSCALE_TIME,Vector3.one*0.0001f);
		TweenScale.Begin (m_ActBtn.gameObject,TWEENSCALE_TIME,Vector3.one*0.0001f);
		TweenScale.Begin(m_NewActBtn.gameObject, TWEENSCALE_TIME, Vector3.one *0.0001f);
		#if SPLIT_MODEL
		TweenScale.Begin(m_Download.gameObject, TWEENSCALE_TIME, Vector3.one *0.0001f);
		#endif

		m_menuUI.willHideUI ();
	}

    /// <summary>
    /// 跟上面一样 
    /// </summary>
	public void ResetScale(float scale)
	{
		m_scaleChat.transform.localScale = Vector3.one * scale;
		m_scaleBtnRoot.transform.localScale = Vector3.one * scale;
		m_scaleShop.transform.localScale = Vector3.one * scale;
		m_DailySign.transform.localScale = Vector3.one * scale;
		m_scaleHead.transform.localScale = Vector3.one * scale;
		m_FirstCharge.transform.localScale = Vector3.one * scale;
		m_ActBtn.transform.localScale = Vector3.one * scale;
		m_NewActBtn.transform.localScale = Vector3.one*scale;

		#if SPLIT_MODEL
		m_Download.transform.localScale = Vector3.one * scale;
		#endif

		m_menuUI.ResetScale (scale);
    }
    void OnDisable(){
		this.ResetScale(1.0f);
        if (LELabels.Count  != 0) {
            for(int i =0;i<LELabels.Count;i++){
                if(LELabels[i] != null)
                    Destroy(LELabels[i].gameObject);
            }
        }
    }

	void Update()
	{
		if (Core.Data.playerManager.RTData.curJingLi > 5 && m_bCanRotate)
		{
			if(!m_objSubFuben.enabled)
			m_objSubFuben.enabled = true;
		}
		else
		{
			if(m_objSubFuben.enabled)
			m_objSubFuben.enabled = false;
		}
	}
	//更新  签到红点
	public void UpdateDailySign(){
		if( Core.Data.ActivityManager.GetSignState() == "1"){
			sp_Tip.gameObject.SetActive (true);
		} else {
			sp_Tip.gameObject.SetActive (false);
		}
	}

	//更新背包红点儿提示
	public void UpdateBagTip()
	{
		m_menuUI.UpdateBagTip ();
	}

	//更新每日奖励红点儿提示
	public void UpdateDailyGiftTip()
	{
		m_menuUI.UpdataDailyGiftTip ();
	}



	//更新活动红点儿提示
	public void UpdateActGiftTip()
	{
		m_menuUI.UpdataActGiftTip ();
	}

	//更新队伍提示红点儿
	public void UpdateTeamTip()
	{
		m_menuUI.UpdateTeamTip ();
	}

	//更新顶部红点儿
	public void UpdateTopTip()
	{
		m_menuUI.UpdateToptip ();
	}

	//设置聊天动画
	public void SetChatAnimEnable(bool enable)
	{
		m_btnChatEffect.enabled = enable;
	}
		

	//观察  今日转盘是啥 
	void ObserveRouletteState(){
		if(ActivityManager.hasUpdateValue == false)
			RouletteLogic.sendSer (HttpRequst, null, 0);
	}
	void HttpRequst(BaseHttpRequest request, BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR)
		{
			GetAwardActivity resp = response as GetAwardActivity;
			if (resp != null && resp.data != null && resp.data.status != null)
			{
				//写入 读取
				ActivityManager.activityZPID = resp.data.status.id;
			}
		}
	}

	public void ArrangeLeftBtnPos (){
		if (LuaTest.Instance.OpenFirstCharge) {
			int canGain = Core.Data.rechargeDataMgr._canGainFirstAward;
			if (canGain == 1) {//已充值  未领取
				RED.SetActive (true, m_FirstCharge.gameObject);
			} else if (canGain == 2) {//已充值  已领取
				RED.SetActive (false, m_FirstCharge.gameObject);
			} else if (canGain == 0) {  //未充值
				RED.SetActive (true, m_FirstCharge.gameObject);
			}
		} else {
			RED.SetActive (false, m_FirstCharge.gameObject);
		}
		BtnGrid.Reposition ();
		BtnGrid.repositionNow = true;
	}

//	//更新空槽位和空装备个数
//	public void UpdateEmptyTip()
//	{
//		int totalMember = Core.Data.playerManager.curConfig.petSlot;
//		int valid = Core.Data.playerManager.RTData.curTeam.validateMember;
//		int empty = totalMember - valid;
//
//		for (int i = 0; i < valid; i++)
//		{
//			for(short j = 0; j < 2; j++)
//			{
//				if (Core.Data.playerManager.RTData.curTeam.getEquip (i, j) == null)
//				{
//					empty++;
//				}
//			}
//		}
//			
//		RED.SetActive (empty > 0, m_emptyTip);
//	}
//
	//升级 vip 之后 刷新 方法 
	public void RefreshVipLv(){
		if(Core.Data.playerManager.curVipLv <= 0)
		{
			vipNone.gameObject.SetActive(true);
			vipReal.gameObject.SetActive(false);
		}
		else
		{
			vipNone.gameObject.SetActive(false);
			vipReal.gameObject.SetActive(true);

			mViplv.text = Core.Data.playerManager.curVipLv.ToString();
			if (Core.Data.playerManager.curVipLv < 4) {
				mVipicon.spriteName = "common-2008";
			} else if (Core.Data.playerManager.curVipLv > 3 && Core.Data.playerManager.curVipLv < 8) {
				mVipicon.spriteName = "common-2009";
			} else if (Core.Data.playerManager.curVipLv > 7 && Core.Data.playerManager.curVipLv < 12) {
				mVipicon.spriteName = "common-2007";
			} else {
				mVipicon.spriteName = "common-2109";
			}
			mVipicon.MakePixelPerfect ();
		}
		if(m_menuUI.gameObject != null)
			UpdateDailyGiftTip ();
		if(sp_Tip.gameObject!= null)
			UpdateDailySign ();
		UpdateActGiftTip ();
	}

//	void OnGUI(){
//		if (GUI.Button (new Rect (200, 200, 100, 100), "text ")) {
//			LevelUpUIOther.OpenUI ();
//			LevelUpUIOther.mInstance.showVipUpdate (12);
//		}
	//	}gamb

	public void ArrangeWeiXin(){
		if(LuaTest.Instance != null)
		{
			bool ShowAct = LuaTest.Instance.OpenWeiXin;
			if (ShowAct) {
				RED.SetActive (true, m_ActBtn.gameObject);
			} else {
				RED.SetActive (false, m_ActBtn.gameObject);
			}
		}
		BtnGrid.repositionNow = true;
	}

	public void ArrangeNewActivity(){
		if (m_NewActBtn != null) {
//			UILabel lblName = m_NewActBtn.GetComponentInChildren<UILabel> ();

			if (m_NameLabel != null) {
				NewActivityResponse tData = Core.Data.HolidayActivityManager.har;
				if (tData != null) {
					if (tData.data != null  && tData.data.Length > 0) {
						m_NameLabel.text = Core.Data.stringManager.getString (25168);
						RED.SetActive (true, m_NewActBtn.gameObject);

					} else {
						RED.SetActive (false, m_NewActBtn.gameObject);
					}
				} else {
					RED.SetActive (false, m_NewActBtn.gameObject);
				}
				if (m_NewActBtn.gameObject.activeSelf) {
					if (sp_ActivityTip != null) {
						if (tData != null) {
							RED.SetActive (tData.IsGetReward(),sp_ActivityTip.gameObject);
						}
					}
				}
				BtnGrid.repositionNow = true;

			}
		}
	}

	public void JumpToWeixin()
	{
		GetGiftPanelController.CreateUIRewardPanel(DBUIController.mDBUIInstance._bottomRoot);
		if (GetGiftPanelController.Instance != null)
			AsyncTask.QueueOnMainThread(()=>{GetGiftPanelController.Instance.OnBtnWeiXin ();},0.1f);
	}

	public void SetLeftShop(){
		m_scaleShop.gameObject.SetActive (LuaTest.Instance.OpenStore);
	}


	public void SetSignDay(){
//		Debug.Log ("switch  sign  === " + LuaTest.Instance.OpenSign);
		m_DailySign.gameObject.SetActive (LuaTest.Instance.OpenSign);
	}

	public void ChatEnter(){
//		Debug.Log ("switch chat    === " + LuaTest.Instance.OpenChat);
		m_scaleChat.gameObject.SetActive (LuaTest.Instance.OpenChat);
	}







}
