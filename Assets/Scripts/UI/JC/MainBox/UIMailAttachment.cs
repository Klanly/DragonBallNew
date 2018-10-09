using UnityEngine;
using System.Collections;

public class UIMailAttachment : MonoBehaviour {

	
	public StarsUI star;
	public UILabel Lab_Name;
	public UISprite Spr_head;
	public GameObject NullObject;
	public GameObject ContentObject;
	public UILabel LabNum;
	void Start ()
	{
	
	}
	
    public void SetData(EmailAward award)
	{
		if(award == null) 
		{
			if(!NullObject.activeSelf)NullObject.SetActive(true);
		    if(ContentObject.activeSelf)ContentObject.SetActive(false);
			return;
		}
		if(NullObject.activeSelf)NullObject.SetActive(false);
		if(!ContentObject.activeSelf)ContentObject.SetActive(true);
		
		ConfigDataType type = DataCore.getDataType(award.pid);
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
				   AtlasMgr.mInstance.SetHeadSprite (Spr_head, data.updateId.ToString());
				   break;
				}

				Lab_Name.text = data.name;
			    star.SetStar(data.star);
				break;
			}		
		}
		Spr_head.MakePixelPerfect();


#region 显示数量
		int temp_num = award.count / 10000;
		if(temp_num > 0)
			LabNum.text =  ( ((float)(int)(((float)award.count/10000f)*100))/100f ).ToString() + Core.Data.stringManager.getString(6117);
		else
			LabNum.text = award.count.ToString();
#endregion

	}
}
