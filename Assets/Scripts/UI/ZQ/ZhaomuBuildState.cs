using UnityEngine;
using System.Collections;

public class ZhaomuBuildState : MonoBehaviour 
{
	public GameObject m_tip;

	void Start()
	{
		InvokeRepeating("CheckZhaomuState", 0.0f, 1.0f);
	}

	void CheckZhaomuState()
	{
		bool isFree = false;
		for(int i = 51001; i <= 51006; i++)
		{
			if(Core.Data.zhaomuMgr.IsZhaomuFree(i))
			{
				isFree = true;
				break;
			}
		}

		RED.SetActive(isFree, m_tip);
	}
}
