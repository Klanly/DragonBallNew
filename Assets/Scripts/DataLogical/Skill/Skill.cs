using System.Reflection;
/// <summary>
/// Skill. 只有第2个技能是根据宠物等级来决定是否开启。
/// </summary>
public class Skill {
	public const int SKILL2_MIN_LEVEL = 30;
	public const int SKILL3_MIN_LEVEL = 45;

	//技能数据，从本地配表读出来
	public SkillData sdConfig;
	//根据SkillData读出skillOpData
	public SkillOpData sodConfig;
	//技能升级数据，从配表读取
	public SkillLvData skillLvConfig;

	//是否升级
	private bool upgrade = true;

	//开启了吗？
	public bool opened;

    string BlueColor = "[00D2FF]";

	private ShowSkParam _showSkParam = new ShowSkParam();
	public ShowSkParam showParam
	{
		get
		{
			if (level <= skillLvConfig.max_lv && level > 0)
			{
				if(level - 1 < skillLvConfig.absorb.Length )
					_showSkParam.absorb = skillLvConfig.absorb[level - 1];

				if(level - 1 < skillLvConfig.add.Length )
					_showSkParam.add = skillLvConfig.add[level - 1];

				if(level - 1 < skillLvConfig.damage.Length )
					_showSkParam.damage = skillLvConfig.damage[level - 1];

				if(level - 1 < skillLvConfig.dec.Length )
					_showSkParam.dec = skillLvConfig.dec[level - 1];

				if(level - 1 < skillLvConfig.gailv.Length )
					_showSkParam.gailv = skillLvConfig.gailv[level - 1];

				if(level - 1 < skillLvConfig.num.Length )
					_showSkParam.num = skillLvConfig.num[level - 1];

				if(level - 1 < skillLvConfig.rate.Length )
					_showSkParam.rate = skillLvConfig.rate[level - 1];

				if(level - 1 < skillLvConfig.rate2.Length )
					_showSkParam.rate2 = skillLvConfig.rate2[level - 1];

				if(level - 1 < skillLvConfig.seal.Length )
					_showSkParam.seal = skillLvConfig.seal[level - 1];

				if(level - 1 < skillLvConfig.nuqi.Length )
					_showSkParam.nuqi = skillLvConfig.nuqi[level - 1];
			}

			return _showSkParam;
		}
	}

	//当前技能参数
	private skParam _curSkParam = new skParam();
	public skParam curSkParam
	{
		get
		{
			if (upgrade)
			{
				if (level <= skillLvConfig.max_lv && level > 0)
				{
					if(level - 1 < skillLvConfig.absorb.Length )
						_curSkParam.absorb = (int)skillLvConfig.absorb[level - 1];

					if(level - 1 < skillLvConfig.add.Length )
						_curSkParam.add = (int)skillLvConfig.add[level - 1];

					if(level - 1 < skillLvConfig.damage.Length )
						_curSkParam.damage = (int)skillLvConfig.damage[level - 1];

					if(level - 1 < skillLvConfig.dec.Length )
						_curSkParam.dec = (int)skillLvConfig.dec[level - 1];
				
					if(level - 1 < skillLvConfig.gailv.Length )
						_curSkParam.gailv = (int)skillLvConfig.gailv[level - 1];

					if(level - 1 < skillLvConfig.num.Length )
						_curSkParam.num = (int)skillLvConfig.num[level - 1];

					if(level - 1 < skillLvConfig.rate.Length )
						_curSkParam.rate = (int)skillLvConfig.rate[level - 1];

					if(level - 1 < skillLvConfig.rate2.Length )
						_curSkParam.rate2 = (int)skillLvConfig.rate2[level - 1];

					if(level - 1 < skillLvConfig.seal.Length )
						_curSkParam.seal = (int)skillLvConfig.seal[level - 1];
						
					if(level - 1 < skillLvConfig.nuqi.Length )
						_curSkParam.nuqi = (int)skillLvConfig.nuqi[level - 1];
				}

				upgrade = false;
			}
			return _curSkParam;
		}
	}

	//技能等级
	private int m_nLevel = 1;
	public int level 
	{
		get 
		{
			return m_nLevel;
		}
		set
		{
			m_nLevel = value;
			upgrade = true;
		}
	}

	//升级消耗升级券
	public int cost_skillCard
	{
		get 
		{
			if (level < skillLvConfig.max_lv && level > 0)
			{
				return skillLvConfig.cost_juanzhou [level - 1];
			}
			return 0;
		}
	}

	//升级消耗金币
	public int cost_coin
	{
		get 
		{
			if (level < skillLvConfig.max_lv && level > 0)
			{
				if(level - 1 < skillLvConfig.cost_gold.Length)
				{
					return skillLvConfig.cost_gold [level - 1];
				}
				return 0;
			}
			return 0;
		}
	}

	public Skill () { }

	public Skill(SkillData data, SkillOpData sod , SkillLvData lvData, int skillLv, bool opened = true) {
		this.sdConfig = data;
		this.sodConfig = sod;
		this.skillLvConfig = lvData;
		level = skillLv;
		this.opened = opened;
//		_curSkParam.absorb = sdConfig.param.absorb;
//		_curSkParam.add = sdConfig.param.add;
//		_curSkParam.damage = sdConfig.param.damage;
//		_curSkParam.dec = sdConfig.param.dec;
//		_curSkParam.gailv = sdConfig.param.gailv;
//		_curSkParam.num = sdConfig.param.num;
//		_curSkParam.nuqi = sdConfig.param.nuqi;
//		_curSkParam.rate = sdConfig.param.rate;
//		_curSkParam.rate2 = sdConfig.param.rate2;
//		_curSkParam.seal = sdConfig.param.seal;
	}

	public Skill(SkillData data, SkillOpData sod, SkillLvData lvData, int monsterLv, int skillLv) {
		this.sdConfig = data;
		this.sodConfig = sod;
		this.skillLvConfig = lvData;
		this.opened = monsterLv >= SKILL2_MIN_LEVEL;
		level = skillLv;

//		_curSkParam.absorb = sdConfig.param.absorb;
//		_curSkParam.add = sdConfig.param.add;
//		_curSkParam.damage = sdConfig.param.damage;
//		_curSkParam.dec = sdConfig.param.dec;
//		_curSkParam.gailv = sdConfig.param.gailv;
//		_curSkParam.num = sdConfig.param.num;
//		_curSkParam.nuqi = sdConfig.param.nuqi;
//		_curSkParam.rate = sdConfig.param.rate;
//		_curSkParam.rate2 = sdConfig.param.rate2;
//		_curSkParam.seal = sdConfig.param.seal;
	}

	//技能的说明
	public string Description {
		get {
			if(sdConfig != null) {
				return sdConfig.mark;
			} else 
				return string.Empty;
		}
	}
	//技能的作用详细说明， 
	//需要根据当前等级调整...
	public string EffecDescription {
		get {
			if(sodConfig != null && sdConfig != null) {
				string tmp = sodConfig.mark;

				FieldInfo[] mFields = ShowSkParam.getFields;
				foreach(FieldInfo fi in mFields) {
					if(fi != null) 
					{
						if(sdConfig.param != null)
						{
							tmp = tmp.Replace(fi.Name,BlueColor+ fi.GetValue(showParam).ToString()+"[-]");
						}
					}
				}

				return tmp;
			} else {
				return string.Empty;
			}
		}
	}
}

