using UnityEngine;
using System.Collections;


//created by zhangqiangat 2014-3-14
public class TeamModifyUI : MDBaseViewController
{
	private static TeamModifyUI _mInstance;
	public static TeamModifyUI mInstance
	{
		get
		{
			return _mInstance;
		}
	}

	public ModifyMonsterUI m_modifyMonsterUI;
	public SwapTeamUI m_swapTeamUI;
	public UIButton m_btnChangePos;
	public UIButton m_btnSwapTeam;

	private const string BTN_SEL = "Symbol 31";
    private const string BTN_UNSEL = "Symbol 32";

	private Color UNSEL_COLOR = Color.white;
    private Color SEL_COLOR = Color.white;//new Color(1f,215f/255f,0);

	private UILabel m_txtChgPos;
	private UILabel m_txtSwapTeam;
	public UIScrollBar bar;
    public UIPanel alphaSidePanel;
	public ModifyMonsterCell m_selCell
	{
		get
		{
			return m_modifyMonsterUI.m_curSelMonster;
		}
		set
		{
			m_modifyMonsterUI.m_curSelMonster = value;
		}
	}

	public static void OpenUI()
	{
		if(_mInstance == null)
		{
			Object prefab = PrefabLoader.loadFromPack("ZQ/TeamModifyUI");
			if(prefab != null)
			{
				GameObject obj = Instantiate(prefab) as GameObject;
				RED.AddChild(obj, DBUIController.mDBUIInstance._bottomRoot);
				obj.transform.localPosition = Vector3.zero;
				obj.transform.localScale = Vector3.one;
				obj.transform.localEulerAngles = Vector3.zero;
			}
		}
		else
		{
			_mInstance.ShowUI(true);
		}
	}

	public static void DestroyUI()
	{
		if(_mInstance != null)
		{
			Destroy(_mInstance.gameObject);
			_mInstance = null;
		}
	}


	void Awake()
	{
		_mInstance = this;
	}

	void Start()
	{
		InitUI();
		OnBtnClickChangePos();
	}

	void InitUI()
	{
		m_btnChangePos.TextID = 5021;
        m_btnSwapTeam.TextID = 5234;
		m_txtChgPos = m_btnChangePos.transform.GetComponentInChildren<UILabel>();
		m_txtSwapTeam = m_btnSwapTeam.transform.GetComponentInChildren<UILabel>();
	}

	void ShowUI(bool bShow)
	{
		RED.SetActive(bShow, this.gameObject);
	}

	public void ChangeMonsterPos(ModifyMonsterCell cell)
	{
		m_modifyMonsterUI.OnChangePos(cell);
	}


	void OnBtnClickChangePos()
	{
		RED.SetBtnSprite(m_btnChangePos, BTN_SEL);
		RED.SetBtnSprite(m_btnSwapTeam, BTN_UNSEL);

		m_txtChgPos.color = SEL_COLOR;
		m_txtSwapTeam.color = UNSEL_COLOR;

		m_modifyMonsterUI.SetShow(true);
		m_swapTeamUI.SetShow(false);
		bar.gameObject.SetActive (true);
        alphaSidePanel.gameObject.SetActive(true);

	}

	void OnBtnClickSwapTeam()
	{
		RED.SetBtnSprite(m_btnChangePos, BTN_UNSEL);
		RED.SetBtnSprite(m_btnSwapTeam, BTN_SEL);

		m_txtChgPos.color = UNSEL_COLOR;
		m_txtSwapTeam.color = SEL_COLOR;

		m_swapTeamUI.SetShow(true);
		m_modifyMonsterUI.SetShow(false);


		bar.gameObject.SetActive (false);
        alphaSidePanel.gameObject.SetActive(false);
	}

	void OnBtnBack()
	{
		DestroyUI();
		if(!FinalTrialMgr.GetInstance().IsFinalTrialToTeam)
		{
			//DBUIController.mDBUIInstance.SetViewState(RUIType.EMViewState.S_Team_NoSelect);
			TeamUI.mInstance.SetShow (true);
			TeamUI.mInstance.FreshCurTeam ();
		}
		else
		{
			UIMiniPlayerController.Instance.SetActive(true);
			FinalTrialMgr.GetInstance().IsFinalTrialToTeam = false;

			FinalTrialMgr.GetInstance().mUITrailEnter.SetActive(true);
			DBUIController.mDBUIInstance.mDuoBaoView.SetActive(true);
		}

	}
}
