using UnityEngine;
using System.Collections;

public class AsyncLoadScene : MonoBehaviour {

    public GameObject m_UpDoor;
    public GameObject m_DownDoor;
    public GameObject m_Panel;
    public UILabel m_Label;

    public float m_EndTime = 0.2f;

    public delegate void DelegateAsyncLoadProgress(float progress);
    public event DelegateAsyncLoadProgress m_EventAsyncLoadProgress;

    private AsyncOperation m_Async;
    private bool m_OpenFrist;
    private bool m_CloseFrist;
    private string m_SceneName;

    private static AsyncLoadScene _instance;
    public static AsyncLoadScene m_Instance
    {
        get
        {
            return _instance;
        }
    }

	void Awake() {
		if(Core.SM != null && Core.SM.isReLogin) {
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start () {
        _instance = this;
        m_OpenFrist = true;
        m_CloseFrist = true;
        DontDestroyOnLoad(this);
	}

	void Update () {
        if (m_Async != null)
        {
            if (m_EventAsyncLoadProgress != null)
            {
                m_EventAsyncLoadProgress(m_Async.progress);
            }
        }
	}

    public void OnDestroy()
    {
        m_EventAsyncLoadProgress -= AsyncLoadProgress;
    }

    public void LoadScene(string sceneName)
    {
        m_SceneName = sceneName;
        m_OpenFrist = true;
        m_CloseFrist = true;
        if (m_OpenFrist)
        {
            m_Panel.SetActive(true);
            TweenPosition UpTweenPosition = m_UpDoor.GetComponent<TweenPosition>();
            UpTweenPosition.PlayForward();
            TweenPosition DownTweenPosition = m_DownDoor.GetComponent<TweenPosition>();
            DownTweenPosition.PlayForward();

            Invoke("LoadSceneOpen", UpTweenPosition.duration);

            m_OpenFrist = false;
        }

		SetFBS (sceneName);

    }

	public void SetFBS(string sceneName){
		#if UNITY_ANDROID
		if (sceneName == SceneName.MAINUI) {
			Application.targetFrameRate = 35;
//			Debug.Log (" FBS == " + Application.targetFrameRate);
		} else if(sceneName == SceneName.GAME_BATTLE) {
			Application.targetFrameRate = 25;
//			Debug.Log (" FBS == " + Application.targetFrameRate);
		}
		#elif UNITY_IPHONE
		if (sceneName == SceneName.MAINUI) {
			Application.targetFrameRate = 35;
		} else if(sceneName == SceneName.GAME_BATTLE) {
			Application.targetFrameRate = 35;
		}
		#endif

		#if UNITY_EDITOR
		Application.targetFrameRate = 60;
		#endif

	}

    private void LoadSceneOpen(){
        if (m_SceneName != string.Empty)
        {
			m_Label.text = Core.Data.stringManager.getString(7503);
#if UNITY_IPHONE
            m_Label.gameObject.SetActive(false);
#elif UNITY_ANDROID
            m_Label.gameObject.SetActive(true);
#else
            m_Label.gameObject.SetActive(false);
#endif
            StartCoroutine(LoadLevelAsync(m_SceneName));
        }
    }

    private IEnumerator LoadLevelAsync(string sceneName){
        yield return null;
        m_Async = Application.LoadLevelAsync(sceneName); 
        yield return m_Async;  
        m_EventAsyncLoadProgress += AsyncLoadProgress;
        m_SceneName = string.Empty;
    }

    private void LoadFinished(){
        if (m_CloseFrist)
        {
            m_Label.gameObject.SetActive(false);
			m_Label.text = Core.Data.stringManager.getString(7503);
            TweenPosition UpTweenPosition = m_UpDoor.GetComponent<TweenPosition>();
            TweenPosition DownTweenPosition = m_DownDoor.GetComponent<TweenPosition>();

            UpTweenPosition.PlayReverse();
            DownTweenPosition.PlayReverse();

            m_Async = null;
            m_EventAsyncLoadProgress -= AsyncLoadProgress;
            Invoke("LoadSceneEnd", UpTweenPosition.duration);

            m_CloseFrist = false;
        }

    }

    private void LoadSceneEnd(){
        m_Panel.SetActive(false);
    }

    private void AsyncLoadProgress(float progress){
        if (progress >= 1)
        {
            Invoke("LoadFinished", m_EndTime);
        } else
        {
//            if (m_OpenFrist)
//            {
//                m_Panel.SetActive(true);
//                TweenPosition UpTweenPosition = m_UpDoor.GetComponent<TweenPosition>();
//                UpTweenPosition.PlayForward();
//                TweenPosition DownTweenPosition = m_DownDoor.GetComponent<TweenPosition>();
//                DownTweenPosition.PlayForward();
//                m_OpenFrist = false;
//            }
        }
    }
//    void OnGUI()
//    {
//        if (GUI.Button (new Rect(0,0,100,100),"NewLevel"))
//        {
//            LoadScene("GameUI");
//        }
//    }
}
