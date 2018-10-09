using UnityEngine;
using System.Collections;
using System;

public class ActDinnerController : RUIMonoBehaviour {

	public enum DinnerState
	{
		isWaiting,
		isDone,
	}

	public DinnerState curDinnerState;
	private static ActDinnerController instance;
	public static ActDinnerController Instance
	{
		get
		{
			return instance;
		}
	}

	public GameObject eatingPanel;
	public GameObject WaitingPanel;
	public UILabel lbl_nextTime;
	[HideInInspector]
	public int startTime;
	[HideInInspector]
	public int endTime;

    public long dinnerTime;
 
    public UILabel LblTextTime;
    public UISprite sp_dinner;
    string  NotEatName = "chilamian-1";
    string  eatedName = "chilamian-3";

	public static ActDinnerController CreateDinnerPanel (ActivityItemType type, GameObject tObj)
	{
		UnityEngine.Object obj = WXLLoadPrefab.GetPrefab (WXLPrefabsName.UIDinnerPanel);
		if (obj != null) {
			GameObject go = Instantiate (obj) as GameObject;
			ActDinnerController fc = go.GetComponent<ActDinnerController> ();
			Transform goTrans = go.transform;
			go.transform.parent = tObj.transform;
			go.transform.localPosition = Vector3.zero;
            goTrans.localScale = Vector3.one;

            RED.TweenShowDialog(go);
			return fc;
		}
		return null;		
	}
        
	void Awake(){
		instance = this;
	}
	
	void Start(){
		//判定状态
		ActivityNetController.GetInstance ().StartDinnerTimeState ();
        LblTextTime.enabled = false;
	}

	public void Init(HaveDinnerStateResponse dinnerState){
        if (dinnerState.data.dinner.isopen == true) {
            if (dinnerState.data.stat == true) {
                ActivityManager.isOpen = 1;

            } else {
                ActivityManager.isOpen = 2;
                sp_dinner.spriteName = eatedName;
            }
        } else {
            if (dinnerState.data.stat == true) {
                ActivityManager.isOpen = 2;
                sp_dinner.spriteName = NotEatName;
            } else {
                ActivityManager.isOpen = 2;
                sp_dinner.spriteName = eatedName;
            }
        }

        if(ActivityManager.isOpen ==1){
			curDinnerState = DinnerState.isDone;
            Core.Data.ActivityManager.SetDailyGiftState (ActivityManager.dinnerType,"1");
		}else{
			curDinnerState = DinnerState.isWaiting;
            Core.Data.ActivityManager.SetDailyGiftState (ActivityManager.dinnerType,"2");
		}

		if(dinnerState.data.dinner.dur != null){
			startTime = dinnerState.data.dinner.dur[0];
			endTime = dinnerState.data.dinner.dur[1];
		}
		Refresh();
	}


	public void OnBtnBack(){
        Destroy(gameObject);
	}


	public void OnHavingDinner(){
        if (curDinnerState == DinnerState.isDone) {
            ActivityNetController.GetInstance ().EatDinnerRequest ();
        } else {
            ActivityNetController.ShowDebug (Core.Data.stringManager.getString(7330));
        
        }
	}

	public void Refresh(){
        sp_dinner.MakePixelPerfect ();
		switch (curDinnerState) {
		case DinnerState.isDone:
			eatingPanel.SetActive (true);
			WaitingPanel.SetActive (false);
			break;
		case DinnerState.isWaiting:
            if (DateHelper.UnixTimeStampToDateTime (Core.TimerEng.curTime, true).Hour >= 18) {
                startTime = 12;
                endTime = 14;
            } else if(DateHelper.UnixTimeStampToDateTime (Core.TimerEng.curTime, true).Hour < 14 && DateHelper.UnixTimeStampToDateTime (Core.TimerEng.curTime, true).Hour >= 12){
                startTime = 18;
                endTime = 20;
            }
			eatingPanel.SetActive (false);
			WaitingPanel.SetActive (true);
            lbl_nextTime.text = startTime + ":00-"+ endTime+":00"; 
			break;
		default:
			eatingPanel.SetActive (false);
			WaitingPanel.SetActive (true);
			break;
		}
	}

//    void UpdateTime(){
//        dinnerTime = ActivityNetController.GetInstance ().dinnerLeft;
//        int l =0;
//        string output = "";
//        output += (dinnerTime/3600).ToString("d2");
//        l = (int)dinnerTime % 3600;
//        output += ":" + (l / 60).ToString ("d2");
//        l = (int)l % 60;
//        output+=":"+l.ToString("d2");
//        LblTextTime.text = output;
//    }
        

}
