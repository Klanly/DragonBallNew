using UnityEngine;
using System.Collections;
using System.Text;
public class SkillShowCellScript : MonoBehaviour
{
	/// <summary>
	/// 技能图标
	/// </summary>
	public UISprite SkillIconSprite;

	[HideInInspector]
	/// <summary>
	/// 显示的技能
	/// </summary>
	public Skill MySkill;

	/// <summary>
	/// 小锁图片
	/// </summary>
	public UISprite LockSprite;

	/// <summary>
	/// 蒙版
	/// </summary>
	public UISprite MarkSprite;

	//技能等级
	public UILabel m_txtLv;
	public UILabel m_txtName;
	public UILabel m_txtType;
	public UILabel m_txtDesp;
	public UILabel m_txtSign;

    string strColor = "[FFDF00]";
	public void Show(Skill skill, int type)
	{
		MySkill = skill;
		if (m_txtType != null)
		{
			m_txtType.text = Core.Data.stringManager.getString (5230 + type);
		}

		if (skill == null)
		{
			m_txtLv.text = "";
			RED.SetActive (false, LockSprite.gameObject, MarkSprite.gameObject, SkillIconSprite.gameObject);

			RED.SetActive (true, m_txtSign.gameObject);
			return;
		}


        SkillIconSprite.spriteName = MySkill.sdConfig.Icon.ToString();
		if (skill.opened) 
		{
			MarkSprite.gameObject.SetActive (false);
			LockSprite.gameObject.SetActive (false);
		} 
		else 
		{
			MarkSprite.gameObject.SetActive (true);
			LockSprite.gameObject.SetActive (true);
		}
			
		RED.SetActive (true, SkillIconSprite.gameObject);

		if (m_txtLv != null)
		{
			StringBuilder strbld = new StringBuilder ("Lv");
			strbld.Append (MySkill.level.ToString ());
			m_txtLv.text = strbld.ToString ();
		}

		if (m_txtName != null)
		{
			m_txtName.text = skill.sdConfig.name;
		}

		if (m_txtSign != null)
		{
			RED.SetActive (false, m_txtSign.gameObject);
		}

		if (m_txtDesp != null)
		{
            m_txtDesp.text = strColor+skill.EffecDescription+"[-]";
		}
	}
}
