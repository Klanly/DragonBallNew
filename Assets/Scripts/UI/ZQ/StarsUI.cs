using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ENUM_POS
{
	middle,
	left, 
	right,
}

public class StarsUI : MonoBehaviour 
{
	public Transform[] m_listStars;
	public int m_Width = 30;

	public ENUM_POS m_pos = ENUM_POS.middle;

	public void SetStar(int star)
	{
		float startX = 0;
		int width = m_Width;
		switch(m_pos)
		{
			case ENUM_POS.left:
			{
				startX = 0;
				break;
			}
			case ENUM_POS.middle:
			{
				startX = (star / 2) * -m_Width;
				if (star % 2 == 0)
				{
					startX += m_Width / 2;
				}
				break;
			}
			case ENUM_POS.right:
			{
				startX = m_Width * m_listStars.Length;
				width = -m_Width;
				break;
			}
		}

		for (int i = 0; i < m_listStars.Length; i++)
		{
			RED.SetActive (i < star, m_listStars [i].gameObject);
		m_listStars[i].localPosition = new Vector3(startX + width * i ,0, m_listStars[i].localPosition.z);
		}
	}
	
	
	public bool BrightORDark
	{
		set
		{
			foreach(Transform t in m_listStars)
			{
				UISprite s = t.GetComponent<UISprite>();
				s.color = value ? new Color(1,1,1,1) : new Color(0.455f,0.455f,0.455f,1f);
			}
		}
	}
	
	
}
