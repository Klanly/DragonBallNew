using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DuiHuanManager : Manager
{
	private List<DuiHuanItem> zhanGongDuiHuanItemListConfig;

	public List<ZhanGongItem> buyItemIDList = new List<ZhanGongItem>();

	public List<GoldBuyItemBuyTotalInfo> buyItemTotalList = new List<GoldBuyItemBuyTotalInfo>();  

	public DuiHuanManager()
	{
		zhanGongDuiHuanItemListConfig = new List<DuiHuanItem>();
	}

	public override bool loadFromConfig () {
		bool success = base.readFromLocalConfigFile<DuiHuanItem> (ConfigType.ZhanGongDuiHuanItem, zhanGongDuiHuanItemListConfig);
		return success;
	}

	public List<DuiHuanItem> getZhanGongDuiHuanItemListConfig()
	{
		return zhanGongDuiHuanItemListConfig;
	}

	public Action buyZhanGongItemCompletedDelegate;
	public Action<QiangDuoGoldBuyItemInfo> qiangDuoGoldBuyItemCompletedDelegate;

	public void buyZhanGongItem(int plistID, int rank)
	{
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.ZHANGONG_BUY_ITEM, new ZhanGongBuyItemParam(int.Parse(Core.Data.playerManager.PlayerID), plistID));

		task.afterCompleted = buyZhanGongItemCompleted;
		task.ErrorOccured = buyZhanGongItemError;

		task.DispatchToRealHandler ();
		ComLoading.Open();
	}

	public bool CheckLingqu(int rank)
	{
		for(int i=0; i<FinalTrialMgr.GetInstance().HaveLingqu.Length; i++)
		{
			if(rank == FinalTrialMgr.GetInstance().HaveLingqu[i])
			{
				return false;
			}
        }
		return true;
	}

	void buyZhanGongItemCompleted(BaseHttpRequest request, BaseResponse response)
	{
		ComLoading.Close();
		if(response != null && response.status != BaseResponse.ERROR)
		{
			ZhanGongBuyItemResponse buyitemResp = response as ZhanGongBuyItemResponse;

			if(buyitemResp != null && buyitemResp.data != null)
			{
				if(buyitemResp.data.zg == 0)SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(20075));
				else
				{
					SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(6034));
				}
				FinalTrialMgr.GetInstance().ZhanGongTotal += buyitemResp.data.zg;
				FinalTrialMgr.GetInstance().Now_Zhangong += buyitemResp.data.zg;
				FinalTrialMgr.GetInstance().qiangDuoPanelScript.duiHuanZhanGong.text = FinalTrialMgr.GetInstance().ZhanGongTotal.ToString();

				if(buyZhanGongItemCompletedDelegate != null)
				{
					buyZhanGongItemCompletedDelegate();
				}
				if(FinalTrialMgr.GetInstance().m_SelectDuihuancell != null)
				{
					if(buyitemResp.data.canBuy)
					{
						FinalTrialMgr.GetInstance().m_SelectDuihuancell.buyButton.isEnabled = true;
					}
					else
					{
						FinalTrialMgr.GetInstance().m_SelectDuihuancell.buyButton.isEnabled = false;
					}
				}


			}
		}
		else if(response != null && response.status == BaseResponse.ERROR)
		{
			if(response.errorCode == 4026)
			{
//				UITooltip.ShowText(Core.Data.stringManager.getString(35006));
//				RED.Log(Core.Data.stringManager.getString(35006));
				ConsoleEx.DebugLog(Core.Data.stringManager.getString(6017));
				SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(6017));
			}
			else if(response.errorCode == 4025)
			{
//				UITooltip.ShowText(Core.Data.stringManager.getString(35007));
//				RED.Log(Core.Data.stringManager.getString(35007));
				ConsoleEx.DebugLog(Core.Data.stringManager.getString(6018));
				SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(6018));
			}
			else if(response.errorCode == 8000)
			{
				ConsoleEx.DebugLog(Core.Data.stringManager.getString(20073));
				SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(20073));
			}
			else
			{
				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
			}
		}
	}

	public void buyZhanGongItemError(BaseHttpRequest request, string error)
	{
		RED.LogError(error);
	}

	public void getBuyItemIDCompleted(GetZhanGongBuyItemIDResponse getZhanGongBuyItemIDResponse)
	{
		if(getZhanGongBuyItemIDResponse != null && getZhanGongBuyItemIDResponse.status != BaseResponse.ERROR)
		{
			buyItemIDList.Clear();    
			if(getZhanGongBuyItemIDResponse.data != null && getZhanGongBuyItemIDResponse.data.item.Length > 0)
			{
				for(int i = 0; i < getZhanGongBuyItemIDResponse.data.item.Length; i++)
				{
					if(getZhanGongBuyItemIDResponse.data.item[i].price != 0)
					{
						buyItemIDList.Add(getZhanGongBuyItemIDResponse.data.item[i]);
                    }
				}
				for(int i = 0; i < getZhanGongBuyItemIDResponse.data.item.Length; i++)
				{
					if(getZhanGongBuyItemIDResponse.data.item[i].price == 0)
					{
						buyItemIDList.Add(getZhanGongBuyItemIDResponse.data.item[i]);
                    }
                }
            }
            
        }
        
    }

	public void getBuyItemIDCompleted(RefreshZhangongShopItemResponse refreshZhangongShopItemResponse)
	{
		if(refreshZhangongShopItemResponse != null && refreshZhangongShopItemResponse.status != BaseResponse.ERROR)
		{
			buyItemIDList.Clear();    
			if(refreshZhangongShopItemResponse.data != null && refreshZhangongShopItemResponse.data.item.Length > 0)
			{
				for(int i = 0; i < refreshZhangongShopItemResponse.data.item.Length; i++)
				{
					if(refreshZhangongShopItemResponse.data.item[i].price != 0)
					{
						buyItemIDList.Add(refreshZhangongShopItemResponse.data.item[i]);
					}
				}
				for(int i = 0; i < refreshZhangongShopItemResponse.data.item.Length; i++)
				{
					if(refreshZhangongShopItemResponse.data.item[i].price == 0)
					{
						buyItemIDList.Add(refreshZhangongShopItemResponse.data.item[i]);
					}
				}
			}
			
		}
		
	}
    
    public void getBuyItemTotalCompleted(GoldBuyItemBuyTotalResponse goldBuyItemBuyTotalResponse)
	{
		if(goldBuyItemBuyTotalResponse != null && goldBuyItemBuyTotalResponse.status != BaseResponse.ERROR)
		{
			buyItemTotalList.Clear();
			for(int i = 0; i < goldBuyItemBuyTotalResponse.data.item.Length; i++)
			{
				buyItemTotalList.Add(goldBuyItemBuyTotalResponse.data.item[i]);
//				ConsoleEx.DebugLog("data is null");
			}

//			if(goldBuyItemBuyTotalResponse.data != null && goldBuyItemBuyTotalResponse.data.Length > 0)
//			{
//				for(int i = 0; i < goldBuyItemBuyTotalResponse.data.item.Length; i++)
//				{
//					GoldBuyItemBuyTotalInfo goldBuyItemBuyTotalInfo = goldBuyItemBuyTotalResponse.data[i];
//					foreach(GoldBuyItemBuyTotalInfo goldBuyItemBuyTotalInfoTemp in buyItemTotalList)
//					{
//						if(goldBuyItemBuyTotalInfo.id == goldBuyItemBuyTotalInfoTemp.id)
//						{
//							goldBuyItemBuyTotalInfoTemp.used = goldBuyItemBuyTotalInfo.used;
//							break;
//						}
//					}
//				}
//			}

		}
	}

	public void qiangDuoBuyItem(int plistID)
	{
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.QIANGDUO_GOLD_BUY_ITEM, new QiangDuoGoldBuyItemParam(int.Parse(Core.Data.playerManager.PlayerID), plistID));

		task.afterCompleted = qiangDuoBuyItemCompleted;
		task.ErrorOccured = qiangDuoBuyItemError;

		task.DispatchToRealHandler ();
	}

	void qiangDuoBuyItemCompleted(BaseHttpRequest request, BaseResponse response)
	{
		if(response != null && response.status != BaseResponse.ERROR)
		{
			QiangDuoGoldBuyItemResponse buyitemResp = response as QiangDuoGoldBuyItemResponse;

			if(buyitemResp != null && buyitemResp.data != null)
			{

				SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(6034));
				if(qiangDuoGoldBuyItemCompletedDelegate != null)
				{
					qiangDuoGoldBuyItemCompletedDelegate(buyitemResp.data);
				}
			}
			DBUIController.mDBUIInstance.RefreshUserInfo();
		}
		else if(response != null && response.status == BaseResponse.ERROR)
		{
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getNetworkErrorString(response.errorCode));
//			if(response.errorCode == 4026) // 金币不足
//			{
////				UITooltip.ShowText(Core.Data.stringManager.getString(35006));
////				RED.Log(Core.Data.stringManager.getString(35006));
//				ConsoleEx.DebugLog(Core.Data.stringManager.getString(6028));
//				SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(6028));
//			}
//			else if(response.errorCode == 4025) // 掠夺积分不足
//			{
////				UITooltip.ShowText(Core.Data.stringManager.getString(35007));
////				RED.Log(Core.Data.stringManager.getString(35007));
//				ConsoleEx.DebugLog(Core.Data.stringManager.getString(6029));
//				SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(6029));
//			}
//			else if(response.errorCode == 4033) // 积分购买物品超过购买次数
//			{
//				SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(25096));
//			}
		}
	}

	public void qiangDuoBuyItemError(BaseHttpRequest request, string error)
	{

	}
}
