using UnityEngine;
using System.Collections;

public class FightRoleAllElement : MonoBehaviour {

	public UISprite m_spIcon;
	public UISprite m_spBg;
	public StarsUI m_star;
	public UISprite m_spAttr;
	public UILabel m_txtLvl;
	public UISprite m_spProp;
	public UISprite m_selected;
	void Start () {
	
	}
	
	public void ShowMonster(Monster data)
	{
		if (data == null)
		{
			RED.SetActive (false, this.gameObject);
			return;
		}

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
		
		m_spProp.spriteName = "Attribute_" + ((int)(data.RTData.Attribute)).ToString();
		m_spBg.spriteName = "star" + ((int)(data.RTData.m_nAttr)).ToString();
		
		RED.SetActive (true, m_star.gameObject, m_spAttr.gameObject);
	}
	
	private bool _isSelected = false;
	public bool isSelected
	{
		set
		{
			_isSelected = value;
			Color color = !value ? new Color(1f,1f,1f,1f) : new Color(0.455f,0.455f,0.455f,1f);
			m_star.BrightORDark = !value;
			m_spIcon.color = color;
			m_spBg.color = color;
			//m_spIcon.transform.parent.GetComponent<BoxCollider>().enabled = !value;
			m_selected.enabled = value;
		}
		get
		{
			return _isSelected;
		}
	}

	
}
