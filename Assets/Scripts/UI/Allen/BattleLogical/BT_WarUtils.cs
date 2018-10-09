using System;
using fastJSON;
using System.Collections.Generic;

namespace AW.Battle {
    public static class BT_WarUtils {

        //-----常量的定义-----

        //每次增加或减少的怒气值的单位是10
		public const int Unit_Angry = 10;
        public const int Max_Angry  = 100;


        public const int proNormal  = 0;
        public const int proSuper   = 1;
        public const int proTypeAll = 2;

        public const float proNormalKill   = 1.15f;
        public const float proNormalBeKill = 0.85f;
        public const float proSuperKill    = 1.25f;
        public const float proSuperBeKill  = 0.95f;
        public const float proAllKill      = 1.3f;
        public const float proNormalVsSuper= 0.85f;

        /// <summary>
        /// 依据技能的优先级来排序
        /// </summary>
        /// <returns>The skill arr.</returns>
        public static int sortSkillArr(BT_Skill sk1, BT_Skill sk2) {
            short SkPrior1 = sk1.opObj.prior;
            short SkPrior2 = sk2.opObj.prior;

            return SkPrior1 <= SkPrior2 ? 1 : -1;
        }

        public static float[] propertyType(MonsterAttribute att){
            float[] result = new float[3];
            int pro = (int)att;

            if(pro >= (int)MonsterAttribute.GOLDEN && pro <= (int)MonsterAttribute.FIRE) {
                result[0] = proNormal;
                result[1] = proNormalKill;
                result[2] = proNormalBeKill;
            }
            else if(pro >= (int)MonsterAttribute.GOLDEN_S && pro <= (int)MonsterAttribute.FIRE_S) {
                result[0] = proSuper;
                result[1] = proSuperKill;
                result[2] = proSuperBeKill;
            }
            else if(pro == (int)MonsterAttribute.ALL){
                result[0] = proTypeAll;
                result[1] = proAllKill;
                result[2] = 1;
            }

            return result;
        }

        public static float[] propertyVs (MonsterAttribute pro1, MonsterAttribute pro2){
            float[] result = new float[] {1,1};

            float[] proArr1 = propertyType(pro1);
            float[] proArr2 = propertyType(pro2);
            if(pro1 == MonsterAttribute.ALL || pro2 == MonsterAttribute.ALL) {
                if(pro1 == MonsterAttribute.ALL && pro2 == MonsterAttribute.ALL)
                    result = new float[] {proArr1[2], proArr2[2]};
                else if(pro1 == MonsterAttribute.ALL)
                    result = new float[] {proArr1[1], proArr2[2]};
                else if(pro2 == MonsterAttribute.ALL)
                    result = new float[] {proArr1[2], proArr2[1]};
            } else{

                result = Vs(pro1, pro2);
                if(result[0] == 1 && result[1] == 1) {
                    result = Vs(pro2, pro1);

                    float temp = result[0];
                    result[0] = result[1];
                    result[1] = temp;
                }

            }
            return result;
        }

        static float[] Vs (MonsterAttribute pro1, MonsterAttribute pro2) {
            float[] result = new float[]{1, 1} ;

            if(pro1 == MonsterAttribute.GOLDEN && pro2 == MonsterAttribute.WOOD) {
                result = new float[] { proNormalKill, proNormalBeKill};
            }
            if(pro1 == MonsterAttribute.GOLDEN && pro2 == MonsterAttribute.WOOD_S) {
                result = new float[] { proNormalKill, proSuperBeKill };
            }
            if(pro1 == MonsterAttribute.WOOD && pro2 == MonsterAttribute.SOIL) {
                result = new float[] { proNormalKill, proNormalBeKill};   
            }
            if(pro1 == MonsterAttribute.WOOD && pro2 == MonsterAttribute.SOIL_S) {
                result = new float[] {proNormalKill, proSuperBeKill};
            }
            if(pro1 == MonsterAttribute.SOIL && pro2 == MonsterAttribute.WATER) {
                result = new float[] {proNormalKill, proNormalBeKill};
            }
            if(pro1 == MonsterAttribute.SOIL && pro2 == MonsterAttribute.WATER_S) {
                result = new float[] {proNormalKill, proSuperBeKill};
            }
            if(pro1 == MonsterAttribute.WATER && pro2 == MonsterAttribute.FIRE) {
                result = new float[] {proNormalKill, proNormalBeKill};
            }
            if(pro1 == MonsterAttribute.WATER && pro2 == MonsterAttribute.FIRE_S) {
                result = new float[] {proNormalKill, proSuperBeKill};
            }
            if(pro1 == MonsterAttribute.FIRE && pro2 == MonsterAttribute.GOLDEN) {
                result = new float[] {proNormalKill, proNormalBeKill};
            }
            if(pro1 == MonsterAttribute.FIRE && pro2 == MonsterAttribute.GOLDEN_S) {
                result = new float[] {proNormalKill, proSuperBeKill};
            }


            if(pro1 == MonsterAttribute.GOLDEN_S && pro2 == MonsterAttribute.WOOD) {
                result = new float[] {proSuperKill, proNormalVsSuper};
            }
            if(pro1 == MonsterAttribute.GOLDEN_S && pro2 == MonsterAttribute.WOOD_S) {
                result = new float[] {proSuperKill, proSuperBeKill};
            }
            if(pro1 == MonsterAttribute.WOOD_S && pro2 == MonsterAttribute.SOIL) {
                result = new float[] {proSuperKill, proNormalVsSuper};   
            }
            if(pro1 == MonsterAttribute.WOOD_S && pro2 == MonsterAttribute.SOIL_S) {
                result = new float[] {proSuperKill, proSuperBeKill};
            }
            if(pro1 == MonsterAttribute.SOIL_S && pro2 == MonsterAttribute.WATER) {
                result = new float[] {proSuperKill, proNormalVsSuper};
            }
            if(pro1 == MonsterAttribute.SOIL_S && pro2 == MonsterAttribute.WATER_S) {
                result = new float[] {proSuperKill, proSuperBeKill};
            }
            if(pro1 == MonsterAttribute.WATER_S && pro2 == MonsterAttribute.FIRE) {
                result = new float[] {proSuperKill, proNormalVsSuper};
            }
            if(pro1 == MonsterAttribute.WATER_S && pro2 == MonsterAttribute.FIRE_S) {
                result = new float[] {proSuperKill, proSuperBeKill};
            }
            if(pro1 == MonsterAttribute.FIRE_S && pro2 == MonsterAttribute.GOLDEN) {
                result = new float[] {proSuperKill, proNormalVsSuper};
            }
            if(pro1 == MonsterAttribute.FIRE_S && pro2 == MonsterAttribute.GOLDEN_S) {
                result = new float[] {proSuperKill, proSuperBeKill};
            }
            return result;
        }

        /// <summary>
        /// specified possibility.
        /// </summary>
        /// <param name="possibility">Possibility.0-100</param>
        public static bool happen(int possibility) { 
            int rnd = PseudoRandom.getInstance().next(100);
            return rnd <= possibility;
        }

        public static void Shuffle<T>(IList<T> list) {
            Random rng = new Random();
            int n = list.Count;  
            while (n > 1) {  
                n--;  
                int k = rng.Next(n + 1);  
                T value = list[k];  
                list[k] = list[n];  
                list[n] = value;
            }  
        }


		/// <summary>
		/// 将战斗时候的数据，转化为Json
		/// </summary>
		/// <returns>The json.</returns>
		/// <param name="msg">Message.</param>
		public static string ToJson(CMsgHeader msg) {
			string json = string.Empty;

			switch(msg.status) {
			case CMsgHeader.STATUS_WAR_BEGIN: {
					var real = msg as CMsgWarBegin;
					if(real == null) json = JSON.Instance.ToJSON(msg);
					else json = JSON.Instance.ToJSON(real);
				}

				break;
			case CMsgHeader.STATUS_ROUND_BEGIN: {
					var real = msg as CMsgRoundBegin;
					if(real == null) json = JSON.Instance.ToJSON(msg);
					else json = JSON.Instance.ToJSON(real);
				}
				break;
			case CMsgHeader.STATUS_PROPERTY_KILL: {
					var real = msg as CMsgPropertyEnchance;
					if(real == null) json = JSON.Instance.ToJSON(msg);
					else json = JSON.Instance.ToJSON(real);
				}
				break;
			case CMsgHeader.STATUS_ATTACK: case CMsgHeader.STATUS_REBOUND_ATT: {
					var real = msg as CMsgNormalAttack;
					if(real == null) json = JSON.Instance.ToJSON(msg);
					else json = JSON.Instance.ToJSON(real);
				}
				break;
			case CMsgHeader.STATUS_WAR_END: {
					var real = msg as CMsgWarEnd;
					if(real == null) json = JSON.Instance.ToJSON(msg);
					else json = JSON.Instance.ToJSON(real);
				}
				break;
			case CMsgHeader.STATUS_BUFF_DEBUFF:{
					var real = msg as CMsgSkillBuffDebuffEffect;
					if(real == null) json = JSON.Instance.ToJSON(msg);
					else json = JSON.Instance.ToJSON(real);
				}
				break;
			case CMsgHeader.STATUS_NSK_401: case CMsgHeader.STATUS_NSK_416: {
					var real = msg as CMsgSkillAttack;
					if(real == null) json = JSON.Instance.ToJSON(msg);
					else json = JSON.Instance.ToJSON(real);
				}
				break;
			case CMsgHeader.STATUS_NSK_402:{
					var real = msg as CMsgSkillChangeAttack;
					if(real == null) json = JSON.Instance.ToJSON(msg);
					else json = JSON.Instance.ToJSON(real);
				}
				break;
			case CMsgHeader.STATUS_NSK_403: {
					var real = msg as CMsgSkillAttackCombo;
					if(real == null) json = JSON.Instance.ToJSON(msg);
					else json = JSON.Instance.ToJSON(real);
				}
				break;
			case CMsgHeader.STATUS_NSK_404:{
					var real = msg as CMsgSkillSuckAttack;
					if(real == null) json = JSON.Instance.ToJSON(msg);
					else json = JSON.Instance.ToJSON(real);
				}
				break;
			case CMsgHeader.STATUS_NSK_405:{
					var real = msg as CMsgSkillChangeCurAttackBoth;
					if(real == null) json = JSON.Instance.ToJSON(msg);
					else json = JSON.Instance.ToJSON(real);
				}
				break;
			case CMsgHeader.STATUS_NSK_406:{
					var real = msg as CMsgSkillRevive;
					if(real == null) json = JSON.Instance.ToJSON(msg);
					else json = JSON.Instance.ToJSON(real);
				}
				break;
			case CMsgHeader.STATUS_NSK_407:{
					var real = msg as CMsgSkillBlast;
					if(real == null) json = JSON.Instance.ToJSON(msg);
					else json = JSON.Instance.ToJSON(real);
				}
				break;
			case CMsgHeader.STATUS_NSK_408:{
					var real = msg as CMsgSkillAngryExchange;
					if(real == null) json = JSON.Instance.ToJSON(msg);
					else json = JSON.Instance.ToJSON(real);
				}
				break;
			case CMsgHeader.STATUS_NSK_409:{
					var real = msg as CMsgSkillBuffOrDebuff;
					if(real == null) json = JSON.Instance.ToJSON(msg);
					else json = JSON.Instance.ToJSON(real);
				}
				break;
			case CMsgHeader.STATUS_NSK_410:{
					var real = msg as CMsgSkillChangeCurAttackAll;
					if(real == null) json = JSON.Instance.ToJSON(msg);
					else json = JSON.Instance.ToJSON(real);
				}
				break;
			case CMsgHeader.STATUS_NSK_411:{
					var real = msg as CMsgSkillDeath;
					if(real == null) json = JSON.Instance.ToJSON(msg);
					else json = JSON.Instance.ToJSON(real);
				}
				break;
			case CMsgHeader.STATUS_NSK_412: case CMsgHeader.STATUS_NSK_419: {
					var real = msg as CMsgSkillRecovery;
					if(real == null) json = JSON.Instance.ToJSON(msg);
					else json = JSON.Instance.ToJSON(real);
				}
				break;
			case CMsgHeader.STATUS_NSK_413:{
					var real = msg as CMsgSkillCut;
					if(real == null) json = JSON.Instance.ToJSON(msg);
					else json = JSON.Instance.ToJSON(real);
				}
				break;
			case CMsgHeader.STATUS_NSK_414:{
					var real = msg as CMsgSkillBomb;
					if(real == null) json = JSON.Instance.ToJSON(msg);
					else json = JSON.Instance.ToJSON(real);
				}
				break;
			case CMsgHeader.STATUS_NSK_415:{
					var real = msg as CMsgSkillAttDelta;
					if(real == null) json = JSON.Instance.ToJSON(msg);
					else json = JSON.Instance.ToJSON(real);
				}
				break;
			case CMsgHeader.STATUS_NSK_417: case CMsgHeader.STATUS_NSK_418: {
					var real = msg as CMsgSkillCast;
					if(real == null) json = JSON.Instance.ToJSON(msg);
					else json = JSON.Instance.ToJSON(real);
				}
				break;
			case CMsgHeader.STATUS_NSK_420:{
					var real = msg as CMsgSkillAfterRecovery;
					if(real == null) json = JSON.Instance.ToJSON(msg);
					else json = JSON.Instance.ToJSON(real);
				}
				break;
			default:
				json = JSON.Instance.ToJSON(msg);
				break;
			}

			return json;
		}



    }

}
