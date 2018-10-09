using System;
using System.Collections.Generic;

internal enum ChaKeLaType {
	//即是定义，又是网络传输时的数组的位置
	Attack = 0x0,
	Defend = 0x1,
	Privot = 0x2,
}

internal enum KeyType {
	//即是定义，又是网络传输时的数组的位置
	GoldenKey = 0x00,
	SilverKey = 0x01,
	CopperKey = 0x02,
	Privot    = 0x03,
}

/*
 * we can get runtime data from server
 * 这里没有任何的方法，都是数据的定义
 */ 
public class RTPlayer {
	//My ID
	public string ID;
	//账号ID
	public string uId;
	//current level
	public Int32Fog curLevel;
	//current coin
	public Int32Fog curCoin;
	//current stone;
	public Int32Fog curStone;
	//新鲜数据
	public bool isFirstData;
	//current 体力
	public Int32Fog curTili;
	//current 精力
	public Int32Fog curJingLi;
	//current 精力突破值
	public Int16Fog enhanceJingLi;
	//current 体力突破值
	public Int16Fog enhanceTiLi;

	// 当前宿敌数
	public Int32Fog curSuDiCount;

	// 当前好友数
	public Int32Fog curFriendCount;

	//current Experience
	public Int32Fog curExp;
	//current VIP
	public Int32Fog curVipLevel;
	//昵称
	public string nickName;

	//体力 - 恢复的剩余全部体力的秒数
	public long unixTimeForTiLiFull;
	//精力 - 恢复的剩余全部精力的秒数
	public long unixTimeForJingLiFull;
	//体力 - 恢复的剩余体力的秒数
	public long unixTimeForTiLi;
	//精力 - 恢复的剩余精力的秒数
	public long unixTimeForJingLi;

	//当前拥有的建筑物的数量
	public Int32Fog curLoadBuildingCount;

	//当前进攻次数
	public Int32Fog curAttackCount;
	//当前防守次数
	public Int32Fog curDefendCount;
	//进攻获胜率
	public float AttackWinRate;
	//防守获胜率
	public float DefendWinRate;

	//尾兽数量
	public Int32Fog curHugeBeastCount;

	//功勋值
	public Int32Fog curGlory;

	// 掠夺积分
	public Int32Fog curQiangDuoJF;

	// 服务器时间
	public long systemTime;

	// 用户创建时间
	public long createTime;

	// 免战结束时间
	public long shiled;

	//头像ID
	public Int32Fog headID;

	public int[] aislt;

	public int happy;

	// 排名
	public Int32Fog curRank;

	//玩家的宠物队列
	public List<MonsterTeam> myTeams;
	//玩家有多少个宠物队列
	public int CapacityOfTeams;
	//玩家正在使用的宠物队列.
	public short curTeamId;
	/// <summary>
	/// 0 未领取 1 领取一次 2 领取两次
	/// </summary>
	public int masgn;
	//7天领奖
	public int sevenAward;

	//是否领取下载奖励(0 下载未领取，   1 下载已经领取)
	public int downloadReawrd;


	#region 玩家总共赌赢的次数

	public int TotalGambleWin;

	public int TotalGambleLose;

	#endregion

	//玩家所有战斗的Combo总和
	public int TotalCombo;


	//获取当前的队伍
	public MonsterTeam curTeam {
		get {
			MonsterTeam mTeam = null;
			foreach (MonsterTeam team in myTeams) {
				if(team != null && team.TeamId == curTeamId){
					mTeam = team;
					break;
				}
			}
			return mTeam;
		}
	}

	/// <summary>
	/// 根据队伍类型得到队伍
	/// </summary>
	/// <param name="type">队伍类型 ： 1 ： 攻击队， 2 ： 防御队</param>
	public MonsterTeam GetTeam(int type)
	{
		MonsterTeam mTeam = null;
		foreach (MonsterTeam team in myTeams) 
		{
			if(team != null && team.TeamId == type)
			{
				mTeam = team;
				break;
			}
		}
		return mTeam;
	}

	//根据teamid获取队伍信息
	public MonsterTeam getTeam(int teamId) {
		MonsterTeam mTeam = null;
		foreach (MonsterTeam team in myTeams) 
		{
			if(team != null && team.TeamId == teamId)
			{
				mTeam = team;
				break;
			}
		}
		return mTeam;
	}

	/// <summary>
	/// 查找出所有的队伍里面，star级别的宠物有多少个
	/// </summary>
	/// <returns>The by star.</returns>
	/// <param name="star">Star.</param>
	public int SplitByStar (int star){
		int total = 0;
		foreach(MonsterTeam team in myTeams) {
			if(team != null) {
				total += team.SplitByStar(star);
			}
		}

		return total;
	}

	/// <summary>
	/// 查找出所有的队伍里面，star级别的装备有多少个
	/// </summary>
	/// <returns>The split by star.</returns>
	/// <param name="star">Star.</param>
	public int EquipSplitByStar (int star) {
		int total = 0;
		foreach(MonsterTeam team in myTeams) {
			if(team != null) {
				total += team.equipSplitByStar(star);
			}
		}
		return total;
	}

	public RTPlayer ()
	{
		downloadReawrd = -1;
		myTeams = new List<MonsterTeam>();
	}

	public bool IsRegister
	{
		get
		{
#if NOGUIDE
			return true;
#else
			if(string.IsNullOrEmpty(nickName))
				return false;
			return !nickName.StartsWith(PlayerInfo.DEFAULT_NAME1) && !nickName.StartsWith(PlayerInfo.DEFAULT_NAME2);
#endif
		}
	}
}


#region 玩家头像类型<add by jc>
public enum PlayerHeadType 
{
	//常规
	General = 0,
	
	Vip = 1,

	//连击
    Combo = 2,
    
	//奖励,赌博
	Reward = 3,
}
#endregion



#region 复仇进度数据结构
//复仇进度数据 add by jc
public class RevengeProgressData
{
	public int curProgress;
	public int maxProgress;
	public int needStone;
	public RevengeProgressData(){}
	public RevengeProgressData(int _curProgress,int _maxProgress,int _needStone)
	{
		this.curProgress = _curProgress;
		this.maxProgress = _maxProgress;
		this.needStone = _needStone;
	}
}
#endregion
/*
 * 这里使用方法来计算动态的值
 */ 
public class PlayerManager : Manager {

	#region 获取当前等级的玩家经验值

	private bool upgrade = true;
	private UserLevelInfo _config;
	private int curMinExp = 0;
	private int curMaxExp = 0;

	//静态的等级数据
	public UserLevelInfo curConfig {
		get {
		
//			if(upgrade)
//			{
				curMinExp = 0;
				curMaxExp = 0;
				_config = null;

				foreach(UserLevelInfo lvInfo in ConfigList) {
					if(lvInfo != null ) {
						if(lvInfo.level == Lv) {
							_config = lvInfo;
							curMaxExp = lvInfo.exp;
							break;
						} else if(lvInfo.level < Lv) {
							curMinExp += lvInfo.exp;
							curMaxExp = curMinExp;
						} 
					}
				}
//				upgrade = false;
//			}
//			Utils.Assert(_config == null, "Can't find User Level Info");
			return _config;
		}
	}

	//复仇进度数据
	public RevengeProgressData  revengeData{get;set;}
	
	//设置当前用户等级
	public void SetCurUserLevel(int lv)
	{
		RTData.curLevel = lv;
		upgrade = true;
	}

	/// <summary>
	/// 返回当前等级的EXP最小数据
	/// </summary>
	/// <value>The get current lv minimum exp.</value>
	public int getCurLvMinExp {
		get {
			if(upgrade) {
				UserLevelInfo uli = curConfig;
				uli = null;
			}

			return curMinExp;
		}
	}

	/// <summary>
	/// 返回当前等级的EXP最大数据
	/// </summary>
	/// <value>The get current lv max exp.</value>
	public int getCurLvMaxExp {
		get {
			if(upgrade) {
				UserLevelInfo uli = curConfig;
				uli = null;
			}

			return curMaxExp;
		}
	}

	/// <summary>
	/// 当前进度的比值
	/// </summary>
	/// <value>The get curlv exp rate.</value>
	public float getCurlvExpRate {
		get {
			int curExpValue = getCurLvMaxExp - getCurLvMinExp;
			return (RTData.curExp.toInt32() * 1.0f) / curExpValue;
		}
	}

	#endregion


	//服务器传回来的信息，或者根据服务的数据计算出来的。
	public RTPlayer RTData;

	//用户每日状态信息
	public DayStatus dayStatus;

	//静态的等级数据列表
	public List<UserLevelInfo> ConfigList = null;

	public PlayerManager() {
		ConfigList = new List<UserLevelInfo>();
		//cache flag
		upgrade = true;
		RTData = new RTPlayer();
	}
	
	
	
	List<UserHeadData> UserHeadListConfig = new List<UserHeadData>(); 
    Dictionary<PlayerHeadType,Dictionary<int,UserHeadData>> headConfig ;
	#region override 覆盖基类的方法
	public override bool loadFromConfig() {
		return base.readFromLocalConfigFile<UserLevelInfo>(ConfigType.PlayerInfo, ConfigList)
		| base.readFromLocalConfigFile<UserHeadData>(ConfigType.UserHead, UserHeadListConfig);	
		
	}
	
	public  Dictionary<PlayerHeadType,Dictionary<int,UserHeadData>> HeadConfigData
	{
		get
		{
			if(headConfig == null)
			{
				headConfig = new Dictionary<PlayerHeadType, Dictionary<int, UserHeadData>>()
				{
					{PlayerHeadType.General,new Dictionary<int, UserHeadData>()},
                    {PlayerHeadType.Combo,new Dictionary<int, UserHeadData>()},
					{PlayerHeadType.Vip,new Dictionary<int, UserHeadData>()},
					{PlayerHeadType.Reward,new Dictionary<int, UserHeadData>()}
				};
				foreach(UserHeadData usrhead in UserHeadListConfig)
				{
					headConfig[(PlayerHeadType)usrhead.type].Add(usrhead.id,usrhead);
				}
			}
			return headConfig;
		}
	}
	
	

	//获取用户等级信息
	public UserLevelInfo GetUserLvlInfo(int level)
	{
		for(int i =0 ; i < ConfigList.Count; i++)
		{
			if (ConfigList [i].level == level)
			{
				return ConfigList [i];
			}
		}
		return null;
	}

	//
	public int GetMonSlotUnLockLvl(int monPos)
	{
		for(int i =0; i < ConfigList.Count; i++)
		{
			if(ConfigList[i].petSlot == monPos)
			{
				return ConfigList[i].level;
			}
		}
		return 1;
	}

	//加钱 -- add Exp
	//减少精力
	public void addItem2 (BaseHttpRequest request,BaseResponse response)
	//public override void addItem (BaseResponse response)
	{
		if(response != null && response.status != BaseResponse.ERROR) 
		{
			BattleResponse battleResp = response as BattleResponse;

			if(battleResp != null && battleResp.data != null)
			{
				addCoinOrExp(request,battleResp.data);
			}

			//同步精力 体力 神秘商店时间 排行
			int JL = -1;
			int pwr = -1;
			int rank = -1;
			if (battleResp != null && battleResp.data != null && battleResp.data.sync != null)
			{
				rank = battleResp.data.sync.rank;
				JL = battleResp.data.sync.eny;
				pwr = battleResp.data.sync.pwr;
			}
		
			if (JL != -1)
			{
				RTData.curJingLi = JL;
			}
			if (pwr != -1)
			{
				RTData.curTili = pwr;
			}
			if (rank != -1)
			{
				RTData.curRank = rank;
			}
		}

	}

	public void fightReward(BaseHttpRequest request, BaseResponse response)
	{
		BattleResponse battleResp = response as BattleResponse;
		if(response != null && response.status != BaseResponse.ERROR)
		{
			//开箱子添加得到的道具
			Core.Data.monManager.addItem(response); 
			Core.Data.EquipManager.addItem(response); 
			Core.Data.itemManager.addItem(response);
			Core.Data.playerManager.addItem2(request,response); 
			Core.Data.soulManager.addItem(response);
           
			addItem2(request, response);

			if(request != null && battleResp.data != null)
			{
				addCoinOrExp(request,battleResp.data);
			}
		}
	}
		
	private void addCoinOrExp (BaseHttpRequest request, BattleSequence reward) 
	{
		if(reward != null)
		{
			if(reward.sync != null)
			{
				if(!Core.Data.guideManger.isGuiding)
				{
					RTData.curCoin = reward.sync.coin;
					RTData.curExp = reward.sync.ep;
					RTData.curStone = reward.sync.stone;
					RTData.curVipLevel = reward.sync.vip;
				}
				else
				{
					//新手引导是假数据不要同步
					RTData.curCoin +=  (reward.reward.bco+reward.reward.eco);
				    RTData.curExp += (reward.reward.bep + reward.reward.eep);
				}
			}

			//可能会导致升等级
			if(RTData.curLevel < reward.sync.lv) 
			{
				HttpRequest req = request as HttpRequest;
		        BattleParam sendData= req.ParamMem as BattleParam;
				//如果是小关卡,打完以后直接升级,如果是BOSS关卡,升级处理将留到战斗播放完毕
				if(sendData != null) 
				{
					if(Core.Data.dungeonsManager.getFloorData(sendData.doorId).isBoss == 0)
						RTData.curLevel = reward.sync.lv;
					else 
						Core.Data.temper.AfterBattleLv = reward.sync.lv;
						
					upgrade = true;
				}
			}
			else if(RTData.curLevel == reward.sync.lv) 
			{
				//.. seems to do nothing
			}

		}

	}
	
    //购买免战牌
    public void BuyMianZhanPai(BaseResponse response) {
        if(response != null && response.status != BaseResponse.ERROR) {
            BuyMianZhanTimeResponse resp = response as BuyMianZhanTimeResponse;
            RTData.curCoin -= Math.Abs(resp.data.coin);
            RTData.curStone -= Math.Abs(resp.data.stone);
        }
    }

	//增加精力   吃拉面
	public void AddJingli(BaseResponse response) {
		HaveDinnerResponse eatDinner = response as HaveDinnerResponse;
		if (eatDinner != null) {
			if (eatDinner.status == 1) {
      
                if (RTData.curJingLi + eatDinner.data.eny >= 999)
                    RTData.curJingLi = 999;
                else   
                    RTData.curJingLi += eatDinner.data.eny;
		
			}
		}
	}

	#endregion

	public void ChangeUserInfo(BaseHttpRequest req, BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR) 
		{
			HttpRequest request = req as HttpRequest;
			ChangeUserInfoParam param = request.ParamMem as ChangeUserInfoParam;

			ChangeUserInfoResponse resp = response as ChangeUserInfoResponse;
			if (param.type == 2)
			{
				Core.Data.playerManager.RTData.nickName = param.param;
				Core.Data.playerManager.RTData.curStone += resp.data.stone;
			}
			else if (param.type == 1) 
			{
				Core.Data.playerManager.RTData.headID = int.Parse(param.param);
			}
		}
	}


	#region UI的方法集合
	public int Gs {
		get {
			return 0;
		}
	}
	//昵称
	public string NickName {
		get {
			if(RTData != null) {
				return RTData.nickName;
			} else {
				return string.Empty;
			}
		}
	} 
	//用户ID
	public string PlayerID {
		get {
			if(RTData != null) 
				return RTData.ID;
			else 
				return string.Empty;
		}
	}
	//用户的金币
	public int Coin {
		get {
			if(RTData != null)
				return RTData.curCoin;
			else 
				return 0;
		}
	}
	//用户的钻石
	public int Stone {
		get {
			if(RTData != null)
				return RTData.curStone;
			else
				return 0;
		}
	}
	//用户当前的等级
	public int Lv {
		get {
			if(RTData != null)
				return RTData.curLevel;
			else
				return 0;
		}
	}
	//用户当前的EXP
	public int curExp {
		get {
			if(RTData != null) 
				return RTData.curExp;
			else
				return 0;
		}
	}
	//用户当前下一等级的需求EXP
	public int nextLvExp {
		get {
			if(curConfig != null) {
				return curConfig.exp;
			} else 
				return 0;
		}
	}

	//当前用户的VIP等级
	public int curVipLv {
		get {
			if(RTData != null)
				return RTData.curVipLevel;
			else
				return 0;
		}
	}

	//当前用户的体力值
	public int curTiLi {
		get {
			if(RTData != null)
				return RTData.curTili;
			else
				return 0;
		}
	}
	//当前用户的精力
	public int curJingLi {
		get {
			if(RTData != null)
				return RTData.curJingLi;
			else 
				return 0;
		}
	}
	//当前等级的体力总值
	public int totalTili {
		get {
			if(curConfig != null && RTData != null)
				return curConfig.maxStamina + RTData.enhanceTiLi;
			else
				return 0;
		}
	}
	//当前等级的精力总值
	public int totalJingli {
		get {
			if(curConfig != null)
				return curConfig.maxEnergy + RTData.enhanceJingLi;
			else
				return 0;
		}
	}

	//体力百分比
	public float curTiLiPercent {
		get {
			return (curTiLi * 1.0f) / totalTili;
		}
	}

	//精力百分比
	public float curJingLiPercent {
		get {
			return (curJingLi * 1.0f) / totalJingli;
		}
	}


	public int remainingNextJingLiTime {
		get {
			return 0;
		}
	}

	public int remainingTotalJingLiTime {
		get {
			return 0;
		}
	}

	public int remainingNextTiLiTime {
		get {
			return 0;
		}
	}

	public int remainingTotalTiLiTime {
		get {
			return 0;
		}
	}

	public int curTeamMemberCount {
		get {
			return 0;
		}
	}


	#endregion

	/// <summary>
	/// Fullfills the by network. 根据参数就是知道MonsterManager,equipManager 必须先初始化好
	/// </summary>
	/// <param name="response">Response.</param>
	/// <param name="monManager">Mon manager.</param>
	/// <param name="equipManager">Equip manager.</param>
	public void fullfillByNetwork(BaseResponse response, MonsterManager monManager, EquipManager equipManager) {

		if(response != null && response.status != BaseResponse.ERROR) {
			LoginResponse resp = response as LoginResponse;
			PlayerInfo playerInfo = resp.data.user;

			if(playerInfo != null) {
				RTData.ID = Convert.ToString(playerInfo.id);
				if(playerInfo.accountId != 0)
					RTData.uId = Convert.ToString(playerInfo.accountId);
				RTData.curExp = playerInfo.exp;
				RTData.nickName = playerInfo.name;
				RTData.curLevel = playerInfo.lv;
				RTData.curVipLevel = playerInfo.vip;
				RTData.curCoin = playerInfo.coin;
				RTData.curStone = playerInfo.stone;
				RTData.curJingLi = playerInfo.jl;
				RTData.curTili = playerInfo.tl;
				RTData.isFirstData = true;
				if(HttpRequestFactory._sessionId == "empty")
					HttpRequestFactory._sessionId = playerInfo.sessionId;

				RTData.unixTimeForJingLi = playerInfo.jldur;
				RTData.unixTimeForJingLiFull = playerInfo.jldurfull;
				RTData.unixTimeForTiLi = playerInfo.tldur;
				RTData.unixTimeForTiLiFull = playerInfo.tldurfull;
				
				RTData.systemTime = playerInfo.systime;
				RTData.createTime = playerInfo.createTime;
				RTData.shiled = playerInfo.shiled;

				RTData.aislt = playerInfo.aislt;
				RTData.headID = playerInfo.headID;
				RTData.happy = playerInfo.happy;
				if (RTData.headID == 0)
				{
					RTData.headID = PlayerInfo.DEFAULT_HEAD;
				}

				Core.TimerEng.OnLogin(RTData.systemTime);

				RTData.curRank = playerInfo.rank;

				RTData.curTeamId = (short)playerInfo.team;
				RTData.curGlory = playerInfo.glory;

				RTData.curSuDiCount = playerInfo.suDiCount;
				RTData.curFriendCount = playerInfo.friendCount;

				RTData.sevenAward = playerInfo.sevenAward;

				dayStatus = playerInfo.dayStatus;
			}

			//added by zhangqiang to check weither add a new team ;
            if(resp.data.monteam != null) {
                int teamLength = resp.data.monteam.Length;
                if(RTData.curTeamId > teamLength)
                {
                    List<MonsterTeamInfo> listTeamInfo = new List<MonsterTeamInfo>(resp.data.monteam);

                    for(int j = teamLength; j < RTData.curTeamId; j++) {
                        MonsterTeamInfo team = new MonsterTeamInfo();
                        team.teamid = (short)(j + 1);
                        listTeamInfo.Add(team);
                    }

                    resp.data.monteam = listTeamInfo.ToArray();
                }

                setTeamList(resp.data.monteam, resp.data.equip, monManager, equipManager);
            }
			else if(RTData.myTeams.Count == 0)
			{
				List<MonsterTeamInfo> listTeamInfo = new List<MonsterTeamInfo> ();
				MonsterTeamInfo team = new MonsterTeamInfo ();
				team.teamid = RTData.curTeamId;
				listTeamInfo.Add (team);
				setTeamList (listTeamInfo.ToArray(), resp.data.equip, monManager, equipManager);
			}
			
			if(Core.Data.temper.mPreLevel == -1)
			{
				Core.Data.temper.mPreLevel = RTData.curLevel;
				Core.Data.temper.mPreVipLv = RTData.curVipLevel;
			}
		}
	}

	//创建队伍数据
	private void setTeamList(MonsterTeamInfo[] TeamList, EquipTeam[] equipTeam, MonsterManager monManager, EquipManager equipManger) {
		int capacity = 14;

		if(TeamList != null) 
		{
            //clear dirty data
            RTData.myTeams.Clear();

			foreach(MonsterTeamInfo mti in TeamList) 
			{
				RTData.myTeams.Add(mti.toMonsterTeam(capacity, monManager));
			}

			setEquipment(equipTeam, equipManger);
		}

	}

	//创建队伍装备数据
	private void setEquipment(EquipTeam[] equipTeam, EquipManager equipManger) {
		if(equipTeam != null) {
			foreach(EquipTeam et in equipTeam) {
				if(et != null) {
					MonsterTeam team = RTData.getTeam(et.id);
					if(team != null && et.EquipIdList != null) {
						int length = et.EquipIdList.Count;
						for(int memberPos = 0; memberPos < length; ++ memberPos) {
							if(et.EquipIdList[memberPos] != null) {
								foreach(int eqId in et.EquipIdList[memberPos]) {
									team.setEquip(equipManger.getEquipment(eqId), memberPos);
								}
							}
						}
					}
				}
			}
		}
	}

	/// <summary>
	/// Changes the team member. 更改队伍类型
	/// </summary>
	public void ChangeTeamMember(BaseHttpRequest request, BaseResponse response, MonsterManager monManager)
	{
		if( request != null && response != null && monManager != null)
		{
			ChangeTeamMemberResponse resp = response as ChangeTeamMemberResponse;

			if(resp != null && resp.data)
			{
				if(request.baseType == BaseHttpRequestType.Common_Http_Request) 
				{
					HttpRequest req = request as HttpRequest;
					if(req != null) 
					{
						ChangeTeamMemberParam param = req.ParamMem as ChangeTeamMemberParam;
						if(param != null) 
						{
							//获取编队
							MonsterTeam team = RTData.getTeam(param.tmid);
							//获取宠物
							Monster mon = monManager.getMonsterById(param.droleid);
							if(team != null && mon != null) 
							{
								team.setMember(mon, param.pos - 1);
							} 
							else if(team != null)
							{ //如果不换上新宠物的话
								team.removeMember(param.pos - 1);
							}

						}
					}
				}

			}

		}
	}

	/// <summary>
	/// Changes the team equip. 更改队伍的装备
	/// </summary>
	/// <param name="request">Request.</param>
	/// <param name="response">Response.</param>
	/// <param name="equipManager">Equip manager.</param>
	public void ChangeTeamEquip(BaseHttpRequest request, BaseResponse response, EquipManager equipManager) {
		if( request != null && response != null && equipManager != null) {
			ChangeEquipmentResponse resp = response as ChangeEquipmentResponse;

			if(resp != null && resp.data) {

				if(request.baseType == BaseHttpRequestType.Common_Http_Request) {
					HttpRequest req = request as HttpRequest;
					if(req != null) {
						ChangeEquipmentParam param = req.ParamMem as ChangeEquipmentParam;

						if(param != null) {
							//获取编队
							MonsterTeam team = RTData.getTeam(param.tmid);
							//获取装备
							Equipment srceq = equipManager.getEquipment(param.seqid);
					
							if(team != null && srceq != null) 
							{
								if(srceq != null) team.removeEquip(srceq, param.pos - 1);
							}

							Equipment equip = equipManager.getEquipment(param.teqid);
							if(team != null && equip != null)
							{
								team.setEquip(equip, param.pos - 1);
							} 
						}

					}

				}

			}

		}

	}

	public void SwapTeam(BaseHttpRequest request, BaseResponse response)
	{
		if(response != null && response.status != BaseResponse.ERROR)
		{
			SwapTeamResponse resp = response as SwapTeamResponse;

			HttpRequest req = request as HttpRequest;
			SwapTeamParam param = req.ParamMem as SwapTeamParam;

			if(resp.data)
			{
				Core.Data.playerManager.RTData.curTeamId = (short)param.teamid;

				//MonsterTeam team = RTData.curTeam;
				if(RTData.curTeam == null)
				{
					MonsterTeamInfo teamInfo = new MonsterTeamInfo();
					teamInfo.teamid = (short)param.teamid;
					RTData.myTeams.Add(teamInfo.toMonsterTeam(14, Core.Data.monManager));
				}
			}

		}
	}

	public void SwapMonsterPos(BaseHttpRequest request, BaseResponse response)
	{
		RED.Log("swap monster success");
		if(response != null && response.status != BaseResponse.ERROR)
		{
			HttpRequest req = request as HttpRequest;
			SwapMonsterPosParam param = req.ParamMem as SwapMonsterPosParam;

			int srcPos = RTData.curTeam.GetMonsterPos(param.sroleid);
			int tgtPos = RTData.curTeam.GetMonsterPos(param.troleid);


			Equipment srcAtk = RTData.curTeam.getEquip(srcPos, 0);
			Equipment srcDef = RTData.curTeam.getEquip(srcPos, 1);

			Equipment tgtAtk = RTData.curTeam.getEquip(tgtPos, 0);
			Equipment tgtDef = RTData.curTeam.getEquip(tgtPos, 1);

			RTData.curTeam.removeEquip(srcAtk, srcPos);
			RTData.curTeam.removeEquip(srcDef, srcPos);

			RTData.curTeam.removeEquip(tgtAtk, tgtPos);
			RTData.curTeam.removeEquip(tgtDef, tgtPos);

			RTData.curTeam.setEquip(tgtAtk, srcPos);
			RTData.curTeam.setEquip(tgtDef, srcPos);

			RTData.curTeam.setEquip(srcAtk, tgtPos);
			RTData.curTeam.setEquip(srcDef, tgtPos);
		}
	}

	public void SellMonster(BaseResponse response)
	{
		ConsoleEx.Write("sell monster Sucess to Update coin");
		if(response != null && response.status != BaseResponse.ERROR) {
			SellMonsterResponse resp = response as SellMonsterResponse;
			if(resp != null) {
				RTData.curCoin += resp.data;
			}
		}
	}

	public void SyncMoney(BaseResponse response)
	{
		if(response != null && response.status != BaseResponse.ERROR) {
			SyncMoneyResponse resp = response as SyncMoneyResponse;
			if(resp != null) {
				RTData.curCoin = resp.data.coin;
				RTData.curStone = resp.data.s;

				AsyncTask.QueueOnMainThread( () => { if(DBUIController.mDBUIInstance != null)  DBUIController.mDBUIInstance.RefreshUserInfoWithoutShow(); });
			}
		}
	}

	public void StrengthenMonster(BaseResponse response)
	{
		ConsoleEx.Write("strengthen monster sucess to update coin");
		if(response != null && response.status != BaseResponse.ERROR) {
			StrengthenResponse resp = response as StrengthenResponse;
			if(resp != null) {
				RTData.curCoin += resp.data.coin;
			}
		}
	}

	public void StrengthEquip(BaseResponse reponse)
	{
		ConsoleEx.Write("strengthen equip sucess to update coin");
		if(reponse != null && reponse.status != BaseResponse.ERROR) 
		{
			StrengthEquipResponse resp = reponse as StrengthEquipResponse;
			if(resp != null) {
				RTData.curCoin += resp.data.coin;
			}
		}
	}


	//建造建筑
	public void BuildCreate(BaseResponse reponse)
	{
		if (reponse != null && reponse.status != BaseResponse.ERROR)
		{
			//			BuildOperateResponse resp = reponse as BuildOperateResponse;
			//to add.......
		}
	}

	//属性变换
	public void AttrSwap(BaseResponse reponse)
	{
		if (reponse != null && reponse.status != BaseResponse.ERROR)
		{
			AttrSwapResponse resp = reponse as AttrSwapResponse;
			RTData.curCoin += resp.data.coin;
			RTData.curStone += resp.data.stone;
		}
	}

	//潜力训练
	public void QianLiXunLian(BaseResponse reponse)
	{
		if (reponse != null && reponse.status != BaseResponse.ERROR)
		{
			QianLiXunLianResponse resp = reponse as QianLiXunLianResponse;
			RTData.curCoin += resp.data.coin;
			RTData.curStone += resp.data.stone;
		}
	}

	//潜力训练
	public void QianLiReset(BaseResponse reponse)
	{
		if (reponse != null && reponse.status != BaseResponse.ERROR)
		{
			QianLiResetResponse resp = reponse as QianLiResetResponse;
			RTData.curCoin += resp.data.coin;
			RTData.curStone += resp.data.stone;

		}
	}

//	public void BuildGet(BaseHttpRequest request, BaseResponse reponse)
//	{
//		if (reponse != null && reponse.status != BaseResponse.ERROR)
//		{
//			BuildGetResponse resp = reponse as BuildGetResponse;
//
//			HttpRequest req = request as HttpRequest;
//			BuildGetParam param = req.ParamMem as BuildGetParam;
//			if (resp != null && resp.data != null )
//			{
//				RTData.curCoin += resp.data.get[0];
//				RTData.curJingLi += resp.data.get[1];
//				RTData.curTili += resp.data.get[1];
//			}
//		}
//	}
//
//	public void BuildGetNow( BaseResponse reponse)
//	{
//		if (reponse != null && reponse.status != BaseResponse.ERROR)
//		{
//			BuildGetNowResponse resp = reponse as BuildGetNowResponse;
//			RTData.curStone += resp.data.s;
//			RTData.curCoin += resp.data.coin;
//		}
//	}
//
//	public void BuildUpgrade(BaseResponse reponse)
//	{
//		if (reponse != null && reponse.status != BaseResponse.ERROR)
//		{
//			BuildUpgradeResponse resp = reponse as BuildUpgradeResponse;
//			RTData.curStone += resp.data.s;
//			RTData.curCoin += resp.data.coin;
//		}
//	}

	public void ZhaoMu(BaseResponse reponse)
	{
		if (reponse != null && reponse.status != BaseResponse.ERROR)
		{
			ZhaoMuResponse resp = reponse as ZhaoMuResponse;
			if (resp.data != null)
			{
				RTData.curStone += resp.data.so;
				RTData.curCoin += resp.data.co;
			}
		}
	}

	public void SellEquip(BaseResponse reponse)
	{
		ConsoleEx.Write("sell equip sucess to update coin");
		if(reponse != null && reponse.status != BaseResponse.ERROR) 
		{
			SellEquipResponse resp = reponse as SellEquipResponse;
			if(resp != null) {
				RTData.curCoin += resp.data;
			}
		}
	}

    public void UseProp(BaseHttpRequest r, BaseResponse reponse)
	{
		ConsoleEx.Write("use prop to update coin");
		if(reponse != null && reponse.status != BaseResponse.ERROR) 
		{
			UsePropResponse resp = reponse as UsePropResponse;
			if(resp != null && resp.data != null) 
			{
				RTData.curCoin += resp.data.coin;

				RTData.curJingLi += resp.data.eny;
				if (RTData.curJingLi > 999)
				{
					RTData.curJingLi = 999;
				}

				RTData.curTili += resp.data.pwr;
				if (RTData.curTili > 99)
				{
					RTData.curTili = 99;
				}

			    RTData.curStone += resp.data.stone;
			}
		}
	}
    //购买精力--- ycg
    public void UseBuyEnergy(BaseHttpRequest r, BaseResponse reponse)
    {
        if(reponse != null && reponse.status != BaseResponse.ERROR) 
        {
            getJLData re = reponse as getJLData;
            RTData.curJingLi += re.data.eny;
            if (RTData.curJingLi > 999)
            {
                RTData.curJingLi = 999;
            }
            RTData.curStone +=re.data.stone ;

        }

    }

    public void BuyItem(BaseResponse reponse)
    {
        ConsoleEx.Write("Buy Item sucess to update coin");
		if(reponse != null && reponse.status != BaseResponse.ERROR) 
        {
			if(reponse is BuyItemResponse)
			{
				BuyItemResponse resp = reponse as BuyItemResponse;
				if(resp != null) 
				{
					RTData.curCoin -= resp.data.coin;
					RTData.curStone -= resp.data.stone;

					//购买得到的钱和钻石
					if (resp.data.Result != null)
					{
						RTData.curJingLi += resp.data.Result.eny;
						RTData.curTili += resp.data.Result.pwr;
						RTData.curStone += resp.data.Result.stone;
						RTData.curCoin += resp.data.Result.coin;
					}
				}
			}
			else if(reponse is SecretShopBuyResponse)
			{
				SecretShopBuyResponse resp = reponse as SecretShopBuyResponse;
				if(resp != null) 
				{
					RTData.curCoin += resp.data.coin;
					RTData.curStone += resp.data.stone;
				}
			}
			else if(reponse is QiangDuoGoldBuyItemResponse)
			{
				QiangDuoGoldBuyItemResponse resp = reponse as QiangDuoGoldBuyItemResponse;
				if(resp != null) 
				{
					RTData.curCoin += resp.data.coin;
					FinalTrialMgr.GetInstance().TotalJifen += resp.data.score;
				}
			}
			else if(reponse is NewFinalTrialFightResponse)
			{
				NewFinalTrialFightResponse fightres = reponse as NewFinalTrialFightResponse;
				if(fightres != null && fightres.data!=null && fightres.data.rushResult != null && fightres.data.rushResult.award != null)
				{
					;
				}
			}
			else if(reponse is GuaGuaLeResponse)
			{
				GuaGuaLeResponse res = reponse as GuaGuaLeResponse;
				if(res != null && res.data != null)
				{
					RTData.curStone += res.data.stone;
				}
			}
			else if(reponse is UsePropResponse)
			{
				UsePropResponse res = reponse as UsePropResponse;
				if(res != null && res.data != null)
				{
                    RTData.curStone += res.data.stone;
                }
            }
			else if(reponse is RefreshZhangongShopItemResponse)
			{
				RefreshZhangongShopItemResponse res = reponse as RefreshZhangongShopItemResponse;
				if(res != null && res.data != null)
				{
					RTData.curStone += res.data.stone;
				}
			}
            else if(reponse is GetFinalTrialStateResponse)
			{
				GetFinalTrialStateResponse res = reponse as GetFinalTrialStateResponse;
				if(res != null && res.data != null)
				{
					if(res.data.shalu != null && res.data.shalu.costStone != 0)
					{
						if(FinalTrialMgr.GetInstance().ShaluBuouResetState == 1)
						{
							RTData.curStone -= res.data.shalu.costStone;
						}
						else if(FinalTrialMgr.GetInstance().ShaluBuouResetState == 2)
						{
							RTData.curStone -= res.data.buou.costStone;
						}
						AsyncTask.QueueOnMainThread( () => { if(DBUIController.mDBUIInstance != null)  DBUIController.mDBUIInstance.RefreshUserInfo(); });
					}
				}
			}

        }
    }

	public void BuyLottery(BaseResponse reponse){
        //	ConsoleEx.Write("Buy lottery sucess to update diamon");
		if(reponse != null && reponse.status != BaseResponse.ERROR) 
		{
			SockFestivalBuyLotteryResponse resp = reponse as SockFestivalBuyLotteryResponse;
			if(resp != null) 	
			{
				if (resp.data.retCode == 1) {
                    if (resp.data.type == 2) {
                        RTData.curStone += resp.data.stone ; 
                    }
				}
			}
		}	
	}

	public void AddPowerUpdate(BaseResponse reponse){
        //	ConsoleEx.Write("增加效果 更新钻石");
		if(reponse != null && reponse.status != BaseResponse.ERROR) 
		{
			SockAddPowerResponse addPower = reponse as SockAddPowerResponse;

			if(addPower != null) 
			{
				if(addPower.data.type== 0) //金币

                    RTData.curCoin -= addPower.data.count;
				else
				{
                    RTData.curStone -= addPower.data.count;
				}

			}
		}	

	}

	public void DecomposeMon(BaseResponse reponse)
	{
		if (reponse != null && reponse.status != BaseResponse.ERROR)
		{
			DecomposeMonsterResponse resp = reponse as DecomposeMonsterResponse;
			if (resp != null && resp.data != null)
			{
				RTData.curCoin += resp.data.coin;
			}
		}
	}

	public void EvolveMonster(BaseResponse reponse)
	{
		if (reponse != null && reponse.status != BaseResponse.ERROR)
		{
			EvolveMonsterResponse resp = reponse as EvolveMonsterResponse;
			if (resp != null && resp.data != null)
			{
				RTData.curCoin += resp.data.coin;
			}
		}
	}

	public void AttackBossUpdate(BaseResponse reponse){
        //	ConsoleEx.Write("钻石更新");
		if (reponse != null && reponse.status != BaseResponse.ERROR) {
			SockAttackBossResponse sockAttack = reponse as SockAttackBossResponse;

			if (sockAttack != null) {
				if (sockAttack.data.battleData.isPay == 1) //金币
                    RTData.curStone += sockAttack.data.battleData.stone;
			}

		}
	}	
		
	public void BuQianUpdateStone(BaseResponse reponse){
		if (reponse != null && reponse.status != BaseResponse.ERROR) {
			SignDayResponse SDResponse = reponse as SignDayResponse;
			if (SDResponse != null) {
                if (SDResponse.data.stone != 0)
                {
                    //补签次数 不更改今天签到次数
                    Core.Data.playerManager.RTData.curStone += SDResponse.data.stone;
                }
                else
                {
                    //正常签到更改  今天签到次数 
                    RTData.masgn++;
 
                }
                //增加道具 
                if(SDResponse.data.p!= null){
                    for (int i = 0; i < SDResponse.data.p.Length; i++)
                    {
                        if(SDResponse.data.p[i] != null)
                            Core.Data.itemManager.AddRewardToBag(SDResponse.data.p[i]);

                    }
                }
			}
		}

	}
    /// <summary>
    /// 类型 1：金币  2：钻石
    /// </summary>
    /// <param name="delta">Delta.</param>
    /// <param name="type">Type.</param>
    public void ReduceCoin(int delta,int type =1){
        // ConsoleEx.Write ("  cur coin "+RTData.curCoin + " delta =" + delta);
        if (type == 1) {
            if (RTData.curCoin + delta >= 0) {
                RTData.curCoin += delta;
            } else {
                RTData.curCoin = 0;
            }
        } else {
            if (RTData.curStone + delta >= 0) {
                RTData.curStone += delta;
            } else {
                RTData.curStone = 0;
            }
        }
    }


	//是否需要更新阵容的红点儿
	public bool IsTeamBetter()
	{
		if (curConfig.petSlot > RTData.curTeam.validateMember)
		{
			if (Core.Data.monManager.GetValidMonCount (SplitType.Split_If_InCurTeam) > 0)
			{
				return true;
			}
		}

		for (int i = 0; i < 2; i++)
		{
			if (curConfig.petSlot > RTData.curTeam.GetValidEquipCount (i))
			{
				int count = Core.Data.EquipManager.GetValidEquipCount (i, SplitType.Split_If_InCurTeam);
				if (count > 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	//首次充值奖励，如果小于等于0则代表没有，大于0则代表有
	public long FirstPurchaseReward {
		get {
			long end = 86400 * 3;
			long time = RTData.createTime + end - Core.TimerEng.curTime;
			if(time < 0) time = 0;
			return time;
		}
	} 
}
