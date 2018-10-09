using System;
using fastJSON;
using System.IO;
using System.Collections.Generic;

///过滤Monster的方式
public enum SplitType
{
	//不过滤
	None,
	//在队伍里面就过滤
	Split_If_InTeam,
	//在当前队伍里面才过滤
	Split_If_InCurTeam,
}

/// <summary>
/// Monster manager. Manager
/// </summary>
public class MonsterManager : Manager , ICore
{
	#region ICore implementation

	void ICore.Dispose ()
	{
		throw new NotImplementedException ();
	}

	void ICore.Reset ()
	{
		throw new NotImplementedException ();
	}

	void ICore.OnLogin (object obj)
	{
		throw new NotImplementedException ();
	}

	#endregion

	//Key is monster Id,
	public Dictionary<int, Monster> mBagOfMonster = null;
	//Key is monster star,这个是辅助mBagOfMonster的读取, 每次新增或减少的时候，都要刷新mBagOfMonster & StarOfMonster
	private readonly Dictionary<int, List<int>> StarOfMonster = null;

	//Key is monster number
	private readonly Dictionary<int, MonsterData> mConfig = null;

	//key is monster star 武者星级信息
	private readonly Dictionary<int , MonStarData> mdicStarData;

	public List<MonsterLvExp> monsterLvExpConfig = null;

	public const int GOLD_EXP_PIG = 19999;			//黄金乌龙
	public const int SILVER_EXP_PIG = 19998;		//纯银乌龙

	public MonsterManager ()
	{
		mConfig = new Dictionary<int, MonsterData> ();
		mBagOfMonster = new Dictionary<int, Monster> ();
		StarOfMonster = new Dictionary<int, List<int>> ();
		this.monsterLvExpConfig = new List<MonsterLvExp>();
		mdicStarData = new Dictionary<int, MonStarData> ();
	}

	public override bool loadFromConfig ()
	{
		bool succ = base.readFromLocalConfigFile<MonsterData> (ConfigType.Monster, mConfig)
		            | base.readFromLocalConfigFile<MonsterLvExp> (ConfigType.MonsterLvExp, this.monsterLvExpConfig)
		            | base.readFromLocalConfigFile<MonStarData> (ConfigType.MonsterStar, this.mdicStarData);
		return succ;
	}

	public override void fullfillByNetwork (BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR) {
			LoginResponse resp = response as LoginResponse;
			if (resp != null && resp.data != null && resp.data.monster != null) {
				MonsterInfo[] mins = resp.data.monster;

                //clear dirty data
                mBagOfMonster.Clear();
                StarOfMonster.Clear();
				Core.Data.AccountMgr.clearBagStatus(ConfigDataType.Monster);

				foreach (MonsterInfo min in mins) {
					if (min != null) {
						Monster mon = min.toMonster (this);
						if (!mConfig.TryGetValue (mon.num, out mon.config)) {
							throw new DragonException ("Monster =" + mon.num + " Config file is not correct..");
						}
						AddMonter(mon);
					}
				}

			}
		}
	}

	/// <summary>
	/// Win Battle & Get Reward
	/// </summary>
	/// <param name="response">Response.</param>
	public override void addItem (BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR) {
			//Boss Battle or not
			BattleResponse battleResp = response as BattleResponse;
			if (battleResp != null && battleResp.data != null )
			{
				BattleReward Rewards = battleResp.data.reward;

				if(Rewards != null)
				{
					addMonster (Rewards);

					if(Rewards.p != null) {
						foreach(ItemOfReward it in Rewards.p) {
							ConfigDataType type = DataCore.getDataType(it.pid);
							if(type == ConfigDataType.Gems)
								Core.Data.gemsManager.AddGems(it);
						}
					}

				}
				if (battleResp.data.ext != null)
				{
					addMonster (battleResp.data.ext.p);
				}
			}

			SecretShopBuyResponse secretshop = response as SecretShopBuyResponse;
			if (secretshop != null && secretshop.data != null && secretshop.data.p != null) {
				AddShopMon (secretshop.data.p);
			}

			SevenDaysBuyResponse seven = response as SevenDaysBuyResponse;
			if (seven != null && seven.data != null) {
				addMonster (seven.data.p);
			}

            GetLevelRewardResponse GLRResponse = response as GetLevelRewardResponse;
            if (GLRResponse != null)
            {
                AddShopMon (GLRResponse.data);
            }

			NewFinalTrialFightResponse fightres = response as NewFinalTrialFightResponse;
			if(fightres != null && fightres.data!=null && fightres.data.rushResult != null && fightres.data.rushResult.award != null)
			{
				addMonster (fightres.data.rushResult.award);
			}

            SockBuyItemResponse buyItem = response as SockBuyItemResponse;
            if(buyItem != null){
                if (buyItem.data.retCode == 1) {
					// if (ActivityNetController.tempHonorGiftId != 0) {
                        ItemOfReward[] tReward = new ItemOfReward[1]{buyItem.data.p};
                        addMonster (tReward);
					//  }
                }
            }

            GetTresureResponse GTResponse = response as GetTresureResponse;
            if (GTResponse != null)
            {
                addMonster (GTResponse.data.p);
            }

		}
	}

	public void AddShopMon(ItemOfReward[] p)
	{
		foreach (ItemOfReward ior in p) 
		{
			if (ior != null && ior.getCurType () == ConfigDataType.Monster) {
				//Add monster
				Monster mon = ior.toMonster (this);
				AddMonter(mon);
			}
		}
	}

	public void AddSevenMon(SevenDaysBuyRewardData[] p)
	{
		foreach (SevenDaysBuyRewardData ior in p) 
		{
			if (ior != null && DataCore.getDataType(ior.pid) == ConfigDataType.Monster) {
				//Add monster
				RuntimeMonster rtMon = new RuntimeMonster();
				rtMon.Attribute = (MonsterAttribute)ior.at;
				rtMon.curExp = ior.ep;
				rtMon.curLevel = (int)ior.lv;
				rtMon.ChaKeLa_Attck = ior.ak / RuntimeMonster.ATTACK_FACTOR;
				rtMon.ChaKeLa_Defend = ior.df / RuntimeMonster.DEFEND_FACTOR;

				Monster mon = new Monster(ior.ppid, ior.pid, rtMon, Core.Data.monManager.getMonsterByNum(ior.pid));
				AddMonter(mon);
			}
		}
	}


	public void UseProp (BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR) {
			UsePropResponse resp = response as UsePropResponse;
			if (resp.data != null &&  resp.data.p != null)
			{
				for(int i = 0; i < resp.data.p.Length; i++)
				{
					if(resp.data.p[i].getCurType () != ConfigDataType.Monster)
					{
						continue;
					}
					
					Monster mon = resp.data.p[i].toMonster (this);
					AddMonter(mon);
				}
			}
		}
	}

	public void addMonster (BattleReward reward)
	{
		Utils.Assert (reward == null, "We can't add empty list.");

		if (reward != null && reward.p != null) 
		{
			addMonster (reward.p);
		}
	}

	void addMonster(ItemOfReward[] p)
	{
		if (p == null)
		{
			return;
		}
		foreach (ItemOfReward ior in p) 
		{
			if (ior != null && ior.getCurType () == ConfigDataType.Monster) {
				//Add monster
				Monster mon = ior.toMonster (this);
				AddMonter(mon);
			}
		}
	}

	/// <summary>
	/// 武者的节日 增加 宠物
	/// </summary>
	/// <param name="response">Response.</param>
	public void AddMonsterInFestival (BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR)
		{
			SockFestivalBuyLotteryResponse buyLotteryResponse = response as SockFestivalBuyLotteryResponse;
			if (buyLotteryResponse != null && buyLotteryResponse.data != null) 
			{
//				ItemOfReward[] p = new ItemOfReward[1]{buyLotteryResponse.data.p };
				Core.Data.itemManager.AddRewardToBag (buyLotteryResponse.data.p);
//                Core.Data.monManager.AddShopMon(p);
				
			}
		}
	}

	//进化宠物
	public void EvolveMonster( BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR)
		{
			EvolveMonsterResponse resp = response as EvolveMonsterResponse;
			if (resp.data != null && resp.data.p != null)
			{
				for (int i = 0; i < resp.data.p.Length; i++)
				{
					if (resp.data.p [i].getCurType () == ConfigDataType.Monster)
					{
						Monster preMon = getMonsterById (resp.data.p [i].ppid);
						bool inTeam = preMon.inTeam;
						Monster mon = resp.data.p [i].toMonster (this);

						for(int j=0;j<preMon.skillLvs.Length;j++){
							mon.skillLvs[j].skillId = preMon.skillLvs[j].skillId;
							mon.skillLvs[j].skillLevel = preMon.skillLvs[j].skillLevel;
						}


						DelMonster (resp.data.p [i].ppid);
						AddMonter (mon);
						if (inTeam)
						{
							foreach(MonsterTeam team in Core.Data.playerManager.RTData.myTeams)
							{
								int pos = team.GetMonsterPos (resp.data.p [i].ppid);
								{
									if (pos != -1)
									{
										team.setMember (mon, pos);
										break;
									}
								}
							}
						}
					}
				}
			}
		}
	}

	//添加宠物
	public void AddMonter (Monster mon)
	{
		mBagOfMonster [mon.pid] = mon;
		List<int> helper = null;
		if (StarOfMonster.TryGetValue (mon.Star, out helper)) 
		{
			helper.Add (mon.pid);
		} 
		else
		{
			helper = new List<int>();
			helper.Add(mon.pid);
			StarOfMonster.Add (mon.Star, helper);
		}

		mon.isNew = Core.Data.AccountMgr.getStatus (mon.pid) == BagOfStatus.STATUS_NEW;

		//加入统计
		Core.Data.AccountMgr.analyseBag(ConfigDataType.Monster, mon.pid);
	}

	//删除宠物
	private void DelMonster (int pid)
	{
		Monster mon = getMonsterById (pid);
		if (mon != null)
		{
			if (StarOfMonster.ContainsKey (mon.Star)) {
				List<int> list = StarOfMonster [mon.Star];
				for (int i = 0; i < list.Count; i++) {
					if (list [i] == pid) 
					{
						list.Remove (pid);
						break;
					}
				}
			}

			if (mBagOfMonster.ContainsKey (pid)) {
				mBagOfMonster.Remove (pid);
			}

			//删除统计
			Core.Data.AccountMgr.setStatus(new BagOfStatus(pid, mon.num, BagOfStatus.STATUS_DELETE));
		}
	}

	public void ZhaoMuMonster (BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR) {
			ZhaoMuResponse resp = response as ZhaoMuResponse;
			if (resp.data.p != null) 
			{
				addMonster (resp.data.p);
			}
		}
	}

	public void SoulHeCheng(BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR)
		{
			SoulHeChenResponse resp = response as SoulHeChenResponse;
			if (resp.data != null)
			{
				addMonster (resp.data);
			}
		}
	}

	public void HeChengMonster (BaseHttpRequest request, BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR) {
			//删除吃掉的宠物
			HeChengResponse resp = response as HeChengResponse;
            for (int i = 0; i < resp.data.delppid.Length; i++) {
                DelMonster (resp.data.delppid[i]);
			}

            //更新 属性
            //for (int n = 0; n < resp.data.Length; n++) {
            Monster mon = getMonsterById(resp.data.ppid);
            if (mon != null)
            {
                mon.RTData.Attribute = (MonsterAttribute)resp.data.at;
            }
            //resp.data [n].toMonster (Core.Data.monManager);
            //	AddMonter (mon);
            //}
		}
	}
	//属性变换
	public void AttrSwap (BaseHttpRequest request, BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR) {
			HttpRequest htReq = request as HttpRequest;
			AttrSwapParam param = htReq.ParamMem as AttrSwapParam;

			AttrSwapResponse resp = response as AttrSwapResponse;

			Monster mon = getMonsterById (param.roleid);
			if (mon != null) {
				mon.RTData.Attribute = (MonsterAttribute)resp.data.atr;
			}

			RED.Log ("attr:    " + resp.data.atr);
			RED.Log ("id:     " + param.roleid);

			mBagOfMonster [mon.pid] = mon;
		}
	}
	//潜力训练
	public void QianLiXunLian (BaseHttpRequest request, BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR) {
			HttpRequest htReq = request as HttpRequest;
			QianLiXunLianParam param = htReq.ParamMem as QianLiXunLianParam;

			QianLiXunLianResponse resp = response as QianLiXunLianResponse;

			Monster mon = getMonsterById (param.roleid);
			if (mon != null) 
			{
				mon.RTData.ChaKeLa_Attck += resp.data.ak;
				mon.RTData.ChaKeLa_Defend += resp.data.df;

//				mon.BTData.CombieAttack = mon.enhanceAttack + mon.baseAttack;
//				mon.BTData.CombieDefend = mon.enhanceDefend + mon.baseDefend;
			
				mon.RTData.uspt += resp.data.ak;
				mon.RTData.uspt += resp.data.df;
			}
		}
	}

	public void QianLiReset(BaseHttpRequest request, BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR)
		{
			HttpRequest htReq = request as HttpRequest;
			QianLiResetParam param = htReq.ParamMem as QianLiResetParam;

			Monster mon = getMonsterById (param.roleid);
			if (mon != null)
			{
				mon.RTData.ChaKeLa_Attck = 0;
				mon.RTData.ChaKeLa_Defend = 0;
				mon.RTData.uspt = 0;
				mon.BTData.CombieAttack = mon.enhanceAttack + mon.baseAttack;
				mon.BTData.CombieDefend = mon.enhanceDefend + mon.baseDefend;
			}
			else
			{
				RED.LogWarning (param.roleid +  "  monster not find");
			}
		}
	}


	public void MonSkillUpgrade(BaseHttpRequest request, BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR)
		{
			HttpRequest htpRq = request as HttpRequest;
			SkillUpgradeParam param = htpRq.ParamMem as SkillUpgradeParam;

			Monster mon = getMonsterById (param.roleId);
			if (mon != null)
			{
				SkillUpgradeResponse resp = response as SkillUpgradeResponse;
				for (int i = 0; i < mon.skillLvs.Length; i++)
				{
					if (mon.skillLvs [i].skillId == param.skillNum)
					{
						mon.skillLvs [i].skillLevel = resp.data.level;
						break;
					}
				}

				List<Skill> list = mon.getSkill;
				for (int i = 0; i < list.Count; i++)
				{
					if (list [i].sdConfig.ID == param.skillNum)
					{
						list [i].level = resp.data.level;
						break;
					}
				}
			}
		}
	}

	/// <summary>
	/// Sells the monster.
	/// </summary>
	/// <param name="request">Request.</param>
	/// <param name="response">Response.</param>
	public void sellMonster (BaseHttpRequest request, BaseResponse response)
	{
		ConsoleEx.Write ("receive sellMonster message Sucess");
		Utils.Assert (request == null || response == null, "Parameter can't be null.");
		if (response.status != BaseResponse.ERROR) {
			if (request.baseType == BaseHttpRequestType.Common_Http_Request) {
				HttpRequest htReq = request as HttpRequest;

				SellMonsterParam param = htReq.ParamMem as SellMonsterParam;
				if (param != null) {
					int[] SoldIds = param.roles;
					delMonsterByIds (SoldIds);
				}
			}
		}
	}

	private void delMonsterByIds (int[] ids)
	{
		if (ids != null && ids.Length > 0) {
			foreach (int id in ids) {
				DelMonster (id);
			}
		}
	}
		
	// strengthen monster
	public void StrengthenMonster (BaseHttpRequest request, BaseResponse response)
	{
		Utils.Assert (request == null || response == null, "Parameter can't be null.");
		if (response.status != BaseResponse.ERROR) {
			if (request.baseType == BaseHttpRequestType.Common_Http_Request) {
				HttpRequest htReq = request as HttpRequest;
				StrengthenResponse htResp = response as StrengthenResponse;

				StrengthenParam param = htReq.ParamMem as StrengthenParam;
				if (param != null) {
					//remove all src monsters
					int[] SoldIds = param.roles;
					delMonsterByIds (SoldIds);

					//level up target monster
					Monster monster = getMonsterById (param.sroleid);
					if (monster != null) {
						monster.RTData.curExp = htResp.data.ep;

						monster.RTData.curLevel = htResp.data.lv;
                        
						monster.BTData = new BattleMonster(monster.baseAttack, monster.enhanceAttack, monster.baseDefend, monster.enhanceDefend);

						List<Skill> skillList = monster.getSkill;
						if (skillList != null && skillList.Count > 2)
						{
							if (skillList [1] != null)
							{
								skillList [1].opened = monster.RTData.curLevel >= Skill.SKILL2_MIN_LEVEL;
							}
						}

                        if(monster.inTeam) {
                            foreach(MonsterTeam team in Core.Data.playerManager.RTData.myTeams) {
                                if(team.inMyTeam(monster.pid)) {
                                    team.upgradeMember();
                                }
                            }
                        }

					}
				}
			}
		}
	}

	/// <summary>
	/// Gets the monster by identifier(pid)
	/// </summary>
	/// <returns>The monster by identifier.</returns>
	/// <param name="id">Identifier.</param>
	public Monster getMonsterById (int pid)
	{
		Monster monster = null;
		if (!mBagOfMonster.TryGetValue (pid, out monster)) {
			monster = null;
		}
		return monster;
	}

	//得到武者个数
	public int GetMonCnt()
	{
		return mBagOfMonster.Count;
	}

	//得到新增的宠物列表
	public List<Monster> GetNewMonList()
	{
		List<Monster> monList = new List<Monster> ();
		foreach (KeyValuePair<int, Monster> itor in mBagOfMonster)
		{
			if (itor.Value.isNew)
			{
				monList.Add (itor.Value);
			}
		}
		return monList;
	}

	public MonsterData getMonsterByNum (int num)
	{
		MonsterData data = null;
		if (mConfig.TryGetValue (num, out data)) {
			return data;
		} else {
            RED.LogWarning (num + "monster config data not find!");
			return null;
		}
	}


	public List<Monster> getEvolveMonListByStar(short star)
	{
		List<Monster> stars = new List<Monster> ();
		if (StarOfMonster.ContainsKey (star)) 
		{
			foreach (int monId in StarOfMonster[star]) 
			{
				Monster mon = getMonsterById (monId);
				if (mon != null && mon.config.jinjie == 1 && mon.Star != 6) 
				{
					stars.Add (mon);
				}
			}

			stars = SortMonList (stars);
		}

		return SortList(stars);
	}

	/// <summary>
	/// Gets the monster list by star. 不要经常调用该方法
	/// </summary>
	/// <returns>The monster list by star.</returns>
	/// <param name="star">Star.</param>
	public List<Monster> getMonsterListByStar (short star, SplitType type = SplitType.None)
	{
		//新手引导，特殊处理龟仙人问题
		List<Monster> stars = new List<Monster> ();
		if (mBagOfMonster.ContainsKey (-1))
		{
			if (Core.Data.guideManger.isGuiding)
			{
				Monster mon = mBagOfMonster [-1];
				if (star == mon.config.star)
				{
					stars.Add (mon);
				}
				return stars;
			}
			else
				return stars;
		}
		else
		{
			if (StarOfMonster.ContainsKey (star)) {
				foreach (int monId in StarOfMonster[star]) {
					Monster mon = getMonsterById (monId);
					if (mon != null) {

						switch (type) {
						case SplitType.None:
							stars.Add (mon);
							break;
						case SplitType.Split_If_InCurTeam:
							if (!mon.inTeam) {
								stars.Add (mon);
							} else {
								if (!Core.Data.playerManager.RTData.curTeam.inMyTeam (mon.pid)) {
									stars.Add (mon);
								}
							}
							break;
						case SplitType.Split_If_InTeam:
							if (!mon.inTeam)
								stars.Add (mon);
							break;
						}
					}
				}

				stars = SortMonList (stars);
			}
		}
		return SortList(stars);
	}


	public List<Monster> SortMonList(List<Monster> srcList)
	{
		//先按照等级对所有宠物排序
		srcList.Sort (new MonIDCompare ());

		//按照num分类
		Dictionary<int, List<Monster>> dicMon = new Dictionary<int, List<Monster>> ();
		for (int i = 0; i < srcList.Count; i++)
		{
			List<Monster> tempList = null;
			if (dicMon.ContainsKey (srcList [i].num))
			{
				tempList = dicMon [srcList [i].num]; 
			}
			else
			{
				tempList = new List<Monster> ();
				dicMon.Add (srcList [i].num, tempList);
			}
			tempList.Add (srcList [i]);
		}

		//子list排序
		foreach(KeyValuePair<int, List<Monster>> itor  in dicMon)
		{
			itor.Value.Sort (new MonAttrCompare ());
		}

		Dictionary<int, Monster> dicTemp = new Dictionary<int, Monster> ();
		for (int i = 0; i < srcList.Count; i++)
		{
			if (!dicTemp.ContainsKey (srcList [i].num))
			{
				dicTemp.Add (srcList [i].num, srcList [i]);
			}
		}

		List<Monster> finalList = new List<Monster> ();
		foreach (KeyValuePair<int, Monster> itor in dicTemp)
		{
			finalList.AddRange (dicMon [itor.Key].ToArray ());
		}
		return finalList;
	}


	//设定分页大小
	public int PageSize {
		get;
		set;
	}
	//返回总的页数
	public int TotalPage (short star, SplitType type = SplitType.None)
	{

		if (PageSize == 0)
			return 0;
		else {
			if (StarOfMonster.ContainsKey (star)) {
				switch (type) {
				case SplitType.Split_If_InCurTeam:
					return (int)Math.Ceiling ((StarOfMonster [star].Count - Core.Data.playerManager.RTData.curTeam.SplitByStar (star)) * 1.0f / PageSize);
				case SplitType.Split_If_InTeam:
					return (int)Math.Ceiling ((StarOfMonster [star].Count - Core.Data.playerManager.RTData.SplitByStar (star)) * 1.0f / PageSize);
				case SplitType.None:
					return (int)Math.Ceiling (StarOfMonster [star].Count * 1.0f / PageSize);
				default:
					return (int)Math.Ceiling (StarOfMonster [star].Count * 1.0f / PageSize);
				}
			} else
				return 0;
		}

	}

	/// <summary>
	/// Gets the monster list by star. curPage从0开始
	/// </summary>
	/// <returns>The monster list by star.</returns>
	/// <param name="star">Star.</param>
	/// <param name="curPage">Current page.</param>
	public List<Monster> getMonsterListByStar (short star, int curPage)
	{
		List<Monster> stars = new List<Monster> ();

		if (PageSize > 0) {
			if (StarOfMonster.ContainsKey (star)) {
				int count = StarOfMonster [star].Count;
				List<int> monId = StarOfMonster [star];

				int startPos = curPage * PageSize;
				if (count > startPos) {
					for (; startPos < count; startPos++) {
						Monster mon = getMonsterById (monId [startPos]);
						if (mon != null) {
							stars.Add (mon);
						}
					}

					stars = SortMonList (stars);
				}
			}
		}

		return SortList(stars);
	}

	public List<Monster> GetShenRenHeChSubMon (short star)
	{
		Monster mainMon = TrainingRoomUI.mInstance.m_hechengUI.MainMon;
		Monster[] subData = TrainingRoomUI.mInstance.m_hechengUI.SubData;
		List<Monster> list = new List<Monster> ();
		if (StarOfMonster.ContainsKey (star)) 
		{
			List<int> monList = StarOfMonster [star];
			for (int i = 0; i < monList.Count; i++) {
				Monster mon = getMonsterById (monList [i]);
				if (mon != null && mon.RTData.m_nStage == RuntimeMonster.NORMAL_MONSTER
				    && mon.num == mainMon.num && mon.pid != mainMon.pid && mon.RTData.m_nAttr != mainMon.RTData.m_nAttr && !mon.inTeam) 
				{
					bool bFind = false;
					for (int j = 0; j < subData.Length; j++) {
						if (subData [j] != null) {
							if (subData [j].pid == mon.pid
							    || (subData [j].RTData.m_nAttr == mon.RTData.m_nAttr && subData [j].num == mon.num)) {
								bFind = true;
							}
						}
					}
					if (!bFind) {
						list.Add (mon);
					}
				}
			}
		}
		return SortMonList (list);;
	}

	/// <summary>
	/// 得到合成真忍者的子宠物
	/// </summary>
	/// <returns>The zhen ren he ch sub mon.</returns>
	/// <param name="star">Star.</param>
	/// <param name="num">Number.</param>
	public List<Monster> GetZhenRenHeChSubMon (short star)
	{
		Monster mainMon = TrainingRoomUI.mInstance.m_hechengUI.MainMon;
		Monster[] subData = TrainingRoomUI.mInstance.m_hechengUI.SubData;
		List<Monster> list = new List<Monster> ();
		if (StarOfMonster.ContainsKey (star)) 
		{
			List<int> monList = StarOfMonster [star];
			for (int i = 0; i < monList.Count; i++) 
			{
				Monster mon = getMonsterById (monList [i]);
				if (mon != null && mon.RTData.m_nStage == RuntimeMonster.NORMAL_MONSTER
				    && mon.num == mainMon.num && mon.pid != mainMon.pid && mon.RTData.Attribute == mainMon.RTData.Attribute && !mon.inTeam) 
				{
					bool bFind = false;
					for (int j = 0; j < subData.Length; j++)
					{
						if (subData [j] != null && subData [j].pid == mon.pid) 
						{
							bFind = true;
						}
					}
					if (!bFind)
					{
						list.Add (mon);
					}
				}
			}
		}
		return SortMonList (list);;
	}

	public List<Monster> GetZhenRenHeShenRenSub (short star)
	{
		Monster mainMon = TrainingRoomUI.mInstance.m_hechengUI.MainMon;
		Monster subData = TrainingRoomUI.mInstance.m_hechengUI.SubData[0];

		List<Monster> list = new List<Monster> ();
		if (StarOfMonster.ContainsKey (star)) {
			List<int> monList = StarOfMonster [star];
			for (int i = 0; i < monList.Count; i++) {
				Monster mon = getMonsterById (monList [i]);
				if (mon != null && mon.RTData.m_nStage == RuntimeMonster.ZHEN_MONSTER
				    && mon.num == mainMon.num && mon.pid != mainMon.pid && mon.RTData.m_nAttr != mainMon.RTData.m_nAttr && !mon.inTeam) 
				{
					if(subData != null) 
					{
					   	if(subData.pid != mon.pid)
						{	
							list.Add (mon);
						}
					}
					else
					{
						list.Add (mon);
					}
				}
			}
		}
		return list;
	}

	/// <summary>
	/// 得到所有相同num的宠物
	/// </summary>
	/// <returns>The all monster by number.</returns>
	/// <param name="star">Star.</param>
	/// <param name="num">Number.</param>
	public List<Monster> GetAllMonsterByNum (int num)
	{
		List<Monster> list = new List<Monster> ();
		foreach (KeyValuePair<int, Monster> itor in mBagOfMonster)
		{
			if (itor.Value.num == num)
			{
				list.Add (itor.Value);
			}
		}
			
		return list;
	}

	/// <summary>
	/// 根据宠物级别得到宠物
	/// </summary>
	/// <returns>The mon list by stage.</returns>
	/// <param name="stage">宠物级别   普通 真  神.</param>
	public List<Monster> GetMonListByStage (int star, int stage)
	{
		List<Monster> list = new List<Monster> ();
		if (StarOfMonster.ContainsKey (star)) {
			List<int> monList = StarOfMonster [star];
			for (int i = 0; i < monList.Count; i++) {
				Monster mon = getMonsterById (monList [i]);
				if (mon != null && mon.RTData.m_nStage == stage) {
					list.Add (mon);
				}
			}
		}
		list = SortMonList (list);
		return SortList(list);
	}

	//
	public List<Monster> GetHechengMon(int star, int num, MonsterAttribute attr)
	{
		List<Monster> list = new List<Monster>();
		foreach (KeyValuePair<int, Monster> itor in mBagOfMonster)
		{
			if (itor.Value.num == num && itor.Value.RTData.Attribute == attr && !itor.Value.inTeam && itor.Value.Star == star)
			{
				list.Add (itor.Value);
			}
		}
		return list;
	}

	//根据宠物界别，得到可以合成的宠物
	public List<Monster> GetHeChengMonByStage(short star, int stage)
	{
		List<Monster> list = new List<Monster> ();
		if (StarOfMonster.ContainsKey (star)) 
		{
			List<int> monList = StarOfMonster [star];
			for (int i = 0; i < monList.Count; i++) 
			{
				Monster mon = getMonsterById (monList [i]);
				if (mon != null && mon.RTData.m_nStage == stage && mon.config.hecheng == 1) 
				{
					list.Add (mon);
				}
			}
		}
		list = SortMonList (list);
		return SortList(list);
	}

	public int getMonsterNextLvExp(int start, int lv)
	{
		for(int i = 0; i < this.monsterLvExpConfig.Count; i++)
		{
			MonsterLvExp mle = monsterLvExpConfig[i];
			if(mle.equipLevel == lv)
			{
				if (start == 1)
				{
					return mle.star1;
				}
				else if (start == 2)
				{
					return mle.star2;
				}
				else if (start == 3)
				{
					return mle.star3;
				}
				else if (start == 4)
				{
					return mle.star4;
				}
				else if (start == 5)
				{
					return mle.star5;
				}
				else if (start == 6)
				{
					return mle.star6;
				}
			}
		}
		return 0;
	}

	public int GetMonTotalExp(int Lv, int star)
	{
		int totalExp = 0;
		for(int i = 0;  i < monsterLvExpConfig.Count; i++)
		{
			int addExp = 0;
			if(monsterLvExpConfig[i].equipLevel < Lv)
			{
				switch(star)
				{
					case 1:
						addExp = monsterLvExpConfig[i].star1;
						break;
					case 2:
						addExp = monsterLvExpConfig[i].star2;
						break;
					case 3:
						addExp = monsterLvExpConfig[i].star3;
						break;
					case 4:
						addExp = monsterLvExpConfig[i].star4;
						break;
					case 5:
						addExp = monsterLvExpConfig[i].star5;
						break;
					case 6:
						addExp = monsterLvExpConfig [i].star6;
						break;
				}
				totalExp += addExp;
			}
		}
		
		return totalExp;
	}


	public int GetMonLevel(int totalExp, int star)
	{
		for(int i = 1;  i < monsterLvExpConfig.Count; i++)
		{
			int curToalExp = GetMonTotalExp(i, star);
			int nextTotalExp = GetMonTotalExp(i+1, star);
			if(totalExp >= curToalExp && totalExp < nextTotalExp)
			{
				return i;
			}
		}
		return monsterLvExpConfig.Count;
	}

	#region 排序

	public class MonIDCompare : IComparer<Monster>
	{
		public int Compare (Monster x, Monster y)
		{
			return (y.RTData.curLevel - x.RTData.curLevel);
		}
	}

	public class MonAttrCompare : IComparer<Monster>
	{
		public int Compare(Monster x, Monster y)
		{
			return (y.RTData.curLevel - x.RTData.curLevel) * 10 + (y.RTData.m_nStage - x.RTData.m_nStage);
		}
	}

	#endregion


	private List<Monster> SortList(List<Monster> monList)
	{
		List<Monster> list = new List<Monster>();
		for (int i = 0 ; i < monList.Count; i++)
		{
			if(monList[i].inTeam)
			{
				list.Add(monList[i]);
			}
		}
		for (int i = 0 ; i < monList.Count; i++)
		{
			if(!monList[i].inTeam)
			{
				list.Add(monList[i]);
			}
		}

		return list;
	}

	public void DecomposeMonster(BaseHttpRequest request, BaseResponse response)
	{                           
		Utils.Assert (request == null || response == null, "Parameter can't be null.");
		if (response.status != BaseResponse.ERROR)
		{
			if (request.baseType == BaseHttpRequestType.Common_Http_Request) 
			{
				HttpRequest req = request as HttpRequest;
				DecomposelMonsterParam param = req.ParamMem as DecomposelMonsterParam;
				delMonsterByIds(param.roles);
			}
		}
	}

	public List<MonsterData> GetRandMonConfigList()
	{
		List<MonsterData> srcList = new List<MonsterData> ();
		foreach(KeyValuePair<int, MonsterData> itor in mConfig)
		{
			if (itor.Value.star >= 3 && itor.Value.ID < 10200)
			{
				srcList.Add (itor.Value);
			}
		}
		return srcList;
	}

	public void BuyItemSuc(BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR)
		{
			if (response is QiangDuoGoldBuyItemResponse) {
				QiangDuoGoldBuyItemResponse buyitemResp = response as QiangDuoGoldBuyItemResponse;
				if (buyitemResp != null && buyitemResp.data != null) 
				{
					addMonster (buyitemResp.data.p);
				}
			}
			else if (response is ZhanGongBuyItemResponse) {
				ZhanGongBuyItemResponse buyitemResp1= response as ZhanGongBuyItemResponse;
				if (buyitemResp1 != null && buyitemResp1.data != null) 
				{
					addMonster (buyitemResp1.data.p);
				}
			}
			else if (response is GetVipLevelRewardResponse) {
				GetVipLevelRewardResponse resp = response as GetVipLevelRewardResponse;
				if (resp != null && resp.data != null) 
				{
					addMonster(resp.data.p);
				}
			}
			else if (response is GetActivityLimittimeRewardResponse) {
				GetActivityLimittimeRewardResponse resp = response as GetActivityLimittimeRewardResponse;
				if (resp != null && resp.data != null) 
				{
					addMonster(resp.data.p);
				}
			}
			else if(response is BuyItemResponse)
			{
				BuyItemResponse resp = response as BuyItemResponse;
				if (resp != null && resp.data != null)
				{
					if (resp.data.Result != null && resp.data.Result.p != null)
					{
						addMonster (resp.data.Result.p);
					}
					else if (resp.data.p != null)
					{
						addMonster (resp.data.p);
					}
				}
			}



		}
	}

	public bool IsExpMon(int monId)
	{
		if(monId == GOLD_EXP_PIG || monId == SILVER_EXP_PIG)
		{
			return true;
		}
		return false;
	}

	//根据配表的星级和要升级的星级，获取升星花费战魂
	public int GetUpCostBattleSoul(int configStar, int upStar)
	{
		if (upStar > configStar && upStar <= 6)
		{
			if (mdicStarData.ContainsKey (configStar))
			{
				MonStarData data = mdicStarData [configStar];
				return data.up_SoulCost [upStar - configStar];
			}
		}
		return 0;
	}

	//根据配表的星级和要升级的星级，获取升星花费金币
	public int GetUpCostCoin(int configStar, int upStar)
	{
		if (upStar > configStar && upStar <= 6)
		{
			if (mdicStarData.ContainsKey (configStar))
			{
				MonStarData data = mdicStarData [configStar];
				return data.up_CoinCost [upStar - configStar];
			}
		}
		return 0;
	}

	//根据配表的星级和要升级的星级，获取升星攻击成长参数
	public float GetMonUpAtkParam(int configStar, int upStar)
	{
		if (upStar > configStar && upStar <= 6)
		{
			if (mdicStarData.ContainsKey (configStar))
			{
				MonStarData data = mdicStarData [configStar];
				return data.up_AtkParam [upStar - configStar];
			}
		}
		return 1;
	}

	//根据配表的星级和要升级的星级，获取升星防御成长参数
	public float GetMonUpDefParam(int configStar, int upStar)
	{
		if (upStar > configStar && upStar <= 6)
		{
			if (mdicStarData.ContainsKey (configStar))
			{
				MonStarData data = mdicStarData [configStar];
				return data.up_DefParam [upStar - configStar];
			}
		}
		return 1;
	}

	//根据配表的星级和要升级的星级，获取分解得到的战魂
	public int GetBattleSoulByResolve(int configStar, int upStar)
	{
		if (upStar >= configStar && upStar <= 6)
		{
			if (mdicStarData.ContainsKey (configStar))
			{
				MonStarData data = mdicStarData [configStar];
				return data.resolve_SoulGet [upStar - configStar];
			}
		}
		return 0;
	}

	//根据配表的星级和要升级的星级，获取分解消耗的金币
	public int GetResolveCostCoin(int configStar, int upStar)
	{
		if (upStar >= configStar && upStar <= 6)
		{
			if (mdicStarData.ContainsKey (configStar))
			{
				MonStarData data = mdicStarData [configStar];
				return data.resolve_CoinCost [upStar - configStar];
			}
		}
		return 0;
	}

	public int GetValidMonCount(SplitType spType)
	{
		int count = 0;
		foreach (KeyValuePair<int, Monster> itor in mBagOfMonster)
		{
			if (IsExpMon (itor.Value.config.ID))
				continue;
			switch (spType)
			{
				case SplitType.None:
					count++;
					break;
				case SplitType.Split_If_InCurTeam:
					if (!itor.Value.inTeam)
					{
						count++;
					}
					else
					{
						if (!Core.Data.playerManager.RTData.curTeam.inMyTeam (itor.Value.pid))
						{
							count++;
						}
					}
					break;
				case SplitType.Split_If_InTeam:
					if (!itor.Value.inTeam)
						count++;
					break;
			}
		}
		return count;
	}

	public void ClearData()
	{
		mBagOfMonster.Clear ();
		StarOfMonster.Clear ();
	}
}



