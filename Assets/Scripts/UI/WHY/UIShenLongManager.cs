using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIShenLongManager : MonoBehaviour 
{
	#region 奥义


	public GameObject aoYiListItemPrb;

	public GameObject dragonAltarPrb;

	public GameObject callDragonSucceedAlertPrb;

	public GameObject selectAoYiAlertPrb;


	#endregion

	#region 龙珠 有关变量
	public UIButton callDragonButton;
	public UILabel toggleDQLabel;
	public UILabel toggleNMKXLabel;
	#endregion

	#region 召唤神龙 有关变量
	private int currentCallDragonExp;
	//private UILabel callDragonExpLabel;
	//private UISlider callDragonExpSlider;
	//private UILabel callDragonLv;

	public UILabel callEarthDragonExpLabel;
	public UISlider callEarthDragonExpSlider;
	public UILabel callEarthDragonLv;

	public UILabel callNMKXDragonExpLabel;
	public UISlider callNMKXDragonExpSlider;
	public UILabel callNMKXDragonLv;

	public UILabel callDragonTimeLabel;
	public long callDragonTime;

	public UILabel mianZhanTimeLabel;

	public long mianZhanTime;

	public bool isCallDragonTimerChange;
	public bool isMianZhanTimerChange;

    //--- 同时最为 地球神龙 和 那美克星神龙
	public List<UILabel> earthDragonBallCountList = new List<UILabel>();
	public List<UISprite> earthDragonBallIconList = new List<UISprite>();
    public List<UISprite> earthBallBg;
    public List<GameObject> earthBallLight;

    public List<UILabel> NMKXDragonBallCountList = new List<UILabel>();
    public List<UISprite> NMKXDragonBallIconList = new List<UISprite>();
    public List<UISprite> NMKXBallBg;
    public List<GameObject> NMKXBallLight;
   

	public TaskID[] callDragonCompletedTaskID = new TaskID[2];

	GameObject callDragonSucceedAlertObj = null;

	#endregion

	#region 切换按钮
	public UIToggle toggleDQ;
	public UIToggle toggleNMKX;

	public GameObject earthCheckmark1;
	public GameObject earthCheckmark2;
	
	public GameObject NMKXCheckmark1;
	public GameObject NMKXCheckmark2;

	public bool isAnimation = false;

    public GameObject Go_Earth;
    public GameObject Go_NaMeike;

    public Vector3 ShowDragonPos;
    public Vector3 HideDragonPos;
    //变换神龙的时间
    public float ChangeDragonTime = 0.5f;
    //改变背景神龙的时间
    public float ChangeBGTime = 0.35f;

    public Vector3 ShakeWave;
    //晃动时间
    public float ShakeTime = 0.2f;


    public List<DragonBallCell> earthCellList;
    public List<DragonBallCell> NMKCellList;

	#endregion

    #region 背景

    public UISprite earthBG;
    public UISprite[] nameikeBG;

    #endregion

    //神龙最大6级
    private const int MAX_LEVEL_OF_DRAGON = 6;

	public UIDragonAltar dragonAltar = null;
	//下载提示次数
	private static int m_nDownLoadTipCnt;
	private static UIShenLongManager instance;
    public static UIShenLongManager Instance{
        get{ 
            return instance;
        }
    }

    private static System.Action CallBackToTask;
	//显示 状态
    public bool isState = false;
    private bool canSync = true;

//	static public UIShenLongManager Instance
//	{
//		get
//		{
//			if (instance == null)
//			{
//               	instance = UnityEngine.Object.FindObjectOfType(typeof(UIShenLongManager)) as UIShenLongManager;
//
//				if (instance == null)
//				{
//					GameObject prb = PrefabLoader.loadFromPack("WHY/pbUIShenLongManager") as GameObject;
//					GameObject prbObj = GameObject.Instantiate(prb) as GameObject;
//                   
//					instance = prbObj.GetComponent<UIShenLongManager>();
//				}
//			}
//			return instance;
//		}
//	}
    public UILabel robBallTimes;

	private DragonManager.DragonType  dragonType;
	bool isDouble = false;

    void Awake(){
        instance = this;
    }

	public UISelectAoYiAlert selectAoYiAlert {get;set;}

	
    public static void setShenLongManagerRoot(GameObject root)
	{
        if (instance == null)
        {
            GameObject prb = PrefabLoader.loadFromPack("WHY/pbUIShenLongManager") as GameObject;
            if (prb != null)
            {
                GameObject prbObj = GameObject.Instantiate(prb) as GameObject;
                instance = prbObj.GetComponent<UIShenLongManager>();
            }
        }
        else
        {
            instance.gameObject.SetActive(true);
            instance.Start();
        }
//        if (UIShenLongManager.instance.gameObject != null && gameObject.activeInHierarchy == false) {
//            gameObject.SetActive (true);
//            this.Start ();
//
//        } else {
        if(root != null)
            instance.gameObject.transform.parent = root.transform;
        else 
            instance.gameObject.transform.parent = DBUIController.mDBUIInstance._bottomRoot.transform;
        instance.gameObject.transform.localPosition = Vector3.back * 10;
        instance.gameObject.transform.localRotation = Quaternion.identity;
        instance.gameObject.transform.localScale = Vector3.one;
         
	}

    public void DragonBallSetUp(System.Action callBack){
        CallBackToTask = callBack;
        setShenLongManagerRoot (DBUIController.mDBUIInstance._bottomRoot);

    }
 
	void Start () 
    {
        isState = true;
		DBUIController.mDBUIInstance.HiddenFor3D_UI();
		UIMiniPlayerController.Instance.SetActive(false);

		this.callDragonCompletedTaskID = new TaskID[2];
		this.callDragonCompletedTaskID[(int)DragonManager.DragonType.EarthDragon] = TaskID.None;
		this.callDragonCompletedTaskID[(int)DragonManager.DragonType.NMKXDragon] = TaskID.None;

		Core.Data.dragonManager.callDragonTimerEvent = callDragonTimerEvent;
		Core.Data.dragonManager.mianZhanTimerEvent = mianZhanTimerEvent;

		//	Debug.Log (" call dragon timer event "  + " earth time " + Core.Data.dragonManager.callEarthDragonTime   +  "  nmkx = " + Core.Data.dragonManager.callNMKXDragonTime);

		this.isAnimation = false;

		if(aoYiListItemPrb == null)
		{
			aoYiListItemPrb = PrefabLoader.loadFromPack("WHY/pbUIAoYiListItem") as GameObject;
		}

		this.dragonAltar = gameObject.GetComponent<UIDragonAltar>();
		//检测 是否两个同时
		CheckDouble ();

		if(Core.Data.dragonManager.currentDragonType == DragonManager.DragonType.EarthDragon)
		{
			UIToggle.current = toggleDQ;
			UIToggle.current.value = true;
            earthCheckmark1.SetActive (true);
            this.earthCheckmark2.SetActive (true);

            clickTabButton(DragonManager.DragonType.EarthDragon, 
                new GameObject[] {this.earthCheckmark1, this.earthCheckmark2}, 
                new GameObject[] {this.NMKXCheckmark1, this.NMKXCheckmark2}, 
                -200, Color.green, Color.white,true);
		}
        else if(Core.Data.dragonManager.currentDragonType == DragonManager.DragonType.NMKXDragon)
		{
			UIToggle.current = toggleNMKX;
			UIToggle.current.value = true;
            clickTabButton(DragonManager.DragonType.NMKXDragon, 
                new GameObject[] {this.NMKXCheckmark1, this.NMKXCheckmark2}, 
                new GameObject[] {this.earthCheckmark1, this.earthCheckmark2}, 
                200, Color.white, Color.green, true);
		}



		dragonAltar.Init();

		showAoYiSolt();
	
		dragonAltar.sortAoYiSlot();

		checkCallDragonSucceed();


        if(FinalTrialMgr.GetInstance().jumpTo == TrialEnum.TrialType_QiangDuoDragonBall){
			StartCoroutine(showQiangDuoDragonBall());
		}

		isAnimation = true;
//		#if SPLIT_MODEL
//        if(m_nDownLoadTipCnt == 0 && !Core.Data.guideManger.isGuiding && Core.Data.playerManager.RTData.downloadReawrd == 0)
//		{
//            List<SouceData> list = Core.Data.sourceManager.GetUpdateModes();
//            if(list != null || list.Count > 0)
//            {
//                UIDownloadPacksWindow.OpenDownLoadUI(null);
//            }
//            m_nDownLoadTipCnt++;
//		}
//		#endif

        ShowLeftTimes();

		if (Core.Data.dragonManager.callEarthDragonTime == 1) {
			Invoke ("SafeCount", 1.5f);
			RED.Log ("time  ==== 1");
		} else if (Core.Data.dragonManager.callNMKXDragonTime == 1) {
			Invoke ("SafeCountNMKX",1.5f);
			RED.Log (" nmkx time  ==== 1");
		}
		if (Core.Data.dragonManager.mianZhanTime == 1) {
			Invoke ("SafeMianzhanCount", 1.5f);
		}


	}
		
    //同步 时间 和龙珠
	void SyncTimeAndBallUI(){
		if(Core.Data.dragonManager.currentDragonType == DragonManager.DragonType.EarthDragon)
		{
			UIToggle.current = toggleDQ;
			UIToggle.current.value = true;
			earthCheckmark1.SetActive (true);
			this.earthCheckmark2.SetActive (true);

			clickTabButton(DragonManager.DragonType.EarthDragon, 
				new GameObject[] {this.earthCheckmark1, this.earthCheckmark2}, 
				new GameObject[] {this.NMKXCheckmark1, this.NMKXCheckmark2}, 
				-200, Color.green, Color.white,true);
		}

		else if(Core.Data.dragonManager.currentDragonType == DragonManager.DragonType.NMKXDragon)
		{
			UIToggle.current = toggleNMKX;
			UIToggle.current.value = true;
			clickTabButton(DragonManager.DragonType.NMKXDragon, 
				new GameObject[] {this.NMKXCheckmark1, this.NMKXCheckmark2}, 
				new GameObject[] {this.earthCheckmark1, this.earthCheckmark2}, 
				200, Color.white, Color.green, true);
		}
				// Debug.Log(" check call dragon succeed in synccccccccccccccccccccccc");
        //checkCallDragonSucceed();

        if (Core.Data.dragonManager.callDragonSucceed [0] == false && Core.Data.dragonManager.callEarthDragonTime > 0) {
            UIShenLongManager.instance.GetComponent<UIShenlongBallAnim> ().setTimeBoxEffect = true;
        }

		this.CtrlBoxEffect (Core.Data.dragonManager.currentDragonType );
	}
    //检测   召唤神龙 是否成功 
	void checkCallDragonSucceed()
	{
        if(callDragonSucceedAlertObj != null)
		{
			return;
		}

		//	Debug.Log("  check call   succeed   callEarthtime: " + Core.Data.dragonManager.callEarthDragonTime );


		if (Core.Data.dragonManager.callDragonSucceed [0] == false && Core.Data.dragonManager.callEarthDragonTime > 0) {
			UIShenLongManager.instance.GetComponent<UIShenlongBallAnim> ().setTimeBoxEffect = true;
		} else {
			UIShenLongManager.instance.GetComponent<UIShenlongBallAnim> ().setTimeBoxEffect = false;
		}

        for (int i = 0; i < Core.Data.dragonManager.callDragonSucceed.Length; i++) {

            //   Debug.Log("is check  dragon type i " + i +"  bool = " +  Core.Data.dragonManager.isCheckedCallDragon[i]);
            if (Core.Data.dragonManager.isCheckedCallDragon[i] == false)
            {
                //    Debug.Log("call succeed  dragon type i " + i +"  bool = " +  Core.Data.dragonManager.isCheckedCallDragon[i]);
               if (Core.Data.dragonManager.callDragonSucceed[i] == true)
                {
                    Core.Data.dragonManager.ChectCallDragonIsFinish(i + 1);
                    break;
                }

            }
        }

	}
    //检测 结束 满足条件  执行 召唤
	 public  void CheckComplete(){

        for(int i = 0; i < Core.Data.dragonManager.callDragonSucceed.Length; i++)
        {
			if (Core.Data.dragonManager.callDragonSucceed [i]) {


				TaskID taskID = TaskID.None;
				if ((DragonManager.DragonType)i == DragonManager.DragonType.EarthDragon) {
					taskID = TaskID.CallEarthDragonTimer;
				} else if ((DragonManager.DragonType)i == DragonManager.DragonType.NMKXDragon) {
					taskID = TaskID.CallNMKXDragonTimer;
				}
               
				if (UIShenLongManager.instance != null) {
					if (UIShenLongManager.instance.isState == true) { 
						//StartCoroutine (CallDragonTimeCompleted (taskID));
						CallDragonTimeCompleted (taskID);
						break;
					}
				}
			}
        }
    }

	IEnumerator showQiangDuoDragonBall()
	{
		yield return new WaitForSeconds(0.1f);
        FinalTrialMgr.GetInstance().CreateScript(TrialEnum.TrialType_QiangDuoDragonBall, QiangduoEnum.QiangduoEnum_List);
        yield return new WaitForSeconds (0.5f);
		if(Core.Data.guideManger.isGuiding == false){
	        FinalTrialMgr.GetInstance ().qiangDuoPanelScript.OnShuaXin ();
		}
	}

    //检测 转换UI
	void Update()
	{
		if(isCallDragonTimerChange)
		{
			DateTime d = new DateTime();
			d = d.AddSeconds(this.callDragonTime);

            string tShow = "";
            if (d.Hour == 0)
            {
                tShow = "{0:D2}:{1:D2}";
                callDragonTimeLabel.text = string.Format( tShow ,d.Minute, d.Second);
            }
            else
            {
                tShow = "{0:D2}:{1:D2}:{2:D2}";
                callDragonTimeLabel.text = string.Format(tShow, d.Hour, d.Minute, d.Second);
            }

//			callDragonTimeLabel.text = string.Format( "{0:D2}:{1:D2}", d.Minute, d.Second);
            gameObject.GetComponent<UIShenlongBallAnim>().lblTimeEffect.text =Core.Data.stringManager.getString(6047) +"   [ffff00]"+ callDragonTimeLabel.text;
			isCallDragonTimerChange = false;
			// wxl 主要检测  当前界面同步时间
			if (isState == true) {
				if (DragonManager.checkTime == true) {
					this.SyncTimeAndBallUI ();
					DragonManager.checkTime = false;
				}
			}
		}
	        

		if(this.isMianZhanTimerChange)
		{
			DateTime d = new DateTime();
			d = d.AddSeconds(this.mianZhanTime);

			if(this.mianZhanTime != 0)
			{
                string tShow = "";
                if (d.Hour == 0)
                {
					tShow = "{0:D2}:{1:D2}:{2:D2}";
					mianZhanTimeLabel.text = string.Format( tShow ,d.Hour,d.Minute, d.Second);
                }
                else
                {
                    tShow = "{0:D2}:{1:D2}:{2:D2}";
                    mianZhanTimeLabel.text = string.Format(tShow, d.Hour, d.Minute, d.Second);
                }
            }
			else
			{
				mianZhanTimeLabel.text = string.Format ("{0:D2}:{1:D2}:{2:D2}",0,0,0);       //Core.Data.stringManager.getString(6100);
			}

			isMianZhanTimerChange = false;
		}
	}
    //每秒检测 
	void callDragonTimerEvent(DragonManager.DragonType dragonType, long callDragonTime)
	{
		if(dragonType == Core.Data.dragonManager.currentDragonType)
		{
			this.callDragonTime = callDragonTime;
			//同步 时间
			if (this.callDragonTime == 30) {
                if (canSync == true)
                {
                    Core.Data.dragonManager.SyncCallDragonTime();
                    canSync = false;
                }
			}

			showCallDragonTime ();
		}
	}

	void mianZhanTimerEvent(long mianZhanTime)
	{
		this.mianZhanTime = mianZhanTime;
		this.isMianZhanTimerChange = true;
	}
    // 刷新 奥义槽
    void showAoYiSolt(bool isLockData = true) {

		List<AoYi> aoYiList = Core.Data.dragonManager.usedToList();
		for(int i = 0; i < 5; i++)
		{
			DragonLockData dragonLockData = Core.Data.dragonManager.getUnLockDragonSlotConfig(i);
			AoYi aoYi = aoYiList[i];
            if(isLockData){
				this.dragonAltar.setAoYiSlot(i, dragonLockData, aoYi);
			}
			else
			{
			    this.dragonAltar.setAoYiSlot(i, aoYi);
			}
		}
	}

    // 召唤神龙 方法  注册回调 
	void CallDragon()
	{
		changeCallDragonButtonState(false);
		ComLoading.Open();

		Core.Data.dragonManager.CallDragonCompletedDelegate = CallDragonRequestCompleted;
		Core.Data.dragonManager.callDragonRequest(Core.Data.dragonManager.currentDragonType);
	}

    //召唤神龙消息返回
	public void CallDragonRequestCompleted(DragonManager.DragonType dragonType, CallDragonResponse callDragonResponse)
	{
		Dragon dragon = Core.Data.dragonManager.DragonList[(int)dragonType];

		dragon.RTData.st = Core.TimerEng.curTime;
		dragon.RTData.dur = callDragonResponse.data.du;
		dragon.RTData.ep = callDragonResponse.data.ep;
		dragon.RTData.lv = callDragonResponse.data.lv;


		if(UIShenLongManager.instance != null   && dragon.RTData.dur > 0 ){
            UIShenLongManager.Instance.gameObject.GetComponent<UIShenlongBallAnim> ().setTimeBoxEffect = true;
            UIShenLongManager.Instance.gameObject.GetComponent<UIShenlongBallAnim> ().ShowTimeEffect ();
        }

		showCallDragonTime();
		if(dragonType == DragonManager.DragonType.EarthDragon)
		{
			Core.Data.dragonManager.callEarthDragonTimeCompletedDelegate = CallDragonTimeCompleted;
		}
		else
		{
			Core.Data.dragonManager.callNMKXDragonTimeCompletedDelegate = CallDragonTimeCompleted;
		}

		Core.Data.dragonManager.startCallDragonTimer(dragonType, Core.TimerEng.curTime, Core.TimerEng.curTime + callDragonResponse.data.du);
	}

	//召唤结束后 清空 时间 回调 等 
	public void CallDragonTimeCompleted(TimerTask timerTask)
	{
		if(timerTask.taskId == TaskID.CallEarthDragonTimer)
		{
			Core.Data.dragonManager.callEarthDragonTimeCompletedDelegate = null;
			Core.Data.dragonManager.callEarthDragonTime = 0;
			this.callDragonTime = 0;
			Core.Data.dragonManager.DragonList[(int)DragonManager.DragonType.EarthDragon].RTData.st = 0;
			Core.Data.dragonManager.callDragonSucceed[(int)DragonManager.DragonType.EarthDragon] = true;
			this.callDragonCompletedTaskID[(int)DragonManager.DragonType.EarthDragon] = timerTask.taskId;
		}
		else if(timerTask.taskId == TaskID.CallNMKXDragonTimer)
		{
			Core.Data.dragonManager.callNMKXDragonTimeCompletedDelegate = null;
			Core.Data.dragonManager.callNMKXDragonTime = 0;
			this.callDragonTime = 0;
			Core.Data.dragonManager.DragonList[(int)DragonManager.DragonType.NMKXDragon].RTData.st = 0;
			Core.Data.dragonManager.callDragonSucceed[(int)DragonManager.DragonType.NMKXDragon] = true;
			this.callDragonCompletedTaskID[(int)DragonManager.DragonType.NMKXDragon] = timerTask.taskId;
		}
		showCallDragonTime ();

//        Debug.Log(" check call dragon succeed in CallDragonTimeCompleteddddddddddddddddddddddddddddd");

		if(Application.loadedLevelName != "GameUI")
		{
			return;
		}
		
        checkCallDragonSucceed();
	}

    /// <summary>
    /// wxl change 召唤结束 生成学习奥义 界面 播放动画
    /// </summary>
    /// <param name="taskID">Task I.</param>
	public void   CallDragonTimeCompleted(TaskID taskID)
	{
		DragonManager.DragonType callDragonSucceedType = DragonManager.DragonType.None;
		if(taskID == TaskID.CallEarthDragonTimer)
		{
			callDragonSucceedType = DragonManager.DragonType.EarthDragon;
			if(Core.Data.dragonManager.currentDragonType == DragonManager.DragonType.EarthDragon)
			{
				changeCallDragonButtonState(true);
			}
		}
		else if(taskID == TaskID.CallNMKXDragonTimer)
		{
			callDragonSucceedType = DragonManager.DragonType.NMKXDragon;
			if(Core.Data.dragonManager.currentDragonType == DragonManager.DragonType.NMKXDragon)
			{
				changeCallDragonButtonState(true);
			}
		}
		dragonType = callDragonSucceedType;
		//召唤  动画
        gameObject.GetComponent<UIShenlongBallAnim>().showUpgradeDragonCompleted();

		Invoke ("CreatCallDragonAlert",0.5f);
		// yield return new WaitForSeconds(1f);
//		if(callDragonSucceedAlertPrb == null)
//		{
//            callDragonSucceedAlertPrb = PrefabLoader.loadFromPack("WHY/pbNewCallDragonSucceed") as GameObject;
//		}
//        if (callDragonSucceedAlertObj == null)
//            callDragonSucceedAlertObj = Instantiate(callDragonSucceedAlertPrb) as GameObject;
//        else
//            callDragonSucceedAlertObj.SetActive(true);
//
//
//        callDragonSucceedAlertObj.transform.parent = DBUIController.mDBUIInstance._bottomRoot.transform;
//		callDragonSucceedAlertObj.transform.localPosition = Vector3.zero;
//		callDragonSucceedAlertObj.transform.localPosition = this.transform.localPosition + Vector3.back * 10;
//		callDragonSucceedAlertObj.transform.localScale = Vector3.one;
//        callDragonSucceedAlertObj.GetComponent<UISelectAoYiAlert>().curType = callDragonSucceedType;
//		selectAoYiAlert = callDragonSucceedAlertObj.GetComponent<UISelectAoYiAlert>();
//		selectAoYiAlert.currentSelectAoYiAlertType = UISelectAoYiAlert.SelectAoYiAlertType.SelectLearnAoYi;
//        selectAoYiAlert.setAoYiItem(Core.Data.dragonManager.getAllAoYi(), true, callDragonSucceedType);
//
//
//		Core.Data.dragonManager.learnAoYiCompletedDelegate = learnAoYiCompleted;
	}

	void CreatCallDragonAlert(){

		if(callDragonSucceedAlertPrb == null)
		{
			callDragonSucceedAlertPrb = PrefabLoader.loadFromPack("WHY/pbNewCallDragonSucceed") as GameObject;
		}
		if (callDragonSucceedAlertObj == null)
			callDragonSucceedAlertObj = Instantiate(callDragonSucceedAlertPrb) as GameObject;
		else
			callDragonSucceedAlertObj.SetActive(true);


		callDragonSucceedAlertObj.transform.parent = DBUIController.mDBUIInstance._bottomRoot.transform;
		callDragonSucceedAlertObj.transform.localPosition = Vector3.zero;
		callDragonSucceedAlertObj.transform.localPosition = this.transform.localPosition + Vector3.back * 10;
		callDragonSucceedAlertObj.transform.localScale = Vector3.one;
		callDragonSucceedAlertObj.GetComponent<UISelectAoYiAlert>().curType = dragonType;
		selectAoYiAlert = callDragonSucceedAlertObj.GetComponent<UISelectAoYiAlert>();
		selectAoYiAlert.currentSelectAoYiAlertType = UISelectAoYiAlert.SelectAoYiAlertType.SelectLearnAoYi;
		selectAoYiAlert.setAoYiItem(Core.Data.dragonManager.getAllAoYi(), true, dragonType);

		Core.Data.dragonManager.learnAoYiCompletedDelegate = learnAoYiCompleted;
	}

    /*void showCallDragonExp()
	{
		if (Core.Data.dragonManager.DragonList == null || Core.Data.dragonManager.DragonList.Count == 0)
		{
			RED.LogWarning ("dragon list is null");
			return;
		}

		Dragon dragon = Core.Data.dragonManager.DragonList[(int)Core.Data.dragonManager.currentDragonType];
		int currentDragonLv = dragon.RTData.lv;
		int nextDragonLv = currentDragonLv + 1;
        if(currentDragonLv >= MAX_LEVEL_OF_DRAGON)
		{
			nextDragonLv = currentDragonLv;
		}

		UpDragonData upDragonData = Core.Data.dragonManager.getDragonConfig(nextDragonLv);
		this.currentCallDragonExp = dragon.RTData.ep;

		float res = ((float)this.currentCallDragonExp) / ((float)upDragonData.exp);
		this.callDragonExpSlider.value = res;
        if(this.currentCallDragonExp == 0) {
            this.callDragonExpSlider.foregroundWidget.gameObject.SetActive(false);
        } else {
            this.callDragonExpSlider.foregroundWidget.gameObject.SetActive(true);
        }

        if(currentDragonLv >= MAX_LEVEL_OF_DRAGON)
            this.callDragonExpLabel.text = "MAX";
        else
            this.callDragonExpLabel.text = this.currentCallDragonExp + "/" + upDragonData.exp;
		this.callDragonLv.text = dragon.RTData.lv.ToString();
	}
*/
	void showCallDragonTime()
	{
		this.isCallDragonTimerChange = true;
	}

	void showMianZhanTime()
	{
		this.isMianZhanTimerChange = true;
	}

	protected void changeCallDragonButtonState(bool isEnabled)
	{
        if (UIShenLongManager.instance == null)
            return;

		this.callDragonButton.isEnabled = isEnabled;
	}

    void AddTruceTime()
    {
        TrucePanelScript.ShowTrucePanel(DBUIController.mDBUIInstance._TopRoot);
    }
	//地球龙珠 按钮 
    void ClickTabDQ()
    {
		clickTabButton(DragonManager.DragonType.EarthDragon, 
			new GameObject[] {this.earthCheckmark1, this.earthCheckmark2}, 
			new GameObject[] {this.NMKXCheckmark1, this.NMKXCheckmark2}, 
            -200, Color.green, Color.white);
    }

	//娜美 可行 龙珠 
    void ClickTabNMKX()
    {
		clickTabButton(DragonManager.DragonType.NMKXDragon, 
			new GameObject[] {this.NMKXCheckmark1, this.NMKXCheckmark2}, 
			new GameObject[] {this.earthCheckmark1, this.earthCheckmark2}, 
            200, Color.white, Color.green);
    }

	void clickTabButton(DragonManager.DragonType dragonType, 
		GameObject[] selectButtonCheckmarkObj, 
		GameObject[] unSelectButtonCheckmarkObj, 
		float moveSelectCotnentBackgroundX, 
		Color toggleDQLabelColor, 
        Color toggleNMKXLabelColor,
        bool firstTime = false)
	{
		if(Core.Data.dragonManager.currentDragonType == dragonType && this.isAnimation)
		{
			return;
		}
		Core.Data.dragonManager.currentDragonType = dragonType;

		tabButtonAnimation(selectButtonCheckmarkObj, unSelectButtonCheckmarkObj, moveSelectCotnentBackgroundX);

		this.toggleDQLabel.color = toggleDQLabelColor;
		this.toggleNMKXLabel.color = toggleNMKXLabelColor;
   
        this.CtrlBoxEffect (dragonType);



        if (firstTime)
        {
            StartCoroutine(ShowDefaultDragon(dragonType));
        }
        else
        {
            StartCoroutine(ShowChangeDragon(dragonType));
        }

		if (dragonType == DragonManager.DragonType.NMKXDragon) {
			if (isDouble == true) {
				checkCallDragonSucceed ();
			}
		}



	}
    void CtrlBoxEffect(DragonManager.DragonType dragonType){
        //切换 状态 时间效果
        if (Core.Data.dragonManager.callDragonSucceed [(int)dragonType] == false ) {
            if (dragonType == DragonManager.DragonType.EarthDragon) {
                if (Core.Data.dragonManager.callEarthDragonTime > 0) {
                    UIShenLongManager.instance.GetComponent<UIShenlongBallAnim> ().setTimeBoxEffect = true;
                } else {
                    UIShenLongManager.instance.GetComponent<UIShenlongBallAnim> ().setTimeBoxEffect = false;
                }

            } else if (dragonType == DragonManager.DragonType.NMKXDragon) {
                if (Core.Data.dragonManager.callNMKXDragonTime > 0) {
                    UIShenLongManager.instance.GetComponent<UIShenlongBallAnim> ().setTimeBoxEffect = true;
                } else {
                    UIShenLongManager.instance.GetComponent<UIShenlongBallAnim> ().setTimeBoxEffect = false;
                }
            }
        }
		if (UIShenLongManager.instance != null && isState == true) {
             this.GetComponent<UIShenlongBallAnim> ().SetShowBoxEffect ();
        }
    }



	#region 动画

    IEnumerator ShowChangeDragon(DragonManager.DragonType dragonType) {
        GameObject GoDragon = null;
        if(dragonType == DragonManager.DragonType.EarthDragon)
            GoDragon = Go_NaMeike;
        else 
            GoDragon = Go_Earth;

        MiniItween.MoveTo(GoDragon, HideDragonPos, ChangeDragonTime, MiniItween.EasingType.EaseInCubic);
        if(dragonType == DragonManager.DragonType.EarthDragon) {
            foreach(DragonBallCell cell in NMKCellList) {
                cell.HideFull();
            }
        } else {
            foreach(DragonBallCell cell in earthCellList) {
                cell.HideFull();
            }
        }

        //等待移动动画完成
        yield return new WaitForSeconds(ChangeDragonTime);

        changeDragon();

        if(dragonType == DragonManager.DragonType.EarthDragon)
            GoDragon = Go_Earth;
        else 
            GoDragon = Go_NaMeike;

        MiniItween.MoveTo(GoDragon, ShowDragonPos, ChangeDragonTime, MiniItween.EasingType.EaseInCubic).FinishedAnim = () => {
            MiniItween.Shake(GoDragon, ShakeWave, ShakeTime, MiniItween.EasingType.EaseOutCubic);
        };

        StartCoroutine(showBackground(dragonType));

        if(dragonType == DragonManager.DragonType.EarthDragon) {
            foreach(DragonBallCell cell in earthCellList) {
                cell.showFull();
            }
        } else {
            foreach(DragonBallCell cell in NMKCellList) {
                cell.showFull();
            }
        }

    }

    IEnumerator ShowDefaultDragon(DragonManager.DragonType dragonType) {
        GameObject GoDragon = null;

        Go_NaMeike.transform.localPosition = HideDragonPos;
        Go_Earth.transform.localPosition = HideDragonPos;

        if(dragonType == DragonManager.DragonType.EarthDragon)
            GoDragon = Go_Earth;
        else 
            GoDragon = Go_NaMeike;

        changeDragon();
        MiniItween.MoveTo(GoDragon, ShowDragonPos, ChangeDragonTime, MiniItween.EasingType.EaseInCubic).FinishedAnim = () => {
            MiniItween.Shake(GoDragon, ShakeWave, ShakeTime, MiniItween.EasingType.EaseOutCubic);
        };

        showDefaultBackground(dragonType);

        if(dragonType == DragonManager.DragonType.EarthDragon) {
            foreach(DragonBallCell cell in earthCellList) {
                cell.showFull();
            }
        }

        yield return new WaitForSeconds(0.1f);
        earthCheckmark1.gameObject.transform.localPosition = Vector3.left * 120;
        earthCheckmark2.gameObject.transform.localPosition = Vector3.right * 120;
    }


    IEnumerator showBackground(DragonManager.DragonType dragonType) {
        Color Full = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        Color Fade = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        if(dragonType == DragonManager.DragonType.EarthDragon) {
            foreach(UISprite bg in nameikeBG)
                TweenColor.Begin(bg.gameObject, ChangeBGTime, Fade);
            yield return new WaitForSeconds(ChangeBGTime * 0.6f);
            TweenColor.Begin(earthBG.gameObject, ChangeBGTime, Full);
        } else {
            TweenColor.Begin(earthBG.gameObject, ChangeBGTime, Fade);
            yield return new WaitForSeconds(ChangeBGTime * 0.6f);
            foreach(UISprite bg in nameikeBG)
                TweenColor.Begin(bg.gameObject, ChangeBGTime, Full);
        }
    }

    void showDefaultBackground(DragonManager.DragonType dragonType){
        Color Full = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        Color Fade = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        if(dragonType == DragonManager.DragonType.EarthDragon) {
            foreach(UISprite bg in nameikeBG)
                bg.color = Fade;
            TweenColor.Begin(earthBG.gameObject, ChangeBGTime, Fade, Full);
        } else {
            earthBG.color = Fade;
            foreach(UISprite bg in nameikeBG)
                TweenColor.Begin(bg.gameObject, ChangeBGTime, Fade, Full);
        }
    }


	void tabButtonAnimation(GameObject[] selectButtonCheckmarkObj, GameObject[] unSelectButtonCheckmarkObj, float moveSelectCotnentBackgroundX)
	{
		
		if(this.isAnimation)
		{

			this.selectButtonCheckmark(selectButtonCheckmarkObj);
			unSelectButtonCheckmark(unSelectButtonCheckmarkObj);

		}
		else
		{
			for(int i = 0; i < selectButtonCheckmarkObj.Length; ++i)
			{
				selectButtonCheckmarkObj[i].SetActive(true);

			}
            earthCheckmark1.GetComponent<UISprite> ().alpha = 1;
            earthCheckmark2.GetComponent<UISprite> ().alpha = 1;

		}
	}


	void MoveSelectCotnentBackgroundComplete(GameObject go)
	{
		MiniItween m = go.GetComponent<MiniItween>();
		m.myDelegateFuncWithObject -= MoveSelectCotnentBackgroundComplete;
	}

	void SelectCotnentBackgroundColorChange(GameObject g, V4 from, V4 to)
	{
	     MiniItween.ColorFromTo(g, from, to, 0.3f, MiniItween.EasingType.Normal, MiniItween.Type.ColorWidget);
	}

	void SelectCotnentBackgroundColorChange(GameObject g)
	{
		SelectCotnentBackgroundColorChange(g, new V4(1.0f, 1.0f, 1.0f, 0.0f), new V4(1.0f, 1.0f, 1.0f, 175.0f/255.0f));
	}

	void selectButtonCheckmark(GameObject[] gs)
	{
		float time = 0.05f;
		for(int i = 0; i < gs.Length; i++)
		{
			GameObject g = gs[i];
			g.SetActive(true);

			g.transform.localScale = new Vector3(3,3,1);

			MiniItween.ColorFromTo(g, new V4(1.0f, 1.0f, 1.0f, 0f), new V4(1.0f, 1.0f, 1.0f, 1.0f), time, MiniItween.EasingType.Normal, MiniItween.Type.ColorWidget);

			MiniItween.ScaleTo(g, new Vector3(1, 1, 1), time);

			MiniItween.MoveTo(g, new Vector3(-120 + (i * 120 * 2) , 0, 0), time);
		}
	}

	void unSelectButtonCheckmark(GameObject[] gs)
	{
		float time = 0.05f;
		for(int i = 0; i < gs.Length; i++)
		{
			GameObject g = gs[i];
			g.SetActive(true);

			MiniItween m = MiniItween.ColorFromTo(g, new V4(1.0f, 1.0f, 1.0f, 1.0f), new V4(1.0f, 1.0f, 1.0f, 0.0f), time, MiniItween.EasingType.Normal, MiniItween.Type.ColorWidget);
			m.myDelegateFuncWithObject += unSelectButtonCheckmarkMoveToComplete;

			MiniItween.MoveTo(g, new Vector3(-145 + (i * 145 * 2), 0, 0), time);
		}
	}

	void unSelectButtonCheckmarkMoveToComplete(GameObject g)
	{
		MiniItween m = g.GetComponent<MiniItween>();
		m.myDelegateFuncWithObject -= unSelectButtonCheckmarkMoveToComplete;
		g.SetActive(false);
	}

	#endregion

	void clickUnSelectCotnentBackground()
	{
		if(Core.Data.dragonManager.currentDragonType == DragonManager.DragonType.EarthDragon)
		{
			UIToggle.current = toggleNMKX;
			UIToggle.current.value = true;
			ClickTabNMKX();
		}
		else if(Core.Data.dragonManager.currentDragonType == DragonManager.DragonType.NMKXDragon)
		{

			UIToggle.current = toggleDQ;
			UIToggle.current.value = true;
			ClickTabDQ();
		}
	}

	public void clickCallDragon()
	{
		CallDragon();
	}

	void changeDragon()
	{
        List<Soul> dragonBallList = null;
		if(Core.Data.dragonManager.currentDragonType == DragonManager.DragonType.EarthDragon)
		{
            dragonBallList = Core.Data.soulManager.GetFramentByType(ItemType.Earth_Frage);
			//	this.callDragonExpLabel = this.callEarthDragonExpLabel;
			//this.callDragonExpSlider = this.callEarthDragonExpSlider;
			this.callDragonTime = Core.Data.dragonManager.callEarthDragonTime;
			//	this.callDragonLv = this.callEarthDragonLv;
		}
		else if(Core.Data.dragonManager.currentDragonType == DragonManager.DragonType.NMKXDragon)
		{
            dragonBallList = Core.Data.soulManager.GetFramentByType(ItemType.Nameike_Frage);
			//	this.callDragonExpLabel = this.callNMKXDragonExpLabel;
			//this.callDragonExpSlider = this.callNMKXDragonExpSlider;
			this.callDragonTime = Core.Data.dragonManager.callNMKXDragonTime;
			//	this.callDragonLv = this.callNMKXDragonLv;
		}

		this.mianZhanTime = Core.Data.dragonManager.mianZhanTime;

//		showAoYiList();

		changeCallDragonButtonState((dragonBallList.Count == 7) && (Core.Data.dragonManager.DragonList[(int)Core.Data.dragonManager.currentDragonType].RTData.st == (long)0));
        setDragonBallCount(Core.Data.soulManager.GetFramentByType(ItemType.Earth_Frage), this.earthDragonBallCountList, this.earthDragonBallIconList, this.earthBallBg, DragonManager.DragonType.EarthDragon);
        setDragonBallCount(Core.Data.soulManager.GetFramentByType(ItemType.Nameike_Frage), this.NMKXDragonBallCountList, this.NMKXDragonBallIconList, this.NMKXBallBg, DragonManager.DragonType.NMKXDragon);

        //	showCallDragonExp();

		showCallDragonTime();

		showMianZhanTime();
	}
    //展示 龙珠 数量
    void setDragonBallCount(List<Soul> dragonBallList, List<UILabel> dragonBallCountList, List<UISprite> dragonBallIconList, List<UISprite> ballBgList, DragonManager.DragonType setDragonType)
	{

		foreach(UILabel dragonBallText in dragonBallCountList)
		{
			dragonBallText.text = "0";
		}
		foreach(UISprite dragonBallIcon in dragonBallIconList)
		{
			dragonBallIcon.gameObject.SetActive(false);
		}

        List<int> ballDragonList = new List<int>();
		for(int i = 0; i < dragonBallList.Count; i++)
		{
            Soul dragonBall = dragonBallList[i];
			int dragonBallIndex = 0;

			if(setDragonType == DragonManager.DragonType.EarthDragon)
			{
                dragonBallIndex = dragonBall.m_RTData.num - 150001;
			}
			else if(setDragonType == DragonManager.DragonType.NMKXDragon)
			{
                dragonBallIndex = dragonBall.m_RTData.num - 150008;
            }

            ballDragonList.Add(dragonBallIndex);
            dragonBallCountList[dragonBallIndex].text = dragonBall.m_RTData.count.ToString();


            if(dragonBall.m_RTData.count != 0)
			{
                dragonBallIconList[dragonBallIndex].gameObject.SetActive(true);
                ballBgList[dragonBallIndex].spriteName = "dragon-1004";
                if (setDragonType == DragonManager.DragonType.EarthDragon)
                {
                    earthBallLight[dragonBallIndex].SetActive(false);
                }
                else if (setDragonType == DragonManager.DragonType.NMKXDragon)
                {
                    NMKXBallLight[dragonBallIndex].SetActive(false);
                }
               
            } else {
                dragonBallIconList[dragonBallIndex].gameObject.SetActive(false);
                ballBgList[dragonBallIndex].spriteName = "dragongrey";
               
            }
            dragonBallIconList[dragonBallIndex].spriteName = "dragon-" + (dragonBallIndex + 1);
        }
        for (int i = 0; i < 7; i++)
        {
            if (!ballDragonList.Contains(i))
            {
                if (setDragonType == DragonManager.DragonType.EarthDragon)
                {
                    earthBallLight[i].SetActive(true);
                }
                else if (setDragonType == DragonManager.DragonType.NMKXDragon)
                {
                    NMKXBallLight[i].SetActive(true);
                }
            }
            else
            { if (setDragonType == DragonManager.DragonType.EarthDragon)
                {
                    earthBallLight[i].SetActive(false);
                }
            else if (setDragonType == DragonManager.DragonType.NMKXDragon)
            {
                NMKXBallLight[i].SetActive(false);
            }

            }
        }

       
        if (setDragonType == DragonManager.DragonType.EarthDragon) {
            if (dragonBallList == null|| dragonBallList.Count ==0) {
                foreach (GameObject  ball in earthBallLight) {
                    ball.SetActive (true);
                }
            }
        } else if(setDragonType == DragonManager.DragonType.NMKXDragon){
            if (dragonBallList == null || dragonBallList.Count == 0) {

                foreach (GameObject  ball in NMKXBallLight) {
                    ball.SetActive (true);
                }
            }
        }

	}


	public void learnAoYiCompleted(AoYi aoYi)
	{
		Core.Data.dragonManager.learnAoYiCompletedDelegate = null;

        List<Soul> dragonBallList = null;
		List<UILabel> dragonBallCountList = null;
		List<UISprite> dragonBallIconList = null;
        List<UISprite> dragonBallCountBGList = null;

		DragonManager.DragonType setDragonType = DragonManager.DragonType.None;

		if(aoYi.AoYiDataConfig.dragonType == ((short)DragonManager.DragonType.EarthDragon + 1))
		{
            dragonBallList = Core.Data.soulManager.GetFramentByType(ItemType.Earth_Frage);
			dragonBallCountList = this.earthDragonBallCountList;
			dragonBallIconList = this.earthDragonBallIconList;
			setDragonType = DragonManager.DragonType.EarthDragon;
            dragonBallCountBGList = this.earthBallBg;
		}
		else
		{
            dragonBallList = Core.Data.soulManager.GetFramentByType(ItemType.Nameike_Frage); //Core.Data.itemManager.getDragonBall(ItemType.Nameike_Frage);
			dragonBallCountList = this.NMKXDragonBallCountList;
			dragonBallIconList = this.NMKXDragonBallIconList;
			setDragonType = DragonManager.DragonType.NMKXDragon;
            dragonBallCountBGList = this.NMKXBallBg;

		}

        ReduceDragonBallCount(dragonBallList);

        setDragonBallCount(dragonBallList, dragonBallCountList, dragonBallIconList, dragonBallCountBGList, setDragonType);

		showAoYiSolt(false);

		if(setDragonType == Core.Data.dragonManager.currentDragonType)
		{
			changeCallDragonButtonState((dragonBallList.Count == 7) && (Core.Data.dragonManager.DragonList[(int)Core.Data.dragonManager.currentDragonType].RTData.st == (long)0));
		}

        for(int i = 0; i < Core.Data.dragonManager.callDragonSucceed.Length; i++)
        {
            if(Core.Data.dragonManager.callDragonSucceed[i])
            {
//                TaskID taskID = TaskID.None;
//                if((DragonManager.DragonType)i == DragonManager.DragonType.EarthDragon)
//                {
//                    taskID = TaskID.CallEarthDragonTimer;
//                }
//                else if ((DragonManager.DragonType)i == DragonManager.DragonType.NMKXDragon)
//                {
//                    taskID = TaskID.CallNMKXDragonTimer;
//                }

                Core.Data.dragonManager.callDragonSucceed[i] = false;
                break;
            }
        }

        gameObject.GetComponent<UIShenlongBallAnim> ().StopLabelEffect ();

        canSync = true;

	}
	//减少合成龙珠数量
    void ReduceDragonBallCount(List<Soul> dragonBallList) {
        for(int i = 0; i < dragonBallList.Count; i ++)
        {
            Soul item = dragonBallList[i];
           
            item.m_RTData.count--;

            if (item.m_RTData.count <= 0) {
                Core.Data.soulManager.RemoveItem (item.m_RTData.id);
            }
        }
    }

    void ClickBack()
    {
        if (!Core.Data.guideManger.isGuiding) {
            if (CallBackToTask != null) {
                CallBackToTask ();
            }
        }
        isState = false;
        CallBackToTask = null;
		UIMiniPlayerController.ElementShowArray = new bool[]{true,true,false,true,true};
		UIMiniPlayerController.Instance.SetActive(true);
		Core.Data.dragonManager.qiangDuoDragonBallPpid = -1;
		Core.Data.dragonManager.currentDragonType = DragonManager.DragonType.EarthDragon;
        NMKXCheckmark1.SetActive (false);
        NMKXCheckmark2.SetActive (false);
        earthCheckmark1.SetActive (true);
        earthCheckmark2.SetActive (true);
        earthCheckmark1.gameObject.transform.localPosition = Vector3.left * 120;
        earthCheckmark2.gameObject.transform.localPosition = Vector3.right * 120;
		FinalTrialMgr.GetInstance().jumpTo = TrialEnum.None;
        gameObject.SetActive (false);
        DuoBaoPanelScript.Instance.RefreshTip();

		CancelInvoke ("SafeCount");
    }

    void OnDestroy() {
        Core.Data.dragonManager.callDragonTimerEvent = null;
	}
	
	
	// id : 0~6   
	public DragonBallCell GetDragonBallButton(int id)
	{
		if(id>0 || id<earthDragonBallCountList.Count)
		return earthDragonBallCountList[id].transform.parent.parent.gameObject.GetComponent<DragonBallCell>();
		return null; 
	}
	
	//选择第一个奥义
	public void ClickYaoYi1()
	{
		if(selectAoYiAlert != null)
		{
			AoYiSlot[] aoYiSlots =selectAoYiAlert.aoYiGrid.GetComponentsInChildren<AoYiSlot>();
			if(aoYiSlots.Length > 0)
			{
				aoYiSlots[0].SelectedDelegate = selectAoYiAlert.selectedAoYiSlot;
				aoYiSlots[0].OnClicked();
			}
		}
	}
	
	public void BtnSelectedAoYiOk()
	{
		 if(selectAoYiAlert != null)
		{
			selectAoYiAlert.OnSelectAoYi();
		}
	}
	


    #region 抢夺龙珠 次数限制

    public void ShowLeftTimes(){
        if (FinalTrialMgr.GetInstance().allPVPRobData != null)
        {
            if(FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus != null){
                GetDuoBaoLoginInfoDataPvpStatus robBallStatus = FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus.ball;
                if (robBallStatus != null)
                {

					int tNum = robBallStatus.count - robBallStatus.passCount;
					if (tNum <= 0)
						tNum = 0;
					robBallTimes.text = tNum.ToString();
                }
            }
        }
    }
    #endregion

	//检测是否同时召唤
	void CheckDouble(){
		int tNum = 0;
		for (int i = 0; i < Core.Data.dragonManager.callDragonSucceed.Length; i++) {
			if (Core.Data.dragonManager.isCheckedCallDragon[i] == false)
			{
				if (Core.Data.dragonManager.callDragonSucceed[i] == true)
				{
					tNum++;
				}
			}
		}
		if (tNum == 2)
			isDouble = true;
		else
			isDouble = false;
	}
	//确保倒计时 停止的时候   时间继续走 
	void SafeCount(){
		if (Core.Data.dragonManager.callEarthDragonTime == 1) {
			RED.Log (" safe count ==== 1");
			Core.Data.dragonManager.callEarthDragonTimeCompletedDelegate = null;
			Core.Data.dragonManager.callEarthDragonTime = 0;
			this.callDragonTime = 0;
			Core.Data.dragonManager.DragonList[(int)DragonManager.DragonType.EarthDragon].RTData.st = 0;
			Core.Data.dragonManager.callDragonSucceed[(int)DragonManager.DragonType.EarthDragon] = true;
			this.callDragonCompletedTaskID [(int)DragonManager.DragonType.EarthDragon] = TaskID.CallEarthDragonTimer;
			showCallDragonTime ();
			if(Application.loadedLevelName != "GameUI")
			{
				return;
			}
			checkCallDragonSucceed();
		}
	}

	void SafeCountNMKX(){
		if (Core.Data.dragonManager.callNMKXDragonTime == 1) {
			RED.Log (" safe count nmkx ==== 1");
			Core.Data.dragonManager.callNMKXDragonTimeCompletedDelegate = null;
			Core.Data.dragonManager.callNMKXDragonTime = 0;
			this.callDragonTime = 0;
			Core.Data.dragonManager.DragonList[(int)DragonManager.DragonType.NMKXDragon].RTData.st = 0;
			Core.Data.dragonManager.callDragonSucceed[(int)DragonManager.DragonType.NMKXDragon] = true;
			this.callDragonCompletedTaskID [(int)DragonManager.DragonType.NMKXDragon] = TaskID.CallNMKXDragonTimer;
			showCallDragonTime ();
			if(Application.loadedLevelName != "GameUI")
			{
				return;
			}
			checkCallDragonSucceed();
		}
	}

	void SafeMianzhanCount(){
		if (Core.Data.dragonManager.mianZhanTime == 1) {
			Core.Data.dragonManager.mianZhanTimeCompletedDelegate = null;
			Core.Data.dragonManager.mianZhanTime = 0;
			this.mianZhanTime = 0;
			isMianZhanTimerChange = true;
		}
	}

}
