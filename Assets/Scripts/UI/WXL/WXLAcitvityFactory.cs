using UnityEngine;
using System.Collections;
// / <summary>
/// simple factory  frame
/// </summary> 
public class WXLAcitvityFactory  {
	public static WXLActivityFestivalController _ActFestivalCtrl= null;
	public static UIActMonsterComeController _ActMonsterCtrl= null;
	public static UITaoBaoController _ActTaoBaoCtrl= null;
	public static ActDinnerController _ActDinnerCtrl= null;
	public static UIDateSignController _ActDateSignCtrl= null;
	public static UILevelRewardController _ActLevelRewardCtrl = null;
    public static TreasureBoxController _ActTreasureBoxCtrl = null;

	public object objPos;
	public static void CreatActivity(ActivityItemType type,object objPos , System.Action callback = null){
		//	AbsActivity AbsA= null;
		switch(type){
		case ActivityItemType.festivalItem:
						if (_ActFestivalCtrl == null)         
				_ActFestivalCtrl = WXLActivityFestivalController.CreateFestivalPanel (ActivityItemType.festivalItem, (GameObject)objPos);
			else
				_ActFestivalCtrl.gameObject.SetActive (true);

            //	ActivityNetController.GetInstance().SendLoginFestival ();
			break;
		case  ActivityItemType.mosterComeItem:
			if (_ActMonsterCtrl == null)
				_ActMonsterCtrl = UIActMonsterComeController.CreateMonsterPanel (ActivityItemType.mosterComeItem, (GameObject)objPos);
			else
				_ActMonsterCtrl.gameObject.SetActive (true);
			break;

		case ActivityItemType.taobaoItem:
			if (_ActTaoBaoCtrl == null)
				_ActTaoBaoCtrl = UITaoBaoController.CreateTaoBaoPanel (ActivityItemType.taobaoItem, (GameObject)objPos);
			else
				_ActTaoBaoCtrl.gameObject.SetActive (true);
			break;
		case ActivityItemType.dinnerItem:
			if (_ActDinnerCtrl == null)
				_ActDinnerCtrl = ActDinnerController.CreateDinnerPanel (ActivityItemType.dinnerItem, (GameObject)objPos);
			else
				_ActDinnerCtrl.gameObject.SetActive (true);
			break;
		case ActivityItemType.qiandaoItem:
			if (_ActDateSignCtrl == null)
				_ActDateSignCtrl = UIDateSignController.CreateUIdateSignPanel (ActivityItemType.qiandaoItem, (GameObject)objPos);
			else
				_ActDateSignCtrl.gameObject.SetActive (true );
			break;

		case ActivityItemType.levelRewardItem:
			if (_ActLevelRewardCtrl == null)
				_ActLevelRewardCtrl = UILevelRewardController.CreateUILevelRewardPanel (ActivityItemType.levelRewardItem, (GameObject)objPos ,callback);
			else
				_ActLevelRewardCtrl.gameObject.SetActive (true);
			break;
        case ActivityItemType.gonggaoItem:
            AnnounceMrg.GetInstance().SetInfoPanel(true);
            break;
        case ActivityItemType.secretShopItem:
			SecretShopMgr.GetInstance().SetSecretShop(true,1);
			DBUIController.mDBUIInstance.HiddenFor3D_UI ();
                break;
		case ActivityItemType.vipEnter:
			UIDragonMallMgr.GetInstance().SetVipLibao();
				break;
        case ActivityItemType.TreasureBoxItem:
            if (_ActTreasureBoxCtrl == null)
                _ActTreasureBoxCtrl = TreasureBoxController.CreatTreasureBoxCtr ();
            else
                _ActTreasureBoxCtrl.gameObject.SetActive (true);
            break;
        case ActivityItemType.RollGamblePanel:
            RollGambleController.CreatRollGamblePanel ();
            break;
        
        case ActivityItemType.DailyGiftPanel:
            DailyGiftController.CreatDailyGiftController ();
            break;
		case ActivityItemType.DragonBank:
			DragonBankController.CreatDragonBankController ();
			break;
		default:
			//	AbsA = null;
			break;
		}		
	}
}
