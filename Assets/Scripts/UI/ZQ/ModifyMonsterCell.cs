using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//created by zhangqiang at 2014-3-14

public class ModifyMonsterCell : MonoBehaviour 
{
	public MonsterHeadCell m_head;
	public UILabel m_txtName;
	public UILabel m_txtAtk;
	public UILabel m_txtDefence;

	public SkillShowCellScript[] m_skills;

	public UIButton m_btnChange;
	public UISprite m_spSelBg;

	[HideInInspector]
	public bool m_bSelected = false;

	private Monster m_data;
	public Monster m_monster
	{
		get
		{
			return m_data;
		}
	}

	void Awake()
	{
		m_bSelected = false;
		m_btnChange.TextID = 5019;
	}
	
	public void InitUI(Monster data)
	{
		m_data = data;
		m_txtName.text = data.config.name;
		int pos = Core.Data.playerManager.RTData.curTeam.GetMonsterPos(data.pid);

		m_txtAtk.text = Core.Data.playerManager.RTData.curTeam.MemberAttack(pos).ToString();
		m_txtDefence.text = Core.Data.playerManager.RTData.curTeam.MemberDefend(pos).ToString();
		m_head.InitUI(data);

		List<Skill> list = data.getSkill;

		int i = 0;
		for(; i < list.Count; i++)
		{
			m_skills[i].Show(list[i], i / 2);
			RED.SetActive(true, m_skills[i].gameObject);
		}

		for (; i < m_skills.Length; i++)
		{
			RED.SetActive(false, m_skills[i].gameObject);
		}

		SetSelected(false);
	}

	public void SetSelected(bool bSel)
	{
		m_bSelected = bSel;
		RED.SetActive(bSel, m_spSelBg.gameObject);
		RED.SetActive(!m_bSelected, m_btnChange.gameObject);
	}

	void OnBtnChangePos()
	{
		TeamModifyUI.mInstance.ChangeMonsterPos(this);
	}

	void OnClick()
	{
		if(m_bSelected)
		{
			return;
		}

		TeamModifyUI.mInstance.m_selCell.SetSelected(false);
		SetSelected(true);
		TeamModifyUI.mInstance.m_selCell = this;

	}

}
