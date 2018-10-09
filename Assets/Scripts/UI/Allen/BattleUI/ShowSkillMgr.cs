using UnityEngine;
using System.Collections;

public class ShowSkillMgr : MonoBehaviour {
	//怒气技 -- 左边这个不赋值
	public SkillIcon Ang;
	//普通技能
	public SkillIcon Nor1;
	public SkillIcon Nor2;

	private int curSkillId = -1;
	private int MonNum     = -1;
	private int lv         = -1;

	/// <summary>
	/// 怒气技等级
	/// </summary>
	private int AngSkLv    = 1;

	/// <summary>
	/// 普通技1等级
	/// </summary>
	private int Nor1SkLv   = 1;

	/// <summary>
	/// 普通技2等级
	/// </summary>
	private int Nor2SkLv   = 1;

	/// <summary>
	/// 登场的时候初始化信息
	/// </summary>
	public void Attend() {
		MonsterManager monMgr = Core.Data.monManager;
		SkillManager   skMgr  = Core.Data.skillManager;
		MonsterData config = monMgr.getMonsterByNum(MonNum);

		if(config != null) {
			int AngId  = config.skill.Value<int>(2);
			int Nor1Id = config.skill.Value<int>(0);
			int Nor2Id = config.skill.Value<int>(1);
			int iconId = 0;
			SkillData sd = null;
			if(Ang != null) {
				sd    = skMgr.getSkillDataConfig(AngId);
				iconId= sd == null ? 0 : sd.Icon;
				StartCoroutine(Ang.attendAnim(AngId, iconId, AngSkLv));
			}

			if(Nor1 != null) {
				sd    = skMgr.getSkillDataConfig(Nor1Id);
				iconId= sd == null ? 0 : sd.Icon;
				StartCoroutine(Nor1.attendAnim(Nor1Id, iconId, Nor1SkLv));
			}

			if(Nor2 != null) {
				sd    = skMgr.getSkillDataConfig(Nor2Id);
				iconId= sd == null ? 0 : sd.Icon;
				StartCoroutine(Nor2.attendAnim(Nor2Id, iconId, Nor2SkLv, lv < 30));
			}

		} else {
			ConsoleEx.DebugLog("No Such Monster = " + MonNum, ConsoleEx.YELLOW);
		}

		curSkillId = -1;
	}

	/// <summary>
	/// 有一方出场的时候
	/// </summary>

	public void Attend(int Monnum, int level, SkillObj AngSkObj, SkillObj[] NorSkObj) {
		MonNum = Monnum;
		lv     = level;

		if(AngSkObj != null) AngSkLv = AngSkObj.skillLevel;

		if(NorSkObj != null) {
			SkillObj nor1 = NorSkObj.Value<SkillObj>(0);
			SkillObj nor2 = NorSkObj.Value<SkillObj>(1);

			if(nor1 != null) Nor1SkLv = nor1.skillLevel;
			if(nor2 != null) Nor2SkLv = nor2.skillLevel;
		}

		Attend();
	}

	/// <summary>
	/// 释放技能
	/// </summary>

	public void SkillAnim(int skillId) {
		curSkillId = skillId;

		if(Ang != null) {
			StartCoroutine(Ang.CastAnim(skillId));
		}

		if(Nor1 != null) {
			StartCoroutine(Nor1.CastAnim(skillId));
		}

		if(Nor2 != null) {
			StartCoroutine(Nor2.CastAnim(skillId));
		}

	}

}
