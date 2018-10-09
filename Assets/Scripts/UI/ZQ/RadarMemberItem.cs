using UnityEngine;
using System.Collections;

public class RadarMemberItem : MonoBehaviour 
{
	public UILabel m_txtLv;
	public UISprite m_spHead;
	public UILabel m_txtName;
	public UILabel m_txtAtk;
	public UILabel m_txtDef;
	public UILabel m_txtState;
	

	//设置队员信息
	public void SetMemberInfo(RoomMembersData data)
	{
		if(data == null)
		{
			m_txtState.text = Core.Data.stringManager.getString(5201);
		}
		else
		{
			if(data.memberState == 1)
			{
				m_txtState.text = Core.Data.stringManager.getString(5202);
			}
			else
			{
				m_txtState.text = Core.Data.stringManager.getString(5199);
			}
		}

		if(data != null)
		{
			if(int.Parse(Core.Data.playerManager.PlayerID) == data.memberId)
			{
				RED.SetActive(false, m_txtState.gameObject);
			}
			else
			{
				RED.SetActive(true, m_txtState.gameObject);
			}
		}
		else
		{
			RED.SetActive(true, m_txtState.gameObject);
		}

		if (data != null)
		{
			m_txtLv.text = "Lv" + data.memberLv.ToString ();
			if(data.iconId == 0)
			{
				data.iconId = 10142;
			}
			AtlasMgr.mInstance.SetHeadSprite (m_spHead, data.iconId.ToString());
			m_txtName.text = data.memberName;
			m_txtAtk.text = data.atk.ToString ();
			m_txtDef.text = data.def.ToString ();
		}
		else
		{
			m_txtLv.text = "";
			m_spHead.spriteName = "";
			m_txtName.text = "";
			m_txtAtk.text = "";
			m_txtDef.text = "";
		}
	}
}
