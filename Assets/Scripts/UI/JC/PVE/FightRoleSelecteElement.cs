using UnityEngine;
using System.Collections;

public class FightRoleSelecteElement : MonoBehaviour {
	
	
	public UISprite m_spIcon;
	public UISprite m_spBg;
	public StarsUI m_star;
	public UISprite m_spAttr;
	public UILabel m_txtLvl;
	public UISprite Spr_Lock;
	public UISprite m_spProp;
	public GameObject Content;
	
	public enum Status
	{
		Normal,
		None,
		Locked,
	}

		
	void Start ()
	{
	
	}
	
	
	public void SetState(Status state)
	{
	     switch(state)
		{
		case Status.Normal:
			Content.SetActive(true);
			Spr_Lock.enabled = false;
			break;
		case Status.None:
			Content.SetActive(false);
			Spr_Lock.enabled = false;
			break;
		case Status.Locked:
			Spr_Lock.enabled = true;
			Content.SetActive(false);
			break;
		}
	}
	
    public void ShowMonster(Monster data)
	{
		AtlasMgr.mInstance.SetHeadSprite (m_spIcon, data.num.ToString ());

		m_spAttr.MakePixelPerfect ();

		m_star.SetStar (data.Star);
        int level = 1;
        if (data.RTData.curLevel <= 0)
        {
            level = 1;
        } 
		else
        {
            level = data.RTData.curLevel;
        }
        m_txtLvl.text = "Lv" + level.ToString ();

		RED.SetActive (true, m_star.gameObject, m_spAttr.gameObject);
		
		m_spProp.spriteName = "Attribute_" + ((int)(data.RTData.Attribute)).ToString();
		m_spBg.spriteName = "star" + ((int)(data.RTData.m_nAttr)).ToString();
	}

}
