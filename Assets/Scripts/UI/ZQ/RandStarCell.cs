using UnityEngine;
using System.Collections;

public class RandStarCell : MonoBehaviour 
{
	public GameObject[] m_stars;

	public void SetStar(int star)
	{
		int i = 0;
		for (; i < star; i++)
		{
			RED.SetActive (true, m_stars [i]);
		}
		for (; i < m_stars.Length; i++)
		{
			RED.SetActive (false, m_stars [i]);
		}
	}

}
