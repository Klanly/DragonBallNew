using UnityEngine;
using System.Collections;

public class RandMonsterUI : MonoBehaviour 
{
	public AutoScrollUI m_stars;
	public AutoScrollUI m_monsters;
	public UILabel m_txtTitle;
	public TweenColor m_spSelBg;

	void Start()
	{
		m_txtTitle.textID(5152);
	}

	public void InitData(Monster[] mons)
	{
		m_stars.InitPos();
		m_monsters.InitPos();
		for (int i = 0; i < m_monsters.m_cells.Length; i++)
		{
			RandMonCell monCell = m_monsters.m_cells[i].GetComponent<RandMonCell>();
			monCell.InitMonster (mons[i]);

			RandStarCell starCell = m_stars.m_cells[i].GetComponent<RandStarCell>();
			starCell.SetStar(mons[i].Star);
		}
	}

	public void StartPlay(AutoScrollUI.RandCallBack callBack, object param)
	{
		m_stars.StarRun (null, null);
		m_monsters.StarRun (callBack, param);
	}

	public void SetShow(bool bShow)
	{
		RED.SetActive (bShow, this.gameObject);
		RED.SetActive (false, m_spSelBg.gameObject);
	}

	public void PlaySelBgAnim()
	{
		RED.SetActive (true, m_spSelBg.gameObject);
	}
}
