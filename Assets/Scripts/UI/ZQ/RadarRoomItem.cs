using UnityEngine;
using System.Collections;

public class RadarRoomItem : MonoBehaviour 
{
	public UILabel m_txtName;
	public UILabel m_txtLv;
	public UILabel m_txtCount;
	public UIButton m_btnJoin;

	private SockRoomInfo m_data;

	void Start()
	{
		m_btnJoin.TextID = 5188;
	}

	public void InitRoomItem( SockRoomInfo room)
	{
		m_data = room;
		m_txtLv.text = "Lv" + room.masterLevel.ToString ();
		m_txtCount.text = room.nowRoomMemberNum.ToString () + "/" + room.roomMaxMemberNum.ToString ();
		m_txtName.text = room.masterName;

		m_btnJoin.isEnabled = (room.joinState == 1);
	}

	void OnClickJoin()
	{
		RadarTeamUI.mInstace.SendJoinRoomRQ (m_data.roomId);
	}
}
