using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIDragonMallMgr  
{
    static UIDragonMallMgr mUIDragonMallMgr;
    public static UIDragonMallMgr GetInstance()
    {
        if(mUIDragonMallMgr == null)
        {
            mUIDragonMallMgr = new UIDragonMallMgr();
        }
        return mUIDragonMallMgr;
    }

    private UIDragonMallMgr()
    {
		CheckOpenWindowList = new List<int>{110029,110039,110041,110044,110046,110091,110061,110064,110049,110050};
		m_TwoBtn = new List<Vector3>{new Vector3(128f,206f,0f), new Vector3(316f,206f,0f)};
		m_ThreeBtn = new List<Vector3>{new Vector3(66f,206f,0f), new Vector3(213f,206f,0f), new Vector3(361f,206f,0f)};
		m_FourBtn = new List<Vector3>{new Vector3(38f,206f,0f), new Vector3(155f,206f,0f), new Vector3(274f,206f,0f), new Vector3(392f,206f,0f)};
		m_CurShopTaglist = new List<ShopItemType>();
		if(LuaTest.Instance != null)
		{
			if(LuaTest.Instance.HotStore)m_CurShopTaglist.Add(ShopItemType.HotSale);
			if(LuaTest.Instance.ExpStore)m_CurShopTaglist.Add(ShopItemType.Egg);
			if(LuaTest.Instance.ItemStore)m_CurShopTaglist.Add(ShopItemType.Item);
		}
    }

	private RechargeDataMgr _rechargemgr{
		get{
			return Core.Data.rechargeDataMgr;
		}
	}

	public List<Vector3> m_TwoBtn;
	public List<Vector3> m_ThreeBtn;
	public List<Vector3> m_FourBtn;

	public VipShopInfo[] _vipstatus;
	public List<int> CheckOpenWindowList;

	public List<ShopItemType> m_CurShopTaglist;

	#region Add by jc
	/*关闭商城的时候使用回调
	 * */
	public System.Action CallBackWhenExit = null;
	public bool isShow2DCamera {get;set;}
	#endregion
	
	public ShopItemType mShopItemType;

    DragonMallVipPanelScript _DragonMallVipPanelScript = null;
    UIDragonVipTequan _UIDragonVipTequan = null;
    UIDragonVipLibao _UIDragonVipLibao = null;
    UIDragonVipRecharge _UIDragonVipRecharge = null;
    public UIDragonRechargeMain _UIDragonRechargeMain = null;
	public DragonMallScrollViewControl mDragonMallScrollViewControl = null;
	
    public PayCountData payData =null;

	public Dictionary<int, VipShopInfo> m_VipStatusDic = new Dictionary<int, VipShopInfo>();

	public List<ActiveShopItem> m_ActiveShopList = new List<ActiveShopItem>();

	float m_VipExcPercent;
	public float GetExcPercent{
		get{
			return m_VipExcPercent;
		}
	}

	public int m_CurLevelMoney;
	public int m_TotalMoney;

	public bool GetIsVipGift{
		get{
			if(Core.Data.vipManager.IsFirstLogin_Vip)
			{
				VipRequest();
				return false;
			}
			else
			{
				if(Core.Data.playerManager.curVipLv - m_VipStatusDic.Count > 0)return true;
				else return false;
            }
		}
	}

    DragonMallVipPanelScript mDragonMallVipPanelScript    
    {
        get{
            if(_DragonMallVipPanelScript == null)
            {
                _DragonMallVipPanelScript = DragonMallVipPanelScript.CreateShangChengPanel();
                RED.AddChild(_DragonMallVipPanelScript.gameObject,DBUIController.mDBUIInstance._TopRoot);
            }
            return _DragonMallVipPanelScript;
        }
    }

    UIDragonVipTequan mUIDragonVipTequan    
    {
        get{
            if(_UIDragonVipTequan == null)
            {
                _UIDragonVipTequan = DragonMallVipTequanPanelScript.CreateShangChengPanel();
                RED.AddChild(_UIDragonVipTequan.gameObject,DBUIController.mDBUIInstance._TopRoot);
            }
            return _UIDragonVipTequan;
        }
    }

    UIDragonVipLibao mUIDragonVipLibao
    {
        get{
            if(_UIDragonVipLibao == null)
            {
                _UIDragonVipLibao = DragonMallVipLibaoPanelScript.CreateShangChengPanel();
                RED.AddChild(_UIDragonVipLibao.gameObject,DBUIController.mDBUIInstance._TopRoot);
            }
            return _UIDragonVipLibao;
        }
    }

    UIDragonVipRecharge mUIDragonVipRecharge
    {
        get{
            if(_UIDragonVipRecharge == null)
            {
                _UIDragonVipRecharge = DragonMallVipRechargePanelScript.CreateShangChengPanel();
                RED.AddChild(_UIDragonVipRecharge.gameObject,DBUIController.mDBUIInstance._TopRoot);
            }
            return _UIDragonVipRecharge;
        }
    }


	public UIDragonRechargeMain mUIDragonRechargeMain
    {
        get{
            if(_UIDragonRechargeMain == null)
            {
                _UIDragonRechargeMain = DragonMallVipRechargeMainPanelScript.CreateShangChengPanel();
                RED.AddChild(_UIDragonRechargeMain.gameObject,DBUIController.mDBUIInstance._TopRoot);
            }
            return _UIDragonRechargeMain;
        }
    }

	public UIAtlas GetActiveAtlas(string m_Icon)
	{
		AtlasMgr _mgr = AtlasMgr.mInstance;
		if(_mgr.equipAtlas.GetSprite(m_Icon) != null)return _mgr.equipAtlas;
		else if(_mgr.commonAtlas.GetSprite(m_Icon) != null)return _mgr.commonAtlas;
		else if(_mgr.headAtlas[0].GetSprite(m_Icon) != null)return _mgr.headAtlas[0];
		else if(_mgr.headAtlas[1].GetSprite(m_Icon) != null)return _mgr.headAtlas[1];
		else if(_mgr.itemAtlas.GetSprite(m_Icon) != null)return _mgr.itemAtlas;
		else if(_mgr.skillAtlas.GetSprite(m_Icon) != null)return _mgr.skillAtlas;
		return null;
	}

	public void SeViptPercent(int mToNextLevel, int total)
	{
		int CurVip = Core.Data.playerManager.curVipLv;
		Debug.Log(mToNextLevel.ToString() + "=-------------");
		VipInfoData _datacur;
		VipInfoData _datanext;
		_datacur = Core.Data.vipManager.GetVipInfoData(CurVip);
		_datanext = Core.Data.vipManager.GetVipInfoData(CurVip+1);
		if(_datacur != null)
		{
			if(_datanext != null)
			{
				m_CurLevelMoney = mToNextLevel;
				m_TotalMoney = total;
				int valueInner = _datanext.rmb - _datacur.rmb;
				float factor   = 1.0f * (valueInner - mToNextLevel);
				if(factor <= 0) factor = 0f;
				m_VipExcPercent = factor / valueInner;
			}
			else
			{
				m_CurLevelMoney = 0;
				m_TotalMoney = total;
				m_VipExcPercent = 100f;
			}
		}
	}


	public void OpenUI(ShopItemType type , System.Action callback = null,bool isShow2D = false)
	{
		if(LuaTest.Instance != null && !LuaTest.Instance.ConvenientBuy)
		{
			return;
		}
		if(!m_CurShopTaglist.Contains(type))
		{
			callback();
			return;
		}
		CallBackWhenExit += callback;
		this.isShow2DCamera = isShow2D;
		this.mShopItemType = type;
		ActiveShopRequest();
	}

	public void OpenUINew(ShopItemType type , System.Action callback = null,bool isShow2D = false)
	{
		CallBackWhenExit += callback;
		this.isShow2DCamera = isShow2D;
		if(DBUIController.mDBUIInstance._UIDragonMallMain != null)mShopItemType = type;
		else
		{
			if(m_CurShopTaglist.Count != null && m_CurShopTaglist.Count > 0)this.mShopItemType = m_CurShopTaglist[0];
			else this.mShopItemType = ShopItemType.Active;
        }

		ActiveShopRequest();
	}

    void OpenUIFromType()
	{
		if(this.mShopItemType == ShopItemType.HotSale)
		{
			DBUIController.mDBUIInstance.mUIDragonMallMain.Btn1_OnClick();
		}
		else if(this.mShopItemType == ShopItemType.Egg)
		{
			DBUIController.mDBUIInstance.mUIDragonMallMain.Btn2_OnClick();
		}
		else if(this.mShopItemType == ShopItemType.Item)
		{
			DBUIController.mDBUIInstance.mUIDragonMallMain.Btn3_OnClick();
		}
		else if(this.mShopItemType == ShopItemType.Vip)
		{
			DBUIController.mDBUIInstance.mUIDragonMallMain.Btn4_OnClick();
		}
	}

    public void SetPanelActive()
    {
        mUIDragonVipTequan.gameObject.SetActive(true);
    }

    public void SetLibaoPanelActive()
    {
        mUIDragonVipLibao.gameObject.SetActive(true);
    }

    public void SetRechargePanelActive()
    {
        mUIDragonVipRecharge.gameObject.SetActive(true);
    }

	public void SetRechargeMainPanelActive()
    {
		Core.Data.rechargeDataMgr.SendHttpRequest();

    }

	public void SetVipLibao()
	{
		if(!Core.Data.vipManager.IsFirstLogin_Vip)
		{
			if(m_VipStatusDic != null)
			{
				mUIDragonVipTequan.gameObject.SetActive(true);
				mUIDragonVipTequan.SetDetail();
            }
		}
	}

	void ActiveShopRequest()
	{
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.ACTIVESHOPITEM, new GetActiveShopItemParam(int.Parse(Core.Data.playerManager.PlayerID)));
		
		task.afterCompleted = getActiveDateCompleted;
		task.ErrorOccured = getActiveDateError;
		
		task.DispatchToRealHandler ();
		ComLoading.Open();
	}

	void getActiveDateCompleted(BaseHttpRequest request, BaseResponse response)
	{
		if(response != null && response.status != BaseResponse.ERROR)
		{
			ActiveShopItemResponse res = response as ActiveShopItemResponse;
			if(res != null)
			{
				m_ActiveShopList = res.data.shopItems;
				if(m_ActiveShopList.Count > 0 && !m_CurShopTaglist.Contains(ShopItemType.Active))m_CurShopTaglist.Add(ShopItemType.Active);
			}
		}

		ComLoading.Close();

		if(DBUIController.mDBUIInstance._UIDragonMallMain != null)
		{
			OpenUIFromType();
		}
		DBUIController.mDBUIInstance.mUIDragonMallMain.SetActive(true);
		DBUIController.mDBUIInstance.HiddenFor3D_UI();
	}

	void getActiveDateError(BaseHttpRequest request, string errbuilor)
	{
		ComLoading.Close();
		SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(7365));
		this.CallBackWhenExit = null;
	}

	public void VipRequest()
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
			m_VipStatusDic.Clear();
			if(res.data != null && res.data.vipstatus != null)
			{
				_vipstatus = res.data.vipstatus;
				for(int i=0; i<res.data.vipstatus.Length; i++)
				{
					if(!m_VipStatusDic.ContainsKey(res.data.vipstatus[i].vipLvl))
					{
						m_VipStatusDic.Add(res.data.vipstatus[i].vipLvl, res.data.vipstatus[i]);
					}
				}
				Core.Data.vipManager.IsFirstLogin_Vip = false;
				
				if (UIDragonMallMgr.GetInstance ().GetIsVipGift == true) {
					Core.Data.ActivityManager.SetActState (ActivityManager.vipLibaoType, "1");
                } else {
                    Core.Data.ActivityManager.SetActState (ActivityManager.vipLibaoType, "2");
                }
			}

            //			ComLoading.Close();
		}
		
	}

	void getDateError(BaseHttpRequest request, string error)
	{
		ComLoading.Close();
		VipRequest();
		ConsoleEx.DebugLog("Error = " + error);
	}



}
