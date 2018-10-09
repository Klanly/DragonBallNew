using UnityEngine;
using System.Collections;

//created by zhangqiang at 2014-3-13
public class SwapTeamUI : MonoBehaviour 
{
	public UIButton m_btnSwap;
	public GameObject m_selTitle;

	public UILabel[] m_txtAttack;
	public UILabel[] m_txtDefence;
	public UILabel[] m_txtMember;

	public UISprite m_bgSelAttack;
	public UISprite m_bgSelDefence;

	public UILabel m_txtAtkTitle;
	public UILabel m_txtDefTitle;

	public UIGrid m_gridAttack;
	public UIGrid m_gridDefence;
	
	private bool m_bShow = true;

	private MonsterTeam m_curTeam ;
	private MonsterTeam m_attackTeam ;
	private MonsterTeam m_defenceTeam ;

	private Vector3 BTN_CHANGE_ATTACK = new Vector3(320,190,0);
	private Vector3 BTN_CHANGE_DEFENCE = new Vector3(320,-72,0);
	

	void Awake()
	{
		m_btnSwap.TextID = 5020;
		m_txtAtkTitle.text = Core.Data.stringManager.getString(5089);
		m_txtDefTitle.text = Core.Data.stringManager.getString(5090);

		InitUI();
	}


	void InitUI()
	{
		m_curTeam = Core.Data.playerManager.RTData.curTeam;
	
		for(int i = 1; i <= 2; i++)
		{
			MonsterTeam team = Core.Data.playerManager.RTData.getTeam(i);
			if(team ==null)
			{
				continue;
			}

			UIGrid gridParent = null;
			if(i == 1)
			{
				m_attackTeam = team;
				gridParent = m_gridAttack;
			}
			else
			{
				m_defenceTeam = team;
				gridParent = m_gridDefence;
			}

			m_txtAttack[i-1].text = team.teamAttack.ToString();
			m_txtDefence[i-1].text = team.teamDefend.ToString();
			m_txtMember[i-1].text = team.validateMember.ToString() + "/" + team.capacity.ToString();

			for(int j= 0; j < team.capacity; j++)
			{
				Monster monster = team.getMember(j);
				if(monster != null)
				{
					 CreateMonsterHeadCell(monster, gridParent.transform, j.ToString());
				}
			}

			gridParent.Reposition();
		}

		ModifySwapBtnPos();
	}


	private void ModifySwapBtnPos()
	{
		if(m_curTeam == m_attackTeam)
		{
			RED.SetActive(true, m_bgSelAttack.gameObject);
			RED.SetActive(false, m_bgSelDefence.gameObject);

			m_btnSwap.transform.localPosition = BTN_CHANGE_DEFENCE;
			m_selTitle.transform.localPosition = BTN_CHANGE_ATTACK;
		}
		else if(m_curTeam == m_defenceTeam)
		{
			RED.SetActive(false, m_bgSelAttack.gameObject);
			RED.SetActive(true, m_bgSelDefence.gameObject);

			m_btnSwap.transform.localPosition = BTN_CHANGE_ATTACK;
			m_selTitle.transform.localPosition = BTN_CHANGE_DEFENCE;
		}
	}

	MonsterHeadCell CreateMonsterHeadCell(Monster monster, Transform parent, string strName)
	{
		Object prefab = PrefabLoader.loadFromPack("ZQ/MonsterHeadCell");
		if(prefab != null)
		{
			GameObject obj = Instantiate(prefab) as GameObject;
			obj.transform.parent = parent;
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localScale = Vector3.one;
			obj.transform.localEulerAngles = Vector3.zero;
			obj.name = strName;
			MonsterHeadCell cell = obj.GetComponent<MonsterHeadCell>();
			cell.InitUI(monster);
		}
		return null;
	}

	public void SetShow(bool bShow)
	{
		if(m_bShow ==bShow)
		{
			return;
		}
		m_bShow = bShow;
		RED.SetActive(m_bShow, this.gameObject);
	}

	void OnBtnSwap()
	{
		if (Core.Data.playerManager.Lv < 20)
		{
			string strText = Core.Data.stringManager.getString (5087);
			strText = string.Format (strText, 20);
			SQYAlertViewMove.CreateAlertViewMove (strText);
			return;
		}

		if(m_curTeam == m_attackTeam)
		{
			m_curTeam = m_defenceTeam;
		}
		else if(m_curTeam == m_defenceTeam)
		{
			m_curTeam = m_attackTeam;
		}

		SwapTeam();
		ModifySwapBtnPos();
	}

	void  SwapTeam()
	{
		HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
		
		SwapTeamParam param = new SwapTeamParam();
		param.gid = Core.Data.playerManager.PlayerID;
		param.teamid = (Core.Data.playerManager.RTData.curTeamId == MonsterTeam.MAIN_TEAM_ID ? MonsterTeam.SUB_TEAM_ID : MonsterTeam.MAIN_TEAM_ID);

		task.AppendCommonParam(RequestType.SWAP_TEAM, param);
		
		task.ErrorOccured += testHttpResp_Error;
		task.afterCompleted += testHttpResp_UI;
		
		RED.Log("send swap team msg");
		RED.Log("team id : " + param.teamid);

		//then you should dispatch to a real handler
		task.DispatchToRealHandler ();
	}

	#region 网络返回
	
	void testHttpResp_UI (BaseHttpRequest request, BaseResponse response)
	{
		if (response.status != BaseResponse.ERROR) 
		{
			HttpRequest rq = request as HttpRequest;
			switch(rq.Type)
			{
			case RequestType.SWAP_TEAM:
				{
					TeamUI.mInstance.SwapTeam ();
					TeamUI.mInstance.FreshCurTeam ();
					DBUIController.mDBUIInstance.RefreshUserInfoWithoutShow ();
					break;
				}
			}
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
