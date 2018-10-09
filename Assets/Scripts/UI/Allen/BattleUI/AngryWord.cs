using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AW.Battle;

public class AngryWord : MonoBehaviour {

    /// 技能的倍数
    /// index 0 是高位， index 1是低位

    public UISprite[] Number_Mul;
	
    /// <summary>
    /// 单个数字
    /// </summary>

    public UISprite   Number_Sin;

    /// <summary>
    /// 两位数字的物体
    /// </summary>
    public GameObject Go_Mul;

    /// <summary>
    /// 一位数字的物体
    /// </summary>
    public GameObject Go_Sin;

    /// <summary>
    /// 技能的名字
    /// </summary>

    public UILabel   SkillName;

    ///   动画参数
    public string _____ = "动画参数";

    //初始的大小
    public Vector3 InitVec = new Vector3(2f, 2f, 2f);
    //最大的放大倍数
    public Vector3 MaxNumVec = new Vector3(2f, 2f, 2f);
    //缩放的时间
    public float ScaleTime = 0.1f;
    public float ScaleNumTime = 0.1f;
    //震动的幅度
    public Vector3 ShakeVec = new Vector3(0.1f, 0.1f, 0.1f);
    public Vector3 ShakeNumVec = new Vector3(0.1f, 0.1f, 0.1f);
    //震动的时间
    public float ShakeTime = 0.1f;
    public float ShakeNumTime = 0.1f;

    ///
    ///当前点击了几次
    ///

    private int _clickTimes = 0;
    public int clickTimes {
        get {
            return _clickTimes;
        }
    }

    /// <summary>
    /// 由底层的战斗逻辑来通知UI层最多能点击几次
    /// </summary>
    private int Max_Click_Times = 0;

    /// <summary>
    /// X物体
    /// </summary>
    public GameObject Go_Chen;

    /// <summary>
    /// “倍”物体
    /// </summary>
    public GameObject Go_Bei;

    private Vector3 ChenHao_Left1 = new Vector3(-62, 66, 0);
    private Vector3 ChenHao_Left2 = new Vector3(-42, 66, 0);

    private Vector3 BeiShu_Right1 = new Vector3(78, 62, 0);
    private Vector3 BeiShu_Right2 = new Vector3(92.4f, 62f, 0);

    void Start() {
        Hide();
    }

    public void Clear() {
        _clickTimes = 0;

        foreach(UISprite num in Number_Mul) {
            num.spriteName = "hit_0";
        }
    }

    public void Hide() {
        gameObject.SetActive(false);
        Go_Mul.SetActive(false);
        Go_Sin.SetActive(false);
        Clear();
    }

	/// <summary>
	/// 让名字在中间一些
	/// </summary>
	/// <param name="name">Name.</param>
	void AdjustName (string name) {

		string fullfilled = string.Empty;

		if(name.Length == 2) {
			fullfilled = "      " + name;
		} else if(name.Length == 3) {
			fullfilled = "   " + name;
		} else if(name.Length == 4) {
			fullfilled = "  " + name;
		} else {
			fullfilled = name;
		}

		SkillName.text = fullfilled;
	}

    /// <summary>
    /// 展示出怒气技的练级次数
    /// </summary>
    public bool ShowCount(int MaxTimes, SkillData skill) {
		AdjustName(skill.name);
        //是否有添加怒气
        bool bAdded    = false;

        if(_clickTimes == 0){
            FirstShow(MaxTimes);
            bAdded     = true;
			//wxl 
//            startTimer();
        } else {
            bAdded     = KeepGrowing();
            timer      += 1;
        }
		startTimer();

        return bAdded;
    } 

    #region 设定计时器
    private int timer = 0;

    private void startTimer() {
		///
		/// --- 如果是自动战斗，则不让时间流逝的变慢 ----
		///
		AccountConfigManager accMgr = Core.Data.AccountMgr;
		bool makeTimeQucik = accMgr.UserConfig.AutoBat == (short) 1;

		if(makeTimeQucik == false)
			TimeMgr.getInstance().setExtLine(BanTimeCenter.Scale_Down_Slow);

        long now = Core.TimerEng.curTime;
        long end = now + 8;
        TimerTask task  = new TimerTask(now, end, 1, ThreadType.MainThread);
		if(makeTimeQucik == false)
			task.onEvent    = QueueNormal;
        task.onEventEnd = QueueEnd;
        task.DispatchToRealHandler();
    }

    void QueueNormal(TimerTask t) {

        AsyncTask.QueueOnMainThread( () =>{

            if(timer <= 0) {
				if(Time.timeScale < TimeMgr.getInstance().getExtLine(BanTimeCenter.Scale_Down_To) ) {
					TimeMgr.getInstance().setExtLine(BanTimeCenter.Scale_Down_To);
                }
            } else {
				TimeMgr.getInstance().setExtLine(BanTimeCenter.Scale_Down_Slow);
            }

            timer = 0;
        });
    }
		
    void QueueEnd(TimerTask t) {
		//回归基线

		///
		///如果战斗没有暂停的话，回归
		if(!TimeMgr.getInstance().WarPause)
			TimeMgr.getInstance().revertToBaseLine();
    }

    #endregion

	#region PVP, PVE战斗的回放，自动累加攻击次数

    public void AutoShowCount(int click, string name) {
        gameObject.SetActive(true);
		SkillName.text = string.Empty;
        StartCoroutine(AutoShow(click));

        AsyncTask.QueueOnMainThread( () => {Hide();}, 4f );
    }

    IEnumerator AutoShow(int click) {
        for(int i = 0; i < click; ++ i) {
            if(i == 0) FirstShow(99);
            else KeepGrowing();

            yield return new WaitForSeconds(0.3f);
        }
    }

	#endregion

	#region 新手引导未来之战

	public void FeatureWarShow(int click, string name, System.Action finishGrowing) {
		gameObject.SetActive(true);

		if(!string.IsNullOrEmpty(name))
			AdjustName(name);

		if(click == 1) FirstShow(99);
		else KeepGrowing();

		if(finishGrowing != null) AsyncTask.QueueOnMainThread(finishGrowing, ScaleTime + ShakeTime);
		finishGrowing = null;
	}


	#endregion

    /// <summary>
    /// 第一次展示出来
    /// </summary>
    void FirstShow(int MaxTimes) {
        _clickTimes = 1;
        Max_Click_Times = MaxTimes;

        gameObject.SetActive(true);
        Go_Mul.SetActive(false);
        Go_Sin.SetActive(true);

        Go_Chen.transform.localPosition = ChenHao_Left2;
        Go_Bei.transform.localPosition  = BeiShu_Right1;

        Number_Sin.spriteName = "hit_" + _clickTimes.ToString();

        //动画
        StartCoroutine(FirstShowAnim());
    }

    /// <summary>
    /// 显示第一次的动画
    /// </summary>

    IEnumerator FirstShowAnim() {
        //先把缩放到0
        gameObject.transform.localScale = InitVec;

        //开始放大
        MiniItween.ScaleTo(gameObject, Vector3.one, ScaleTime, MiniItween.EasingType.EaseInCirc);
        yield return new WaitForSeconds(ScaleTime);

        //震动
        MiniItween.Shake(gameObject, ShakeVec, ShakeTime, MiniItween.EasingType.EaseOutCirc);
    }

    /// <summary>
    /// 持续的点击
    /// </summary>
    bool KeepGrowing() {
        bool Added = false;

        if(_clickTimes < Max_Click_Times) {
            _clickTimes += 1;

            List<int> sep = new List<int>();
            int count = MathHelper.howMany(_clickTimes, sep);

            if(count == 2) {
                Go_Mul.SetActive(true);
                Go_Sin.SetActive(false);

                Go_Chen.transform.localPosition = ChenHao_Left1;
                Go_Bei.transform.localPosition  = BeiShu_Right2;

                Number_Mul[0].spriteName = "hit_" + sep[0].ToString();
                Number_Mul[1].spriteName = "hit_" + sep[1].ToString();

                Go_Mul.transform.localPosition = new Vector3(0f, 60f, 0f);
                StartCoroutine(KeepShowAnim( new GameObject[] {Go_Mul}) );
            } else if(count == 1) {
                Go_Mul.SetActive(false);
                Go_Sin.SetActive(true);

                Number_Sin.spriteName = "hit_" + sep[0].ToString();
                Go_Sin.transform.localPosition = new Vector3(0f, 60f, 0f);
                StartCoroutine(KeepShowAnim( new GameObject[] {Go_Sin}) );
            } else {
                Go_Mul.SetActive(true);
                Go_Sin.SetActive(false);

                Number_Mul[0].spriteName = "hit_9";
                Number_Mul[1].spriteName = "hit_9";

                StartCoroutine(KeepShowAnim( new GameObject[] {Go_Mul}) );
            }

            Added = true;
        }

        return Added;
    }

    /// <summary>
    /// 持续的点击
    /// </summary>

    IEnumerator KeepShowAnim(GameObject[] GoObj) {

        foreach(GameObject go in GoObj) {
            //先把缩放到0
            go.transform.localScale = MaxNumVec;

            //开始放大
            MiniItween.ScaleTo(go, Vector3.one, ScaleNumTime, MiniItween.EasingType.EaseInCirc);
            yield return new WaitForSeconds(ScaleTime);

            //震动
            MiniItween.Shake(go, ShakeNumVec, ShakeNumTime, MiniItween.EasingType.EaseOutCirc);
        }
       
    }

    /*** 测试代码
    void OnGUI() {
        if(GUI.Button(new Rect(0,0, 100, 100), "show growing")) {
            ShowCount(20, null);
        }

        if(GUI.Button(new Rect(0, 100, 50, 50), "show Clear")) {
            Clear();
        }

        if(GUI.Button(new Rect(0, 200, 50, 50), "Keep Going")) {
            ShowCount(20, null);
        }
    }
    */

}