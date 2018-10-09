using UnityEngine;
using System.Collections;

public class SkillTipItem : MonoBehaviour 
{
	private Skill m_data;
	public UISprite m_spHead;

	public void InitSkill(Skill skill)
	{
		m_data = skill;
		m_spHead.spriteName = skill.sdConfig.Icon.ToString();
		RED.SetActive (true, this.gameObject);
	}

	void OnPress (bool pressed)
	{
		if (m_data == null)
			return;

		if(pressed)
			MonFragUI.mInstance.ShowTipUI (m_data.sdConfig.name, m_data.EffecDescription, this.transform);
		else
			MonFragUI.mInstance.HideTipUI ();
	}
}
