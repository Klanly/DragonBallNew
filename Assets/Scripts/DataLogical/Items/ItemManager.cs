
using System;
using System.Collections;
using System.Collections.Generic;

public class ItemManager : Manager, ICore {
	#region ICore implementation

	public void Dispose ()
	{
		throw new System.NotImplementedException ();
	}

	public void Reset ()
	{
		throw new System.NotImplementedException ();
	}

	public void OnLogin (object obj)
	{
		throw new System.NotImplementedException ();
	}

	#endregion

	//key is Item number
	private readonly Dictionary<int, ItemData> mConfig = null;
	//b帮助分析的定义，key is type. 可以根据type来找到某一种类的物品, value(list<int>)是保存物品num
	private readonly Dictionary<int,List<int>> reverse = null;
	//b帮助分析的定义，key is type2. 可以根据type2来找到某一种类的物品, value(list<int>)是保存物品num
	private readonly Dictionary<int,List<int>> reverse2 = null;

	//背包里面的物品, key is ID
	private Dictionary<int, Item> BagOfItem = null;
	//背包里面的物品，key is num
	private Dictionary<int, List<Item>> BagOfItem2 = null;

	public const int CHAOSHENSHUI = 110032;				//超神水
	public const int WUXINGWAN = 110048;				//五行丸
	public const int MIANZHANPAI = 110049;				//免战牌
	public const int SKILL_CARD = 110089;				//技能券
	public const int SAODANG_CARD = 110090;				//扫荡券
	public const int HIGH_BATTLE_SOUL = 110084;			//战魂

	public const int SILVER_EXP_PIG = 110061;			//纯银乌龙
	public const int SILVER_PIG_PACKAGE = 110029;		//纯银乌龙包

    public const int  SILVER_GUN_ONE = 110041 ;         //白银棍*1
	public const int SILVER_GUN = 110044;				//白银棍 *10
    public const int SILVER_HUWAN_ONE = 110039;             //白银护腕*1
	public const int SILVER_HUWAN = 110046;				//白银护腕*10

	public const int COIN_PACKAGE = 110019;				//一袋金币
	public const int COIN_BOX = 110020;					//一箱金币
	public const int JINGLI = 110085;    //精力
	public const int JIFEN = 110185;      //积分

	public ItemManager() {
		//静态数据
		mConfig = new Dictionary<int, ItemData>();
		reverse = new Dictionary<int, List<int>>();
		reverse2 = new Dictionary<int, List<int>>();

		//服务来的数据
		BagOfItem = new Dictionary<int, Item>();
		BagOfItem2 = new Dictionary<int, List<Item>>();
	}

	#region 完成ovveride的方法
	public override bool loadFromConfig () {
		return base.readFromLocalConfigFile<ItemData> (ConfigType.Items, mConfig) & anaylize();
	}

	public override void fullfillByNetwork (BaseResponse response) {
		if(response != null && response.status != BaseResponse.ERROR) {
			LoginResponse loginResp = response as LoginResponse;
			if(loginResp != null && loginResp.data != null && loginResp.data.item != null) {

                //clear dirty data
                BagOfItem.Clear();
                BagOfItem2.Clear();
				Core.Data.AccountMgr.clearBagStatus(ConfigDataType.Item);

				ItemInfo[] items = loginResp.data.item;
				foreach(ItemInfo itinfo in items) 
				{
					if(itinfo != null) 
					{
						if(itinfo.num != JIFEN)
						{
							Utils.Assert(!mConfig.ContainsKey(itinfo.num),"Item Config file may be wrong. ID = " + itinfo.num);
							Item one = new Item(mConfig[itinfo.num], itinfo);
							BagOfItem.Add(itinfo.id, one);
							
							List<Item> itemList = null;
							if(BagOfItem2.TryGetValue(itinfo.num, out itemList)) 
								itemList.Add(one);
							else
								BagOfItem2.Add(itinfo.num, new List<Item>( new Item[] {one} ));
                            
                            one.isNew = Core.Data.AccountMgr.getStatus (one.RtData.id) == BagOfStatus.STATUS_NEW;
                            //加入分析统计
                            Core.Data.AccountMgr.analyseBag(ConfigDataType.Item, one.RtData.id);
						}

					}
				}
			}
		}
	}

	public override void addItem (BaseResponse response)
	{
		if(response != null && response.status != BaseResponse.ERROR)
		{
			BattleResponse battleResp = response as BattleResponse;
			if(battleResp != null && battleResp.data != null)
			{
				if (battleResp.data.reward != null)
				{
					addItem (battleResp.data.reward);
				}
				if (battleResp.data.ext != null)
				{
					addItem (battleResp.data.ext.p);
				}
			}

			NewFinalTrialFightResponse fightres = response as NewFinalTrialFightResponse;
			if(fightres != null && fightres.data!=null && fightres.data.rushResult != null && fightres.data.rushResult.award != null)
			{
				addItem (fightres.data.rushResult.award);
			}

			GetVipGiftResponse res = response as GetVipGiftResponse;
			if(res != null && res.data != null && res.data.award != null)
			{
				addItem(res.data.award);
			}

		}
	}

	public void addItemshop(BaseResponse response)
    {
        if(response != null && response.status != BaseResponse.ERROR)
        {
			if (response is BuyItemResponse) {
				BuyItemResponse buyitemResp = response as BuyItemResponse;
				if (buyitemResp != null && buyitemResp.data != null)
				{
					if(buyitemResp.data.ndProp != null && buyitemResp.data.ndProp.Length != 0)
					{
						int pid = Core.Data.itemManager.GetBagItemPid(buyitemResp.data.ndProp[0]);
						if(pid != -1)
						{
							Core.Data.itemManager.UseItem(pid, buyitemResp.data.ndProp[1]);
						}
						
					}
					if (buyitemResp.data.Result != null) {
						if (buyitemResp.data.Result.p != null) {
							addItem (buyitemResp.data.Result.p);
						}
					} else {
						addItem (buyitemResp.data.p);
					}
				}
			} else if (response is ZhanGongBuyItemResponse) {
				ZhanGongBuyItemResponse buyitemResp = response as ZhanGongBuyItemResponse;
				if (buyitemResp != null && buyitemResp.data != null) {
					addItem (buyitemResp.data.p);
//					foreach (ItemdataStruct ids in buyitemResp.data.p) {
//						AddItem (ids);
//					}
				}
			} else if (response is QiangDuoGoldBuyItemResponse) {
				QiangDuoGoldBuyItemResponse buyitemResp = response as QiangDuoGoldBuyItemResponse;
				if (buyitemResp != null && buyitemResp.data != null) {
					addItem (buyitemResp.data.p);
//					foreach (ItemdataStruct ids in buyitemResp.data.p) {
//						AddItem (ids);
//					}

				}
			} else if (response is SecretShopBuyResponse) {
				SecretShopBuyResponse buyitemResp = response as SecretShopBuyResponse;
				if (buyitemResp != null) {

					addItem (buyitemResp.data.p);
				}

			} else if (response is SevenDaysBuyResponse) {
				SevenDaysBuyResponse sevenbuy = response as SevenDaysBuyResponse;
				if (sevenbuy != null) {
					AddSevenDayRewardItem (sevenbuy);
				}
			} else if (response is UsePropResponse) {
				UsePropResponse propose = response as UsePropResponse;
				if (propose != null) {
					addItem (propose.data.p);
				}
			} else if (response is GetMailAttachmentResponse) {
				GetMailAttachmentResponse resp = response as GetMailAttachmentResponse;
				if (resp != null && resp.data != null) {
					for (int i = 0; i < resp.data.p.Length; i++)
						AddRewardToBag (resp.data.p [i]);
				}
			} else if (response is GetFirstChargeGiftResponse) {
				GetFirstChargeGiftResponse resp = response as GetFirstChargeGiftResponse;
				if (resp != null && resp.data != null) {
					for (int i = 0; i < resp.data.award.Length; i++)
						AddRewardToBag (resp.data.award [i]);
				}
			} else if (response is GetVipLevelRewardResponse) {
				GetVipLevelRewardResponse resp = response as GetVipLevelRewardResponse;
				if (resp != null && resp.data != null) {
					addItem (resp.data.p);
				}
			} else if (response is GetMonthGiftResponse) {
				GetMonthGiftResponse resp = response  as GetMonthGiftResponse;
				if (resp != null && resp.data != null) {
					addItem ( resp.data);
				}
			}else if (response is GetActivityLimittimeRewardResponse) {
				GetActivityLimittimeRewardResponse resp = response  as GetActivityLimittimeRewardResponse;
				if (resp != null && resp.data != null) {
					addItem ( resp.data.p);
				}
			}

        }
    }

//    public void AddKeyReward( BaseHttpRequest r,BaseResponse response){
//        if (response != null && response.status != BaseResponse.ERROR) {
//      
//            GetTresureResponse GTR = response as GetTresureResponse;
//            HttpRequest htReq = r as HttpRequest;
//            OpenTreasureBoxParam oTBP = htReq.ParamMem as OpenTreasureBoxParam;
//
//
//            if (GTR != null) { 
//                ConsoleEx.DebugLog (" ppid = "+GTR.data.p [0].ppid  +" state "+TreasureBoxController.doubleNum[oTBP.boxType -1] );
//                if (GTR.data.p [0].pid != 110060) {
//                    addItem (GTR.data.p);
//                    if(TreasureBoxController.doubleNum[oTBP.boxType - 1] >1)
//                        TreasureBoxController.doubleNum[oTBP.boxType - 1]--;
//                }
//                else {
//                    if (TreasureBoxController.doubleNum [oTBP.boxType - 1] == 0) {
//                        TreasureBoxController.doubleNum [oTBP.boxType - 1] += 2;
//                    } else if (TreasureBoxController.doubleNum [oTBP.boxType - 1] > 0) {
//                        TreasureBoxController.doubleNum [oTBP.boxType - 1]++;
//                    } else {
//                        TreasureBoxController.doubleNum [oTBP.boxType - 1] = 2;
//                    }
//                    //ActivityNetController.GetInstance ().GetTreasureBoxState ();
//                }
//            }
//        }
//    }

    public void UseKeyItem(BaseHttpRequest r, BaseResponse response){
        if (response != null && response.status != BaseResponse.ERROR) {
            HttpRequest htReq = r as HttpRequest;
            OpenTreasureBoxParam oTBP = htReq.ParamMem as OpenTreasureBoxParam;
            //数量 1
			int pid = 0;

			
            if (oTBP.boxType == 1) {
				pid = GetBagItemPid (110021);
            } else if (oTBP.boxType == 2) {
				pid = GetBagItemPid (110022);

            } else if (oTBP.boxType == 3) {
				pid = GetBagItemPid (110023);

            }
			UseItem (pid,1);
        }

    }

    
    public void UseItem(BaseHttpRequest r, BaseResponse response)
	{
		if(response != null && response.status != BaseResponse.ERROR) 
		{
			HttpRequest htReq = r as HttpRequest;
			UsePropParam req = htReq.ParamMem as UsePropParam;

            UseItem (req.propid, req.nm);
			UsePropResponse propResp = response as UsePropResponse;
			if (propResp.data.p != null) {

				for(int i=0;i< propResp.data.p.Length;i++){
					if(propResp.data.p[i].getCurType() == ConfigDataType.Item)
						AddRewardToBag(propResp.data.p[i]);	
				}
			}
		}
	}


	public Item GetBagItem(int itemId)
	{
		foreach(Item item in BagOfItem.Values)
		{
			if(item.RtData.id == itemId)
			{
				return item;
			}
		}
		return null;
	}


	public void Zhaomu(BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR) {
			ZhaoMuResponse resp = response as ZhaoMuResponse;
			if (resp.data.p != null) 
			{
				addItem (resp.data.p);
			}
		}
	}

	public void RemoveBagItem(int itemId)
	{
		if(BagOfItem.ContainsKey(itemId))
		{
			int num = BagOfItem[itemId].configData.ID;
			BagOfItem.Remove(itemId);
			//删除统计
			Core.Data.AccountMgr.setStatus(new BagOfStatus(itemId, num, BagOfStatus.STATUS_DELETE));
		}
	}
	
	#region Add by jc
	/*该方法可以通过物品编号得到物品的唯一id(如果背包里面有的话)
	   注意:如果遇到一种占一格的物品，只能返回第一个的唯一值,适用于可以叠加的物品
	 * */
	public int GetBagItemPid(int itemId)
	{
		foreach(Item item in BagOfItem.Values)
		{
			if(item.RtData.num == itemId)
			{
				return item.RtData.id;
			}
		}
		return -1;
	}
	#endregion

	//根据物品NUM得到物品个数
	public int GetBagItemCount(int itemNum)
	{
		foreach(Item item in BagOfItem.Values)
		{
			if(item.RtData.num == itemNum)
			{
				return item.RtData.count;
			}
		}
		return 0;
	}

	private void addItem(BattleReward reward)
	{
		Utils.Assert(reward == null, "We can't add empty list.");

		if(reward != null && reward.p != null)
		{
			this.addItem(reward.p);
		}
	}

	public void addItem(params ItemOfReward[] p)
	{
		if(p != null)
		{
			foreach(ItemOfReward ior in p)
			{
				ConfigDataType type = ior.getCurType();
				if(ior != null && type == ConfigDataType.Item) 
				{
					if(ior.showpid != 0)
					{
						ConfigDataType realType = DataCore.getDataType(ior.pid);
						switch(realType)
						{
							case ConfigDataType.Frag:
								Soul realSoul = new Soul();
								realSoul.m_RTData = new SoulInfo();
								realSoul.m_RTData.id = ior.ppid;
								realSoul.m_RTData.num = ior.pid;
								realSoul.m_RTData.count = ior.num;
								realSoul.m_config = Core.Data.soulManager.GetSoulConfigByNum(ior.pid);
								Core.Data.soulManager.AddSoul(realSoul);
								break;
							case ConfigDataType.Item:
								ItemData itemData = getItemData(ior.pid);
								if(itemData.type == (int)ItemType.Coin)
								{
									Core.Data.playerManager.RTData.curCoin += ior.num;
								}
							    else if(itemData.type == (int)ItemType.Stone)
								{
									Core.Data.playerManager.RTData.curStone += ior.num;
								}
								break;
						}
					}
					else
					{
						ItemData itemData = getItemData(ior.pid);
						if (itemData.type == (int)ItemType.Coin)
						{
							Core.Data.playerManager.RTData.curCoin += ior.num;
						}
						else if (itemData.type == (int)ItemType.Stone)
						{
							Core.Data.playerManager.RTData.curStone += ior.num;
						}
						else if (itemData.ID == JINGLI)
						{
							Core.Data.playerManager.RTData.curJingLi += ior.num;
						}
						else if(itemData.ID == JIFEN)
						{
							;
						}
						else 
						{
							Item item = ior.toItem (this);
							if(BagOfItem.ContainsKey(item.RtData.id))
							{
								BagOfItem[item.RtData.id].RtData.count += item.RtData.count;
								BagOfItem [item.RtData.id].isNew = true;
							} 
							else 
							{
								BagOfItem.Add(item.RtData.id, item);
							}
							List<Item> itemList = new List<Item>();
							if(BagOfItem2.TryGetValue(item.configData.ID, out itemList)) 
							{
								bool bFind = false;
								for (int i = 0; i < itemList.Count; i++) {
									if (itemList [i].RtData.id == item.RtData.id) {
										itemList [i].RtData.count = BagOfItem [item.RtData.id].RtData.count;
										bFind = true;
										break;
									}
								}

								if (!bFind) 
								{
									itemList.Add (item);
								}
							
							} 
							else
							{					
								BagOfItem2.Add(item.configData.ID, new List<Item>(new Item[]{item}) );
							}
							//加入分析统计
							Core.Data.AccountMgr.analyseBag(ConfigDataType.Item, item.RtData.id);

						}
					}
		
				}
			}
		}
	}

    private void AddItem(ItemdataStruct Itemdatastruct)
    {
        Utils.Assert(Itemdatastruct == null, "We can't add empty list."); 
        if(Itemdatastruct.pid >= 0)
        {                        

            ItemInfo itIf = new ItemInfo();
            itIf.id = Itemdatastruct.ppid;
            itIf.num = Itemdatastruct.pid;
            itIf.count = Itemdatastruct.num;
            
			if(Itemdatastruct.getCurType() == ConfigDataType.Item) {
				Item item = new Item(getItemData(Itemdatastruct.pid), itIf);
				
				if(BagOfItem.ContainsKey(item.RtData.id))
				{
					BagOfItem[item.RtData.id].RtData.count += Itemdatastruct.num;
					BagOfItem [item.RtData.id].isNew = true;
				} 
				else 
				{
					BagOfItem.Add(item.RtData.id, item);
				}
				
				List<Item> itemList = null;
				
				if(BagOfItem2.TryGetValue(item.configData.ID, out itemList))
				{
                    //因为上面的BagOfItem值已经修正了。
                    //这里就没必要做什么了。
				}
				else 
				{
					BagOfItem2.Add(item.configData.ID, new List<Item>(new Item[]{item}) );
				}

				//加入分析统计
				Core.Data.AccountMgr.analyseBag(ConfigDataType.Item, item.RtData.id);
			}
        }

    }

    public void addItem(ClientBTShaBuResponse resp) {
		if(resp != null && resp.data != null && resp.status != BaseResponse.ERROR) {

			if(resp.data.award != null) {
				foreach(ItemOfReward item in resp.data.award) {
					ConfigDataType type = DataCore.getDataType(item.pid);
					switch(type)
					{
					case ConfigDataType.Item:
						Core.Data.itemManager.addItem(item);
						break;
					case ConfigDataType.Monster:
						Monster monster = item.toMonster(Core.Data.monManager);
						if(monster!=null)
							Core.Data.monManager.AddMonter(monster);
						break;
					case ConfigDataType.Equip:
						Core.Data.EquipManager.AddEquip(item);
						break;
					case ConfigDataType.Gems:
						Core.Data.gemsManager.AddGems(item);
						break;
					case ConfigDataType.Frag:
						Soul soul = item.toSoul(Core.Data.soulManager);
						if(soul != null)
							Core.Data.soulManager.AddSoul(soul);
						break;
					}
				}
			}


		}
	
	}

	private void AddSevenDayRewardItem(SevenDaysBuyResponse res)
	{
		Utils.Assert(res == null, "We can't add empty list."); 
		if(res != null)
		{                        
			for(int j=0; j<res.data.p.Length; j++)
			{
				if (res !=  null && res.data != null && res.data.p[j] != null && DataCore.getDataType(res.data.p[j].pid) == ConfigDataType.Item)
				{
					ItemInfo itIf = new ItemInfo();
					itIf.id = res.data.p[j].ppid;
					itIf.num = res.data.p[j].pid;
					itIf.count = res.data.p[j].num;
					
					Item item = new Item(getItemData(res.data.p[j].pid), itIf);
                    if (res.data.p[j].pid == 110052)
                    {
                        Core.Data.playerManager.RTData.curStone += res.data.p[j].num;

                    }
                    else if(res.data.p[j].pid == 110051)
                    {
                        Core.Data.playerManager.RTData.curCoin += res.data.p[j].num;
                    }

					if (item != null && item.configData.ID != 110051 && item.configData.ID != 110052 && item.configData.ID != JIFEN)
                    {


                        if (BagOfItem.ContainsKey(item.RtData.id))
                        {
                            BagOfItem[item.RtData.id].RtData.count += res.data.p[j].num;
                            BagOfItem[item.RtData.id].isNew = true;
                        }
                        else
                        {
                            BagOfItem.Add(item.RtData.id, item);
                        }
                    
                        List<Item> itemList = null;
                    
                        if (BagOfItem2.TryGetValue(item.configData.ID, out itemList))
                        {
                            itemList.Add(item);
                        }
                        else
                        {
                            BagOfItem2.Add(item.configData.ID, new List<Item>(new Item[]{ item }));
                        }
                    }
				}
			}
	
		}
		
	}
    
    #endregion
    
    private bool anaylize() {
        
        foreach(ItemData id in mConfig.Values) {
			if(id != null) {

				List<int> result = null, result2 = null;

				//分析type
				if(reverse.TryGetValue(id.type, out result)) {
					result.Add(id.ID);
				} else {
					reverse.Add(id.type, new List<int>( new int[] {id.ID} ));
				}

				//分析type2
				if(id.type2 != null && id.type2.Length > 0) {
					foreach(short type in id.type2) {

						if(reverse2.TryGetValue((int)type, out result2)) {
							result2.Add(id.ID);
						} else {
							reverse2.Add((int)type, new List<int>( new int[] {id.ID} ) );
						}

					}
				}

			}
		}

		return true;
	}


	public ItemData getItemData (int num) 
	{
		if(mConfig.ContainsKey(num))
		{
			return mConfig[num];
		}

		RED.LogWarning(num + " item not find config data");
		return null;
	}

	#region 根据type or type2来查找数据

	public List<Item> getMianZhanPai() {
		List<Item> mItem = new List<Item>();
		List<Item> itemList = null;

        List<ItemData> items = getItemConfigList((int)ItemType.No_War, true);

		foreach(ItemData itemData in items)
		{
			if(BagOfItem2.TryGetValue(itemData.ID, out itemList)) 
			{
				mItem.AddRange(itemList);

			}
			else
			{
				Item item = new Item(itemData, new ItemInfo(0, itemData.ID, 0));
				mItem.Add(item);
			}
		}
		return mItem;
	}


	/// <summary>
	/// 根据type来找到每一个类型的静态数据ItemData, bReverse决定是使用reverse 还是 reverse2, 实际上就是一个是根据使用类型，一个是商店类型
	/// </summary>
	/// <returns>The item config list.</returns>
	/// <param name="type">Type.</param>
	public List<ItemData> getItemConfigList(int type, bool useType = true) {
		List<ItemData> items = new List<ItemData>();

		List<int> itemNum = null;
		Dictionary<int, List<int>> dic = null;
		if(useType)
			dic = reverse;
		else 
			dic = reverse2;

		if(dic.TryGetValue(type, out itemNum)) {

			ItemData id = null;
			foreach(int num in itemNum) {
				if(mConfig.TryGetValue(num, out id)) {
					items.Add(id);
				}
			}
		}

		return items;
	}

	/// <summary>
	/// 根据type来找到每一个类型的动态数据Item， bReverse决定是使用reverse 还是 reverse2
	/// </summary>
	/// <returns>The item list.</returns>
	/// <param name="type">Type.</param>
	public List<Item> getItemList(int type, bool bReverse = true) {
		List<Item> items = new List<Item>();

		List<int> itemNum = null;
		Dictionary<int, List<int>> dic = null;
		if(bReverse)
			dic = reverse;
		else 
			dic = reverse2;

		if(dic.TryGetValue(type, out itemNum)) {

			List<Item> itemList = null;
			foreach(int num in itemNum) {
				if(BagOfItem2.TryGetValue(num, out itemList)) {
					items.AddRange(itemList);
				}
			}

		}

		return items;
	}

	public List<Item> getAllItem() {
		List<Item> items = new List<Item>();
		foreach(Item it in BagOfItem.Values)
		{
			items.Add(it);
		}
		return items;
	}

	//added by zhangqiang 
	public List<Item> GetItemByStar(short star)
	{
		List<Item> items = new List<Item>();
		foreach(Item it in BagOfItem.Values)
		{
			if(it.configData.star == star)
			{
				items.Add(it);
			}
		}
		return items;
	}
		
    public List<ItemData> GetShopItem(ShopItemType mItemType)
    {
        List<ItemData> items = new List<ItemData>();
        ItemData chosen = null;
        List<int> shopNum = null;
        if(reverse2.TryGetValue((int)mItemType+1, out shopNum)) {
            foreach(int num in shopNum) {
                if(mConfig.TryGetValue(num, out chosen)) {
                    items.Add(chosen);
                }
            }
        }

        return items;
    }

	public void buyItem(Action<BaseHttpRequest, BaseResponse> afterCompleted, Action<BaseHttpRequest, string> ErrorOccured, ItemData mdata, int buyCount = 1)
	{
		int mNeedMoney = 0;
        if(mdata.price.Length != 0)
        {
            mNeedMoney = mdata.price[1];
        }
        if(mdata.price[0] == 0)
        {
            if(Core.Data.playerManager.Coin < mNeedMoney)
            {
				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(35000));
                return;
            }
        }
        else
        {
            if(Core.Data.playerManager.Stone < mNeedMoney)
            {
				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(35006));
                return;
            }
        }

        PurchaseParam param = new PurchaseParam();

        param.gid = Core.Data.playerManager.PlayerID;
        param.propid = mdata.ID;
		param.nm = buyCount;
        
        HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
        task.AppendCommonParam(RequestType.BUY_ITEM, param);
        

		task.afterCompleted += afterCompleted;
		task.ErrorOccured += ErrorOccured;

        
        //then you should dispatch to a real handler
        task.DispatchToRealHandler ();
	}
 
	#endregion

	//属性转换
	public void AttrSwap(BaseHttpRequest request, BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR)
		{
			AttrSwapResponse resp = response as AttrSwapResponse;
			UseItem (resp.data.prop, resp.data.propnm);
		}
	}

	//技能升级
	public void SkillUpgrade(BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR)
		{
			SkillUpgradeResponse resp = response as SkillUpgradeResponse;
			int pid = GetBagItemPid (SKILL_CARD);
			UseItem(pid, resp.data.scroll);

			Core.Data.playerManager.RTData.curCoin += resp.data.coin;
		}
	}


	//潜力训练
	public void QianLiXunLian(BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR)
		{
			QianLiXunLianResponse resp = response as QianLiXunLianResponse;
			UseItem (resp.data.prop, resp.data.propnm);
		}
	}

	public void UseItem(int itemID, int count)
	{
		Item prop = GetBagItem(itemID);
		if(prop != null)
		{
			prop.RtData.count -= count;
			if(prop.RtData.count <= 0)
			{
				BagOfItem.Remove(prop.RtData.id);
				//删除统计
				Core.Data.AccountMgr.setStatus(new BagOfStatus(prop.RtData.id, prop.configData.ID, BagOfStatus.STATUS_DELETE));
			}
		}
	}

	public void DecomposeMonster(BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR)
		{
			DecomposeMonsterResponse resp = response as DecomposeMonsterResponse;
			if(resp != null && resp.data != null)
			{
				addItem(resp.data.p);
			}
		}
	}

	public void EvolveMonster(BaseHttpRequest request, BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR)
		{
			HttpRequest req = request as HttpRequest;
			EvolveMonsterParam param = req.ParamMem as EvolveMonsterParam;
			Monster mon = Core.Data.monManager.getMonsterById (param.roleId);

			int soul = Core.Data.monManager.GetUpCostBattleSoul (mon.config.star, mon.Star);
			int pid = GetBagItemPid (HIGH_BATTLE_SOUL);
			UseItem (pid, soul);
		}
	}

    public void LevelUpDateItem(BaseResponse response){
        if (response != null && response.status != BaseResponse.ERROR)
        {
            GetLevelRewardResponse GLRResponse = response as GetLevelRewardResponse;
            if (GLRResponse != null)
            {
                Core.Data.itemManager.addItem(GLRResponse.data);
            }
        } 
    }

	public List<Item> GetNewItem()
	{
		List<Item> list = new List<Item> ();
		foreach (KeyValuePair<int, Item> itor in BagOfItem)
		{
			if (itor.Value.isNew)
			{
				list.Add (itor.Value);
			}
		}

		return list;
	}

	#region AddByJC
	public void AddRewardToBag(ItemOfReward item)
	{	
		ConfigDataType type = DataCore.getDataType(item.pid);
		switch(type)
		{
			case ConfigDataType.Item:
			{   
			    Core.Data.itemManager.addItem(item);
			    break;
			}		
			case ConfigDataType.Monster:
			{		 
			    Monster monster = item.toMonster(Core.Data.monManager);
			    if(monster!=null)
			    Core.Data.monManager.AddMonter(monster);
				break;
			}	
			case ConfigDataType.Equip:
			{
				Core.Data.EquipManager.AddEquip(item);
			    break;
			}			
			case ConfigDataType.Gems:
			{
			    Core.Data.gemsManager.AddGems(item);
			    break;
			}				
			case ConfigDataType.Frag:
			{
			    Soul soul = item.toSoul(Core.Data.soulManager);
			    if(soul != null)
			    Core.Data.soulManager.AddSoul(soul);
				break;
			}
		}
	}
	#endregion

	void ClearData()
	{
		BagOfItem.Clear();
		BagOfItem2.Clear();
	}

	public void ClearBagData()
	{
		ClearData ();
		Core.Data.monManager.ClearData ();
		Core.Data.EquipManager.ClearData ();
		Core.Data.gemsManager.ClearData ();
		Core.Data.soulManager.ClearData ();
	}
	
	
}
