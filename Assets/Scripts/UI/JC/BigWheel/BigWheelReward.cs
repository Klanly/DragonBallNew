using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BigWheelReward : MonoBehaviour {

	public StarsUI star;
	public UILabel Lab_Name;
	public UISprite Spr_head;
	public UISprite Spr_Num;
	public UISprite Spr_Border;
	public UILabel LabNum;
	public UISprite UISelected ;
	public UISprite Spr_Sign;
	
	public List<UISprite> list_stars = new List<UISprite>();
	
	void Start ()
	{
	     isEnable = true;
	}
	
    public void SetData(int[] award)
	{
		if(award == null || award.Length == 0) 
		{
			return;
		}

		ConfigDataType type = DataCore.getDataType(award[0]);
		switch(type)
		{
			case ConfigDataType.Item:
			{
				ItemData data = Core.Data.itemManager.getItemData(award[0]);
				if(data != null)
				{
					Spr_head.atlas = AtlasMgr.mInstance.itemAtlas;
				    Spr_head.spriteName = award[0].ToString();
					Lab_Name.text = data.name;
				     star.SetStar(data.star);
				}
			    break;
			}		
			case ConfigDataType.Monster:
			{		  
				MonsterData data = Core.Data.monManager.getMonsterByNum(award[0]);
			     if(data != null)
				{
					  AtlasMgr.mInstance.SetHeadSprite (Spr_head,award[0].ToString());
				      Lab_Name.text = data.name;
				      star.SetStar(data.star);
				}
				break;
			}	
			case ConfigDataType.Equip:
			{
				EquipData data = Core.Data.EquipManager.getEquipConfig(award[0]);
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
				GemData data = Core.Data.gemsManager.getGemData(award[0]);
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
			    SoulData data = Core.Data.soulManager.GetSoulConfigByNum(award[0]);
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

        LabNum.text =         ItemNumLogic.setItemNum(award[1] , LabNum ,Spr_Num );
        //award[1].ToString();
		gameObject.name = award[0].ToString();
		
		isEnable = System.Convert.ToBoolean( award[2] );
	}
	
	
	public bool isSelected
	{
		set
		{
			UISelected.enabled = value;
		}
		get
		{
			return UISelected.enabled;
		}
	}
	

	bool _enable = false;
	public bool isEnable
	{
		set
		{
			_enable = value;
			Spr_Sign.enabled = !_enable;
			foreach (UISprite s in list_stars)
			{
				if( value )
			       s.color = new Color(1f,1f,1f,1f);
				else
				    s.color = new Color(0,0,0,1f);
			}
			if(_enable)
			{
				Spr_head.color = new Color(1f,1f,1f,1f);
				Spr_Num.color = new Color(1f,1f,1f,1f);
				Spr_Border.color = new Color(1f,1f,1f,1f);

			}
			else
			{
				 Spr_head.color = new Color(0,0,0,1f);
				 Spr_Num.color = new Color(0,0,0,1f);
				Spr_Border.color = new Color(0,0,0,1f);
			}
		}
		get
		{
			return _enable;
		}
	}
	
	
	
	

}
