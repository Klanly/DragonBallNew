using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using RUIType;

#region 沙鲁的游戏 和 布欧的游戏 数据部分
public class NewMapFinalTrial
{
	public NewFloorState State;
	public MapFinalTrialData Data;
	public int Starnum;

	public NewMapFinalTrial(NewFloorState _State, MapFinalTrialData _data, int _Starnum)
	{
		State = _State;
		Data = _data;
		Starnum = _Starnum;
	}

	public Vector3 localPosition
	{
		get {return new Vector3(Data.Pos[0],-Data.Pos[1],0);}
	}
}

public class FinalTrialData
{
    bool mIsFight ;
    public bool IsFight{
        get{
            return mIsFight;
        }
        set{
            mIsFight = value;
        }
    }

    int mMostStar;
    public int MostStar{
        get{
            return mMostStar;
        }
        set{
            mMostStar = value;
        }
    }

    int mAddPercent;
    public int AddPercent{
        get{
            return mAddPercent;
        }
        set{
            mAddPercent = value;
        }
    }

    int mSelfRank;
    public int SelfRank{
        get{
            return mSelfRank;
        }
        set{
            mSelfRank = value;
        }
    }

    int mCurChallengeNum;
    public int CurChallengeNum{
        get{
            return mCurChallengeNum;
        }
        set{
            mCurChallengeNum = value;
        }
    }

	int mRemainChallengeNum;
	public int RemainChallengeNum{
		get{
			return mRemainChallengeNum;
		}
		set{
			mRemainChallengeNum = value;
		}
	}

    int mBestScore;
    public int BestScore{
        get{
            return mBestScore;
        }
        set{
            mBestScore = value;
        }
    }

    bool mIsInDungeon;
    public bool IsInDungeon{
        get{
            return mIsInDungeon;
        }
        set{
            mIsInDungeon = value;
        }
    }

    int mHighestDungeonNum;
    public int HighestDungeonNum{
        get{
            return mHighestDungeonNum;
        }
        set{
            mHighestDungeonNum = value;
        }
    }

	int mSelfMonNum;
	public int SelfMonNum{
		get{
			return mSelfMonNum;
		}
		set{
			mSelfMonNum = value;
		}
	}

    int mHistoryDungeonNum;
    public int HistoryDungeonNum{
        get{
            return mHistoryDungeonNum;
        }
        set{
            mHistoryDungeonNum = value;
        }
    }

    int mCurDungeon;
    public int CurDungeon{
        get{
            return mCurDungeon;
        }
        set{
            mCurDungeon = value;
        }
    }

    FinalTrialAdditionType mAdditionType;
    public FinalTrialAdditionType AdditionType{
        get{
            return mAdditionType;
        }
        set{
            mAdditionType = value;
        }
    }

    bool mIsTreasure;
    public bool  IsTreasure{
        get{
            return mIsTreasure;
        }
        set{
            mIsTreasure = value;
        }
    }

    List<int> mPetList = new List<int>() ;
    public List<int> PetList{
        get{
            return mPetList;
        }
        set{
            mPetList = value;
        }
    }

    List<int> mPetListCount = new List<int>() ;
    public List<int> PetListCount{
        get{
            return mPetListCount;
        }
        set{
            mPetListCount = value;
        }
    }

    List<int> mAddPrecentList = new List<int>(); 
    public List<int> AddPrecentList{
        get{
            return mAddPrecentList;
        }
        set{
            mAddPrecentList = value;
        }
    }

    bool mIsHaveAdd;
    public bool IsHaveAdd{
        get{
            return mIsHaveAdd;
        }
        set{
            mIsHaveAdd = value;
        }
    }

	bool mIsFirst;
	public bool IsFirst{
		get{
			return mIsFirst;
		}
		set{
			mIsFirst = value;
		}
	}

	FinalTrialAtcOrDef mFinalTrialAtcOrDef;
	public FinalTrialAtcOrDef finalTrialAtcOrDef
	{
		get{
			return mFinalTrialAtcOrDef;
		}
		set{
			mFinalTrialAtcOrDef = value;
		}
	}

	GetFinalTrialStateData mGetFinalTrialStateDataShalu;
	public GetFinalTrialStateData getFinalTrialStateDataShalu
	{
		get{
			return mGetFinalTrialStateDataShalu;
		}
		set{
			mGetFinalTrialStateDataShalu = value;
		}
	}

	GetFinalTrialStateData mGetFinalTrialStateDataBuou;
	public GetFinalTrialStateData getFinalTrialStateDataBuou
	{
		get{
			return mGetFinalTrialStateDataBuou;
		}
		set{
			mGetFinalTrialStateDataBuou = value;
		}
	}

	GetChallengeRank mGetChallengeRank;
	public GetChallengeRank m_GetChallengeRank
	{
		get{
			return mGetChallengeRank;
		}
		set{
			mGetChallengeRank = value;
		}
	}

    public int[] mChooseAddAtr;

	public int[] mChooseAddAtrStar;

    public FinalTrialData()
    {
        mChooseAddAtr = new int[6];
		mChooseAddAtrStar = new int[6];
        IsHaveAdd = true;
	}
    
}

public enum BattleVideoTagType
{
	Type_None = 0,
	Type_Share = 1,
	Type_PlayBack = 2,
}

public enum DuihuanFromType
{
	Type_None = 0,
	Type_Zhaomu = 1,
	Type_Duobao = 2,
}

#endregion

public class FinalTrialMgr 
{
	public int m_nDownLoadTipCnt;
	static FinalTrialMgr mFinalTrialMgr;
	
	public static FinalTrialMgr GetInstance()
	{
		if (mFinalTrialMgr == null) 
		{
			mFinalTrialMgr = new FinalTrialMgr();
		}
		return mFinalTrialMgr;
	}

	private FinalTrialMgr()
	{
		_IsNullPlayer = false;
	}

    private const string mStr = "" ;

    private const int MaxDungeonNum = 9;
    public bool[] mIsDungeonOpenArray = new bool[12];
    public bool[] mIsBoxOrDungeonArray = new bool[12];

	//新地图信息结构的列表
	public List<NewMapFinalTrial> mNewMapFinalTrialList = new List<NewMapFinalTrial>();

	public UIPVPPlotBuilding mCurUIPVPPlotBuilding = null;

	public int EnemyIndex;

	public bool IsFinalTrialToTeam = false;

	public NewFinalTrialFightMapInfo _NewFinalTrialFightMapInfo;

	public NewFinalTrialFightResponse _NewFinalTrialFightResponse;

	public NewFinalTrialAddBufferInfo _buffer;

    public FinalTrialData _FinalTrialData = new FinalTrialData();

	public bool CheckShaluEnter;
	public bool CheckBuOuEnter;

	public bool IsInFloorShalu;
	public bool IsInFloorBuou;

	GetDuoBaoLoginInfoData m_allPVPRobData;
    public GetDuoBaoLoginInfoData allPVPRobData
	{
		set{
			m_allPVPRobData = value;
		}
		get{
			return m_allPVPRobData;
		}
	}

	public DuihuanFromType m_DuihuanFromType;

	//究极试炼类型
    TrialEnum mNowEnum;

	//抢夺挑战神龙2级界面的type
	QiangduoEnum qiangduoEnum;

    public TrialEnum NowEnum{
        get{
            return mNowEnum;
        }
        set{
            mNowEnum = value;
        }
    }

	public QiangduoEnum m_QiangduoEnum{
		get{
			return qiangduoEnum;
		}
		set{
			qiangduoEnum = value;
		}
	}

	//脚本对象部分
    UITrailEnter _UITrailEnter = null;
    UITrialMap _UITrialMap = null;
    UITrailAddAttribute _UITrailAddAttribute  = null;
	UITrialMapNotAdd _UITrialMapNotAdd = null;

	[HideInInspector]
	public UITrialAlternative _UITrialAlternative = null;

	UIMapOfFinalTrial _UIMapOfFinalTrial = null;
	UIPVPBuildDes _UIPVPBuildDes = null;

	[HideInInspector]
	public PvpShaluBuouRoot _PvpShaluBuouRoot = null;
	FinalTrial3D _FinalTrial3D = null;

	public PvpShaluBuouRoot m_PvpShaluBuouRoot
	{
		get{
			if(_PvpShaluBuouRoot == null)
			{
				_PvpShaluBuouRoot = PvpShaluBuouRoot.OpenUI();
			}
			return _PvpShaluBuouRoot;
		}
	}

	public UIPVPBuildDes mUIPVPBuildDes
	{
		get{
			if(_UIPVPBuildDes == null)
			{
				_UIPVPBuildDes = UIPVPBuildDes.CreatePanel();
			}
			return _UIPVPBuildDes;
		}
	}

	public UIMapOfFinalTrial mUIMapOfFinalTrial
	{
		get{
			if(_UIMapOfFinalTrial == null)
			{
				_UIMapOfFinalTrial = UIMapOfFinalTrial.CreatePanel();
			}
			return _UIMapOfFinalTrial;
		}
	}

	public UITrialAlternative mUITrialAlternative
	{
		get{
			if(_UITrialAlternative == null)
			{
				_UITrialAlternative = UITrialAlternative.CreatePanel();
			}
			return _UITrialAlternative;
		}
	}

	public UITrailAddAttribute mUITrailAddAttribute
	{
		get{
			if(_UITrailAddAttribute == null)
			{
				_UITrailAddAttribute = TrialAttributePanel.CreateShangChengPanel();
				RED.AddChild(_UITrailAddAttribute.gameObject,DBUIController.mDBUIInstance._bottomRoot);
			}
			return _UITrailAddAttribute;
		}
	}

	public FinalTrial3D mFinalTrial3D
	{
		get{
			if(_FinalTrial3D == null)
			{
				_FinalTrial3D = FinalTrialMap3D.Create3DModal();
			}
			return _FinalTrial3D;
		}
    }
    
    public UITrailEnter mUITrailEnter    
    {
        get{
            if(_UITrailEnter == null)
            {
                _UITrailEnter = TrialEnterPanel.CreateShangChengPanel();
                RED.AddChild(_UITrailEnter.gameObject,DBUIController.mDBUIInstance._bottomRoot);
            }
            return _UITrailEnter;
        }
    }

    public UITrialMap mUITrialMap    
    {
        get{
            if(_UITrialMap == null)
            {
                _UITrialMap = TrialMapPanel.CreateShangChengPanel();
                RED.AddChild(_UITrialMap.gameObject,DBUIController.mDBUIInstance._bottomRoot);
            }
            return _UITrialMap;
        }
    }

	public UITrialMapNotAdd mUITrialMapNotAdd
	{
		get{
			if(_UITrialMapNotAdd == null)
			{
				_UITrialMapNotAdd = TrialMapNotAttrPanel.CreateShangChengPanel();
				RED.AddChild(_UITrialMapNotAdd.gameObject,DBUIController.mDBUIInstance._bottomRoot);
			}
			return _UITrialMapNotAdd;
		}
	}

	public bool _IsNullPlayer;
	
	public QiangDuoPanelScript qiangDuoPanelScript = null;

	
	public QiangDuoGoldOpponentsInfo qiangDuoGoldOpponentsInfo;
	
	public TianXiaDiYiInfo tianXiaDiYiInfo;
	
	public Queue<FightOpponentInfo> TianXiaRoleQueue = new Queue<FightOpponentInfo>();
	
	public Queue<FightOpponentInfo> QiangduoRoleQueue = new Queue<FightOpponentInfo>();

	public Queue<FightOpponentInfo> sudiDataQueue = new Queue<FightOpponentInfo>(); //宿敌数据队列。。。。
	public List<FightOpponentInfo> suDiTempList = new List<FightOpponentInfo>(); // 用于当前列表显示的宿敌据缓存，当删除某个时使用。
	
	public Action<TianXiaDiYiInfo> getTianXiaDiYiOpponentsCompletedDelegate;
	
	public FightOpponentInfo currentFightOpponentInfo;
	
	public List<FightOpponentInfo> currentQiangDuoDragonBallList = new List<FightOpponentInfo>();
	
	public BattleVideoPlaybackData[]  m_BattleVideoPlaybackdata;
	
	public GameObject BattleVideoroot;
	
	public BattleResponse BattleVideoPlaybackRes;
	public TemporyData.BattleType BattleVideoPlaybackType;
	
	public UIShareVideoTag mUIShareVideoTag;
	
	//抢夺外部调用回调
	public delegate void BackCallBack();
	public BackCallBack _BackCallBack;
	
	public delegate void QiangduoDuihuanBack();
	public QiangduoDuihuanBack _QiangduoDuihuanBack;
	
	//任务回调
	public delegate void MissionBackCallBack();
	public MissionBackCallBack _MissionBackCallBack;
	
	public int ZhanGongTotal = 0;
	
	public int TotalJifen = 0;
	
	public int QiangduoCoin = 0;
	
	public int YetCount = 0;
	
	public int[] HaveLingqu;
	
	public EMViewState _EMViewState = EMViewState.NONE ;
	
	public int Now_Zhangong;
	
	public int EveryMinZhangong;
	
	public bool IsFromDuoBao = true;
	
	public int QiangduoRefreshCount;

	public bool m_IsBeginFight = false;

	public int m_LastTaskId = 0;
	public int m_NowTaskId = 1;

	public int ShaluBuouResetState = -1;  //沙鲁布欧重置的类型  1 是沙鲁  2是布欧

	public long m_PVPBallLeftTime = 0;
//	long m_temprank;
	public long m_PVPRankLeftTime= 0;

	public long m_PVPRobLeftTime = 0;
	public long m_PVPRevengeLeftTime = 0;

	public int m_RefreshZhangongMoney;
	public int m_ZhangongsurplusRefreshTimes;
	public int m_ZhangongRefreshMaxTimes;
	public int m_ZhangongRefreshMoneyType;

	public UIDuiHuanCell m_SelectDuihuancell = null;

	#region 试炼4选一界面所有数据请求
    public TrialEnum jumpTo = TrialEnum.None;
	bool isAllDataCompleted = false;

	public void getAllData() 
	{
		isAllDataCompleted = false;
		GetAllDataRequest();
	}

	public void GetAllDataRequest()
	{
		ComLoading.Open();
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.FINALTRIAL_GETFINALLOGININFO, new FinalTrialLoginInfoParam(Core.Data.playerManager.PlayerID));
		
		task.afterCompleted = getAllDateCompleted;
		task.ErrorOccured = getAllDateError;
		
		task.DispatchToRealHandler ();
		
	}
	
	void getAllDateCompleted(BaseHttpRequest request, BaseResponse response)
	{
		HttpRequest httpRequest = request as HttpRequest;
		if(response != null && response.status != BaseResponse.ERROR)
		{
			//新的究极试炼界面的信息
			if(httpRequest.Act == HttpRequestFactory.ACTION_FINALTRIAL_GETFINALLOGININFO)
			{
				isAllDataCompleted = true;
				GetDuoBaoLoginInfoResponse res = response as GetDuoBaoLoginInfoResponse;
				
				SaveAllDataStatus(res.data);
				
				this.GetFinalTrialShaluBuouDataCompleted(null, response);
				DBUIController.mDBUIInstance.mDuoBaoView.tianXiaDiYiRankLabel.text = res.data.rankStatus.rank.ToString();
				EveryMinZhangong = res.data.rankStatus.recozg;

            }
            if(isAllDataCompleted)
            {
                DBUIController.mDBUIInstance.mDuoBaoView.show();
                if(jumpTo != TrialEnum.None)
                {
					if(m_QiangduoEnum == QiangduoEnum.QiangduoEnum_None)
					{
						CreateScript(jumpTo, QiangduoEnum.QiangduoEnum_List);
					}
					else
					{
						CreateScript(jumpTo, m_QiangduoEnum);
					}

                }
				if (m_LastTaskId == m_NowTaskId)
				{
					if(Core.Data.guideManger.isGuiding)
					{
						Core.Data.guideManger.AutoRUN();
					}
				}

            }

            
        }
        ComLoading.Close();
    }

	void SaveAllDataStatus( GetDuoBaoLoginInfoData data)
	{
		if(Core.Data.guideManger.isGuiding)
		{
			if(m_NowTaskId != m_LastTaskId)
			{
				return;
			}
		}
		if(allPVPRobData == null)
		{
			allPVPRobData = new GetDuoBaoLoginInfoData();
		}
		if(data.atkStatus != null)
			allPVPRobData.atkStatus = data.atkStatus;
		if(data.defStatus != null)
			allPVPRobData.defStatus = data.defStatus;
		if(data.pvpStatus != null)
			allPVPRobData.pvpStatus = data.pvpStatus;
		if (data.rankStatus != null)
			allPVPRobData.rankStatus = data.rankStatus;
		if (data.robStatus != null)
			allPVPRobData.robStatus = data.robStatus;
        
        if(data.pvpStatus != null && data.pvpStatus.revenge != null)
		{
			if(Core.Data.playerManager.revengeData == null)
			{
				Core.Data.playerManager.revengeData = new RevengeProgressData();
				Core.Data.playerManager.revengeData.curProgress = data.pvpStatus.revenge.passCount;
				Core.Data.playerManager.revengeData.maxProgress = data.pvpStatus.revenge.count;
				if(data.pvpStatus.revenge.count <= data.pvpStatus.revenge.passCount)
				{
					Core.Data.playerManager.revengeData.needStone = data.pvpStatus.revenge.needStone;
				}
				else
				{
					Core.Data.playerManager.revengeData.needStone = 0;
				}
			}
			else
			{
				Core.Data.playerManager.revengeData.curProgress = data.pvpStatus.revenge.passCount;
				Core.Data.playerManager.revengeData.maxProgress = data.pvpStatus.revenge.count;
				if(data.pvpStatus.revenge.count <= data.pvpStatus.revenge.passCount)
				{
					Core.Data.playerManager.revengeData.needStone = data.pvpStatus.revenge.needStone;
				}
				else
				{
					Core.Data.playerManager.revengeData.needStone = 0;
				}
			}
			
		}
		SetPVPCoolTime();
	

    }

	public void GetFinalTrialShaluBuouDataCompleted(BaseHttpRequest request, BaseResponse response)
	{
		GetDuoBaoLoginInfoResponse res = response as GetDuoBaoLoginInfoResponse;
		if(res != null)
		{
			if((res.data.atkStatus.totalCout-res.data.atkStatus.yetCount) <= 0)
			{
				CheckShaluEnter = false;
			}
			if((res.data.defStatus.totalCout-res.data.defStatus.yetCount) <= 0)
			{
				CheckBuOuEnter = false;
			}
			if((res.data.atkStatus.totalCout-res.data.atkStatus.yetCount) > 0)
			{
				CheckShaluEnter = true;
			}
			if((res.data.defStatus.totalCout-res.data.defStatus.yetCount) > 0)
            {
                CheckBuOuEnter = true;
            }
            DBUIController.mDBUIInstance.mDuoBaoView.SetShaluBuouData(res);
        }
    }
    #endregion
    

	
	#region 创建究极试炼的各个界面
	public void CreateScript(TrialEnum m_TrialEnum, QiangduoEnum m_QiangduoEnum)
	{	
		switch(m_TrialEnum)
		{
			case TrialEnum.TrialType_Title:
				break;
			case TrialEnum.TrialType_TianXiaDiYi:
				NowEnum = m_TrialEnum;
				if(Core.Data.playerManager.RTData.curLevel < 10)
				{
					return;
				}
				RequestByQiangduoType(m_QiangduoEnum);
				break;
			case TrialEnum.TrialType_QiangDuoGold:
				NowEnum = m_TrialEnum;
				if (Core.Data.playerManager.RTData.curLevel < 20)
				{
					return;
				}
				RequestByQiangduoType(m_QiangduoEnum);
				break;
			case TrialEnum.TrialType_QiangDuoDragonBall:
				NowEnum = m_TrialEnum;
				RequestByQiangduoType(m_QiangduoEnum);
            break;
        case TrialEnum.TrialType_ShaluAndBuou:
            if(Core.Data.playerManager.RTData.curLevel < 20 )
            {
                return;
            }
            NewFinalTrialStateRequest();
            break;
        }
    }
    #endregion
    
    
	#region 沙鲁的游戏 和 布欧的游戏

	//沙鲁布欧地图取金币值
	public int GetMapComboCoin(TrialEnum _TrialEnum) {
		int m_curlayer = this._FinalTrialData.CurDungeon;
		if(_TrialEnum == TrialEnum.TrialType_PuWuChoose)
			m_curlayer += 0xE;
		else
			m_curlayer -= 1;
		
		int _Coin = 0;
		if(mNewMapFinalTrialList != null && mNewMapFinalTrialList.Count != 0) {
			if(m_curlayer < mNewMapFinalTrialList.Count && m_curlayer >= 0)
				_Coin = mNewMapFinalTrialList[m_curlayer].Data.Cprice;
        }
        return _Coin;
    }

	public void SetShaluBuouStatus(int m_iswin)
	{
		if(m_iswin == 1)
		{
			if(_FinalTrialData.CurDungeon <= 15)
			{
				if(NowEnum == TrialEnum.TrialType_ShaLuChoose)
				{
					_FinalTrialData.getFinalTrialStateDataShalu.cLayer += 1;
				}
				else if(NowEnum == TrialEnum.TrialType_PuWuChoose)
				{
					_FinalTrialData.getFinalTrialStateDataBuou.cLayer += 1;
				}
			}
		}
		else 
		{
			if(NowEnum == TrialEnum.TrialType_ShaLuChoose)
			{
                _FinalTrialData.getFinalTrialStateDataShalu.fCount += 1;
            }
            else if(NowEnum == TrialEnum.TrialType_PuWuChoose)
            {
                _FinalTrialData.getFinalTrialStateDataBuou.fCount += 1;
            }
        }
	}

	public void OpenNewMap(int m_type)
	{
		if(m_type == 1)
		{
			NowEnum = TrialEnum.TrialType_ShaLuChoose;
			_FinalTrialData.finalTrialAtcOrDef = FinalTrialAtcOrDef.FinalTrialAtcOrDef_Attack;
			_FinalTrialData.CurDungeon = _FinalTrialData.getFinalTrialStateDataShalu.cLayer;
			_FinalTrialData.CurChallengeNum = _FinalTrialData.getFinalTrialStateDataShalu.fCount;
			_FinalTrialData.RemainChallengeNum = _FinalTrialData.getFinalTrialStateDataShalu.count - _FinalTrialData.getFinalTrialStateDataShalu.fCount;
		}
		else if(m_type == 2)
		{
			NowEnum = TrialEnum.TrialType_PuWuChoose;
			_FinalTrialData.finalTrialAtcOrDef = FinalTrialAtcOrDef.FinalTrialAtcOrDef_Defense;
			_FinalTrialData.CurDungeon = _FinalTrialData.getFinalTrialStateDataBuou.cLayer;
			_FinalTrialData.CurChallengeNum = _FinalTrialData.getFinalTrialStateDataBuou.fCount;
			_FinalTrialData.RemainChallengeNum = _FinalTrialData.getFinalTrialStateDataBuou.count - _FinalTrialData.getFinalTrialStateDataBuou.fCount;
		}
		InitNewMapInfo();
        m_PvpShaluBuouRoot.SetMapDetail(mNewMapFinalTrialList);
        DBUIController.mDBUIInstance.mDuoBaoView.SetActive(false);
    }	

	//初始化地图信息
	void InitNewMapInfo()
	{
		mNewMapFinalTrialList.Clear();
		List<MapFinalTrialData> m_temp = new List<MapFinalTrialData>();
		if(mNowEnum == TrialEnum.TrialType_ShaLuChoose)
		{
			m_temp = Core.Data.FinalTrialDataMgr.GetShaluOrBuouList(0);
		}
		else if(mNowEnum == TrialEnum.TrialType_PuWuChoose)
		{
			m_temp = Core.Data.FinalTrialDataMgr.GetShaluOrBuouList(1);
		}
		
		for(int i=0; i<m_temp.Count; i++)
		{
			if(i+1 < _FinalTrialData.CurDungeon)mNewMapFinalTrialList.Add(new NewMapFinalTrial(NewFloorState.Pass, m_temp[i], 0));
			else if(i+1 == _FinalTrialData.CurDungeon)
			{
				mNewMapFinalTrialList.Add(new NewMapFinalTrial(NewFloorState.Current, m_temp[i], 0));
			}
			else if(i+1 > _FinalTrialData.CurDungeon)
            {
                mNewMapFinalTrialList.Add(new NewMapFinalTrial(NewFloorState.Unlocked, m_temp[i], 0));
            }	
        }
    }

	//沙鲁布欧次数重置
    public void ResetFinalTrialValueRequest(int _type)
	{
		ShaluBuouResetState = _type;
		ComLoading.Open();
		ResetFinalTrialValueParam param = new ResetFinalTrialValueParam(Core.Data.playerManager.PlayerID, _type);
		HttpTask task = new HttpTask(ThreadType.MainThread,TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.RESETFINALTRIALVALUE, param);
		
		task.afterCompleted += SetNewFinalTrialData;
		task.DispatchToRealHandler();
	}

    //查看当前层状态
    public void NewFinalTrialCurDungeonRequest(UIPVPPlotBuilding m_Script)
	{
		ComLoading.Open();
		mCurUIPVPPlotBuilding = m_Script;
		NewFinalTrialCurDungeonParam param = new NewFinalTrialCurDungeonParam(int.Parse(Core.Data.playerManager.PlayerID), m_Script.mNewMapFinalTrial.Data.ID, (int)NowEnum);
		HttpTask task = new HttpTask(ThreadType.MainThread,TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.NEW_FINALTRIAL_CURDUNGEON, param);
		
		task.afterCompleted += SetNewFinalTrialData;
		task.DispatchToRealHandler();
	}
	
	//获取守关队伍数据
	public void NewFinalTrialCurTeamRequest(int m_id, int[] array, int teamid)
	{
		ComLoading.Open();
		NewFinalTrialTeamParam param = new NewFinalTrialTeamParam(int.Parse(Core.Data.playerManager.PlayerID), m_id, (int)NowEnum, array, teamid);
		HttpTask task = new HttpTask(ThreadType.MainThread,TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.NEW_FINALTRIAL_TEAM, param);
        
        task.afterCompleted += SetNewFinalTrialData;
        task.DispatchToRealHandler();
    }
    
	//查看玩家远征状态
	public void NewFinalTrialStateRequest()
	{
		ComLoading.Open();
		NewFinalTrialStateParam param = new NewFinalTrialStateParam(int.Parse(Core.Data.playerManager.PlayerID));
		HttpTask task = new HttpTask(ThreadType.MainThread,TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.NEW_FINALTRIAL_STATE, param);
		
		task.afterCompleted += SetNewFinalTrialData;
        task.DispatchToRealHandler();
    }
    
	
	//沙鲁布欧返回处理
	void SetNewFinalTrialData(BaseHttpRequest request, BaseResponse response)
	{
		ComLoading.Close();
		if(response != null && response.status != BaseResponse.ERROR)
		{
			HttpRequest httprequest = request as HttpRequest;
			if(httprequest.Act == HttpRequestFactory.ACTION_NEW_FINALTRIAL_STATE)
			{
				GetFinalTrialStateResponse res = response as GetFinalTrialStateResponse;
				_FinalTrialData.getFinalTrialStateDataShalu = res.data.shalu;
				_FinalTrialData.getFinalTrialStateDataBuou = res.data.buou;
				mUITrialAlternative.OnShow(res.data);
			}
			else if(httprequest.Act == HttpRequestFactory.ACTION_NEW_FINALTRIAL_CURDUNGEON)
			{
				GetFinalTrialCurDungeonResponse res = response as GetFinalTrialCurDungeonResponse;
				
				m_PvpShaluBuouRoot.SetBuildDes(mCurUIPVPPlotBuilding.mNewMapFinalTrial, res.data.defName, res.data.members);
			}
			else if(httprequest.Act == HttpRequestFactory.ACTION_NEW_FINALTRIAL_TEAM)
			{
				ClientBattleResponse res = response as ClientBattleResponse;
				if(res != null) 
				{ 
					if(NowEnum == TrialEnum.TrialType_ShaLuChoose)Core.Data.temper.currentBattleType = TemporyData.BattleType.FinalTrialShalu;
					else Core.Data.temper.currentBattleType = TemporyData.BattleType.FinalTrialBuou;
					
					TemporyData temp = Core.Data.temper;
					if(NowEnum == TrialEnum.TrialType_ShaLuChoose)temp.currentBattleType = TemporyData.BattleType.FinalTrialShalu;
					else temp.currentBattleType = TemporyData.BattleType.FinalTrialBuou;
					temp.clientDataResp = res;
					
					#if LOCAL_AUTO
					temp.Open_StepMode = false;
					#else
					temp.Open_StepMode = true;
					#endif
					temp.Open_LocalWarMode = true;
					
					HttpRequest req = request as HttpRequest;
					if(req != null) {
						NewFinalTrialTeamParam param = req.ParamMem as NewFinalTrialTeamParam;
                        if(param != null)
                            temp.shaluBuOuParam = param;
                    }
                    
                    this.jumpTo = TrialEnum.None;
                    JumpToBattleView();
                    
                }
            }
			else if(httprequest.Act == HttpRequestFactory.ACTION_RESETFINALTRIALVALUE)
			{
				GetFinalTrialStateResponse res = response as GetFinalTrialStateResponse;
				_FinalTrialData.getFinalTrialStateDataShalu = res.data.shalu;
				_FinalTrialData.getFinalTrialStateDataBuou = res.data.buou;
				mUITrialAlternative.OnShow(res.data);
			}
        }
        else if(response != null && response.status == BaseResponse.ERROR)
        {
            SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
        }
        
    }

	void JumpToBattleView() 
	{
		BattleToUIInfo.From = RUIType.EMViewState.S_QiangDuo;
		Core.Data.temper.CitySence = mCurUIPVPPlotBuilding.mNewMapFinalTrial.Data.Scence;
		Core.SM.beforeLoadLevel(Application.loadedLevelName, SceneName.GAME_BATTLE);
		AsyncLoadScene.m_Instance.LoadScene(SceneName.GAME_BATTLE);
    }
    
	#endregion
        
	#region 挑战抢夺龙珠根据type请求2级界面部分
	public void RequestByQiangduoType(QiangduoEnum m_QiangduoEnum)
	{
		this.m_QiangduoEnum = m_QiangduoEnum;
		if(mNowEnum == TrialEnum.TrialType_TianXiaDiYi)
		{
			if(m_QiangduoEnum == QiangduoEnum.QiangduoEnum_List)
			{
				tianXiaDiYiRequest( getTianXiaDiYiCompleted, getTianXiaDiYiOpponentsError);
            }
			else if(m_QiangduoEnum == QiangduoEnum.QiangduoEnum_Sudi) 
			{
				getSuDiRequest(2);
			}
			else if(m_QiangduoEnum == QiangduoEnum.QiangduoEnum_Duihuan)
			{
				getBuyItemID();
			}
			else if(m_QiangduoEnum == QiangduoEnum.QiangduoEnum_Playback)
            {
				BattleVideoRequest();
            }
        }
		else if(mNowEnum == TrialEnum.TrialType_QiangDuoGold)
		{
			if(m_QiangduoEnum == QiangduoEnum.QiangduoEnum_List)
			{
				getQiangDuoGoldOpponentsRequest( getQiangDuoGoldOpponentsCompleted, getQiangDuoGoldOpponentsError);
			}
			else if(m_QiangduoEnum == QiangduoEnum.QiangduoEnum_Sudi) 
			{
				getSuDiRequest(3);
			}
			else if(m_QiangduoEnum == QiangduoEnum.QiangduoEnum_Duihuan)
			{
				getQiangDuoGoldTotalRequest();
			}
			else if(m_QiangduoEnum == QiangduoEnum.QiangduoEnum_Playback)
			{
				BattleVideoRequest();
			}
		}
		else if(mNowEnum == TrialEnum.TrialType_QiangDuoDragonBall)
		{
			if(m_QiangduoEnum == QiangduoEnum.QiangduoEnum_List)
			{
				ShowQiangDuoPanelScript(QiangduoEnum.QiangduoEnum_List);
			}
			else if(m_QiangduoEnum == QiangduoEnum.QiangduoEnum_Sudi) 
			{
				getSuDiRequest(1);
			}
			else if(m_QiangduoEnum == QiangduoEnum.QiangduoEnum_Duihuan)
			{
				;
			}
			else if(m_QiangduoEnum == QiangduoEnum.QiangduoEnum_Playback)
			{
				BattleVideoRequest();
			}
		}
	}

	void ShowQiangDuoPanelScript(QiangduoEnum m_QiangduoEnum)
	{
		UIMiniPlayerController.ElementShowArray = new bool[]{true,true,false,true,true};
		DBUIController.mDBUIInstance.HiddenFor3D_UI();
		UIMiniPlayerController.Instance.SetActive(true);

		if(qiangDuoPanelScript == null)
		{
			qiangDuoPanelScript = QiangDuoPanelScript.CreateQiangDuoPanel();
			qiangDuoPanelScript.transform.parent = DBUIController.mDBUIInstance._bottomRoot.transform;
			qiangDuoPanelScript.transform.localScale = Vector3.one;
		}
		
		if(qiangDuoPanelScript != null)
		{
			qiangDuoPanelScript.ChangeShowType(m_QiangduoEnum);
        }
        DBUIController.mDBUIInstance.RefreshUserInfo ();
	}

	//天下第一挑战界面list
	void tianXiaDiYiRequest(Action<BaseHttpRequest, BaseResponse> afterCompleted, Action<BaseHttpRequest, string> ErrorOccured)
	{
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		int isGuide = System.Convert.ToInt32( Core.Data.guideManger.isGuiding );
		task.AppendCommonParam(RequestType.GET_TIANXIADIYI_OPPONENTS, new GetTianXiaDiYiOpponentsParam(Core.Data.playerManager.PlayerID,isGuide));
		task.afterCompleted += afterCompleted;
		task.ErrorOccured += ErrorOccured;
		task.DispatchToRealHandler ();
		ComLoading.Open();
	}

	public void getTianXiaDiYiCompleted(BaseHttpRequest request, BaseResponse response)
	{
		if(response != null && response.status != BaseResponse.ERROR) 
		{
			GetTianXiaDiYiOpponentsResponse getTianXiaDiYiOpponentsResponse = response as GetTianXiaDiYiOpponentsResponse;
			if(getTianXiaDiYiOpponentsResponse.data.roles != null &&getTianXiaDiYiOpponentsResponse.data.roles.Length == 0)_IsNullPlayer = true;
			Now_Zhangong = getTianXiaDiYiOpponentsResponse.data.userzg;
			if(this._BackCallBack != null)
			{
				getBuyItemID();
				this.getTianXiaDiYiOpponentsCompleted(getTianXiaDiYiOpponentsResponse.data);
			}
			else
			{
				this.getTianXiaDiYiOpponentsCompleted(getTianXiaDiYiOpponentsResponse.data);
				if(getTianXiaDiYiOpponentsCompletedDelegate != null)
				{
					getTianXiaDiYiOpponentsCompletedDelegate(getTianXiaDiYiOpponentsResponse.data);
				}
				if(this.IsFromDuoBao)
				{
					if(Core.Data.guideManger.isGuiding)
                    {
                        Core.Data.guideManger.AutoRUN();
						this.IsFromDuoBao = false;
                    }
                }

				this.ShowQiangDuoPanelScript(this.m_QiangduoEnum);
                ComLoading.Close();
            }
            
        }
	}

	public void getTianXiaDiYiOpponentsError(BaseHttpRequest request, string error)
	{
		ComLoading.Close();
    }

	public void getTianXiaDiYiOpponentsCompleted(TianXiaDiYiInfo tianXiaDiYiInfo)
	{
		this.tianXiaDiYiInfo = tianXiaDiYiInfo;
		TianXiaRoleQueue.Clear();
		for(int i=0; i<this.tianXiaDiYiInfo.roles.Length; i++)
		{
			TianXiaRoleQueue.Enqueue(tianXiaDiYiInfo.roles[i]);
		}
		sortFightOpponentInfoByRank(this.tianXiaDiYiInfo.roles);
	}
    
	//抢夺金币战界面list
	void getQiangDuoGoldOpponentsRequest(Action<BaseHttpRequest, BaseResponse> afterCompleted, Action<BaseHttpRequest, string> ErrorOccured)
	{
		
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.GET_QIANGDUO_GOLD_OPPONENTS, new GetQiangDuoGlodOpponentsParam(Core.Data.playerManager.PlayerID));
		
		task.afterCompleted = afterCompleted;
		task.ErrorOccured = ErrorOccured;
		
		task.DispatchToRealHandler ();
		ComLoading.Open();
	}

	public void getQiangDuoGoldOpponentsCompleted(BaseHttpRequest request, BaseResponse response)
	{
		if(response != null && response.status != BaseResponse.ERROR) 
		{
			GetQiangDuoGoldOpponentsResponse getQiangDuoGoldOpponentsRequest = response as GetQiangDuoGoldOpponentsResponse;
			TotalJifen = getQiangDuoGoldOpponentsRequest.data.status.score;
			QiangduoCoin = getQiangDuoGoldOpponentsRequest.data.status.robcoins;
			YetCount = getQiangDuoGoldOpponentsRequest.data.yetcount;
			if(_QiangduoDuihuanBack != null)
			{
				getQiangDuoGoldTotalRequest();
			}
			else
			{
				ComLoading.Close();
				if(getQiangDuoGoldOpponentsRequest.data.player != null &&getQiangDuoGoldOpponentsRequest.data.player.Length == 0)
				{
					_IsNullPlayer = true;
				}
				this.qiangDuoGoldOpponentsInfo = getQiangDuoGoldOpponentsRequest.data;
				
				this.sortFightOpponentInfoByRank(this.qiangDuoGoldOpponentsInfo.player);
				
				QiangduoRoleQueue.Clear();
				for(int i=0; i<this.qiangDuoGoldOpponentsInfo.player.Length; i++)
				{
					QiangduoRoleQueue.Enqueue(qiangDuoGoldOpponentsInfo.player[i]);
				}
				Core.Data.AccountMgr.SaveFinalTrialQiangduoQueue();
				this.ShowQiangDuoPanelScript(this.m_QiangduoEnum);
            }
        }
        else if (response != null && response.status == BaseResponse.ERROR)
        {
            SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
        }
        ComLoading.Close();
    }

	public void getQiangDuoGoldOpponentsError(BaseHttpRequest request, string error)
	{
		ComLoading.Close();
    }
    
    //抢夺龙珠战界面整合部队列表
	public void getCurrentQiangDuoDragonBallOpponents()
	{
		if(Core.Data.dragonManager.qiangDuoDragonBallFightOpponentList.Count > 5)
		{
			RED.Log ("  get count      dragon ball  opponent s   = "+ Core.Data.dragonManager.qiangDuoDragonBallFightOpponentList.Count);
			this.currentQiangDuoDragonBallList.Clear();
			while(this.currentQiangDuoDragonBallList.Count < 5)
			{
				int index = UnityEngine.Random.Range(0, Core.Data.dragonManager.qiangDuoDragonBallFightOpponentList.Count);
				FightOpponentInfo foi = Core.Data.dragonManager.qiangDuoDragonBallFightOpponentList[index];
				if(!this.currentQiangDuoDragonBallList.Contains(foi))
				{
					this.currentQiangDuoDragonBallList.Add(foi);
				}
			}
		}
		else
		{
			this.currentQiangDuoDragonBallList = new List<FightOpponentInfo>(Core.Data.dragonManager.qiangDuoDragonBallFightOpponentList);
			RED.Log ("  else  count = "+ currentQiangDuoDragonBallList.Count );
        }
        
        this.sortFightOpponentInfoByRank(this.currentQiangDuoDragonBallList.ToArray());
    }

	public void sortFightOpponentInfoByRank(FightOpponentInfo[] roles)
	{
		if(roles == null || roles.Length == 0)
		{
			return;
		}
		
		for(int i = 1; i < roles.Length; i++)   
		{  
			for(int j = roles.Length - 1; j >= i; j--)  
			{  
				if(roles[j-1].r > roles[j].r)  
				{
					FightOpponentInfo temp = roles[j-1];
                    roles[j-1] = roles[j];  
                    roles[j] = temp;        
                }  
            }          
        }
    }


	// >  > > > > > > 宿敌请求 > > > > > > > >henry edit
	/// <summary>
	/// 宿敌的请求协议
	/// 
	/// act     int     141
	// 参数
	//  gid     int     玩家角色ID
	//  type    int     1:抢夺龙珠宿敌，2：排行榜宿敌，3：抢夺金币宿敌
	//  ballId  int     在type=1时，龙珠Id
	
	/// </summary>
	private int _ballId;
	void getSuDiRequest(int typeId=-100,int ballId=-100)
	{	
		ComLoading.Open();
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.GET_SU_DI_LIST, new GetSuDiParam(int.Parse(Core.Data.playerManager.PlayerID),typeId,ballId)); //TODO HENRY
		
		task.afterCompleted = getSuDiRequestCompleted;
		task.ErrorOccured = getSuDiRequestError;
		
		task.DispatchToRealHandler ();
	}

	public void getSuDiRequestCompleted(BaseHttpRequest request, BaseResponse response)
	{
		ComLoading.Close();
		if(response != null && response.status != BaseResponse.ERROR)
		{
			GetSuDiResponse getSuDiResponse = response as GetSuDiResponse;
			
			if(getSuDiResponse.data != null)
			{
				GetSuDiListInfo getSudiListInfo = getSuDiResponse.data;
				FightOpponentInfo[] suDiList = getSudiListInfo.enemy;
				
				sudiDataQueue.Clear();           
				int lens = suDiList.Length;
				for(int i=0;i<lens; i++)
				{
					sudiDataQueue.Enqueue(suDiList[i]);
				}
				this.ShowQiangDuoPanelScript(m_QiangduoEnum);
			}
			
		}
		else if(response != null && response.status == BaseResponse.ERROR)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
		}
		
	}
	
	public void getSuDiRequestError(BaseHttpRequest request, string error)
	{
		
		ComLoading.Close();
	}

	private int _deletGid;
	///   // > > > > > > > > > > > 删除宿敌 >  > >  > > > > > > > > > > > > > > > > > > > > >  > > > > > 
	public void deleteSuDiRequest(int gid)
	{
		_deletGid = gid;
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.DELETE_SU_DI, new DeleteSuDiParam(int.Parse(Core.Data.playerManager.PlayerID), gid));
		task.afterCompleted += deleteSuDiCompleted;
		task.ErrorOccured += deleteSuDiError;
		task.DispatchToRealHandler ();
	}
	
	// 删除协议返回的处理 //
	private void deleteSuDiCompleted(BaseHttpRequest request, BaseResponse response)
	{
		if(response != null && response.status != BaseResponse.ERROR)
		{
			DeleteSuDiResponse deleteSuDiResponse = response as DeleteSuDiResponse;
			Debug.Log("deleteSuDiResponse.data"+deleteSuDiResponse.data);
			deleteSuDiResponse.data = true;
			if(deleteSuDiResponse.data)
			{
				//删除某 条数据
				deleteSudiTempSudi();
				this.ShowQiangDuoPanelScript(QiangduoEnum.QiangduoEnum_Sudi);
			}
		}
		
		ComLoading.Close();
	}
	//删除某 条数据
	private void deleteSudiTempSudi(){
		for(int i=0; i<suDiTempList.Count;i++){
			FightOpponentInfo info=suDiTempList[i];
			
			if(info.g==_deletGid){
				suDiTempList.Remove(info);
			}
		}
		// 数据重新给队列
		moveListToQueue();
	}
	//重新移动数据
	private void moveListToQueue(){
		Queue<FightOpponentInfo> temp = new Queue<FightOpponentInfo>();
		for(int i=suDiTempList.Count-1;i>=0;i--){
			FightOpponentInfo info=suDiTempList[i];
			temp.Enqueue(info);
		}
		suDiTempList.Clear();
		
		while(sudiDataQueue.Count>0)
		{
			temp.Enqueue(sudiDataQueue.Dequeue()); //折腾队列
		}
		sudiDataQueue = null;
		sudiDataQueue = temp;
	}
	//异常处理
	private void deleteSuDiError(BaseHttpRequest request, string error)
	{
		ComLoading.Close();
	}
	// > > > > > > > > > > > 添 加 宿 敌 >  > >  > > > > > > > > > > > > > > > > > > > > >  > > > > >
	public Action addSuDiCompletedDelegate;
	
	public void addSuDiRequest(int eyid)
	{
		Debug.Log("添加宿敌--->"+this);
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.ADD_SU_DI, new AddSuDiParam(int.Parse(Core.Data.playerManager.PlayerID),eyid));
		task.afterCompleted += addSuDiCompleted;
		task.ErrorOccured += addSuDiError;
		task.DispatchToRealHandler ();
	}
	
	public void addSuDiCompleted(BaseHttpRequest request, BaseResponse response)
	{
		if(response != null && response.status != BaseResponse.ERROR)
		{
			AddSuDiResponse addSuDiResponse = response as AddSuDiResponse;
			if(addSuDiResponse.data == 0)
			{
				if(addSuDiCompletedDelegate != null)
				{
					addSuDiCompletedDelegate();
				}
			}
			
		}
	}
	
	public void addSuDiError(BaseHttpRequest request, string error)
	{

	}

	
	/// <summary>
	/// 更新战功商城
	/// </summary>
	public void RefreshZhanGongShop()
	{

		ComLoading.Open();
		RefreshZhangongShopParam param = new RefreshZhangongShopParam(int.Parse(Core.Data.playerManager.PlayerID));
		HttpTask task = new HttpTask(ThreadType.MainThread,TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.REFRESH_ZHANGONG_BUY_ITEM, param);
		
		task.afterCompleted += getzhangongjifenCompleted;
		task.DispatchToRealHandler();
		ComLoading.Open();
	}

	//战功数据请求
	void getBuyItemID()
	{
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.GET_ZHANGONG_BUY_ITEM_ID, new GetZhanGongBuyItemIDParam(Core.Data.playerManager.PlayerID));
		
		task.afterCompleted = getzhangongjifenCompleted;
		task.ErrorOccured = getAllDateError;
		
		task.DispatchToRealHandler ();
		ComLoading.Open();
	}

	//获取抢夺金币的奖励
	void getQiangDuoGoldTotalRequest()
	{
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.GET_QIANGDUO_GOLD_ITEM_TOTAL, new GetQiangDuoGoldBuyItemTotalParam(Core.Data.playerManager.PlayerID));
		
		task.afterCompleted = getzhangongjifenCompleted;
		task.ErrorOccured = getAllDateError;
		
		task.DispatchToRealHandler ();
		ComLoading.Open();
	}

	public void getzhangongjifenCompleted(BaseHttpRequest request, BaseResponse response)
	{
		HttpRequest httpRequest = request as HttpRequest;
		if(response != null && response.status != BaseResponse.ERROR)
		{
			if(httpRequest.Act == HttpRequestFactory.ACTION_GET_ZHANGONG_BUY_ITEM_ID)
			{
				GetZhanGongBuyItemIDResponse getZhanGongBuyItemIDResponse = response as GetZhanGongBuyItemIDResponse;
				if(this._BackCallBack != null)
				{
					UIMiniPlayerController.ElementShowArray = new bool[]{true,true,false,true,true};
					DBUIController.mDBUIInstance.HiddenFor3D_UI();			
					UIMiniPlayerController.Instance.SetActive(true);
					if(qiangDuoPanelScript == null)
					{
						qiangDuoPanelScript = QiangDuoPanelScript.CreateQiangDuoPanel();
						qiangDuoPanelScript.transform.parent = DBUIController.mDBUIInstance._bottomRoot.transform;
						qiangDuoPanelScript.transform.localScale = Vector3.one;
					}
					
					if(qiangDuoPanelScript != null)
					{
						qiangDuoPanelScript.actionButtonLabel.text = Core.Data.stringManager.getString(6000);// "挑战";
						OpenDuihuan();
					}
					if(m_DuihuanFromType != DuihuanFromType.Type_Zhaomu)
					{
						this._BackCallBack = null;
					}
					
					DBUIController.mDBUIInstance.RefreshUserInfo ();
				}
				if(getZhanGongBuyItemIDResponse.data.refreshMoney != null && getZhanGongBuyItemIDResponse.data.refreshMoney.Length == 2)
				{
					m_ZhangongRefreshMoneyType = getZhanGongBuyItemIDResponse.data.refreshMoney[0];
					m_RefreshZhangongMoney = getZhanGongBuyItemIDResponse.data.refreshMoney[1];
				}
					
				m_ZhangongRefreshMaxTimes = getZhanGongBuyItemIDResponse.data.refreshMaxTimes;
				m_ZhangongsurplusRefreshTimes = getZhanGongBuyItemIDResponse.data.surplusRefreshTimes;
				HaveLingqu = getZhanGongBuyItemIDResponse.data.status;
				Core.Data.DuiHuanManager.getBuyItemIDCompleted(getZhanGongBuyItemIDResponse);
				ShowQiangDuoPanelScript(QiangduoEnum.QiangduoEnum_Duihuan);
				ZhanGongTotal = getZhanGongBuyItemIDResponse.data.glorynum;
				qiangDuoPanelScript.duiHuanZhanGong.text = getZhanGongBuyItemIDResponse.data.glorynum.ToString();
				ComLoading.Close();
				return;
				
			}
			else if(httpRequest.Act == HttpRequestFactory.ACTION_GET_QIANGDUO_GOLD_ITEM_TOTAL)
			{
				if(_QiangduoDuihuanBack != null)
				{
					UIMiniPlayerController.ElementShowArray = new bool[]{true,true,false,true,true};
					DBUIController.mDBUIInstance.HiddenFor3D_UI();			
					UIMiniPlayerController.Instance.SetActive(true);
					if(qiangDuoPanelScript == null)
					{
						qiangDuoPanelScript = QiangDuoPanelScript.CreateQiangDuoPanel();
						qiangDuoPanelScript.transform.parent = DBUIController.mDBUIInstance._bottomRoot.transform;
						qiangDuoPanelScript.transform.localScale = Vector3.one;
					}
					
					if(qiangDuoPanelScript != null)
					{
						OpenDuihuan();
					}
					DBUIController.mDBUIInstance.RefreshUserInfo ();
					_QiangduoDuihuanBack = null;
				}
				GoldBuyItemBuyTotalResponse goldBuyItemBuyTotalResponse = response as GoldBuyItemBuyTotalResponse;
				Core.Data.DuiHuanManager.getBuyItemTotalCompleted(goldBuyItemBuyTotalResponse);
				ShowQiangDuoPanelScript(QiangduoEnum.QiangduoEnum_Duihuan);
				ComLoading.Close();
				return;
			}
			else if(httpRequest.Act == HttpRequestFactory.ACTION_REFRESH_ZHANGONG_BUY_ITEM)
			{
				RefreshZhangongShopItemResponse res = response as RefreshZhangongShopItemResponse;
				if(res.data.refreshMoney != null && res.data.refreshMoney.Length == 2)
				{
					m_ZhangongRefreshMoneyType = res.data.refreshMoney[0];
					m_RefreshZhangongMoney = res.data.refreshMoney[1];
				}
					
				m_ZhangongRefreshMaxTimes = res.data.refreshMaxTimes;
				m_ZhangongsurplusRefreshTimes = res.data.surplusRefreshTimes;
				HaveLingqu = res.data.status;
				Core.Data.DuiHuanManager.getBuyItemIDCompleted(res);
				ShowQiangDuoPanelScript(QiangduoEnum.QiangduoEnum_Duihuan);
				ZhanGongTotal = res.data.glorynum;
				qiangDuoPanelScript.duiHuanZhanGong.text = res.data.glorynum.ToString();
				ComLoading.Close();
			}
			if(httpRequest.Act == HttpRequestFactory.ACTION_GET_ZHANGONG_BUY_ITEM_ID)
			{

			}
		}
	}

	void getAllDateError(BaseHttpRequest request, string error)
	{
		ComLoading.Close();
		if(!string.IsNullOrEmpty(error)) {
			SQYAlertViewMove.CreateAlertViewMove(error);
			ConsoleEx.DebugLog("Error = " + error);
		}
	}

	public void OpenDuihuan()
	{ 
		if(m_DuihuanFromType == DuihuanFromType.Type_Zhaomu)
		{
			qiangDuoPanelScript.qiangDuoToggle.isEnabled = false;
			qiangDuoPanelScript.suDiToggle.isEnabled = false;
			qiangDuoPanelScript.huifangToggle.isEnabled = false;
			qiangDuoPanelScript.shuaXinButton.isEnabled = false;
		}
		else
		{
			qiangDuoPanelScript.qiangDuoToggle.isEnabled = true;
			qiangDuoPanelScript.suDiToggle.isEnabled = true;
			qiangDuoPanelScript.huifangToggle.isEnabled = true;
			qiangDuoPanelScript.shuaXinButton.isEnabled = false;
		}
		ComLoading.Close();
	}



	//战斗回放请求列表
	void BattleVideoRequest()
	{
		int _type = ChooseType();
		
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.BattleVideoPlayback, new BattleVideoPlaybackParam(int.Parse(Core.Data.playerManager.PlayerID), _type));
		
		task.afterCompleted = BattleVideoCompleted;
		
		task.DispatchToRealHandler ();
		ComLoading.Open();
	}

	//单个战斗回放请求
	public FinalTrialMgr BattleVideoRequestSingle(string _id, EMViewState _state)
	{
		_EMViewState = _state;
		int AttOrSuff = -1;
		switch(_state) 
		{
			case EMViewState.S_MailBox:
				AttOrSuff = -1;
				break;
			case EMViewState.S_QiangDuo:
				AttOrSuff = 1;
				break;
			case EMViewState.S_XiaoXi:
				AttOrSuff = 0;
				break;
		}
		
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.SingleBattleVideoPlayback, new GetBattleVideoPlaybackParam(int.Parse(Core.Data.playerManager.PlayerID), int.Parse(_id), AttOrSuff) );
		
		task.afterCompleted = BattleVideoCompleted;
		
		task.DispatchToRealHandler ();
		ComLoading.Open();
		return this;
	}

	//所有回放的返回
	public void BattleVideoCompleted(BaseHttpRequest request, BaseResponse response)
	{
		HttpRequest httprequest = request as HttpRequest;
		if(httprequest.Act == HttpRequestFactory.ACTION_BATTLEVIDEOPLAYBACK)
		{
			ComLoading.Close();
			BattleVideoPlaybackResponse mresponse = response as BattleVideoPlaybackResponse;
			if(mresponse != null)
			{
				m_BattleVideoPlaybackdata = mresponse.data;
				this.ShowQiangDuoPanelScript(QiangduoEnum.QiangduoEnum_Playback);
			}
			
		}
		else if(httprequest.Act == HttpRequestFactory.ACTION_SINGLEBATTLEVIDEOPLAYBACK)
		{
			ComLoading.Close();
			BattleVideoPlaybackSingleResponse mresponse = response as BattleVideoPlaybackSingleResponse;
			if(mresponse != null)
			{
				OpenMessageTag(request, mresponse.data);
			}
		}
	}

	int ChooseType()
	{
		if(NowEnum ==  TrialEnum.TrialType_QiangDuoDragonBall)return 1;
		else if(NowEnum ==  TrialEnum.TrialType_TianXiaDiYi)return 2;
		else return 3;
	}

	//打开战斗回放2级界面
	public void OpenMessageTag( BaseHttpRequest request, BattleVideoPlaybackData data)
	{
		if(mUIShareVideoTag == null)
		{
			CreateMessageTag();
		}
		if(mUIShareVideoTag != null)
		{
			int AttOrSuff = 0;
			if(request != null) {
				HttpRequest req = request as HttpRequest;
				if(req != null) {
					GetBattleVideoPlaybackParam param = req.ParamMem as GetBattleVideoPlaybackParam;
					if(param != null) {
						AttOrSuff = param.AttOrSuff;
					}
				}
			}
			
			mUIShareVideoTag.OnShow(AttOrSuff, BattleVideoTagType.Type_PlayBack, data);
		}
	}

	public void OpenMessageTag(string _id, string attname, string defname, BattleVideoTagType type)
	{
		if(mUIShareVideoTag == null)
		{
			CreateMessageTag();
		}
		if(mUIShareVideoTag != null)
		{
			mUIShareVideoTag.OnShow(_id,attname,defname,type);
		}
	}

	void CreateMessageTag()
	{
		UnityEngine.Object obj = PrefabLoader.loadFromPack("LS/pbLSShareVideoTag");
		if(obj != null)
		{
			GameObject go = RUIMonoBehaviour.Instantiate(obj) as GameObject;
			mUIShareVideoTag = go.GetComponent<UIShareVideoTag>();
			if(Core.SM.CurScenesName == SceneName.GAME_BATTLE)
			{
				RED.AddChild(go.gameObject, BanBattleManager.Instance.go_uiPanel);
			}
			else if(Core.SM.CurScenesName == SceneName.MAINUI)
			{
				RED.AddChild(go.gameObject, DBUIController.mDBUIInstance._TopRoot);
			}
			
		}
	}

    #endregion
    

	#region  外部调用跳转兑换界面的接口
	public void OpenDuihuanRequest(BackCallBack mBackCallBack, DuihuanFromType _DuihuanFromType)
	{
		ComLoading.Open();
		this._BackCallBack = mBackCallBack;
		m_DuihuanFromType = _DuihuanFromType;
		//为了取战功
		
		this.NowEnum = TrialEnum.TrialType_TianXiaDiYi;
		RequestByQiangduoType(QiangduoEnum.QiangduoEnum_List);
	}
	
	public void OpenDuihuanGoldRequest(QiangduoDuihuanBack mBackCallBack)
	{
		ComLoading.Open();
		this._QiangduoDuihuanBack = mBackCallBack;
		
		//为了取jifen
		this.NowEnum = TrialEnum.TrialType_QiangDuoGold;
		RequestByQiangduoType(QiangduoEnum.QiangduoEnum_List);
	}
	#endregion
    


	#region   赌博部分
	//天下第一武道大会// add by wxl  加上赌博参数
	public void tianXiaDiYiFightRequest(int otherid, int otherrank,  EMViewState state,int index = -1,int revenge = 0)
	{
		if(m_IsBeginFight)return;
		m_IsBeginFight = true;
		_EMViewState = state;
		int m_Stone = 0;
		if (!Core.Data.guideManger.isGuiding) {
			if (allPVPRobData != null && allPVPRobData.pvpStatus != null && allPVPRobData.pvpStatus.rank.count - allPVPRobData.pvpStatus.rank.passCount > 0) {
				m_Stone = 0;
			} else {
				
				m_Stone = allPVPRobData.pvpStatus.rank.needStone;
			}
		}
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.TIANXIADIYI_BATTLE, new FightBattleParam(int.Parse(Core.Data.playerManager.PlayerID), otherid,
		                                                                            otherrank,index,0,m_Stone));
		
		task.afterCompleted = tianXiaDiYiFightCompleted;
		task.ErrorOccured = tianXiaDiYiFightError;
		
		task.DispatchToRealHandler ();
		ComLoading.Open();
	}
	
	public void tianXiaDiYiFightCompleted(BaseHttpRequest request, BaseResponse response)
	{
		if(response != null && response.status != BaseResponse.ERROR)
		{
			BattleResponse battleResponse = response as BattleResponse;

			if(battleResponse != null && battleResponse.data != null && allPVPRobData != null && allPVPRobData.pvpStatus != null) 
			{
				allPVPRobData.pvpStatus = battleResponse.data.pvpStatus;
				SetPVPCoolTime();
			}
			this.fightResponse(battleResponse, TrialEnum.TrialType_TianXiaDiYi, TemporyData.BattleType.TianXiaDiYiBattle, _EMViewState);
			
		}
		else if(response != null && response.status == BaseResponse.ERROR)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
		}
		m_IsBeginFight = false;
		ComLoading.Close();
	}
	
	public void tianXiaDiYiFightError(BaseHttpRequest request, string error)
	{
		ConsoleEx.DebugLog ("---- Http Resp - Error has ocurred." + error);
		ComLoading.Close();
	}

	//抢夺龙珠赌博部分  add by wxl   增加 默认 -1   赌博id 
	public void qiangDuoDragonBallFightRequest(string otherid,int index= -1)
	{
		if(m_IsBeginFight)return;
		m_IsBeginFight = true;
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		int isGuide = System.Convert.ToInt32(Core.Data.guideManger.isGuiding);
		int m_Stone = 0;
		if (!Core.Data.guideManger.isGuiding) 
		{
			if(allPVPRobData != null && allPVPRobData.pvpStatus != null && allPVPRobData.pvpStatus.ball != null)
			{
				if (allPVPRobData.pvpStatus.ball.count - allPVPRobData.pvpStatus.ball.passCount > 0) {
					m_Stone = 0;
				} else {
					m_Stone = allPVPRobData.pvpStatus.ball.needStone;
                }
			}
	
		}
		
		Debug.Log ("m_Stone   =====   " + m_Stone);
		task.AppendCommonParam(RequestType.QIANGDUO_DRAGONBALL_BATTLE, new QiangDuoDragonBallBattleParam(Core.Data.playerManager.PlayerID, 
		                                                                                                 otherid, 
		                                                                                                 Core.Data.dragonManager.qiangDuoDragonBallPpid.ToString(), isGuide,index,0,m_Stone));
		
		task.afterCompleted = qiangDuoDragonBallCompleted;
		task.ErrorOccured = qiangDuoDragonBallError;
		
		task.DispatchToRealHandler ();
		ComLoading.Open();
	}

	public void qiangDuoDragonBallCompleted(BaseHttpRequest request, BaseResponse response)
	{
		ComLoading.Close();
		if(response != null && response.status != BaseResponse.ERROR)
		{
			BattleResponse battleResponse = response as BattleResponse;
			if(battleResponse != null && battleResponse.data != null)
			{
				allPVPRobData.pvpStatus = battleResponse.data.pvpStatus;
				SetPVPCoolTime();
			}		
			this.fightResponse(battleResponse, TrialEnum.TrialType_QiangDuoDragonBall, TemporyData.BattleType.QiangDuoDragonBallBattle, RUIType.EMViewState.S_QiangDuoDragonBall);
			if(currentFightOpponentInfo != null && battleResponse.data != null && battleResponse.data.ext != null)
				this.currentFightOpponentInfo.htm = battleResponse.data.ext.hittms;
			else {
				ConsoleEx.DebugLog("Qiang Duo Response is Empty.");
			}
			//DBUIController.mDBUIInstance.RefreshUserInfo ();
		}
		else if(response != null && response.status == BaseResponse.ERROR)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
		}
		m_IsBeginFight = false;
	}

	public void qiangDuoDragonBallError(BaseHttpRequest request, string error)
	{
		SQYAlertViewMove.CreateAlertViewMove (error);
		ComLoading.Close();
	}

	//抢夺金币和宿敌部分add by wxl    增加 赌博类型
	public FinalTrialMgr qiangDuoGoldFightRequest(int otherid, int otherrank,  EMViewState state,int index = -1,int revenge = 0,int fightMail_id = 0)
	{
		if(m_IsBeginFight)return this;
		m_IsBeginFight = true;
		_EMViewState = state;
		int m_Stone = 0;
		if(revenge == 0)
		{
			if(allPVPRobData != null && allPVPRobData.pvpStatus != null && allPVPRobData.pvpStatus.robs != null)
			{
				if(allPVPRobData.pvpStatus.robs.count - allPVPRobData.pvpStatus.robs.passCount > 0)
				{
					m_Stone = 0;
				}
				else
                {
                    m_Stone = allPVPRobData.pvpStatus.robs.needStone;
                }
			}

		}
		else if(revenge == 1)
		{	
			if(Core.Data.playerManager.revengeData.maxProgress - Core.Data.playerManager.revengeData.curProgress > 0)
			{
				m_Stone = 0;
			}
			else
			{
				m_Stone = Core.Data.playerManager.revengeData.needStone;
			}
		}
		
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.QIANGDUO_GOLD_BATTLE, new FightBattleParam(int.Parse(Core.Data.playerManager.PlayerID), 
		                                                                              otherid, otherrank, index,revenge, m_Stone));
		
		task.afterCompleted = (BaseHttpRequest request, BaseResponse response) =>
		{
			//服务器数据返回正常处理
			if(response != null && response.status != BaseResponse.ERROR)
			{
				BattleResponse battleResponse = response as BattleResponse;			
				
				if(revenge >0)
				{
					this.fightResponse(battleResponse, NowEnum , TemporyData.BattleType.QiangDuoGoldBattle, _EMViewState);
					if(battleResponse.data != null)
					{
						if(Core.Data.playerManager.revengeData != null)
						{
							if(battleResponse.data.pvpStatus != null)
							{
								Core.Data.playerManager.revengeData.curProgress = battleResponse.data.pvpStatus.revenge.passCount;
								Core.Data.playerManager.revengeData.maxProgress = battleResponse.data.pvpStatus.revenge.count;
								if(allPVPRobData == null)
								{
									allPVPRobData = new GetDuoBaoLoginInfoData();
								}
								allPVPRobData.pvpStatus = battleResponse.data.pvpStatus;
								SetPVPCoolTime();
								if(battleResponse.data.pvpStatus.revenge.count <= battleResponse.data.pvpStatus.revenge.passCount)
								{
									Core.Data.playerManager.revengeData.needStone = battleResponse.data.pvpStatus.revenge.needStone;
								}
								else
								{
									Core.Data.playerManager.revengeData.needStone = 0;
								}
								
							}
							
						}
						
					}
				}
				else
				{
					this.fightResponse(battleResponse, TrialEnum.TrialType_QiangDuoGold, TemporyData.BattleType.QiangDuoGoldBattle, _EMViewState);
					allPVPRobData.pvpStatus = battleResponse.data.pvpStatus;
					SetPVPCoolTime();
                }
			}
			else if(response != null && response.status == BaseResponse.ERROR)
			{
				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
			}
			m_IsBeginFight = false;
			ComLoading.Close();
		};
		task.ErrorOccured = qiangDuoGoldError;
		
		task.DispatchToRealHandler ();
		ComLoading.Open();
		return this;
	}
	
	public System.Action OnqiangDuoGoldError;
	public void qiangDuoGoldError(BaseHttpRequest request, string error)
	{
		ComLoading.Close();
		SQYAlertViewMove.CreateAlertViewMove(error);
		if(OnqiangDuoGoldError != null){OnqiangDuoGoldError(); OnqiangDuoGoldError = null;}
	}

	#endregion
    
    
	#region  战斗请求
	void fightResponse(BattleResponse battleResponse, TrialEnum jumpTo, TemporyData.BattleType battleType, RUIType.EMViewState from = RUIType.EMViewState.S_QiangDuo)
	{
		battleResponse.data.battleData.rsty = null;
		battleResponse.data.battleData.rsmg = null;
		Core.Data.temper.warBattle = battleResponse.data;
		Core.Data.temper.currentBattleType = battleType;
		
		Core.Data.temper.mPreLevel = Core.Data.playerManager.RTData.curLevel;
		Core.Data.temper.AfterBattleLv = battleResponse.data.sync.lv;
		
		if(battleResponse != null && battleResponse.data != null && battleResponse.data.reward != null)
			Core.Data.playerManager.RTData.curVipLevel = battleResponse.data.sync.vip;
		
		this.jumpTo = jumpTo;
		BattleToUIInfo.From = from;
		Core.SM.beforeLoadLevel(Application.loadedLevelName, SceneName.GAME_BATTLE);
		AsyncLoadScene.m_Instance.LoadScene(SceneName.GAME_BATTLE);
	}
	#endregion
    
	#region 获取复仇进度 add by jc
	public System.Action<RevengeProgressData> OnRevengeProgress;
	public void GetRevengeProgress(int FightMail_id)
	{
		ComLoading.Open();
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.GET_REVENGE_DATA, new GetRevengeData(Core.Data.playerManager.PlayerID));
		
		task.afterCompleted = (BaseHttpRequest request, BaseResponse response) =>
		{
			ComLoading.Close();
			if(response != null && response.status != BaseResponse.ERROR)
			{
				RevengeResponse resp = response as RevengeResponse;
				if(OnRevengeProgress != null)
				{
					RevengeProgressData rpdata = new RevengeProgressData(resp.data.yetCount,resp.data.maxCount,resp.data.needStone);
					OnRevengeProgress(rpdata);
				}
			}
		};
		task.ErrorOccured = (BaseHttpRequest request, string error) =>
		{
			ComLoading.Close();
			if(OnRevengeProgress != null)
			OnRevengeProgress(null);
		};
		
		task.DispatchToRealHandler ();
	}
	#endregion
	
	
	#region 武道大会请求第一名部分
	public void SendFirstRansMsg()
	{
//		if(!Core.Data.guideManger.isGuiding)
		{
			HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
			task.AppendCommonParam (RequestType.GET_TIANXIADIYI_RANKSINGLE, new GetTianXiaRankParam (int.Parse (Core.Data.playerManager.PlayerID), 1));
			task.afterCompleted = SingleRankAfterComplete;
			task.ErrorOccured = testHttpResp_Error;
			task.DispatchToRealHandler ();
		}
	}

	void testHttpResp_Error (BaseHttpRequest request, string error)
	{
		ConsoleEx.DebugLog ("---- Http Resp - Error has ocurred." + error);
	}
	
	public void SingleRankAfterComplete(BaseHttpRequest request, BaseResponse response)
	{
		if (response != null && response.status != BaseResponse.ERROR)
		{
			GetChallengeRankResponse res = response as GetChallengeRankResponse;
			if (res != null && res.data != null && res.data.list != null)
			{
				if (res.data.list.Length > 0)
				{
					this._FinalTrialData.m_GetChallengeRank = res.data.list [0];
					
					AsyncTask.QueueOnMainThread( () => 
					                            { 
						if(BuildScene.mInstance != null)
							BuildScene.mInstance.UpdateWudaohui();
					});
				}
			}
			else
			{
				RED.LogWarning ("SingleRankAfterComplete  data is null");
			}
		}
	}
	#endregion
	
	#region 外卡任务链接部分
	public void InterfaceCreateScript(TrialEnum m_TrialEnum, MissionBackCallBack mMissionBackCallBack)
	{
		UIMiniPlayerController.ElementShowArray = new bool[]{true,true,false,true,true};
		DBUIController.mDBUIInstance.HiddenFor3D_UI();
		this._MissionBackCallBack = mMissionBackCallBack;
		
		switch(m_TrialEnum)
		{
			case TrialEnum.TrialType_Title:
				break;
			case TrialEnum.TrialType_TianXiaDiYi:
				NowEnum = m_TrialEnum;
				tianXiaDiYiRequest
					(
						getTianXiaDiYiCompleted,
						getTianXiaDiYiOpponentsError
						);
				break;
			case TrialEnum.TrialType_QiangDuoGold:
				NowEnum = m_TrialEnum;
				getQiangDuoGoldOpponentsRequest
					(
						getQiangDuoGoldOpponentsCompleted,
						getQiangDuoGoldOpponentsError
						
						);
				break;
			case TrialEnum.TrialType_QiangDuoDragonBall:
				NowEnum = m_TrialEnum;
				//            ShowQiangDuoPanelScript(QiangDuoPanelScript.ShowType.QiangDuoDragonBall);
				break;
				
		}
	}
	#endregion
    
    


	public void NewFinalTrialFightResponse(BattleSequence sequence, TemporyData.BattleType battleType)
    {
		sequence.battleData.rsty = null;
		sequence.battleData.rsmg = null;
		Core.Data.temper.warBattle = sequence;
        Core.Data.temper.currentBattleType = battleType;

		Core.Data.temper._TempCurCoin = Core.Data.playerManager.Coin;

		if(sequence != null && sequence.reward != null)Core.Data.playerManager.RTData.curVipLevel = sequence.sync.vip;

		this.jumpTo = TrialEnum.None;
		BattleToUIInfo.From = RUIType.EMViewState.S_QiangDuo;
        Core.SM.beforeLoadLevel(Application.loadedLevelName, SceneName.GAME_BATTLE);
        AsyncLoadScene.m_Instance.LoadScene(SceneName.GAME_BATTLE);
    }
    
	public void fightResponse(BattleResponse battleResponse, TemporyData.BattleType battleType)
    {
		battleResponse.data.battleData.rsty = null;
		battleResponse.data.battleData.rsmg = null;
        Core.Data.temper.warBattle = battleResponse.data;
        Core.Data.temper.currentBattleType = battleType;

		Core.Data.temper.mPreLevel = Core.Data.playerManager.RTData.curLevel;
		Core.Data.temper.AfterBattleLv = battleResponse.data.sync.lv;

		if(battleResponse != null && battleResponse.data != null && battleResponse.data.reward != null)Core.Data.playerManager.RTData.curVipLevel = battleResponse.data.sync.vip;

		this.jumpTo = TrialEnum.None;
		BattleToUIInfo.From = RUIType.EMViewState.S_QiangDuo;
        Core.SM.beforeLoadLevel(Application.loadedLevelName, SceneName.GAME_BATTLE);
        AsyncLoadScene.m_Instance.LoadScene(SceneName.GAME_BATTLE);
    }


	#region 注销
	public void Unregister() {
		if(_FinalTrialData != null)
			_FinalTrialData.m_GetChallengeRank = null;
	}
	#endregion


	#region  PVP倒计时     1 ball 2 rank 3 rob 4 revenge
	public void DeletePVPTimer()
	{
		Core.TimerEng.deleteTask(TaskID.PVPBallLeftTime);
		Core.TimerEng.deleteTask(TaskID.PVPRankLeftTime);
		Core.TimerEng.deleteTask(TaskID.PVPRobLeftTime);
		Core.TimerEng.deleteTask(TaskID.PVPRevengeLeftTime);

		m_PVPBallLeftTime = 0;
		m_PVPRankLeftTime = 0;
		m_PVPRobLeftTime = 0;
		m_PVPRevengeLeftTime = 0;
	}

	public void PVPTimerMgr(long cooltime, int type)
	{
		long _starttime = Core.TimerEng.curTime;
		long _endtime = Core.TimerEng.curTime + cooltime;

		TimerTask task = new TimerTask(_starttime, _endtime, 1);
		if(type == 1)
		{
			m_PVPBallLeftTime = cooltime;
            task.taskId = TaskID.PVPBallLeftTime;
			task.onEvent += (TimerTask t) =>
			{
				if(task.leftTime <= 1)
				{
					AsyncTask.QueueOnMainThread( ()  => {
						if(FinalTrialMgr.GetInstance().allPVPRobData != null && FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus != null 
						   && FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus.ball != null)
						{
							FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus.ball.passCount -= 1;
						    if(qiangDuoPanelScript != null)
				      		{
								if(qiangDuoPanelScript.lbl_RobDragonBallTimes != null)
								{
									qiangDuoPanelScript.lbl_RobDragonBallTimes.text = (FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus.ball.count - FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus.ball.passCount).ToString();
									qiangDuoPanelScript.lbl_RobDragonBallTimes.gameObject.SetActive(true);
									qiangDuoPanelScript.lbl_RobDragonBallTimesTitle.gameObject.SetActive(true);
									qiangDuoPanelScript.m_BallLeftTimeLab.SafeText("");
								}

								foreach(FightCell cell in qiangDuoPanelScript.ListCell)
								{
									cell.setRoleIcons();
								}
							}
							if(UIShenLongManager.Instance != null)
							{
								UIShenLongManager.Instance.ShowLeftTimes();
							}
				
	                    }
					});
                }
                m_PVPBallLeftTime = task.leftTime;
			};
		}
		else if(type == 2)
		{
			m_PVPRankLeftTime = cooltime;
			task.taskId = TaskID.PVPRankLeftTime;
			task.onEvent += (TimerTask t) =>
			{
				if(task.leftTime <= 1)
				{
					AsyncTask.QueueOnMainThread( ()  => {
						if(FinalTrialMgr.GetInstance().allPVPRobData != null && FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus != null 
						   && FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus.rank != null)
						{
							FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus.rank.passCount -= 1;

						    if(qiangDuoPanelScript != null)
						    {
								if(qiangDuoPanelScript.tianXiaDiYiTiaoZhanCountLabel != null)
								{
									qiangDuoPanelScript.tianXiaDiYiTiaoZhanCountLabel.text = (FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus.rank.count - FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus.rank.passCount).ToString();
									qiangDuoPanelScript.tianXiaDiYiTiaoZhanCountLabel.gameObject.SetActive(true);
									qiangDuoPanelScript.tianXiaDiYiFreeNameTitleLabel.gameObject.SetActive(true);
									qiangDuoPanelScript.m_RankLeftTimeLab.SafeText("");
								}
								foreach(FightCell cell in qiangDuoPanelScript.ListCell)
								{
									cell.setRoleIcons();
								}
							}

                        }
					});
						
                }
                m_PVPRankLeftTime = task.leftTime;
            };
		}
		else if(type == 3)
		{
			m_PVPRobLeftTime = cooltime;
			task.taskId = TaskID.PVPRobLeftTime;
			task.onEvent += (TimerTask t) =>
			{
				if(task.leftTime <= 1)
				{
					AsyncTask.QueueOnMainThread( ()  => {
						if(FinalTrialMgr.GetInstance().allPVPRobData != null && FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus != null 
						   && FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus.robs != null)
						{
							FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus.robs.passCount -= 1;
						    if(qiangDuoPanelScript != null)
						    {
								if(qiangDuoPanelScript.qiangDuoCount != null)
								{
									qiangDuoPanelScript.qiangDuoCount.text = (FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus.robs.count - FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus.robs.passCount).ToString();
									qiangDuoPanelScript.qiangDuoCount.gameObject.SetActive(true);
									qiangDuoPanelScript.qiangDuoTitleName.text = Core.Data.stringManager.getString (6007);
									qiangDuoPanelScript.qiangDuoTitleName.gameObject.SetActive(true);
									qiangDuoPanelScript.m_RobLeftTimeLab.SafeText("");
								}
								foreach(FightCell cell in qiangDuoPanelScript.ListCell)
								{
									cell.setRoleIcons();
								}
							}
	
	                    }
					});
                }
                m_PVPRobLeftTime = task.leftTime;
                
            };
        }
        else if(type == 4)
        {
            m_PVPRevengeLeftTime = cooltime;
			task.taskId = TaskID.PVPRevengeLeftTime;
			task.onEvent += (TimerTask t) =>
			{
				if(task.leftTime <= 1)
				{
					AsyncTask.QueueOnMainThread( ()  => {
						if(FinalTrialMgr.GetInstance().allPVPRobData != null && FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus != null 
						   && FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus.revenge != null )
						{
							FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus.revenge.passCount -= 1;
						}
						if(Core.Data.playerManager.revengeData != null ) 
						{
							Core.Data.playerManager.revengeData.curProgress -= 1;
							Core.Data.playerManager.revengeData.needStone = 0;
							if(qiangDuoPanelScript != null)
							{
								if(qiangDuoPanelScript.suDiCount != null)
								{
									qiangDuoPanelScript.suDiCount.text =Core.Data.playerManager.revengeData.curProgress.ToString() + "/" + Core.Data.playerManager.revengeData.maxProgress.ToString();
									qiangDuoPanelScript.suDiCount.gameObject.SetActive(true);
									qiangDuoPanelScript.suDiNameTitle.gameObject.SetActive(true);
									qiangDuoPanelScript.m_RevengeLeftTimeLab.SafeText("");
								}
								foreach(FightCell cell in qiangDuoPanelScript.ListCell)
								{
									cell.setRoleIcons();
								}
							}
	                    }
	
					});
                }
                m_PVPRevengeLeftTime = task.leftTime;
            };
		}
		task.DispatchToRealHandler();
	}

	void SetPVPCoolTime()
	{
		if(allPVPRobData != null && allPVPRobData.pvpStatus != null)
		{
			DeletePVPTimer();
			if(allPVPRobData.pvpStatus.ball != null && allPVPRobData.pvpStatus.ball.coolTime != 0)
				PVPTimerMgr(allPVPRobData.pvpStatus.ball.coolTime, 1);
			if(allPVPRobData.pvpStatus.rank != null && allPVPRobData.pvpStatus.rank.coolTime != 0)
				PVPTimerMgr(allPVPRobData.pvpStatus.rank.coolTime, 2);
			if(allPVPRobData.pvpStatus.robs != null && allPVPRobData.pvpStatus.robs.coolTime != 0 )
				PVPTimerMgr(allPVPRobData.pvpStatus.robs.coolTime, 3);
			if(allPVPRobData.pvpStatus.revenge != null && allPVPRobData.pvpStatus.revenge.coolTime != 0)
				PVPTimerMgr(allPVPRobData.pvpStatus.revenge.coolTime, 4);
        }
    }
    #endregion
}
