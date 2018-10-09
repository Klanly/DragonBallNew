using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RUIType;

public partial class DBUIController {

	public void OnBtnPlayerViewID(int ide)
	{
		switch(ide)
		{
			case SQYPlayerController.ACT_BTN_COIN:
//				UIDragonMallMgr.GetInstance().OpenUI(ShopItemType.Item);
            JCRestoreEnergyMsg.OpenUI (110019, 110020, 2);
				break;
			case SQYPlayerController.ACT_BTN_STONE:
				UIDragonMallMgr.GetInstance().SetRechargeMainPanelActive();
				break;
			case SQYPlayerController.ACT_BTN_ENERGY:
//				UIDragonMallMgr.GetInstance ().OpenUI (ShopItemType.Item);
				JCRestoreEnergyMsg.OpenUI (110015, 110017, 0);
                break;
            case SQYPlayerController.ACT_BTN_POWER:
                UIDragonMallMgr.GetInstance().OpenUI(ShopItemType.Item);
                break;
		}
	}

	public void OnBtnMainViewID(int ide)
	{//ide
        if (UIWXLActivityMainController.Instance != null && UIWXLActivityMainController.Instance.active == true && ide != SQYMainController.CLICK_HuoDong)
        {
           
            UIWXLActivityMainController.Instance.OnBtnClick();
        }

		switch(ide)
		{
		case SQYMainController.CLICK_HaoYou:
			SetViewState(EMViewState.S_Friend);
			break;
		case SQYMainController.CLICK_BeiBao:
			SetViewState(EMViewState.S_Bag,EMBoxType.LOOK_Charator);
			HiddenFor3D_UI();
			break;
		case SQYMainController.CLICK_MONSTER:
			SetViewState (EMViewState.S_Team_NoSelect);
			HiddenFor3D_UI (false);
			break;
		case SQYMainController.CLICK_ShenLong:
			{      
				if (Core.Data.playerManager.RTData.curLevel < 15)
				{
					string strText = Core.Data.stringManager.getString (6054);
					strText = strText.Replace ("#", "15");
					SQYAlertViewMove.CreateAlertViewMove (strText);
					return;
				}
				SetViewState(EMViewState.S_ShenLong);
			}
			break;
		case SQYMainController.CLICK_RECHARGE:
			UIDragonMallMgr.GetInstance().SetRechargeMainPanelActive();
			break;
		case SQYMainController.CLICK_FuBen:
			SetViewState(EMViewState.S_FuBen);
			break;

		case SQYMainController.CLICK_DuoBao:
			if (Core.Data.playerManager.RTData.curLevel < 10)
			{
				string strText = Core.Data.stringManager.getString (6054);
				strText = strText.Replace ("#", "10");
				SQYAlertViewMove.CreateAlertViewMove (strText);
				return;
			}
			SetViewState(EMViewState.S_QiangDuo);
			break;

		case SQYMainController.CLICK_ShangCheng:
			if (!Core.Data.BuildingManager.ZhaoMuUnlock)
			{
				string strText = Core.Data.stringManager.getString (9111);
				strText = string.Format (strText, RED.GetChineseNum(4));
				SQYAlertViewMove.CreateAlertViewMove (strText);
				return;
			}
            SetViewState(EMViewState.S_ShangCheng);
            break;

        case SQYMainController.CLICK_HuoDong:
                //   RED.Log("  in huo dong");
			SetViewState(EMViewState.S_HuoDong);
			break;
        case SQYMainController.CLICK_XiaoXi:
            SetViewState(EMViewState.S_XiaoXi);
            break;
		case SQYMainController.CLICK_QiTianJiangLi:
			GetGiftPanelController.CreateUIRewardPanel(_bottomRoot);
			//WXLAcitvityFactory.CreatActivity (ActivityItemType.DailyGiftPanel,_bottomRoot,null);
            //SetViewState(EMViewState.S_SevenDaysReward);
			break;

        case SQYMainController.CLICK_RollAct:
            RouletteLogic.CreateRouleView ();
            break;
        case SQYMainController.CLICK_SuperGift:
		    UIBigWheel.OpenUI();
			HiddenFor3D_UI();
            break;
        case SQYMainController.CLICK_HappyScratch:
			UIGuaGuaLeMain.OpenUI();
			HiddenFor3D_UI();
            break;
        case SQYMainController.CLICK_GodGift:
            WXLAcitvityFactory.CreatActivity (ActivityItemType.RollGamblePanel, _bottomRoot, null);
            break;
        case SQYMainController.CLICK_RadarGroup:
			ComLoading.Open();
 //           SetViewState (EMViewState.S_HuoDong);
            ActivityNetController.GetInstance ().OutRadarLoginMSG (null, OnGoToRadarGroup);
            break;
	
		case SQYMainController. CLICK_FIRSTRECHARGE:

				DBUIController.mDBUIInstance.HiddenFor3D_UI(false);
				UIFirstRechargePanel.OpenUI();
				UIFirstRecharge.OpenUI();
				Core.Data.rechargeDataMgr._IsOpenFirstScene = true;
			break;

		case SQYMainController.CLICK_ACTIVITY:

			HolidayActivityLogic._instence();
			break;

		case SQYMainController.CLICK_DragonBank:
			WXLAcitvityFactory.CreatActivity (ActivityItemType.DragonBank, _bottomRoot, null);
			break;
		}
	}

    void OnGoToRadarGroup()
	{
		HiddenFor3D_UI ();
        RadarTeamUI.OpenUI ();

    }
}



public class BattleToUIInfo
{
    public static RUIType.EMViewState From
    {
        get;
        set;
    }
}
