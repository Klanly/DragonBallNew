using UnityEngine;
using System.Collections;


#region 究极试炼的各种挑战
public enum TrialEnum 
{
	TrialType_Title = 0,
	TrialType_ShaLuChoose = 1,
	TrialType_PuWuChoose = 2,
	TrialType_TianXiaDiYi = 3,
	TrialType_QiangDuoGold = 4,
	TrialType_QiangDuoDragonBall = 5,
    TrialType_Enter = 6,
//    TrialType_Map = 7,
//	TrialType_Map_AddAttr = 8,
//    TrialType_SuDi=9, //宿敌
	TrialType_ShaluAndBuou = 7,
	None
}
#endregion
#region 抢夺挑战龙珠界面的4选项
public enum QiangduoEnum
{
	QiangduoEnum_None = 0,
	QiangduoEnum_List = 1,
	QiangduoEnum_Sudi = 2,
	QiangduoEnum_Duihuan = 3,
	QiangduoEnum_Playback = 4,
}
#endregion
#region
//终极试炼入场选择加成类型
public enum FinalTrialAdditionType
{
    Addition_None = 0,
    Addition_SelfAttack = 1,
    Addition_SelfDefese = 2,
    Addition_SelfSkill = 3,
    Addition_EnemyAttack = 4,
    Addition_EnemyDefese = 5,
    Addition_EnemySkill = 6,
}
#endregion

#region
//终极试攻击或者防御
public enum FinalTrialAtcOrDef
{
	FinalTrialAtcOrDef_None = 0,
	FinalTrialAtcOrDef_Attack = 1,
	FinalTrialAtcOrDef_Defense = 2,
}
#endregion

#region
//终极试攻击或者防御
public enum FinalTrialDougoenType
{
	FinalTrialDougoenType_None = 0,
	FinalTrialDougoenType_Fight = 1,
	FinalTrialDougoenType_Award = 2,
	FinalTrialDougoenType_Addattr = 3,
	FinalTrialDougoenType_Point = 4,
	FinalTrialDougoenType_AwardAndAddattr = 5,
}
#endregion
