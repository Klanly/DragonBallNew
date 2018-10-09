using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PvpShaluBuouRoot : RUIMonoBehaviour 
{
	public UIPVPBuildDes m_UIPVPBuildDes;
	public UIMapOfFinalTrial m_UIMapOfFinalTrial;

	static public PvpShaluBuouRoot OpenUI()
	{
		Object obj = PrefabLoader.loadFromPack("LS/pbLSShaluBuouRoot");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			if(go != null)
			{
				RED.AddChild(go, DBUIController.mDBUIInstance._bottomRoot);
				PvpShaluBuouRoot script = go.GetComponent<PvpShaluBuouRoot>();
				return script;
			}
		}
		return null;
	}

	void Start()
	{
		TopMenuUI.OpenUI ();
	}

	public void SetMapDetail(List<NewMapFinalTrial> _Temp)
	{
		m_UIPVPBuildDes.SetActive(false);
		if(m_UIMapOfFinalTrial != null)
		{
			m_UIMapOfFinalTrial.OnShow(_Temp);
		}
	}

	public void SetBuildDes(NewMapFinalTrial _TempMap, string _TempName, ItemOfReward[] _member)
	{
		m_UIPVPBuildDes.SetActive(true);
		if(m_UIPVPBuildDes != null)
		{
			m_UIPVPBuildDes.OnShow(_TempMap, _TempName, _member);
		}
	}

	
	void OnClickCallback(GameObject obj)
	{
		switch(obj.name)
		{
		case "BackBtn":
			DBUIController.mDBUIInstance.mDuoBaoView.SetActive(true);
			DestroyUI ();
			break;
		}
	}

	public void DestroyUI()
	{
		TopMenuUI.DestroyUI();
		m_UIMapOfFinalTrial.DeleteCell();
		this.dealloc();
	}
}
