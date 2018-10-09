using UnityEngine;

/// <summary>
/// Handle network exception.
/// </summary>
public class HandleNetworkException : MonoBehaviour {

	[HideInInspector]
	private Transform Root;
	// Use this for initialization
	void Start () {
		if(Core.SM != null && Core.SM.isReLogin) return;

		Core.EVC.RegisterDefaultHandler(MsgType.HTTP_EXCEPTION, HttpDefaultHandler);
		Core.EVC.RegisterDefaultHandler(MsgType.SOCK_EXCEPTION, SockDefaultHandler);
		Core.EVC.RegisterDefaultHandler(MsgType.HTTP_SESSION_TIMEOUT, HttpSessionHandler);
		DontDestroyOnLoad(this);
		findUIRoot();
	}

	void findUIRoot () {
		GameObject go_Root = GameObject.FindGameObjectWithTag("Root");
		Utils.Assert(go_Root == null, "Every Scene must contain a Root");
		Root = go_Root.transform;
	}

	//Exception Happened
	void HttpDefaultHandler(BaseTaskAbstract task) {
		if(task != null) {
			ComLoading.Close ();
			//如果是新手引导阶段则不应该弹出错误的信息，直接吞掉
			if(Core.Data.guideManger.isGuiding) {
				return;
			}

			HttpTask ht = task as HttpTask;
			if(ht != null) {

				string content = string.Empty, title = string.Empty;
				#if DEBUG
				content = ht.takeErrorOver;
				#else
				content = Core.Data.stringManager.getString(17);
				#endif
				title = Core.Data.stringManager.getString(5030);

				UIInformation.GetInstance().SetInformation(content, title, task.DispatchToRealHandler);
			}
		}
	}

	/// <summary>
	/// Handle Session timeout
	/// </summary>
	/// <param name="task">Task.</param>
	void HttpSessionHandler(BaseTaskAbstract task) {
		HttpTask ht = task as HttpTask;
		if (ht.response != null) {
			UIInformation.GetInstance ().SetInformation (Core.Data.stringManager.getNetworkErrorString (ht.response.errorCode), Core.Data.stringManager.getString (5030), KickOut, KickOut);

		}
	}

	void KickOut(){
#if Spade
		SpadeIOSLogin spadeSDK = Native.mInstace.m_thridParty as SpadeIOSLogin;
		spadeSDK.tryLogout();
#else
		Core.SM.OnUnregister();
		Core.SM.beforeLoadLevel(Application.loadedLevelName, SceneName.LOGIN_SCENE);
		AsyncLoadScene.m_Instance.LoadScene(SceneName.LOGIN_SCENE); 
#endif
	}
	//Exception Happened
	void SockDefaultHandler(BaseTaskAbstract task) {
		ComLoading.Close ();
		if(task != null) {


			//如果是新手引导阶段则不应该弹出错误的信息，直接吞掉
			if(Core.Data.guideManger.isGuiding) {
				return;
			}

			if(Core.SM != null && Core.SM.CurScenesName == SceneName.GAME_BATTLE)
				return;

			SocketTask st = task as SocketTask;
			if (st != null && st.request != null) {
				if (st.request.type == BaseSocketRequestType.Internal_Control)
					return;
                UIInformation.GetInstance ().SetInformation (st.errorInfo, Core.Data.stringManager.getString (5030), st.DispatchToRealHandler);
			} else {
				//只打印出timeout的信息
                if (st != null) {
					if (st.errorInfo.Contains ("Connection timed out")) {
						UIInformation.GetInstance ().SetInformation (Core.Data.stringManager.getString (7364), Core.Data.stringManager.getString (7387), Reconnect, BackToMainPanel);
					} else if (st.errorInfo.Contains ("No route to host")) {
						UIInformation.GetInstance ().SetInformation (Core.Data.stringManager.getString (7365), Core.Data.stringManager.getString (7132), BackToMainPanel, BackToMainPanel);
					} else if (st.errorInfo.Contains ("System call failed")) {
						UIInformation.GetInstance ().SetInformation (Core.Data.stringManager.getString (7365), Core.Data.stringManager.getString (7132), Reconnect, BackToMainPanel);
					} else if (st.errorInfo.Contains ("Network is unreachable")) {
						UIInformation.GetInstance ().SetInformation (Core.Data.stringManager.getString (7365), Core.Data.stringManager.getString (5030), Reconnect, BackToMainPanel);
//					}else if(st.errorInfo.Contains ("The descriptor is not a socket")){
//						UIInformation.GetInstance ().SetInformation (Core.Data.stringManager.getString (7365), Core.Data.stringManager.getString (5030), Reconnect, BackToMainPanel);
					}else if(st.errorInfo.Contains ("Connection refused")){
						UIInformation.GetInstance ().SetInformation (Core.Data.stringManager.getString (7365), Core.Data.stringManager.getString (5030), Reconnect, BackToMainPanel);
//					}else if(st.errorInfo.Contains ("interrupted")){
//						UIInformation.GetInstance ().SetInformation (Core.Data.stringManager.getString (7365), Core.Data.stringManager.getString (5030), Reconnect, BackToMainPanel);

//					}else if (st.errorInfo.Contains("Disconnect from the Server")){
//                        UIInformation.GetInstance ().SetInformation (Core.Data.stringManager.getString(7365), Core.Data.stringManager.getString (5030),NullConnect,);
					}
                } else {
                    UIInformation.GetInstance ().SetInformation (st.errorInfo, Core.Data.stringManager.getString (5030), null);
                }
			}


		}
		else
		{
			if(UIMessageMain.Instance != null)
			{
				UIInformation.GetInstance ().SetInformation (Core.Data.stringManager.getString (7365), Core.Data.stringManager.getString (7132), Reconnect);
			}
			else
			{
				UIInformation.GetInstance ().SetInformation (Core.Data.stringManager.getString (7365), Core.Data.stringManager.getString (7132), Reconnect, BackToMainPanel);
			}
	
		}
	}


    public void Reconnect(){
        Core.NetEng.SockEngine.ReConnect();

    }
    public void NullConnect(){

    }
    public void BackToMainPanel(){
        ActivityNetController.GetInstance ().BackToActMainPanel ();
    }
}
