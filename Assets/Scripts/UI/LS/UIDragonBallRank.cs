using UnityEngine;
using System.Collections;

public class UIDragonBallRank : RUIMonoBehaviour {

    public GameObject _Grid;
    public UILabel[] mLabel;
    public UISprite[] mBtnSpriteY;
    public UISprite[] mBtnSpriteG;
	UIScrollView mView;
	UIGrid mGrid;



	public void OnShow(GetNewFinalTrialRankResponse mresponse)
    {
		mGrid.Reposition();
		mView.ResetPosition();
    }

	public void SetDragPanel(bool _isdrag)
	{
		mView.disableDragIfFits = _isdrag;

	}

	void ClearBtnSprite()
	{
		mBtnSpriteY[0].gameObject.SetActive(true);
		mBtnSpriteY[1].gameObject.SetActive(true);
		mBtnSpriteY[2].gameObject.SetActive(true);
		mBtnSpriteY[3].gameObject.SetActive(true);
		mBtnSpriteY[4].gameObject.SetActive(true);
		
		mBtnSpriteG[0].gameObject.SetActive(false);
		mBtnSpriteG[1].gameObject.SetActive(false);
		mBtnSpriteG[2].gameObject.SetActive(false);
		mBtnSpriteG[3].gameObject.SetActive(false);
		mBtnSpriteG[4].gameObject.SetActive(false);
	}

	// Use this for initialization
	void Start () 
    {
//        DragonBallRankMgr.GetInstance().InitRankCell(_Grid);
		SetBtnContent(RankBtnType.Five_Formation);
		mView = _Grid.transform.parent.gameObject.GetComponent<UIScrollView>();
		mGrid = _Grid.GetComponent<UIGrid>();

		DragonBallRankMgr.GetInstance().mRoot = _Grid;
	}
	
    void Back_OnClick()
    {
        gameObject.SetActive(false);
		DragonBallRankMgr.GetInstance().DeleteRankCellList();
		Destroy(gameObject);
    }

	void OnDestroy()
	{
		_Grid = null;
		mView = null;
		mGrid = null;
	}

    void Five_OnClick()
    {
		SetBtnContent(RankBtnType.Five_Formation);
		DragonBallRankMgr.GetInstance().FinalTrialRankRequest(5);
    }

	void Six_OnClick()
    {
		SetBtnContent(RankBtnType.Six_Formation);
		DragonBallRankMgr.GetInstance().FinalTrialRankRequest(6);
    }

    void Eight_OnClick()
    {
		SetBtnContent(RankBtnType.Eight_Formation);
		DragonBallRankMgr.GetInstance().FinalTrialRankRequest(8);
    }

    void Ten_OnClick()
    {
		SetBtnContent(RankBtnType.Ten_Formation);
		DragonBallRankMgr.GetInstance().FinalTrialRankRequest(10);
    }

    void Tewele_OnClick()
    {
		SetBtnContent(RankBtnType.Twelve_Formation);
		DragonBallRankMgr.GetInstance().FinalTrialRankRequest(12);
    }

	void SetBtnContent(RankBtnType mType)
	{
		ClearBtnSprite();
		if(mType == RankBtnType.Five_Formation)
		{
			mBtnSpriteY[0].gameObject.SetActive(false);
			mBtnSpriteG[0].gameObject.SetActive(true);
		}
		else if(mType == RankBtnType.Six_Formation)
		{
			mBtnSpriteY[1].gameObject.SetActive(false);
			mBtnSpriteG[1].gameObject.SetActive(true);
		}
		else if(mType == RankBtnType.Eight_Formation)
		{
			mBtnSpriteY[2].gameObject.SetActive(false);
			mBtnSpriteG[2].gameObject.SetActive(true);
		}
		else if(mType == RankBtnType.Ten_Formation)
		{
			mBtnSpriteY[3].gameObject.SetActive(false);
			mBtnSpriteG[3].gameObject.SetActive(true);
		}
		else if(mType == RankBtnType.Twelve_Formation)
		{
			mBtnSpriteY[4].gameObject.SetActive(false);
			mBtnSpriteG[4].gameObject.SetActive(true);
		}
	}


}

public enum RankBtnType
{
	Five_Formation,
	Six_Formation,
	Eight_Formation,
	Ten_Formation,
	Twelve_Formation,
}
