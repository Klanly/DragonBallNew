using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RUIType;




public class NewLSReceiver : EventReceiver 
{
	protected override void OnEvent(EventTypeDefine p_e, object p_param)
	{
		switch(p_e)
		{
			case EventTypeDefine.Click_DuoBaoButton:		
				FinalTrialMgr.GetInstance().m_NowTaskId = Core.Data.guideManger.LastTaskID;
				FinalTrialMgr.GetInstance().m_LastTaskId = Core.Data.guideManger.LastTaskID;
				DBUIController.mDBUIInstance.SetViewState (EMViewState.S_QiangDuo);
				break;
			case EventTypeDefine.Click_MartialConference:

				FinalTrialMgr.GetInstance().m_NowTaskId = Core.Data.guideManger.LastTaskID;
				FinalTrialMgr.GetInstance().CreateScript(TrialEnum.TrialType_TianXiaDiYi, QiangduoEnum.QiangduoEnum_List);
				break;
			case EventTypeDefine.Click_Exchange_DuoBao:
				FinalTrialMgr.GetInstance().RequestByQiangduoType(QiangduoEnum.QiangduoEnum_Duihuan);
	//			FinalTrialMgr.GetInstance().qiangDuoPanelScript.MoveToTarget();
				break;
			case EventTypeDefine.Click_GetChaoShenShui:
				Core.Data.DuiHuanManager.buyZhanGongItem(1, 0);
				break;
			case EventTypeDefine.Click_BackToDuoBao:
				FinalTrialMgr.GetInstance().m_QiangduoEnum = QiangduoEnum.QiangduoEnum_Duihuan;
				FinalTrialMgr.GetInstance().qiangDuoPanelScript.OnBtnClose();
				break;
			case EventTypeDefine.Click_DuoBaoBackToMain:
			    DBUIController.mDBUIInstance.mDuoBaoView.OnBtnQuit();
				break;
			case EventTypeDefine.Click_SevenRewardButton:
				UISevenDayRewardMain.OpenUI();
				break;
			case EventTypeDefine.Click_GetSevenReward:
				UISevenDayRewardMain.GetInstance().mReward.mCellList[0].SendMsg();
			    Core.Data.guideManger.HideGuide();
				break;
			case EventTypeDefine.Click_CloseSevenReward:
				UISevenDayRewardMain.GetInstance().Back_OnClick();
				break;
			case EventTypeDefine.Click_RunTiaoZhanBtn:
				FinalTrialMgr.GetInstance().currentFightOpponentInfo = FinalTrialMgr.GetInstance().qiangDuoPanelScript.ListCell[0].fightOpponentInfo;
				Core.Data.temper._PvpEnemyName = FinalTrialMgr.GetInstance().qiangDuoPanelScript.ListCell[0].fightOpponentInfo.n;
				FinalTrialMgr.GetInstance().tianXiaDiYiFightRequest(FinalTrialMgr.GetInstance().qiangDuoPanelScript.ListCell[0].fightOpponentInfo.g,
				                                                    FinalTrialMgr.GetInstance().qiangDuoPanelScript.ListCell[0].fightOpponentInfo.r , EMViewState.S_QiangDuo, Core.Data.temper.gambleTypeId);
			    Core.Data.guideManger.HideGuide();
				break;
			case EventTypeDefine.Click_Shop:
				UIDragonMallMgr.GetInstance ().OpenUI (ShopItemType.HotSale);
				break;
			case EventTypeDefine.Click_BuyFiveStarEgg:
				List<ItemData> mitems = Core.Data.itemManager.GetShopItem(ShopItemType.HotSale);
				ItemData _data = null;
				foreach(ItemData data in mitems)
				{
					if(data.ID == 110025)
					{
						_data = data ;
						break;
					}
				}
				SecretShopMgr.GetInstance().SetSecretShopTag(true, _data, ShopItemType.HotSale, 2);
				break;
			case EventTypeDefine.Click_SureBuyFiveStarEgg:
				SecretShopMgr.GetInstance()._UISecretShopTag.Buy_OnClick();
			    Core.Data.guideManger.HideGuide();
				break;
			case EventTypeDefine.Click_ExitShop:
				DBUIController.mDBUIInstance.mUIDragonMallMain.Back_OnClick();
				break;
			case EventTypeDefine.UnLock_God_And_SecondPos:
				//Core.Data.guideManger.AutoRUN();
				break;
			default:
				break;

		}
	}


}
