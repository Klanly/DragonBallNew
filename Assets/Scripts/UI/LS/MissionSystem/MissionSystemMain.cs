using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissionSystemMain : RUIMonoBehaviour
{

	public UILabel _MissionDes;
	public UILabel _MissionCondition;
	public UILabel _RewardCoin;
	public UILabel _RewardStone;

	public UIGrid _Missionlistroot;
	public UIGrid _Rewardlistroot;
	
    List<MissionSystemCell> _MissionList = new List<MissionSystemCell>();
	List<MonsterHeadCell> _RewardList = new List<MonsterHeadCell>();

	[HideInInspector]
	public MissionSystemCell _MissionSystemCell;

	public void ShowMissionDetail()
	{
		_MissionDes.text = "";
		_MissionCondition.text = "";
		_RewardCoin.text = "";
		_RewardStone.text = "";

		SetReward();
	}

	void SetReward()
	{

	}

	void CreateMissionCell(int length)
	{
		Object prefab = PrefabLoader.loadFromPack ("LS/pbLSMissionCell");
		if (prefab != null)
		{
			for(int i=0; i<length; i++)
			{
				GameObject obj = Instantiate (prefab) as GameObject;
				obj.name = (i+1).ToString();
				RED.AddChild (obj, _Missionlistroot.gameObject);
				MissionSystemCell mm = obj.GetComponent<MissionSystemCell> ();
				_MissionList.Add(mm);
            }
            
        }
		_Missionlistroot.Reposition();
	}

	void CreateMissionReward(int length)
	{
		Object prefab = PrefabLoader.loadFromPack ("ZQ/MonsterHeadcell");
		if (prefab != null)
		{
            
        }
		_Rewardlistroot.Reposition();
    }
    
    public void DeleteCell()
	{
		for(int i=0; i<_MissionList.Count; i++)
		{
			_MissionList[i].dealloc();
		}
		for(int i=0; i<_RewardList.Count; i++)
		{
			Destroy(_RewardList[i]);
        }
    }

	public void SendMissionSysRequest()
	{
        
    }

	void OnDestroy()
	{
		_MissionDes = null;
		_MissionCondition = null;
		_RewardCoin = null;
		_RewardStone = null;
		_MissionList.Clear();
		_RewardList.Clear();
		_MissionSystemCell = null;
	}








}
