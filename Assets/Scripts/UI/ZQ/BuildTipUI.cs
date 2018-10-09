using UnityEngine;
using System.Collections;
using System;

public class BuildTipUI : MonoBehaviour 
{
	private TextMesh[] m_txtMesh;
	private Building m_data;

	public void InitName(Building build)
	{
		if(build == null)
		{
			return;
		}

		m_data = build;
		if(m_txtMesh == null)
		{
			m_txtMesh = transform.GetComponentsInChildren<TextMesh>();
		}
				 
		for(int i = 0; i < m_txtMesh.Length; i++)
		{
			m_txtMesh[i].text = m_data.config.name;
		}
			

		Color clr = new Color(255 / 255f, 255f/ 255f, 234f/ 255f);

		if (m_data.config.id == BaseBuildingData.BUILD_ZHAOMU)
		{
			if (!Core.Data.BuildingManager.ZhaoMuUnlock)
			{
				clr = Color.gray;
			}
		}
		else
		{
			if (Core.Data.playerManager.RTData.curLevel < m_data.config.limitLevel || m_data.config.limitLevel == -1)
			{
				clr = Color.gray;
			}
		}

		for(int i = 0; i < m_txtMesh.Length; i++)
		{
			if(m_txtMesh[i].name != "txtbg")
			{
				m_txtMesh[i].color = clr;
			}
		}

	}
	
//	void Update()
//	{
//		if(BuildScene.mInstance!= null && BuildScene.mInstance.m_camera != null)
//		{
//			this.transform.LookAt(BuildScene.mInstance.m_camera.transform.position, new Vector3(0, -1, 0));
//		}
//	}
}
