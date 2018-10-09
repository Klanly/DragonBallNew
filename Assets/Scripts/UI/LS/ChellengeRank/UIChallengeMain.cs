using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIChallengeMain : MonoBehaviour 
{
	public UIGrid _Grid;
	public UIScrollView _ScrollView;


	List<UIChallengeRankCell> _UIChallengeRankCelList = new List<UIChallengeRankCell>();
	List<UITianXiaRankCell> _UITianXiaRankCellList = new  List<UITianXiaRankCell>();

	bool m_IsSingle;

	public void ChellengeRankRequest()
	{	
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.GET_CHALLENGE_RANK, new GetChellengeRankParam(int.Parse(Core.Data.playerManager.PlayerID)));
		
		task.afterCompleted = AfterComplete;
		task.ErrorOccured = testHttpResp_Error;
		
		task.DispatchToRealHandler ();
		ComLoading.Open();
	}

	public void TianXiaRankRequest(int _NeedNum, bool _IsSingle = false)
	{
		if(!Core.Data.guideManger.isGuiding)
		{
			m_IsSingle = _IsSingle;
			HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
			if(!_IsSingle)
			{
				task.AppendCommonParam(RequestType.GET_TIANXIADIYI_RANK, new GetTianXiaRankParam(int.Parse(Core.Data.playerManager.PlayerID), _NeedNum));
				task.afterCompleted = AfterComplete;
				task.ErrorOccured = testHttpResp_Error;
			}
			else
			{
				task.AppendCommonParam(RequestType.GET_TIANXIADIYI_RANKSINGLE, new GetTianXiaRankParam(int.Parse(Core.Data.playerManager.PlayerID), _NeedNum));
				task.afterCompleted = AfterComplete;
				task.ErrorOccured = testHttpResp_Error;
			}
	        task.DispatchToRealHandler ();
			ComLoading.Open();
		}
    }

	void testHttpResp_Error (BaseHttpRequest request, string error)
	{
		ConsoleEx.DebugLog ("---- Http Resp - Error has ocurred." + error);
	}  

	void AfterComplete(BaseHttpRequest request, BaseResponse response)
	{
		if(response != null && response.status != BaseResponse.ERROR)
		{
			HttpRequest httprequest = request as HttpRequest;
			if(httprequest.Act == HttpRequestFactory.ACTION_GET_CHALLENGE_RANK)
			{		
				GetChallengeRankResponse res = response as GetChallengeRankResponse;
				if(res != null)
				{
					CreateRobCell(res);
					_Grid.Reposition();

                }
			}
			else if(httprequest.Act == HttpRequestFactory.ACTION_GET_TIANXIADIYI_RANK)
			{		
				GetChallengeRankResponse res = response as GetChallengeRankResponse;
				if(res != null)
				{
					TianXiaCell(res);
                    _Grid.Reposition();
                }
            }
			ComLoading.Close();
        }
		else if(response != null && response.status == BaseResponse.ERROR)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
		}
    }
    
    void CreateRobCell(GetChallengeRankResponse res)
	{
		GameObject obj1 = PrefabLoader.loadFromPack("LS/pbLSChallengeRankCell") as GameObject ;
		if(obj1 != null)
		{ 
			for(int i=0; i<res.data.list.Length; i++)
			{
				GameObject go = NGUITools.AddChild (_Grid.gameObject, obj1);
				go.gameObject.name = (1000+i).ToString();
				UIChallengeRankCell mm = go.gameObject.GetComponent<UIChallengeRankCell>();
				mm.OnShow(res.data.list[i], i+1);
				_UIChallengeRankCelList.Add (mm);
			}
			if(res.data.list.Length < 7)_ScrollView.disableDragIfFits = true;
			else _ScrollView.disableDragIfFits = false;
		}
	}

	void TianXiaCell(GetChallengeRankResponse res)
	{
		GameObject obj1 = PrefabLoader.loadFromPack("LS/pbLSTianXiaChallengeRankCell") as GameObject ;
		if(obj1 != null)
		{ 
			for(int i=0; i<res.data.list.Length; i++)
			{
				GameObject go = NGUITools.AddChild (_Grid.gameObject, obj1);
				go.gameObject.name = (1000+i).ToString();
				UITianXiaRankCell mm = go.gameObject.GetComponent<UITianXiaRankCell>();
				mm.OnShow(res.data.list[i], i+1, res.data.list[i].gid);
                _UITianXiaRankCellList.Add (mm);
            }
			if(res.data.list.Length < 7)_ScrollView.disableDragIfFits = true;
			else _ScrollView.disableDragIfFits = false;
        }
    }
    
	public void DeleteCell()
	{
		for(int i=0; i<_UIChallengeRankCelList.Count; i++)
		{
			_UIChallengeRankCelList[i].dealloc();
		}
		_UIChallengeRankCelList.Clear();

		for(int i=0; i<_UITianXiaRankCellList.Count; i++)
		{
			_UITianXiaRankCellList[i].dealloc();
		}
		_UITianXiaRankCellList.Clear();
    }

	void OnDestroy()
	{
		_Grid = null;

	}
}
