using System;
using Framework;
using fastJSON;
using System.Collections.Generic;
using AW.Battle;

/*
 * 
 * 所有的服务器响应格式的定义都在这里。
 * 
 */ 


#region 获取服务器分区的信息

[Serializable]
public class Server {
    //1 = 新服 2=火爆的服务器 3=维护 4=停服 5=合服 6=推荐服
	public const int STATUS_NEW = 1;
	public const int STATUS_HOT = 2;
	public const int STATUS_FULL = 3;
	public const int STATUS_STOP = 4;
	public const int STATUS_COMBINE = 5;
    public const int STATUS_RECOMMAND = 6;

	public int sid;
	public int id;
	public string name;
	public int status;
	public string url;
	//计费服务器地址
	public string payUrl;

    //------  only for test --- ,正式发布删除
	public const string outNetActiveURL = "42.121.116.36";//"42.121.194.167";
    public const string outNetChatURL = "42.62.30.82";//"42.62.30.82";

//    public const string inNetAcitveURL = "192.168.1.110";//110
//    public const string inNetChatURL = "192.168.1.110";

	public const string inNetAcitveURL = "192.168.1.110";//110
	public const string inNetChatURL = "192.168.1.110";

    //聊天地址
	public string chat_ip;
    //活动地址
	public string active_ip;
    //端口号
    public int chat_port;
    public int active_port;

	public Server() { 
    }
}

[Serializable]
public class PartitionServer {
	//All servers
	public Server[] sv;
	//last表示最近登陆过的服务器id
	public int last;

	/// 
	/// ---------- 公告的内容和标题 ------------
	/// 
	public string noticeContent;
	public string noticeTitle;

	//第三方token（快速登录没用）
	public string platToken;

	//第三方唯一ID（快速登录没用）
	public string platId;

	//游戏登陆token
	public string token;

	public PartitionServer() { }
}

[Serializable]
public class GetPartitionServerResponse : BaseResponse
{
	public PartitionServer data;

	public GetPartitionServerResponse () { }
}



[Serializable]
public class ThirdGetServerResponse : BaseResponse
{
	public PartitionServer data;

	public ThirdGetServerResponse () { }
}





#endregion

#region 获取资源更新
[Serializable]
public class UpdateDetails {
	public string fn;
	//资源类型 配置文件和资源文件
	public short type;
	public long size;
	public string md5;
}

[Serializable]
public class ResourcesUpdateInfo {
	//下载地址, 完整的下载地址是: url + file.fn
	public string url;
	public UpdateDetails[] file;
}

[Serializable]
public class ResourceResponse : BaseResponse {
	public ResourcesUpdateInfo data;

	public ResourceResponse() { }
}

#endregion


#region 登陆

//神龙的信息
[Serializable]
public class DragonInfo {
	//地球为1，那美克星为2
	public short num;
	//持续时间
	public long dur;
	//开始时间
	public long st;

	public int lv;
	//经验值
	public int ep;
}

//关卡进度的信息
[Serializable]
public class DungeonsInfo {
	//Int[]里面的位置
	public const int ID_POS = 0x0;
	public const int PRO_POS = 0x1;

	//每个子关卡的进度
	public List<int[]> pro;
	//最后的打过的子关卡ID
	public int end;
}

//宝石背包的信息
public class GemInfo {
	public int id;
	public int num;
}

[Serializable]
// 玩家建筑物信息 bd
public class BuildingTeamInfo 
{
	public int num; // 编号
	public int id; // ID
	public int lv; // 等级

	// 建筑冷却剩余时间
	public long dur  = 0;

	// 如果是资源建筑，表示是否被抢
	public int robc; 
	//建筑物被抢夺的比例
	public float robRate= 0; 			

	public int coin;			//收取的金币
	public int stone;			//收取的钻石

	public 	int propId  = 0;	//消耗道具ID
	public 	int propNum = 0;	//消耗道具数量

	public 	int openType = 0;	//建筑物开启类型0：未开启 1：小电池开启，2：中电池开启；3：大电池开启    开启建筑物 时候  添加 type 
}   

[Serializable]
public class EquipTeam {
	public int id;

	public List<int[]> EquipIdList = null;
	//------------------ 位置1  丑陋的数据格式， ------------------
	//--------- 里面是装备ID，同时没有顺序 ----------

	#region ugly
	public int[] p1;
	public int[] p2;
	public int[] p3;
	public int[] p4;
	public int[] p5;
	public int[] p6;
	public int[] p7;
	public int[] p8;
	public int[] p9;
	public int[] p10;
	public int[] p11;
	public int[] p12;
	public int[] p13;
	public int[] p14;
	public int[] p15;

	public void HumanReadable() {
		EquipIdList = new List<int[]>();

		EquipIdList.Add(p1);
		EquipIdList.Add(p2);
		EquipIdList.Add(p3);
		EquipIdList.Add(p4);
		EquipIdList.Add(p5);
		EquipIdList.Add(p6);
		EquipIdList.Add(p7);
		EquipIdList.Add(p8);
		EquipIdList.Add(p9);
		EquipIdList.Add(p10);
		EquipIdList.Add(p11);
		EquipIdList.Add(p12);
		EquipIdList.Add(p13);
		EquipIdList.Add(p14);
		EquipIdList.Add(p15);
	}

	#endregion
}

//宝石id和宝石lv
[Serializable]
public class EquipSlot {
	//if id = 0, which menas no add gems
	public int id;
	public short color;
	//this will be fullfilled when created Equipment.
	public Gems mGem;
	/// <summary>
	/// Gets the color of the slot.
	/// </summary>
	/// <value>The color of the s.</value>
	public SlotOrGemsColor eqColor {
		get {
			SlotOrGemsColor clr = (SlotOrGemsColor) color;
			if(Enum.IsDefined(typeof(SlotOrGemsColor), clr))
				return clr;
			else {
				return SlotOrGemsColor.Default_No;
			}
		}
	}

	public bool hasGemsEquipped {
		get {
			return id != 0;
		}
	}

}

//物品信息 - 包括各种道具，不包括镶嵌在装备上的宝石信息
[Serializable]
public class ItemInfo {
	public int id;
	public int num;
	public int count;

	public ItemInfo()
	{

	}
	public ItemInfo(int id, int num, int count)
	{
		this.id = id;
		this.num = num;
		this.count = count;
	}
}

//魂魄信息
[Serializable]
public class SoulInfo {
	public int id;
	public int num;
	public int count;

	public SoulInfo()
	{

	}
	public SoulInfo(int id, int num, int count)
	{
		this.id = id;
		this.num = num;
		this.count = count;
	}
}

//装备信息
[Serializable]
public class EquipInfo {
	public int id;
	public short lv;
	public int num;
	public int exp;

	public short[] slc;
	public int[] slt;

	private EquipSlot[] _slot;
	public EquipSlot[] slot {
		get {
			if(_slot != null) 
				return _slot;
			else {
				if(slc != null && slt != null) {
					EquipSlot[] eslot = new EquipSlot[slc.Length];
					for(int i = 0; i < slc.Length; ++ i) {
						EquipSlot es = new EquipSlot();
						es.id = slt[i];
						es.color = slc[i];
						eslot[i] = es;            
					}
					_slot = eslot;
					return _slot;
				} else {
					return null;
				}
			}
		}
	}

	/// <summary>
	/// Gets the slot count.该装备的宝石槽数量
	/// </summary>
	/// <value>The slot count.</value>
	public int SlotCount {
		get {
			if(slc != null) {
				return slc.Length;
			} else {
				return 0;
			}
		}
	}

	//已装备的宝石个数
	public int EquipedGemCount
	{
		get
		{
			if(slot == null || slot.Length == 0)
			{
				return 0;
			}
			
			int count = 0;
			foreach(EquipSlot itor in slot)
			{
				if(itor.id > 0)
				{
					count++;
				}
			}
			return count;
		}
	}
}

//宠物队伍信息
[Serializable]
public class MonsterTeamInfo {
	public short teamid;
	public Dictionary<string, int> mon;

	public MonsterTeam toMonsterTeam (int capcity, MonsterManager monManager) {
		MonsterTeam team = new MonsterTeam(teamid, capcity);

		if(mon != null) 
		{
			foreach (KeyValuePair<string, int > itor in mon)
			{

				int pos = int.Parse (itor.Key);
				pos = pos % 100;
				pos -= 1;
				Monster monster = monManager.getMonsterById(itor.Value);
				team.setMember(monster, pos);
			}
		}

		return team;
	}
}


[Serializable]
public class MonsterInfo {
	public int lv;
	public int num;
	public int id;
	public int exp;
	public int star;
	//攻,防 加值
	public int[] plus;
	//宠物的属性
	public int attri;
	public int uspt;

	//技能
	public SkillLevel[]	skillLevel;

	public MonsterAttribute getAttribute {
		get {
			if(Enum.IsDefined(typeof(MonsterAttribute), attri))
				return (MonsterAttribute)attri;
			else {
				return MonsterAttribute.DEFAULT_NO;
			}
		}
	}

	/// <summary>
	/// Easy way to the monster. 
	/// </summary>
	/// <returns>The monster.</returns>
	public Monster toMonster(MonsterManager monManager) {
		Monster mon = new Monster();
		mon.pid = id;
		mon.num = num;
		mon.RTData = new RuntimeMonster();
		mon.RTData.curLevel = lv;
		mon.RTData.addStar = star;
		mon.RTData.curExp = exp;
		mon.RTData.uspt = uspt;
		if(plus != null && plus.Length == (int)ChaKeLaType.Privot) {
			mon.RTData.ChaKeLa_Attck = plus[(int)ChaKeLaType.Attack];
			mon.RTData.ChaKeLa_Defend = plus[(int)ChaKeLaType.Defend];
		}
		mon.RTData.Attribute = getAttribute;	
	
		mon.InitConfig();

		if (skillLevel != null)
		{
			for (int i = 0; i < skillLevel.Length; i++)
			{
				for(int j = 0; j < mon.skillLvs.Length; j++)
				{
					if(mon.skillLvs[j].skillId == skillLevel[i].skillId)
					{
						mon.skillLvs [j] = skillLevel [i];
						break;
					}
				}
			}
		}

		mon.BTData = new BattleMonster(mon.baseAttack, mon.enhanceAttack, mon.baseDefend, mon.enhanceDefend);
		return mon;
	}
}

//玩家的基本信息
[Serializable]
public class PlayerInfo {
	//角色ID
	public int id;
	//账号ID
	public int accountId;
	//
	public string sessionId;

	public string name;
	//引导完成进度 -1标识未开启新手引导，0标识完成了新手引导
	public int guide;
	//当前等级
	public int lv;
	//Vip等级
	public short vip;
	//金币
	public int coin;
	//钻石
	public int stone;
	//精力值
	public int jl;
	//精力+1点的秒数
	public long jldur;
	//下一个精力+1的秒数
	public long jldurfull;
	//体力值
	public int tl;
	//体力+1点的秒数
	public long tldur;
	//下一个体力+1的秒数
	public long tldurfull;
	//当前使用的队伍
	public int team;
	//战功值
	public int glory;

	//神龙祭坛奥义凹槽购买状态 1为已经购买 0为还没购买
	public int[] aislt;

	//当前的经验值
	public int exp;
	//服务器的当前时间
	public long systime;
	//创建时间
	public long createTime;

	//免战结束时间
	public long shiled;
	/// <summary>
	/// masgn代表今日领取签到奖励标识 0 未领取 1 领取一次 2 领取两次
	/// </summary>
    //	public int masgn;
	//头像ID
	public int headID;
	//祝福值
	public int happy;

	public int rank = 909;
    //  public Dinner dinner;


	public const string DEFAULT_NAME1 = "ZQ250_";
	public const string DEFAULT_NAME2 = "@*ZQ250*@";
	public const int DEFAULT_HEAD = 10142;

	// 当前宿敌数
	public int suDiCount;

	// 当前好友数
	public int friendCount;

	//7天领奖
	public int sevenAward;
	
	//每日状态
	public DayStatus dayStatus;
}


// 用户每天的状态
public class DayStatus
{
	// 体力道具使用次数  --- 暂时没有
	public int tlUse = 0;

    // 精力道具使用次数--- 暂时没有
	public int jlUse = 0;

    //今天买精力次数  
    public int buyEnyCount = 0;

	// 免费超忍使用
	public int freeRecruit = 0;

	// 免费终极试炼次数
	public int bossBattle = 0;

	// 免费沙鲁挑战次数
	public int bloodshaluBattle = 0;

	// 免费布欧挑战次数
	public int bloodbuouBattle = 0;

	// 免费排行榜boss挑战
	public int freeChallenges = 0;

	// 免费抢夺金币次数
	public int freeRob = 0;

	// 免费抢夺龙珠次数
	public int freeRobBall = 0;

	// vip等级奖励领取次数
	public int vipaward = 0;

	// 免费世界聊天使用次数
	public int worldchat = 0;
}

#region PVE活动副本倒计时
public class PVECountdownData
{
	public int type;
	public long startTime;
	public long endTime;
	public int count;
	public int passCount;
	public int failCount;
	public int buyCount  ;		//已经购买次数
	public int needStone ;		//本次购买所需钻石数量
}

public class ExplorDoors
{
	public PVECountdownData skill;
	public PVECountdownData souls;
	public PVECountdownData exp;
	public PVECountdownData gems;
}
#endregion



public class loginInfo {
	//玩家的基本信息
	public PlayerInfo user;
	//宠物信息
	public MonsterInfo[] monster;
	//宠物编队
	public MonsterTeamInfo[] monteam;
	//背包的宝石
	public GemInfo[] gems;
	//装备信息
	public EquipInfo[] eqip;
	//背包里的物品
	public ItemInfo[] item;
	//建筑物信息
	public BuildingTeamInfo[] buliding;
	//装备信息
	public EquipTeam[] equip;
	//副本进度
	public DungeonsInfo floor;
	//奥义的信息
	public RTAoYi[] aoyi;
	//神龙的信息
	public DragonInfo[] dragon;
	//魂魄
	public SoulInfo[] chip;
    //活动状态
    public ActStateInfo actInfo;
    //消息 
    public AlertInfo alertInfo;
	//PVE活动副本倒计时
	public ExplorDoors explorDoors;
	//出战队伍信息
	public BattleTeamInfo[] teamSeqs;
	//每日副本信息
	public DoorDayStatus[] doorDayStatus;
	//系统信息
	public SysStatus sysStatus;
	//Vip信息
	public VipStatus vipInfo;
}

/// 
/// 服务器信息，目前使用：服务器决定是否开启支付
/// 
public class SysStatus {
	//是否开放充值0表示为开放，1表示开放
	public short opencharge;
	//是否开放激活码0标识未开放,  1表示开放
	public short openActvity;
	//是否开放漫画下载0标识未开放, 1表示开放
	public short openComic;
	//首充奖励		0未开放  1是开放
	public short openFirstCash;
	//下载奖励    	0 未开  1 开放
	public short openDownAward;
	//七天奖励    	0未开 1 开放 
	public short openSevenDay;
	//等级奖励    	0未开  1开放
	public short openLevelAward;
	//命运大转轮		0未开  1开放
	public short openFateAward;
	//签到奖励		0未开  1开放
	public short openSign;
	//vip特权礼包   	0未开  1 开放
	public short openVipAward;
	//武者的节日		0未开放  1开放
	public short openFeast;
	//魔王来袭		0未开放 	1开放
	public short openDevil;
	//聊天功能		0未开放	1开放
	public short openChat;
	//雷达组队		0未开放  	1开放
	public short openTeamPlay;
	//微信          	0未开发  1开放
	public short openWeixin;
	//十连抽开关      0未开发  1开放
	public short openRecruitRoles;
	//商店开关        0未开发  1开放
	public short oepnStore;
	//幸运转轮
	public short openLuckWheel;
	//猜猜看
	public short openGuess;
}

public class DoorDayStatus
{
	public int doorId;			//关卡ID
	public int passCount;		//关卡过关次数
	public int resetCount;		//关卡重置次数
}

public class BattleTeamInfo
{
	public int battleType;			//战斗类型
	public string teamSeq1;			//队伍1
	public string teamSeq2;			//队伍2
}

[Serializable]
public class LoginResponse : BaseResponse {
	public loginInfo data;

	public LoginResponse() { }

	//网络数据的封装，把数据调整的稍微好一点
	public override void handleResponse () {
		if(data != null && data.equip != null) {
			foreach(EquipTeam eqTeam in data.equip) {
				if(eqTeam != null) {
					eqTeam.HumanReadable();
				}
			}
		}
	}
}

#endregion


#region 登录 消息
public class AlertInfo{
    public  AnnouceResponseData notice ;
    //1 可以领  2是 入口在不能领  3.入口没有
    public int isSevenSigin;
    public DataList dailySgin;
    public SevenDaysListData sevenSgin;

    public AlertInfo(){
    
    }
}
#endregion

#region 更换队员
public class ChangeTeamMemberResponse : BaseResponse {
	public bool data;

	public ChangeTeamMemberResponse() { }
}

#endregion

#region 更换装备
public class ChangeEquipmentResponse : BaseResponse {
	public bool data;

	public ChangeEquipmentResponse() { }
}

#endregion

#region 战斗信息

/// <summary>
/// ppid:唯一标示  pid：实际得到的物品num  showpid：展示用的物品num   num：物品个数
/// lv：等级  ep：经验  at：宠物属性  at：攻击值  df：防御值 star：升星的星级 slotc：格子颜色
/// </summary>
public class ItemOfReward {
	public int ppid;
	public int pid;
	public int showpid;
	public int num;
	public short lv;
	public int ep;
	public short at;
	public int ak;
	public int df;
	public int star;


	// ----- 用来排序 ----
	public int mStar;

	public short[] slotc;

	public ConfigDataType getCurType()
	{
		int itemID = pid;
		if(showpid != 0)
		{
			itemID = showpid;
		}
		return DataCore.getDataType(itemID);
	}

	public Monster toMonster(MonsterManager monManager) {
		RuntimeMonster rtMon = new RuntimeMonster();
		rtMon.Attribute = (MonsterAttribute)at;
		rtMon.curExp = ep;
		rtMon.addStar = star;
		rtMon.curLevel = (int)lv;
		rtMon.ChaKeLa_Attck = ak / RuntimeMonster.ATTACK_FACTOR;
		rtMon.ChaKeLa_Defend = df / RuntimeMonster.DEFEND_FACTOR;

		Monster mon = new Monster(ppid, pid, rtMon, monManager.getMonsterByNum(pid));

		return mon;
	}

	public Equipment toEquipment(EquipManager epManager, GemsManager gemsManager) {

		EquipInfo eqInfo = new EquipInfo();
		eqInfo.id = ppid;
		eqInfo.exp = ep;
		eqInfo.lv = lv;
		eqInfo.num = pid;
		eqInfo.slc = slotc;
		
		eqInfo.slt =new int[]{0,0,0,0}; //new int[]{0,1,2,3};

		Equipment eq = new Equipment(eqInfo, epManager.getEquipConfig(pid), gemsManager);

		return eq;
	}

	public Item toItem(ItemManager itManager) {

		ItemInfo itIf = new ItemInfo();
		itIf.id = ppid;
		if (showpid != 0)
		{
			itIf.num = showpid;
		}
		else
		{
			itIf.num = pid;
		}

		itIf.count = num;

		Item item = new Item(itManager.getItemData(itIf.num), itIf);
		return item;
	}


	public Soul toSoul(SoulManager soulMgr)
	{
		SoulInfo info = new SoulInfo (ppid, pid, num);

		Soul soul = new Soul ();
		soul.m_config = soulMgr.GetSoulConfigByNum (info.num);
		soul.m_RTData = info;

		return soul;
	}


	public Gems  toGem(GemsManager gemMgr)
	{
		GemData gemInfo = gemMgr.getGemData(pid);
		Gems gem = new Gems(gemInfo);
		gem.id = ppid;
		return gem;
	}
}
	
#region 战斗返回数据   
//战斗返回数据
public class BattleSequence
{
	//战斗同步数据
	public BattleSyncData sync;
	//战斗序列
	public BattleData battleData;
	// 系统奖励
	public BattleReward reward;
    //combo奖励
    public ComboReward comboReward;
	// 掠夺奖励
	public FightBattleReward ext;
	//赌博结果
	public GambleItem gambleResult;
	//雷达组队神秘大奖
	public RadarReward radarReward; 
	//PVE活动副本倒计时
	public ExplorDoors explorDoors;
	//PVE副本已通过的次数
	public int passCount;
	//复仇下一次要花的钻石
	public int needStone;

	public GetDuoBaoLoginInfoDataPvp pvpStatus;
}
// combo奖励
public class ComboReward 
{
    public int combo   = 0;        //combo数
    public int award   = 0;        //奖励
}

public class RadarReward
{
	public ItemOfReward[] p;		//神秘大奖内容
	public int user_id;			//神秘大奖获得者
	public int zone_id;			//服务器ID
}

//战斗同步数据（当前值，直接 =）
public class BattleSyncData
{
	//当前精力
	public  int eny	= -1;
	//当前体力
	public int pwr = -1;
	//当前排名
	public int rank = -1;
	//用户当前经验
	public int ep;
	//用户当前等级
	public int lv;
	//用户当前vip等级
	public int vip;
	//钻石
	public int stone;
	//金币
	public int coin;
}

//战斗序列数据
public class BattleData
{
	// 己方攻击
	public int ak;
	// 对方防御值
	public int df;
	// 0 - false 1 - true
	public int isfirst = 0;
	//Base64 string
	public string rsmg;
	//Base64 string
	public string rsty;
	public int retCode;        //0 :lose  1 win
	//public string attData;   //攻击动画序列
	public int isPay;
	//判定输赢（1=代表赢，2代表输， 目前仅在世界boss有用）
	public int iswin;
    public int stone;

	public List<short> classType;
	//battle info
    public List<AW.Battle.CMsgHeader> info;

    public int videoId; //PVP战斗录像ID

    public int stars; // yangcg 战斗胜利副本星级
}


public class BattleReward {
	//bep:基础经验
	//bco:基础金币
	//eep:附加经验
	//eco:附加金币
	public int bep;
	public int bco;
	public int eep;
	public int eco;

	//战斗之后抽奖的另外两个
	public int[] np;
	//开箱子得到的物品
	public ItemOfReward [] p;
}

//掠夺所得数据
public class FightBattleReward
{
	// 掠夺获得的金币
	public int coin;
    // 掠夺获得的钻石
    public int stone;
	//战功
	public int zg;
	// 掠夺获得的物品
	public ItemOfReward[] p;


	// 对方防御值 -- 客户端自己冲BattleSequence里面的df赋值
	public int df;
	// 抢夺龙珠次数
	public int hittms;
}

#endregion


//boss的战斗
[Serializable]
public class BattleResponse : BaseResponse {

	public BattleSequence data;

	public BattleResponse() { }

	public override void handleResponse ()
	{
		if(data == null || data.battleData == null) return;

		//decompress the string and then json.deserize it
		if(data != null && !string.IsNullOrEmpty(data.battleData.rsty)) {
			string compressed = data.battleData.rsty;
			string json = DeflateEx.Decompress(compressed);

			data.battleData.classType = JSON.Instance.ToObject(json, typeof(List<short>)) as List<short>;
		}

		if(data != null && data.battleData.classType != null && data.battleData.rsmg != null) {
			string compressed = data.battleData.rsmg;
			if(!string.IsNullOrEmpty(compressed)) {
				string[] jsons = DeflateEx.Decompress(compressed).Split('@');
				if(jsons != null) {
					int length = jsons.Length;

					if(length == data.battleData.classType.Count) {
						data.battleData.info = new List<CMsgHeader>();

                        Type WarEnd = typeof(CMsgWarEnd);
						for(int i = 0; i < length; ++ i) {
							Type type = CMsgHeader.Table[data.battleData.classType[i]];
							data.battleData.info.Add(JSON.Instance.ToObject(jsons[i], type) as CMsgHeader);
                            //data.info.Add( JsonFx.Json.JsonReader.Deserialize(jsons[i], type) as War.CMsgHeader);

                            if(type == WarEnd) 
								data.battleData.retCode = ( ( (data.battleData.info[i] as CMsgWarEnd).winner ) == "att") ? 1 : 0;
							
						}

					}

				}
			}
		}
	}
}

#endregion

#region Sell Monster 

[Serializable]
public class SellMonsterResponse : BaseResponse
{
	public int data;
	
	public SellMonsterResponse () { }
}

#endregion


#region Decompose Monster 

[Serializable]
public class DecomposeMonsterResponse : BaseResponse
{
	public DecomposeData data;
	public DecomposeMonsterResponse () { }
}

public class DecomposeData
{
	public int coin;
	public ItemOfReward[] p;
}

#endregion

#region Evolve Monster 

[Serializable]
public class EvolveMonsterResponse : BaseResponse
{
	public DecomposeData data;
	public EvolveMonsterResponse () { }
}
	
#endregion

#region 魂魄合成
[Serializable]
public class SoulHeChenResponse : BaseResponse
{
	public ItemOfReward[] data;
	public SoulHeChenResponse() { }
}
#endregion

#region strengthen monster
//{"act":131,"status":1,"data":{"lv":"25","ep":368,"coin":1600}}
[Serializable]
public class StrengthenResponse: BaseResponse
{

	public StrenData data;
	public StrengthenResponse() {}
}

public class StrenData
{
	public int lv;
	public int ep;
	public int coin;
}
#endregion

#region strengthen equiment;
//"lv":"25","ep":368,"coin":1600}
[Serializable]
public class StrengthEquipResponse : BaseResponse
{
	public StrenData data;

	public StrengthEquipResponse() {}
}
#endregion

#region sell equipment
[Serializable]
public class SellEquipResponse : BaseResponse
{
	public int data;

	public SellEquipResponse() {}
}
#endregion

#region 战斗信息

//public class ItemsStruct
//{
//    public int ppid;
//    public int pid;
//    public int num;
//    public short lv;
//    public int ep;
//    public int ak;
//    public int df;
//}

public class ItemdataStruct
{
    public int ppid;
    public int pid;
    public int num;
    public short lv;
    public int ep;
    public int ak;
    public int df;

    public ItemdataStruct(){}

	public ConfigDataType getCurType() {
		return DataCore.getDataType(pid);
	}
}

public class BuyItemStruct
{
    public int stone;
    public int coin;
	public ItemOfReward[] p;
	public UserPropData Result;
	public int[] ndProp;


    public BuyItemStruct(){}
}

/// <summary>
///BuyItem
/// 唯一标识, "pid":物品编号,"num":数量,"lv":等级,"ep":经验,"ak":附加攻击,"df":附加防,"
/// </summary>
public class BuyItemResponse : BaseResponse {
    public BuyItemStruct data;
	public BuyItemResponse() { }
}
#endregion

#region
//{"act":107,"status":1,"data":{"p":[{"ppid":55,"pid":40117,"num":1,"lv":1,"ep":0,"ak":0,"df":0,"slotc":[3,3,1]}]}}
//{"act":107,"status":1,"data":{"eny":10}}
//{"act":107,"status":1,"data":{"coin":30000}}
//{"act":107,"status":1,"data":{"pwr":0}}
public class UsePropResponse : BaseResponse
{
	public UserPropData data;
	public UsePropResponse() { }
}
public class UserPropData
{
	public ItemOfReward[] p;
	public int eny;
	public int coin;
	public int pwr;
	public int stone;
}
#endregion

#region
public class SwapTeamResponse : BaseResponse
{
	public bool data;
	public SwapTeamResponse() {}
}
#endregion

#region
public class SwapMonsterPosResponse : BaseResponse
{
	public bool data;
	public SwapMonsterPosResponse() {}
}
#endregion

#region 活动奖励
public class BuyLotteryResponse:BaseResponse{

	public string dataStr;
	public ActivityReward data;
	public BuyLotteryResponse(){

	}

}


public class ActivityReward {

	public int needDia;
	public int needCoin;
	public int gift;

}
#endregion

#region 召唤神龙信息
[Serializable]
public class CallDragonInfo 
{
	public long st;
	public long du;
	public int lv;
	public int ep;
}

[Serializable]
public class CallDragonResponse : BaseResponse
{
	public CallDragonInfo data;
	public CallDragonResponse() {}
//	public override void handleResponse () {
//	}
}
#endregion

#region 学习奥义
[Serializable]
public class LearnAoYiResponse : BaseResponse
{
	public RTAoYi data;
	public LearnAoYiResponse() {}
}
#endregion

#region 装备奥义
[Serializable]
public class EquipAoYiResponse : BaseResponse
{
	public bool data;
	public EquipAoYiResponse() {}
}
#endregion

#region 购买奥义凹槽
//{"act":154,"status":1,"data":{'isSucce':是否成功,'stone':消费宝石,'coin':0}]
[Serializable]
public class BuyAoYiSlotInfo
{
	public int slot;
	public int stone;
	public int coin;
}
[Serializable]
public class BuyAoYiSlotResponse : BaseResponse
{
	public BuyAoYiSlotInfo data;
	public BuyAoYiSlotResponse() 
	{

	}
}
#endregion

#region 直接购买免战时间
//{"act":154,"status":1,"data":{'et':结束时间,'stone':消费宝石,'coin':0}]
[Serializable]
public class BuyMianZhanTimeInfo
{
	public long et;
	public int stone;
	public int coin;
}
[Serializable]
public class BuyMianZhanTimeResponse : BaseResponse
{
	public BuyMianZhanTimeInfo data;
	public BuyMianZhanTimeResponse() 
	{

	}
}
#endregion

#region 天下第一比武大会对手列表
[Serializable]
public class FightOpponentInfo
{
	// 用户id
	public int g;
	// 排名
	public int r;
	// 名字
	public string n;
	// 等级
	public int lv;

	// vip 等级
	public int vipLv;
	// 上场角色ID
	public SimpleMonster[] p;
	// 战功
	public int zg;

	public int izg;

	public int c;

	// 删除宿敌时候用得值
	public int eyid;

	// 抢夺龙珠次数
	public int htm;

	// 用户头像
	public int hi;
	
}

public class SimpleMonster
{
	public int num;				//宠物num
	public int star;			//宠物升级的星级
}

[Serializable]
public class TianXiaDiYiInfo 
{
	// 排名
	public int rank;

	public int userzg;

	public int yetcount;

	public int totalcount;

	public int recozg;
	// 对手列表
	public FightOpponentInfo[] roles;
}

[Serializable]
public class GetTianXiaDiYiOpponentsResponse : BaseResponse
{
	public TianXiaDiYiInfo data;
	public GetTianXiaDiYiOpponentsResponse() 
	{

	}
}
#endregion

#region 天下第一物品兑换
[Serializable]
public class ZhanGongBuyItemResponse : BaseResponse
{
	public ZhanGongBuyItemDataStruct data;
	public ZhanGongBuyItemResponse() 
	{

	}
}

public class ZhanGongBuyItemDataStruct
{
	public int zg;
	public int rank;
	public ItemOfReward[] p;
	public bool canBuy;
	public int[] ndProp;
}

//public class ZhanGongItemData
//{
//	public int ppid;
//	public int pid;
//	public int num;
//}

[Serializable]
public class GetZhanGongBuyItemIDResponse : BaseResponse
{
	public ZhanGongBuyItemData data;
	public GetZhanGongBuyItemIDResponse()
	{
	}
}

public class ZhanGongBuyItemData
{
	public int[] status;
	public ZhanGongItem[] item;
	public int glorynum;
	public int refreshMaxTimes;
	public int[] refreshMoney;
	public int surplusRefreshTimes;
}

public class ZhanGongItem
{
	public int id;
	public int num;
	public int pid;
	public int price;
	public int rank;
	public string des;
	public int discount;
	public bool canBuy;
	public int mType;
}
#endregion

#region 抢夺金币对手列表
[Serializable]
public class QiangDuoGoldOpponentsInfo
{
//	// 当天抢夺的次数
//	public int freetimes;
//
//	// 本次的抢夺次数(此数据如果为0则本次列表不被刷新)
//	public int qdCount;
//
//	public int jifen;
//
//	// 抢夺的金钱
//	public int robc;

	// 对手列表
	public FightOpponentInfo[] player;
	public GetDuoBaoLoginInfoDataRob status;
	public int yetcount;
}

[Serializable]
public class GetQiangDuoGoldOpponentsResponse : BaseResponse
{
	public QiangDuoGoldOpponentsInfo data;
	public GetQiangDuoGoldOpponentsResponse() 
	{

	}
}
#endregion

#region 抢夺金币兑换物品
[Serializable]
public class QiangDuoGoldBuyItemInfo
{
	public int coin;

	public int score;

	public ItemOfReward[] p;
}

public class QiangDuoGoldBuyItemResponse : BaseResponse
{
	public QiangDuoGoldBuyItemInfo data;
	public QiangDuoGoldBuyItemResponse()
	{}
}
#endregion

#region 抢夺金币兑换物品每天兑换次数
[Serializable]
public class GoldBuyItemBuyTotalInfo
{
	public int id;
	public int price;
	public int num;
	public int jifen;
	public int pid;
	public int discountjf;

	public GoldBuyItemBuyTotalInfo()
	{
	}

//	public GoldBuyItemBuyTotalInfo(int id, int used)
//	{
//		this.id = id;
//		this.used = used;
//	}
}

[Serializable]
public class GoldBuyItemBuyTotalResponse : BaseResponse
{
	public GoldBuyItemBuyTotalData data;
	public GoldBuyItemBuyTotalResponse()
	{
	}
}

public class GoldBuyItemBuyTotalData
{
	public GoldBuyItemBuyTotalInfo[] item;
}
#endregion

#region 取得宿敌列表
[Serializable]
public class GetSuDiResponse : BaseResponse
{
	public GetSuDiListInfo data;
	public GetSuDiResponse()
	{

	}
}
#endregion

#region 取得抢夺龙珠对手列表
[Serializable]
public class GetQiangDuoDragonBallOpponentsResponse : BaseResponse
{
	public FightOpponentInfo[] data;
	public GetQiangDuoDragonBallOpponentsResponse()
	{

	}
}
#endregion

#region 抢夺登录界面信息
[Serializable]
public class GetDuoBaoLoginInfoResponse : BaseResponse
{
	public GetDuoBaoLoginInfoData data;
	public GetDuoBaoLoginInfoResponse()
	{
		
    }
}

public class GetDuoBaoLoginInfoData
{
	public GetDuoBaoLoginInfoDataRank rankStatus;
	public GetDuoBaoLoginInfoDataRob robStatus;
	public GetDuoBaoLoginInfoDataBo defStatus;
	public GetDuoBaoLoginInfoDataSl atkStatus;
	public GetDuoBaoLoginInfoDataPvp pvpStatus;
}

public class GetDuoBaoLoginInfoDataPvp
{
	public GetDuoBaoLoginInfoDataPvpStatus ball;
	public GetDuoBaoLoginInfoDataPvpStatus rank;
	public GetDuoBaoLoginInfoDataPvpStatus robs;
	public GetDuoBaoLoginInfoDataPvpStatus revenge;
}

public class GetDuoBaoLoginInfoDataPvpStatus
{
	public int type;
	public long startTime;
	public long endTime;
	public int count;
	public int passCount;
	public int failCount;
	public int buyTime;
	public int needStone;
	public long coolTime;
}

public class GetDuoBaoLoginInfoDataRank
{
	public int rank;
	public int userzg;
	public int yetcount;
	public int totalcount;
	public int recozg;
}

public class GetDuoBaoLoginInfoDataRob
{
	public int robcoins;
	public int score;
}

public class GetDuoBaoLoginInfoDataBo
{
	public int historyBest;
	public int todayBest;
	public int totalCout;
	public int yetCount;
}

public class GetDuoBaoLoginInfoDataSl
{
	public int historyBest;
	public int todayBest;
	public int totalCout;
	public int yetCount;
}
#endregion

#region 删除宿敌
public class DeleteSuDiResponse : BaseResponse
{
	public bool data;
	public DeleteSuDiResponse()
	{

	}
}
#endregion

#region 添加宿敌
public class AddSuDiResponse : BaseResponse
{
	public int data;
	public AddSuDiResponse()
	{

	}
}
#endregion

#region 获得好友列表
[Serializable]
public class OtherUserInfo
{
	// 用户id
	public int g;
	// 排名
	public int r;
	// 名字
	public string n;
	// 等级
	public int lv;

	// vip等级
	public int vipLv;

	// 上场角色ID
	public SimpleMonster[] p;

	// 删除宿敌时候用的值
	public int eyid;

	public int hi;

	// 浇水次数
	public int wc;

}
#endregion

#region 宿敌列表
[Serializable]
public class GetSuDiListInfo
{
//	// 好友总数量
//	public int count;
//	// 当前页码
//	public int pageId;

    public FightOpponentInfo[] enemy;
}
#endregion


#region 获取添加好友请求列表
public class GetFriendRequestListResponse : BaseResponse
{
	public GetSuDiListInfo data;
	public GetFriendRequestListResponse()
	{

	}
}
#endregion

#region 同意或者拒绝添加好友
public class AgreeOrRefusedFriendResponse : BaseResponse
{
	public int data;
	public AgreeOrRefusedFriendResponse()
	{

	}
}
#endregion

#region 留言
public class SendMessageResponse : BaseResponse
{
	public bool data;
	public SendMessageResponse()
	{
	}
}
#endregion

#region 搜索
public class SearchUserResponse : BaseResponse
{
	public OtherUserInfo[] data;
	public SearchUserResponse()
	{
	}
}
#endregion

#region 添加好友
public class AddOrDeleteFriendResponse : BaseResponse
{
	public int data;
	public AddOrDeleteFriendResponse()
	{
	}
}
#endregion


#region 建筑操作应答
[Serializable]
public class BuildOperateResponse : BaseResponse
{
	public BuildingTeamInfo data;

	public BuildOperateResponse () {}
}

//public class BuildOperateData
//{
//	public BuildingTeamInfo bd;		//建筑信息
//}
#endregion

#region 招募
//coin 消耗金币  stone 消耗钻石 rwd 宠物信息 type 激活类型 0 1 2 3 4 5
[Serializable]
public class ZhaoMuResponse : BaseResponse
{
	public ZhaoMuData data;
	public ZhaoMuResponse ()
    {

    }
}
//twoStartTotal 总的次数 twoStart剩余次数 twoStartTime剩余时间 和 五星的一样
public class ZhaoMuData
{
	public ItemOfReward[] p;				//招募结果
	public int co;
	public int so;
	public ZhaoMuStateData status;			//招募状态
}
#endregion

#region 招募状态
[Serializable]
public class ZhaoMuStateResponse : BaseResponse
{
	public ZhaoMuStateData[] data;
  
	public ZhaoMuStateResponse ()
    {

    }
}

public class ZhaoMuStateData
{
	public int pron;				//ID
	public int totalcount;			//总次数
	public int freecount;			//剩余免费次数 
	public long coolTime;			//冷却时间  
	public int spGetCnt;			//特殊奖励剩余次数
}
#endregion


#region 合成
//array('ppid'=>$mr,'at'=>$findId,'delppid'=>$el);


public class HeChengResult{
    public int ppid;
    public int at;
    public int[] delppid;

}
[Serializable]
public class HeChengResponse : BaseResponse
{
    public HeChengResult data;


	public HeChengResponse() {}
}
#endregion


#region 宝石合成
[Serializable]
public class GemSyntheitcData
{
	public int succ;
	public int upId;
	public int delId;
	public int coin;
	public int armNum;
	public int nm;
	public int fpid;
	public int stone;
}
public class GemSyntheitcResponse : BaseResponse
{
	public GemSyntheitcData data;
	public GemSyntheitcResponse() {}
}
#endregion

#region 宝石兑换
public class GemExChangeData
{
	public int ppid;
	public int pid;
	public int fpid;
	public int nm;
}
//{"act":202,"status":1,"data":{"ppid":1121,"pid":120101,"fpid":1264,"nm":1}}
public class GemExChangeResponse : BaseResponse
{
	public GemExChangeData data;
	public GemExChangeResponse(){}
}
#endregion

public class GemInlayRemoveResponse : BaseResponse
{
	public bool data;
	public GemInlayRemoveResponse(){}
}

public class GemRecastData
{
	public int stone;
	public short[] slotc;
	public int eqid;
}
public class GemRecastResponse : BaseResponse
{
	public GemRecastData data;
	public GemRecastResponse(){}
}

#region
//{"act":172,"status":1,"data":{"star":最高星星数, "ak":攻击,"df":防御力,"sf":技能加成,"eak":敌方攻击,"edf":敌方防御力,"esf":敌方技能加成}}
[Serializable]
public class FinalTrialAddAttributeResponse : BaseResponse
{
    public FinalTrialAttribute data;
    
    public FinalTrialAddAttributeResponse() 
    {

    }
}

public class FinalTrialAttribute
{
    public int fst;
    public int star;
    public int ak;
    public int df;
    public int sf;
    public int eak;
    public int edf;
    public int esf;
}

//选择加成{"gid":玩家ID,"ty":1}
public class FinalTrialChooseAttributeResponse : BaseResponse
{
    public int cid;
    public int rank;
    public int ctms;
    
    public FinalTrialChooseAttributeResponse() 
    {
        
    }
}

//获取挑战列表:{"gid":玩家ID}
public class FinalTrialChallengeListResponse : BaseResponse
{
    public FinalTrialChallengeList data;
    
    public FinalTrialChallengeListResponse() { }
}

public class FinalTrialChallengeList
{
    public List<int[]> p;
    public int cid;
    public int topcid;
    public int hstcid;
   
}

//获取每3关加成数据{"gid":玩家ID}
public class FinalTrialDungeonAddResponse : BaseResponse
{
    public FinalTrialDungeonAdd[] data;
    public FinalTrialDungeonAddResponse(){}
}

public class FinalTrialDungeonAdd
{
    public int ty;
    public int pls;
    public int str;
    public FinalTrialDungeonAdd(){}
}

//每3关一加成{"gid":玩家ID, "ty":1}
public class FinalTrialAddResponse : BaseResponse
{
    public bool data;
}

//获取排行{"gid":玩家ID,"teamnm":5}
public class FinalTrialRankResponse : BaseResponse
{
    FinalTrialRank[] data;
    public FinalTrialRankResponse(){}
}

public class FinalTrialRank
{
    public int g;
    public int lv;
    public string n;
    public int cid;
    public int str;
    public int lday;
}

//查看阵容
public class CheckFinalTrialRankResponse : BaseResponse
{
    public CheckFinalTrialRank[] data;
    public CheckFinalTrialRankResponse(){}
}

public class CheckFinalTrialRank
{
	public int g;
	public int lv;
	public string n;
	public int cid;
	public int str;
	public int lday;
}
#endregion

#region 属性变换
[Serializable]
public class AttrSwapResponse : BaseResponse
{
	public AttrSwapData data;
	public AttrSwapResponse() {}
}
public class AttrSwapData
{
	public int atr;		//返回属性
	public int coin;	//消耗金币
	public int stone;	//消耗宝石
	public int prop;	//消耗道具
	public int propnm;	//消耗道具个数
}
#endregion

#region 签到奖励

public class SignItem{
	public int pid;
	public int num;
	public int coin;
	public int stone;
	public int rate;
	public int vip;
	public int day;
	public SignItem(){
		//RED.Log(pid + "num "+num);
	}
}

public class DataList{

	public DataGiftData[] days;
	/// <summary>
	/// 签到次数
	/// </summary>
	public int signtms;
	/// <summary>
	/// 补签次数
	/// </summary>
	public int pairtms;
	/// <summary>
	/// 最大补签次数
	/// </summary>
	public int pairmax;

	public DataList(){

	}
}


public class DataGiftData{
	/// <summary>
	/// 道具编号，数量
	/// </summary>
	public int[] p;
	/// <summary>
	/// 钻石金币 。。。。。'd':[coin,stone,vip,day,rate]
	/// </summary>
    public int[] d;

	public SignItem signItem = new SignItem();

	public DataGiftData(){

	}
}

[Serializable]
public class SignDateStateResponse : BaseResponse
{
    public DataList data;
    //   public int stone;
	public SignDateStateResponse() {

	}

	public override void handleResponse ()
	{
		base.handleResponse ();
		if (data != null) {
			for (int i = 0; i < data.days.Length; i++) {
				data.days[i].signItem.pid =data.days[i]. p [0];
				data.days[i].signItem.num = data.days[i].p [1];
				data.days[i].signItem.coin = data.days[i].d [0];
				data.days[i].signItem.vip = data.days[i].d [2];
				data.days[i].signItem.day = data.days[i].d [3];
				data.days[i].signItem.rate = data.days[i].d [4];
			}
		}
	}
}



public class SignDayData{
	public ItemOfReward[] p;
    public int stone;
}


public class SignDayResponse:BaseResponse{
	public SignDayData data;
	
	public SignDayResponse(){
	
	}

}
#endregion



#region  公告
public class AnnouceResponse : BaseResponse
{
    public AnnouceResponseData data;
}

public class AnnouceResponseData
{
    public string[] Title;
    public string[] Content;
}

#endregion

#region 吃拉面
[Serializable]
public class HaveDinnerResponse : BaseResponse
{
    public DinnerData data;
	public HaveDinnerResponse() {}
}

public class DinnerData{
    public int eny;
    public bool stat;
    public int dinnertime;
}



//{"status":1,"act":800,"data":{"dinner":{"dur":[18,20],"isopen":true,"vigor":"100"},"stat":true}}
public class HaveDinnerStateDate{
    public DinnerType dinner;
    public bool stat;
}
public class DinnerType{
	public int[] dur;
	public bool isopen;
	public string vigor;
}

public class HaveDinnerStateResponse : BaseResponse
{
	public HaveDinnerStateDate data;
	public HaveDinnerStateResponse() {}
}
	
#endregion


#region 淘宝
[Serializable]	
public class TaobaoResponse:BaseResponse{
	//public int status;
	public TaobaoBackData data;
	//	public int errorCode;
	public TaobaoResponse(){
	
	}

}

public class TaobaoBackData{
	/// <summary>
	/// s:增加的宝石数
	/// </summary>
	public string s;
	///c:增加的游戏币数量
	public string c;
}
#endregion

#region 潜力训练
[Serializable]
public class QianLiXunLianResponse : BaseResponse
{
	public QianLiXunLianData data;
	public QianLiXunLianResponse() {}
}
public class QianLiXunLianData
{
	public int coin;	//消费金币
	public int stone;	//消费宝石
	public int prop;    //消费道具的唯一标识
	public int propnm;	//消费的数量
	public int ak;		//增加攻击点
	public int df;		//增加防御点
}
#endregion

#region 潜力重置
[Serializable]
public class QianLiResetResponse : BaseResponse
{
	public QianLiResetData data;
	public QianLiResetResponse() {}
}
public class QianLiResetData
{
	public int coin;	//消费金币
	public int stone;	//消费宝石
}
#endregion

#region 消息
public class MessageInformationResponse : BaseResponse
{
    public FightMegData data;
	public MessageInformationResponse()
	{

	}
}

public class MessageGiftData
{
    public List<int[]> p;
    public int coin;
    public int stone;
}

public class FightMegData
{
	public FightMegCellData[] msg;
}

public class FightMegCellData
{
    public int id;        //战报ID
	public int cgid;    //挑战者ID
	public string cName;		//挑战者名字
	public int cLevel;             //挑战者等级
	public int islost;               //是否失败
	public int lostStone;        //如果大于> 0说明丢失钻石 
	public int type;                 //类型 1：抢夺龙珠，2：排行榜，3抢夺金币
	//如果是抢夺龙珠，则这里为失去的龙珠; 
	//如果是抢夺金币，这里是失去的金币数量;
	//如果是排行榜，	这里是失去的战功;
	public int lost;                 
	public int rank;             //如果是排行榜，这里是下降的排行
	public int videoId;         //战斗录像Id
	public int ctm;               //时间
	public int status;
	                
}
#endregion

#region 邮件返回
public class MegMailResponse:BaseResponse
{
	public MegMailData data;
}
public class MegMailData
{
	public MegMailCellData[] msg;
}
public class MegMailCellData
{
	public int id;
	public int cgid;                                      //发送者Id
	public string cName;                            //发送者名字
	public int cLevel;                                  //发送者等级
	public int type;                                      // 1:系统消息，2:玩家留言
	public int status;                                   //0:未读，1已读，2附件已提取
	public string content;                            //邮件内容
	public EmailAward[] attachment;         //附件('pid'=>'count');
	public int ctm;                                       //时间
}

public class EmailAward
{
	public int pid;
	public int count;
}
#endregion





#region 改变邮件状态
public class ChangeMailStateResponse:BaseResponse
{
	public bool data;
}
#endregion






#region
public class FinalTrialBattleResultResponse : BaseResponse
{
	public FinalTrialBattleResultData data;

	public override void handleResponse ()
	{
		//decompress the string and then json.deserize it
		if(data != null && !string.IsNullOrEmpty(data.rsty)) {
			string compressed = data.rsty;
			string json = DeflateEx.Decompress(compressed);
			
			data.classType = JSON.Instance.ToObject(json, typeof(List<short>)) as List<short>;
		}
		
		if(data != null && data.classType != null && data.rsmg != null) {
			string compressed = data.rsmg;
			if(!string.IsNullOrEmpty(compressed)) {
				string[] jsons = DeflateEx.Decompress(compressed).Split('@');
				if(jsons != null) {
					int length = jsons.Length;
					
					if(length == data.classType.Count) {
                        data.info = new List<CMsgHeader>();
						
						for(int i = 0; i < length; ++ i) {
                            Type type = CMsgHeader.Table[data.classType[i]];
                            data.info.Add(JSON.Instance.ToObject(jsons[i], type) as CMsgHeader);
							//data.info.Add( JsonFx.Json.JsonReader.Deserialize(jsons[i], type) as War.CMsgHeader);
						}
						
					}
					
				}
			}
		}
		
	}
}

public class FinalTrialBattleResultData
{
	//Base64 string
	public string rsmg;
	//Base64 string
	public string rsty;

	public List<short> classType;
	//battle info
    public List<CMsgHeader> info;
}
#endregion

#region 神秘商店

public class SecretShopResponse : BaseResponse
{
	public SecretShopDataList data;
}

public class SecretShopDataList
{
	public SecretShopDataStruct[] Items;
	public long[] Flush;
	public int[] souls;
	public int refreshMaxTimes; //总共次数
	public int surplusRefreshTimes; //剩余次数
	public int[] refreshMoney;   // 0 stone 1 coin
	public int purchaseSoulMoney ;  //抽魂魄价格
	public int jifen;
}

public class SecretShopDataStruct
{
	public int id;
	public int num;
	public int count;
	public int[] money;
    public int type;    ////0 no ,1 vip, 2 lv
	public int limit;
	public int buyTime;
	public bool canBuy;
}


#endregion

#region 神秘商店购买

public class SecretShopBuyResponse : BaseResponse
{
	public SecretShopBuyResponseInfo data;
}

public class SecretShopBuyResponseInfo
{
	public int stone;
	public int coin;
	public ItemOfReward[] p;
	public int[] ndProp;
}

//public class SecretShopBuyResponseData
//{
//	public int ppid;
//	public int pid;
//	public int num;
//	public short lv;
//	public int ep;
//	public int[] slotc;
//
//	public SecretShopBuyResponseData(){}
//
//	public ConfigDataType getCurType() {
//		return DataCore.getDataType(pid);
//    }
//}

//public class SecretShopItemdataStruct
//{
//	public int ppid;
//	public int pid;
//	public int num;
//	public short lv;
//	public int ep;
//	public int ak;
//	public int df;
//	public int[] slotc;
//
//	public SecretShopItemdataStruct(){}
//	
//	public ConfigDataType getCurType() {
//		return DataCore.getDataType(pid);
//    }
//}

#endregion

#region 等级奖励 
/// <summary>
/// 获取等级奖励 的状态
/// </summary>
public class LevelRewardStateResponse:BaseResponse{
    public int[] data;
	public LevelRewardStateResponse(){
				
	} 
} 

/// <summary>
/// 获取等级奖励
/// </summary>
public class GetLevelRewardResponse:BaseResponse{
		public ItemOfReward[] data;
	public GetLevelRewardResponse(){
	
	}
}

#endregion



#region 随机产生名字
[Serializable]
public class RandomNameResponse : BaseResponse
{
	public string data;
}
#endregion

#region 更改用户信息
[Serializable]
public class ChangeUserInfoResponse : BaseResponse
{
	public ChangeUserInfoData data;
}

public class ChangeUserInfoData
{
	public int stone;
}

#endregion

#region 究极试炼登录信息
[Serializable]
public class FinalTrialShaLuBuOuAllResponse : BaseResponse
{
	public FinalTrialShaLuBuOuAllData data;
}

public class FinalTrialShaLuBuOuAllData
{
	public int topcid1;
	public int topcid2;
	public int cid1;
	public int cid2;
	public int lftms;
}

#endregion

#region 排行内部信息
[Serializable]
public class FinalTrialRankCheckInfoResponse : BaseResponse
{
	public FinalTrialRankCheckInfoData[] data;
}

public class FinalTrialRankCheckInfoData
{
	public MonsterInfo p;
	public int[] eq;
	public int lv;
}
#endregion


#region 活动状态 
[Serializable]
public class ActStateInfo{

    public string festival;
    public string monster;
    public string taobao;
	public DinnerInfo[] food; 

	public int masgn;
    public string gonggao;
    public int lvReward;
    public string SecretShop;
    public int Vip;
    public int sevenAward;

	public ActStateInfo (){
	}
	
}


public class DinnerInfo{

	public int start;
	public int end;

    /// <summary>
    ///  1 是上午  2 是下午
    /// </summary>
	public int type;
    /// <summary>
    /// 1 是 未吃过  2是 吃过
    /// </summary>
	public int eaten;
    //  DinnerTimeT time;
    public DinnerInfo(){
    
    }

}
   
public class DinnerTimeT{
    long[] am;
    long[] pm;
}


#endregion

#region 战斗回放 
public class BattleVideoPlaybackResponse : BaseResponse
{
	public BattleVideoPlaybackData[] data;
}

public class BattleVideoPlaybackData
{
	public string creattime;
	public string winid;
	public string id;
	public string atid;
	public string dfid;
	public BattleVideoContent content;
	public BattleVideoData battledata;
}

public class BattleVideoContent
{
	public string msgArr;
	public string typeArr;
}

public class BattleVideoData
{
	public BattleVideoTeamData attTeam;
	public BattleVideoTeamData defTeam;
	public RTAoYi[] attAoYi;
	public RTAoYi[] defAoYi;
}

public class BattleVideoTeamData
{
	 public int at;
     public int df;
	 public string name;
     public int headId;
	 public int level;
	 public int rank;
	 public int roleId;
	 public BattleVideoSelfTeamData[] team;
	 public int vip;
}

public class BattleVideoSelfTeamData
{
	public int lvl;
	public int property;
	public int petNum;
	public int petId;
	public int sf;
//	public string ur_level;
	public int[] skill;

}


#endregion

#region 战斗回放 
public class BattleVideoPlaybackSingleResponse : BaseResponse
{
	public BattleVideoPlaybackData data;
}
#endregion


#region 7天登录列表 
public class SevenDaysListResponse : BaseResponse
{
	public SevenDaysListData data;
}

public class SevenDaysListData
{
	public int index;
	public bool canGain;
	public SevenDaysAwardList[] awads;
}

public class SevenDaysAwardList
{
	public int day;
	public List<int[]> reward;
}

#endregion

#region 7天登录购买请求
public class SevenDaysBuyResponse : BaseResponse
{
	public SevenDaysBuyData data;
}

public class SevenDaysBuyData
{
	public int eep;
	public int eco;
	public int[] np;
	public ItemOfReward[] p;
}

public class SevenDaysBuyRewardData
{
	public int ppid;
	public int pid;
	public int num;
	public int at;
	public int lv;
	public int ep;
	public int ak;
	public int df;
}

#endregion


#region 资源配表
public class ConfigResponse : BaseResponse
{
	public bool result;
	public string message;
	public string url;
	public string md5;
	public int size;
}
#endregion

#region 开宝箱返回
public class GetTresureResponse :BaseResponse{

    public NormalReward data;

}

public class NormalReward{
    public int eep;
    public int eco;
    public int[] np;

    public ItemOfReward[] p;

    public NormalReward (){

    }
}

public class GetTreasureStateResponse:BaseResponse{
    public TreasureState data;
}

public class TreasureState{
    public int[] boxstatus;
}

#endregion


#region 获取VIP商城信息
public class GetVipShopInfoResponse :BaseResponse
{
	public VipShopInfoData data;

}

public class VipShopInfoData
{
	public VipShopInfo[] vipstatus;
}

public class VipShopInfo
{
	public int vipLvl;
	public int propId;
	public int buyTime;
}

#endregion

#region 获取VIP Gift
public class GetVipGiftResponse :BaseResponse
{
	public VipGiftDataInfo data;
	
}

public class VipGiftDataInfo
{
	public ItemOfReward[] award;
}

//public class VipGiftInfo
//{
//	public int ppid;
//	public int pid;
//	public int buyTime;
//}

#endregion

#region 究极试炼第一界面进入
public class NewFinalTrialEnterResponse : BaseResponse
{
	public NewFinalTrialEnterData data;
}

public class NewFinalTrialEnterData
{
	public NewFinalTrialEnterInfo ulstatus;
}

public class NewFinalTrialEnterInfo
{
	public int todayRank;		//今天的排行
	public int todayTime;		//今天已经挑战的此时
	public int todayMax;		//今天的最大的星数
	public int totalCount;	//今天可以挑战的此时
	public int isFirst;		//是不是第一次， 0 不是第一次， 1，是第一次，如果是第一次，需要执行选择buffer操作
	public int currLayer;
}
#endregion

#region 究极试炼Add buffer
public class NewFinalTrialAddBufferResponse : BaseResponse
{
	public NewFinalTrialAddBufferData data;
}

public class NewFinalTrialAddBufferData
{
	public NewFinalTrialAddBufferInfo ulbuffer;
}

public class NewFinalTrialAddBufferInfo
{
	public int stars;			//今天获得的星数
	public NewFinalTrialBuffer[] bufferList;
	public int totalScore;
	public int costScore;
}

public class NewFinalTrialBuffer
{
	public string name;
	public int buffer;
	public int needStar;
}
#endregion

#region 究极试炼地图
public class NewFinalTrialMapResponse : BaseResponse
{
	public NewFinalTrialMapData data;
}

public class NewFinalTrialMapData
{
	public NewFinalTrialMapInfoData ulbattle;
}

public class NewFinalTrialMapInfoData
{
	public int ak;						//累计攻击加成
	public int edf;
	public int eak;
	public int df;
	public int sf;
	public int esf;
	public int totalScore;				//今天总得分
	public int costScore;					//今天总计花费的星数
	public int maxLayer	;					//今天最大关卡层数
	public int currLayer;					//当前层数
	public int yetTime;					//已经执行的次数
	public int historyBest;
	public int currCards;
	public NewFinalTrialAddBufferInfo buffer;
	public NewFinalTrialTeamInfo[] currEnemy0;   //hard
	public NewFinalTrialTeamInfo[] currEnemy1;   //normal
	public NewFinalTrialTeamInfo[] currEnemy2;   //easy
}

public class NewFinalTrialTeamInfo
{
	public int ArmsLv;
	public int EnemyLv;
	public int EnemyArms;
	public int EnemyArmor;
	public int ArmorLv;
	public int EnemyPet;

}
#endregion

#region 究极试炼战斗
public class NewFinalTrialFightResponse : BaseResponse
{
	public NewFinalTrialFightData data;

	public override void handleResponse()
	{
		//decompress the string and then json.deserize it
		if(data != null  && !string.IsNullOrEmpty(data.battleData.rsty)) {
			string compressed = data.battleData.rsty;
			string json = DeflateEx.Decompress(compressed);
			
			data.battleData.classType = JSON.Instance.ToObject(json, typeof(List<short>)) as List<short>;
		}
		
		if(data != null && data.battleData.classType != null && data.battleData.rsmg != null) {
			string compressed = data.battleData.rsmg;
			if(!string.IsNullOrEmpty(compressed)) {
				string[] jsons = DeflateEx.Decompress(compressed).Split('@');
				if(jsons != null) {
					int length = jsons.Length;
					
					if(length == data.battleData.classType.Count) {
						data.battleData.info = new List<CMsgHeader>();
						
						Type WarEnd = typeof(CMsgWarEnd);
						for(int i = 0; i < length; ++ i) {
							Type type = CMsgHeader.Table[data.battleData.classType[i]];
							data.battleData.info.Add(JSON.Instance.ToObject(jsons[i], type) as CMsgHeader);
							
							if(type == WarEnd) 
								data.battleData.retCode = ( ( (data.battleData.info[i] as CMsgWarEnd).winner ) == "att") ? 1 : 0;
							
						}
						
					}
					
				}
			}
		}
	}
}

public class NewFinalTrialFightData : BattleSequence
{
	public NewFinalTrialFightInfoData rushResult;
}

public class NewFinalTrialFightInfoData
{
	public NewFinalTrialAddBufferInfo buffer;
	public ItemOfReward[] award;
	public NewFinalTrialFightMapInfo alertInfo;
	public int pass;
	public int gainStars;
}

public class NewFinalTrialFightMapInfo
{
	public int award;
	public int buffer;
	public bool	 canAward;
	public bool canBuffer;
}
#endregion

#region 获取究极试炼排行
//获取排行{"gid":玩家ID,"teamnm":5}
public class GetNewFinalTrialRankResponse : BaseResponse
{
	public GetNewFinalTrialRankData data;
}

public class GetNewFinalTrialRankData
{
	public GetNewFinalTrialRank[] ranklist;
}

public class GetNewFinalTrialRank
{
	public int g;
	public int lv;
	public string n;
	public int cid;
	public int str;
	public int lday;
	public int layer;
}
#endregion

#region 获取究极试炼详细信息
public class GetNewFinalTrialRankCheckInfoResponse : BaseResponse
{
	public GetNewFinalTrialRankCheckInfoData data;
}

public class GetNewFinalTrialRankCheckInfoData
{
	public loginInfo detail;
	public int currTeam;
	public int lv;
}

//public class GetNewFinalTrialRankCheckInfo
//{
//	public MonsterInfo p;
//	public int[] eq;
//	public int lv;
//}
#endregion

#region 下载获取
public class GetDownloadReceoveResponse : BaseResponse
{
    public DownloadReceoveP data;
}

public class DownloadReceoveP {
    public ItemOfReward[] p;
}

#endregion

#region 是否下载
public class GetDownloadFinishResponse : BaseResponse
{
    public DownloadFinishDate data;
}
public class DownloadFinishDate {
    public int status;
}

#endregion

#region 下载查看
public class GetDownloadCheckResponse : BaseResponse
{
    public DownloadCheckDate data;
}

public class DownloadCheckDate{
    public List<int[]> award;
}
    
#endregion

#region 连击
public class BattleComboResponse : BaseResponse
{
	public int id;
}
#endregion

#region 主界面的Notice

public class MovableNoticeResponse : BaseResponse {
	public NoticeDataConfig[] data;
}

#endregion


#region 服务器返回玩家最高的连击次数
public class ComboData
{
	public int max;
	public int total;
}
public class GambleData
{
	public int win;
	public int lose;
}

public class ComboAndGamBle
{
	public ComboData combo;
	public GambleData gamble;
}
public class ComboResponse : BaseResponse
{
	public ComboAndGamBle data;
}
#endregion


#region 赌博


public class GambleItem {
	public int id;
	public int type; 			//1，1对多人数，2，技能概率
	public int cost;			//花费金额
	public int win;				//赢得金额
	public int condition;		//条件
	public bool winFlag;	    //是否赢得赌局

    public ShowData show;			//展示的列表
}

public class ShowData{
    public int[] skillarr;
    public int[] killarry;
    public MiniMonsterData role;
}

public class MiniMonsterData{
    public int num;
    public int attri;
    public int lv;
}
////技能结构
//public class SkillAry{
//	public int[] skillList;
//}
//
////连击结构
//public class KillComboData{
//	public int[] comboData;
//    public int data;
//}


//请求获取赌博状态
public class GetGambleStateResponse:BaseResponse{

    //  public GambleItem[] data;
    public GetGambleStateData data;
}
public class GetGambleStateData{
    public GambleItem[] list;
    public GambleLeftTimes status;
}
public class GambleLeftTimes{
    public int total;
    public int yet;
}

//获取赌博结果 请求
//public class GetGambleResultResponse:BaseResponse{
//	public	GetGambleResultData data;
//}
//public class GetGambleResultData{
//	public GambleItem gambleResult;
//}

public class GetMailAttachmentResponse:BaseResponse
{
	 //ItemOfReward
	public GetMailAttachmentData data;
}

public class GetMailAttachmentData
{
	public ItemOfReward[] p;
}
#endregion

#region
public class GetChallengeRankResponse : BaseResponse
{
	public GetChallengeRankData data;
}

public class GetChallengeRankData
{
	public GetChallengeRank[] list;
}

public class GetChallengeRank
{
	public int gid;
	public string name;
	public int level;
	public int rank;
	public int robcoins;
	public int scores;
	public int vip;
	public FirstRoleNum firstRole;
}

public class FirstRoleNum
{
	public int roleLv; //卡牌等级
	public int roleNum; //卡牌策划编号
	public int roleAttr; //卡牌属性
	public int roleStar; //卡牌星级		(全星级，基础星级 + 附件星级)
	public int roleLots; //卡牌缘是否配齐	(0,缘没有配齐，1，缘已经配齐)
}
#endregion
public class GetCallDragonIsFinishedResponse:BaseResponse{
    public bool data;

    public GetCallDragonIsFinishedResponse(){
    }
}
#region 任务列表返回
public class TaskListResponse:BaseResponse{
    public TaskListData data;
    public TaskListResponse()
	{
		
    }
}
public class TaskListData
{
	public TaskDataResponse[] tasks;
}
public class TaskDataResponse
{
	public int id;
	public int type;
	public int condition;
	public int minLevel;
	public int maxLevel;
	public int progress;
	public int isGain;
}
#endregion

#region 任务奖励返回
public class TaskRewardResponse:BaseResponse{
    public TaskRewardData data;
    public TaskRewardResponse()
	{
		
    }
}
public class TaskRewardData
{
	public ItemOfReward[] award;
	public TaskDataResponse nextTask;
	
}
#endregion

#region
public class GuaGuaStatusResponse : BaseResponse
{
	public GuaGuaStatusData data;
}

public class GuaGuaStatusData
{
	public GuaGuaStatusDataDetail status;
}

public class GuaGuaStatusDataDetail
{
	public int count;
	public int needStone;
}
#endregion

#region
public class GuaGuaLeResponse : BaseResponse
{
	public GuaGuaLeResponseData data;
}

public class GuaGuaLeResponseData
{
	public int stone;
	public int nextStone;
	public ItemOfReward[] p;
}


#endregion

#region 天神的赐福
public class GotGodGiftListResponse:BaseResponse{
    public GotGodGiftListData data;
}
public class GotGodGiftListData{
    public List<int[]> awardList;
    public int needStone ;
}
public class GotGodGiftResponse:BaseResponse{
    public GodGiftData data;
}
public class GodGiftData{
    public ItemOfReward[] award;
    public int stone;
    public GotGodGiftListData nextAward;
}


#endregion

#region 大转盘列表

public class BigWheelListResponse:BaseResponse
{
	public BigWheelListData data;	
}

public class BigWheelListData
{
	public List<int[]> awardList;
	public int needStone;
	public int totalCount;
	public int flushCount;
}
#endregion

#region 使用大转盘
public class UseBigWheelResponse:BaseResponse
{
	public UseBigWheelData data;	
}

public class UseBigWheelData
{
	public int count;
	public int index;
	public ItemOfReward[] p;
	public int stone;
	public int nStone;
}

#endregion

#region 奖励活动大转盘

public class GetAwardActivity:BaseResponse
{
	//活动编号，1:幸运转盘，2：摇摇乐，3：刮刮乐，4，组队副本
   //可以参见的次数
   //是否已经结束，true,已经结束，false,为结束
    public AwardStutas data ;
    public GetAwardActivity()
    {

    }
   
}
public class AwardStutas
{
    public DataAward status ;
    public AwardStutas()
    {

    }
}

public class DataAward
{
    public int  id ; 
    public int count ; 
    public bool end ; 
	public int visitCount;
}
#endregion

#region  下订单返回
public class PayResponse : BaseResponse
{
	public PayData data;
	public PayResponse() {}
}

public class PayData
{
	public string orderId;			//订单号
	public string payCallback;		//第三方支付回调地址
}
#endregion

#region 查询订单状态返回
public class PayStatusResponse : BaseResponse
{
	public PayStatusData data;
	public PayStatusResponse () { }
}

public class PayStatusData
{
	public int billStatus;   //(0: 失败    1： 成功)  2 成功，但订单以过期
	public VipStatus vipStatus;
	public ItemOfReward[] p;
}

//VIP状态
public class VipStatus {
	public int userVip;		//玩家VIP等级
	public int totalCach;   //玩家累计充值额(分)
	public int toNextLevle;	//玩家到达下一级别VIP所需还需要充值额(分)
	public int curStone;    //玩家当前钻石数量
}
#endregion

#region 查询所购物品状态
public class PayCountResponse : BaseResponse
{
	public PayCountData data;
	public PayCountResponse () { }
}

public class PayCountData
{
	public List<int[]> buyCounts;
}
#endregion

#region 向服务器 同步状态
public class SendResponse:BaseResponse{

}
public class SendDeviceInfoResponse:BaseResponse{

}
#endregion

//扫荡返回结果
public class SaoDangResponse:BaseResponse
{
	public SaoDangResult data;
	public SaoDangResponse(){}
}

public class SaoDangResult
{
	public BattleSequence[] sweepList;
	public int passCount;					//已经通过的次数
}


#region 	获取月卡状态
public class GetMonthGiftStateResponse:BaseResponse{
    public MonthGiftData data;
    public GetMonthGiftStateResponse(){}
}
public class MonthGiftData{
    public int canGain ;
    public int lastDay ;
}

#endregion
#region 	首充奖励
public class GetFirstChargeStateResponse:BaseResponse{
	public FirstChargeState data;
	public GetFirstChargeStateResponse(){}
}
public class FirstChargeState{
	//0 未充值   1已充值 未领取   2已充已领
	public int canGainFirstAward;
	public List<int[]> awards;
}
	
public class GetFirstChargeGiftResponse:BaseResponse{
	public FirstGiftData data;
	public GetFirstChargeGiftResponse(){}
}

public class FirstGiftData{
	public ItemOfReward[] award;
}

#endregion

#region 同步召唤神龙 数据
public class SyncCallDragonTimeResponse:BaseResponse{
	public SyncCallBallData data;
	public SyncCallDragonTimeResponse(){

	}
}

public class SyncCallBallData{
	public DragonBallItemData[] balls;
	public DragonInfo[] dragons;
	public SyncCallBallData(){
		
	}
}

public class DragonBallItemData{
	public int c_id ;
	public int c_pid ;
	public int c_num ;
}
#endregion

#region 新战斗的定义

/// <summary>
/// 
/// </summary>
public class SkillObj {
	public int skillId;
	public int skillLevel;

	public SkillObj() { }
}

/// <summary>
/// 下面的所有都是进入副本的请求
/// </summary>
public class Battle_Monster {
    //宠物Id
    public int petId;
    public int petNum;
    //宠物属性
    public short property;
    //宠物等级
    public int lvl;
    //宠物的经过各种计算的最终攻击力
    public int finalAtt;
    //缘分配齐
    public bool fullfated;
	//每次缘分配齐一次的时候，加一些怒气
	public int nuqi;
	//释放是boss战斗
	public short isBoss;
    //技能ID列表
	public SkillObj[] skill;
    public float skillUp;
}

public class Battle_Data {
    public string roleId;
    public Battle_Monster[] team;
}

public class Battle_AoYi {
    //ID
    public int num;
    //等级
    public int lv;
}

public class ClientData {
	//左边是攻击方 1，否则为 0
	public int isatk;
	public Battle_Data attTeam;
	public Battle_AoYi[] attAoYi;

	public Battle_Data defTeam;
	public Battle_AoYi[] defAoYi;
}

/// <summary>
/// 掉落物品
/// </summary>
public class DropItem {
	//实际上是配表里面的number
	public int pid;
	//数量
	public int num;
}

[Serializable]
public class MonsterAndDrop {
	public ClientData warInfo;
	public DropItem[] preAward;
}

public class ClientBattleResponse : BaseResponse {
	public MonsterAndDrop data;
	public ClientBattleResponse(){ }

	public override void handleResponse () {
		if(data != null) {
			//计算生成战斗序列
		}
	}
}



/// <summary>
/// 沙鲁布欧的战斗结算
/// </summary>

public class ClientShaLu {
	public ItemOfReward[] award;
	public int coin;
	public int store;
	public int comboCoin;
}

public class ClientBTShaBuResponse : BaseResponse {
	public ClientShaLu data;

	public ClientBTShaBuResponse() { }
}

#endregion


#region 技能升级
public class SkillUpgradeResponse : BaseResponse
{
	public SkillUpgradeData data;
	public SkillUpgradeResponse () { }
}

public class SkillUpgradeData
{
	public int level;					//技能等级
	public int scroll;					//技能卷轴
	public int coin;					//消耗金币
}
#endregion

#region 查看玩家远征状态
public class GetFinalTrialStateResponse : BaseResponse
{
	public GetFinalTrialStateInfo data;
}

public class GetFinalTrialStateInfo
{
	public GetFinalTrialStateData shalu;
	public GetFinalTrialStateData buou;
}

public class GetFinalTrialStateData
{
	public int gid;
	public int type;
	public int count;//允许的最大此时
	public int fCount;//已经失败的次数
	public int cLayer;//当前层级
	public int needStone;
	public int costStone;
}
#endregion

#region 查看当前层状态
public class GetFinalTrialCurDungeonResponse : BaseResponse
{
	public GetFinalTrialCurDungeon data;
}

public class GetFinalTrialCurDungeon
{
	public int basic;
	public string defGid; //守关人ID
	public string defName; //守关人名字
	public int type;  //关卡类型	1：沙鲁，2：布欧
	public int team;  //守关人队伍类型 12人阵
	public ItemOfReward[] members;   //守关人卡牌数组 
}

#endregion
#region 刷新神秘商店
public class RefreshSecretShopResponse:BaseResponse{
    //  public
    public SecretShopDataList data;
}
#endregion

#region 精力

public class getJLData: BaseResponse {
    public DataJL data;

    public getJLData() { }
}

public class DataJL {
    public  int   stone ;     //  消耗的宝石
    public  int   eny;         // 恢复的精力
    public  int   buycount ;       // 已经购买次数
}

#endregion

#region 同步副本
public class SyncPveResponse : BaseResponse
{
	//PVE活动副本倒计时
	public ExplorDoors data;
}
#endregion
#region 限时活动
public class HolidayActivityResponse : BaseResponse {
    public HolidayData data;
}
    
public class HolidayData {
    public string mainName; // 活动标题
    public HolidayActivityItem[] activityList; // 活动个数
}

public class HolidayActivityItem
{
    public string id; 
    public int type; 
    public string iconId; 
    public string name; 
    public long startTime; 
    public long endTime; 
    public string description; 
}
#endregion

#region 重置挑战关卡次数
public class ResetFloorResponse : BaseResponse
{
	public ResetFloorData data;
}

public class ResetFloorData
{
	public int resetCount;		
	public int stone;
}
#endregion

#region 同步钱
public class SyncMoneyResponse : BaseResponse
{
	public SyncMoneyData data;
}

public class SyncMoneyData
{
	public int coin;		
	public int s;
}
#endregion

#region 复仇

public class RevengeResponseData
{
	public int maxCount;	//允许最大复仇次数
	public int yetCount;	//已经复仇次数
	public int needStone;
}
class RevengeResponse : BaseResponse
{
	public RevengeResponseData data;
	/*扩展字段
	public int ballCount =	0;	//已经复仇龙珠抢夺此时
	public int rankCount =	0;	//已经复仇排行榜次数
	public int robCount  =	0;	//已经复仇抢夺金币次数
*/
}
#endregion

#region vip奖励获取
public class GetVipLevelRewardResponse : BaseResponse
{
	public GetVipLevelRewardData data;
}

public class GetVipLevelRewardData
{
	public ItemOfReward[] p;
}
#endregion


#region 月卡
public class GetMonthGiftResponse:BaseResponse{
	public ItemOfReward[] data;
}
#endregion

#region 活动商城数据信息
public class ActiveShopItemResponse : BaseResponse
{
	public ActiveShopItemData data;
}

public class ActiveShopItemData
{
	public List<ActiveShopItem> shopItems; // 活动商品列表 
}

public class ActiveShopItem
{
	public int	pid;			//商品编号

	public string	pName;			//商品名称

	public int	pStar;			//商品星级

	public int num;			//商品数量

	public int local;			//位置

	public string	iconId;		//商品图标

	public string des;			//商品描述

	public int mType;			//货币类型，0，金币，1，钻石,  2, 积分    3,战功   其余的若为物品就为物品ID 

	public int price;			//未打折价格

	public int disPrice;		//打折后价格

	public int buyCount;		//允许购买次数

	public int endTime;		//截至日期

	public int startTime;     //起止时间

	public int yetBuyCount;	//已经购买次数
    
}
#endregion

#region url
public class UrlData
{
	public string url;
}
#endregion

#region 战功商店更新
public class RefreshZhangongShopItemResponse : BaseResponse
{
	public RefreshZhangongShopData data;
}

public class RefreshZhangongShopData : ZhanGongBuyItemData
{
	public int stone;
	public int refresh;
}

#endregion


#region 获取活动状态
public class GetSystemStatusData : BaseResponse
{
	public SysStatus getSysStatus;
}

#endregion

#region 新版活动
public class NewActivityItem
{
	public string itemID;    
	public string completeDesc;     //说明
	public string completeWhere;   //已完成total进度
	public int completeRate;      //当前进度

	/// <summary>
	/// const STATUS_OK = 1;        //可以领取
	/// const STATUS_REWARDED = 2;  //已领取
	/// const STATUS_NO = 3;        //不可领取
	/// const STATUS_JUMP = 4;      //跳转到商城
	/// </summary>
	public int status;       //是否领取
	public ItemOfReward[] rewards;      //获奖物品

}

public class NewActivityData
{
	public string activityId;
	public string activityDesc;
	public long activityEndTime;
	public long activityStartTime;
	public string activityName;
	public string activityIcon;
	public NewActivityItem[] items;
}
	
public class NewActivityResponse : BaseResponse
{
	public NewActivityData[] data;

	public bool IsGetReward()
	{
		if(data != null && data.Length != 0)
		{
			foreach(NewActivityData _data in data)
			{
				if(_data.items != null && _data.items.Length != 0)
				{
					foreach(NewActivityItem item in _data.items)
					{
						if(item.status == 1)return true;
					}
				}
			}
		}
		return false;
	}
}

#endregion

#region vip奖励获取
public class GetActivityLimittimeRewardResponse : BaseResponse
{
	public GetActivityLimittimeRewardData data;
}

public class GetActivityLimittimeRewardData
{
	public ItemOfReward[] p;
	public NewActivityItem item;
}
#endregion


#region  龙珠银行   3008-3010
public class GetDragonBankResponse :BaseResponse
{
	public BankData data;

}
public class BankData{
	//已收益
	public int profit_stone;
	//收益时间
	public int profit_time;
	//存款时间
	public int store_time;

	public int stone;
	public int gid;
	public float profit;
	public int surplusTime;
}
//3010

public class ReceiveBankMoney:BaseResponse{

	public ReceiveMoney data;
}

public class ReceiveMoney{
	public int stone;
}

#endregion
