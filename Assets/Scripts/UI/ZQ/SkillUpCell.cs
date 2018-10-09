using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;

public class SkillUpCell : MonoBehaviour 
{
	private Skill m_skill;
	private int m_nType;

	public UIButton m_btnUpgrade;					//升级按钮
	public UILabel m_txtSkillCard;					//技能升级券
	public UILabel m_txtCoin;						//金币
	public SkillShowCellScript m_skillCell;			//技能UI
	public UISprite m_spBg;

	public readonly Color ANGRYSKILL_COLOR = new Color (247f / 255f, 143f / 255f, 255f/ 255f);
	public readonly Color NORMALSKILL_COLOR = new Color(79f / 255f, 192f / 255f, 255f / 255f);

	public void InitUI(Skill skill , int type)
	{
		m_skill = skill;
		m_nType = type;

		UpdateUI ();
	}

	//显示升级的增加值
	public void ShowUpParam(List<string> list)
	{
		if(list == null || list.Count == 0)
		{
			return;
		}

		for(int i = 0; i < list.Count; i++)
		{
			//need to modify...
            if(m_skillCell != null)
                LabelEffect.CreateLabelEffect(list[i],40,1f,Color.green,m_skillCell.m_txtLv.transform.localPosition,m_skillCell.transform,300,i*0.4f);
			RED.Log(list[i]);
		}
	}


	public void UpdateUI()
	{
		m_skillCell.Show (m_skill, m_nType);
		m_txtSkillCard.text = m_skill.cost_skillCard.ToString ();
		m_txtCoin.text = m_skill.cost_coin.ToString ();

		bool enable = Core.Data.itemManager.GetBagItemCount (ItemManager.SKILL_CARD) >= m_skill.cost_skillCard
			&& Core.Data.playerManager.RTData.curCoin > m_skill.cost_coin
				&& m_skill.level < m_skill.skillLvConfig.max_lv
					&& m_skill.opened;
		SetUpBtnEnable(enable);

		if (m_nType == 0)
		{
			m_spBg.color = NORMALSKILL_COLOR;
		}
		else
		{
			m_spBg.color = ANGRYSKILL_COLOR;
		}
	}
		
	void Start()
	{
		UpdateUI ();
	}

	public void OnClickUpgrade()
	{
		if(m_skill.level >= m_skill.skillLvConfig.max_lv)
		{
			SetUpBtnEnable(false);
			return;
		}

		if(!m_skill.opened)
		{
			SetUpBtnEnable(false);
			return;
		}

		if (Core.Data.itemManager.GetBagItemCount (ItemManager.SKILL_CARD) < m_skill.cost_skillCard)
		{
			SetUpBtnEnable(false);
			return;
		}

		if (Core.Data.playerManager.Coin < m_skill.cost_coin)
		{
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (6028));
			SetUpBtnEnable(false);
			return;
		}

		SkillUpUI.mInstance.OnClickSkillUpgrade (m_skill.sdConfig.ID);
	}

	void SetUpBtnEnable(bool enable)
	{
		m_btnUpgrade.isEnabled = enable;
		Color clr = enable ? Color.white : Color.black;

		UISprite[] sp = m_btnUpgrade.GetComponentsInChildren<UISprite>();
		for (int i = 0; i < sp.Length; i++)
		{
			sp [i].color = clr;
		}
	}
}
