using UnityEngine;
using System.Collections;
using System.Text;

public class UIDragonMallCell : RUIMonoBehaviour {

    public UISprite mItemIcon ;
    public UISprite[] mStars;
    public UILabel mItemName;
    public UILabel mPrice;
    public UILabel mSalePrice;
    public UILabel mIconContent;
    public UISprite mIsSaleIcon;
	public UISprite mSaleBgIcon;

    public UISprite mMoneyIcon;
    public UISprite mSaleMoneyIcon;
	public UISprite mSaleSoulIcon;
    public UISprite mMoneyIconBG;
    public UISprite mSaleLineIcon;
	public UISprite mCoinIcon;
	public UISprite mSaleCoinIcon;
	public UISprite mBg;
    public UILabel uiLab ;
	[HideInInspector]
	public GameObject ob = null;
    
	Vector3 m1 = new Vector3(-14f,-52f,0f);
    float mm = 9.5f;

    ItemData mdata;

	ActiveShopItem mActivedata;

	ShopItemType _type;

	int _ItemNum = 0;
//	int mNeedMoney = 0;
    
	public void OnShow(ItemData data, ShopItemType mType)
    {
		_ItemNum = 1;
        mdata = data;
		_type = mType;
        int mCount = 0;

        uiLab.text = data.description;

        while(mCount < data.star)
        {
            if(mStars[mCount] != null)mStars[mCount].gameObject.SetActive(true);
            mStars[mCount].transform.localPosition = new Vector3(m1.x+(mCount+1)*2*mm,m1.y,m1.z);
            mCount++;
        }
        for(int j=0; j<mCount; j++)
        {
            mStars[j].transform.localPosition = new Vector3(mStars[j].transform.localPosition.x-(mCount+1)*mm,m1.y,m1.z); ;
        }
        for(;mCount<5; mCount++)
        {
            if(mStars[mCount] != null)mStars[mCount].gameObject.SetActive(false);
        }

		if(data.num != null)
		{
			for(int i=0; i<data.num.Count; i++)
			{
				
				if(data.num[i].Length < 4)continue;
				_ItemNum += data.num[i][3];
			}
		}


        if(mItemName != null)mItemName.text = data.name;
        if(mIconContent != null)mIconContent.text = data.description;
		if(mItemIcon != null)
		{
			if(data.ID == 110092)
			{
				mItemIcon.spriteName = "110026";
			}
			else 
			{
				mItemIcon.spriteName = data.iconID.ToString();
			}
		}

        if(data.discount.Length == 0)
        {
			if(mdata.price == null)
			{
				ConsoleEx.DebugLog(mdata.name);
			}
			if(mdata.price != null)
			{	
				if(mSalePrice != null && data.price.Length != 0)mSalePrice.text = data.price[1].ToString();
				if(mPrice != null)mPrice.gameObject.SetActive(false);
				if(mMoneyIcon != null)mMoneyIcon.gameObject.SetActive(false);
				if(mCoinIcon != null)mCoinIcon.gameObject.SetActive(false);
				if(mIsSaleIcon != null)mIsSaleIcon.alpha = 0f;
				if(mSaleBgIcon != null)mSaleBgIcon.alpha =0f;
				if(mMoneyIconBG != null)mMoneyIconBG.gameObject.SetActive(false);
				if(mSaleLineIcon != null)mSaleLineIcon.gameObject.SetActive(false);
				if(data.price.Length != 0)
				{
					if(data.price[0] == 0)
					{
						mSaleMoneyIcon.gameObject.SetActive(false);
						mSaleCoinIcon.gameObject.SetActive(true);
						mSaleSoulIcon.gameObject.SetActive(false);
					}
					else if(data.price[0] == 1)
					{
						mSaleMoneyIcon.gameObject.SetActive(true);
						mSaleCoinIcon.gameObject.SetActive(false);
						mSaleSoulIcon.gameObject.SetActive(false);
					}
					else
					{
						mSaleMoneyIcon.gameObject.SetActive(false);
						mSaleCoinIcon.gameObject.SetActive(false);
						mSaleSoulIcon.gameObject.SetActive(true);
					}
				}
			}

        }
        else
        {
			if(mPrice != null && data.discount.Length != 0)mPrice.text = data.discount[1].ToString();
            mPrice.gameObject.SetActive(true);
            if(mSalePrice != null)mSalePrice.text = data.price[1].ToString();
			if(mIsSaleIcon != null)mIsSaleIcon.alpha = 1f;
			if(mSaleBgIcon != null)mSaleBgIcon.alpha = 1f;
			if(data.discount[0] == 0)
			{
				if(mMoneyIcon != null)mMoneyIcon.gameObject.SetActive(false);
				if(mCoinIcon != null)mCoinIcon.gameObject.SetActive(true);
				if(mSaleMoneyIcon != null)mSaleMoneyIcon.gameObject.SetActive(false);
				if(mSaleCoinIcon != null)mSaleCoinIcon.gameObject.SetActive(true);
				if(mSaleSoulIcon != null)mSaleSoulIcon.gameObject.SetActive(false);
			}
			else if(data.discount[0] == 1)
			{
				if(mMoneyIcon != null)mMoneyIcon.gameObject.SetActive(true);
				if(mCoinIcon != null)mCoinIcon.gameObject.SetActive(false);
				if(mSaleMoneyIcon != null)mSaleMoneyIcon.gameObject.SetActive(true);
				if(mSaleCoinIcon != null)mSaleCoinIcon.gameObject.SetActive(false);
				if(mSaleSoulIcon != null)mSaleSoulIcon.gameObject.SetActive(false);
            }
			else
			{
				if(mMoneyIcon != null)mMoneyIcon.gameObject.SetActive(false);
				if(mCoinIcon != null)mCoinIcon.gameObject.SetActive(false);
				if(mSaleMoneyIcon != null)mSaleMoneyIcon.gameObject.SetActive(false);
				if(mSaleCoinIcon != null)mSaleCoinIcon.gameObject.SetActive(false);
				if(mSaleSoulIcon != null)mSaleSoulIcon.gameObject.SetActive(true);
			}
            if(mMoneyIconBG != null)mMoneyIconBG.gameObject.SetActive(true);
            if(mSaleLineIcon != null)mSaleLineIcon.gameObject.SetActive(true);
        }
        gameObject.SetActive(true);
    }

	public void OnShow(ActiveShopItem data, ShopItemType mType)
	{
		_ItemNum = 1;
		mActivedata = data;
		_type = mType;
		int mCount = 0;

		SetActiveItemBuyNum(uiLab);

		if(mType == ShopItemType.Active)
		{
			CheckCanBuy(mActivedata.yetBuyCount, mActivedata.buyCount);
		}
		
		while(mCount < data.pStar)
		{
			if(mStars[mCount] != null)mStars[mCount].gameObject.SetActive(true);
			mStars[mCount].transform.localPosition = new Vector3(m1.x+(mCount+1)*2*mm,m1.y,m1.z);
			mCount++;
		}
		for(int j=0; j<mCount; j++)
		{
			mStars[j].transform.localPosition = new Vector3(mStars[j].transform.localPosition.x-(mCount+1)*mm,m1.y,m1.z); ;
		}
		for(;mCount<5; mCount++)
		{
			if(mStars[mCount] != null)mStars[mCount].gameObject.SetActive(false);
		}

		if(mItemName != null)mItemName.text = data.pName;
		if(mIconContent != null)
		{
			;
//			string _str = data.des;
//			_str += string.Format(Core.Data.stringManager.getString(25169), data.yetBuyCount.ToString(), data.buyCount.ToString());
//			mIconContent.text = _str;
		}
		if(mItemIcon != null)
		{
			UIAtlas _atlas = UIDragonMallMgr.GetInstance().GetActiveAtlas(data.iconId);
			if(_atlas != null)
			{
				mItemIcon.atlas = _atlas;
				mItemIcon.spriteName = data.iconId;
			}
		}
		
		if(data.disPrice == 0)
		{
			if(mSalePrice != null)mSalePrice.text = data.price.ToString();
			if(mPrice != null)mPrice.gameObject.SetActive(false);
			if(mMoneyIcon != null)mMoneyIcon.gameObject.SetActive(false);
			if(mCoinIcon != null)mCoinIcon.gameObject.SetActive(false);
			if(mIsSaleIcon != null)mIsSaleIcon.alpha = 0f;
			if(mSaleBgIcon != null)mSaleBgIcon.alpha = 0f;
			if(mMoneyIconBG != null)mMoneyIconBG.gameObject.SetActive(false);
			if(mSaleLineIcon != null)mSaleLineIcon.gameObject.SetActive(false);
			if(data.mType == 0)
			{
				mSaleMoneyIcon.gameObject.SetActive(false);
				mSaleCoinIcon.gameObject.SetActive(true);
				mSaleSoulIcon.gameObject.SetActive(false);
			}
			else if(data.mType== 1)
			{
				mSaleMoneyIcon.gameObject.SetActive(true);
				mSaleCoinIcon.gameObject.SetActive(false);
				mSaleSoulIcon.gameObject.SetActive(false);
			}
			else
			{
				mSaleMoneyIcon.gameObject.SetActive(false);
				mSaleCoinIcon.gameObject.SetActive(false);
				mSaleSoulIcon.gameObject.SetActive(true);
				if(data.mType == 2)
				{
					mSaleSoulIcon.atlas = AtlasMgr.mInstance.commonAtlas;
					mSaleSoulIcon.spriteName = "jifen";
				}
				else if(data.mType == 3)
				{
					mSaleSoulIcon.atlas = AtlasMgr.mInstance.commonAtlas;
					mSaleSoulIcon.spriteName = "common-0077";
				}
				else
				{
					GetMarkAtlasAndSet(data.mType, mSaleSoulIcon);
					mSaleSoulIcon.gameObject.SetActive(true);
				}

			}
		}
		else
		{
			if(mPrice != null)mPrice.text = data.disPrice.ToString();
			mPrice.gameObject.SetActive(true);
			if(mSalePrice != null)mSalePrice.text = data.price.ToString();
			if(mIsSaleIcon != null)mIsSaleIcon.alpha = 1f;
			if(mSaleBgIcon != null)mSaleBgIcon.alpha = 1f;
			if(data.mType == 0)
			{
				if(mMoneyIcon != null)mMoneyIcon.gameObject.SetActive(false);
				if(mCoinIcon != null)mCoinIcon.gameObject.SetActive(true);
				if(mSaleMoneyIcon != null)mSaleMoneyIcon.gameObject.SetActive(false);
				if(mSaleCoinIcon != null)mSaleCoinIcon.gameObject.SetActive(true);
                if(mSaleSoulIcon != null)mSaleSoulIcon.gameObject.SetActive(false);
            }
			else if(data.mType == 1)
            {
                if(mMoneyIcon != null)mMoneyIcon.gameObject.SetActive(true);
                if(mCoinIcon != null)mCoinIcon.gameObject.SetActive(false);
                if(mSaleMoneyIcon != null)mSaleMoneyIcon.gameObject.SetActive(true);
                if(mSaleCoinIcon != null)mSaleCoinIcon.gameObject.SetActive(false);
                if(mSaleSoulIcon != null)mSaleSoulIcon.gameObject.SetActive(false);
            }
            else
            {
                if(mMoneyIcon != null)mMoneyIcon.gameObject.SetActive(false);
                if(mCoinIcon != null)mCoinIcon.gameObject.SetActive(false);
                if(mSaleMoneyIcon != null)mSaleMoneyIcon.gameObject.SetActive(false);
                if(mSaleCoinIcon != null)mSaleCoinIcon.gameObject.SetActive(false);
				mSaleSoulIcon.gameObject.SetActive(true);
				if(data.mType == 2)
				{
					mSaleSoulIcon.atlas = AtlasMgr.mInstance.commonAtlas;
					mSaleSoulIcon.spriteName = "jifen";
				}
				else if(data.mType == 3)
				{
					mSaleSoulIcon.atlas = AtlasMgr.mInstance.commonAtlas;
					mSaleSoulIcon.spriteName = "common-0077";
				}
                else
                {
                    GetMarkAtlasAndSet(data.mType, mSaleSoulIcon);
                    mSaleSoulIcon.gameObject.SetActive(true);
                }
              
            }
            if(mMoneyIconBG != null)mMoneyIconBG.gameObject.SetActive(true);
            if(mSaleLineIcon != null)mSaleLineIcon.gameObject.SetActive(true);
        }
        gameObject.SetActive(true);
    }

	void CheckCanBuy(int yetbuycount, int buycount)
	{
		if(mActivedata.buyCount >= 0)
		{
			if(yetbuycount >= buycount)
			{
				ob = NGUITools.AddChild(gameObject);
				ob.name = "Alphabg";
				
				ob.AddComponent<UISprite>();
				ob.AddComponent<UIDragScrollView>();
				UISprite _tempbg = ob.GetComponent<UISprite>();
				
				_tempbg.atlas = AtlasMgr.mInstance.commonAtlas;
				_tempbg.spriteName = "common-0062";
				_tempbg.color = new Color(1.0f,1.0f,1.0f,0.5f);
				_tempbg.height = 172;
				_tempbg.width = 338;
				_tempbg.depth = 100;
				_tempbg.type = UISprite.Type.Sliced;
				ob.transform.localPosition = mBg.transform.localPosition;
				ob.AddComponent<BoxCollider>().size = new Vector3(338,172,0);
				if(ob.GetComponent<UIDragScrollView>() == null)
				{
					ob.AddComponent<UIDragScrollView>();
				}
			}
		}

	}

	void SetActiveItemBuyNum(UILabel _label)
	{
		if(_label != null)
		{
			_label.text = "";
			StringBuilder _str = new StringBuilder();
			_str.Append(mActivedata.des);
			if(mActivedata.buyCount >= 0)
			{
				_str.Append(Core.Data.stringManager.getString(25169));
				if(mActivedata.yetBuyCount >= mActivedata.buyCount)
				{
					_str.Append("[FF0000]");
				}
				else
				{
					_str.Append("[00FF00]");
				}
				_str.Append(mActivedata.yetBuyCount.ToString());
				_str.Append("/");
				_str.Append(mActivedata.buyCount.ToString());
			}
			_label.text = _str.ToString();
		}

	}
    
    public void OnHide()
    {
        gameObject.SetActive(false);
    }
    
    void BuyItem_OnClick()
    {
        BuyRequest();
    }
    
    void OnClick()
    {
        BuyItem_OnClick();
    }
    
    
    
    void BuyRequest()
    {
        SecretShopMgr.GetInstance()._UIDragonMallCell = this;
		if(_type == ShopItemType.Active) 
		{
			SecretShopMgr.GetInstance().SetSecretShopTag(true, mActivedata, _type, 2);
		}
        else 
		{
			SecretShopMgr.GetInstance().SetSecretShopTag(true, mdata, _type, 2);
		}
    }
    
    void testHttpResp_Error(BaseHttpRequest request, string error)
    {
		ComLoading.Close ();
        ConsoleEx.DebugLog ("---- Http Resp - Error has ocurred." + error);
    }
		
	void BuyItemSuc(BaseHttpRequest request, BaseResponse response)
	{
		BuyItemResponse resp = response as BuyItemResponse;
		if(resp.data == null)
		{
			return;
		}

		int propid = 0;
		ItemOfReward[] rewards = null;
		HttpRequest req = request as HttpRequest;
		if(_type == ShopItemType.Active)
		{
			BuyActiveShopItemParam param = req.ParamMem as BuyActiveShopItemParam;
			propid = param.propid;
		}
		else
		{
			PurchaseParam param = req.ParamMem as PurchaseParam;
			propid = param.propid;
        }
            
        if (resp.data.Result != null)
		{
			if(resp.data.Result.coin > 0)
			{
				string strText = Core.Data.stringManager.getString(5214);
				strText = string.Format(strText, resp.data.Result.coin, Core.Data.stringManager.getString(5037));
				SQYAlertViewMove.CreateAlertViewMove(strText);
				DBUIController.mDBUIInstance.RefreshUserInfo ();
			}
			else if(resp.data.Result.stone > 0)
			{
				string strText = Core.Data.stringManager.getString(5214);
				strText = string.Format(strText, resp.data.Result.stone, Core.Data.stringManager.getString(5070));
				SQYAlertViewMove.CreateAlertViewMove(strText);
				DBUIController.mDBUIInstance.RefreshUserInfo ();
			}
			else if(resp.data.Result.eny > 0)
			{
				string strText = Core.Data.stringManager.getString(5214);
				strText = string.Format(strText, resp.data.Result.eny, Core.Data.stringManager.getString(5038));
				SQYAlertViewMove.CreateAlertViewMove(strText);
				DBUIController.mDBUIInstance.RefreshUserInfo ();
			}
			else if(resp.data.Result.pwr > 0)
			{
				string strText = Core.Data.stringManager.getString(5214);
				strText = string.Format(strText, resp.data.Result.pwr, Core.Data.stringManager.getString(5039));
				SQYAlertViewMove.CreateAlertViewMove(strText);
				DBUIController.mDBUIInstance.RefreshUserInfo ();
			}

			if (resp.data.Result.p != null)
			{
				rewards = resp.data.Result.p;
				ShowRewards (propid, rewards);
			}

		}
		else
		{
			if(UIDragonMallMgr.GetInstance().CheckOpenWindowList.Contains(propid))
			{
				SQYAlertViewMove.CreateAlertViewMove(string.Format(Core.Data.stringManager.getString(5214), Core.Data.itemManager.getItemData(propid).name,""));
			}
			else 
			{
				GetRewardSucUI.OpenUI(resp.data.p, Core.Data.stringManager.getString (5097));
			}

			DBUIController.mDBUIInstance.RefreshUserInfo ();
		}


		SQYMainController.mInstance.UpdateBagTip ();
	}


	void ShowRewards(int srcID, ItemOfReward[] rewards)
	{
		if (rewards != null)
		{
			if (rewards.Length == 0)
			{
				RED.LogWarning ("get nothing!");
				return;
			}
			if (srcID == 110026)
			{
				ComLoading.Close();
				CradSystemFx.GetInstance ().SetCardPanel (rewards,3150,  true, true, false);
				UIMiniPlayerController.Instance.SetActive(false);
			}
			else if((srcID < 110026 && srcID >= 110024) || srcID == 110092)
			{
				CradSystemFx.GetInstance ().SetCardSinglePanel (rewards, 350, true, true, false);
				UIMiniPlayerController.Instance.SetActive(false);
			}
			else
			{
				if(UIDragonMallMgr.GetInstance().CheckOpenWindowList.Contains(srcID))
				{
					SQYAlertViewMove.CreateAlertViewMove(string.Format(Core.Data.stringManager.getString(5214), Core.Data.itemManager.getItemData(srcID).name,""));
				}
				else
				{
					GetRewardSucUI.OpenUI (rewards, Core.Data.stringManager.getString (5097));
                }

				DBUIController.mDBUIInstance.RefreshUserInfo ();
			}
		}
	}

   public void testHttpResp_UI(BaseHttpRequest request, BaseResponse response)
    {
		ComLoading.Close ();
		if(response != null && response.status != BaseResponse.ERROR)
		{
			BuyItemSuc (request, response);

			if(_type == ShopItemType.Active)
            {
				if(mActivedata != null)
				{
					mActivedata.yetBuyCount += 1;
					SetActiveItemBuyNum(uiLab);
				}

				CheckCanBuy(mActivedata.yetBuyCount, mActivedata.buyCount);
			}

		}
		else if(response != null && response.status == BaseResponse.ERROR)
		{
			if(response.errorCode == 30000)
			{
				SQYAlertViewMove.CreateAlertViewMove(string.Format(Core.Data.stringManager.getNetworkErrorString(response.errorCode), Core.Data.itemManager.GetBagItemCount(110084)));
			}
			else
			{
				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
			}

		}
    }

	void GetMarkAtlasAndSet(int m_pid, UISprite mItemIcon)
	{
		ConfigDataType datatype = DataCore.getDataType(m_pid);
		if (datatype == ConfigDataType.Monster)
		{
			AtlasMgr.mInstance.SetHeadSprite (mItemIcon, m_pid.ToString ());
			return;
		}
		else if (datatype == ConfigDataType.Equip)
		{
			mItemIcon.atlas = AtlasMgr.mInstance.equipAtlas;
		}
		else if (datatype == ConfigDataType.Gems)
		{
			mItemIcon.atlas = AtlasMgr.mInstance.commonAtlas;
		}
		else if (datatype == ConfigDataType.Item)
		{
			mItemIcon.atlas = AtlasMgr.mInstance.itemAtlas; 
		}
		else if(datatype == ConfigDataType.Frag)
		{
			
			SoulInfo info = new SoulInfo (0, m_pid, 1);
			Soul soul = new Soul ();
			soul.m_config = Core.Data.soulManager.GetSoulConfigByNum (info.num);
			soul.m_RTData = info;
			if (soul.m_config.type == (int)ItemType.Monster_Frage)
			{
				MonsterData mon = Core.Data.monManager.getMonsterByNum (soul.m_config.updateId);
				if(mon != null)
				{
					AtlasMgr.mInstance.SetHeadSprite (mItemIcon, mon.ID.ToString ());
				}
			}
			else if (soul.m_config.type == (int)ItemType.Equip_Frage)
			{
				EquipData equip = Core.Data.EquipManager.getEquipConfig (soul.m_config.updateId);
				if (equip != null)
				{
					mItemIcon.atlas = AtlasMgr.mInstance.equipAtlas;
					mItemIcon.spriteName = soul.m_config.updateId.ToString ();
				}
			}
			else  
			{
				mItemIcon.atlas = AtlasMgr.mInstance.itemAtlas;
				mItemIcon.spriteName = soul.m_RTData.num.ToString ();
			}
			return;
		}
		ItemData _data = Core.Data.itemManager.getItemData(m_pid);
		if(_data != null)mItemIcon.spriteName = _data.iconID.ToString();
	}

	public void DestorySelf()
	{
		Destroy(gameObject);
	}


}
