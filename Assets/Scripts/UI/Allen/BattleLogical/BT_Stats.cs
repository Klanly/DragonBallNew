using System;

/// <summary>
/// Monster State 战斗时候的状态数据, 同时都有攻击，防守方
/// </summary>
namespace AW.Battle {

    public class BT_Stats <T1, T2>  where T1 : BT_Monster where T2 : BT_Monster {
        //检测进入的条件，如果进入条件不符合，则线面的execute都不会执行
        //或者是设定进入的变量值
        public virtual void Enter(T1 mon1, T2 mon2) { }
        //执行当前状态的工作
        public virtual void Execute(T1 mon1, T2 mon2) { }
        //执行退出当前状态的工作
        public virtual void Exit(T1 mon1, T2 mon2) { }
    }







}