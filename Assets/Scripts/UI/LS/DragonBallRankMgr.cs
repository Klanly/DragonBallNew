using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DragonBallRankMgr 
{

    static DragonBallRankMgr mDragonBallRankMgr;
    public static DragonBallRankMgr GetInstance()
    {
        if(mDragonBallRankMgr == null)
        {
            mDragonBallRankMgr = new DragonBallRankMgr();
        }
        return mDragonBallRankMgr;
    }

    UIDragonBallRank mUIDragonBallRank = null;
	UIDragonBallRankCheckInfo mUIDragonBallRankCheckInfo = null;
    List<UIDragonBallRankCell> mUIDragonBallRankCellList = new List<UIDragonBallRankCell>();
	public List<RankRoleIcon> mRankRoleIcon = new List<RankRoleIcon>();

	public int _NowChooseIndex;

	public GameObject mRoot;

	public GetNewFinalTrialRankCheckInfoResponse m_response;


    void InitRankPanel()
    {
        UnityEngine.Object obj = PrefabLoader.loadFromPack("LS/pbLSDragonBallRank");
        if(obj != null)
        {
            GameObject go = RUIMonoBehaviour.Instantiate(obj) as GameObject;
            mUIDragonBallRank = go.GetComponent<UIDragonBallRank>();
            RED.AddChild(go.gameObject,DBUIController.mDBUIInstance._bottomRoot);
        }
    }

	void InitRankCheckInfoPanel()
	{
		UnityEngine.Object obj = PrefabLoader.loadFromPack("LS/pbLSDragonBallRankCheckInfo");
		if(obj != null)
		{
			GameObject go = RUIMonoBehaviour.Instantiate(obj) as GameObject;
			mUIDragonBallRankCheckInfo = go.GetComponent<UIDragonBallRankCheckInfo>();
			RED.AddChild(go.gameObject,DBUIController.mDBUIInstance._bottomRoot);
		}
	}

	public void InitRankCheckInfo(GameObject mGrid)
	{
		if(m_response == null)
		{
			RED.Log("FinalTrialRankCheckInfoResponse is nulll");
		}
		GameObject obj1 = PrefabLoader.loadFromPack("LS/pbLSRankRole") as GameObject ;
		int length = m_response.data.detail.monteam.Length;
        for(int i=0; i<length; i++)
		{
			if(obj1 != null)
			{ 
				GameObject go = NGUITools.AddChild (mGrid, obj1);
				go.gameObject.name = "pbLSRankRole" + i.ToString();
				RankRoleIcon mm = go.gameObject.GetComponent<RankRoleIcon>();
				mRankRoleIcon.Add(mm);

//				mm.SetDetail(m_response.data[i], i);
            }
		}
		mRankRoleIcon[0].SetChoose(true);
		_NowChooseIndex = 0;
	}
            
    public void SetRankPanel(bool key)
    {
		if(mUIDragonBallRank == null)
        {
            InitRankPanel();
        }
		FinalTrialRankRequest(5);
        mUIDragonBallRank.gameObject.SetActive(key);
    }

	public void SetRankCheckInfoPanel(bool key)
	{
		if(mUIDragonBallRankCheckInfo == null)
		{
			InitRankCheckInfoPanel();
		}
	}


	public void InitRankCell(GetNewFinalTrialRankResponse mresponse)
    {
        if(mUIDragonBallRankCellList.Count != 0)return;
        
        GameObject obj1 = PrefabLoader.loadFromPack("LS/pbLSDragonBallRankCell") as GameObject ;

		int m_Length = 0;
		if(mresponse.data.ranklist.Length >= 50)m_Length = 50;
		else m_Length = mresponse.data.ranklist.Length;

        if(obj1 != null)
        { 
			for(int i=0; i<m_Length; i++)
            {
                GameObject go = NGUITools.AddChild (mRoot, obj1);
                go.gameObject.name = (1000 + i).ToString();
                UIDragonBallRankCell mm = go.gameObject.GetComponent<UIDragonBallRankCell>();
                mUIDragonBallRankCellList.Add (mm);
				mm.gameObject.SetActive(false);
            }
//			int _MaxItemNum = mresponse.data.ranklist.Length;
        }

    }

	public void RefreshCell(bool key, GetNewFinalTrialRankResponse mresponse)
    {
		int mindex = 0;
		if(mresponse.data.ranklist.Length > 50)mindex = 49;
		else mindex = mresponse.data.ranklist.Length - 1;
		for(; mindex>=0; mindex-- )
        {
			mUIDragonBallRankCellList[mindex].OnShow(mindex, mresponse.data.ranklist[mindex]);
            mUIDragonBallRankCellList[mindex].gameObject.SetActive(true);
        }
    }

	public void FinalTrialRankRequest(int type)
	{
//		DeleteRankCellList();
//		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
//		task.AppendCommonParam(RequestType.NEW_FINALTRIAL_GETRANK, new NewFinalTrialGetRankParam(int.Parse(Core.Data.playerManager.PlayerID), (int)FinalTrialMgr.GetInstance().NowEnum, type));
//		
//		task.afterCompleted = SetFinalTrialRankData;
//		
//		task.DispatchToRealHandler ();
//		ComLoading.Open();
	}

	public void FinalTrialRankCheckInfoRequest(int mEnemyid)
	{
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.CHECK_TEAMRANKINFO, new NewFinalTrialRankCheckInfoParam(int.Parse(Core.Data.playerManager.PlayerID), mEnemyid));
		
		task.afterCompleted = SetFinalTrialRankData;
		
		task.DispatchToRealHandler ();
		ComLoading.Open();
    }

	public void SetFinalTrialRankData(BaseHttpRequest request, BaseResponse response)
	{
		ComLoading.Close();
		if(response != null && response.status != BaseResponse.ERROR)
		{
			HttpRequest httprequest = request as HttpRequest;
			if(httprequest.Act == HttpRequestFactory.ACTION_NEW_FINALTRIAL_GETRANK)
			{
				GetNewFinalTrialRankResponse mresponse = response as GetNewFinalTrialRankResponse;
				if(mresponse != null)
				{
					if(mresponse.data == null)
					{
						ConsoleEx.DebugLog("CheckFinalTrialRankResponse's data is null");
					}
					else
					{
						mUIDragonBallRank.OnShow(mresponse);
						InitRankCell(mresponse);
						RefreshCell(true,mresponse);
						UIGrid mm = mRoot.GetComponent<UIGrid>();
						mm.Reposition();
						if(mresponse.data.ranklist.Length <= 6)mUIDragonBallRank.SetDragPanel(true);
						else mUIDragonBallRank.SetDragPanel(false);
					}
				}
			}
			else if(httprequest.Act == HttpRequestFactory.ACTION_CHECK_TEAMRANKINFO)
			{
				GetNewFinalTrialRankCheckInfoResponse mresponse = response as GetNewFinalTrialRankCheckInfoResponse;
				if(mresponse != null)
				{
					if(mUIDragonBallRankCheckInfo != null)
					{
						mUIDragonBallRankCheckInfo.gameObject.SetActive(true);
						mUIDragonBallRankCheckInfo.OnShow(mresponse);
                    }
					else
					{
						m_response = mresponse;
						SetRankCheckInfoPanel(true);
						mUIDragonBallRankCheckInfo.OnShow(mresponse);
					}

				}
			}

		}

	}

	public void SetCheckInfoListChoose(int m_NowChooseIndex)
	{
		if(_NowChooseIndex >= 0 && _NowChooseIndex < mRankRoleIcon.Count)
		{
			mRankRoleIcon[_NowChooseIndex].SetChoose(false);
			_NowChooseIndex = m_NowChooseIndex;
		}
		else
		{
			ConsoleEx.DebugLog("_NowChooseIndex is not correct key");
		}

	}

	public void SetChooseDetail(RankRoleIcon _mm)
	{
		mUIDragonBallRankCheckInfo._RankRoleIcon = _mm;
		mUIDragonBallRankCheckInfo.SetChoose();
	}

	public void DestoryChild()
	{
		for(int i=0; i<mRankRoleIcon.Count; i++)
		{
			mRankRoleIcon[i].dealloc();
		}
		mRankRoleIcon.Clear();
	}

	public void DeleteRankCellList()
	{
		if(mUIDragonBallRankCellList.Count != 0)
		{
			for(int i=0; i<mUIDragonBallRankCellList.Count; i++)
			{
				mUIDragonBallRankCellList[i].dealloc();
			}
		}
		mUIDragonBallRankCellList.Clear();
	}
    
}
