using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//created by zhangqiang At 2013-3-14
public class ModifyMonsterUI : MonoBehaviour 
{
	public UIGrid m_grid;
	
	public ModifyMonsterCell m_curSelMonster;

	private bool m_bShow;

	private int m_nCurTeamID;
	

	void Start()
	{
		InitUI();
	}

	public void InitUI()
	{
		if(m_nCurTeamID != Core.Data.playerManager.RTData.curTeamId)
		{
			while(m_grid.transform.childCount > 0)
			{
				Transform tf = m_grid.transform.GetChild(0);
				tf.parent = null;
				Destroy(tf.gameObject);
			}

			m_nCurTeamID = Core.Data.playerManager.RTData.curTeamId;
			int count = 0;
			MonsterTeam team = Core.Data.playerManager.RTData.curTeam;
			for(int i = 0; i < team.capacity; i++)
			{
				Monster monster = team.getMember(i);
				if(monster != null)
				{
					ModifyMonsterCell cell = CreateMonsterCell(monster);
					if(count == 0)
					{
						cell.SetSelected(true);
						m_curSelMonster = cell;
					}
					else
					{
						cell.SetSelected(false);
					}
					cell.name = (i + 11).ToString();
					count++;
				}
			}

			m_grid.Reposition();
		}
	}


	private ModifyMonsterCell CreateMonsterCell(Monster data)
	{
		Object prefab = PrefabLoader.loadFromPack("ZQ/ModifyMonsterCell");
		if(prefab != null)
		{
			GameObject obj = Instantiate(prefab) as GameObject;
			obj.transform.parent = m_grid.transform;
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localScale = Vector3.one;
			obj.transform.localEulerAngles = Vector3.zero;

			ModifyMonsterCell cell = obj.GetComponent<ModifyMonsterCell>();
			cell.InitUI(data);

			return cell;
		}
		return null;
	}

	public void OnChangePos(ModifyMonsterCell cell)
	{
		if(m_curSelMonster == cell)
		{
			return;
		}

		cell.SetSelected(false);
		m_curSelMonster.SetSelected(true);

		string strName = m_curSelMonster.name;
		m_curSelMonster.name = cell.name;
		cell.name = strName;

		MonsterTeam team = Core.Data.playerManager.RTData.curTeam;

		int cellPos = team.GetMonsterPos(cell.m_monster.pid);

		int curSelPos = team.GetMonsterPos(m_curSelMonster.m_monster.pid);

        team.removeMember(curSelPos);
        team.removeMember(cellPos);
		team.setMember(cell.m_monster, curSelPos);
		team.setMember(m_curSelMonster.m_monster, cellPos);
		m_grid.Reposition();

		SendChangePosMsg(cell.m_monster.pid, m_curSelMonster.m_monster.pid);
	}



	void SendChangePosMsg(int monsterId, int targetMonsterId)
	{
		HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
		
		SwapMonsterPosParam param = new SwapMonsterPosParam();
		param.gid = Core.Data.playerManager.PlayerID;
		param.sroleid = monsterId;
		param.troleid = targetMonsterId;

		task.AppendCommonParam(RequestType.SWAP_MONSTER_POS, param);
		
		task.ErrorOccured += testHttpResp_Error;
		task.afterCompleted += testHttpResp_UI;

		//then you should dispatch to a real handler
		task.DispatchToRealHandler ();
	}

	public void SetShow(bool bShow)
	{
		if(m_bShow == bShow)
		{
			return;

		}
		m_bShow = bShow;
		RED.SetActive(m_bShow, this.gameObject);
		if(bShow)
		{
			InitUI();
		}
	}

	#region 网络返回
	
	void testHttpResp_UI (BaseHttpRequest request, BaseResponse response)
	{
		if (response.status != BaseResponse.ERROR) 
		{
			TeamUI.mInstance.FreshCurTeam();
//			TeamUI.mInstance.curMonster = Core.Data.playerManager.RTData.curTeam.getMember (TeamUI.mInstance.mSelectIndex);
		} 
		else 
		{
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getNetworkErrorString(response.errorCode));
		}
	}
	
	void testHttpResp_Error (BaseHttpRequest request, string error)
	{
		ConsoleEx.DebugLog ("---- Http Resp - Error has ocurred." + error);
	}
	
	#endregion
}
