using UnityEngine;
using System.Collections;

public class BuildGetUI : MonoBehaviour 
{
	public GameObject m_getCoin;
	public GameObject m_getHalfCoin;
	public GameObject m_tip;
	public GameObject m_effect;

	public GameObject m_effectGetCoin;

	private BuildItem m_build;
	private Building m_buildData;

	
	public void InitUI(Building data, BuildItem item)
	{
		m_build = item;
		UpdateUI(data);
	}

	public void GetSuccess()
	{
		if(m_buildData.config.build_kind != BaseBuildingData.BUILD_KIND_PRODUCE)
		{
			return;
		}

		GameObject obj = Instantiate(m_effectGetCoin) as GameObject;
		RED.AddChild(obj, this.gameObject);
		obj.transform.localPosition = Vector3.zero;

		StartCoroutine(StarUpdateUI());
	}

	IEnumerator StarUpdateUI()
	{
		yield return new WaitForSeconds(1);
		UpdateUI(m_buildData);
	}
	//wxl
	public void UpdateUI(Building data)
	{
		m_buildData = data;

		RED.SetActive(Core.Data.playerManager.RTData.curLevel >= data.config.limitLevel, this.gameObject);
		if (m_buildData.config.build_kind == BaseBuildingData.BUILD_KIND_PRODUCE)
		{
			if (m_buildData.RTData.dur <= 0)
			{
				if (m_buildData.RTData.openType != 0) {
					if (m_buildData.RTData.robc != 0) {
						RED.SetActive (true, m_getHalfCoin);
						RED.SetActive (false, m_getCoin, m_effect);
					} else {
						RED.SetActive (true, m_getCoin.gameObject, m_effect);
						RED.SetActive (false, m_getHalfCoin);
					}
					RED.SetActive(m_buildData.RTData.dur>0, m_tip);
				} else {
					RED.SetActive (false, m_getCoin.gameObject, m_effect);
					RED.SetActive (false, m_getHalfCoin);
					RED.SetActive(false, m_tip);
				}
			}
			else
			{
				if(m_buildData.RTData.openType != 0)
					RED.SetActive(m_buildData.RTData.dur > 0, m_tip);
				else 
					RED.SetActive(false, m_tip);

				RED.SetActive(false, m_getCoin, m_getHalfCoin,m_effect);
			}
		}
		else if(m_buildData.config.build_kind == BaseBuildingData.BUILD_KIND_BATTLE)
		{
			RED.SetActive(false, m_getCoin, m_getHalfCoin,m_effect);
			RED.SetActive(false, m_tip);
		}
//		RED.SetActive(m_buildData.RTData.dur > 0, m_tip);
	}
		

	void OnClick()
	{
		if (m_build == null)
		{
			BuildScene.mInstance.UpdateAllBuilds ();
		}
		else if(m_buildData.config.build_kind == BaseBuildingData.BUILD_KIND_PRODUCE)
		{
			if ( m_buildData.RTData.openType != 0 &&m_buildData.RTData.robc > 0 && !m_build.m_bRobTipOpened)
			{
				m_build.ShowRobUI ();
				return;
			}

			if(m_buildData.RTData.openType != 0  && m_buildData.RTData.dur == 0)
			{
				m_build.OnClickGet();
			}
		}
	}

	public void OnTouchDown()
	{
	
	}


	public void OnTouchUp()
	{
	}
}
