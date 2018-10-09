using UnityEngine;
using System.Collections;

public class UIMailNewAttachment : MonoBehaviour {


	public UILabel Lab_NameAndCount;
	public UISprite Spr_Item;
	public UISprite Spr_Gou;

	void Start ()
	{
	
	}
	
    public void SetData(EmailAward award)
	{

		if(award == null) 
		{
			return;
		}

		if(!gameObject.activeSelf)gameObject.SetActive(true);

		ConfigDataType type = DataCore.getDataType(award.pid);

		bool anotherSacle = false; 
		switch(type)
		{
			case ConfigDataType.Item:
			{
				ItemData data = Core.Data.itemManager.getItemData(award.pid);
				if(data != null)
				{
					Spr_Item.atlas = AtlasMgr.mInstance.itemAtlas;
//					Spr_Item.spriteName = award.pid.ToString();
					Spr_Item.spriteName = data.iconID.ToString ();
				    Lab_NameAndCount.text = data.name;
				}
			    break;
			}		
			case ConfigDataType.Monster:
			{		  
				MonsterData data = Core.Data.monManager.getMonsterByNum(award.pid);
			     if(data != null)
				{
				      AtlasMgr.mInstance.SetHeadSprite (Spr_Item, award.pid.ToString());
				      Lab_NameAndCount.text = data.name;
				      anotherSacle = true;
				}
				break;
			}	
			case ConfigDataType.Equip:
			{
				EquipData data = Core.Data.EquipManager.getEquipConfig(award.pid);
			    if(data != null)
				{
					Spr_Item.atlas = AtlasMgr.mInstance.equipAtlas;
					Spr_Item.spriteName = data.ID.ToString();
					Lab_NameAndCount.text = data.name;
				}
			    break;
			}			
			case ConfigDataType.Gems:
			{
				GemData data = Core.Data.gemsManager.getGemData(award.pid);
			    if(data != null)
				{
					Spr_Item.atlas = AtlasMgr.mInstance.commonAtlas;
					Spr_Item.spriteName = data.anime2D;
				    Lab_NameAndCount.text = data.name;
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
					Spr_Item.atlas = AtlasMgr.mInstance.itemAtlas;
					Spr_Item.spriteName = data.ID.ToString();
				   break;
			    case ItemType.Monster_Frage:
				   AtlasMgr.mInstance.SetHeadSprite (Spr_Item, data.updateId.ToString());
				   anotherSacle = true;
					break;
			    case ItemType.Equip_Frage:
					Spr_Item.atlas = AtlasMgr.mInstance.equipAtlas;
				    Spr_Item.spriteName = data.updateId.ToString();
					break;
				}
			    Lab_NameAndCount.text = data.name;
				break;
			}		
		}
		Spr_Item.MakePixelPerfect();
		Spr_Item.transform.localScale = anotherSacle ? new Vector3(0.3f,0.3f,0) : new Vector3(0.4f,0.4f,0);

#region 显示数量
		string str_count = "x";
		int temp_num = award.count / 10000;
		if(temp_num > 0)
			str_count +=  ( ((float)(int)(((float)award.count/10000f)*100))/100f ).ToString() + Core.Data.stringManager.getString(6117);
		else
			str_count += award.count.ToString();
#endregion
		Lab_NameAndCount.text = Lab_NameAndCount.text+" [ffff00]"+str_count+"[-]";
	}

	//是否已领取
	bool _isReceived = false;
	public bool isReceived 
	{
		set
		{
			if(!gameObject.activeSelf)gameObject.SetActive(true);
			_isReceived = value;
			Spr_Gou.gameObject.SetActive(_isReceived);
		}
		get
		{
			return _isReceived;
		}
	}

}
