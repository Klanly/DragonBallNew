using UnityEngine;
using System.Collections;
using System;

public class SQYPlayerController : MDBaseViewController {

	public const int ACT_BTN_COIN = 1;
	public const int ACT_BTN_STONE = 2;
    public const int ACT_BTN_ENERGY = 3;
    public const int ACT_BTN_POWER = 4;

	public const float TWEENSCALE_TIME = 0.1F;

	public System.Action<int> myPlayerViewBehaviour;

	public UILabel lab_coin;//金币
	public UILabel lab_stone;//钻石
	public UILabel lab_Energy;//精力
	public UISlider energySlider;

	public UILabel lab_Exp; // exp
	public UISlider ExpSlider;
	public UIPanel mPanel;


	//显示精力的类型（1，倒计时显示， 2，精力值显示）
	private	int m_nShowEnergType = 1;
	private int ENERGY_SWITCH_TIME = 20;
	private int m_nTime;

	public  static TimeSpan energyTime;
	public TimeSpan powerTime;

    //启动 心跳 根据 action button
    public UISprite[] spFunBtn;

	public TweenScale m_scaleRoot;

	private PlayerManager player = null;
	private int mCurJingLi = 0;

	// Use this for initialization

	private void Start () 
	{

		player = Core.Data.playerManager;
		mCurJingLi = 0;
		freshPlayerInfoView ();

		if (gameObject.name.Contains("pbSQYPlayerController"))
		{
			InitTimer ();
		}
	}

	protected void InitTimer()
	{
		CalTimer();
		if (!this.IsInvoking ("AutoChecker")) {
//			Debug.Log ("run  auto checker ");
			InvokeRepeating ("AutoChecker", 0.01f, 1.0f);
		}
		m_nTime = ENERGY_SWITCH_TIME;
		//InvokeRepeating ("StartTime", 0.02f, 1);

		StopCoroutine ("StartTime");
		StartCoroutine ("StartTime");
	}

	void OnDestory() {
		CancelInvoke("AutoChecker");
		StopCoroutine ("StartTime");
	}

	#region 体力和精力的自动回复模块

	protected void AutoChecker() {
		if(player==null)
		{
			Debug.LogError("AutoChecker() player==null add by jc");
			return;
		}

		if(mCurJingLi != player.curJingLi) {
			if (lab_Energy != null) {
				lab_Energy.SafeText (player.curJingLi.ToString () + "/" + player.totalJingli.ToString ());
			}
			mCurJingLi = player.curJingLi;
		}

		//精力满的情况下
		if(player.curJingLi >= player.totalJingli) {
			Core.TimerEng.deleteTask(TaskID.CalJingLi);

		} else {
//			Debug.Log (" checkkkkkkkk  == " + Core.TimerEng.checkExist (TaskID.CalJingLi)+ "   ==    energy   -===  "   + energyTime.Seconds);
			if (!Core.TimerEng.checkExist (TaskID.CalJingLi)) {
				long nNow = Core.TimerEng.curTime;
				Core.TimerEng.deleteTask(TaskID.CalJingLi);

				ConsoleEx.DebugLog ("player.RTData.unixTimeForJingLi = " + player.RTData.unixTimeForJingLi);
				TimerTask MonaTask = new TimerTask (nNow, nNow + player.RTData.unixTimeForJingLi, 1, ThreadType.MainThread);
				MonaTask.taskId = TaskID.CalJingLi;
				MonaTask.onEventEnd +=this.JingLiCalEnd;
				MonaTask.onEvent += this.JingliMinus;
				MonaTask.DispatchToRealHandler ();
			} 
			
		}
	}
        
	//开始启动计时机制
	private IEnumerator StartTime()
	{
		while (true)
		{
			yield return new WaitForSeconds (1);
			m_nTime--;
			if (m_nShowEnergType == 2)
			{
				if (player.curJingLi <player.totalJingli && energyTime.TotalSeconds > 0)
				{
					string sTime = string.Format("{0:D2}:{1:D2}", energyTime.Minutes, energyTime.Seconds);//energyTime.Minutes.ToString ("{0:D2}") + ":" + energyTime.Seconds.ToString ("{0:D2}");
					DBUIController.mDBUIInstance._playerViewCtl.lab_Energy.SafeText(sTime);
				}
			}

			if (m_nTime <= 0)
			{
				m_nTime = ENERGY_SWITCH_TIME;
				if (m_nShowEnergType == 1 && Core.Data.playerManager.RTData.curJingLi < Core.Data.playerManager.totalJingli )
				{
					m_nShowEnergType = 2;
				}
				else
				{
					m_nShowEnergType = 1;
					DBUIController.mDBUIInstance._playerViewCtl.lab_Energy.SafeText(player.curJingLi.ToString()+"/"+player.totalJingli.ToString());
				}
			}
		}
	}
	//第一次 执行
	protected void CalTimer() {
		if(player.RTData.isFirstData) {
			player.RTData.isFirstData = false;
			Core.TimerEng.deleteTask(TaskID.CalJingLi);
			long passedTime = Core.TimerEng.curTime - player.RTData.systemTime;

			long calTime = 0;

			long now = Core.TimerEng.curTime;

			//精力
			calTime = 0;

			if(passedTime < player.RTData.unixTimeForJingLiFull) {
				calTime = player.RTData.unixTimeForJingLiFull - passedTime;
			} else if(passedTime == player.RTData.unixTimeForJingLiFull) {
				calTime = player.RTData.unixTimeForJingLi;
			} else {
				//体力已经增加一点了。
				calTime = player.RTData.unixTimeForJingLi - ( passedTime - player.RTData.unixTimeForJingLiFull );
				if(player.totalJingli > player.curJingLi)
					player.RTData.curJingLi += (int)( (passedTime - player.RTData.unixTimeForJingLiFull) / player.RTData.unixTimeForJingLi + 1 );
			}

			if(player.totalJingli > player.curJingLi) {
//				ConsoleEx.DebugLog ("calTime = " + calTime,"yellow");
				TimerTask MonaTask = new TimerTask(now, now + calTime, 1, ThreadType.MainThread);
				MonaTask.taskId = TaskID.CalJingLi;
				MonaTask.onEventEnd += JingLiCalEnd;
				MonaTask.onEvent = JingliMinus;
				MonaTask.DispatchToRealHandler();
			}
		}
	}


	//精力计时完成
	protected void JingLiCalEnd(TimerTask task) {
		if(player.totalJingli > player.RTData.curJingLi) {
			player.RTData.curJingLi += 1;
			if (lab_Energy != null) {
				lab_Energy.SafeText (player.curJingLi.ToString () + "/" + player.totalJingli.ToString ());

				float res = (float)player.curJingLi / (float)player.totalJingli;
				if(energySlider != null)
				{
					energySlider.value = res;
				}
			}
		}
//		Debug.LogWarning ("  jing li  end    total jingli = " + player.totalJingli + " cur  jingli " + player.RTData.curJingLi);
		if(player.totalJingli > player.RTData.curJingLi) {
			long nNow = Core.TimerEng.curTime;

			TimerTask MonaTask = new TimerTask(nNow, nNow + player.RTData.unixTimeForJingLi, 1, ThreadType.MainThread);
			MonaTask.taskId = TaskID.CalJingLi;
			MonaTask.onEventEnd += JingLiCalEnd;
			MonaTask.onEvent += JingliMinus;
			MonaTask.DispatchToRealHandler();
		}
	}


	protected void JingliMinus(TimerTask task)
	{
		if (m_nShowEnergType == 2)
		{
			DateTime time = DateTime.Now.AddSeconds (task.leftTime);
			energyTime = time - DateTime.Now;
//			Debug.Log (" time === " + energyTime);
		}
	}

	#endregion

	public string formatCurrencyUnit(int num)
	{
		string s = num.ToString();
		return s;
	}

	/// <summary>
	/// 由此更改效果
	/// </summary>
	public void freshPlayerInfoView()
    {
        PlayerManager pm = Core.Data.playerManager;

        if (gameObject.activeInHierarchy == false)
        {
            lab_coin.SafeText(formatCurrencyUnit(pm.Coin));
            lab_stone.SafeText(formatCurrencyUnit(pm.Stone));
            lab_Energy.SafeText(pm.curJingLi.ToString() + "/" + pm.totalJingli.ToString());


//			Debug.Log ("refresh user info");
        }
        else
        {
            if(lab_coin != null && lab_coin.text != pm.Coin.ToString())
            {
				if(Core.Data.temper._TempCurCoin == pm.Coin)
				{
					lab_coin.text = pm.Coin.ToString();
					Core.Data.temper._TempCurCoin = -1;
				}
				else
				{
					StartCoroutine( ShiftCoinEffect(lab_coin,pm.Coin));
				}
            }
            if(lab_stone != null && lab_stone.text != pm.Stone.ToString())
            {
                StartCoroutine( ShiftStoneEffect(lab_stone,pm.Stone));       
            }

			if (m_nShowEnergType == 1)
			{
				string[] txt = lab_Energy.text.Split ('/');

				if (lab_Energy != null && txt [0] != pm.curJingLi.ToString ())
				{
					StartCoroutine (ShiftEnergyEffect (lab_Energy, pm.curJingLi, pm.totalJingli));     
				}
			}
        }

		float res = (float)pm.curJingLi / (float)pm.totalJingli;
		if(energySlider != null)
		{
			energySlider.value = res;
		}


		res = (float)pm.curExp / (float)pm.nextLvExp;

		if(ExpSlider != null)
		{
			ExpSlider.value = res;
		}

		if (lab_Exp != null)
		{
			int val = (int)(res * 100);
			lab_Exp.text = val.ToString() + "%";
		}

        for (int i = 0; i < spFunBtn.Length; i++)
        {
            if(spFunBtn[i] != null)
                spFunBtn[i].gameObject.transform.localScale = Vector3.one;
        }
	}

    /// <summary>
    /// 数字变换的效果
    /// </summary>
    /// <returns>The number effect.</returns>
    /// <param name="targetNum">Target number.</param>
    public IEnumerator ShiftCoinEffect(UILabel lab ,int endNum){
        int startNum = 0;
       
  
        startNum = int.Parse(lab.text);
      
        int delta = endNum - startNum;
        int length = 0;


        if (delta >= 0)
        {
            length = (int)Mathf.Log10(delta);
            this.SetPulseScale (spFunBtn[0]);
            this.SetLabelPulseScale (lab);
            for (int i = length; i > 0; i--)
            {
                // 10 的 i 次方
                int tNum = (int)Mathf.Pow(10, i);
                //余数
                int pNum = (int)delta / tNum;
                for (int j = 0; j < pNum; j++)
                {

                    startNum += (int)(Mathf.Pow(10, i));
                    delta -= (int)Mathf.Pow(10, i);
                    lab.text = startNum.ToString();
                    yield return  new WaitForSeconds(0.03f);
                }
            }

        }
        else
        {
            delta = Mathf.Abs(delta);
            length = (int)Mathf.Log10(delta);
            this.SetPulseScale (spFunBtn[0]);
            this.SetLabelPulseScale (lab);
            for (int i = length; i > 0; i--)
            {
                // 10 的 i 次方
                int tNum = (int)Mathf.Pow(10, i);
                //余数
                int pNum =(int)delta / tNum;

                for (int j = 0; j < pNum; j++)
                {
					startNum -= (int)(Mathf.Pow(10, i));
                    delta -= (int)Mathf.Pow(10, i);
                    lab.text = startNum.ToString();
                    yield return  new WaitForSeconds(0.03f);
                }
            }
        
        }
            lab.text = endNum.ToString();

    }
        

	public void OnBtnAddCoin()
	{
		if(myPlayerViewBehaviour != null && Core.Data.playerManager.RTData.curLevel >= 5)
		{
			myPlayerViewBehaviour(ACT_BTN_COIN);
		}
	}
	public void OnBtnAddStone()
	{
		if(myPlayerViewBehaviour != null && Core.Data.playerManager.RTData.curLevel >= 1)
		{
			myPlayerViewBehaviour(ACT_BTN_STONE);
		}
	}
	
    public void OnBtnAddEnergy(){
		if (myPlayerViewBehaviour != null && Core.Data.playerManager.RTData.curLevel >= 5)
            myPlayerViewBehaviour(ACT_BTN_ENERGY);
	}

	public void OnBtnAddPower(){
		if (myPlayerViewBehaviour != null && Core.Data.playerManager.RTData.curLevel >= 5)
            myPlayerViewBehaviour(ACT_BTN_POWER);
	}




	public static SQYPlayerController CreatePlayerView()
	{
		UnityEngine.Object obj = PrefabLoader.loadFromPack("SQY/pbSQYPlayerController");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			SQYPlayerController player = go.GetComponent<SQYPlayerController>();
			return player;
		}
		return null;
	}

	public void WillShowUI(float time = TWEENSCALE_TIME)
	{
		AnimationCurve anim = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(0.4f, 1.3f, 0.5f, 0.5f), new Keyframe(1f, 1f, 1f, 0f));
		m_scaleRoot.animationCurve = anim;
		
		TweenScale.Begin(m_scaleRoot.gameObject, time, Vector3.one);
	}

	public void WillHideUI()
	{
		AnimationCurve anim = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));
		m_scaleRoot.animationCurve = anim;
	
		TweenScale.Begin(m_scaleRoot.gameObject, TWEENSCALE_TIME, Vector3.one * 0.00001f);
	}
	

    public void SetPulseScale(UISprite lab){
      
        MiniItween.ScaleTo (lab.gameObject, Vector3.one * 1.8f, 0.1f, MiniItween.EasingType.Linear).myDelegateFunc += delegate() {
            MiniItween.ScaleTo (lab.gameObject, Vector3.one * 1f, 0.1f, MiniItween.EasingType.Linear);
        };

    }

    public void SetLabelPulseScale(UILabel lab){
        MiniItween.ScaleTo (lab.gameObject, Vector3.one * 1.4f, 0.05f, MiniItween.EasingType.Linear).myDelegateFunc += delegate() {
            MiniItween.ScaleTo (lab.gameObject, Vector3.one * 1f, 0.15f, MiniItween.EasingType.Linear);
        };
    }




    public IEnumerator ShiftEnergyEffect(UILabel lab ,int endNum,int backNum =0){
        int startNum = 0;
        //     int backNum = 0;
        if (lab.text.Contains("/") == true)
        {

            string[] tStr = lab.text.Split('/');
            startNum = int.Parse(tStr[0]);
            //    backNum = int.Parse(tStr[1]);
        }
        else
        {
			if(lab.text != null && !lab.text.Contains(":"))
            	startNum = int.Parse(lab.text);
        }
        int delta = endNum - startNum;
        int length = 0;

        if (delta != 0) {
            this.SetPulseScale (spFunBtn [2]);
            this.SetLabelPulseScale (lab);
        }

        if (delta >= 0)
        {
            length = (int)Mathf.Log10(delta);


            for (int i = length; i > 0; i--)
            {
                // 10 的 i 次方
                int tNum = (int)Mathf.Pow(10, i);
                //余数
                int pNum = (int)delta / tNum;
                for (int j = 0; j < pNum; j++)
                {

                    startNum += (int)(Mathf.Pow(10, i));
                    delta -= (int)Mathf.Pow(10, i);
                    lab.text = startNum.ToString();
                    yield return  new WaitForSeconds(0.03f);
                }
                //  yield return StartCoroutine(ObjPulse(0.4f,spFunBtn[2]));
            }
        }
        else
        {
            delta = Mathf.Abs(delta);
            length = (int)Mathf.Log10(delta);
            for (int i = length; i > 0; i--)
            {
                // 10 的 i 次方
                int tNum = (int)Mathf.Pow(10, i);
                //余数
                int pNum =(int)delta / tNum;

                for (int j = 0; j < pNum; j++)
                {
                    startNum -= (int)(Mathf.Pow(10, i));
                    delta -= (int)Mathf.Pow(10, i);
                    lab.text = startNum.ToString();
                    yield return  new WaitForSeconds(0.03f);
                }
                //   yield return  StartCoroutine(ObjPulse(0.4f,spFunBtn[2]));
            }
        }
        if (backNum == 0)
            lab.text = endNum.ToString();
        else
            lab.text = endNum.ToString() + "/" + backNum.ToString();


    }

    public IEnumerator ShiftStoneEffect(UILabel lab ,int endNum){

        int startNum = 0;
     
        startNum = int.Parse(lab.text);
        int delta = endNum - startNum;
        int length = 0;
        if (delta != 0) {
            this.SetPulseScale (spFunBtn [1]);
            this.SetLabelPulseScale (lab);
        }


        if (delta >= 0)
        {
            length = (int)Mathf.Log10(delta);


            for (int i = length; i > 0; i--)
            {
                // 10 的 i 次方
                int tNum = (int)Mathf.Pow(10, i);
                //余数
                int pNum = (int)delta / tNum;
                for (int j = 0; j < pNum; j++)
                {

                    startNum += (int)(Mathf.Pow(10, i));
                    delta -= (int)Mathf.Pow(10, i);
                    lab.text = startNum.ToString();
                    yield return  new WaitForSeconds(0.03f);
                }
                //  yield return  StartCoroutine(ObjPulse(0.4f,spFunBtn[1]));
            }
        }
        else
        {
            delta = Mathf.Abs(delta);
            length = (int)Mathf.Log10(delta);
            for (int i = length; i > 0; i--)
            {
                // 10 的 i 次方
                int tNum = (int)Mathf.Pow(10, i);
                //余数
                int pNum =(int)delta / tNum;

                for (int j = 0; j < pNum; j++)
                {
                    startNum -= (int)(Mathf.Pow(10, i));
                    delta -= (int)Mathf.Pow(10, i);
                    lab.text = startNum.ToString();
                    yield return  new WaitForSeconds(0.03f);
                }
                //  yield return StartCoroutine(ObjPulse(0.4f,spFunBtn[1]));
            }
        }
    
        lab.text = endNum.ToString();
      
    }


	public override void SetActive(bool bShow)
	{
		RED.SetActive(bShow, this.gameObject);
		if (bShow) {
			StartCoroutine("StartTime");
			//InvokeRepeating ("StartTime",0,1f);

		} else {
			StopCoroutine("StartTime");
		}
	}


    void OnDisable(){
        Core.Data.temper.tempTeamAtk = Core.Data.playerManager.RTData.curTeam.teamAttack;
        Core.Data.temper.tempTeamDef = Core.Data.playerManager.RTData.curTeam.teamDefend;
	}
		

}
