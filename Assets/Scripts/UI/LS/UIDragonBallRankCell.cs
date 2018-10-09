using UnityEngine;
using System.Collections;

public class UIDragonBallRankCell : RUIMonoBehaviour {

    public UILabel mRank;
    public UILabel mName;
    public UILabel mLevel;
    public UILabel mDonguenNum;
    public UILabel mStarNum;
    public UILabel mContinueNum;

	private int EnemyId;

	public void OnShow(int m_Index, GetNewFinalTrialRank data)
	{
		if(data == null)return;
		EnemyId = data.g;

		mRank.text = (m_Index+1).ToString();
		mName.text = data.n;
		mLevel.text = data.lv.ToString();
		mDonguenNum.text = data.layer.ToString();
		mStarNum.text = data.str.ToString();
		mContinueNum.text = data.lday.ToString();
    }

    void ZhenRong_OnClick()
    {
		DragonBallRankMgr.GetInstance().FinalTrialRankCheckInfoRequest(EnemyId);
    }

	void OnDestroy()
	{
		mRank = null;
		mName = null;
		mLevel = null;
		mDonguenNum = null;
		mStarNum = null;
		mContinueNum = null;
	}
}
