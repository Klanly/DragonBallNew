using UnityEngine;
using System.Collections;

public class UISecretShopTag : RUIMonoBehaviour 
{
	public GameObject mTweenScale;
	public UILabel mName;
	public UILabel mDescription;
	public UILabel mMoney;

	public GameObject SingleType;
	public GameObject MultipleType;

	#region Add by jc
	public JCBuyNumberWidget BuyNumber;
	#endregion
	

	string colorString = "[FFF300]";
	
	//1 secretshop 2 normal
	public int Shop_Index = 0;

	ItemData mdata;
	ShopItemType _type;

	ActiveShopItem ActiveItemdata;

	public void SetDetail(string name, string dec, int[] money, SecretShopDataStruct m_data, int index)
	{
		SingleType.gameObject.SetActive(true);
		MultipleType.gameObject.SetActive(false);
		Shop_Index = index;
//		this._data = m_data;

		SetTagMoneyName(money[0] , money[1] , name, mMoney, true);
//		if(money[0] == 0)	mMoney.text = string.Format(Core.Data.stringManager.getString(20091),colorString+money[1].ToString()+"[-]", Core.Data.stringManager.getString(5070), name);
//		else if(money[0] == 1) mMoney.text = string.Format(Core.Data.stringManager.getString(20091), colorString + money[1].ToString() +"[-]", Core.Data.stringManager.getString(5037), name);
		
		mDescription.text = dec;
		
		mName.text = string.Format(Core.Data.stringManager.getString(20092), name);
	}

	public void SetNormalDetail(ItemData data, ShopItemType _type, int index)
	{
		TweenScale.Begin<TweenScale>(mTweenScale, 0.3f);
		Shop_Index = index;
		this.mdata = data;
		this._type = _type;

		if(this.mdata.max == 0)
		{
			SetSingleType();
		}
		else
        {
            SetMultipleType(this.mdata.max);
		}
	}

	public void SetActiveDetail(ActiveShopItem data, ShopItemType _type, int index)
	{
		Shop_Index = index;
		this.ActiveItemdata = data;
		this._type = _type;

		SingleType.gameObject.SetActive(true);
		MultipleType.gameObject.SetActive(false);
		
		if(data.disPrice == 0)
		{
			SetTagMoneyName(data.mType, data.price, data.pName, mMoney);
//			if(data.mType == 0)	mMoney.text = string.Format(Core.Data.stringManager.getString(20091), colorString +data.price.ToString()+"[-]", Core.Data.stringManager.getString(5037), data.pName);
//			else if(data.mType == 1) mMoney.text = string.Format(Core.Data.stringManager.getString(20091), colorString + data.price.ToString() + "[-]", Core.Data.stringManager.getString(5070),data.pName);
//			else mMoney.text = string.Format(Core.Data.stringManager.getString(20091), colorString + data.price.ToString() + "[-]", Core.Data.stringManager.getString(5166),data.pName);
		}
		else
		{
			SetTagMoneyName(data.mType, data.disPrice, data.pName, mMoney);
//			if(data.mType == 0)	mMoney.text = string.Format(Core.Data.stringManager.getString(20091),colorString+ data.disPrice.ToString()+"[-]", Core.Data.stringManager.getString(5037),data.pName);
//			else if(data.mType == 1) mMoney.text = string.Format(Core.Data.stringManager.getString(20091), colorString + data.disPrice.ToString()+"[-]", Core.Data.stringManager.getString(5070),data.pName);
//			else mMoney.text = string.Format(Core.Data.stringManager.getString(20091), colorString + mdata.discount[1].ToString()+"[-]", Core.Data.stringManager.getString(5166),data.pName);
		}
		
		
		mDescription.text = data.des;
        
		mName.text = string.Format(Core.Data.stringManager.getString(20092), data.pName);

    }

	void SetMultipleType(int m_Max)
	{
		SingleType.gameObject.SetActive(false);
		MultipleType.gameObject.SetActive(true);

		if(mdata.discount.Length == 0)
		{
			if(mdata.price[0] == 0)	
			{
				BuyNumber._Name.text = string.Format(Core.Data.stringManager.getString(20092), mdata.name);
				BuyNumber._Icon.spriteName = "common-0013";
			}
			else
			{
				BuyNumber._Name.text = string.Format(Core.Data.stringManager.getString(20092),mdata.name);
				BuyNumber._Icon.spriteName = "common-0014";
			}
			BuyNumber._unitprice = mdata.price[1];
		}
		else
		{
			if(mdata.discount[0] == 0)
			{
				BuyNumber._Name.text = string.Format(Core.Data.stringManager.getString(20092), mdata.name);
				BuyNumber._Icon.spriteName = "common-0013";
			}
			else
			{
				BuyNumber._Name.text = string.Format(Core.Data.stringManager.getString(20092), mdata.name);
				BuyNumber._Icon.spriteName = "common-0014";
			}
			BuyNumber._unitprice = mdata.discount[1];
		}
		
//		BuyNumber._Name.text = BuyNumber._unitprice.ToString();
		BuyNumber.Maxnum = m_Max;
	}

	void SetSingleType()
	{
		SingleType.gameObject.SetActive(true);
		MultipleType.gameObject.SetActive(false);

		if(mdata.discount.Length == 0)
		{
			SetTagMoneyName(mdata.price[0], mdata.price[1], mdata.name, mMoney);
//			if(mdata.price[0] == 0)	mMoney.text = string.Format(Core.Data.stringManager.getString(20091), colorString +mdata.price[1].ToString()+"[-]", Core.Data.stringManager.getString(5037), mdata.name);
//			else if(mdata.price[0] == 1) mMoney.text = string.Format(Core.Data.stringManager.getString(20091), colorString + mdata.price[1].ToString() + "[-]", Core.Data.stringManager.getString(5070),mdata.name);
//			else mMoney.text = string.Format(Core.Data.stringManager.getString(20091), colorString + mdata.price[1].ToString() + "[-]", Core.Data.stringManager.getString(5166),mdata.name);
		}
		else
		{
			SetTagMoneyName(mdata.discount[0], mdata.discount[1], mdata.name, mMoney);
//			if(mdata.discount[0] == 0)	mMoney.text = string.Format(Core.Data.stringManager.getString(20091),colorString+ mdata.discount[1].ToString()+"[-]", Core.Data.stringManager.getString(5037),mdata.name);
//			else if(mdata.discount[0] == 1) mMoney.text = string.Format(Core.Data.stringManager.getString(20091), colorString + mdata.discount[1].ToString()+"[-]", Core.Data.stringManager.getString(5070),mdata.name);
//			else mMoney.text = string.Format(Core.Data.stringManager.getString(20091), colorString + mdata.discount[1].ToString()+"[-]", Core.Data.stringManager.getString(5166),mdata.name);
		}
		
		
		mDescription.text = mdata.description;
		
		mName.text = string.Format(Core.Data.stringManager.getString(20092), mdata.name);
	}

	void SetTagMoneyName(int m_itemid, int m_num, string m_itemname, UILabel m_label, bool m_IsSecretshop = false)
	{
		if(m_itemid == 0)
		{
			if(!m_IsSecretshop)m_label.text = string.Format(Core.Data.stringManager.getString(20091), colorString +m_num.ToString()+"[-]", Core.Data.stringManager.getString(5037), m_itemname);
			else m_label.text = string.Format(Core.Data.stringManager.getString(20091), colorString +m_num.ToString()+"[-]", Core.Data.stringManager.getString(5070), m_itemname);
            
		}
		else if(m_itemid == 1)
		{
			if(!m_IsSecretshop)m_label.text = string.Format(Core.Data.stringManager.getString(20091), colorString +m_num.ToString()+"[-]", Core.Data.stringManager.getString(5070), m_itemname);
			else m_label.text = string.Format(Core.Data.stringManager.getString(20091), colorString +m_num.ToString()+"[-]", Core.Data.stringManager.getString(5037), m_itemname);
            
        }
		else if(m_itemid == 2)
		{
			m_label.text = string.Format(Core.Data.stringManager.getString(20091), colorString +m_num.ToString()+"[-]", Core.Data.stringManager.getString(25175), m_itemname);
        }
		else if(m_itemid == 3)
		{
			m_label.text = string.Format(Core.Data.stringManager.getString(20091), colorString +m_num.ToString()+"[-]", Core.Data.stringManager.getString(25176), m_itemname);
        }
		else
		{
			ItemData _data = null;
			_data = Core.Data.itemManager.getItemData(m_itemid);
			if(_data != null)
			{
				m_label.text = string.Format(Core.Data.stringManager.getString(20091), colorString +m_num.ToString()+"[-]", _data.name, m_itemname);
			}
			else
			{
				m_label.SafeText("");
			}
		}
        
	}

	void Back_OnClick()
	{
		Destroy(gameObject);
	}

	void OnDestroy()
	{
		BuyNumber = null;
	}

	public void Buy_OnClick()
	{
		if(Shop_Index == 2)
		{
			BuyNormal_OnClick();
		}
		else
		{
			SecretShopMgr.GetInstance().SecretShopBuyRequest();
		}

		gameObject.SetActive(false);
	}

	bool CheckVipBuy()
	{
		if(_type != ShopItemType.Vip)return true;
		int choose = mdata.ID-110066+1;
		if(Core.Data.playerManager.curVipLv < choose)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(20063));
			return false;
		}
		else
		{
			for(int i=0; i<UIDragonMallMgr.GetInstance()._vipstatus.Length; i++)
			{
				if(choose == UIDragonMallMgr.GetInstance()._vipstatus[i].vipLvl)
				{
					SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(20064));
					return false;
				}
			}
			
		}
		return true;
	}

	void BuyNormal_OnClick()
	{
		if(!CheckVipBuy())return;

		int mNeedMoney = 0;
		int mMoneyType = 0;

		if(_type == ShopItemType.Active)
		{
			mMoneyType = ActiveItemdata.mType;
            if(ActiveItemdata.disPrice == 0)
			{
				mNeedMoney = ActiveItemdata.price;
			}
			else
			{
				mNeedMoney = ActiveItemdata.disPrice;
			}
		}
		else
		{
			if(mdata.discount.Length != 0)
			{
				mMoneyType = mdata.discount[0];
				mNeedMoney = mdata.discount[1];
			}
			else
			{
				if(mdata.price.Length != 0)
                {
                    mMoneyType = mdata.price[0];
                    mNeedMoney = mdata.price[1];
                }
			}
		}


		if(mMoneyType == 0)
		{
			if(Core.Data.playerManager.Coin < mNeedMoney)
			{
                JCRestoreEnergyMsg.OpenUI(ItemManager.COIN_PACKAGE,ItemManager.COIN_BOX,2);
				return;
			}
		}
		else if(mMoneyType == 1)
		{
			if(Core.Data.playerManager.Stone < mNeedMoney)
			{
				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(35006));
				return;
			}

            // talkingdata add by wxl
			if(_type == ShopItemType.Active)TDataOnBuyItem (ActiveItemdata.pid,1,mNeedMoney);
            else TDataOnBuyItem (mdata.ID,1,mNeedMoney);
		}
		else
		{
			if(mMoneyType != 2 && mMoneyType != 3)
			{
				int _battlesoul = 0;
				ItemData data = null;
				_battlesoul = Core.Data.itemManager.GetBagItemCount(mMoneyType);
				data = Core.Data.itemManager.getItemData(mMoneyType);
				
				if(_battlesoul < mNeedMoney)
				{
					if(data != null)
					{
						SQYAlertViewMove.CreateAlertViewMove(string.Format(Core.Data.stringManager.getString(25174), data.name));
					}
					return;
				}
			}
			else
			{
				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(7999));

				return;
			}

		}

		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
        if(_type != ShopItemType.Active)
		{
			PurchaseParam param = new PurchaseParam();
			param.gid = Core.Data.playerManager.PlayerID;
			param.propid = mdata.ID;
			if(this.mdata.max == 0)param.nm = 1;
			else param.nm = BuyNumber.NumIndex;
			task.AppendCommonParam(RequestType.BUY_ITEM, param);
		}
		else
		{
			BuyActiveShopItemParam param = new BuyActiveShopItemParam();
			param.gid = int.Parse(Core.Data.playerManager.PlayerID);
			param.propid = ActiveItemdata.pid;
			param.nm = 1;
			task.AppendCommonParam(RequestType.ACTIVESHOPBUYITEM, param);
		}
		if(Core.Data.guideManger.isGuiding)
		{
			task.afterCompleted += SecretShopMgr.GetInstance().testHttpResp_UI;
		}
		else
		{
			task.afterCompleted += SecretShopMgr.GetInstance()._UIDragonMallCell.testHttpResp_UI;
		}
		task.DispatchToRealHandler ();
		ComLoading.Open ();
		Back_OnClick();
	}

    void TDataOnBuyItem(int id,int num,int stoneNum){
        string itemName = "";
        switch (DataCore.getDataType (id)){
            case  ConfigDataType.Item:
                itemName =Core.Data.itemManager.getItemData (id).name ;
                break;
            case ConfigDataType.Frag:
                itemName = Core.Data.soulManager.GetSoulConfigByNum (id).name;
                break;
            case ConfigDataType.Equip:
                itemName = Core.Data.EquipManager.getEquipConfig (id).name;
                break;
            default:
                itemName = null;
                break;
        }
        if (string.IsNullOrEmpty (itemName) == false) {
            Core.Data.ActivityManager.OnPurchaseVirtualCurrency (itemName, num, stoneNum);
        }
    }
}
