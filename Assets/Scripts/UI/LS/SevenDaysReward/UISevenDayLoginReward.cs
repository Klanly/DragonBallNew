using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UISevenDayLoginReward : RUIMonoBehaviour 
{
	public GameObject tableroot;
	public UITable mTable;

	public List<UISevenDayRewardCell> mCellList = new List<UISevenDayRewardCell>();

	const int Max_day = 7;

	public void DeleteCell()
	{
		foreach(UISevenDayRewardCell cell in mCellList)
		{
			cell.dealloc();
		}
		Destroy(gameObject);
	}

	void InitChild()
	{
		Object prefab = PrefabLoader.loadFromPack ("LS/pbLSSevenDaysCell");
		if (prefab != null)
		{
			for(int i=0; i<Max_day; i++)
			{
				GameObject obj = Instantiate (prefab) as GameObject;
				obj.name = (i+1).ToString();
				RED.AddChild (obj, tableroot);
				UISevenDayRewardCell mm = obj.GetComponent<UISevenDayRewardCell> ();
				mCellList.Add(mm);
			}

		}
		mTable.Reposition();
	}

	public void SendMsg()
	{
		ComLoading.Open();
		if(NoticeManager.openSign && NoticeManager.firstShowState == 1)
		{
			ComLoading.Close();
			SetSevenRewardDetail(NoticeManager._AlertInfo.sevenSgin);
			if(Core.Data.guideManger.isGuiding)	Core.Data.guideManger.AutoRUN();
			
		}
		else
		{
			GetSevenRewardListParam param = new GetSevenRewardListParam(int.Parse(Core.Data.playerManager.PlayerID));
			HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
			
			task.AppendCommonParam(RequestType.SEVENDAYREWARD, param);
			
			task.ErrorOccured += testHttpResp_Error;
			task.afterCompleted += testHttpResp_UI;
            
            task.DispatchToRealHandler ();
		}

	}

	void testHttpResp_UI(BaseHttpRequest request, BaseResponse response)
	{
		ComLoading.Close();
		if(response != null)
		{
			if(response.status != BaseResponse.ERROR)
			{
				SevenDaysListResponse sevenres = response as SevenDaysListResponse;
			    SetSevenRewardDetail(sevenres.data);
				if(Core.Data.guideManger.isGuiding)	Core.Data.guideManger.AutoRUN();

			}

		}

	}

	void testHttpResp_Error (BaseHttpRequest request, string error)
	{
		ComLoading.Close();
		ConsoleEx.DebugLog ("---- Http Resp - Error has ocurred." + error);
	}

	void SetSevenRewardDetail(SevenDaysListData data)
	{
		InitChild();
		for(int i=0; i<data.awads.Length; i++)
		{
			if(i<data.index)mCellList[i].Show(SevenDayCellType.SevenDayCellType_HAVETAKE,data.awads,i+1);
			else if(i==data.index)
			{
				if(data.canGain)mCellList[i].Show(SevenDayCellType.SevenDayCellType_WAITTAKE,data.awads,i+1);
				else 
				{
					mCellList[i].Show(SevenDayCellType.SevenDayCellType_NOTOPEN,data.awads,i+1);
				}
			}
			else if(i>data.index)
			{
				mCellList[i].Show(SevenDayCellType.SevenDayCellType_NOTOPEN,data.awads,i+1);
            }
        }
//		SpringPanel.Begin(,,12f);
    }
    


	void OnDestroy()
	{
		tableroot = null;
		mTable = null;
		mCellList.Clear();
	}


}
