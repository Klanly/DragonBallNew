using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class CradSystemFx
{
	static CradSystemFx mInstance;
	public static CradSystemFx GetInstance()
	{
		if(mInstance == null)
		{
			mInstance = new CradSystemFx();
		}
		return mInstance;
	}

	public CRLuo_ShowStage _CRLuo_ShowStage = null;
	public CRLuo_Rot_Inversion _CRLuo_Rot_Inversion = null;
	public CardSystemPanel mCardSystemPanel = null;
	public UICardPointFx mUICardPointFx = null;
	public List<CardSystemCell> CardCellList = new List<CardSystemCell>();
	public List<UICardHeadCell> CardHeadCellList = new List<UICardHeadCell>();
	public UIEggCellIntroduct m_UIEggCellIntroduct;

	public CardSystemCell mSelectScript = null;

	public Dictionary<int,Vector3> mTargetPosDic = new Dictionary<int,Vector3>();    //每个卡牌的原始位置
	public ItemOfReward[] mItemOfReward;   //10武将
	List<int> mTargetPosDicIndex = new List<int>();

	public int SingleRewardId;

	public Vector3 m_IntroducePos;

	Dictionary<string, GameObject> mCradCellHoverDic = new Dictionary<string, GameObject>();
	
	Vector3 m2 = new Vector3(0,40,-20);
	Vector3 m3 = new Vector3(0,-125,-20);

	public Vector3 mPetBoxPos = new Vector3(90,253,0);

	public int mAllCount = 0;

	float m_temp = 90.0f;

	public bool mIsChoose = false;

	public CardEggType m_CardEggType;

//	public delegate void CardEggFxCallback();
//	public CardEggFxCallback m_CardEggFxCallback;

	private  bool bShowEgg = true;
	private int m_NeedMoney;
	private bool m_IsStone;
	public bool m_AbleRepay;

	public CRLuo_ShowStage mStage
	{
		get{
			if(_CRLuo_ShowStage == null)
			{
				_CRLuo_ShowStage = InitCardFx();
			}
			return _CRLuo_ShowStage;
		}
	}

	CRLuo_ShowStage InitCardFx()
	{
	    UnityEngine.Object obj = PrefabLoader.loadFromPack("LS/ShowStage_Card");
		if(obj != null)
		{
			GameObject go = UnityEngine.MonoBehaviour.Instantiate(obj) as GameObject;
			CRLuo_ShowStage mStage = go.GetComponent<CRLuo_ShowStage>();
			return mStage;
		}
		return null;
	}
//
//	public void ParpareReplaySingleCard(ItemOfReward[] rewards,int _NeedMoney, bool isShowEgg = true, bool _IsStnoe = true)
//	{
//		mItemOfReward = rewards;
//		bShowEgg = isShowEgg;
//		m_NeedMoney = _NeedMoney;
//		m_IsStone = _IsStnoe;
//		this.m_CardEggFxCallback = EggSingleReplay;
//	}

	void EggSingleReplay()
	{
		SetCardSinglePanel(mItemOfReward, m_NeedMoney, bShowEgg, m_IsStone);
	}

	public void InitHeadIntroduce(ItemOfReward _ItemOfReward, Vector3 m_Pos)
	{
		GameObject obj1 = null;
		m_IntroducePos = m_Pos;
		obj1 = PrefabLoader.loadFromPack("LS/pbLSCardHeadIntroduct") as GameObject ;
		if(obj1 != null)
		{
			GameObject go = NGUITools.AddChild (DBUIController.mDBUIInstance._TopRoot, obj1);
			UIEggCellIntroduct mm = go.gameObject.GetComponent<UIEggCellIntroduct>();
			m_UIEggCellIntroduct = mm;
			m_UIEggCellIntroduct.OnShow(_ItemOfReward);
		}
	}

	public void InitCardCell(ItemOfReward reward, int mid, int m_index)
	{
		Monster mon = reward.toMonster(Core.Data.monManager);
		if(mon != null)
		{
			GameObject obj1 = null;
			obj1 = PrefabLoader.loadFromPack("LS/pbLSCard1") as GameObject ;
			if(obj1 != null)
			{
				GameObject go = NGUITools.AddChild (DBUIController.mDBUIInstance._TopRoot, obj1);
				go.gameObject.name = "pbLSCard";
				go.transform.localRotation = Quaternion.Euler (new Vector3(0,180,0));
				CardSystemCell mm = go.gameObject.GetComponent<CardSystemCell>();
				mSelectScript = mm;
				mSelectScript.SetInitPos(mid,m_index,reward);
			}
		}
	}

	public void InitHeadCell(ItemOfReward[] _reward, GameObject m_Grid)
	{	
		mAllCount = _reward.Length;
		UnityEngine.Object obj = PrefabLoader.loadFromPack("LS/pbLSCardRole");
		if(obj != null)
		{
			for(int i=0; i<_reward.Length; i++)
			{
				GameObject go = UnityEngine.MonoBehaviour.Instantiate(obj) as GameObject;
				RED.AddChild (go, m_Grid);
				UICardHeadCell mm = go.GetComponent<UICardHeadCell>();
				mm.SetDetail(_reward[i]);
				CardHeadCellList.Add(mm);
				//计算卡牌位置的方法有待优化，去适应任意个数的位置
				if (_reward.Length == 1)
				{
					mTargetPosDic.Add(i,Vector3.zero);
				}
				else
				{
					if(i < 5)
					{
						mTargetPosDic.Add(i,new Vector3((m2.x+(i+1)*2*m_temp) - 6*m_temp, m2.y, m2.z));
					}
					else
					{
						mTargetPosDic.Add(i,new Vector3((m3.x+(i+1-5)*2*m_temp) - 6*m_temp, m3.y, m3.z));
					}
				}
				
				mTargetPosDicIndex.Add(i);
			}

		}
		mCardSystemPanel.SetBeginAnimation();
	}
	

	void InitCardPanel()
	{
		GameObject obj = PrefabLoader.loadFromPack("LS/pbLSCradSystemFX") as GameObject ;
		if(obj != null)
		{
			GameObject go = UnityEngine.MonoBehaviour.Instantiate(obj) as GameObject;
			mCardSystemPanel = go.gameObject.GetComponent<CardSystemPanel>();
			RED.AddChild(go.gameObject, DBUIController.mDBUIInstance._bottomRoot);
		}
	}

	public void SetIntroductTrue(ItemOfReward  _reward,Vector3 _pos)
	{
		m_UIEggCellIntroduct.gameObject.SetActive(false);  
		m_IntroducePos =_pos; 
		m_UIEggCellIntroduct.OnShow(_reward);
		m_UIEggCellIntroduct.gameObject.SetActive(true);
	}

	public void SetCardPanel(ItemOfReward[] rewards, int _NeedMoney, bool isShowEgg = true, bool _IsStnoe = true, bool _AbleRepay = true)
	{
		mItemOfReward = rewards;
		m_AbleRepay = _AbleRepay;
		if(mCardSystemPanel == null)
		{
			InitCardPanel();
		}
		mCardSystemPanel.CheckRepalyOrPlayTen(rewards, _NeedMoney, isShowEgg, _IsStnoe, _AbleRepay);
		UIMiniPlayerController.Instance.SetActive(false);
	}

	public void SetCardSinglePanel(ItemOfReward[] rewards,int _NeedMoney, bool isShowEgg = true, bool _IsStnoe = true, bool _AbleRepay = true)
	{
		UIMiniPlayerController.Instance.SetActive(false);
		mItemOfReward = rewards;
		SingleRewardId = mItemOfReward[0].pid;
		bShowEgg = isShowEgg;
		m_NeedMoney = _NeedMoney;
		m_AbleRepay = _AbleRepay;
		m_IsStone = _IsStnoe;
#if NEWEGG
		SingleDownload();
#else
		#if SPLIT_MODEL
		if (!Core.Data.sourceManager.IsModelExist (rewards[0].pid))
        {
			UIDownModel.OpenDownLoadUI (rewards[0].pid, DownLoadSingle, SingleDownload, UIDownModel.WinType.WT_Two);
		}
		else
		{
			DownLoadSingle(null);
		}
		#else
		DownLoadSingle(null);
		#endif
#endif

	}

	void SingleDownload()
	{
		if(mCardSystemPanel == null)
		{
			InitCardPanel();
		}
		mCardSystemPanel.CheckRepalyOrPlay(mItemOfReward,m_NeedMoney,bShowEgg,m_IsStone, m_AbleRepay);
	}

	void  DownLoadSingle(AssetTask task)
	{
		SingleDownload();
	}

	public void SetShowStage(int ID, GameObject obj)
	{
		if(mStage != null)
		{
			_CRLuo_Rot_Inversion = mStage.GetComponentInChildren<CRLuo_Rot_Inversion>();

			CardSystemCell cell = obj.GetComponent<CardSystemCell> ();
			if (cell != null)
			{
				_CRLuo_Rot_Inversion.InputOBJ = cell.kapai.gameObject;
			}
			else
			{
				_CRLuo_Rot_Inversion.InputOBJ = obj;
			}
				
			mStage.Try_key = false;
			mStage.ShowCharactor(ID);
		}
	}

	public bool CheckIsOtherCardCellHover(string m_name)
	{
		GameObject o = null;
		if(mCradCellHoverDic.TryGetValue(m_name, out o))
		{
			return true;
		}
		return false;
	}

	public void BeginCardHeadAnimati(int mid,int m_index)
	{
		CardHeadCellList [m_index].SetInitPos (mid, m_index);
	}

	public void BeginRotateAnimation(int m_index)
	{
		CardCellList [m_index].SetRotate (m_index);
	}

	public void SetReceiveCardBtn(bool key)
	{
		mCardSystemPanel.SetOkBtn(key);
	}

	public void ReceiveCard()
	{
		mSelectScript.CountCircleExpression();
	}

	public void DeleteList()
	{
		if(CardHeadCellList.Count == 0)return;
		for(int i=0; i<CardHeadCellList.Count; i++)
		{
			CardHeadCellList[i].dealloc();
		}
		CardHeadCellList.Clear();
		mCradCellHoverDic.Clear();
		mTargetPosDic.Clear();
		mTargetPosDicIndex.Clear();
		if(_CRLuo_ShowStage != null)
		{
			_CRLuo_ShowStage.DeleteSelf();
		}
		mIsChoose = false;
		mAllCount = 0;
	}

	public void CheckClosePanelWindow()
	{
		if(mAllCount <= 0)
		{
			mCardSystemPanel.Back_Onclick();
		}
	}

	public void DeleteAllOneKey()
	{
		if(EggCardSingle.GetInstance() != null)EggCardSingle.GetInstance().Delete();
		if(EggCard.GetInstance() != null)EggCard.GetInstance().dealloc();
		if(UISingleCardFx.GetInstance() != null)UISingleCardFx.GetInstance().dealloc();
		if(UICardPointFx.GetInstance() != null)UICardPointFx.GetInstance().dealloc();
		if(this.mSelectScript != null)this.mSelectScript.dealloc();
		if(this.m_UIEggCellIntroduct != null)this.m_UIEggCellIntroduct.dealloc();
    }
}

//抽蛋种类
public enum CardEggType
{
	CardEggType_None = 0,
	CardEggType_SingleBag = 1,
	CardEggType_TenBag = 2,
	CardEggType_SingleZhaomuCoin = 3,
	CardEggType_SingleZhaomuStone = 4,
	CardEggType_TenZhaomuStone = 5,
	CardEggType_TenZhaomuCoin = 6,
	CardEggType_SingleActiveStone = 7,
}
