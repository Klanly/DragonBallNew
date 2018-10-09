using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RadarRoomListUI : MonoBehaviour 
{
	public UIGrid m_grid;
	public GameObject m_radarBg;
	public GameObject m_radarDir;

	private Dictionary<int, RadarRoomItem> m_dicRoomList = new Dictionary<int, RadarRoomItem>();


	//更新房间列表
	public void UpdateRoom(SockRoomInfo room)
	{
		if (m_dicRoomList.ContainsKey (room.roomId))
		{
			m_dicRoomList [room.roomId].InitRoomItem (room);
		}
	}

	//添加房间
	public void AddRoom(SockRoomInfo room)
	{
		if (!m_dicRoomList.ContainsKey (room.roomId))
		{
			//to add...
			Object prefab = PrefabLoader.loadFromPack ("ZQ/roomItem");
			if (prefab != null)
			{
				GameObject obj = Instantiate (prefab) as GameObject;
				RED.AddChild (obj, m_grid.gameObject);
				RadarRoomItem roomItem = obj.GetComponent<RadarRoomItem> ();
				roomItem.InitRoomItem (room);
				m_dicRoomList.Add (room.roomId, roomItem);
			}
			else
			{
				RED.LogWarning ("ZQ/roomItem    not find");
			}
			m_grid.Reposition ();
		}
		else
		{
			UpdateRoom (room);
		}
	}

	//删除房间
	public void DelRoom(SockRoomInfo room)
	{
		if (m_dicRoomList.ContainsKey (room.roomId))
		{
			m_dicRoomList [room.roomId].transform.parent = null;
			Destroy (m_dicRoomList [room.roomId].gameObject);
			m_grid.Reposition ();
		}
	}

	public void SetShow(bool bShow)
	{
		RED.SetActive (bShow, this.gameObject);
	}

	void OnBtnBack()
	{
		RadarTeamUI.mInstace.m_operateUI.SetShow (true);
		SetShow (false);
	}

	void Update()
	{
		m_radarDir.transform.RotateAround(m_radarBg.transform.position, Vector3.forward, -180 * Time.deltaTime);
	}
}
