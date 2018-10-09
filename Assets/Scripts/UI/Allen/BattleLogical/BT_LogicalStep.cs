using System;
using System.Collections.Generic;

namespace AW.Battle {


    /// 
    /// ------- 这里的所有方法都是提供出 “一回合走两次” 战斗的模式
    /// ------- 也就是说每个回合战斗，都会提前计算出来当前回合是否可以释放怒气技，
    /// ------- 1. 如果可以释放怒气技能，则会设定一个等待用户释放技能的时间。等待用户的操作后，在走当前回合剩下的逻辑
    /// ------- 2. 如果不可以释放怒气技，则直接完成当前回合。进入下个回合。
    /// ------- 每个回合的开始，都是由UI层调用，以回调通知UI做动画为一个阶段的结束
    /// 
    /// 

    public partial class BT_Logical {
   
        //采用分步模式吗
        private bool bStepMode = false;
        public bool StepMode {
            get { return bStepMode; }
        }

        //采用本地计算战斗的模式吗
        private bool bLocalMode = false;
        public bool LocalMode {
            get { return bLocalMode; }
        }

        private void init() {
            StepRecorder = new List<CMsgHeader>();
            TemporyData temp = Core.Data.temper;

            bStepMode  = temp.Open_StepMode;
            bLocalMode = temp.Open_LocalWarMode;
        }

        //存储分步计算的战斗序列
        //只记录下当前步骤的战斗序列
        private List<CMsgHeader> StepRecorder = null;

        //UI 战斗序列的回调
        private Action<List<CMsgHeader>> StepComplete;
        public void RegisterLogicalCmp(Action<List<CMsgHeader>> uiWork) {
            StepComplete = uiWork;
        }

        private Action<HttpTask> WarResult = null;
        public void RegisterWarCmp(Action<HttpTask> uiWarRes) {
            WarResult = uiWarRes;
        }

        #region 分步战斗的逻辑

        /// <summary>
        /// 把消息加入分步模式的动作列表里
        /// </summary>
        private void addMsgToStepRecorder(CMsgHeader msg) {
            if(bStepMode) {
                if(msg is CMsgSkillCast) {
                    CMsgSkillCast cast = msg as CMsgSkillCast;
                    BT_MonsterTeam team = teamFromPveId(cast.caster);
                    cast.curAngry = team.curAngry;
                }

                StepRecorder.Add(msg);
            }
        }

        private void reportStepToUI() {
            List<CMsgHeader> localCopy = new List<CMsgHeader>(StepRecorder);
            StepRecorder.Clear();

            AsyncTask.QueueOnMainThread (
                () => {
                    if(StepComplete != null) {
                        StepComplete(localCopy);
                    }
                }
            );
        }

        //只计算战斗开始的数据
        public void startWarOfWarBegin() {
            WarBegin();

            List<CMsgHeader> localCopy = new List<CMsgHeader>(StepRecorder);
            StepRecorder.Clear();

            TemporyData temp = Core.Data.temper;
            temp.warBattle = new BattleSequence();
            temp.warBattle.battleData = new BattleData() {
                info = localCopy,
            };
        }

        /// <summary>
        /// 开启一个回合的开始计算
        /// </summary>
        public void StartWarStep() {
            bool begin = RoundBegin();
            if(begin) {
                //当前回合有怒气技吗？
                BT_Skill angrySk = null;
                int Count = 0;
                bool castAngry = curRound_CastAngry(ref angrySk, ref Count);

                if(castAngry) {
                    addAngryByAoYi();
                    bufOrDebufSkill();
                    propertyChg();
                    propertyKill();
                    //通知UI层显示
					AngryIsReady(angrySk.id, Count, angrySk.forcastInfo());
                } else {
                    addAngryByAoYi();
                    bufOrDebufSkill();
                    propertyChg();
                    propertyKill();

                    castAngrySkill();
                    castNormalSkill();
                    fighting();
                    castEndSkill();

                    EndWarStep();
                }
                //通知UI
                reportStepToUI();
            }
        }

        /// <summary>
        /// 继续一个回合的计算,是否释放怒气技
        /// </summary>
        public void KeepWarStep(int castCount) {
            if(castCount >= 1) {
                castMultAngrySkill(castCount);
            } else {
                castEnemyAngrySkill();
            }

            castNormalSkill();
            fighting();
            castEndSkill();

            EndWarStep();
            //通知UI
            reportStepToUI();
        }

        /// <summary>
        /// 结束一个回合的计算
        /// </summary>
        private void EndWarStep() {
            ConsoleEx.DebugLog("############# EndWarStep ###########");
            bool over = checkToNextOrOver();

            if(!over) {
                #if DEBUG
                ConsoleEx.DebugLog("############## War is Over ##############");
                #endif
                WarEnd();
            }
            
        } 


        /// <summary>
        /// 战斗结束要发起网络请求.. 保存战斗数据到temper，以备重放功能
        /// </summary>
        private void WarEnd() { 
            TemporyData temp = Core.Data.temper;

            temp.warBattle = new BattleSequence();
            temp.warBattle.battleData = new BattleData() {
                info = _warRecorder,
            };

            //SettleBattle();
        }

        /// <summary>
        /// 怒气技能准备好了，通知UI层
        /// </summary>
		private void AngryIsReady(int skillId, int ClickCnt, ForcastSk forcastsk){
			CMsgAngryReady readyMsg = new CMsgAngryReady(ClickCnt, skillId, forcastsk);
            addMsgToRecorder(readyMsg);
        }
        #endregion

        /// <summary>
        /// 当前回合有足够的怒气，去释放怒气技能吗
        /// </summary>
        private bool curRound_CastAngry(ref BT_Skill angry, ref int Count) {
            bool can = false;

            List<BT_Skill> angryArr = _curAtter.get_AngrySkillArr;
            if(angryArr != null) {
                foreach(BT_Skill sk in angryArr) {
                    can = sk.param.nuqi <= _curAtter._selfTeam.curAngry;
                    can = can && sk.real == 0;
                    if(can) {
                        angry = sk;
                        Count = _curAtter._selfTeam.curAngry / sk.param.nuqi;
                        break;
                    }
                }
            }

            return can;
        }

		//敌人释放全部的怒气
		//客户端计算战斗逻辑的时候只能敌人释放
		private void CastAllAngry(List<BT_Skill> angryArr, BT_Monster mon) {
			if(angryArr != null && angryArr.Count > 0 && mon != null) {
				foreach(BT_Skill sk in angryArr) {
					bool can = sk.param.nuqi <= mon._selfTeam.curAngry;
					can = can && sk.real == 0;
					can = can && sk.isSkillByType(SkillOpData.Anger_Skill);
					if(can) {
						int Count = mon._selfTeam.curAngry / sk.param.nuqi;
						sk.RdCast = Count;
					}
				}
			}
		}

        private void WarBegin() {
            ConsoleEx.DebugLog("############## WarBegin ##############");
            _battleStatus = CMsgHeader.STATUS_WAR_BEGIN;

            int[] AttAoYiArr = null, DefAoYiArr = null;
            if(_attAoYi.getAoYi != null)
                AttAoYiArr = _attAoYi.getAoYi.ToArray();
            if(_defAoYi.getAoYi != null)
                DefAoYiArr = _defAoYi.getAoYi.ToArray();

            CMsgWarBegin msg = new CMsgWarBegin(_attTeam.toMonsterTeam(), _defTeam.toMonsterTeam(), AttAoYiArr, DefAoYiArr);
            addMsgToRecorder(msg);
        }

        private bool RoundBegin() {
            ConsoleEx.DebugLog("############## RoundBegin ##############");
            _battleStatus = CMsgHeader.STATUS_ROUND_BEGIN;
            _curAtter = _attTeam.curPet;
            _curDefer = _defTeam.curPet;

            bool canBegin = false;
            //------------ 可能因为自爆技能导致，后续队员的死亡 -------
			if(_curDefer != null && _curAtter != null && _curDefer.alive && _curAtter.alive) {
                canBegin = true;
            }

            if(canBegin) {
                //重置一个回合奥义释放的次数
                _AoYiCastCount_Att = 0;
                _AoYiCastCount_Def = 0;
                fullFateOneAngry();
                CMsgRoundBegin msg = new CMsgRoundBegin(_curAtter.pveId,_curDefer.pveId,
                    _curAtter.curAtt,_curDefer.curAtt,
                    _attTeam.curAngry,_defTeam.curAngry);
                addMsgToRecorder(msg);
            }

            return canBegin;
        }

    }
}
