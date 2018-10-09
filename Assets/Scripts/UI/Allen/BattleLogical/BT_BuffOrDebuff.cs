using System;

namespace AW.Battle {
    /// <summary>
    /// 战斗过程中，Buff 和 Debuff. 下面的除了type 和 round,其他的值未必会赋值
    /// </summary>
    public class BT_BuffOrDebuff  {

        public short type;
        public int round;

        //技能 -
        public BT_Skill skill;
        //吸取 - 伤害
        public int suckDmg;

        //伤害翻倍
        public float rate;
    }
}
