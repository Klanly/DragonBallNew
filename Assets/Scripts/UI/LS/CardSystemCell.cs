using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardSystemCell : RUIMonoBehaviour 
{
	public UISprite mAttribute;
	public UILabel mDes;
	public UILabel mAttack;
	public UILabel mDefense;
	public UISprite[] mStars;
	public UISprite[] mStarAphla;

	public Animator mAnimator;

	public GameObject kapai;

	public ParticleSystem mParticleSystem1;
	public ParticleSystem mParticleSystem2;

	public GameObject mCardbg;
	public GameObject mCardbtm;
	public GameObject mCardframe;


//	public Card3DBg kapai;

	bool key = false;
	bool OneTime = false;
	public float mAngle = -180.0f;
	public float mAcc = 0.1f;

	float mZacc = 1.0f;

	float mSec = 0.0f;

	bool Zkey = false;

	int mId;
	int mIndex;

	
	bool mLockClick = true;
//	bool mIsSetDetail = false;
	bool misParabolic = false;
//	bool mIsStartLine = false;

	Vector3 mLocalPos;
	
	Vector3 shakeV3 = new Vector3(25f,25f,0f);

	Vector3 mStarPos = new Vector3(-1.15f,2.27f,0f);
	float mParabolicOffset;
	float mStarOffset = -0.25f;
//	float mParabolicOffsetX = 0.0f;
//	float mParabolicOffsetScaleX = 100.0f;
//	float mAccParabolic = 5.0f;

//	float mIsStartLinePosY = 0.0f;
//	float mIsStartLineOffset = 0.0f;

//	float mStartLineOffset = 0.0f;
	List<Vector3> mStarPosList = new List<Vector3>();

	public GameObject mPanel;
	
	MonsterData mMonsterData = null;

	Monster mMonster = null;

	public MiniItween.EasingType mShakeType;

	public void PlayFx()
	{
		if(mParticleSystem1 != null)
		{
			mParticleSystem1.Play();
		}		
		if(mParticleSystem2 != null)
		{
			mParticleSystem2.Play();
		}
	}

	public void SetRotate(int m_index)
	{
//		float z = this.gameObject.transform.localRotation.z;
//		MiniItween r = MiniItween.RotateAround(this.gameObject, Vector3.zero, Vector3.forward, -m_index*36f, 0.1f*(mIndex+1), MiniItween.EasingType.Clerp, false);
//		MiniItween rr = MiniItween.RotateTo(gameObject, new Vector3(0f,180f,m_index*36f) ,0.1f*(mIndex+1));

		mLockClick = false;
	}

	public void SetInitPos(int m_Id, int m_index, ItemOfReward reward)
	{
		mId = m_Id;
		mIndex = m_index;
		mAnimator.SetFloat("aa", 20.0f);
		SetStarPos(reward);

        
        if(CradSystemFx.GetInstance().mTargetPosDic.TryGetValue(m_index, out mLocalPos))
		{
			gameObject.transform.localPosition = mLocalPos;
//			MiniItween m =MiniItween.MoveTo(gameObject,new Vector3(gameObject.transform.localPosition.x,gameObject.transform.localPosition.y+120f, -20f+m_index*0.2f), 0.2f, MiniItween.EasingType.Linear , false);
//			MiniItween.ScaleTo(gameObject,new Vector3(40,40,40), 0.2f);
		}
//		mLockClick = false;
		MiniItween m = MiniItween.MoveTo(gameObject, new Vector3(0,29,-340.0f), 0.25f);
		MiniItween.ScaleTo(gameObject, new Vector3(95,95,75),0.25f);
		m.myDelegateFuncWithObject += SetCompleteMove;
	}

	IEnumerator StarScale()
	{
		for(int i=0; i<mStarPosList.Count; i++)
		{
			mStars[i].gameObject.SetActive(true);
			mStars[i].gameObject.transform.localPosition = mStarPosList[i];
			MiniItween.ScaleTo(mStars[i].gameObject, new Vector3(0.01f,0.01f,0.01f), 0.15f);
			if(i >= mStarPosList.Count-1)
			{
				SetDetail();
				MiniItween m = MiniItween.ScaleTo(mStars[i].gameObject, new Vector3(0.01f,0.01f,0.01f), 0.15f);
				m.myDelegateFuncWithObject += StarShake;
			}
		}
			
		yield return new WaitForSeconds(0f);
	}

	void StarShake(GameObject go)
	{
		MiniItween.Shake(this.gameObject, shakeV3, 0.4f, mShakeType, false);
	}

	IEnumerator CardAnimationOver()
	{
		Core.Data.soundManager.SoundFxPlay (SoundFx.FX_Strengthen_Card);
		yield return new WaitForSeconds(1.5f);
		OneTime = true;
		CradSystemFx.GetInstance().SetReceiveCardBtn(true);
		PlayFx();
//		CradSystemFx.GetInstance().PlayParticleFx();
		StartCoroutine(StarScale());
	}

//	IEnumerator CardDetail()
//	{
//		yield return new WaitForSeconds(0.7f);
//
//	}

	public void CountCircleExpression()
	{
//		Vector3 Point1 = gameObject.transform.localPosition;
//		Vector3 Point2 = CradSystemFx.GetInstance().mPetBoxPos;
//		Vector3 Point2 = mLocalPos;
//		mParabolicOffset = 0.05f;
//		mParabolicOffset = (Point2.y-Point1.y)/(Point2.x-Point1.x);
//		mParabolicOffsetX = gameObject.transform.localPosition.x;
		misParabolic = true;

		MiniItween r = MiniItween.MoveTo(this.gameObject, mLocalPos, 0.6f);
		r.myDelegateFuncWithObject += SetNewFinal;
		MiniItween.ScaleTo(this.gameObject, new Vector3(1f,1f,1f), 0.6f);
	}

	void SetNewFinal(GameObject obj)
	{
		misParabolic = false;
		SetFinalPos(gameObject);
		CradSystemFx.GetInstance().CheckClosePanelWindow();
    }
    
	void SetAllFalse()
	{
		for(int i=0; i<5; i++)
		{
			mStars[i].gameObject.SetActive(false);
			mStarAphla[i].gameObject.SetActive(false);
		}
		mDes.text = "";
		mAttack.text = "";
		mDefense.text = "";
		mAttribute.gameObject.SetActive(false);
	}

	void SetStarPos(ItemOfReward reward)
	{
		if(CradSystemFx.GetInstance().mItemOfReward == null)
		{
			RED.LogError("card index is error");
			return;
		}
		if(mIndex >= CradSystemFx.GetInstance().mItemOfReward.Length)
		{
			RED.LogError("card index is error");
			return;
		}

		mMonster = reward.toMonster(Core.Data.monManager);
//		mMonster = CradSystemFx.GetInstance().mItemOfReward[mIndex].toMonster(Core.Data.monManager);
		mMonsterData = Core.Data.monManager.getMonsterByNum(mId);
		int mStarNum = mMonsterData.star;
		int mCount = 0;

		mCardframe.renderer.material.mainTexture = AtlasMgr.mInstance.Get3DCardFrameTexture (mStarNum);
		mCardbg.renderer.material.mainTexture = AtlasMgr.mInstance.Get3DCardBgTexture (mStarNum);
		mCardbtm.renderer.material.mainTexture = AtlasMgr.mInstance.Get3DCardBtmTexture (mStarNum);
        
        while(mCount < mStarNum)
		{
			mStarPosList.Add(new Vector3(mStarPos.x-(mCount)*mStarOffset,mStarPos.y,mStarPos.z));
			mCount++;
		}
	}

	void SetDetail()
	{
		if(mMonster != null)
		{

			mAttribute.gameObject.SetActive(true);
			int attr = (int)(mMonster.RTData.Attribute);
			mAttribute.spriteName = "Attribute_" + attr.ToString();
		}

		mDes.text = mMonsterData.name;
		mAttack.text = mMonsterData.atk.ToString();
		mDefense.text = mMonsterData.def.ToString();
		MiniItween.ScaleTo(mDes.gameObject, new Vector3(0.01f,0.01f,0.01f), 0.15f);
		MiniItween.ScaleTo(mAttack.gameObject, new Vector3(0.01f,0.01f,0.01f), 0.15f);
		MiniItween.ScaleTo(mDefense.gameObject, new Vector3(0.01f,0.01f,0.01f), 0.15f);
	}

	// Use this for initialization
	void Start () 
	{
		mPanel = gameObject.transform.parent.gameObject.transform.parent.gameObject;
		UIEventListener.Get(gameObject).onHover = Hover;
		SetAllFalse();
		mParticleSystem1.Stop();
		mParticleSystem2.Stop();

//		kapai.SetMonStar (3);
	}

	public void SetFinalPos()
	{
		if(key)return;
		MiniItween m = MiniItween.MoveTo(gameObject, CradSystemFx.GetInstance().mPetBoxPos, 0.4f);
		MiniItween.ScaleTo(gameObject, new Vector3(0,0,0), 0.4f);
		MiniItween.RotateTo(gameObject, Quaternion.Euler(new Vector3(0,180,0)), 1.5f);
		m.myDelegateFuncWithObject += SetFinalPos;
		
	}

	void OnDestroy()
	{
		mParticleSystem1 = null;
		mParticleSystem2 = null;
		mAttribute = null;
		mDes = null;
		mAttack = null;
		mDefense = null;
		mAnimator = null;
		mPanel = null;
//		kapai = null;
		if(CradSystemFx.GetInstance()._CRLuo_Rot_Inversion != null)
		{
			CradSystemFx.GetInstance()._CRLuo_Rot_Inversion.InputOBJ = null;
		}

		if(CradSystemFx.GetInstance()._CRLuo_ShowStage != null)
		{
			CradSystemFx.GetInstance()._CRLuo_ShowStage.DeleteSelf();
		}
		for(int i=0; i<5; i++)
		{
			mStars[i] = null;
		}
	}
	

	void Update () 
	{
		if(key)
		{
			Core.Data.soundManager.SoundFxPlay(SoundFx.Fx_PickCard);
			mAnimator.SetFloat("Time", 10.0f);
			StartCoroutine(CardAnimationOver());
//			StartCoroutine(CardDetail());
			key = false;
//			if(gameObject.transform.localRotation.y <= 90.0f)
//			{
//				if(!mIsSetDetail)	
//				{
//					SetDetail();
//					mIsSetDetail = true;
//				}
//			}
		}

		if(misParabolic)
		{
//			if(gameObject.transform.localPosition.y < CradSystemFx.GetInstance().mPetBoxPos.y)
//			{
//				mParabolicOffsetX+=3.4f;
//				mParabolicOffsetScaleX -= 4.6f;
				mAngle += 10.0f;
				gameObject.transform.localRotation = Quaternion.Euler (new Vector3(gameObject.transform.localRotation.x,mAngle,gameObject.transform.localRotation.z));

//				gameObject.transform.localPosition = new Vector3(mParabolicOffsetX, (mParabolicOffsetX)*(mParabolicOffsetX)*mParabolicOffset,gameObject.transform.localPosition.z);
//				gameObject.transform.localPosition = new Vector3(mParabolicOffsetX, mParabolicOffsetX*mParabolicOffset,gameObject.transform.localPosition.z);
//                gameObject.transform.localScale = new Vector3(mParabolicOffsetScaleX,mParabolicOffsetScaleX,mParabolicOffsetScaleX);
//			}
//			else
//			{
//				SetFinalPos(gameObject);
//				CradSystemFx.GetInstance().mAllCount --;
//				CradSystemFx.GetInstance().CheckClosePanelWindow();
//				misParabolic = false;
//			}

		}
		if(Zkey)
		{
			gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z-mZacc);
			if(gameObject.transform.localPosition.z <= -70)
			{
				gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, -70);
				Zkey = false;
			}
		}
		mSec ++;
	}

	void SetFinalPos(GameObject g)
	{
//		CradSystemFx.GetInstance().SetReceiveCardBtn(false);
		CradSystemFx.GetInstance().mIsChoose = false;
		Destroy(gameObject);
	}

	void SetCompleteScale(GameObject g)
	{
//		mLockClick = false;
		MiniItween m = MiniItween.MoveTo(gameObject, new Vector3(0,29,-340.0f), 0.25f);
		MiniItween.ScaleTo(gameObject, new Vector3(95,95,75),0.25f);
		m.myDelegateFuncWithObject += SetCompleteMove;
    }
    
    void SetCompleteMove(GameObject g)
	{
		CradSystemFx.GetInstance().SetShowStage(mId, gameObject);
		key = true;
	}

	void OnClick()
	{
#if SPLIT_MODEL
		if (!Core.Data.sourceManager.IsModelExist (mId))
		{
			if(!CradSystemFx.GetInstance().mIsChoose)
			{
				if(!mLockClick)
				{
					if(OneTime)
					{
						return;
					}
				}
				UIDownModel.OpenDownLoadUI (mId, DownLoadFinish, PlayCardAnim, UIDownModel.WinType.WT_Two);
			}
		}
		else
		{
			DownLoadFinish (null);
		}
#else
		DownLoadFinish (null);
#endif
	}


	void PlayCardAnim()
	{
//				kapai.SetMonStar (mMonster.Star);

		CardSystemCell nn = gameObject.GetComponent<CardSystemCell>();
		CradSystemFx.GetInstance().mSelectScript = nn;

		MiniItween m = MiniItween.ScaleTo(gameObject, new Vector3(44,44,40),0.1f);
		m.myDelegateFuncWithObject += SetCompleteScale;


	}


	void DownLoadFinish(AssetTask task)
	{
		if(!CradSystemFx.GetInstance().mIsChoose)
		{
			if(!mLockClick)
			{
				if(OneTime)
				{
					return;
				}
				MiniItween nn = MiniItween.MoveTo(gameObject, CradSystemFx.GetInstance().mCardSystemPanel.mGoalPos[mIndex].transform.localPosition, 0.6f);
				nn.myDelegateFuncWithObject += CompleteFly;
				mLockClick = true;
				CradSystemFx.GetInstance().mIsChoose = true;
			}
		}

	}

	void CompleteFly(GameObject obj)
	{
		StartCoroutine(PrepareToTuranAround());
	}

	IEnumerator PrepareToTuranAround()
	{
		yield return new WaitForSeconds(0.8f);
		gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0f,180f,0f));
		gameObject.transform.localPosition = CradSystemFx.GetInstance().mCardSystemPanel.mGoalPos[0].transform.localPosition;
		PlayCardAnim ();
	}

	void Hover(GameObject obj, bool ishover)
	{

	}

	public void MoveToLocal()
	{
		this.mLockClick = true;
//		MiniItween m = MiniItween.ScaleTo(gameObject, new Vector3(40,40,40),0.1f);
	}


	
}
