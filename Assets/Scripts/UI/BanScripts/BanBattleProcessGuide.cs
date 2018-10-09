using UnityEngine;
using System.Collections;

public partial class BanBattleProcess : MonoBehaviour {

	//如果有新手引导则看当前是在第几步
	private GuideStep curStep = GuideStep.XiaoWuKong_V_BiKe;

	enum GuideStep {

		/// <summary>
		/// 到编号为8的时候，都是未来之战的版本，即不再使用了
		/// </summary>

		//小悟空克制比克大魔王
		XiaoWuKong_V_BiKe         = 0x0,
		//小悟空怒气技能（教学）狂揍-释放4倍
		XiaoWuKong_OS_4           = 0x1,
		//孙悟空2继承小悟空20怒气
		WuKong2_Anger             = 0x2,
		//孙悟空2释放终结技能
		WuKong2_OS_4              = 0x3,
		//孙悟空3释放终结技能
		WuKong3_OS_4              = 0x4,
		//比鲁斯
		BILUSI_V_WuKong3          = 0x5,
		//没有怒气值不能释放主动技能
		WuKong3_NO_ANGER          = 0x6,
		//超级孙悟空3对决比鲁斯
		WuKong3_Vs_BILUSI         = 0x7,
		//布尔玛出现
		BuErMa_Show               = 0x8,



		/// <summary>
		/// 下面的编号，是新版本的未来之战
		/// 所有的编号，都是两个为一组，第一个表示阶段，第二个表示说明
		/// </summary>

		//小悟空克制比克大魔王
		XiaoWuKong_Conflict_BiKe1  = 0x9,
		XiaoWuKong_Conflict_BiKe2  = 0xA,

		//小悟空释放怒气技
		XiaoWuKong_OverSkill1      = 0xB,
		XiaoWuKong_OverSkill2      = 0xC,

		//小悟空释放普通技能
		XiaoWuKong_NormalSkill1    = 0xD,
		XiaoWuKong_NormalSkill2    = 0xE,

		//小悟空对决比克大魔王
		XiaoWuKong_Vs_BiKe1        = 0xF,
		XiaoWuKong_Vs_BiKe2        = 0x10,

		//新手引导第一个关,释放怒气技的时候
		XiaoWuKong_OS_1           = 0x20,
		//小悟空在第四关关掉，给下面的人留怒气的时候
		XiaoWuKong_Die            = 0x21,

		//点击按钮正式释放怒气技能
		XiaoWuKong_Cast_Overskill = 0x22,
		//点击按钮正式释放怒气技能
		XiaoWuKong_Cast_Over      = 0x23,

		//未定义
		None                      = 0x24,
	}

	void showGuideX(Item curItem, BanBattleManager battleMgr) {

		GuideManager mgr = Core.Data.guideManger;
		TemporyData  temp = Core.Data.temper;

		if(mgr.isGuiding){
			if(temp.currentBattleType == TemporyData.BattleType.FightWithFulisa) {
				StartCoroutine(showFeatureWar(curItem, battleMgr, mgr));
			} else if(temp.currentBattleType == TemporyData.BattleType.BossBattle) {
				StartCoroutine(showLevel1(curItem, battleMgr, mgr, temp));
			}
		}

	}

	/// <summary>
	/// 未来之战的引导
	/// </summary>

	IEnumerator showFeatureWar(Item curItem, BanBattleManager battleMgr, GuideManager mgr) {
		if(isGuide_1(curItem, battleMgr)) {
			yield return new WaitForSeconds(BanTimeCenter.XiaoWuKong_V_BiKe);
			Time.timeScale = 0.0f;
			mgr.AutoRUN();
		}

		if(isGuide_2(curItem, battleMgr)) {
			//小悟空的怒气技要把按钮暴露出来
			SkillData sd = Core.Data.skillManager.getSkillDataConfig(25037); 
			battleMgr.AngryUI(true, sd, 4);
			yield return new WaitForSeconds(BanTimeCenter.XiaoWuKong_OS_4);
			mgr.AutoRUN();
			yield return new WaitForSeconds(1F);
			Time.timeScale = 0.0f;
		}

		if(isGuide_3(curItem, battleMgr)) {
			yield return new WaitForSeconds(BanTimeCenter.WuKong2_Anger);
			Time.timeScale = 0.0f;
			mgr.AutoRUN();
		}

		if(isGuide_4(curItem, battleMgr)) {
			//悟空2的怒气技要把按钮暴露出来
			SkillData sd = Core.Data.skillManager.getSkillDataConfig(25037); 
			battleMgr.AngryUI(true, sd, 4);
			yield return new WaitForSeconds(BanTimeCenter.XiaoWuKong_OS_4);
			Time.timeScale = 0.0f;
			mgr.AutoRUN();
		}

		if(isGuide_5(curItem, battleMgr)) {
			//悟空3的怒气技要把按钮暴露出来
			SkillData sd = Core.Data.skillManager.getSkillDataConfig(25058); 
			battleMgr.AngryUI(true, sd, 4);
			yield return new WaitForSeconds(BanTimeCenter.XiaoWuKong_OS_4);
			Time.timeScale = 0.0f;
			mgr.AutoRUN();
		}

		if(isGuide_6(curItem, battleMgr)) {
			yield return new WaitForSeconds(BanTimeCenter.BILUSI_V_WuKong3);
			Time.timeScale = 0.0f;
			mgr.AutoRUN();
		}

		//没有怒气值不能释放主动技能
		if(isGuide_7(curItem, battleMgr)) {
			yield return new WaitForSeconds(BanTimeCenter.WuKong3_NO_ANGER);
			Time.timeScale = 0.0f;
			mgr.AutoRUN();
		}

		//超级孙悟空3对决比鲁斯
		if(isGuide_8(curItem, battleMgr)) {
			Time.timeScale = 0.0f;
			mgr.AutoRUN();
		}
	}

	#region 未来之战详细的每个阶段新手引导的判定
	//小悟空克制比克大魔王
	bool isGuide_1(Item curItem, BanBattleManager battleMgr) {
		bool guide = false;
		int attNum = battleMgr.GetBattleRole(curItem.attackIndex).number;
		int defNum = battleMgr.GetBattleRole(curItem.defenseIndex).number;

		if(attNum == 10142 && defNum == 10193) {
			if(curItem.period == Period.AttributeConflict) {
				guide = true;
				curStep = GuideStep.XiaoWuKong_V_BiKe;
				ConsoleEx.DebugLog("isGuide_1 is going", ConsoleEx.RED);
			}
		}

		return guide;
	}

	//小悟空怒气技能（教学）狂揍-释放4倍
	bool isGuide_2(Item curItem, BanBattleManager battleMgr) {
		bool guide = false;
		int attNum = battleMgr.GetBattleRole(curItem.attackIndex).number;
		int defNum = battleMgr.GetBattleRole(curItem.defenseIndex).number;

		if(attNum == 10142 && defNum == 10193) {
			if(curItem.period == Period.AngrySkill) {
				guide = true;
				curStep = GuideStep.XiaoWuKong_OS_4;
				ConsoleEx.DebugLog("isGuide_2 is going", ConsoleEx.RED);
			}
		}

		return guide;
	}

	//孙悟空2继承小悟空20怒气
	bool isGuide_3(Item curItem, BanBattleManager battleMgr) {
		bool guide = false;
		int attNum = battleMgr.GetBattleRole(curItem.attackIndex).number;
		int defNum = battleMgr.GetBattleRole(curItem.defenseIndex).number;

		if(attNum == 10107 && defNum == 10105 && curItem.period == Period.Attend) {
			guide = true;
			curStep = GuideStep.WuKong2_Anger;
			ConsoleEx.DebugLog("isGuide_3 is going", ConsoleEx.RED);
		}

		return guide;
	}

	//孙悟空2释放终结技能
	bool isGuide_4(Item curItem, BanBattleManager battleMgr) {
		bool guide = false;
		int attNum = battleMgr.GetBattleRole(curItem.attackIndex).number;
		int defNum = battleMgr.GetBattleRole(curItem.defenseIndex).number;

		if(attNum == 10107 && defNum == 10105 && curItem.period == Period.AngrySkill) {
			guide = true;
			curStep = GuideStep.WuKong2_OS_4;
			ConsoleEx.DebugLog("isGuide_4 is going", ConsoleEx.RED);
		}

		return guide;
	}

	//孙悟空3释放终结技能
	bool isGuide_5(Item curItem, BanBattleManager battleMgr) {
		bool guide = false;
		int attNum = battleMgr.GetBattleRole(curItem.attackIndex).number;
		int defNum = battleMgr.GetBattleRole(curItem.defenseIndex).number;

		if(attNum == 10218 && defNum == 10104 && curItem.period == Period.AngrySkill) {
			guide = true;
			curStep = GuideStep.WuKong3_OS_4;
			ConsoleEx.DebugLog("isGuide_5 is going", ConsoleEx.RED);
		}

		return guide;
	}

	//比鲁斯
	bool isGuide_6(Item curItem, BanBattleManager battleMgr) {
		bool guide = false;
		int attNum = battleMgr.GetBattleRole(curItem.attackIndex).number;

		if(attNum == 10218 && curItem.defenseIndex == 6) {
			if(curItem.period == Period.AttributeConflict) {
				guide = true;
				curStep = GuideStep.BILUSI_V_WuKong3;
				ConsoleEx.DebugLog("isGuide_6 is going", ConsoleEx.RED);
			}
		}

		return guide;
	}

	//没有怒气值不能释放主动技能
	bool isGuide_7(Item curItem, BanBattleManager battleMgr) {
		bool guide = false;
		int attNum = battleMgr.GetBattleRole(curItem.attackIndex).number;

		if(attNum == 10218 && curItem.defenseIndex == 6) {
			if(curItem.period == Period.AttributeConflict) {
				guide = true;
				curStep = GuideStep.WuKong3_NO_ANGER;
				ConsoleEx.DebugLog("isGuide_7 is going", ConsoleEx.RED);
			}
		}

		return guide;
	}

	//对决的新手引导
	bool isGuide_8(Item curItem, BanBattleManager battleMgr) {
		bool guide = false;
		int attNum = battleMgr.GetBattleRole(curItem.attackIndex).number;

		if(attNum == 10218 && curItem.defenseIndex == 6) {
			if(curItem.period == Period.NormalAttack) {
				guide = true;
				curStep = GuideStep.WuKong3_Vs_BILUSI;
				ConsoleEx.DebugLog("isGuide_7 is going", ConsoleEx.RED);
			}
		}

		return guide;
	}

	#endregion

	#region 第一个关卡的新手引导

	/// <summary>
	/// 新手引导第一关
	/// </summary>

	IEnumerator showLevel1(Item curItem, BanBattleManager battleMgr, GuideManager mgr, TemporyData temp) {
		NewFloor floor = Core.Data.newDungeonsManager.curFightingFloor;

		#if NewGuide
		if(floor != null && floor.config.ID == 60103 && mgr.isGuiding) {
		#else
		if(floor != null && floor.config.ID == 60101 && mgr.isGuiding) {
		#endif
			if(isGuideLevel1(curItem, battleMgr)) {
				SkillData sd = Core.Data.skillManager.getSkillDataConfig(25037); 
				battleMgr.AngryUI(true, sd, 1);
				yield return new WaitForSeconds(BanTimeCenter.XiaoWuKong_Level1);
				Time.timeScale = 0.0f;
				mgr.AutoRUN();
			}
		}

	}		

	bool isGuideLevel1(Item curItem, BanBattleManager battleMgr) {
		bool guide = false;
		int attNum = battleMgr.GetBattleRole(curItem.attackIndex).number;
		int defNum = battleMgr.GetBattleRole(curItem.defenseIndex).number;

		#if NewGuide
		if(attNum == 10142 && defNum == 10172) {
		#else
		if(attNum == 10142 && defNum == 10175) {
		#endif
			if(curItem.period == Period.AngrySkillReady) {
				guide = true;
				curStep = GuideStep.XiaoWuKong_OS_1;
				ConsoleEx.DebugLog("isGuideLevel1 is going", ConsoleEx.RED);
			}
		}

		return guide;
	}

	#endregion

}
