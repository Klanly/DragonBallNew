using UnityEngine;
using System.Collections;

public class UIDuiHuanCell : MonoBehaviour
{
	[HideInInspector]
	public ZhanGongItem duiHuanItem = null;

	[HideInInspector]
	public GoldBuyItemBuyTotalInfo Goldbuyitem = null;

	public UISprite m_ItemIcon;
	public UISprite m_TypeIcon;
	public UILabel m_Num;
	public UILabel m_Money;
	public UILabel m_NameLab;
	public UILabel m_Dec;
	public UILabel m_BtnName;
	public StarsUI m_StarUI;
	public UIButton buyButton;
	public UISprite m_Attr;
	public UILabel m_MoneySale;
	public UISprite m_SaleLine;

	public enum DuiHuanType
	{
		ZhanGongDuiHuanCell,
		JinBiDuiHuanCell
	}

	public DuiHuanType currentDuiHuanType = DuiHuanType.ZhanGongDuiHuanCell;

	void buyItme()
	{
       
		if(currentDuiHuanType == DuiHuanType.ZhanGongDuiHuanCell)
		{
			FinalTrialMgr.GetInstance().m_SelectDuihuancell = this;
			if(duiHuanItem.price == 0)
			{

				if(!Core.Data.DuiHuanManager.buyItemIDList.Contains(duiHuanItem))
				{
					Core.Data.DuiHuanManager.buyZhanGongItemCompletedDelegate = buyZhanGongItemCompleted;
					Core.Data.DuiHuanManager.buyZhanGongItem(duiHuanItem.id, duiHuanItem.rank);
				}
				else
				{
					Core.Data.DuiHuanManager.buyZhanGongItemCompletedDelegate = buyZhanGongItemCompleted;
					Core.Data.DuiHuanManager.buyZhanGongItem(duiHuanItem.id, duiHuanItem.rank);
				}
			}
			else
			{
				int _money = 0;
				if(duiHuanItem.discount != 0)
				{
					_money = duiHuanItem.discount;
				}
				else
				{
					_money = duiHuanItem.price;
				}
				if(FinalTrialMgr.GetInstance().Now_Zhangong >= _money)
				{
					Core.Data.DuiHuanManager.buyZhanGongItemCompletedDelegate = buyZhanGongItemCompleted;
					Core.Data.DuiHuanManager.buyZhanGongItem(duiHuanItem.id, duiHuanItem.rank);
				}
				else
				{
					SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(6018));
				}
			}
		}
		else if(currentDuiHuanType == DuiHuanType.JinBiDuiHuanCell)
		{
			int _money = 0;
			if(Goldbuyitem.discountjf != 0)
			{
				_money = Goldbuyitem.discountjf;
			}
			else
            {
				_money = Goldbuyitem.jifen;
            }
			if(FinalTrialMgr.GetInstance().TotalJifen < _money)
			{
				SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(6029));
			}
			else
			{
				Core.Data.DuiHuanManager.qiangDuoGoldBuyItemCompletedDelegate = qiangDuoGoldBuyItemCompleted;
				Core.Data.DuiHuanManager.qiangDuoBuyItem(Goldbuyitem.id);

			}
		}
	}

	public void changeButtonEnabled(UIButton button, bool isEnabled)
	{
		button.enabled = isEnabled;
		button.isEnabled = isEnabled;
	}

	public void qiangDuoGoldBuyItemCompleted(QiangDuoGoldBuyItemInfo qiangDuoGoldBuyItemInfo)
	{
		Core.Data.DuiHuanManager.qiangDuoGoldBuyItemCompletedDelegate = null;

//		Core.Data.playerManager.RTData.curCoin = Core.Data.playerManager.RTData.curCoin - qiangDuoGoldBuyItemInfo.coin;
		if(FinalTrialMgr.GetInstance().qiangDuoGoldOpponentsInfo != null)
		{
			FinalTrialMgr.GetInstance().qiangDuoGoldOpponentsInfo.status.score = FinalTrialMgr.GetInstance().qiangDuoGoldOpponentsInfo.status.score - qiangDuoGoldBuyItemInfo.score;
            
        }
	
		if(QiangDuoPanelScript.Instance != null)
		{
			QiangDuoPanelScript.Instance.duiHuanZhanGong.text = FinalTrialMgr.GetInstance().TotalJifen.ToString();
		}
	}

	public void buyZhanGongItemCompleted()
	{
		Core.Data.DuiHuanManager.buyZhanGongItemCompletedDelegate = null;
		if(duiHuanItem.price == 0)
		{
			changeButtonEnabled(this.buyButton, false);
            m_BtnName.text =Core.Data.stringManager.getString(20075); //yangchenguang 不能领取的时候 按钮字修改 为已经领取
		}
		else
		{
//			Core.Data.playerManager.RTData.curGlory = Core.Data.playerManager.RTData.curGlory - duiHuanItem.price;

//			if(QiangDuoPanelScript.Instance != null)
//			{
//				QiangDuoPanelScript.Instance.duiHuanZhanGong.text = Core.Data.playerManager.RTData.curGlory.ToString();
//			}

		}
	}
}
