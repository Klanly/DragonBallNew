using System;
using System.Text;
using System.Collections.Generic;

namespace AW.Battle {
    /// <summary>
    /// 整个战斗的核心控制类
    /// </summary>
    public partial class BT_Logical {
        //keep singleton
		public BT_Logical(ClientData netdata) {
            _battleStatus = CMsgHeader.STATUS_WAR_BEGIN;

			//客户端不计算世界boss的战斗
			_isWorldBoss = 0;
			//战斗统计信息
			statictis    = new BT_Statictis();
			//初始化Manager
			DragonManager dragonMgr = Core.Data.dragonManager;
			SkillManager skillMgr   = Core.Data.skillManager;

			if(netdata != null) {
				//初始化奥义
				_attAoYi = new BattleAoYi(netdata.attAoYi, dragonMgr, skillMgr);
				_defAoYi = new BattleAoYi(netdata.defAoYi, dragonMgr, skillMgr);

				//初始化队伍
//				_attTeam = new BT_MonsterTeam(netdata.attTeam, this, 0, "Att", _attAoYi, _defAoYi);
//				int attTeamCnt = _attTeam._team.Count;


				//wxl 
//				Battle_Monster tBM = netdata.defTeam.team [netdata.defTeam.team.Length - 1];
//				if (tBM != null) {
//					tBM.petNum = 10105;
//					tBM.property = 10;
//					//21006,21014,25006
//					tBM.lvl = 30;
//
//					SkillObj oneObj = new SkillObj ();
//					oneObj.skillId = 21006;
//					oneObj.skillLevel = 30;
//					SkillObj twoObj = new SkillObj ();
//					twoObj.skillId = 21014;
//					twoObj.skillLevel = 2;
//					SkillObj threeObj = new SkillObj ();
//					twoObj.skillId = 25006;
//					twoObj.skillLevel = 2;
//					tBM.skill = new SkillObj[3]{oneObj,twoObj,threeObj};
//					tBM.finalAtt = 8000;
//
//				}
//				for (int i = 0; i < netdata.defTeam.team.Length; i++) {
//					netdata.defTeam.team [i] = tBM;
//				}

//				for (int i = 0; i < netdata.attTeam.team.Length; i++) {
//					netdata.attTeam.team [i] = tBM;
//				}
				_attTeam = new BT_MonsterTeam(netdata.attTeam, this, 0, "Att", _attAoYi, _defAoYi);
				int attTeamCnt = _attTeam._team.Count;
				_defTeam = new BT_MonsterTeam(netdata.defTeam, this, attTeamCnt, "Def", _defAoYi, _attAoYi);

				_attTeam.setupRelation(_defTeam);
				_defTeam.setupRelation(_attTeam);

				m_ShabuReplay = false;
			}

            //分步战斗的数据初始化
            init();
			//初始化战斗结果，默认是输
			initWar();
			//战斗结果的存储地方
			_warRecorder = new List<CMsgHeader>();
        }

		//战斗的回合数
        private int _curLun = 0;
		//战斗的阶段状态
        private short _battleStatus = -1;

        //两个队伍
        private BT_MonsterTeam _attTeam;
        private BT_MonsterTeam _defTeam;
        //当前上阵的宠物
        private BT_Monster _curAtter;
        private BT_Monster _curDefer;
        //两个队伍的奥义
        private BattleAoYi _attAoYi;
        private BattleAoYi _defAoYi;

        // 1是世界boss 0 不是
        private short _isWorldBoss;
        // 记录下:一个回合，奥义释放的次数
        public int _AoYiCastCount_Att = 0;
        public int _AoYiCastCount_Def = 0;

        //对战时的差值
        public int fightDiff;
		//战斗的数据统计
		private BT_Statictis statictis = null;
		//保存战斗数据
		private List<CMsgHeader> _warRecorder = null;

        //战斗的输赢, 1 左边赢 0 右边赢
        private int WarIsOver = -1;

		//逻辑上战斗结束了吗？
		public bool WarIsOnGoing {
			get {
				return WarIsOver == -1;
			}
		}

		private bool m_ShabuReplay;

		void initWar() {
			/// --- 中途退出战斗一定算输 (当然不是重播）---
			TemporyData temp = Core.Data.temper;
			temp.warBattle = new BattleSequence();
			temp.warBattle.battleData = new BattleData() {
				iswin = 2,
			};
		}

		public void OnResetShabu()
		{
			m_ShabuReplay = false;
		}

		#region 战斗序列

		public void addMsgToRecorder(CMsgHeader msg) {
            //可能要加入分步列表里
            addMsgToStepRecorder(msg);

			if(msg is CMsgSkillCast) {
				CMsgSkillCast cast = msg as CMsgSkillCast;
				BT_MonsterTeam team = teamFromPveId(cast.caster);
				cast.curAngry = team.curAngry;
			}

			_warRecorder.Add(msg);

            if(msg is CMsgWarEnd) {
                CMsgWarEnd end = msg as CMsgWarEnd;
                WarIsOver = end.winner == "Att" ? 1 : 0;
            }

        }

		/// <summary>
		/// Generates the video.生成战斗的录像数据
		/// </summary>
		private string generateVideo() {
			#if Video
			StringBuilder video = new StringBuilder();

			foreach(CMsgHeader msg in _warRecorder) {
				if(msg != null) {
					video.Append(BT_WarUtils.ToJson(msg));
				}
			}
			string flat = video.ToString();
			//会自动Base64
			string compressed = Framework.DeflateEx.Compress(flat);
			return compressed;

			#else
			//只记录下登场回合的信息
			StringBuilder video = new StringBuilder();

			foreach(CMsgHeader msg in _warRecorder) {
				if(msg != null && msg.status == CMsgHeader.STATUS_ROUND_BEGIN) {
					video.Append(BT_WarUtils.ToJson(msg)).Append(",");
				}
			}
			string flat = video.ToString();
			if(!string.IsNullOrEmpty(flat)) {
				int length = flat.Length;
				if(length > 1) flat = flat.Substring(0, length - 1);
			}
			flat = "[" + flat + "]";

			ConsoleEx.DebugLog(flat, ConsoleEx.RED);

			return flat;
			#endif
		}

		/// <summary>
		/// 生成录像的反解析信息
		/// </summary>
		/// <returns>The video parse.</returns>
		private short[] generateVideoParse() {
			List<short> video = new List<short>();

			foreach(CMsgHeader msg in _warRecorder) {
				if(msg != null) {
					video.Add(msg.status);
				}
			}

			return video.ToArray();
		}
		#endregion

        #region 战斗统计
        ///
        /// ----   关于技能的统计，仅仅针对 攻击方   ----
        /// 

        //设定普通技能释放次数
        public void setTotalNormalSkill(BT_MonsterTeam Team, BT_Skill skill) {
            if(Team != null && skill != null) {
                string whichTeam = Team.getTeamName;
                if(whichTeam == "Att") {
                    //既不是怒气技也不是奥义技
                    if(skill.opObj.type != SkillOpData.Anger_Skill && skill.real != 1) {
						statictis._totalNoramSkill ++;
						statictis._NorSkillArr.Add(skill.id);
                    }
                }
            }
        }    

        //获取释放的技能ID列表
        public int[] getNormalSkillIDs {
			get {return statictis._NorSkillArr.ToArray();}
        }

        //获取本队的普通技能
        public int getTotalNormalSkill {
            get {
				return statictis._totalNoramSkill;
            }
        }

        //人均释放的普通图技能的次数
        public float getAverageNoramlSkill {
            get {
                int memberCount = _attTeam._team.Count;
				return statictis._totalNoramSkill * 1.0f/ memberCount; 
            }
        }

        ///
        /// -----   敌人一共死亡的波数   -----
        ///
        //-1代表没有打赢任何一个敌人
        public int getEnemyDieIndex {
            get {
				if(statictis.enemyIndex <= -1) return -1;
                //己方的个数
                int selfCount = _attTeam._team.Count;
				int b = statictis.enemyIndex - selfCount;
                return b;
            }
        }


        ///
        /// ----   关于杀人数的统计，仅仅针对 攻击方   ----
        ///

        //获取最大的攻击方的杀人数
        public int getMaxAttackTeamKillCount {
            get {
                int max = 0;
                foreach (BT_Monster pet in _attTeam._team) {
                    if(pet != null) {
                        int temp = pet.getKillCount;
                        if(temp > max) max = temp;
                    }
                }
                return max;
            }
        }

        //获取攻击方最大的杀人数的列表
        public MaxKillData getMaxDeadArray {
            get {
                int max = 0;

                int index = 0;
                int length = _attTeam._team.Count;
                for (int i = 0; i < length; i++) {
                    BT_Monster pet = _attTeam._team[i];
                    if(pet != null) {
                        int temp = pet.getKillCount;
                        if(temp > max) {
                            max = temp;
                            index = i;
                        }
                    }
                }

                BT_Monster maxPet = _attTeam._team[index];
                MaxKillOfWhoDid Role = new MaxKillOfWhoDid(){
                    num = maxPet._num,
                    attri = (int)maxPet._initProperty,
                    lv = maxPet._level
                };

                int[] killArray = maxPet.getKillIds;


                MaxKillData result = new MaxKillData() {
                    kill_array = killArray,
                    role = Role
                };

                return result;
            }
        }

        /// <summary>
        /// 仅仅作为数据统计的用法
        /// 杀人数最多的是哪个人
        /// </summary>
        [Serializable]
        public class MaxKillOfWhoDid {
            public int num;
            public int attri;
            public int lv;
        }

        [Serializable]
        public class MaxKillData {
            public MaxKillOfWhoDid role;
            public int[] kill_array;
        }

        #endregion

        #region 奥义相关操作

        //能否释放奥义技能
        public bool canCastAoYi(BT_Skill sk, BT_MonsterTeam Team) {
            int AoYiID = sk.id;
            BattleAoYi workAoYi = null;
            if(Team.getTeamName == "Att") {
                workAoYi = this._attAoYi;
            } else {
                workAoYi = this._defAoYi;
            }

            bool can = false;


			ConsoleEx.Write (" AY id = " + AoYiID + " name = " + Core.Data.dragonManager.getAoYiData(AoYiID).name ,"yellow");
            if(workAoYi.isFirstAoYi(AoYiID)) {
                if(workAoYi.curAoYiCast(AoYiID) == false) {
                    can = true;
                }
            } else {
                //前一个是否已经使用过
                bool pre = workAoYi.prevAoYiCast(AoYiID);
                bool cur = workAoYi.curAoYiCast(AoYiID);

				//wxl 
				if(/*pre == true &&*/ cur == false) {
                    can = true;
                }
            }


            return can;
        }

        //设置这个奥义已经使用过
        public void setCastAoYi(BT_Skill sk, BT_MonsterTeam Team) {
            int AoYiID = sk.id;
            BattleAoYi workAoYi = null;
            if(Team.getTeamName == "Att") {
                workAoYi = this._attAoYi;
            } else {
                workAoYi = this._defAoYi;
            }
            //设置这个奥义已经使用过
            workAoYi.setCurAoYiCast(AoYiID);
        }

        //重置奥义
        public void resetAoYi(BT_Skill sk, BT_MonsterTeam Team) {
            int AoYiID = sk.id;
            BattleAoYi workAoYi = null;
            if(Team.getTeamName == "Att") {
                workAoYi = this._attAoYi;
            } else {
                workAoYi = this._defAoYi;
            }
            //设置这个奥义未使用过
            workAoYi.resetCurAoYi(AoYiID);
        }

        #endregion

        #region 战斗过程中的小方法

        public BT_MonsterTeam teamFromPveId(int pveId) {
            int attTeamCnt = _attTeam._team.Count;
            return pveId < attTeamCnt ? _attTeam : _defTeam;
        }

        public BT_Monster enemy(BT_Monster self){
            if( self.pveId != this._curAtter.pveId &&
                self.pveId != this._curDefer.pveId)
                throw new DragonException("Current Monster is not atter nor defer");

            return self == _curAtter ? _curDefer : _curAtter;
        }

        //当前的状态
        public short status {
            get {
                return _battleStatus;
            }
        }
        #endregion

		#region 网络相关
        private HttpTask cachedTask = null;
        public HttpTask CachedTask {
            get { return cachedTask; }
        }

		/// <summary>
		/// 在UI层的EndOfWar来发起网络请求
		/// </summary>
		public void SettleBattle() {
			///加入数据的统计，将统计的结果通知给服务器
			TemporyData temp = Core.Data.temper;

			HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		
			int[] killArr = getMaxDeadArray.kill_array;
			statictis.anaylizeHp(_attTeam);
			int   star    = statictis.getRank(WarIsOver);
			MaxKillOfWhoDid MaxRole = getMaxDeadArray.role;

			Statistic any = new Statistic(){
				skillcount      = getTotalNormalSkill,
				skillcountratio = getAverageNoramlSkill,
				maxkill         = getMaxAttackTeamKillCount,
				enemyDieIndex   = getEnemyDieIndex,
				star            = star,
			};
			StaticsticList full = new StaticsticList() {
				skillarr  = getNormalSkillIDs,
				killarry  = killArr,
				role      = MaxRole,
			};

			ComboGather combo = new ComboGather() {
				btnMax      = temp.MaxCombo,
				btnTotal    = temp.TotalCombo,
				fingerMax   = temp.FingerMaxCombo,
				fingerTotal = temp.FingerTotalCombo,
			};

			ClientBattleCheckParam param = null;
			RequestType ReqType = RequestType.SETTLE_BOSSBATTLE;

			if(temp.currentBattleType == TemporyData.BattleType.BossBattle)
				param = new ClientBattleCheckParam (temp.clientReqParam, any, full, generateVideo(), generateVideoParse(), combo);
			else if(temp.currentBattleType == TemporyData.BattleType.FinalTrialShalu || 
				temp.currentBattleType == TemporyData.BattleType.FinalTrialBuou) {
				param = new ClientBTShaBuParam (temp.shaluBuOuParam, any, full, generateVideo(), generateVideoParse(), combo);
				ReqType = RequestType.SETTLE_SHABU;
			} else {
				ConsoleEx.DebugLog("TemporyData CurrentBattleType = " + temp.currentBattleType, ConsoleEx.RED);
			}

			temp.LetGoACT = HttpRequestFactory.ACTION_SETTLE_BATTLE_BOSS;

            param.sequence.LeftWin = WarIsOver;
			task.AppendCommonParam(ReqType, param);

			task.ErrorOccured = HttpResp_Error;
			task.afterCompleted = Http_Suc;

			//then you should dispatch to a real handler

			if(temp.currentBattleType == TemporyData.BattleType.FinalTrialShalu || 
			   temp.currentBattleType == TemporyData.BattleType.FinalTrialBuou)
			{
				if(!m_ShabuReplay)
				{
					task.DispatchToRealHandler();
					m_ShabuReplay = true;
				}
			}
			else
			{
				task.DispatchToRealHandler();
			}


            cachedTask = task;
			temp.warBattle.battleData.iswin = WarIsOver == 0 ? 2 : 1;

			SettleInLocal();
		}

		/// <summary>
		/// 本地的通知, 这个必须网络正常的情况下，才能走。如果不正常，则必须设定本地的星级为0
		/// </summary>
		void SettleInLocal() {
			//告诉客户端，当前的状态（星级）
			TemporyData temp = Core.Data.temper;
			if(temp.currentBattleType == TemporyData.BattleType.BossBattle) {
				NewFloor newFloor = Core.Data.newDungeonsManager.curFightingFloor;
				if(newFloor != null && newFloor.isBoss) {
					newFloor.star = Math.Max(newFloor.star, statictis.getRank(WarIsOver));
				}
			}
		}

		/// 
		/// 网络不正常，则必须设定本地的星级为0
		/// 
		void SttleInLocalErr() {
			//告诉客户端，当前的状态（星级）
			TemporyData temp = Core.Data.temper;
			if(temp.currentBattleType == TemporyData.BattleType.BossBattle) {
				NewFloor newFloor = Core.Data.newDungeonsManager.curFightingFloor;
				if(newFloor != null && newFloor.isBoss) {
					newFloor.star = 0;
				}
			}
		}

		void SyncPveTime()
		{
			NewDungeonsManager newDM = Core.Data.newDungeonsManager;
			for (int i = 1; i <= 4; i++)
			{
				ExploreConfigData config = newDM.GetExploreData (i);

				if (config != null && config.openfloor == newDM.curFightingFloor.config.ID)
				{
					HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
					task.AppendCommonParam(RequestType.SYNC_PVE, new SyncPveParam(Core.Data.playerManager.PlayerID));
					task.afterCompleted += Http_Suc;
					task.ErrorOccured += HttpResp_Error;
					task.DispatchToRealHandler();
					break;
				}
			}
		}

		private void Http_Suc (BaseHttpRequest request, BaseResponse response) {
			TemporyData temp = Core.Data.temper;
			if(temp.currentBattleType == TemporyData.BattleType.BossBattle) {
				//isSpecialGuide(WarIsOver);
				BattleResponse battleResp = response as BattleResponse;
				if(battleResp != null && battleResp.status != BaseResponse.ERROR && battleResp.data != null) {
					//保存战斗结果的数据
					storeWarResult(battleResp);
					handleResponse(request, battleResp);
					SyncPveTime ();

					///
					/// ------- 新手引导，第4关 -------
					///
#if NewGuide
					if(battleResp.data.battleData.isfirst != 1000)
#endif
					Core.Data.guideManger.SpecialGuideID = battleResp.data.battleData.isfirst;

					SettleInLocal();
				} else {
					//错误展示
					if(battleResp != null && battleResp.status == BaseResponse.ERROR) Core.Data.temper.WarErrorCode = battleResp.errorCode;
				}
			} else if(temp.currentBattleType == TemporyData.BattleType.FinalTrialShalu || 
				temp.currentBattleType == TemporyData.BattleType.FinalTrialBuou) {

				ClientBTShaBuResponse battleResp = response as ClientBTShaBuResponse;
				if(battleResp != null && battleResp.status != BaseResponse.ERROR && battleResp.data != null) {
					// 战斗序列
					temp.warBattle.battleData.stars = statictis.getRank(WarIsOver);
					temp.warBattle.reward = new BattleReward(){
						p   = battleResp.data.award,
						eco = battleResp.data.comboCoin,
					};
				} else {
					//错误展示
					if(battleResp != null && battleResp.status == BaseResponse.ERROR) Core.Data.temper.WarErrorCode = battleResp.errorCode;
				}
			}

            //通知UI展示战斗结算
            if(WarResult != null) WarResult(cachedTask);
		}

        /// <summary>
        /// 保存战斗结果的数据
        /// </summary>
        /// <param name="battleResp">Battle resp.</param>
		private void storeWarResult(BattleResponse battleResp) {
            //把服务器过来的战斗结算消息赋值
            TemporyData temp = Core.Data.temper;
			NewDungeonsManager ddMgr = Core.Data.newDungeonsManager;

            if(temp.warBattle == null) {
                temp.warBattle = new BattleSequence() {
                    //战斗同步数据
                    sync   = battleResp.data.sync,
                    // 系统奖励
                    reward = battleResp.data.reward,
                    // 掠夺奖励
                    ext    = battleResp.data.ext,
                    //赌博结果
                    gambleResult = battleResp.data.gambleResult,
                    //雷达组队神秘大奖
                    radarReward  = battleResp.data.radarReward, 
                    //战斗序列
                    battleData   = battleResp.data.battleData,
					//PVE活动副本倒计时
					explorDoors  = battleResp.data.explorDoors,
					//已打通的次数
					passCount    = battleResp.data.passCount,
					//连击
					comboReward  = battleResp.data.comboReward,
                };
            } else {
                //战斗同步数据
                temp.warBattle.sync   = battleResp.data.sync;
                // 系统奖励
                temp.warBattle.reward = battleResp.data.reward;
                // 掠夺奖励
                temp.warBattle.ext    = battleResp.data.ext;
                //赌博结果
                temp.warBattle.gambleResult = battleResp.data.gambleResult;
                //雷达组队神秘大奖
                temp.warBattle.radarReward  = battleResp.data.radarReward;
                // 战斗序列
                temp.warBattle.battleData.stars   =  battleResp.data.battleData.stars;
				//技能副本的引导需要这个值
				temp.warBattle.battleData.isfirst = battleResp.data.battleData.isfirst;

				temp.warBattle.explorDoors  = battleResp.data.explorDoors;
				//已打通的次数
				temp.warBattle.passCount    = battleResp.data.passCount;
				//连击
				temp.warBattle.comboReward  = battleResp.data.comboReward;
            }
				
			//PVE活动副本倒计时
			if(battleResp.data.explorDoors != null) 
				ddMgr.explorDoors = battleResp.data.explorDoors;
				
			//保存pvp挑战次数
			NewFloor floor = ddMgr.GetFloorData (ddMgr.curFightingFloor.config.ID);
			if(floor != null) floor.passTimes = battleResp.data.passCount;
        }

        /// <summary>
        /// 把战斗结果加入本地数据层
        /// </summary>
        private void handleResponse(BaseHttpRequest request, BattleResponse r) {
            TemporyData temp = Core.Data.temper;
            if(r != null) {
				if(r.data != null) {
					if(r.data.reward != null && r.data.sync != null) {
						PlayerManager player = Core.Data.playerManager;

						player.RTData.curVipLevel = r.data.sync.vip;
						temp.mPreLevel = player.RTData.curLevel;
						temp.AfterBattleLv = r.data.sync.lv;
					}

					if(r.data.battleData != null) {
						r.data.battleData.rsty = null;
						r.data.battleData.rsmg = null;
					}
				}
            } 
        }

		private void HttpResp_Error (BaseHttpRequest request, string error) {
            //错误展示
            Core.Data.temper.WarReq = request;
            //通知UI展示战斗结算
            if(WarResult != null) WarResult(cachedTask);
			SttleInLocalErr();
		}


		#endregion

        #region 完整战斗的逻辑
        //TODO:战斗逻辑在这里...
        public int startWar(){
            WarBegin();
			return EveryRound();
        }

		// ... 战斗结束 ...
		public void EndWar() {
			TemporyData temp = Core.Data.temper;

			temp.warBattle = new BattleSequence();
			temp.warBattle.battleData = new BattleData() {
				info = _warRecorder,
			};

			//SettleBattle();
		}

		private int EveryRound() {
			do {
                bool begin = RoundBegin();

                if(begin) {
					addAngryByAoYi();
					bufOrDebufSkill();
					propertyChg();
					propertyKill();
					castAngrySkill();
					castNormalSkill();
					fighting();
					castEndSkill();
				}

			} while(checkToNextOrOver());

			return _attTeam.over ? 2 : 1;
		}

		//这个函数已不再使用
        private int startOneRound() {
            _battleStatus = CMsgHeader.STATUS_ROUND_BEGIN;
            _curAtter = _attTeam.curPet;
            _curDefer = _defTeam.curPet;

            //------------ 可能因为自爆技能导致，后续队员的死亡 -------
            if(_curDefer.alive && _curAtter.alive) {
                //重置一个回合奥义释放的次数
                _AoYiCastCount_Att = 0;
                _AoYiCastCount_Def = 0;
                fullFateOneAngry();

                CMsgRoundBegin msg = new CMsgRoundBegin(_curAtter.pveId,_curDefer.pveId,
                    _curAtter.curAtt,_curDefer.curAtt,
                    _attTeam.curAngry,_defTeam.curAngry);
                addMsgToRecorder(msg);

                addAngryByAoYi();
                bufOrDebufSkill();
                propertyChg();
                propertyKill();
                castAngrySkill();
                castNormalSkill();
                fighting();
                castEndSkill();
            }

            checkToNextOrOver();
            return _attTeam.over ? 2 : 1;
        }

        private void fullFateOneAngry() {
            //额外的气，每个缘配齐加一些气
			if(_curAtter.getFateNuqi > 0) {
                if(!_curAtter.getFateAngry) {
					_attTeam.addAngry(_curAtter.getFateNuqi);
                    _curAtter.setFateAngry();
                }
            }
            //额外的气，每个缘配齐加一些气
			if(_curDefer.getFateNuqi > 0) {
                if(!_curDefer.getFateAngry) {
					_defTeam.addAngry(_curDefer.getFateNuqi);
                    _curDefer.setFateAngry();
                }
            }
        }


        private void addAngryByAoYi(){
            _battleStatus = CMsgHeader.statusAttend;

            //变全属性...
            _curAtter.shouldAddAngryByAoYi();
            _curDefer.shouldAddAngryByAoYi();
        }

        private void propertyChg(){
            _battleStatus = CMsgHeader.STATUS_PROPERTY_CHG;

            //变全属性...
            _curAtter.shouldCastPropertyChangeToAll();
            _curDefer.shouldCastPropertyChangeToAll();
        }

        private void bufOrDebufSkill() {
            _battleStatus = CMsgHeader.STATUS_BUFF_DEBUFF;
            //检测是否有buff or debuff在这个时候生效
            _curAtter.castBufforDebuff();
            _curDefer.castBufforDebuff();
        }

        private void propertyKill(){
            _battleStatus = CMsgHeader.STATUS_PROPERTY_KILL;
            float[] proMul = BT_WarUtils.propertyVs(_curAtter._property, _curDefer._property);

            if(proMul[0] > 1) {
                //无视属性克制
                if(_curDefer.shouldCastIgnorePropertyKill())
                    return;
            }

            if(proMul[1] > 1) {
                //无视属性克制
                if(_curAtter.shouldCastIgnorePropertyKill())
                    return;
            }

            _curAtter.curAttEnhance(proMul[0]);

            if(_isWorldBoss == 0)//不是世界boss
                _curDefer.curAttEnhance(proMul[1]);

            CMsgPropertyEnchance msg = new CMsgPropertyEnchance(proMul[0],proMul[1], _curAtter.curAtt, _curDefer.curAtt);
            addMsgToRecorder(msg);
        }

        private void castSkill(short skType, bool onlyEnmy = false){
            if(skType != SkillOpData.Death_Closed_Skill) {
                bool attCheck = _curAtter.alive;
                bool defCheck = _curDefer.alive;

                if (SkillOpData.Anger_Skill == skType || SkillOpData.Common_Skill == skType) {
                    if(!attCheck || !defCheck)
                        return;
                }

                List<BT_Skill> castArr = new List<BT_Skill>();
                List<BT_Skill> realCastArr = new List<BT_Skill>();

                ///
                /// 保证己方的怒气技能优先触发
                ///

                if(defCheck) {
                    List<BT_Skill> defCast = _curDefer.canCastSkill(skType);
					CastAllAngry(defCast, _curDefer);
                    castArr.AddRange(defCast);
                }

                if(attCheck && onlyEnmy == false) {
                    List<BT_Skill> attCast = _curAtter.canCastSkill(skType);
                    castArr.AddRange(attCast);
                }

                if(castArr.Count > 0) {
                    castArr.Sort(BT_WarUtils.sortSkillArr);

                    bool cancelAoyi = false;
                    foreach (BT_Skill sk in castArr) {
                        //如果是某些特殊技能的情况下，则奥义技能不应该释放
                        if(sk.op == 27 || sk.op == 29 || sk.op == 202) {
                            if(sk.real == 1) {
                                cancelAoyi = true;
                            }
                        } 
                    }

                    if(cancelAoyi) {
                        foreach (BT_Skill sk in castArr) {
                            if(sk.real != 1) {
                                realCastArr.Add(sk);
                            } else {
                                resetAoYi(sk, sk.owner.ownerTeam);
                            }
                        }
                    } else {
                        realCastArr = castArr;
                    }

                    if(realCastArr.Count > 0) {
                        foreach (BT_Skill sk in realCastArr) {
                            //不能触发特殊技能（属性不被克制）,也不能触发变为全属性技能, 也不能是登场加怒气的技能
                            if(sk.op != 27 && sk.op != 29 && sk.op != 202) {
                                sk.castSkill();
								sk.RdCast = 1;
                            }
                        }
                    }
                }
            }
            else{
                ConsoleEx.DebugLog("战斗模块不牵扯濒死技能!");
            }
        }

        private void castAngrySkill() {
            _battleStatus = CMsgHeader.STATUS_NOMARL_SKILL;
            castSkill(SkillOpData.Anger_Skill);
        }

        /// <summary>
        /// 只释放敌人的怒气技.
        /// </summary>
        private void castEnemyAngrySkill() {
            _battleStatus = CMsgHeader.STATUS_NOMARL_SKILL;
            castSkill(SkillOpData.Anger_Skill, true);
        }

        /// <summary>
        /// 多次释放同一个怒气技, 技能不排序
        /// </summary>
        private void castMultAngrySkill(int MulTimes = 1) {
            short skType = SkillOpData.Anger_Skill;

            bool attCheck = _curAtter.alive;
            bool defCheck = _curDefer.alive;

            if(!attCheck || !defCheck)
                return;
            
			List<BT_Skill> castArr     = new List<BT_Skill>();

			List<BT_Skill> attCast = _curAtter.canCastSkill(skType);
			foreach(BT_Skill sk in attCast)
				sk.RdCast = MulTimes;

			castArr.AddRange(attCast);

			List<BT_Skill> defCast = _curDefer.canCastSkill(skType);
			CastAllAngry(defCast, _curDefer);
			castArr.AddRange(defCast);

            if(castArr.Count > 0) {

				foreach (BT_Skill sk in castArr) {
                    //不能触发特殊技能（属性不被克制）,也不能触发变为全属性技能, 也不能是登场加怒气的技能
                    if(sk.op != 27 && sk.op != 29 && sk.op != 202) {
                        sk.castSkill();
                        sk.RdCast = 1;
                    }
                }

            }
          
        }


        private void castNormalSkill() {
            _battleStatus = CMsgHeader.STATUS_NOMARL_SKILL;
            castSkill(SkillOpData.Common_Skill);
        }

        /// <summary>
        /// 对决
        /// </summary>
        private void fighting() {
            _battleStatus = CMsgHeader.STATUS_ATTACK;

            while(_curAtter.alive && _curDefer.alive) {
                fightDiff = Math.Abs(_curAtter.curAtt - _curDefer.curAtt);
                int att = Math.Min(_curAtter.curAtt, _curDefer.curAtt);

                _curAtter.sufferNormalAttack(att, _curDefer);
                _curDefer.sufferNormalAttack(att, _curAtter);
            }
        }

        //战后技能.如果有一方活着的话...
        private void castEndSkill() {    
            _battleStatus = CMsgHeader.STATUS_NOMARL_SKILL;
            castSkill(SkillOpData.After_War_Skill);
        }

        private bool checkToNextOrOver() { //下一波?
			bool next = false;

            fightDiff = 0;

            //---- 加入杀人统计---
            if(!_curDefer.alive) {
                _curAtter.addKillCount(_curDefer);
                //记录下当前敌人在第几波
                statictis.enemyIndex = _curDefer.pveId;
            }

            if(!_curAtter.alive) {
                _curDefer.addKillCount(_curAtter);
            }

            if(_attTeam.over || _defTeam.over){
                _battleStatus = CMsgHeader.STATUS_WAR_END;


                CMsgWarEnd msg = null;
                if(_attTeam.over){
                    msg = new CMsgWarEnd("Def");
                } else {
                    msg = new CMsgWarEnd("Att");
                }

                addMsgToRecorder(msg);
                ConsoleEx.DebugLog("CheckToNextOrOver: Game Over!");
            } else{//下一回合开始.
                _curLun++;
                _curAtter.enterNextLun();
                _curDefer.enterNextLun();
                
				//可以进入下个回合
				next = true;
            }

			return next;
        }

        #endregion

    }
}