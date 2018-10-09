using UnityEngine;
using System.Collections;

public class UIEggCellIntroduct : RUIMonoBehaviour 
{

	public UILabel m_Itemname;
	public UISprite m_Icon;
	public UILabel m_BagNum;
	public StarsUI m_StarsUI;
	public UILabel m_Des;
	public UISprite m_Circle;
	public UISprite m_Attr;

	public TweenScale m_TweenScale;
	public GameObject obj;

	ItemOfReward m_ItemOfReward;

	void Start()
	{
		transform.localPosition = CradSystemFx.GetInstance().m_IntroducePos;
	}

	void OnEnable()
	{
		transform.localPosition = CradSystemFx.GetInstance().m_IntroducePos;
		TweenScale.Begin<TweenScale>(obj, 0.3f);
	}

	public void OnShow(ItemOfReward mreward)
	{
		m_ItemOfReward = mreward;
		if (mreward.getCurType () == ConfigDataType.Monster)
		{
			Monster data = mreward.toMonster (Core.Data.monManager);
			ShowMonster(data);
		}
		else if (mreward.getCurType () == ConfigDataType.Equip)
		{
			Equipment data = mreward.toEquipment (Core.Data.EquipManager, Core.Data.gemsManager);
			ShowEquip(data);
		}
		else if (mreward.getCurType () == ConfigDataType.Gems)
		{
			Gems data = mreward.toGem (Core.Data.gemsManager);
			ShowGem(data);
		}
		else if (mreward.getCurType () == ConfigDataType.Frag)
		{
			Soul data = mreward.toSoul (Core.Data.soulManager);
			ShowFrag(data);
		}
		else if (mreward.getCurType () == ConfigDataType.Item)
		{
			Item item = mreward.toItem (Core.Data.itemManager);
			ShowItem(item);
		}
		else
		{
			RED.LogWarning("unknow reward type");
		}
		m_Icon.MakePixelPerfect();
	}

	void ShowMonster(Monster _data)
	{
		if(_data != null)
		{
			m_Itemname.text = _data.config.name;
			AtlasMgr.mInstance.SetHeadSprite(m_Icon, _data.config.ID.ToString());
			m_StarsUI.SetStar(_data.Star);
			
			m_Des.SafeText(_data.config.description);
			int attr = (int)(_data.RTData.Attribute); 

			int temp = Core.Data.monManager.GetAllMonsterByNum(_data.num).Count;
			m_BagNum.SafeText(string.Format(Core.Data.stringManager.getString(25158), temp));


            m_Attr.gameObject.SetActive(true);

            if (Core.Data.monManager.IsExpMon(_data.num))
            {
                m_Attr.spriteName = "common-1055" ;
                m_Attr.MakePixelPerfect();
                m_Circle.spriteName = "star6";
            }
            else
            {
                m_Circle.spriteName = "star" + _data.RTData.m_nAttr.ToString();
                m_Attr.spriteName = "Attribute_" + attr.ToString();


            }

		}
	}
	
	void ShowGem(Gems _data)
	{
		if(_data != null)
		{
			m_Itemname.text = _data.configData.name;
			m_Icon.atlas = AtlasMgr.mInstance.commonAtlas;
			m_Icon.spriteName = _data.configData.anime2D.ToString();
			m_StarsUI.SetStar(_data.configData.star);
			m_Circle.spriteName = "star6";
			m_Des.SafeText(_data.configData.description);
			m_Attr.gameObject.SetActive(false);
			int temp = Core.Data.gemsManager.getSameGemCount(_data);
			m_BagNum.SafeText(string.Format(Core.Data.stringManager.getString(25158), temp));
		}
	}
	
	void ShowEquip(Equipment _data)
	{
		if(_data != null)
		{
			m_Itemname.text = _data.ConfigEquip.name;
			m_Icon.atlas = AtlasMgr.mInstance.equipAtlas;
			m_Icon.spriteName = _data.ConfigEquip.ID.ToString();
			m_StarsUI.SetStar(_data.ConfigEquip.star);
			m_Circle.spriteName = "star6";
			m_Des.SafeText(_data.ConfigEquip.description);
			m_Attr.gameObject.SetActive(false);
			int temp = Core.Data.EquipManager.GetAllEquipByNum(_data.Num).Count;
			m_BagNum.SafeText(string.Format(Core.Data.stringManager.getString(25158), temp));
		}
	}
	
	void ShowFrag(Soul _data)
	{
		if(_data != null)
		{
			if (_data.m_config.type == (int)ItemType.Monster_Frage) 
			{
				MonsterData mon = Core.Data.monManager.getMonsterByNum(_data.m_config.updateId);
				if(mon != null)
				{
					m_Itemname.text = _data.m_config.name;
					AtlasMgr.mInstance.SetHeadSprite(m_Icon, mon.ID.ToString());
					m_StarsUI.SetStar(mon.star);
					m_Circle.spriteName = "star6";
					m_Des.SafeText(mon.description);
					m_Attr.gameObject.SetActive(true);
					m_Attr.spriteName = "bag-0003";
//					int temp = Core.Data.soulManager.GetFramentByType(ItemType.Monster_Frage).Count;
					int temp = Core.Data.soulManager.GetSoulCountByNum(_data.m_config.ID);
					m_BagNum.SafeText(string.Format(Core.Data.stringManager.getString(25158), temp));
				}
				
			}
			else if (_data.m_config.type == (int)ItemType.Equip_Frage)
			{
				EquipData equip = Core.Data.EquipManager.getEquipConfig (_data.m_config.updateId);
				if (equip != null)
				{
					m_Itemname.text =  _data.m_config.name;
					m_Icon.atlas = AtlasMgr.mInstance.equipAtlas;
					m_Icon.spriteName = _data.m_config.updateId.ToString ();
					m_Circle.spriteName = "star6";
					m_StarsUI.SetStar (equip.star);
					m_Attr.gameObject.SetActive(true);
					m_Attr.spriteName = "sui";
					m_Des.SafeText(_data.m_config.description);
					int temp = Core.Data.soulManager.GetSoulCountByNum(_data.m_config.ID);
//					int temp = Core.Data.soulManager.GetFramentByType(ItemType.Equip_Frage).Count;
					m_BagNum.SafeText(string.Format(Core.Data.stringManager.getString(25158), temp));
				}
			}
			else 
			{
				m_Itemname.text = _data.m_config.name;
				AtlasMgr.mInstance.SetHeadSprite(m_Icon, _data.m_config.ID.ToString());
				m_StarsUI.SetStar(_data.m_config.star);
				m_Circle.spriteName = "star6";
				m_Des.SafeText(_data.m_config.description);
				m_Attr.gameObject.SetActive(false);
				int temp = 0;
				if(_data.m_config.type == (int)ItemType.Earth_Frage)
				{
					temp = Core.Data.soulManager.GetSoulCountByNum(_data.m_config.ID);
//					temp = Core.Data.soulManager.GetFramentByType(ItemType.Earth_Frage).Count;
				}
				else if(_data.m_config.type == (int)ItemType.Frage_Bag)
				{
					temp = Core.Data.soulManager.GetSoulCountByNum(_data.m_config.ID);
//					temp = Core.Data.soulManager.GetFramentByType(ItemType.Frage_Bag).Count;
				}
				else if(_data.m_config.type == (int)ItemType.Nameike_Frage)
				{
					temp = Core.Data.soulManager.GetSoulCountByNum(_data.m_config.ID);
//					temp = Core.Data.soulManager.GetFramentByType(ItemType.Nameike_Frage).Count;
				}

				m_BagNum.SafeText(string.Format(Core.Data.stringManager.getString(25158), temp));
			}
		}
	}
	
	void ShowItem(Item _data)
	{
		//如果是金币则，只能特殊处理，金币的数量太大
		string name = string.Empty;
		if(_data.configData.type == (int)ItemType.Coin || _data.configData.type == (int)ItemType.Stone)
		{
			name = _data.configData.name + "X" +  m_ItemOfReward.num.ToString();
		}
		else
		{
			name = _data.configData.name;
		}
		
		m_Itemname.text = name;
		m_Icon.atlas = AtlasMgr.mInstance.itemAtlas;
//		m_Icon.spriteName = _data.RtData.num.ToString();
		m_Icon.spriteName = _data.configData.iconID.ToString ();
		m_Circle.spriteName = "star6";
		m_StarsUI.SetStar(_data.configData.star);
		m_Des.SafeText(_data.configData.description);
		m_Attr.gameObject.SetActive(false);
		int temp = Core.Data.itemManager.GetBagItemCount(_data.configData.ID);
		m_BagNum.SafeText(string.Format(Core.Data.stringManager.getString(25158), temp));

	}

}
