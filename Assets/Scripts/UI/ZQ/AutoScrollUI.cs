using UnityEngine;
using System.Collections;

public class AutoScrollUI : MonoBehaviour 
{
	public int m_nWidth;			//cell宽度
	public int m_nHeight;			//cell高度
	public int m_nMaxSize;			//cell最大容量
	public Transform[] m_cells;      //单元cell

	public float m_nTime;			//自动滚动时间
	public float m_nSpeed;			//滚动速度
	public float m_nAddSpeed;		//加速度
	public float m_nSlowSpeed;		//降速度
	public delegate void RandCallBack (object param);	//停止滚动的回调函数

	public RandCallBack m_callBack;
	public object m_param;

	bool m_bRun = false;
	private float m_maxRange = 0;		//最大的边

	float m_nStartSpeed = 0;
	int m_nSpeedSwap = 1;

	void Start()
	{
		InitPos();
	}

	public void InitPos()
	{
		if (m_cells.Length % 2 == 0)
		{
			m_maxRange = m_nHeight * m_cells.Length / 2;
		}
		else
		{
			m_maxRange = m_nHeight * (m_cells.Length - 1) / 2;
		}
		
		for (int i = 0; i < transform.childCount; i++)
		{
			m_cells [i].localPosition = new Vector3 (0, m_maxRange - m_nHeight * i, 0);
		}
	}

	public void StarRun(RandCallBack action = null, object param = null)
	{
		m_bRun = true;
		m_nStartSpeed = 0;
		m_callBack = action;
		m_param = param;
		StartCoroutine("StartCellRun");
	}

	IEnumerator StartCellRun()
	{
		yield return new WaitForSeconds(m_nTime);
		m_bRun = false;
		if (m_callBack != null)
		{
			m_callBack (m_param);
		}
	}

	void Update()
	{
		if (m_bRun)
		{
			for (int i = 0; i < m_cells.Length; i++)
			{
				RunCell (m_cells [i], i);
			}
		}
	}

	void RunCell(Transform cell, int index)
	{
		float addSpeed = m_nAddSpeed;
		if (m_nSpeedSwap == -1)
		{
			addSpeed = m_nSlowSpeed;
		}
		m_nStartSpeed += m_nSpeedSwap * addSpeed;

		if (m_nStartSpeed >= m_nSpeed)
		{
			m_nSpeedSwap = -1;
		}
		if (m_nStartSpeed <= 0)
		{
			m_nSpeedSwap = 1;
		}

		cell.Translate (Vector3.up * Time.deltaTime * m_nStartSpeed);
		Vector3 pos = cell.transform.localPosition;
		if (pos.y >= m_maxRange)
		{
			int newpos = index - 1;
			if (newpos < 0)
			{
				newpos = m_cells.Length - 1;
			}

			pos.y = m_cells[newpos].localPosition.y - m_nHeight;
			cell.transform.localPosition = pos;
		}
	}
}
