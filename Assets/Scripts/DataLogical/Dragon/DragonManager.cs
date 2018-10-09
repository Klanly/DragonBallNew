using System;
using System.Collections.Generic;

public class DragonManager : Manager {

	#region 基本数据
	//Key 为奥义的ID(number）
	private readonly Dictionary <int, AoYiData> AoyiConfig = null;
	//混合数据的格式，我会拆分成两个,所以是一个临时数据
	private List<CombineHBData> HugeBeastCombineConfig = null;
	//一个是神龙的升级数据
	private readonly List<UpDragonData> UpDragonConfig = null;
	//一个是神龙祭坛槽的解锁数据
	private readonly List<DragonLockData> LockDragonConfig = null;

	//服务器传来的所有学习过的奥义信息（有的在使用，有的没有使用）,key is ID from server
	public Dictionary<int, AoYi> AoYiDic = null;
	//服务器传来的神龙信息
	public List<Dragon> DragonList = null;

	public long callEarthDragonTime;
	public long callNMKXDragonTime;

	public long mianZhanTime;

	public enum DragonType
	{
		EarthDragon = 0,
        NMKXDragon = 1,
		None,
	};

    public Dictionary<int,int> ballMailDic = null;

	public DragonType currentDragonType = DragonType.EarthDragon;
	public static  bool checkTime = false;

	public DragonManager() {
		AoyiConfig = new Dictionary<int, AoYiData>();
		HugeBeastCombineConfig = new List<CombineHBData>();

		UpDragonConfig = new List<UpDragonData>();
		LockDragonConfig = new List<DragonLockData>();

		AoYiDic = new Dictionary<int, AoYi>();
		DragonList = new List<Dragon>();

		this.callDragonSucceed[(int)DragonType.EarthDragon] = false;
		this.callDragonSucceed[(int)DragonType.NMKXDragon] = false;
	}

	public override bool loadFromConfig () {
		bool success = base.readFromLocalConfigFile<AoYiData>(ConfigType.HugeBeastWish, AoyiConfig) | 
		               base.readFromLocalConfigFile<CombineHBData>(ConfigType.HugeBeastCb, HugeBeastCombineConfig);
		anaylize();
		return success;
	}


	public override void fullfillByNetwork (BaseResponse response) {
		if(response != null && response.status != BaseResponse.ERROR) {
			LoginResponse loginResp = response as LoginResponse;
			if(loginResp != null && loginResp.data != null) {
				//奥义
				if(loginResp.data.aoyi != null) {
                    //clear dirty data
                    AoYiDic.Clear();
					foreach(RTAoYi rt in loginResp.data.aoyi) {
						if(rt != null) {
							AoYi ao = new AoYi(rt, this);
							AoYiDic.Add(ao.ID, ao);
						}
					}
				}

				//神龙
                if(loginResp.data.dragon != null) {
                    DragonList.Clear();

                    Core.TimerEng.deleteTask( new TaskID[] {TaskID.CallEarthDragonTimer, TaskID.CallNMKXDragonTimer} );

                    foreach(DragonInfo di in loginResp.data.dragon) {
                        if(di != null) {
                            Dragon d = new Dragon(di, this, Core.Data.soulManager);
							DragonList.Add(d);
                            if(d.RTData.st != 0) {
								long callDragonEndTime = d.RTData.st + d.RTData.dur;
								long res = Core.TimerEng.curTime - callDragonEndTime;
                               
                                if(res < 0) {
                                    RED.Log("res <0 "+this.callDragonSucceed[(d.RTData.num - 1)]);
									this.callDragonSucceed[(d.RTData.num - 1)] = false;
									d.RTData.st = Core.TimerEng.curTime;
									startCallDragonTimer((DragonType)(d.RTData.num - 1), Core.TimerEng.curTime, callDragonEndTime);

                                } else if(res >= 0) {
									d.RTData.st = 0;
									d.RTData.dur = 0;
                                    RED.Log(" res >=0 "+this.callDragonSucceed[(d.RTData.num - 1)]);
									this.callDragonSucceed[(d.RTData.num - 1)] = true;
								}
							}
						}
					}
				}

				// 免战
				long res1 = Core.TimerEng.curTime - Core.Data.playerManager.RTData.shiled;
				if (res1 < 0) {
					Core.TimerEng.deleteTask (TaskID.DragonMianZhanTimer);

					startMianZhanTimer (Core.TimerEng.curTime, Core.Data.playerManager.RTData.shiled);
				} else {
					Core.TimerEng.deleteTask (TaskID.DragonMianZhanTimer);
					this.mianZhanTime = 0;
				}
			}
		}
	}


	//拆分为两个数据格式
	private void anaylize() {
		if(HugeBeastCombineConfig != null) {
			foreach(CombineHBData chb in HugeBeastCombineConfig) {
				if(chb != null) {
					DragonLockData hugebeastld = chb.toDragonLockData();
					if(hugebeastld != null) {
						LockDragonConfig.Add(hugebeastld);
					} else {
						UpDragonConfig.Add(chb.toUpDragonData());
					}
				}
			}
			HugeBeastCombineConfig.safeFree();
		}
	}
	#endregion

	#region 召唤神龙方法

	public Action<DragonType, CallDragonResponse> CallDragonCompletedDelegate;
	public Action CallDragonErrorDelegate;

	public Action<DragonType, long> callDragonTimerEvent;

	public Action<long> mianZhanTimerEvent;

	public Action<TimerTask> callEarthDragonTimeCompletedDelegate;
	public Action<TimerTask> callNMKXDragonTimeCompletedDelegate;

	public Action<TimerTask> mianZhanTimeCompletedDelegate;
    /// <summary>
    ///  前提是loginInfo中 如果神龙的时间 calldragonsucceed = true  则需要检测 是否召唤成功！每次 召唤神龙结束之后,先检测,如果检测之后 callsucceed  参数还为 true ， 则执行学习奥义；初始为 false   
    /// </summary>
    public bool[] isCheckedCallDragon = new bool[2];
	public bool[] callDragonSucceed = new bool[2];

	public void callDragonRequest(DragonType currentDragonType)
	{
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.CALL_DRAGON, new CallDragonParam(Core.Data.playerManager.PlayerID, ((int)currentDragonType + 1)));

		task.afterCompleted += CallDragonCompleted;
		task.ErrorOccured += CallDragonError;

		//then you should dispatch to a real handler
		task.DispatchToRealHandler ();
	}

	public void CallDragonCompleted(BaseHttpRequest request, BaseResponse response)
	{
		if(response != null && response.status != BaseResponse.ERROR) 
		{
			CallDragonResponse callDragonResponse = response as CallDragonResponse;
		
			if(CallDragonCompletedDelegate != null)
			{
				int callDragonType = ((request as HttpRequest).ParamMem as CallDragonParam).slty - 1;
			
				CallDragonCompletedDelegate((DragonType)callDragonType, callDragonResponse);
			}
		}
		else if(response != null)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
			Core.Data.dragonManager.learnAoYiCompletedDelegate = null;
		}

		ComLoading.Close();
	}

	public void CallDragonError(BaseHttpRequest request, string error)
	{
		ComLoading.Close();
		if(CallDragonErrorDelegate != null)
		{
			CallDragonErrorDelegate();
		}
	}

	public void startCallDragonTimer(DragonType dragonType, long startTime, long endTime)
	{
		TimerTask task = new TimerTask(startTime, endTime, 1, ThreadType.MainThread);

		long leftTime = endTime - startTime;
		leftTime = leftTime <= 0 ? 0 : leftTime;
		if (leftTime > 0 && leftTime < 5) {
			leftTime = 10;
		}

		if(dragonType == DragonType.EarthDragon)
		{
			task.taskId = TaskID.CallEarthDragonTimer;
			this.callEarthDragonTime = leftTime;
			if(callDragonTimerEvent != null)
			{
				callDragonTimerEvent(DragonType.EarthDragon, this.callEarthDragonTime);
			}
		}
		else if(dragonType == DragonType.NMKXDragon)
		{
			task.taskId = TaskID.CallNMKXDragonTimer;
			this.callNMKXDragonTime = leftTime;
			if(callDragonTimerEvent != null)
			{
				callDragonTimerEvent(DragonType.NMKXDragon, this.callNMKXDragonTime);
			}
		}


		task.onEventEnd = callDragonTimeCompleted;

		task.onEvent = (TimerTask t) => 
		{
			if(task.taskId == TaskID.CallEarthDragonTimer)
			{
				this.callEarthDragonTime = t.leftTime;
				if(callDragonTimerEvent != null)
				{
					callDragonTimerEvent(DragonType.EarthDragon, this.callEarthDragonTime);
				}
			}
			else if(task.taskId == TaskID.CallNMKXDragonTimer)
			{
				this.callNMKXDragonTime = t.leftTime;
				if(callDragonTimerEvent != null)
				{
					callDragonTimerEvent(DragonType.NMKXDragon, this.callNMKXDragonTime);
				}
			}
		} ;
		task.DispatchToRealHandler();
	}



    public void CancelCallDTimer(int dType){
        if ((DragonType)dType == DragonType.EarthDragon)
        {
            Core.TimerEng.deleteTask(TaskID.CallEarthDragonTimer);
            this.callEarthDragonTime = 0;
            if(callDragonTimerEvent != null)
            {
                callDragonTimerEvent(DragonType.EarthDragon, this.callEarthDragonTime);
            }
            this.callDragonSucceed[0] = false;
        }
        else
        {
            Core.TimerEng.deleteTask(TaskID.CallNMKXDragonTimer);
            this.callNMKXDragonTime = 0;
            if(callDragonTimerEvent != null)
            {
                callDragonTimerEvent(DragonType.NMKXDragon, this.callNMKXDragonTime);
            }
            this.callDragonSucceed[1] = false;
        }
  
    }
	#endregion


	#region 同步时间
	public void SyncCallDragonTime(){
		GetPayCntParam param = new GetPayCntParam (Core.Data.playerManager.PlayerID);
		HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam (RequestType.SYNC_CALLDRAGONTIME, param);
		task.afterCompleted += BackSyncCallDragonTime;
		task.DispatchToRealHandler ();
		checkTime = true;
	}

	void BackSyncCallDragonTime(BaseHttpRequest request, BaseResponse response){
		if (response != null) {
			SyncCallDragonTimeResponse syncCallResp = response as SyncCallDragonTimeResponse;
			if (syncCallResp.data != null) {
				DragonList.Clear();
				//同步 召唤时间
				Core.TimerEng.deleteTask( new TaskID[] {TaskID.CallEarthDragonTimer, TaskID.CallNMKXDragonTimer} );
				foreach(DragonInfo di in syncCallResp.data.dragons) {
					if(di != null) {
						Dragon d = new Dragon(di, this, Core.Data.soulManager);
						DragonList.Add(d);
						if(d.RTData.st != 0) {
							long callDragonEndTime = d.RTData.st + d.RTData.dur;
							long res = Core.TimerEng.curTime - callDragonEndTime;

							if(res < 0) {
								this.callDragonSucceed[(d.RTData.num - 1)] = false;
								d.RTData.st = Core.TimerEng.curTime;
								startCallDragonTimer((DragonType)(d.RTData.num - 1), Core.TimerEng.curTime, callDragonEndTime);
							} else if(res >= 0) {
								d.RTData.st = 0;
								d.RTData.dur = 0;
								this.callDragonSucceed[(d.RTData.num - 1)] = true;
							}
						}
					}
				}
				
				//同步召唤的龙珠 数量
				SyncDragonBallNum (syncCallResp.data.balls);
			}
		}
	}

	//同步 龙珠数量
	void SyncDragonBallNum(DragonBallItemData[] balls){
		if (balls != null) {
			Core.Data.soulManager.SyncDBSoulNum (balls);
		}
	}
	#endregion

    #region 检测神龙 是否召唤成功
    //检测召唤神龙  请求
    public void ChectCallDragonIsFinish(int type){
        HttpTask task = new HttpTask (ThreadType.MainThread,TaskResponse.Default_Response);
        task.AppendCommonParam (RequestType.GET_CALLDRAGONISFINISH,new GetCallDragonIsFinishParam(int.Parse(Core.Data.playerManager.PlayerID),type));
        task.afterCompleted += BackCheckCallDragon;
        task.ErrorOccured += CheckCallDragonError;
        task.DispatchToRealHandler();
    }
    public void BackCheckCallDragon(BaseHttpRequest request,BaseResponse response){
        if (response != null && response.status != BaseResponse.ERROR)
        {
            GetCallDragonIsFinishedResponse resp = response as GetCallDragonIsFinishedResponse;
            HttpRequest httpRequest = request as HttpRequest;
            GetCallDragonIsFinishParam httpParam = httpRequest.ParamMem as GetCallDragonIsFinishParam;

            if (UIShenLongManager.Instance.isState)
            {
                if (httpParam.type == 1)
                {//地球

                    if (resp.data == false)
                    {

                        callDragonSucceed[0] = false;
                    }
                    else
                    {
                        callDragonSucceed[0] = true;

                    }
                    isCheckedCallDragon[0] = true;

                }
                else if (httpParam.type == 2)
                {
                    if (resp.data == false)
                    {
                        callDragonSucceed[1] = false;
                    }
                    else
                    {
                        callDragonSucceed[1] = true;
                    }
                    isCheckedCallDragon[1] = true;
                }
            }
            //检测结束
            UIShenLongManager.Instance.CheckComplete();
        }
    }

    public void CheckCallDragonError(BaseHttpRequest request, string error){

        HttpRequest httpRequest = request as HttpRequest;
        GetCallDragonIsFinishParam httpParam = httpRequest.ParamMem as GetCallDragonIsFinishParam;
        RED.Log (" call dragon = " + httpParam.type);
        if (httpParam.type == 1) {
            callDragonSucceed [0] = false;
        } else {
            callDragonSucceed [1] = false;
        }
        ActivityNetController.ShowDebug (error);
    }
    #endregion


	#region 方面使用的方法

	public AoYiData getAoYiData (int num) {
		AoYiData result = null;
		if(!AoyiConfig.TryGetValue(num, out result))
			result = null;
		return result;
	}

	public Dictionary <int, AoYiData> getAoYiConfig()
	{
		return this.AoyiConfig;
	}

	/// <summary>
	/// 当前等级的神龙的数据
	/// </summary>
	/// <returns>The next huge beast config.</returns>
	public UpDragonData getDragonConfig(int dragonLv) {
        Utils.Assert(dragonLv <= 0, "Dragon level must greate than or equal to 1");

		UpDragonData result = null;
		foreach(UpDragonData uhbd in UpDragonConfig) {
			if(uhbd != null && uhbd.dragonLevel == dragonLv) {
				result = uhbd;
				break;
			}
		}

        Utils.Assert(result == null, "Dragon level must less than 6");
		return result;
	}

	public DragonLockData getUnLockDragonSlotConfig(int index)
	{
		DragonLockData result = null;

		foreach(DragonLockData dragonLockDataTemp in LockDragonConfig)
		{
			if(dragonLockDataTemp.dragonSlot == (index + 1))
			{
				result = dragonLockDataTemp;
				break;
			}
		}
		return result;
	}

	/// <summary>
	/// Gets the unlock dragon slot. 这个仅仅返回根据等级决定的神龙祭坛的槽数开放到的位置(从0开始）
	/// </summary>
	/// <returns>The unlock dragon slot.</returns>
	/// <param name="playerLv">Player lv.</param>
	public int getUnLockDragonSlot(int playerLv) {
		Utils.Assert(playerLv <= 0, "Player Level must greate than or equal to 1");

		DragonLockData result = null;
		foreach(DragonLockData hbld in LockDragonConfig) {
			if(hbld != null && hbld.type == DragonLockData.PLAYER_LEVEL_TYPE ) {
				if(hbld.num < playerLv) {
					result = hbld;
					continue;
				} else if(hbld.num == playerLv) {
					result = hbld;
					break;
				} else {
					break;
				}
			}
		}
		Utils.Assert(result == null, "player level = " + playerLv);

		return result.dragonSlot - 1;
	}

	/// <summary>
	/// Gets the unlock dragon slot. 这个仅仅返回根据宝石决定神龙祭坛的槽数开放的第5个位置（索引为4）
	/// </summary>
	/// <returns>The un lock dragon slot2.</returns>
	/// <param name="stone">If set to <c>true</c> stone.</param>
	public int getUnLockDragonSlot2(bool stone) {
		DragonLockData result = null;
		foreach(DragonLockData hbld in LockDragonConfig) {
			if(hbld != null && hbld.type == DragonLockData.DIAMOND_TYPE ) {
				result = hbld;
				break;
			}
		}
		Utils.Assert(result == null, "Lock Huge Beast Config is wrong.");
		return result.dragonSlot - 1;
	}

	//获取奥义可以上阵的总数量
	public int AoYiTotalCount {
		get {
			return LockDragonConfig.Count;
		}
	}

    /// <summary>
    /// 学习过的奥义
    /// </summary>
    /// <returns>The to list.</returns>
    public List<AoYi> usedAoYiToList() {
        List<AoYi> used = new List<AoYi>();
        if(AoYiDic != null) {
            foreach(AoYi ao in AoYiDic.Values) {
                used.Add(ao);
            }
        }
        return used;
    }

	/// <summary>
	/// 把使用的奥义按照位置来排列，主要用于UI的显示
	/// </summary>
	/// <returns>The to list.</returns>
	public List<AoYi> usedToList() {
		List<AoYi> used = new List<AoYi>();

		for(int i = 0; i < AoYiTotalCount; ++ i) {
			used.Add(null);
		}

		if(AoYiDic != null) {
			foreach(AoYi ao in AoYiDic.Values) {
                if (ao.Pos >= 0){
                    used[ao.Pos] = ao;
                }
			}
		}

		return used;
	}

    //返回攻击值加成的AOYI参数
    //返回 0.0f
    public float getAttAoYi() {
        float ened = 0.0f;

        //偷袭奥义
        int AttAoYiNum = 270007;
        foreach(AoYi ao in AoYiDic.Values) {
            if(ao.Num == AttAoYiNum) {
                ened = ened + ao.AoYiDataConfig.ef_first[ao.RTAoYi.lv - 1] * MathHelper.ONE_HUNDRED;
                break;
            }
        }

        return ened;
    }

    //返回防御值加成的AOYI参数
    //返回 0.0f
    public float getDefAoYi() {
        float ened = 0.0f;

        //偷袭奥义
        int AttAoYiNum = 270009;
        foreach(AoYi ao in AoYiDic.Values) {
            if(ao.Num == AttAoYiNum) {
                ened = ened + ao.AoYiDataConfig.ef_first[ao.RTAoYi.lv - 1] * MathHelper.ONE_HUNDRED;
                break;
            }
        }

        return ened;
    }

	public List<AoYi> getAllAoYi()
	{
		List<AoYi> aoYiList = new List<AoYi>();
		List<AoYiData> aoYiDataList = new List<AoYiData>(AoyiConfig.Values);
		for(int i = 0; i < aoYiDataList.Count; i++)
		{
			AoYiData aoYiData = aoYiDataList[i];
			AoYi aoYi = null;

			foreach(AoYi aoyiTemp in this.AoYiDic.Values)
			{
				if(aoYiData.ID == aoyiTemp.AoYiDataConfig.ID)
				{
					aoYi = aoyiTemp;
					break;
				}
			}

			if(aoYi == null)
			{
				aoYi = new AoYi(new RTAoYi(aoYiData.ID), aoYiData);
				aoYiList.Insert(aoYiList.Count, aoYi);
			}
			else
			{
				aoYiList.Insert(0, aoYi);
			}
		}
		return aoYiList;
	}

	public List<AoYi> getNoPosAoYi()
	{
		List<AoYi> aoYiList = new List<AoYi>();
		List<AoYiData> aoYiDataList = new List<AoYiData>(AoyiConfig.Values);
		for(int i = 0; i < aoYiDataList.Count; i++)
		{
			AoYiData aoYiData = aoYiDataList[i];
			AoYi aoYi = null;

			foreach(AoYi aoyiTemp in this.AoYiDic.Values)
			{
				if(aoYiData.ID == aoyiTemp.AoYiDataConfig.ID)
				{
					aoYi = aoyiTemp;
					break;
				}
			}

			if(aoYi == null)
			{
				aoYi = new AoYi(new RTAoYi(aoYiData.ID), aoYiData);

				if(aoYi.Pos != -1)
				{
					continue;
				}

				aoYiList.Insert(aoYiList.Count, aoYi);
			}
			else
			{
				if(aoYi.Pos != -1)
				{
					continue;
				}
				aoYiList.Insert(0, aoYi);
			}


		}
		return aoYiList;
	}

	public void learnAoYiRequest(string aoYiId)
	{
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.LEARN_AOYI, new learnAoYiRequestParam(Core.Data.playerManager.PlayerID, aoYiId));

		task.afterCompleted += learnAoYiCompleted;
		task.ErrorOccured += learnAoYiError;

		//then you should dispatch to a real handler
		task.DispatchToRealHandler ();
	}

	public Action<AoYi> learnAoYiCompletedDelegate;

	public void learnAoYiCompleted(BaseHttpRequest request, BaseResponse response)
	{
		if(response != null && response.status != BaseResponse.ERROR) 
		{
			LearnAoYiResponse learnAoYiResponse = response as LearnAoYiResponse;
			if(learnAoYiResponse.data != null)
			{
				AoYi aoyi = new AoYi(learnAoYiResponse.data, this);
				this.AoYiDic[aoyi.ID] = aoyi;
				if(learnAoYiCompletedDelegate != null)
				{
					learnAoYiCompletedDelegate(aoyi);
				}
                // if(aoyi.AoYiDataConfig.dragonType == 1){
                if (aoyi != null)
                    Core.Data.dragonManager.isCheckedCallDragon [aoyi.AoYiDataConfig.dragonType - 1] = false;
              
				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(5155));
			}
		}
		else if(response != null)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
			Core.Data.dragonManager.learnAoYiCompletedDelegate = null;
		}
		ComLoading.Close();
	}

	public void learnAoYiError(BaseHttpRequest request, string error)
	{
		ComLoading.Close();
	}

	public void equipAoYiRequest(int aoYippID, int pos)
	{
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.EQUIP_AOYI, new EquipAoYiRequestParam(Core.Data.playerManager.PlayerID, aoYippID, pos));

		task.afterCompleted += equipAoYiCompleted;
		task.ErrorOccured += equipAoYiError;

		//then you should dispatch to a real handler
		task.DispatchToRealHandler ();
	}

	public Action<bool> equipAoYiCompletedDelegate;

	public void equipAoYiCompleted(BaseHttpRequest request, BaseResponse response)
	{
		if(response != null && response.status != BaseResponse.ERROR) 
		{
			EquipAoYiResponse equipAoYiResponse = response as EquipAoYiResponse;

			if(equipAoYiCompletedDelegate != null)
			{
				equipAoYiCompletedDelegate(equipAoYiResponse.data);
			}

		}
		else if(response != null)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
		}

		ComLoading.Close();
		Core.Data.dragonManager.learnAoYiCompletedDelegate = null;
	}

	public void equipAoYiError(BaseHttpRequest request, string error)
	{
		ComLoading.Close();
	}

	public void getQiangDuoDragonBallOpponentsRequest(int ppid)
	{
		this.qiangDuoDragonBallPpid = ppid;
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		
		int Guide = System.Convert.ToInt32(Core.Data.guideManger.isGuiding);
		task.AppendCommonParam(RequestType.GET_QIANGDUO_DRAGONBALL_OPPONENTS, new GetQiangDuoDragonBallOpponentsParam(Core.Data.playerManager.PlayerID, ppid,Guide));

		task.afterCompleted += getQiangDuoDragonBallDataCompleted;
		task.ErrorOccured += getQiangDuoDragonBallDataError;

		//then you should dispatch to a real handler
		task.DispatchToRealHandler ();
	}

	public List<FightOpponentInfo> qiangDuoDragonBallFightOpponentList = new List<FightOpponentInfo>();

	public bool isGetQiangDuoDragonOpponentsCompleted = false;

	public bool isGetSuDiListCompleted = false;

	public int qiangDuoDragonBallPpid = -1;

	public void getQiangDuoDragonBallDataCompleted(BaseHttpRequest request, BaseResponse response)
	{
		HttpRequest httpRequest = request as HttpRequest;
		if(response != null && response.status != BaseResponse.ERROR) 
		{
			if(httpRequest.Act == HttpRequestFactory.ACTION_GET_QIANGDUO_DRAGONBALL_OPPONENTS)
			{

				this.qiangDuoDragonBallFightOpponentList.Clear();
				GetQiangDuoDragonBallOpponentsResponse getQiangDuoDragonBallOpponentsResponse = response as GetQiangDuoDragonBallOpponentsResponse;

				if(getQiangDuoDragonBallOpponentsResponse.data != null)
				{
					this.qiangDuoDragonBallFightOpponentList.AddRange(getQiangDuoDragonBallOpponentsResponse.data);
					FinalTrialMgr.GetInstance().getCurrentQiangDuoDragonBallOpponents();

				}
				isGetQiangDuoDragonOpponentsCompleted = true;
				UIMiniPlayerController.ElementShowArray = new bool[]{true,true,false,true,true};
				UIMiniPlayerController.Instance.SetActive(true);
			}

            isGetSuDiListCompleted = true;
			if(isGetQiangDuoDragonOpponentsCompleted && isGetSuDiListCompleted)
			{
				FinalTrialMgr.GetInstance().CreateScript(TrialEnum.TrialType_QiangDuoDragonBall, QiangduoEnum.QiangduoEnum_List);
				ComLoading.Close();
			}
		}
		else if(response != null && response.status == BaseResponse.ERROR)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
			ComLoading.Close();
		}

		ConsoleEx.Write(" core  opponentlist  =   " + Core.Data.dragonManager.qiangDuoDragonBallFightOpponentList .Count,"lightblue");

	}
	public void getQiangDuoDragonBallDataError(BaseHttpRequest request, string error)
	{

	}
	#endregion

	#region 神龙祭坛
	public Action<int> buyAoYiSlotCompletedDelegate;

	public void buyAoYiSlotRequest(int slot)
	{
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.BUY_AOYI_SLOT, new BuyAoYiSlotParam(Core.Data.playerManager.PlayerID, slot));

		task.afterCompleted += buyAoYiSlotCompleted;
		task.ErrorOccured += buyAoYiSlotError;

		//then you should dispatch to a real handler
		task.DispatchToRealHandler ();
	}

	public void buyAoYiSlotCompleted(BaseHttpRequest request, BaseResponse response)
	{
		if(response != null && response.status != BaseResponse.ERROR) 
		{
			BuyAoYiSlotResponse buyAoYiSlotResponse = response as BuyAoYiSlotResponse;
			if(buyAoYiSlotResponse.data != null)
			{
				Core.Data.playerManager.RTData.curCoin += buyAoYiSlotResponse.data.coin;
				Core.Data.playerManager.RTData.curStone += buyAoYiSlotResponse.data.stone;
                //talking data add by wxl 
                if(buyAoYiSlotResponse.data.stone!= 0)
                    Core.Data.ActivityManager.OnPurchaseVirtualCurrency (ActivityManager.DragonSlotType,1,buyAoYiSlotResponse.data.stone);
				if(buyAoYiSlotCompletedDelegate != null)
				{
					buyAoYiSlotCompletedDelegate(buyAoYiSlotResponse.data.slot);
				}
			}
		}
		else if(response != null)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
		}

		ComLoading.Close();
	}

	public void buyAoYiSlotError(BaseHttpRequest request, string error)
	{
		ComLoading.Close();

	}

	#endregion

	#region 免战
	public Action buyMianZhanTimeCompletedDelegate;

    public void buyMianZhanTimeRequest(int type, int ppid)
	{
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
        task.AppendCommonParam(RequestType.BUY_MIANZHAN_TIME, new BuyMianZhanTimeParam(Core.Data.playerManager.PlayerID, type, ppid));

		task.afterCompleted += buyMianZhanTimeCompleted;
		task.ErrorOccured += buyMianZhanTimeError;

		//then you should dispatch to a real handler
		task.DispatchToRealHandler ();
	}

	public void buyMianZhanTimeCompleted(BaseHttpRequest request, BaseResponse response)
	{
		if(response != null && response.status != BaseResponse.ERROR) 
		{
			BuyMianZhanTimeResponse buyMianZhanTimeResponse = response as BuyMianZhanTimeResponse;
			HttpRequest req = request as HttpRequest;
			BuyMianZhanTimeParam tMZParam = req.ParamMem as BuyMianZhanTimeParam;
			if(buyMianZhanTimeResponse.data != null)
			{
//				Core.Data.playerManager.RTData.curCoin -= buyMianZhanTimeResponse.data.coin;
//				Core.Data.playerManager.RTData.curStone -= buyMianZhanTimeResponse.data.stone;
				Core.TimerEng.deleteTask(TaskID.DragonMianZhanTimer);

				startMianZhanTimer(Core.TimerEng.curTime, buyMianZhanTimeResponse.data.et);
				if (tMZParam.type == 3) {
					long endTime = buyMianZhanTimeResponse.data.et - Core.TimerEng.curTime;
					if (endTime > 3700) {//太阳石   86400
						Core.Data.itemManager.UseItem (Core.Data.itemManager.GetBagItemPid(110050),1);
					} else {
						Core.Data.itemManager.UseItem (Core.Data.itemManager.GetBagItemPid(110049),1);
					}
				}
                SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(6034));
				if(this.buyMianZhanTimeCompletedDelegate != null)
				{
					this.buyMianZhanTimeCompletedDelegate();
				}


			}
		}
		else if(response != null)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
		}
		DBUIController.mDBUIInstance.RefreshUserInfoWithoutShow ();
		UIMiniPlayerController.Instance.freshPlayerInfoView ();
		UIMiniPlayerController.Instance.SetActive (false);
		ComLoading.Close();
	}

	public void buyMianZhanTimeError(BaseHttpRequest request, string error)
	{
		ComLoading.Close();
	}

	public void startMianZhanTimer(long startTime, long endTime)
	{
		TimerTask task = new TimerTask(startTime, endTime, 1);
		task.taskId = TaskID.DragonMianZhanTimer;

		task.onEventEnd += mianZhanTimeCompleted;

		this.mianZhanTime = endTime - startTime;
		this.mianZhanTime = this.mianZhanTime <= 0 ? 0 : this.mianZhanTime;

		if(this.mianZhanTimerEvent != null)
		{
			this.mianZhanTimerEvent(this.mianZhanTime);
		}

		task.onEvent += (TimerTask t) => 
		{
			if(task.taskId == TaskID.DragonMianZhanTimer)
			{
				this.mianZhanTime = t.leftTime;
				if(this.mianZhanTimerEvent != null)
				{
					this.mianZhanTimerEvent(this.mianZhanTime);
				}
			}
		};
		task.DispatchToRealHandler();
	}

	public void callDragonTimeCompleted(TimerTask task)
	{
		if(task == null)
		{
			return;
		}

        ConsoleEx.Write(" call dragon time complete in manager ========= " + task.onEventEnd );
		task.onEventEnd -= callDragonTimeCompleted;

		if(task.taskId == TaskID.CallEarthDragonTimer && this.callEarthDragonTimeCompletedDelegate != null)
		{
			callEarthDragonTimeCompletedDelegate(task);
		}
		else if(task.taskId == TaskID.CallNMKXDragonTimer && this.callNMKXDragonTimeCompletedDelegate != null)
		{
			callNMKXDragonTimeCompletedDelegate(task);
		}
		
	}

	public void mianZhanTimeCompleted(TimerTask task)
	{
		if(task == null)
		{
			return;
		}

		task.onEventEnd -= mianZhanTimeCompleted;

		if(mianZhanTimeCompletedDelegate != null)
		{
			mianZhanTimeCompletedDelegate(task);
		}

	}
	#endregion

	/*   public void SaveDragonBall(Dictionary<int,int> mailDic){
        if (mailDic != null) {
            if (ballMailDic != null) {
                foreach (int mailDicKey in mailDic.Keys) {
                    if (!ballMailDic.ContainsKey (mailDicKey)) {
                        ballMailDic.Add (mailDicKey, mailDic [mailDicKey]);
                        Core.Data.soulManager.DelSoul (mailDic [mailDicKey]);
                    }
                }
            } else {
                ballMailDic = new Dictionary<int, int> ();
                foreach (int mailDicKey in mailDic.Keys) {
                    if (!ballMailDic.ContainsKey (mailDicKey)) {
                        ballMailDic.Add (mailDicKey, mailDic [mailDicKey]);
                    }
                }
            }
        } else {
            ballMailDic = new Dictionary<int, int> ();
        }
    }

*/


}
