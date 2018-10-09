 using UnityEngine;
using System.Collections;


public class CardSystemPanel : RUIMonoBehaviour {

	public GameObject[] mGoalPos;

	public GameObject grid;

	public GameObject SecondGrid;

	public GameObject m_AgainBtnRoot;

	public UISprite m_MoneyIcon;

	public UILabel m_MoneyNum;

	public Transform Point;

	public UIButton mOk;
	public UIButton mAllOk;


	private int m_NeedMoney = 0;
	private bool m_IsStone;
	private bool m_AbleRepay;
	private Vector3 m_AgainBtnPos;
	private Vector3 m_BtnPos;
	int mIndex ;
	int rotateindex;
	int mid;


	float mAngle = 0.0f;
	bool key = false;

	bool mIsPlay = false;

	private ItemOfReward[] _rewards;


	public void OnShow(ItemOfReward[] _rewards, int _Moneynum, bool isShowEgg, bool _IsStone, bool _AbleRepay)
	{
		this._rewards = _rewards;
		m_NeedMoney = _Moneynum;
		m_IsStone = _IsStone;
		m_AbleRepay = _AbleRepay;
        EggCard.Open3D();

		Invoke("BeginPointFx", 5.6f);
		Invoke("PlayAudio", 2.0f);
	}

	void OnShowSingle(ItemOfReward[] _rewards, int _Moneynum, bool isShowEgg, bool _IsStone, bool _AbleRepay)
	{
		m_NeedMoney = _Moneynum;
		m_IsStone = _IsStone;
		m_AbleRepay = _AbleRepay;
		this._rewards = _rewards;
		Monster mm = _rewards[0].toMonster(Core.Data.monManager);
		UISingleCardFx.OpenCardSinglePanel();
		if (isShowEgg == false) {
			EggCardSingle.Open3DWithoutEgg (mm.Star);
			UISingleCardFx.GetInstance ().showTime = 5;
		
		} else {
			EggCardSingle.Open3D(mm.Star);
		
		}
		if (isShowEgg) {
			Invoke ("SetSingleBtn", 10f);
			Invoke ("PlayAudio", 2.0f);
		} else {
			Invoke ("SetSingleBtn", 6f);
			Invoke ("PlayAudio", 2.0f);
		}
	}

	public void CheckRepalyOrPlayTen(ItemOfReward[] _rewards, int _Moneynum, bool isShowEgg, bool _IsStone, bool _AbleRepay)
	{
		if(UICardPointFx.GetInstance() == null)OnShow(_rewards, _Moneynum, isShowEgg, _IsStone, _AbleRepay);
		else
		{
			CradSystemFx.GetInstance().DeleteList();
			if(CradSystemFx.GetInstance().mSelectScript != null)Destroy(CradSystemFx.GetInstance().mSelectScript.gameObject);
			if(CradSystemFx.GetInstance().m_UIEggCellIntroduct != null)Destroy(CradSystemFx.GetInstance().m_UIEggCellIntroduct.gameObject);
			if(UICardPointFx.GetInstance() != null)UICardPointFx.GetInstance().dealloc();
			Start();
			OnShow(_rewards, _Moneynum, isShowEgg, _IsStone, _AbleRepay);
        }
    }

	public void CheckRepalyOrPlay(ItemOfReward[] _rewards, int _Moneynum, bool isShowEgg, bool _IsStone, bool _AbleRepay)
	{
		if(UISingleCardFx.GetInstance() == null)OnShowSingle(_rewards, _Moneynum, isShowEgg, _IsStone, _AbleRepay);
		else
		{
			Start();
			OnShowSingle(_rewards, _Moneynum, isShowEgg, _IsStone, _AbleRepay);
		}
	}

	//单抽显示所有按钮
	void SetSingleBtn()
	{
		mAllOk.gameObject.SetActive(true);
		mAllOk.Text = Core.Data.stringManager.getString(25098);
		if(m_IsStone)m_MoneyIcon.spriteName = "common-0014";
		else m_MoneyIcon.spriteName = "common-0013";
		m_MoneyNum.SafeText(m_NeedMoney.ToString());
		if(CradSystemFx.GetInstance().m_AbleRepay)
		{
			m_AgainBtnRoot.gameObject.SetActive(true);
		}

#if NEWEGG
		#if SPLIT_MODEL
		if (!Core.Data.sourceManager.IsModelExist (this._rewards[0].pid))
		{
			UIDownModel.OpenDownLoadUI (this._rewards[0].pid, DownLoadSingleFinish, null, UIDownModel.WinType.WT_Two);
		}
		else
		{
			;
		}
		#else
		#endif
#else

#endif

	}

	void  DownLoadSingleFinish(AssetTask task)
	{
		Monster curmon = this._rewards[0].toMonster(Core.Data.monManager);
		if(curmon != null)
		{
			if(EggCardSingle.GetInstance()._CRLuo_ShowANDelCharactor != null)
			{
				EggCardSingle.GetInstance()._CRLuo_ShowANDelCharactor.CharactorID = curmon.config.ID;
				EggCardSingle.GetInstance()._CRLuo_ShowANDelCharactor.CreateModel();
				EggCardSingle.GetInstance()._CRLuo_ShowANDelCharactor.DeleteModel();
			}
		}
	}

	//头像飞完后显示所有按钮
	public void SetTenBtn()
	{
		SetOkBtn(false);
		m_MoneyNum.SafeText(m_NeedMoney.ToString());
		if(m_IsStone)m_MoneyIcon.spriteName = "common-0014";
		else m_MoneyIcon.spriteName = "common-0013";
	}
        
	void SingleDownLoad()
	{

	}

	public void SetBeginAnimation()
	{
		StartCoroutine(SetBeginAnimationIE());
	}

	IEnumerator SetBeginAnimationIE()
	{
		while(mIndex < _rewards.Length)
		{
			yield return new WaitForSeconds(0.2f);
			CradSystemFx.GetInstance().BeginCardHeadAnimati(_rewards[mIndex].pid,mIndex);
			mIndex++;
		}
	}

	void BeginPointFx()
	{
		UICardPointFx.OpenUI();
		Invoke("AllOnShow", 1.7f);
		Core.Data.soundManager.SoundFxPlay (SoundFx.FX_Dragon);
	}

	void PlayAudio()
	{
		Core.Data.soundManager.SoundFxPlay (SoundFx.FX_Egg);
	}

	void StartRotate()
	{
		while(rotateindex < _rewards.Length)
		{
			CradSystemFx.GetInstance().BeginRotateAnimation(rotateindex);
			rotateindex++;
		}
	}

	IEnumerator SetEggAnimationRoundIE()
	{
		yield return new WaitForSeconds(0.0f);
	}
	

	void AllOnShow()
	{
        rotateindex = 0;
		mIndex = 0;
		CradSystemFx.GetInstance().InitHeadCell(_rewards, grid);
	}

	public void Back_Onclick()
	{
		if(EggCard.GetInstance() != null)EggCard.GetInstance().dealloc();
		if(EggCardSingle.GetInstance() != null)EggCardSingle.GetInstance().Delete();
		if(UISingleCardFx.GetInstance() != null)UISingleCardFx.GetInstance().dealloc();
		if(UICardPointFx.GetInstance() != null)UICardPointFx.GetInstance().dealloc();
		if(CradSystemFx.GetInstance().mSelectScript != null)Destroy(CradSystemFx.GetInstance().mSelectScript.gameObject);
		if(CradSystemFx.GetInstance().m_UIEggCellIntroduct != null)Destroy(CradSystemFx.GetInstance().m_UIEggCellIntroduct.gameObject);
		DBUIController.mDBUIInstance.RefreshUserInfo ();
		UIMiniPlayerController.Instance.SetActive(true);
		Destroy(gameObject);
	}

	void OnDestroy()
	{
		CradSystemFx.GetInstance().DeleteList();
		grid = null;
		mOk = null;
		mAllOk = null;
		Point = null;
		CradSystemFx.GetInstance().mCardSystemPanel = null;
	}

	public void SetOkBtn(bool _key)
	{
		if(mOk != null)
		{
			mOk.gameObject.SetActive(_key);
			mAllOk.gameObject.SetActive(!_key);
			mAllOk.Text = Core.Data.stringManager.getString(25099);
			if(CradSystemFx.GetInstance().m_AbleRepay)
			{
				m_AgainBtnRoot.gameObject.SetActive(!_key);
			}
		}
	}

	void Ok_OnClick()
	{
		CradSystemFx.GetInstance().ReceiveCard();
		SetOkBtn(false);
	}

	void All_OnClick()
	{
		Back_Onclick();
		if(Core.Data.guideManger.isGuiding)
		{
			Core.Data.guideManger.AutoRUN();
		}
	}

	void Again_OnClick()
	{
		if(ZhaoMuUI.mInstance.m_zhaomuSubUI != null)
		{
			ZhaoMuUI.mInstance.m_zhaomuSubUI.SendZhaomuMsg();
		}
	}

	// Use this for initialization
	void Start () 
	{
		m_AgainBtnPos = new Vector3(140f, -260f,0f);
		m_BtnPos =  new Vector3(0f, -260f,0f);
		mOk.gameObject.SetActive(false);
		m_AgainBtnRoot.gameObject.SetActive(false);
		mIsPlay = true;
		if(!CradSystemFx.GetInstance().m_AbleRepay)
		{
			mAllOk.transform.localPosition = m_BtnPos;
		}
		else
		{
			mAllOk.transform.localPosition = m_AgainBtnPos;
		}
		mAllOk.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(key)
		{
			mAngle -= 3.0f;
		}
		if(mIsPlay)
		{

		}
	}
}
