using UnityEngine;
using System.Collections;

public class DateSignItem : MonoBehaviour,IItem {
	public UILabel lbl_VipDouble;
	public UILabel lbl_gifNum;
	public GameObject obj_vipTitle;
	public GameObject obj_Signed;
	public UISprite spt_giftIcon;
    public UILabel lbl_GiftName;
    public UISprite lbl_gifNumBg;//yangcg
	public enum SignItemState
	{
		isSigned,
		isWaitSigned,
		isNotSigned,
	}

	public SignItemState curSignItem;
	public SignItem itemValue;
	//第几位
	public int localNum;
	public int id;
	string icon;
	public int giftNum;
    public bool canClick = true;
    public string myName;
	public UISprite Bg_ItemSp;
    public UISprite frag_Icon;
	Color WaitColor = new Color(1f,215f/255f,8f/255f,1f);
	const string equipStr = "sui";
	const string monStr = "bag-0003";

    void Start(){
        
    }
	public void SetItemValue(object obj){
		itemValue = obj as SignItem;
        giftNum = itemValue.num;
        id = itemValue.pid;
        myName = this.GetGiftName();
		canClick = true;
		this.Refresh ();		
	}

	public object ReturnValue(){
		return (object)itemValue;
	}


	public void Refresh(){
		if (curSignItem == SignItemState.isSigned) {
			obj_Signed.SetActive (true);
			Bg_ItemSp.color = Color.white;
		} else if(curSignItem == SignItemState.isNotSigned){
			Bg_ItemSp.color = Color.white;
			obj_Signed.SetActive (false);
		} else if(curSignItem == SignItemState.isWaitSigned){
			obj_Signed.SetActive(false);
			Bg_ItemSp.color = WaitColor;
		}


		if (itemValue.vip != 0) {
			obj_vipTitle.SetActive (true);
			lbl_VipDouble.text = "[FFC100]v" + itemValue.vip + Core.Data.stringManager.getString (7303);
			}
		else {
			obj_vipTitle.SetActive (false);
		}

        lbl_gifNum.text = ItemNumLogic.setItemNum(giftNum , lbl_gifNum , lbl_gifNumBg); // yangchenguang 
        lbl_GiftName.text = myName;
        
	}

    public string GetGiftName(){
        if (id == 0)
            return string.Empty;

        frag_Icon.gameObject.SetActive(false);
        ConfigDataType type = DataCore.getDataType(id);
        switch (type)
        {
            case ConfigDataType.Monster:
            myName = Core.Data.monManager.getMonsterByNum(id).name;
                AtlasMgr.mInstance.SetHeadSprite(spt_giftIcon,id.ToString());
                break;
			case ConfigDataType.Item:
				ItemData tData = Core.Data.itemManager.getItemData (id);
				if (tData != null) {
					myName = tData.name;
					spt_giftIcon.atlas = AtlasMgr.mInstance.itemAtlas;
					spt_giftIcon.spriteName = tData.iconID.ToString ();
					spt_giftIcon.MakePixelPerfect ();
				}
				return myName;
            case ConfigDataType.Equip:
            myName = Core.Data.EquipManager.getEquipConfig(id).name;
                spt_giftIcon.atlas = AtlasMgr.mInstance.equipAtlas;              
                break;
            case ConfigDataType.Gems:
            myName = Core.Data.gemsManager.getGemData(id).name;
                spt_giftIcon.atlas = AtlasMgr.mInstance.commonAtlas;
                break;
			case ConfigDataType.Frag:
				SoulData soulD = Core.Data.soulManager.GetSoulConfigByNum (id);
				myName = soulD.name;
				int upId = soulD.updateId;
				AtlasMgr.mInstance.SetHeadSprite (spt_giftIcon, upId.ToString ());
				spt_giftIcon.MakePixelPerfect ();
				if (soulD.type == (int)ItemType.Monster_Frage) {
					frag_Icon.gameObject.SetActive (true);
					frag_Icon.spriteName = monStr;
				}else if(soulD.type == (int)ItemType.Equip_Frage) {
					frag_Icon.gameObject.SetActive (true);
					frag_Icon.spriteName = equipStr;
				}
				return myName;
            default:
                RED.LogError(" not found  : " + id);
                break;
        }
		spt_giftIcon.spriteName = id.ToString();
		spt_giftIcon.MakePixelPerfect ();
        return myName;
    }


	public void OnClickSignToday(){
//		Debug.Log ("can click   === "+canClick);
        if (canClick == false)
            return;
        if(curSignItem == SignItemState.isNotSigned){
//			Debug.Log ("not sign   === "+curSignItem);
            ActivityNetController.ShowDebug (string.Format(Core.Data.stringManager.getString(7329),localNum +1,myName));
            return;
        }
        //正常签到
//		Debug.Log ("not sign   === "+localNum  + "     signtms " + NoticeManager.signtms  +" masgnggg  "+ Core.Data.playerManager.RTData.masgn );
		if (localNum ==  NoticeManager.signtms){
            if (curSignItem == SignItemState.isWaitSigned) {
                //正常签到
				if(Core.Data.ActivityManager.GetSignState() == "1"){
//                if (Core.Data.playerManager.RTData.masgn < 1) {
                    ActivityNetController.GetInstance ().SignDayRequest (UIDateSignController.nowTime.Day);
                }
                canClick = false;
            } 
		}
	}


}
