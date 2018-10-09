using UnityEngine;
using System.Collections;

/// <summary>
/// 每个等级奖励中的一个奖励
/// </summary>
public class RewardCollectionItem : RUIMonoBehaviour,IItem
{
    public int giftId;
    public int num;
	public LevelRewardCollection.CollectionState curItemState;

	private CollectionItem colItem;
    public UISprite spt_GiftIcon;
    public UILabel lbl_GiftNum;
    public UILabel lbl_GiftName;
    public UISprite bg_Gift;
    public UISprite bg_lab;
    public UISprite lbl_GiftNumBg;
    Color lblColor = new Color (243f / 255f, 215f / 255f, 190f / 255f, 1f);
//    public enum GiftItemState
//    {
//        isUnlockState,
//        isGotState,
//        isLockState,
//    }


	//  LvItem tLvItem;
    string myName;
    public StarsUI stars;
    public UISprite frag_Icon;
	const string equipStr = "sui";
	const string monStr = "bag-0003";

    #region IItem implementation

    public void SetItemValue (object obj)
    {
		colItem = obj as CollectionItem;

		if (colItem != null) {
			giftId = colItem.ID;
			num = colItem.num;
			curItemState = colItem.giftType;
		}

		GetObject (giftId);

        Refresh ();
    }

    public object ReturnValue ()
    {
		return (object)colItem;
    }

    public void Refresh ()
    {

        lbl_GiftNum.text =  ItemNumLogic.setItemNum( num ,lbl_GiftNum , lbl_GiftNumBg); //yangcg 计算（道具个数）背景的大小

        switch (curItemState) {
		case LevelRewardCollection.CollectionState.LockReward:
            bg_Gift.color = Color.grey;
            bg_lab.color = Color.grey;
            spt_GiftIcon.color =Color.grey;
                // lbl_GiftName.color =Color.grey;
            break;
		case LevelRewardCollection.CollectionState.UnlockReward:
            bg_Gift.color = Color.white;
            bg_lab.color = Color.white;
            spt_GiftIcon.color = Color.white;
                //   lbl_GiftName.color = Color.white;
            bg_Gift.color = lblColor;
            break;
		case LevelRewardCollection.CollectionState.GotReward:
            bg_Gift.color = Color.white;
            bg_lab.color = Color.white;
            spt_GiftIcon.color = Color.white;
                // lbl_GiftName.color = Color.white;
            bg_Gift.color = lblColor;
            break;

        default:
            break;
        }
    }

    /// <summary>
    /// 获取 obj
    /// </summary>
    /// <returns>The object.</returns>
    /// <param name="pid">Pid.</param>
	public void GetObject (int pid)
    {
		if (colItem != null) {
			ConfigDataType type = DataCore.getDataType (colItem.ID);
			frag_Icon.gameObject.SetActive (false);
			switch (type) {
			case ConfigDataType.Monster:

				MonsterData mData = Core.Data.monManager.getMonsterByNum (pid);
				if (mData != null) {
					myName = mData.name;
					AtlasMgr.mInstance.SetHeadSprite (spt_GiftIcon, pid.ToString ());
					//	spt_GiftIcon.spriteName = colItem.ID.ToString ();
					stars.SetStar (mData.star);
				}
				break;
			case ConfigDataType.Item:

				ItemData iData = Core.Data.itemManager.getItemData (pid);
				if (iData != null) {
					myName = iData.name;
					spt_GiftIcon.atlas = AtlasMgr.mInstance.itemAtlas;
					spt_GiftIcon.spriteName = iData.iconID.ToString ();
					stars.SetStar (iData.star);
				}
				break;
			case ConfigDataType.Equip:
				EquipData eData = Core.Data.EquipManager.getEquipConfig (pid);
				if (eData != null) {
					myName = eData.name;
					spt_GiftIcon.atlas = AtlasMgr.mInstance.equipAtlas;
					spt_GiftIcon.spriteName = colItem.ID.ToString ();
					stars.SetStar (eData.star);
				}
				break;
			case ConfigDataType.Gems:
				GemData gData = Core.Data.gemsManager.getGemData (pid);
				if (gData != null) {
					myName = gData.name;
					spt_GiftIcon.atlas = AtlasMgr.mInstance.commonAtlas;
					spt_GiftIcon.spriteName = gData.anime2D;
					stars.SetStar (gData.star);
				}
				break;
			case ConfigDataType.Frag:
				SoulData data = Core.Data.soulManager.GetSoulConfigByNum (pid);
				myName = data.name;
				AtlasMgr.mInstance.SetHeadSprite (spt_GiftIcon, data.updateId.ToString ());
				stars.SetStar (Core.Data.soulManager.GetSoulConfigByNum (pid).star);
				if (data.type == (int)ItemType.Monster_Frage) {
					frag_Icon.gameObject.SetActive (true);
					frag_Icon.spriteName = monStr;
				} else if (data.type == (int)ItemType.Equip_Frage) {
					frag_Icon.gameObject.SetActive (true);
					frag_Icon.spriteName = equipStr;
				}

				break;
			default:
				RED.LogWarning ("reward collection item  not found  : " + pid);
				break;
			}
		
			lbl_GiftName.text = myName;
			spt_GiftIcon.MakePixelPerfect ();
			//return tObj;   
		}
    }


    #endregion
}
public class CollectionItem{
	public int ID;
	public int num;
	public LevelRewardCollection.CollectionState giftType;
	public CollectionItem (int tId, int tNum, LevelRewardCollection.CollectionState tGiftType){
		ID = tId;
		num = tNum;
		giftType = tGiftType;
	}
}
