using UnityEngine;
using System.Collections;

public class UIFirstRecharge : RUIMonoBehaviour 
{
	private static UIFirstRecharge _Instance;
	public static UIFirstRecharge GetInstance()
	{
		return _Instance;
	}

	public Camera _Camera;

	private CRLuo_ShowStage _Stage;
	private GameObject m_CreateRoleBg;

//	private float _Acc = 40f;
//	private float _Acc2 = 0.7f;

//	private float MultipleX;
//	private float MultipleY;

//	private float MultipleRotate;

	private bool _IsRemove;

	public static void OpenUI()
	{
		Object obj = PrefabLoader.loadFromPack("LS/pbLSFirstRechargeCamera");
		if(obj != null)
		{
			GameObject go = Instantiate (obj) as GameObject;
			if(go != null)
			{
				RED.AddChild(go, DBUIController.mDBUIInstance._bottomRoot);
				_Instance = go.GetComponent<UIFirstRecharge>();
				go.transform.localPosition = new Vector3(220f,0f,0f);
			}
		}
	}

	void Start()
	{
//		_IsRemove = true;
		if(_Camera != null)
		{
			_Camera.transform.localPosition = new Vector3(-53f, 458f, -1089f);
//			_Camera.transform.localRotation = Quaternion.Euler(new Vector3(3.1f, 0f, 0f));
			_Camera.transform.localRotation = Quaternion.Euler(new Vector3(-6f, 0f, 0f));
		}

//		MultipleX = Mathf.Abs(458f - 118f)/Mathf.Abs(-2436f + 1089f);
//		MultipleY = Mathf.Abs(53f - 47f)/Mathf.Abs(-2436f + 1089f);
//		MultipleRotate = Mathf.Abs(3.1f + 6f)/Mathf.Abs(-2436f + 1089f);

		Object bg = PrefabLoader.loadFromPack("ZQ/CreateRoleBg");
		if (bg != null)
		{
			m_CreateRoleBg = Instantiate(bg) as GameObject;
			RED.AddChild(m_CreateRoleBg, DBUIController.mDBUIInstance._bottomRoot);
        }

		_Stage = CRLuo_ShowStage.CreateRoleShowStage();
		if(_Stage != null)
		{
			_Stage.Try_key = false;
			_Stage.CameraOBJ.gameObject.SetActive(false);
			_Stage.ShowCharactor(Core.Data.rechargeDataMgr.NPCId);
			_Stage.gameObject.transform.localPosition = new Vector3(0.6f,0f,-1.7f);
		}
		BeginAnimat();
	}

	IEnumerator BeginStart()
	{
		yield return new WaitForSeconds(0.2f);

		BeginAnimat();
	}

	void BeginAnimat()
	{
		MiniItween.MoveTo(_Camera.gameObject, new Vector3(-47f, 118f,-2436f), 2f, MiniItween.EasingType.EaseOutQuint);
//		MiniItween.RotateTo(_Camera.gameObject, Quaternion.Euler(new Vector3(-6f,0f,0f)), 2.5f, MiniItween.EasingType.EaseInOutQuint);
	}

	public void Close()
	{
		if(_Stage != null)
		{
			_Stage.DeleteSelf();
		}
		if(m_CreateRoleBg != null)
		{
			Destroy(m_CreateRoleBg.gameObject);
		}

		Destroy(gameObject);
	}
 
	void OnDestroy()
	{
		_Instance = null;
	}
	
//	void Update()
//	{
//		if(_IsRemove)
//		{
//			if(_Acc > 0.003f)
//			{
//				_Acc -= _Acc2;
//				if(_Acc2 >= 0.001f)
//				{
//					_Acc2 -= 0.0045f;
//				}
//				else
//				{
//					_Acc2 = 0.001f;
//				}
//			}
//			else 
//			{
//				_Acc = 0.003f;
//			}
//			_Camera.transform.localPosition = new Vector3(_Camera.transform.localPosition.x+_Acc*MultipleY ,_Camera.transform.localPosition.y-_Acc*MultipleX,_Camera.transform.localPosition.z-_Acc);
////			_Camera.transform.localRotation = Quaternion.Euler(new Vector3(_Camera.transform.localRotation.x - _Acc*(MultipleRotate+0.02f),0f,0f));
//			if(_Camera.transform.localPosition.z <= -2436f)_IsRemove = false;
//		}
//
//	}

}
