using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZhaomuMgr
{
	private Dictionary<int, int> m_dicTimer;							//key is pron  value is task._id
	private Dictionary<int, ZhaoMuStateData> m_dicZhaomuData;			//key is pron

	private bool m_bInited = false;
	public bool bInit
	{
		get
		{
			return m_bInited;
		}
		set
		{
			m_bInited = value;
		}
	}

	public ZhaomuMgr()
	{
		m_dicTimer = new Dictionary<int, int> ();
		m_dicZhaomuData = new Dictionary<int, ZhaoMuStateData> ();
	}


	public ZhaoMuStateData GetZhaomuState(int pron)
	{
		if (m_dicZhaomuData.ContainsKey (pron))
			return m_dicZhaomuData [pron];

		RED.LogWarning ("not find zhaomustatedata   " + pron);
		return null;
	}

	//发送招募时间状态请求
	public void SendZhaomuStateMsg()
	{
		if(m_bInited)
			return;
		if(Core.Data.guideManger.isGuiding) 
			return;

		HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
		ZhaoMuStateParam param = new ZhaoMuStateParam (Core.Data.playerManager.PlayerID);

		task.AppendCommonParam(RequestType.ZHAOMU_STATE, param);

		task.ErrorOccured += testHttpResp_Error;
		task.afterCompleted += testHttpResp_UI;

		//then you should dispatch to a real handler
		task.DispatchToRealHandler ();
	}

	//招募是否free
	public bool IsZhaomuFree(int pron)
	{
		if (m_dicZhaomuData.ContainsKey (pron))
		{
			ZhaoMuStateData data = m_dicZhaomuData [pron];
			return (data.freecount > 0 && data.coolTime == 0);
		}
		RED.LogWarning ("not find zhao mu pron  " + pron);
		return false;
	}
		
	void StartTimer(ZhaoMuStateData state)
	{
		if (m_dicZhaomuData.ContainsKey (state.pron))
		{
			m_dicZhaomuData [state.pron] = state;
		}
		else
		{
			m_dicZhaomuData.Add (state.pron, state);
		}
			
		if (m_dicTimer.ContainsKey (state.pron))
		{
			Core.TimerEng.deleteTask (m_dicTimer [state.pron]);
			m_dicTimer.Remove (state.pron);
		}
			
		long curTime = Core.TimerEng.curTime;
		if (state.coolTime > 0)
		{
			TimerTask task = new TimerTask (curTime, curTime + state.coolTime, 1, ThreadType.MainThread);
			task.onEventEnd = OnTimeEnd;
			task.onEvent = OnTimer;
			task.DispatchToRealHandler ();

			m_dicTimer.Add (state.pron, task._Id);
		}
	}


	void OnTimeEnd(TimerTask task)
	{
		int pron = -1;
		foreach (KeyValuePair<int, int> itor in m_dicTimer)
		{
			if (itor.Value == task._Id)
			{
				pron = itor.Key;
				break;
			}
		}

		if(pron != -1)
		{
			if (m_dicZhaomuData.ContainsKey (pron))
			{
				ZhaoMuStateData state = m_dicZhaomuData [pron];
				state.coolTime = 0;

				m_dicTimer.Remove (pron);

				Core.TimerEng.deleteTask (task);
			}
			else
			{
				RED.LogWarning ("zhaomu pron data not find   " + pron);
			}
		}
		else
		{
			RED.LogWarning ("zhaomu timer task id not find   " + task._Id);
		}
	}

	void OnTimer(TimerTask task)
	{
		int pron = -1;
		foreach (KeyValuePair<int, int> itor in m_dicTimer)
		{
			if (itor.Value == task._Id)
			{
				pron = itor.Key;
				break;
			}
		}

		if(pron != -1)
		{
			if (m_dicZhaomuData.ContainsKey (pron))
			{
				ZhaoMuStateData state = m_dicZhaomuData [pron];
				state.coolTime = task.leftTime;
			}
			else
			{
				RED.LogWarning ("zhaomu pron data not find   " + pron);
			}
		}
		else
		{
			RED.LogWarning ("zhaomu timer task id not find   " + task._Id);
		}
	}


	public void OnZhaomuMsg(BaseHttpRequest request, BaseResponse reponse)
	{
		if (reponse.status != BaseResponse.ERROR)
		{
			HttpRequest req = request as HttpRequest;

			if (req.Type == RequestType.ZHAOMU)
			{
				ZhaoMuResponse resp = reponse as ZhaoMuResponse;

				StartTimer (resp.data.status);
			}
		}
	}

	void testHttpResp_UI (BaseHttpRequest request, BaseResponse reponse)
	{
		if (reponse != null && reponse.status != BaseResponse.ERROR) 
		{
			HttpRequest req = request as HttpRequest;
			if (req.Type == RequestType.ZHAOMU_STATE)
			{
				m_bInited = true;
				ZhaoMuStateResponse resp = reponse as ZhaoMuStateResponse;

				m_dicZhaomuData.Clear ();
				for (int i = 0; i < resp.data.Length; i++)
				{
					//招募未解锁，重置时间为free
					if (!Core.Data.BuildingManager.ZhaoMuUnlock)
					{
						if (resp.data [i].pron == 51001 || resp.data [i].pron == 51003)
						{
							resp.data [i].coolTime = 0;
						}
					}

					StartTimer (resp.data [i]);
				}
			}
		} 
	}

	void testHttpResp_Error (BaseHttpRequest request, string error)
	{
		RED.Log ("---- Http Resp - Error has ocurred." + error);
	}
}
