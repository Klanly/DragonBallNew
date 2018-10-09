using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnnounceMrg 
{

    static AnnounceMrg m_AnnounceMrg;
    public static AnnounceMrg GetInstance()
    {
        if(m_AnnounceMrg == null)
        {
            m_AnnounceMrg = new AnnounceMrg();
        }
        return m_AnnounceMrg;
    }

    List<UIAnnounceCell> mUIAnnounceCellList = new List<UIAnnounceCell>();

    public string[] mTitleList;
    public string[] mContentList;

    public GameObject _Grid;
    public UIAnnounceCell mChooseScript = null;

    UIAnnounce mUIAnnounce = null;


    void CreateaAnnounceUI()
    {
        UnityEngine.Object obj = PrefabLoader.loadFromPack("LS/pbLSAnnounce");
        if(obj != null)
        {
            GameObject go = RUIMonoBehaviour.Instantiate(obj) as GameObject;
            mUIAnnounce = go.GetComponent<UIAnnounce>();
    		//RED.TweenShowDialog (go);
    		RED.AddChild(go.gameObject,DBUIController.mDBUIInstance._TopRoot);

        }

    }
    
    public void SetInfoPanel(bool key)
    {
        if(mUIAnnounce == null)
        {
            CreateaAnnounceUI();
        }

		mUIAnnounce.OnShow();
        mUIAnnounce.gameObject.SetActive(key);
    }

    public void CreateAnnounceCell()
    {
        if(mUIAnnounceCellList.Count != 0)return;
        
        GameObject obj1 = PrefabLoader.loadFromPack("LS/pbLSAnnounceCell") as GameObject ;
        
        if(obj1 != null)
        { 
			for(int i=0; i<mTitleList.Length; i++)
            {
                GameObject go = NGUITools.AddChild (_Grid, obj1);
                go.gameObject.name = (1000 + i).ToString();
                UIAnnounceCell mm = go.gameObject.GetComponent<UIAnnounceCell>();
                mUIAnnounceCellList.Add (mm);
            }
			mUIAnnounceCellList[0].SetBtnStatus(true);
			mChooseScript = mUIAnnounceCellList[0];
        }
    
	}

	public void AnnounceRequest(GameObject obj)
	{
		_Grid = obj;
		if(NoticeManager.openAnnounce) {
			ComLoading.Close();
			AnnounceDetail(NoticeManager._AlertInfo.notice);
		} else {

			if(!NoticeManager.isFirstShow && NoticeManager._AlertInfo.notice != null)return;

			AnnounceParam param = new AnnounceParam(Core.Data.playerManager.PlayerID, 1);
			HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
			task.AppendCommonParam(RequestType.ANNOUNCE_INFO, param);
			
			task.afterCompleted += CompletedRequest;  
			task.DispatchToRealHandler();
        }
    }
    
    public void CompletedRequest(BaseHttpRequest request, BaseResponse response)
    {
		ComLoading.Close();
        if(response != null && response.status != BaseResponse.ERROR)
        {
            AnnouceResponse mresponse = response as AnnouceResponse;
			if(mresponse.data == null || mresponse.data.Title == null || mresponse.data.Title.Length == 0)
			{
				return;
			}
			if(mUIAnnounce == null)
			{
				return;
			}
			else
			{
				AnnounceDetail(mresponse.data);
			}
        }
		else if(response != null)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
		}
    }

	void AnnounceDetail(AnnouceResponseData data)
	{
		mTitleList = data.Title;
		mContentList = data.Content;
		CreateAnnounceCell();
		for(int i=0; i<mUIAnnounceCellList.Count; i++)
		{
			if(i < mTitleList.Length)
			{
				mUIAnnounceCellList[i].OnShow(i, mTitleList[i]);
				mUIAnnounceCellList[i].SetActive(true);
			}
		}
		mUIAnnounce.SetContent(mContentList[0]);
		mUIAnnounce.Reposition();
	}
	
	public void DeleteCell()
	{
		for(int i=0; i<mUIAnnounceCellList.Count; i++)
        {
            mUIAnnounceCellList[i].dealloc();
        }
        mUIAnnounceCellList.Clear();
		mUIAnnounce = null;
    }

    public void SetAnnounceContent(int _index, UIAnnounceCell _Script)
    {
        if(_index < mTitleList.Length)
        {
            mUIAnnounce.SetContent(mContentList[_index]);
        }
		if(mChooseScript != null)
		{
			mChooseScript.SetBtnStatus(false);
		}
		mChooseScript = _Script;
		{
			mChooseScript.SetBtnStatus(true);
		}

    }

}
