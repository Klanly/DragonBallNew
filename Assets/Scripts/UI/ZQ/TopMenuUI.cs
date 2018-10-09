using UnityEngine;
using System.Collections;

public class TopMenuUI : MonoBehaviour 
{
	public GameObject[] m_arryBtns;
	public UISprite m_spDirection;				//箭头
	private bool m_bExpand;
	public GameObject m_teamTip;
	public TweenScale m_bg;					//小背景
	public GameObject m_maskBg;				//大背景
	public GameObject m_toptip;				//顶部红点提示

	public const float ANIM_TIME = 0.1f;
	public System.Action freshCurTeamCallback = null;

	private static TopMenuUI m_instance;
	public static TopMenuUI mInstance
	{
		get
		{
			return m_instance;
		}
	}

	public static void OpenUI()
	{
		if (mInstance == null)
		{
			Object prefab = PrefabLoader.loadFromPack ("ZQ/TopMenuUI");
			if (prefab != null)
			{
				GameObject obj = Instantiate (prefab) as GameObject;
				RED.AddChild (obj, DBUIController.mDBUIInstance._TopRoot);
			}
		}

		mInstance.SetShow (true);
	}

	public static void DestroyUI()
	{
		if (mInstance != null)
		{
			Destroy (m_instance.gameObject);
			m_instance = null;
		}
	}

	void Awake()
	{
		m_instance = this;
	}

	void Start()
	{
		ExpandUI (false);
		SQYMainController.mInstance.UpdateTeamTip ();
		UpdateToptip ();
	}

	void SetShow(bool bShow)
	{
		RED.SetActive (bShow, this.gameObject);
	}

	IEnumerator HideUI(bool bShow)
	{
		yield return new WaitForSeconds(ANIM_TIME / 2);
		for(int i = 0; i < m_arryBtns.Length; i++)
		{
			RED.SetActive(false, m_arryBtns[i]);
		}
		RED.SetActive(false, m_bg.gameObject, m_maskBg);
		SetShow (bShow);
	}

	public void ExpandUI(bool bExpand, bool bShow = true)
	{
		m_bExpand = bExpand;
		if(m_bExpand)
		{
			RED.SetActive(true, m_bg.gameObject, m_maskBg);
			TweenScale.Begin(m_bg.gameObject, ANIM_TIME, Vector3.one);
			for(int i = 0; i < m_arryBtns.Length; i++)
			{
				RED.SetActive(true, m_arryBtns[i]);
				TweenPosition.Begin(m_arryBtns[i], ANIM_TIME, new Vector3(0, i * -110, 0));
			}
			m_spDirection.transform.localEulerAngles = Vector3.forward * 90;
			RED.SetActive (false, m_toptip);
		}
		else
		{
			TweenScale.Begin(m_bg.gameObject, ANIM_TIME, new Vector3(1, 0.00001f, 1));
			for(int i = 0; i < m_arryBtns.Length; i++)
			{
				TweenPosition.Begin(m_arryBtns[i], ANIM_TIME, Vector3.zero);
			}
			m_spDirection.transform.localEulerAngles = Vector3.forward * 270;
			StartCoroutine("HideUI", bShow);
			UpdateToptip ();
		}
	}
	public bool IsExpand
	{
		get
		{
			return m_bExpand;
		}
	}


	public void OnBtnTop()
	{
		ExpandUI (!m_bExpand);
	}

	public void OnBtnTeam()
	{
		DBUIController.mDBUIInstance._PVERoot.SetActive (false);
		if (FinalTrialMgr.GetInstance ()._PvpShaluBuouRoot != null)
		{
			FinalTrialMgr.GetInstance ()._PvpShaluBuouRoot.SetActive (false);
		}

		//夺宝界面
		if (DuoBaoPanelScript.Instance != null)
		{
			DuoBaoPanelScript.Instance.SetActive (false);
		}

		UIMiniPlayerController.Instance.SetActive (false);
		TeamUI.OpenUI (TeamUICallBack, FromType.FromTopUI);
		ExpandUI (false, false);
	}

	void TeamUICallBack()
	{
		DBUIController.mDBUIInstance.HiddenFor3D_UI ();
		DBUIController.mDBUIInstance._PVERoot.SetActive (true);
		if (FinalTrialMgr.GetInstance ()._PvpShaluBuouRoot != null)
		{
			FinalTrialMgr.GetInstance ()._PvpShaluBuouRoot.SetActive (true);
		}

		SetShow (true);
	
		if (freshCurTeamCallback != null)
		{
			freshCurTeamCallback ();
		}

		if (FightRoleSelectPanel.Instance != null)
			UIMiniPlayerController.Instance.SetActive (false);
	}

	public void OnBtnTask()
	{
		UITask.Open (UITaskType.None,TaskCallBack);
		ExpandUI (false, false);
	}

	void TaskCallBack()
	{
		SetShow (true);
	}

	void OnClickBg()
	{
		ExpandUI (false);
	}


	void OnBtnFrag()
	{
		DBUIController.mDBUIInstance._PVERoot.SetActive (false);
		if (FinalTrialMgr.GetInstance ()._PvpShaluBuouRoot != null)
		{
			FinalTrialMgr.GetInstance ()._PvpShaluBuouRoot.SetActive (false);
		}

		//夺宝界面
		if (DuoBaoPanelScript.Instance != null)
		{
			DuoBaoPanelScript.Instance.SetActive (false);
		}

		DBUIController.mDBUIInstance.SetViewState (RUIType.EMViewState.S_Bag, RUIType.EMBoxType.LOOK_MonFrag);
		ExpandUI (false, false);
	}

	public void OnBtnHome()
	{
		DBUIController.mDBUIInstance.ShowFor2D_UI ();
		UIMiniPlayerController.Instance.SetActive (false);

		//一键退出副本
		DBUIController.mDBUIInstance._PVERoot.ResetPVESystem();

		//沙鲁布欧界面
		if (FinalTrialMgr.GetInstance () != null && FinalTrialMgr.GetInstance ().m_PvpShaluBuouRoot != null)
		{
			FinalTrialMgr.GetInstance ().m_PvpShaluBuouRoot.DestroyUI ();
		}

		//夺宝界面
		if (DuoBaoPanelScript.Instance != null)
		{
			DuoBaoPanelScript.Instance.OnBtnQuit ();
		}
		DestroyUI ();
	}


	public void UpdateToptip()
	{
		if (!m_bExpand)
		{
			bool showToptip = Core.Data.playerManager.IsTeamBetter () | Core.Data.taskManager.isHaveTaskComplete;
			RED.SetActive (showToptip, m_toptip);
		}
	}
}
