using UnityEngine;
using System.Collections;

public class UIBattleVideoCell : RUIMonoBehaviour 
{
	public GameObject[] mEnemy;
	public UILabel mName;
	public UILabel mLevel;
	public UILabel mRankNum;
	public UISprite mSelfCircle;
	public UISprite mHead;
	public UILabel mVipNum;
	public UISprite mVipIcon;
	public UILabel mIntroduction;

	public UISprite[] mEnemyHead;
	public UISprite[] mEnemyCircle;
	public UISprite[] mStarArray1;
	public UISprite[] mStarArray2;
	public UISprite[] mStarArray3;

	Vector3[] temp = new Vector3[]{Vector3.zero, Vector3.zero, Vector3.zero};

	string Attackname;
	string Defensename;
	string id;

	public void OnShow(BattleVideoPlaybackData data)
	{

		temp[0] = mStarArray1[0].transform.localPosition;
		temp[1] = mStarArray2[0].transform.localPosition;
		temp[2] = mStarArray3[0].transform.localPosition;
		
		for(int i=0; i<5; i++)
		{
			mStarArray1[i].gameObject.SetActive(false);
			mStarArray2[i].gameObject.SetActive(false);
			mStarArray3[i].gameObject.SetActive(false);
		}

		BattleVideoTeamData _teamdata;

		Attackname = data.battledata.attTeam.name;
		Defensename = data.battledata.defTeam.name;
		id = data.id;

		if(Core.Data.playerManager.PlayerID == data.battledata.attTeam.roleId.ToString()) 
		{
			_teamdata = data.battledata.defTeam;
		}
		else
		{
			_teamdata = data.battledata.attTeam;
		}
		


		int mEnemyindex = 0;
		int teamLength;
		if(_teamdata.team.Length > 3)teamLength = 3;
		else
		{
			teamLength = _teamdata.team.Length;
		}

		while(mEnemyindex < teamLength)
		{
			mEnemy[mEnemyindex ].gameObject.SetActive(true);
			mEnemyHead[mEnemyindex].gameObject.SetActive(true);
			AtlasMgr.mInstance.SetHeadSprite(mEnemyHead[mEnemyindex], _teamdata.team[mEnemyindex].petNum.ToString());
			mEnemyindex++;
		}
		for(int j=mEnemyindex; j<3; j++)
		{
			mEnemy[j].gameObject.SetActive(false);
		}

		mName.text = _teamdata.name;
		mLevel.text = "Lv" + _teamdata.level;
		mRankNum.text = _teamdata.rank.ToString();
		if(_teamdata.headId == 0)AtlasMgr.mInstance.SetHeadSprite(mHead, "10142");
		else
		{
			AtlasMgr.mInstance.SetHeadSprite(mHead, _teamdata.headId.ToString());
		}

		if(_teamdata.vip <= 0)
		{
			mVipNum.text = "";
			mSelfCircle.spriteName = "main-1005";
			mVipIcon.gameObject.SetActive(false);
		}
		else
		{
			mVipNum.text = _teamdata.vip.ToString();
			if(_teamdata.vip > 0 && _teamdata.vip < 4)
			{
				mSelfCircle.spriteName = "starvip1";
				mVipIcon.spriteName = "common-2008";
            }
			else if(_teamdata.vip > 3 && _teamdata.vip < 8)
			{
				mSelfCircle.spriteName = "starvip2";
				mVipIcon.spriteName = "common-2009";
            }
			else if(_teamdata.vip > 7)
			{
				mSelfCircle.spriteName = "starvip3";
				mVipIcon.spriteName = "common-2007";
            }
		}

		int maxCountStar = 0;


		int index = 0;
		while(index < Core.Data.monManager.getMonsterByNum(_teamdata.team[0].petNum).star)
		{
			if(mStarArray1[index] != null)mStarArray1[index].gameObject.SetActive(true);
			mStarArray1[index].transform.localPosition = new Vector3(temp[0].x+(index+1)*2*9.5f,temp[0].y,temp[0].z);
			index++;
		}
		for(int j=0; j<index; j++)
		{
			mStarArray1[j].transform.localPosition = new Vector3(mStarArray1[j].transform.localPosition.x-(index+1)*9.5f,temp[0].y,temp[0].z); ;
		}
		maxCountStar++;

		if(maxCountStar < teamLength)
		{
			index = 0;
			while(index < Core.Data.monManager.getMonsterByNum(_teamdata.team[1].petNum).star)
			{
				if(mStarArray2[index] != null)mStarArray2[index].gameObject.SetActive(true);
				mStarArray2[index].transform.localPosition = new Vector3(temp[1].x+(index+1)*2*9.5f,temp[1].y,temp[1].z);
				index++;
			}
			for(int j=0; j<index; j++)
			{
				mStarArray2[j].transform.localPosition = new Vector3(mStarArray2[j].transform.localPosition.x-(index+1)*9.5f,temp[1].y,temp[1].z); 
			}
		}
		maxCountStar++;


		if(maxCountStar < teamLength)
		{
			index = 0;
			while(index < Core.Data.monManager.getMonsterByNum(_teamdata.team[2].petNum).star)
			{
				if(mStarArray3[index] != null)mStarArray3[index].gameObject.SetActive(true);
				mStarArray3[index].transform.localPosition = new Vector3(temp[2].x+(index+1)*2*9.5f,temp[2].y,temp[2].z);
				index++;
			}
			for(int j=0; j<index; j++)
			{
				mStarArray3[j].transform.localPosition = new Vector3(mStarArray3[j].transform.localPosition.x-(index+1)*9.5f,temp[2].y,temp[2].z); ;
			}
		}
	}

	void OnDestroy()
	{
		mName = null;
		mLevel = null;
		mRankNum = null;
		mSelfCircle = null;
		mHead = null;
		mVipNum = null;
		mVipIcon = null;
		mIntroduction = null;

		for(int i=0 ; i<3; i++)
		{
			mEnemyHead[i] = null;
			mEnemyCircle[i] = null;
			mEnemy[i] = null;
		}

		for(int j=0; j<5; j++)
		{
			mStarArray1[j] = null;
			mStarArray2[j] = null;
			mStarArray3[j] = null;
		}
	}

	void Playback_OnClick()
	{
//		FinalTrialMgr.GetInstance().fightResponse(FinalTrialMgr.GetInstance().BattleVideoPlaybackRes, FinalTrialMgr.GetInstance().BattleVideoPlaybackType);
		FinalTrialMgr.GetInstance().BattleVideoRequestSingle(id, RUIType.EMViewState.S_QiangDuo);
	}

	void Share_OnClick()
	{
		FinalTrialMgr.GetInstance().OpenMessageTag(id,Attackname,Defensename, BattleVideoTagType.Type_Share);

	}

}
