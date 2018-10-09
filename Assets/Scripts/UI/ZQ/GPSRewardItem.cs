using UnityEngine;
using System.Collections;

public class GPSRewardItem : MonoBehaviour 
{
	public UILabel m_txtName;
	public UILabel m_txtCnt;
	public RewardCell m_reward;
	public GameObject m_objSpecial;

	public void SetGPSRewardData(GPSRewardData rd)
	{
		m_txtName.text = GetRewardName (rd.reward);
		m_txtCnt.text = "+" + rd.reward.num;
		m_reward.InitUI (rd.reward);
		RED.SetActive (false, m_reward.m_txtName.gameObject, m_reward.m_cntRoot.gameObject, m_reward.m_star.gameObject);
		RED.SetActive (rd.IsSpecial, m_objSpecial);
	}

	string GetRewardName(ItemOfReward reward)
	{
		string strName = "";
		if (reward.getCurType () == ConfigDataType.Monster)
		{
			Monster data = reward.toMonster (Core.Data.monManager);
			strName = data.config.name;
		}
		else if (reward.getCurType () == ConfigDataType.Equip)
		{
			Equipment data = reward.toEquipment (Core.Data.EquipManager, Core.Data.gemsManager);
			strName = data.ConfigEquip.name;
		}
		else if (reward.getCurType () == ConfigDataType.Gems)
		{
			Gems data = reward.toGem (Core.Data.gemsManager);
			strName = data.configData.name;
		}
		else if (reward.getCurType () == ConfigDataType.Frag)
		{
			Soul data = reward.toSoul (Core.Data.soulManager);
			strName = data.m_config.name;
		}
		else if (reward.getCurType () == ConfigDataType.Item)
		{
			ConfigDataType type = DataCore.getDataType(reward.pid);
			if(type == ConfigDataType.Item)
			{
				Item realItem = new Item();
				realItem.RtData = new ItemInfo();
				realItem.RtData.num = reward.pid;
				realItem.configData = Core.Data.itemManager.getItemData(reward.pid);
				strName = realItem.configData.name;
			}
			else if(type == ConfigDataType.Frag)
			{
				Soul soul = new Soul();
				soul.m_RTData = new SoulInfo();
				soul.m_RTData.num = reward.pid;
				soul.m_RTData.count = 1;
				soul.m_config = Core.Data.soulManager.GetSoulConfigByNum(reward.pid);
				strName = soul.m_config.name;
			}
		}
		return strName;
	}
}

public class GPSRewardData
{
	public ItemOfReward reward;		//奖励
	public bool IsSpecial;			//是否特殊奖励
}
