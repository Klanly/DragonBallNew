using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum GetGiftType{
    isLvReward =4,
    isFirstChargeReward = 1,
    isRewardsGroup =3,
    isSevenDayReward = 2,
	isActivationCode = 5,
	isWeiXinPanel = 6,
}

public class GetGiftPanelController : RUIMonoBehaviour {


    private static GetGiftPanelController instance;

    public static GetGiftPanelController Instance {
        get {
            return instance;
        }
    }

    public static GetGiftPanelController CreateUIRewardPanel( GameObject tObj)
    {
        UnityEngine.Object obj = WXLLoadPrefab.GetPrefab(WXLPrefabsName.UIGetRewardPanel);
        if (obj != null) {
            GameObject go = Instantiate(obj) as GameObject;
            GetGiftPanelController fc = go.GetComponent<GetGiftPanelController>();
            Transform goTrans = go.transform;
            go.transform.parent = tObj.transform;
            go.transform.localPosition = Vector3.zero;
            goTrans.localScale = Vector3.one;
            return fc;
        }
        return null;        
    }


    public GetGiftType curRewardType;
    public UIToggle togLvReward;
    public UIToggle togFirstChargeReward;
    public UIToggle togRewardsGroup;
    public UIToggle togSevenReward;
	public UIToggle togActivationCode;
	public UIToggle togWeiXin;
    public UISprite mainBg;

	public UIInput CodeInputvalue;

    public UIGrid lvRewardGrid;
    public UIGrid rewardsGroupGird;
    public GameObject firstChargeObj;
    public UIPanel sevenObjPanel;
    public UIPanel levelObjPanel;
    public UIPanel giftsObjPanel;
	public UIPanel activationCodePanel;
	public UIPanel weiXinPanel;
    public UIScrollBar sevenscBar;
    public UIScrollBar lvScBar;

	public List<UISprite> spTipList;

	#region 首充
	public List<RewardCell> reward;
	#endregion

    #region 等级礼包
    public List<LevelRewardCollection> RewardColList = new List<LevelRewardCollection>();
    public const int colNum = 24;
    private int tempTargetNum = 27;
    protected int maxLvLimit;
	int offsetX = -412;
    #endregion

    #region 7天奖励
    public UITable sevenTableRoot;
    public List<UISevenDayRewardCell> mCellList = new List<UISevenDayRewardCell>();
    const int Max_day = 7;
	private Vector3 itemOrgPos = new Vector3(0,50,0);
    #endregion

    #region 等级串烧
    public List<DailyGiftItem> dialyItemList = new List<DailyGiftItem>();
    private List<DailyGiftItemClass> curDailyGiftList = new List<DailyGiftItemClass>();
    public List<int> durTime  = new List<int>(); 
    #endregion


    void Awake(){
        instance = this;
     
    }

    void Start(){
		InitBtns ();
		DBUIController.mDBUIInstance.HiddenFor3D_UI();
    }

    void InitBtns(){
		curRewardType = GetGiftType.isRewardsGroup;
		firstChargeObj.gameObject.SetActive (false);
		togFirstChargeReward.gameObject.SetActive (false);

		//微信
		togWeiXin.gameObject.SetActive (LuaTest.Instance.OpenWeiXin);
		//等级奖励
		togLvReward.gameObject.SetActive (LuaTest.Instance.OpenLevelAward);
		//激活码
		togActivationCode.gameObject.SetActive (LuaTest.Instance.ActivationCode);
		//七天
		if (LuaTest.Instance.OpenSevenAward == true) {
			if (Core.Data.ActivityManager.GetDailyGiftState (ActivityManager.sevenDayType) != "3") {
				togSevenReward.gameObject.SetActive (true);
				curRewardType = GetGiftType.isSevenDayReward;
			} else {
				togSevenReward.gameObject.SetActive (false);
			}
		} else {
			togSevenReward.gameObject.SetActive (false);
		}

		activationCodePanel.gameObject.SetActive(false);

        //首充判断    && //首充之后是否领奖
//		if (Core.Data.rechargeDataMgr._RechageStatus == 1) {
//			//是否领奖
//			togFirstChargeReward.gameObject.SetActive (true);
//			firstGiftCanGet = false;
//			curRewardType = GetGiftType.isFirstChargeReward;
//		} else if (Core.Data.rechargeDataMgr._RechageStatus == 2) {
//			string tempValue = Core.Data.ActivityManager.GetDailyGiftState (ActivityManager.firstChargeType);
//				if (tempValue == "1") {
//					togFirstChargeReward.gameObject.SetActive (true);
//					firstGiftCanGet = true;
//					curRewardType = GetGiftType.isFirstChargeReward;
//				}else if(tempValue != null && tempValue != "1"){
//					firstGiftCanGet = false;
//					togFirstChargeReward.gameObject.SetActive (false);
//					firstChargeObj.gameObject.SetActive (false);
//			}
//		}
//
//		if (ActivityManager.curGetGiftType != 0) {
//			curRewardType = (GetGiftType)ActivityManager.curGetGiftType;
//			if (curRewardType == GetGiftType.isRewardsGroup) {
//				firstChargeObj.gameObject.SetActive(false);
//			} else if(curRewardType == GetGiftType.isFirstChargeReward) {
//				firstChargeObj.gameObject.SetActive(true);
//			}
//		} 

		switch (curRewardType) {
//		case GetGiftType.isFirstChargeReward:
//			togFirstChargeReward.startsActive = true;
//			break;
		case GetGiftType.isLvReward:
			togLvReward.startsActive = true;
			break;
		case GetGiftType.isRewardsGroup:
			togRewardsGroup.startsActive = true;
			break;
		case GetGiftType.isSevenDayReward:
			togSevenReward.startsActive = true;
			break;
		default:
			break;
		}
        
        this.Refresh();
    }

    void Refresh(){
        lvScBar.gameObject.SetActive(false);
        sevenscBar.gameObject.SetActive(false);
        switch(curRewardType){
//            case GetGiftType.isFirstChargeReward:
//                UIToggle.current = togFirstChargeReward;
//                this.InitFirstChargeData();
//                break;
            case  GetGiftType.isLvReward:
                UIToggle.current = togLvReward;
                this.LevelRewardMethod();
                break;
            case GetGiftType.isRewardsGroup:
                UIToggle.current = togRewardsGroup;
                this.InitDailyRewardGroup();
                break;
            case GetGiftType.isSevenDayReward:
                UIToggle.current = togSevenReward;
                this.InitSevenRewardPanel();
                break;
			case GetGiftType.isActivationCode:
				UIToggle.current = togActivationCode;
				this.InitActivationCodePanel();
				break;
			case GetGiftType.isWeiXinPanel:
				UIToggle.current = togWeiXin;
				this.WeiXinShowPanel ();
				break;
            default:
                UIToggle.current = null;
                break;
        }
		this.ShowTipCtrl ();
        UIToggle.current.value = true;
		//记录状态
      
		ActivityManager.curGetGiftType = (int)curRewardType;
    }

	public void ShowTipCtrl(){
		//首充
//		if (Core.Data.ActivityManager.GetDailyGiftState (ActivityManager.firstChargeType) != "1") {
//			spTipList [0].gameObject.SetActive (false);
//		} else {
//			spTipList [0].gameObject.SetActive (true);
//		}
		//7天
		if (Core.Data.ActivityManager.GetDailyGiftState(ActivityManager.sevenDayType) != "1") {
			spTipList[1].gameObject.SetActive(false);
		}else{
			spTipList[1].gameObject.SetActive(true);
		}
		//等级
		if (Core.Data.ActivityManager.GetDailyGiftState(ActivityManager.lvRewardType) != "1" ) {
			spTipList[3].gameObject.SetActive(false);
		}else{
			spTipList[3].gameObject.SetActive(true);
		}

		//礼包串烧
		if (Core.Data.ActivityManager.GetDailyGiftState(ActivityManager.dinnerType) == "1" || Core.Data.ActivityManager.GetDailyGiftState(ActivityManager.monthGiftType) == "1"/*|| Core.Data.ActivityManager.GetDailyGiftState(ActivityManager.vipLibaoType) == "1"*/ ) {
			spTipList[2].gameObject.SetActive(true);
		}else{
			spTipList[2].gameObject.SetActive(false);
		}

        spTipList[4].gameObject.SetActive(false);

	}

    void HideCurPanel(){
        switch(curRewardType){
//            case GetGiftType.isFirstChargeReward:
//                firstChargeObj.SetActive(false);
//                break;
            case GetGiftType.isLvReward:
                levelObjPanel.gameObject.SetActive(false);
                break;
            case GetGiftType.isRewardsGroup:
                giftsObjPanel.gameObject.SetActive(false);
                break;
            case GetGiftType.isSevenDayReward:
                sevenObjPanel.gameObject.SetActive(false);
                break;
			case GetGiftType.isActivationCode:
				activationCodePanel.gameObject.SetActive(false);
				break;
			case GetGiftType.isWeiXinPanel:
				weiXinPanel.gameObject.SetActive (false);
				break;
            default:
                break;
        }
    }


	/*  #region 首充礼包

    void InitFirstChargeData(){
        if (curRewardType == GetGiftType.isFirstChargeReward){
            firstChargeObj.SetActive(true);
			InitFirstReward ();
        }
    }

	void InitFirstReward(){
		List<ItemOfReward> tR = Core.Data.ActivityManager.BackIorList ();
		if (tR != null) {
			for (int i = 0; i < reward.Count; i++) {
				if (i < tR.Count) {
					reward [i].gameObject.SetActive (true);
					reward [i].InitUI (tR[i]);
				} else {
					reward [i].gameObject.SetActive (false);
				}
			}
		}
	}

    void OnGetChargeGift(){
		if (firstGiftCanGet == true) {
			ActivityNetController.GetInstance ().GetFirstChargeGiftRequest ();
		} else {
			UIDragonMallMgr.GetInstance ().SetRechargeMainPanelActive ();
			Core.Data.rechargeDataMgr._RechargeCallback = CallBack ;
			this.BtnBack ();
		}
    }

	public void BackGetFBGift(){
		firstGiftCanGet = false;
		ActivityNetController.ShowDebug (Core.Data.stringManager.getString(32106));
		ShowTipCtrl ();
	}

	void CallBack(){
		 CreateUIRewardPanel(DBUIController.mDBUIInstance._bottomRoot);
	}

    #endregion*/


    #region   等级礼包 重写
    void LevelRewardMethod(){
        if (curRewardType == GetGiftType.isLvReward)
        {
			if (Core.Data.ActivityManager.isLevelRequest == false) {
				ActivityNetController.GetInstance ().LevelGiftStateRequest (0);    
				Core.Data.ActivityManager.isLevelRequest = true;
			} else {
				InitLevelRewardData ();
			}
            lvRewardGrid.GetComponent<UIGrid>().repositionNow = true;
            mainBg.gameObject.SetActive(false);
            lvScBar.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void  InitLevelRewardData() 
    {
		List<LevelUpRewardData>  tlevelRewardData = Core.Data.ActivityManager.GetLeveRewardDataList();
        maxLvLimit = (int)Core.Data.playerManager.Lv / 5; 
        if (RewardColList != null || RewardColList .Count != 0)
            levelObjPanel.gameObject.SetActive(true);
        if (RewardColList.Count == 0)
        {
            for (int i = 0; i < colNum; i++)
            {
                SpawnItem();
            }

			for (int i = 0; i < colNum; i++) {
				LvCollection tLvCollectionD = new LvCollection (tlevelRewardData[i].level,tlevelRewardData[i].tLvReward,tlevelRewardData[i].reward,this.RefreshItemState(i, maxLvLimit));
				RewardColList [i].SetItemValue (tLvCollectionD);
			}

        }
        else
        { 
			for (int i = 0; i < colNum; i++) {
				LvCollection tLvCollectionD = new LvCollection (tlevelRewardData[i].level,tlevelRewardData[i].tLvReward,tlevelRewardData[i].reward,this.RefreshItemState(i, maxLvLimit));
				RewardColList [i].SetItemValue (tLvCollectionD);
			}
        }
        Invoke ("AutoMoveToCanGet",0.2f);
    }

	public LevelRewardCollection.CollectionState  RefreshItemState(int num ,int tLimit){  
		List<int> levelRewardList = Core.Data.ActivityManager.GetGotLvReward ();
		LevelRewardCollection.CollectionState curItemState = LevelRewardCollection.CollectionState.LockReward;

			if (levelRewardList != null && levelRewardList.Count != 0 )//领过奖励 
            { 
                if (num < tLimit)  //解锁 可以领
                {
				if (levelRewardList.Contains ((num+1)*5)) {
		
						curItemState =LevelRewardCollection.CollectionState.GotReward;

					}
                    else {
						curItemState = LevelRewardCollection.CollectionState.UnlockReward;
                    }
                }
                else
                {
					curItemState = LevelRewardCollection.CollectionState.LockReward;
                }
            }
            else
            {// 从未领过  可以开启 
                if (num < tLimit) {
					curItemState = LevelRewardCollection.CollectionState.UnlockReward;
                }
                else
					curItemState = LevelRewardCollection.CollectionState.LockReward;
            }
		return curItemState;

    }

    public void AutoMoveToCanGet(){
        tempTargetNum = this.CheckNew ();
		if (tempTargetNum >= 0 || tempTargetNum < 20) {
			if (tempTargetNum > 20)
				tempTargetNum = 20;
            Vector3 targetPos = new Vector3 (-250 * tempTargetNum, 0, 0);
            SpringPanel.Begin (lvRewardGrid.transform.parent.gameObject, targetPos + Vector3.right * offsetX, 8);
            lvRewardGrid.transform.parent.GetComponent<SpringPanel> ().enabled = true;
        }
		this.ShowTipCtrl ();
    }

    public void SpawnItem()
    {
        if (RewardColList != null && RewardColList.Count == colNum) {
            return;
        }
        UnityEngine.Object ItemObj = WXLLoadPrefab.GetPrefab(WXLPrefabsName.UIColItem);
        for (int i = 0; i < colNum; i++) {
            GameObject tObj = Instantiate(ItemObj)as GameObject;
            tObj.transform.parent = lvRewardGrid.transform;
            tObj.transform.localScale = Vector3.one;
			int tX = i * 250 ;
			tObj.transform.localPosition = new Vector3(tX, 0, 0);
            RewardColList.Add(tObj.GetComponent<LevelRewardCollection>());
        }
    }

    int CheckNew(){
        tempTargetNum = 27;
		List<int> gotList = Core.Data.ActivityManager.GetGotLvReward ();//ActivityNetController.levelRewardList;
        int temp = Core.Data.playerManager.Lv / 5;
        if (gotList != null) {
            for (int i = 0; i < temp; i++) {
                if (!gotList.Contains ((i + 1) * 5)) {
                    if (i < tempTargetNum) {
                        tempTargetNum = i;
                    }
                }
            }
        } else {
            tempTargetNum = 0;
        }
        if (tempTargetNum == 27) {
            int tNum = Core.Data.playerManager.Lv % 5;
            if (tNum != 0) {
                if (Core.Data.playerManager.Lv - tNum <= 0)
                    return 0;
                else {
                    return (Core.Data.playerManager.Lv - tNum) / 5;
                }
            } else {
                return temp;
            }
        }
        else return tempTargetNum;
    }

    #endregion


	#region 激活码部分
	void InitActivationCodePanel(){
		if (curRewardType == GetGiftType.isActivationCode)
		{
			activationCodePanel.gameObject.SetActive(true);
			mainBg.gameObject.SetActive(false);
		}
	}

	void SendCode()
	{	
		string mContent = "";
		if(CodeInputvalue != null)
		{
			mContent = CodeInputvalue.value;
			if(string.IsNullOrEmpty(mContent.Trim()))
			{
				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(75002));
				return;
			}
			ActivityNetController.GetInstance().SendActivationCodeRequest(mContent);
			CodeInputvalue.value = "";
		}
	}

	#endregion

    #region 七日礼包 重写
    void InitSevenRewardPanel(){
        if(curRewardType == GetGiftType.isSevenDayReward)
        {
			if (Core.Data.ActivityManager.GetSevenData () == null)
				ActivityNetController.GetInstance ().GetSevenDayRewardData ();
			else {
				SetSevenRewardDetail (Core.Data.ActivityManager.GetSevenData());
			}
            
			sevenObjPanel.gameObject.SetActive(true);
            sevenscBar.gameObject.SetActive(true);
            mainBg.gameObject.SetActive(false);
		    
        }
    }


    void InitChild()
    {
        Object prefab = PrefabLoader.loadFromPack ("LS/pbLSSevenDaysCell");
        if (prefab != null)
        {
            for(int i=0; i<Max_day; i++)
            {
                GameObject obj = Instantiate (prefab) as GameObject;
                obj.name = (i+1).ToString();
                RED.AddChild (obj, sevenTableRoot.gameObject);
                UISevenDayRewardCell mm = obj.GetComponent<UISevenDayRewardCell> ();
                mCellList.Add(mm);
            }
        }
        sevenTableRoot.repositionNow = true;
        sevenTableRoot.Reposition();

    }
		
    public void SetSevenRewardDetail(SevenDaysListData data)
    {

        if (mCellList.Count == 0)
        {
            InitChild();
        }
      
        for(int i=0; i<data.awads.Length; i++)
        {
            if(i<data.index)mCellList[i].Show(SevenDayCellType.SevenDayCellType_HAVETAKE,data.awads,i+1);
            else if(i==data.index)
            {
                if(data.canGain)mCellList[i].Show(SevenDayCellType.SevenDayCellType_WAITTAKE,data.awads,i+1);
                else 
                {
                    mCellList[i].Show(SevenDayCellType.SevenDayCellType_NOTOPEN,data.awads,i+1);
                }
            }
            else if(i>data.index)
            {
                mCellList[i].Show(SevenDayCellType.SevenDayCellType_NOTOPEN,data.awads,i+1);
            }
        }
		this.CheckNewPos ();
    }

    public void DeleteCell()
    {
        foreach(UISevenDayRewardCell cell in mCellList)
        {
            cell.dealloc();
        }
        Destroy(gameObject);
    }

    public void CheckNewPos(){
		sevenTableRoot.repositionNow = true;
		sevenTableRoot.Reposition();

        SevenDaysListData sData = Core.Data.ActivityManager.GetSevenData ();

        if (sData != null)
        {

			if (sData.index >=0 || sData.index <8 ) {
				int tNum = sData.index;
				if (sData.index > 4)
					tNum = 4;
				Vector3 itemTargetPos = itemOrgPos + (Vector3.left * 320*tNum) ;
				SpringPanel.Begin (sevenObjPanel.gameObject,itemTargetPos,10);
				sevenObjPanel.GetComponent<SpringPanel> ().enabled = true;

            }
        }
		this.ShowTipCtrl ();
    }

	//7日礼包  领完之后的刷新 
	public void SevenDayRewardRefresh(){
		Core.Data.ActivityManager.SetDailyGiftState (ActivityManager.sevenDayType, "3"); 
		sevenObjPanel.gameObject.SetActive (false);
		InitBtns ();
	}

    #endregion

//	void OnGUI(){
//		if (GUI.Button (new Rect (100, 100, 100, 100), "text ")) {
//
//			Core.Data.ActivityManager.SetDailyGiftState (ActivityManager.sevenDayType, "3"); 
//			sevenObjPanel.gameObject.SetActive (false);
//			InitBtns ();
//		}
//	}


    #region 每日礼包 重写
    void InitDailyRewardGroup(){
        if (curRewardType == GetGiftType.isRewardsGroup)
        {
            giftsObjPanel.gameObject.SetActive(true);
            ActivityNetController.GetInstance ().GetAllRewardData ();
            mainBg.gameObject.SetActive(true);

        }
    }

    public void InitGiftGroupData(){
        SortList(ActivityNetController.GetInstance().dailyGiftList,null);

    }

    public void SpawnDailyItem(int colNum)
    { 

        UnityEngine.Object ItemObj = WXLLoadPrefab.GetPrefab(WXLPrefabsName.UIDailyGiftItem);
        for (int i = 0; i < colNum; i++) {
            GameObject tObj = Instantiate(ItemObj)as GameObject;
            tObj.transform.parent = rewardsGroupGird.gameObject.transform;
            tObj.transform.localScale = Vector3.one;
            tObj.transform.localPosition = Vector3.zero;
            dialyItemList.Add(tObj.GetComponent<DailyGiftItem>());
        }
    }

    void ResortList(){
        ComLoading.Open ();

        for (int i = 0; i < dialyItemList.Count; i++) {
            Destroy (dialyItemList [i].gameObject);
        }
        dialyItemList.Clear ();
        SortList (curDailyGiftList,null);
        this.DelayRefresh ();

    }


    void SortList(List<DailyGiftItemClass> dailyGiftList , List<DailyGiftItemClass> sevenList = null){
		ComLoading.Close();
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
        Invoke ("DelayRefresh",0.2f);
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
			rewardsGroupGird.repositionNow = true;
			break;

		case DailyGiftItemClass.dailyItemType.monthGiftType:
			for (int i = 0; i < dialyItemList.Count; i++) {
				if (dialyItemList [i].curType == rType) {
					DailyGiftItemClass tClass = dialyItemList [i].ReturnValue () as DailyGiftItemClass;
					MonthGiftData td = Core.Data.ActivityManager.GetMonthStateData ();
					if (tClass.otherPara > 0) {
						if (td != null) {
							tClass.otherPara = td.lastDay;
							tClass.canGet =  td.canGain == 1 ? true:false;
						}
					}
				}
			}
			this.ResortList ();
			break;
		}
		this.ShowTipCtrl ();

	}

    void DelayRefresh(){
       if (dialyItemList.Count==0 && curDailyGiftList.Count != 0)
           {
             this.SpawnDailyItem(curDailyGiftList.Count);
           }
            
        this.DailyGiftRefresh ();
    }

    public void DailyGiftRefresh(){
        for (int i = 0; i < dialyItemList.Count; i++) {
            if(i< curDailyGiftList.Count)
                dialyItemList [i].SetItemValue (curDailyGiftList[i]);
        }
        rewardsGroupGird.repositionNow = true;
    }
		

    #endregion


	#region 微信界面
	void WeiXinShowPanel(){
		if (curRewardType == GetGiftType.isWeiXinPanel) {
			weiXinPanel.gameObject.SetActive (true);
			mainBg.gameObject.SetActive(false);
		}
	}

	#endregion

    #region 按钮事件
    void OnBtnLvReward(){
        if (curRewardType == GetGiftType.isLvReward)
            return;
        this.HideCurPanel();
        curRewardType = GetGiftType.isLvReward;
        Refresh();
    }

    void OnBtnFirstCharge(){
        if (curRewardType == GetGiftType.isFirstChargeReward)
            return;
        this.HideCurPanel();
        curRewardType = GetGiftType.isFirstChargeReward;
        Refresh();
    }

    void OnBtnGiftGroup(){
        if (curRewardType == GetGiftType.isRewardsGroup)
            return;
        this.HideCurPanel();
        curRewardType = GetGiftType.isRewardsGroup;
        Refresh();
    }

    void OnBtnSevenDay(){
        if (curRewardType == GetGiftType.isSevenDayReward)
            return;
        this.HideCurPanel();
        curRewardType = GetGiftType.isSevenDayReward;
        Refresh();
    }


	void OnBtnActivationCode(){
		if (curRewardType == GetGiftType.isActivationCode)
			return;
		this.HideCurPanel();
		curRewardType = GetGiftType.isActivationCode;
		Refresh();
	}

	public void OnBtnWeiXin(){
		if (curRewardType == GetGiftType.isWeiXinPanel)
			return;
		this.HideCurPanel ();
		curRewardType = GetGiftType.isWeiXinPanel;
		Refresh ();
	}
	public void BtnBack(){
		SQYMainController.mInstance.UpdateDailyGiftTip ();
        Destroy(gameObject);
		ActivityManager.curGetGiftType = 0;
        DBUIController.mDBUIInstance.ShowFor2D_UI();
		instance = null;
    }
		

    #endregion



}
