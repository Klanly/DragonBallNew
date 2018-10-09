using System;
using fastJSON;
using System.Collections.Generic;

namespace AW.Battle {

	public class Monster {
        public int id;
		public int num;
		public int property;
		public int level;
		//普通技能
		public SkillObj[] nSkill;
		//怒气技能
		public SkillObj[] aSkill;

		public int curAtt;
		public int pveIndex;

        //0 = false 1 == true
        public short allfated;
		//0 = false 1 == true
		public short isBoss;

		//可以使用这个来判定属性
		public MonsterAttribute myAttribute {
			get {
				if(Enum.IsDefined(typeof(MonsterAttribute), property))
					return (MonsterAttribute)property;
				else 
					return MonsterAttribute.DEFAULT_NO;
			}
		}

        //缘是否配齐
        public bool AllFated {
            get {
                return allfated == 1;
            }
        }
	}

	public class Monsterteam {
		public Monster[] team;
		//怒气
		public short angryCnt;
		//用户的ID
        public int roleId;
		//0 攻击, 1防守
		public short type;
	}

		
   //------------------------------------- Msg Phase ----------------------------------
	public class CMsgHeader{
		//战斗开始
		public const short STATUS_WAR_BEGIN    = -1;
		//回合开始
		public const short STATUS_ROUND_BEGIN  = 0;
		//属性更改
		public const short STATUS_PROPERTY_CHG = 1;
		//属性相克
		public const short STATUS_PROPERTY_KILL= 2;
		//普通技释放*
		public const short STATUS_NOMARL_SKILL = 4;
        //本地战斗计算的逻辑，怒气技准备好了
        public const short STATUS_ANGRY_READY  = -2;

		//普通攻击
		public const short STATUS_ATTACK = 5;
		//战斗结束
		public const short STATUS_WAR_END = 7;
		//反弹攻击
		public const short STATUS_REBOUND_ATT = 6; 
		//buff debuff 效果.
		public const short STATUS_BUFF_DEBUFF = 8;
        //登场
        public const short statusAttend = 9;

		//buff debuff type define:
		public const short	BUFF_DEBUFF_SEAL        = 200;
		public const short	BUFF_DEBUFF_GOLDDEFENSE = 201;
		public const short	BUFF_DEBUFF_DEFENSE     = 202;
		public const short	BUFF_DEBUFF_HURTUP      = 203;
		public const short	BUFF_DEBUFF_IMMUNITY    = 204;
		public const short	BUFF_DEBUFF_REBOUND     = 205;
        public const short  BUFF_DEBUFF_REDUCE_HP   = 206;

		public const short STATUS_NSK_401 = 401;

        ///战后技
		public const short STATUS_NSK_402 = 402;

		/// <summary>
		/// 连击
		/// </summary>
		public const short STATUS_NSK_403 = 403;//
		/// <summary>
		/// 吸血
		/// </summary>
		public const short STATUS_NSK_404 = 404;//
		/// <summary>
		/// 自损攻击
		/// </summary>
		public const short STATUS_NSK_405 = 405;//
		/// <summary>
		/// 复活
		/// </summary>
		public const short STATUS_NSK_406 = 406; //
		/// <summary>
		///死亡后造成伤害，被动自爆
		/// </summary>
		public const short STATUS_NSK_407 = 407;//
		/// <summary>
		/// 增加怒气
		/// </summary>
		public const short STATUS_NSK_408 = 408;//
		/// <summary>
		/// 状态技能
		/// </summary>
		public const short STATUS_NSK_409 = 409;//
		/// <summary>
		/// 群体增益技能
		/// </summary>
		public const short STATUS_NSK_410 = 410;//
		/// <summary>
		/// 假死技能
		/// </summary>
		public const short STATUS_NSK_411 = 411;//
		/// <summary>
		/// 恢复技能
		/// </summary>
		public const short STATUS_NSK_412 = 412;//
		/// <summary>
		/// 斩杀
		/// </summary>
		public const short STATUS_NSK_413 = 413;//
		/// <summary>
		/// 主动自爆
		/// </summary>
		public const short STATUS_NSK_414 = 414;//

		/// <summary>
		/// 争夺差值
		/// </summary>
		public const short STATUS_NSK_415 = 415;//
		/// <summary>
		/// 无视属性克制
		/// </summary>
		public const short STATUS_NSK_416 = 416;//
		/// <summary>
		/// 变成全属性
		/// </summary>
		public const short STATUS_NSK_417 = 417;//
        /// <summary>
        /// 添加怒气的技能
        /// </summary>
        public const short STATUS_NSK_418 = 418;

        /// <summary>
        /// 添加自身复活的技能
        /// </summary>
        public const short STATUS_NSK_419 = 419;


        /// <summary>
        /// 战后恢复技能
        /// </summary>
        public const short STATUS_NSK_420 = 420;


		public short status;
		public string[] debugInfo;

		public static readonly Dictionary<short, Type> Table = new Dictionary<short, Type>(){
            {STATUS_WAR_BEGIN,     typeof(CMsgWarBegin) },
            {STATUS_ROUND_BEGIN,   typeof(CMsgRoundBegin) },
			//{STATUS_PROPERTY_CHG, typeof(.....) },
			{STATUS_PROPERTY_KILL, typeof(CMsgPropertyEnchance) },
            {STATUS_NSK_401,       typeof(CMsgSkillAttack) },
            {STATUS_NSK_402,       typeof(CMsgSkillChangeAttack) },
            {STATUS_NSK_403,       typeof(CMsgSkillAttackCombo) },
            {STATUS_NSK_404,       typeof(CMsgSkillSuckAttack) },
            {STATUS_NSK_405,       typeof(CMsgSkillChangeCurAttackBoth) },
            {STATUS_NSK_406,       typeof(CMsgSkillRevive) },
            {STATUS_NSK_407,       typeof(CMsgSkillBlast) },
            {STATUS_NSK_408,       typeof(CMsgSkillAngryExchange) },
            {STATUS_NSK_409,       typeof(CMsgSkillBuffOrDebuff) },
            {STATUS_NSK_410,       typeof(CMsgSkillChangeCurAttackAll) },
            {STATUS_NSK_411,       typeof(CMsgSkillDeath) },
            {STATUS_NSK_412,       typeof(CMsgSkillRecovery) },
            {STATUS_NSK_413,       typeof(CMsgSkillCut) },
            {STATUS_NSK_414,       typeof(CMsgSkillBomb) },
            {STATUS_NSK_416,       typeof(CMsgSkillAttack) },
			{STATUS_NSK_417,       typeof(CMsgSkillCast) },
            {STATUS_NSK_415,       typeof(CMsgSkillAttDelta) },
            {STATUS_NSK_418,       typeof(CMsgSkillCast)},
            {STATUS_NSK_419,       typeof(CMsgSkillRecovery)},
            {STATUS_NSK_420,       typeof(CMsgSkillAfterRecovery)},
            {STATUS_ATTACK,        typeof(CMsgNormalAttack)},
            {STATUS_REBOUND_ATT,   typeof(CMsgNormalAttack)},
            {STATUS_WAR_END,       typeof(CMsgWarEnd) },
            {STATUS_BUFF_DEBUFF,   typeof(CMsgSkillBuffDebuffEffect) },
			{STATUS_ANGRY_READY,   typeof(CMsgAngryReady)},
		};

	}

    public class CMsgAngryReady : CMsgHeader {
        //可释放的怒气技能次数
        public int Count;
        //释放的技能Id
        public int SkillId;
		//技能的伤害/效果 信息
		public ForcastSk forcast;

        public CMsgAngryReady() {
            status = CMsgHeader.STATUS_ANGRY_READY;
        }

		public CMsgAngryReady(int clickCnt, int skillId, ForcastSk forct) {
            status = CMsgHeader.STATUS_ANGRY_READY;
            this.Count   = clickCnt;
            this.SkillId = skillId;
			this.forcast = forct;
        }

    }

	public class CMsgWarBegin : CMsgHeader {
		//攻击方
		public Monsterteam attTeam;
		//防守方
		public Monsterteam defTeam;
        //攻击方的奥义
        public int[] attAoYi;
        //防守方的奥义
        public int[] defAoYi;

        public CMsgWarBegin(){ }

        public CMsgWarBegin(Monsterteam att, Monsterteam def, int[] attAoYi, int[] defAoYi) {
            status = CMsgHeader.STATUS_WAR_BEGIN;
            attTeam = att;
            defTeam = def;
            this.attAoYi = attAoYi;
            this.defAoYi = defAoYi;
        }
	}

	//每轮开始
	public class CMsgRoundBegin : CMsgHeader {
        //攻击方的pveindex 唯一id
		public int attacker;
		//防守方的pveIndex 唯一id
		public int defense;
		//攻击方当前战斗力
		public int attackerCurAtt;
		//防守方当前战斗力
		public int defenseCurAtt;

		//攻击方当前怒气
		public int attTeamAngry;
		//防守方当前怒气
		public int defTeamAngry;

        public CMsgRoundBegin() { }

        public CMsgRoundBegin (int attacker, int defense, int attackerCurAtt, int defenseCurAtt, int attTeamAngry, int defTeamAngry){
            status = CMsgHeader.STATUS_ROUND_BEGIN;
            this.attacker = attacker;
            this.defense  = defense;
            this.attackerCurAtt = attackerCurAtt;
            this.defenseCurAtt = defenseCurAtt;
            this.attTeamAngry = attTeamAngry;
            this.defTeamAngry = defTeamAngry;
        }

	}

	public class CMsgPropertyEnchance : CMsgHeader {
		//攻击方 防守方加成系数
		public float attackerEnch;
		public float defenseEnch;
		//加成后 攻击方 防守方战斗力
		public int attackerCurAtt;
		public int defenseCurAtt;

        public CMsgPropertyEnchance() { } 

        public CMsgPropertyEnchance(float attEnc, float defenseEnc, int attackerCurAtt, int defenseCurAtt){
            status = CMsgHeader.STATUS_PROPERTY_KILL;
            attackerEnch = attEnc;
            defenseEnch = defenseEnc;
            this.attackerCurAtt = attackerCurAtt;
            this.defenseCurAtt = defenseCurAtt;
        }
	}

	public class CMsgNormalAttack : CMsgHeader {
		//谁受到伤害
		public int suffer;
		//开始伤害 最终伤害 剩余战斗力.
		public int startAtt;
		public int finalAtt;
		public int curAtt;

        public CMsgNormalAttack() { }

        public CMsgNormalAttack(int suffer, int startAtt, short status = CMsgSkillCast.STATUS_ATTACK){
            this.status = status;
            this.suffer = suffer;
            this.startAtt = startAtt;
        }

	}

	//技能基类
	public class CMsgSkillCast : CMsgHeader {

        public const int categoryDirectAttack        = 1;
        public const int categoryChangeCurAttack     = 2;
        public const int categoryAttackCombo         = 3;
        public const int categorySuckAttack          = 4;
        public const int categoryChangeCurAttackBoth = 5;
        public const int categoryRevive              = 6;
        public const int categoryBlast               = 7;
        public const int categoryAngryExchange       = 8;
        public const int categoryBuffOrDebuff        = 9;
        public const int categoryChangeCurAttackAll  = 10;
        public const int categoryDeath               = 11;
        public const int categoryRecovery            = 12;
        public const int categoryCut                 = 13;
        public const int categoryBomb                = 14;
        public const int categoryAttDelta            = 15;
        public const int categoryIgnorePropertyKill  = 16;
        public const int categoryPropertyChangeToAll = 17;
        public const int categoryAddAngryPoint       = 18;
        public const int categoryReviveSelf          = 19;
        public const int categoryRecoveryAfterBattle = 20;

		//技能类型...
		public int category;

		//技能id 类型 与 放技能方.
		public int skillId;
		public int skillOp;
        //技能类型 0 怒气 1 普通 2 濒死 3 战后
		public int skillType;
		public int curAngry;
        //同一个技能释放的次数
        public int Mul;

		public int caster;

        public CMsgSkillCast() { }

        public CMsgSkillCast(BT_Skill skill, int category){
            this.status  =   CMsgHeader.STATUS_NOMARL_SKILL;
            this.skillId =   skill.id;
            this.skillOp =   skill.opObj.ID;
            this.skillType = skill.opObj.type;
            this.caster  =   skill.owner.pveId;

            this.category= category;
            this.status  = (short)(status * 100 + category);
            this.Mul     = skill.RdCast;
        }

	}

    //技能类型 0 怒气 1 普通 2 濒死 3 战后
    public enum CSkill_Type {
        Anger    = 0,
        Normal   = 1,
        Die      = 2,
        AfterWar = 3,
    }

    //1,2,101,102,116,117, 119
	public class CMsgSkillAttack : CMsgSkillCast {
		//谁受到伤害
		public int suffer;
		//开始伤害 最终伤害 剩余战斗力.
		public int startAtt;
		public int finalAtt;
		public int curAtt;
        //恢复的怒气
        public int recoverAngry;
        //消耗的怒气
        public int costAngry;
        //消耗时的怒气
        public int preAngry;

        public CMsgSkillAttack() { }

        public CMsgSkillAttack (BT_Skill skill, int suffer, int startAtt) : base(skill, CMsgSkillCast.categoryDirectAttack) {
            this.suffer   = suffer;
            this.startAtt = startAtt;
        }
	}

	//3,6,103,108,5,107
	public class CMsgSkillChangeAttack : CMsgSkillCast {
		//谁受到
		public int suffer;
		//加成系数 加成后战斗力.
		public float addAtt;
		public int curAtt;

        public CMsgSkillChangeAttack() { }

        public CMsgSkillChangeAttack (BT_Skill skill, int suffer) : base(skill, CMsgSkillCast.categoryChangeCurAttack) {
            this.suffer = suffer;
        }
	}

	//4,109
	public class CMsgSkillAttackCombo : CMsgSkillCast{
		public CMsgSkillAttack[] attackArr; //被攻击者数组

        public CMsgSkillAttackCombo() { }

        public CMsgSkillAttackCombo(BT_Skill skill) : base(skill, CMsgSkillCast.categoryAttackCombo) {
            attackArr = null;
        }
	}

	//7,8,110,111
	public class CMsgSkillSuckAttack : CMsgSkillCast {
	    public int suffer;			
	    public int suckAttack;		//初始攻击

	    public int convertAttack;	//增加自己多少攻击
	    public int finalSuckAtt;	//最终攻击
	    public int sufferCurAtt;	//被攻击者当前攻击
	    public int casterCurAtt;	//攻击者当前攻击

        public CMsgSkillSuckAttack() { }

        public CMsgSkillSuckAttack(BT_Skill skill, int suffer, int suckAttack) : base (skill, CMsgSkillCast.categorySuckAttack) {
            this.suffer     = suffer;
            this.suckAttack = suckAttack;
        }
	}

	//9,10
	public class CMsgSkillChangeCurAttackBoth : CMsgSkillCast{
		public int selfAttChange;	//自身攻击变化	
	    public int enemyAttChange;	//敌人攻击变化
	    public int selfCurAtt;	    //自己当前攻击
	    public int enemyCurAtt;	    //敌方当前攻击

        public CMsgSkillChangeCurAttackBoth() { }

        public CMsgSkillChangeCurAttackBoth(BT_Skill skill) : base (skill, CMsgSkillCast.categoryChangeCurAttackBoth) {

        }
	}

	//11,12
	public class CMsgSkillRevive : CMsgSkillCast{
	    public int[] reviveArr;	//被复活者列表

        public CMsgSkillRevive() { }

        public CMsgSkillRevive(BT_Skill skill) : base (skill, CMsgSkillCast.categoryRevive) {
            reviveArr = null;
        }
	}

	//13,14,15,17
	public class CMsgSkillBlast : CMsgSkillCast{
	    public CMsgSkillAttack[] sufferArr;	//被攻击者数组

        public CMsgSkillBlast () { }

        public CMsgSkillBlast(BT_Skill skill) : base (skill, CMsgSkillCast.categoryBlast) {
            sufferArr = null;
        }
	}
	
    //105
	public class CMsgSkillAngryExchange : CMsgSkillCast{
	    public int costAngry;	//花费怒气
	    public int addAngry;	//增加怒气

        public CMsgSkillAngryExchange () { }

        public CMsgSkillAngryExchange(BT_Skill skill) : base(skill, CMsgSkillCast.categoryAngryExchange) {

        }
	}

	public class CMsgSkillBuffOrDebuff : CMsgSkillCast{
	    public int type;       //buff Or debuff类型.
		public int round;      //持续回合
        public int[] sufferArr;  //玩家列表
		public float arg1;
		public float arg2;
		public float arg3;

        public CMsgSkillBuffOrDebuff() { }

        public CMsgSkillBuffOrDebuff(BT_Skill skill) : base(skill, CMsgSkillCast.categoryBuffOrDebuff) {
            sufferArr = null;
        }
	}
	//18
	public class CMsgSkillChangeCurAttackAll : CMsgSkillCast{
	    public CMsgSkillChangeAttack[] sufferArr;	//攻击变化数组

        public CMsgSkillChangeCurAttackAll() { }

        public CMsgSkillChangeCurAttackAll(BT_Skill skill) : base(skill, CMsgSkillCast.categoryChangeCurAttackAll) {
            sufferArr = null;
        }
	}
	//20
	public class CMsgSkillDeath : CMsgSkillCast{
	    public int suffer;	//释放者	
	    public int curAtt;	//当前攻击

        public CMsgSkillDeath () { }

        public CMsgSkillDeath (BT_Skill skill) : base(skill, CMsgSkillCast.categoryDeath) {

        }
	}
	//
	public class CMsgSkillRecovery : CMsgSkillCast{
		public int suffer;		//释放者
	    public int startAtt;	//开始攻击
	    public int finalAtt;	//回复后,最终攻击

        public CMsgSkillRecovery () { }

        public CMsgSkillRecovery(BT_Skill skill) : base(skill, CMsgSkillCast.categoryRecovery) {

        }
	}
    //22
    public class CMsgSkillAfterRecovery : CMsgSkillCast{
        public int suffer;      //释放者
        public int startAtt;    //开始攻击
        public int finalAtt;    //回复后,最终攻击
        public int enemyAtt;    //敌人的血量

        public CMsgSkillAfterRecovery() { }

        public CMsgSkillAfterRecovery(BT_Skill skill) : base(skill, CMsgSkillCast.categoryRecoveryAfterBattle) {

        }
    }
	//104
	public class CMsgSkillCut : CMsgSkillCast{
	    public int suffer;		//敌人
	    public int startAtt;	//初始攻击
	    public int finalAtt;	//最终攻击
	    public int curAtt;		//敌人当前攻击

        public CMsgSkillCut() { }

        public CMsgSkillCut(BT_Skill skill, int suffer) : base(skill, CMsgSkillCast.categoryCut) {
            this.suffer = suffer;
        }
	}
	//112
	public class CMsgSkillBomb : CMsgSkillCast{ //注释同上
        public int suffer;
	    public int startAtt;
	    public int finalAtt;
	    public int selfCurAtt;
	    public int enemyCurAtt;

        public CMsgSkillBomb() { }

        public CMsgSkillBomb(BT_Skill skill, int suffer, int startAtt) : base(skill, CMsgSkillCast.categoryBomb) {
            this.suffer   = suffer;
            this.startAtt = startAtt;
        }
	}
	//28
	public class CMsgSkillAttDelta : CMsgSkillCast{
		public int addAtt;	//增加了多少攻击
		public int beforeAtt;
	    public int curAtt;
        public int enemyAtt;

        public CMsgSkillAttDelta() { }

        public CMsgSkillAttDelta(BT_Skill skill, int addAtt) : base(skill, CMsgSkillCast.categoryAttDelta) {
            this.addAtt = addAtt;
        }
	}

    public class CMsgSkillBuffDebuff : CMsgSkillCast {
        public int type;         //buff Or debuff类型.
        public int round;        //持续回合
        public int[] sufferArr;  //玩家列表
        public int arg1;
        public int arg2;
        public int arg3;

        public CMsgSkillBuffDebuff() { }

        public CMsgSkillBuffDebuff(BT_Skill skill) : base(skill, CMsgSkillCast.categoryBuffOrDebuff) {
            sufferArr = null;
        }
    }


    public class CMsgSkillReviveSelf : CMsgSkillCast {
        public int suffer;
        public int startAtt;
        public int finalAtt;

        public CMsgSkillReviveSelf() { }

        public CMsgSkillReviveSelf(BT_Skill skill) : base(skill, CMsgSkillCast.categoryReviveSelf) {

        }
    }



    public class CMsgSkillBuffDebuffEffect : CMsgHeader {
		public int suffer;

	    public int type; 	//buff / debuff 类型...
	    public int round;	//剩余回合
	    public int arg1;
	    public int arg2;
	    public int arg3;

        public CMsgSkillBuffDebuffEffect() { }

        public CMsgSkillBuffDebuffEffect(BT_Monster pet, BT_BuffOrDebuff bufOrDebuff, short status = CMsgSkillCast.STATUS_BUFF_DEBUFF) {
            this.status = status;
            this.suffer = pet.pveId;
            this.type   = bufOrDebuff.type;
            this.round  = bufOrDebuff.round;
        }

	}

	public class CMsgWarEnd : CMsgHeader {
		public string winner; //att def...

        public CMsgWarEnd() { }

        public CMsgWarEnd(string winner) {
            status = CMsgHeader.STATUS_WAR_END;
            this.winner = winner;
        }
	}

}