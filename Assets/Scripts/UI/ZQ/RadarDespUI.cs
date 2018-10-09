using UnityEngine;
using System.Collections;

public class RadarDespUI : MonoBehaviour 
{
	public UILabel m_txtName;
	public UILabel m_txtDesp;
	public UILabel m_txtEnermy;
	public UILabel m_txtReward;

	public UIGrid m_gridEnergy;
	public UIGrid m_gridReward;

	public UIButton m_btnChallenge;

	private GPSWarInfo m_data;

	private Object prefabEnermy;
	private Object prefabReward;


	void Start()
	{
		prefabEnermy = PrefabLoader.loadFromPack ("ZQ/RadamItem");
		prefabReward = PrefabLoader.loadFromPack ("ZQ/RewardCell");
	}

	public void SetGPSWarInfo(GPSWarInfo warInfo)
	{
		m_data = warInfo;

		m_txtName.text = warInfo.Name;
		m_txtDesp.text = warInfo.Describe;
		m_txtEnermy.text = Core.Data.stringManager.getString (5192);
		m_txtReward.text = Core.Data.stringManager.getString (5193);
		m_btnChallenge.TextID = 6000;

		if (prefabEnermy == null)
		{
			prefabEnermy = PrefabLoader.loadFromPack ("ZQ/RandomMonCell");
		}
		if (prefabReward == null)
		{
			prefabReward = PrefabLoader.loadFromPack ("ZQ/RewardCell");
		}

		while(m_gridEnergy.transform.childCount > 0)
		{
			Transform tf = m_gridEnergy.transform.GetChild (0);
			tf.parent = null;
			Destroy (tf.gameObject);
		}

		for (int i = 0; i < m_data.Boss.Count; i++)
		{
			GameObject obj = Instantiate (prefabEnermy) as GameObject;
			RED.AddChild (obj, m_gridEnergy.gameObject);
			obj.transform.localScale = Vector3.one * 0.8f;
			obj.name = i.ToString ();

			RandMonCell cell = obj.GetComponent<RandMonCell> ();
			Monster mon = new Monster ();
			mon.config = Core.Data.monManager.getMonsterByNum (m_data.Boss [i] [0]);
			cell.InitMonster (mon);
			RED.SetActive(false, cell.m_spAttr.gameObject);
		}
		m_gridEnergy.Reposition ();
	
		while(m_gridReward.transform.childCount > 0)
		{
			Transform tf = m_gridReward.transform.GetChild (0);
			tf.parent = null;
			Destroy (tf.gameObject);
		}
			
		for (int i = 0; i < m_data.Show_reward.Length; i++)
		{
			GameObject obj = Instantiate (prefabReward) as GameObject;
			RED.AddChild (obj, m_gridReward.gameObject);
			obj.transform.localScale = Vector3.one * 0.8f;
			obj.name = i.ToString ();

			RewardCell cell = obj.GetComponent<RewardCell> ();
			ItemOfReward reward = new ItemOfReward ();
			reward.pid = m_data.Show_reward [i];
			reward.num = 1;
			cell.InitUI (reward);
		}
		m_gridReward.Reposition ();
		UpdateChallengeBtn();
	}


	public void UpdateChallengeBtn()
	{
		bool canChallenge = false;
		if (Core.Data.gpsWarManager.curRoom != null)
		{
			if (Core.Data.gpsWarManager.curRoom.validMember == Core.Data.gpsWarManager.curRoom.totalMember 
			    && Core.Data.gpsWarManager.AmILeader() && Core.Data.playerManager.RTData.curLevel >= m_data.Unlock_Lv)
			{
				// all ready check
				bool allReady = true;
				for(int i = 0; i < Core.Data.gpsWarManager.curRoom.members.Length; i++)
				{
					if(Core.Data.gpsWarManager.curRoom.members[i].memberState != 1)
					{
						allReady = false;
						break;
					}
				}
				
				canChallenge = allReady;
			}
		}
		
		m_btnChallenge.isEnabled = canChallenge;
	}

	public void SetShow(bool bShow)
	{
		RED.SetActive (bShow, this.gameObject);
	}
		
	void OnbtnChallenge()
	{
		RadarTeamUI.mInstace.SendStartWarRQ (m_data.ID);
	}

	void OnBtnExit()
	{
		SetShow (false);
	}
}
