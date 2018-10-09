using UnityEngine;
using System.Collections;
#if InMobi
using System.Collections.Generic;
#endif

public class InitializedFirstly : MonoBehaviour {

	private bool paused;

	[HideInInspector]
	public static InitializedFirstly self;
	//We must keep this is run firstly.
	void Awake () {
		ConsoleEx.DebugLog("*************** InitializedFirstly ***************", ConsoleEx.YELLOW);

		if(Core.SM != null && Core.SM.isReLogin) {
			Destroy(gameObject);
			return;
		}

		#region Global Define
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		#if UNITY_EDITOR
		Application.targetFrameRate = 60;
		#else
		Application.targetFrameRate = 35;
		#endif

		DontDestroyOnLoad(this);
		#endregion

		self = this;
		Core.SM = new StateMachine ();
		Core.SM.onGameLaunched();
		
		#region Add by jc
		if(!SQYAlertViewMove.initialized) SQYAlertViewMove.Initialize();
		#endregion

		#if InMobi
		InMobi();
		#endif
	}


	void OnApplicationPause(bool pauseStatus) {
		paused = pauseStatus;

		try {
			if(paused) {
				Core.SM.onGamePause(Application.loadedLevelName);
			} else {
				Core.SM.onGameResume(Application.loadedLevelName);
			}
		} catch(System.Exception ex) {
			ConsoleEx.DebugLog(ex.ToString());
		}


	}

	//function is only for unity editor
	void OnApplicationQuit(){
		if(Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor) {
			if(Core.SM != null)
				Core.SM.onGameQuit(Application.loadedLevelName);
		}
	}


	#if InMobi
	void InMobi() {
		var dict = new Dictionary<string,string>();
		dict.Add( "platform", "Spade" );
		InMobiAndroid.init( "fd3acd00dd704881a786c5fd4b7b6c21", dict);
	}
	#endif
}
