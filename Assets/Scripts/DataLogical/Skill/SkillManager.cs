using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System;

public class SkillManager : Manager {

	//key is Skill ID
	public Dictionary<int,SkillData> mSDConfig = null;
	//key is skillOp ID
	public Dictionary<int, SkillOpData> mSDOConfig = null;
	//等级升级
	public Dictionary<int, SkillLvData> mSkillUpConfig = null;

	public SkillManager () {
		mSDConfig = new Dictionary<int, SkillData>();
		mSDOConfig = new Dictionary<int, SkillOpData>();
		mSkillUpConfig = new Dictionary<int, SkillLvData> ();
	}

	public override bool loadFromConfig () {
		return base.readFromLocalConfigFile<SkillData>(ConfigType.Skill, mSDConfig) 
			| base.readFromLocalConfigFile<SkillOpData>(ConfigType.SkillOP, mSDOConfig)
			| base.readFromLocalConfigFile<SkillLvData>(ConfigType.SkillLv, mSkillUpConfig);
	}

	/// <summary>
	/// Gets the skill data config. 根据Skill ID来获取Skill Data
	/// </summary>
	/// <returns>The skill data config.</returns>
	/// <param name="ID">I.</param>
	public SkillData getSkillDataConfig (int ID) {
		SkillData sd = null;
		if(mSDConfig != null) {
			if(!mSDConfig.TryGetValue(ID, out sd)) {
				sd = null;
			}
		}
		return sd;
	}

	/// <summary>
	/// Gets the skill op data config. 根据Skill OP ID 来获取Skill OP Data
	/// </summary>
	/// <returns>The skill op data config.</returns>
	/// <param name="ID">I.</param>
	public SkillOpData getSkillOpDataConfig (int ID) {
		SkillOpData sod = null;

		if(mSDOConfig != null) {
			if(!mSDOConfig.TryGetValue(ID, out sod)) {
				sod = null;
			}
		}

		return sod;
	}

	//得到技能等级数据
	public SkillLvData GetSkillLvDataConfig(int ID)
	{
		if (mSkillUpConfig != null && mSkillUpConfig.ContainsKey (ID))
		{
			return mSkillUpConfig [ID];
		}
		return null;
	}

	//根据技能ID和技能等级，得到技能参数
	public skParam GetSkParamData(int skillID, int skillLv)
	{
		SkillLvData skillUpData = GetSkillLvDataConfig(skillID);

		//技能没找到
		if (skillUpData == null)
		{
			StringBuilder strBld = new StringBuilder ();
			strBld.Append (skillID.ToString ());
			strBld.Append ("  skill not find up data");
			RED.LogWarning (strBld.ToString());
			return null;
		}

		// 非法等级
		if (skillLv < 1 || skillLv > skillUpData.max_lv)
		{
			StringBuilder strBld = new StringBuilder ();
			strBld.Append (skillID.ToString ());
			strBld.Append ("  skill's  level is out of range ");
			strBld.Append (skillLv.ToString ());
			RED.LogWarning (strBld.ToString());
			return null;
		}

		SkillData sd = getSkillDataConfig(skillID);
		skParam param = new skParam ();

		if(skillLv - 1 < skillUpData.absorb.Length )
			param.absorb = (int)(skillUpData.absorb[skillLv - 1] + 0.5f);

		if(skillLv - 1 < skillUpData.add.Length )
			param.add    = (int)(skillUpData.add[skillLv - 1] + 0.5f);

		if(skillLv - 1 < skillUpData.damage.Length )
			param.damage = (int)(skillUpData.damage[skillLv - 1] + 0.5f);

		if(skillLv - 1 < skillUpData.dec.Length )
			param.dec    = (int)(skillUpData.dec[skillLv - 1] + 0.5f);

		if(skillLv - 1 < skillUpData.gailv.Length )
			param.gailv  = (int)(skillUpData.gailv[skillLv - 1] + 0.5f);

		if(skillLv - 1 < skillUpData.num.Length )
			param.num    = (int)(skillUpData.num[skillLv - 1] + 0.5f);

		if(skillLv - 1 < skillUpData.rate.Length )
			param.rate   = (int)(skillUpData.rate[skillLv - 1] + 0.5f);

		if(skillLv - 1 < skillUpData.rate2.Length )
			param.rate2  = (int)(skillUpData.rate2[skillLv - 1] + 0.5f);

		if(skillLv - 1 < skillUpData.seal.Length )
			param.seal   = (int)(skillUpData.seal[skillLv - 1] + 0.5f);
			
		if(skillLv - 1 < skillUpData.nuqi.Length )
			param.nuqi   = sd.param.nuqi;
		return param;
	}
}

public class ShowSkParam
{
	public float rate2;
	public float rate;
	public float gailv;
	public float damage;
	public float nuqi;
	public float num;
	public float add;
	public float dec;
	public float seal;
	public float absorb;

	private static FieldInfo[] publicFields;
	public static FieldInfo[] getFields {
		get {
			if(publicFields == null) {
				Type t = typeof(ShowSkParam);
				publicFields = t.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
			}
			return publicFields;
		}
	}
}
