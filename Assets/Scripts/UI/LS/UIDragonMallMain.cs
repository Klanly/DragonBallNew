using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIDragonMallMain : RUIMonoBehaviour {

    public UIButton mBtn1;
    public UIButton mBtn2;
    public UIButton mBtn3;
    public UIButton mBtn4;
	public UIButton mRechargeBtn;

    public UISprite[] mSpriteL;
    public UISprite[] mSpriteR;

	public UILabel mVipCur;
	public UISlider mSlider;
	public UILabel mExc;
	public UILabel mVipTitle1;
	public UILabel mMoney;
	public UILabel mVipTitle2;
	public UILabel mVipNext;

	public GameObject m_Pic;

	UIDragonMallMgr m_Mgr{
		get{
			return UIDragonMallMgr.GetInstance();
		}
	}

//    public UILabel[] mBtnLabel;
    /// <summary>
    /// 记录 是否从商店进入背包  （前提是从商店进入 一定回到商店）
    /// </summary>
    public static bool enterFromMall = false;

	void Start () 
    {
		LuaTest _luatest = LuaTest.Instance;
		if(_luatest != null && _luatest.HasBilling)
		{
			mRechargeBtn.isEnabled = true;
		}
		else
		{
			mRechargeBtn.isEnabled = false;
		}
		SetBtnStatus();

        enterFromMall = false;
	}

	void OnEnable()
	{
		if(UIDragonMallMgr.GetInstance().mDragonMallScrollViewControl != null)
		{
			UIDragonMallMgr.GetInstance().mDragonMallScrollViewControl.mGrid.Reposition();
		}
		
	}

	UIButton CheckShopTypeBtn(ShopItemType m_type)
	{
		if(m_type == ShopItemType.HotSale) return mBtn1;
		else if(m_type == ShopItemType.Egg) return mBtn2;
		else if(m_type == ShopItemType.Item) return mBtn3;
		else 
			return mBtn4;
	}

	void HideBtn(List<UIButton> mBtnlist)
	{
		if(mBtnlist != null && mBtnlist.Count > 1)
		{
			NGUITools.SetActive(mBtn1.gameObject, mBtnlist.Contains(mBtn1));
			NGUITools.SetActive(mBtn2.gameObject, mBtnlist.Contains(mBtn2));
			NGUITools.SetActive(mBtn3.gameObject, mBtnlist.Contains(mBtn3));
			NGUITools.SetActive(mBtn4.gameObject, mBtnlist.Contains(mBtn4));
		}
		SetBtnPos(mBtnlist);
	}

	void SetBtnPos(List<UIButton> mBtnlist)
	{
		if(mBtnlist != null && mBtnlist.Count > 1)
		{
			if(mBtnlist.Count == 2)SetBtnPosTwo(mBtnlist);
			else if(mBtnlist.Count == 3)SetBtnPosThree(mBtnlist);
			else if(mBtnlist.Count == 4)SetBtnPosFour(mBtnlist);
		}
	}

	void SetBtnPosTwo(List<UIButton> mBtnlist)
	{
		mBtnlist[0].gameObject.transform.localPosition = m_Mgr.m_TwoBtn[0];
		mBtnlist[1].gameObject.transform.localPosition = m_Mgr.m_TwoBtn[1];
	}

	void SetBtnPosThree(List<UIButton> mBtnlist)
	{
		mBtnlist[0].gameObject.transform.localPosition = m_Mgr.m_ThreeBtn[0];
		mBtnlist[1].gameObject.transform.localPosition = m_Mgr.m_ThreeBtn[1];
		mBtnlist[2].gameObject.transform.localPosition = m_Mgr.m_ThreeBtn[2];
	}

	void SetBtnPosFour(List<UIButton> mBtnlist)
	{
		mBtnlist[0].gameObject.transform.localPosition = m_Mgr.m_FourBtn[0];
		mBtnlist[1].gameObject.transform.localPosition = m_Mgr.m_FourBtn[1];
		mBtnlist[2].gameObject.transform.localPosition = m_Mgr.m_FourBtn[2];
		mBtnlist[3].gameObject.transform.localPosition = m_Mgr.m_FourBtn[3];
	}

	void SetBtnStatus()
	{
		List<ShopItemType> mList = m_Mgr.m_CurShopTaglist;
		if(mList.Count == 0 || mList.Count == 1)
		{
			mBtn1.gameObject.SetActive(false);
			mBtn2.gameObject.SetActive(false);
			mBtn3.gameObject.SetActive(false);
			mBtn4.gameObject.SetActive(false);
			m_Pic.transform.localPosition = new Vector3(255f,0f,0f);
		}
		else
		{
			m_Pic.transform.localPosition = new Vector3(0f,0f,0f);
			mBtn1.Text = Core.Data.stringManager.getString(25113);
			mBtn2.Text = Core.Data.stringManager.getString(25114);
			mBtn3.Text = Core.Data.stringManager.getString(25115);
			mBtn4.Text = Core.Data.stringManager.getString(25168);
			List<UIButton> _Btnlist = new List<UIButton>();
			foreach(ShopItemType type in mList)
			{
				_Btnlist.Add(CheckShopTypeBtn(type));
			}
			HideBtn(_Btnlist);

			InitOther(UIDragonMallMgr.GetInstance().mShopItemType);

		}
	}



	public void InitOther(ShopItemType mType )
	{
		ClearBtnSprite();
		if(mType == ShopItemType.HotSale)
		{
			mSpriteR[0].gameObject.SetActive(true);
			mSpriteL[0].gameObject.SetActive(false);

		}
		else if(mType == ShopItemType.Egg)
		{
			mSpriteR[1].gameObject.SetActive(true);
			mSpriteL[1].gameObject.SetActive(false);
      
		}
		else if(mType == ShopItemType.Item)
		{
			mSpriteR[2].gameObject.SetActive(true);
			mSpriteL[2].gameObject.SetActive(false);
       
		}
		else if(mType == ShopItemType.Active)
		{
			mSpriteR[3].gameObject.SetActive(true);
			mSpriteL[3].gameObject.SetActive(false);
    
		}
	}


    void ClearBtnSprite()
    {
        mSpriteL[0].gameObject.SetActive(true);
        mSpriteL[1].gameObject.SetActive(true);
        mSpriteL[2].gameObject.SetActive(true);
        mSpriteL[3].gameObject.SetActive(true);

        mSpriteR[0].gameObject.SetActive(false);
        mSpriteR[1].gameObject.SetActive(false);
        mSpriteR[2].gameObject.SetActive(false);
        mSpriteR[3].gameObject.SetActive(false);


    }

    void SetBtnContent(ShopItemType mType)
    {
		UIDragonMallMgr.GetInstance().mDragonMallScrollViewControl.deleteBg();
		UIDragonMallMgr.GetInstance().mDragonMallScrollViewControl.ReFresh(mType);
		ClearBtnSprite();
		if(mType == ShopItemType.HotSale)
		{
			mSpriteR[0].gameObject.SetActive(true);
			mSpriteL[0].gameObject.SetActive(false);
		}
		else if(mType == ShopItemType.Egg)
		{
			mSpriteR[1].gameObject.SetActive(true);
			mSpriteL[1].gameObject.SetActive(false);
		}
		else if(mType == ShopItemType.Item)
		{
			mSpriteR[2].gameObject.SetActive(true);
			mSpriteL[2].gameObject.SetActive(false);
		}
		else if(mType == ShopItemType.Active)
		{
			mSpriteR[3].gameObject.SetActive(true);
			mSpriteL[3].gameObject.SetActive(false);
		}

    }

    void testfunction()
    {
        DBUIController.mDBUIInstance.SetViewState(RUIType.EMViewState.S_Bag, RUIType.EMBoxType.LOOK_Charator);
    }
//UIBottun

    public void Back_OnClick()
    {
		UIDragonMallMgr.GetInstance().mDragonMallScrollViewControl.ResetPos();

        
		            
        if (UIDragonMallMgr.GetInstance().CallBackWhenExit != null)
        {				
			if(UIDragonMallMgr.GetInstance().isShow2DCamera)
			{
				DBUIController.mDBUIInstance.ShowFor2D_UI(false);
				UIDragonMallMgr.GetInstance().isShow2DCamera = false;
			}
            UIDragonMallMgr.GetInstance().CallBackWhenExit();
            UIDragonMallMgr.GetInstance().CallBackWhenExit = null;
        }
        else
        {
            if (UICityFloorManager.Instance != null && UICityFloorManager.Instance.gameObject.activeInHierarchy == true)
            {
                DBUIController.mDBUIInstance.HiddenFor3D_UI();
                UIMiniPlayerController.Instance.HideFunc();
			}else if(DuoBaoPanelScript.Instance != null && DuoBaoPanelScript.Instance.gameObject.activeInHierarchy == true)
            {
               
                DBUIController.mDBUIInstance.HiddenFor3D_UI();
                UIMiniPlayerController.Instance.HideFunc();
            }
			else if (SQYChapterController.Instance!= null && SQYChapterController.Instance.gameObject.activeInHierarchy == true)
            {
             
                DBUIController.mDBUIInstance.HiddenFor3D_UI();
                UIMiniPlayerController.Instance.HideFunc();
            }else if(UIMessageMain.Instance != null && UIMessageMain.Instance.gameObject.activeInHierarchy == true)
            {
             
                DBUIController.mDBUIInstance.HiddenFor3D_UI();
                UIMiniPlayerController.Instance.HideFunc();
			}
            else
            {
//                if (SQYTeamInfoView.mInstance != null)
//                {
//                    if (SQYTeamInfoView.mInstance.go_background != null)
//                    {
//                        Debug.Log(" goback   null ");
//                        DBUIController.mDBUIInstance.HiddenFor3D_UI();
//                        UIMiniPlayerController.Instance.SetActive(false);
//                    }
//                    else
//                    {
//                        Debug.Log(" both  null ");
//                        DBUIController.mDBUIInstance.ShowFor2D_UI(false);
//                    }
//                }
//                else
//                {
                    DBUIController.mDBUIInstance.ShowFor2D_UI(false);
//                }
            }
        }
           
		Destroy(gameObject);
    }

	void VipRequest()
	{
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.VIPSHOPINFO, new GetVipShopParam(int.Parse(Core.Data.playerManager.PlayerID)));
		
		task.afterCompleted = getDateCompleted;
		task.ErrorOccured = getDateError;
		
		task.DispatchToRealHandler ();
	}

	void getDateCompleted(BaseHttpRequest request, BaseResponse response)
	{
		if(response != null && response.status != BaseResponse.ERROR)
		{
			GetVipShopInfoResponse res = response as GetVipShopInfoResponse;
			UIDragonMallMgr.GetInstance()._vipstatus = res.data.vipstatus;
			SetBtnContent(ShopItemType.Vip);
			SetVipDetail();
			ComLoading.Close();
		}

	}

	void SetVipDetail()
	{
		int viplevel = Core.Data.playerManager.curVipLv;
		VipInfoData _datanext;
		VipInfoData _datacur;
		_datanext = Core.Data.vipManager.GetVipInfoData(viplevel+1);
		_datacur = Core.Data.vipManager.GetVipInfoData(viplevel);
		if(_datanext == null && _datacur == null)
		{
			mMoney.SafeText("");
			mExc.SafeText("-/-");
		}
		else if(_datanext == null && _datacur != null)
		{
			mMoney.SafeText("");
			mExc.SafeText((_datacur.rmb *10).ToString() +  "/-");  // *10  =钻石了
		}
		else if(_datanext != null && _datacur == null)
		{
			RED.LogError(viplevel.ToString() + "VIP's vipinfo is null");
		}
		else
		{
			mMoney.SafeText((_datanext.rmb*10 - _datacur.rmb*10).ToString());
			mExc.SafeText((_datacur.rmb*10).ToString() + "/" + (_datanext.rmb*10).ToString());
			
			
			mSlider.value = (float)_datacur.rmb/ (float)_datanext.rmb;
		}

		mVipTitle1.text = Core.Data.stringManager.getString(25119);
		if(viplevel == 0)mVipTitle2.SafeText(string.Format(Core.Data.stringManager.getString(25120),"8"));
		if(viplevel < 3 && viplevel > 0)mVipTitle2.SafeText(string.Format(Core.Data.stringManager.getString(25120), ((viplevel+1)*10).ToString()));
		else
		{
			mVipTitle2.SafeText(Core.Data.stringManager.getString(25121));
		}
		if(viplevel+1 > 13)mVipNext.SafeText("-");
		else
		{
			mVipNext.SafeText((viplevel+1).ToString());
		}
		mVipCur.SafeText(viplevel.ToString());
		
	}

    void getDateError(BaseHttpRequest request, string error)
	{
		ComLoading.Close();
		ConsoleEx.DebugLog("Error = " + error);
	}
	
    public void Btn1_OnClick()
    {
        SetBtnContent(ShopItemType.HotSale);
		UIDragonMallMgr.GetInstance().mShopItemType = ShopItemType.HotSale;
    }

    public void Btn2_OnClick()
    {
        SetBtnContent(ShopItemType.Egg);
		UIDragonMallMgr.GetInstance().mShopItemType = ShopItemType.Egg;
    }

    public void Btn3_OnClick()
    {
        SetBtnContent(ShopItemType.Item);
		UIDragonMallMgr.GetInstance().mShopItemType = ShopItemType.Item;
    }

    public void Btn4_OnClick()
    {
		SetBtnContent(ShopItemType.Active);
		UIDragonMallMgr.GetInstance().mShopItemType = ShopItemType.Active;
    }

    public void Bag_OnClick()
    {
        DBUIController.mDBUIInstance.SetViewState(RUIType.EMViewState.S_Bag, RUIType.EMBoxType.LOOK_Props);

		UIDragonMallMgr.GetInstance().mDragonMallScrollViewControl.mScrollView.ResetPosition();

        gameObject.SetActive(false);

        enterFromMall = true;
    }


    public void Recharge_OnClick()
    {
		UIDragonMallMgr.GetInstance().SetRechargeMainPanelActive();
		Core.Data.rechargeDataMgr._RechargeCallback = RechargeFunc;
//        gameObject.SetActive(false);
    }

	void RechargeFunc()
	{
		UIDragonMallMgr.GetInstance().OpenUINew(UIDragonMallMgr.GetInstance().mShopItemType);
		UIMiniPlayerController.Instance.SetActive(true);
	}
}
