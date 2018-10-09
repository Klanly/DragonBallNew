using System;
using System.Collections.Generic;

namespace AW.Battle {

    //定义伤害类型
    public enum BT_Hurt_Type {
        //普通攻击
        HURT_COMMON,
        //普通技能攻击
        HURT_SKILL,
        //怒气技能攻击
        HURT_ANGRY_SKILL,
    }

    //定义伤害
    public struct BT_Hurt {
        //伤害类型
        public BT_Hurt_Type hurtType;
        //伤害值
        public float damageVal;
    }

    /// <summary>
    /// 战斗过程中，宠物的定义
    /// </summary>
    public class BT_Monster {

        private int _id;
        public  int _num;
        public  MonsterAttribute _property;
        public  MonsterAttribute _initProperty;
        public  int _level;
		private Int32Fog _finalAtt;

        // --- 技能ID -----
		private List<SkillObj> _nSkill = null;
		private List<SkillObj> _aSkill = null;

        // ---- 技能 skill 对象List -----
        private List<BT_Skill> _askillArr = null; //技能..
        private List<BT_Skill> _nskillArr = null;

        //缘分是否配齐
        private bool _fullFated = false;
		//缘分提供的怒气
		private int _fateNuqi = 0;
        //增加过怒气吗
        private bool _fullFatedAdd = false;
        //使用了缘额外添加的气
        private bool _useFateAngry = false;
        //杀死的敌人数
        private int _killCount = 0;
        //击杀的敌人Id
        private List<int> _killEnemyId = null;

        //当前的血或者说是战斗力
		private Int32Fog _curAtt;
        //是否还是活着
        private bool _isAlive = true;
        //是否进入濒死
        public bool _almostDie = false;
        //Index索引
        private int _pveIndex;
		//释放是boss
		private short _isBoss = 0;

        //relation...
        public BT_Logical _war;
        public BT_MonsterTeam _selfTeam;
        public BT_MonsterTeam _enemyTeam;

        //记录轮数
        private int _lastLun = 0;

        //buff & debuff...
        private Dictionary<short, BT_BuffOrDebuff> _buffArr;
        private Dictionary<short, BT_BuffOrDebuff> _debuffArr;


        // --------------  需要的底层依赖 -----------------
        private SkillManager skillMgr = null;


        public BT_Monster(Battle_Monster value, int pveIndex, BT_Logical war, BT_MonsterTeam selfTeam, BT_MonsterTeam enemyTeam) {
            //初始化 技能
			_nSkill    = new List<SkillObj>();
			_aSkill    = new List<SkillObj>();
            _nskillArr = new List<BT_Skill>();
            _askillArr = new List<BT_Skill>();

            //初始化 敌人ID
            _killEnemyId = new List<int>();
            //初始化 Buff & Debuff
            _buffArr   = new Dictionary<short, BT_BuffOrDebuff>();
            _debuffArr = new Dictionary<short, BT_BuffOrDebuff>();

            //初始化底层依赖
            skillMgr   = Core.Data.skillManager;

            this._id       = value.petId;
            this._num      = value.petNum;
            this._property = (MonsterAttribute) value.property;
            _initProperty  = _property; //记住默认初始的属性，部分宠物可能会属性转换
            this._level    = value.lvl;
            this._finalAtt = value.finalAtt;
            _killCount     = 0;
            //缘配齐
            _fullFated     = value.fullfated;
			_fateNuqi      = value.nuqi;
            float skillup  = value.skillUp;   // 技能概率的提升
			this._isBoss    = value.isBoss;    // 是否是boss
            //取出技能1，2，3
			//初始化宠物的技能信息
			parseSkill(value, skillup);
        
            _pveIndex = pveIndex;
            _war      = war;
            _selfTeam = selfTeam;
            _enemyTeam= enemyTeam;

            this.setCurAtt(_finalAtt);
        }

        public void setEnmeyTeam(BT_MonsterTeam enemyTeam){
            _enemyTeam = enemyTeam;
        }

        //提升技能触发概率
        private void improvePossibility(BT_Skill sk, float rate){
            if(sk.param.gailv > 0) {
                sk.param.gailv = MathHelper.MidpointRounding(sk.param.gailv * rate);
                if(sk.param.gailv > 100) sk.param.gailv = 100;
            }       
        }

		#region 提取技能信息

		void parseSkill(Battle_Monster value, float skillup) {
			//取出技能1，2，3
			SkillObj skId1 = null, skId2 = null, skId3 = null;
			if(value.skill != null) {
				skId1 = value.skill.Value<SkillObj>(0);
				skId2 = value.skill.Value<SkillObj>(1);
				skId3 = value.skill.Value<SkillObj>(2);
			}

			if(skId1 == null) skId1 = new SkillObj();
			if(skId2 == null) skId2 = new SkillObj();
			if(skId3 == null) skId3 = new SkillObj();

			_nSkill.Add(skId1);
			//只有宠物大于30级的时候，才能释放第二个技能
			if(_level >= 30 && skId2.skillId != 0) {
				_nSkill.Add(skId2);
			} 

			_aSkill.Add(skId3);

			//获取普通技能
			foreach (SkillObj nSkill in _nSkill) {
				if(nSkill.skillId > 0) {
					BT_Skill sk = new BT_Skill(nSkill.skillId, nSkill.skillLevel, skillMgr, this);
					if(sk != null) {
						improvePossibility(sk, skillup);
						_nskillArr.Add(sk);
					}
				}
			}
			//获取怒气技能
			foreach (SkillObj aSkill in _aSkill) {
				if(aSkill.skillId > 0) {
					BT_Skill sk = new BT_Skill (aSkill.skillId, aSkill.skillLevel, skillMgr, this);
					improvePossibility(sk, skillup);
					_askillArr.Add(sk);
				}
			}
		}

		#endregion

        #region Get 方法合集
        public int getServerId {
            get {
                return _id;
            }
        }

        //增加一个杀死敌人的数量
        public void addKillCount(BT_Monster Enemy) {
            _killCount ++;
            if(Enemy != null) {
                _killEnemyId.Add(Enemy._num);
            }
        }

        //返回杀死敌人的Num列表
        public int[] getKillIds {
            get {
                return _killEnemyId.ToArray();
            }
        }
        //返回杀死敌人的个数
        public int getKillCount {
            get {
                return _killCount;
            }
        }

        //返回缘分是否配齐
        public bool getFullFated {
            get {
                return _fullFated;
            }
        }

		//返回缘分提供的怒气
		public int getFateNuqi {
			get {
				return _fateNuqi;
			}
		}

        //设置缘已设定了怒气
        public void setFateAngry() {
            _fullFatedAdd = true;
        }

        public bool getFateAngry {
            get {
                return _fullFatedAdd;
            }
        }

        //使用了缘额外添加的气
        public void useFateAngry() {
            _useFateAngry = true;
        }

        public bool isUsedFateAngry {
            get {
                return _useFateAngry;
            }
        }

        //返回Normal技能
        public List<BT_Skill> get_NormalSkillArr {
            get {
                return _nskillArr;
            }
        }

        //返回怒气技能
        public List<BT_Skill> get_AngrySkillArr {
            get {
                return _askillArr;
            }
        }

        //添加怒气技能
        public void set_AngrySkillArr(BT_Skill askill) {
            _askillArr.Add(askill);
        }

        //添加Normal技能
        public void set_NormalSkillArr(BT_Skill nskill) {
            _nskillArr.Add(nskill);
        }

        //加血
        public void curAttEnhance(float mul){
            setCurAtt(_curAtt * mul);
        }

        //加血
        public void curAttAdd(float add){
            setCurAtt(_curAtt + add);
        }

        //返回初始化的时候宠物的攻击力
        public int initAtt {
            get {
                return _finalAtt;
            }
        }
        //返回当前宠物的攻击力
        public int curAtt {
            get {
                return _curAtt;
            }
        }

        public bool alive {
            get {
                return _isAlive;
            }
        }

        public void setAlive() {
            _isAlive = true;
        }

        public int pveId {
            get {
                return _pveIndex;
            }
        }

        public void setCurAtt(float att, bool ignoreDS = false){
            _curAtt = MathHelper.MidpointRounding(att);
            if(_curAtt <= 0){
                _curAtt = 0;

                if(!ignoreDS) {
                    //这里可能触发濒死技...
                    _almostDie = true;
                    List<BT_Skill> castArr = canCastSkill(SkillOpData.Death_Closed_Skill);
                    foreach (BT_Skill sk in castArr) {
                        sk.castSkill();
                    }
                }

                if(_curAtt <= 0)
                    _isAlive = false;
                //这里看是否真挂了...
                if(!alive){
					MonsterData md = Core.Data.monManager.getMonsterByNum(_num);
					int Ap = md == null ? BT_WarUtils.Unit_Angry : md.nuqi2;
                    _selfTeam.curPetDie(_fullFated, isUsedFateAngry, Ap);
                    clearBuffOrDebuff();
                }
            }
        }

        public BT_MonsterTeam ownerTeam {
            get {
                return _selfTeam;
            }
        }

        public BT_MonsterTeam vsTeam {
            get {
                return _enemyTeam;
            }
        }

        #endregion

        #region Buff & Debuff

        public void addBuffOrDebuff(BT_BuffOrDebuff bufOrDebuff, bool beBuff = true) {
            Dictionary<short, BT_BuffOrDebuff> worked = beBuff ? _buffArr : _debuffArr;
            worked[bufOrDebuff.type] = bufOrDebuff;
        }

        public BT_BuffOrDebuff getBuffOrDebuff(short type, bool beBuff = true) {
            Dictionary<short, BT_BuffOrDebuff> worked = beBuff ? _buffArr : _debuffArr;

            BT_BuffOrDebuff buf = null;
            if(!worked.TryGetValue(type, out buf)){
                buf = null;
            }

            return buf;
        } 

        //进入下个回合
        /// <summary>
        /// 目前Buff和Debuff只有特定的两个而且还都是Buff，就是 CMsgHeader.BUFF_DEBUFF_IMMUNITY 和 CMsgHeader.BUFF_DEBUFF_REDUCE_HP
        /// </summary>
        public void enterNextLun(){
            _lastLun ++;
            //一些buff debuff 更新...
            BT_BuffOrDebuff immunityBuff = getBuffOrDebuff(CMsgHeader.BUFF_DEBUFF_IMMUNITY);
            if(immunityBuff != null) {
                immunityBuff.round --;
                if(immunityBuff.round < 0) immunityBuff.round = 0;
                _buffArr[CMsgHeader.BUFF_DEBUFF_IMMUNITY] = immunityBuff;
            }

            BT_BuffOrDebuff ReduceHpDebuff = getBuffOrDebuff(CMsgHeader.BUFF_DEBUFF_REDUCE_HP);
            if(ReduceHpDebuff != null) {
                ReduceHpDebuff.round --;
                if(ReduceHpDebuff.round < 0) ReduceHpDebuff.round = 0; 
                _buffArr[CMsgHeader.BUFF_DEBUFF_REDUCE_HP] = ReduceHpDebuff;
            }
        }

        //如果宠物死亡，则要清除BuffOrDebuff状态
        public void clearBuffOrDebuff() {
            BT_BuffOrDebuff immunityBuff = getBuffOrDebuff(CMsgHeader.BUFF_DEBUFF_IMMUNITY);
            if(immunityBuff != null){
                _buffArr.Remove(CMsgHeader.BUFF_DEBUFF_IMMUNITY);
            }

            BT_BuffOrDebuff ReduceHpDebuff = getBuffOrDebuff(CMsgHeader.BUFF_DEBUFF_REDUCE_HP);
            if(ReduceHpDebuff != null) {
                _buffArr.Remove(CMsgHeader.BUFF_DEBUFF_REDUCE_HP);
            }
        }

        #endregion

        #region 技能释放的判定

        //能否遭受技能的攻击
        public bool canSufferSkillDamage() {
            bool shouldSuffer = true;
            BT_BuffOrDebuff immunityBuff = getBuffOrDebuff(CMsgHeader.BUFF_DEBUFF_IMMUNITY);
            if(immunityBuff != null && immunityBuff.round >0 ) {
                shouldSuffer = false;
            }
            if(!shouldSuffer) {
                CMsgSkillBuffDebuffEffect msg = new CMsgSkillBuffDebuffEffect(this, immunityBuff);
                _war.addMsgToRecorder(msg);    
            }
            return shouldSuffer;
        }

        //能否释放技能 -- 根据Debuff的状态来决定
        public bool canCastSkillAcc2BuffOrDebuff(BT_Skill sk, int skType){
            bool canCast = true;
            if(skType == SkillOpData.Common_Skill) {
                BT_BuffOrDebuff sealDebuff = getBuffOrDebuff(CMsgHeader.BUFF_DEBUFF_SEAL, false);
                if(sealDebuff != null && _lastLun == 0){
                    canCast = false;

                    CMsgSkillBuffDebuffEffect msg = new CMsgSkillBuffDebuffEffect(this, sealDebuff);
                    msg.arg1 = sk.id;
                    _war.addMsgToRecorder(msg);
                }
            }
            return canCast;
        }

        /// <summary>
        /// 怒气/普通技 能否释放. 普通技和怒气技是一种类型的分类，而死亡技则是另一种分类
        /// </summary>
        /// <returns><c>true</c>, if cast skill was caned, <c>false</c> otherwise.</returns>
        /// <param name="skType">Sk type.</param>
        public List<BT_Skill> canCastSkill(int skType) {
            List<BT_Skill> skillArr = skType == SkillOpData.Anger_Skill ? _askillArr : _nskillArr;
            if(skType == SkillOpData.Death_Closed_Skill) {
                //如果是死亡技,则需要考虑他可能是普通技能
                skillArr.AddRange(_askillArr);
            }

            List<BT_Skill> castArr = new List<BT_Skill>();
            if(skillArr != null) {
                foreach (BT_Skill sk in skillArr) {
                    if (sk.isSkillByType(skType) && sk.canCast() && canCastSkillAcc2BuffOrDebuff(sk, skType)) {

                        //奥义技能，如果是奥义技能则考虑当前的奥义是否
                        if(sk.real == 1) {
                            bool canAoYICast = false;
                            if(_selfTeam.getTeamName == "Att") {
                                if(_war._AoYiCastCount_Att == 0) {
                                    _war._AoYiCastCount_Att = 1;
                                    canAoYICast = true;
                                } 
                            } else {
                                if(_war._AoYiCastCount_Def == 0) {
                                    _war._AoYiCastCount_Def = 1;
                                    canAoYICast = true;
                                }
                            }

                            if(canAoYICast) {
                                castArr.Add(sk);
                            } else {
                                //如果这个奥义不适用，则标注为没释放过
                                _war.resetAoYi(sk, _selfTeam);
                            }
                        } else {
                            castArr.Add(sk);
                        }

                    }
                }
            }

            return castArr;
        }

        //返回实现伤害值
        public int sufferAttack(BT_Hurt damage, BT_Monster enemy) {
            int finalDamage = MathHelper.MidpointRounding(damage.damageVal);
            //技能来的攻击
            if( (damage.hurtType == BT_Hurt_Type.HURT_SKILL || damage.hurtType == BT_Hurt_Type.HURT_ANGRY_SKILL) && canSufferSkillDamage() == false){
                finalDamage = 0;
            }

            #region 各种buff or defbuff处理...

            //伤害全部防御技能
            short bufDebuffType = CMsgHeader.BUFF_DEBUFF_GOLDDEFENSE;
            BT_BuffOrDebuff goldShieldBuf = getBuffOrDebuff(bufDebuffType);
            if(goldShieldBuf != null && -- goldShieldBuf.round >= 0) {
                finalDamage = 0;

                CMsgSkillBuffDebuffEffect msg = new CMsgSkillBuffDebuffEffect(this, goldShieldBuf);
                _war.addMsgToRecorder(msg);

                _buffArr[bufDebuffType] = goldShieldBuf;
            }

            //伤害加深
            bufDebuffType = CMsgHeader.BUFF_DEBUFF_HURTUP;
            BT_BuffOrDebuff hurtUpDebuff = getBuffOrDebuff(bufDebuffType, false);
            if(hurtUpDebuff != null && --hurtUpDebuff.round >= 0){
                int rDamage = MathHelper.MidpointRounding(damage.damageVal * (1 + hurtUpDebuff.rate * Consts.oneHundred));

                CMsgSkillBuffDebuffEffect msg = new CMsgSkillBuffDebuffEffect(this, hurtUpDebuff);
                msg.arg1 = MathHelper.MidpointRounding(damage.damageVal);
                msg.arg2 = rDamage;
                _war.addMsgToRecorder(msg);
                _debuffArr[bufDebuffType] = hurtUpDebuff;

                finalDamage = rDamage;
            }

            //反弹部分伤害到敌人...
            bufDebuffType = CMsgHeader.BUFF_DEBUFF_REBOUND;
            BT_BuffOrDebuff reboundBuff = getBuffOrDebuff(bufDebuffType);
            if(reboundBuff != null && --reboundBuff.round >= 0 && enemy.alive){
                float rebound = damage.damageVal * reboundBuff.rate * Consts.oneHundred;
                damage.damageVal -= rebound;

                enemy.sufferNormalAttack(MathHelper.MidpointRounding(rebound), this, true);
                _buffArr[bufDebuffType] = reboundBuff;

                finalDamage = MathHelper.MidpointRounding (damage.damageVal);
            }

            //护盾技能...
            bufDebuffType = CMsgHeader.BUFF_DEBUFF_DEFENSE;
            BT_BuffOrDebuff shieldBuff = getBuffOrDebuff(bufDebuffType);
            if(shieldBuff != null && shieldBuff.suckDmg > 0) {
                int beforeShield = shieldBuff.suckDmg;
                if(beforeShield > damage.damageVal){
                    shieldBuff.suckDmg -= MathHelper.MidpointRounding(damage.damageVal);
                    finalDamage = 0;
                } else {
                    damage.damageVal -= beforeShield;
                    shieldBuff.suckDmg = 0;
                    finalDamage = MathHelper.MidpointRounding(damage.damageVal);
                }

                CMsgSkillBuffDebuffEffect msg = new CMsgSkillBuffDebuffEffect(this, shieldBuff);
                msg.arg1 = beforeShield;
                msg.arg2 = shieldBuff.suckDmg;
                _war.addMsgToRecorder(msg);

                _buffArr[bufDebuffType] = shieldBuff;
            }

            #endregion

            setCurAtt(_curAtt - finalDamage);

            return finalDamage;
        }

        //受到BufforDebuff
        public void castBufforDebuff() {
            BT_BuffOrDebuff ReduceHpDebuff = getBuffOrDebuff(CMsgHeader.BUFF_DEBUFF_REDUCE_HP);
            if(ReduceHpDebuff != null && ReduceHpDebuff.round > 0) {
                BT_Skill skill = ReduceHpDebuff.skill;
                if(skill != null) {
                    skill.castDecSkill(skill.param.num);
                }
            }
        }

        //受到普通伤害.
        public void sufferNormalAttack(int att, BT_Monster enemy, bool beReBound = false){
            CMsgNormalAttack msg = new CMsgNormalAttack(_pveIndex, att, beReBound ? CMsgHeader.STATUS_REBOUND_ATT : CMsgHeader.STATUS_ATTACK);
            _war.addMsgToRecorder(msg);

            BT_Hurt hurt = new BT_Hurt() {
                hurtType = BT_Hurt_Type.HURT_COMMON,
                damageVal = att
            };

            int finalDamage = sufferAttack(hurt, enemy);
            msg.finalAtt = finalDamage;
            msg.curAtt = curAtt;
        }

        //复活.
        public void revive(){
            if(alive) {
                ConsoleEx.DebugLog("Monster is alive. Don't need to revive.");
            }

            _isAlive = true;
            _lastLun = 0;
            setCurAtt(_finalAtt);
            //设置为初始的属性
            _property = _initProperty;
        }

        #endregion

        #region 属性方面的技能
        //忽视属性克制
        public bool shouldCastIgnorePropertyKill(){
            foreach (BT_Skill sk in _nskillArr) {
                if(sk != null && sk.castIgnorePropertyKill()){
                    return true;
                }
            }
            return false;
        }
        //属性变为全
        public bool shouldCastPropertyChangeToAll(){
            foreach (BT_Skill sk in _nskillArr) {
                if( sk != null && sk.castPropertyChangeToAll(_property)){
                    _property = MonsterAttribute.ALL;
                    return true;
                }
            }
            return false;
        }

        #endregion

        //气源奥义可能会开始的时候就添加怒气
        public bool shouldAddAngryByAoYi() {
            foreach (BT_Skill sk in _nskillArr) {
                if(sk != null && sk.castAddAngryWhenAttend()){
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 转换类型
        /// </summary>
        /// <returns>The monster.</returns>
        public Monster toMonster() {
            Monster mon = new Monster() {
                id       = _id,
                num      = _num,
                property = (int)_property,
                level    = _level,
                nSkill   = _nSkill == null ? null : _nSkill.ToArray(),
                aSkill   = _aSkill == null ? null : _aSkill.ToArray(),
                curAtt   = _curAtt,
                pveIndex = _pveIndex,
                allfated = _fullFated ? (short)1 : (short)0,
				isBoss   = _isBoss,
            };
            return mon;
        }

    }

}
