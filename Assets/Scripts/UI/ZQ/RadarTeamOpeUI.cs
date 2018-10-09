using UnityEngine;
using System.Collections;

public class RadarTeamOpeUI : MonoBehaviour 
{
	//雷达挑战关卡
	public RadarItem[] m_radarItems;
	//创建房间按钮
	public UIButton m_btnCreate;
	//退出房间按钮
	public UIButton m_btnExitRoom;
	//加入房间
	public UIButton m_btnJoin;
	//队长信息
	public RadarMemberItem m_leader;
	//队员信息
	public RadarMemberItem m_member;
	//提示信息
	public UILabel m_txtDesp;

	//队员root
	private GameObject m_memberRoot;



	void Start()
	{
		InitUI ();
		UpdateUI ();
	}

	void InitUI()
	{
		m_btnCreate.TextID = 5189;
		m_btnJoin.TextID = 5188;
		m_btnExitRoom.TextID = 5191;
		m_txtDesp.text = Core.Data.stringManager.getString (5186);
	}

	public void UpdateUI()
	{
		m_memberRoot = m_leader.transform.parent.gameObject;
		if (Core.Data.gpsWarManager.curRoom == null || Core.Data.gpsWarManager.curRoom.members == null || Core.Data.gpsWarManager.curRoom.members.Length == 0)
		{
			RED.SetActive (false, m_memberRoot);
			RED.SetActive (true, m_btnCreate.gameObject, m_btnJoin.gameObject, m_txtDesp.gameObject);
		}
		else
		{
			RED.SetActive (true, m_memberRoot);
			RED.SetActive (false, m_btnCreate.gameObject, m_btnJoin.gameObject, m_txtDesp.gameObject);

			m_leader.SetMemberInfo (null);
			m_member.SetMemberInfo (null);

			for (int i = 0; i < Core.Data.gpsWarManager.curRoom.members.Length; i++)
			{
				RoomMembersData member = Core.Data.gpsWarManager.curRoom.members [i];
				if (member != null)
				{
					if (member.memberType == 1)
					{
						m_leader.SetMemberInfo (member);
					}
					else
					{
						m_member.SetMemberInfo (member);
					}
				}
			}

			if(Core.Data.gpsWarManager.AmILeader())
			{
				m_btnExitRoom.transform.localPosition = new Vector3(-280, -95, 0);
			}
			else
			{
				m_btnExitRoom.transform.localPosition = new Vector3(280, -95, 0);
			}
		}
	}
	
	//点击创建房间
	void OnClickCreate()
	{
		RadarTeamUI.mInstace.SendGetStutasRQ ();
	}
		


	//点击假如房间，打开房间列表
	void OnClickJoin()
	{
		SetShow (false);
		RadarTeamUI.mInstace.SendUpdateRoomListRQ ();
		RadarTeamUI.mInstace.m_roomListUI.SetShow (true);
	}

	void OnBtnback()
	{
		if (Core.Data.gpsWarManager.curRoom != null)
		{
			UIInformation.GetInstance ().SetInformation (Core.Data.stringManager.getString (5190), Core.Data.stringManager.getString (5030), ExitRoomCallBack);
		}
		else
		{
			RadarTeamUI.mInstace.SendExitActRQ ();
		}
	}
		
	void ExitRoomCallBack()
	{
		RadarTeamUI.mInstace.SendExitRoomRQ (true);
	}

	//点击退出当前房间
	void OnClickExitRoom()
	{
		RadarTeamUI.mInstace.SendExitRoomRQ (false);
	}

	//点击雷达关卡
	void OnClickradarItem(GameObject obj)
	{
		for (int i = 0; i < m_radarItems.Length; i++)
		{
			if (obj == m_radarItems [i].gameObject)
			{
				RadarTeamUI.mInstace.m_despUI.SetShow (true);
				GPSWarInfo info = Core.Data.gpsWarManager.GetGPSWarInfo (i + 1);

				RadarTeamUI.mInstace.m_despUI.SetGPSWarInfo (info);
			}
		}
	}

	public void SetShow(bool bShow)
	{
		RED.SetActive (bShow, this.gameObject);
	}



}
