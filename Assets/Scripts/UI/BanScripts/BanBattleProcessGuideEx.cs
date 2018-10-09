using UnityEngine;
using System.Collections;

public partial class BanBattleProcess : MonoBehaviour {

	//战斗新手引导的第。。。版本
	IEnumerator showGuideXX(Item curItem, BanBattleManager battleMgr) {

		GuideManager mgr = Core.Data.guideManger;
		TemporyData  temp = Core.Data.temper;

		if(temp.currentBattleType == TemporyData.BattleType.FightWithFulisa) {
			yield return StartCoroutine(FeatureWar(curItem, battleMgr));
		} else if(temp.currentBattleType == TemporyData.BattleType.BossBattle) {
			StartCoroutine(showLevel1(curItem, battleMgr, mgr, temp));
			showLevel16(curItem, battleMgr);
		}

	}

	//新版本未来之战的新手引导的逻辑
	IEnumerator FeatureWar(Item curItem, BanBattleManager battleMgr) {
		int attNum = battleMgr.GetBattleRole(curItem.attackIndex).number;
		int defNum = battleMgr.GetBattleRole(curItem.defenseIndex).number;

		//克制阶段
		if(isGuide_9(curItem, attNum, defNum)) {
			yield return StartCoroutine(battleMgr.getGuideUI.Guide_9());

			AsyncTask.QueueOnMainThread( ()=> {
				StartCoroutine(battleMgr.getGuideUI.Guide_A());
			}, 1.2f );
		}

		//怒气阶段
		if(isGuide_B(curItem, attNum, defNum)) {
			yield return StartCoroutine(battleMgr.getGuideUI.Guide_B());
			StartCoroutine(battleMgr.getGuideUI.Guide_C());
		}

		//普通技能阶段
		if(isGuide_D(curItem, attNum, defNum)) {
			yield return StartCoroutine(battleMgr.getGuideUI.Guide_D());

			AsyncTask.QueueOnMainThread( ()=> {
				StartCoroutine(battleMgr.getGuideUI.Guide_E());
			}, 0.05f );
		}

		//对决阶段
		if(isGuide_F(curItem, attNum, defNum)) {
			yield return StartCoroutine(battleMgr.getGuideUI.Guide_F());
			StartCoroutine(battleMgr.getGuideUI.Guide_10());
		}

		yield return null;
	}

	#region 释放未来之战技能的UI模块

	//适配屏幕
	float FitScreen(float Y, float manualHeight) {
		if(Y > 0)
			return Y + (manualHeight - 640f) * 0.5f;
		else 
			return Y - (manualHeight - 640f) * 0.5f;
	}

	private GameObject castGuide = null;

	private float now = 0;
	public bool FeatureWar_Cast (BoxCollider collider, float manualHeight) {
		//用来判断是否为特殊步骤
		bool over = true;
		BanBattleManager battleMgr = BanBattleManager.Instance;

		if(curStep == GuideStep.XiaoWuKong_OverSkill1) {
			Object obj = PrefabLoader.loadFromUnPack("Ban/AngryGuide", false);
			castGuide = Instantiate(obj) as GameObject;
			RED.AddChild(castGuide, BanBattleManager.Instance.AngryPos);
			castGuide.transform.localPosition = new Vector3(0f, 11f, 0f);
			//走到这一步
			curStep = GuideStep.XiaoWuKong_Cast_Overskill;
			//小悟空的怒气技要把按钮暴露出来
			StartCoroutine(turnAngryBtn());

			//设定Boxcollider的位置
			collider.center = new Vector3(-424f, FitScreen(-198f, manualHeight), 0);
			collider.size   = new Vector3(160f, 160f, 0);

			over = false;
			now  = Time.realtimeSinceStartup;

		} else if(curStep == GuideStep.XiaoWuKong_Cast_Overskill) {

			///
			/// --- 为了防止用户的快速点击操作 -----
			///
			float cur = Time.realtimeSinceStartup;
			float delta = cur - now;
			if(delta <= 1) return false;

			if(castGuide != null) Destroy(castGuide);
			Time.timeScale = 1.0f;

			BanSideInfo attside = battleMgr.attackSideInfo;
			attside.PlayerAngryWord.FeatureWarShow(1, string.Empty, ()=> { battleMgr.AngryUI(false, null, -1);});
			attside.angrySlot.curAP = 0;

			//走到这一步
			curStep = GuideStep.XiaoWuKong_Cast_Over;
			//设定Boxcollider的位置
			collider.center = Vector3.zero;
			collider.size   = new Vector3(1600f, 1200f, 0);

			over = false;

			StartCoroutine(battleMgr.getGuideUI.Guide_DX());
		}

		return over;
	}

	//小悟空的怒气技要把按钮暴露出来
	IEnumerator turnAngryBtn() {
		BanBattleManager battleMgr = BanBattleManager.Instance;
		Time.timeScale = 1.0f;
		yield return new WaitForSeconds(0.1f);
		SkillData sd = Core.Data.skillManager.getSkillDataConfig(25037); 
		battleMgr.AngryUI(true, sd, 1);
		Invoke("stopAgain", 0.6f);
	}

	void stopAgain() {
		Time.timeScale = 0.0f;
	}

	#endregion

	#region 新版本未来之战引导的详细判定
	//小悟空克制比克大魔王
	bool isGuide_9(Item curItem, int attNum, int defNum) {
		bool guide = false;

		if(attNum == 10142 && defNum == 10193) {
			if(curItem.period == Period.AttributeConflict) {
				guide = true;
				curStep = GuideStep.XiaoWuKong_Conflict_BiKe1;
				ConsoleEx.DebugLog("isGuide_9 is going", ConsoleEx.RED);
			}
		}

		return guide;
	}

	//小悟空释放怒气技
	bool isGuide_B(Item curItem, int attNum, int defNum) {
		bool guide = false;

		#if !LOCAL_AUTO
		//攻击方是小悟空
		bool bfromAtk = curItem.extra_1 == 0;

		if(attNum == 10142 && defNum == 10193 && bfromAtk) {
			if(curItem.period == Period.AngrySkill) {
				guide = true;
				curStep = GuideStep.XiaoWuKong_OverSkill1;
				ConsoleEx.DebugLog("isGuide_B is going", ConsoleEx.RED);
			}
		}
		#endif
		return guide;
	}

	//小悟空释放普通技能
	bool isGuide_D(Item curItem, int attNum, int defNum) {
		bool guide = false;
		//攻击方是小悟空
		bool bfromAtk = curItem.extra_1 == 0;

		if(attNum == 10142 && defNum == 10193 && bfromAtk) {
			if(curItem.period == Period.NormalSkill) {
				guide = true;
				curStep = GuideStep.XiaoWuKong_NormalSkill1;
				ConsoleEx.DebugLog("isGuide_D is going", ConsoleEx.RED);
			}
		}

		return guide;
	}


	//小悟空对决比克大魔王
	bool isGuide_F(Item curItem, int attNum, int defNum) {
		bool guide = false;

		if(attNum == 10142 && defNum == 10193) {
			if(curItem.period == Period.NormalAttack) {
				guide = true;
				curStep = GuideStep.XiaoWuKong_Vs_BiKe1;
				ConsoleEx.DebugLog("isGuide_F is going", ConsoleEx.RED);
			}
		}

		return guide;
	}


	#endregion


	#region 第16关的新手引导

	void showLevel16(Item curItem, BanBattleManager battleMgr) {
		if(isGuide16Floor(curItem, battleMgr)){
			AsyncTask.QueueOnMainThread( ()=> {
				StartCoroutine(battleMgr.getGuideUI.Guide_Level16());
			},
				#if NewGuide
				0.45f );
				#else
				0.85f );
				#endif
		}
	}

	/// 
	/// 判定是否为第16关的boss 属性克制
	/// 
	bool isGuide16Floor(Item curItem, BanBattleManager battleMgr) {
		bool found = false;
		NewFloor floor = Core.Data.newDungeonsManager.curFightingFloor;

		if(floor != null && floor.config.ID == 60116) {

			BanBattleRole curBattleRole = battleMgr.GetBattleRole(curItem.defenseIndex);
			if(curBattleRole != null) {
				if(curBattleRole.isBoss == (short) 1) {
					if(curItem.period == Period.AttributeConflict) {
						AccountConfigManager accMgr = Core.Data.AccountMgr;

						if(accMgr.UserConfig.ShengWuZhe == (short)0) {
							ConsoleEx.DebugLog("isGuide16Floor is going", ConsoleEx.RED);

							accMgr.UserConfig.ShengWuZhe = 1;
							accMgr.save();

							found = true;
						}

					}

				}
			}
		}

		return found;

	}

	#endregion

}
