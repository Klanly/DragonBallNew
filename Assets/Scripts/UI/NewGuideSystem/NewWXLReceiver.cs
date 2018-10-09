using UnityEngine;
using System.Collections;

public class NewWXLReceiver : EventReceiver
{
	public NewWXLReceiver ()
    {

    }

    protected override void OnEvent (EventTypeDefine p_e, object p_param)
    {

        switch (p_e) {
        case EventTypeDefine.Click_Activity:
            DBUIController.mDBUIInstance.OnBtnMainViewID (SQYMainController.CLICK_HuoDong);
            //          UIWXLActivityMainController.Instance.MoveToTarget();
            break;
        case EventTypeDefine.Click_ActivityGiftBag:
            ActivityNetController.GetInstance ().GotLevelGiftRequest (Core.Data.playerManager.Lv);
			Core.Data.guideManger.HideGuide();
            break;
        case EventTypeDefine.Click_GetActivityGiftBag:
		    //5级奖励包
//            ActivityNetController.GetInstance ().GotLevelGiftRequest (Core.Data.playerManager.Lv);
//            UIGuide.Instance.HideGuide();
            // LevelRewardCollection.OnClickGiftOnGuide ();
            break;
        case EventTypeDefine.Click_HideUIGuide:
			Core.Data.guideManger.HideGuide ();
            break;
        case EventTypeDefine.Click_ExitLevelFiveReward:
            UILevelRewardController.Instance.OnBtnBack ();
            break;
        case EventTypeDefine.Click_SigninButton:
            UIWXLActivityMainController.Instance.OnQiandao ();
            break;
        case EventTypeDefine.Click_GetSigninRewardButton:
			if (UIDateSignController.Instance.ItemList != null) {
				int TN = NoticeManager.signtms;
				UIDateSignController.Instance.ItemList [TN].OnClickSignToday ();
			}
			Core.Data.guideManger.HideGuide ();
            break;
        case EventTypeDefine.Click_CloseSignin:
            UIDateSignController.Instance.OnBackBtn ();
            break;
        case EventTypeDefine.Click_DayRewardButton:
            WXLAcitvityFactory.CreatActivity (ActivityItemType.DailyGiftPanel,(object)DBUIController.mDBUIInstance._TopRoot,null);
            break;
        case EventTypeDefine.Click_SevenRewardUI:
                //    ActivityNetController.GetInstance ().GetSevenDayReward ();
			Core.Data.guideManger.HideGuide();
            break;
        case EventTypeDefine.Click_CloseDayRewardPanel:
            DailyGiftController.Instance.OnClickBack ();
            break;
        case EventTypeDefine.Sliding_Texture:
			Core.Data.guideManger.HideGuide();
            PVEDownloadCartoonController.Instance.RunToBottom();
            break;
        case EventTypeDefine.Click_CloseTexture:
            PVEDownloadCartoonController.Instance.CloseCartoonPanel();
            break;

        }
    }
}
