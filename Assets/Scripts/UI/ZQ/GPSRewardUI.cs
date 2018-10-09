using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GPSRewardUI : MonoBehaviour 
{
	//	public UILabel m_txtTip;
	public UILabel m_txtSpecial;
	public UIButton m_btnBack;
	public UIButton m_btnReplay;

	public UIGrid m_gridEnermy;
	public UIGrid m_gridReward;

	private Object m_prefabReward;
	private Object m_prefabEmermy;

	private static GPSRewardUI _instace;
	public static GPSRewardUI mInstance
	{
		get
		{
			return _instace;
		}
	}


	public static void OpenUI()
	{
		if (_instace == null)
		{
			Object obj = PrefabLoader.loadFromPack ("ZQ/GPSRewardUI");
			GameObject ui = Instantiate (obj) as GameObject;
			RED.AddChild (ui, UICamera.currentCamera.gameObject);
		}
		else
		{
			mInstance.SetShow (true);
		}
	}

	void Awake()
	{
		_instace = this;
	}

	void Start()
	{
		InitUI ();
	}

	void InitUI()
	{
		m_btnBack.TextID = 5195;
		m_btnReplay.TextID = 5194;
		m_txtSpecial.text = "";
		//		m_txtTip.text = "";

		BattleSequence battle = Core.Data.temper.warBattle;
		List<GPSRewardData> list = new List<GPSRewardData> ();

		for(int i = 0; i < m_gridEnermy.transform.childCount; i++)
		{
			Transform tf = m_gridEnermy.transform.GetChild(i);
			tf.parent = null;
			Destroy(tf.gameObject);
		}

		for(int i = 0; i < m_gridReward.transform.childCount; i++)
		{
			Transform tf = m_gridReward.transform.GetChild(i);
			tf.parent = null;
			Destroy(tf.gameObject);
		}

		if(battle.reward != null && battle.reward.p != null)
		{
			battle.reward.p = AnalysisReward(battle.reward.p);

			for (int i = 0; i < battle.reward.p.Length; i++)
			{
				GPSRewardData rd = new GPSRewardData ();
				rd.reward = battle.reward.p [i];
				rd.IsSpecial = false;
				list.Add (rd);
			}
		}

		if( battle.radarReward != null && battle.radarReward.p != null)
		{
			for (int i = 0; i < battle.radarReward.p.Length; i++)
			{
				GPSRewardData rd = new GPSRewardData ();
				rd.reward = battle.radarReward.p [i];
				rd.IsSpecial = true;
				list.Add (rd);
			}
		}

		if (m_prefabReward == null)
		{
			m_prefabReward = PrefabLoader.loadFromPack ("ZQ/GPSRewardItem");
		}
		if (m_prefabEmermy == null)
		{
			m_prefabEmermy = PrefabLoader.loadFromPack ("ZQ/GPSEnermyItem");
		}
		for (int i = 0; i < list.Count; i++)
		{
			GameObject objRwd = Instantiate (m_prefabReward) as GameObject;
			RED.AddChild (objRwd, m_gridReward.gameObject);

			GPSRewardItem item = objRwd.GetComponent<GPSRewardItem>();
			item.SetGPSRewardData (list [i]);
		}

		List<int> enermys = BanBattleManager.Instance.GetDeadEnmeyList ();
		if(enermys != null)
		{
			for (int i = 0; i < enermys.Count; i++)
			{
				Monster mon = new Monster ();
				mon.config = Core.Data.monManager.getMonsterByNum (enermys [i]);

				GameObject objEmy = Instantiate (m_prefabEmermy) as GameObject;
				RED.AddChild (objEmy, m_gridEnermy.gameObject);
				GPSEnermyItem item = objEmy.GetComponent<GPSEnermyItem> ();
				item.SetEnermyData (mon);
			}
		}

		m_gridEnermy.Reposition ();
		m_gridReward.Reposition ();

		if (battle.radarReward != null && battle.radarReward.p != null)
		{
			string strName = "";
			for (int i = 0; i < Core.Data.gpsWarManager.curRoom.members.Length; i++)
			{
				if (Core.Data.gpsWarManager.curRoom.members [i].memberId == battle.radarReward.user_id)
				{
					strName = Core.Data.gpsWarManager.curRoom.members [i].memberName;
					break;
				}
			}

			string strText = Core.Data.stringManager.getString (5196);
			strText = string.Format (strText, strName);
			m_txtSpecial.text = strText;
		}


		Vector3 tpos = Vector3.up * enermys.Count * 100;
		tpos.y -= 330;
		SpringPanel.Begin(m_gridEnermy.transform.parent.gameObject, tpos , 5.0f);

//		tpos = Vector3.up * list.Count * 100;
//		tpos.y -= 330;
//		SpringPanel.Begin(m_gridReward.transform.parent.gameObject, tpos, 5.0f);
	}

	ItemOfReward[] AnalysisReward(ItemOfReward[] tempData)
	{
		ItemOfReward[] arry = null;
		Dictionary<int, ItemOfReward> dicTemp = new Dictionary<int, ItemOfReward> ();
		for (int i = 0; i < tempData.Length; i++) 
		{
			if (dicTemp.ContainsKey (tempData [i].pid)) 
			{
				dicTemp [tempData [i].pid].num++;
			}
			else
			{
				dicTemp.Add (tempData [i].pid, tempData [i]);
			}
		}
	
		arry = new ItemOfReward[dicTemp.Count];
		arry.safeFree ();
		int count = 0;
		foreach (KeyValuePair<int, ItemOfReward> itor in dicTemp)
		{
			arry [count] = itor.Value;
			count++;
		}

		return arry;
	}


	public void SetShow(bool bShow)
	{
		RED.SetActive (bShow, this.gameObject);
	}

	void OnBtnReplay()
	{
		SetShow (false);
		BanBattleManager.Instance.Replay ();
	}

	void OnBtnBackRoom()
	{
		ActivityNetController.GetInstance ().FightComplete ();
		BattleToUIInfo.From = RUIType.EMViewState.S_GPSWar;
		FinalTrialMgr.GetInstance().jumpTo = TrialEnum.None;
		BanBattleManager.Instance.ReleaseADestory();
		Core.SM.beforeLoadLevel(SceneName.GAME_BATTLE, SceneName.MAINUI);
		AsyncLoadScene.m_Instance.LoadScene(SceneName.MAINUI);
	}
}
