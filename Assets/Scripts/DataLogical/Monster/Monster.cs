using System;
using System.Collections.Generic;

/*
 * Monster have unique PID 
 * 这里我会定义一些动态的值，以供战斗时的计算，这些值是会变动的都是.数据的定义. 
 * 同时考虑反破解的数据定义,具体的逻辑请参考PrivateMemoryLayout
 * 
 * Allen 
 */ 

public class RuntimeMonster {
	//当前的属性值
	public MonsterAttribute Attribute;

	//当前属性  金木水火土
	public int m_nAttr
	{
		get
		{
			int attr = (int) Attribute;
			attr = attr % 10;
			return attr;
		}
	}

	//当前阶段  1普通  2真 3神
	public int m_nStage
	{
		get
		{
			int attr = (int) Attribute;
			int stage = 0;
			if(attr > 0 && attr <= 5)
			{
				stage = 1;
			}
			else if(attr == 10)
			{
				stage =  3;
			}
			else if(attr > 10 && attr <= 15)
			{
				stage =  2;
			}
			return stage;
		}
	}

	//进化增长的星级（不是真正的星级）
	public Int32Fog addStar;
	//当前的经验值
	public Int32Fog curExp;
	//定义一个基础的数据，不会经常变动
	public Int32Fog curLevel;
	//通常查克拉丸 会加成攻击和血量，防御和血量
	public Int32Fog ChaKeLa_Attck;
	public Int32Fog ChaKeLa_Defend;

	//使用过的潜力值
	public Int32Fog uspt;

	//一点查克拉丸能变为多少的攻击力和防御力
	public const short ATTACK_FACTOR = 1;
	public const short DEFEND_FACTOR = 1;

	//普通  真  神
	public const short NORMAL_MONSTER = 1;
	public const short ZHEN_MONSTER = 2;
	public const short SHEN_MONSTER = 3;
}


//技能等级
public class SkillLevel
{
	public int skillId;
	public int skillLevel;
}

/*
 * 战斗时候的运算数据
 */ 
public class BattleMonster {
	//定义一个合成的数据，
	//以攻击威力：CombieAttack = atk + atkGrowth * (宠物等级－1) + 查克拉丸的加值属性
	public Int32Fog CombieAttack;
	public Int32Fog CombieDefend;
	//血量上限
	//MaxHp = BaseHp * 技能的加成 * 尾兽的加成
	public Int32Fog MaxHp;

	//当前战斗力
	public Int32Fog Gs;

	//Dynamic Values
	//计算时候会随时更新的
	public float curAttack;
	public float curDefend;

	public BattleMonster(int baseAtt, int enhanceAtt, int baseDef, int enhanceDef) {
		CombieAttack = baseAtt + enhanceAtt;
		CombieDefend = baseDef + enhanceDef;
		//useless or ignore it
	}
}

/*
 * 添加所有的操作
 */ 
public class Monster : BaseData 
{
	//是否新增的
	public bool isNew;
	//是否在队伍中
	public bool inTeam;
	public int num;
	//default monster PID = 0;
	public MonsterData config;
	//Runtime data: from server
	public RuntimeMonster RTData;
	//Battle data - flash vars. when battle is starting
	public BattleMonster BTData;
	//技能等级
	public SkillLevel[]	skillLvs;


	//总潜力值
	public int totalQianli
	{
		get 
		{
			float atk = config.atk + config.atkGrowth * (RTData.curLevel - 1);
			float def = config.def + config.defGrowth * (RTData.curLevel - 1);
			if (RTData.addStar > 0) {
				float tUp_atk = Core.Data.monManager.GetMonUpAtkParam (config.star,Star) ;
				atk *= tUp_atk;
				float tUp_def = Core.Data.monManager.GetMonUpDefParam (config.star,Star) ;
				def *= tUp_def;
			}
			return (int)((atk + def) * 0.6f - 0.5f);
		}
	}
	//当前潜力值
	public int curQianli
	{
		get 
		{
			int left = totalQianli - RTData.uspt;
            MathHelper.KeepCreateZero(ref left);
			return left;
		}
	}

	// 真正的查克拉攻击值
	public int curChakala_Atk
	{
		get
		{
			int atk = 0;

			if(RTData.uspt > totalQianli)
			{
				if(RTData.uspt != 0)
				{
					float rate = RTData.ChaKeLa_Attck / RTData.uspt;
					atk = (int)(totalQianli * rate);
				}
			}
			else
			{
				return (int)RTData.ChaKeLa_Attck;
			}
			return atk;
		}
	}

	//真正的查克拉防御值
	public int curChakala_Def
	{
		get
		{
			int def = 0;
			if(RTData.uspt > totalQianli)
			{
				if(RTData.uspt != 0)
				{
					float rate = RTData.ChaKeLa_Defend / RTData.uspt;
					def = (int)(totalQianli * rate);
				}
			}
			else
			{
				return RTData.ChaKeLa_Defend;
			}
			return def;
		}
	}

	//default constructor
	public Monster() 
	{
		isNew = true;
		inTeam = false;
	}


	public void InitConfig()
	{
		config = Core.Data.monManager.getMonsterByNum(num);
		if(config == null) 
		{
			RED.LogWarning(num + "config file not find!");
			return;
		}

		skillLvs = new SkillLevel[config.skill.Length];
		for (int i = 0; i < skillLvs.Length; i++)
		{
			skillLvs [i] = new SkillLevel ();
			skillLvs [i].skillId = config.skill [i];
			skillLvs [i].skillLevel = 1;
		}
	}

	public Monster(int pid, int num, RuntimeMonster RTData, MonsterData config) {

		this.pid = pid;
		this.num = num;
		this.RTData = RTData;

		InitConfig();

		this.inTeam = false;
		this.isNew = true;
		//create the 战斗时候的运算数据对象
		this.BTData = new BattleMonster(baseAttack, enhanceAttack, baseDefend, enhanceDefend);

	}

	/// <summary>
	/// Gets the star.真正的星级
	/// </summary>
	/// <value>The star.</value>
	public short Star {
		get {
			if (RTData != null)
				return (short)(RTData.addStar + config.star);
			else
				return 0;
		}
	}

	/// <summary>
	/// Gets the gender. 返回性别（默认男性)
	/// </summary>
	/// <value>The gender.</value>
	public Gender Gender {
		get {
			if(config != null)
				return config.toGenderEnum;
			else
				return Gender.MALE;
		}
	}


	#region 考虑查克拉的加成的攻击力

	public int getAttack 
	{
		get
		{
			if (RTData != null)
			{
				float atk = baseAttack + curChakala_Atk;
				if (Star > config.star)
				{
					float param = Core.Data.monManager.GetMonUpAtkParam (config.star, Star);
					atk *= param;

				}
				return (int)atk;
			}
			else 
				return 0;
		}
	}

	public int getDefend 
	{
		get
		{
			if (RTData != null)
			{
				float def = baseDefend + curChakala_Def;
				if (Star > config.star)
				{
					float param = Core.Data.monManager.GetMonUpDefParam (config.star, Star);
					def *= param;
				}
				return (int)def;
			}
			else
				return 0;
		}
	}

	#endregion

	#region 不考虑查克拉的情况下的，基础攻击力
	public int baseAttack {
		get {
			if(config != null && RTData != null)
				return MathHelper.MidpointRounding(config.atk + config.atkGrowth * (RTData.curLevel - 1));
			else {
				return 0;
			}
		}
	}

	public int baseDefend {
		get {
			if(config != null && RTData != null)
				return MathHelper.MidpointRounding(config.def + config.defGrowth * (RTData.curLevel - 1));
			else 
				return 0;
		}
	}

	#endregion


	// ******** 一点查克拉丸能变为多少的攻击力和防御力，将来还有血量 *********
	#region 查克拉的的加成
	public int enhanceAttack {
		get {
			if(RTData != null)
			//	return RTData.ChaKeLa_Attck * RuntimeMonster.ATTACK_FACTOR;
				return curChakala_Atk * RuntimeMonster.ATTACK_FACTOR;
			else
				return 0;
		}
	} 

	public int enhanceDefend {
		get {
			if(RTData != null)
			//	return RTData.ChaKeLa_Defend * RuntimeMonster.DEFEND_FACTOR;
				return curChakala_Def * RuntimeMonster.ATTACK_FACTOR;

			else
				return 0;
		}
	}
	
	#endregion
	/*	***************************************************************/

	#region 获取缘分

	public List<FateData> getMyFate(FateManager manager){
		List<FateData> myFate = new List<FateData>();

		if(config.fateID != null) {
			foreach(int fateId in config.fateID) {
				myFate.Add(manager.getFateDataFromID(fateId));
			}
		}

		return myFate;
	}

	/// <summary>
	/// Checks my fate. 只有在队伍里面的才有缘分的价值，同时还有奥义数据
	/// </summary>
	/// <returns><c>true</c>, if my fate was checked, <c>false</c> otherwise.</returns>
	/// <param name="faDa">Fa da.</param>
	/// <param name="team">Team.</param>
	public bool checkMyFate(FateData faDa, MonsterTeam mTeam, List<AoYi> usedTeam) {
		bool check = true;
		if(faDa != null && mTeam != null && usedTeam != null) {
			int count = faDa.CountOfCondition;
			for(int i = 0; i < count; ++ i) {
				int[] condition = faDa.MyFate(i);
				if(condition != null) {
					ConfigDataType conData = (ConfigDataType)condition[FateData.Type_Pos];
					int ID = condition[FateData.Item_ID_Pos];
					int amount = condition[FateData.Item_Count_Pos];

					switch(conData) {
					case ConfigDataType.HugeBeast:
						check = checkAoYi(ID, amount, usedTeam) && check;
						break;
					case ConfigDataType.Gender:
						check = checkGender((short)ID, amount, mTeam) && check;
						break;
					case ConfigDataType.Monster:
						check = checkMonster(ID, amount, mTeam) && check;
						break;
					case ConfigDataType.Star:
						check = checkStar(ID, amount, mTeam) && check;
						break;
					case ConfigDataType.Equip:
						check = checkEquip(ID, amount, mTeam) && check;
						break;
					}

				}
			}

		}

		return check;
	}

	//不关心Count
	private bool checkAoYi(int AoYiNum, int count, List<AoYi> usedTeam) {
		bool found = false;
		if(usedTeam != null) {
			foreach(AoYi ao in usedTeam) {
				if(ao != null){
					if(ao.Num == AoYiNum) {
						found = true;
						break;
					}
				}
			}
		}

		return found;
	}

	private bool checkMonster(int MonNum, int count, MonsterTeam team) {
		int teamCount = team.capacity;

		int totalCount = 0;

		for(int pos = 0; pos < teamCount; ++ pos) {
			Monster mon = team.getMember(pos);
			if(mon != null) {
				if(mon.num == MonNum) 
					totalCount ++;
			}
		}

		return totalCount >= count; 
	}

	private bool checkStar(int StarLevel, int count, MonsterTeam team) {
		int teamCount = team.capacity;

		int totalcCount = 0;

		for(int pos = 0; pos < teamCount; ++ pos) {
			Monster mon = team.getMember(pos);
			if(mon != null && mon.Star == StarLevel) {
				totalcCount ++;
			}
		}

		return totalcCount >= count;
	}

	private bool checkEquip(int EquipNum, int count, MonsterTeam team) {
		int teamCount = team.capacity;
		int requiredCount = 0;


		for(int pos = 0; pos < teamCount; ++ pos) {
			Monster mon = team.getMember(pos);
			if(mon != null && mon.pid == this.pid) {
				Equipment equip1 = team.getEquip(pos, EquipData.TYPE_ATTACK);
				Equipment equip2 = team.getEquip(pos, EquipData.TYPE_DEFEND);

				if(equip1 != null && equip1.Num == EquipNum) {
					requiredCount ++;
				}

				if(equip2 != null && equip2.Num == EquipNum) {
					requiredCount ++;
				}

				break;
			}
		}

		return requiredCount >= count;

	}

	private bool checkGender(short gender, int count, MonsterTeam team) {
		int teamCount = team.capacity;
		int totalCount = 0;

		for(int pos = 0; pos < teamCount; ++ pos) {
			Monster mon = team.getMember(pos);
			if(mon != null && mon.Gender == (Gender)gender) {
				totalCount ++;
			}
		}

		return totalCount >= count;
	}


	#endregion

	#region 获取技能信息
	private List<Skill> mySkill;
	public List<Skill> getSkill
	{
		get
		{
			if(mySkill == null)
			{
				mySkill = new List<Skill>();
				if(config != null && config.skill != null)
				{
					SkillManager skillManager = Core.Data.skillManager;
					int index = 0;
					foreach(int skillId in config.skill) 
					{
						if(skillId != 0) 
						{//宠物可能会没有某种技能
							SkillData sd = skillManager.getSkillDataConfig(skillId);
							Utils.Assert(sd == null, "Skill ConfigData is wrong. Skill ID = " + skillId);
							SkillOpData sod = skillManager.getSkillOpDataConfig(sd.op);
							Utils.Assert(sod == null, "SkillOp ConfigData is wrong. op = " + sd.op);
							SkillLvData lvConfig = skillManager.GetSkillLvDataConfig (skillId);
							Utils.Assert(lvConfig == null, "SkillLv ConfigData is wrong. op = " + skillId);
							//只有第二个技能才需要判定是否激活
							
							int skillLv = 1;
							if (skillLvs != null)
							{
								for (int i = 0; i < skillLvs.Length; i++)
								{
									if (skillLvs [i].skillId == skillId)
									{
										skillLv = skillLvs [i].skillLevel;
										break;
									}
								}
							}
							
							if(index ++ == 1) {
								mySkill.Add(new Skill(sd, sod, lvConfig, RTData.curLevel, skillLv));
							} else {
								mySkill.Add(new Skill(sd, sod, lvConfig, skillLv));
							}
						}
					}
				}
			}

			return mySkill;
		}
	}
		
	#endregion

}