using System;
using System.Collections.Generic;

public class EquipManager : Manager 
{
	//Key is Equip number
	private Dictionary<int, EquipData> ConfigEquip = null;
	//装备升级的配置信息
	private List<UpEquipLevel> ConfigUpEquipLv = null;
	//Key is Equip ID
	private Dictionary<int, Equipment> BagOfEquips = null;
	//Key is Equip star,这个是辅助BagOfEquips的读取, 每次新增或减少的时候，BagOfEquips & StarOfEquips
	private readonly Dictionary<int, List<int>> StarOfEquips = null;
    public const int  SilverRod_EXP =  40998 ;  // 纯银棍
    public const int  SilverBracers_EXP =  45998 ; //纯银护腕


	public EquipManager () {
		//Read from config file and then to fullfill them
		ConfigEquip = new Dictionary<int, EquipData>();
		ConfigUpEquipLv = new List<UpEquipLevel>();

		//Equips of bag
		BagOfEquips = new Dictionary<int, Equipment>();
		StarOfEquips = new Dictionary<int, List<int>>();
	}

	public override bool loadFromConfig () {
		return base.readFromLocalConfigFile<EquipData>(ConfigType.Equipment, ConfigEquip) &&
		       base.readFromLocalConfigFile<UpEquipLevel>(ConfigType.UpEquip, ConfigUpEquipLv);
	}
    public bool IsExpEquipment (int id )
    {
        if (id == SilverRod_EXP || id == SilverBracers_EXP)
        {
            return true ;

        }
        return false ;
    }
	/// <summary>
	/// Gets the equip config.获得装给信息
	/// </summary>
	/// <returns>The equip config.</returns>
	/// <param name="num">equip Number.</param>
	public EquipData getEquipConfig(int num) {
		EquipData eqData = null;
		if(!ConfigEquip.TryGetValue(num, out eqData)){
			eqData = null;
		}
		return eqData;
	}

	//根据id获取装备
	public Equipment getEquipment(int id) {
		Equipment eq = null;
		if(!BagOfEquips.TryGetValue(id, out eq)) {
			eq = null;
		}
		return eq;
	}

	/// <summary>
	/// Fullfills the by network. 根据网络获取数据并填充好背包。
	/// </summary>
	/// <param name="response">Response.</param>
	public override void fullfillByNetwork (BaseResponse response) {
		if(response != null && response.status != BaseResponse.ERROR) {
			LoginResponse logResp = response as LoginResponse;
			if(logResp != null && logResp.data != null && logResp.data.eqip != null) {

                //clear dirty data
                BagOfEquips.Clear();
                StarOfEquips.Clear();
				Core.Data.AccountMgr.clearBagStatus(ConfigDataType.Equip);

				List<int> helper = null;

				EquipInfo[] equipList = logResp.data.eqip;
				int length = equipList.Length;

				for(int i = 0; i < length ; ++i)
				{
                    EquipData ed = getEquipConfig(equipList[i].num);
                    if(ed == null) continue;
					Equipment one = new Equipment(equipList[i], ed, Core.Data.gemsManager);
					BagOfEquips.Add(one.RtEquip.id, one);

					if(StarOfEquips.TryGetValue(one.ConfigEquip.star, out helper)) {
						helper.Add(one.RtEquip.id);
					} else {
						StarOfEquips.Add(one.ConfigEquip.star, new List<int>( new int[]{one.RtEquip.id} ));
					}

					one.isNew = Core.Data.AccountMgr.getStatus (one.RtEquip.id) == BagOfStatus.STATUS_NEW;
					//加入统计
					Core.Data.AccountMgr.analyseBag(ConfigDataType.Equip, one.RtEquip.id);
				}
			}

		}
	}

	public override void addItem (BaseResponse response) {
		if(response != null && response.status != BaseResponse.ERROR) {
			BattleResponse battleResp = response as BattleResponse;
			if(battleResp != null && battleResp.data != null)
			{
				if (battleResp.data.reward != null)
				{
					addEquipment (battleResp.data.reward);
				}
				if (battleResp.data.ext != null)
				{
					AddEquip (battleResp.data.ext.p);
				}
			}
				
            GetLevelRewardResponse GLRResponse = response as GetLevelRewardResponse;
            if (GLRResponse != null)
            {
				AddEquip (GLRResponse.data);
            }

			NewFinalTrialFightResponse fightres = response as NewFinalTrialFightResponse;
			if(fightres != null && fightres.data!=null && fightres.data.rushResult != null && fightres.data.rushResult.award != null)
			{
				AddEquip (fightres.data.rushResult.award);
			}

            SockBuyItemResponse buyItem = response as SockBuyItemResponse;
            if (buyItem != null) {
                if (buyItem.data.retCode == 1) {
                    ItemOfReward[] tReward = new ItemOfReward[1]{buyItem.data.p};
                    AddEquip (tReward);
                }
            }

			UsePropResponse propose = response as UsePropResponse;
			if(propose != null && propose.data != null && propose.data.p != null)
			{
				AddEquip(propose.data.p);
			}


            SevenDaysBuyResponse seven = response as SevenDaysBuyResponse;
            if (seven != null && seven.data != null) {
                AddEquip (seven.data.p);
            }

            GetTresureResponse GTResponse = response as GetTresureResponse;
            if (GTResponse != null)
            {
                AddEquip (GTResponse.data.p);
            }
		}
	}

	public void AddShopItem(BaseResponse response)
	{
		if(response != null && response.status != BaseResponse.ERROR) 
		{
			if(response is SecretShopBuyResponse){
			SecretShopBuyResponse secretshop = response as SecretShopBuyResponse;
			if(secretshop != null && secretshop.data != null && secretshop.data.p != null)
			{
				AddEquip (secretshop.data.p);
			}
			}

			if (response is QiangDuoGoldBuyItemResponse) {
			QiangDuoGoldBuyItemResponse buyitemResp = response as QiangDuoGoldBuyItemResponse;
			if (buyitemResp != null && buyitemResp.data != null) 
			{
					AddEquip (buyitemResp.data.p);
			}
			}
			else if (response is ZhanGongBuyItemResponse) {
			ZhanGongBuyItemResponse buyitemResp1= response as ZhanGongBuyItemResponse;
			if (buyitemResp1 != null && buyitemResp1.data != null) 
			{
				AddEquip (buyitemResp1.data.p);
         	}
			}
			else if (response is GetVipLevelRewardResponse) {
				GetVipLevelRewardResponse resp = response as GetVipLevelRewardResponse;
				if (resp != null && resp.data != null) 
				{
					AddEquip(resp.data.p);
				}
			}
			else if(response is UsePropResponse)
			{
				UsePropResponse resp = response as UsePropResponse;
				if (resp != null && resp.data != null && resp.data.p != null)
                {
					AddEquip (resp.data.p);
                }
			}
			else if(response is GetActivityLimittimeRewardResponse)
			{
				GetActivityLimittimeRewardResponse resp = response as GetActivityLimittimeRewardResponse;
				if (resp != null && resp.data != null && resp.data.p != null)
				{
					AddEquip (resp.data.p);
				}
			}
		}
		if(response != null && response.status != BaseResponse.ERROR) 
		{
			BuyItemResponse buyres = response as BuyItemResponse;
			if(buyres != null && buyres.data != null)
			{
				if (buyres.data.Result != null && buyres.data.Result.p != null)
				{
					AddEquip (buyres.data.Result.p);
				}
				else if(buyres.data.p != null)
				{
					AddEquip (buyres.data.p);
				}
            }
        }
    }
		

	private void addEquipment(BattleReward reward) 
	{
		Utils.Assert(reward == null, "We can't add empty list.");

		if(reward != null && reward.p != null)
		{
			AddEquip (reward.p);
		}
	}

	public void ZhaomuEquip(BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR)
		{
			ZhaoMuResponse resp = response as ZhaoMuResponse;
			if (resp.data != null)
			{
				AddEquip (resp.data.p);
			}
		}
	}

	public void AddEquip(params ItemOfReward[] p) {
		if (p == null) {
			return;
		}
		List<int> helper = null;
		foreach(ItemOfReward ior in p)
		{
			if(ior != null && ior.getCurType() == ConfigDataType.Equip)
			{
				//Add Equipment
				Equipment ep = ior.toEquipment(this, Core.Data.gemsManager);

				if (BagOfEquips.ContainsKey (ep.ID))
				{
					BagOfEquips [ep.ID] = ep;
				}
				else
				{
					BagOfEquips.Add (ep.ID, ep);
				}

				if(StarOfEquips.TryGetValue(ep.ConfigEquip.star, out helper))
				{
					helper.Add(ep.ID);
				} 
				else 
				{
					StarOfEquips.Add(ep.ConfigEquip.star, new List<int>( new int[] {ep.ID} ));
				}
				//加入统计
				Core.Data.AccountMgr.analyseBag(ConfigDataType.Equip, ep.RtEquip.id);
			}
		}
	}

	//设定分页大小
	public int PageSize {
		get;set;
	}

	//返回总的页数
	public int TotalPage (short star, SplitType type = SplitType.None) {

		if(PageSize == 0)
			return 0;
		else {
			if(StarOfEquips.ContainsKey(star)) {
				switch(type) {
				case SplitType.Split_If_InCurTeam:
					return (int)Math.Ceiling( (StarOfEquips[star].Count - Core.Data.playerManager.RTData.curTeam.equipSplitByStar(star)) * 1.0f / PageSize);
				case SplitType.Split_If_InTeam:
					return (int)Math.Ceiling( (StarOfEquips[star].Count - Core.Data.playerManager.RTData.EquipSplitByStar(star) )* 1.0f / PageSize);
				case SplitType.None:
					return (int)Math.Ceiling( StarOfEquips[star].Count * 1.0f / PageSize);
				default:
					return (int)Math.Ceiling( StarOfEquips[star].Count * 1.0f / PageSize);
				}
			}
			else
				return 0;
		}

	}

	/// <summary>
	/// Gets the Equipment list by star. 不要经常调用该方法
	/// </summary>
	/// <returns>The monster list by star.</returns>
	/// <param name="star">Star.</param>
	public List<Equipment> getEquipListByStar(short star, SplitType type = SplitType.None , bool isHaveSolt = false)
	{
		List<Equipment> stars = new List<Equipment>();
		List<Equipment> list = new List<Equipment>();
		if(StarOfEquips.ContainsKey(star))
		{
			foreach(int monId in StarOfEquips[star])
			{
				Equipment eq = getEquipment(monId);
				if(eq != null)
				{
					switch(type) 
					{
					case SplitType.None:
						if(isHaveSolt)
						{
							if(eq.RtEquip.SlotCount > 0)
								stars.Add(eq);
						}
						else
						   stars.Add(eq);
						break;
					case SplitType.Split_If_InCurTeam:
						if(!eq.equipped) 
						{
							stars.Add(eq);
						} 
						else
						{
							if(!Core.Data.playerManager.RTData.curTeam.equipInMyTeam(eq.ID)) {
								stars.Add(eq);
							}
						}
						break;
					case SplitType.Split_If_InTeam:
						if(!eq.equipped)
							stars.Add(eq);
						break;
					}

				}
			}

			stars.Sort(new EquipIDCompare());

			for(int i = 0; i < stars.Count; i++)
			{
				if(stars[i].equipped)
				{
					list.Add(stars[i]);
				}
			}

			for(int i = 0; i < stars.Count; i++)
			{
				if(!stars[i].equipped)
				{
					list.Add(stars[i]);
				}
			}
		}

		return list;
	}
	
	//	-1 : all    0: atk   1:def
	public List<Equipment> GetEquipByStar(short star, int equipType = -1, SplitType type = SplitType.None)
	{
		List<Equipment> stars = new List<Equipment>();
		List<Equipment> list = new List<Equipment>();
		if(StarOfEquips.ContainsKey(star))
		{
			foreach(int monId in StarOfEquips[star])
			{
				Equipment eq = getEquipment(monId);
				if(eq != null)
				{
//					if(eq.RtEquip.EquipedGemCount <= 0)
//					{
						switch(type) 
						{
							case SplitType.None:
								if(equipType == -1)
									stars.Add(eq);
								else if(equipType == eq.ConfigEquip.type)
									stars.Add(eq);
								break;
							case SplitType.Split_If_InCurTeam:
								if(!eq.equipped) 
								{
									if(equipType == -1)
										stars.Add(eq);
									else if(equipType == eq.ConfigEquip.type)
										stars.Add(eq);
								} 
								else
								{
									if(!Core.Data.playerManager.RTData.curTeam.equipInMyTeam(eq.ID)) 
									{
										if(equipType == -1)
											stars.Add(eq);
										else if(equipType == eq.ConfigEquip.type)
											stars.Add(eq);
									}
								}
								break;
							case SplitType.Split_If_InTeam:
								if(!eq.equipped)
								{
									if(equipType == -1)
										stars.Add(eq);
									else if(equipType == eq.ConfigEquip.type)
										stars.Add(eq);
								}
								break;
						}
//					}
				}
			}
			
			stars.Sort(new EquipIDCompare());
			
			for(int i = 0; i < stars.Count; i++)
			{
				if(stars[i].equipped)
				{
					list.Add(stars[i]);
				}
			}
			
			for(int i = 0; i < stars.Count; i++)
			{
				if(!stars[i].equipped)
				{
					list.Add(stars[i]);
				}
			}
		}
		return list;
	}

	//合成
	public void SoulHeCheng(BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR)
		{
			SoulHeChenResponse resp = response as SoulHeChenResponse;
			if (resp.data != null)
			{
				AddEquip (resp.data);
			}
		}
	}

	//得到装备总个数
	public int GetEquipCount()
	{
		return BagOfEquips.Count;
	}

	//得到攻击装备
	public List<Equipment> getAtkEquipByStar(short star, SplitType type = SplitType.None, bool equipedGem = false)
	{
		List<Equipment> stars = new List<Equipment>();

		if(StarOfEquips.ContainsKey(star)) {
			foreach(int monId in StarOfEquips[star]) {
				Equipment eq = getEquipment(monId);
				if(eq != null && eq.ConfigEquip.type == 0)
				{
					switch(type) {
						case SplitType.None:
							if(equipedGem)
							{
								if(eq.RtEquip.EquipedGemCount > 0)
								{
									stars.Add(eq);
								}
							}
							else
							{
								stars.Add(eq);
							}
							break;
						case SplitType.Split_If_InCurTeam:
							if(!eq.equipped) 
							{
								if(equipedGem)
								{
									if(eq.RtEquip.EquipedGemCount > 0)
									{
										stars.Add(eq);
									}
								}
								else
								{
									stars.Add(eq);
								}
							}
							else
							{
								if(!Core.Data.playerManager.RTData.curTeam.equipInMyTeam(eq.ID))
								{
									if(equipedGem)
									{
										if(eq.RtEquip.EquipedGemCount > 0)
										{
											stars.Add(eq);
										}
									}
									else
									{
										stars.Add(eq);
									}
								}
							}
							break;
						case SplitType.Split_If_InTeam:
							if(!eq.equipped)
							{
								if(equipedGem)
								{
									if(eq.RtEquip.EquipedGemCount > 0)
									{
										stars.Add(eq);
									}
								}
								else
								{
									stars.Add(eq);
								}
							}
							break;
					}

				}
			}

			stars.Sort(new EquipIDCompare());
		}

		return stars;
	}

	//type 0: 攻击装备  1：防守装备
	public List<Equipment> GetEquipList(int type, SplitType splitType = SplitType.None)
	{
		List<Equipment> list = new List<Equipment> ();
		foreach (Equipment equip in BagOfEquips.Values)
		{
			if (equip.ConfigEquip.type == type)
			{
				switch(splitType) 
				{
					case SplitType.None:
						list.Add (equip);
						break;
					case SplitType.Split_If_InCurTeam:
						if(!equip.equipped) 
						{
							list.Add(equip);
						} 
						else 
						{
							if(!Core.Data.playerManager.RTData.curTeam.equipInMyTeam(equip.ID))
							{
								list.Add(equip);
							}
						}
						break;
					case SplitType.Split_If_InTeam:
						if(!equip.equipped)
							list.Add(equip);
						break;
				}
			}
		}
		return list;
	}

	//得到防御装备
	public List<Equipment> getDefEquipByStar(short star, SplitType type = SplitType.None, bool equipedGem = false)
	{
		List<Equipment> stars = new List<Equipment>();

		if(StarOfEquips.ContainsKey(star)) 
		{
			foreach(int monId in StarOfEquips[star])
			{
				Equipment eq = getEquipment(monId);
				if(eq != null && eq.ConfigEquip.type == 1) 
				{
					switch(type) 
					{
						case SplitType.None:
							if(equipedGem)
							{
								if(eq.RtEquip.EquipedGemCount > 0)
								{
									stars.Add(eq);
								}
							}
							else
							{
								stars.Add(eq);
							}
							break;
						case SplitType.Split_If_InCurTeam:
							if(!eq.equipped) 
							{
								if(equipedGem)
								{
									if(eq.RtEquip.EquipedGemCount > 0)
									{
										stars.Add(eq);
									}
								}
								else
								{
									stars.Add(eq);
								}
							} 
							else 
							{
								if(!Core.Data.playerManager.RTData.curTeam.equipInMyTeam(eq.RtEquip.id))
								{
									if(equipedGem)
									{
										if(eq.RtEquip.EquipedGemCount > 0)
										{
											stars.Add(eq);
										}
									}
									else
									{
										stars.Add(eq);
									}
								}
							}
							break;
						case SplitType.Split_If_InTeam:
							if(!eq.equipped)
							{
								if(equipedGem)
								{
									if(eq.RtEquip.EquipedGemCount > 0)
									{
										stars.Add(eq);
									}
								}
								else
								{
									stars.Add(eq);
								}
							}
							break;
					}

				}
			}

			stars.Sort(new EquipIDCompare());
		}

		return stars;
	}


	/// <summary>
	/// Gets the un equipped item.获得没有装备的装备, 这些会在PlayerManager里的队伍里，填充好（分析某个装备是否穿上)。
	/// </summary>
	/// <returns>The un equipped item.</returns>
	public List<Equipment> getUnEquippedItem() {
		List<Equipment> unEuipped = new List<Equipment>();

		foreach(Equipment eq in BagOfEquips.Values) {
			if(eq != null && !eq.equipped) {
				unEuipped.Add(eq);
			}
		}
		return unEuipped;
	}

	/// <summary>
	/// Gets the equipped item. 获取装了的装备
	/// </summary>
	/// <returns>The equipped item.</returns>
	public List<Equipment> getEquippedItem() {
		List<Equipment> Euipped = new List<Equipment>();

		foreach(Equipment eq in BagOfEquips.Values) {
			if(eq != null && eq.equipped) {
				Euipped.Add(eq);
			}
		}
		return Euipped;
	}



	//added by zhangqiang at 2013-03-07
	// strengthen monster
	public void StrengthEquip(BaseHttpRequest request, BaseResponse response)
	{
		ConsoleEx.Write("receive strengthen Equip message Sucess");
		Utils.Assert(request == null || response == null, "Parameter can't be null.");
		if(response.status != BaseResponse.ERROR) 
		{
			if(request.baseType == BaseHttpRequestType.Common_Http_Request)
			{
				HttpRequest htReq = request as HttpRequest;
				StrengthEquipParam param = htReq.ParamMem as StrengthEquipParam;

				StrengthEquipResponse htResp = response as StrengthEquipResponse;

				if(param != null) 
				{
					//remove all equip
					int[] SoldIds = param.para;
					DelEquipByIds(SoldIds);
					
					//level up target monster
					Equipment equip = getEquipment(param.seqid);
					if(equip != null)
					{
						equip.RtEquip.exp = htResp.data.ep;
						equip.RtEquip.lv = (short)htResp.data.lv;
					}
				}
			}
		}

		Core.Data.playerManager.RTData.curTeam.upgradeMember ();
	}


	public void UserProp(BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR)
		{
			UsePropResponse resp = response as UsePropResponse;
			if (resp.data != null && resp.data.p != null)
			{
				//Add Equipment
				List<int> helper = null;
				for(int i = 0; i < resp.data.p.Length; i++)
				{
					if(resp.data.p[i].getCurType() != ConfigDataType.Equip)
					{
						continue;
					}
					Equipment ep = resp.data.p[i].toEquipment(Core.Data.EquipManager, Core.Data.gemsManager);

					RED.Log ("ep.ID   " + ep.ID);
					if (BagOfEquips.ContainsKey (ep.RtEquip.id))
					{
						BagOfEquips[ep.RtEquip.id] = ep;
					}
					else
					{
						BagOfEquips.Add(ep.RtEquip.id, ep);
					}

					
					if(StarOfEquips.TryGetValue(ep.ConfigEquip.star, out helper)) {
						helper.Add(ep.ID);
					} else {
						StarOfEquips.Add(ep.ConfigEquip.star, new List<int>( new int[] {ep.ID} ));
					}
				}

			}
		}
	}
		
	//remove equip
	public void DelEquipByIds(int[] roles)
	{
		if(roles == null)
		{
			return;
		}

		for(int i = 0; i < roles.Length; i++)
		{
			DelEquipById(roles[i]);
		}
	}

	public void DelEquipById(int equipId)
	{
		Equipment equip = null;
		if(BagOfEquips.TryGetValue(equipId, out equip))
		{
			foreach(EquipSlot slot in equip.RtEquip.slot)
			{
				if (slot != null && slot.mGem != null)
				{
					Core.Data.gemsManager.removeGems (slot.mGem);
					slot.mGem = null;
				}
			}

			for (int i = 0; i < equip.RtEquip.slt.Length; i++)
			{
				equip.RtEquip.slt [i] = 0;
			}

			List<int> list = null;
			if(StarOfEquips.TryGetValue(equip.ConfigEquip.star, out list))
			{
				list.Remove(equipId);
			}

			BagOfEquips.Remove(equipId);
			//删除统计
			Core.Data.AccountMgr.setStatus(new BagOfStatus(equipId, equip.ConfigEquip.ID, BagOfStatus.STATUS_DELETE));
		}
	}


	public void SellEquip(BaseHttpRequest request, BaseResponse response)
	{
		ConsoleEx.Write("receive sell Equip message Sucess");
		Utils.Assert(request == null || response == null, "Parameter can't be null.");
		if(response.status != BaseResponse.ERROR) 
		{
			if(request.baseType == BaseHttpRequestType.Common_Http_Request)
			{
				HttpRequest htReq = request as HttpRequest;
				SellEquipParam param = htReq.ParamMem as SellEquipParam;

				if(param != null) 
				{
					//remove all equip
					int[] SoldIds = param.equips;
					DelEquipByIds(SoldIds);
				}
			}
		}
	}
	//end of added by zhangqiang

	#region 排序

	public class EquipIDCompare : IComparer<Equipment> {
		#region IComparer implementation

		public int Compare (Equipment x, Equipment y)
		{
			return (y.RtEquip.lv - x.RtEquip.lv) * 100 + (y.RtEquip.num - x.RtEquip.num);
		}

		#endregion

	}

	#endregion
	
	/*镶嵌宝石到装备
	 * */
	public void InlayGemToEquipment(BaseHttpRequest req, BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR) 
		{
			HttpRequest htReq = req as HttpRequest;
			Send_GemInLaySystem sendparam = htReq.ParamMem as Send_GemInLaySystem;
					
            Equipment equipment = getEquipment(sendparam.eid);
			EquipSlot[] slots = equipment.RtEquip.slot;

			/*镶嵌宝石
				 * */
				
			if(sendparam.slot>=0 && sendparam.slot<slots.Length)
			{
				//如果原来孔上里就有宝石,则摘除
				if(slots[sendparam.slot].id > 0)
				{
                    slots[sendparam.slot].mGem.equipped = false;
                    slots[sendparam.slot].mGem = null;
					slots[sendparam.slot].id=0;
				}
                if(sendparam.beUp==1)
				{
					Gems gem=FrogingSystem.ForgingRoomUI.Instance.InlaySystem.SelectedGemdata;
                    slots[sendparam.slot].mGem = gem;
					slots[sendparam.slot].id = gem.id;
					Core.Data.gemsManager.EuipOneGem(gem.id,true);	
				}							
			}
			else
			{
                RED.Log("InlayGemToEquipment FUC -> slots is out of range!!!" );
			}
		}
		
	}
	
	/*装备宝石孔重铸
	 * */
	public void EquipmentGemSlotRecast(BaseHttpRequest req, BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR) 
		{			    
			GemRecastResponse resp = response as GemRecastResponse;
		    Equipment SelectedEqudata = Core.Data.EquipManager.getEquipment(resp.data.eqid);
			if(SelectedEqudata.RtEquip.slot.Length==resp.data.slotc.Length)
			{
				for(int i=0;i<resp.data.slotc.Length;i++)
				{
					SelectedEqudata.RtEquip.slot[i].color=resp.data.slotc[i];
				}
				Core.Data.playerManager.RTData.curStone+=resp.data.stone;
			}
			else
			{
				RED.LogError("数量不匹配");
			}
		}
	}



	public int GetEquipTotalExp(int lv, int star)
	{
		int totalExp = 0;
		for(int i = 0;  i < ConfigUpEquipLv.Count; i++)
		{
			int addExp = 0;
			if(ConfigUpEquipLv[i].equipLevel < lv)
			{
				switch(star)
				{
					case 1:
						addExp = ConfigUpEquipLv[i].star1;
						break;
					case 2:
						addExp = ConfigUpEquipLv[i].star2;
						break;
					case 3:
						addExp = ConfigUpEquipLv[i].star3;
						break;
					case 4:
						addExp = ConfigUpEquipLv[i].star4;
						break;
					case 5:
						addExp = ConfigUpEquipLv[i].star5;
						break;
				}
				totalExp += addExp;
			}
		}

		return totalExp;
	}

	public int GetEquipLv(int totalExp, int star)
	{
		for(int i = 1;  i < ConfigUpEquipLv.Count; i++)
		{
			int curExp = GetEquipTotalExp(i, star);
			int nextExp = GetEquipTotalExp(i+1, star);
			if(totalExp >= curExp && totalExp < nextExp)
			{
				return i;
			}
		}
		return ConfigUpEquipLv.Count;
	}

	public int GetEquipUpExp(int lv, int star)
	{
		int exp = 0;
		for(int i = 0;  i < ConfigUpEquipLv.Count; i++)
		{
			if(ConfigUpEquipLv[i].equipLevel == lv)
			{
				switch(star)
				{
					case 1:
						exp = ConfigUpEquipLv[i].star1;
						break;
					case 2:
						exp = ConfigUpEquipLv[i].star2;
						break;
					case 3:
						exp = ConfigUpEquipLv[i].star3;
						break;
					case 4:
						exp = ConfigUpEquipLv[i].star4;
						break;
					case 5:
						exp = ConfigUpEquipLv[i].star5;
						break;
				}
				break;
			}
		}
		return exp;
	}


	//装备是否装上
	public bool IsEquiped(int num)
	{
		foreach (KeyValuePair<int,Equipment> itor in BagOfEquips)
		{
			if (itor.Value.Num == num && itor.Value.equipped)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsExpEquip(int equipID)
	{
		if(equipID == 40999 || equipID == 40998 || equipID == 45999 || equipID == 45998)
			return true;
		return false;
	}

	//装备是否镶嵌宝石激活额外加成
	public bool isHaveEffect(Equipment eqi)
	{
		if(eqi.RtEquip == null) return false;
		for(int i=0;i<eqi.RtEquip.slot.Length;i++)
		{
			if(eqi.RtEquip.slc[i]==0)
				return false;
			else if(eqi.RtEquip.slot[i].mGem ==null)
				return false;
			//else if(eqi.RtEquip.slc[i]!=eqi.RtEquip.slot[i].mGem.configData.color)
			else if(eqi.RtEquip.slot[i].color!=eqi.RtEquip.slot[i].mGem.configData.color)
				return false;
		}
		return true;
	}

	public List<Equipment> GetNewEquip()
	{
		List<Equipment> list = new List<Equipment> ();
		foreach (KeyValuePair<int, Equipment> itor in BagOfEquips)
		{
			if (itor.Value.isNew)
			{
				list.Add (itor.Value);
			}
		}
		return list;
	}

	public List<Equipment> GetAllEquipByNum(int num)
	{
		List<Equipment> list = new List<Equipment> ();
		foreach (KeyValuePair<int, Equipment> itor in BagOfEquips)
		{
			if (itor.Value.Num == num)
			{
				list.Add (itor.Value);
			}
		}
		return list;
	}

	public int GetValidEquipCount(int type, SplitType spType)
	{
		int count = 0;
		foreach (KeyValuePair<int, Equipment> itor in BagOfEquips)
		{
			if (itor.Value.ConfigEquip.type == type && !IsExpEquip(itor.Value.ConfigEquip.ID))
			{
				switch (spType)
				{
					case SplitType.None:
						count++;
						break;
					case SplitType.Split_If_InCurTeam:
						if (!itor.Value.equipped)
						{
							count++;
						}
						else
						{
							if (!Core.Data.playerManager.RTData.curTeam.equipInMyTeam (itor.Value.ID))
							{
								count++;
							}
						}
						break;
					case SplitType.Split_If_InTeam:
						if (!itor.Value.equipped)
							count++;
						break;
				}
			}
		}
		return count;
	}

	public void ClearData()
	{
		BagOfEquips.Clear ();
		StarOfEquips.Clear ();
	}

}