using UnityEngine;
using System.Collections;

public class UITaskRewardData
{
	public UITaskRewardData(){}
	public UITaskRewardData(int _Reward_ItemID ,int _Reward_ItemCount)
	{
		Reward_ItemID = _Reward_ItemID;
		Reward_ItemCount = _Reward_ItemCount;
	}
	public int Reward_ItemID;
	public int Reward_ItemCount;
}


public class TaskReward : MonoBehaviour {

	public UILabel Lab_Name;
	public UILabel Lab_Num;
	public UISprite Spr_Head;
		
	public void SetTaskReward(UITaskRewardData data)
	{
		if(data == null)
		{
			if(gameObject.activeSelf)gameObject.SetActive(false);
			return;
		}
		
		if(!gameObject.activeSelf)gameObject.SetActive(true);
        SetInfo(data);
		
	}
    //add by wxl 
    void SetInfo(UITaskRewardData data)
	{
        switch(DataCore.getDataType(data.Reward_ItemID))
        {
            case ConfigDataType.Item:
                Spr_Head.atlas = AtlasMgr.mInstance.itemAtlas;

                ItemData tItem = Core.Data.itemManager.getItemData(data.Reward_ItemID);

                if (tItem != null)
                {
					Lab_Name.text = tItem.name;
					if(tItem.ID == 110185)
					{
						Spr_Head.atlas = AtlasMgr.mInstance.commonAtlas;
						Spr_Head.spriteName = "jifen";
						Spr_Head.height = 58;
						Spr_Head.width = 58;
					}
					else
					{
						Spr_Head.atlas = AtlasMgr.mInstance.itemAtlas;
						if (tItem.ID != 110052 || tItem.ID != 110051)
						{
							//                        Spr_Head.spriteName = tItem.ID.ToString();
							Spr_Head.spriteName = tItem.iconID.ToString ();
						}
						else 
						{
							//                        Spr_Head.spriteName = tItem.ID.ToString();
							Spr_Head.spriteName = tItem.iconID.ToString ();
						}
						Spr_Head.MakePixelPerfect();
					}
                }
				Lab_Num.text = ItemNumLogic.setItemNum( data.Reward_ItemCount,Lab_Num , Lab_Num.gameObject.transform.parent.gameObject.GetComponent<UISprite>()); //yangcg 计算（道具个数）背景的大小
				return;
            case ConfigDataType.Frag:
                SoulData sd = Core.Data.soulManager.GetSoulConfigByNum(data.Reward_ItemID);
                if(sd != null)
                {
                    AtlasMgr.mInstance.SetHeadSprite(Spr_Head, sd.updateId.ToString());
                    Lab_Name.text = sd.name;
                }
                break;
            case ConfigDataType.Equip:
                EquipData ed = Core.Data.EquipManager.getEquipConfig(data.Reward_ItemID);
                if (ed != null)
                {
                    Spr_Head.atlas = AtlasMgr.mInstance.equipAtlas;
                    Spr_Head.spriteName = ed.ID.ToString();
                    Lab_Name.text = ed.name;
                }
                break;

            case ConfigDataType.Monster:

                MonsterData md = Core.Data.monManager.getMonsterByNum(data.Reward_ItemID);
                if (md != null)
                {
                    AtlasMgr.mInstance.SetHeadSprite(Spr_Head,md.ID.ToString()); // yangchenguang 
                    Lab_Name.text = md.name;
                }
                break;
            case ConfigDataType.Gems:
                GemData gd = Core.Data.gemsManager.getGemData(data.Reward_ItemID);
                if (gd != null)
                {
                    Spr_Head.atlas = AtlasMgr.mInstance.commonAtlas;
                    Spr_Head.spriteName = gd.anime2D.ToString();
                    Lab_Name.text = gd.name;
                }
                break;
        }

        Lab_Num.text = ItemNumLogic.setItemNum( data.Reward_ItemCount,Lab_Num , Lab_Num.gameObject.transform.parent.gameObject.GetComponent<UISprite>()); //yangcg 计算（道具个数）背景的大小
        Spr_Head.MakePixelPerfect();
    }
}
