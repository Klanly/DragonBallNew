using System;
using Framework;
/*
 *  把所有要传给服务器的参数，全部定义在这个类里面。
 * 
 */


#region 第三方登录获取服务器列表
[Serializable]
class ThirdGetServerParam : BaseRequestParam {

	public string account;			//唯一号
	public string session;			//部分平台为null
	public int os;					//系统
	public int maket;				//平台市场（360， 腾讯，91···）
	public string extension;        //扩展字段-目前用来黑桃的渠道ID

	public ThirdGetServerParam () 
	{
		AccountData ad = Native.mInstace.m_thridParty.GetAccountData();
		account = ad.uniqueId;
		session = ad.session;
		os = (int)ad.platform;
		maket = (int)ad.maket;
		extension = ad.extension;
	}
}
#endregion



#region 获取服务器列表
[Serializable]
class PartitionServerParam : BaseRequestParam {
	private const int FLAT_IOS = 1;
	private const int FLAT_ANDROID = 2;
	private const int FLAT_WP = 3;
	private const int FLAT_DEFAULT = FLAT_IOS;

	public int appv;
	public int account;
	public int flat;

	public PartitionServerParam (){ }

	public PartitionServerParam (int appversion, int acc) {
		#if UNITY_IPHONE
		flat = FLAT_IOS;
		#elif UNITY_ANDROID
		flat = FLAT_IOS;
		#elif UNITY_WP8
		flat = FLAT_WP;
		#else
		flat = FLAT_DEFAULT;
		#endif
		appv = appversion;
		account = acc;

	}
}
#endregion


#region 更换战斗队伍的队员

class ChangeTeamMemberParam : BaseRequestParam {
	public string gid;

	//sroleid:源角色唯一标识 
	public int sroleid;
	//droleid:目标角色唯一标识
	public int droleid;
	//pos:位置 One-Based
	public int pos;
	//tmid:队伍编号
	public int tmid;

	public ChangeTeamMemberParam() { }

	public ChangeTeamMemberParam(string gameid, int srcMonsterId, int tarMonsterId, int pos, int whichTeamId) {
		this.gid = gameid;
		this.sroleid = srcMonsterId;
		this.droleid = tarMonsterId;
		this.pos = pos;
		this.tmid = whichTeamId;
	}

}

#endregion

#region 更换装备

class ChangeEquipmentParam : BaseRequestParam {
	//"gid":玩家id
	public string gid;
	//"pos":位置 One-Based
	public short pos;
	//"teqid":目标装备唯一标识
	public int teqid;
	//"seqid":源目标准备唯一标识
	public int seqid;
	//tmid : 队伍编号
	public int tmid;

	public ChangeEquipmentParam() { }

	public ChangeEquipmentParam(string gameid, short pos, int targetId, int srcId, int whichTeamId) {
		this.gid = gameid;
		this.pos = pos;
		this.teqid = targetId;
		this.seqid = srcId;
		this.tmid = whichTeamId;
	}
}

#endregion

#region 登陆信息

public enum LoginType {
	TYPE_QUICK = 0x1,
	TYPE_THIRDPARTY = 0x2,
}

[Serializable]
class LoginParam : BaseRequestParam {
	//账户id
	public string accid;

	public string token;

	//选中服务器id
	public string sid;
	//1(快速上线)|2(账户上线)
	public short type;

	public LoginParam() { }

	public LoginParam(string loginToken, string serverId) 
	{ 
		AccountData ad = Native.mInstace.m_thridParty.GetAccountData ();
		accid = ad.uniqueId;
		token = loginToken;
		//type = (short)ad.lType;
		type = 2;
		sid = serverId;
	}
}

#endregion
[Serializable]
class ThirdPartyLogin: BaseRequestParam 
{
	public int appv;				//版本号
	public string account;			//唯一号
	public string session;			//部分平台为null
	public int os;					//系统
	public int maket;				//平台市场（360， 腾讯，91···）
	public string sid;				//选中服务器id

	public ThirdPartyLogin() { }

	public ThirdPartyLogin(AccountData ad, string selServer, int version) 
	{ 
		appv = version;
		account = ad.uniqueId;
		session = ad.session;
		os = (int)ad.platform;
		maket = (int)ad.maket;
		sid = selServer;
	}
}



#region 第三方登录


#endregion

#region 进入战斗
[Serializable]
class BattleParam : BaseRequestParam {

	//用户游戏ID
	public string gid;
	//doorId 小关卡ID 
	public int doorId;
	//subId 中关卡ID  
	public int subId;
	//masterId 大关卡ID
	public int masterId;
	//标识是否免费     0:免费   1:付费
	public int flag;
	//是否是这一章的最后一个关卡
	public int isLastFloorOfChapter;
	//是否是新手引导中
	public int isGuide;
	
	public BattleParam () { }

	public BattleParam (string GameId, int FloorId, int ID, int ChapterId ,int _flag = 0,int _isLastFloorOfChapter = 0, int isGuide = -1) {
		gid = GameId;
		doorId = FloorId;
		subId = ID;
		masterId = ChapterId;
		flag = _flag;
		isLastFloorOfChapter = _isLastFloorOfChapter;
		this.isGuide = isGuide;
	}
}

#endregion


#region Sell Monster
//{"gid":924464,%22roles%22:[197,199,201]}]

[Serializable]
class SellMonsterParam : BaseRequestParam {
	//Role id
	public string gid;
	public int[] roles; 

	public SellMonsterParam() { }

	public SellMonsterParam(string playerId, int[] monsterIds) {
		gid = playerId;
		roles = monsterIds;
	}
}
#endregion

#region 分解宠物
//{"gid":924464,%22roles%22:[197,199,201]}]

[Serializable]
class DecomposelMonsterParam : BaseRequestParam {
	//Role id
	public string gid;
	public int[] roles; 
	
	public DecomposelMonsterParam() { }
	
	public DecomposelMonsterParam(string playerId, int[] monsterIds)
	{
		gid = playerId;
		roles = monsterIds;
	}
}
#endregion

#region 进化宠物
//{"gid":924464,%22roles%22:[197,199,201]}]

[Serializable]
class EvolveMonsterParam : BaseRequestParam {
	//Role id
	public string gid;
	public int roleId; 

	public EvolveMonsterParam() { }

	public EvolveMonsterParam(string playerId, int monsterIds)
	{
		gid = playerId;
		roleId = monsterIds;
	}
}
#endregion

#region 魂魄合成
//request  -> act:135 data=[{"gid":玩家id,"chipId":碎片唯一标识}]
[Serializable]
class SoulHeChenParam : BaseRequestParam {
	//Role id
	public string gid;
	public int chipId; 

	public SoulHeChenParam() { }

	public SoulHeChenParam(string playerId, int chid)
	{
		gid = playerId;
		chipId = chid;
	}
}

#endregion


#region strengthen
//角色升级{"gid":玩家id,"sroleid":升级角色id,"roles"[目标角色id,目标角色id]}
[Serializable]
class StrengthenParam : BaseRequestParam
{
	public string gid;
	public int sroleid;
	public int[] roles;

	public StrengthenParam(){}

	public StrengthenParam(string playerID, int targetID, int[] srcMonsterIDs)
	{
		gid = playerID;
		sroleid = targetID;
		roles = srcMonsterIDs;
	}
}
#endregion


#region strenthEquip
//角色升级{"gid":玩家id,"sroleid":升级角色id,"roles"[目标角色id,目标角色id]}
[Serializable]
class StrengthEquipParam : BaseRequestParam
{
	public string gid;
	public int seqid;
	public int[] para;
	public int type;		// 1:武器，2：防具
	public int pnum;		//策划编号

	public StrengthEquipParam() {}

	public StrengthEquipParam(string playerId, int targetID, int[] srcIds, int tp, int num)
	{
		gid = playerId;
		seqid = targetID;
		para = srcIds;
		this.type = tp;
		this.pnum = num;
	}
}
#endregion


#region SellEquip
//卖装备 {"gid":玩家id, "equips”:[装备id]}
[Serializable]
class SellEquipParam : BaseRequestParam
{
	public string gid;
	public int[] equips;

	public SellEquipParam() {}

	public SellEquipParam(string playerId, int[] sells)
	{
		gid = playerId;
		equips = sells;
	}
}

#endregion

#region BuySomething
//购买物品{""gid":玩家id, "propid":道具编号(110025),"nm":数量}
[Serializable]
class PurchaseParam : BaseRequestParam
{
    public string gid;
    public int propid;
    public int nm;
    
    public PurchaseParam() {}
    
    public PurchaseParam(string playerId, int itemid, int itemNum)
    {
        gid = playerId;
        propid = itemid;
        nm = itemNum;
    }
}


#endregion

#region Swap Monster Pos
//[{"gid":829493,"sroleid":170,"troleid":171}]
[Serializable]
class SwapMonsterPosParam : BaseRequestParam
{
	public string gid;
	public int sroleid;
	public int troleid;


	public SwapMonsterPosParam() {}
	public SwapMonsterPosParam(string strPlayId, int srcId, int tgtId)
	{
		gid = strPlayId;
		sroleid = srcId;
		troleid = tgtId;

	}
}
#endregion

#region 召唤神龙
//{"gid":玩家id, "slty":1地球龙 | 2纳美克星}
[Serializable]
class CallDragonParam : BaseRequestParam
{
	public string gid;
	public int slty;

	public CallDragonParam(string strPlayId, int slty)
	{
		this.gid = strPlayId;
		this.slty = slty;
	}
}
#endregion

#region 学习奥义
[Serializable]
class learnAoYiRequestParam : BaseRequestParam{
	public string gid;
	public string pid;
	public learnAoYiRequestParam(string gid,string pid){
		this.gid = gid;
		this.pid = pid;
	}

}
#endregion

#region 装备奥义
[Serializable]
class EquipAoYiRequestParam : BaseRequestParam{
	public string gid;
	public int ppid;
	public int pos;
	public EquipAoYiRequestParam(string gid, int ppid, int pos){
		this.gid = gid;
		this.ppid = ppid;
		this.pos = pos;
	}

}
#endregion

#region 购买神龙祭坛奥义凹槽
// {"gid":玩家ID, "slot":奥义凹槽从1开始}
[Serializable]
class BuyAoYiSlotParam : BaseRequestParam
{
	public string gid;
	public int slot;

	public BuyAoYiSlotParam(string strPlayId, int slot)
	{
		this.gid = strPlayId;
		this.slot = slot;
	}
}
#endregion

#region 获取抢夺龙珠玩家列表
// {"gid":玩家ID, "oid":被抢玩家id, "ppid":道具编号}
[Serializable]
class GetQiangDuoDragonBallOpponentsParam : BaseRequestParam
{
	public string gid;
	public int pid;
	public int isGuide;
	public GetQiangDuoDragonBallOpponentsParam(string strPlayId, int pid,int isguide = 0)
	{
		this.gid = strPlayId;
		this.pid = pid;
		this.isGuide = isguide;
	}
}
#endregion

#region 抢夺龙珠战斗
[Serializable]
class QiangDuoDragonBallBattleParam : BaseRequestParam
{
	public string gid;
	public string oid;
	public string ppid;
	public int isGuide;
    public int id;
	public int revenge;   //默认0，1：复仇
	public int stone;
	public QiangDuoDragonBallBattleParam(string gid, string otherid, string ppid,int isguide,int id = -1,int revenge = 0, int m_stone = 0)
    {
		this.gid = gid;
		this.oid = otherid;
		this.ppid = ppid;
        this.isGuide = isguide;
        this.id = id;
		this.revenge = revenge;
		this.stone = m_stone;
	}
}
#endregion

#region 直接购买免战时间
[Serializable]
class BuyMianZhanTimeParam : BaseRequestParam
{
	public string gid;
    //1金币开启，2钻石开启，3道具开启
    public int type;
    // type 为3的时候，指定的玩家道具的唯一标识
    public int propId;

    public BuyMianZhanTimeParam(string gid, int type, int propId) {
		this.gid = gid;
        this.type = type;
        this.propId = propId;
	}
}
#endregion

#region
//请求:{"gid":玩家id,"propid":道具编号,"nm":数量}
[Serializable]
class UsePropParam : BaseRequestParam
{
	public string gid;
	public int propid;
	public int propNum;
	public int nm;

	public UsePropParam() {}

	public UsePropParam(string strId, int prop, int num, int count)
	{
		gid = strId;
		propid = prop;
		propNum = num;
		nm = count;
	}
}
#endregion

#region
//换队伍{"gid":玩家id,"teamid":队伍id}
[Serializable]
class SwapTeamParam : BaseRequestParam
{
	public string gid;
	public int teamid;
	

	public SwapTeamParam() {}
	public SwapTeamParam(string strid, int tid)
	{
		gid = strid;
		teamid = tid;
	}
}
#endregion

#region 活动奖励
[Serializable]
class BuyLotteryParam : BaseRequestParam{
	public string playerId;
	public int money;
	public int type;  // 1,dia  2,free
	public BuyLotteryParam(){

	}
	public BuyLotteryParam(string pId,int BuyType,int needMoney){
		playerId = pId;
		type = BuyType;
		money = needMoney;
	}

}

#endregion

#region 天下第一战斗
[Serializable]
class FightBattleParam : BaseRequestParam
{
	public int gid;
	public int otherid;
	public int otherRank;
    public int id;      //   赌博  
	public int revenge; //默认0,1：复仇
	public int stone;
	public FightBattleParam(int gid, int otherid, int rank,int id = -1,int revenge = 0, int m_stone = 0)
    {
		this.gid = gid;
		this.otherid = otherid;
		this.otherRank = rank;
        this.id = id;
		this.revenge = revenge;
		this.stone = m_stone;
	}
}
#endregion

#region 天下第一比武大会对手列表
[Serializable]
class GetTianXiaDiYiOpponentsParam : BaseRequestParam
{
	public string gid;
	public int isGuide;
	public GetTianXiaDiYiOpponentsParam(string gid,int isGuide)
	{
		this.gid = gid;
		this.isGuide = isGuide;
	}
}
#endregion

#region 战功购买物品
[Serializable]
class ZhanGongBuyItemParam : BaseRequestParam
{
	public int gid;
	public int id;
	public ZhanGongBuyItemParam(int gid, int propid)
	{
		this.gid = gid;
		this.id = propid;
	}
}
#endregion

#region 得到已经领到的礼品ID
[Serializable]
class GetZhanGongBuyItemIDParam : BaseRequestParam
{
	public string gid;
	public GetZhanGongBuyItemIDParam(string gid)
	{
		this.gid = gid;
	}
}
#endregion

#region 夺取金币对手列表
[Serializable]
class GetQiangDuoGlodOpponentsParam : BaseRequestParam
{
	public string gid;

	public GetQiangDuoGlodOpponentsParam(string gid)
	{
		this.gid = gid;
	}
}
#endregion

#region 战功购买物品
[Serializable]
class QiangDuoGoldBuyItemParam : BaseRequestParam
{
	public int gid;
	public int id;
	public QiangDuoGoldBuyItemParam(int gid, int propid)
	{
		this.gid = gid;
		this.id = propid;
	}
}
#endregion

#region 得到已经领到的礼品ID
[Serializable]
class GetQiangDuoGoldBuyItemTotalParam : BaseRequestParam
{
	public string gid;
	public GetQiangDuoGoldBuyItemTotalParam(string gid)
	{
		this.gid = gid;
	}
}
#endregion

#region 添加宿敌
[Serializable]
class AddSuDiParam : BaseRequestParam
{
    public int gid;
    public int eid;
    public AddSuDiParam(int gid, int enemyid)
    {
        this.gid = gid;
        this.eid = enemyid;
    }
}
#endregion

#region 得到宿敌列表
[Serializable]
class GetSuDiParam : BaseRequestParam
{ 
    // 用户ID
    public int gid;    // 玩家角色ID
    public int type;   //1:抢夺龙珠宿敌，2：排行榜宿敌，3：抢夺金币宿敌
    public int ballId; //在type=1时，龙珠Id

    public GetSuDiParam(int gid,int type,int ballId)
	{
		this.gid = gid;
        this.type = type;
        this.ballId = ballId;
	}
}
#endregion

#region 删除宿敌列表
[Serializable]
class DeleteSuDiParam : BaseRequestParam
{
	public int gid;
	public int eid;
	public DeleteSuDiParam(int gid, int eid)
	{
		this.gid = gid;
        this.eid = eid;
	}
}
#endregion

#region 建筑收钱精力
[Serializable]
class BuildGetParam : BaseRequestParam
{
	public string gid;
	public int bid;					
	public int sync;
	public int gainType;
	public BuildGetParam(string id, int buildId, int _gainType = 0)
	{
		gid = id;
		bid = buildId;
		sync = 1;
		gainType = _gainType;
	}
}
#endregion

#region 建造建筑
[Serializable]
class CreateBuildParam : BaseRequestParam
{
	public string gid;
	public int bnum;
	public int sync;

	public CreateBuildParam(string id, int buildNum)
	{
		gid = id;
		bnum = buildNum;
		sync = 1;
	}
}
#endregion

#region 建筑升级
//gid:玩家ID  bid：建筑ID 
[Serializable]
class BuildUpgradeParam : BaseRequestParam
{
	public string gid;
	public int bid;
	public int sync;

	public BuildUpgradeParam(string id, int build)
	{
		gid = id;
		bid = build;
		sync = 1;
	}
}
#endregion

#region 建筑物开启
[Serializable]
public class BattleBuildOpenParam : BaseRequestParam
{
	public string gid;
	public int bid;
	public int type;
	public int sync;
	public int openType;			//建筑物开启类型 1：小电池开启，2：中电池开启，3：大电池开启
	
	public BattleBuildOpenParam(string id, int build,int tp,int _type = 0)
	{
		gid = id;
		bid = build;
		type = tp;
		sync = 1;
		openType = _type;
	}
}
#endregion

#region 合成
//mr：主宠    el：吃宠  ty:1:真武者合成，2：真武者合成超武者，3：武者直接合成超武者
[Serializable]
public class HeChengParam : BaseRequestParam
{
	public string gid;
	public int mr;
	public int[] el;
	public int ty;

	public HeChengParam() {}

	public HeChengParam(string id, int main, int[] eat, int type)
	{
		gid = id;
		mr = main;
		el = eat;
		ty = type;
	}
}
#endregion


#region 招募状态
//gid:玩家ID
[Serializable]
public class ZhaoMuStateParam : BaseRequestParam
{
	public string gid;

	public ZhaoMuStateParam(string id)
	{
		gid = id;
	}
}
#endregion

#region 招募
//gid:玩家ID  type：1 2 3 4 5
[Serializable]
public class ZhaoMuParam : BaseRequestParam
{
	public string gid;
	public int poolnm;
    public  int flag ;
    public ZhaoMuParam(string id, int tp, int f)
	{
		gid = id;
		poolnm = tp;
        flag = f;
	}
}
#endregion

#region 获取终极试炼信息1
//ak:1 df:2 sf:3 eak:4 edf:5 esf:6
class GetFinalTrialInfoParam : BaseRequestParam
{
    public string gid;
    public int ty;
    public int wty;
    public GetFinalTrialInfoParam(string id, int type, int wtype)
    {
        gid = id;
        ty = type;
        wty = wtype;
    }
    
}
#endregion

#region 获取终极试炼信息2
//ak:1 df:2 sf:3 eak:4 edf:5 esf:6
class GetFinalTrialInfoSecondParam : BaseRequestParam
{
    public string gid;
    public int wty;
    public GetFinalTrialInfoSecondParam(string id, int mwty)
    {
        gid = id;
        wty = mwty;
    }
    
}
#endregion

#region 终极试炼选择加成
//ak:1 df:2 sf:3 eak:4 edf:5 esf:6
class FinalTrialJiachengParam : BaseRequestParam
{
    public int ty;
    public FinalTrialJiachengParam(int type)
    {
        ty = type;
    }
    
}
#endregion

#region 每三关加成数据
//1->easy 2->middle 3->hard
class FinalTrialChallengeParam : BaseRequestParam
{
    public int ty;
    public FinalTrialChallengeParam(int type)
    {
        ty = type;
    }
    
}
#endregion

#region 每三关加成
//ty-> ak:1 df:2 sf:3 eak:4 edf:5 esf:6 pls->加成值   str->星星数
class FinalTrialDungoenAddParam : BaseRequestParam
{
    public int ty;
    public int pls;
    public int str;
    public FinalTrialDungoenAddParam(int type, int mypls, int starnum)
    {
        ty = type;
        pls = mypls;
        str = starnum;
    }
    
}

#endregion

#region 变换属性
//gid:玩家ID  mid 宠物ID
[Serializable]
class AttrSwapParam : BaseRequestParam
{
	public string gid;
	public int roleid;

	public AttrSwapParam(string id, int monId)
	{
		gid = id;
		roleid = monId;
	}
}

#endregion
class AnnounceParam : BaseRequestParam
{
    public string gid;
    public int ty;
    
    public AnnounceParam(string id, int mty)
    {
        gid = id;
        ty = mty;
    }
}
#region 

#endregion

#region 请求吃美食的状态

[Serializable]
class HavingDinnerStateParam : BaseRequestParam
{
	public string gid;

	public HavingDinnerStateParam(string id)
	{
		gid = id;
	}
}

#endregion

#region 请求吃美食

[Serializable]
class HavingDinnerParam : BaseRequestParam
{
	public string gid;

	public HavingDinnerParam(string id)
	{
		gid = id;
	}
}

#endregion

#region 签到
[Serializable]
class SignDateParam : BaseRequestParam
{
	public string gid;

	public SignDateParam(string id)
	{
		gid = id;
	}
}
class SignParam:BaseRequestParam{
	public string gid;
	public int day;
	public SignParam(string id,int day){
		gid = id;
		this.day = day;
	}
}


#endregion


#region 淘宝
[Serializable]
class TaobaoParam : BaseRequestParam
{
	public string gid;
	public string num;
	public TaobaoParam(string id,string num)
	{
		gid = id;
		this.num = num;
	}
}

#endregion




#region 获得好友列表
class GetFriendListParam : BaseRequestParam
{
	// 用户ID
	public string roleId;

	// 页码
	public int pageId;

	// 预留字段
	public int type;

	public GetFriendListParam(string roleId, int pageId, int type)
	{
		this.roleId = roleId;
		this.pageId = pageId;
		this.type = type;
	}
}
#endregion

#region 留言
class SendMessageParam : BaseRequestParam
{
	public string roleId;
	public int sRodeId;
	public string msg;

	public SendMessageParam(string roleId, int sRodeId, string msg)
	{
		this.roleId = roleId;
		this.sRodeId = sRodeId;
		this.msg = msg;
	}
}
#endregion

#region 搜索
class SearchUserParam : BaseRequestParam
{
	public string roleId;
	public string condition;
	// 1搜索，2推荐	
	public int op;

	public SearchUserParam(string roleId, string condition, int op)
	{
		this.roleId = roleId;
		this.condition = condition;
		this.op = op;
	}
}
#endregion

#region 获取添加好友请求列表
public class GetRequestFriendListParam : BaseRequestParam
{
	// 用户ID
	public string roleId;

	// 页码
	public int pageId;

	// 预留字段
	public int type;

	public GetRequestFriendListParam(string roleId, int pageId, int type)
	{
		this.roleId = roleId;
		this.pageId = pageId;
		this.type = type;
	}
}
#endregion

#region 同意或者拒绝添加好友
public class AgreeOrRefusedFriendParam : BaseRequestParam
{
	public string roleId;
	public string sRodeId;
	// 1为 同意 2为 不同意
	public int op;
	public AgreeOrRefusedFriendParam(string roleId, string sRodeId, int op)
	{
		this.roleId = roleId;
		this.sRodeId = sRodeId;
		this.op = op;
	}
}
#endregion


#region 潜力训练
//赔养角色{"gid":790683,"roleid":角色唯一标识,"ty":1(普通)|2(特训),"target":1(ak)|2(df)}
public class QianLiXunLianParam : BaseRequestParam
{
	public string gid;
	public int roleid;
	public int ty;
	public int target;
	public int num;
	public QianLiXunLianParam(string gid, int id, int type, int tgt,int tnum)
	{
		this.gid = gid;
		this.roleid = id;
		this.ty = type;
		this.target = tgt;
		this.num = tnum;
	}
}
#endregion

#region 潜力重置
//赔养角色{"gid":790683,
public class QianLiResetParam : BaseRequestParam
{
	public string gid;
	public int roleid;

	public QianLiResetParam(string gd, int pd)
	{
		this.gid = gd;
		this.roleid = pd;
	}
}
#endregion


#region 添加/删除好友
public class AddOrDeleteFriendParam : BaseRequestParam
{
	public string roleId;
	public string sRoleId;

	// 1申请添加好友,2删除好友
	public int op;
	public AddOrDeleteFriendParam(string roleId, string sRoleId, int op)
	{
		this.roleId = roleId;
		this.sRoleId = sRoleId;
		this.op = op;
	}
}
#endregion

#region 发送玩家ID，用于请求战报，邮件等协议参数
public class PlayerIDParam : BaseRequestParam
{
    public string gid;
	public int sync;
    public PlayerIDParam(string gid, int syn = 0)
    {
        this.gid = gid;
		this.sync = syn;
    }
}
#endregion

#region 为消息设置状态
/**
 * 为消息设置状态
 * @param int 	$gid			玩家角色ID
 * @param array $ids			消息ID
 * @param int 	$type			操作类型 1：设置为已读，2，删除
 * @param int 	$msgType		消息类型	1：战报，2，邮件
 */
public class ChangeMailStateParam : BaseRequestParam
{
	public string gid;
	public int[] ids;
	public int type;
	public int msgType;
}
#endregion



#region 究极试炼战斗
public class FinalTrialFightParam : BaseRequestParam
{
    public string gid;
    public int ty;
    public int wty;
    public FinalTrialFightParam(string gid, int ty, int wty)
    {
        this.gid = gid;
        this.ty = ty;
        this.wty = wty; 
    }
}
#endregion

#region 究极试炼排行请求
public class FinalTrialGetRankParam : BaseRequestParam
{
	public string gid;
	public int teamnm;
	public int wty;
	public FinalTrialGetRankParam(string gid, int teamnm, int wty)
	{
		this.gid = gid;
		this.teamnm = teamnm;
		this.wty = wty;
	}
}
#endregion

#region 究极试炼登录请求
public class FinalTrialLoginDataParam : BaseRequestParam
{
	public string gid;
	public FinalTrialLoginDataParam(string gid)
	{
		this.gid = gid;
	}
}
#endregion

#region 新究极试炼登录请求
public class FinalTrialLoginInfoParam : BaseRequestParam
{
	public string gid;
	public FinalTrialLoginInfoParam(string gid)
	{
		this.gid = gid;
	}
}
#endregion

#region 神秘商店商品请求
public class SecretShopParam : BaseRequestParam
{
	public int gid;
	public int no;
	public SecretShopParam(int gid, int no)
	{
		this.gid = gid;
		this.no = no;
	}
}
#endregion

#region 神秘商店购买
public class SecretShopBuyParam : BaseRequestParam
{
	public int gid;
	public int id;
	public int num;
	public int count;
	public int no;
	public SecretShopBuyParam(int gid, int id, int num, int count, int no)
	{
		this.gid = gid;
		this.id = id;
		this.num = num;
		this.count = count;
		this.no = no;
	}
}
#endregion



#region 等级奖励
public class LevelGiftStateParam:BaseRequestParam{
	public string gid;

	public LevelGiftStateParam(string gid){
		this.gid =gid;

	}
}
public class GetLevelGiftParam:BaseRequestParam{
	public string gid;
    public int lv;
	public GetLevelGiftParam(string gid,int dataNum){
		this.gid = gid;
        this.lv = dataNum;
	}
}
#endregion


#region 随机产生用户名
[Serializable]
public class RandomNameParam : BaseRequestParam{
	public string gid;

	public RandomNameParam(string gid)
	{
		this.gid =gid;
	}
}
#endregion

#region 修改用户信息
[Serializable]
//type : 1 改头像 2  改昵称      param：根据type的参数
public class ChangeUserInfoParam : BaseRequestParam
{
	public string gid;
	public short type;
	public string param;

	public ChangeUserInfoParam(string gid, short tp, string strParam)
	{
		this.gid =gid;
		this.type = tp;
		this.param = strParam;
	}
}
#endregion


public class Send_GemSynticSystem:BaseRequestParam{
	/*玩家ID
	 * */
	public string gid;
	/*第一颗宝石
	 * */
	public int g_id1;
	/*第二颗宝石
	 * */
	public int g_id2;
	/*打磨石的数量
	 * */
	public int nm;
	/*是否保底*/
	public bool bd;
}


public class Send_GemInLaySystem:BaseRequestParam
{
	/*玩家ID
	 * */
	public string gid;
	/*装备ID
	 * */
	public int eid;
	/*槽格子从0开始计算
	 * */
	public int slot;
	/*宝石唯一标识
	 * */
	public int gemId;
	/*1(装上)|0(卸下)
	 * */
	public int beUp;
}


public class Send_GemExchangeSystem:BaseRequestParam
{
	/*玩家ID
	 * */
	public string gid;
	/*宝石编号
	 * */
	public int pid;
}

#region 查询究极试炼排行阵型
[Serializable]
public class FinalTrialRankCheckInfoParam : BaseRequestParam
{
	public string gid;
	public string oid;
	
	public FinalTrialRankCheckInfoParam(string gid,  string oid)
	{
		this.gid =gid;
		this.oid = oid;
	}
}
#endregion

public class Send_GemRecastSystem:BaseRequestParam
{
	/*玩家ID
	 * */
	public string gid;
	/*装备ID
	 * */
	public int eqid;
	/*锁定槽数组
	 * */
	public int[] locks;
}

public class Send_GuideFinish : BaseRequestParam
{
	/*玩家I
		 * */
	public string gid;
	/*新手引导大阶段
		 * */
	public int step;
	
	public int sync;

	public int isNew;
}

#region 请求战斗回放列表
public class BattleVideoPlaybackParam : BaseRequestParam
{
	public int gid;
	public int type;

	public BattleVideoPlaybackParam(int _gid, int _type)
	{
		this.gid = _gid;
		this.type = _type;
	}
}
#endregion

#region 请求战斗回放
public class GetBattleVideoPlaybackParam : BaseRequestParam
{
	public int gid;
	public int videoId;
	//请求的是攻击还是挨打，服务器不关心 ( 1 代表我主动攻击， -1 代表我挨打）
	public int AttOrSuff;

	public GetBattleVideoPlaybackParam(int _gid, int _videoid, int AttorSuff) {
		this.gid = _gid;
		this.videoId = _videoid;
		this.AttOrSuff = AttorSuff;
    }
}
#endregion

#region 请求7天数据
public class GetSevenRewardListParam : BaseRequestParam
{
	public int gid;

	public GetSevenRewardListParam(int _gid)
	{
		this.gid = _gid;
	}
}
#endregion

#region 7天奖励领取
public class ReceiveSevenRewardParam : BaseRequestParam
{
	public int gid;
	
	public ReceiveSevenRewardParam(int _gid)
	{
		this.gid = _gid;
	}
}
#endregion


#region 检查更新配表
public class CheckConfig:BaseRequestParam
{
	public string game;
	public string version;
	public string md5;
	public int  act;
	public string platform;
	public CheckConfig(int act,string _md5,string _version = "1")
	{
		this.act = act;
		this.game = "dragonball";
		
#if UNITY_IPHONE
		this.platform = "ios";
#elif UNITY_ANDROID
        this.platform = "android";
#endif
		this.md5 = _md5;
		this.version = _version;
	}
}
#endregion

#region 开宝箱请求  获取宝箱状态 请求 
public class GetTreasureStateParam:BaseRequestParam{

    public int gid;
    public GetTreasureStateParam(int _gid){
        this.gid = _gid;
    }

}


public class OpenTreasureBoxParam:BaseRequestParam{
    /// <summary>
    /// 1:Cu 2:Ag 3:Au
    /// </summary>
    public int boxType;
    public int gid;
    public int type;
    public OpenTreasureBoxParam(int _qid,int _type,int _openType){
        this.gid = _qid;
        this.boxType = _type;
        this.type = _openType;
    }

}
#endregion

#region 	Vipinfoshop

public class GetVipShopParam:BaseRequestParam
{
	public int gid;
	
	public GetVipShopParam(int _gid)
	{
		this.gid = _gid;
	}
	
}
#endregion

#region VIP领取奖励

public class GetVipGiftParam:BaseRequestParam
{
	public int gid;
	
	public GetVipGiftParam(int _gid)
	{
		this.gid = _gid;
	}
	
}

#endregion

#region 究极试炼第一界面
public class GetNewFinalTrialEnterRequestParam:BaseRequestParam
{
	public int gid;	//		玩家角色ID
	public int type;   //		试炼类型，1 布欧的游戏，2 沙鲁的游戏

	public GetNewFinalTrialEnterRequestParam(int gid, int type)
	{
		this.gid = gid;
		this.type = type;
	}
}
#endregion

#region 究极试炼Add Buffer
public class GetNewFinalTrialAddBufferRequestParam:BaseRequestParam
{
	public int gid;	//		玩家角色ID
	public int type;   //		试炼类型，1 布欧的游戏，2 沙鲁的游戏
	
	public GetNewFinalTrialAddBufferRequestParam(int gid, int type)
	{
		this.gid = gid;
		this.type = type;
	}
}
#endregion

#region 究极试炼地图
public class GetNewFinalTrialMapRequestParam:BaseRequestParam
{
	public int gid;	 //玩家角色ID
	public int type;               //	试炼类型，1 布欧的游戏，2 沙鲁的游戏
	public int index;	    //		0,1,2,3 根据		bufferList 来
	
	public GetNewFinalTrialMapRequestParam(int gid, int type, int index)
	{
		this.gid = gid;
		this.type = type;
		this.index = index;
	}
}

#endregion

#region 究极试炼地图
public class GetNewFinalTrialMapNotAddRequestParam:BaseRequestParam
{
	public int gid;	 //玩家角色ID
	public int type;               //	试炼类型，1 布欧的游戏，2 沙鲁的游戏
	
	public GetNewFinalTrialMapNotAddRequestParam(int gid, int type)
	{
		this.gid = gid;
		this.type = type;
	}
}
#endregion

#region 究极试炼战斗
public class GetNewFinalTrialFightRequestParam:BaseRequestParam
{
	public int gid;	 //玩家角色ID
	public int type;               //	试炼类型，1 布欧的游戏，2 沙鲁的游戏
	public int level;   //0hard 1normal 2easy
	public int layer;   //curdoungeon
	
	public GetNewFinalTrialFightRequestParam(int gid, int type, int level, int layer)
	{
		this.gid = gid;
		this.type = type;
		this.level = level;
		this.layer = layer;
    }
}
#endregion

#region 究极试炼排行请求
public class NewFinalTrialGetRankParam : BaseRequestParam
{
	public int gid;
	public int type;
	public int teamtype;
	public NewFinalTrialGetRankParam(int gid, int type, int teamtype)
	{
		this.gid = gid;
		this.type = type;
		this.teamtype = teamtype;
	}
}
#endregion

#region 查询究极试炼排行阵型
[Serializable]
public class NewFinalTrialRankCheckInfoParam : BaseRequestParam
{
	public int gid;
	public int otherId;
	
	public NewFinalTrialRankCheckInfoParam(int gid, int otherId)
	{
		this.gid =gid;
		this.otherId = otherId;
	}
}
#endregion

#region 连击
public class BattleComboParam : BaseRequestParam
{
	public string gid;		//用户id
	public int combo;		//   怒气   连击数
	public int tCombo;      //		怒气  总连技数
	public int endCombo;	//  结束后 屏幕 combo
	public int totalCombo;	//  综上  总连击数


	public BattleComboParam(string id, int cmb, int tcmb,int tEndcmb,int tTCombo) {
		gid = id;
		combo = cmb;
		tCombo = tcmb;
		endCombo = tEndcmb;
		totalCombo = tTCombo;
	}

	public BattleComboParam() { }

}

#endregion

#region 是否下载
public class DownloadFinishParam : BaseRequestParam
{
    public int gid;
    public int version;
    public int md5;

    public DownloadFinishParam(int gid,int version,int md5){
        this.gid = gid;
        this.version = version;
        this.md5 = md5;
    }
}
#endregion

#region 下载获取
public class DownloadReceoveParam : BaseRequestParam
{
    public int gid;
    public int version;
    public int md5;

    public DownloadReceoveParam(int gid,int version,int md5){
        this.gid = gid;
        this.version = version;
        this.md5 = md5;
    }
}
#endregion

#region 下载查看
public class DownloadCheckParam : BaseRequestParam
{
    public int gid;
    public DownloadCheckParam(int gid){
        this.gid = gid;
    }
}
#endregion

#region 获取主界面的公告
public class MovableNoticeParam : BaseRequestParam
{
	public string gid;

	public MovableNoticeParam () { }

	public MovableNoticeParam(string uId) {
		this.gid = uId;
	}
}
#endregion

#region 从服务器获取连击数
public class GetComboParam:BaseRequestParam
{
	public string gid;
	public GetComboParam(){}
	public GetComboParam(string _gid)
	{
		this.gid = _gid;
	}
}
#endregion

#region 请求下注类型列表
public class GetGambleParam:BaseRequestParam
{
    public string gid;

    public GetGambleParam(){}
    public GetGambleParam(string _gid)
    {
        this.gid = _gid;
    }
}
#endregion

#region 发送邮件留言
public class SendEmailParam:BaseRequestParam
{
	public string gid;
	public int cgid;
	public string content;
	public SendEmailParam(){}
	public SendEmailParam(string gid,int cgid,string content)
	{
		this.gid = gid;
		this.cgid = cgid;
		this.content = content;
	}
}

#endregion

#region 提取附件
public class GetMailAttchment:BaseRequestParam
{
	public string gid;
	public int msgId;
	public GetMailAttchment(){}
	public GetMailAttchment(string gid,int msgId)
	{
		this.gid = gid;
		this.msgId = msgId;
	}
}
#endregion

#region 抢夺排行榜
public class GetChellengeRankParam : BaseRequestParam
{
	public int gid;
	public GetChellengeRankParam(int gid)
	{
		this.gid = gid;
	}
}
#endregion

#region 天下第一排行榜
public class GetTianXiaRankParam : BaseRequestParam
{
	public int gid;
	public int limit;
	public GetTianXiaRankParam(int gid, int limit)
	{
		this.gid = gid;
		this.limit = limit;
	}
}
#endregion
#region
public class GetCallDragonIsFinishParam:BaseRequestParam{
    public int gid;
    public int type;        // 1 earth      2 NMKX
    public GetCallDragonIsFinishParam(int gid,int type){
        this.gid = gid;
        this.type = type;
    }
}
#endregion

#region 任务列表
public class Send_TaskList:BaseRequestParam
{
	/*玩家ID
	 * */
	public string gid;
	public Send_TaskList(string gid)
	{
		this.gid = gid;
	}
}
#endregion

public class Send_TaskReward:BaseRequestParam
{
	public string gid;
	public int taskId;
	public Send_TaskReward(string gid,int taskId)
	{
		this.gid = gid;
		this.taskId = taskId;
	}
}

#region 刮刮乐状态
public class GuaGuaStatusRequest : BaseRequestParam
{
	public int gid;
	public GuaGuaStatusRequest (int _gid)
	{
		this.gid = _gid;
	}
}
#endregion

#region 刮刮乐
public class GuaGuaLeRequest : BaseRequestParam
{
	public int gid;
	public GuaGuaLeRequest (int _gid)
	{
		this.gid = _gid;
	}
}
#endregion
    
//  获取列表 604 
public class GotGodGiftListParam : BaseRequestParam{
    public int gid;
    public GotGodGiftListParam(int gid){
        this.gid = gid;
    }
}

//买奖励    605
public class BuyGodGiftParam:BaseRequestParam{
    public int gid;
    public BuyGodGiftParam(int gid){
        this.gid = gid;
    }


}

//大转盘列表
class BigWheelListParam : BaseRequestParam
{
	public string gid;
    public BigWheelListParam(string gid){
        this.gid = gid;
    }
}

public class RollActParam:BaseRequestParam{
    public int gid;
    public int isRoll;
    public RollActParam(int tGid,int tIsRoll){
        this.gid = tGid;
        this.isRoll = tIsRoll;
    }
}

public class HolidayActivityParam : BaseRequestParam
{
    public string gid;
    public HolidayActivityParam(string gid )
    {
        this.gid = gid; 
    }

}


#region 第三方支付下订单
public class PayParam : BaseRequestParam
{
	public int sid;					//服务器ID
	public int maket;				//市场
	public string uid;				//账号ID
	public string gid;				//玩家游戏ID
	public string goodId;			//充值物品ID
	public int num;					//订单数量
	public string extension;       //扩展字段

	public PayParam(string gdId)
	{
		AccountData ad = Native.mInstace.m_thridParty.GetAccountData ();
		sid = Core.SM.curServer.sid;
		maket = (int)ad.maket;
		uid = Core.Data.playerManager.RTData.uId;
		gid = Core.Data.playerManager.PlayerID;
		goodId = gdId;
		num = 1;
		extension = ad.extension;
	}
}
#endregion


#region 查询订单状态
public class QueryPayStatusParam : BaseRequestParam
{
	public string gid;				//玩家游戏ID
	public string billId;			//订单ID

	public QueryPayStatusParam(string tgid, string oId)
	{
		gid = tgid;
		billId = oId;
	}
}
#endregion

#region 获取充值次数列表
public class GetPayCntParam : BaseRequestParam
{
	public string gid;				//玩家游戏ID

	public GetPayCntParam(string tgid)
	{
		gid = tgid;
	}
}
#endregion

#region 则卷密保
public class GetSecretSoulHeroParam : BaseRequestParam
{
	public int gid;				//玩家游戏ID
	
	public GetSecretSoulHeroParam(int tgid)
	{
		gid = tgid;
    }
}
#endregion

#region 通知服务器当前进度
public class SendCurUserStateParam:BaseRequestParam
{
//	public UserTypeData user;
//	public ControllerEventData[] step;
    public int gid;
    public int guideid;
    public int lv;

    public SendCurUserStateParam(int  tGid, int tGuideId,int tLv){
        gid = tGid;
        guideid = tGuideId;
        lv = tLv;
	}

}

public class SendUserDeviceInfoParam:BaseRequestParam{
	public ModelTypeData[] modeltype;
	public SendUserDeviceInfoParam(ModelTypeData[] tModelType){
		modeltype = tModelType;
	}
}

public class ModelTypeData{
	public string type;
	public ModelTypeData(string tType){
		if(string.IsNullOrEmpty(tType))
			tType = "unknown";
		type = tType;
	}
}
public class UserTypeData{
	public int accountId;
	public int id;
	public int lv;
	public int vip;
	public int guide;

	public UserTypeData(int acId,int tId,int tLv, int tVip,int tGuide){
		accountId = acId;
		id = tId;
		lv = tLv;
		vip = tVip;
		guide = tGuide;
	}
}

public class ControllerEventData{
	public string ctrl;
	public string eventData;
	public ControllerEventData(string tCtrl,string tEventData){
		ctrl = tCtrl;
		eventData = tEventData;
	}
}

#endregion


#region 客户端计算战斗的版本

//请求战斗
public class ClientBattleParam : BaseRequestParam {
	//用户游戏ID
	public string gid;
	//doorId 小关卡ID 
	public int doorId;
	//subId 中关卡ID  
	public int subId;
	//masterId 大关卡ID
	public int masterId;
	//标识是否免费     0:免费   1:付费
	public int flag;
	//是否是这一章的最后一个关卡
	public int isLastFloorOfChapter;
	//是否是新手引导
	public int isGuide;

	//上阵的所有片牌
	public int[] cardIds;
	//当前的队伍ID
	public int tid;

	public string teamSeq;			//战斗队伍顺序
	public int battleType;			//战斗类型

	public ClientBattleParam ( ) { }

	public ClientBattleParam (string GameId, int FloorId, int ID=0, int ChapterId=0 ,int _flag = 0, int _isLastFloorOfChapter = 0, int Guide = -1,int[] array = null,int teamID = 1) { 
		gid = GameId;
		doorId = FloorId;
		subId = ID;
		masterId = ChapterId;
		flag = _flag;
		isLastFloorOfChapter = _isLastFloorOfChapter;
		isGuide = Guide;
		cardIds = array;
		tid = teamID;

		//初始化战斗队伍顺序
		battleType = Core.Data.battleTeamMgr.GetBattleType (doorId);
		BattleTeam bteam = Core.Data.battleTeamMgr.GetBattleTeam (battleType);
		if (bteam != null)
		{
			if (this.tid == 1)
			{
				teamSeq = bteam.atkTeam;
			}
			else
			{
				teamSeq = bteam.defTeam;
			}
		}
	}
}

//请求扫荡
public class SaoDangParam : BaseRequestParam
{
	//用户游戏ID
	public string gid;
	//doorId 关卡ID 
	public int doorId;
	//扫荡次数
	public int count;
	
	public SaoDangParam(string PlayerID, int FloorId,int count)
	{
		gid = PlayerID;
		doorId = FloorId;
		this.count = count;
	}
}

//战斗结算，战斗失败也要向服务器通知
public class ClientBattleCheckParam : BaseRequestParam {
	//用户游戏ID
	public string gid;
	//当前处于新手引导 0 非新手，1，新手引导阶段
	public int isGuide;

    //具体的信息
    public ClientBattleInfo sequence;

	public ClientBattleCheckParam() { }

	public ClientBattleCheckParam(ClientBattleParam argu, Statistic any, StaticsticList full, string video, short[] videoParse, ComboGather combo) { 
		gid = argu.gid;
		isGuide = argu.isGuide;
		sequence = new ClientBattleInfo(argu, any, full, video, videoParse, combo);
	}
}

//技能升级请求
public class SkillUpgradeParam : BaseRequestParam
{
	public string gid;				//用户游戏ID
	public int roleId;				//武者ID
	public int skillNum;				//技能ID

	public SkillUpgradeParam(string userId, int mid, int sid)
	{
		gid = userId;
		roleId = mid;
		skillNum = sid;
	}
}

/// <summary>
/// 连击统计
/// </summary>
public class ComboGather{
	//第一阶段
	public int btnMax;
	public int btnTotal;

	/// <summary>
	/// 第二阶段
	/// </summary>
	public int fingerMax;
	public int fingerTotal;
}

public class ClientBattleInfo {

    //用户游戏ID
    public string gid;
    //doorId 小关卡ID 
    public int doorId;
    //subId 中关卡ID  
    public int subId;
    //masterId 大关卡ID
    public int masterId;
    //标识是否免费     0:免费   1:付费
    public int flag;
    //是否是这一章的最后一个关卡
    public int isLastFloorOfChapter;

    //标识左边的输赢，0:输 1:赢
    public int LeftWin;

    //是否是新手引导
    public int isGuide;

	//战斗序列
    public string video;
	//战斗序列的反序列化
	public short[] videoparse;

    public Statistic Statistics;
    public StaticsticList show;

	//连击
	public ComboGather combo;


    public ClientBattleInfo() { }

	/// <summary>
	/// PVE副本普通战斗
	/// </summary>

	public ClientBattleInfo(ClientBattleParam argu, Statistic any, StaticsticList full, string video, short[] videoParse, ComboGather cbInfo) {
        gid = argu.gid;
        doorId = argu.doorId;
        subId = argu.subId;
        masterId = argu.masterId;
        flag = argu.flag;
        isLastFloorOfChapter = argu.isLastFloorOfChapter;
        isGuide = argu.isGuide;

        Statistics = any;
        show = full;

        this.video = video;
		this.videoparse = videoParse;
		combo = cbInfo;
    }

	/// <summary>
	/// 沙鲁，布欧
	/// </summary>
	public ClientBattleInfo(NewFinalTrialTeamParam argu, Statistic any, StaticsticList full, string video, short[] videoParse, ComboGather cbInfo) {
		gid    = argu.gid.ToString();
		doorId = argu.doorId;

		Statistics = any;
		show = full;

		this.video = video;
		this.videoparse = videoParse;
		combo = cbInfo;
	}

}

public class Statistic {
	//释放技能的数量
	public int skillcount;
	//技能释放的比例
	public float skillcountratio;
	//最大杀敌的数量
	public int maxkill;
	//杀到第几波敌人
	public int enemyDieIndex;

	//战斗评星
	public int star;
}

public class StaticsticList {
	//技能释放列表
	public int[] skillarr;
	//杀人列表
	public int[] killarry;
	public AW.Battle.BT_Logical.MaxKillOfWhoDid role;
}

/// <summary>
/// 沙鲁布欧的战斗结算的请求
/// </summary>
//gid			int		玩家角色ID
//doorId		int		关卡ID
//type		    int		类型 	1:沙鲁，2布欧
//sequence	    string	战斗序列
public class ClientBTShaBuParam : ClientBattleCheckParam {
	public int doorId;
	public int type;

	public ClientBTShaBuParam() { }

	public ClientBTShaBuParam(NewFinalTrialTeamParam param, Statistic any, StaticsticList full, string video, short[] videoParse, ComboGather combo) {
		gid    = param.gid.ToString();
		doorId = param.doorId;
		type   = param.type;

		sequence = new ClientBattleInfo(param, any, full, video, videoParse, combo);
	}
}


#endregion

#region 查看玩家远征状态
public class NewFinalTrialStateParam : BaseRequestParam
{
	public int gid;
	public NewFinalTrialStateParam(int _gid)
	{
		this.gid = _gid;
	}
}
#endregion

#region 查看当前层状态
public class NewFinalTrialCurDungeonParam : BaseRequestParam
{
	public int gid;
	public int doorId;  //关卡ID
	public int type;    //类型 	1:沙鲁，2布欧
	public NewFinalTrialCurDungeonParam(int _gid, int _doorId, int _type)
	{
		this.gid = _gid;
		this.doorId = _doorId;
		this.type = _type;
	}
}
#endregion

#region 获取守关队伍数据
public class NewFinalTrialTeamParam : BaseRequestParam
{
	public int gid;
	public int doorId;  //关卡ID
	public int type;    //类型 	1:沙鲁，2布欧

	public string teamSeq;			//战斗队伍顺序
	public int battleType;			//战斗类型
	public int[] cardIds;    //选择的卡牌ID []
	public int tid;   //所用队伍ID

	public NewFinalTrialTeamParam(int _gid, int _doorId, int _type, int[] _cardIds, int _tid)
	{
		this.gid = _gid;
		this.doorId = _doorId;
		this.type = _type;
		this.cardIds = _cardIds;
		this.tid = _tid;

		//初始化战斗队伍顺序
		if (type == 1)
		{
			battleType = 6;
		}
		else
		{
			battleType = 7;
		}
		BattleTeam bteam = Core.Data.battleTeamMgr.GetBattleTeam (battleType);
		if (bteam != null)
		{
			if (battleType == 6)
			{
				teamSeq = bteam.atkTeam;
			}
			else
			{
				teamSeq = bteam.defTeam;
			}
		}
	}
}
#endregion

public class RefreshSecretShopParam:BaseRequestParam{
    public int gid;
    public int no;
    public RefreshSecretShopParam(int tGid,int tNo){
        gid = tGid;
        no = tNo;
    }
}
#region 精力
public class sendJLData:BaseRequestParam
{
    public  int  gid    ; // 玩家角色ID
    public  int stone   ; //本次购买消耗的钻石
    public sendJLData(int id ,int stoneNum)
    {
        gid =id ; 
        stone =stoneNum ;
    }
}
#endregion

#region 激活码
public class ActivationCodeParam:BaseRequestParam
{
	public  int  gid    ; // 玩家角色ID
	public  string key   ; //激活码
	public ActivationCodeParam(int m_gid ,string m_key)
	{
		gid =m_gid ; 
		key =m_key ;
	}
}
#endregion

#region 同步pve数据
public class SyncPveParam : BaseRequestParam
{
	public  string  gid    ; // 玩家角色ID
	public SyncPveParam(string id)
	{
		gid = id;
	}
}
#endregion

#region 重置关卡挑战次数
public class ResetFloorParam : BaseRequestParam
{
	public string gid;		//t	玩家角色ID
	public int doorId;		//关卡ID

	public ResetFloorParam(string id, int dID)
	{
		gid = id;
		doorId = dID;
	}
}
#endregion

#region 重置关卡挑战次数
public class SyncMoneyParam : BaseRequestParam
{
	public string gid;		//t	玩家角色ID

	public SyncMoneyParam(string id)
	{
		gid = id;
	}
}
#endregion

#region 复仇
[Serializable]
class GetRevengeData : BaseRequestParam
{
	public string gid;
	public GetRevengeData(string playerID)
	{
		this.gid = playerID;
	}
}
#endregion

#region  沙鲁布欧重置
public class ResetFinalTrialValueParam : BaseRequestParam
{
	public string gid;
	public int type;
	public ResetFinalTrialValueParam(string gid, int type)
	{
		this.gid = gid;
		this.type = type;
	}
}
#endregion

#region 购买PVE活动副本次数(只有VIP才有这个资格)
[Serializable]
class BuyPVEActivityFB : BaseRequestParam
{
	public string gid;
	public int type;     //1：技能副本；2：战魂副本；3：经验副本；4：宝石副本
	public int sync;
	public BuyPVEActivityFB(string playerID,int type,int _sync)
	{
		this.gid = playerID;
		this.type = type;
		this.sync = _sync;
	}
}
#endregion

#region  Vip gift
public class GetVipLevelRewardParam : BaseRequestParam
{
	public int gid;
	public int viplvl;
	public GetVipLevelRewardParam(int gid, int viplvl)
	{
		this.gid = gid;
		this.viplvl = viplvl;
	}
}

#endregion

#region
public class GetActiveShopItemParam : BaseRequestParam
{
	public int gid;
	public GetActiveShopItemParam(int gid)
	{
		this.gid = gid;
	}
}
#endregion

#region
public class BuyActiveShopItemParam : BaseRequestParam
{
	public int gid;
	public int propid;
	public int nm;

	public BuyActiveShopItemParam()
	{

	}

	public BuyActiveShopItemParam(int gid, int propid, int nm)
	{
		this.gid = gid;
		this.propid = propid;
		this.nm = nm;
	}
}
#endregion

#region
public class RefreshZhangongShopParam : BaseRequestParam
{
	public int gid;
	public RefreshZhangongShopParam(int gid)
	{
		this.gid = gid;
	}
}
#endregion

#region
public class GetSysStatusParam : BaseRequestParam
{
	public int gid;
	public GetSysStatusParam(int gid)
	{
		this.gid = gid;
	}
}
#endregion


#region  限时活动请求
public class ActivityLimitTimeParam : BaseRequestParam
{
	public int gid;
	public ActivityLimitTimeParam(int gid)
	{
		this.gid = gid;
	}
}
#endregion

#region  限时活动请求
public class GetActivityLimitTimeParam : BaseRequestParam
{
	public int gid;
	public int actId;
	public int itemID;
	public GetActivityLimitTimeParam(int gid, int actId, int itemID)
	{
		this.gid = gid;
		this.actId = actId;
		this.itemID = itemID;
	}
}
#endregion

#region 龙珠银行 3008-3010

public class GetDragonBankParam :BaseRequestParam
{
	public string gid;
	public GetDragonBankParam(string gid){
		this.gid = gid;
	}
}

public class GetSaveMoneyParam:BaseRequestParam{
	public string gid;
	public int stone; 
	public GetSaveMoneyParam(string gid,int tStone){
		this.gid = gid;
		this.stone = tStone;
	}
}

#endregion



