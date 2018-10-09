using System;
using System.Reflection;
using System.Collections.Generic;

namespace AW.Battle {

    /// <summary>
    /// Skill op 的定义在ConfigData里面已经有了
    /// </summary>
	public partial class BT_Skill {
        public int id;
        public string name;
        public string mark;
        public string level;   // 强度，这个只是UI上的等级（比如S，A，B，C）
		public int curLv;      // 真正的等级  
        public skParam param;  // 参数.
        public int op;         // 操作类型.
        public SkillOpData opObj;
        // 如果是1，则表示为AoYi假装为技能，如果是0，则就是技能
        public int real; 
        // 如果real == 1，则这里才有值。否则为空
        public CAoYi AoYi;

        //谁拥有这个技能
        public BT_Monster owner;

        //当前回合释放的次数
        public int RdCast;

        /// <summary>
        /// Initializes a new instance of the <see cref="AW.Battle.BT_Skill"/> class.
        /// </summary>
        /// <param name="skCfg">Skill cfg.</param>
        /// <param name="id">Num</param>
        /// <param name="skillMgr">Skill mgr.</param>
        /// <param name="ownerBy">Owner by.</param>
		public BT_Skill(SkillData skCfg, int id, int lv, SkillManager skillMgr, BT_Monster ownerBy) {
            if(skCfg == null) throw new DragonException("Skill configure is null.");
            if(skillMgr == null) throw new DragonException("Skill manager is null.");
            if(ownerBy == null) throw new DragonException("Owner is null");

            //默认一定能释放一次
            RdCast  = 1;
            this.id = id;

            name  = skCfg.name;
            mark  = skCfg.mark;
            level = skCfg.level;
			param = skillMgr.GetSkParamData(id, lv);
            op    = skCfg.op;
            opObj = skillMgr.getSkillOpDataConfig(op);

            real  = 0;
            AoYi  = null;

            owner = ownerBy;
        }

		public BT_Skill(int skillNum, int lv, SkillManager skillMgr, BT_Monster ownerBy) {
            if(skillMgr == null) throw new DragonException("Skill manager is null.");
            if(ownerBy == null) throw new DragonException("Owner is null");

            SkillData skCfg = skillMgr.getSkillDataConfig(skillNum);
            if(skCfg == null) throw new DragonException("Skill configure is null." + skillNum);

            //默认一定能释放一次
            RdCast= 1;
            id    = skillNum;
			curLv = lv;
            name  = skCfg.name;
            mark  = skCfg.mark;
            level = skCfg.level;
			param = skillMgr.GetSkParamData(skillNum, lv);
            op    = skCfg.op;
            opObj = skillMgr.getSkillOpDataConfig(op);

            real  = 0;
            AoYi  = null;

            owner = ownerBy;
        }

        // 使用CAoYi来创建技能
        public BT_Skill (CAoYi AoYiItem, BT_Monster ownerBy) {
            owner = ownerBy;

            int AoYiLevel = AoYiItem.getLv;
            BT_AoYi AoYi = AoYiItem.getAoYiItem;
            // 参数信息
            float[] effect = AoYiItem.getEffectByLv (AoYiLevel);

            // 构建真实的技能参数数据
            skParam realEffect = new skParam();

            int length = AoYi._efinfo.Length;
            for(int i = 0; i < length; ++ i) {
                //其实可以反射的写入值，但这里不太多，就不用反射了。
                string key = AoYi._efinfo[i];
                if(!string.IsNullOrEmpty(key)) {
                    switch(key) {
                    case "rate2":
                        realEffect.rate2  = MathHelper.MidpointRounding(effect [i]);
                        break;
                    case "rate":
                        realEffect.rate   = MathHelper.MidpointRounding(effect [i]);
                        break;
                    case "gailv":
                        realEffect.gailv  = MathHelper.MidpointRounding(effect [i]);
                        break;
                    case "damage":
                        realEffect.damage = MathHelper.MidpointRounding(effect [i]);
                        break;
                    case "nuqi":
                        realEffect.nuqi   = MathHelper.MidpointRounding(effect [i]);
                        break;
                    case "num":
                        realEffect.num    = MathHelper.MidpointRounding(effect [i]);
                        break;
                    }
                }
            }

            // 构造对象
            //默认一定能释放一次
            RdCast = 1;
            id     = AoYi._ID;
			curLv  = AoYiLevel;
            opObj  = AoYi._SkillOpConfig;
            name   = AoYi._Name;
            level  = "A";
            param  = realEffect;
            op     = AoYi._skillOp;
        }

        // 是否为指定类型的技能.
        public bool isSkillByType(int skType) { 
            return opObj.type == skType;
        }

        // 技能可释放?
        public bool canCast() { 
            // 目前这个指检测op = 4的技能，op == 4 的技能概率是100%触发
            // 剩下的都走了canCastSkillOpDefault技能
            if(op == 4) {
                return canCastSkillOp4();
            } else {
                return canCastSkillOpDefault ();
            }
        }

        //该类型的技能100%触发
        private bool canCastSkillOp4() {
            // 检测释放为奥义伪造的技能，如果是则判定 奥义是否触发过
            if (real == 1 && AoYi != null && AoYi.getUsed == 1) {
                return false;
            } else {
                return true;
            }
        }

        // 技能能否释放默认实现... 每次战斗有X%概率...
        private bool canCastSkillOpDefault() {
            //检测释放为奥义伪造的技能，如果是则判定 奥义是否触发过
			BT_Logical war = owner._war;
            BT_MonsterTeam selfTeam = owner.ownerTeam;

            if (real == 1) {
                //如果不能释放，则直接跳过。如果可以释放，则再判定概率的问题
                bool can = war.canCastAoYi(this, selfTeam);
                if(can == false) {
                    return can;
                }
            }

            //再次判定概率的问题
            bool probility = false;
            int possibility = 0;

            if (opObj.type == SkillOpData.Anger_Skill) {
                int curTeamAngry = selfTeam.curAngry;
                int needNuqi = param.nuqi;
                if (needNuqi == 0)
                    throw new DragonException ( "调用canCastSkillOpDefault时,不存在怒气配置. id:" + id);
                probility = curTeamAngry >= needNuqi;
            } else if (opObj.type == SkillOpData.Death_Closed_Skill) {
                possibility = param.gailv;
                // 如果有概率的话，则考虑概率触发，否则一定触发
                if (possibility > 0) {
                    probility = BT_WarUtils.happen (possibility);
                } else {
                    // 默认触发
                    probility = owner._almostDie;
                }
            } else {
                possibility = param.gailv;
                if (possibility == 0)
                    throw new DragonException ("调用canCastSkillOpDefault时,不存在概率配置. id:" + id);
                probility = BT_WarUtils.happen (possibility);
            }

            //如果概率Ok，则奥义默认释放
            if(probility) {
                if(real == 1) {
                    //设定奥义
                    war.setCastAoYi(this, selfTeam);
                }
            }

            return probility;

        }

        // 技能释放
        public void castSkill() { 
            string funName = "castSkillOp" + opObj.ID;
            // 战斗过程中，释放技能方可能会死亡。所以要检测一下。
            if (owner.alive) {
                Type t = typeof(BT_Skill);

                t.InvokeMember( funName, BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance, 
                    null, this, null);

                //统计技能释放的次数
				owner._war.setTotalNormalSkill(owner.ownerTeam, this);
            }
        }


        #region 开始具体的技能逻辑
        // TODO: 直接攻击型技能
        // 对对方造成damage的伤害...
        private CMsgSkillAttack makeAttackMsg(BT_Monster enemy, float damage) {
            BT_Hurt hurt = new BT_Hurt() {
                damageVal = damage,
                hurtType  = BT_Hurt_Type.HURT_SKILL,
            };

            CMsgSkillAttack msg = new CMsgSkillAttack (this, enemy.pveId, MathHelper.MidpointRounding(damage));
            int finalDamage = enemy.sufferAttack (hurt, owner );
            msg.finalAtt = finalDamage;
            msg.curAtt = enemy.curAtt;
            msg.curAngry = enemy.ownerTeam.curAngry;
            return msg;
        }


        // damage 伤害
        // angryCost 怒气的消耗
        // angryRecover 怒气的恢复
        private void castAttackSkill(int damage, int angryCost = 0, int angryRecover = 0) {
            BT_Logical war   = owner._war;
            BT_Monster enemy = war.enemy (owner);
            if (enemy.alive) {

                // 扣怒气...
                owner.ownerTeam.costAngry (angryCost);
                int angry = owner.ownerTeam.curAngry;
                owner.ownerTeam.addAngry ( angryRecover );

                CMsgSkillAttack msg = makeAttackMsg ( enemy, damage );
                msg.recoverAngry    = angryRecover;
                msg.costAngry       = angryCost;
                msg.preAngry        = angry;
                war.addMsgToRecorder (msg);
            }
        }

        private void castSkillOp1() {
            int damage = param.damage;
            int Volumn = param.rate;
            damage     = unstableDamage (damage, Volumn);
            castAttackSkill (damage);
        }
        private void castSkillOp2() {
            int damage = MathHelper.MidpointRounding(param.rate * Consts.oneHundred * owner.curAtt);
            castAttackSkill (damage);
        }
        private void castSkillOp101() {
            int damage = param.damage;
            int Volumn = param.rate;

            damage     = unstableDamage (damage, Volumn) * RdCast;
            castAttackSkill (damage, param.nuqi * RdCast);
        }
        private void castSkillOp102() {
            int damage = MathHelper.MidpointRounding(param.rate * Consts.oneHundred * owner.curAtt * RdCast);
            castAttackSkill ( damage, param.nuqi * RdCast);
        }
        private void castSkillOp116() {
            int damage = MathHelper.MidpointRounding(param.rate * Consts.oneHundred * owner.curAtt * RdCast);
            castAttackSkill ( damage, param.nuqi * RdCast, param.num * RdCast);
        }
        private void castSkillOp117() {
            BT_Logical war   = owner._war;
            BT_Monster enemy = war.enemy (owner);

            int damage = MathHelper.MidpointRounding(param.rate * Consts.oneHundred * enemy.curAtt * RdCast);
            castAttackSkill ( damage, param.nuqi * RdCast, param.num * RdCast);
        }
        //带基础伤害的怒气技能
        private void castSkillOp119() {
			int damage = param.num * RdCast + MathHelper.MidpointRounding(param.rate * Consts.oneHundred * owner.curAtt * RdCast);
            castAttackSkill ( damage, param.nuqi * RdCast);
        }

        // 浮动伤害
        private int unstableDamage(int damage, int rate) {
            // 0 减值 ， 1 加值
            int result = PseudoRandom.getInstance().next(2);
            int vol    = MathHelper.MidpointRounding(damage * PseudoRandom.getInstance().next(rate) * Consts.oneHundred);
            if (result == 1) {
                damage += vol;
            } else {
                damage -= vol;
            }
            return damage;
        }

        // --------- 反弹伤害 ---------
        private float returnDamage(BT_Monster Pet, BT_Monster enemy, float damage) {
            short bufDebuffType = CMsgHeader.BUFF_DEBUFF_REBOUND;
            BT_BuffOrDebuff reboundBuff = Pet.getBuffOrDebuff (bufDebuffType);
            if (reboundBuff != null && -- reboundBuff.round >= 0 && enemy.alive) {
                //
                float rebound = damage * reboundBuff.rate * Consts.oneHundred;
                damage -= rebound;
                if(damage < 0) damage = 0;
                // 反弹部分伤害到敌人...
                enemy.sufferNormalAttack ( MathHelper.MidpointRounding(rebound), Pet, true);
            }

            return damage;
        }

        // TODO: 变更当前攻击力技能
        // 增加 自己/对手 当前战斗力X% type:0---百分比 1---固定值
        private void castChangeAttackSkill(bool beOwner, float addRate, int type = 0, int angryCost = 0) {
            BT_Logical war = owner._war;
            BT_Monster enemy = war.enemy (owner);

            BT_Monster target = beOwner ? owner : enemy;
            if (target.alive) {

                if (angryCost > 0) { // 扣怒气...
                    BT_MonsterTeam selfTeam = owner.ownerTeam;
                    selfTeam.costAngry (angryCost);
                }

                CMsgSkillChangeAttack msg = new CMsgSkillChangeAttack (this, target.pveId);
                int beforeAtt = target.curAtt;
                if (0 == type) {
                    if (beOwner) {
                        target.curAttEnhance (1 + addRate * 0.01f);
                    } else {
                        int hp = target.curAtt;
                        float damage = hp * addRate * 0.01f;
                        damage = returnDamage ( owner, enemy, damage );

                        if( false == target.canSufferSkillDamage() ) {
                            damage = 0;
                        }

                        target.curAttAdd (- damage);
                        // target.curAttEnhance ( 1 - addRate / 100 );
                    }
                } else {
                    target.curAttAdd ( addRate );
                }

                int afterAtt = target.curAtt;
                msg.curAtt = afterAtt;
                msg.addAtt = afterAtt - beforeAtt;
                war.addMsgToRecorder ( msg );
            }
        }

        private void castSkillOp3() {
            castChangeAttackSkill (false, param.rate);
        }

        private void castSkillOp6() {
            castChangeAttackSkill (true, param.rate);
        }

        private void castSkillOp103() {
            castChangeAttackSkill (false, param.rate * RdCast, 0, param.nuqi * RdCast);
        }

        private void castSkillOp108() {
            castChangeAttackSkill (true, param.rate * RdCast, 0, param.nuqi * RdCast);
        }

        private void castSkillOp5() {
            int att = param.add;
            int volumn = param.rate;
            att = unstableDamage (att, volumn );
            castChangeAttackSkill (false, att, 1 );
        }

        private void castSkillOp107() {
            castChangeAttackSkill (true, param.add * RdCast, 1, param.nuqi * RdCast);
        }

        // TODO: 连击技能...
        // 对对方造成damage的伤害 并且可能有额外几次的伤害...
        private void castAttackComboSkill(float damage, int extra, int angryCost = 0) {
            BT_Logical war = owner._war;
            BT_Monster enemy = war.enemy (owner);

            if (enemy.alive) {
                CMsgSkillAttackCombo msg = new CMsgSkillAttackCombo ( this );

                BT_MonsterTeam selfTeam = owner.ownerTeam;
                // 扣怒气...
                if (angryCost > 0) { 
                    selfTeam.costAngry ( angryCost );
                }
                war.addMsgToRecorder (msg);

                // 返回当前的怒气值
                msg.curAngry  = selfTeam.curAngry;
                msg.attackArr = null;

				List<CMsgSkillAttack> realArr = new List<CMsgSkillAttack>();

                for(int i = 0; i < extra + 1; i ++) {
                    if (enemy.alive ) {
                        CMsgSkillAttack attackMsg = makeAttackMsg ( enemy, damage );
						realArr.Add(attackMsg);
                    }
                }

				if(realArr.Count > 0) {
					msg.attackArr = realArr.ToArray();
				}

            }
        }

        private void castSkillOp4() {
			float damage = param.rate * Consts.oneHundred * owner.curAtt;
            // 最少造成1点伤害
            if (damage <= 0)
                damage = 1;
            int extra = 0;
			if ( BT_WarUtils.happen (param.gailv) )
                extra ++;
            castAttackComboSkill ( damage, extra );
        }

        private void castSkillOp109() {
            float damage = param.rate * Consts.oneHundred * owner.curAtt * RdCast;
            castAttackComboSkill ( damage, param.num - 1, param.nuqi * RdCast);
        }

        // TODO: 吸血技能.
        // 吸取敌人攻击力,转换其中一部分变成自己的攻击力
        // /num 吸取敌人血的百分比
        // /type 0:固定值 1:比例
        // /convert 吸取血之后加到自己身上的百分比
        // /angryCost 怒气
        // /selfInc 0：convert吸取血之后加到自己身上的百分比 1：自身攻击力增加convert
        private void castSuckSkill(float num, int type, float convert, int angryCost = 0, int selfInc = 0) {
            BT_Logical war = owner._war;
            BT_Monster enemy = war.enemy ( owner );
            BT_MonsterTeam selfTeam = owner.ownerTeam;

            if (selfTeam.curAngry >= angryCost) {

                if (enemy.alive ) {
                    float suck = num;
                    if (type == 1) {
                        suck = num * Consts.oneHundred * enemy.curAtt;
                    }

                    if (suck > enemy.curAtt) {
                        suck = enemy.curAtt;
                    }

                    if (angryCost > 0) { // 扣怒气...
                        selfTeam.costAngry ( angryCost );
                    }

                    CMsgSkillSuckAttack msg = new CMsgSkillSuckAttack ( this, enemy.pveId, MathHelper.MidpointRounding(suck) );
                    war.addMsgToRecorder ( msg );

                    // 减敌方血,加给自己.
                    BT_Hurt hurt = new BT_Hurt() {
                        damageVal = suck,
                        hurtType  = BT_Hurt_Type.HURT_SKILL,
                    };

                    int finalSuck = enemy.sufferAttack ( hurt, owner );
                    float addAtt = 0f;
                    if (selfInc == 0) {
                        addAtt = convert * Consts.oneHundred * finalSuck;
                    } else {
                        addAtt = convert * Consts.oneHundred * owner.curAtt;
                    }

                    owner.curAttAdd ( addAtt );
                    msg.convertAttack = MathHelper.MidpointRounding(addAtt);
                    msg.finalSuckAtt = finalSuck;
                    msg.sufferCurAtt = enemy.curAtt;
                    msg.casterCurAtt = owner.curAtt;
                    msg.curAngry = selfTeam.curAngry;
                }
            }
        }

        private void castSkillOp7() {
            int damage = param.damage;
            int rate = param.rate2;
            damage = unstableDamage ( damage, rate );
            castSuckSkill ( damage, 0, param.rate );
        }

        private void castSkillOp8() {
            castSuckSkill ( param.rate, 1, param.rate2 );
        }

        private void castSkillOp110() {
            castSuckSkill ( param.damage * RdCast, 0, param.rate, param.nuqi * RdCast);
        }

        private void castSkillOp111() {
            castSuckSkill ( param.rate * RdCast, 1, param.rate2, param.nuqi * RdCast);
        }

        private void castSkillOp118() {
			castSuckSkill ( param.rate * RdCast, 1, param.rate2 * RdCast, param.nuqi * RdCast, 1);
        }


        // TODO: 自损技能
        // type 0:固定值 1:比例
        private void castChangeAttBothSKill(int snum, int stype, int senum, int etype, int angryCost = 0) {
            BT_Logical war = owner._war;
            BT_Monster enemy = war.enemy ( owner );
            if (enemy.alive ) {
                if (angryCost > 0) { // 扣怒气...
                    BT_MonsterTeam selfTeam = owner.ownerTeam;
                    selfTeam.costAngry ( angryCost );
                }

                float sAdd = snum;
                if (stype == 1)
                    sAdd = snum * Consts.oneHundred * owner.curAtt;

                float eAdd = senum;
                if (etype == 1)
                    eAdd = senum * Consts.oneHundred * enemy.curAtt;

                sAdd = - sAdd;
                eAdd = - eAdd;

                CMsgSkillChangeCurAttackBoth msg = new CMsgSkillChangeCurAttackBoth (this);
                war.addMsgToRecorder ( msg );

                owner.curAttAdd ( sAdd );

                if (enemy.canSufferSkillDamage ())
                    enemy.curAttAdd ( eAdd );

                msg.selfAttChange = MathHelper.MidpointRounding(sAdd);
                msg.enemyAttChange = MathHelper.MidpointRounding(eAdd);
                msg.selfCurAtt = owner.curAtt;
                msg.enemyCurAtt = enemy.curAtt;
            }
        }

        private void castSkillOp9() {
            castChangeAttBothSKill ( param.dec, 0, param.damage, 0 );
        }

        private void castSkillOp10() {
            castChangeAttBothSKill ( param.rate, 1, param.rate2, 1 );
        }


        // TODO: 复活技能.
        // type: 0--随机复活 1--从前向后 2--从后到前
        private void castReviveSkill(int num, int type, int angryCost = 0) { 
            BT_MonsterTeam selfTeam = owner.ownerTeam;
            // 扣怒气...
            if (angryCost > 0) {
                selfTeam.costAngry ( angryCost );
            }

            List<BT_Monster> dieArr = new List<BT_Monster>();
            List<BT_Monster> sortDieArry = new List<BT_Monster>();

            foreach (BT_Monster pet in selfTeam._team) {
                if (pet == owner) {
                    break;
                }
                if (!pet.alive) {
                    dieArr.Add(pet);
                }
            }

            int tataoDieCount = dieArr.Count;
            if (tataoDieCount >= 1) {

                if (0 == type) {
                    BT_WarUtils.Shuffle( dieArr );
                    type = 1;
                }

                int minCount = tataoDieCount > num ? num : tataoDieCount;

                for(int j = 0; j < minCount; j ++) {
                    sortDieArry.Add(dieArr [j]);
                }

                sortDieArry.Sort(my_sort);

                BT_Logical war = owner._war;
                CMsgSkillRevive msg = new CMsgSkillRevive (this);
                war.addMsgToRecorder ( msg );
                msg.reviveArr = new int[minCount];

                for(int i = 0; i < minCount; i ++) {
                    BT_Monster pet = type == 1 ? array_shift ( sortDieArry ) : array_pop ( sortDieArry );
                    if (pet != null) {
                        pet.revive ();
                        selfTeam._team.Add(pet);
                        msg.reviveArr [i] = pet.pveId;
                    }
                }
                selfTeam.teamLenChanged ();
            } else {
                //如果没有人死亡，则奥义的技能就算没触发过
                if(real == 1) {
                    BT_Logical war = owner._war;
                    war.resetAoYi(this,selfTeam);
                } 
            }
        }

        static int my_sort(BT_Monster a, BT_Monster b) {
            return (a.pveId >= b.pveId ) ? 1 : - 1;
        }

        BT_Monster array_shift(List<BT_Monster> monList) {
            BT_Monster first = null;
            if(monList != null && monList.Count > 0) {
                first = monList[0];
                monList.RemoveAt(0);
            } 
            return first;
        }

        BT_Monster array_pop(List<BT_Monster> monList) {
            BT_Monster last = null;
            if(monList != null && monList.Count > 0) {
                int count = monList.Count;
                last = monList[count - 1];
                monList.RemoveAt(count - 1);
            } 
            return last;
        }


        private void castSkillOp11() {
            castReviveSkill ( param.num, 0 );
        }

        private void castSkillOp12() {
            castReviveSkill ( 1, 0 );
        }

        // TODO: 自爆技能
        // type 0:固定值 1:自身初始战斗比例 2:对方当前战斗比例
        private void castBlastSkill(float damage, int type, int sufferCnt = 1, int angryCost = 0) {
            BT_Logical war = owner._war;
            BT_MonsterTeam enemyTeam = owner.vsTeam;

            CMsgSkillBlast msg = new CMsgSkillBlast (this);
            war.addMsgToRecorder (msg);

            if (1 == type)
                damage = owner.initAtt * damage * Consts.oneHundred;

            int teamLen = enemyTeam._team.Count;
            int sufferNum = 0;

            List<CMsgSkillAttack> temp = new List<CMsgSkillAttack>();
            for(int i = enemyTeam.curPetTeamIndex; i < teamLen && sufferNum < sufferCnt; i ++, sufferNum ++) {
                BT_Monster enemy = enemyTeam._team [i];
                if (2 == type) {
                    if(war.fightDiff != 0)
                        damage = war.fightDiff * damage * Consts.oneHundred;
                    else 
                        damage = enemy.curAtt * damage * Consts.oneHundred;
                }

                CMsgSkillAttack attackMsg = makeAttackMsg ( enemy, damage );
                temp.Add(attackMsg);
            }
            msg.sufferArr = temp.ToArray();
        }

        private void castSkillOp13() {
            castBlastSkill ( param.damage, 0 );
        }
        private void castSkillOp14() {
            castBlastSkill ( param.rate, 1 );
        }
        private void castSkillOp15() {
            castBlastSkill ( param.rate, 2 );
        }
        private void castSkillOp17() {
            castBlastSkill ( param.num, 1, param.rate );
        }

        // TODO: 怒气回复
        private void castAngryRecoverSkill(int addAngry, int angryCost = 0) {
            BT_Logical war = owner._war;
            BT_MonsterTeam selfTeam = owner.ownerTeam;
            if (angryCost > 0) { // 扣怒气...
                selfTeam.costAngry ( angryCost );
            }

            selfTeam.costAngry ( - addAngry );
            CMsgSkillAngryExchange msg = new CMsgSkillAngryExchange ( this );
            war.addMsgToRecorder ( msg );

            msg.costAngry = angryCost;
            msg.addAngry = addAngry;
        }

        private void castSkillOp105() {
            castAngryRecoverSkill ( param.num * RdCast, param.nuqi * RdCast);
        }

        // TODO: 封印实现 ---效果呆实现... [here]
        private void castSealSkill(int sealCnt, int angryCost = 0) {
            BT_Logical war = owner._war;
            BT_MonsterTeam selfTeam = owner.ownerTeam;
            BT_MonsterTeam enemyTeam = owner.vsTeam;
            if (angryCost > 0) { // 扣怒气...
                selfTeam.costAngry ( angryCost );
            }

            short bufType = CMsgHeader.BUFF_DEBUFF_SEAL;
            int bufRound = 1;

            CMsgSkillBuffDebuff msg = new CMsgSkillBuffDebuff ( this );
            msg.type = bufType;
            msg.round = bufRound;
            war.addMsgToRecorder ( msg );

            int teamLen = enemyTeam._team.Count;
            int sufferNum = 0;

            List<int> sufferList = new List<int>();
            for(int i = enemyTeam.curPetTeamIndex; i < teamLen && sufferNum < sealCnt; i ++, sufferNum ++) {
                BT_Monster enemy = enemyTeam._team [i];
                BT_BuffOrDebuff buf = new BT_BuffOrDebuff(){
                    type  = bufType,
                    round = bufRound,
                };
                enemy.addBuffOrDebuff ( buf, false );
                sufferList.Add(enemy.pveId);
            }
            msg.sufferArr = sufferList.ToArray();
        }

        private void castSkillOp16() {
            castSealSkill ( param.seal );
        }
        private void castSkillOp106() {
            castSealSkill ( param.num * RdCast, param.nuqi * RdCast);
        }

        // TODO: 金钟罩
        private void castGoldShieldSkill(int round = 1, int angryCost = 0) {
            BT_Logical war = owner._war;
            BT_MonsterTeam selfTeam = owner.ownerTeam;
            if (angryCost > 0) { // 扣怒气...
                selfTeam.costAngry ( angryCost );
            }

            short bufType = CMsgHeader.BUFF_DEBUFF_GOLDDEFENSE;
            int bufRound = round;

            CMsgSkillBuffDebuff msg = new CMsgSkillBuffDebuff ( this );
            msg.type = bufType;
            msg.round = bufRound;
            war.addMsgToRecorder ( msg );

            BT_BuffOrDebuff buf = new BT_BuffOrDebuff(){
                type  = bufType,
                round = bufRound,
            };

            owner.addBuffOrDebuff(buf);
            msg.sufferArr = new int[] {owner.pveId};
        }

        private void castSkillOp19() {
            castGoldShieldSkill ();
        }

        // TODO: 护盾 0---固定值 1---百分比
        private void castShieldSkill(float addRate, int type = 0, int angryCost = 0) {
            BT_Logical war = owner._war;
            BT_MonsterTeam selfTeam = owner.ownerTeam;
            if (angryCost > 0) { // 扣怒气...
                selfTeam.costAngry ( angryCost );
            }

            float suckDmg = addRate;
            if (1 == type)
                suckDmg = owner.curAtt * addRate * Consts.oneHundred;

            short bufType = CMsgHeader.BUFF_DEBUFF_DEFENSE;

            CMsgSkillBuffDebuff msg = new CMsgSkillBuffDebuff (this);
            msg.type = bufType;
            msg.arg1 = MathHelper.MidpointRounding(suckDmg);
            war.addMsgToRecorder (msg);

            BT_BuffOrDebuff buf = new BT_BuffOrDebuff(){
                type  = bufType,
                suckDmg = MathHelper.MidpointRounding(suckDmg),
            };

            owner.addBuffOrDebuff (buf);
            msg.sufferArr = new int[] {owner.pveId};
        }

        private void castSkillOp23() {
            castShieldSkill ( param.absorb );
        }

        private void castSkillOp24() {
            castShieldSkill ( param.absorb, 1 );
        }

        private void castSkillOp113() {
            castShieldSkill (param.rate * RdCast, 1, param.nuqi * RdCast);
        }

        // TODO: 免疫技能
        private void castImmunitySkill(int round, int angryCost = 0) {
            BT_Logical war = owner._war;
            BT_MonsterTeam selfTeam = owner.ownerTeam;
            if (angryCost > 0) { // 扣怒气...
                selfTeam.costAngry ( angryCost );
            }

            short bufType = CMsgHeader.BUFF_DEBUFF_IMMUNITY;
            int bufRound = round;

			CMsgSkillBuffDebuff msg = new CMsgSkillBuffDebuff ( this );
            msg.type = bufType;
            msg.round = bufRound;
            war.addMsgToRecorder ( msg );

            BT_BuffOrDebuff buf = new BT_BuffOrDebuff(){
                type  = bufType,
                round = bufRound,
            };

            owner.addBuffOrDebuff (buf);

            msg.sufferArr = new int[] { owner.pveId };
        }
        private void castSkillOp25() {
            castImmunitySkill (param.rate);
        }

        // 后减
        public void castDecSkill(float rate, int angryCost = 0) {
            BT_Logical war = owner._war;
            BT_MonsterTeam selfTeam = owner.ownerTeam;
            if (angryCost > 0) { // 扣怒气...
                selfTeam.costAngry ( angryCost );
            }

            // 减血
            CMsgSkillRecovery msg = new CMsgSkillRecovery ( this );
            msg.startAtt = owner.curAtt;

            war.addMsgToRecorder ( msg );
			float finalAtt = owner.curAtt * (1 - rate * Consts.oneHundred);
            if (finalAtt < 1)
                finalAtt = 1;
            // 减血
            owner.setCurAtt ( finalAtt );

            msg.finalAtt = MathHelper.MidpointRounding(finalAtt);
            msg.suffer = owner.pveId;
        }

        // 先增后减
        private void castIncDecSkill(float rate, int num, int angryCost = 0) {
            BT_Logical war = owner._war;
            BT_MonsterTeam selfTeam = owner.ownerTeam;
            if (angryCost > 0) { // 扣怒气...
                selfTeam.costAngry ( angryCost );
            }

            short bufType = CMsgHeader.BUFF_DEBUFF_REDUCE_HP;
            int bufRound = 2;

            // 加血
            CMsgSkillRecovery msg = new CMsgSkillRecovery ( this );
            msg.startAtt = owner.curAtt;

            war.addMsgToRecorder ( msg );

            BT_BuffOrDebuff buf = new BT_BuffOrDebuff(){
                type  = bufType,
                round = bufRound,
                skill = this,
            };

            owner.addBuffOrDebuff ( buf );

            float finalAtt = owner.curAtt * (1 + rate * Consts.oneHundred);
            owner.setCurAtt ( finalAtt );

            msg.curAngry = selfTeam.curAngry;
            msg.finalAtt = MathHelper.MidpointRounding(finalAtt);
            msg.suffer = owner.pveId;
        }


        private void castSkillOp26() {
            castIncDecSkill ( param.rate, param.num );
        }

        // TODO: 伤害加倍
        private void castHurtUpSkill(float rate, int round, int angryCost = 0) {
            BT_Logical war = owner._war;
            BT_MonsterTeam selfTeam = owner.ownerTeam;
            if (angryCost > 0) { // 扣怒气...
                selfTeam.costAngry ( angryCost );
            }

            short bufType = CMsgHeader.BUFF_DEBUFF_HURTUP;
            int bufRound = round;

            CMsgSkillBuffDebuff msg = new CMsgSkillBuffDebuff ( this );
            msg.type = bufType;
            msg.round = bufRound;
            war.addMsgToRecorder ( msg );

            BT_Monster enemy = war.enemy ( owner );

            BT_BuffOrDebuff debuf = new BT_BuffOrDebuff(){
                type  = bufType,
                round = bufRound,
                rate  = rate,
            };

            enemy.addBuffOrDebuff (debuf, false);
            msg.sufferArr = new int[] {enemy.pveId};
        }
        private void castSkillOp21() {
            castHurtUpSkill ( param.num, param.rate );
        }
   
        // TODO: 以下ok
        // TODO:群体增益
        // 增加自己后rate名队友num%的初始战斗力。 0--固定值 1--百分比.
        private void castChangeAttackAllSkill(float addRate, int sufferCnt, bool beOwnerTeam = true, int type = 1, int angryCost = 0) {
            BT_Logical war = owner._war;
            CMsgSkillChangeCurAttackAll msg = new CMsgSkillChangeCurAttackAll ( this );
            war.addMsgToRecorder ( msg );

            BT_MonsterTeam selfTeam = owner.ownerTeam;
            BT_MonsterTeam enemyTeam = owner.vsTeam;
            if (angryCost > 0) { // 扣怒气...
                selfTeam.costAngry ( angryCost );
            }
            BT_MonsterTeam armTeam = beOwnerTeam ? selfTeam : enemyTeam;

            int teamLen = armTeam._team.Count;
            int sufferNum = 0;
            List<CMsgSkillChangeAttack> tmp = new List<CMsgSkillChangeAttack>();
            for(int i = armTeam.curPetTeamIndex + 1; i < teamLen && sufferNum < sufferCnt; i ++, sufferNum ++) {
                BT_Monster enemy = armTeam._team [i];

                float addAtt = addRate;
                if (1 == type)
                    addAtt = enemy.initAtt * addRate * Consts.oneHundred;

                CMsgSkillChangeAttack msgtmp = new CMsgSkillChangeAttack (this, enemy.pveId);
                int beforeAtt = enemy.curAtt;
                enemy.curAttAdd ( addAtt );
                int afterAtt = enemy.curAtt;
                msgtmp.curAtt = afterAtt;
                msgtmp.addAtt = afterAtt - beforeAtt;

                tmp.Add(msgtmp);
            }

            msg.sufferArr = tmp.ToArray();
        }
        private void castSkillOp18() {
            castChangeAttackAllSkill ( param.num, param.rate );
        }

        // TODO: 假死
        private void castDeathSkill(float rate, int angryCost = 0) {
            BT_Logical war = owner._war;
            BT_MonsterTeam selfTeam = owner.ownerTeam;
            if (angryCost > 0) { // 扣怒气...
                selfTeam.costAngry ( angryCost );
            }

            CMsgSkillDeath msg = new CMsgSkillDeath ( this );
            msg.suffer = owner.pveId;
            war.addMsgToRecorder ( msg );

            float leftAtt = owner.initAtt * rate * Consts.oneHundred;
            owner.setCurAtt ( leftAtt );
            owner.setAlive();
            msg.curAtt = owner.curAtt;
        }

        private void castSkillOp20() {
            castDeathSkill ( param.rate );
        }

        // TODO: 战后回复
        private void castRecoverySkill(float rate, int angryCost = 0) {
            BT_Logical war = owner._war;
            BT_MonsterTeam selfTeam = owner.ownerTeam;
            BT_Monster enemy = war.enemy ( owner );
            if (angryCost > 0) { // 扣怒气...
                selfTeam.costAngry ( angryCost );
            }

            CMsgSkillAfterRecovery msg = new CMsgSkillAfterRecovery ( this );
            msg.suffer = owner.pveId;
            msg.startAtt = owner.curAtt;
            msg.enemyAtt = enemy.curAtt;

            war.addMsgToRecorder ( msg );

            float addAtt = owner.initAtt * rate * Consts.oneHundred;

            owner.curAttAdd ( addAtt );
            msg.finalAtt = owner.curAtt;
        }
        private void castSkillOp22() {
            castRecoverySkill ( param.rate );
        }

        // TODO: 斩杀
        private void castCutSkill(float damage, float rate, int angryCost = 0) {
            BT_Logical war = owner._war;
            BT_MonsterTeam selfTeam = owner.ownerTeam;
            if (angryCost > 0) { // 扣怒气...
                selfTeam.costAngry ( angryCost );
            }
            BT_Monster enemy = war.enemy ( owner );

            CMsgSkillCut msg = new CMsgSkillCut ( this, enemy.pveId );
            msg.startAtt = MathHelper.MidpointRounding(damage);
            war.addMsgToRecorder ( msg );

            int beforeAtt = enemy.curAtt;
            float cutAtt = enemy.initAtt * rate * Consts.oneHundred;
            enemy.sufferAttack ( new BT_Hurt(){
                damageVal = damage,
                hurtType  = BT_Hurt_Type.HURT_SKILL,
            } , owner );

            if (enemy.alive && enemy.curAtt < cutAtt) {
                enemy.setCurAtt ( 0 );
            }
            msg.finalAtt = enemy.curAtt - beforeAtt;
            msg.curAtt = enemy.curAtt;
        }
        private void castSkillOp104() {
            castCutSkill ( param.damage, param.rate, param.nuqi);
        }

        // TODO: 自爆
        private void castBombSkill(float rate, int angryCost = 0) {
            BT_Logical war = owner._war;
            BT_MonsterTeam selfTeam = owner.ownerTeam;
            if (angryCost > 0) { // 扣怒气...
                selfTeam.costAngry ( angryCost );
            }
            BT_Monster enemy = war.enemy ( owner );

            owner.setCurAtt ( 0, true );
            float damage = enemy.curAtt * rate * Consts.oneHundred;

            CMsgSkillBomb msg = new CMsgSkillBomb ( this, enemy.pveId, MathHelper.MidpointRounding(damage) );
            war.addMsgToRecorder ( msg );
            int finalDamage = enemy.sufferAttack ( new BT_Hurt(){
                hurtType = BT_Hurt_Type.HURT_SKILL,
                damageVal = damage, 
                }, owner );
            msg.finalAtt = finalDamage;
            msg.enemyCurAtt = enemy.curAtt;
            msg.selfCurAtt = owner.curAtt;
        }
        private void castSkillOp112() {
            castBombSkill (param.rate * RdCast, param.nuqi * RdCast);
        }

        // TODO: 夺取差值
        private void castSkillAttDelta(float rate, int angryCost = 0) {
            BT_Logical war = owner._war;
            BT_MonsterTeam selfTeam = owner.ownerTeam;
            if (angryCost > 0) { // 扣怒气...
                selfTeam.costAngry ( angryCost );
            }
            BT_Monster enemy = war.enemy ( owner );

            if (owner.curAtt >= enemy.curAtt )
                return;

            float attDistance = Math.Abs ( enemy.curAtt - owner.curAtt );
            attDistance = attDistance * rate * Consts.oneHundred;
            attDistance = enemy.sufferAttack ( new BT_Hurt(){ 
                hurtType = BT_Hurt_Type.HURT_SKILL,
                damageVal = attDistance,
            }, owner );

            CMsgSkillAttDelta msg = new CMsgSkillAttDelta ( this, MathHelper.MidpointRounding(attDistance) );
            war.addMsgToRecorder ( msg );
            msg.beforeAtt = owner.curAtt;
            owner.curAttAdd ( attDistance );
            msg.curAtt = owner.curAtt;
            msg.enemyAtt = enemy.curAtt;
        }
        private void castSkillOp28() {
            castSkillAttDelta (param.rate);
        }


        // TODO: 无视属性克制
        public bool castIgnorePropertyKill() {
            BT_Logical war = owner._war;
            bool ignore = false;

            if (opObj.ID == 27 && canCastSkillOpDefault ()) {
                ignore = true;

                BT_MonsterTeam selfTeam = owner.ownerTeam;
                if(selfTeam.getTeamName == "Att") {
                    war._AoYiCastCount_Att = 1;
                } else {
                    war._AoYiCastCount_Def = 1;
                }

                CMsgSkillCast msg = new CMsgSkillCast ( this, CMsgSkillCast.categoryIgnorePropertyKill );
                war.addMsgToRecorder ( msg );
            }
            return ignore;
        }

        // TODO: 变为全属性 (ok)
        public bool castPropertyChangeToAll(MonsterAttribute curPro) {
            BT_Logical war = owner._war;

            bool change = false;
            if (opObj.ID == 29 && canCastSkillOpDefault ()) {

                if(curPro != MonsterAttribute.ALL) {
                    change = true;

                    BT_MonsterTeam selfTeam = owner.ownerTeam;
                    if(selfTeam.getTeamName == "Att") {
                        war._AoYiCastCount_Att = 1;
                    } else {
                        war._AoYiCastCount_Def = 1;
                    }

                    CMsgSkillCast msg = new CMsgSkillCast ( this, CMsgSkillCast.categoryPropertyChangeToAll );
                    war.addMsgToRecorder ( msg );
                } else {
                    if(real == 1) {
                        war.resetAoYi(this, owner.ownerTeam);
                    }
                }
            }
            return change;
        }

        // TODO : 登场的时候增加怒气
        public bool castAddAngryWhenAttend() {
            bool Add = false;
            if (opObj.ID == 202 && canCastSkillOpDefault () ) {
                Add = true;
                castSkillOp202();
            }
            return Add;
        }


        // TODO: 反弹技能伤害
        private void castReboundSkill(int round, float rate, int angryCost = 0) {
            BT_Logical war = owner._war;
            BT_MonsterTeam selfTeam = owner.ownerTeam;
            if (angryCost > 0) { // 扣怒气...
                selfTeam.costAngry ( angryCost );
            }

            short bufType = CMsgHeader.BUFF_DEBUFF_REBOUND;
            int bufRound = round;

            CMsgSkillBuffDebuff msg = new CMsgSkillBuffDebuff ( this );
            msg.type = bufType;
            msg.round = bufRound;
            war.addMsgToRecorder ( msg );

            BT_BuffOrDebuff buf = new BT_BuffOrDebuff(){
                type  = bufType,
                round = bufRound,
                rate  = rate,
            };
            owner.addBuffOrDebuff (buf);
            msg.sufferArr = new int[] {owner.pveId};
        }

        private void castSkillOp115() {
            castReboundSkill ( param.rate * RdCast, 1.0f, param.nuqi * RdCast);
        }

        // TODO: 避免死亡，有几率回复生命
        private void castAngryAvoidDie(float rate, int angryCost = 0) {
            BT_Logical war = owner._war;
            BT_MonsterTeam selfTeam = owner.ownerTeam;

            if (selfTeam.curAngry >= angryCost) {
                if (angryCost > 0) { // 扣怒气...
                    selfTeam.costAngry ( angryCost );
                }

                CMsgSkillReviveSelf msg = new CMsgSkillReviveSelf ( this );
                msg.suffer = owner.pveId;
                msg.startAtt = owner.curAtt;
                war.addMsgToRecorder ( msg );

                float addAtt = owner.initAtt * rate * Consts.oneHundred;
                owner.curAttAdd ( addAtt );
                msg.finalAtt = owner.curAtt;
            }
        }
        private void castSkillOp114() {
            castAngryAvoidDie ( param.rate * RdCast, param.nuqi * RdCast);
        }

        // 加气的技能-目前是只有神龙奥义在使用
        private void AddAngryCount(int num) {
            BT_Logical war = owner._war;
            BT_MonsterTeam selfTeam = owner.ownerTeam;

            selfTeam.addAngry ( num );

            CMsgSkillCast msg = new CMsgSkillCast ( this, CMsgSkillCast.categoryAddAngryPoint );
            msg.curAngry = selfTeam.curAngry;
            war.addMsgToRecorder ( msg );
        }
        // 死亡加气
        private void castSkillOp201() {
            AddAngryCount ( param.num );
        }
        // 进场加气
        private void castSkillOp202() {
            AddAngryCount ( param.num );
        }
        // 杀人加气
        private void castSkillOp203() {
            AddAngryCount ( param.num );
        }

        #endregion

    }

}