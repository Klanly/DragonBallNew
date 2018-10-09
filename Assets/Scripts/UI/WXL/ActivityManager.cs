using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 整个活动 activity manager
/// </summary>
public class ActivityManager : Manager,ICore
{
    #region ICore implementation

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public void OnLogin(object obj)
    {
        throw new NotImplementedException();
    }

    #endregion

    //存放活动信息
	private	List<LevelUpRewardData> listLevelUpReward = null;
	private List<int> gotLevelReward = null;
    public List<DinnerInfo> listDinnerInfoList = null;
    public List<TreasureBoxDespData> listTreasureBoxDesp = null;
    private List<WorldBossRewardData> listBossRewardData = null;
    //
    private List<HonorItemData> honorGiftDataList = null;
	public Dictionary<int,string> rewardDic = null;
    private SevenDaysListData sevenData = null;
    public List<int> dinnerDurTime = new List<int>(4){12,14,18,20};
    public Dictionary<int,string> actDic = null;

	public List<ItemOfReward> firstIOR = null;

	private MonthGiftData monthState = null;
	public bool isMonthRequest = false;
	public bool isLevelRequest = false;


    //测试 talkingdata   app id
	private const string TDataAppId = "A1AEAF53D07D174564E5D9DDED85D88C";

    public const string FestivalPurchaseType = "festival";
    public const string MonsterComeBackType = "monster";
    public const string HappyRollType = "yaoyaole";
    public const string ScratchType = "guaguale";
    public const string BigWheelType = "dazhuanlun";
    public const string SignDayType = "buqian";
    public const string DragonSlotType = "aoyi";
    public const string BuildOpenType = "buildOpen";
    public const string GPSTYPE = "GPS";
    public const string DAYTASK = "DAYTASK";
    public const string XINGYUNWHEEL = "xingyunwheel";
    public const string GUAGUALE = "GUAGUALE";
    public const string YAOYAOLE = "YAOYAOLE";
    public const string MAIL = "mail";
    public const string TreasureBoxType = "baoxiang";
    public const string ChapterType = "maoxian";
    public const string TraningType = "xunlian";
    public const string ForgingType = "baoshi";
    public const string ChangeInfoType = "userInfo";

    TDGAAccount account;

    //活动 包括 魔王 节日 雷达 签到   百宝箱 vip 特权 
    public const int festivalType = 0;
    public const int monsterType = 1;
    public const int radarType = 2;
    public const int guessType = 3;
    public const int treasureType = 5;
	public const int happyScratchType = 4;
	public const int superGiftType = 6;
    // public const int gonggaoType = 5;
    // public const int secretShopType = 7;

    //领奖 里面包括  大餐 等级 七天奖励 vip礼包
    public const int sevenDayType = 0;
    public const int monthGiftType = 1;
    public const int dinnerType = 2;
    public const int lvRewardType = 3;
    public const int vipLibaoType = 4;
//	public const int firstChargeType = 5;
    //活动转盘ID
	public static bool hasUpdateValue = false;

	private static int _activityZPID = 0;//活动转盘ID 
	public static int activityZPID {
		get {
			return _activityZPID;
		} 
		set {
			_activityZPID = value;
			hasUpdateValue = true;
		}
	}

    /// <summary>
    /// 货币类型
    /// </summary>
    public static int moneyType;
    /// <summary>
    /// 加成数量
    /// </summary>
    public static int addPecent = 0;
    public static int buyLeftTimes = 5;
    /// <summary>
    /// 状态    1是活动开启还没吃      没开启  2 等待上午   3 等待下午  4晚饭过后
    /// </summary>
    public static int isOpen = 0;
    //true  可以领   false 领过
    public static bool canGet = false;

	public static int curGetGiftType = 0;
    private List<int[]> rollList =null;
	private string signStatus = null;

    private  HolidayActivityResponse _har = null   ; // yangchenguang  限时活动 11月28日


    public  int  HolidayActivity = 0;// 0 是没有请求过 1 请求过

    public ActivityManager()
    {	

        //等级奖励
        listLevelUpReward = new List<LevelUpRewardData>();
        listDinnerInfoList = new List<DinnerInfo>();
        listTreasureBoxDesp = new List<TreasureBoxDespData>();
        listBossRewardData = new List<WorldBossRewardData>();

        honorGiftDataList = new List<HonorItemData>();
        rewardDic = new Dictionary<int, string>();
        actDic = new Dictionary<int, string> ();
		gotLevelReward = new List<int> ();
        rollList = new List<int[]>();
    }

    public override bool loadFromConfig()
    {
        bool readBossReward_B = base.readFromLocalConfigFile<WorldBossRewardData>(ConfigType.WorldBossReward, listBossRewardData);
        bool success_H = base.readFromLocalConfigFile<HonorItemData>(ConfigType.HonorItem, honorGiftDataList);
        bool success_R = base.readFromLocalConfigFile<LevelUpRewardData>(ConfigType.LvUpReward, listLevelUpReward);
        bool success_T = base.readFromLocalConfigFile<TreasureBoxDespData>(ConfigType.TreasureDesp, listTreasureBoxDesp);

        if (success_R == false || success_T == false || readBossReward_B == false || success_H == false)
            return false;
        else
            return true;
    }

    /// <summary>
    ///  1是 开始    2 ：结束 
    /// </summary>
    /// <param name="response">Response.</param>
    public override void fullfillByNetwork(BaseResponse response)
    {
        if (response != null && response.status != BaseResponse.ERROR)
        {
            LoginResponse loginResp = response as LoginResponse;
            if (loginResp != null && loginResp.data != null && loginResp.data.actInfo != null)
            {

                if (loginResp.data.actInfo.food != null)
                {
                    listDinnerInfoList.Clear();

                    int length = loginResp.data.actInfo.food.Length;
                    for (int i = 0; i < length; i++)
                    {
                        listDinnerInfoList.Add(loginResp.data.actInfo.food[i]);
                    }
                }
                actDic.Clear ();
                rewardDic.Clear ();
				sevenData = null;
				isLevelRequest = false;
                //如果是0 表示今天没领   1：表示 今天领了    为了 表示 活动被激活 1- loginresp    1= 被激活 
//                if (loginResp.data.actInfo.masgn == 0)
//                {
//                    canGet = true;
//                }
//                else
//                {
//                    canGet = false;
//                }

				//    	SetActState(signType, (1 - loginResp.data.actInfo.masgn).ToString());
				SetActState(guessType, "0");
				signStatus = (1 - loginResp.data.actInfo.masgn).ToString ();

				SetSignState (signStatus);

                SetActState(vipLibaoType, loginResp.data.actInfo.Vip.ToString());
                SetActState(festivalType, loginResp.data.actInfo.festival);
                SetActState(monsterType, loginResp.data.actInfo.monster);
                SetActState(treasureType, "2");
                SetActState(radarType,"2");
				SetActState (happyScratchType,"2");
				SetActState (superGiftType,"2");

				gotLevelReward.Clear ();
                string str = GetDinnerState().ToString();
                SetDailyGiftState(dinnerType, str);
                SetDailyGiftState(lvRewardType, loginResp.data.actInfo.lvReward.ToString());
                // 1可以领，2是不可以领  开 入口  3.不开入口 
                SetDailyGiftState(sevenDayType, loginResp.data.actInfo.sevenAward.ToString());

				ActivityNetController.GetInstance().GetMonthStateRequest();
				isMonthRequest = true;
                Core.TimerEng.deleteTask(TaskID.Active);
                ActivityNetController.GetInstance().SetDinnerTimerState();
				hasUpdateValue = false;
                _har= null ;
                HolidayActivity = 0 ;

            }
        }
    }

	public string GetSignState(){
		return signStatus;
	}
	public void SetSignState(string tState){
		signStatus = tState;
	}



    //   从0 开始的
    public String GetActivityStateData(int num)
    {
        string tStr = null;
        if (actDic.ContainsKey(num))
        {
           
            if (actDic.TryGetValue(num, out tStr))
            {
                return tStr;
            }
        }
        return tStr;
    }
    //1:活动关闭  2：活动开启
    public void SetActState(int num, string state)
    {
        if (actDic.ContainsKey(num))
        {
            actDic[num] = state;
        }
        else
        {
            actDic.Add(num, state);
        }
    }

    //获取奖励 状态
    public String GetDailyGiftState(int num)
    {
        string tStr = null;
        if (rewardDic.ContainsKey(num))
        {

            if (rewardDic.TryGetValue(num, out tStr))
            {
                return tStr;
            }
        }
        return tStr;
    }
    //奖励 状态
    public void SetDailyGiftState(int num, string state)
    {
        if (rewardDic.ContainsKey(num))
        {
            rewardDic[num] = state;
        }
        else
        {
            rewardDic.Add(num, state);
        }
    }
    public HolidayActivityResponse  har
    {
        set
        {
            _har = value ; 
        }
        get
        {
            return _har ; 
        }
    }

    int GetDinnerState()
    {
        long nowTime = Core.TimerEng.curTime;
        //判断 给定的是否是 增量时间
        for (int i = 0; i < listDinnerInfoList.Count; i++)
        {
            if (nowTime > listDinnerInfoList[i].start && nowTime < listDinnerInfoList[i].end && listDinnerInfoList[i].eaten == 1)
            {
                isOpen = 1;
                return isOpen;
            }
        }
        isOpen = CommonShiftUI();
        return isOpen;
    }
        
    /// <summary>
    ///  初始化 状态 用   1  没吃   2 等上午  3 等下午 
    /// </summary>
    public int CommonShiftUI()
    {
        int mType = 0;
        for (int i = 0; i < listDinnerInfoList.Count; i++)
        {
            if (Core.TimerEng.curTime < listDinnerInfoList[i].end)
            {
                mType = i + 1;
                break;
            }
        }
        if (mType == 1)
        {
            isOpen = 2;
        }
        else
        {
            isOpen = 3;
        }
        return isOpen;
    }

    public int[] GetDinnerTimeShow(){
        long curTime = Core.TimerEng.curTime;
        int[] tMorning = new int[2]{12,14};
        int[] tAfternoon = new int[2]{18,20};
        int[] tNow = tMorning;
        if (listDinnerInfoList != null && listDinnerInfoList.Count >0)
        {
            if (curTime < listDinnerInfoList[0].start)
            {
                tNow = tMorning;
            }
            if (curTime > listDinnerInfoList[0].start && curTime < listDinnerInfoList[1].start)
            {
                tNow = tAfternoon;
            }
            if (curTime > listDinnerInfoList[1].end)
            {
                tNow = tMorning;
            }
        }
        return tNow;
    }


    public static void AddPecent()
    {
        if (buyLeftTimes > 0)
        { 
            if (moneyType == 1)
                addPecent += 20;
            else
                addPecent += 5;

            buyLeftTimes--;
            UIActMonsterComeController.Instance.AttackBuffAndTimes();
            UIActMonsterComeController.Instance.CancelShowTip();
        }
        else
        {
            ActivityNetController.ShowDebug("u have no chance");
        }
    }
		

	#region   等级奖励

    public LevelUpRewardData GetRewardData(int num)
    {
        foreach (LevelUpRewardData tData in listLevelUpReward)
        {
            if (tData.level == num)
            {
                return tData;
            }
        }
        return null;
    }

	public List<LevelUpRewardData> GetLeveRewardDataList()
    {
        return listLevelUpReward;
    }

	//获取的等级奖励
	public void SaveGotLvReward(int lv){
		if (gotLevelReward == null)
			gotLevelReward = new List<int> ();
		gotLevelReward.Add (lv);
	}
	public List<int> GetGotLvReward(){
		return gotLevelReward;
	}

	#endregion


    public TreasureBoxDespData GetRewardDataList(int num)
    {
        TreasureBoxDespData tBoxData = null;
        if (listTreasureBoxDesp == null)
            return tBoxData;

        for (int i = 0; i < listTreasureBoxDesp.Count; i++)
        {
            if (num == listTreasureBoxDesp[i].id)
            {
                tBoxData = listTreasureBoxDesp[i];
            }
        }
        return tBoxData;
    }

    public List<WorldBossRewardData> GetWorldBossRewardDataList()
    {
        return listBossRewardData;
    }
    // 荣誉兑换奖励
    public void AddHonorItem(BaseResponse response)
    {
        if (response.status != BaseResponse.ERROR)
        {
            SockBuyItemResponse buyItem = response as SockBuyItemResponse;
            if (buyItem.data.retCode == 1)
            {
//                if (ActivityNetController.tempHonorGiftId != 0)
//                {
                    ItemOfReward TR = buyItem.data.p;
					// ItemOfReward[] tReward = new ItemOfReward[1]{ TR };
					//Core.Data.itemManager.addItem(tReward);
					Core.Data.itemManager.AddRewardToBag (TR);
				//     }

            }
        }

    }

    public List<HonorItemData> GetHonorItemList()
    {
//        for (int i = 0; i < honorGiftDataList.Count; i++) {
//            HonorItemData tItem =   GetItemByData(ListGiftDataList [i].goodId);
//            honorGiftDataList.Add (tItem);
//        }
        return honorGiftDataList;
    }

    public HonorItemData getHonorItemById(int id)
    {
        for (int i = 0; i < honorGiftDataList.Count; i++)
        {
			if (honorGiftDataList[i].id == id)
            {
                return honorGiftDataList[i];        
            }
        }
        return null;
    }

    public void SaveSevenDayData(SevenDaysListData tempData)
    {
        if (tempData != null)
        {
            sevenData = null;
            sevenData = tempData;
        }
    }

    public SevenDaysListData GetSevenData()
    {
        return sevenData;
    }
	public void SaveListReward( ItemOfReward[] iorList){
		firstIOR = new List<ItemOfReward> ();
		if (iorList != null) {
			for (int i = 0; i < iorList.Length; i++) {
				firstIOR.Add (iorList [i]);
			}
		}

	}

	public List<ItemOfReward> BackIorList(){
		return firstIOR;
	}
    //存储 摇摇乐 奖励
    public void SaveRollGambleList(List<int[]> awardList){
        if (awardList != null)
        {
            rollList = awardList;
        }
    }
    public List<int[]> GetRollGamebleList(){
        return rollList;
    }

	public void SaveMonthStateData(MonthGiftData tD ){
		if(tD != null)
			monthState = tD;
	}
	public MonthGiftData GetMonthStateData(){
		return monthState;
	}

    #region talking data 数据统计

	#if UNITY_ANDROID
	private static string UNTIFY_CLASS = "com.unity3d.player.UnityPlayer";
	#endif

    //初始化 数据统计
    public void InitAccount()
    {

		Server curServer = Core.SM.curServer;
		PlayerManager player = Core.Data.playerManager;
		#if UNITY_ANDROID && !UNITY_EDITOR
	        
			if (account == null) {
				AndroidJavaClass unityClass = new AndroidJavaClass(UNTIFY_CLASS);
				AndroidJavaObject activity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
				activity.Call("runOnUiThread", new AndroidJavaRunnable(() => {
					TalkingDataGA.OnStart(TDataAppId, curServer.id.ToString());
					account = TDGAAccount.SetAccount(player.PlayerID.ToString());
					account.SetGameServer(curServer.id.ToString());
					account.SetLevel (player.Lv);
				}));
			}
        #endif

		#if UNITY_IPHONE 
		NotificationServices.RegisterForRemoteNotificationTypes(
			RemoteNotificationType.Alert |
			RemoteNotificationType.Badge |
			RemoteNotificationType.Sound
		);


		TalkingDataGA.OnStart(TDataAppId, curServer.id.ToString());
		account = TDGAAccount.SetAccount(player.PlayerID.ToString());
		account.SetGameServer(curServer.id.ToString());
		//account.SetLevel(player.Lv);

        #endif

    }
    //升级
    public void SetTDAccountLv()
    {
//        if (account == null)
//        {
//            InitAccount();
//        }
//        if (account != null)
//            account.SetLevel(Core.Data.playerManager.Lv);
    }

    /// <summary>
    /// 购买道具 消费的虚拟币  本游戏中指   钻石
    /// </summary>
    /// <param name="itemID">Item 消费的物品name </param>
    /// <param name="itemNum">Item number.</param>
    /// <param name="priceVirtualCurrency">虚拟币数量</param>
    public void OnPurchaseVirtualCurrency(string itemID, int itemNum, double priceVirtualCurrency)
    {
//        TDGAItem.OnPurchase(itemID, itemNum, priceVirtualCurrency);
    }

    /// <summary>
    /// 使用道具
    /// </summary>
    /// <param name="itemName">道具名.</param>
    /// <param name="itemNum">道具数量.</param>
    public void OnUsedItem(string itemName, int itemNum)
    {
//        TDGAItem.OnUse(itemName, itemNum);
    }

    /// <summary>
    /// 充值请求
    /// </summary>
    /// <param name="orderName">订单名称.</param>
    /// <param name="iapId">充值包id.</param>
    /// <param name="currencyNum">充值金额.</param>
    /// <param name="virtualCurrencyNum">虚拟币为多少.</param>
    public void OnChargeRequest(string orderName, string iapId, int currencyNum, int virtualCurrencyNum)
    {
//		TDGAVirtualCurrency.OnChargeRequest(orderName, iapId, currencyNum, "CNY", virtualCurrencyNum, "PP");
    }

    public void OnChargeSuccess(string orderName, string iapId, int currencyNum)
    {
//        string sResult = orderName + " , " + iapId + " , " + currencyNum;
//        TDGAVirtualCurrency.OnChargeSuccess(sResult);
    }

    /// <summary>
    /// 系统奖励
    /// </summary>
    /// <param name="rewardNum">Reward 奖励数量.</param>
    /// <param name="reason">奖励原因.</param>
    public void OnReward(int rewardNum, string reason)
    {
//        TDGAVirtualCurrency.OnReward(rewardNum, reason);
    }

    /// <summary>
    /// 进行到哪个关卡
    /// </summary>
    /// <param name="floorNum">可能是大小关节的名字.</param>
    public void OnMissionBegin(string floorName)
    {
//        TDGAMission.OnBegin(floorName);
    }

    public void OnMissionComplete(string floorName)
    {
//        TDGAMission.OnCompleted(floorName);
    }

    public void OnMissionFail(string floorName)
    {
//        TDGAMission.OnFailed(floorName, "  failed ");
    }



    public void setOnReward(ItemOfReward itemOfReward, string str)
    {
//        ItemOfReward itemOf = itemOfReward; 
//        if (itemOf != null)
//        {
//            if (DataCore.getDataType(itemOfReward.pid) == ConfigDataType.Item)
//            {
//                ItemData itemData = Core.Data.itemManager.getItemData(itemOf.pid);
//                if (itemData != null)
//                {
//                    if (itemData.type == (int)ItemType.Stone)
//                    {
//                        Core.Data.ActivityManager.OnReward(itemOf.num, str); //yangchenguang 获得砖石
//                    }
//                }
//            }
//        }
    }


	//added by zhangqiang to check whether has new daily gift
	//是否有新的每日奖励
	public bool HasNewDailyGift()
	{
		Dictionary<int,string> tDic = rewardDic;
		int tNum = tDic.Count;

		foreach(int keyNum in tDic.Keys){
			if (tDic.ContainsKey (keyNum)) {
				if (tDic [keyNum] == "1") {
					if (tNum > 0) {
						tNum--;
					}
				} 
			}
		}

		if (LuaTest.Instance.OpenSevenAward == false ) {
			string val = "";
			if(tDic.TryGetValue(sevenDayType,out val)){
				if(val == "1"){
					tNum++;}
			}
		}

		if (LuaTest.Instance.OpenLevelAward == false){
			if(tDic.ContainsKey(lvRewardType) &&  tDic [lvRewardType] == "1"){
				tNum++;
			}
		}
		return tNum != tDic.Count;
	}

	//added by zhangqiang to check whether has new ACT
	//是否有新的活动
	public bool HasNewAct()
	{
		Dictionary<int,string> tDic = Core.Data.ActivityManager.actDic;
		int tNum = tDic.Count;

		foreach(int keyNum in tDic.Keys){
			if (tDic.ContainsKey (keyNum)) {
				if (tDic [keyNum] == "1") {
					if(tNum >0)
						tNum--;
				} 
			}
		}
		return tNum != tDic.Count;

	}

	#endregion
}
