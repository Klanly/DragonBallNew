using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JCCreateRole : MonoBehaviour {

	public UIButton m_btnOK;
	public UIButton m_btnRandom;
	public UIInput m_inputName;
	public UILabel m_txtTitle;
	private GameObject m_CreateRoleBg;
	CRLuo_ShowStage stage = null;

	private List<string> m_listNames = new List<string>();
	private int m_nIndex;

	//当前用户选择中的名字
	private string curName;

	public static void OpenUI()
	{
		Object prefab = PrefabLoader.loadFromPack("JC/JCCreateRole");
		if(prefab != null)
		{
			GameObject obj = Instantiate(prefab) as GameObject;
			RED.AddChild(obj, DBUIController.mDBUIInstance._TopRoot);
		}
	}


	void Start()
	{
		ActivityNetController.GetInstance ().SendUserDeviceInfo ();
		//m_btnOK.TextID = 5030;
		m_txtTitle.text = Core.Data.stringManager.getString(5134);
		m_inputName.label.text = "";
        m_inputName.characterLimit = 20;
		stage = CRLuo_ShowStage.CreateRoleShowStage();
		stage.Try_key = false;
		stage.ShowCharactor(PlayerInfo.DEFAULT_HEAD, MonsterAttribute.FIRE);
		stage.CameraOBJ.transform.localPosition = new Vector3(-0.55f, 0.3f, -2);
		stage.NewOBJ = false;
		stage.CameraOBJ.transform.localPosition = new Vector3 (0, 0.45f, -10);

		Object bg = PrefabLoader.loadFromPack("ZQ/CreateRoleBg");
		if (bg != null)
		{
			m_CreateRoleBg = Instantiate(bg) as GameObject;
			RED.AddChild(m_CreateRoleBg, DBUIController.mDBUIInstance._bottomRoot);
		}

		SendRandomNameMsg();
	}

	void OnBtnClickOK()
	{
		if(string.IsNullOrEmpty(m_inputName.label.text))
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(5136));
			return;
		}

		if (m_inputName.label.text.Length > 6)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(5138));
			return;
		}

		if(m_inputName.label.text.StartsWith(PlayerInfo.DEFAULT_NAME1) || m_inputName.label.text.StartsWith(PlayerInfo.DEFAULT_NAME2))
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(5135));
			return;
		}

		if (SensitiveFilterManager.getInstance ().check (m_inputName.label.text))
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(5137));
			return;
		}
		SendChangeNameMsg();
	}

	void OnBtnClickRandom()
	{
		if(m_listNames == null || m_listNames.Count == 0)
		{
			SendRandomNameMsg();
			return;
		}
		if(m_nIndex < m_listNames.Count - 1)
		{
			m_nIndex++;
			m_inputName.label.text =  m_listNames[m_nIndex];
		}
		else
		{
			m_listNames.Clear();
			m_nIndex = 0;
			SendRandomNameMsg();
		}
	}


	void SendRandomNameMsg()
	{
		m_listNames.Clear();
		m_nIndex = 0;
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.RANDOM_NAME, new RandomNameParam(Core.Data.playerManager.PlayerID));
		
		task.ErrorOccured += testHttpResp_Error;
		task.afterCompleted += testHttpResp_UI;
		
		//then you should dispatch to a real handler
		task.DispatchToRealHandler ();
	}

	bool isSendChangNameMsging = false;
	void SendChangeNameMsg()
	{
		if(isSendChangNameMsging) return;
		isSendChangNameMsging = false;
		curName = m_inputName.label.text;
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.CHANGE_USERINFO, new ChangeUserInfoParam(Core.Data.playerManager.PlayerID, 2, m_inputName.label.text));
		
		task.ErrorOccured += testHttpResp_Error;
		task.afterCompleted += testHttpResp_UI;
		
		//then you should dispatch to a real handler
		task.DispatchToRealHandler ();
	}


	void DestroyUI()
	{
		Destroy (m_CreateRoleBg);
		stage.DeleteSelf ();
		Destroy(gameObject);
	}
	
	#region 网络返回
	void testHttpResp_UI (BaseHttpRequest request, BaseResponse response)
	{
		isSendChangNameMsging = false;
		HttpRequest rq = request as HttpRequest;
		if (response.status != BaseResponse.ERROR) 
		{
			if (rq.Type == RequestType.RANDOM_NAME)
			{
				RandomNameResponse resp = response as RandomNameResponse;
				string[] strNames = resp.data.Split('|');
				m_listNames.AddRange(strNames);
	
				m_nIndex = 0;
				m_inputName.label.text = m_listNames[0];
			}
			else if(rq.Type == RequestType.CHANGE_USERINFO)
			{
				DBUIController.mDBUIInstance.ShowFor2D_UI (false);
				DBUIController.mDBUIInstance.StartCoroutine("CheckSunState");
				DBUIController.mDBUIInstance.RefreshUserInfoWithoutShow();
				#if !NOGUIDE
				Core.Data.temper.SetGameTouch(false);
				#endif
				DestroyUI();

				#if UNITY_ANDROID && !UNITY_EDITOR
				AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
				if(string.IsNullOrEmpty(curName))
					curName = "Empty Name";
				jo.Call("CreateRole", curName);
				#endif
			}
		}
		else
		{
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getNetworkErrorString(response.errorCode));
		}
	}
	
	void testHttpResp_Error (BaseHttpRequest request, string error)
	{
		isSendChangNameMsging = false;
		SQYAlertViewMove.CreateAlertViewMove (error);
	}
	#endregion
	
}
