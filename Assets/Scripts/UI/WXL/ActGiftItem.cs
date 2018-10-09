using UnityEngine;
using System.Collections;

public class ActGiftItem : RUIMonoBehaviour,IItem {

	public UILabel lblName;
	public UILabel lblGiftNum;
	public UILabel lblHonorNum;
	public UISprite icon;
	int id2;
	int id1;
	string strName;
	int giftNum;
	int needHonorNum;
	HonorItemData itemInfo;
	public UISprite chargeIcon;
    public UISprite LblGiftNumBg;//yangcg
    bool canClick =false;
	public UISprite soulIcon;

	public void SetItemValue(object obj){
		gameObject.GetComponent<UIButtonMessage> ().target = gameObject;
		gameObject.GetComponent<UIButtonMessage> ().functionName = "ChargeHonorItem";
		itemInfo = obj as HonorItemData;
		id1 = itemInfo.index;
		strName = itemInfo.name;
		needHonorNum = itemInfo.cost;
		giftNum = itemInfo.count;
		this.Refresh();
	}
	/// <summary>
	/// Returns the value.
	/// </summary>
	public object ReturnValue(){
		return (object)itemInfo;
	}


    public void SetGiftSpt(){

        ConfigDataType type = DataCore.getDataType(id1);
        switch (type)
        {
            case ConfigDataType.Monster:
                AtlasMgr.mInstance.SetHeadSprite(icon,id1.ToString());
                strName = Core.Data.monManager.getMonsterByNum(id1).name;
			soulIcon.gameObject.SetActive (false);
                break;
            case ConfigDataType.Item:
                icon.atlas = AtlasMgr.mInstance.itemAtlas;
                icon.spriteName = id1.ToString();
                strName = Core.Data.itemManager.getItemData(id1).name;
			soulIcon.gameObject.SetActive (false);
                break;
            case ConfigDataType.Equip:
                icon.atlas = AtlasMgr.mInstance.equipAtlas;
                icon.spriteName = id1.ToString();
                strName = Core.Data.EquipManager.getEquipConfig(id1).name;
			soulIcon.gameObject.SetActive (false);
                break;
            case ConfigDataType.Gems:
                icon.atlas = AtlasMgr.mInstance.commonAtlas;
                icon.spriteName = Core.Data.gemsManager.getGemData(id1).anime2D;
                strName = Core.Data.gemsManager.getGemData(id1).name;
			soulIcon.gameObject.SetActive (false);
                break;
		case ConfigDataType.Frag:
			SoulData tSoulD = Core.Data.soulManager.GetSoulConfigByNum (id1);
			if (tSoulD != null) {	
				strName = tSoulD.name;
				if (tSoulD.type == (int)ItemType.Monster_Frage) {
					AtlasMgr.mInstance.SetHeadSprite (icon, tSoulD.updateId.ToString ());
					soulIcon.gameObject.SetActive (true);
					soulIcon.spriteName = "bag-0003";
				} else if (tSoulD.type == (int)ItemType.Equip_Frage) {
					soulIcon.gameObject.SetActive (true);
					soulIcon.spriteName = "sui";
					AtlasMgr.mInstance.SetHeadSprite (icon, tSoulD.updateId.ToString ());
				} else {
					icon.atlas = AtlasMgr.mInstance.itemAtlas;
					icon.spriteName = tSoulD.ID.ToString ();
					soulIcon.gameObject.SetActive (false);
				}
			}
                break;
                default:
            RED.LogError("monstercome charge not found  : " + id1);
                break;
        }
 
    }
        
	public void Refresh(){
		lblName.text =  strName;
        lblGiftNum.text =  ItemNumLogic.setItemNum(giftNum , lblGiftNum , LblGiftNumBg );//yangcg
		lblHonorNum.text = needHonorNum.ToString ();
        this.ShiftChargeSp ();
        this.SetGiftSpt();
	}


    void ShiftChargeSp(){
        int tCurP = UIActMonsterComeController.Instance.curMyPointNum;
        if (tCurP >= needHonorNum) {
			chargeIcon.spriteName = "EUMO-02";
            canClick = true;
        } else {
			chargeIcon.spriteName = "EUMO-03";
            canClick = false;
        }
    }

	public void ChargeHonorItem(){
		if (itemInfo != null) {
			if (canClick == true) {
				ActivityNetController.GetInstance ().SendChargeItem (itemInfo.id);
			} else
				ActivityNetController.ShowDebug (Core.Data.stringManager.getString (7305));
		}
	}
}
