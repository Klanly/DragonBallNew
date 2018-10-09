//动画时间
public class BanTimeCenter {
	//登场
	public static float F_DengChang = 1f;
	//属性相克
	public static float F_Attribute_Time = 1.2f;
	//回合间隔
	public static float F_ROUND_GAP = 0.25f;
	//血量动画时间
	public static float F_HP_ANIM = 0.5f;
	//对决血量的时间
	public static float F_HP_SLOW_ANIM = 1.2f;

    public const float F_REDUCE_HP = 1.0f;
    public const float F_DIE_GOHST = 1.3f;
	public const float F_KO = 1.2f;

	public const float F_DIE_GOHST_PH1 = 0.3f;
	public const float F_DIE_GOHST_PH2 = 1.0f;

    public const float F_SELF_ENHANCE = 3.8f;
    //鬼魂飞的时间
    public const float F_GOHST_FLY_TIME = 0.5f;
    //回合时间
    public const float F_DIE_IDLE = 1.0f;
    //HP动画时间
    public const float F_HP_ANIM_TIME = 0.1f;
    //Vs动画的时间
	#if VS
	public const float F_VS_HP_TIME   = 3.4f;
	#else
	public const float F_VS_HP_TIME   = 0.4f;
	#endif
    
	//乌龙变身时间
	public const float F_WU_BIANSHEN  = 1.4f;

	//op ==3 掉血
	public const float F_Combo_HP     = 2f;
	//头像移动
	public const float F_MOVE_HEAD    = 1.3f;

    //等待普通战斗的等待时间
    public const float F_Wait_For_Battle = 0.5f;

    //等待释放技能名字的时间
    public const float F_Wait_For_Show_SkillName = 1.2f;

    //等待一会儿
    public const float F_Wait_For_CommonSkill = 1.0f;

    //吸取的时间
    public const float F_Wait_For_SuckBlood = 1.0f;

    //等待奥义的时间
    public const float F_Wait_For_AoYi = 6.0f;

    //暴怒气-准备出现点击按钮的阶段
    public const float F_Free_Time = 0.5f;

    //怒气技能的时间
    public const float F_Angry_WT  = 2f;

    //播放overskill
    public const float F_Show_OVERSKILL = 1.5f;

    public const float F_Hide_Btn = 3.8f;

	//龙珠掉落的时间
	public const float F_Drop_Ball = 1.6f;
    //播放VS的动画时间
    public const float VS_FIGHT_TIME = 1.6f;

    //自己复活自己的时间
    public const float F_WAIT_REVIVE = 2.0f;

    //回复的时间
    public const float F_WAIT_RECOVER = 1.6f;

    //登场时间
    public const float F_Attend = 1f;

    //某些特殊的技能，会没有那么快出现InjureFlyGo
    public const float F_LongSkill = 7f;

    //自爆的时间
    public const float F_EXPLORE_TIME = 2f;

    //增加一点怒气的时间
    public const float AddOneAngry = 1f;

    //等待小悟空克制比克大魔王的时间
	public const float XiaoWuKong_V_BiKe = 0.3f;

	//小悟空怒气技能（教学）狂揍-释放4倍
	public const float XiaoWuKong_OS_4 = 0.34f;
	//孙悟空2继承小悟空20怒气
	public const float WuKong2_Anger   = 0.5f;
	//孙悟空2释放终结技能
	public const float WuKong2_OS_4    = 2f;
	//孙悟空3释放终结技能
	public const float WuKong3_OS_4    = 2f;
	//比鲁斯
	public const float BILUSI_V_WuKong3= 0.3f;
	//没有怒气值不能释放主动技能
	public const float WuKong3_NO_ANGER= 3f;
	//孙悟空3对决比鲁斯
	public const float WuKong3_Vs_BiLuSi = 0.2f;

	//布尔玛出现
	public const float BuErMa_Show     = 2f;

	//新手引导的第一关
	public const float XiaoWuKong_Level1 = 0.4f;

	//等待一会儿，再调回主界面
	public const float WaitForJump = 1f;

	//乌龙变身，攻击的时间
	public const float WaitWuLong = 3.6f;

	//死亡技能挨上后，要站起来
	public const float WaitForDeathSkill_StandUp = 2f;

	//死亡技能掉血
	public const float WaitForDeathSkill_ReduceHp = 2f;

    //释放怒气技能
    public const float Wait_For_User_Cast_AngrySk = 2.5f;

	//防止点击怒气技太快，导致UI不显示出来
	public const float Wait_If_Time_Is_Little = 1f;

    //技能释放时候，时间变慢
    public const float Scale_Down_Slow = 0.15f;
    public const float Scale_Down_To = 0.6f;
    //技能释放
    public const float Scale_Down_To2 = 0.8f;

	//每次都等上一会儿，检测怒气技是否可用
	public const float Wait_Per_Unit = 0.2f;

	//新版本的登场动画
	public const float Wait_For_AttendAnim = 1.3f;

	//死亡的时候播放慢动作
	public const float Scale_Down_If_Die = 0.3f;
}
