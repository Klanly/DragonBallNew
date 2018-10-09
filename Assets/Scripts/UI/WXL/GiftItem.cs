using UnityEngine;
using System.Collections;

public class GiftItem : MonoBehaviour,IItem {

    public UILabel lblNum;
    public UISprite spBG;
    public UISprite spIcon;
    private ItemOfReward myData;
    public StarsUI stars;
	public UILabel lblName;
    public UISprite lblBg;// yangchenguang itemNum 显示道具个数BG


    #region IItem implementation

    public void SetItemValue (object obj)
    {
        myData = obj as ItemOfReward;
        if (myData != null) {
            this.Refresh ();
        }
    }

    public object ReturnValue ()
    {
        return (object)myData;
    }

    public void Refresh ()
    {
        if (myData != null) {

            lblNum.text =ItemNumLogic.setItemNum(myData.num,lblNum,lblBg);//yangcg
            SetDetail (myData.pid);
        }
    }


    public void  SetDetail (int pid)
    {
        ConfigDataType type = DataCore.getDataType (pid);
        stars.gameObject.SetActive (true);
        object tObj = null;
        switch (type) {
		case ConfigDataType.Monster:
			tObj = Core.Data.monManager.getMonsterByNum (pid);
			MonsterData tMdata = tObj as MonsterData;
			if (tMdata != null) {
				lblName.text = tMdata.name;
			}
            AtlasMgr.mInstance.SetHeadSprite (spIcon, pid.ToString ());
            spIcon.spriteName = pid.ToString ();
            stars.SetStar (Core.Data.monManager.getMonsterByNum (pid).star);
            break;
        case ConfigDataType.Item:
            tObj = Core.Data.itemManager.getItemData (pid);
            ItemData tData = tObj as ItemData;
            if (tData.type == (int)ItemType.Stone) {
                stars.gameObject.SetActive (false);
            }
			if(tData != null)
				lblName.text = tData.name;
            spIcon.atlas = AtlasMgr.mInstance.itemAtlas;
            spIcon.spriteName = pid.ToString ();
            stars.SetStar (Core.Data.itemManager.getItemData (pid).star);
            break;
        case ConfigDataType.Equip:
            tObj = Core.Data.EquipManager.getEquipConfig (pid);
			EquipData eqData = tObj as EquipData;
			if (eqData != null) {
				lblName.text = eqData.name;
			}
            spIcon.atlas = AtlasMgr.mInstance.equipAtlas;
            spIcon.spriteName = pid.ToString ();
            stars.SetStar (Core.Data.EquipManager.getEquipConfig (pid).star);
            break;
        case ConfigDataType.Gems:
            tObj = Core.Data.gemsManager.getGemData (pid);
			GemData tGdata = tObj as GemData;
			if (tGdata != null) {
				lblName.text = tGdata.name;
			}
            spIcon.atlas = AtlasMgr.mInstance.commonAtlas;
            spIcon.spriteName = Core.Data.gemsManager.getGemData (pid).anime2D;
            stars.SetStar (Core.Data.gemsManager.getGemData (pid).star);
            break;
        case ConfigDataType.Frag:
            SoulData data = Core.Data.soulManager.GetSoulConfigByNum (pid);
			if (data != null) {
				lblName.text = data.name;
			}
            AtlasMgr.mInstance.SetHeadSprite (spIcon, data.updateId.ToString ());
            stars.SetStar (Core.Data.soulManager.GetSoulConfigByNum (pid).star);
            break;
        default:
            RED.LogError ("reward item  not found  : " + pid);
            break;
        }

        if (pid == 110031)
        {
            lblName.text = Core.Data.stringManager.getString(10006);
        }

        spIcon.width = 65;
        spIcon.height = 65;
    }


    #endregion


    
}
