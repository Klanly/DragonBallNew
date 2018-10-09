using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


public class UIDateSignController : RUIMonoBehaviour{


	private static UIDateSignController instance;
	public static UIDateSignController Instance
	{
		get
		{
			return instance;
		}
	}

	public static int monthDaysNum;
    public GameObject itemGroupObj;
    [HideInInspector]
    public  List<DateSignItem> ItemList;
	public UILabel lbl_Times;
	public UILabel lbl_leavesTimes;
	public static bool canClick = true; 
	public  int signtms;
	public  int pairmax;
	public  int pairtms;
	//今天几号 
	public static DateTime nowTime ;


	public static UIDateSignController CreateUIdateSignPanel (ActivityItemType type, GameObject tObj)
	{
      
		UnityEngine.Object obj = WXLLoadPrefab.GetPrefab (WXLPrefabsName.UISignPanel);
		if (obj != null) {
			GameObject go = Instantiate (obj) as GameObject;
			UIDateSignController fc = go.GetComponent<UIDateSignController> ();
			Transform goTrans = go.transform;
			go.transform.parent = tObj.transform;
			go.transform.localPosition = Vector3.zero;
			goTrans.localScale = Vector3.one;
			RED.TweenShowDialog (go);
			return fc;
		}
		return null;		
	}

	void Awake(){
		instance = this;
        nowTime = DateHelper.UnixTimeStampToDateTime (Core.Data.playerManager.RTData.systemTime).ToLocalTime();
		canClick = true; 
        ComLoading.Open();
        StartCoroutine(RequestState());
	}

    //void RequestState(){
    IEnumerator RequestState(){  
        yield return new WaitForSeconds(0.3f); 
        if (Core.Data.noticManager.GetDateGiftList () == null || Core.Data.noticManager.GetDateGiftList ().Count == 0) {
            ActivityNetController.GetInstance ().SignDateStateRequest ();
        } else {
            this.Init (null);
        }
    }

	/// <summary>
	/// 初始化 每日签到数据
	/// </summary>
	public void Init(SignDateStateResponse SDResponse){
        ItemList = new List<DateSignItem> ();
        if (SDResponse != null) {
            pairmax = SDResponse.data.pairmax;
            pairtms = SDResponse.data.pairtms;
            signtms = SDResponse.data.signtms;

            NoticeManager.pairmax = SDResponse.data.pairmax;
            NoticeManager.pairtms =SDResponse.data.pairtms;
            NoticeManager.signtms = SDResponse.data.signtms;
            Core.Data.noticManager.FullFillDateGiftDateList (SDResponse.data.days);
        } else {
            pairmax = NoticeManager.pairmax;
            pairtms = NoticeManager.pairtms;
            signtms = NoticeManager.signtms;
        }


        if (ItemList.Count == 0)
        {
            SpawnItem();
        }

        ComLoading.Close();

        List<DataGiftData> tList = Core.Data.noticManager.GetDateGiftList ();
		//item  赋值 
		for (int i = 0; i < ItemList.Count; i++) 
        {
            ItemList [i].localNum = i;

            if (i < signtms) {
				ItemList [i].curSignItem = DateSignItem.SignItemState.isSigned;
			} else if(i>signtms) {
				ItemList [i].curSignItem = DateSignItem.SignItemState.isNotSigned;
			}else if(i == signtms ){
				//if( int.Parse(Core.Data.ActivityManager.GetActivityStateData(ActivityManager.signType)) == 1)
				if( int.Parse(Core.Data.ActivityManager.GetSignState()) == 1)
					ItemList[i].curSignItem = DateSignItem.SignItemState.isWaitSigned;
			   else
					ItemList[i].curSignItem =DateSignItem.SignItemState.isNotSigned;
			}

            ItemList [i].SetItemValue (  tList[i].signItem);
           
		}

		this.Refresh ();

	}
        

	public void SpawnItem(){

		if (nowTime.Month == 1 || nowTime.Month == 3 || nowTime.Month == 5 || nowTime.Month == 7 || nowTime.Month == 8 || nowTime.Month == 10 || nowTime.Month == 12) {
			monthDaysNum = 31;
		}
		else if(nowTime.Month == 4||nowTime.Month == 6||nowTime.Month == 9||nowTime.Month == 11){
				monthDaysNum = 30;
		}
		else if(nowTime.Month == 2)
		{ 
			if(nowTime.Year % 4 == 0)
			{	
				monthDaysNum = 29;
			}
			else
				monthDaysNum = 28;
		} 

		UnityEngine.Object ItemObj = WXLLoadPrefab.GetPrefab (WXLPrefabsName.UISignItem);
		for (int i = 0; i < monthDaysNum; i++) {
			GameObject tObj = Instantiate (ItemObj)as GameObject;
            tObj.transform.parent = itemGroupObj.transform;
			tObj.transform.localScale = Vector3.one;
            int tY = 55-((int)i / 5)*168 ;
            int tX = (i % 5) * 173- 350 ;
			tObj.transform.localPosition = new Vector3 (tX,tY,0);
            ItemList.Add (tObj.GetComponent<DateSignItem>());
		}
	}
		

	public void OnBackBtn(){
        NoticeManager.openSign = false;
        Destroy(gameObject);
		SQYMainController.mInstance.UpdateDailySign ();
	}

	/// <summary>
	/// 补签 成功 返回
	/// </summary>
	public void BackBuQian(SignDayResponse resp){
		DateSignItem tItemData = null;
		tItemData = ItemList [signtms];
        tItemData.curSignItem = DateSignItem.SignItemState.isSigned;
        tItemData.Refresh();
		SignItem tItem = tItemData.itemValue;
		canClick = true;
        pairtms++;
		signtms++;

		this.ShowGetGift (tItem,resp.data.p);

        this.Refresh();
	}

	/// <summary>
	/// 返回正常签到
	/// </summary>
	public void BackUINormalSignDay(SignDayResponse resp){

		ItemList [signtms].curSignItem = DateSignItem.SignItemState.isSigned;
		ItemList [signtms].Refresh ();
		SignItem tItem = (SignItem)ItemList [signtms].ReturnValue ();
		canClick = true;	
		this.ShowGetGift (tItem,resp.data.p);
		signtms++;
        this.Refresh();
	}

	/// <summary>
	/// 展示签到成功
	/// </summary>
	public void ShowGetGift(SignItem Reward,ItemOfReward[] tIOR){


        if (Core.Data.playerManager.curVipLv >= Reward.vip && Reward.vip != 0)
        { 
            // Dictionary<int,int> pidList = new  Dictionary<int, int>();  
            //分析  分成两部分 给奖励 主要为了 vip展示 
//            for (int i = 0; i < tIOR.Length; i++) {
//                if (pidList.ContainsKey (tIOR [i].pid))
//                    pidList [tIOR [i].pid]+= tIOR[i].num;
//                else
//                    pidList.Add (tIOR [i].pid, tIOR[i].num);
//            }
//
//            ItemOfReward[] tIor = null;
//            foreach(int pair in pidList.Keys){
//
//                ItemOfReward iRe = new ItemOfReward ();
//                iRe.pid = pair;
//                if (pidList.TryGetValue (iRe.pid, out iRe.num)) {
//                    iRe.num = iRe.num / 2;
//                }
//                tIor = new ItemOfReward[]{ iRe, iRe };
//
//            }
//            Debug.Log (" item  num " + tIor.Length  );
            ItemOfReward item = new ItemOfReward ();
            if (tIOR != null) {
                item = tIOR [0];
                item.num = tIOR [0].num / 2;
            }

            ItemOfReward[] result = new ItemOfReward[]{ item,item};
            GetRewardSucUI.OpenUI (result,Core.Data.stringManager.getString(5047),false);
            
        }
        else{
            GetRewardSucUI.OpenUI (tIOR,Core.Data.stringManager.getString(5047));
        }
//		UIActivityReward ar = WXLLoadPrefab.GetPrefabClass<UIActivityReward>();
//		if(ar != null)
//		{
//			Transform t = ar.transform;
//			t.parent = transform;
//			t.localPosition = Vector3.back;
//			t.localRotation = Quaternion.identity;
//			t.localScale = Vector3.one;
//
//		}

	}
	/// <summary>
    /// 补签 发 0 就可以 
	/// </summary>
	public void OnBuQianBtn(){
        if (nowTime.Day <= signtms) {
            ActivityNetController.ShowDebug (Core.Data.stringManager.getString(7328));
			return;
		}

        if(Core.Data.playerManager.RTData.curStone < ActivityNetController.buqian){
            ActivityNetController.ShowDebug(Core.Data.stringManager.getString(35006));
            return;
        }

        if (pairmax - pairtms > 0) {
            if (canClick == true) {
                ActivityNetController.GetInstance ().SignDayRequest (0);
            }
        } else {
            ActivityNetController.ShowDebug (Core.Data.stringManager.getString(34050));
          
        }
	}

	public void Refresh(){
		lbl_leavesTimes.text = (pairmax - pairtms).ToString ();
		lbl_Times.text = signtms.ToString ();
		SQYMainController.mInstance.UpdateDailySign ();
//		if (UIWXLActivityMainController.Instance != null) {
//            UIWXLActivityMainController.Instance.Refresh ();
//        }
		this.AutoMoveToTarget ();
	}

	void AutoMoveToTarget(){
		int targetNum = (signtms / 5 )* 170;
		if (targetNum  > 615)
			targetNum = 615;
		Vector3 targetPos = new Vector3 (0,targetNum, 0);
		SpringPanel.Begin (itemGroupObj.transform.parent.gameObject, targetPos + Vector3.down * 14, 8);
		itemGroupObj.transform.parent.GetComponent<SpringPanel> ().enabled = true;
	}


}
