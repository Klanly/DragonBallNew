using UnityEngine;
using System.Collections;

public class UISecretShopCell : RUIMonoBehaviour
{
	public UISprite mItemIcon ;
	public UISprite mSoulIcon;
	public UILabel mItemNum;
    //public UISprite[] mStars;
	public UILabel mItemName;
	public UISprite mChoose;
	public UISprite OverIcon;
	public UISprite AlphaIcon;
	public UISprite LockIcon;
	public UILabel mMoney;
	public UISprite mMoneyIcon;
    public StarsUI starsObj;
    public GameObject titleObj;
    public UISprite spDiscountTitle;
	SecretShopDataStruct mdata;
	ItemData _data;
	SecretShopBuyStatus _BuyStatus = SecretShopBuyStatus.Type_None;

//	Vector3 m1 = new Vector3(-440,-30,0);
//	float mm = 9.5f;
	int starnum;
	string str;

	public void OnShow(SecretShopDataStruct data)
	{
		if(data == null)
		{
			gameObject.SetActive(false);
			return;
		}
		else
		{
			gameObject.SetActive(true);
		}

		mdata = data;

		mItemIcon.gameObject.SetActive(true);
		mItemNum.gameObject.SetActive(true);
		mItemName.gameObject.SetActive(true);
		mChoose.gameObject.SetActive(true);

		GetObject(data.num);

		SetItemStatus();
		SetItemPrice();

        titleObj.SetActive (true);

        if (mdata.type == 1) {
         
            spDiscountTitle.spriteName = "sc_msz_dz";
        } else if (mdata.type == 2) {
            spDiscountTitle.spriteName = "sc_msz_dj1";
        } else if(mdata.type ==0) {
            titleObj.SetActive (false);
        }

		mChoose.gameObject.SetActive(false);

		mItemName.text = str;

		mSoulIcon.gameObject.SetActive(false);
        mItemNum.text = ItemNumLogic.setItemNum(mdata.count,mItemNum,mItemNum.gameObject.transform.parent.gameObject.GetComponent<UISprite>());
		ConfigDataType datatype = DataCore.getDataType(data.num);
		if (datatype == ConfigDataType.Monster)
		{
			AtlasMgr.mInstance.SetHeadSprite (mItemIcon, data.num.ToString ());
			mItemIcon.MakePixelPerfect ();
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

			SoulInfo info = new SoulInfo (0, data.num, data.count);
			Soul soul = new Soul ();
			soul.m_config = Core.Data.soulManager.GetSoulConfigByNum (info.num);
			soul.m_RTData = info;
			if (soul.m_config.type == (int)ItemType.Monster_Frage)
			{
				MonsterData mon = Core.Data.monManager.getMonsterByNum (soul.m_config.updateId);
				if(mon != null)
				{
					mSoulIcon.gameObject.SetActive(true); 
					AtlasMgr.mInstance.SetHeadSprite (mItemIcon, mon.ID.ToString ());
					mItemIcon.MakePixelPerfect ();
					mSoulIcon.spriteName = "bag-0003";
				}
			}
			else if (soul.m_config.type == (int)ItemType.Equip_Frage)
			{
				EquipData equip = Core.Data.EquipManager.getEquipConfig (soul.m_config.updateId);
				if (equip != null)
				{
					mSoulIcon.gameObject.SetActive(true); 
					mItemIcon.atlas = AtlasMgr.mInstance.equipAtlas;
					mItemIcon.spriteName = soul.m_config.updateId.ToString ();
					mItemIcon.MakePixelPerfect ();
					mSoulIcon.spriteName = "sui";
				}
			}
           else  
			{
				mItemIcon.atlas = AtlasMgr.mInstance.itemAtlas;
				mItemIcon.spriteName = soul.m_RTData.num.ToString ();
			}
			return;
		}
		else
		{
			RED.LogWarning("unknow reward type");
		}
			
		if (datatype == ConfigDataType.Gems)
		{
			GemsManager gemsMgr = Core.Data.gemsManager;
			if (gemsMgr != null)
			{
				GemData Gdata = gemsMgr.getGemData (mdata.num);
				if (Gdata != null)
				{
					mItemIcon.spriteName = Gdata.anime2D;
				}
			}
		}
		else if (datatype == ConfigDataType.Item)
		{
			ItemData item = Core.Data.itemManager.getItemData (mdata.num);
			if (item != null)
				mItemIcon.spriteName = item.iconID.ToString ();
		}
		else
		{
			mItemIcon.spriteName = mdata.num.ToString ();
		}

        mItemIcon.MakePixelPerfect ();
	}

	public void OnHide()
	{
		mItemIcon.gameObject.SetActive(false) ;
		mItemNum.gameObject.SetActive(false);
		mItemName.gameObject.SetActive(false);
		mChoose.gameObject.SetActive(false);
//		for(int i=0; i<mStars.Length; i++)
//		{
//			mStars[i].gameObject.SetActive(false);
//		}
	}

	void OnClick()
	{
		SecretShopMgr.GetInstance().ClearAllChoose();
		SecretShopMgr.GetInstance()._UISecretShopCell = this;
        SecretShopMgr.GetInstance().SetSecretShopTag(true, SecretShopType.SecretShopType_Buy, _BuyStatus , mdata, 1);
		mChoose.gameObject.SetActive(true);
		//add by wxl 
//		if(_data != null){
//			string tName = _data.name.ToString ();
//			ControllerEventData ctrl = new ControllerEventData (tName, "UISecretShopCell");
//			ActivityNetController.GetInstance ().SendCurrentUserState (ctrl);
//		}
	}

	void GetObject( int pid)
	{
		ConfigDataType type = DataCore.getDataType(pid);
		switch(type)
		{
		case ConfigDataType.Monster:
			str = Core.Data.monManager.getMonsterByNum(pid).name;
			starnum = Core.Data.monManager.getMonsterByNum(pid).star;
			break;
		case ConfigDataType.Item:
			starnum = Core.Data.itemManager.getItemData(pid).star;
			str = Core.Data.itemManager.getItemData(pid).name;
			break;
		case ConfigDataType.Equip:
			starnum = Core.Data.EquipManager.getEquipConfig(pid).star;
			str = Core.Data.EquipManager.getEquipConfig(pid).name;
			break;
		case ConfigDataType.Gems:
			starnum = Core.Data.gemsManager.getGemData(pid).star;
			str = Core.Data.gemsManager.getGemData(pid).name;
            break;
		case ConfigDataType.Frag:
			starnum = Core.Data.soulManager.GetSoulConfigByNum(pid).star;
			str = Core.Data.soulManager.GetSoulConfigByNum(pid).name;
			break;
        default:
          ConsoleEx.DebugLog(" not found  : " +  pid);
            break;
        }
        starsObj.SetStar (starnum);
    }

	void SetItemPrice()
	{
		if(mdata != null && mdata.money != null)
		{
			if(mdata.money.Length != 2)return;
			if(mdata.money[0] == 0)
			{
				mMoneyIcon.atlas = AtlasMgr.mInstance.commonAtlas;
				mMoneyIcon.spriteName = "common-0014";
			}
			else if(mdata.money[0] == 1)
			{
				mMoneyIcon.atlas = AtlasMgr.mInstance.commonAtlas;
				mMoneyIcon.spriteName = "common-0013";
			}
			else if(mdata.money[0] == 2)
			{
				mMoneyIcon.atlas = AtlasMgr.mInstance.commonAtlas;
				mMoneyIcon.spriteName = "jifen";
			}
			else if(mdata.money[0] == 3)
			{
				mMoneyIcon.atlas = AtlasMgr.mInstance.commonAtlas;
				mMoneyIcon.spriteName = "common-0077";
			}
			else
			{
				GetMarkAtlasAndSet(mdata.money[0], mMoneyIcon);
			}
			mMoney.SafeText(mdata.money[1].ToString());
		}
		else
		{
			RED.LogError("mdata is null");
		}
	}

	void SetItemStatus()
	{
		if(mdata != null)
		{
			if(!mdata.canBuy)
			{
				_BuyStatus = SecretShopBuyStatus.Type_Lock;
				LockIcon.gameObject.SetActive(true);
				AlphaIcon.gameObject.SetActive(false);
				OverIcon.gameObject.SetActive(false);
			}
			else
			{
				LockIcon.gameObject.SetActive(false);
				if(mdata.buyTime == 0)
				{
					_BuyStatus = SecretShopBuyStatus.Type_Ok;
					AlphaIcon.gameObject.SetActive(false);
					OverIcon.gameObject.SetActive(false);
				}
				else
				{
					_BuyStatus = SecretShopBuyStatus.Type_SellOut;
					AlphaIcon.gameObject.SetActive(true);
					OverIcon.gameObject.SetActive(true);
				}

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

	public void HideItemSellout()
	{
		LockIcon.gameObject.SetActive(false);
		_BuyStatus = SecretShopBuyStatus.Type_SellOut;
		AlphaIcon.gameObject.SetActive(true);
		OverIcon.gameObject.SetActive(true);
	}
    
    
}
