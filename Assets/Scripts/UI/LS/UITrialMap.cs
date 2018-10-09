using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UITrialMap : RUIMonoBehaviour {

	public UILabel mCurDungeonNum;
    public UILabel _MostDungeon;
    public UILabel _HistoryDungeonNum;

	public UILabel[] _AttrLabel;

	public UISprite[] mEnemyIcon;

	public TweenScale mPointTween;
	public TweenPosition mPointTweenPos;

	public UISpriteAnimation[] spriteanimation;

	public UISpriteAnimation _openbox;

	public GameObject mFight;       //选择人物动画object
	public GameObject mReward;    //领取宝箱object
	public GameObject mBackground;    //背景冲锋的object
	public GameObject mPoint;      //箭头指示object

	public GameObject RunFx1;
	public GameObject RunFx2;

	public UIButton[] mEnemyPlayer;
    
	public GameObject mEnemyTitleGroup;
	public List<UIFinalMapEnemy> mEnemyList = new List<UIFinalMapEnemy>();
	List<TweenScale> mEnemyTween = new List<TweenScale>();
	

	//是否奔跑
	private bool isRun;

	Vector3 Player1Pos = new Vector3(-274f,-55f,0f);
	Vector3 Player2Pos = new Vector3(-5f,-54f,0f);
	Vector3 Player3Pos = new Vector3(260f,-54f,0f);

	Vector3 Player1LocalPos = new Vector3(-442f,84f,0f);
	Vector3 Player2LocalPos = new Vector3(0f,80f,0f);
    Vector3 Player3LocalPos = new Vector3(442f,84f,0f);


	float OffsetPlayer1;
	float OffsetPlayer2;
    float OffsetPlayer3;
    
//	float k1 = 0.006f;
//	float k3 = 0.003f;

    public void OnShow()
    {
		GetScript();

		SetChooseAnimat();

		SetTitleDetail();

		FinalTrialMgr.GetInstance().mFinalTrial3D.OnShow();
        
		RunFx1.gameObject.SetActive(false);
		RunFx2.gameObject.SetActive(false);

    }

	public void SetTitleDetail()
	{
		_MostDungeon.text = FinalTrialMgr.GetInstance()._FinalTrialData.HighestDungeonNum.ToString();
		_HistoryDungeonNum.text = FinalTrialMgr.GetInstance()._FinalTrialData.HistoryDungeonNum.ToString();
		mCurDungeonNum.text = FinalTrialMgr.GetInstance()._FinalTrialData.CurDungeon.ToString();

		_AttrLabel[0].text = string.Format(Core.Data.stringManager.getString(25023), "[7da7ff]"+FinalTrialMgr.GetInstance()._FinalTrialData.mChooseAddAtr[0]);
		_AttrLabel[1].text = string.Format(Core.Data.stringManager.getString(25024), "[7da7ff]"+FinalTrialMgr.GetInstance()._FinalTrialData.mChooseAddAtr[1]);
		_AttrLabel[2].text = string.Format(Core.Data.stringManager.getString(25047), "[7da7ff]"+FinalTrialMgr.GetInstance()._FinalTrialData.mChooseAddAtr[2]);
		_AttrLabel[3].text = string.Format(Core.Data.stringManager.getString(25048), "[7da7ff]"+FinalTrialMgr.GetInstance()._FinalTrialData.mChooseAddAtr[3]);
		_AttrLabel[4].text = string.Format(Core.Data.stringManager.getString(25049), "[7da7ff]"+FinalTrialMgr.GetInstance()._FinalTrialData.mChooseAddAtr[4]);
		_AttrLabel[5].text = string.Format(Core.Data.stringManager.getString(25050), "[7da7ff]"+FinalTrialMgr.GetInstance()._FinalTrialData.mChooseAddAtr[5]);
	}
    
	void GetScript()
	{
		if(mEnemyTween.Count == 0)
		{
			for(int i=0; i<mEnemyList.Count; i++)
			{
				mEnemyTween.Add(mEnemyList[i].gameObject.GetComponent<TweenScale>());
            }
		}

	}

	void SetChooseAnimat()
	{
//		if(FinalTrialMgr.GetInstance().NowDougoenType == FinalTrialDougoenType.FinalTrialDougoenType_Point)
//		{
//			PlayPointanimat();
//		}
//		else if(FinalTrialMgr.GetInstance().NowDougoenType == FinalTrialDougoenType.FinalTrialDougoenType_Award)
//		{
//			AnimatStatus(4);
//			Invoke("PlayRewardAnimat",1.0f);
//		} 
//		else if(FinalTrialMgr.GetInstance().NowDougoenType == FinalTrialDougoenType.FinalTrialDougoenType_Addattr)
//		{
//            FinalTrialMgr.GetInstance().mUITrialMapNotAdd.SetActive(true);
//        }
//		else if(FinalTrialMgr.GetInstance().NowDougoenType == FinalTrialDougoenType.FinalTrialDougoenType_AwardAndAddattr)
//		{
//			FinalTrialMgr.GetInstance().mUITrialMapNotAdd.SetActive(true);
//			AnimatStatus(4);
//			Invoke("PlayRewardAnimat",1.0f);
//		}
	}

	#region  打开宝箱部分
	void PlayRewardAnimat()
	{
		RewardAnimat();
	}

	void RewardAnimat()
	{
		_openbox.gameObject.GetComponent<UISpriteAnimation>().enabled = true;

		_openbox.namePrefix = "ActiveBox-";
		_openbox.framesPerSecond = 7;
		_openbox.loop = false;
//		_openbox.Reset();

    }
	#endregion

	#region  选择敌人部分
	void PlayFightAnimat(GameObject obj)
	{


		Core.SoundEng.StopChannel(layer);

		Core.Data.soundManager.SoundFxPlay(SoundFx.Fx_float);

		AnimatStatus(3);

		mEnemyTitleGroup.gameObject.SetActive(false);
		if(FinalTrialMgr.GetInstance().NowEnum == TrialEnum.TrialType_PuWuChoose) 
		{
			mEnemyIcon[2].spriteName = "challenge-1023";
			mEnemyIcon[2].width = 120;
			mEnemyIcon[2].height = 238;
			mEnemyIcon[2].transform.parent.gameObject.GetComponent<BoxCollider>().size = new Vector3(120f, 238f, 0f);
		}
		else 
		{
			mEnemyIcon[2].spriteName = "challenge-1024";
			mEnemyIcon[2].width = 174;
			mEnemyIcon[2].height = 242;
			mEnemyIcon[2].transform.parent.gameObject.GetComponent<BoxCollider>().size = new Vector3(174f, 242f, 0f);

		}
		isRun = false;
        FightAnimat();
	}

	void FightAnimat()
	{
		Player1Fun();
		Invoke("Player2Fun",0.1f);
		Invoke("Player3Fun",0.2f);
//		PlayTitleAnimat();
	}

	void PlayTitleAnimat(GameObject go)
	{
		mEnemyTitleGroup.gameObject.SetActive(true);

        for(int i=0; i<mEnemyTween.Count; i++)
		{
			TweenScale.Begin<TweenScale>(mEnemyTween[i].gameObject, 0.3f);
        }

		SetAnemyDetail();
    }

	void SetAnemyDetail()
	{
		for(int i=0; i<mEnemyList.Count; i++)
		{
			mEnemyList[i].OnShow(i+1);
			mEnemyList[i]._EnemyNum.text = FinalTrialMgr.GetInstance()._FinalTrialData.PetListCount[i].ToString();
			AtlasMgr.mInstance.SetHeadSprite(mEnemyList[i]._Head, FinalTrialMgr.GetInstance()._FinalTrialData.PetList[i].ToString());
			mEnemyList[i]._SelfNum.text = FinalTrialMgr.GetInstance()._FinalTrialData.SelfMonNum.ToString();
		}
	}

	void Player1Fun()
	{
		MiniItween.MoveTo(mEnemyPlayer[0].gameObject, Player1Pos, 0.3f);
	}

	void Player2Fun()
	{
		MiniItween.MoveTo(mEnemyPlayer[1].gameObject, Player2Pos, 0.3f);
    }

	void Player3Fun()
	{
		MiniItween mm = MiniItween.MoveTo(mEnemyPlayer[2].gameObject, Player3Pos, 0.3f);
		mm.myDelegateFuncWithObject += PlayTitleAnimat;
    }
    #endregion

	#region 奔跑部分
	private int layer = -1;
	void PlayRunAnimat()
	{
		layer = Core.Data.soundManager.SoundFxPlay(SoundFx.Fx_walk);

		FinalTrialMgr.GetInstance().mFinalTrial3D.SetRotate(PlayFightAnimat);

		AnimatStatus(2);

//		for(int i=0; i<spriteanimation.Length; i++)
//		{
//			spriteanimation[i].gameObject.GetComponent<UISpriteAnimation>().enabled = true;
//			spriteanimation[i].namePrefix = "challenge-";
//			spriteanimation[i].framesPerSecond = 10;
//			spriteanimation[i].loop = true;
//		}
		RunFx1.gameObject.SetActive(true);
		isRun = true;
		StartCoroutine(PlayRunSprite());
	}

	IEnumerator PlayRunSprite()
	{
		yield return new WaitForSeconds(0.13f);
		if(RunFx1.gameObject.activeSelf)
		{
			RunFx1.gameObject.SetActive(false);
			RunFx2.gameObject.SetActive(true);
		}
		else
		{
			RunFx1.gameObject.SetActive(true);
			RunFx2.gameObject.SetActive(false);
        }
		if(isRun)
		{
			StartCoroutine(PlayRunSprite());
		}
		else
		{
			RunFx1.gameObject.SetActive(false);
			RunFx2.gameObject.SetActive(false);
        }
	}

	#endregion

	#region 指示部分
	void PlayPointanimat()
	{
		FinalTrialMgr.GetInstance().mFinalTrial3D.OnShow();
		AnimatStatus(1);
		mPointTween.PlayForward();
		mPointTweenPos.PlayForward();
	}
	
	#endregion

    void Back_OnClick()
    {
        gameObject.SetActive(false);
		Destroy(FinalTrialMgr.GetInstance().mFinalTrial3D.gameObject);
//		FinalTrialMgr.GetInstance().NowDougoenType = FinalTrialDougoenType.FinalTrialDougoenType_Point;
		mEnemyPlayer[0].transform.localPosition = Player1LocalPos;
		mEnemyPlayer[1].transform.localPosition = Player2LocalPos;
		mEnemyPlayer[2].transform.localPosition = Player3LocalPos;

		if(FinalTrialMgr.GetInstance()._MissionBackCallBack != null)
		{
			FinalTrialMgr.GetInstance()._MissionBackCallBack();
			FinalTrialMgr.GetInstance()._MissionBackCallBack = null;
			if(DBUIController.mDBUIInstance.mDuoBaoView != null)
			{
				DBUIController.mDBUIInstance.mDuoBaoView.OnBtnQuit();
			}
			else 
			{
				DBUIController.mDBUIInstance.ShowFor2D_UI();
            }
		}

    }

    void Btn1_OnClick()
    {
        EnterBattle(2);
    }

    void Btn2_OnClick()
    {
        EnterBattle(0);
    }

    void Btn3_OnClick()
    {
        EnterBattle(1);
    }

	void Moveto_OnClick()
	{
		PlayRunAnimat();
	}

	void GetReward_OnClick()
	{
		GetRewardSucUI.OpenUI(FinalTrialMgr.GetInstance()._NewFinalTrialFightResponse.data.rushResult.award, Core.Data.stringManager.getString(5047));
		PlayPointanimat();
		_openbox.ResetToBeginning();
		DBUIController.mDBUIInstance.RefreshUserInfo ();
	}

    private void EnterBattle(int type)
    {
//		FinalTrialMgr.GetInstance().NewFinalTrialFightRequest(type);
    }


	//选择隐藏显示哪个动画 1 point 2 冲锋 3 fight choose 4 reward
	void AnimatStatus(int index)
	{
		mFight.gameObject.SetActive(false);
		mReward.gameObject.SetActive(false);
		mBackground.gameObject.SetActive(false);
		mPoint.gameObject.SetActive(false);

		switch(index)
		{
			case 0:
				break;
			case 1:
				mPoint.gameObject.SetActive(true);
				break;
			case 2:
				mBackground.gameObject.SetActive(true);
				break;
			case 3:
				mFight.gameObject.SetActive(true);
				break;
			case 4:
				mReward.gameObject.SetActive(true);
				break;
		}
	}



}
