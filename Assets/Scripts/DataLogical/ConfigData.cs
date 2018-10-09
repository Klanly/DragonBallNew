using System;
using System.Reflection;
using System.Collections.Generic;

#region 根据数据的ID来判定是何种数据
//1、宠物；2、技能；3、大关卡；4、装备；5、中关卡；6、小关卡；7、尾兽；8、建筑；9、缘；10、奖励池;11、道具;12、宝石；13、星级；14、性别；15、碎片
public enum ConfigDataType
{
	Default_No = 0x0,
	Monster = 0x1,
	Skill = 0x2,
	Chapter = 0x3,
	Equip = 0x4,
	City = 0x5,
	Floor = 0x6,
	HugeBeast = 0x7,
	Building = 0x8,
	Fate = 0x9,
	RewardPool = 0xA,
	Item = 0xB,
	Gems = 0xC,
	Star = 0xD,
	Gender = 0xE,
	Frag = 0xF,
}
#endregion
#region 性别
public enum Gender
{
	FEMALE = 0x0,
	MALE = 0x1,
	UNKNOW = 0x2,
}
#endregion
#region 配置文件的定义，决定如何读取（通常是设定读取的位置）
public enum ConfigType
{
	Monster,
	MonsterStar,
	Building,
	Chapter,
	City,
	Floor,
	PlayerInfo,
	Strings,
	HugeBeastWish,
	HugeBeastCb,
	Fate,
	Equipment,
	UpEquip,
	Gems,
	UpGems,
	Skill,
	SkillOP,
	SkillLv,
	Items,
	Activity,
	ZhanGongDuiHuanItem,
	VipGift,
	VipInfo,
	GoldDuiHuanItem,
	HonorItem,
	Message,
	FinalTrila,
	LvUpReward,
	SecretShop,
	MonsterLvExp,
	SoundConfig,
    VersionConfig,
	Guide,
	Soul,
    SensitiveData,
	UserHead,
	TreasureDesp,
    WorldBossReward,
	Task,
	PGSWAR,				//雷达组队站
	GuaGuaLe,
	RechargeInfo,
	NewChapter,
	NewFloor,
	Explore,				//特殊副本
	MapOfFinalTrial,
    BuyEnergy,
	DeblockingBuild,		
	ResetFloor,				//重置副本
}

public class HowToRead
{
	public ConfigType configType;
	//I will put it under StreamingAssets for demo, while i will put it under Documents for product.
	public string path;
	//Which data struct will be
	public Type format;

	public HowToRead(ConfigType type, string p, Type whichType)
	{
		configType = type;
		path = p;
		format = whichType;
	}
}

public static class Config
{
	public static Dictionary<ConfigType, HowToRead> LocalConfigs = new Dictionary<ConfigType, HowToRead>() { 
        { ConfigType.Monster,       new HowToRead(ConfigType.Monster,        "Config/Monster.bytes",        typeof(MonsterData)) }, 
		{ ConfigType.MonsterStar,   new HowToRead(ConfigType.MonsterStar,    "Config/Star.bytes",        	typeof(MonStarData)) }, 
		{ ConfigType.Chapter,       new HowToRead(ConfigType.Chapter,        "Config/ChapterData.bytes",    typeof(ChapterData)) }, 
		{ ConfigType.NewChapter,    new HowToRead(ConfigType.NewChapter,     "Config/NewChapterData.bytes", typeof(NewChapterData)) }, 
        { ConfigType.City,          new HowToRead(ConfigType.City,           "Config/CityData.bytes",       typeof(CityData)) }, 
        { ConfigType.Floor,         new HowToRead(ConfigType.Floor,          "Config/FloorData.bytes",      typeof(FloorData)) }, 
		{ ConfigType.Explore,     	new HowToRead(ConfigType.Explore,        "Config/ExploreData.bytes",    typeof(ExploreConfigData)) }, 
		{ ConfigType.NewFloor,      new HowToRead(ConfigType.NewFloor,       "Config/NewFloorData.bytes",   typeof(NewFloorData)) }, 
        { ConfigType.PlayerInfo,    new HowToRead(ConfigType.PlayerInfo,     "Config/UserLevel.bytes",      typeof(UserLevelInfo)) }, 
        { ConfigType.Strings,       new HowToRead(ConfigType.Strings,        "Config/Strings.bytes",        typeof(StringsData)) }, 
        { ConfigType.Fate,          new HowToRead(ConfigType.Fate,           "Config/Fate.bytes",           typeof(FateData)) }, 
        { ConfigType.Equipment,     new HowToRead(ConfigType.Equipment,      "Config/Equip.bytes",          typeof(EquipData)) }, 
        { ConfigType.UpEquip,       new HowToRead(ConfigType.UpEquip,        "Config/UpEquip.bytes",        typeof(UpEquipLevel)) }, 
        { ConfigType.Gems,          new HowToRead(ConfigType.Gems,           "Config/Gems.bytes",           typeof(GemData)) }, 
        { ConfigType.Skill,         new HowToRead(ConfigType.Skill,          "Config/Skill.bytes",          typeof(SkillData)) }, 
        { ConfigType.SkillOP,       new HowToRead(ConfigType.SkillOP,        "Config/SkillOP.bytes",        typeof(SkillOpData)) },
		{ ConfigType.SkillLv,       new HowToRead(ConfigType.SkillLv,        "Config/SkillUp.bytes",        typeof(SkillLvData)) },
		{ ConfigType.HugeBeastWish, new HowToRead(ConfigType.HugeBeastWish,  "Config/HBWish.bytes",         typeof(AoYiData)) }, 
        { ConfigType.HugeBeastCb,   new HowToRead(ConfigType.HugeBeastCb,    "Config/HugeBeastCb.bytes",    typeof(CombineHBData)) }, 
        { ConfigType.Items,         new HowToRead(ConfigType.Items,          "Config/Items.bytes",          typeof(ItemData)) }, 
        { ConfigType.Building,      new HowToRead(ConfigType.Building,       "Config/Building.bytes",       typeof(BaseBuildingData)) },
		{ ConfigType.ZhanGongDuiHuanItem, new HowToRead(ConfigType.ZhanGongDuiHuanItem, "Config/ZhanGongDuiHuanItem.bytes", typeof(DuiHuanItem)) }, 
        { ConfigType.VipGift,       new HowToRead(ConfigType.VipGift,       "Config/VIPGift.bytes",       typeof(VipGiftData)) }, 
        { ConfigType.VipInfo,       new HowToRead(ConfigType.VipInfo,       "Config/VIPInfo.bytes",       typeof(VipInfoData)) }, 
        { ConfigType.HonorItem,     new HowToRead(ConfigType.HonorItem,     "Config/worldbosschange.bytes",typeof(HonorItemData)) }, 
        { ConfigType.LvUpReward,    new HowToRead(ConfigType.LvUpReward,    "Config/LevelUpReward.bytes", typeof(LevelUpRewardData)) },
		{ ConfigType.Message,       new HowToRead(ConfigType.Message,       "Config/MailInfo.bytes",      typeof(MessageInfoData))},
		{ ConfigType.FinalTrila,    new HowToRead(ConfigType.FinalTrila,    "Config/FinalTrial.bytes",    typeof(FinalTrialDungoenData))},
		{ ConfigType.MonsterLvExp,  new HowToRead(ConfigType.MonsterLvExp,  "Config/MonsterLvExp.bytes",  typeof(MonsterLvExp))},
		{ ConfigType.SoundConfig,   new HowToRead(ConfigType.SoundConfig,   "Config/SoundData.bytes",     typeof(SoundData))},
        { ConfigType.SecretShop,    new HowToRead(ConfigType.SecretShop,    "Config/SecretShop.bytes",    typeof(SecretShopData))},
        { ConfigType.VersionConfig, new HowToRead(ConfigType.VersionConfig, "Config/VerRes.bytes",        typeof(VersionNumItem))},
#if NewGuide
		{ ConfigType.Guide,         new HowToRead(ConfigType.Guide,         "Config/NewGuide.bytes",         typeof(GuideData))},
#else
        { ConfigType.Guide,         new HowToRead(ConfigType.Guide,         "Config/Guide.bytes",         typeof(GuideData))},
#endif
		{ ConfigType.Soul,          new HowToRead(ConfigType.Soul,         	"Config/Fragment.bytes",      typeof(SoulData))},
        { ConfigType.SensitiveData, new HowToRead(ConfigType.SensitiveData, "Config/SensitiveData.bytes", typeof(SoulData))},
		{ ConfigType.UserHead, 		new HowToRead(ConfigType.UserHead, 		"Config/UserHead.bytes", 	  typeof(UserHeadData))},
		{ ConfigType.TreasureDesp, 	new HowToRead(ConfigType.TreasureDesp, 	"Config/Chest.bytes", 		  typeof(TreasureBoxDespData))},
		{ ConfigType.Task,  		new HowToRead(ConfigType.Task,  		"Config/Task.bytes",          typeof(TaskData))},
		{ ConfigType.PGSWAR,  		new HowToRead(ConfigType.PGSWAR,  		"Config/GPSWar.bytes",        typeof(GPSWarInfo))},
		{ ConfigType.WorldBossReward,new HowToRead(ConfigType.WorldBossReward,"Config/Worldbossreward.bytes",    typeof(WorldBossRewardData))},
		{ ConfigType.GuaGuaLe,      new HowToRead(ConfigType.GuaGuaLe,      "Config/guaguale.bytes",             typeof(GuaGuaLeData))},
		{ ConfigType.RechargeInfo,  new HowToRead(ConfigType.RechargeInfo,  "Config/Pay.bytes",                  typeof(RechargeData))},
		{ ConfigType.MapOfFinalTrial,new HowToRead(ConfigType.MapOfFinalTrial,"Config/shilian.bytes",            typeof(MapFinalTrialData))},
        { ConfigType.BuyEnergy,     new HowToRead(ConfigType.BuyEnergy,     "Config/BuyEnergy.bytes",            typeof(BuyEnergy))},
		{ ConfigType.DeblockingBuild,new HowToRead(ConfigType.DeblockingBuild,"Config/DeblockingBuild.bytes",    typeof(DeblockingBuildData))},
		{ ConfigType.ResetFloor,  	new HowToRead(ConfigType.ResetFloor,  	"Config/refreshtimes.bytes",         typeof(ResetFloor))},
	};

}
#endregion

public class BaseData {
	public int pid;
}

/// <summary>
/// 所有含有ID字段的数据都继承自UniqueBaseData
/// </summary>
[Serializable]
public class UniqueBaseData {
	//其实是Number的作用，
	public int ID;
}

[Serializable]
public class VersionNumItem {
    public string FileName;
    public int num;

    public VersionNumItem() { }

    public VersionNumItem(string fn, int ver) { 
        FileName = fn;
        num = ver;
    }
}

//声效的配置文件
public class SoundData : UniqueBaseData {
	public string name;
	public short type;
}

//这里的UniqueBaseData中的ID就是id
public class StringsData : UniqueBaseData {
	public string txt;
}

//宠物升级信息
[Serializable]
public class MonsterLvExp : UniqueBaseData {
	public int star1;
	public int star3;
	public int star5;
	public int star2;
	public int star6;
	public int equipLevel;
	public int star4;
}
//宠物的配置信息
[Serializable]
public class MonsterData : UniqueBaseData {

	public string name;
	public short star;
	public float atk;
	//攻击力成长值｛atk+atkGrowth*(玩家等级－1)＝当前攻击力｝
	public float atkGrowth;
	public float def;
	// 防御力成长值｛def+defGrowth*(玩家等级－1)＝当前防御力｝
	public float defGrowth;
	//defined at the top of class
	public short gender;

	//基础经验
	public int exp;

	//调用缘表中的ID。
	public int[] fateID;
	//一个宠物身上最多会有两个技能，第一个技能初始开启，第二个技能在宠物30级的时候开启, 第三个技能是终结技（使用气）,只要宠物有终结技，就开启.
	public int[] skill;

	//调用2D漫画的文件，以数组字符串形式呈现
	public int[] anime2D;
	public int[] anime3D;
	//描述
	public string description;

	//碎片个数
	public int Fragment;

	//是否可以升星
	public short jinjie;

	//是否可以合成
	public short hecheng;

	//转换为Gender枚举
	public Gender toGenderEnum {
		get {
			if(Enum.IsDefined(typeof(Gender), gender)) {
				return (Gender)gender;
			} else {
				return Gender.UNKNOW;
			}
		}
	}

    public int nuqi1;
	//用来设置怒气值
    public int nuqi2;

	public MonsterData () { }
}

//物品的配置信息
[Serializable]
public class ItemData : UniqueBaseData {

	public string name;
	//0、地球碎片 1、体力 2、精力 3、钥匙 4、金币兑换 5、潜力 6、龙珠 7、奖励效果加成 8、宠物 9、兑换钻石 
	//10、扭蛋 11、免战牌 12、宝石 13、幸运大奖 14、钻石 15、金币 16、属性切换 17、属性重置 18、装备道具 
	//19、装备道具包 20、碎片包 21、VIP礼包 22、娜美克碎片
	public short type;
	//在商店里的位置,可以出现在多个地方
	public short[] type2;

	//原始价格,一定是两位，第一位是0-金币,1-宝石，第二位是价格
	public int[] price;
	//VIP价格
	public int[] discount;

	//购买道具得到的物品
	public List<int[]> num;
	//使用道具得到的物品
	public List<int[]> num2;

	//星级
	public short star;
	//描述
	public string description;
	//显示限制
	public int[] Visible;
	//是否弹滑条的窗
	public int max;
	//显示图标ID
	public int iconID;

	public bool CanUse()
	{
		bool canUse = (num2 != null && num2.Count > 0) || 
			(type == 1 || type == 2 || type == 3 || type == 4 || type == 9);
			
		bool isSpecial = (ID == ItemManager.WUXINGWAN || ID == ItemManager.CHAOSHENSHUI);

		return (canUse || isSpecial);
	}
}

//物品的配置信息
[Serializable]
public class SoulData : UniqueBaseData 
{
	//名字
	public string name;
	//类型
	public int type;
	//描述
	public string description;
	//合成目标ID
	public int updateId;
	//合成所需个数
	public int quantity;
	//star
	public int star;
}

[Serializable]
public class FateData : UniqueBaseData {
	public const short EFFECT_SELF_ATTACK = 0x0;
	public const short EFFECT_SELF_DEFEND = 0x1;
	public const short EFFECT_ENEMY_ATTACK = 0x2;
	public const short EFFECT_ENEMY_DEFEND = 0x3;

	public const short Type_Pos = 0x0;
	public const short Item_ID_Pos = 0x1;
	public const short Item_Count_Pos = 0x2;

	public string name;
    public int nuqi;
	//填写与宠物有缘的物品ID。注：将本身的ID也要写在其中。
	//第二位开始，[ID,count]
	public List<int[]> itemID;

	//获取本身的缘ID
	public int WhoesFateId {
		get {
			return itemID[0].Value(0);
		}
	}

	//对UI的人来说，从0开始
	public int[] MyFate(int pos) {
		if(pos >= 0 && pos < CountOfCondition )
			return itemID[pos + 1];//对UI的人来说，从0开始,但实际上从1开始
		else 
			return new int[3];
	}

	public int CountOfCondition {
		get {
			return itemID.Count - 1;
		}
	}
	//以数组的形式呈现：[我攻,我防,敌攻,敌防]
	public int[] effect;

	public bool ImproveAttack {
		get {
			return effect.Value<int>(EFFECT_SELF_ATTACK) > 0;
		}
	}

	public bool ImproveDefend {
		get {
			return effect.Value<int>(EFFECT_SELF_DEFEND) > 0;
		}
	}

	public string description {
		get {
			string des = string.Empty;
			string tmp = string.Empty;

			ConfigDataType type = ConfigDataType.Default_No;

			int count = CountOfCondition;
			for(int i = 0; i < count; ++ i) {

				int[] one = MyFate(i);

				type = (ConfigDataType) one[Type_Pos];
				switch(type) {
				case ConfigDataType.HugeBeast:
					AoYiData hbwd = Core.Data.dragonManager.getAoYiData(one[Item_ID_Pos]);
					Utils.Assert(hbwd == null, "Fate Confige file may not saticify with HugeBeastWish Config file");

					if(string.IsNullOrEmpty(des))
						des = Core.Data.stringManager.getString(1007);
					tmp += "<" + hbwd.name + ">";
					break;
				case ConfigDataType.Gender:
					Utils.Assert(!Enum.IsDefined(typeof(Gender), one[Item_ID_Pos]), "Unknow Gender = " + one[Item_ID_Pos]);
					Utils.Assert(count != 1, "Gender can only be one.");

					Gender gender = (Gender)one[Item_ID_Pos];
					string strGender = string.Empty;
					if(gender == Gender.MALE) 
						strGender = "<" + Core.Data.stringManager.getString(4) + ">";
					else 
						strGender = "<" + Core.Data.stringManager.getString(5) + ">";

					int amount = one[Item_Count_Pos];

					des = Core.Data.stringManager.getString(1006);
					des = string.Format(des, amount, strGender);

					break;
				case ConfigDataType.Monster:
					MonsterData md = Core.Data.monManager.getMonsterByNum(one[Item_ID_Pos]);
					Utils.Assert(md == null, "Can't find Monster Data Config.");

					if(string.IsNullOrEmpty(des))
						des = Core.Data.stringManager.getString(1004);
					tmp += "<"+ md.name + ">";
					break;
				case ConfigDataType.Star:
					Utils.Assert(count != 1, "Star can only be one.");

					string strStar = "<" + one[Item_ID_Pos].ToString() + Core.Data.stringManager.getString(6) + ">";
					int amount2 = one[Item_Count_Pos];
					des = Core.Data.stringManager.getString(1006);
					des = string.Format(des, amount2, strStar);

					break;
				case ConfigDataType.Equip:
					EquipData ed = Core.Data.EquipManager.getEquipConfig(one[Item_ID_Pos]);
					Utils.Assert(ed == null, "Can't find Equipment Data Config.");

					if(string.IsNullOrEmpty(des))
						des = Core.Data.stringManager.getString(1005);

					tmp += "<" + ed.name + ">";
					break;
				}
			}

			//只有性别和星级类型特殊
			if(type != ConfigDataType.Gender && type != ConfigDataType.Star) {
				des = string.Format(des, tmp);
			} 


			//把效果字符串化
			if(effect != null) {
				if(effect[EFFECT_SELF_ATTACK] > 0)
					des += string.Format(Core.Data.stringManager.getString(1000), effect[EFFECT_SELF_ATTACK]) + ",";
				if(effect[EFFECT_SELF_DEFEND] > 0)
					des += string.Format(Core.Data.stringManager.getString(1001), effect[EFFECT_SELF_DEFEND].ToString()) + ",";
				if(effect[EFFECT_ENEMY_ATTACK] > 0)
					des += string.Format(Core.Data.stringManager.getString(1002), effect[EFFECT_ENEMY_ATTACK].ToString()) + ",";
				if(effect[EFFECT_ENEMY_DEFEND] > 0)
					des += string.Format(Core.Data.stringManager.getString(1003), effect[EFFECT_ENEMY_DEFEND].ToString()) + ",";

				des = des.Remove(des.Length - 1);
			}


			return des;
		}
	}

	#region Help Routine to get values
	public int SelfAttack {
		get { 
			return effect.Value<int>(EFFECT_SELF_ATTACK);
		}
	}

	public int SelfDefend {
		get {
			return effect.Value<int>(EFFECT_SELF_DEFEND);
		}
	}


	public int EnemyAttack {
		get {
			return effect.Value<int>(EFFECT_ENEMY_ATTACK);
		}
	}

	public int EnemyDefend {
		get {
			return effect.Value<int>(EFFECT_ENEMY_DEFEND);
		}
	}


	#endregion

	public FateData() { }
}


[Serializable]
public class UpEquipLevel {
	//装备等级
	public int equipLevel;
	//1星装备
	public int star1;
	//2星装备
	public int star2;
	//3星装备
	public int star3;
	//4星装备
	public int star4;
	//5星装备
	public int star5;
}

[Serializable]
public class EquipData : UniqueBaseData {
	public const short TYPE_ATTACK = 0;
	public const short TYPE_DEFEND = 1;

	//攻击，防御，技能触发几率
	public const int Effect_Attack_Pos = 0;
	public const int Effect_Defend_Pos = 1;
	public const int Effect_Skill_Pos = 2;

	public string name;
	public string description;
	//装备星级
	public short star;
	//装备每升一级所需经验。
	public int exp;
	//装备类型
	public short type;
	//基础攻击力
	public float atk;
	//攻击力成长值｛atk+atkGrowth*(玩家等级－1)＝当前攻击力｝
	public float atkGrowth;
	//基础防御力
	public float def;
	//防御力成长值｛def+defGrowth*(玩家等级－1)＝当前防御力｝
	public float defGrowth;
	//镶嵌上对应的宝石可获得的特殊效果[攻击，防御，技能触发几率]
	public int[] effect;

	//攻击
	public int GemsImproveAttack {
		get {
			return effect.Value<int>(Effect_Attack_Pos);
		}
	}

	//防御
	public int GemsImproveDef {
		get {
			return effect.Value<int>(Effect_Defend_Pos);
		}
	}

	//技能触发
	public int GemsImproveSkill {
		get {
			return effect.Value<int>(Effect_Skill_Pos);
		}
	}

	//宝石插槽数(上限)
	public int Mosaic;

	//ignore
	//public int drop;
	public EquipData () { }

}

[Serializable]
public class DragonLockData {
	//解锁条件类型：1、玩家等级；2、钻石数
	public const short PLAYER_LEVEL_TYPE = 0x01;
	public const short DIAMOND_TYPE = 0x02;

	// 解锁等级
	public int num;
	//解锁条件类型
	public short type;
	public short dragonSlot;
	public int price;
}

[Serializable]
public class UpDragonData {
	public short dragonLevel;
	//(该规则可能不再使用：界面上则以这个基数exp＊50显示升到本级所需要的经验)
	public int exp;
}

[Serializable]
/// <summary>
/// 该类是HugeBeastLockData 和 UpHugeBeastData的混合类
/// </summary>
public class CombineHBData
{
	public int num;
	//解锁条件类型
	public short type;
	public short dragonSlot;
	// **********************************
	public short dragonLevel;
	//(该规则可能不再使用：界面上则以这个基数exp＊50显示升到本级所需要的经验)
	public int exp;
	public int price;

	public DragonLockData toDragonLockData()
	{
		if(dragonLevel == 0)
		{
			DragonLockData to = new DragonLockData();
			to.num = num;
			to.type = type;
			to.dragonSlot = dragonSlot;
			to.price = price;
			return to;
		}
		else return null;
	}

	public UpDragonData toUpDragonData()
	{
		if(dragonLevel > 0) 
		{
			UpDragonData to = new UpDragonData();
			to.dragonLevel = dragonLevel;
			to.exp = exp;
			return to;
		}
		else return null;
	}
}

[Serializable]
public class AoYiData : UniqueBaseData {
	public const short EXP_FACTOR = 0x32;

	public const short DRAGON_EARTH = 0x01;
	public const short DRAGON_NAMEIKE = 0x02;

	public string name;
	//可以提升的最大等级
	public int maxLevel;
	//每级所需的升级次数, (该规则可能不再使用：界面上则以这个基数exp＊50显示升到本级所需要的经验)
	public int[] exp;
	//每级的技能效果，以数组的形式呈现。[参数1,参数2,参数3,参数4,参数5,参数6, 参数7, 参数8, 参数9]
	public List<float[]> effect;
    //第一个效果
    public float[] ef_first;

    public float[] full_effect(int index) {

        float[] full = null;
        if(effect != null && index < effect.Count && effect.Count > 0) {
            int secEff_length = effect[index].Length;

            full = new float[secEff_length + 1];
            full[0] = ef_first[index];

            for(int i = 0; i < secEff_length; ++ i) {
                full[i + 1] = effect[index][i];
            }
        } else {
            full = new float[1];
            full[0] = ef_first[0];
        }

        return full;
    }

	//概括的介绍
	public string info;
	//故事性介绍
	public string description;

	public int skillID;
	//解锁等级（这里指的是神龙的等级）
	public int unlockLevel;
	//地球神龙愿望1，那美克星神龙愿望2
	public short dragonType;

	//1 自我增强 0 抑制敌人
	public short enhanced;
    //增强的技能中的哪些参数
    public string[] efinfo;
    //奥义增强的前半部分的效果. 具体参考BT_AoYi
    public int type;

    //奥义加成的基础值
    public float basic;
    //奥义加成的随等级的加成值
    public float growth;

	public AoYiData () { }
}


public class DungeonsData {
	//definition of status
	public const short STATUS_CLEAR = 0x1;
	public const short STATUS_NEW = 0x2;
	public const short STATUS_ONGOING = 0x3;

	public short status;
}



#region ————####  新副本  ####——————
//新副本章节配表数据
[Serializable]
public class NewChapterData : UniqueBaseData
{
	public string name;
	
	public int[] floorID;
	
	public string mapID;
	
	public string pathMapID;
	
	public float[] MapColor;
	
	public float[] pathPosition;
		
	public NewChapterData() { }
}

//新副本小关数据配表数据
[Serializable]
public class  NewFloorData : UniqueBaseData
{
	public string name;
	
	public float[] Pos;
	
	public short isBoss; 
	
	public string[] TextrueID;
	
	public float[] MapColor;
	
	public short FightType;
	
	public List<int[]> Reward;
	
	public int NeedEnergy;
	
	public string Des;
	
	public short Scence;
	
	public int TeamSize;

	public int Unlock;			//解锁等级
	public int Sweep;			//扫荡条件
	public int times;			//扫荡次数
	
	public string[] cartoon; 	// 漫画

	public int Cprice;          // 单次击打的金币

	public NewFloorData() { }
}


//特殊副本配表数据
[Serializable]
public class  ExploreConfigData : UniqueBaseData
{
	public string opendayDesp;	//开放日期描述
	public int openfloor;			//开放关卡

	public ExploreConfigData() { }
}

#endregion

//大关数据
[Serializable]
public class ChapterData : UniqueBaseData {
	public string name;
	//此大关中包含的所有中关ID，以数组的形式呈现。
	public int[] cityID;

	//may be contain chapter image name
	public ChapterData() { }
}

//中关数据
[Serializable]
public class CityData : UniqueBaseData  {
	public string name;
	//此中关中包含的所有小关ID，以数组的形式呈现。
	public int[] floorID;

	public CityData() { }
	
}

//小关数据
[Serializable]
public class  FloorData : UniqueBaseData{
	//definition of boss关卡
	public const short ISNOT_BOSS = 0x00;
	public const short IS_BOSS = 0x01;

	//definition of 特殊奖励
	public const short DOESNOT_HAVE_SEPCIAL_REWARD = 0x00;
	public const short HAS_SPECIAL_REWARD = 0x01;

	//definition of boss information in the array position (zero based)
	public const short BOSS_ID_POS = 0x00;
	public const short BOSS_LEVEL_POS = 0x01;
	public const short BOSS_EQUIP1_ID_POS = 0x02;
	public const short BOSS_EQUIP1_LEVEL_POS = 0x03;

	public const short REWARD_EXP = 0x1;
	public const short REWARD_COIN = 0x2;

	public string name;
	//每个小关需要完成的次数
	public short wave;
	//每关需要消耗的能量
	public short needeEnergy;
	//每关的故事
	public string description;

	//采用的奖励池ID。
	public int[] reward;

	//完成此关后可获得的玩家经验。
	public int exp {
		get {
			return reward.Value<int>(REWARD_EXP);
		}
	}
	//完成此关后玩家可获得的金币数。
	public int coin {
		get {
			return reward.Value<int>(REWARD_COIN);
		}
	}

    //场景ID
    public int cj;

	//攻防位置  -1代表没意义，1 = 攻击, 0防御
	public int gf;

	//是否是boss关卡
	public short isBoss;

	//角色ID,角色等级, 角色装备1ID,角色装备1等级
	public List<int[]> boss;

	// ignore 角色装备2ID,角色装备2等级

	#region boss informatino

	public int bossID (int pos) {
		if(isBoss == IS_BOSS)
			return boss[pos].Value<int>(BOSS_ID_POS);
		else 
			return 0;
	}

	public int bossLevel(int pos) {
		if(isBoss == IS_BOSS)
			return boss[pos].Value<int>(BOSS_LEVEL_POS);
		else
			return 0;
	}

	public int bossEquip1ID(int pos) {
		if(isBoss == IS_BOSS)
			return boss[pos].Value<int>(BOSS_EQUIP1_ID_POS);
		else
			return 0;
	}

	public int bossEquip1Level(int pos) {
		if(isBoss == IS_BOSS)
			return boss[pos].Value<int>(BOSS_EQUIP1_LEVEL_POS);
		else
			return 0;
	}

	//Ignore, Requirement is changed.
	public int bossEquip2ID (int pos) {
		if(isBoss == IS_BOSS)
			return 0;
		else
			return 0;
	}
	//Ignore, Requirement is changed.
	public int bossEquip2Level (int pos) {
		if(isBoss == IS_BOSS)
			return 0;
		else
			return 0;
	}

	#endregion

	//推荐战力
	public int battlePoint;
	//免费战斗次
	public short laveTime;
	//漫画名称
	//Usually, array of image2D contains the very one element.
	public string[] image2D;
	
	//Boss关的免费次数(会随VIP的等级变化而变化)
	public int BossLaveTime
	{
		get
		{
			return Core.Data.vipManager.GetVipInfoData(Core.Data.playerManager.curVipLv).freeBossChallenge;
		}		
	}
	
	
	
	/*特殊奖励(几率掉落)  add by jc
	 * */
	public int specialRewardID;
	
	public string CityImage {
		get {
			return image2D.Value<string>(0);
		}
	}

	public FloorData() { }
}

//奖励池
[Serializable]
public class RewardPool : UniqueBaseData {
	//奖池中的物品ID
	public int itemID;
	//奖品权重。
	public int weight;
	// 奖品数量。
	public short num;

	public RewardPool() { }
}

//角色升级数据
[Serializable]
public class UserLevelInfo {

	//Position of LevelReward
	public const short ITEM_ID_POS = 0x0;
	public const short ITEM_NUM_POS = 0x1;

	public int level;
	//玩家升到下一级所需要的经验
	public int exp;
	//当前等级精力上限。（可以被突破）
	public int maxEnergy;
	//当前等级体力上限。（是否可以被突破要再看）
	public int maxStamina;
	//当前等级最多能修的建筑。
	public short maxBuilding;
	//当前等级能同时上阵的宠物数量。
	public short petSlot;
	// 等级奖励，数组形式[物品ID,物品数量]
	public int[] levelReward;
	//升级后的效果描述
	public string levelTips;

	public string[] Unlock;

	public string[] icon;

	public int RewardCount {
		get {
			if(levelReward != null && levelReward.Length > 0) {
				return levelReward.Length / 2;
			} else 
				return 0;
		}
	}

	/*
	 * 数组形式[物品ID,物品数量]
	 */ 
	public int[] GetReward(int pos) {
		int[] reward = null;

		if(pos < RewardCount) {
			reward = new int[2];
			reward[ ITEM_ID_POS ] = levelReward[pos * 2 + ITEM_ID_POS];
			reward[ ITEM_NUM_POS ] = levelReward[pos * 2 + ITEM_NUM_POS];
		}

		return reward;
	}

	public UserLevelInfo() { }

}

//建筑数据
[Serializable]
public class BaseBuildingData : UniqueBaseData 
{
	//不是真正的ID,
	public string ID;
	//真正的ID
	public int id;

	//作战建筑
	public const int BUILD_KIND_BATTLE = 81;
	//普通建筑类型
	public const int BUILD_KIND_NORMAL = 82;
	//产出建筑
	public const int BUILD_KIND_PRODUCE = 83;

	public const int BUILD_BATTLE = 810001;		//作战建筑
	public const int BUILD_PRODUCT = 830001;	//产出建筑

	public const int BUILD_ZHAOMU = 820002;		//招募屋
	public const int BUILD_XUNLIAN = 820001;	//训练屋
	public const int BUILD_YELIAN = 820003;		//冶炼屋
	public const int BUILD_TREE = 820004;		//生命树
	public const int BUILD_SHOP = 820005;		//神秘商店
	public const int Q_phD = 820006;			//Q博士
	public const int BUILD_MailBox = 820007; 	//邮箱
	public const int BUILD_FUBEN = 820008; 	//副本
	public const int BUILD_CHALLENGE = 820009; 	//挑战
	public const int BUILD_FRAGMENT = 820010; 	//图鉴

	//建筑物的等级
	public short Lv;
	public string name;
	//这个是不完整的
	public string description;


	//升级消耗金币
	public int[] up_cost;
	//开启作战建筑消耗金币
	public int[]  start_cost;
	//玩家可修建的等级限制
	public int limitLevel;
	//建筑冷却时间
	public long time;
	public int[] force_gain;

	//建筑类型  普通 作战 产出
	public int build_kind
	{
		get 
		{
			return id / 10000;
		}
	}

	//[金币收益,钻石收益]（经济建筑，长度是2）
	//[攻击，防御，触发率]（作战建筑，长度是3）
	public long[] effect;



	//产出建筑得到的金币
	public long GetCoin
	{
		get
		{
			return effect [0];
		}
	}

	//产出建筑得到的钻石
	public long GetStone
	{
		get
		{
			return effect [1];
		}
	}

	public long GetAtk{
		get{ 
			return effect [0];
		}
	}
	public long GetDef{
		get{ 
			return effect [1];
		}
	}

	public long GetRate{
		get{ 
			return effect [2];
		}
	}




	//升级消耗金币
	public int UpCostCoin
	{
		get
		{
			return up_cost [0];
		}
	}
	//升级消耗钻石
	public int UpCostStone
	{
		get
		{
			return up_cost [1];
		}
	}

	//开启作战建筑花费金币
	public int OpenBattleCostCoin
	{
		get
		{
			return start_cost [0];
		}
	}

	//开启作战建筑消耗钻石
	public int OpenBattleCostStone
	{
		get
		{
			return start_cost [1];
		}
	}
	//	需要道具 id    110182  ，3，4     小 中 大
	public int OpenProduceCostItem{
		get{
			return start_cost[2];
		}
	}


	//需要 道具 数量
	public int OpenProduceCostItemNum{
		get{ 
			return start_cost [3];
		}
	}

	public int ForceGainCost{
		get{ 
			if (force_gain != null && force_gain.Length != 0) {
				for (int i = 0; i < force_gain.Length; i++) {
					if (force_gain [i] != 0) {

						return force_gain [i];
					}
				}
			}
			return 0;
		}
	}



	public BaseBuildingData() { }


}

[Serializable]
public class FeatureBuildingData {

	public BaseBuildingData config;

	private const short SELF_NINJIA_IMP = 0x0;
	private const short ENEMY_NINJIA_DEC = 0x1;
	private const short SELF_ROB_IMP = 0x2;
	private const short ENEMY_ROB_DEC = 0x3;

	//effect[]
	//建筑每一级的效果
	//[提高己方忍术触发率、降低敌方忍术触发率、提升己方抢夺成功率、降低地方抢夺成功率]

	#region help routine

	public int IncSelfNinia {
		get {
			return (int)config.effect.Value<long>(SELF_NINJIA_IMP);
		}
	}


	#endregion

	public FeatureBuildingData() { }
}

[Serializable]
public class GemData : UniqueBaseData {
	public string name;
	public string description;
	public short star;
	public int level;
	public int atk;
	public int def;
	public float skillEffect;
	public int target;
	public short color;
	public string anime2D;
	public int price;
	public int coin;
	public float probability;
	public int stone;
	
	public SlotOrGemsColor getColor {
		get {			
	        SlotOrGemsColor clr = (SlotOrGemsColor) color;
			if(Enum.IsDefined(typeof(SlotOrGemsColor), clr))
				return clr;
			else
				RED.LogWarning("color is out of range !!!!" + color);
				return SlotOrGemsColor.Default_No;
		}
	}
}

public class GuideData : UniqueBaseData
{
	public int NeedLevel;
	public int ArrowDir;
	public string Dialogue;
	public int MaskType;
	public float MaskX;
	public float MaskY;
	public float ZoomX;
	public float ZoomY;
	public int Type;
	public int Operation;
	public int TaskID;
	public int AutoNext;
	public int Sound;
	public int RenderMask;
	public string RoleName;
	public int RoleDir;
	public int RequestServer;
	public string JumpStep;
	public List<int[]> Multi;
	public int MultiIndex;
	public int AdapterToSceen;

	public int RoleID;
}

[Serializable]
public class SkillOpData : UniqueBaseData {

	//怒气技能
	public const short Anger_Skill = 0;
	//常规技能
	public const short Common_Skill = 1;
	//濒死技能
	public const short Death_Closed_Skill = 2;
	//战后技能
	public const short After_War_Skill = 3;

	public string mark;
	public string name;
	//怒气技能 常规技能 濒死技能 战后技能
	public short type;
	//优先级
	public short prior;
    //奥义增强
    public string[] enhanced;
}
	
//技能升级数据
[Serializable]
public class SkillLvData : UniqueBaseData 
{
	public int max_lv;				//最大等级
	public int[] cost_gold;			//消耗金币
	public int[] cost_juanzhou;		//消耗卷轴

	public float[] gailv;			//
	public float[] rate;
	public float[] rate2;
	public float[] add;
	public float[] damage;
	public float[] num;
	public float[] dec;
	public float[] seal;
	public float[] absorb;
	public float[] nuqi;


	public List<string> GetUpParam(int level)
	{
		//need to modify
		List<string> list = new List<string>();
		list.Add(Core.Data.stringManager.getString(5233));
		return list;
	}
}


//活动数据类
[Serializable]
public class ActData:UniqueBaseData{
    public int rank;
    public long beginTime;
    public long endTime;
    public int actId;
    public string name;
}


[Serializable]
public class skParam 
{
	public int rate2;
	public int rate;
	public int gailv;
	public int damage;
	public int nuqi;
	public int num;
	public int add;
	public int dec;
	public int seal;
	public int absorb;
}

[Serializable]
public class SkillData : UniqueBaseData {
	public string mark;
	public string name;
	// 等级的换算公式
	//（0-D,1-C,2-B,3-A,4-S）
	public string level;
	public int op;
	public skParam param;

    public int Icon;
	/// <summary>
	/// 把技能的字符的等级 转换 数字等级
	/// </summary>
	/// <value>The number level.</value>
	public int numLevel {
		get {

			int num = 0;
			switch(level) {
			case "D":
				num = 0;
				break;
			case "C":
				num = 1;
				break;
			case "B":
				num = 2;
				break;
			case "A":
				num = 3;
				break;
			case "S":
				num = 4;
				break;
			}

			return num;
		}
	}

}

[Serializable]
public class DuiHuanItem : UniqueBaseData
{
	// 
	public int plistID;

	// item 文件中得 ID
	public int pid;

	// 花费的战功
	public int zgprice;

	// 花费的掠夺积分
	public int jfprice;

	// 花费的金币数量
	public int goldprice;

	// 一次性购买的数量
	public int num;

	// 兑换物品描述
	public string des;

	// 战功兑换物品时 当人物第一次到该排行 -1为该值 不参与计算
	public int rank;

	// 当天兑换次数
	public int dayduihuantotal;

}

[Serializable]
//Vip Data
public class VipInfoData : UniqueBaseData
{
	// vip等级
	public int vipLv;

	// 每日1快尾兽碎片
	public int everydaySplits;

	// 超忍招募每日免费次数
	public int freeSuperHR;

	// 究极试炼次数
	public int bloodshaluBattle;

	public int bloodbuouBattle;

	// 建筑升级不受等级限制
	public int unlockBuildingLevel;

	// 礼物
//	public int present;

	// 尾兽升级不受到祭坛等级限制
	public int unlockPetLevel;

	// 等级解锁
	public int freeLevel;

	// 描述
	public string tips;

	// 宿敌上限
	public int enemyLimit;

	// 刮刮卡免费次数
	public int freeGuagua;

	// 好友上限
	public int friendLimit;

	// 体力道具使用上限
	public int staminaItemLimit;

	// 付费开启金额
	public int rmb;

	// 精力道具使用上限
	public int energyItemLimit;
	

	// 排名战日免费次数
	public int freeChallenges;

	// 
	public int freeBossChallenge;

	//聊天次数
	public int worldchat;

    //精力购买上线
    public int buy ; 

	//副本重置次数
	public int starttimes;

	//领奖的物品
	public List<int[]> reward;

	//技能副本额外购买次数
	public int specialdoor1;
	//战魂副本额外购买次数
	public int specialdoor2;
	//经验副本额外购买次数
	public int specialdoor3;
	//宝石副本额外购买次数
	public int specialdoor4;
	//"扫荡解锁（0未解锁，1是解锁）"
	public int raidunlock;
	//加速 0 关闭， 1 开启
	public short speedup;
	//IQ博士是否开启
	public int iqshow;

	public int pvptype1; // dragonball

	public int pvptype2; // tianxia

	public int pvptype3; // grab gold

	public int pvptype4; //revenge

	public int expeditionreset;// 沙鲁布欧重置次数
	//银行存入
	public int bankmin;

	public int bankmax;
}

[Serializable]
//VipGift Data
public class VipGiftData : UniqueBaseData
{
	public string name;
	public string desc;

}

public static class ArrayExtension
{
	public static T Value<T>(this T[] array, int pos) {
		if(array != null && array.Length > pos ) {
			return array[pos];
		} else {
			return default(T);
		}
	}

	public static bool IsNullOrEmpty (this System.Array array) {
		if(array != null && array.Length > 0)
			return false;
		else 
			return true;
	}

}

[Serializable]
public class HonorItemData:UniqueBaseData{
	public int id;
	public int index;
	public int count;
	public int cost;
	public string name;

}

#region Message
[Serializable]
public class MessageInfoData : UniqueBaseData
{
	// id
	public int id;
	// News
	public string News;

	//type
	public int type;
	//type2
	public int type2;

}
#endregion

#region 等级奖励
public class LvItem{
	public int id;
	public int num;
}


[Serializable]
public class LevelUpRewardData : UniqueBaseData
{
	public int level;
	public List<LvItem[]> tLvReward;
	public List<int[]> reward;
	public LevelUpRewardData (){
		//			rewards = new List<int[]>();
		//			tLvReward = new List<LvItem[]>();
		//
		//		for(int i=0;i<rewards.Count;i++){
		//			List<LvItem> ItemList = new List<LvItem>();
		//			for (int j = 0; j < 3; j++) {
		//				LvItem tItem = new LvItem ();
		//				tItem.id = rewards [j] [0];
		//				tItem.num = rewards[j][1];
		//				tLvReward [i] [j] = tItem;
		//				ItemList.Add(tItem);
		//				RED.Log(tItem.id + " num " + tItem.num);
		//			}
		//			tLvReward.Add(ItemList.ToArray());
		//		}
	}
}
#endregion


#region FinalTrial
[Serializable]
public class FinalTrialDungoenData : UniqueBaseData
{
	// id
	public int id;

	// basePlus
	public float[] basePlus;

	//lastDayPlus
	public float[] lastDayPlus;

	// eCounts
	//    public List<List<int[]>> eCounts;

	// groups
	//    public List<int[]> groups;

	//rewards
	public List<int[]> rewards;

	//Double
	public int Double;

}
#endregion

#region FinalTrial
[Serializable]
public class SecretShopData : UniqueBaseData
{
	// id
	public int id2;
	
	// basePlus
	public int id;
	
	//lastDayPlus
	public int quantity;
	
	public int limit;

	public int type;

	public int[] money;
	
}
#endregion

[Serializable]
public class UpGemsData //: UniqueBaseData
{
	public float probability;
	
	public int level;
	
	public int coin;
}


#region UserHead
[Serializable]
public class UserHeadData : UniqueBaseData
{
	// 
	public int id;

	public int VIPLv;

	public int type;

	public int lv;

	public int Batter;
	
	public int Gamble;
}
#endregion

#region Treasurebox desp  
[Serializable]

public class TreasureBoxDespData:UniqueBaseData{
	public int Double;
	public int Gem;
	public int Prizepool;
	public int id;
	public int[] Show;
	public string chest;
}
#endregion
#region Treasurebox desp  
[Serializable]
public class WorldBossRewardData:UniqueBaseData{
    public int id;//排位
    public int multiple;//加成
    public int credit1;//失败获得名望
    public string rank;//排行队伍
    public int credit;//胜利获得名望

}
#endregion

#region Task add by jc

public class TaskData:UniqueBaseData
{
	/*
	 *0:打副本；         
	  *1:排行榜；      
	  *2:沙鲁游戏 
	  *3:布欧游戏  
	  *4:抢夺战      
	  *5:抢龙珠  
	  *6:赌博     
	  *7:共享录像   
	  *8:收获经济建筑
	  *9:开启战斗建筑
	  *10:打蓝牙本  
	  *11:完成当天所有每日任务   
	  *12:打通副本；
	  *13:强化武者；
	  *14:强化武器；
	  *15:强化防具；
	  *16:获得5星武者
	  *17:获得6星武者
	  *18获得觉武者 
	   *19:获得神武者
	 * */
    public int TASKTYPE;
	public int[]  LEVEL;
	public int Count;
//	public int Reward_ItemID;
//	public int Reward_ItemCount;
	public int Type;
	public string Title;
	public string Content;
	public string Require;
	public int Reward_Type;
	public int Progress;
	
	//服务器传
	public int curProgress;
	
	public List<int[]> Reward_ItemID;	
}
#endregion

#region
public class GuaGuaLeData : UniqueBaseData
{
	public int MonsterId;
	public int Stone;
}
#endregion

#region
public class RechargeData : UniqueBaseData
{
	//描述
	public string Describe;
	//额外描述
	public string Describe1;
	//图标ID
	public string Iconid;
	//是否上架
	public int Sell;
	//推荐
	public int Recommend;
	//获得钻石数
	public int[] Diamond;
	//非翻倍购买奖励
	public List<int[]> Present1;
	//标题
	public string Title;
	//价格（元）
	public int Price;
	//购买次数
	public int Double;
	//翻倍购买奖励
	public List<int[]> Present2;
	//类型
	public int Type;
	//商店排序
	public int Rank;

	//已经购买次数(从服务器获取)
	public int buyCount = 0;


	// ------专门给黑桃使用 ----
	public string SpadeName;
	public string SpadeDes;
}
#endregion

#region 沙鲁布欧地图信息
public class MapFinalTrialData : UniqueBaseData
{
	//关卡名
	public string name;
	//描述
	public string Des;
	//坐标
	public List<int> Pos;
	//贴图ID
	public string TextrueID;
	//奖励
	public List<int[]> Reward;
	//奖池
	public List<int> RewardExt;
	//战斗类型0:攻 1:防
	public int FightType;
	//战斗场景
	public int Scence;
	//次数购买价格
	public List<int> Price;

	public int members;

	public int Cprice;//每次连击单价

}
#endregion

#region 武者星级
public class MonStarData : UniqueBaseData
{
	public float[] up_AtkParam;		//升星攻击加成参数
	public float[] up_DefParam;		//升星防御加成参数
	public int[] up_CoinCost;		//升星消耗金币
	public int[] up_SoulCost;		//升星消耗战魂

	public int[] resolve_SoulGet;	//分解得到战魂
	public int[] resolve_CoinCost;	//分解消耗金币
}
#endregion
#region 精力购买配表
public class BuyEnergy : UniqueBaseData
{
    public int num ; 
    public int cost_D;
}
#endregion

#region 副本重置购买配表
public class ResetFloor : UniqueBaseData
{
	public int cost_D;
}
#endregion

#region 解锁功能配表
public class DeblockingBuildData : UniqueBaseData
{
	public int num ; 
	public string[] icon;
	public string[] name;
	public int type;
	public string deblockingType;
}
#endregion