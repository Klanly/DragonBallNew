using UnityEngine;
using System.Collections;

public class DropElement : MonoBehaviour {

	public StarsUI stars;
	public UISprite Spr_specialReward;
	
	public UILabel Lab_CityName;
	
	public UISprite Btn_Go;
	
	Floor floordata = null;
	
	public UILabel Lab_specialRewardName;
	
	public void SetData (Floor floordata, City city) 
	{
		gameObject.name = floordata.config.ID.ToString();
		this.floordata = floordata;
		Lab_CityName.text = city.config.name;
	     SetSpecialReward(floordata.config.specialRewardID);
		EnableJumpBtn = floordata.config.ID <= Core.Data.dungeonsManager.lastFloorId;
	}
	
	
	/*设置特殊奖励(几率掉落)
	 * */
	void SetSpecialReward(int itemId)
	{
//		Debug.Log("------显示特殊奖励:"+itemId.ToString());
		int itemtype=itemId/10000;
		switch(itemtype)
		{
		case 1:
			/*宠物
			 * */
			Spr_specialReward.gameObject.name=itemId.ToString();
			AtlasMgr.mInstance.SetHeadSprite(Spr_specialReward,itemId.ToString());
			MonsterData monster = Core.Data.monManager.getMonsterByNum(itemId);
			if(monster != null)
			{
				stars.SetStar( monster.star );
				Spr_specialReward.MakePixelPerfect();
				Lab_specialRewardName.text = monster.name;
			}
			break;
		case 4:
			/*装备
			 * */
			Spr_specialReward.atlas= AtlasMgr.mInstance.equipAtlas;
			Spr_specialReward.spriteName=itemId.ToString();		
			Spr_specialReward.gameObject.name=itemId.ToString();
			Spr_specialReward.MakePixelPerfect();
			EquipData equip = Core.Data.EquipManager.getEquipConfig(itemId);
			if(equip != null)
			{
			    stars.SetStar( equip.star );
				Lab_specialRewardName.text = equip.name;
			}
			break;
		case 15:
			{
				SoulData data = Core.Data.soulManager.GetSoulConfigByNum(itemId);
				if(data != null)
				{
				     stars.SetStar(data.star);
				       Spr_specialReward.gameObject.name=data.updateId.ToString();
						if (data.type == (int)ItemType.Monster_Frage) 
						{
							AtlasMgr.mInstance.SetHeadSprite(Spr_specialReward, data.updateId.ToString());
						}
						else if(data.type == (int)ItemType.Nameike_Frage || data.type == (int)ItemType.Earth_Frage)
						{
							Spr_specialReward.atlas = AtlasMgr.mInstance.itemAtlas;
					        Spr_specialReward.spriteName = data.updateId.ToString();
						}
				    Lab_specialRewardName.text = data.name;
			    }	
			     break;	
			}
		}		

	}
	
	
	
	public bool EnableJumpBtn
	{
		set
		{
			Btn_Go.color = value ? new Color(1f,1f,1f,1f) : new Color(0,0,0,1f);
			Btn_Go.transform.parent.GetComponent<BoxCollider>().enabled = value;
			Btn_Go.transform.parent.GetComponent<UIButtonColor>().enabled = value;
		}
	}
	
	
	//跳转到指定的副本
	void Jump()
	{
		DBUIController.mDBUIInstance.JumpToFB(this.floordata.config.ID);
		DropRewardPanel.DestroySelf();	
	}
	
	
}
