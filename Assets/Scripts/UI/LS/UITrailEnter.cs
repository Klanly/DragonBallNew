using UnityEngine;
using System.Collections;

public class UITrailEnter : RUIMonoBehaviour {

    public UILabel[] mLabel;
    public UILabel _SelfRank;
    public UILabel _ChallengeNum;
    public UILabel _BestScore;
    public UILabel _Content;
    public UILabel _IsFight;
	public UILabel _Formation;

	public UISprite BuOuSprite;
	public UISprite ShaLuSprite;

    public void OnShow()
    {
        if(FinalTrialMgr.GetInstance().NowEnum == TrialEnum.TrialType_ShaLuChoose)
		{
			mLabel[0].text = Core.Data.stringManager.getString(25138);
			BuOuSprite.gameObject.SetActive(false);
			ShaLuSprite.gameObject.SetActive(true);
			_Content.text = Core.Data.stringManager.getString(20078);
		}
        else
        {
			mLabel[0].text = Core.Data.stringManager.getString(25139);
			ShaLuSprite.gameObject.SetActive(false);
			BuOuSprite.gameObject.SetActive(true);
			_Content.text = Core.Data.stringManager.getString(20079);
        }
        mLabel[1].text = Core.Data.stringManager.getString(25008);
        mLabel[2].text = Core.Data.stringManager.getString(25010);
        mLabel[3].text = Core.Data.stringManager.getString(25011);
        mLabel[4].text = Core.Data.stringManager.getString(25012);
        mLabel[5].text = Core.Data.stringManager.getString(25013);
        mLabel[6].text = Core.Data.stringManager.getString(25014);
        mLabel[7].text = Core.Data.stringManager.getString(25015);

        _SelfRank.text = FinalTrialMgr.GetInstance()._FinalTrialData.SelfRank.ToString();
        _ChallengeNum.text = FinalTrialMgr.GetInstance()._FinalTrialData.CurChallengeNum.ToString();
		_BestScore.text =  string.Format(Core.Data.stringManager.getString(25100), FinalTrialMgr.GetInstance()._FinalTrialData.BestScore.ToString()) ; 

        if(FinalTrialMgr.GetInstance()._FinalTrialData.CurDungeon > 1)
		{
			_IsFight.text = Core.Data.stringManager.getString(25012);
        }
        else
        {
			_IsFight.text = Core.Data.stringManager.getString(25101);
        }

		if(Core.Data.playerManager.Lv >= 18 && Core.Data.playerManager.Lv < 30)
		{
			_Formation.text = "(" + Core.Data.stringManager.getString(25032) + ")";
		}
		else if(Core.Data.playerManager.Lv >= 30 && Core.Data.playerManager.Lv <= 49)
		{
			_Formation.text = "(" + Core.Data.stringManager.getString(25033) + ")";
		}
		else if(Core.Data.playerManager.Lv >= 50 && Core.Data.playerManager.Lv <= 69)
		{
			_Formation.text =  "(" + Core.Data.stringManager.getString(25034) + ")";
		}
		else if(Core.Data.playerManager.Lv >= 70 && Core.Data.playerManager.Lv <= 89)
		{
			_Formation.text = "(" +  Core.Data.stringManager.getString(25035) + ")";
		}
		else if(Core.Data.playerManager.Lv > 90)
		{
			_Formation.text = "(" +  Core.Data.stringManager.getString(25036) + ")";
		}
    }

    void Back_OnClick()
    {
		if(FinalTrialMgr.GetInstance()._MissionBackCallBack != null)
		{
			FinalTrialMgr.GetInstance()._MissionBackCallBack();
			FinalTrialMgr.GetInstance()._MissionBackCallBack = null;
			DBUIController.mDBUIInstance.ShowFor2D_UI();
		}
        gameObject.SetActive(false);
    }

    void SearchRank_OnClick()
    {
        DragonBallRankMgr.GetInstance().SetRankPanel(true);
    }

    void ChangeQueue_OnClick()
    {
		DBUIController.mDBUIInstance.HiddenFor3D_UI();
		UIMiniPlayerController.Instance.SetActive(false);
        TeamModifyUI.OpenUI();
		TeamUI.OpenUI ();
		FinalTrialMgr.GetInstance().IsFinalTrialToTeam = true;
		gameObject.SetActive(false);
		DBUIController.mDBUIInstance.mDuoBaoView.SetActive(false);
    }

    void BeginFight_OnClick()
    {
		if(FinalTrialMgr.GetInstance().NowEnum == TrialEnum.TrialType_PuWuChoose)
		{
			if(!FinalTrialMgr.GetInstance().CheckBuOuEnter) 
			{
				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(20089));
				return;
			}
		}
		else if(FinalTrialMgr.GetInstance().NowEnum == TrialEnum.TrialType_ShaLuChoose)
		{
			if(!FinalTrialMgr.GetInstance().CheckShaluEnter)
			{
				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(20089));
				return;
			}
		}



    }
}
