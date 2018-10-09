using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DailyGiftController : RUIMonoBehaviour {
    private static DailyGiftController instance;
    public static DailyGiftController Instance
    {
        get
        {
            return instance;
        }
    }

    public UIGrid gridObj;
    public List<DailyGiftItem> dialyItemList = new List<DailyGiftItem>();
    private List<DailyGiftItemClass> curDailyGiftList = null;

    public List<int> durTime; 

    public static DailyGiftController CreatDailyGiftController(){
        UnityEngine.Object obj = WXLLoadPrefab.GetPrefab(WXLPrefabsName.UIDailyGiftPanel);
        if (obj != null) {
            GameObject go = Instantiate(obj) as GameObject;
            DailyGiftController fc = go.GetComponent<DailyGiftController>();
            Transform goTrans = go.transform;
            go.transform.parent = DBUIController.mDBUIInstance._TopRoot.transform;
            go.transform.localPosition = Vector3.zero;
            goTrans.localScale = Vector3.one;
            RED.TweenShowDialog (go);
            return fc;
        }
        return null;
    }

    void Awake(){
        instance = this;
    }

    void Start(){
        this.InitData ();    
    }

    public void Init(){
        SortList (ActivityNetController.GetInstance ().dailyGiftList,ActivityNetController.GetInstance ().sevenDailyGiftList);
        if (Core.Data.guideManger.isGuiding)
            Core.Data.guideManger.AutoRUN ();
    }
    public void InitData(){
        ActivityNetController.GetInstance ().GetAllRewardData ();
        ComLoading.Open();
    }

    public void SpawnItem(int colNum)
    {
        UnityEngine.Object ItemObj = WXLLoadPrefab.GetPrefab(WXLPrefabsName.UIDailyGiftItem);
        for (int i = 0; i < colNum; i++) {
            GameObject tObj = Instantiate(ItemObj)as GameObject;
            tObj.transform.parent = gridObj.gameObject.transform;
            tObj.transform.localScale = Vector3.one;
            tObj.transform.localPosition = Vector3.zero;
            dialyItemList.Add(tObj.GetComponent<DailyGiftItem>());
        }
    }

    public void Refresh(){
        for (int i = 0; i < dialyItemList.Count; i++) {
            if(i< curDailyGiftList.Count)
                dialyItemList [i].SetItemValue (curDailyGiftList[i]);
        }
        gridObj.GetComponent<UIGrid> ().repositionNow = true;
        ComLoading.Close ();
    }


    public void SimpleRefresh(DailyGiftItemClass.dailyItemType rType,bool dinnerType = false){
        switch(rType){
        case DailyGiftItemClass.dailyItemType.dinnerType:
            for (int i = 0; i < dialyItemList.Count; i++) {
                if (dialyItemList [i].curType == rType) {
                    DailyGiftItemClass tClass = dialyItemList [i].ReturnValue () as DailyGiftItemClass;
                    if (dinnerType == false) {//吃过 
                        tClass.canGet = false;
                        tClass.id = 1;
                    } else {//开启
                        tClass.canGet =true;
                        tClass.id = 2;
                    }
                    dialyItemList [i].SetItemValue (tClass);
                }
            }
            this.ResortList ();
            break;
        case DailyGiftItemClass.dailyItemType.levelGiftType:
            for (int j = 0; j < dialyItemList.Count; j++) {
                if (dialyItemList [j].curType == rType) {
                    DailyGiftItemClass tClass = dialyItemList[j].ReturnValue () as DailyGiftItemClass ;
                    tClass.id = ActivityNetController.UnGotGiftNum;
                    List<ItemOfReward> tItemList = new List<ItemOfReward> ();
                    List<int[]> tRewardList = Core.Data.ActivityManager.GetRewardData (tClass.id).reward;
                    for (int i = 0; i < tRewardList.Count; i++) {
                        ItemOfReward tIor = new ItemOfReward ();
                        tIor.pid = tRewardList [i] [0];
                        tIor.num = tRewardList [i] [1];
                        tItemList.Add (tIor);
                    }
                    tClass.giftReward = tItemList;
                    
                    if (tClass.id <= Core.Data.playerManager.Lv) {
                        tClass.canGet = true;
                    } else {
                        tClass.canGet = false;
                    }
                    dialyItemList [j].SetItemValue (tClass);
                }
            }
            this.ResortList ();
            break;
        case DailyGiftItemClass.dailyItemType.vipGiftType:
            for (int i = 0; i < dialyItemList.Count; i++) {
                if (dialyItemList [i].curType == rType) {
                    if (Core.Data.ActivityManager.GetDailyGiftState (ActivityManager.vipLibaoType) != "1")
                        dialyItemList [i].gameObject.SetActive (false);
                    DailyGiftItemClass tCl = dialyItemList [i].ReturnValue () as DailyGiftItemClass;
                    dialyItemList.Remove (dialyItemList [i]);
                    for (int j = 0; j < curDailyGiftList.Count; j++) {
                       
                        if (curDailyGiftList [j].curItemType == tCl.curItemType)
                            curDailyGiftList.Remove (tCl);
                    }
                }
            }
            gridObj.repositionNow = true;

            break;

        case DailyGiftItemClass.dailyItemType.sevenGiftType:
            for (int i = 0; i < dialyItemList.Count; i++) {
                if (dialyItemList [i].curType == rType) {
                    DailyGiftItemClass tClass = dialyItemList [i].ReturnValue () as DailyGiftItemClass;
                    if (tClass.id == ActivityNetController.curSevenGetIndex) {

                        for (int j = 0; j < curDailyGiftList.Count; j++) {
                            if (curDailyGiftList [j].id == tClass.id) {
                                curDailyGiftList.Remove (curDailyGiftList [j]);
                                Destroy (dialyItemList [i].gameObject);
                                dialyItemList.Remove (dialyItemList [i]);
                            }
                        }
                    }
                }
            }
            gridObj.GetComponent<UIGrid> ().repositionNow = true;
            // this.ResortList ();
            break;

        case DailyGiftItemClass.dailyItemType.monthGiftType:
            for (int i = 0; i < dialyItemList.Count; i++) {
                if (dialyItemList [i].curType == rType) {
                    DailyGiftItemClass tClass = dialyItemList [i].ReturnValue () as DailyGiftItemClass;
					tClass.id = 1;
                }
            }
            this.ResortList ();
            break;
        }

		SQYMainController.mInstance.UpdateDailyGiftTip ();
    }

    void ResortList(){
        ComLoading.Open ();
     
        for (int i = 0; i < dialyItemList.Count; i++) {
            Destroy (dialyItemList [i].gameObject);
        }

         dialyItemList.Clear ();

        SortList (curDailyGiftList,null);
        this.Refresh ();
     
    }


    void SortList(List<DailyGiftItemClass> dailyGiftList , List<DailyGiftItemClass> sevenList = null){
        List<DailyGiftItemClass>  forwardList  = new List<DailyGiftItemClass>();    //可领
        List<DailyGiftItemClass>  backwardList  = new List<DailyGiftItemClass>();   //不可领
        if (dailyGiftList != null) {
            if (sevenList != null) {
                if (ActivityNetController.curSevenGetIndex >= 0 && ActivityNetController.curSevenGetIndex < 7) {
                    //第一个 排在第一位
                    int tCount = sevenList.Count;
                    for (int i = 0; i < tCount; i++) {
                        if (i < sevenList.Count) {
                            if (sevenList [i].id <= ActivityNetController.curSevenGetIndex) {
                                sevenList.Remove (sevenList [i]);
                            } else {
                                if (sevenList [i].id == ActivityNetController.curSevenGetIndex + 1) {
                                    dailyGiftList.Add (sevenList [i]);
                                    sevenList.Remove (sevenList [i]);
                                }
                            }
                        }
                    }
                }
            } else {
                int tC = dailyGiftList.Count;
                sevenList = new List<DailyGiftItemClass> ();
                List<DailyGiftItemClass> temList =  new List<DailyGiftItemClass> ();
                for (int i = 0; i < tC; i++) {
                    if(i< dailyGiftList.Count){
                        if (dailyGiftList [i].curItemType == DailyGiftItemClass.dailyItemType.sevenGiftType) {
                            sevenList.Add (dailyGiftList [i]);
                            //  dailyGiftList.Remove (dailyGiftList[i]);

                        } else {
                            temList.Add (dailyGiftList[i]);
                        }
                    }
                }
                dailyGiftList.Clear ();
                dailyGiftList = temList;
            }
           

            for(int j=1;j< 6;j++){
                for (int i = 0; i < dailyGiftList.Count; i++) {
                    if((int)dailyGiftList [i].curItemType == j) {
                        if (dailyGiftList [i].canGet) {
                            forwardList.Add (dailyGiftList [i]);
                        } else {
                            backwardList.Add (dailyGiftList [i]);
                        }
                    }
                }
            }

            if (sevenList != null) {
                backwardList.AddRange (sevenList);
            }
        }
        curDailyGiftList = new List<DailyGiftItemClass> ();
        curDailyGiftList.AddRange (forwardList);
        curDailyGiftList.AddRange (backwardList);
        Invoke ("DelayRefresh",0.3f);
       
    }

    void DelayRefresh(){
        this.SpawnItem (curDailyGiftList.Count);    
        Refresh ();
    }

    public void OnClickBack(){
        Destroy (gameObject);
		SQYMainController.mInstance.UpdateDailyGiftTip ();
    }

}



public class DailyGiftItemClass{
    public int id;

    public enum dailyItemType
    {
        sevenGiftType =1,
        monthGiftType =2,
        dinnerType =3,
        levelGiftType =4,
        vipGiftType = 5,
    }
    public dailyItemType curItemType;

    public bool canGet;

    public List<ItemOfReward> giftReward;
	public int otherPara;

}
