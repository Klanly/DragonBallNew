using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UISingleCardFx : RUIMonoBehaviour
{
	static UISingleCardFx _mInstance;
	public static UISingleCardFx GetInstance()
	{
		return _mInstance;
	}
	Vector3 mStarPos = new Vector3(146f,465f,100f);
	Vector3 shakeV3 = new Vector3(0.1f,0.1f,0f);
	float mStarOffset = 28f;
	List<Vector3> mStarPosList = new List<Vector3>();
	public UILabel mName;
	public UILabel at;
	public UILabel df;
	public UISprite Attribute; 

	public UISprite[] mStars;
//	public UISprite[] mStarAphla;

	public float showTime = 10f;
	Monster mMonster;

	public static void OpenCardSinglePanel()
	{
		if(_mInstance == null)
		{
			Object obj = PrefabLoader.loadFromPack("LS/pbLSEggCarlPanel",true);
			if(obj != null)
			{
				GameObject go = Instantiate(obj) as GameObject;
				RED.AddChild (go, DBUIController.mDBUIInstance._TopRoot);
				_mInstance = go.GetComponent<UISingleCardFx>();
				go.layer = 9;
			}
		}
		else
		{
			_mInstance.ResetDefault();
			_mInstance.Start();
		}


	}

	void ResetDefault()
	{
		for(int i=0; i<mStars.Length; i++)
		{
			mStars[i].transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
		}
		mName.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
		at.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
		df.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
	}
	
	void Delete()
	{
		this.dealloc();
	}

	void OnDestroy()
	{
		_mInstance = null;
	}

	void SetDetail()
	{
		mName.gameObject.SetActive(true);
		at.gameObject.SetActive(true);
		df.gameObject.SetActive(true);
		Attribute.gameObject.SetActive(true);

		mName.text = mMonster.config.name;
		at.text = Mathf.RoundToInt(mMonster.config.atk+0.5f).ToString();
		df.text = Mathf.RoundToInt(mMonster.config.def+0.5f).ToString();
		int attr = (int)(mMonster.RTData.Attribute);
		Attribute.spriteName = "Attribute_" + attr.ToString();
		MiniItween mm = MiniItween.ScaleTo(mName.gameObject, new Vector3(1f,1f,1f), 0.15f);
		MiniItween.ScaleTo(at.gameObject, new Vector3(1f,1f,1f), 0.15f);
		MiniItween.ScaleTo(df.gameObject, new Vector3(1f,1f,1f), 0.15f);

		if(mMonster.config.jinjie > 0)
		{
			int m_temp = mMonster.config.star;
			int m_max = 6;
			for(int i=m_max-1; i>-1; i--)
			{
				mStars[i].spriteName = "common-0043";
				if(i > (m_max-m_temp)-1)
				{
					mStars[i].color = Color.white;
				}
				else
				{
					mStars[i].color = Color.black;
				}
			}
		}
		else
		{
			int m_max = 6;
			for(int i=0; i<m_max; i++)
			{
				mStars[i].spriteName = "common-0043";
				mStars[i].color = Color.white;
			}
		}
		for(int i=0; i<mStarPosList.Count; i++)
		{
			mStars[i].gameObject.SetActive(true);
			mStars[i].gameObject.transform.localPosition = mStarPosList[i];
			MiniItween.ScaleTo(mStars[i].gameObject, new Vector3(1.1f,1.1f,1.1f), 0.15f).myDelegateFuncWithObject += StarShake;
		}


		mm.myDelegateFuncWithObject += ShakeCamera;
	}

	void StarShake(GameObject go)
	{
		MiniItween.Shake(CradSystemFx.GetInstance().mCardSystemPanel.gameObject, shakeV3, 0.4f, MiniItween.EasingType.Bounce, false);
	}

	void ShakeCamera(GameObject obk)
	{
		MiniItween.Shake(EggCardSingle.GetInstance().ShakeCamera.gameObject, shakeV3, 0.4f, MiniItween.EasingType.Bounce, false);
	}

	void Start()
	{
		mName.gameObject.SetActive(false);
		at.gameObject.SetActive(false);
		df.gameObject.SetActive(false);
		Attribute.gameObject.SetActive(false);
		mStarPosList.Clear();
		mMonster = CradSystemFx.GetInstance().mItemOfReward[0].toMonster(Core.Data.monManager);
		int mStarNum = 0;
		if(mMonster.config.jinjie > 0)mStarNum = 6;
		else mStarNum = mMonster.Star;
		int mCount = 0;
		while(mCount < mStarNum)
		{
			mStarPosList.Add(new Vector3(mStarPos.x-(mCount)*mStarOffset,mStarPos.y,mStarPos.z));
			mCount++;
		}

		for(int i=0; i<mStars.Length; i++)
		{
			mStars[i].gameObject.SetActive(false);
		}

		EggCardSingle.GetInstance()._CRLuo_ShowANDelCharactor.CharactorID = CradSystemFx.GetInstance().SingleRewardId;
		Invoke("SetDetail", showTime);
	}

}
