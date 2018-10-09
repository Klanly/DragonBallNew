using System;
using System.Collections.Generic;

public class GemsManager : Manager {

	//Key is Gems Number
	public Dictionary<int,GemData> ConfigData = null;
	//Key is Gems Id
	public Dictionary<int,Gems> BagOfGems = null;
    public Dictionary<int,Gems> sameGems = null;


	public GemsManager () {
		ConfigData = new Dictionary<int, GemData>();
		BagOfGems = new Dictionary<int, Gems>();
	}
	
	
    public override bool loadFromConfig () {

		bool result = base.readFromLocalConfigFile<GemData>(ConfigType.Gems, ConfigData);

		return result;
	}


	/// <summary>
	/// Sets the get gem data.
	/// </summary>
	/// <value>The get gem data.</value>
	public GemData getGemData (int num) {
		GemData data = null;
		if(!ConfigData.TryGetValue(num, out data)) {
			data = null;
		}
		return data;
	}

	#region Operation

	public Gems getGems (int id) {
		Gems data = null;
		if(!BagOfGems.TryGetValue(id, out data))
			data = null;
		return data;
	}
	
	public int getSameGemCount (Gems Gem)
	{
		int count = 0;
        sameGems = new Dictionary<int, Gems>();
		foreach(Gems gem in BagOfGems.Values)
		{
            if (gem.configData.color == Gem.configData.color && gem.configData.level == Gem.configData.level && !gem.equipped)
            {
                count++;
                sameGems.Add(gem.id,gem);
            }
		}
		return count;
	}
	
	
	/// <summary>
	/// Equips the gems. 装备上宝石
	/// </summary>
	/// <returns>The gems.</returns>
	/// <param name="id">Identifier.</param>
	public Gems equipGems (int id) {
		Gems mGem = getGems(id);
		if(mGem != null) mGem.equipped = true;
		return mGem;
	}

	/// <summary>
	/// Removes the gems. 卸下宝石
	/// </summary>
	/// <param name="g">The green component.</param>
	public void removeGems (Gems g) {
		if(g != null) {
			g.equipped = false;
		}
	}

	#endregion

	public override void fullfillByNetwork (BaseResponse response) {
		if(response != null && response.status != BaseResponse.ERROR) {
			LoginResponse loginResp = response as LoginResponse;

			if(loginResp != null && loginResp.data != null) {
				GemInfo[] gemList = loginResp.data.gems;
				if(gemList != null) {

                    //clear dirty data
                    BagOfGems.Clear();
					Core.Data.AccountMgr.clearBagStatus(ConfigDataType.Gems);

					GemData gd = null;
					foreach(GemInfo gi in gemList) {
						if(gi != null && ConfigData.TryGetValue(gi.num, out gd)) {
							Gems gm = new Gems (gd);
							gm.id = gi.id;
							BagOfGems.Add(gi.id,gm);

							gm.isNew = Core.Data.AccountMgr.getStatus (gm.id) == BagOfStatus.STATUS_NEW;
							//加入统计
							Core.Data.AccountMgr.analyseBag(ConfigDataType.Gems, gi.id);
						}
					}

				}
			}
		}
	}

	/// <summary>
	/// Uns the equip gems. 获取没有装备的宝石列表
	/// </summary>
	/// <returns>The equip gems.</returns>
	public List<Gems> UnEquipGemsByStar(short star) 
	{
		List<Gems> unEquipped = new List<Gems>();
		foreach(Gems g in BagOfGems.Values)
		{
			if (g != null && !g.equipped && g.configData.star == star)
			{
				unEquipped.Add (g);
			}
		}

		return unEquipped;
	}

	/// <summary>
	/// Uns the equip gems. 获取没有装备的宝石列表
	/// </summary>
	/// <returns>The equip gems.</returns>
	public List<Gems> GetGemsByStar(short star, SplitType type,int levellimit = 100) 
	{	
		List<Gems> list = new List<Gems>();
		foreach(Gems g in BagOfGems.Values)
		{
            if (g != null ){
                //      ConsoleEx.Write(" star ========== " +  g.configData.star,"blue" );
                if (g.configData != null)
                {
                    if (g.configData.star == star)
                    {
                        if (type == SplitType.Split_If_InTeam)
                        {
                            if (!g.equipped && g.configData.level < levellimit)
                                list.Add(g);
                        }
                        else
                        {
                            if (g.configData.level < levellimit)
                                list.Add(g);
                        }
                    }
                }
			}
		}

        //  ArrangeListByLevel(list);

        return   ArrangeListByLevel(list);
	}

    List<Gems> ArrangeListByLevel( List<Gems> gList){
        int starNum = 3;   // star   3~5
        List<Gems> starList3 = new List<Gems>();
        List<Gems> starList4 = new List<Gems>();
        List<Gems> starList5 = new List<Gems>();
        List<Gems> allList = new List<Gems>();
        foreach (Gems tG  in gList)
        { 
            if (tG.configData.star == 3)
            {
                starList3.Add(tG);
            }
            else if (tG.configData.star == 4)
            {
                starList4.Add(tG);
            }
            else if (tG.configData.star == 5)
            {
                starList5.Add(tG);
            }
        }
        starList3.Sort(SortByLevel);
        starList4.Sort(SortByLevel);
        starList5.Sort(SortByLevel);

        allList.AddRange(starList3);
        allList.AddRange(starList4);
        allList.AddRange(starList5);

        return allList;
    }
    static public int  SortByLevel(Gems tG_one,Gems tG_two){
        if (tG_two.equipped && !tG_one.equipped)
            return 1;
        else if (!tG_two.equipped && tG_one.equipped)
            return -1;
        else
        {
            if (tG_one.configData.level <= tG_two.configData.level)
            {
                return 1;
            }
            else
                return -1;
        }
    }

	public void Zhaomu (BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR) {
			ZhaoMuResponse resp = response as ZhaoMuResponse;
			if (resp.data.p != null) 
			{
				AddGems (resp.data.p);
			}
		}
	}
	
	/*从背包中筛选出当前星级，同时去除掉之前选择的要镶嵌的宝石
	 * */
	public List<Gems> GetGemsByStarAndAllLastSelected(short star) 
	{
		List<Gems> list = new List<Gems>();
		List<int> gem_list=FrogingSystem.ForgingRoomUI.Instance.InlaySystem.AllSelectedGems;
		foreach(Gems g in BagOfGems.Values)
		{
			if (g != null && g.configData.star == star)
			{
				bool find=false;
				foreach(int already in gem_list)
				{
					if(g.id==already)
					{
						find=true;
						break;
					}
				}
				if(!find)
					list.Add(g);
			}
		}
		
		return list;
	}
	
	
	/*过滤操作根据第一个选择的宝石筛选出可以和它合成的且未装备所有宝石
	 * */
	public List<Gems> GetGemByFirstSelectedAndStar(short star)
	{
		List<Gems> subGemList = new List<Gems>();
		Gems LGem= FrogingSystem.ForgingRoomUI.Instance.SyntheticSystem.Selected_Frist_GemData;
		foreach(Gems g in BagOfGems.Values)
		{
			if (g != null && !g.equipped && g.configData.star == star &&   g.configData.color==LGem.configData.color  && g.configData.level== LGem.configData.level && g.id != LGem.id && g.configData.level < 10)
			{
				//RED.Log("ID="+g.configData.ID+"    level="+g.configData.level.ToString());
				subGemList.Add (g);
			}
		}
		return subGemList;
	}

	/*删除一个宝石 (背包)
	 * */
	public void SellGem( BaseHttpRequest request, BaseResponse response)
	{
		if(response != null && response.status != BaseResponse.ERROR)
		{
			HttpRequest req = request as HttpRequest;
			SellEquipParam param = req.ParamMem as SellEquipParam;

			for(int i = 0; i < param.equips.Length; i++)
			{
				if(BagOfGems.ContainsKey(param.equips[i]))
				{
					int num = BagOfGems[param.equips[i]].configData.ID;
					BagOfGems.Remove(param.equips[i]);
					//删除统计
					Core.Data.AccountMgr.setStatus(new BagOfStatus(param.equips[i], num, BagOfStatus.STATUS_DELETE));
				}
			}
		}
	}

	public void UsePropSuc(BaseResponse response)
	{
		if(response != null && response.status != BaseResponse.ERROR)
		{
			UsePropResponse resp = response as UsePropResponse;
			if (resp != null && resp.data != null && resp.data.p != null)
			{
				AddGems (resp.data.p);
			}
		}
	}
	
	/*添加一个宝石 (背包)
	 * */

	public void AddGems(params ItemOfReward[] p)
	{
		if (p != null && p.Length > 0)
		{
			for (int i = 0; i < p.Length; i++)
			{
				if (p [i].getCurType () == ConfigDataType.Gems)
				{
					BagOfGems.Add (p [i].ppid, p [i].toGem (this));
					//加入统计
					Core.Data.AccountMgr.analyseBag(ConfigDataType.Gems, p[i].ppid);
				}
			}
		}
	}

    public void AddLevelGem(BaseResponse response){
        if (response != null && response.status != BaseResponse.ERROR) {
     
            GetLevelRewardResponse GLRResponse = response as GetLevelRewardResponse;
            if (GLRResponse != null) {
                AddGems (GLRResponse.data);
            }
        }
	}
	

	/// <summary>
	/// Euips the gems.获取装备了的宝石列表
	/// </summary>
	/// <returns>The gems.</returns>

	public List<Gems> EuipGems() {
		List<Gems> Equipped = new List<Gems>();
		foreach(Gems g in BagOfGems.Values){
			if(g != null && g.equipped)
				Equipped.Add(g);
		}

		return Equipped;
	}
	
	/*从服务器接收返回值,处理宝石合成
	 * */
	public void SyntheticGem(BaseHttpRequest req, BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR) 
		{
			GemSyntheitcResponse param = response as GemSyntheitcResponse;
			/*如果没有保底
			 * */
			if( param.data.succ ==1)
			{
				/*删除合成掉的宝石
				 * */
				if(BagOfGems.ContainsKey(param.data.delId)) {
					int num = BagOfGems[param.data.delId].configData.ID;
					BagOfGems.Remove(param.data.delId);
					//删除统计
					Core.Data.AccountMgr.setStatus(new BagOfStatus(param.data.delId, num, BagOfStatus.STATUS_DELETE));
				}
				    
				/*升级/降级宝石
				 * */
				if(BagOfGems.ContainsKey(param.data.upId))
				{
					GemData gd=null;
					ConfigData.TryGetValue(param.data.armNum, out gd);
					Gems gm = new Gems (gd);
					gm.id=param.data.upId;
					
					BagOfGems[param.data.upId]=gm;
				}
			}
			else
			{	
                if (BagOfGems.ContainsKey(param.data.delId))
                {
                    BagOfGems.Remove(param.data.delId);
                }			
			}

			Core.Data.playerManager.RTData.curStone+=param.data.stone;
			Core.Data.playerManager.RTData.curCoin+=param.data.coin;

			/*用掉宝石模具
			 * */			
			Item it= Core.Data.itemManager.GetBagItem(param.data.fpid);
			if(it!=null)
			{
				
				it.RtData.count  -= param.data.nm;		
				/*如果数量为0,直接从背包中删除
				 * */
				if(it.RtData.count<=0)
				{
					Core.Data.itemManager.RemoveBagItem(param.data.fpid);
				}
			}
			
		}
	}
	
	/*处理宝石兑换成功
	 * */
	public void ExchangeGem(BaseHttpRequest req, BaseResponse response)
	{
		if (response.status != BaseResponse.ERROR) 
		{		
			HttpRequest rq = req as HttpRequest;
			if (rq.Type == RequestType.GEM_EXCHANGE)
			{			
				GemExChangeResponse param = response as GemExChangeResponse;
				GemData gd=null;
				ConfigData.TryGetValue(param.data.pid, out gd);
				Gems gm = new Gems (gd);
				gm.id=param.data.ppid;
				if(BagOfGems.ContainsKey(param.data.ppid))
					BagOfGems[param.data.ppid]=gm;
				else
					BagOfGems.Add(param.data.ppid,gm);
				/*使用掉宝石精华
				 * */
				Item it= Core.Data.itemManager.GetBagItem(param.data.fpid);
				if(it!=null)
				{
					it.RtData.count-=param.data.nm;		
					/*如果数量为0,直接从背包中删除
					 * */
					if(it.RtData.count<=0)
					{
						Core.Data.itemManager.RemoveBagItem(param.data.fpid);
					}
				}
			}
		} 
	}
	
	
	/*将背包中的一个宝石标记为已穿戴
	 * */
	public void EuipOneGem(int id,bool isUp)
	{
		if(BagOfGems.ContainsKey(id))
		{
			BagOfGems[id].equipped=isUp;
		}
		else
		{
			RED.LogError("BagOfGems["+id+"]==null");
		}
	}

	//购买物品成功
	public void BuyItemSuc(BaseResponse response)
	{
		if (response.status != BaseResponse.ERROR)
		{
			BuyItemResponse resp = response as BuyItemResponse;
			if (resp != null && resp.data != null)
			{
				if (resp.data.Result != null && resp.data.Result.p != null)
				{
					AddGems (resp.data.Result.p);
				}
				else if (resp.data.p != null)
				{
					AddGems (resp.data.p);
				}
			}
			SecretShopBuyResponse secretshop = response as SecretShopBuyResponse;
			if(secretshop != null && secretshop.data != null && secretshop.data.p != null) 
			{
				AddGems(secretshop.data.p);
			}

			if (response is QiangDuoGoldBuyItemResponse) {
				QiangDuoGoldBuyItemResponse buyitemResp = response as QiangDuoGoldBuyItemResponse;
				if (buyitemResp != null && buyitemResp.data != null) 
				{
					AddGems (buyitemResp.data.p);
				}
			}
			else if (response is ZhanGongBuyItemResponse) {
				ZhanGongBuyItemResponse buyitemResp1= response as ZhanGongBuyItemResponse;
				if (buyitemResp1 != null && buyitemResp1.data != null) 
				{
					AddGems (buyitemResp1.data.p);
				}
			}
			else if (response is GetVipLevelRewardResponse) {
				GetVipLevelRewardResponse resp1 = response as GetVipLevelRewardResponse;
				if (resp1 != null && resp1.data != null) 
				{
					AddGems(resp1.data.p);
				}
			}
			else if (response is GetActivityLimittimeRewardResponse) {
				GetActivityLimittimeRewardResponse resp1 = response as GetActivityLimittimeRewardResponse;
				if (resp1 != null && resp1.data != null) 
				{
					AddGems(resp1.data.p);
				}
			}
		}
	}

	public List<Gems> GetNewGem()
	{
		List<Gems> list = new List<Gems> ();
		foreach (KeyValuePair<int, Gems> itor in BagOfGems)
		{
			if (itor.Value.isNew)
			{
				list.Add (itor.Value);
			}
		}
		return list;
	}

    public override void addItem (BaseResponse response)
    {
        if (response != null && response.status != BaseResponse.ERROR) {
            //Boss Battle or not

            SevenDaysBuyResponse seven = response as SevenDaysBuyResponse;
            if (seven != null && seven.data != null) {
                AddGems (seven.data.p);
            }

            GetLevelRewardResponse GLRResponse = response as GetLevelRewardResponse;
            if (GLRResponse != null)
            {
                AddGems (GLRResponse.data);
            }

            GetTresureResponse GTResponse = response as GetTresureResponse;
            if (GTResponse != null)
            {
                AddGems (GTResponse.data.p);
            }

        }
    }

	public void ClearData()
	{
		BagOfGems.Clear ();
		if(sameGems != null)
			sameGems.Clear ();
	}
}


