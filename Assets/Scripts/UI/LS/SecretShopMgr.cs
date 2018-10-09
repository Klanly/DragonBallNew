using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SecretShopMgr 
{
	static SecretShopMgr mSecretShopMgr;
	public static SecretShopMgr GetInstance()
	{
		if(mSecretShopMgr == null)
		{
			mSecretShopMgr = new SecretShopMgr();
		}
		return mSecretShopMgr;
	}

	private SecretShopMgr ()
	{

	}

	UISecretShop _UISecretShop = null;
	public UISecretShopTag _UISecretShopTag = null;
	public UIDragonMallCell _UIDragonMallCell = null;

	public UISecretShopCell _UISecretShopCell = null;

	public SecretShopDataStruct m_data = null;


	public SecretShopBuyStatus _BuyStatus = SecretShopBuyStatus.Type_None;

	public int _ShopType;

	public int _RefreshMoney;
	public int _RefreshMoneyType;

	public int _purchaseSoulStone;

	public int TotalJifen = 0;

	void CreateSecretShop()
	{
		UnityEngine.Object obj = PrefabLoader.loadFromPack("LS/pbLSDragonBallSecretMall");
		if(obj != null)
		{
			GameObject go = RUIMonoBehaviour.Instantiate(obj) as GameObject;
			_UISecretShop = go.GetComponent<UISecretShop>();
			RED.AddChild(go.gameObject, DBUIController.mDBUIInstance._bottomRoot);
		}
	}
	
	public void SetSecretShop(bool key, int shoptype)
	{
		_ShopType = shoptype;
		if(_UISecretShop == null)
		{
			CreateSecretShop();
		}
//		_UISecretShop.gameObject.SetActive(false);
		SecretShopRequest();
	}

	void CreateSecretShopTag()
	{
		UnityEngine.Object obj = PrefabLoader.loadFromPack("LS/pbLSScrectShopTag");
		if(obj != null)
		{
			GameObject go = RUIMonoBehaviour.Instantiate(obj) as GameObject;
			_UISecretShopTag = go.GetComponent<UISecretShopTag>();
			RED.AddChild(go.gameObject, DBUIController.mDBUIInstance._TopRoot);
		}
	}
	

	public void SetSecretShopTag(bool key, SecretShopType mType, SecretShopBuyStatus _status, SecretShopDataStruct data, int index)
	{
		m_data = data;
		_BuyStatus = _status;
		if(_UISecretShopTag == null)
		{
			CreateSecretShopTag();
		}
		_UISecretShopTag.gameObject.SetActive(key);
		OpenTag( mType, index);
	}

	public void SetSecretShopTag(bool key, ItemData data, ShopItemType _type, int index)
	{
		if(_UISecretShopTag == null)
		{
			CreateSecretShopTag();
		}
		_UISecretShopTag.gameObject.SetActive(key);
		_UISecretShopTag.SetNormalDetail(data, _type, index);
	}

	public void  SetSecretShopTag(bool key, ActiveShopItem data, ShopItemType _type, int index)
	{
		if(_UISecretShopTag == null)
		{
			CreateSecretShopTag();
		}
		_UISecretShopTag.gameObject.SetActive(key);
		_UISecretShopTag.SetActiveDetail(data, _type, index);
    }

	public void SecretShopRequest()
	{
		SecretShopParam param = new SecretShopParam(int.Parse(Core.Data.playerManager.PlayerID), _ShopType);
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.SECRETSHOP, param);
		
		task.afterCompleted += SetSecretShopData;  
		task.DispatchToRealHandler();
		ComLoading.Open();
	}
	
	void SetSecretShopData(BaseHttpRequest request, BaseResponse response)
	{
		ComLoading.Close();
		if(response != null && response.status != BaseResponse.ERROR)
		{
			SecretShopResponse mresponse = response as SecretShopResponse;
			if(mresponse != null)
			{
				if(_ShopType == 1)TimerMgr(mresponse.data.Flush);
				if(mresponse.data != null && mresponse.data.refreshMoney != null && mresponse.data.refreshMoney.Length == 2)
				{
					_purchaseSoulStone = mresponse.data.purchaseSoulMoney;
					_RefreshMoneyType =  mresponse.data.refreshMoney[0];
					_RefreshMoney = mresponse.data.refreshMoney[1];
					TotalJifen = mresponse.data.jifen;
				}

				_UISecretShop.OnShow(mresponse, _ShopType);
//				_UISecretShop.gameObject.SetActive(true);
//				DBUIController.mDBUIInstance.HiddenFor3D_UI ();
            }
		}
		else if(response != null && response.status == BaseResponse.ERROR)
		{
			if(response.errorCode == 7041)
			{
				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(35012));
			}
			else if(response.errorCode == 7042)
			{
				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(35013));
			}
			else if(response.errorCode == 7043 || response.errorCode == 7044)
			{
				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(35014));
            }
			else if(response.errorCode == 3000)
			{
				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(35022));
			}
			else
			{
				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
			}
        }
	} 

	public void SecretSoulHeroRequest()
	{
		if(Core.Data.playerManager.Stone < _purchaseSoulStone)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(7310));
			return;
		}
		GetSecretSoulHeroParam param = new GetSecretSoulHeroParam(int.Parse(Core.Data.playerManager.PlayerID));
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.SECRETSHOP_BUYSOULHERO, param);
		
		task.afterCompleted += SetSecretShopBuyData;  
		task.DispatchToRealHandler();
		ComLoading.Open();
    }
    
    public void SecretShopBuyRequest()
	{
		if(m_data == null)
		{
			ConsoleEx.DebugLog("m_data is null");
			return;
		}
		if(m_data.money == null)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(20060));
			return;
		}
		if(_BuyStatus == SecretShopBuyStatus.Type_SellOut){
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(20066));
			return;
		}
		if(_BuyStatus == SecretShopBuyStatus.Type_Lock){
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(20067));
			return;
		}
		if(_BuyStatus == SecretShopBuyStatus.Type_None){
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(35000));


			return;
		}
		if(m_data.money.Length != 0 && m_data.money.Length == 2)
		{
			if(m_data.money[0] == 0)
			{
				if(Core.Data.playerManager.Stone < m_data.money[1])
				{
					SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(35006));
					return;
				}
                //talkingData add by wxl
                TDataOnBuyItem (m_data.id,m_data.num,m_data.money[1]);
			}
			else if(m_data.money[0] == 1)
			{
				if(Core.Data.playerManager.Coin < m_data.money[1])
				{

                    JCRestoreEnergyMsg.OpenUI(ItemManager.COIN_PACKAGE,ItemManager.COIN_BOX,2);
					return;
				}
			}
			else if(m_data.money[0] == 2)
			{
				if(TotalJifen < m_data.money[1])
				{
					SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(6029));
					return;
				}
			}
			else if(m_data.money[0] == 3)
			{
				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(7999));
				return;
			}
			else
			{
				int _battlesoul = 0;
				ItemData data = null;
				_battlesoul = Core.Data.itemManager.GetBagItemCount(m_data.money[0]);
				data = Core.Data.itemManager.getItemData(m_data.money[0]);
				
				if(_battlesoul < m_data.money[1])
				{
					if(data != null)
					{
						SQYAlertViewMove.CreateAlertViewMove(string.Format(Core.Data.stringManager.getString(25174), data.name));
					}
					return;
				}
            }
            
            
            SecretShopBuyParam param = new SecretShopBuyParam(int.Parse(Core.Data.playerManager.PlayerID),m_data.id, m_data.num, m_data.count, _ShopType);
			HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
			task.AppendCommonParam(RequestType.SECRETSHOP_BUY, param);

			task.afterCompleted += SetSecretShopBuyData;  
			task.DispatchToRealHandler();
           ComLoading.Open();
		}
	}
    //购买道具 花钻石
    void TDataOnBuyItem(int id,int num,int stoneNum){
        string itemName = "";
        switch (DataCore.getDataType (id)){
        case  ConfigDataType.Item:
            itemName =Core.Data.itemManager.getItemData (id).name ;
            break;
        case ConfigDataType.Frag:
            itemName = Core.Data.soulManager.GetSoulConfigByNum (id).name;
            break;
        case ConfigDataType.Equip:
            itemName = Core.Data.EquipManager.getEquipConfig (id).name;
            break;
        default:
            itemName = null;
            break;
        }
        if (string.IsNullOrEmpty (itemName) == false) {
             Core.Data.ActivityManager.OnPurchaseVirtualCurrency (itemName, num, stoneNum);
        }
    }

//    public void TimerMgr(long endTime)
//	{
//        long tempTime;
//        TimerTask task = new TimerTask(Core.TimerEng.curTime,endTime, 1,ThreadType.MainThread);
//		task.taskId = TaskID.SecretShop;
//		
//		task.onEventEnd += EndTimeHandleQphD;
//		task.onEvent += (TimerTask t) => { 
//            tempTime = t.leftTime;
//        };
//        
//        task.DispatchToRealHandler();
//	}


	void TimerMgr(long[] secrettime)
	{
//		int _index = 0;
//		
//        for(int i=0; i<secrettime.Length; i++)
//		{
//			if(secrettime[i] > Core.TimerEng.curTime)
//			{
//				_temp.Add(i,secrettime[i] - Core.TimerEng.curTime);
//			}
//		}

	}

	public void CloseSecretTime()
	{
		Core.TimerEng.deleteTask(TaskID.SecretShop);
    }

	void EndTimeHandleQphD(TimerTask task)
	{
		if(_UISecretShop != null)
		{
			_UISecretShop.Back_OnClick();
		}
	}

	void EndTimeHandle(TimerTask task)
	{
		SecretShopRequest();
	}
	
	void SetSecretShopBuyData(BaseHttpRequest request, BaseResponse response)
	{
		ComLoading.Close();
		if(response != null && response.status != BaseResponse.ERROR)
		{
			HttpRequest httprequest = request as HttpRequest;
            DBUIController.mDBUIInstance.RefreshUserInfo();
			SecretShopBuyResponse mresponse = response as SecretShopBuyResponse;
			if(mresponse != null)
			{
				if(mresponse.data.ndProp != null && mresponse.data.ndProp.Length != 0)
				{
					if(mresponse.data.ndProp[0] == 110185)
					{
						TotalJifen -= mresponse.data.ndProp[1];
						if(TotalJifen < 0)TotalJifen = 0;
						if(_ShopType == 1) _UISecretShop.m_CurJifenNum.SafeText(TotalJifen.ToString());
					}
					else
					{
						int pid = Core.Data.itemManager.GetBagItemPid(mresponse.data.ndProp[0]);
						if(pid != -1)
                        {
							Core.Data.itemManager.UseItem(pid, mresponse.data.ndProp[1]);
                        }
                    }
                    
                }
                if(httprequest.Act == HttpRequestFactory.ACTION_SECRETSHOP_BUYSOULHERO)
				{
					_UISecretShop._SoulRewardPanel.p = mresponse.data.p;
					if(!_UISecretShop._SoulRewardPanel.gameObject.activeInHierarchy)
					{
						_UISecretShop._SoulRewardPanel.gameObject.SetActive(true);

					}
					else
					{
						_UISecretShop._SoulRewardPanel.Reset();
					}
				}

				else
				{
					SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(6034));
					_UISecretShopCell.HideItemSellout();
                }

                
            }
            
            
        }
		else if(response != null && response.status == BaseResponse.ERROR)
		{
			if(response.errorCode == 6004)
			{
				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(20058));

			}
			else if(response.errorCode == 45000)
			{
				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(20059));
			}
			else if(response.errorCode == 7001)
			{
				SecretShopRequest();
			}
			else
			{
				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
			}

		}
	} 

	public void OpenTag(SecretShopType mType, int index)
	{
		string _name = "";
		string _dec = "";
		if(mType != SecretShopType.SecretShopType_Dec)
		{
			if(m_data == null)return;
			ConfigDataType type = DataCore.getDataType(m_data.num);
			switch(type)
			{
				case ConfigDataType.Monster:
					_name = Core.Data.monManager.getMonsterByNum(m_data.num).name;
					_dec = Core.Data.monManager.getMonsterByNum(m_data.num).description;
					break;
				case ConfigDataType.Item:
					_name = Core.Data.itemManager.getItemData(m_data.num).name;
					_dec = Core.Data.itemManager.getItemData(m_data.num).description;
					break;
				case ConfigDataType.Equip:
					_name = Core.Data.EquipManager.getEquipConfig(m_data.num).name;
					_dec = Core.Data.EquipManager.getEquipConfig(m_data.num).description;
					break;
				case ConfigDataType.Gems:
					_name = Core.Data.gemsManager.getGemData(m_data.num).name;
					_dec = Core.Data.gemsManager.getGemData(m_data.num).description;
					break;
				case ConfigDataType.Frag:
					_name = Core.Data.soulManager.GetSoulConfigByNum(m_data.num).name;
					_dec = Core.Data.soulManager.GetSoulConfigByNum(m_data.num).description;
					break;
				default:
					RED.LogError(" not found  : " +  m_data.num);
					break;
			}
		}

		_UISecretShopTag.SetDetail (_name, _dec, m_data.money, m_data,index);
    }
    
	public void ClearAllChoose()
	{
		if(_UISecretShop != null)
		{		
			_UISecretShop.ClearChoose();
		}

	}

	public void Clear()
	{
		_UISecretShopCell = null;
	}


    #region 刷新神秘商店 
    public void RefreshSecretShop()
	{
        RefreshSecretShopParam param = new RefreshSecretShopParam (int.Parse(Core.Data.playerManager.PlayerID),_ShopType);
        HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
        task.AppendCommonParam (RequestType.REFRESH_SECRETSHOP, param);
        task.afterCompleted += BackRefreshSecretShop;
        task.DispatchToRealHandler ();
		ComLoading.Open();
    }

    public void BackRefreshSecretShop(BaseHttpRequest request, BaseResponse response)
	{
		ComLoading.Close();
        if (response != null && response.status != BaseResponse.ERROR)
        {
            RefreshSecretShopResponse resp = response as RefreshSecretShopResponse;
            if (resp.data != null)
            {
                SecretShopResponse tResp = new SecretShopResponse();
                tResp.data = resp.data;
				TotalJifen = resp.data.jifen;
				_purchaseSoulStone = resp.data.purchaseSoulMoney;
				if(resp.data.refreshMoney != null && resp.data.refreshMoney.Length == 2)
				{					
					_RefreshMoneyType =  resp.data.refreshMoney[0];
					_RefreshMoney = resp.data.refreshMoney[1];
					if(_RefreshMoneyType == 0)Core.Data.playerManager.RTData.curStone -= _RefreshMoney;
					else if(_RefreshMoneyType == 1)Core.Data.playerManager.RTData.curCoin -= _RefreshMoney;
				}

                _UISecretShop.OnShow(tResp,_ShopType);
            }
        }
		else if(response != null && response.status == BaseResponse.ERROR)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
		}
    }
	#endregion 

	#region 新手引导购买局部处理
	public void testHttpResp_UI(BaseHttpRequest request, BaseResponse response)
	{
		ComLoading.Close ();
		if(response != null && response.status != BaseResponse.ERROR)
		{
			BuyItemSuc (request, response);
		}
	}

	void BuyItemSuc(BaseHttpRequest request, BaseResponse response)
	{
		BuyItemResponse resp = response as BuyItemResponse;
		if(resp.data == null)
		{
			return;
		}
		
		HttpRequest req = request as HttpRequest;
		PurchaseParam param = req.ParamMem as PurchaseParam;
		
		ItemOfReward[] rewards = null;
		if (resp.data.Result != null)
		{
			if(resp.data.Result.coin > 0)
			{
				string strText = Core.Data.stringManager.getString(5214);
				strText = string.Format(strText, resp.data.Result.coin, Core.Data.stringManager.getString(5037));
				SQYAlertViewMove.CreateAlertViewMove(strText);
				DBUIController.mDBUIInstance.RefreshUserInfo ();
			}
			else if(resp.data.Result.stone > 0)
			{
				string strText = Core.Data.stringManager.getString(5214);
				strText = string.Format(strText, resp.data.Result.stone, Core.Data.stringManager.getString(5070));
				SQYAlertViewMove.CreateAlertViewMove(strText);
				DBUIController.mDBUIInstance.RefreshUserInfo ();
			}
			else if(resp.data.Result.eny > 0)
			{
				string strText = Core.Data.stringManager.getString(5214);
				strText = string.Format(strText, resp.data.Result.eny, Core.Data.stringManager.getString(5038));
				SQYAlertViewMove.CreateAlertViewMove(strText);
				DBUIController.mDBUIInstance.RefreshUserInfo ();
			}
			else if(resp.data.Result.pwr > 0)
			{
				string strText = Core.Data.stringManager.getString(5214);
				strText = string.Format(strText, resp.data.Result.pwr, Core.Data.stringManager.getString(5039));
				SQYAlertViewMove.CreateAlertViewMove(strText);
				DBUIController.mDBUIInstance.RefreshUserInfo ();
			}
			
			if (resp.data.Result.p != null)
			{
				rewards = resp.data.Result.p;
				ShowRewards (param.propid, rewards);
			}
			
		}
		else
		{
			GetRewardSucUI.OpenUI(resp.data.p, Core.Data.stringManager.getString (5097));
			DBUIController.mDBUIInstance.RefreshUserInfo ();
		}
		
		
		SQYMainController.mInstance.UpdateBagTip ();
	}
	
	
	void ShowRewards(int srcID, ItemOfReward[] rewards)
	{
		if (rewards != null)
		{
			if (rewards.Length == 0)
			{
				RED.LogWarning ("get nothing!");
				return;
			}
			if (srcID == 110026)
			{
				ComLoading.Close();
				CradSystemFx.GetInstance ().SetCardPanel (rewards, 3150,  true, true, false);
				UIMiniPlayerController.Instance.SetActive(false);
			}
			else if((srcID < 110026 && srcID >= 110024) || srcID == 110092)
			{
				CradSystemFx.GetInstance ().SetCardSinglePanel (rewards,350,  true, true, false);
				UIMiniPlayerController.Instance.SetActive(false);
			}
			else
			{
				GetRewardSucUI.OpenUI (rewards, Core.Data.stringManager.getString (5097));
				DBUIController.mDBUIInstance.RefreshUserInfo ();
			}
		}
	}
	#endregion

}

public enum SecretShopType
{
	SecretShopType_None = 0,
	SecretShopType_Dec = 1,
	SecretShopType_Buy = 2,
	SecretShopType_NotEnough = 3,
	SecretShopType_NotEnough1 = 4,
	SecretShopType_NotEnoughvip = 5,
}

public enum SecretShopBuyStatus
{
	Type_None = 0,
	Type_Ok = 1,
	Type_SellOut = 2,
	Type_Lock = 3,
}
