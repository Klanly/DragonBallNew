using UnityEngine;
using System.Collections;

//created by zhangqiang at 2014-3-14

public class MonsterHeadCell : MonoBehaviour 
{
	public UISprite	m_spHead;
	public UISprite m_spProp;
	public UISprite m_spHeadBg;

	public UILabel m_txtLevel;
	public StarsUI m_stars;
//	public StarsUI m_extStars;

	private Monster m_data;
	public Monster Data
	{
		get 
		{
			return m_data;
		}
	}

	public void InitUI(Monster data)
	{
		m_data = data;
		AtlasMgr.mInstance.SetHeadSprite(m_spHead, m_data.num.ToString());

		m_spHeadBg.spriteName = "star" + data.RTData.m_nAttr.ToString();
		int attr = (int)(m_data.RTData.Attribute); 
		if(data.num != 19999 && data.num != 19998)
		{
			m_spProp.spriteName = "Attribute_" + attr.ToString();
		}
		else
		{
			m_spProp.spriteName = "common-1055";
		}
		m_spProp.MakePixelPerfect();
		
		m_txtLevel.text = "Lv" + m_data.RTData.curLevel.ToString();
		Vector3 pos = m_stars.transform.localPosition;
		if (data.config.jinjie == 1)
		{
			float val = (m_stars.m_listStars.Length - 1) / 2.0f;
			pos.x = val * m_stars.m_Width * -1;
			m_stars.m_pos = ENUM_POS.left;
		}
		else
		{
			pos.x = 0;
			m_stars.m_pos = ENUM_POS.middle;
		}
		m_stars.transform.localPosition = pos;
//		if (m_extStars != null)
//		{
//			m_extStars.transform.localPosition = pos;
//			RED.SetActive (data.config.jinjie == 1, m_extStars.gameObject);
//			m_extStars.SetStar (6);
//		}

		m_stars.SetStar (data.Star);
	}

	void OnClick()
    {
//        if(TeamModifyUI.mInstance.gameObject.activeInHierarchy == false)
//             MonsterInfoUI.OpenUI (m_data,ShowFatePanelController.FateInPanelType.isInRecruitPanel, false);
//        else
            MonsterInfoUI.OpenUI (m_data,ShowFatePanelController.FateInPanelType.isInTeamModifyPanel, false);
    }
}
