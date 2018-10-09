using System;
using System.Reflection;

namespace AW.Battle {

	/// <summary>
	/// 预先播报技能的情况
	/// </summary>
	public class ForcastSk {
		/// <summary>
		/// 1 自己， -1 敌人
		/// </summary>
		public int target;

		/// <summary>
		/// 如果target == 1， 则是回血
		/// </summary>
		public int dmgOrRec;

		/// <summary>
		/// 封印个数
		/// </summary>
		public int SealCnt;

		/// <summary>
		/// 怒气回复数量
		/// </summary>
		public int AngryRec;
	} 

	/// <summary>
	/// 这个主要考虑的是预先计算的技能逻辑
	/// </summary>

	public partial class BT_Skill {

		//需要预先计算的怒气技能合集
		private int[] AngryOpArr = new int[]{
			101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 116, 118, 119,
		};

		public ForcastSk forcastInfo() {
			string funName = "preCastSkOp" + opObj.ID;
			Type t = typeof(BT_Skill);

			Object PreCast = t.InvokeMember( funName, 
				BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance, 
				null, this, null);

			ForcastSk realRes = PreCast as ForcastSk;
			return realRes;
		}

		#region 计算的怒气技能伤害的预计算

		private ForcastSk preCastSkOp101() {
			int damage = param.damage;
			int Volumn = param.rate;
			damage     = unstableDamage (damage, Volumn);

			ForcastSk forcast = new ForcastSk() {
				target   = -1,
				dmgOrRec = damage,
			};

			return forcast;
		}

		private ForcastSk preCastSkOp102() {
			int damage = MathHelper.MidpointRounding(param.rate * Consts.oneHundred * owner.curAtt);
			ForcastSk forcast = new ForcastSk() {
				target   = -1,
				dmgOrRec = damage,
			};

			return forcast;
		}

		private ForcastSk preCastSkOp103() {
			BT_Logical war   = owner._war;
			BT_Monster enemy = war.enemy(owner);
			float addRate    = param.rate;
			int hp           = enemy.curAtt;
			int damage       = MathHelper.MidpointRounding (hp * addRate * Consts.oneHundred);

			ForcastSk forcast = new ForcastSk() {
				target   = -1,
				dmgOrRec = damage,
			};
			return forcast;
		}

		private ForcastSk preCastSkOp104() {
			BT_Logical war = owner._war;
			BT_Monster enemy = war.enemy ( owner );

			int beforeAtt = enemy.curAtt;
			float cutAtt  = enemy.initAtt * param.rate * Consts.oneHundred;
			int damage    = param.damage;

			if ( (beforeAtt - damage) < cutAtt) {
				damage = beforeAtt;
			}

			ForcastSk forcast = new ForcastSk() {
				target   = -1,
				dmgOrRec = damage,
			};
			return forcast;
		}

		/// <summary>
		/// 怒气回复技能
		/// </summary>
		private ForcastSk preCastSkOp105() {
			ForcastSk forcast = new ForcastSk() {
				target   = 1,
				AngryRec = param.num,
			};
			return forcast;
		}

		/// <summary>
		/// 封印技能
		/// </summary>
		private ForcastSk preCastSkOp106() {
			ForcastSk forcast = new ForcastSk() {
				target   = -1,
				SealCnt  = param.num,
			};
			return forcast;
		}

		private ForcastSk preCastSkOp107() {
			int Rec = param.add;

			ForcastSk forcast = new ForcastSk() {
				target   = 1,
				dmgOrRec = Rec,
			};
			return forcast;
		}

		private ForcastSk preCastSkOp108() {
			int Rec = MathHelper.MidpointRounding(
				owner.curAtt * ( 1 + param.rate * Consts.oneHundred)
			);

			ForcastSk forcast = new ForcastSk() {
				target   = 1,
				dmgOrRec = Rec,
			};
			return forcast;
		}

		private ForcastSk preCastSkOp109() {
			float dmg  = param.rate * Consts.oneHundred * owner.curAtt;
			int damage = MathHelper.MidpointRounding(dmg * param.num);

			ForcastSk forcast = new ForcastSk() {
				target   = -1,
				dmgOrRec = damage,
			};
			return forcast;
		}

		/// <summary>
		/// 吸血技能
		/// </summary>
		private ForcastSk preCastSkOp110() {
			int damage = param.damage;

			ForcastSk forcast = new ForcastSk() {
				target   = -1,
				dmgOrRec = damage,
			};
			return forcast;
		}

		private ForcastSk preCastSkOp111() {
			BT_Logical war   = owner._war;
			BT_Monster enemy = war.enemy(owner);

			int damage = MathHelper.MidpointRounding(param.rate * Consts.oneHundred * enemy.curAtt);

			ForcastSk forcast = new ForcastSk() {
				target   = -1,
				dmgOrRec = damage,
			};
			return forcast;
		}

		private ForcastSk preCastSkOp112() {
			int damage = MathHelper.MidpointRounding(param.rate * Consts.oneHundred * owner.curAtt);

			ForcastSk forcast = new ForcastSk() {
				target   = -1,
				dmgOrRec = damage,
			};
			return forcast;
		}

		private ForcastSk preCastSkOp116() {
			int damage = MathHelper.MidpointRounding(
				param.rate * Consts.oneHundred * owner.curAtt
			);

			int angryrec = param.num;

			ForcastSk forcast = new ForcastSk() {
				target   = -1,
				dmgOrRec = damage,
				AngryRec = angryrec,
			};
			return forcast;
		}

		private ForcastSk preCastSkOp118() {
			BT_Logical war = owner._war;
			BT_Monster enemy = war.enemy ( owner );

			int damage = MathHelper.MidpointRounding(
				param.rate * Consts.oneHundred * enemy.curAtt
			);

			ForcastSk forcast = new ForcastSk() {
				target   = -1,
				dmgOrRec = damage,
			};
			return forcast;
		}

		private ForcastSk preCastSkOp119() {
			BT_Logical war = owner._war;
			BT_Monster enemy = war.enemy ( owner );

			int damage = MathHelper.MidpointRounding(
				param.rate * Consts.oneHundred * enemy.curAtt
			) + param.num;

			ForcastSk forcast = new ForcastSk() {
				target   = -1,
				dmgOrRec = damage,
			};
			return forcast;
		}

		#endregion
	}

}

