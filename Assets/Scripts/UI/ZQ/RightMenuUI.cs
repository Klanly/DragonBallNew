using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RightMenuUI : MonoBehaviour 
{
	public GameObject[] m_arryBtns;

	public TweenScale m_bg;
	public TweenScale m_scaleBtnRoot;
	public TweenScale m_scaleBtnTop;

	public UISprite m_spDirection;				//箭头

	public GameObject m_toptip;					//顶部红点提示
	public GameObject m_teamTip;				//队伍红点提示
	public GameObject m_bagTip;					//背包红点提示
	public GameObject m_actTip;					//活动红点提示
	public GameObject m_dailyRewardTip;			//每日奖励红点

	private bool m_bExpand;
	private bool m_bShow;

	public const float ANIM_TIME = 0.1f;

	void Start()
	{
		ExpandUI(true);

		//每月签到
//		if(!Core.Data.ActivityManager.rewardDic.ContainsKey(ActivityManager.monthGiftType))
//			ActivityNetController.GetInstance().GetMonthStateRequest();
//        if (!Core.Data.ActivityManager.rewardDic.ContainsKey (ActivityManager.firstChargeType))
//			ActivityNetController.GetInstance ().GetFirstChargeStateRequest ();

		UpdateBagTip ();
		UpdataDailyGiftTip ();
		UpdataActGiftTip ();
		UpdateToptip ();
		UpdateTeamTip ();
	}
		
	public void SetShow(bool bShow)
	{
		m_bShow = bShow;
		RED.SetActive(bShow, this.gameObject);
	}

	public bool IsShow
	{
		get
		{
			return m_bShow;
		}
	}

	void OnBtnClickMenu()
	{
		ExpandUI(!m_bExpand);
	}


	IEnumerator HideUI()
	{
		yield return new WaitForSeconds(ANIM_TIME / 2);
		for(int i = 0; i < m_arryBtns.Length; i++)
		{
			RED.SetActive(false, m_arryBtns[i]);
		}
		RED.SetActive(false, m_bg.gameObject);
	}
	
	public void ExpandUI(bool bExpand)
	{
		m_bExpand = bExpand;
		if(m_bExpand)
		{
			RED.SetActive(true, m_bg.gameObject);
			TweenScale.Begin(m_bg.gameObject, ANIM_TIME, Vector3.one);
			for(int i = 0; i < m_arryBtns.Length; i++)
			{
				RED.SetActive(true, m_arryBtns[i]);
				TweenPosition.Begin(m_arryBtns[i], ANIM_TIME, new Vector3(0, i * -110, 0));
			}
			//	m_spDirection.transform.localEulerAngles = Vector3.forward;
			m_spDirection.spriteName = "shouqi";
			RED.SetActive (false, m_toptip);
		}
		else
		{
			TweenScale.Begin(m_bg.gameObject, ANIM_TIME, new Vector3(1, 0.00001f, 1));
			for(int i = 0; i < m_arryBtns.Length; i++)
			{
				TweenPosition.Begin(m_arryBtns[i], ANIM_TIME, Vector3.zero);
			}
			//m_spDirection.transform.localEulerAngles = Vector3.forward * 180;

			m_spDirection.spriteName = "zhankai";
			StartCoroutine("HideUI");
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

	public void willShowUI(float time = ANIM_TIME)
	{
		if (!this.gameObject.activeInHierarchy)
		{
			return;
		}
		
		AnimationCurve anim = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(0.4f, 1.3f, 0.5f, 0.5f), new Keyframe(1f, 1f, 1f, 0f));
		m_bg.animationCurve = anim;
		m_scaleBtnTop.animationCurve = anim;
		m_scaleBtnRoot.animationCurve = anim;

		TweenScale.Begin(m_bg.gameObject, time, Vector3.one);
		TweenScale.Begin(m_scaleBtnTop.gameObject, time, Vector3.one);
		TweenScale.Begin(m_scaleBtnRoot.gameObject, time, Vector3.one);
	}
	
	public void willHideUI()
	{
		if (!this.gameObject.activeInHierarchy)
		{
			return;
		}
		
		AnimationCurve anim = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(0.4f, 1.3f, 0.5f, 0.5f), new Keyframe(1f, 1f, 1f, 0f));
		m_bg.animationCurve = anim;
		m_scaleBtnTop.animationCurve = anim;
		m_scaleBtnRoot.animationCurve = anim;
		
		TweenScale.Begin(m_bg.gameObject, ANIM_TIME, Vector3.one *0.0001f);
		TweenScale.Begin(m_scaleBtnTop.gameObject, ANIM_TIME, Vector3.one *0.0001f);
		TweenScale.Begin(m_scaleBtnRoot.gameObject, ANIM_TIME, Vector3.one *0.0001f);
	}

	public void ResetScale(float scale)
	{
		m_bg.transform.localScale = Vector3.one * scale;
		m_scaleBtnTop.transform.localScale = Vector3.one * scale;
		m_scaleBtnRoot.transform.localScale = Vector3.one * scale;
	}


	public void UpdateTeamTip()
	{
		UpdateTeamTip (m_teamTip);
		if (TopMenuUI.mInstance != null)
		{
			UpdateTeamTip (TopMenuUI.mInstance.m_teamTip);
		}
		UpdateToptip ();
	}

	void UpdateTeamTip(GameObject teamTip)
	{
		if (teamTip == null)
		{
			return;
		}
			
		bool bShow = Core.Data.playerManager.IsTeamBetter ();
		RED.SetActive (bShow, teamTip);
	}

	public void UpdateBagTip()
	{
		bool hasNew = (Core.Data.monManager.GetNewMonList().Count > 0 ||
			Core.Data.EquipManager.GetNewEquip().Count > 0 ||
			Core.Data.gemsManager.GetNewGem().Count > 0 ||
			Core.Data.itemManager.GetNewItem().Count > 0);

		RED.SetActive (hasNew, m_bagTip);
		UpdateToptip ();
	}

	public void UpdataDailyGiftTip()
	{
		RED.SetActive ( Core.Data.ActivityManager.HasNewDailyGift(),m_dailyRewardTip);
		UpdateToptip ();
	}


	public void UpdataActGiftTip()
	{
		RED.SetActive ( Core.Data.ActivityManager.HasNewAct() ,m_actTip);
		UpdateToptip ();
	}

	public void UpdateToptip()
	{
		if (!m_bExpand)
		{
			bool showBag = (Core.Data.monManager.GetNewMonList ().Count > 0 ||
			               Core.Data.EquipManager.GetNewEquip ().Count > 0 ||
			               Core.Data.gemsManager.GetNewGem ().Count > 0 ||
			               Core.Data.itemManager.GetNewItem ().Count > 0);

			bool showToptip = showBag | Core.Data.ActivityManager.HasNewAct () | Core.Data.ActivityManager.HasNewDailyGift () | Core.Data.playerManager.IsTeamBetter () | Core.Data.taskManager.isHaveTaskComplete;
			RED.SetActive (showToptip, m_toptip);
		}

		if (TopMenuUI.mInstance != null)
			TopMenuUI.mInstance.UpdateToptip ();
	}
}
