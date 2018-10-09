using UnityEngine;
using System.Collections;

public class UIChallengeRank : RUIMonoBehaviour
{
	static UIChallengeRank _Instance = null;
	public static UIChallengeRank GetInstance()
	{
		return _Instance;
	}

	public UIChallengeMain _UIChallengeMain;
	public GameObject RobType;
	public GameObject TianXiaType;
	
	//1是天下第一 2是抢夺
	int Type;

	public static void OpenUI(int _type)
	{
		if(_Instance == null)
		{
			UnityEngine.Object obj = PrefabLoader.loadFromPack("LS/pbLSChallengeRankRoot");
			if(obj != null)
			{
				GameObject go = RUIMonoBehaviour.Instantiate(obj) as GameObject;
				_Instance = go.GetComponent<UIChallengeRank>();
				RED.AddChild(go.gameObject, DBUIController.mDBUIInstance._bottomRoot);
            }

			_Instance.Type = _type;
		}

	}

	void Start()
	{
		if(Type == 2)
		{
			RobType.gameObject.SetActive(true);
			TianXiaType.gameObject.SetActive(false);
			_UIChallengeMain.ChellengeRankRequest();
		}
		else 
		{
			RobType.gameObject.SetActive(false);
			TianXiaType.gameObject.SetActive(true);
			_UIChallengeMain.TianXiaRankRequest(10);
        }

	}

	void Back_OnClick()
	{
		_UIChallengeMain.DeleteCell();
		Destroy(gameObject);
	}

	void OnDestroy()
	{
		_UIChallengeMain = null;
	}

}
