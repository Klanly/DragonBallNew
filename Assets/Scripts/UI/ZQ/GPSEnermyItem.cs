using UnityEngine;
using System.Collections;

public class GPSEnermyItem : MonoBehaviour 
{
	public UILabel m_txtName;
	public RandMonCell m_head;


	public void SetEnermyData(Monster mon)
	{
		m_head.InitMonster (mon);
		m_txtName.text = mon.config.name;
		RED.SetActive(false, m_head.m_spAttr.gameObject);
	}

}
