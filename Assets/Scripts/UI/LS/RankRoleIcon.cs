using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RankRoleIcon : RUIMonoBehaviour {

	public UISprite	m_spHead;
	public UISprite m_spProp;
	public UISprite m_spHeadBg;
	
	public UILabel m_txtLevel;
	public UILabel mName;
    public UISprite mSlg;
	
	private Monster m_data;
	public Monster Data
	{
		get 
		{
			return m_data;
		}
	}

	private int[] m_EquipData;
	public int[] EquipData
	{
		get{
			return m_EquipData;
		}
	}

	private MonsterTeamInfo[] m_Monteam;
	public MonsterTeamInfo[] Monteam
	{
		get{
			return m_Monteam;
		}
	}
	
	bool _IsChoose;
	public int _index;
	public List<Skill> skilllist = new List<Skill>();
	
	public void InitUI(Monster data, int m_Index, int[] equipdata, MonsterTeamInfo[] monteam)
	{
		_index = m_Index;
		m_data = data;
		m_EquipData = equipdata;
		m_Monteam = monteam;
		skilllist = data.getSkill;

		AtlasMgr.mInstance.SetHeadSprite(m_spHead, m_data.num.ToString());
		
		m_spHeadBg.spriteName = "star" + data.RTData.m_nAttr.ToString();
		int attr = (int)(m_data.RTData.Attribute); 
		m_spProp.spriteName = "Attribute_" + attr.ToString();
		
		m_txtLevel.text = "Lv" + m_data.RTData.curLevel.ToString();
		mName.text = m_data.config.name;


		SetChoose(false);
	}
	

	public void SetChoose(bool _key)
	{
		if(mSlg != null)
		{
			mSlg.gameObject.SetActive(_key);
			_IsChoose = _key;
		}
	}

	void OnClick()
	{
		if(_IsChoose)
		{
			return;
		}
		DragonBallRankMgr.GetInstance().SetCheckInfoListChoose(_index); 
		DragonBallRankMgr.GetInstance().SetChooseDetail(this);
		SetChoose(true);


	}
}
