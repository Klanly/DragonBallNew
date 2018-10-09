using UnityEngine;
using System.Collections;

//ui类型
public enum FromType
{
	FromMainUI,				//从主界面进
	FromTopUI,				//从topUI进
}
	
public class TeamUI : MonoBehaviour 
{
	public SQYTeamController m_teamCtrller;
	public SQYTeamInfoView m_teamView;
	CRLuo_ShowStage mShowOne;

	public delegate void ExitTeamCallback();
	public ExitTeamCallback m_callBack;					//返回回调

	private FromType m_type;								//ui类型
	public FromType UIType
	{
		get
		{
			return m_type;
		}
	}
	private static TeamUI _mInstance;
	public static TeamUI mInstance
	{
		get 
		{
			return _mInstance;
		}
	}
		
	//第二个槽位是否解锁
	public static bool secondPosUnLock
	{
		get
		{
			return Core.Data.BuildingManager.ZhaoMuUnlock;
		}

	}

	int previouNum = 0;														//前一个武者NUM
	MonsterAttribute previousAttr = MonsterAttribute.DEFAULT_NO;			//前一个武者属性
	[HideInInspector]
	public int mSelectIndex = 0;											//当前选中武者索引
	public Monster curMonster //= null;										//当前武者
	{
		get
		{
			return Core.Data.playerManager.RTData.curTeam.getMember (mSelectIndex);
		}
	}
	public MonsterTeam curTeam 												//当前队伍
	{
		get
		{
			return Core.Data.playerManager.RTData.curTeam;
		}
	}

	public static void OpenUI(ExitTeamCallback callback = null, FromType type = FromType.FromMainUI)
	{
		if (_mInstance == null)
		{
			Object prefab = PrefabLoader.loadFromPack ("ZQ/TeamUI");
			if (prefab != null)
			{
				GameObject obj = Instantiate (prefab) as GameObject;
				RED.AddChild (obj, DBUIController.mDBUIInstance._bottomRoot);
				_mInstance = obj.GetComponent<TeamUI> ();
			}
		}
		else
		{
			_mInstance.SetShow (true);
		}
		_mInstance.mSelectIndex = 0;
		SQYUIManager.getInstance ().opIndex = 0;
		_mInstance.m_type = type;
		_mInstance.m_callBack = callback;
	}

	void Awake()
	{
		_mInstance = this;
	}

	void Start()
	{
		m_teamView.OnBtnWithIndex += OnBtnTeamViewWitnIndex;
		FreshCurTeam ();
		SetShow (true);
	}

	public void FreshCurTeam()
	{
		m_teamCtrller.RefreshCurTeam ();
	}

	public void SwapTeam()
	{
//		curMonster = null;
		mSelectIndex = 0;
		SQYUIManager.getInstance ().opIndex = 0;
		SQYUIManager.getInstance ().opMonster = Core.Data.playerManager.RTData.curTeam.getMember (0);
	}

	public TeamMonsterCell GetMonCellByPos(int pos)
	{
		return m_teamCtrller.GetMonCellByPos (pos);
	}

	public void RefreshMonster(Monster mon)
	{
		m_teamCtrller.RefreshMonster (mon);
		mSelectIndex = curTeam.GetMonsterPos (mon.pid);
//		curMonster = mon;

		FreshSelMonster ();
	}

	public void RefreshSlot(int pos)
	{
		m_teamCtrller.RefreshSlot (pos);
	}

	public bool IsShow
	{
		get
		{
			return this.gameObject.activeInHierarchy;
		}
	}

	public void SetShow(bool bShow)
	{
		RED.SetActive (bShow, this.gameObject, m_teamCtrller.gameObject, m_teamView.gameObject);
		if (!bShow)
		{
			del3DModel ();
		}
		else
		{
			if (curMonster == null)
			{
//				curMonster = curTeam.getMember (mSelectIndex);
				mSelectIndex = 0;
			}

			FreshSelMonster ();

			//mini bar 
			UIMiniPlayerController.Instance.SetActive (false);
		}

	}

	public void CloseUI()
	{
		mSelectIndex = 0;
//		curMonster = null;
		del3DModel();
		mShowOne = null;
		DestroyUI ();
	}

	void DestroyUI()
	{
		if(_mInstance != null)
		{
			Destroy(_mInstance.gameObject);
			_mInstance = null;
		}
	}

	public void OnBtnTeamViewWitnIndex(int index,bool curPosHaveMon = true)
	{
		switch(index)
		{
			case SQYTeamInfoView.BTN_BACK:
				CloseUI ();
				DBUIController.mDBUIInstance.ShowFor2D_UI ();
				if(m_callBack != null)
				{
					m_callBack();
				}
				break;

			case SQYTeamInfoView.BTN_CHANGE:
				if (curMonster != null)
				{
					SQYUIManager.getInstance ().opTeam = curTeam;
					SQYUIManager.getInstance ().opMonster = curMonster;
					//              SQYUIManager.getInstance().opIndex = mSelectIndex;
					Core.Data.temper.infoPetAtk = 0;
					Core.Data.temper.infoPetDef = 0;

					SetShow (false);
					DBUIController.mDBUIInstance.SetViewState (RUIType.EMViewState.S_Bag, RUIType.EMBoxType.CHANGE);
				}
				break;
			case SQYTeamInfoView.BTN_QiangHua:
				if (curMonster != null)
				{
					SQYUIManager.getInstance ().opMonster = curMonster;
					SQYPetBoxController.enterStrengthIndex = 2;

					if (curPosHaveMon)
					{
						Core.Data.temper.infoPetAtk = (int)curTeam.MemberAttack (mSelectIndex);
						Core.Data.temper.infoPetDef = (int)curTeam.MemberDefend (mSelectIndex);
					}
					DBUIController.mDBUIInstance.SetViewState (RUIType.EMViewState.S_Bag, RUIType.EMBoxType.QiangHua);

					SetShow (false);
				}
				break;
		}
	}

	void OnClickSwapTeam()
	{
		if(Core.Data.playerManager.RTData.curLevel < 5)
		{	
			SQYAlertViewMove.CreateAlertViewMove( Core.Data.stringManager.getString(6054).Replace("#","5") );
			return;
		}

		m_teamView.SetActive (false);
		del3DModel ();
		m_teamCtrller.SetShow (true);

		DBUIController.mDBUIInstance.HiddenFor3D_UI(false);
		TeamModifyUI.OpenUI();
	}

	public void FreshSelMonster()
	{
		if (curMonster == null)
			mSelectIndex = 0;


		if(curMonster != null)
		{
			bool allFated = m_teamView.FreshSelMonster();
			EquipmentTableManager.Instance.RefreshEquipment (mSelectIndex);

			show3DModel(curMonster.num, curMonster.RTData.Attribute, allFated);
			Core.Data.temper.curShowMonster = curMonster;
			SQYUIManager.getInstance ().opIndex = Core.Data.playerManager.RTData.curTeam.GetMonsterPos (curMonster.pid);
		}
//		else
//		{
//			OnBtnTeamViewWitnIndex(SQYTeamInfoView.BTN_CHANGE,false);
//		}
	}
	public void show3DModel(int num, MonsterAttribute attri, bool AllFated)
	{
		if(mShowOne == null)
		{
			mShowOne = CRLuo_ShowStage.CreateShowStage();
			mShowOne.Try_key = false;
		}
		RED.SetActive(true, mShowOne.gameObject);

		if(previouNum != num || attri != previousAttr)
		{
			mShowOne.ShowCharactor(num, attri, AllFated);
			if (Core.Data.sourceManager.IsModelExist (num))
			{
				previouNum = num;
				previousAttr = attri;
			}
			else
			{
				previouNum = 0;
				previousAttr = MonsterAttribute.DEFAULT_NO;;
			}
		}
	}

	public void del3DModel()
	{
		if(mShowOne != null)
		{
			previouNum = 0;
			previousAttr = MonsterAttribute.DEFAULT_NO;
			mShowOne.DeleteSelf();
			mShowOne = null;
		}
	}

	public void ClickTeamMonster(TeamMonsterCell cell)
	{
		TeamMonsterCell cv = cell;
		SQYUIManager.getInstance().opIndex = cell.m_nPos;
		mSelectIndex = cell.m_nPos;
//		curMonster = cv.m_monster;

		//阵容 
		Core.Data.temper.curShowMonster = curMonster;

		if(cv.m_monster == null)
		{
			TeamUI.mInstance.OnBtnTeamViewWitnIndex(SQYTeamInfoView.BTN_CHANGE,false);
		}
		else
		{
			FreshSelMonster ();
		}

		m_teamCtrller.SetMonSelected (cell);
		RED.SetActive (true, m_teamView.gameObject);
	}
}
