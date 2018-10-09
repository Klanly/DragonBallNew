using System;

public class TemporyData {
	//战斗结束后更新用户等级
	public int AfterBattleLv;
	
	public int mPreLevel = -1;
	public int mPreVipLv;

	public TemporyData() { 
		AfterBattleLv = 1;
		TempTouch = true;
	}
	
    #region 战斗结算的存放地

	public BattleSequence warBattle;
    //只有服务器错误才有这个值
    public int WarErrorCode = -1;
    //只有网络异常的时候有这个值
    public BaseHttpRequest WarReq;
    #endregion

	public enum BattleType
	{
		BossBattle,
		TianXiaDiYiBattle,
		QiangDuoGoldBattle,
		QiangDuoDragonBallBattle,
		SuDiBattle,
		None,
		WorldBossWar,
        FinalTrialBuou,
        FinalTrialShalu,
		Firend,
		PVPVideo,
		FightWithFulisa,        //新手引导的未来之战
		Revenge,
		GPSWar,					//pgs组队战
	}

	public BattleType currentBattleType = BattleType.None;
    //战斗场景
    public int CitySence;

    public Monster preMonsterData;

	public Equipment preEquipData;

    /// <summary>
    /// 隐藏主界面之前  判定是否 teaminfo 是否展开 
    /// </summary>
    public bool isShowTeam;
	
    //public bool isDownloadIMG = true; 

	public bool BattleSudi = false;

    //主界面
    public int tempTeamAtk = 0;
    public int tempTeamDef =0;
    public int deltaAtk = 0;
    public int deltaDef =0;

    //人物信息界面
    public int infoPetAtk =0;
    public int infoPetDef =0;
    public int tempId =0;
    public int deltaInfoAtk =0;
    public int deltaInfoDef =0;
    public int equipAtk =0;
    public int equipDef =0;

    #region 赌博
    public int gambleTypeId = -1;
    #endregion

	#region Purchase Status
	public int PurStatus = -1;
	#endregion

    #region 展示一场战斗的总连击数
    private int mTotalCombo;

    public int TotalCombo {
        get {
            return mTotalCombo;
        }
        set {
            //如果重播了，则不计算
            if(!hasLiquidate)
                mTotalCombo = value;
        }
    }

    //强制刷新一个值
    public int ForceWriteTotalCombo {
        set { mTotalCombo = value;}
    }
    #endregion

    #region 展示一场战斗的最大连击数
    private int mMAXCombo;

    public int MaxCombo {
        get {
            return mMAXCombo;
        }
        set {
            //如果重播了，则不计算
            if(mMAXCombo < value && !hasLiquidate) {
                mMAXCombo = value;
            }
        }
    }
    //强制刷新一个值
    public int ForceWriteCombo {
        set { 
			mMAXCombo = value;
			mTotalCombo = value;
			mFingerMaxCombo = value;
			mFingerTotalCombo = value;
		}
    }

	//副本中下载模型提示次数
	public int BattleDownloadCnt;


	/// <summary>
	/// 怒气技能阶段，用户点击的总次数
	/// </summary>
	private int mFingerTotalCombo;
	public int FingerTotalCombo {
		get {
			return mFingerTotalCombo;
		}
		set {
			//如果重播了，则不计算
			if(!hasLiquidate)
				mFingerTotalCombo = value;
		}

	}

	/// <summary>
	/// 怒气技能阶段，用户点击次数最大值
	/// </summary>
	private int mFingerMaxCombo;
	public int FingerMaxCombo {
		get {
			return mFingerMaxCombo;
		}
		set {
			//如果重播了，则不计算
			if(mFingerMaxCombo < value && !hasLiquidate) {
				mFingerMaxCombo = value;
			}
		}
	}


    #endregion


    #region 一场战斗看了有多少个回合
    private int _round;
    public int WatchedhowManyRound {
        get {
            return _round;
        }
        set {
            if(_round < value) {
                _round = value;
            }
        }
    }

    #endregion
    /// <summary>
    /// 表示最近的战斗是否跳过了, 这个也仅仅表示 第一次看战斗的时候有没有跳过
    /// </summary>
    /// <value><c>true</c> if skip battle this turn; otherwise, <c>false</c>.</value>
    private bool _skip;
    public bool SkipBattle {
        get {
            return _skip;
        }
        set {
            if(!hasLiquidate)  {
                _skip = value;
            }
        }
    }

    //是否已经结算过了
    public bool hasLiquidate {
        get;set;
    }


	/// <summary>
	/// 表示最近的战斗是否放弃,
	/// </summary>
	private bool _giveup;
	public bool GiveUpBattle {
		get {
			return _giveup;
		} 
		set {
			if(!hasLiquidate) {
				_giveup = value;
			}
		}
	}

	#region Pvp录像

	//pvp录像的己方攻击力
	public int PvpVideo_Self_Attack = 0;
	//pvp录像敌方的攻击力
	public int PvpVideo_Enemy_Defend = 0;
	//pvp录像的己方等级
	public int PvpVideo_Self_Lv = 0;
	//pvp录像的敌方等级
	public int PvpVideo_Enemy_Lv = 0;
	//己方是攻方还是击防御方  0无效  1攻击  2防御
	public int PvpVideo_AttackOrDefense = 0;
	//己方是否赢了, 0 输了，1 赢了
	public int PvpVideo_SelfWin = 0;
	//和我有关系嘛
	public bool isMyBussiness;
	//己方的名字
	public string self_name;
	//敌方的名字
	public string enemy_name;
	#endregion

	#region 世界boss
	//boss的等级
	public int WorldBoss_lv;
	//boss的攻击力
	public int WorldBoss_Att;
	//获得的积分
	public int WorldPoint;
	#endregion

	#region 沙鲁布欧的试炼

	//敌人的等级
	public int ShaLuBuOu_lv = 0;
	//敌人的攻击力
	public int ShaLuBuOu_Att = 0;
	//自己的攻击力
	public int ShaLuBuOu_Self_Att = 0;
	//攻击方赢 0， 防御方赢1
	public int ShaLuBuOu_WhoWin = 0;
	//己方是攻击方还是防御方  0无效  1攻击  2防御
	public int ShaLuBuOu_AttackOrDefense = 0;
	//己方是赢还是
	public int pass;
	//星数
	public int ShaLuBuOu_Star;
	//多少回合增强属性
	public int ShaLuBuOu_Round1;
	//多少回合有奖励
	public int ShaluBuOu_Round2;
	#endregion

	#region 复仇

	//敌人的等级
	public int Revenge_Lv;
	//敌人的名字
	public string Revenge_Name = null;

	#endregion

	#region 通知事件

	//表示是否应该要去服务器上更新，true代表不要更新, false代表要更新
	private bool mNoticeReady = false;
	public void setNoticeReady(bool ready) { mNoticeReady = ready; }
	public bool getNoticeReady { get { return mNoticeReady; } }

	public void onNetworkCallBack(TimerTask t) {
		ConsoleEx.DebugLog("----------------------- Notice should be getten ---------------------", ConsoleEx.YELLOW);
		setNoticeReady(false);
	}

	#endregion

	public int DragonballNum;
	
	//关闭/打开整个游戏的事件接收功能
	public void SetGameTouch(bool isCanTouch)
	{
		RED.Log("SetGameTouch["+isCanTouch.ToString()+"]");	
		if(DBUIController.mDBUIInstance!=null)
			DBUIController.mDBUIInstance.UITopAndBottomTouch = isCanTouch;
		TempTouch = isCanTouch;
		if(BattleCamera.Instance != null)
			BattleCamera.Instance.UITopAndBottomTouch = isCanTouch;
	}
	
	public bool TempTouch{get;set;}

	public int _TempCurCoin;

	//PVP ENEMY name
	public string _PvpEnemyName;


	//命运转轮剩余时间
	public long leftTimeDestiny;

	#region 整个Client端计算的数据临时存放

	/// <summary>
	/// 普通副本的信息保存
	/// </summary>
	public ClientBattleParam clientReqParam;

	/// <summary>
	/// 沙鲁布欧的请求信息
	/// </summary>
	public NewFinalTrialTeamParam shaluBuOuParam;

	//普通副本的信息保存 和 沙鲁布欧的请求信息
	public ClientBattleResponse clientDataResp;

    //是否开启分步模式
    public bool Open_StepMode;

    //是否开启本地战斗模式
    public bool Open_LocalWarMode;

	#endregion


	#region 新手引导不跳过网络请求

	private int _act = -1;
	public int LetGoACT {
		get {
			int temp = _act;
			_act = -1;
			return temp;
		}
		set {
			_act = value;
		}
	}

	#endregion
	
	
	
	#region JCPVETimerManager倒计时临时变量
	public long ExpDJS = 0;
	public long GemDJS = 0;
	public long SkillDJS = 0;
	public long FightSoulDJS = 0;
	#endregion
	


	#region 当前选择的monster
	public Monster curShowMonster = null;

	public Equipment curSelectEquip = null;

	public Monster bagSelectMonster = null;
	#endregion
	
}