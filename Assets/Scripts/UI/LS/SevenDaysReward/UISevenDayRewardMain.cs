using UnityEngine;
using System.Collections;

public class UISevenDayRewardMain : RUIMonoBehaviour 
{

	static UISevenDayRewardMain instance = null;
	public TweenScale tweenScale;
	public static UISevenDayRewardMain GetInstance()
	{
		return instance;
	}

	public UISevenDayLoginReward mReward;

	public static void OpenUI()
	{
		if(instance == null)
		{
			Object prefab = PrefabLoader.loadFromPack ("LS/pbLSSevenDayRewardRoot");
			if (prefab != null)
			{
				GameObject obj = Instantiate (prefab) as GameObject;
				RED.AddChild (obj, DBUIController.mDBUIInstance._bottomRoot);
				//RED.TweenShowDialog(obj);
				instance = obj.GetComponent<UISevenDayRewardMain> ();
            }
		}
	}
	
	
//	void PlayAnimation()
//	{
//		tweenScale.Play();
//	}
//	
	// Use this for initialization
	void Start () 
	{
		EventDelegate _delegate = new EventDelegate(OnStartSend);
		tweenScale.onFinished.Add(_delegate);
		Invoke("PlayAnimation",0.2f);


	}

	void OnStartSend()
	{
		if(mReward != null)
		{
			mReward.SendMsg();
        }
		tweenScale.onFinished.Clear();
    }
	


	public void Back_OnClick()
	{
		mReward.DeleteCell();

		NoticeManager.openSign = false;
		Destroy(gameObject);
		if(instance != null)instance = null;
        

//		DBUIController.mDBUIInstance.ShowFor2D_UI ();
    }
}

public enum SevenDayCellType
{
	SevenDayCellType_NONE,
	SevenDayCellType_HAVETAKE,
	SevenDayCellType_WAITTAKE,
	SevenDayCellType_NOTOPEN,
}
