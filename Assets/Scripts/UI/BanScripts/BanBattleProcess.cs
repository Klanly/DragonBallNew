using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AW.Battle;

public partial class BanBattleProcess : MonoBehaviour {

    //所有的要播放的动画片段
    public List<Item> list_Item = new List<Item>();

    //战斗的各种阶段
    public enum Period{
        AttributeChange,
        AttributeIgnore,
        AttributeConflict,
        AngrySkill,
        NormalSkill,
        NormalAttack,
        AfterBattle,
        DieSkill,
        DragonAoYi,
        Attend,
        EndWar,
        AngrySkillReady,
        None,
    }

    [System.Serializable]
    public class Item {
        //Useless
        public const int Extra3_Skill_Type_Attack = 1;
        //额外三号数据，增加攻击类型技能
        public const int Extra3_Skill_Type_PowerUp = 2;
        /// <summary>
        /// 吸血
        /// </summary>
        public const int Extra3_Skill_Type_Suck = 3; 
        public const int Extra3_Skill_Type_SelfBlast = 4; 
        public const int Extra3_Skill_Type_Revive = 5; 
        //怒气连击, 或是普通技能的连击
        public const int Extra3_Combo = 6;
        //双方等待的某些技能 自损攻击，比如 初代魔人炮
        public const int Extra3_DoubleIdle = 7;
        //更新Angry Point
        public const int Extra3_UpdateAP = 8;
        //增加Angry Point
        public const int Extra3_AddAP = 9;
        //恢复技能
        public const int Extra3_Recover = 10;
        //有一些技能，可能挨打的人不会这么快有Injure_Fly_Go，所以导致动画会跳过
        public const int Extra3_LongSkill = 11;
        //消耗一些怒气，增加一些怒气
        public const int CostAndRecoverAngry = 12;
		//自爆的技能
		public const int Extra3_Explore = 13;
		//奥义增加怒气
		public const int Extra3_AoYiAngry = 14;
		//属性更改
		public const int Extra3_Attribute_CHG = 15;

        public Item(int lunCount,int huiHeCount,int attackIndex,int defenseIndex,int attackBP,int defenseBP,int attackAP,int defenseAP,Period period,int extra_1,int extra_2,int extra_3,int sAction,int leftAp,string skillname="", CMsgHeader item=null){
            this.lunCount = lunCount;
            this.huiHeCount = huiHeCount;
            this.attackIndex = attackIndex;
            this.defenseIndex = defenseIndex;
            this.attackBP = attackBP;
            this.defenseBP = defenseBP;
            this.attackAP = attackAP;
            this.defenseAP = defenseAP;
            this.period = period;
            this.skillAction=sAction; 
            this.leftAp = leftAp; 
            this.extra_1 = extra_1;
            this.extra_2 = extra_2;
            this.extra_3 = extra_3;
            this.skillName = skillname;
            this.aHeader = item;
        }

        //技能类型
        public CSkill_Type skillType;

        //轮数
        public int lunCount;

        //回合数
        public int huiHeCount;

        //攻击方索引
        public int attackIndex;

        //防御方索引
        public int defenseIndex;

        //攻击方战斗力点数
        public int attackBP;

        //防御方战斗力点数
        public int defenseBP;

        //攻击方怒气点数
        public int attackAP;

        //防御方怒气点数
        public int defenseAP;

        //阶段类型
        public Period period;
        /// <summary>
        /// The skill 动作.
        /// </summary>
        public int skillAction; 
        /// <summary>
        /// The 施法方 剩余怒气.
        /// </summary>
        public int leftAp; 
        public string skillName;
        public int skillId;

        //消耗的怒气
        public int costAngry;
        //增加的怒气
        public int recoverAngry;
        //消耗时当前的怒气
        public int preAngry;

        public CMsgHeader aHeader;
        //额外的三个数据 
        public int extra_1 = -1;

        public int extra_2 = -1;

        public int extra_3 = -1;

		//有时候会有奥义的信息
		public AoYiData aoyiConfig;

		//
		public int VsMiddle = -1;
    }

    //保留是为了濒死技能的显示
    public class DieSkill {
        public bool showDieSkill;
        public Item item;
    }

    private DieSkill diePeriod;

    public static BanBattleProcess Instance = null;
    //攻击方宠物的索引
    public int attackerIndex{
        get{
            return attackPBM.index;
        }
    }

    //防御方宠物的索引
    public int defenderIndex{
        get{
            return defensePBM.index;
        }
    }

    //攻击方的宠物
    private PetBattleModel attackPBM;

    //防御方的宠物
    private PetBattleModel defensePBM;

    //根据索引找到宠物
    public PetBattleModel GetPBM(int index){
        if (attackPBM == null)
            ConsoleEx.DebugLog("Current PBM is Null.");
        if(attackPBM.index == index){
            return attackPBM;
        }
        if(defensePBM.index == index){
            return defensePBM;
        }
        return null;
    }

    //代表攻击方的PlayAnim
    private CRLuo_PlayAnim_FX triggerPlayAnim = null;

    //代表防御方的PlayAnim
    private CRLuo_PlayAnim_FX triggerPlayAnim2 = null;

    public void Awake(){
        Instance = this;
    }

    //处理核心内容
    public void HandleItem(){
        StartCoroutine(HandleItemIE());
    }

    //停止处理核心内容
    public void HandleItemEnd(){
        //BanTools.Log ("停止播放战斗动画");
        // StopCoroutine("HandleItemIE");
        StopAllCoroutines ();
        //清除怒气
        clearAngryPoint();
        //清除死亡技能信息
        diePeriod = null;
    }


    //展示技能名字
    void showSkillName(Item item) {
        if (!string.IsNullOrEmpty (item.skillName)) {
            bool bfromAtk = BanBattleManager.Instance.GetBattleRole (item.extra_1).group == BanBattleRole.Group.Attack; 

            item.skillName = item.skillName.Replace( Core.Data.stringManager.getString(11), " ");

            try {
                if(bfromAtk)
                    BanBattleManager.Instance.attackSideInfo.ShowSkillName (item.skillName);
                else
                    BanBattleManager.Instance.defenseSideInfo.ShowSkillName (item.skillName);
            } catch(System.Exception ex) {
                ConsoleEx.DebugLog("ex : " + ex.ToString() + "--- " + item.skillName);
            }

        }
    }

	//展示倒地的效果
	void ShowInjureAction(bool left) {
		Main3DManager main3DMgr = BanBattleManager.Instance.main3DManager;
		main3DMgr.KillYou = false;
		CRLuo_PlayAnim_FX fx = null;

		if(left) {
			fx = main3DMgr.Man_L;
		} else {
			fx = main3DMgr.Man_R;
		}

		fx.SaveLife = false;
		fx.Injure_Key = false;
		fx.HandleTypeAnim (CRLuoAnim_Main.Type.Injure_Fly_Go);
	}

    //技能动作
    //[1.入场；2.增益；3.属克；4.普攻；5.普技波1；6.特技；7.群攻；8.究极技能]
    IEnumerator ShowSkillAction(Item item) { 

		BanBattleManager battleMgr = BanBattleManager.Instance;
		Main3DManager main3DMgr = BanBattleManager.Instance.main3DManager;

		bool bfromAtk = battleMgr.GetBattleRole (item.extra_1).group == BanBattleRole.Group.Attack; 
        showSkillName(item);

        switch (item.skillAction) { 
        case 1: case 3: case 4: case 6:
            //Ignoral
            break;
        case 2:
			main3DMgr.Free1_Ban (bfromAtk);
            break;
        case 5:
            //现在只有右边的怒气技能，才需要free1动作
            if(item.skillType == CSkill_Type.Anger && bfromAtk == false) {
				main3DMgr.Free1_Ban (bfromAtk);
				yield return new WaitForSeconds(BanTimeCenter.F_Free_Time);
			}
            NormalSkillFight (item);
            break;
        case 7:
			if(bfromAtk) {
				if(item.defenseBP == 0) main3DMgr.KillYou = true;
				else main3DMgr.KillYou = false;
			} else {
				if(item.attackBP == 0) main3DMgr.KillYou = true;
				else main3DMgr.KillYou = false;
			}
            if(item.period == Period.DieSkill) {
				StartCoroutine(main3DMgr.GroupExploreSelf(bfromAtk));
                yield return new WaitForSeconds(BanTimeCenter.F_EXPLORE_TIME);
				main3DMgr.Die(bfromAtk);
				main3DMgr.FlyDown(!bfromAtk, battleMgr.main3DManager.KillYou);
            } else {
                //现在只有右边的怒气技能，才需要free1动作
                if(item.skillType == CSkill_Type.Anger && bfromAtk == false) {
					main3DMgr.Free1_Ban (bfromAtk);
					yield return new WaitForSeconds(BanTimeCenter.F_Free_Time);
				}
				main3DMgr.GroupSkill_Ban (bfromAtk);
            }
            break;
        case 8:
            //现在只有右边的怒气技能，才需要free1动作
            if(bfromAtk == false) {
                main3DMgr.Free1_Ban (bfromAtk);
                yield return new WaitForSeconds(BanTimeCenter.F_Free_Time);
                yield return new WaitForSeconds(BanTimeCenter.F_Show_OVERSKILL);
            }

			if(bfromAtk) {
				if(item.defenseBP == 0) main3DMgr.KillYou = true;
				else main3DMgr.KillYou = false;
			} else {
				if(item.attackBP == 0) main3DMgr.KillYou = true;
				else main3DMgr.KillYou = false;
			}

            CMsgSkillCast cast = item.aHeader as CMsgSkillCast;
            int CastTime = cast.Mul >= 1 ? cast.Mul - 1 : 0;

            if(item.period == Period.DieSkill) {
                StartCoroutine(main3DMgr.ExploreSelf (bfromAtk, CastTime));
				yield return new WaitForSeconds(BanTimeCenter.F_EXPLORE_TIME);
				main3DMgr.Die(bfromAtk);
				main3DMgr.FlyDown(!bfromAtk, main3DMgr.KillYou);
            } else {
                main3DMgr.OverSkillAction_Ban (bfromAtk, CastTime);
				getReduceDownHp(item, bfromAtk);
				InvokeRepeating ("UpdateHpIfOverSkillAnim", 0.01f, 0.3f);
            }

            break;
        default :
            NormalSkillFight (item);
            break;
        }
    }

    #region 怒气技掉多次血--- 目前这个不再使用

	private int Overskill_0, Overskill_1, Overskill_2, Overskill_3;
	private bool LeftReduce = false;
	//攻击方的血量
	private int castHp;
	//有特殊的技能，初始Overskill_0攻击次数必须少
	private int OverSkill_0_Count = 0;

	void getReduceDownHp(Item item, bool bfromAtk) {
		int totalHp = 0;
		int curHp = 0;

		if(bfromAtk) {
			curHp = BanBattleManager.Instance.getHp(false);
			totalHp = curHp - item.defenseBP;
			castHp = BanBattleManager.Instance.getHp(true);

			LeftReduce = false;
		} else {
			curHp = BanBattleManager.Instance.getHp(true);
			totalHp = curHp - item.attackBP;
			castHp = BanBattleManager.Instance.getHp(false);

			LeftReduce = true;
		}

		Overskill_0 = curHp - MathHelper.MidpointRounding(totalHp * 0.2f);
		Overskill_1 = curHp - MathHelper.MidpointRounding(totalHp * 0.3f);
		Overskill_2 = curHp - MathHelper.MidpointRounding(totalHp * 0.5f);
		Overskill_3 = curHp - MathHelper.MidpointRounding(totalHp);

        if(item.skillId == 25004 || item.skillId == 25017)
			OverSkill_0_Count = 2;
		else 
			OverSkill_0_Count = 5;
	}

	private bool OverSkill_Start = false;
	private string mCurAnim = string.Empty;
	//调用
	void UpdateHpIfOverSkillAnim() {
		if(triggerPlayAnim != null) {   //攻击方

			OverSkill_Start = true;
			string curAnim = triggerPlayAnim.GetCurAnim();

			///
			/// ----- 跳过功能，经常会导致动画依然没有结束，所以必须判定这个状态 -----
			///
			BanBattleManager batMgr = BanBattleManager.Instance;

			if(curAnim == "OverSkill_0") {
				if(curAnim != mCurAnim) {
					mCurAnim = curAnim;

					if(LeftReduce) {
						if(batMgr != null) batMgr.UpdateHp(Overskill_0, castHp, BanTimeCenter.F_HP_ANIM_TIME, OverSkill_0_Count, false);
					} else {
						if(batMgr != null) batMgr.UpdateHp(castHp, Overskill_0, BanTimeCenter.F_HP_ANIM_TIME, OverSkill_0_Count, false);
					}
				}
			}


			if(curAnim == "OverSkill_1") {
				if(curAnim != mCurAnim) {
					mCurAnim = curAnim;

					if(LeftReduce) {
						if(batMgr != null) batMgr.UpdateHp(Overskill_1, castHp, BanTimeCenter.F_HP_ANIM_TIME, 1, false);
					} else {
						if(batMgr != null) batMgr.UpdateHp(castHp, Overskill_1, BanTimeCenter.F_HP_ANIM_TIME, 1, false);
					}
				}
			}

			if(curAnim == "OverSkill_2") {
				if(curAnim != mCurAnim) {
					mCurAnim = curAnim;

					if(LeftReduce) {
						if(batMgr != null) batMgr.UpdateHp(Overskill_2, castHp, BanTimeCenter.F_HP_ANIM_TIME, 1, false);
					} else {
						if(batMgr != null) batMgr.UpdateHp(castHp, Overskill_2, BanTimeCenter.F_HP_ANIM_TIME, 1, false);
					}
				}

			} 

		}
	}

	/// <summary>
	/// 普通技能和怒气技能
	/// </summary>
	void curAnimIsFlyGo() {
		//怒气技能的结算
		if(OverSkill_Start) {
			BanBattleManager batMgr = BanBattleManager.Instance;

			if(LeftReduce) {
				if(batMgr != null) batMgr.UpdateHp(Overskill_3, castHp, BanTimeCenter.F_HP_ANIM_TIME, 1, false);
			} else {
				if(batMgr != null) batMgr.UpdateHp(castHp, Overskill_3, BanTimeCenter.F_HP_ANIM_TIME, 1, false);
			}

			OverSkill_Start = false;
			CancelInvoke("UpdateHpIfOverSkillAnim");

			BattleHitMgr hitCCMgr = batMgr.hitCCMgr;
			hitCCMgr.HideWhenOverSkillStart();
		} else {

		}
	}

    #endregion

    private bool newPlayAnim = false;
    private bool isReplayed = false;
    //更新两方角色
	IEnumerator UpdateBothRole (int attackIndex, int defenseIndex, Item curItem, BanBattleManager battleMgr) {
        newPlayAnim = false;

        if(attackPBM == null || attackPBM.index != attackIndex || isReplayed) {
			BanBattleRole curBattleRole = battleMgr.GetBattleRole(attackIndex);

            int number = curBattleRole.number;
			CRLuo_PlayAnim_FX playAnim = battleMgr.main3DManager.Show_LAction(number);
			while((playAnim = battleMgr.main3DManager.Man_L) == null)
                yield return new WaitForSeconds(0.1f);

            playAnim.BodyFX_ON_OFF(curBattleRole.AllFated);

            attackPBM = new PetBattleModel(attackIndex, playAnim, curBattleRole);
			//登场的回合也要更新血量（OP18的技能是释放一个群体buff,作用是增加后面若干武者的战斗力)
			curBattleRole.attackBP = curItem.attackBP;

			if(attackIndex == 0)
				battleMgr.attackSideInfo.ReadFirstRole( curBattleRole );
			else 
				AsyncTask.QueueOnMainThread(() => { 
					if(Core.Data.temper.GiveUpBattle) return;  
					battleMgr.attackSideInfo.ReadFirstRole(curBattleRole);
				}, BanTimeCenter.F_MOVE_HEAD);
			if(attackIndex > 0)
				battleMgr.attackSideInfo.RemoveFirstRoleIcon(BanTimeCenter.F_HP_ANIM);

            newPlayAnim = true;
		} 

        if(defensePBM == null || defensePBM.index != defenseIndex || isReplayed){
			BanBattleRole curBattleRole = battleMgr.GetBattleRole(defenseIndex);
            int number = curBattleRole.number;

			if(curBattleRole.isBoss == (short)1) { //boss登场 
				yield return StartCoroutine(battleMgr.BossMgr.PlayAnim());
			}

			CRLuo_PlayAnim_FX playAnim = battleMgr.main3DManager.Show_RAction(number);
			while( (playAnim = battleMgr.main3DManager.Man_R) == null)
                yield return new WaitForSeconds(0.1f);

            playAnim.BodyFX_ON_OFF(curBattleRole.AllFated);
			playAnim.BodyFX_ON_OFF(curBattleRole.isBoss == (short)1);

            defensePBM = new PetBattleModel(defenseIndex, playAnim, curBattleRole);
			//登场的回合也要更新血量（OP18的技能是释放一个群体buff,作用是增加后面若干武者的战斗力)
			curBattleRole.defenseBP = curItem.defenseBP;

			if( defenseIndex == battleMgr.attackSideInfo.Count )
				battleMgr.defenseSideInfo.ReadFirstRole( curBattleRole );
			else
				AsyncTask.QueueOnMainThread(() => { 
					if(Core.Data.temper.GiveUpBattle) return;
					battleMgr.defenseSideInfo.ReadFirstRole(curBattleRole);
				}, BanTimeCenter.F_MOVE_HEAD);
			if( defenseIndex > battleMgr.attackSideInfo.Count )
				battleMgr.defenseSideInfo.RemoveFirstRoleIcon(BanTimeCenter.F_HP_ANIM);
            newPlayAnim = true;
		} 

		if(newPlayAnim) {
			//重置SkillIcon
			battleMgr.attackSideInfo.ssMgr.Attend();
			battleMgr.defenseSideInfo.ssMgr.Attend();
		}

        isReplayed = false;
    }


    private void checkAngryPoint(bool fromAttack, BanBattleManager battleMgr, Item curItem) {
        if(fromAttack){
            battleMgr.main3DManager.KillYou = (curItem.defenseBP == 0);

            if(battleMgr.attackSideInfo.angrySlot.curAP != curItem.attackAP)
                battleMgr.attackSideInfo.angrySlot.curAP = curItem.attackAP;
        }else{
            battleMgr.main3DManager.KillYou = (curItem.attackBP == 0);
            if(battleMgr.defenseSideInfo.angrySlot.curAP != curItem.defenseAP)
                battleMgr.defenseSideInfo.angrySlot.curAP = curItem.defenseAP;
        }
    }

    private void clearAngryPoint(){
        BanBattleManager.Instance.attackSideInfo.angrySlot.curAP = 0;
        BanBattleManager.Instance.defenseSideInfo.angrySlot.curAP = 0;
    }

    //展示下个阶段的怒气
	//这个暂时不用了
    private void showNextProgressItemAngry (int processIndex) {
        if(processIndex + 1 < list_Item.Count){
            Item nextItme = list_Item[processIndex + 1];
            if(nextItme != null){
				//怒气恢复的，有别的动画
				if(nextItme.extra_3 != Item.CostAndRecoverAngry) {

					//不能是奥义的添加怒气
					if(nextItme.period != Period.DragonAoYi) {
                        BanBattleManager.Instance.attackSideInfo.angrySlot.curAP = nextItme.attackAP;
                        BanBattleManager.Instance.defenseSideInfo.angrySlot.curAP = nextItme.defenseAP;
					} else {
						BanBattleRole fromRole = BanBattleManager.Instance.GetBattleRole (nextItme.extra_1);
						if(fromRole.group == BanBattleRole.Group.Attack) {
                            BanBattleManager.Instance.defenseSideInfo.angrySlot.curAP = nextItme.defenseAP;
						} else {
                            BanBattleManager.Instance.attackSideInfo.angrySlot.curAP = nextItme.attackAP;
						}
					}

				}


                
            }
            nextItme = null;
        }
    }


    /// <summary>
    /// Afters the battle item. 这是用来展示战后回血技能的，
    /// </summary>
    /// <returns>The battle item.</returns>
    /// <param name="processIndex">Process index.</param>
    /// <param name="curItem">Current item.</param>
    /// <param name="result">Result.</param>
    IEnumerator AfterBattleItem(int processIndex, Item curItem, int result) {
        BanBattleManager battleMgr = BanBattleManager.Instance;

		int length = list_Item.Count;
		int totalLoop = 2, curLoop = 1;


		while(processIndex + 1 < length && curLoop <= totalLoop) {

			Item nextItme = list_Item[processIndex + 1];
			if(nextItme != null && nextItme.period == Period.AfterBattle){

				if(nextItme.extra_3 != Item.Extra3_AddAP) {

					yield return new WaitForSeconds(BanTimeCenter.F_Free_Time);
					yield return StartCoroutine(ShowSkillAction(nextItme));

					curItem.attackBP = nextItme.attackBP;
					curItem.defenseBP = nextItme.defenseBP;

					battleMgr.UpdateHp(curItem.attackBP, curItem.defenseBP, BanTimeCenter.F_HP_ANIM_TIME, 1, false);

					yield return new WaitForSeconds(BanTimeCenter.F_DIE_IDLE * 2.5f);
				}

			}

			processIndex += 1;
			curLoop += 1;
		}

    }


    const int RESULT_NONE = 0;
    const int RESULT_ATTACKER_WIN = 1;
    const int RESULT_DEFENDER_WIN = 2;
    const int RESULT_BOTH_WIN = 3;
    /// <summary>
    /// 普通攻击技能.
    /// </summary>
    /// <param name="item">Item.</param>
    public void NormalSkillFight(Item item)
    {
        BanBattleRole fromRole = BanBattleManager.Instance.GetBattleRole(item.extra_1);

        bool fromAttack = fromRole.group == BanBattleRole.Group.Attack;

        BanBattleManager.Instance.main3DManager.SkillAction_Ban(fromAttack);
        if(GetPBM(item.extra_1)!=null)
            triggerPlayAnim = GetPBM(item.extra_1).playAnim;
        if(GetPBM(item.extra_2)!=null)
            triggerPlayAnim2 = GetPBM(item.extra_2).playAnim;

        if(fromAttack){
            BanBattleManager.Instance.main3DManager.KillYou = (item.defenseBP == 0);
        } else {
            BanBattleManager.Instance.main3DManager.KillYou = (item.attackBP == 0);
        }
    }

    /// <summary>
    /// 普通对打的时候，死亡技能有可能会先于对决出现
	/// 技能挨打的时候，挨打先于死亡技能
    /// </summary>
    /// <returns><c>true</c>, if skill was deathed, <c>false</c> otherwise.</returns>
    /// <param name="processIndex">Process index.</param>
    private IEnumerator showDeathSkill() {
        if(diePeriod != null && diePeriod.showDieSkill) {
			BanBattleManager battleMgr = BanBattleManager.Instance;
			BanBattleRole fromRole = battleMgr.GetBattleRole (diePeriod.item.extra_1);
			bool fromAttack = (fromRole.group == BanBattleRole.Group.Attack);

			if(fromAttack) battleMgr.attackSideInfo.ssMgr.SkillAnim(diePeriod.item.skillId);
			else battleMgr.defenseSideInfo.ssMgr.SkillAnim(diePeriod.item.skillId);

			//有可能是奥义的技能
			if(diePeriod.item.period == Period.DragonAoYi) {
				ShowInjureAction(fromAttack);
				yield return StartCoroutine(showAoYiSkill(battleMgr, diePeriod.item));

				int hp = battleMgr.getHp(!fromAttack);

				if(fromAttack)
					battleMgr.UpdateHp(diePeriod.item.attackBP, hp, BanTimeCenter.F_HP_ANIM_TIME);
				else
					battleMgr.UpdateHp(hp, diePeriod.item.defenseBP, BanTimeCenter.F_HP_ANIM_TIME);

				diePeriod = null;
			} else {

				battleMgr.main3DManager.KillYou = fromAttack ? (diePeriod.item.defenseBP == 0) : (diePeriod.item.attackBP == 0);

				yield return StartCoroutine(ShowSkillAction(diePeriod.item));

				if(diePeriod.item.period == Period.DieSkill) {
					yield return new WaitForSeconds(BanTimeCenter.WaitForDeathSkill_ReduceHp);
				}

				battleMgr.UpdateHp(diePeriod.item.attackBP, diePeriod.item.defenseBP, BanTimeCenter.F_HP_ANIM_TIME);

				diePeriod = null;
				yield return new WaitForSeconds(BanTimeCenter.WaitForDeathSkill_StandUp);
			}
           
        }
    }


	private IEnumerator AoYiAddAngryWhenEnemyDie(BanBattleManager battleMgr, Item curItem) {
		BanBattleRole fromRole = battleMgr.GetBattleRole (curItem.extra_1);
		bool fromAttack = (fromRole.group == BanBattleRole.Group.Attack);
		MonsterManager monMgr = Core.Data.monManager;
		MonsterData md = monMgr.getMonsterByNum(fromRole.number);
		int AngryUnit = md == null ? BT_WarUtils.Unit_Angry : md.nuqi2;

		if(fromAttack) {
			battleMgr.defenseSideInfo.angrySlot.curAP += AngryUnit;
		} else {
			battleMgr.attackSideInfo.angrySlot.curAP += AngryUnit;
		}

		yield return StartCoroutine(showAoYiSkill(battleMgr, curItem));

	}

    // 0 代表没有 -1 代表左边 1代表右边
	private int checkDeathSkill(int processIndex) {
        int check = 0;
        if(diePeriod != null && diePeriod.showDieSkill) {
            BanBattleManager battleMgr = BanBattleManager.Instance;
            BanBattleRole fromRole = battleMgr.GetBattleRole (diePeriod.item.extra_1);

            check = (fromRole.group == BanBattleRole.Group.Attack) ? -1 : 1;
		} else {

			if(processIndex + 1 < list_Item.Count) {
				Item nextItme = list_Item[processIndex + 1];
				if(nextItme != null && nextItme.period == Period.DieSkill){

					if(nextItme.VsMiddle == 0) { 
						diePeriod = new DieSkill();
						diePeriod.item = nextItme;
						diePeriod.showDieSkill = true;

						BanBattleManager battleMgr = BanBattleManager.Instance;
						BanBattleRole fromRole = battleMgr.GetBattleRole (diePeriod.item.extra_1);

						check = (fromRole.group == BanBattleRole.Group.Attack) ? -1 : 1;
					}

				}
			}
		}
        return check;
    }

	//下一个是否有死亡技能
	private bool checkNextDeathSkill(int processIndex) {
		bool nexthas = false;
		if(processIndex + 1 < list_Item.Count) {
			Item nextItme = list_Item[processIndex + 1];
			if(nextItme != null && nextItme.period == Period.DieSkill){
				if(nextItme.VsMiddle == 0) { 
					nexthas = true;
				}
			}
		}
		return nexthas;
	}

	// 0 代表没有 -1 代表左边 1代表右边
	private int UpdateDeathHp(Item curItem, int processindex) {

		int hasDeathSkill = checkDeathSkill(processindex);
		if( hasDeathSkill != 0) {
			if(hasDeathSkill == 1) {
				curItem.defenseBP = 0;
			} else {
				curItem.attackBP = 0;
			}
		} 
		return hasDeathSkill;
	}


    //恢复怒气的技能展示，需要展示出AngryPoint的改变
    private IEnumerator showRecoverAngry(Item curItem, bool fromAttack) {
        if(curItem.extra_3 == Item.CostAndRecoverAngry) {
            int preAngry = 0, targetAngry = 0;
            if(fromAttack) {
                preAngry = curItem.preAngry;
                targetAngry = curItem.attackAP;
            } else {
                preAngry = curItem.preAngry;
                targetAngry = curItem.defenseAP;
            }
            yield return StartCoroutine(showRecoverAngry(preAngry, targetAngry, fromAttack));
        }
    }

    //恢复怒气的技能展示，需要展示出AngryPoint的改变
    private IEnumerator showRecoverAngry(int preAngry, int targetAngry, bool fromAttackSide) {

        BanSideInfo side = null;
        if(fromAttackSide) {
            side = BanBattleManager.Instance.attackSideInfo;
        } else {
            side = BanBattleManager.Instance.defenseSideInfo;
        }
        side.angrySlot.curAP = preAngry;
        yield return new WaitForSeconds(BanTimeCenter.AddOneAngry + 0.5f);
        side.angrySlot.curAP = targetAngry;
        yield return new WaitForSeconds(BanTimeCenter.AddOneAngry + 0.5f);
    }

	//展示神龙奥义的技能
	private IEnumerator showAoYiSkill(BanBattleManager battleMgr, Item curItem) {
		BanBattleRole fromRole = battleMgr.GetBattleRole (curItem.extra_1); 
		bool bfromAtk = fromRole.group == BanBattleRole.Group.Attack;
		// --- 老版本的动画 ---
		//battleMgr.DragonAoYiAnim.DragonName = curItem.skillName;
		//battleMgr.DragonAoYiAnim.PlayDragon(fromRole.group == BanBattleRole.Group.Attack, curItem.skillAction);

		short enhanced = 0;
		if(curItem.aoyiConfig != null) {
			enhanced = curItem.aoyiConfig.enhanced;
		}

		// --- 新版本的动画 ---
		battleMgr.main3DManager.ForcedScreenOff = true;
		// --- 遗气奥义 为270011， 这个奥义因为是死亡时触发，则不应该展示出闪电或火焰
		bool skip = curItem.aoyiConfig.ID == 270011;
		bool death = false;
		if(skip == false) {
			if(curItem.aoyiConfig.ID == 270004) { // 270004 爆裂奥义，可能会导致对手死亡
				if(bfromAtk) {
					death = curItem.defenseBP == 0;
				} else {
					death = curItem.attackAP == 0;
				}
			}
		}

		battleMgr.AoyiMgr.PlayDragon(fromRole.group == BanBattleRole.Group.Attack, enhanced == 0, curItem.skillAction, curItem.skillName, skip, () => {
			if(curItem.aoyiConfig.ID == 270004) {
				battleMgr.main3DManager.FlyDown(!bfromAtk, death);

				if(death) {
					battleMgr.AoyiMgr.HideFireAndLight(!bfromAtk);
				}
			}

			if(curItem.extra_3 == Item.Extra3_AddAP || curItem.extra_3 == Item.Extra3_AoYiAngry) {
				checkAngryPoint(fromRole.group == BanBattleRole.Group.Attack, battleMgr, curItem);
			} else if(curItem.extra_3 == Item.Extra3_Attribute_CHG) {
				//do nothing ...
			} else {
				battleMgr.UpdateHp(curItem.attackBP, curItem.defenseBP, BanTimeCenter.F_REDUCE_HP);
			}
		});

		if(fromRole.group == BanBattleRole.Group.Attack){
			triggerPlayAnim = attackPBM.playAnim;
		} else {
			triggerPlayAnim = defensePBM.playAnim;
		}

		yield return new WaitForSeconds(BanTimeCenter.F_Wait_For_AoYi);

		battleMgr.main3DManager.ForcedScreenOff = false;
	}

	//展示复活技能
	private IEnumerator showReviveSkill(bool fromAttack, Item curItem, BanBattleManager battleMgr){
        CMsgSkillRevive item = curItem.aHeader as CMsgSkillRevive;

		if(fromAttack)
			battleMgr.attackSideInfo.ReviveRole(item.reviveArr);
		else
			battleMgr.defenseSideInfo.ReviveRole(item.reviveArr, battleMgr.attackSideInfo.Count);

		yield return StartCoroutine(ShowSkillAction (curItem));

		yield return new WaitForSeconds(BanTimeCenter.F_Attribute_Time);
	}

	//展示复活奥义技能
	private IEnumerator showReviveAoYi(Item curItem, BanBattleManager battleMgr) {
		BanBattleRole fromRole = battleMgr.GetBattleRole (curItem.extra_1); 
		bool fromAttack = fromRole.group == BanBattleRole.Group.Attack; 

		CMsgSkillRevive item = curItem.aHeader as CMsgSkillRevive;

		if(fromAttack)
			battleMgr.attackSideInfo.ReviveRole(item.reviveArr);
		else
			battleMgr.defenseSideInfo.ReviveRole(item.reviveArr, battleMgr.attackSideInfo.Count);

		yield return StartCoroutine(showAoYiSkill(battleMgr, curItem));
	}

	//当某一方挂掉的时候
	//有的时候死亡技能会还没有出现，所以要预读取下一个
	private IEnumerator SomeOneDie(BanBattleManager battleMgr, int result, Item curItem, int processIndex) {

		///
		/// 更新result变量
		///
		if(curItem.period != Period.NormalAttack) {
			result = curItem.attackBP  <= 0 ? RESULT_DEFENDER_WIN : RESULT_NONE;
			if(result != RESULT_DEFENDER_WIN)
				result = curItem.defenseBP <= 0 ? RESULT_ATTACKER_WIN : RESULT_NONE;
		}

		//隐藏Combo数量
		battleMgr.hitCCMgr.HideWhenOverSkillOver(result, curItem);

		triggerPlayAnim2 = null;
		//更新血量（如果有死亡技能）
		int hasDeathSkill = UpdateDeathHp(curItem, processIndex);

		if(!skipUpdateHp)
			battleMgr.UpdateHp(curItem.attackBP, curItem.defenseBP, BanTimeCenter.F_HP_ANIM_TIME, 1, hasDeathSkill == 0);

		skipUpdateHp = false;

		if(hasDeathSkill != 0)
			yield return StartCoroutine(showDeathSkill());

		#if VS
		if(result == RESULT_ATTACKER_WIN || result == RESULT_DEFENDER_WIN) {
			TimeMgr.getInstance().setExtLine(BanTimeCenter.Scale_Down_If_Die);
			yield return new WaitForSeconds(BanTimeCenter.F_DIE_GOHST_PH1);
			TimeMgr.getInstance().revertToBaseLine();
			yield return new WaitForSeconds(BanTimeCenter.F_DIE_GOHST_PH2);
		} else {
			yield return new WaitForSeconds(BanTimeCenter.F_DIE_GOHST);
		}
		#else
		yield return new WaitForSeconds(BanTimeCenter.F_DIE_GOHST);
		#endif

		//如果左边赢，则为True。右边赢则为False
		if(result == RESULT_ATTACKER_WIN || result == RESULT_DEFENDER_WIN) {
			battleMgr.AoyiMgr.HideFireAndLight(result == RESULT_DEFENDER_WIN);

			//这个时候会死亡一方
			battleMgr.GhostMgr.createGhostly();

			Transform whoDie = null;
            Vector3   Pos    = Vector3.zero;
			if(result == RESULT_ATTACKER_WIN) {
                if(battleMgr.main3DManager.Man_R != null) {
                    whoDie = battleMgr.main3DManager.Man_R.transform;
					Pos    = battleMgr.defenseSideInfo.AngerTarget.position;

					BanBattleRole defRole = battleMgr.GetBattleRole(curItem.defenseIndex);
					MonsterData md = defRole == null ? null : Core.Data.monManager.getMonsterByNum(defRole.number);
					DieAp = md == null ? BT_WarUtils.Unit_Angry : md.nuqi2;
					/// 播放加怒气的动画
					Invoke("defAp", 2.3f);
                }
			} else {
                if(battleMgr.main3DManager.Man_L != null) {
                    whoDie = battleMgr.main3DManager.Man_L.transform;
					Pos    = battleMgr.attackSideInfo.AngerTarget.position;

					BanBattleRole attRole = battleMgr.GetBattleRole(curItem.attackIndex);
					MonsterData md = attRole == null ? null : Core.Data.monManager.getMonsterByNum(attRole.number);
					DieAp = md == null ? BT_WarUtils.Unit_Angry : md.nuqi2;
					/// 播放加怒气的动画
					Invoke("attAp", 2.3f);
                }
			}

			//播放KO动画
			yield return new WaitForSeconds(BanTimeCenter.F_KO);
			//播放KO动画
			battleMgr.koMgr.playAnim();
			if(whoDie != null) battleMgr.GhostMgr.ShowGhostFly(whoDie, Pos);

			///
			/// 在boss赢的时候， 有可能不展示win的动画
			///
			if(result == RESULT_ATTACKER_WIN) {
				BanBattleRole curBattleRole = battleMgr.GetBattleRole(curItem.defenseIndex);
				if(curBattleRole != null) {
					if(curBattleRole.isBoss != (short) 1) {
						battleMgr.main3DManager.Win(true);
					}
				} else {
					battleMgr.main3DManager.Win(true);
				}
			}
				
			else if(result == RESULT_DEFENDER_WIN)
				battleMgr.main3DManager.Win(false);

			yield return new WaitForSeconds(BanTimeCenter.F_DIE_GOHST);
		}
			
	}

	private int DieAp = 0;

	void attAp() {
		BanBattleManager battleMgr = BanBattleManager.Instance;
		if(DieAp > 0)
			battleMgr.attackSideInfo.angrySlot.curAP += DieAp;
		DieAp = 0;
	}

	void defAp() {
		BanBattleManager battleMgr = BanBattleManager.Instance;
		if(DieAp > 0)
			battleMgr.defenseSideInfo.angrySlot.curAP += DieAp;
		DieAp = 0;
	}



	/// <summary>
	/// 神龙奥义 --- 属性改变
	/// </summary>
	/// <returns>The yi attribute changed.</returns>
	/// <param name="battleMgr">Battle mgr.</param>
	/// <param name="curItem">Current item.</param>
	private IEnumerator AoYiAttributeChanged (BanBattleManager battleMgr, Item curItem) {
		BanBattleRole fromRole = battleMgr.GetBattleRole (curItem.extra_1); 
		bool fromAttack = fromRole.group == BanBattleRole.Group.Attack; 

		yield return StartCoroutine(showAoYiSkill(battleMgr, curItem));

		if(fromAttack) {
			battleMgr.attackSideInfo.ChangeToAllAttribute();
			battleMgr.GetBattleRole(curItem.attackIndex).attribute = BanBattleRole.Attribute.Quan;
		} else {
			battleMgr.defenseSideInfo.ChangeToAllAttribute();
			battleMgr.GetBattleRole(curItem.defenseIndex).attribute = BanBattleRole.Attribute.Quan;
		}

		battleMgr.main3DManager.ChangeAttribute_Ban (fromAttack);    
	}

	private bool skipUpdateHp = false;
	//普通技能
	private IEnumerator NormalSkill_Extra3Else (Item curItem, BanBattleManager battleMgr, bool fromAttack) {
		if(curItem.extra_1 != -1 && GetPBM(curItem.extra_1) != null)
			triggerPlayAnim = GetPBM(curItem.extra_1).playAnim;
		if(curItem.extra_2 != -1 && GetPBM(curItem.extra_2) != null)
			triggerPlayAnim2 = GetPBM(curItem.extra_2).playAnim;
		yield return StartCoroutine(ShowSkillAction (curItem));


		if(fromAttack){
			battleMgr.main3DManager.KillYou = (curItem.defenseBP == 0);
		} else {
			battleMgr.main3DManager.KillYou = (curItem.attackBP == 0);
		}

		switch(curItem.extra_3) {
		case Item.Extra3_Combo://连击
			if(curItem.skillId == 21049) { // 多等个1秒
				yield return new WaitForSeconds(1f);
				skipUpdateHp = true;
			} else if(curItem.skillId == 21007) { // 多等个5秒
				yield return new WaitForSeconds(5f);
				skipUpdateHp = true;
			}
			battleMgr.UpdateHp(curItem.attackBP, curItem.defenseBP, BanTimeCenter.F_REDUCE_HP, 1, false);
			yield return new WaitForSeconds(BanTimeCenter.F_REDUCE_HP);
			break;
		case Item.Extra3_DoubleIdle: //需要多等等待一会儿
			yield return new WaitForSeconds(BanTimeCenter.F_Wait_For_Show_SkillName);
			break;
		case Item.Extra3_AddAP://有时候会有怒气的更新
			checkAngryPoint(fromAttack, battleMgr, curItem);
			break;
		case Item.Extra3_Skill_Type_Suck://吸取技能
			yield return new WaitForSeconds(BanTimeCenter.F_Wait_For_SuckBlood);
			break;
		case Item.Extra3_LongSkill : //这个必须很特殊的技能
			if(curItem.skillId == 21174) {
				yield return new WaitForSeconds(BanTimeCenter.WaitWuLong);
				battleMgr.UpdateHp(curItem.attackBP, curItem.defenseBP, BanTimeCenter.F_REDUCE_HP, 1, false);
				yield return new WaitForSeconds(BanTimeCenter.F_LongSkill - 2);
			} else {
				battleMgr.UpdateHp(curItem.attackBP, curItem.defenseBP, BanTimeCenter.F_Combo_HP, 1, false);
			}

			break;
		default:
			yield return new WaitForSeconds(BanTimeCenter.F_Wait_For_CommonSkill);
			break;
		}
	}

	//普通技能，PowerUP类型的处理
	private IEnumerator NormalSkill_PowerUp(Item curItem, BanBattleManager battleMgr, bool fromAttack) {
		BanBattleRole toRole = null; 

		if (curItem.extra_2 != -1) { 
			toRole = battleMgr.GetBattleRole(curItem.extra_2); 
			triggerPlayAnim2 = GetPBM (curItem.extra_2).playAnim; 
		} 

		battleMgr.main3DManager.Free1_Ban(fromAttack); 
		showSkillName(curItem);
		if (toRole != null) { 
			battleMgr.main3DManager.Free2_Ban(battleMgr.GetBattleRole(toRole.index).group == BanBattleRole.Group.Attack); 
		}

		if(fromAttack){
			battleMgr.main3DManager.KillYou = (curItem.defenseBP == 0);
			//理论上怒气技能，才会减少怒气，但是发现有BuffDebuff技能也会减少怒气
            if(curItem.skillType == CSkill_Type.Anger || curItem.attackAP != battleMgr.defenseSideInfo.angrySlot.curAP)
                battleMgr.attackSideInfo.angrySlot.curAP = curItem.attackAP;
		} else {
			battleMgr.main3DManager.KillYou = (curItem.attackBP == 0);
			//理论上怒气技能，才会减少怒气，但是发现有BuffDebuff技能也会减少怒气
            if(curItem.skillType == CSkill_Type.Anger || curItem.defenseAP != battleMgr.defenseSideInfo.angrySlot.curAP)
                battleMgr.defenseSideInfo.angrySlot.curAP = curItem.defenseAP;
		}

		triggerPlayAnim = GetPBM(curItem.extra_1).playAnim; 
		battleMgr.UpdateHp(curItem.attackBP,curItem.defenseBP, BanTimeCenter.F_REDUCE_HP); 
		yield return new WaitForSeconds(BanTimeCenter.F_Attribute_Time);
	}

	//怒气技能,更新状态（血量，怒气值）
	private IEnumerator AngrySkill_Attack(Item curItem, BanBattleManager battleMgr, bool fromAttack) {
		//如果恢复怒气技，则立刻修改
		if(curItem.extra_3 == Item.CostAndRecoverAngry)
			checkAngryPoint(fromAttack, battleMgr, curItem);

        //属于自我增强吗？
		bool selfEnhance = false;
		PetBattleModel pbModel = GetPBM(curItem.extra_1);
		if(pbModel != null) {
			triggerPlayAnim = pbModel.playAnim;
		} 

		pbModel = GetPBM(curItem.extra_2);
		if(pbModel != null) {
			triggerPlayAnim2 = pbModel.playAnim;
			selfEnhance = false;
		} else {
			selfEnhance = true;
		}

		if(curItem.extra_3 == Item.Extra3_Combo) {
			if(!OverSkill_Start) {
				battleMgr.UpdateHp(curItem.attackBP, curItem.defenseBP, BanTimeCenter.F_HP_ANIM_TIME, curItem.lunCount, true);
			}
		} else {
			if(selfEnhance) {
				if(!OverSkill_Start) {
					battleMgr.UpdateHp(curItem.attackBP, curItem.defenseBP, BanTimeCenter.F_REDUCE_HP); 
					yield return new WaitForSeconds(BanTimeCenter.F_REDUCE_HP);
				}
            } 

			yield return StartCoroutine(showRecoverAngry(curItem, fromAttack));
		}

        float waitOfTime = 0f;
		if(selfEnhance)
			waitOfTime = BanTimeCenter.F_SELF_ENHANCE;
		else {
            waitOfTime = BanTimeCenter.F_Attribute_Time;
		}

        yield return new WaitForSeconds(waitOfTime);
	}

	//boss掉落的方向, true = left, false = right
	bool BossDropDirection() {
		bool direction = false;

		int index = list_Item.Count - 2;
		if(index >= 0) {
			Item PreItem = list_Item[index];
			if(PreItem != null) {
				direction = PreItem.period == Period.NormalAttack;
			}
		}

		return direction;
	}

	//创建boss 掉落位置
	GameObject createDropGameObj(bool direction, Main3DManager m3DMgr) {
		float Dis = 3.8f;
		GameObject anchor = new GameObject();
		anchor.name = "DropPos";
		if(direction) {
			anchor.transform.localPosition = new Vector3(0f, 0f, -1f);
		} else {
			if (m3DMgr.Man_L != null)
				anchor.transform.localPosition = m3DMgr.Man_L.transform.localPosition + new Vector3 (0f, 0f, Dis);
			else {
				anchor.transform.localPosition = new Vector3 (0f, 0f, Dis);
			}
		}
		return anchor;
	}

	//掉落龙珠的动画
	void dropBallAnim(GameObject father, bool isInleftArea = true) {
		Object obj = PrefabLoader.loadFromUnPack("Ban/DropBall", false);
		GameObject ball = Instantiate(obj) as GameObject;
		RED.AddChildResvere(ball, father);

		if(isInleftArea) ball.transform.eulerAngles = new Vector3(0f, 0f, 0f);
		Core.Data.soundManager.SoundFxPlay(SoundFx.FX_DragonBeadDrop);
	}

	//掉落Box的动画
	void dropBoxAnim(GameObject start, bool isInleftArea, Main3DManager m3DMgr) {
		Object obj = PrefabLoader.loadFromUnPack("Ban/DropBox", false);
		GameObject box = Instantiate(obj) as GameObject;

		Vector3 endPos = Vector3.zero;
		if(isInleftArea) {
			if (m3DMgr.Man_L != null) {
				endPos = m3DMgr.Man_L.transform.localPosition - new Vector3 (0f, 0f, 1f);
			}
			else {
				endPos = Vector3.back;
			}
		} else {
			if (m3DMgr.Man_L != null) {
				endPos = m3DMgr.Man_L.transform.localPosition + new Vector3 (0f, 0f, 1f);
			} else { 
				endPos = Vector3.forward;
			}
		}

		box.transform.localPosition = start.transform.localPosition;
		MiniItween.MoveTo(box, endPos, 0.15f);

		Core.Data.soundManager.SoundFxPlay(SoundFx.FX_DragonBeadDrop);
	}

    #region 简化HandleItemIE里面的Switch case

    /// <summary>
    /// 登场的阶段
    /// </summary>
    private IEnumerator AttendPeriod(Item curItem, BanBattleManager battleMgr) {
        bool isChanged = false;
        //登场的回合也可能有怒气
        if(battleMgr.attackSideInfo.angrySlot.curAP != curItem.attackAP) {
            isChanged = true;
            battleMgr.attackSideInfo.angrySlot.curAP = curItem.attackAP;
        }

        if(battleMgr.defenseSideInfo.angrySlot.curAP != curItem.defenseAP) {
            isChanged = true;
            battleMgr.defenseSideInfo.angrySlot.curAP = curItem.defenseAP;
        }

        if(isChanged)
            yield return new WaitForSeconds(BanTimeCenter.F_Attend);
    }

    /// <summary>
    /// 神龙奥义的阶段
    /// </summary>
    private IEnumerator AoYiPeriod(Item curItem, BanBattleManager battleMgr) {
        //虽然是奥义技能，但是也可能是DieSkill，如果是死亡技能，则有可能是夹在两个互殴的片段之间
        //所以，这里我们也必须特殊处理一下。
        if(curItem.extra_3 == Item.Extra3_Explore || curItem.extra_3 == Item.Extra3_AoYiAngry) {
            diePeriod = new DieSkill();
            diePeriod.item = curItem;
            diePeriod.showDieSkill = true;
        } else if(curItem.extra_3 == Item.Extra3_Skill_Type_Revive) {
            yield return StartCoroutine(showReviveAoYi(curItem, battleMgr));
        } else if(curItem.extra_3 == Item.Extra3_Recover){
            if(curItem.attackBP > battleMgr.attackSideInfo.maxBp)
                battleMgr.attackSideInfo.maxAndCurBp = curItem.attackBP;

            if(curItem.defenseBP > battleMgr.defenseSideInfo.maxBp)
                battleMgr.defenseSideInfo.maxAndCurBp = curItem.defenseBP;
            yield return StartCoroutine(showAoYiSkill(battleMgr, curItem));
        } else if(curItem.extra_3 == Item.Extra3_AddAP) {
            //会增加死亡方的怒气
            yield return StartCoroutine(AoYiAddAngryWhenEnemyDie(battleMgr, curItem));
        } else if(curItem.extra_3 == Item.Extra3_Attribute_CHG) {
            yield return StartCoroutine(AoYiAttributeChanged(battleMgr, curItem));
        } else {
            yield return StartCoroutine(showAoYiSkill(battleMgr, curItem));
        }
    }

    /// <summary>
    /// 属性克制被忽略
    /// </summary>
    private IEnumerator AttributeIgnorePeriod(Item curItem, BanBattleManager battleMgr) {
        BanBattleRole fromRole = battleMgr.GetBattleRole (curItem.extra_1); 
        bool fromAttack = fromRole.group == BanBattleRole.Group.Attack; 

		//展示技能Icon
		if(fromAttack) battleMgr.attackSideInfo.ssMgr.SkillAnim(curItem.skillId);
		else battleMgr.defenseSideInfo.ssMgr.SkillAnim(curItem.skillId);

        showSkillName(curItem);

        battleMgr.main3DManager.Free1_Ban(fromAttack); 
        yield return new WaitForSeconds(BanTimeCenter.F_Attribute_Time);
    }

    /// <summary>
    /// 属性改变的方法 -- 非神龙奥义
    /// </summary>
    /// <param name="battleMgr">Battle mgr.</param>
    /// <param name="curItem">Current item.</param>
    private IEnumerator AttributeChanged (BanBattleManager battleMgr, Item curItem) {
        BanBattleRole fromRole = battleMgr.GetBattleRole (curItem.extra_1); 
        bool fromAttack = fromRole.group == BanBattleRole.Group.Attack; 

		//展示技能Icon
		if(fromAttack) battleMgr.attackSideInfo.ssMgr.SkillAnim(curItem.skillId);
		else battleMgr.defenseSideInfo.ssMgr.SkillAnim(curItem.skillId);

        showSkillName(curItem);

        if(fromAttack) {
            battleMgr.attackSideInfo.ChangeToAllAttribute();
            battleMgr.GetBattleRole(curItem.attackIndex).attribute = BanBattleRole.Attribute.Quan;
        } else {
            battleMgr.defenseSideInfo.ChangeToAllAttribute();
            battleMgr.GetBattleRole(curItem.defenseIndex).attribute = BanBattleRole.Attribute.Quan;
        }

        battleMgr.main3DManager.Free1_Ban(fromAttack); 
        yield return new WaitForSeconds(BanTimeCenter.F_Attribute_Time);
        battleMgr.main3DManager.ChangeAttribute_Ban (fromAttack);    
        yield return new WaitForSeconds(BanTimeCenter.F_Attribute_Time);
    }

    /// <summary>
    /// 属性克制的状态
    /// </summary>
    private IEnumerator AttributeKillPeriod(Item curItem, BanBattleManager battleMgr) {
        if(curItem.extra_3 != Item.Extra3_UpdateAP) {
            BanBattleRole attackRole = battleMgr.GetBattleRole(curItem.attackIndex);
            BanBattleRole defenseRole = battleMgr.GetBattleRole(curItem.defenseIndex);
			bool hasConflict = battleMgr.GenerateAttributeConflict(attackRole.attribute,defenseRole.attribute);
			if(hasConflict) {
				yield return new WaitForSeconds(BanTimeCenter.F_Attribute_Time);
				int state = BanTools.GetAttributeState(attackRole.attribute, defenseRole.attribute);

				if(state == 1){
					battleMgr.main3DManager.Conflict_Provoke(true);
					triggerPlayAnim = attackPBM.playAnim;
				} else if(state == -1) {
					battleMgr.main3DManager.Conflict_Provoke(false);
					triggerPlayAnim = defensePBM.playAnim;
				}

				if(curItem.attackBP > battleMgr.attackSideInfo.maxBp)
					battleMgr.attackSideInfo.maxAndCurBp = curItem.attackBP;

				if(curItem.defenseBP > battleMgr.defenseSideInfo.maxBp)
					battleMgr.defenseSideInfo.maxAndCurBp = curItem.defenseBP;

				battleMgr.UpdateHpIfAttriConflict(curItem.attackBP, curItem.defenseBP);
				yield return new WaitForSeconds(BanTimeCenter.F_REDUCE_HP);
			}
        }
    }
		
    /// <summary>
    /// 死亡技能
    /// </summary>
    private void DieSkillPeriod(Item curItem){
        ConsoleEx.DebugLog("---- Die skill ----");
        diePeriod = new DieSkill();
        diePeriod.item = curItem;
        diePeriod.showDieSkill = true;
    }

    /// <summary>
    /// 怒气技能阶段
    /// </summary>
    private IEnumerator AngrySkillPeriod(Item curItem, BanBattleManager battleMgr) {
		if(curStep == GuideStep.XiaoWuKong_OverSkill1) yield return new WaitForSeconds(2f);

        BanBattleRole fromRole = battleMgr.GetBattleRole(curItem.extra_1);
        bool fromAttack = fromRole.group == BanBattleRole.Group.Attack;

        if(curItem.skillAction == 8) OverSkill_Start = true;
        else OverSkill_Start = false;

		TemporyData temp = Core.Data.temper;
		///
		///   当首次PVE的战斗的时候，出现“点击屏幕任意按钮”,
		///   如果有LOCAL_AUTO的宏，则不走这部分逻辑
		///

		#if !LOCAL_AUTO
		if(fromAttack) {
			if(temp.currentBattleType == TemporyData.BattleType.BossBattle || temp.currentBattleType == TemporyData.BattleType.FightWithFulisa
				||temp.currentBattleType == TemporyData.BattleType.FinalTrialShalu || temp.currentBattleType == TemporyData.BattleType.FinalTrialBuou) {
				BattleHitMgr hitCCMgr = battleMgr.hitCCMgr;
				if(!temp.hasLiquidate) { //不是战斗重放, 就是首次打战斗

					int price = 0; //新手引导的战斗不给金币

					if(temp.currentBattleType == TemporyData.BattleType.BossBattle) { //普通副本，
						NewDungeonsManager mgr = Core.Data.newDungeonsManager;
						if(mgr != null && mgr.curFightingFloor != null && mgr.curFightingFloor.config != null)
							price = mgr.curFightingFloor.config.Cprice;
					} else { //沙鲁布欧的副本
						TrialEnum type = temp.currentBattleType == TemporyData.BattleType.FinalTrialShalu ? TrialEnum.TrialType_ShaLuChoose : TrialEnum.TrialType_PuWuChoose;
						price = FinalTrialMgr.GetInstance().GetMapComboCoin(type);
					}

					hitCCMgr.ShowPressButton(price);
				} else {

				}

			}
		}
		#endif

        ///
        ///   当PVP的战斗和PVE战斗的回放， 新手引导的时候也不应该播放
        ///

		if(fromAttack && temp.currentBattleType != TemporyData.BattleType.FightWithFulisa ) {
            if(temp.hasLiquidate || temp.currentBattleType != TemporyData.BattleType.BossBattle) {
                CMsgSkillCast skillmsg = curItem.aHeader as CMsgSkillCast;
				battleMgr.attackSideInfo.PlayerAngryWord.AutoShowCount(skillmsg.Mul == 0 ? 1 : skillmsg.Mul, curItem.skillName);
            }
        }

		///
		/// ------------- 右边出现怒气技的释放次数 -----------
		///
		if(!fromAttack) {
			CMsgSkillCast skillmsg = curItem.aHeader as CMsgSkillCast;
			battleMgr.defenseSideInfo.PlayerAngryWord.AutoShowCount(skillmsg.Mul == 0 ? 1 : skillmsg.Mul, curItem.skillName);

			//展示技能Icon
			battleMgr.defenseSideInfo.ssMgr.SkillAnim(curItem.skillId);
		}

		///
		/// 使用怒气技能的时候就减少怒气值，但是如果是普通PVE的客户端计算的战斗，只更新右边的怒气
		///
		if(curItem.extra_3 != Item.CostAndRecoverAngry) {
			if(temp.currentBattleType == TemporyData.BattleType.BossBattle) {
				if(fromAttack == false) { //仅仅是右边的时候更新怒气
					battleMgr.defenseSideInfo.angrySlot.curAP = curItem.defenseAP;
				}
				#if LOCAL_AUTO
				ConsoleEx.DebugLog("ap =" + curItem.attackAP);
				battleMgr.attackSideInfo.angrySlot.curAP = curItem.attackAP;
				#endif
			} else {
				battleMgr.attackSideInfo.angrySlot.curAP  = curItem.attackAP ;
				battleMgr.defenseSideInfo.angrySlot.curAP = curItem.defenseAP;
			}
		}

        //播放怒气技能动画
        yield return StartCoroutine(ShowSkillAction (curItem));

        yield return StartCoroutine(AngrySkill_Attack(curItem, battleMgr, fromAttack));
    }

    /// <summary>
    /// 普通技能阶段
    /// </summary>
    private IEnumerator NormalSkillPeriod(Item curItem, BanBattleManager battleMgr) {
        BanBattleRole fromRole = battleMgr.GetBattleRole(curItem.extra_1); 
        bool fromAttack = fromRole.group == BanBattleRole.Group.Attack;

		if(fromAttack) battleMgr.attackSideInfo.ssMgr.SkillAnim(curItem.skillId);
		else battleMgr.defenseSideInfo.ssMgr.SkillAnim(curItem.skillId);

        if(curItem.extra_3 == Item.Extra3_Skill_Type_PowerUp) { 
            yield return StartCoroutine(NormalSkill_PowerUp(curItem, battleMgr, fromAttack));
        } else if(curItem.extra_3 == Item.Extra3_Skill_Type_Revive) {
            yield return StartCoroutine(showReviveSkill(fromAttack, curItem, battleMgr));
        } else if(curItem.extra_3 == Item.Extra3_Recover) {
            battleMgr.main3DManager.Free1_Ban(fromAttack);
            showSkillName(curItem);

            battleMgr.UpdateHp(curItem.attackBP,curItem.defenseBP, BanTimeCenter.F_REDUCE_HP); 

            if(curItem.attackBP > battleMgr.attackSideInfo.maxBp)
                battleMgr.attackSideInfo.maxAndCurBp = curItem.attackBP;

            if(curItem.defenseBP > battleMgr.defenseSideInfo.maxBp)
                battleMgr.defenseSideInfo.maxAndCurBp = curItem.defenseBP;

            yield return new WaitForSeconds(BanTimeCenter.F_WAIT_RECOVER);
        } else { //Extra3_Skill_Type_Attack or something else
            yield return StartCoroutine(NormalSkill_Extra3Else(curItem, battleMgr, fromAttack));
        }
    }

    /// <summary>
    /// 战后技能阶段 
    /// </summary>

    private IEnumerator AfterBattlePeriod(Item curItem, BanBattleManager battleMgr) {
        //有时候会有怒气的更新
        if(curItem.extra_3 == Item.Extra3_AddAP) {
            BanBattleRole fromRole = battleMgr.GetBattleRole(curItem.extra_1);
            bool fromAttack = fromRole.group == BanBattleRole.Group.Attack;
            checkAngryPoint(fromAttack, battleMgr, curItem);

			if(fromAttack) battleMgr.attackSideInfo.ssMgr.SkillAnim(curItem.skillId);
			else battleMgr.defenseSideInfo.ssMgr.SkillAnim(curItem.skillId);
            
            yield return StartCoroutine(ShowSkillAction(curItem));
            yield return new WaitForSeconds(BanTimeCenter.F_Attribute_Time);
        }
    }

    /// <summary>
    /// 怒气技准备好的阶段, 如果怒气值不足以支持下一个怒气技能的释放，则提前结束
    /// </summary>

    private IEnumerator AngrySkillReadyPeriod(Item curItem, BanBattleManager battleMgr) {
        CMsgAngryReady angrdy = curItem.aHeader as CMsgAngryReady;
        SkillData sd = Core.Data.skillManager.getSkillDataConfig(angrdy.SkillId); 
        battleMgr.AngryUI(true, sd, angrdy.Count);
        //右边进入暴怒气阶段
        battleMgr.main3DManager.Free1_Ban (true);
		TimeMgr.getInstance().setExtLine(BanTimeCenter.Scale_Down_To);

		bool flag = true;
		float curTime = 0f;
		do {
			yield return new WaitForSeconds(BanTimeCenter.Wait_Per_Unit);
			curTime += BanTimeCenter.Wait_Per_Unit;

			if(battleMgr != null && battleMgr.attackSideInfo != null)
				flag = battleMgr.attackSideInfo.hasNextClick && curTime < BanTimeCenter.Wait_For_User_Cast_AngrySk;
			else 
				flag = false;
		}
		while(flag);
        
		TimeMgr.getInstance().revertToBaseLine();
        //关闭UI
		if(curTime < BanTimeCenter.Wait_If_Time_Is_Little)
			yield return new WaitForSeconds(BanTimeCenter.Wait_If_Time_Is_Little - curTime);
        battleMgr.AngryUI(false, null, -1);
    }

    /// <summary>
    /// 战斗结束阶段
    /// </summary>

	void EndOfWarPeriod(Item curItem, BanBattleManager battleMgr) {
        CMsgWarEnd end = curItem.aHeader as CMsgWarEnd;
        if(end != null) {
			battleMgr.battleWin = end.winner.ToLower() == "att";

			///
			/// 战斗统计, 理论上是只有PVE才有
			///
			TemporyData temp = Core.Data.temper;
			if(temp.currentBattleType == TemporyData.BattleType.BossBattle ||
				temp.currentBattleType == TemporyData.BattleType.FinalTrialBuou ||
				temp.currentBattleType == TemporyData.BattleType.FinalTrialShalu) {

				if(battleMgr.War != null) battleMgr.War.SettleBattle();
			}

			///
			/// 转动赢家
			///
			Main3DManager m3DMgr = battleMgr.main3DManager;
			if(m3DMgr != null) {
				CRLuo_PlayAnim_FX winn = battleMgr.battleWin ? m3DMgr.Man_L : m3DMgr.Man_R;
				if(winn != null) {
					GameObject winner = winn.gameObject;
					winner.transform.localRotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
					m3DMgr.Win(battleMgr.battleWin);
				}
			}

			///
			/// Boss掉落 --- 只有PVE有，其他没有掉落
			///
			if(battleMgr.battleWin) {

				if(temp.currentBattleType == TemporyData.BattleType.BossBattle 
					|| temp.currentBattleType == TemporyData.BattleType.FinalTrialShalu
					|| temp.currentBattleType == TemporyData.BattleType.FinalTrialBuou )
				{
					//boss掉落方向
					bool direction = BossDropDirection();

					//先掉落龙珠
					int count = battleMgr.getBossDropBall;
					GameObject anchor = createDropGameObj(direction, m3DMgr);

					if(count > 0) {
						dropBallAnim(anchor, direction);
					}

					//再掉落普通物品
					count = battleMgr.getBossOtherDrop;
					if(count >= 0) {
						dropBoxAnim(anchor, direction, m3DMgr);
					}
				}
			}

        }

		battleMgr.SpeedOrSkipMgr.SpeedUpAndAutoSave();
    }

    #endregion




	/// <summary>
	/// 用来控制跳过战斗动画
	/// </summary>
	public bool skip = false;

    IEnumerator HandleItemIE(){
        yield return null;

        BanBattleManager battleMgr = BanBattleManager.Instance;
        bool warEnd                = false;
        bool AngrySkillReady       = false;
		isReplayed                 = battleMgr == null ? false : battleMgr.IsReplay;

        for(int processIndex = 0; processIndex < list_Item.Count; processIndex ++) {
			//如果按下跳过，则不应该继续
			if(skip) break;
            BanTimeCenter.F_HP_ANIM = 0.5f;

            Item curItem = list_Item[processIndex];

			yield return StartCoroutine(UpdateBothRole(curItem.attackIndex, curItem.defenseIndex, curItem, battleMgr));
            if(newPlayAnim) yield return new WaitForSeconds(BanTimeCenter.F_DengChang);

            int result = RESULT_NONE;

			/** 未来之战的引导 **/
			yield return StartCoroutine(showGuideXX(curItem, battleMgr));

            //记录观看的 回合数
            Core.Data.temper.WatchedhowManyRound = processIndex;

            switch(curItem.period) {
            case Period.Attend:
				battleMgr.AttendStageMgr.showAnim();
				yield return new WaitForSeconds(BanTimeCenter.Wait_For_AttendAnim);
                yield return StartCoroutine(AttendPeriod(curItem, battleMgr));
                break;
            case Period.DragonAoYi:
                yield return StartCoroutine(AoYiPeriod(curItem, battleMgr));
                break;
            case Period.AttributeIgnore:
                yield return StartCoroutine(AttributeIgnorePeriod(curItem, battleMgr));
                break;
            case Period.AttributeChange: 
				yield return StartCoroutine(AttributeChanged(battleMgr, curItem));
                break;
            case Period.AttributeConflict:
                yield return StartCoroutine(AttributeKillPeriod(curItem, battleMgr));
                break;
            case Period.AngrySkill:
                yield return StartCoroutine(AngrySkillPeriod(curItem, battleMgr));
                break;
            case Period.NormalSkill: 
                yield return StartCoroutine(NormalSkillPeriod(curItem, battleMgr));
                break;
            case Period.NormalAttack: {
					//对决在未来之战中出现的太早
					if(curStep == GuideStep.XiaoWuKong_Vs_BiKe1) 
						yield return new WaitForSeconds(1f);

					#if !VS
                    //对战动画
                    BanBattleManager.Instance.VsMgr.Forward();
                    battleMgr.main3DManager.RePositionEx();
                    yield return new WaitForSeconds(BanTimeCenter.VS_FIGHT_TIME);
					#endif

                    bool isLeftAdvance;
                    bool isDead = false;
                   
                    if(curItem.attackBP == 0 && curItem.defenseBP == 0) {
                        battleMgr.main3DManager.KillYou = true;
                        battleMgr.main3DManager.DoubleKill_Ban();
                        triggerPlayAnim = attackPBM.playAnim;
                        triggerPlayAnim2 = defensePBM.playAnim;
                        result = RESULT_BOTH_WIN;

                        battleMgr.UpdateHpIfVs(curItem.attackBP, curItem.defenseBP, BanTimeCenter.F_REDUCE_HP, 1, true);
                    } else{
                        if(curItem.attackBP > curItem.defenseBP){
                            isLeftAdvance = true;
                            if(curItem.defenseBP <= 0 ){
                                result = RESULT_ATTACKER_WIN;
                                isDead = true;
                            }
                            triggerPlayAnim = attackPBM.playAnim;
                            triggerPlayAnim2 = defensePBM.playAnim;
                        } else {
                            isLeftAdvance = false;
                            if(curItem.attackBP <= 0){
                                result = RESULT_DEFENDER_WIN;
                                isDead = true;
                            }
                            triggerPlayAnim = defensePBM.playAnim;
                            triggerPlayAnim2 = attackPBM.playAnim;
                        }

						battleMgr.main3DManager.KillYou = isDead;
						battleMgr.main3DManager.AttackAction_Ban(isLeftAdvance); 

						#if VS
						battleMgr.UpdateHpIfVs(curItem.attackBP, curItem.defenseBP, BanTimeCenter.F_VS_HP_TIME, 4, isDead);
						#endif

						yield return new WaitForSeconds(BanTimeCenter.F_VS_HP_TIME);
						#if !VS
						battleMgr.UpdateHpIfVs(curItem.attackBP, curItem.defenseBP, BanTimeCenter.F_VS_HP_TIME, 1, isDead);
						#endif
                        
                    }
                }
                break;
            case Period.DieSkill:
                DieSkillPeriod(curItem);
                break;
            case Period.AfterBattle:
                yield return StartCoroutine(AfterBattlePeriod(curItem, battleMgr));
                break;
            case Period.EndWar:
                //依据这个标志位来判定战斗结束, 所有版本有此状态
				EndOfWarPeriod(curItem, battleMgr);
                warEnd = true;
                break;
            case Period.AngrySkillReady:
                AngrySkillReady = true;
                yield return StartCoroutine(AngrySkillReadyPeriod(curItem, battleMgr));
                break;
            }

            if(diePeriod == null) {
				if(!OverSkill_Start) {
					battleMgr.UpdateHpData(curItem.attackBP, curItem.defenseBP);
				}
            }

            A:
            while(true){

                if(result != RESULT_BOTH_WIN) {
                    if(triggerPlayAnim2 != null) {   
                        //挨打方
                        string curAnim = triggerPlayAnim2.GetCurAnim();
                        if(OverSkill_Start) {
                            do {
                                curAnim = triggerPlayAnim2.GetCurAnim();
                                yield return new WaitForSeconds(0.1f);
                            } while(curAnim != "Injure_Fly_Go");
                        } 

                        if(curAnim == "Injure_Fly_Go" || curItem.period == Period.NormalAttack) {
							curAnimIsFlyGo();
							//处理有一方死亡的各种情况
							yield return StartCoroutine(SomeOneDie(battleMgr, result, curItem, processIndex));
							if(checkNextDeathSkill(processIndex)) {
								processIndex += 1;
							}
                        }
                    }

                    if(triggerPlayAnim != null){   //攻击方

                        if(triggerPlayAnim.GetCurAnim() == "Idle") { 
                            if(result == RESULT_ATTACKER_WIN) {
                                //预先处理是否有战后技能
                                yield return StartCoroutine(AfterBattleItem(processIndex, curItem, result));
                                result = RESULT_NONE;

                                goto A;
                            } else if(result == RESULT_DEFENDER_WIN) {
                                //预先处理是否有战后技能
                                yield return StartCoroutine(AfterBattleItem(processIndex, curItem, result));
                                result = RESULT_NONE;

                                goto A;
                            } 

                            triggerPlayAnim = null;
                            break;
                        } else {
							string anima = triggerPlayAnim.GetCurAnim();
							if(anima == "Injure_Fly_Go")
								triggerPlayAnim = null;
                        }
                    } else {
                        break;
                    }
                } else {
                    //这个写法比较恐怖
                    if(triggerPlayAnim == null && triggerPlayAnim2 == null){
						//攻击方和防御方没有血条动画
						if(result == RESULT_BOTH_WIN) {
							battleMgr.AoyiMgr.HideFireAndLight(false);
							battleMgr.AoyiMgr.HideFireAndLight(true);
						}
                        break;
                    }

                    attackPBM = null;
                    defensePBM = null;
                }
                yield return null;
            }

            yield return new WaitForSeconds(BanTimeCenter.F_ROUND_GAP);
        }
        list_Item.Clear();
        if(warEnd) {
            //如果不是按下跳过按钮，则应该走这个流程
            if(!skip) {
                //是否直接跳过战斗
                ShowOrSkipCalculate();
            } 
			battleMgr.SpeedOrSkipMgr.ResetSpeedUp();
        } else {
			BT_Logical war = BanBattleManager.Instance == null ? null : battleMgr.War;
            if(war != null && war.StepMode && war.LocalMode) {
                if(AngrySkillReady) 
                    war.KeepWarStep(battleMgr.attackSideInfo.getTotalClickCount);
                else
                    war.StartWarStep();
            }
        }

    }

    HttpTask errorTask = null;

    /// <summary>
    /// 在第一次的弗利萨战斗中，不展示战斗结算画面
    /// </summary>
	private bool ShowOrSkipCalculate() {
		bool jump = false;

        TemporyData tempory = Core.Data.temper;
        if(tempory.currentBattleType == TemporyData.BattleType.FightWithFulisa) {
            tempory.hasLiquidate = true;
			Invoke("JumpLater", BanTimeCenter.WaitForJump);
			jump = true;
        }
		//added by zhangqiang 
		//发送gps组队战斗结束的消息
        else if(tempory.currentBattleType == TemporyData.BattleType.GPSWar) {
			GPSRewardUI.OpenUI ();
		} else {
            sldSwBosBat();
        }

		return jump;
    }

    void sldSwBosBat() {
        TemporyData tempory = Core.Data.temper;
        /// 如果当前的是普通PVE的boss战斗，则要考虑网络是否完成
        /// 如果完成，则要先看是否有错误码。如果有错误码，则应该是要放弃这次战斗
        /// 再次要判定是否是网络异常，如果是网络异常，则应该有重试的机制
        if(tempory.currentBattleType  == TemporyData.BattleType.BossBattle ||
			tempory.currentBattleType == TemporyData.BattleType.FinalTrialBuou ||
			tempory.currentBattleType == TemporyData.BattleType.FinalTrialShalu) {

			if(tempory.warBattle.battleData.iswin == 2) {
				BanBattleManager.Instance.ShowCalculate ();
			} else {
				BanBattleManager battleMgr = BanBattleManager.Instance;

				if(tempory.warBattle.reward == null) {
					if(tempory.WarErrorCode != -1) {
						showErrorCode(tempory.WarErrorCode);
						tempory.WarErrorCode = -1;
					} else if(tempory.WarReq != null) {
						showNetworkDown(errorTask);
						tempory.WarReq = null;
					} else {
						ComLoading.Open();
					}
					battleMgr.War.RegisterWarCmp( (t)=> { ComLoading.Close(); errorTask = t; sldSwBosBat (); } );
				} else {
					BanBattleManager.Instance.ShowCalculate ();
				}
			}
            
        } else {
            BanBattleManager.Instance.ShowCalculate ();
        }
    }

    /// <summary>
    /// 通常是服务器校验失败
    /// </summary>

    void showErrorCode(int errorCode) {
        string content = Core.Data.stringManager.getNetworkErrorString(errorCode) + "\n" + Core.Data.stringManager.getString(28);
        string title   = Core.Data.stringManager.getString(5030);
        //string title   = Core.Data.stringManager.getString(17);

		UIInformation.GetInstance().SetInformation(content, title, ErrorJumpLater);
    }

    /// <summary>
    /// 通常是网络不给力
    /// </summary>
    void showNetworkDown(HttpTask task) {
        string content = Core.Data.stringManager.getString(29);
        //string title   = Core.Data.stringManager.getString(17);
        string title   = Core.Data.stringManager.getString(2003);
        if(task == null) task = BanBattleManager.Instance.War.CachedTask;
		task.errorInfo = null;
		UIInformation.GetInstance().SetInformation(content, title, task.DispatchToRealHandler, ErrorJumpLater);
    }


	void JumpLater() {
		Core.SM.beforeLoadLevel(SceneName.GAME_BATTLE, SceneName.MAINUI);
        AsyncLoadScene.m_Instance.LoadScene(SceneName.MAINUI);
	}

	//网络错误的时候，调回主界面，应该设定为输了
	void ErrorJumpLater () {
		TemporyData temp = Core.Data.temper;
		if(temp != null) {
			if(temp.warBattle != null && temp.warBattle.battleData != null)
				temp.warBattle.battleData.iswin = 2;
		}

		Core.SM.beforeLoadLevel(SceneName.GAME_BATTLE, SceneName.MAINUI);
		AsyncLoadScene.m_Instance.LoadScene(SceneName.MAINUI);
	}

}

public class PetBattleModel {
    public int index;
    public CRLuo_PlayAnim_FX playAnim;
    public PetBattleModel(int index, CRLuo_PlayAnim_FX playAnim, BanBattleRole banBattleRole) {
        this.index = index;
        this.playAnim = playAnim;

        if(banBattleRole.attribute == BanBattleRole.Attribute.Quan){
            this.playAnim.AddGoldenGlow();
        }
    }

    public void DoInjureAnim(){
        switch(Random.Range(0,3)){
        case 0:
            playAnim.HandleTypeAnim(CRLuoAnim_Main.Type.Injure_0);
            break;
        case 1:
            playAnim.HandleTypeAnim(CRLuoAnim_Main.Type.Injure_1);
            break;
        case 2:
            playAnim.HandleTypeAnim(CRLuoAnim_Main.Type.Injure_2);
            break;
        }
    }

    public float GetTimePoint(){
        bool next = false;
        for(int i = 0;i<playAnim.myRivalAmin.Length;i++){
            RivalAmin curRivalAmin = playAnim.myRivalAmin[i];
            if(curRivalAmin.FXtype == CRLuoAnim_Main.Type.Attack){
                for(int j = 0;j<curRivalAmin.myRivalAminElement.Length;j++){
                    RivalAminElement curRivalAminElement = curRivalAmin.myRivalAminElement[j];
                    if(next){
                        if(curRivalAminElement.ON_OFF){
                            return curRivalAminElement.Time_Wait;
                        }
                    }else{
                        if(curRivalAminElement.RivalInjuredAnimName == RivalInjuredAnim.Type.Attack && curRivalAminElement.ON_OFF){
                            next = true;
                        }
                    }
                }

            }
        }
        return 0;
    }

}