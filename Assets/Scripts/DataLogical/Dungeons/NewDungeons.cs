using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//新副本章节数据
public class NewChapter : DungeonsData
{
    public NewChapterData config;

    public NewChapter()
    {
    }

    public Color color
    {
        get { return new Color(config.MapColor[0] / 255f, config.MapColor[1] / 255f, config.MapColor[2] / 255f, config.MapColor[3] / 255f); }
    }

    public Vector3 pathPosition
    {
        get { return new Vector3(config.pathPosition[0], -config.pathPosition[1], 0); }
    }
}
//新副本小关数据
public class NewPVEReward
{
    public int pid;
    public int num;
}

public enum NewFloorState
{
    Unlocked,
    Current,
    Pass,
}

public class NewFloor
{
    public int star;
    public NewFloorData config;
	public int passTimes;			//已通关次数
	public int resetTimes;			//重置次数

    public NewFloor()
    {
        state = NewFloorState.Unlocked;
    }

    public Vector3 localPosition
    {
        get {
			Vector3 v = Vector3.zero;
			if(config.Pos!= null && config.Pos.Length > 1)
			{
				v.x = config.Pos[0];
				v.y = - config.Pos[1];
			}
			return v;
		}
    }
	//是否是BOSS关卡
    public bool isBoss
    {
        get { return System.Convert.ToBoolean(config.isBoss); }
    }

    NewPVEReward[] _rewards;
    //客户端显示本关可能掉落的物品
    public NewPVEReward[] Rewards
    {
        get
        {
            if (_rewards == null)
            {
                List<NewPVEReward> l = new List<NewPVEReward>();
                foreach (int[] p in config.Reward)
                {
                    NewPVEReward r = new NewPVEReward();
                    r.pid = p[0];
                    r.num = p[1];
                    l.Add(r);
                }
                _rewards = l.ToArray();
            }
            return _rewards;
        }
    }
	//当前关卡状态
    public NewFloorState state{ get; set; }
	//所属章节ID
    public int BelongChapterID;
}
//新副本管理器
public class NewDungeonsManager : Manager
{
    private readonly  Dictionary<int, NewChapterData> ChapterConfigList = null;
    private  readonly Dictionary<int, NewFloorData> FloorConfigList = null;
	//特殊副本配置信息
	private readonly Dictionary<int, ExploreConfigData> exploreConfigList = null;
	//重置副本购买信息
	private readonly Dictionary<int, ResetFloor> dicResetFloor = null;
    
	public Dictionary<int,NewChapter> ChapterList = null;
    public Dictionary<int,NewFloor> FloorList = null;
    public Dictionary<int, List<int>> floorAndRewardDic = null;
    //反向分析 
    //private readonly Dictionary<int, int> reverse = null;


    public NewFloor curFightingFloor{ get; set; }

    public  int startFloorID = 60100;
    public int startChapterID = 30100;
	public int endChapterID = 0;
    //默认打的剧情
    public string curFightingFBType = "PVEType_Plot";
    //最后打过的的最大子关卡ID
    public int lastFloorId{ get; set; }
	
	//PVE副本倒计时<登陆的时候和战半结束的时候>
	public ExplorDoors explorDoors ;
		
    //构造初始化
    public NewDungeonsManager()
    {
        ChapterConfigList = new Dictionary<int, NewChapterData>();
        FloorConfigList = new Dictionary<int, NewFloorData>();

		exploreConfigList = new Dictionary<int, ExploreConfigData> ();
		dicResetFloor = new Dictionary<int, ResetFloor> ();

        floorAndRewardDic = new Dictionary<int, List<int>>();
        //reverse = new Dictionary<int, int>();

    }
	
	
	
    //读表初始化
    public override bool loadFromConfig()
    {
        bool success = base.readFromLocalConfigFile<NewChapterData>(ConfigType.NewChapter, ChapterConfigList)
                 | base.readFromLocalConfigFile<NewFloorData>(ConfigType.NewFloor, FloorConfigList)
			| base.readFromLocalConfigFile<ExploreConfigData>(ConfigType.Explore, exploreConfigList)
				| base.readFromLocalConfigFile<ResetFloor>(ConfigType.ResetFloor, dicResetFloor);		

        return success;
    }
    //网络初始化
    public override void fullfillByNetwork(BaseResponse response)
    {
        if (response != null && response.status != BaseResponse.ERROR)
        {
            LoginResponse loginResp = response as LoginResponse;
            if (loginResp != null && loginResp.data != null && loginResp.data.floor != null)
            {
				InitData();
				
                lastFloorId = loginResp.data.floor.end;
                if (lastFloorId == 0)
                    lastFloorId = startFloorID;

                DungeonsInfo dungeonse = loginResp.data.floor;
                if (dungeonse != null && dungeonse.pro != null)
                {
                    foreach (int[] oneFloor in dungeonse.pro)
                    {
                        int floorId = oneFloor[0];
                        int star = oneFloor[1];
                        NewFloor floor = null;
                        if (FloorList.TryGetValue(floorId, out floor))
                            floor.star = star;
                    }
                }
				
                //设置各个小关卡的状态
                NewFloor floordata = null;
                int floorid = startFloorID;
                for (; floorid <= lastFloorId; floorid++)
                {
                    if (FloorList.TryGetValue(floorid, out floordata))
                    {
                        floordata.state = NewFloorState.Pass;
                    }
                }

                Debug.Log("CurrentID=" + floorid.ToString());
                //设置开启的下一关<当前关卡>				
                if (FloorList.TryGetValue(floorid, out floordata))
                {
                    floordata.state = NewFloorState.Current;
                }
				
				//开启PVE副本倒计时
				if(loginResp.data.explorDoors != null)
				{
					explorDoors = loginResp.data.explorDoors;
					AsyncTask.QueueOnMainThread( () =>
				    {
					    JCPVETimerManager.Instance.AutoOpenPVESystemTimer();
					});
				}

				Analysis();
               // AnaylizeFloor();
            }

			//挑战次数和重置次数
			if (loginResp != null && loginResp.data != null && loginResp.data.doorDayStatus != null)
			{
				for (int i = 0; i < loginResp.data.doorDayStatus.Length; i++)
				{
					DoorDayStatus doorStatus = loginResp.data.doorDayStatus [i];
					NewFloor newflr = GetFloorData (doorStatus.doorId);
					if (newflr != null)
					{
						newflr.passTimes = doorStatus.passCount;
						newflr.resetTimes = doorStatus.resetCount;
					}
					else
					{
						RED.LogWarning (doorStatus.doorId + "not find floor data");
					}
				}
			}
        }

		//更新招募和位置2是否解锁
		PlayerManager playerMgr = Core.Data.playerManager;
		if (playerMgr.RTData != null && playerMgr.RTData.curTeam != null)
			Core.Data.BuildingManager.ZhaoMuUnlock = (playerMgr.RTData.curTeam.getMember (1) != null); //|| lastFloorId >= 60104);
    }

	//同步pve副本时间
	public void SyncPveData(BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR)
		{
			SyncPveResponse resp = response as SyncPveResponse;
			if (resp != null && resp.data != null)
			{
				//开启PVE副本倒计时
				explorDoors = resp.data;
			}
		}
	}

	public void ResetFloor(BaseHttpRequest request, BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR)
		{
			HttpRequest req = request as HttpRequest;
			ResetFloorParam param = req.ParamMem as ResetFloorParam;

			ResetFloorResponse resp = response as ResetFloorResponse;
			NewFloor flr = GetFloorData (param.doorId);
			flr.passTimes = 0;
			flr.resetTimes = resp.data.resetCount;

			Core.Data.playerManager.RTData.curStone += resp.data.stone;
		}
	}


	public ExploreConfigData GetExploreData(int type)
	{
		if (exploreConfigList.ContainsKey (type))
		{
			return exploreConfigList [type];
		}
		return null;
	}

	public NewFloor GetFloorData(int floorID)
	{
		if (FloorList.ContainsKey (floorID))
		{
			return FloorList [floorID];
		}
		return null;
	}

    //用于获取小关卡 数据
	public NewFloorData GetFloorConfigData(int num) {
        NewFloorData fd = null;
        if(!FloorConfigList.TryGetValue(num, out fd)) 
            fd = null;
        return fd;
    }

	//重置关卡需要花费的钻石
	public int GetResetFloorCost(int resetTime)
	{
		RED.LogWarning ("resetTime :: " + resetTime);

		RED.LogWarning ("count  " + dicResetFloor.Count);
		if (dicResetFloor.ContainsKey (resetTime))
		{
			RED.LogWarning ("costStone :: " + dicResetFloor [resetTime].cost_D);
			return dicResetFloor [resetTime].cost_D;
		}
		return 0;
	}

    void InitData()
    {
        //转换完整章节信息结构<包含网络信息>
        ChapterList = new Dictionary<int, NewChapter>();
        FloorList = new Dictionary<int, NewFloor>();

        //转换完整小关卡信息结构<包含网络信息>
        foreach (int key in FloorConfigList.Keys)
        {
            NewFloor floor = new NewFloor();
            floor.config = FloorConfigList[key];
            FloorList.Add(key, floor);
        }
		
		int ChapterPlotCount = 0;
        foreach (int key in ChapterConfigList.Keys)
        {
			if(key/10000 ==3)ChapterPlotCount++;
			
            NewChapter chapter = new NewChapter();
            chapter.config = ChapterConfigList[key];		
            ChapterList.Add(key, chapter);
            int[] floorid = chapter.config.floorID;
            foreach (int id in floorid)
            {
                NewFloor floordata = null;
                if (FloorList.TryGetValue(id, out floordata))
                    floordata.BelongChapterID = key;
            }
        }
		
		endChapterID = startChapterID + (ChapterPlotCount-1)*100;
    }

    /// <summary>
    /// 分析副本中的碎片   add by  wxl 
    /// </summary>
    public void Analysis()
    {
		floorAndRewardDic.Clear ();
        foreach (NewFloorData fdata in FloorConfigList.Values)
        {
			if (fdata != null) {
				if (fdata.ID > 0) {
					if (fdata.isBoss == 1) {
						if (fdata.Reward != null && fdata.Reward.Count != 0) {
							for (int i = 0; i < fdata.Reward.Count; i++) {
								if (fdata.Reward [i] [0] != null) {

									if (!floorAndRewardDic.ContainsKey (fdata.Reward [i] [0])) {
										List<int> floorIdList = new List<int> ();
										floorIdList.Add (fdata.ID);
										int tId = fdata.Reward [i] [0];
										if (DataCore.getDataType (fdata.Reward [i] [0]) == ConfigDataType.Frag) {
											SoulData soulConfig = Core.Data.soulManager.GetSoulConfigByNum (fdata.Reward [i] [0]);

											if (soulConfig != null) {
												if (soulConfig.type == (int)ItemType.Monster_Frage) {
													tId = soulConfig.updateId;
												}else if(soulConfig.type == (int)ItemType.Equip_Frage)
													tId = soulConfig.updateId;
											} else {
												RED.LogWarning ("  soul config  is null = " + tId);
												continue;
											}
										} else if (DataCore.getDataType (fdata.Reward [i] [0]) == ConfigDataType.Monster) {
											tId = fdata.Reward [i] [0];
										} else if (DataCore.getDataType (fdata.Reward [i] [0]) == ConfigDataType.Equip) {
											tId = fdata.Reward [i] [0];
										} else {
											RED.Log ("  other type id = " + tId);
										}

										if (!floorAndRewardDic.ContainsKey (tId)) {
											floorAndRewardDic.Add (tId, floorIdList);                   
										} else {
											List<int> tList = new List<int> ();
											floorAndRewardDic.TryGetValue (tId, out tList);
											floorAndRewardDic.Remove (tId);
											tList.Add (fdata.ID);
											floorAndRewardDic.Add (tId, tList);
										}
									} else {
										List<int> tList = new List<int> ();
										floorAndRewardDic.TryGetValue (fdata.Reward [i] [0], out tList);
										floorAndRewardDic.Remove (fdata.Reward [i] [0]);
										tList.Add (fdata.ID);
										floorAndRewardDic.Add (fdata.Reward [i] [0], tList);
									}
								}
							}
						}
					}
				}
			}
        }
    }

    //用于分析掉落
    public List<int> GetFloorIdByGiftId(int giftId){
        List<int> targetId =new List<int>();
        if(giftId != 0){

            if(floorAndRewardDic.TryGetValue(giftId,out targetId)){
                return targetId;
            }
        }
        return targetId;
    }
	

    //反向分析
//    private void AnaylizeFloor() {
//       
//		reverse.Clear();
//        foreach (NewChapterData chData in ChapterConfigList.Values) {
//            if (chData != null) {
//                foreach (int floorId in chData.floorID) {
//                    //    ConsoleEx.Write( " floor id == "+floorId + "     chDataId == " +chData.ID +"  new chapter count  " + ChapterConfigList.Count,"yellow");
//                    reverse.Add (floorId, chData.ID);
//                }
//            }
//        }
//    }
//
//
//    public NewChapterData ReverseToChapter(int floorId) {
//        NewChapterData chapter = null;
//        int ChapterId = 0;
//        if(reverse.TryGetValue(floorId, out ChapterId)){
//            if(!ChapterConfigList.TryGetValue(ChapterId, out chapter)) {
//                chapter = null;
//            }
//        }
//
//        // if(chapter != null)
//        // ConsoleEx.Write(" floor id  == " + floorId  + " Chapter  " + chapter.ID  ,"yellow");
//        return chapter;
//    }
	
}
   

