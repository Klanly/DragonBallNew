using UnityEngine;
using System.Collections;

public class TBGiftDespItem : MonoBehaviour {

	public int id;
	public UISprite giftIcon;
	public UILabel lblTitle;
	public UILabel lblDesp;

	void Start(){
		this.AddInfo();
	}

	public void AddInfo()
    { string myName = "";
        string tDesp = "";
		if (id == 0)
			return;
		switch(DataCore.getDataType(id)){
        case ConfigDataType.Monster:
            MonsterData tMonData = Core.Data.monManager.getMonsterByNum (id);
            myName = tMonData.name;
            AtlasMgr.mInstance.SetHeadSprite (giftIcon, id.ToString ());
            giftIcon.spriteName = tMonData.ID.ToString ();
            giftIcon.MakePixelPerfect ();
            tDesp = tMonData.description;
			break;
        case ConfigDataType.Item:
            ItemData tItemData = Core.Data.itemManager.getItemData (id);
            myName = tItemData.name;
            giftIcon.atlas = AtlasMgr.mInstance.itemAtlas;
            giftIcon.spriteName = tItemData.ID.ToString ();
            tDesp = tItemData.description;
			break;
        case ConfigDataType.Equip:
            EquipData tEquipData = Core.Data.EquipManager.getEquipConfig (id);
            myName = tEquipData.name;
            giftIcon.atlas = AtlasMgr.mInstance.equipAtlas;
            giftIcon.spriteName = tEquipData.ID.ToString ();
            tDesp = tEquipData.description;
			break;
        case ConfigDataType.Gems:
            GemData tGemData = Core.Data.gemsManager.getGemData (id);
            myName = tGemData.name;
            giftIcon.atlas = AtlasMgr.mInstance.commonAtlas;
            giftIcon.spriteName = tGemData.anime2D;
            tDesp = tGemData.description;
			break;
        case ConfigDataType.Frag:
            SoulData soul = Core.Data.soulManager.GetSoulConfigByNum (id);	
            if (soul != null) {
                myName = soul.name;
                AtlasMgr.mInstance.SetHeadSprite (giftIcon, soul.updateId.ToString ());
            } else {
                Debug.LogError (id);
            }
            tDesp = soul.description;

			break;
		default:
			RED.LogError(" not found  : " + id);
			break;
		}
        lblDesp.text = tDesp;
		lblTitle.text = myName;
	}
    public static TBGiftDespItem CreatGiftDespItem(GameObject tParent,int id){
		UnityEngine.Object obj = WXLLoadPrefab.GetPrefab (WXLPrefabsName.UITBDespItem);
		if (obj != null) {
			GameObject go = Instantiate (obj) as GameObject;
			TBGiftDespItem fc = go.GetComponent<TBGiftDespItem> ();
			Transform goTrans = go.transform;
            RED.AddChild(go,tParent);
			go.transform.localPosition = Vector3.zero;
			goTrans.localScale = Vector3.one;
			fc.id = id;

			return fc;
		}
		return null;		
	}
}
