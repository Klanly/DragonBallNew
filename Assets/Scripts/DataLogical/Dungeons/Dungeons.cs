using System;
using System.Collections.Generic;

// contains status : clear ongoing new
public class Chapter : DungeonsData,ICloneable
{
	public ChapterData config;
	//本大关卡可以攻打的中关卡ID
	public int toFightCityId;
	//补丁:只有直接跳转,自己伪造的结构才有值，正常为null
	public City toFightCity;
		
	public Chapter(){}
	
	public object Clone()
    {
		Chapter o = new Chapter();
		o.config = this.config;
		o.toFightCityId = this.toFightCityId;
		o.toFightCity = this.toFightCity;
		return o;
    }
}

// contains status : clear ongoing new
public class City : DungeonsData {
	public CityData config;
	//本中关卡可以攻打的子关卡ID
	public int toFightFloorId;
	//是否开启过
	public bool isOpen;
}

// contains status : clear ongoing new
public class Floor : DungeonsData {

	//免费打关卡boss的次数
	public int curFreeTimes;
	/* add by jc --> 当前关卡一共打过几次,(一共可以打的次数为wave+laveTime),如果超过可打
	 * 的总次数,则为总次数,判断这个值是否和总次数相当可知挑战是否要花费钻石*/
	public int curProgress;

	public FloorData config;
	
	public bool isfree;
	

	
	//当前关卡是否是boss关卡
	public bool isBoss {
		get {
			if(config != null)
				return config.isBoss == FloorData.IS_BOSS;
			else 
				return false;
		}
	}

}

//记录下来什么改变了
public struct WhichChange {
	public bool FloorChange;
	public bool CityChanged;
	public bool ChapterChanged;

	public void reset() {
		FloorChange = false;
		CityChanged = false;
		ChapterChanged = false;
	}
}


public class DungeonsManager : Manager {

	//Config files...
	//Key is Chapter ID & City Id & Floor ID
	private readonly Dictionary<int, ChapterData> ChapterConfigList = null;
	private readonly Dictionary<int, CityData> CityConfigList = null;
	private readonly Dictionary<int, FloorData> FloorConfigList = null;

	//帮助反向分析关卡数据，比如中关->大关，小关->中关
	private readonly Dictionary<int, int> reverse = null;

	//Dynamic files... 
	//Key is Chapter ID & City Id & Floor ID
	public Dictionary<int, Chapter> ChapterList = null;
	public Dictionary<int, City> CityList = null;
	public Dictionary<int, Floor> FloorList = null;
    public Dictionary<int, List<int>> floorAndRewardDic = null;
	//记录下来改变的数据
	private WhichChange changed;

	//最后打过的的最大子关卡ID
	public int lastFloorId{get;set;}
	//服务器发来的进度数组的最后一个关卡的id  <指最后的一个被通关的关卡,endProId可能会超过lastFloorId>
	public int endProId{get;set;}
	
	//-------------------  on going or finished ------------------
	/*最近一次是否胜利
	 * */
	public bool isWinOfLastFloor{get;set;}
	/*最近一次战斗是否是BOSS关
	 * */
	public bool isBossOfLastFloor{get;set;}
	//是否是第一次运行
    public bool isFristRun{get;set;}
	
	//最近打的那一关的ID
	public int isLastPlayFloor{get;set;}
	
	public DungeonsManager() {
		isFristRun = true;
		isWinOfLastFloor = true;
		isBossOfLastFloor = true;
		ChapterConfigList = new Dictionary<int, ChapterData>();
		CityConfigList = new Dictionary<int, CityData>();
		FloorConfigList = new Dictionary<int, FloorData>();
		//反向分析
		reverse = new Dictionary<int, int>();

		ChapterList = new Dictionary<int, Chapter>();
		CityList = new Dictionary<int, City>();
		FloorList = new Dictionary<int, Floor>();

		lastFloorId = 0;

		changed = new WhichChange();
		changed.reset();
        //关卡 和奖励
        floorAndRewardDic = new Dictionary<int, List<int>> ();
	}

	//fullfilled by local config file
	public override bool loadFromConfig () {
#if NEWPVE
		bool success = true;
#else
		bool success = base.readFromLocalConfigFile<ChapterData>(ConfigType.Chapter, ChapterConfigList);
		success |= base.readFromLocalConfigFile<CityData>(ConfigType.City, CityConfigList);
		success |= base.readFromLocalConfigFile<FloorData>(ConfigType.Floor, FloorConfigList);
		//分析数据
		anaylize();
#endif		
		
	
		return success;
	}

	public override void fullfillByNetwork (BaseResponse response) {
	#if !NEWPVE	
		if(response != null && response.status != BaseResponse.ERROR) {

			LoginResponse loginResp = response as LoginResponse;
			if(loginResp != null && loginResp.data != null) {

                initDynamicDungeonsData(loginResp.data.floor != null);
				if(loginResp.data.floor != null) {
					anaylizeRuntimeDungeons(loginResp.data.floor.end);
					anaylizeProgress(loginResp.data.floor);

//					RED.Log("kkkkkkkkkkkkkkk->"+fightCityId.ToString());
					AsyncTask.QueueOnMainThread( () => 
					{
						if(CityList.ContainsKey(fightCityId))
						CityFloorData.Instance.SyncCity(CityList[fightCityId]);
					});
				}
			}
		}
		#endif	
	}

	public FloorData getFloorData(int num) {
		FloorData fd = null;
		if(!FloorConfigList.TryGetValue(num, out fd)) 
			fd = null;
		return fd;
	}

	public WhichChange getChanged {
		get {
			WhichChange theNewOne = changed;
			changed.reset();
			return theNewOne;
		}
	}

	/// <summary>
	/// 可以挑战最新的一个大关卡Id.
	/// </summary>
	/// <value>The fight floor identifier.</value>
	public int fightChapterId {
		get {
			return reverseToChapterMore(fightFloorId).ID;
		}
	}

	/// <summary>
	/// 可以挑战最新的一个中关卡Id.
	/// </summary>
	/// <value>The fight floor identifier.</value>
	public int fightCityId {
		get {
	#region Add by jc
			if(lastFloorId==0)
				lastFloorId=60101;
			if(getFloor(lastFloorId).isBoss)
			{
				//if(isWinOfLastFloor)
				//{
					//RED.Log("run getNextCityId FUNCTION==>"+reverseToCity(lastFloorId).ID);
					if(checkLastCity(reverseToCity(lastFloorId).ID) )
					{
						return reverseToCity(lastFloorId).ID+1;
					}
					return getNextCityId(reverseToCity(lastFloorId).ID);
				//}
				
			}
			return reverseToCity(lastFloorId).ID;
	#endregion				
		}
	}

	/// <summary>
	/// 可以挑战最新的一个小关卡Id.结果可能为最后一个子关卡ID， 如果为最后一个子关卡ID则表示没有新的小关卡可以攻打。
	/// </summary>
	/// <value>The fight floor identifier.</value>
	public int fightFloorId {
		get {
			//初始化的第一个子关卡ID = 60101
			int challegeId = 60101;

			if(lastFloorId != 0) {

				FloorData fd = null;

				if(FloorConfigList.TryGetValue(lastFloorId, out fd)) {
					if(fd.isBoss == FloorData.IS_BOSS) {

						CityData cd = reverseToCity(lastFloorId);
						Utils.Assert(cd == null, "Dungeons Config file is wrong. lastFloorId = " + lastFloorId);
						if(checkLastCity(cd.ID)) {//最后一个city关卡？

							//我要找到新的chapter
							ChapterData chaDa = reverseToChapterMore(lastFloorId);
							Utils.Assert(chaDa == null, "Dungeons Config file is wrong. lastFloorId = " + lastFloorId);
							int nextChaptr = 0;
							/*如果打赢了,会跳到下一关   add by jc*/
							if(isWinOfLastFloor)
							 nextChaptr = getNextChapterId(chaDa.ID);

							if(nextChaptr == 0) 
							{
								challegeId = lastFloorId;
							} 
							else
							{
								ChapterData c = null;
								CityData c2 = null;
								if(ChapterConfigList.TryGetValue(nextChaptr, out c)) {
									if(CityConfigList.TryGetValue(c.cityID[0], out c2)) {
										challegeId = c2.floorID[0];
									}
								} 
							}

						} else {
							int nextCityid = getNextCityId(cd.ID);
							challegeId = getMinFloorId(nextCityid);
						}

					} else {
						challegeId = getNextFloorId(lastFloorId);
					}
				}

			}

			return challegeId;
		}
	}

	/// <summary>
	/// 根据City ID 来决定当前正在打的Floor ID
	/// </summary>
	/// <returns>The floor pro.</returns>
	/// <param name="curCity">Current city.</param>
	public int getFloorPro (int curCityId) {
		int progress = 0;

		City city = null;
		if(CityList.TryGetValue(curCityId, out city)) {
			if(city.status == DungeonsData.STATUS_CLEAR) {

				int[] floorIds = city.config.floorID;
				if(floorIds != null && floorIds.Length > 0) {

					Floor floor = null;
					foreach(int floorId in floorIds) {
						if(FloorList.TryGetValue(floorId, out floor)) {
							if(floor.isBoss) {
								progress = floor.config.ID;
								break;
							} else {
								if(floor.curProgress < floor.config.wave) {
									progress = floor.config.ID;
									break;
								}
							}

						}
					}
				}


			} else {
				progress = fightFloorId;
			}
		}

		return progress;
	}

	/// <summary>
	/// Inits the dynamic dungeons data. 默认的状态是NEW
	/// </summary>
    private void initDynamicDungeonsData (bool dirtyData) {
        //没有脏数据的时候，不更新
        if(!dirtyData) {
            int count = ChapterList.Count;
            if(count > 0)
                return;
        }
            
        ChapterList.Clear();
        CityList.Clear();
        FloorList.Clear();

        //Chapter
        foreach(ChapterData cd in ChapterConfigList.Values) {
            if(cd != null) {
                Chapter chapter = new Chapter();
                chapter.config = cd;
                chapter.status = DungeonsData.STATUS_NEW;
                chapter.toFightCityId = cd.cityID[0];
                ChapterList.Add(cd.ID, chapter);
            }
        }
        //City
        foreach(CityData cd in CityConfigList.Values) {
            if(cd != null) {
                City city = new City();
                city.config = cd;
                city.status = DungeonsData.STATUS_NEW;
                city.toFightFloorId = cd.floorID[0];
                CityList.Add(cd.ID, city);
            }
        }
        //Floor
        foreach(FloorData fd in FloorConfigList.Values){
            if(fd != null) {
                Floor floor = new Floor();
                floor.config = fd;
                floor.status = DungeonsData.STATUS_NEW;

                if(floor.isBoss) {
                    floor.curFreeTimes = fd.laveTime;
                }
                FloorList.Add(fd.ID, floor);
            }
        }

	}

	//根据服务器的状态位，填充好地下城完成的状态
	private void anaylizeRuntimeDungeons(int lf) {

		/*lastFloorId 为服务器传来的该玩家目前通过的最后一关
		 * */
        lastFloorId = lf;
		if(lastFloorId != 0) {
			int[] less_Floor_Ids = getFloorIds_Less(lastFloorId);

			bool isBoss = false;
			// -------------- 设置好子关卡的状态 ----------------
			Floor floor = null;
			if(less_Floor_Ids != null && less_Floor_Ids.Length > 0) {
				foreach(int less in less_Floor_Ids) {
					if(FloorList.TryGetValue(less, out floor)) {
						floor.status = DungeonsData.STATUS_CLEAR;
					}
				}

			}
			//当前的子关卡也是完成的状态
			if(FloorList.TryGetValue(lastFloorId, out floor)) {
				floor.status = DungeonsData.STATUS_CLEAR;
				isBoss = floor.isBoss;
			}
			//开启下一个子关卡（设置为new,即不设置，默认就是new）

			//----------------- 设置好中关卡的状态 --------------------
			City city = null;
			int[] less_City_Ids = getCityIds_LessMore(lastFloorId);
			if(less_City_Ids != null && less_City_Ids.Length > 0) {
				foreach(int less in less_City_Ids) {
					if(CityList.TryGetValue(less, out city)) {
						city.status = DungeonsData.STATUS_CLEAR;

						//找到中关卡里的所有小关卡
						Floor innalFloor = null;
						foreach(int fid in city.config.floorID) {
							if(FloorList.TryGetValue(fid, out innalFloor)) {
								innalFloor.status = DungeonsData.STATUS_CLEAR;
							}
						}

					}
				}
			}


			short status = DungeonsData.STATUS_NEW;
			//当前的中关卡是设置为ongoing还是clear得要看lastFloorId是不是boss关
			if(CityList.TryGetValue( reverseToCity(lastFloorId).ID, out city)) {
				if(isBoss) {
					city.status = DungeonsData.STATUS_CLEAR;
				} else {
					city.status = DungeonsData.STATUS_ONGOING;
				}

				status = city.status;
			}

			//---------------  设置大关卡的状态 ------------------
			Chapter chapter = null;
			int[] less_chapter_ids = getChapterIds_LessMore(lastFloorId);
			if(less_chapter_ids != null && less_chapter_ids.Length > 0) {
				foreach(int less in less_chapter_ids) {
					if(ChapterList.TryGetValue(less, out chapter)) {
						chapter.status = DungeonsData.STATUS_CLEAR;
						//设置当前的焦点
						chapter.toFightCityId = chapter.config.cityID[chapter.config.cityID.Length - 1];
					}
				}
			}

			//当前的大关卡的状态
			if(ChapterList.TryGetValue( reverseToChapterMore(lastFloorId).ID, out chapter)) {
				if(status == DungeonsData.STATUS_ONGOING){
                    chapter.status = DungeonsData.STATUS_ONGOING;
                    chapter.toFightCityId = reverseToCity(lastFloorId).ID;
                } else if(status == DungeonsData.STATUS_CLEAR ){
					if( checkLastCity(reverseToCity(lastFloorId).ID) ){
						chapter.status = DungeonsData.STATUS_CLEAR;
						//设置当前的焦点
						chapter.toFightCityId = chapter.config.cityID[chapter.config.cityID.Length - 1];
					} else {
						chapter.status = DungeonsData.STATUS_ONGOING;
						//设置当前的焦点
						chapter.toFightCityId = getNextCityId(reverseToCity(lastFloorId).ID);
					}
				}
			}
		}
		
	}
	
	
    //某关是否打满(使用小关卡 非BOSS关)
	public bool isFloorProgressFull(int floorID)
	{
		Floor tempfloor = null;
		if(FloorList.TryGetValue(floorID,out tempfloor))
		{
			return tempfloor.config.wave+tempfloor.config.laveTime == tempfloor.curProgress;
		}
		else
	    {
			return true;
		}
	}
	
	
	
	/*add by jc*/
	public Floor getFloor(int num)
	{
		Floor floor=null;
		FloorList.TryGetValue(num, out floor);
		return floor;
	}
	
	//根据服务器的进度，设置好地下城每个子关卡的进度
	private void anaylizeProgress(DungeonsInfo dungeonse) {
		Utils.Assert(dungeonse == null, "We did not get dungeonse information from server.");

		if(dungeonse.pro != null && dungeonse.pro.Count > 0) {
			
			//endProId = dungeonse.pro[ dungeonse.pro.Count -1].;
			
			foreach(int[] oneFloor in dungeonse.pro) {

				if(oneFloor != null) 
				{
					int floorId = oneFloor[DungeonsInfo.ID_POS];
					int progress = oneFloor[DungeonsInfo.PRO_POS];
					if(progress > 0) 
					{
						Floor floor = null;
						if(FloorList.TryGetValue(floorId, out floor)) 
						{
							if(floor.isBoss) 
							{
								floor.curFreeTimes = floor.config.laveTime - progress;
								if(floor.curFreeTimes < 0) floor.curFreeTimes = 0;
								/*add by jc*/
								floor.curProgress = progress;
								
							} 
							else 
							{
								floor.curProgress = progress;
								//给City里填充toFightFloorId
								City city = null;
								if(CityList.TryGetValue(reverseToCity(floorId).ID, out city))
								{
									if(floor.curProgress < floor.config.wave)
									{
										city.toFightFloorId = floorId;
									} 
									else
									{
										city.toFightFloorId = getNextFloorId(floorId);
									}
								}
								
								int[] floorIds = getFloorIds_Less(floorId);
								if(floorIds != null && floorIds.Length > 0) {
									Floor less = null;

									foreach(int id in floorIds) {
										if(FloorList.TryGetValue(id, out less)){
											less.curProgress = less.config.wave;
										}
									}

								}

							}
						}
					}

				}

			}

		}
				
	}

	private void anaylize() {
        //分析大关卡数据，生成中关-》大关
        foreach (ChapterData chData in ChapterConfigList.Values) {
            if (chData != null) {
                foreach (int cityId in chData.cityID) {
                    reverse.Add (cityId, chData.ID);
                }
            }
        }
        //分析小关卡数据,生成小关-》中关
        foreach (CityData cityData in CityConfigList.Values) {
            if (cityData != null) {
                foreach (int floorId in cityData.floorID) {
                    reverse.Add (floorId, cityData.ID);
                }
            }
        }
            
    }

    /// <summary>
    /// 分析副本中的碎片   add by  wxl 
    /// </summary>
    /*   public void Analysis(){
        foreach (FloorData fdata in FloorConfigList.Values) {
            if (fdata != null) {
                if (fdata.isBoss == 1) {
                    if (!floorAndRewardDic.ContainsKey (fdata.specialRewardID)) {
                        List<int> floorIdList = new List<int> ();
                        floorIdList.Add (fdata.ID);
                        int tId = fdata.specialRewardID;
                        if (DataCore.getDataType (fdata.specialRewardID) == ConfigDataType.Frag) {
                            SoulData soulConfig = Core.Data.soulManager.GetSoulConfigByNum (fdata.specialRewardID);

                            if (soulConfig != null) {
                                if (soulConfig.type == (int)ItemType.Monster_Frage) {
                                    tId = soulConfig.updateId;
                                }
                            } else {
                                RED.LogWarning ("  soul config  is null = " + tId);
								continue;
                            }

                        } else if (DataCore.getDataType (fdata.specialRewardID) == ConfigDataType.Monster) {

                            tId = fdata.specialRewardID;
                        } else if (DataCore.getDataType (fdata.specialRewardID) == ConfigDataType.Equip) {
                            tId = fdata.specialRewardID;

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
                        floorAndRewardDic.TryGetValue (fdata.specialRewardID, out tList);
                        floorAndRewardDic.Remove (fdata.specialRewardID);
                        tList.Add (fdata.ID);
                        floorAndRewardDic.Add (fdata.specialRewardID, tList);
                    }
                }
            }
        }
    }
	*/

	/// <summary>
	/// 通过宠物或者装备id获得关卡id  maybe equip ,frag and monster
	/// </summary>
	/// <returns>The floor id by giftid</returns>
	/// <param name="giftId">Gift identifier.</param>
//	public List<int> GetFloorIdByGiftId(int giftId){
//		List<int> targetId =new List<int>();
//		if(giftId != 0){
//
//			if(floorAndRewardDic.TryGetValue(giftId,out targetId)){
//				return targetId;
//			}
//		}
//			return targetId;
//	}

	#region 更新地下城状态

	public void OnFinishFloor(BaseHttpRequest request,BaseResponse response) {
		Utils.Assert(request == null, "Request is null.");
		
		if(response.status==BaseResponse.ERROR)return;
		
		if(request.baseType == BaseHttpRequestType.Common_Http_Request) 
		{
			HttpRequest req = request as HttpRequest;
			if(req != null)
			{
				BaseRequestParam param = req.ParamMem;
				BattleResponse res= response as BattleResponse;

					if(param != null) 
					{
						bool isWin=false;
						BattleParam bp = param as BattleParam;
						if(res!=null)
						{
						   if(res.status!=BaseResponse.ERROR)
							{

								isWin=System.Convert.ToBoolean(res.data.battleData.retCode);
							    isBossOfLastFloor = true;
							}
						}
						else
					    {
						     /*没有返回结果说明是小关(小关100%成功)*/
						     isWin = true;
						     isBossOfLastFloor = false;
					    }
					 
					
						#region 测试代码
						//isWin=false;
						#endregion
					
					    this.isWinOfLastFloor=isWin;
                        if(bp != null) 
					    {
						    OnFinishFloor(bp.doorId,isWin);
					    }
						
					}
				
			}
		}
	}

	public void OnFailToWar(BaseHttpRequest request) {
		Utils.Assert(request == null, "Request is null");

		if(request.baseType == BaseHttpRequestType.Common_Http_Request) {
			HttpRequest req = request as HttpRequest;
			if(req != null) {
				BaseRequestParam param = req.ParamMem;
				if(param != null) {
					BattleParam bp = param as BattleParam;
					if(bp != null) FailToWar(bp.doorId);
				}
			}
		}

	}

	/// <summary>
	/// 首先找到Floor，如果是Boss关卡，则要查找City，如果是City是最后一个中关卡，则要查找Chapter，如果是最后一个Chpater，则没有新的大关卡。
	/// 如果不是boss关卡, 则仅仅更新Floor的status
	/// directly 是直接打的吗？
	/// </summary>
	/// <param name="finishFloorId">Finish floor identifier.</param>
	private void OnFinishFloor(int finishFloorId,bool isWin) 
	{		
		isLastPlayFloor = finishFloorId;
			
		changed.reset();

		Floor floor = null;
		FloorList.TryGetValue(finishFloorId, out floor);
		Utils.Assert(floor == null, "Can't find Floor. floorId = " + finishFloorId.ToString());

		City city = null;
		
		CityList.TryGetValue(reverseToCity(finishFloorId).ID, out city);
		Utils.Assert(city == null, "Can't find City. CityId = " + reverseToCity(finishFloorId).ID);

		if(floor.isBoss) 
		{
            if (isWin) {
                lastFloorId = Math.Max (lastFloorId, finishFloorId);
                // talkingdata     add by  wxl   
//                if (city != null && floor != null) {
//                    Core.Data.ActivityManager.OnMissionComplete (city.config.name + " , " + floor.config.name);
//                }
			}
//            } else {
//                // talkingdata     add by  wxl 
//                Core.Data.ActivityManager.OnMissionFail (city.config.name + " , " + floor.config.name);
//            }
			
        
			if(city.toFightFloorId == finishFloorId)
			{
                OnFinishFloor(finishFloorId, city, false,isWin);
			} 
			else
			{
				OnFinishFloor(finishFloorId, city, true,isWin);
			}
		}
		else
		{
			lastFloorId = Math.Max(lastFloorId, finishFloorId);
			OnFinishFloor(finishFloorId, city, false,isWin);
		}
		
		//RED.Log(">>>>>>>>>>>lastFloorId<<<<<<<<<<<="+lastFloorId.ToString()+"  finishFloorId="+finishFloorId.ToString()+ "   isWin="+isWin.ToString());
		
	}

	/// <summary>
	/// 准备挑战关卡
	/// </summary>
	/// <param name="finishFloorId">Finish floor identifier.</param>
	/// <param name="directly">If set to <c>true</c> directly.</param>
	private void FailToWar(int finishFloorId) {
		changed.reset();

		City city = null;
		CityList.TryGetValue(reverseToCity(finishFloorId).ID, out city);
		Utils.Assert(city == null, "Can't find City. CityId = " + reverseToCity(finishFloorId).ID);


		Floor floor;
		if(FloorList.TryGetValue(finishFloorId, out floor)) {
			if(floor.isBoss) {
				if(city.toFightFloorId == finishFloorId) {
					failToWar(finishFloorId, floor, false);
				} else {
					failToWar(finishFloorId, floor, true);
				}
			} 
		}
	}

	private void OnFinishFloor(int finishFloorId, City city, bool directly,bool isWin)
	{
		
		
		Floor floor, sameLvFloor;
		if(FloorList.TryGetValue(finishFloorId, out floor)) 
		{
			if(floor.isBoss)
			{
				if(isWin)
				floor.curProgress ++;//仅仅记录打过几次
				else
				{
					Floor beforeData=Core.Data.dungeonsManager.getFloor(floor.config.ID-1);
					//如果是[执行0/1],换句话如果当前是使用的vip机会失败也会算用掉一次机会
					if(beforeData!=null && beforeData.curProgress!=beforeData.config.wave+beforeData.config.laveTime)
					{
						//RED.Log("=================>[vip change]  beforeID:"+beforeData.config.ID.ToString()+"   "+beforeData.curProgress.ToString()+"/"+(beforeData.config.wave+beforeData.config.laveTime).ToString()+"    "+floor.curProgress.ToString() );
						floor.curProgress ++;
					}
				}
				
				
				/*add by jc  注:<重要> 指本关一共打过几次，如果超过(wave+laveTime总共可以打的次数,则让这个值与可以打的次数相等,此逻辑与服务器一致,说明要花钱)*/
				if(floor.curProgress>floor.config.wave+floor.config.BossLaveTime)floor.curProgress=floor.config.wave+floor.config.BossLaveTime;
				
				floor.status = DungeonsData.STATUS_CLEAR;
				changed.FloorChange = true;

				city.status = DungeonsData.STATUS_CLEAR;
				changed.CityChanged = true;


				Chapter chapter = null;
				
				if(ChapterList.TryGetValue(reverseToChapterMore(finishFloorId).ID, out chapter)) 
				{
					//是最后一个City中关卡吗？
					if(checkLastCity(city.config.ID))
					{
						/*最后一关的boss关过了以后跳到下一关
						 * */
						if(isWin)
						{
							
							chapter.status = DungeonsData.STATUS_CLEAR;
							changed.ChapterChanged = true;
							chapter.toFightCityId = chapter.config.cityID[chapter.config.cityID.Length - 1];
						}
					} 
					else 
					{
						/*如果战斗赢了,则跳到下一关   add by jc*/
						if(isWin)
						{
						    chapter.toFightCityId = getNextCityId(city.config.ID);

                            //统计数据 talkingdata   wxl
                           // Core.Data.ActivityManager.OnMissionBegin (city.config.name );
						}
						else
						{
						}
					}

				}

				//直接打的boss关卡
				if(isWin)
				{
					if(directly) 
					{	
						floor.curFreeTimes --;
						//RED.Log("Dungeons.cs line 585!");
						MathHelper.KeepCreateZero(ref floor.curFreeTimes);
					} 
					else
					{
						floor.curFreeTimes --;
						//清空所有当前子关卡的进度
						int[] curFloorIds = getFloorIds(finishFloorId);
						//Utils.Assert(curFloorIds == null, "Config file is wrong. Floor Id = " + finishFloorId);
						foreach(int fid in curFloorIds)
						{
							
							if(FloorList.TryGetValue(fid, out sameLvFloor)) 
							{
								if(!sameLvFloor.isBoss) 
								{
									sameLvFloor.curProgress = 0;
								}
							}
						}
						city.toFightFloorId = curFloorIds[0];
					}
				}	
				CityFloorData.Instance.chapter = chapter;
			} 
			else 
			{
				floor.curProgress ++;
				
				if(floor.curProgress >= floor.config.wave)
				{
					#region 如果该小关的下一关是BOSS关,则让BOSS关的已执行次数-1
					Floor nextData=Core.Data.dungeonsManager.getFloor(floor.config.ID+1);
					if(nextData!=null && nextData.isBoss)
					{
						if(nextData.curProgress > 0 )
						{
							nextData.curProgress --;
						}
					}
					#endregion
						
					floor.curProgress = floor.config.wave;
					floor.status = DungeonsData.STATUS_CLEAR;
					changed.FloorChange = true;
					city.toFightFloorId = getNextFloorId(finishFloorId);
										
				} 
				else 
				{
					floor.status = DungeonsData.STATUS_ONGOING;
				}

			}

		}
	}


	private void failToWar(int finishFloorId, Floor floor, bool directly) {
		
		if(floor.isBoss) {
			floor.curProgress ++;//仅仅记录打过几次
			floor.status = DungeonsData.STATUS_ONGOING;

			//直接打的boss关卡
			if(directly) {
				floor.curFreeTimes --;
				RED.Log("Dungeons.cs line 627!");
				MathHelper.KeepCreateZero(ref floor.curFreeTimes);
			}

		}
	}

	#endregion

	#region 反向获取数据的方法
	/// <summary>
	/// Reverses to city.小关-》中关
	/// </summary>
	/// <returns>The to city.</returns>
	/// <param name="floorId">Floor identifier.</param>
	public CityData reverseToCity( int floorId) {
		CityData city = null;

		int CityId = 0;
		if(reverse.TryGetValue(floorId, out CityId)) {
			if(!CityConfigList.TryGetValue(CityId, out city)) {
				city = null;
			}
		}

		return city;
	}

	/// <summary>
	/// Reverses to chapter.中关-》大关
	/// </summary>
	/// <returns>The to chapter.</returns>
	/// <param name="cityId">City identifier.</param>
	public ChapterData reverseToChapter(int cityId) {
		ChapterData chapter = null;

		int ChapterId = 0;
		if(reverse.TryGetValue(cityId, out ChapterId)){
			if(!ChapterConfigList.TryGetValue(ChapterId, out chapter)) {
				chapter = null;
			}
		}

		return chapter;
	}

	/// <summary>
	/// Reverses to chapter.小关-》大关
	/// </summary>
	/// <returns>The to chapter more.</returns>
	/// <param name="floorId">Floor identifier.</param>
	public ChapterData reverseToChapterMore(int floorId) {
		ChapterData chapter = null;

		int CityId = 0, ChapterId = 0;
		if(reverse.TryGetValue(floorId, out CityId)) {
			if(reverse.TryGetValue(CityId, out ChapterId)) {
				if(!ChapterConfigList.TryGetValue(ChapterId, out chapter)) {
					chapter = null;
				}
			}
		}

		return chapter;
	}

	#endregion

	#region 获取同级别的数据
	/// <summary>
	/// 根据ChapterID找到第后一个中关卡ID
	/// </summary>
	/// <returns>The max city identifier.</returns>
	/// <param name="chapter">Chapter.</param>
	private int getMaxCityId (int chapter) {
		ChapterData cd = null;
		if(ChapterConfigList.TryGetValue(chapter, out cd)) {
			return cd.cityID[cd.cityID.Length - 1];
		} else {
			throw new DragonException("Can't Chatper Data according to Chapter Id = " + chapter);
		}
	}
	/// <summary>
	/// 根据CityID找到第一个子关卡ID
	/// </summary>
	/// <returns>The minimum floor identifier.</returns>
	/// <param name="cityId">City identifier.</param>
	private int getMinFloorId (int cityId) {
		CityData cd = null;
		if(CityConfigList.TryGetValue(cityId, out cd)) {
			return cd.floorID[0];
		} else {
			throw new DragonException("Can't City Data according to cityId = " + cityId);
		}
	}

	//该方法仅能找到同一个city下面的数据
	private int getNextFloorId (int curFloorId) {
		int result = 0;

		int[] curFloorIds = getFloorIds(curFloorId);
		if(curFloorIds != null && curFloorIds.Length > 0) {

			foreach(int sameLevel in curFloorIds) {
				if(sameLevel > curFloorId) {
					result = sameLevel;
					break;
				}
			}
		}

		return result;
	}

	//该方法仅能找到同一个Chapter下面的CityID
	private int getNextCityId (int curCityId) {
		int result = 0;

		int[] curCityIds = getCityIds(curCityId);
		if(curCityIds != null && curCityIds.Length > 0) {
			foreach(int sameLevel in curCityIds) {
				if(sameLevel > curCityId) {
					result = sameLevel;
					break;
				}
			}
		}

		return result;
	}

	//找到下一个chapter ID，但还可能会返回0（因为Chapter数据已经是最后一个大关卡了。）
	private int getNextChapterId (int curChapterId) {
		int result = 0;

		foreach(ChapterData cd in ChapterConfigList.Values) {
			if(cd != null && cd.ID > curChapterId) {
				result = cd.ID;
				break;
			}
		}

		return result;
	}


	public int[] getCityIds (int curCityId) {
		ChapterData chapter = reverseToChapter(curCityId);
		Utils.Assert(chapter == null, "Chapter Or City Config file is wrong. CityId = " + curCityId);
		return chapter.cityID;
	}

	public int[] getFloorIds (int curFloorId) {
		CityData city = reverseToCity(curFloorId);
		Utils.Assert(city == null, "City or Floor Config file is wrong. FloorId = " + curFloorId);
		return city.floorID;
	}

	//根据Chapter ID 来获取当前ID之前的大关卡
	public int[] getChapterIds_less(int curChapterId) {
		List<int> tmp = new List<int>();

		foreach(ChapterData cpd in ChapterConfigList.Values) {
			if(cpd.ID < curChapterId)
				tmp.Add(cpd.ID);
		}

		return tmp.ToArray();
	}

	//根据City ID来获取当前ID之前的City中关卡
	public int[] getCityIds_Less (int curCityId) {
		List<int> tmp = new List<int>(getCityIds(curCityId));
		int count = tmp.Count;

		for(int i = count - 1; i >= 0; --i) {
			if(tmp[i] >= curCityId)
				tmp.RemoveAt(i);
		}
		return tmp.ToArray();
	}

	//根据Floor ID来获取当前ID之前的Floor的子关卡
	public int[] getFloorIds_Less (int curFloorId) {
		List<int> tmp = new List<int>(getFloorIds(curFloorId));
		int count = tmp.Count;

		for(int i = count - 1; i >= 0; --i) {
			if(tmp[i] >= curFloorId)
				tmp.RemoveAt(i);
		}
		return tmp.ToArray();
	}

	//根据Floor Id来获取当前中关卡之前的 中关卡
	public int[] getCityIds_LessMore(int curFloorId) {
		//根据子关卡ID找到中关卡数据
		CityData curCity = reverseToCity(curFloorId);
		Utils.Assert(curCity == null, "Can't find City Data from FloorId. ID = " + curFloorId);

		int curCityId = curCity.ID;
		return getCityIds_Less(curCityId);
	}

	//根据Floor Id来获取当前大关卡之前的 大关卡
	public int[] getChapterIds_LessMore(int curFloorId) {
		ChapterData curChapter = reverseToChapterMore(curFloorId);
		Utils.Assert(curChapter == null, "Can't find Chapter Data from FloorId. ID = " + curFloorId);

		int curChapterId = curChapter.ID;

		return getChapterIds_less(curChapterId);
	}

	/// <summary>
	/// Checks the last city. 根据CityId来判定，该City是不是Chapter中最后一个
	/// </summary>
	/// <returns><c>true</c>, if last city was checked, <c>false</c> otherwise.</returns>
	public bool checkLastCity(int cityId) {
		bool find = false;

		ChapterData chapter = null;
		int chapterId = reverseToChapter(cityId).ID;
		if(ChapterConfigList.TryGetValue(chapterId, out chapter)) {
			int[] citylist = chapter.cityID;
			if(citylist[citylist.Length - 1] == cityId)
				find = true;
		}

		return find;
	}
 



	
	#endregion
	
	#region 获取BOSS关按钮的显示内容
	public string GetBossShowContent(int BossfloorID)
	{
		string result = "";
		Floor data = null;
		if(FloorList.ContainsKey(BossfloorID) && FloorList[BossfloorID].isBoss)
	     data = FloorList[BossfloorID];
		else
			return result;

		if(data.curProgress>=1)
		{
			Floor beforeData=Core.Data.dungeonsManager.getFloor(data.config.ID-1);
		    if(beforeData!=null)
		    {
				if( beforeData.curProgress==beforeData.config.wave+beforeData.config.laveTime )
				{
					result =  "0/1";
				}
				else
					result ="vip"+Core.Data.playerManager.curVipLv+":"+(data.curProgress-1).ToString()+"/" + data.config.BossLaveTime;						
			}
		}
		else
			result =  "0/1";				
		
		return result;
	}
	
	
	#endregion

}


//小关卡的按照ID排序
public class FloorIDComparer : IComparer<Floor> {
	int IComparer<Floor>.Compare (Floor x, Floor y) {
		return x.config.ID - y.config.ID;	
	}
}

//中关卡ID排序
public class CityIDComparer : IComparer<City> {
	int IComparer<City>.Compare (City x, City y) {
		return x.config.ID - y.config.ID;	
	}
}

public class ChapterIDComparer : IComparer<Chapter> {
//大关卡ID排序
	int IComparer<Chapter>.Compare (Chapter x, Chapter y) {
		return x.config.ID - y.config.ID;	
	}
	
	
	
	

	
}
   

