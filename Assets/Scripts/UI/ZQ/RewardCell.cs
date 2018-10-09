using UnityEngine;
using System.Collections;

public class RewardCell : MonoBehaviour 
{
	private ItemOfReward m_date;

	//是否展示m_txtCnt
	private bool ShowTxtCnt = true;

	public UISprite m_spIcon;
	public UISprite m_spAttr;
	public UISprite m_spBg;
	public StarsUI m_star;
	public UILabel m_txtCnt;
	public UILabel m_txtLvl;
	public UILabel m_txtName;
	public GameObject m_cntRoot;
	public UISprite m_AddSpr;

	public void InitUI(ItemOfReward reward)
	{
		m_date = reward;
		ShowTxtCnt = true;
		if (reward.getCurType () == ConfigDataType.Monster)
		{
			Monster data = reward.toMonster (Core.Data.monManager);
			ShowMonster(data);
		}
		else if (reward.getCurType () == ConfigDataType.Equip)
		{
			Equipment data = reward.toEquipment (Core.Data.EquipManager, Core.Data.gemsManager);
			ShowEquip(data);
		}
		else if (reward.getCurType () == ConfigDataType.Gems)
		{
			Gems data = reward.toGem (Core.Data.gemsManager);
			ShowGem(data);
		}
		else if (reward.getCurType () == ConfigDataType.Frag)
		{
			Soul data = reward.toSoul (Core.Data.soulManager);
			ShowFrag(data);
//			if (data.m_config.type == (int)ItemType.Monster_Frage)
//			{
//				ShowSoul (data);
//			}
//			else if(data.m_config.type == (int)ItemType.Nameike_Frage || data.m_config.type == Earth_Frage)
//			{
//				ShowFrag(data);
//			}
		}
		else if (reward.getCurType () == ConfigDataType.Item)
		{
			Item item = reward.toItem (Core.Data.itemManager);

			if(reward.showpid != 0)
			{
				ConfigDataType type = DataCore.getDataType(reward.pid);
				if(type == ConfigDataType.Item)
				{
					Item realItem = new Item();
					realItem.RtData = new ItemInfo();
					realItem.RtData.num = reward.pid;
					realItem.configData = Core.Data.itemManager.getItemData(reward.pid);
					ShowItem(realItem);
				}
				else if(type == ConfigDataType.Frag)
				{
					Soul soul = new Soul();
					soul.m_RTData = new SoulInfo();
					soul.m_RTData.num = reward.pid;
					soul.m_RTData.count = 1;
					soul.m_config = Core.Data.soulManager.GetSoulConfigByNum(reward.pid);
					ShowFrag(soul);
				}
			}
			else
			{
				ShowItem(item);
			}
		}
		else if(reward.getCurType() == ConfigDataType.Frag)
		{
			Soul soul = reward.toSoul(Core.Data.soulManager);
			ShowFrag(soul);
		}
		else
		{
			RED.LogWarning("unknow reward type");
		}

		if(ShowTxtCnt)
			RED.SetActive (reward.num > 1, m_cntRoot);
		else 
			RED.SetActive (false, m_cntRoot);
       
        m_txtCnt.text =  ItemNumLogic.setItemNum(reward.num,m_txtCnt , m_cntRoot.GetComponent<UISprite>()); // yangchenguang 
		m_spIcon.MakePixelPerfect ();
	}

	
	
	
	void ShowFrag(Soul soul)
	{
       
		if (soul.m_config.type == (int)ItemType.Monster_Frage || soul.m_config.type == (int)ItemType.Equip_Frage) 
		{
			ShowSoul(soul);
		}
		else 
		{
			m_txtName.text = soul.m_config.name;
			m_spIcon.atlas = AtlasMgr.mInstance.itemAtlas;
			m_spIcon.spriteName = soul.m_RTData.num.ToString();
			m_spBg.spriteName = "star6";
			m_star.SetStar (soul.m_config.star);
			RED.SetActive (false, m_spAttr.gameObject);
			m_txtLvl.text = "";
		}
	}

	void ShowItem(Item item)
	{

		//如果是金币则，只能特殊处理，金币的数量太大
		string name = string.Empty;
		if(item.configData.type == (int)ItemType.Coin || item.configData.type == (int)ItemType.Stone)
		{
			name = item.configData.name + "X" +  m_date.num.ToString();
			ShowTxtCnt = false;
		}
		else
		{
			name = item.configData.name;
		}

		m_txtName.text = name;
		if(item.configData.ID == 110185)
		{
			m_spIcon.atlas = AtlasMgr.mInstance.commonAtlas;
			m_spIcon.spriteName = "jifen";
		}
		else
		{
			m_spIcon.atlas = AtlasMgr.mInstance.itemAtlas;
			m_spIcon.spriteName = item.configData.iconID.ToString();
		}
		
//		m_spIcon.spriteName = item.RtData.num.ToString();

		m_spBg.spriteName = "star6";
		m_txtLvl.text = "";
		RED.SetActive (false, m_spAttr.gameObject);

		if (item.configData.type == (int)ItemType.Coin)
		{
			RED.SetActive (false, m_star.gameObject);
		}
		else
		{
			RED.SetActive (true, m_star.gameObject);
			m_star.SetStar (item.configData.star);
		}
	}

	void ShowGem(Gems data)
	{
       
		m_txtName.text = data.configData.name;
		m_spIcon.atlas = AtlasMgr.mInstance.commonAtlas;
		m_spIcon.spriteName = data.configData.anime2D;
		m_spBg.spriteName = "star6";
		m_star.SetStar (data.configData.star);
		RED.SetActive (false, m_spAttr.gameObject);
		m_txtLvl.text = "";
	}

	void ShowSoul(Soul data)
	{
		if (data.m_config.type == (int)ItemType.Monster_Frage)
		{
			MonsterData mon = Core.Data.monManager.getMonsterByNum (data.m_config.updateId);
			m_txtName.text = data.m_config.name;
			AtlasMgr.mInstance.SetHeadSprite (m_spIcon, mon.ID.ToString());
			m_spBg.spriteName = "star6";
			m_star.SetStar (mon.star);
			m_spAttr.spriteName = "bag-0003";
		}
		else if (data.m_config.type == (int)ItemType.Equip_Frage)
		{
			EquipData equip = Core.Data.EquipManager.getEquipConfig (data.m_config.updateId);
			if (equip != null)
			{
				m_txtName.text = data.m_config.name;

				m_spIcon.atlas = AtlasMgr.mInstance.equipAtlas;
				m_spIcon.spriteName = data.m_config.updateId.ToString ();

				m_spBg.spriteName = "star6";
				m_star.SetStar (equip.star);
				m_spAttr.spriteName = "sui";
			}
		}

		m_spAttr.MakePixelPerfect ();
			
        m_txtLvl.text = "";
		RED.SetActive (true, m_spAttr.gameObject);
	}

	void ShowEquip(Equipment data)
	{
       
		m_txtName.text = data.ConfigEquip.name;
		m_spIcon.atlas = AtlasMgr.mInstance.equipAtlas;
		m_spIcon.spriteName = data.Num.ToString ();


        if (data.Num == 40999 || data.Num == 40998 || data.Num == 45999 || data.Num == 45998)
        {
            //yangchenguang 抽到经验装备显示EXP字样
            m_spAttr.gameObject.SetActive(true);
            m_spAttr.spriteName = "common-1055" ;
            m_spAttr.width= 46;
            m_spAttr.height = 26 ; 

        }else
        {
            RED.SetActive (false, m_spAttr.gameObject);
        }

		m_spBg.spriteName = "star6";
		m_star.SetStar (data.ConfigEquip.star);
		if(data.RtEquip != null)
		{
			
			if(data.RtEquip.lv == 0)
			{
				m_txtLvl.text = "Lv1";
			}
			else
			{
                m_txtLvl.text = "Lv" + data.RtEquip.lv.ToString ();
			}
		}

	}

	public void ShowMonster(Monster data)
	{
		if (data == null)
			return;

		m_txtName.text = data.config.name;
		AtlasMgr.mInstance.SetHeadSprite (m_spIcon, data.num.ToString ());

        //yangchenguang 抽到经验装备显示EXP字样
        if (data.num == 19999 || data.num == 19998 || data.num == 40999 || data.num == 40998 || data.num == 45999 || data.num == 45998)
        {

            m_spAttr.spriteName = "common-1055" ;
            m_spBg.spriteName = "star6" ; 
        }
		else
        {
            m_spAttr.spriteName = "Attribute_" + ((int)(data.RTData.Attribute)).ToString ();
            m_spBg.spriteName = "star" + data.RTData.m_nAttr.ToString ();
        }
		m_spAttr.MakePixelPerfect ();

	
		m_star.SetStar (data.Star);
        int level = 1;
        if (data.RTData.curLevel <= 0)
        {
            level = 1;
        } else
        {
            level = data.RTData.curLevel;
        }
        m_txtLvl.text = "Lv" + level.ToString ();

		RED.SetActive (true, m_star.gameObject, m_spAttr.gameObject);
	}
	
	
	

	
}
