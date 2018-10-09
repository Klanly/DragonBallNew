using UnityEngine;
using System.Collections;

public class JCPVEPlotDesMonsterHead : MonoBehaviour {

	public UISprite Spr_head;
	public StarsUI star;
	public float CellWidth {get;set;}
	public UILabel Spr_Sgin;
	public UILabel Lab_Name;
	public UISprite sp_soul;

	private ItemOfReward m_data;

	void Awake()
	{
		CellWidth = 130f;
	}
	
	public void SetData (ItemOfReward award)
	{
		m_data = award;
		ConfigDataType type = DataCore.getDataType(award.pid);
		RED.SetActive (false, sp_soul.gameObject);
		switch(type)
		{	
			case ConfigDataType.Item:
			{
				ItemData data = Core.Data.itemManager.getItemData(award.pid);
				if(data != null)
				{
					Spr_head.atlas = AtlasMgr.mInstance.itemAtlas;
//				    Spr_head.spriteName = award.pid.ToString();
					Spr_head.spriteName = data.iconID.ToString ();
					Lab_Name.text = data.name;
				    star.SetStar(data.star);
				}
			    break;
			}		
			case ConfigDataType.Monster:
			{		  
				MonsterData data = Core.Data.monManager.getMonsterByNum(award.pid);
			     if(data != null)
				{
					  AtlasMgr.mInstance.SetHeadSprite (Spr_head, award.pid.ToString());
				      Lab_Name.text = data.name;
				      star.SetStar(data.star);
					if(sp_soul != null)
					{
						Monster mon = award.toMonster(Core.Data.monManager);
						int attr = (int)(mon.RTData.Attribute); 
						sp_soul.spriteName = "Attribute_" + attr.ToString();
						RED.SetActive (true, sp_soul.gameObject);
					}
				}
				break;
			}	
			case ConfigDataType.Equip:
			{
				EquipData data = Core.Data.EquipManager.getEquipConfig(award.pid);
			    if(data != null)
				{
					Spr_head.atlas = AtlasMgr.mInstance.equipAtlas;
				    Spr_head.spriteName = data.ID.ToString();
			        Lab_Name.text = data.name;
			        star.SetStar(data.star);
				}
			    break;
			}			
			case ConfigDataType.Gems:
			{
				GemData data = Core.Data.gemsManager.getGemData(award.pid);
			    if(data != null)
				{
					Spr_head.atlas = AtlasMgr.mInstance.commonAtlas;
				    Spr_head.spriteName = data.anime2D;
				    Lab_Name.text = data.name;
				    star.SetStar(data.star);
				}
			    break;
			}				
			case ConfigDataType.Frag:
			{
			    SoulData data = Core.Data.soulManager.GetSoulConfigByNum(award.pid);
			    switch((ItemType)data.type)
				{
				case ItemType.Earth_Frage:
				case ItemType.Nameike_Frage:
				   Spr_head.atlas = AtlasMgr.mInstance.itemAtlas;
			       Spr_head.spriteName = data.ID.ToString();
				   break;
				case ItemType.Monster_Frage:
				   AtlasMgr.mInstance.SetHeadSprite (Spr_head, data.updateId.ToString ());
				   sp_soul.spriteName = "bag-0003";
				   sp_soul.MakePixelPerfect();
				   RED.SetActive (true, sp_soul.gameObject);
				   break;
			     case ItemType.Equip_Frage:
				   Spr_head.atlas = AtlasMgr.mInstance.equipAtlas;
				   Spr_head.spriteName = data.updateId.ToString();
				   Spr_head.MakePixelPerfect();
				   sp_soul.spriteName = "sui";
				   sp_soul.MakePixelPerfect();
				   RED.SetActive (true, sp_soul.gameObject);
				   break;
				}

				Lab_Name.text = data.name;
			    star.SetStar(data.star);
				break;
			}
		}
		Spr_head.MakePixelPerfect();

	}
	
	public void Scale(float Value)
	{
		CellWidth*=Value;
		transform.localScale = new Vector3(Value,Value,1f);
	}
	
	public bool Rare
	{
		set
		{
			if(value)
				Scale(1.2f);			
			else
				Scale(1);
			Spr_Sgin.enabled = value;
		}
	}

//	void OnClick()
//	{
////		ConfigDataType type = DataCore.getDataType(m_data.pid);
////		if(type == ConfigDataType.Monster)	
////		{		  
////			Monster mon = m_data.toMonster (Core.Data.monManager);
////			MonsterInfoUI.OpenUI (mon, ShowFatePanelController.FateInPanelType.isInTeamModifyPanel, false);
////		}
//		Debug.LogError("OnClick");
//		CradSystemFx.GetInstance().InitHeadIntroduce(m_data,Vector3.zero);
//	}
	void OnPress(bool isPressed)
	{
		if(isPressed)
		{
			Vector3 pos = transform.localPosition;
			pos.y+= 135;
			CradSystemFx.GetInstance().InitHeadIntroduce(m_data,pos);
		}
		else
		{
			CradSystemFx.GetInstance().m_UIEggCellIntroduct.dealloc();
		}
	}

}
