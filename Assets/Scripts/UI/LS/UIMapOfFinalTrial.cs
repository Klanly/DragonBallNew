using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIMapOfFinalTrial : RUIMonoBehaviour 
{
	public UISprite mMapLeft;
	public UISprite mMapRight;
	public UILabel mTitle;
	public GameObject root;
	public GameObject mSignObj;
	public UIPanel mMapPanel;
	public UIScrollView mMapScrollView;
	SpringPanel mSpringPanel;

//	bool mIsSignOk = false;
	private Dictionary<int, UIPVPPlotBuilding> PVPPlotBuildingList = new Dictionary<int, UIPVPPlotBuilding>();

//	private Vector3 mCachePos;

	private readonly float mHalfPos = 536;

	void Start()
	{
		if(FinalTrialMgr.GetInstance().NowEnum == TrialEnum.TrialType_ShaLuChoose)
		{
			mMapLeft.color = new Color(0.88f,0.65f,0.309f);
			mMapRight.color = new Color(0.88f,0.65f,0.309f);
			mTitle.SafeText(Core.Data.stringManager.getString(25003));
		}
		else if(FinalTrialMgr.GetInstance().NowEnum == TrialEnum.TrialType_PuWuChoose)
		{
			mMapLeft.color = new Color(0.545f,0.658f,0.815f);
			mMapRight.color = new Color(0.545f,0.658f,0.815f);
			mTitle.SafeText(Core.Data.stringManager.getString(25002));
		}
		mMapScrollView.enabled = true;
	}

	public void OnShow(List<NewMapFinalTrial> m_NewMapData)
	{
		CreateBuildIcon(m_NewMapData);
		SetSignPos();
	}

	void SpringGo(float m_temp)
	{
		if(m_temp > 0 && m_temp < 1067)
		{
			SpringPanel.Begin(mMapPanel.gameObject, new Vector3(-m_temp, 0f, 0f), 10f);
		}
		else if(m_temp >= 1067)
		{
			SpringPanel.Begin(mMapPanel.gameObject, new Vector3(-1066, 0f, 0f), 10f);
		}
	}

	void SetSignPos()
	{
		int m_curlayer = FinalTrialMgr.GetInstance()._FinalTrialData.CurDungeon;
		if(FinalTrialMgr.GetInstance().NowEnum == TrialEnum.TrialType_ShaLuChoose)m_curlayer += 10000;
		else if(FinalTrialMgr.GetInstance().NowEnum == TrialEnum.TrialType_PuWuChoose)m_curlayer += 20000;
		UIPVPPlotBuilding m_temp = null;
		if(PVPPlotBuildingList.TryGetValue(m_curlayer, out m_temp))
		{
			Vector3 pos = new Vector3(m_temp.mNewMapFinalTrial.localPosition.x - 536, m_temp.mNewMapFinalTrial.localPosition.y + 211 + 126, m_temp.mNewMapFinalTrial.localPosition.z);
			mSignObj.transform.parent.transform.localPosition = pos;
//			mCachePos = pos;

			SpringGo(m_temp.mNewMapFinalTrial.localPosition.x - mHalfPos);
		}
		else
		{
			mSignObj.gameObject.SetActive(false);
		}
	}

	void CreateBuildIcon(List<NewMapFinalTrial> m_NewMapData)
	{
		if(m_NewMapData != null && m_NewMapData.Count != 0)
		{
			Object obj = PrefabLoader.loadFromPack("LS/LSPVPPlotBuilding");
			if(obj != null)
			{
				foreach(NewMapFinalTrial data in m_NewMapData)
				{
					GameObject go = Instantiate(obj) as GameObject;
					go.name = (data.Data.ID).ToString();
					RED.AddChild(go, root);
					UIPVPPlotBuilding m_script = go.GetComponent<UIPVPPlotBuilding>();
					PVPPlotBuildingList.Add(data.Data.ID, m_script);
					m_script.SetData(data);
				}
			}
		}
	}

	void OnClickBuild(GameObject obj)
	{
		int m_id = 0;
		m_id = int.Parse(obj.name);
		UIPVPPlotBuilding m_temp = null;
		if(PVPPlotBuildingList.TryGetValue(m_id, out m_temp))
		{
			if(FinalTrialMgr.GetInstance()._FinalTrialData.RemainChallengeNum <= 0)
			{
				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(25143));
				return;
			}
			if(FinalTrialMgr.GetInstance()._FinalTrialData.CurDungeon > 15)
			{
				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(25144));
				return;
			}
			if(m_temp.mNewMapFinalTrial.State == NewFloorState.Pass)
			{
				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(25141));
				return;
			}
			else if(m_temp.mNewMapFinalTrial.State == NewFloorState.Unlocked)
			{
				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(25142));
				return;
            }
			FinalTrialMgr.GetInstance().NewFinalTrialCurDungeonRequest(m_temp);
		}
	}

	public void DeleteCell()
	{
		foreach(UIPVPPlotBuilding data in PVPPlotBuildingList.Values)
		{
			data.dealloc();
		}
		PVPPlotBuildingList.Clear();
	}

	static public UIMapOfFinalTrial CreatePanel()
	{
		Object obj = PrefabLoader.loadFromPack("LS/pbLSShaluBuouMap");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			if(go != null)
			{
				RED.AddChild(go, DBUIController.mDBUIInstance._bottomRoot);
				UIMapOfFinalTrial script = go.GetComponent<UIMapOfFinalTrial>();
				return script;
			}
		}
		return null;
	}

}
