using System;
using System.Collections.Generic;

public class BuildingManager : Manager, ICore {
	#region ICore implementation
	public void Dispose ()
	{
		throw new NotImplementedException ();
	}
	public void Reset ()
	{
		throw new NotImplementedException ();
	}
	public void OnLogin (object obj)
	{
		throw new NotImplementedException ();
	}
	#endregion

	//建筑状态
	public const short BUILD_LOCK = 0;			//未解锁
	public const short BUILD_UNCREATE = 1;		//未创建
	public const short BUILD_CREATED = 2;		//已创建
	
	//building Num as Key
	private readonly List<BaseBuildingData> listBuildingConfig = null;
	//当前的建筑物信息
	private Dictionary<int, Building> BagOfBuiling = null;
	
	//added by zhangqiang at 2014-3-19
	//main key as build id
	//sub main key as build level
	private Dictionary<int, Dictionary<int, BaseBuildingData>> dicBuildConfig;

	//限制等级
	private Dictionary<int, List<BaseBuildingData>> dicLimitLvBuild;
	//end

	public BuildingManager () {
		listBuildingConfig = new List<BaseBuildingData>();
		BagOfBuiling = new Dictionary<int, Building> ();

		dicBuildConfig = new Dictionary<int, Dictionary<int, BaseBuildingData>>();
		dicLimitLvBuild = new Dictionary<int, List<BaseBuildingData>>();
	}


	private bool m_bZhaoMuUnlock;
	public bool ZhaoMuUnlock
	{
		get
		{
			//return m_bZhaoMuUnlock;
			#if NewGuide
			return ( Core.Data.playerManager.RTData.curTeam.getMember (1) != null || Core.Data.newDungeonsManager.lastFloorId >= 60104);
			#else
			return m_bZhaoMuUnlock;
			#endif
//			return ( Core.Data.playerManager.RTData.curTeam.getMember (1) != null || Core.Data.newDungeonsManager.lastFloorId >= 60104);
		}
		set
		{
			m_bZhaoMuUnlock = value;
		}
	}

	public void UpdateZhaoMu()
	{
		if (TeamUI.mInstance != null)
		{
			TeamUI.mInstance.RefreshSlot (1);
		}
		if (BuildScene.mInstance != null)
		{
			BuildScene.mInstance.UpdateBuildByNum (BaseBuildingData.BUILD_ZHAOMU);
		}
	}

	//本地读取文件
	public override bool loadFromConfig () 
	{
		if(!base.readFromLocalConfigFile<BaseBuildingData>(ConfigType.Building, listBuildingConfig)) {
			RED.LogWarning("fail to read build config file!");
			return false;
		}

		//clear dirty data
		dicBuildConfig.Clear();
		dicLimitLvBuild.Clear();

		foreach(BaseBuildingData build in listBuildingConfig) {

			if(build.ID.Contains("_")) {
				string[] strRealID = build.ID.Split('_');
				build.ID = strRealID[0];
			}

			build.id = int.Parse(build.ID);

			Dictionary<int, BaseBuildingData> dic = null;
			if(dicBuildConfig.ContainsKey(build.id))
			{
				dic = dicBuildConfig[build.id];
			}
			else
			{
				dic = new Dictionary<int, BaseBuildingData>();
				dicBuildConfig.Add(build.id, dic);
			}
			dic.Add(build.Lv, build);
		}

		foreach(BaseBuildingData build in listBuildingConfig) {
			List<BaseBuildingData> list = null;
			if(dicLimitLvBuild.ContainsKey(build.limitLevel))
			{
				list = dicLimitLvBuild[build.limitLevel];
			}
			else
			{
				list = new List<BaseBuildingData>();
				dicLimitLvBuild.Add(build.limitLevel, list);
			}
			list.Add(build);
		}
	
		//普通建筑添加到背包中
		AddNormalBuildToBag();
		return true;
	}

	//普通建筑添加到背包中
	private void AddNormalBuildToBag()
	{
        //clear dirty data
        BagOfBuiling.Clear();

		foreach(int id in dicBuildConfig.Keys)
		{
			Building bd = new Building ();
			bd.config = GetConfigByBuildLv (id,1);
			if (bd.config == null)
			{
				RED.LogError (id + "build data is null");
				continue;
			}
			bd.RTData = new BuildingTeamInfo ();
			bd.RTData.num = bd.config.id;

			BagOfBuiling.Add (id, bd);
		}
	}

	//网络消息解析
	public override void fullfillByNetwork (BaseResponse response) 
	{

		if(response != null && response.status != BaseResponse.ERROR) 
		{
			LoginResponse loginResp = response as LoginResponse;
			if(loginResp != null && loginResp.data != null && loginResp.data.buliding != null) 
			{
				BuildingTeamInfo[] arryBuild = loginResp.data.buliding;
				if(arryBuild.Length == 0)
					return;

				AddNormalBuildToBag();
				foreach(BuildingTeamInfo buildInfo in arryBuild) 
				{
					if(buildInfo != null) 
					{
						Building build = new Building ();
						build.config = GetConfigByBuildLv (buildInfo.num, buildInfo.lv);
						build.RTData = buildInfo;
						build.fTime = DateHelper.UnixTimeStampToDateTime ( Core.TimerEng.curTime + build.RTData.dur);
					
						Building bds = GetBuildFromBagByNum (buildInfo.num);
						if (bds != null) {
							//bds = build;
							BagOfBuiling [buildInfo.num] = build;
						} else {
							BagOfBuiling.Add (build.RTData.num, build);
						}

//						bool bFind = false;
//						for(int i = 0; i < BagOfBuiling.Count; i++)
//						{
//							if(BagOfBuiling[i].RTData.num == build.RTData.num)
//							{
//								BagOfBuiling[i] = build;
//								bFind = true;
//								break;
//							}
//						}
//
//						if(!bFind)
//						{
//							BagOfBuiling.Add(build);
//						}

					}
				}

			}
		}
	}


	//根据等级得到解锁建筑
	public List<BaseBuildingData> GetLockBuildByLevel(int level)
	{
		if(dicLimitLvBuild.ContainsKey(level))
		{
			return dicLimitLvBuild[level];
		}
		return null;
	}

	//建筑是否解锁
	public bool IsBuildUnLock(int buildNum, int level)
	{
		for (int i = 0; i <= level; i++)
		{
			List<BaseBuildingData> list = GetLockBuildByLevel (i);
			if (list != null)
			{
				foreach (BaseBuildingData data in list)
				{
					if (data.id == buildNum)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	//根据ID从背包得到建筑
	public Building GetBuildFromBagByID(int buildID)
	{
		foreach(Building build in BagOfBuiling.Values)
		{
			if(build.RTData.id == buildID)
			{
				return build;
			}
		}

		return null;
	}

	public Building GetBuildFromBagByNum(int buildnum)
	{
		if (BagOfBuiling.ContainsKey (buildnum))
			return BagOfBuiling [buildnum];

		return null;

//		foreach(Building build in BagOfBuiling)
//		{
//			if (build == null || build.config == null)
//			{
//				RED.LogWarning ("buildnum :: " + buildnum);
//			}
//			if(build.config.id == buildnum)
//			{
//				return build;
//			}
//		}
//
//		RED.LogWarning ("GetBuildFromBagByNum :: build not find in build bag :: " + buildnum);
//		RED.LogWarning ("build bag count :: " + BagOfBuiling.Count);
//		foreach(Building build in BagOfBuiling)
//		{
//			RED.LogWarning (build.config.ID.ToString ());
//		}
//		return null;
	}

	//根据建筑ID得到建筑config信息
	public BaseBuildingData GetConfigByBuildLv(int num, int level)
	{
		Dictionary<int, BaseBuildingData> dic = null;
		if (dicBuildConfig.ContainsKey (num))
		{
			dic = dicBuildConfig [num];
			if (dic.ContainsKey (level))
			{
				return dic [level];
			}
		}
		RED.LogWarning ("not find build nun : level" + num.ToString () + "  :  " + level.ToString ());
		return null;
	}

	//得到建筑解锁等级
	public int GetBuildUnlockLevel(int num, int buildLv)
	{
		if (dicBuildConfig.ContainsKey (num))
		{
			Dictionary<int, BaseBuildingData> dic = dicBuildConfig [num];
			if (dic.ContainsKey (buildLv))
			{
				return dic [buildLv].limitLevel;
			}
		}
		return 0;
	}

	//得到该建筑最大升级级数
	public int GetBuildMaxLvl(int buildNum)
	{
		if (dicBuildConfig.ContainsKey (buildNum))
		{
			return dicBuildConfig [buildNum].Count;
		}
		return 0;
	}

	public bool CanLvlUp(int num, int lvl)
	{
		int nextLvl = lvl + 1;
		int maxLvl = GetBuildMaxLvl (num);
		if (lvl - 1 >= maxLvl)
		{
			return false;
		}
		nextLvl = nextLvl > maxLvl ? maxLvl : nextLvl;

		int limitLvl = 1;

		if (dicBuildConfig.ContainsKey(num))
		{
			if (dicBuildConfig [num].ContainsKey (lvl))
			{
				limitLvl = dicBuildConfig [num] [lvl].limitLevel;

				if (Core.Data.playerManager.RTData.curLevel >= limitLvl)
				{
					return true;
				}
			}
		}

		return false;
	}
		
	#region 网络消息
	public void BuildUpgrade(BaseHttpRequest request, BaseResponse reponse)
	{
		RED.Log ("build mgr              BuildUpgrade");
		if (reponse != null && reponse.status != BaseResponse.ERROR)
		{
			BuildOperateResponse resp = reponse as BuildOperateResponse;

//			Building build = GetBuildFromBagByID (resp.data.id);
			Building build = GetBuildFromBagByNum (resp.data.num);
			if (build != null)
			{
				build.RTData.lv = resp.data.lv;
				build.config = GetConfigByBuildLv (build.RTData.num, build.RTData.lv);

				build.RTData.dur = resp.data.dur;
				build.fTime = DateHelper.UnixTimeStampToDateTime (Core.TimerEng.curTime + build.RTData.dur);

//				bool bfind = false;
//				for (int i = 0; i < BagOfBuiling.Count; i++)
//				{
//					if (BagOfBuiling [i].RTData.id == build.RTData.id)
//					{
//						BagOfBuiling [i] = build;
//						bfind = true;
//					}
//				}



				Core.Data.playerManager.RTData.curCoin += resp.data.coin;
				Core.Data.playerManager.RTData.curStone += resp.data.stone;

//				if (!bfind)
//				{
//					RED.LogWarning ("BuildCreate :: build is wrong" + build.RTData.num + " : " + build.RTData.id);
//				}
			}
			else
			{
				RED.LogWarning ("BuildUpgrade :: not find build num :: " + resp.data.num);
			}
		}
	}

	public void BuildGet(BaseHttpRequest request, BaseResponse reponse)
	{
		if (reponse != null && reponse.status != BaseResponse.ERROR)
		{
			BuildOperateResponse resp = reponse as BuildOperateResponse;
			if (resp != null && resp.data != null)
			{
//				Building build = GetBuildFromBagByID (resp.data.id);
				Building build = GetBuildFromBagByNum (resp.data.num);
				if (build != null)
				{
					build.RTData.dur = resp.data.dur;

					build.fTime = DateHelper.UnixTimeStampToDateTime (Core.TimerEng.curTime + build.RTData.dur);
					build.RTData.openType = resp.data.openType;
				}
				else
				{
					RED.LogWarning ("build get ::  not find build :: " + resp.data.num);
				}


//				bool bfind = false;
//				for (int i = 0; i < BagOfBuiling.Count; i++)
//				{
//					if (BagOfBuiling [i].RTData.id == build.RTData.id)
//					{
//						BagOfBuiling [i] = build;
//						bfind = true;
//					}
//				}

				Core.Data.playerManager.RTData.curCoin += resp.data.coin;
				Core.Data.playerManager.RTData.curStone += resp.data.stone;

			}
		}
	}


//	public void BuildCreate(BaseHttpRequest request, BaseResponse reponse)
//	{
//		RED.Log ("buld mgr :     BuildCreate");
//
//		if (reponse != null && reponse.status != BaseResponse.ERROR)
//		{
//			BuildOperateResponse resp = reponse as BuildOperateResponse;
//
//			if (resp.data.bd != null)
//			{
//				Building build = new Building ();
//				build.config = GetConfigByBuildLv (resp.data.bd.num, resp.data.bd.lv);
//				build.RTData = resp.data.bd;
//
//				build.Type = BUILD_CREATED;
//				build.fTime = new DateTime[build.RTData.dur.Length];
//				for (int j = 0; j < build.RTData.dur.Length; j++)
//				{
//					build.fTime [j] = DateHelper.UnixTimeStampToDateTime ( Core.TimerEng.curTime + build.RTData.dur [j]);
//				}
//
//				bool bfind = false;
//				for (int i = 0; i < BagOfBuiling.Count; i++)
//				{
//					if (BagOfBuiling [i].RTData.num == build.RTData.num)
//					{
//						BagOfBuiling [i] = build;
//						bfind = true;
//					}
//				}
//
//				if (!bfind)
//				{
//					RED.LogWarning ("BuildCreate :: build is wrong" + build.RTData.num + " : " + build.RTData.id);
//				}
//
//				if (!Core.Data.guideManger.isGuiding)
//				{
//					if (build.config.CostCoin > 0)
//					{
//						Core.Data.playerManager.RTData.curCoin -= build.config.CostCoin;
//					}
//					else if (build.config.CostStone > 0)
//					{
//						Core.Data.playerManager.RTData.curStone -= build.config.CostStone;
//
//                    //    Core.Data.ActivityManager.OnPurchaseVirtualCurrency (ActivityManager.BuildOpenType,1,build.config.CostStone);
//
//					}
//					if (Core.Data.playerManager.RTData.curCoin < 0)
//					{
//						Core.Data.playerManager.RTData.curCoin = 0;
//					}
//					if (Core.Data.playerManager.RTData.curStone < 0)
//					{
//						Core.Data.playerManager.RTData.curStone = 0;
//					}
//				}
//			}
//			else
//			{
//				RED.LogWarning ("BuildCreate :: build data is null");
//			}
//		}
//	}

	//建筑物开启
	public void BattleBuildOpen(BaseHttpRequest request, BaseResponse reponse)
	{
		if(reponse != null && reponse.status != BaseResponse.ERROR)
		{
			BuildOperateResponse resp = reponse as BuildOperateResponse;

			Building build = Core.Data.BuildingManager.GetBuildFromBagByID(resp.data.id);
			if (build != null) {
				build.RTData = resp.data;
				build.RTData.dur = resp.data.dur;
				build.fTime = DateHelper.UnixTimeStampToDateTime (Core.TimerEng.curTime + build.RTData.dur);
				build.RTData.openType = resp.data.openType;
			}
			Core.Data.playerManager.RTData.curCoin += resp.data.coin;
			Core.Data.playerManager.RTData.curStone += resp.data.stone;

			Core.Data.itemManager.UseItem (Core.Data.itemManager.GetBagItemPid(resp.data.propId),Math.Abs( resp.data.propNum));
		}
	}

	//建筑攻击值
	public float GetBuildAtk()
	{
		Building atk = GetBuildFromBagByNum (BaseBuildingData.BUILD_BATTLE);
		if (atk != null)
		{
			if (atk.RTData != null)
			{
				if (atk.RTData.dur > 0)
				{
					return atk.config.effect[0];
				}
			}
		}

		return 0;
	}

	//建筑防御值
	public float GetBuildDef()
	{
		Building atk = GetBuildFromBagByNum (BaseBuildingData.BUILD_BATTLE);
		if (atk != null)
		{
			if (atk.RTData != null)
			{
				if (atk.RTData.dur > 0)
				{
					return atk.config.effect[1];
				}
			}
		}

		return 0;
	}
		
	#endregion

}
