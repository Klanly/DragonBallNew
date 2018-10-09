using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using AW.Battle;

public partial class BanBattleManager : MonoBehaviourEx {

    private ConvertData localCached;


    #region 往队列里添加数据的简单方法

    //回合开始
    void addItemWhenRoundBegin(ConvertData data) {
        BanBattleProcess UIProc = BanBattleProcess.Instance;

        UIProc.list_Item.Add(new BanBattleProcess.Item(0,0, 
            data.attIndex, data.defIndex, data.attCurBp, data.defCurBp, data.attAPCount, data.defAPCount,
            BanBattleProcess.Period.Attend, -1,-1,-1,-1,-1));
    }
    //属性克制
    void addItemWhenPropertyKill(ConvertData data, int extra_3 = 0) {
        BanBattleProcess UIProc = BanBattleProcess.Instance;

        BanBattleProcess.Item i = new BanBattleProcess.Item(0,0, 
            data.attIndex, data.defIndex, data.attCurBp, data.defCurBp, data.attAPCount, data.defAPCount,
            BanBattleProcess.Period.AttributeConflict, -1,-1,-1,-1,-1);

        i.extra_3 = extra_3;

        UIProc.list_Item.Add(i);
    }
    //释放技能
    BanBattleProcess.Item AddItemWhenSkill(ConvertData data, int skillType, int from, int to, int mType, int skillId, int leftAp, CMsgHeader item) {
        BanBattleProcess.Item newItem = AddSkillItem (skillType, 
            data.attIndex, data.defIndex, data.attCurBp, data.defCurBp, data.attAPCount, data.defAPCount,
            from, to, mType, skillId, leftAp, item);

        return newItem;
    }
    //属性转换
    void addItemWhenPropertyChg(ConvertData data, int from, string skillname, int skillId) {
        BanBattleProcess UIProc = BanBattleProcess.Instance;

		BanBattleProcess.Item item = new BanBattleProcess.Item(0,0,
			data.attIndex, data.defIndex, data.attCurBp, data.defCurBp, data.attAPCount, data.defAPCount,
			BanBattleProcess.Period.AttributeChange,from,-1,-1,-1,-1,skillname);

		item.skillId = skillId;

        UIProc.list_Item.Add(item);
    }
    #endregion

    #region 分步计算战斗的方法

    void RoundBegin(CMsgHeader aHeader) {
        CMsgRoundBegin aRoundBegin = aHeader as CMsgRoundBegin;
        localCached.attIndex = aRoundBegin.attacker;
        localCached.defIndex = aRoundBegin.defense;
        localCached.attCurBp = aRoundBegin.attackerCurAtt;
        localCached.defCurBp = aRoundBegin.defenseCurAtt;
        localCached.attAPCount = aRoundBegin.attTeamAngry;
        localCached.defAPCount = aRoundBegin.defTeamAngry;

        maxAttendEnemyIndex = aRoundBegin.defense;

        addItemWhenRoundBegin(localCached);
    }

    void Property_Kill(CMsgHeader aHeader) {
        //属性相克
        CMsgPropertyEnchance confict = aHeader as CMsgPropertyEnchance;

        if(confict.attackerEnch != 1){
            localCached.attCurBp = confict.attackerCurAtt;
            localCached.defCurBp = confict.defenseCurAtt;

            addItemWhenPropertyKill(localCached);
        } else {
            //更新AP
            addItemWhenPropertyKill(localCached, BanBattleProcess.Item.Extra3_UpdateAP);
        }
    }

    void WarEnd (CMsgHeader aHeader) {
        CMsgWarEnd aWarEnd = aHeader as CMsgWarEnd;
        if(side == 0)
            battleWin = (aWarEnd.winner == "att");
        else 
            battleWin = (aWarEnd.winner == "def");

        if(aWarEnd.winner == "att") maxAttendEnemyIndex = -1;

        BanBattleProcess.Instance.list_Item.Add( new BanBattleProcess.Item (
            0,0,
            localCached.attIndex, localCached.defIndex, localCached.attCurBp, localCached.defCurBp, localCached.attAPCount, localCached.defAPCount,
            BanBattleProcess.Period.EndWar, -1,-1,-1,-1,-1, string.Empty, aHeader) 
        );
    }

    void NormalAttack( CMsgHeader aHeader) {
        //普通攻击
        CMsgNormalAttack aNormalAttack = aHeader as CMsgNormalAttack;

        if(localCached.firstNormalAttack == null){
            localCached.firstNormalAttack = aNormalAttack;
        }else{
            if(localCached.secondNormalAttack == null){
                localCached.secondNormalAttack = aNormalAttack;

                if(GetBattleRole(localCached.firstNormalAttack.suffer).group != BanBattleRole.Group.Attack){
                    CMsgNormalAttack temp = localCached.firstNormalAttack;
                    localCached.firstNormalAttack = localCached.secondNormalAttack;
                    localCached.secondNormalAttack = temp;
                }

                BanBattleProcess.Instance.list_Item.Add( new BanBattleProcess.Item (
                    0,0,
                    localCached.firstNormalAttack.suffer,
                    localCached.secondNormalAttack.suffer,
                    localCached.firstNormalAttack.curAtt + localCached.AttExploreHurt,
                    localCached.secondNormalAttack.curAtt + localCached.DefExploreHurt,
                    localCached.attAPCount,
                    localCached.defAPCount,
                    BanBattleProcess.Period.NormalAttack, -1,-1,-1,-1,-1) 
                );

                localCached.firstNormalAttack = null;
                localCached.secondNormalAttack = null;

                localCached.AttExploreHurt = 0;
                localCached.DefExploreHurt = 0;
            }else{
                #if DEBUG
                Debug.Log("can not come here");
                #endif
            }
        }

    }

    /// <summary>
    /// 怒气技能已经准备好了
    /// </summary>
    /// <param name="aHeader">A header.</param>
    void AngrySkill_Ready(CMsgHeader aHeader) {
        BanBattleProcess UIProc = BanBattleProcess.Instance;

        UIProc.list_Item.Add(new BanBattleProcess.Item(0, 0,
            localCached.attIndex, localCached.defIndex, localCached.attCurBp, localCached.defCurBp, localCached.attAPCount, localCached.defAPCount,
            BanBattleProcess.Period.AngrySkillReady, -1,-1,-1,-1,-1, string.Empty, aHeader));
    }

    //释放技能
    void CastSkill(int Op, CMsgHeader aHeader) {
        Type t = typeof(BanBattleManager);
        string funName = "Skill_" + Op;
        t.InvokeMember( funName, BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance, 
            null, this, new object[]{ aHeader });
    }

    void CastSkill(int Op, CMsgHeader aHeader, List<CMsgHeader> battleInfo, int headerIndex) {
        Type t = typeof(BanBattleManager);
        string funName = "Skill_" + Op;
        t.InvokeMember( funName, BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance, 
            null, this, new object[]{aHeader, battleInfo, headerIndex});
    }

    void Skill_401 (CMsgHeader aHeader) {
        //技能
        CMsgSkillAttack skill = aHeader as CMsgSkillAttack;

        int from = skill.caster;
        int to   = skill.suffer; //谁受到伤害
        if(skill.suffer == localCached.attIndex){
            localCached.attCurBp   = skill.curAtt;
            localCached.defAPCount = skill.curAngry;
        }else{
            localCached.defCurBp   = skill.curAtt;
            localCached.attAPCount = skill.curAngry;
        }

        int skillId      = skill.skillId;
        //恢复的怒气
        int RecoverAngry = skill.recoverAngry;
        //消耗的怒气
        int CostAngry    = skill.costAngry;
        //消耗时的怒气
        int PreAngry     = skill.preAngry;

        BanBattleProcess.Item newItem = null;

        if(RecoverAngry > 0) {

            newItem = AddItemWhenSkill(localCached, skill.skillType, from, to, 
                BanBattleProcess.Item.CostAndRecoverAngry, skillId, skill.curAngry, skill);
            newItem.recoverAngry = RecoverAngry;
            newItem.costAngry    = CostAngry;
            newItem.preAngry     = PreAngry;
        } else {
            AddItemWhenSkill(localCached, skill.skillType, from, to, 
                0, skillId, skill.curAngry, skill);
        }
    }

    void Skill_402(CMsgHeader aHeader) {
        CMsgSkillChangeAttack skill = aHeader as CMsgSkillChangeAttack;

        int from = skill.caster;
        int to = skill.suffer;

        //打击对方的技能
        if(skill.suffer != skill.caster) {
            if(to == localCached.attIndex) {
                localCached.attCurBp = skill.curAtt;
                localCached.defAPCount = skill.curAngry;
            } else {
                localCached.defCurBp = skill.curAtt;
                localCached.attAPCount = skill.curAngry;
            }
        } else {
            //增强自身型技能
            if( skill.suffer == localCached.attIndex ){
                from = localCached.attIndex;
                to = -1;
                localCached.attCurBp = skill.curAtt;
                localCached.attAPCount = skill.curAngry;
            } else {
                from = localCached.defIndex;
                to = -1;
                localCached.defCurBp = skill.curAtt;
                localCached.defAPCount = skill.curAngry;
            }

        }

        int skillId = skill.skillId;

        if (skill.suffer == skill.caster)
            AddItemWhenSkill(localCached, skill.skillType, from, -1, 
                BanBattleProcess.Item.Extra3_Skill_Type_PowerUp, skillId, skill.curAngry, skill);
        else
            AddItemWhenSkill(localCached, skill.skillType, from, to, 
                BanBattleProcess.Item.Extra3_LongSkill, skillId, skill.curAngry, skill);
    }
        
    void Skill_403(CMsgHeader aHeader) {
        //10100  悟天克斯布欧
        CMsgSkillAttackCombo skill = aHeader as CMsgSkillAttackCombo; 

        int last = skill.attackArr.Length - 1;
        if(last < 0) return;
        CMsgSkillAttack lastItem = skill.attackArr[last];

        int from = 0; 
        int to   = 0;

        int skillId = skill.skillId;
        //连击的情况，我们应该先把怒气使用怒气
        if(lastItem.skillType == 0) {

            //左边是挨打方
            if (lastItem.suffer == localCached.attIndex) { 
                localCached.attCurBp = lastItem.curAtt; 
                localCached.attAPCount = lastItem.curAngry;
                localCached.defAPCount = skill.curAngry;
                from = localCached.defIndex; 
                to = localCached.attIndex; 
            } else {
                //右边是挨打方
                localCached.defCurBp = lastItem.curAtt;
                localCached.defAPCount = lastItem.curAngry;
                localCached.attAPCount = skill.curAngry;
                from = localCached.attIndex; 
                to = localCached.defIndex; 
            } 

            AddItemWhenSkill(localCached, lastItem.skillType, from, to, 
                BanBattleProcess.Item.Extra3_Combo, skillId, lastItem.curAngry, skill);
        } else {

            foreach (CMsgSkillAttack item in skill.attackArr) {

                from = item.caster; 
                to = item.suffer; 

                if(item.suffer == localCached.defIndex) {
                    localCached.defCurBp = item.curAtt; 
                    localCached.attAPCount = skill.curAngry;
                    localCached.defAPCount = item.curAngry;
                } else {
                    localCached.attCurBp = item.curAtt;
                    localCached.attAPCount = item.curAngry;
                    localCached.defAPCount = skill.curAngry;
                }

                AddItemWhenSkill(localCached, item.skillType, from, to, 
                    BanBattleProcess.Item.Extra3_Combo, skillId, item.curAngry, skill);
            }

        }
    }

    void Skill_404(CMsgHeader aHeader) {
        CMsgSkillSuckAttack item = aHeader as CMsgSkillSuckAttack;

        int from = item.caster;
        int to   = item.suffer;
        if( item.suffer == localCached.attIndex ){
            localCached.attCurBp = item.sufferCurAtt;
            localCached.defCurBp = item.casterCurAtt;
            localCached.defAPCount = item.curAngry;
        }else{
            localCached.attCurBp = item.casterCurAtt;
            localCached.defCurBp = item.sufferCurAtt;
            localCached.attAPCount = item.curAngry;
        }
        int skillId = item.skillId;

        AddItemWhenSkill(localCached, item.skillType, from, to, 
            BanBattleProcess.Item.Extra3_Skill_Type_Suck, skillId, item.curAngry, item);
    }

    void Skill_405(CMsgHeader aHeader) {
        CMsgSkillChangeCurAttackBoth item = aHeader as CMsgSkillChangeCurAttackBoth;

        int from;
        int to;
        if( item.caster == localCached.attIndex ) {
            localCached.attCurBp = item.selfCurAtt;
            localCached.defCurBp = item.enemyCurAtt;
            from = localCached.attIndex;
            to = localCached.defIndex;

        } else {
            localCached.attCurBp = item.enemyCurAtt;
            localCached.defCurBp = item.selfCurAtt;
            from = localCached.defIndex;
            to = localCached.attIndex;
        }

        int skillId = item.skillId;

        AddItemWhenSkill(localCached, item.skillType, from, to, 
            BanBattleProcess.Item.Extra3_DoubleIdle, skillId, item.curAngry, item);
    }

    void Skill_406(CMsgHeader aHeader) {
        CMsgSkillRevive item = aHeader as CMsgSkillRevive;
        int from;
        if( item.caster == localCached.attIndex ){
            from = localCached.attIndex;
        } else {
            from = localCached.defIndex;
        }

        int skillId = item.skillId;
        AddItemWhenSkill(localCached, item.skillType, from, -1, 
            BanBattleProcess.Item.Extra3_Skill_Type_Revive, skillId, item.curAngry, aHeader);
    }

    void Skill_407(CMsgHeader aHeader, List<CMsgHeader> battleInfo, int headerIndex) {
        //自爆技能
        //因为自爆技能的特殊，很有可能是夹在两个互殴片段之间
        //所以我们要判定是否还处于互殴阶段
        bool Vs1 = false, Vs2 = false, Vs = false;
        CMsgNormalAttack AfterVsAttack = null;
        int maxLength = battleInfo.Count;
        int preIndex = headerIndex - 2;

        if(headerIndex < maxLength) {
            int tmp = headerIndex;
            CMsgHeader nextItem = null;
            do{
                nextItem = battleInfo[tmp];

                CMsgSkillCast check = nextItem as CMsgSkillCast;
                if(check != null) {
                    if(check.skillType == (int)CSkill_Type.Die) {
                        tmp += 1;
                    } else {
                        break;
                    }
                } else {ConsoleEx.DebugLog("check = " +  nextItem.GetType().ToString()); break;}

            } while(tmp < maxLength);


            if(nextItem.status == CMsgHeader.STATUS_ATTACK) {
                Vs1 = true;
                AfterVsAttack = nextItem as CMsgNormalAttack;
            }
        }

        if(preIndex >= 0) {
            CMsgHeader preItem = battleInfo[preIndex];
            if(preItem.status == CMsgHeader.STATUS_ATTACK) {
                Vs2 = true;
            }
        }
        Vs = Vs1 && Vs2;

        ///
        /// 我只对受伤队列的第一个感兴趣，后面的，登场时候血量就会减少
        ///
        CMsgSkillBlast item = aHeader as CMsgSkillBlast;

        int nFrom;
        CMsgSkillAttack one = item.sufferArr.Value<CMsgSkillAttack>(0);

        if( item.caster == localCached.attIndex ){
            localCached.attCurBp = 0;
            if(Vs) {
                localCached.defCurBp = AfterVsAttack.curAtt;

                if(one != null) localCached.DefExploreHurt = one.finalAtt;
            } else {
                if(one != null) localCached.defCurBp = one.curAtt;
            }

            nFrom = localCached.attIndex;
        } else {
            if(Vs) {
                localCached.attCurBp = AfterVsAttack.curAtt;
                if(one != null) localCached.AttExploreHurt = one.finalAtt;
            } else {
                if(one != null) localCached.attCurBp = one.curAtt;
            }

            localCached.defCurBp = 0;
            nFrom = localCached.defIndex;
        }

        int skillId = item.skillId;

        BanBattleProcess.Item added = AddItemWhenSkill(localCached, item.skillType, nFrom, -1, 
            BanBattleProcess.Item.Extra3_Explore, skillId, item.curAngry, item);

        added.VsMiddle = Vs ? 1 : 0;
    }

    void Skill_408(CMsgHeader aHeader) {
        CMsgSkillAngryExchange skill = aHeader as CMsgSkillAngryExchange;

        int from = skill.caster;
        if( skill.caster == localCached.attIndex ){
            localCached.attAPCount = skill.curAngry;
        }else{
            localCached.defAPCount = skill.curAngry;
        }
        int skillId = skill.skillId;

        AddItemWhenSkill(localCached, skill.skillType, from, -1, 
            BanBattleProcess.Item.Extra3_Skill_Type_PowerUp, skillId, skill.curAngry, skill);
    }

    void Skill_409(CMsgHeader aHeader) {
        CMsgSkillBuffDebuff skill = aHeader as CMsgSkillBuffDebuff;
		if(skill == null) {
			CMsgSkillBuffOrDebuff skill1 = aHeader as CMsgSkillBuffOrDebuff;

			skill = new CMsgSkillBuffDebuff() {

				status   = skill1.status,
				category = skill1.category,
				skillId  = skill1.skillId,
				skillOp  = skill1.skillOp,
				skillType = skill1.skillType,
				curAngry = skill1.curAngry,
				Mul      = skill1.Mul,
				caster   = skill1.caster,

				type     = skill1.type,
				round    = skill1.round,
				sufferArr= skill1.sufferArr,
				arg1     = (int)skill1.arg1,
				arg2     = (int)skill1.arg2,
				arg3     = (int)skill1.arg3,
			};

		}

        int from = 0;
        int to   = 0;
        switch (skill.type) {
        case CMsgHeader.BUFF_DEBUFF_SEAL:{//封印
                if(skill.caster == localCached.attIndex) {
                    from = localCached.attIndex;
                    to = localCached.defIndex;
                    localCached.attAPCount = skill.curAngry;
                } else {
                    from = localCached.defIndex;
                    to = localCached.attIndex;
                    localCached.defAPCount = skill.curAngry;
                }
                int skillId = skill.skillId;

                AddItemWhenSkill(localCached, skill.skillType, from, to, 
                    0, skillId, skill.curAngry, skill);
            }
            break;
        case CMsgHeader.BUFF_DEBUFF_GOLDDEFENSE://金钟罩 免疫下次伤害
            ConsoleEx.DebugLog ("------释放buff---金钟罩 免疫下次伤害-");
            break;
        case CMsgHeader.BUFF_DEBUFF_DEFENSE: //护盾
            ConsoleEx.DebugLog ("------释放buff---护盾-");
            break;
        case CMsgHeader.BUFF_DEBUFF_HURTUP://加倍伤害
            {
                ConsoleEx.DebugLog ("------释放buff---加倍伤害-");
                if( skill.caster == localCached.attIndex ){
                    localCached.attCurBp = (int)skill.arg2;
                    from = localCached.attIndex;
                    to = localCached.defIndex;
                } else {
                    from = localCached.defIndex;
                    to = localCached.attIndex;
                    localCached.defCurBp = (int)skill.arg2;
                }
                int skillId = skill.skillId;

                AddItemWhenSkill(localCached, skill.skillType, from, to, 
                    BanBattleProcess.Item.Extra3_Skill_Type_Attack, skillId, skill.curAngry, skill);
            }
            break;
        case CMsgHeader.BUFF_DEBUFF_IMMUNITY://技能免疫
            {
                ConsoleEx.DebugLog ("------释放buff---技能免疫-");
                from = skill.caster;
                to   = -1;
                int skillId = skill.skillId;

                AddItemWhenSkill(localCached, skill.skillType, from, to, 
                    BanBattleProcess.Item.Extra3_Skill_Type_Attack, skillId, skill.curAngry, skill);
            }
            break;
        case CMsgHeader.BUFF_DEBUFF_REBOUND://反弹伤害
            {
                ConsoleEx.DebugLog ("------释放buff---反弹伤害-");
                if( skill.caster == localCached.attIndex ){
                    from = localCached.attIndex;
                    to = localCached.defIndex;
                    localCached.attAPCount = skill.curAngry;
                } else {
                    from = localCached.defIndex;
                    to = localCached.attIndex;
                    localCached.defAPCount = skill.curAngry;
                }
                int skillId = skill.skillId;

                AddItemWhenSkill(localCached, skill.skillType, from, to, 
                    BanBattleProcess.Item.Extra3_Skill_Type_Attack, skillId, skill.curAngry, skill);
            }
            break;
        }
    }

    void Skill_410(CMsgHeader aHeader) {
        ConsoleEx.DebugLog ("----群体增益-----");
        CMsgSkillChangeCurAttackAll skill = aHeader as CMsgSkillChangeCurAttackAll;
        int from = skill.caster;
        int skillId = skill.skillId;
        AddItemWhenSkill(localCached, skill.skillType, from, -1, 
            BanBattleProcess.Item.Extra3_Skill_Type_PowerUp, skillId, skill.curAngry, skill);

    }

    void Skill_411(CMsgHeader aHeader) {
        ConsoleEx.DebugLog ("-----假死-----");
        CMsgSkillDeath skill = aHeader as CMsgSkillDeath;

        int from = 0;
        int to   = 0;
        if( skill.suffer == localCached.attIndex ){
            localCached.attCurBp = skill.curAtt;

            from = localCached.defIndex;
            to = -1;

        }else{

            localCached.defCurBp = skill.curAtt;
            from = localCached.attIndex;
            to = -1;
        }
        int skillId = skill.skillId;
        AddItemWhenSkill(localCached, skill.skillType, from, to, 
            BanBattleProcess.Item.Extra3_Skill_Type_PowerUp, skillId, skill.curAngry, skill);
    }

    void Skill_412(CMsgHeader aHeader) {
        CMsgSkillRecovery skill = aHeader as CMsgSkillRecovery;

        int from = skill.caster;
        int to = -1;
        if( skill.suffer == localCached.attIndex ){
            localCached.attCurBp = skill.finalAtt;
            localCached.attAPCount = skill.curAngry;
        }else{
            localCached.defCurBp = skill.finalAtt;
            localCached.defAPCount = skill.curAngry;
        }
        int skillId = skill.skillId;
        AddItemWhenSkill(localCached, skill.skillType, from, to, 
            BanBattleProcess.Item.Extra3_Recover, skillId, skill.curAngry, skill);
    }

    void Skill_413(CMsgHeader aHeader) {
        ConsoleEx.DebugLog ("--------斩杀--------");

        //技能
        CMsgSkillCut skill = aHeader as CMsgSkillCut;

        int from = 0;
        int to   = 0;
        if(skill.suffer == localCached.attIndex){
            localCached.attCurBp = skill.curAtt;
            from = localCached.defIndex;
            to = localCached.attIndex;
        }else{
            localCached.defCurBp = skill.curAtt;
            from = localCached.attIndex;
            to = localCached.defIndex;
        }
        int skillId = skill.skillId;
        AddItemWhenSkill(localCached, skill.skillType, from, to, 
            -1, skillId, skill.curAngry, skill);
    }

    void Skill_414(CMsgHeader aHeader) {
        CMsgSkillBomb skill = aHeader as CMsgSkillBomb;
        int from = 0;
        int to   = 0;
        if(skill.suffer == localCached.attIndex){
            localCached.attCurBp = skill.enemyCurAtt;
            localCached.defCurBp = 0;
            from = localCached.defIndex;
            to = localCached.attIndex;
        }else{
            localCached.defCurBp = skill.enemyCurAtt;
            localCached.attCurBp = 0;
            from = localCached.attIndex;
            to = localCached.defIndex;
        }
        int skillId = skill.skillId;
        AddItemWhenSkill(localCached, skill.skillType, from, to, 
            BanBattleProcess.Item.Extra3_Skill_Type_Attack, skillId, skill.curAngry, skill);
    }

    void Skill_415(CMsgHeader aHeader) {
        CMsgSkillAttDelta skill = aHeader as CMsgSkillAttDelta;
        int from = 0;
        int to   = 0;
        if(skill.caster == localCached.attIndex){
            localCached.attCurBp = skill.curAtt;
            localCached.defCurBp = skill.enemyAtt;
            from = localCached.attIndex;
            to = localCached.defIndex;
        } else {
            localCached.attCurBp = skill.enemyAtt;
            localCached.defCurBp = skill.curAtt;
            from = localCached.defIndex;
            to = localCached.attIndex;
        }
        int skillId = skill.skillId;
        AddItemWhenSkill(localCached, skill.skillType, from, to, 
            BanBattleProcess.Item.Extra3_Skill_Type_Suck, skillId, skill.curAngry, skill);
    }

    void Skill_416(CMsgHeader aHeader) {
        //无视属性相克
        CMsgSkillAttack skill = aHeader as CMsgSkillAttack;

        int from = skill.caster;
        int to   = 0;
        if(skill.suffer == localCached.attIndex){
            to = localCached.defIndex;
        } else {
            to = localCached.attIndex;
        }

        int skillId = skill.skillId;
        AddItemWhenSkill(localCached, skill.skillType, from, to, -1, skillId, -1, skill);
    }

    void Skill_417(CMsgHeader aHeader) {
        //变成全属性
        CMsgSkillCast skill = aHeader as CMsgSkillCast;

        int from = skill.caster;
        int to = skill.caster;

        int skillId = skill.skillId;

        //有可能是奥义的技能，所以要转换一下
        string skillname = string.Empty;
        if(isAoYiSkill(skillId)) {
            AddItemWhenSkill(localCached, skill.skillType, from, to, 
                BanBattleProcess.Item.Extra3_Attribute_CHG, skillId, skill.curAngry, skill);
        } else {
            skillname = Core.Data.skillManager.getSkillDataConfig(skillId).name;
			addItemWhenPropertyChg(localCached, from, skillname, skillId);
        }
    }

    void Skill_418(CMsgHeader aHeader, List<CMsgHeader> battleInfo, int headerIndex) {
        //虽然是增加怒气，但是有可能增加怒气的技能师怒气技能或者是奥义技能
        //如果是奥义技能的话，还要区分是否是死亡时候触发
        //所以我们要判定是否还处于互殴阶段

        //增加怒气
        CMsgSkillCast addAngry = aHeader as CMsgSkillCast;

        bool Vs = false;
        if(addAngry.skillType == (int)CSkill_Type.Die) {
            //我们要判定是否还处于互殴阶段
            bool Vs1 = false, Vs2 = false;
            int maxLength = battleInfo.Count;
            int preIndex = headerIndex - 2;

            if(headerIndex < maxLength) {
                CMsgHeader nextItem = battleInfo[headerIndex];
                if(nextItem.status == CMsgHeader.STATUS_ATTACK) {
                    Vs1 = true;
                }
            }

            if(preIndex >= 0) {
                CMsgHeader preItem = null;

                do {
                    preItem = battleInfo[preIndex];
                    CMsgSkillCast check = preItem as CMsgSkillCast;
                    if(check != null) {
                        if(check.skillType == (int) CSkill_Type.Die) {
                            preIndex -= 1;
                        } else {
                            break;
                        }
                    } else {
                        ConsoleEx.DebugLog("check = " +  preItem.GetType().ToString());
                        break;
                    }
                } while(preIndex >= 0);


                if(preItem.status == CMsgHeader.STATUS_ATTACK) {
                    Vs2 = true;
                } 
            }

            Vs = Vs1 && Vs2;
        }


        int from = addAngry.caster;
        int to = -1;
        int skillId = addAngry.skillId;
        if(addAngry.caster == localCached.attIndex) {
            localCached.attAPCount = addAngry.curAngry;
        } else {
            localCached.defAPCount = addAngry.curAngry;
        }

        if(Vs) {
            if(addAngry.caster == localCached.attIndex) localCached.attCurBp = 0;
            else localCached.defCurBp = 0;

            AddItemWhenSkill(localCached, addAngry.skillType, from, to, 
                BanBattleProcess.Item.Extra3_AoYiAngry, skillId, addAngry.curAngry, addAngry);
        } else {
            AddItemWhenSkill(localCached, addAngry.skillType, from, to, 
                BanBattleProcess.Item.Extra3_AddAP, skillId, addAngry.curAngry, addAngry);
        }
    }

    void Skill_419(CMsgHeader aHeader) {
        CMsgSkillRecovery skill = aHeader as CMsgSkillRecovery;

        int from = skill.caster;
        int to   = -1;
        if( skill.suffer == localCached.attIndex ){
            localCached.attCurBp = skill.finalAtt;
            localCached.attAPCount = skill.curAngry;
        }else{
            localCached.defCurBp = skill.finalAtt;
            localCached.defAPCount = skill.curAngry;
        }
        int skillId = skill.skillId;
        AddItemWhenSkill(localCached, skill.skillType, from, to, 
            0, skillId, skill.curAngry, skill);
    }

    void Skill_420(CMsgHeader aHeader) {
        CMsgSkillAfterRecovery skill = aHeader as CMsgSkillAfterRecovery;

        int from = skill.caster;
        int to   = -1;
        if( skill.suffer == localCached.attIndex ){
            localCached.attCurBp = skill.finalAtt;
            localCached.attAPCount = skill.curAngry;
            localCached.defCurBp = skill.enemyAtt;
        }else{
            localCached.defCurBp = skill.finalAtt;
            localCached.defAPCount = skill.curAngry;
            localCached.attCurBp = skill.enemyAtt;
        }
        int skillId = skill.skillId;
        AddItemWhenSkill(localCached, skill.skillType, from, to, 
            BanBattleProcess.Item.Extra3_Recover, skillId, skill.curAngry, skill);
    }


    /// <summary>
    /// 从本地读取战斗序列
    /// </summary>
    private void ReadFromLocal(List<CMsgHeader> recorder) {
        if(localCached == null) localCached = new ConvertData();

        #if DEBUG
		ConsoleEx.DebugLog("Battle Sequence : " + fastJSON.JSON.Instance.ToJSON(recorder), ConsoleEx.RED);
        #endif

        int headerIndex = 0;
        if(recorder != null) {

            foreach(CMsgHeader aHeader in recorder) {
                headerIndex ++;
                switch(aHeader.status) {
                case CMsgHeader.STATUS_ROUND_BEGIN:
                    RoundBegin(aHeader);
                    break;
                case CMsgHeader.STATUS_PROPERTY_KILL:
                    Property_Kill(aHeader);
                    break;
                case CMsgHeader.STATUS_ATTACK:
                    NormalAttack(aHeader);
                    break;
                case CMsgHeader.STATUS_WAR_END:
                    WarEnd(aHeader);
                    break;
                case CMsgHeader.STATUS_ANGRY_READY:
                    AngrySkill_Ready(aHeader);
                    break;
                    ///
                    ///  ------  下面是各种技能的代码 -------
                    ///
                case CMsgHeader.STATUS_NSK_401: case CMsgHeader.STATUS_NSK_402: case CMsgHeader.STATUS_NSK_403:
                case CMsgHeader.STATUS_NSK_404: case CMsgHeader.STATUS_NSK_405: case CMsgHeader.STATUS_NSK_406:
                case CMsgHeader.STATUS_NSK_408: case CMsgHeader.STATUS_NSK_409: case CMsgHeader.STATUS_NSK_410:
                case CMsgHeader.STATUS_NSK_411: case CMsgHeader.STATUS_NSK_412: case CMsgHeader.STATUS_NSK_413:
                case CMsgHeader.STATUS_NSK_414: case CMsgHeader.STATUS_NSK_415: case CMsgHeader.STATUS_NSK_417:
                case CMsgHeader.STATUS_NSK_419: case CMsgHeader.STATUS_NSK_420:
                    CastSkill(aHeader.status, aHeader);
                    break;
               
                case CMsgHeader.STATUS_NSK_407:
                case CMsgHeader.STATUS_NSK_418:
                    CastSkill(aHeader.status, aHeader, recorder, headerIndex);
                    break;
                }
            }

            //开始UI展现
			if(Core.Data.temper.SkipBattle == false) BanBattleProcess.Instance.HandleItem();
        }

    }

    /// <summary>
    /// 怒气技能按钮出现和消失,消失多次调用会保证能取到点击的次数
    /// </summary>

    public void AngryUI(bool OnOff, SkillData sk, int Count) {
        if(OnOff) {
            attackSideInfo.MaxSkillCount = Count;
            attackSideInfo.curAnSk       = sk;
            attackSideInfo.PlayerAngryBtn.ShowSkill(sk);
			attackSideInfo.InitBtnClick();
			Core.Data.soundManager.SoundFxPlay(SoundFx.Btn1);
        } else {
            bool hasShown = attackSideInfo.PlayerAngryBtn.HideSkill();
			if(hasShown) attackSideInfo.HideBtnClick();
        }
    }

    #endregion


    #region 公共的方法

    private void WarBegin(CMsgHeader aHeader) {
        //战斗开始，读取队伍信息，怒气槽等信息
        CMsgWarBegin aWarBegin = aHeader as CMsgWarBegin;
        foreach (AW.Battle.Monster aMonster in aWarBegin.attTeam.team) {
			BanBattleRole role = new BanBattleRole (aMonster.pveIndex, aMonster.num, BanTools.TransAttributeType (aMonster.myAttribute), aMonster.level, aMonster.curAtt, 0, BanBattleRole.Group.Attack, aMonster.curAtt, aMonster.AllFated);
			role.angSkObj = aMonster.aSkill;
			role.norSkObj = aMonster.nSkill;
			role.isBoss   = aMonster.isBoss;

			list_BattleRole.Add (role);
        }

        foreach (AW.Battle.Monster aMonster in aWarBegin.defTeam.team) {
			BanBattleRole role = new BanBattleRole (aMonster.pveIndex, aMonster.num, BanTools.TransAttributeType (aMonster.myAttribute), aMonster.level, 0, aMonster.curAtt, BanBattleRole.Group.Defense, aMonster.curAtt, aMonster.AllFated);
			role.angSkObj = aMonster.aSkill;
			role.norSkObj = aMonster.nSkill;
			role.isBoss   = aMonster.isBoss;

            list_BattleRole.Add (role);
        }

        //added by zq for atk and def icon
        attackSideInfo.IsAttack = (0 == aWarBegin.attTeam.type);
        defenseSideInfo.IsAttack = !attackSideInfo.IsAttack;
        //end

        if(aWarBegin.attAoYi != null){
            foreach(int aoyi in aWarBegin.attAoYi) {
                list_AttAoYi.Add(aoyi);
            }
        }

        if(aWarBegin.defAoYi != null) {
            foreach(int aoyi in aWarBegin.defAoYi) {
                list_DefAoYi.Add(aoyi);
            }
        }

        if(Core.Data.guideManger.isGuiding) {
            side = 0;
        } else {
            if(attackSideInfo.IsAttack) {
                side = 0;
            } else {
                side = 1;
            }
        }
    }

    #endregion


    /// <summary>
    /// 这个类的主要作用：保存一个回合，可能会分成两节的战斗
    /// </summary>
    private class ConvertData {
        public CMsgNormalAttack firstNormalAttack = null;
        public CMsgNormalAttack secondNormalAttack = null;
        public int attAPCount = 0;
        public int defAPCount = 0;

        public int attIndex = 0;
        public int defIndex = 0;

        public int attCurBp = 0;
        public int defCurBp = 0;

        //自爆的伤害
        public int AttExploreHurt = 0;
        public int DefExploreHurt = 0;
    }

}
