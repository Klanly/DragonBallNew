#if UNITY_ANDROID
using UnityEngine;
using System.Collections;

public class Android_QuitGame : MonoBehaviour {

	private static Android_QuitGame _current;
	private static bool initialized = false;
	public static Android_QuitGame Initialize() {
		if (!initialized) {
			var g = new GameObject("AndroidQuitEventListener");
			var gobal = GameObject.FindGameObjectWithTag("Gobal");
			if(gobal != null) g.transform.parent = gobal.transform;
			_current = g.AddComponent<Android_QuitGame>();
			DontDestroyOnLoad(g);
            initialized = true;
		}
		return _current;
	}

	// Update is called once per frame
	void Update () {
		if (Application.platform == RuntimePlatform.Android) {
			if (Input.GetKeyDown(KeyCode.Escape)) {
#if QiHo360
				Native.mInstace.m_thridParty.Quit ();
#elif Spade 
				SpadeIOSLogin spadeSDK = Native.mInstace.m_thridParty as SpadeIOSLogin;
				spadeSDK.quitGame();
#else

#endif
			}
		}
	}

}
#endif
