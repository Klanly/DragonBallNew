using System;
using System.Collections;
using System.Collections.Generic;

//魂魄
public class Soul
{
	public SoulInfo m_RTData;		//魂魄数据
	public SoulData m_config;		//配置数据
	public bool isNew;

	public Soul() { isNew = true;}
}

public class SoulManager : Manager, ICore
{
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

	private Dictionary<int, SoulData> m_dicConfig;
	private Dictionary<int, Soul> m_dicSoulBag;

	//碎片辅助数据，用于背包显示用
	private Dictionary<int, Soul> m_dicMonFrag;		//武者碎片
	private Dictionary<int, Soul> m_dicAtkFrag;		//武器碎片
	private Dictionary<int, Soul> m_dicDefFrag;		//防具碎片

    private const int EBallId = 150001;
    private const int NBallId = 150008;
   
	public SoulManager()
	{
		m_dicConfig = new Dictionary<int, SoulData> ();
		m_dicSoulBag = new Dictionary<int, Soul> ();

		m_dicMonFrag = new Dictionary<int, Soul> ();
		m_dicAtkFrag = new Dictionary<int, Soul> ();
		m_dicDefFrag = new Dictionary<int, Soul> ();
	}

	#region 完成ovveride的方法
	public override bool loadFromConfig ()
	{
        bool succ = base.readFromLocalConfigFile<SoulData> (ConfigType.Soul, m_dicConfig) ;


		foreach (KeyValuePair<int, SoulData> itor in m_dicConfig)
		{
			if (itor.Value.type == (int)ItemType.Monster_Frage)
			{
				Soul soul = new Soul ();
				soul.m_config = itor.Value;
				soul.m_RTData = new SoulInfo (itor.Value.ID * -1, itor.Value.ID, 0);
				m_dicMonFrag.Add (itor.Value.ID, soul);
			}
			else if (itor.Value.type == (int)ItemType.Equip_Frage)
			{
				EquipData equip = Core.Data.EquipManager.getEquipConfig (itor.Value.updateId);
				if (equip != null)
				{
					Soul soul = new Soul ();
					soul.m_config = itor.Value;
					soul.m_RTData = new SoulInfo (itor.Value.ID * -1, itor.Value.ID, 0);

					if (equip.type == 0)
					{
						m_dicAtkFrag.Add (itor.Value.ID, soul);
					}
					else
					{
						m_dicDefFrag.Add (itor.Value.ID, soul);
					}
				}
				else
				{
					RED.LogWarning ("equip frag not find equip :: " + itor.Value.ID + "  ::  " + itor.Value.updateId);
				}
			}
		}
        return succ;
	}

	public override void fullfillByNetwork (BaseResponse response)
	{
		if(response != null && response.status != BaseResponse.ERROR) 
		{
			LoginResponse loginResp = response as LoginResponse;
            if (loginResp != null && loginResp.data != null && loginResp.data.chip != null)
			{
				SoulInfo[] soulInfo = loginResp.data.chip;
                if(soulInfo != null) {
                    //clear dirty data
                    m_dicSoulBag.Clear();
					Core.Data.AccountMgr.clearBagStatus(ConfigDataType.Frag);

                    int length = soulInfo.Length;
                    for(int i = 0; i < length; i++) 
					{
                        Soul soul = new Soul();
                        soul.m_RTData = soulInfo[i];
						if (m_dicConfig.ContainsKey (soulInfo [i].num))
						{
							soul.m_config = m_dicConfig [soulInfo [i].num];
						}
						else
						{
							RED.LogWarning (soulInfo [i].num + " not find in soul config");
							continue;
						}

                        m_dicSoulBag.Add (soul.m_RTData.id, soul);

						if (soul.m_config.type == (int)ItemType.Monster_Frage)
						{
//							soul.isNew = Core.Data.AccountMgr.getStatus (soul.m_RTData.id) == BagOfStatus.STATUS_NEW;
//							Core.Data.AccountMgr.analyseBag (ConfigDataType.Frag, soul.m_RTData.id);

							if (m_dicMonFrag.ContainsKey (soul.m_config.ID))
							{
								m_dicMonFrag [soul.m_config.ID] = soul;
							}
						}
						else if (soul.m_config.type == (int)ItemType.Equip_Frage)
						{
							if (m_dicAtkFrag.ContainsKey (soul.m_config.ID))
							{
								m_dicAtkFrag [soul.m_config.ID] = soul;
							}
							else if (m_dicDefFrag.ContainsKey (soul.m_config.ID))
							{
								m_dicDefFrag [soul.m_config.ID] = soul;
							}
						}
                    }
                }

                CheckAgainCallDragon(loginResp.data.chip);
			}
		}
	}
	#endregion

    void CheckAgainCallDragon(SoulInfo[] chip){
        int ballCount = 0;
        int NBallCount = 0;
        for (int i = 0; i < chip.Length; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                if (chip[i].num == EBallId + j )
                {
                    if (chip[i].count != 0)
                    {
                        ballCount++;
                       
                    }
                }
                if (chip[i].num == NBallId + j)
                {
                    if (chip[i].count != 0)
                    {
                        NBallCount++;

                    }
                }
            }
        }
        if (ballCount != 7)
        {
            Core.Data.dragonManager.CancelCallDTimer(0);
        }
        if (NBallCount != 7)
        {
            Core.Data.dragonManager.CancelCallDTimer(1);
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
					AddSoul (battleResp.data.reward.p);
				}
				if (battleResp.data.ext != null)
				{
					AddSoul (battleResp.data.ext.p);
				}
			}

			SecretShopBuyResponse secretshop = response as SecretShopBuyResponse;
			if(secretshop != null && secretshop.data != null && secretshop.data.p != null) 
			{
				AddSoul(secretshop.data.p);
			}

            GetLevelRewardResponse GLRResponse = response as GetLevelRewardResponse;
            if (GLRResponse != null)
            {
                AddSoul (GLRResponse.data);
            }

            SevenDaysBuyResponse seven = response as SevenDaysBuyResponse;
            if (seven != null && seven.data != null) {
                AddSoul (seven.data.p);
            }

			NewFinalTrialFightResponse fightres = response as NewFinalTrialFightResponse;
			if(fightres != null && fightres.data!=null && fightres.data.rushResult != null && fightres.data.rushResult.award != null)
			{
				AddSoul (fightres.data.rushResult.award);
			}

            GetTresureResponse GTResponse = response as GetTresureResponse;
            if (GTResponse != null)
            {
                AddSoul (GTResponse.data.p);
            }

		}
	}


	public void UseProp(BaseResponse response)
	{
		if(response != null && response.status != BaseResponse.ERROR)
		{
			UsePropResponse resp = response as UsePropResponse;
			if(resp != null && resp.data != null && resp.data.p != null)
			{
				AddSoul(resp.data.p);
			}
		}
	}

	public SoulData GetSoulConfigByNum(int num)
	{
		if (m_dicConfig.ContainsKey (num))
		{
			return m_dicConfig [num];
		}
		return null;
	}

//	//得到碎片
//	public List<Soul> GetEquipAndMonFrag(short star)
//	{
//		List<Soul> aline = new List<Soul> ();
//		aline.AddRange (GetFragmentByStar (star, ItemType.Monster_Frage).ToArray());
//		aline.AddRange (GetFragmentByStar (star, ItemType.Equip_Frage).ToArray());
////		aline.Sort (new SoulCompare ());
//		return aline;
//	}
//
//	public class SoulCompare : IComparer<Soul>
//	{
//		public int Compare (Soul x, Soul y)
//		{
//			return (x.m_RTData.count - y.m_RTData.count);
//		}
//	}


	//根据星级返回魂魄list
	//type 
	// EARTH_DRAGON = 0;			//地球龙珠
	// NAMEIKEXING_DRAGON = 22;	//那美克星龙珠
	// MONSTER_SOUL = 28;			//宠物魂魄

	public List<Soul> GetFragmentByStar(short star, ItemType tp)
	{
		List<Soul> list = new List<Soul> ();

		foreach (Soul soul in m_dicSoulBag.Values)
		{
			if(soul.m_config == null)
			{
				RED.LogWarning(soul.m_RTData.num + "config data is not find");
				continue;
			}

			if (soul.m_config.star == star && soul.m_config.type == (int)tp)
			{
				list.Add (soul);
			}
		}
		return list;
	}

	public void Zhaomu(BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR) {
			ZhaoMuResponse resp = response as ZhaoMuResponse;
			if (resp.data.p != null) 
			{
				AddSoul (resp.data.p);
			}
		}
	}


    //根据类型返回列表
    public List<Soul> GetFramentByType(ItemType tp) 
	{
        List<Soul> list = new List<Soul> ();
        foreach (Soul soul in m_dicSoulBag.Values) 
		{
            if(soul.m_config == null) 
			{
                RED.LogWarning(soul.m_RTData.num + "config data is not find");
                continue;
            }

            if (soul.m_config.type == (int)tp)
                list.Add (soul);
        }
		//  ConsoleEx.Write ("  this   type  = " + tp  +   " ,  count  = "+ list.Count);
        return list;
    }

	public int GetSoulCountByNum(int num)
	{
		foreach (Soul soul in m_dicSoulBag.Values) 
		{
			if(soul.m_config != null) 
			{
				if(num == soul.m_config.ID)
				{
					return soul.m_RTData.count;
				}
			}
		}
		return 0;
	}

	public Soul GetSoulByID(int id)
	{
		if (m_dicSoulBag.ContainsKey (id))
		{
			return m_dicSoulBag [id];
		}
		return null;
	}
        

    public Soul GetSoulByNum(int num)
    {
        foreach(KeyValuePair<int, Soul> itor in m_dicSoulBag)
        {
            if (itor.Value.m_config.ID == num)
            {
                return itor.Value;
            }
        }
        return null;
    }


    public void DelSoul(int pid)
    {
        Soul soul = GetSoulByNum(pid);
		//	ConsoleEx.Write (  " delete  soule   " + pid + " sou id " + soul.m_config.ID  + " type "+ soul.m_config.type);
		if (soul != null) 
		{
			soul.m_RTData.count--;
			if (soul.m_RTData.count <= 0)
			{
				m_dicSoulBag.Remove (soul.m_RTData.id);
				Core.Data.AccountMgr.setStatus (new BagOfStatus (soul.m_RTData.id, soul.m_config.ID, BagOfStatus.STATUS_DELETE));
				//如果当前龙珠 不存在了  即 数量为零 则取消 召唤时间
				CancelCallDragonTime (pid);
			}
		} 
    }

	/// <summary>
	/// 同步碎片数量 只用于神龙
	/// </summary>
	/// <param name="pid">Pid.</param>
	public void SyncDBSoulNum(DragonBallItemData[] balls){
		if (balls != null) {
			List<int> tempIdList = new List<int> ();

			for (int i = 0; i < balls.Length; i++) {
				tempIdList.Add (balls[i].c_pid);
			}

			for (int i = 0; i < 7; i++) {
				Soul earthBall = GetSoulByNum (EBallId + i);
				Soul nmkxBall = GetSoulByNum (NBallId + i);

				if (earthBall != null) {
					if (tempIdList.Contains (earthBall.m_RTData.num)) {
						foreach (DragonBallItemData bData in balls) {
							if (earthBall.m_RTData.num == bData.c_pid) {
								earthBall.m_RTData.count = bData.c_num;
							}
						}
					} else {
						DelSoul (earthBall.m_RTData.num);
					}
				} 

				if (nmkxBall != null) {
					if (tempIdList.Contains (nmkxBall.m_RTData.num)) {
						foreach (DragonBallItemData bData in balls) {
							if (nmkxBall.m_RTData.num == bData.c_pid) {
								nmkxBall.m_RTData.count = bData.c_num;
							}
						}
					} else {
						DelSoul (nmkxBall.m_RTData.num);
					}
				}
			}
		}
	}

    void CancelCallDragonTime(int pid){
        if (!m_dicSoulBag.ContainsKey(pid))
        {   
			SoulData fragData = GetSoulConfigByNum (pid);
			if (fragData != null)
            {
				if (fragData.type == (int)ItemType.Earth_Frage)
                {
                    Core.Data.dragonManager.CancelCallDTimer(0);
                }
				else if(fragData.type == (int)ItemType.Nameike_Frage)
                {
                    Core.Data.dragonManager.CancelCallDTimer(1);
                }
            }
        }
    }


	public void SoulHeCheng(BaseHttpRequest request, BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR)
		{
			HttpRequest rq = request as HttpRequest;
			SoulHeChenParam param = rq.ParamMem as SoulHeChenParam;

			Soul soul = GetSoulByID (param.chipId);
			soul.m_RTData.count -= soul.m_config.quantity;

			if (soul.m_RTData.count <= 0)
			{
				int num = m_dicSoulBag[param.chipId].m_config.ID;
				m_dicSoulBag.Remove (param.chipId);
				//删除统计
				Core.Data.AccountMgr.setStatus(new BagOfStatus(param.chipId, num, BagOfStatus.STATUS_DELETE));
			}

			if (m_dicMonFrag.ContainsKey (soul.m_config.ID))
			{
//				m_dicMonFrag[soul.m_config.ID].m_RTData.count -= soul.m_config.quantity;
				if (m_dicMonFrag [soul.m_config.ID].m_RTData.count < 0)
				{
					m_dicMonFrag [soul.m_config.ID].m_RTData.count = 0;
				}
				
			}
			else if(m_dicAtkFrag.ContainsKey(soul.m_config.ID))
			{
//				m_dicAtkFrag[soul.m_config.ID].m_RTData.count -= soul.m_config.quantity;
				if (m_dicAtkFrag [soul.m_config.ID].m_RTData.count < 0)
				{
					m_dicAtkFrag [soul.m_config.ID].m_RTData.count = 0;
				}
			}
			else if(m_dicDefFrag.ContainsKey(soul.m_config.ID))
			{
//				m_dicDefFrag[soul.m_config.ID].m_RTData.count -= soul.m_config.quantity;
				if (m_dicDefFrag [soul.m_config.ID].m_RTData.count < 0)
				{
					m_dicDefFrag [soul.m_config.ID].m_RTData.count = 0;
				}
			}
		}
	}

	public void AddSoul(ItemOfReward[] p)
	{
		if(p != null)
		{
			foreach(ItemOfReward ior in p)
			{
				ConfigDataType type = ior.getCurType();
				if(ior != null && type == ConfigDataType.Frag) 
				{
					Soul soul = ior.toSoul (this);
					AddSoul(soul);
				}
			}
				
		}
	}

	public void AddSoul(Soul soul)
	{
		if(m_dicSoulBag.ContainsKey(soul.m_RTData.id))
		{
			m_dicSoulBag[soul.m_RTData.id].m_RTData.count += soul.m_RTData.count;
			m_dicSoulBag [soul.m_RTData.id].isNew = true;
		} 
		else
		{
			m_dicSoulBag.Add(soul.m_RTData.id, soul);

			if (soul.m_config.type == (int)ItemType.Monster_Frage)
			{
				if (m_dicMonFrag.ContainsKey (soul.m_config.ID))
				{
					m_dicMonFrag [soul.m_config.ID] = soul;
				}
			}
			else if (soul.m_config.type == (int)ItemType.Equip_Frage)
			{
				if (m_dicAtkFrag.ContainsKey (soul.m_config.ID))
				{
					m_dicAtkFrag [soul.m_config.ID] = soul;
				}
				else if (m_dicDefFrag.ContainsKey (soul.m_config.ID))
				{
					m_dicDefFrag [soul.m_config.ID] = soul;
				}
			}
		}
		//加入统计
		if (soul.m_config.type == (int)ItemType.Monster_Frage)
		{
			Core.Data.AccountMgr.analyseBag (ConfigDataType.Frag, soul.m_RTData.id);
		}

//		if (m_dicMonFrag.ContainsKey (soul.m_config.ID))
//		{
//			m_dicMonFrag [soul.m_config.ID].m_RTData.count += soul.m_RTData.count;
//		}
//		else if (m_dicAtkFrag.ContainsKey (soul.m_config.ID))
//		{
//			m_dicAtkFrag [soul.m_config.ID].m_RTData.count += soul.m_RTData.count;
//		}
//		else if (m_dicDefFrag.ContainsKey (soul.m_config.ID))
//		{
//			m_dicDefFrag [soul.m_config.ID].m_RTData.count = soul.m_RTData.count;
//		}
	}

    public void RemoveItem(int id){
        Soul soul = GetSoulByID (id);
       
        if (soul != null)
		{
            soul.m_RTData.count -= soul.m_config.quantity;

            if (soul.m_RTData.count <= 0)
			{
                int num = m_dicSoulBag [id].m_config.ID;
                m_dicSoulBag.Remove (id);
                //删除统计
                Core.Data.AccountMgr.setStatus (new BagOfStatus (id, num, BagOfStatus.STATUS_DELETE));
            }

			if (m_dicMonFrag.ContainsKey (soul.m_config.ID))
			{
				m_dicMonFrag[soul.m_config.ID].m_RTData.count -= soul.m_config.quantity;
				if (m_dicMonFrag [soul.m_config.ID].m_RTData.count < 0)
				{
					m_dicMonFrag [soul.m_config.ID].m_RTData.count = 0;
				}

			}
			else if(m_dicAtkFrag.ContainsKey(soul.m_config.ID))
			{
				m_dicAtkFrag[soul.m_config.ID].m_RTData.count -= soul.m_config.quantity;
				if (m_dicAtkFrag [soul.m_config.ID].m_RTData.count < 0)
				{
					m_dicAtkFrag [soul.m_config.ID].m_RTData.count = 0;
				}
			}
			else if(m_dicDefFrag.ContainsKey(soul.m_config.ID))
			{
				m_dicDefFrag[soul.m_config.ID].m_RTData.count -= soul.m_config.quantity;
				if (m_dicDefFrag [soul.m_config.ID].m_RTData.count < 0)
				{
					m_dicDefFrag [soul.m_config.ID].m_RTData.count = 0;
				}
			}
        }
    }

	public void BuyItemSuc(BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR)
		{
			if (response is QiangDuoGoldBuyItemResponse) {
				QiangDuoGoldBuyItemResponse buyitemResp = response as QiangDuoGoldBuyItemResponse;
				if (buyitemResp != null && buyitemResp.data != null) 
				{
					AddSoul (buyitemResp.data.p);
				}
			}
			else if (response is ZhanGongBuyItemResponse) {
				ZhanGongBuyItemResponse buyitemResp1= response as ZhanGongBuyItemResponse;
				if (buyitemResp1 != null && buyitemResp1.data != null) 
				{
					AddSoul (buyitemResp1.data.p);
				}
			}

			else if (response is GetVipLevelRewardResponse) {
				GetVipLevelRewardResponse resp = response as GetVipLevelRewardResponse;
				if (resp != null && resp.data != null) 
				{
					AddSoul(resp.data.p);
				}
			}

			else if (response is GetActivityLimittimeRewardResponse) {
				GetActivityLimittimeRewardResponse resp = response as GetActivityLimittimeRewardResponse;
				if (resp != null && resp.data != null) 
				{
					AddSoul(resp.data.p);
				}
			}

			else if(response is BuyItemResponse)
			{
				BuyItemResponse resp = response as BuyItemResponse;
				if (resp != null && resp.data != null)
				{
					if (resp.data.Result != null && resp.data.Result.p != null)
					{
						AddSoul (resp.data.Result.p);
					}
					else if (resp.data.p != null)
					{
						AddSoul (resp.data.p);
					}
				}
			}
		}
	}

	public List<Soul> GetNewSoul()
	{
		List<Soul> list = new List<Soul> ();
		foreach (KeyValuePair<int, Soul> itor in m_dicSoulBag)
		{
			if (itor.Value.isNew && 
				(itor.Value.m_config.type == (int)ItemType.Monster_Frage 
					||  itor.Value.m_config.type == (int)ItemType.Equip_Frage))
			{
				list.Add (itor.Value);
			}
		}

		return list;
	}

	public void ClearData()
	{
		m_dicSoulBag.Clear ();
//		m_dicAtkFrag.Clear ();
//		m_dicDefFrag.Clear ();
//		m_dicMonFrag.Clear ();
		foreach (KeyValuePair<int, Soul> itor in m_dicAtkFrag)
		{
			itor.Value.m_RTData.count = 0;
		}

		foreach (KeyValuePair<int, Soul> itor in m_dicDefFrag)
		{
			itor.Value.m_RTData.count = 0;
		}

		foreach (KeyValuePair<int, Soul> itor in m_dicMonFrag)
		{
			itor.Value.m_RTData.count = 0;
		}
	}

	public List<Soul> GetMonFragByStar(int star)
	{
		List<Soul> list = new List<Soul> ();
		foreach (KeyValuePair<int, Soul> itor in m_dicMonFrag)
		{
			if (itor.Value.m_config.star == star)
			{
				list.Add (itor.Value);
			}
		}
		list.Sort (new FragCompare ());
		return list;
	}

	public List<Soul> GetAtkFragByStar(int star)
	{
		List<Soul> list = new List<Soul> ();
		foreach (KeyValuePair<int, Soul> itor in m_dicAtkFrag)
		{
			if (itor.Value.m_config.star == star)
			{
				list.Add (itor.Value);
			}
		}
		list.Sort (new FragCompare ());
		return list;
	}

	public List<Soul> GetDefFragByStar(int star)
	{
		List<Soul> list = new List<Soul> ();
		foreach (KeyValuePair<int, Soul> itor in m_dicDefFrag)
		{
			if (itor.Value.m_config.star == star)
			{
				list.Add (itor.Value);
			}
		}
		list.Sort (new FragCompare ());
		return list;
	}

	#region 排序

	public class FragCompare : IComparer<Soul>
	{
		public int Compare (Soul x, Soul y)
		{
			return (y.m_RTData.count - x.m_RTData.count);
		}
	}

	#endregion


	#region 检测是否有碎片集齐
	public bool IsMonFragOK()
	{
		foreach(KeyValuePair<int, Soul> itor in m_dicMonFrag)
			if(itor.Value.m_RTData.count >= itor.Value.m_config.quantity)
				return true;
		return false;
	}

	public bool IsAtkEquipOk()
	{
		foreach(KeyValuePair<int, Soul> itor in m_dicAtkFrag)
			if(itor.Value.m_RTData.count >= itor.Value.m_config.quantity)
				return true;
		return false;
	}

	public bool IsDefEquipOK()
	{
		foreach(KeyValuePair<int, Soul> itor in m_dicDefFrag)
			if(itor.Value.m_RTData.count >= itor.Value.m_config.quantity)
				return true;
		return false;
	}

	public bool IsHaveFullFrag()
	{
		return IsMonFragOK() || IsAtkEquipOk() || IsDefEquipOK();
	}

	#endregion


}


