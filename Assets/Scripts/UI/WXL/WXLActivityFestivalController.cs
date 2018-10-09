using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WXLActivityFestivalController:RUIMonoBehaviour
{

	private static WXLActivityFestivalController instance;
	public static WXLActivityFestivalController Instance
	{
		get
		{
			return instance;
		}
	}
		
	public UILabel lbl_Title_ActivityTime;
	public UILabel curScore;
	public UILabel giftName;
	public UILabel needDiaNum;
    public UISprite giftIcon;

	public GameObject objNum;

	public UILabel leaveNextTime;

	public GameObject objParent;
	public GameObject detailsObj;
	public GameObject uiGrid;
    public long timeLeft = -1;

	public UIButton freeBtn;
	public UIButton diaBtn;

    public List<FestivalRankItem> playerItemList;
	 
	private const int rankMaxNum= 9;
    private const int NeedDiamon = 350;

	public long startDate;
	public long endDate;
	bool isFree = false;
    bool isStoneFree = false;

    public StarsUI starNum;
	[HideInInspector]
    //	public bool canBuyLottery = true;
    //WXLActivityFestivalController festivalCtrl;
	private ActivityReward m_rewardData;

    public UISprite blackPic;
	public UILabel lblMoneyBtn;

    MonsterData  bestMonster = null;
//    const string disableSpStr = "common-0018_gray";
//    const string normalSpStr = "common-0018";
//    public string testDisStr = "common-0018_gray";
//    public string testNorStr = "common-0018";
//    public UISprite freeBtnSp;
//    public UISprite diaBtnSp;

	void Awake(){		
		instance = this;
	}
				

	public void Init(SockLoginFestivalData data){

        if (data.times == 1) {
			leaveNextTime.text = Core.Data.stringManager.getString (7136);
			isFree = true;

        } else if(data.times ==0){
			isFree = false;
            timeLeft=long.Parse(data.freeTime);
            string output = "";
            output +=(timeLeft/3600).ToString("d2");
            long l = (long)timeLeft % 3600;
            output += ":" + (l / 60).ToString("d2");
            l = (long)l % 60;
            output += ":" + l.ToString("d2");
            //  Debug.Log (output);
            leaveNextTime.text = output;
			this.TimerCounting (long.Parse(data.nowTime),Core.TimerEng.curTime+long.Parse(data.freeTime),1);

			InvokeRepeating ("TimeLblCtrl" ,0,1);
		}
        if (data.stoneTimes == 1) {
            isStoneFree = true;
        } else {
            isStoneFree = false;
        }
		startDate = long.Parse( data.startTime);
		endDate = long.Parse( data.endTime);
		this.ActivityDateCtrl ();
		curScore.text = data.jifen.ToString();

		if (data.wzTimes > 0)
			lblMoneyBtn.text = Core.Data.stringManager.getString (5070) + "(" + data.wzTimes + "/10)";
		else if (data.stoneTimes == 1) {
			lblMoneyBtn.text = Core.Data.stringManager.getString (5070);
		}

		bestMonster = Core.Data.monManager.getMonsterByNum (data.goodId);

        this.AnalysisICon (data.goodId);

		this.BtnCtrl ();
        blackPic.gameObject.SetActive (false);
		UILabel tNum = objNum.GetComponentInChildren<UILabel>();
		if (data.goodNum > 0) {
			objNum.SetActive (true);
			tNum.text = ItemNumLogic.setItemNum (data.goodNum, tNum, objNum.GetComponent<UISprite> ()); // yangchenguang 
		} else {
			objNum.SetActive (false);
		}
	}


    void AnalysisICon(int rewardId){

        if (rewardId == 0)
            return;
        switch(DataCore.getDataType(rewardId)){
			case ConfigDataType.Monster:
				MonsterData tDM = Core.Data.monManager.getMonsterByNum (rewardId);
				if (tDM != null) {
					giftName.text = tDM.name;
					AtlasMgr.mInstance.SetHeadSprite (giftIcon, rewardId.ToString ());
					giftIcon.spriteName = tDM.ID.ToString ();
					starNum.SetStar (tDM.star);
				}
                break;
			case ConfigDataType.Item:
			ItemData tDI = Core.Data.itemManager.getItemData (rewardId);
			if (tDI != null) {
				giftName.text = tDI.name;
				giftIcon.atlas = AtlasMgr.mInstance.itemAtlas;
				giftIcon.spriteName = tDI.iconID.ToString ();
				starNum.SetStar (tDI.star);
			}
                break;
			case ConfigDataType.Equip:
			EquipData tDE = Core.Data.EquipManager.getEquipConfig (rewardId);
			if (tDE != null) {
				giftName.text = tDE.name;
				giftIcon.atlas = AtlasMgr.mInstance.equipAtlas;
				giftIcon.spriteName = tDE.ID.ToString ();
				starNum.SetStar (tDE.star);
			}
                break;
			case ConfigDataType.Gems:
				GemData tDG = Core.Data.gemsManager.getGemData (rewardId);
				if (tDG != null) {
					giftName.text =tDG.name;
					giftIcon.atlas = AtlasMgr.mInstance.commonAtlas;
					giftIcon.spriteName = tDG.anime2D;
					starNum.SetStar (tDG.star);
				}
                break;
			case ConfigDataType.Frag:
				SoulData tDS = Core.Data.soulManager.GetSoulConfigByNum (rewardId);
				if (tDS != null) {

					ConfigDataType realType = DataCore.getDataType(tDS.updateId);
					switch(realType)
					{
					case ConfigDataType.Monster:
						AtlasMgr.mInstance.SetHeadSprite (giftIcon, tDS.updateId.ToString ());
						break;
					case ConfigDataType.Equip:
						giftIcon.atlas = AtlasMgr.mInstance.equipAtlas;
						giftIcon.spriteName = tDS.updateId.ToString ();
						break;
					case ConfigDataType.Item:
						giftIcon.atlas = AtlasMgr.mInstance.itemAtlas;
						giftIcon.spriteName = tDS.updateId.ToString ();
						break;
					}
					giftName.text = tDS.name;
//					giftIcon.atlas = AtlasMgr.mInstance;
//					giftIcon.spriteName = tDS.updateId.ToString ();
					starNum.SetStar (tDS.star);
				}
	            break;
            default:
                RED.LogError(" not found  : " + rewardId);
                break;
        }
    }
	

	void Start(){
        //  canBuyLottery = true;
        ActivityNetController.GetInstance().SendLoginFestival (ReconnectBack);
		DBUIController.mDBUIInstance.HiddenFor3D_UI();
	}

	void BtnCtrl(){
    
        if (isFree == true) {
            freeBtn.isEnabled = true;
        } else {
            freeBtn.isEnabled = false;
        }
        if (isStoneFree == true) {
            needDiaNum.text = Core.Data.stringManager.getString (7136);
            diaBtn.isEnabled = true;
        }
        else{
            if(NeedDiamon > Core.Data.playerManager.RTData.curStone){
                diaBtn.isEnabled = false;
            }else{
                diaBtn.isEnabled = true;
            }
            needDiaNum.text = NeedDiamon.ToString();
        }


	}

//
//	public WXLActivityFestivalController(object obj){
//			objParent = (GameObject)obj;
//			festivalCtrl = CreateFestivalPanel (ActivityItemType.festivalItem,objParent);
//			
//	}
	
	public static WXLActivityFestivalController CreateFestivalPanel(ActivityItemType type,GameObject tObj){
		UnityEngine.Object obj = WXLLoadPrefab.GetPrefab (WXLPrefabsName.UIFestivalPanel);
        if(obj != null)
        {
            GameObject go = Instantiate(obj) as GameObject;
            WXLActivityFestivalController fc = go.GetComponent<WXLActivityFestivalController>();
			Transform goTrans = go.transform;
			go.transform.parent = tObj.transform;
			go.transform.localPosition = Vector3.zero;
			goTrans.localScale = Vector3.one;
            return fc;
        }
        return null;		
	}
	
	/// <summary>
	///  the freebutton
	/// </summary>
	public void On_BtnFree(){
		if (isFree == true) {
            freeBtn.isEnabled = false;
            diaBtn.isEnabled = false;
			ActivityNetController.GetInstance ().BuyLotteryInFestival (1);

		}
	}
	
	public void On_BtnDia(){
        if (isStoneFree == false) {
            if (Core.Data.playerManager.RTData.curStone < NeedDiamon) {
                ActivityNetController.ShowDebug (Core.Data.stringManager.getString (7310));
                return;
            } else {
                diaBtn.isEnabled = false;      
                freeBtn.isEnabled = false;
                ActivityNetController.GetInstance ().BuyLotteryInFestival (2);
            }
        }else{
            diaBtn.isEnabled = false;      
            freeBtn.isEnabled = false;
            ActivityNetController.GetInstance ().BuyLotteryInFestival (2);
        }
	
	}

    void ReconnectBack(){
        DBUIController.mDBUIInstance.ShowFor2D_UI ();
        Destroy (gameObject);
    }
	
	public void On_Back(){
		ActivityNetController.GetInstance ().SendLogOutFestival ();
		DBUIController.mDBUIInstance.ShowFor2D_UI ();
        Destroy (gameObject);
		instance = null;
	}

    void OnDestroy(){
		if (UIWXLActivityMainController.Instance.gameObject.activeInHierarchy == false)
			UIWXLActivityMainController.Instance.SetActive (true);
        UIWXLActivityMainController.Instance.Refresh ();
    }
	
	public void On_Details(){

        TweenScale.Begin (detailsObj,0.3f,Vector3.one);
        blackPic.gameObject.SetActive (true);
	}
	
	public void On_BtnClose(){
        TweenScale.Begin (detailsObj,0.3f,Vector3.zero);
        blackPic.gameObject.SetActive (false);
	}
	
	public void Refresh(){
	
	}
	
	/// <summary>
	/// Refreshs the rank board.  value for item is null   
	/// </summary>
	public void RefreshRank(List<SockFRankItem> rankList){

		for(int i=0;i< rankMaxNum;i++){
			if (i < rankList.Count)
				playerItemList [i].SetItemValue (rankList [i]);
			else {
				SockFRankItem tItem = new SockFRankItem ();
				tItem.point = 0;
                tItem.userName = Core.Data.stringManager.getString(7354);
				playerItemList [i].SetItemValue (tItem);
			}
		}

	}

	/// <summary>
	/// 展示奖品
	/// </summary>

	public void ShowReward( ItemOfReward[] Reward){
		DBUIController.mDBUIInstance.RefreshUserInfo();
		//GetRewardSucUI.OpenUI(Reward,Core.Data.stringManager.getString(7317));
		this.AnalysisReward (Reward);
	}


	void AnalysisReward(ItemOfReward[] Reward){
		if (Reward.Length > 0 && Reward.Length < 2) {
			if (Reward [0].pid != 0) {
				if (Reward [0].getCurType () == ConfigDataType.Monster) {
					MonsterData tMD = Core.Data.monManager.getMonsterByNum (Reward [0].pid);
					if (tMD != null) {
						if (tMD.star > 3) {
							CradSystemFx.GetInstance ().SetCardSinglePanel (Reward, 350, false, true, false);
						} else {

							GetRewardSucUI.OpenUI (Reward, Core.Data.stringManager.getString (7317));
						}
					}
				} else {
					GetRewardSucUI.OpenUI (Reward,Core.Data.stringManager.getString(7317));
				}
			}
		} else {
			GetRewardSucUI.OpenUI (Reward,Core.Data.stringManager.getString(7317));
		}

	}


	public void ShowScore(SockFestivalBugLotteryData data){
		curScore.text = data.jifen.ToString();
		if (data.wzTimes > 0)
			lblMoneyBtn.text = Core.Data.stringManager.getString (5070) + "(" + data.wzTimes + "/10)";
		else if (data.stoneTimes == 1) {
			lblMoneyBtn.text = Core.Data.stringManager.getString (5070);
		}

        if (data.type == 1) {
            isFree = false;
            this.TimerCounting (0, long.Parse (data.freeTime) + Core.TimerEng.curTime, 1);
            timeLeft =int.Parse( data.freeTime );
            InvokeRepeating ("TimeLblCtrl", 0, 1);
        } 
        if (data.stoneTimes == 1 ) {
            isStoneFree = true;
        } else
            isStoneFree = false;

		this.BtnCtrl ();
	}


	#region 计时器

	/// <summary>
	/// 计时器
	/// </summary>
	/// <param name="sTime">开始时间.</param>
	/// <param name="endTime">截止时间.</param>
	/// <param name="interval">时间间隔  = 1 秒.</param>
	void TimerCounting (long sTime, long DeadTime, int interval)
	{
        TimerTask task = new TimerTask (Core.TimerEng.curTime, DeadTime, interval);
		task.onEventBegin += eventBegin;
		task.onEventEnd += eventEnd;
		task.onEvent += (TimerTask t) => {
			timeLeft = t.leftTime;
		};

		task.DispatchToRealHandler ();
	}

	void eventBegin (TimerTask task)
	{
        timeLeft = 0;
	}
	void eventEnd (TimerTask task)
	{
        if (task == null) {
            return;
        }
        timeLeft = -1;
//        CancelInvoke("TimeLblCtrl");
//       // timeLeft = 0;
//        leaveNextTime.text = Core.Data.stringManager.getString (7136);
//        isFree = true;
//        this.BtnCtrl();
    }

	#endregion


	public void  TimeLblCtrl (){

        if (timeLeft < 0)
        {
            leaveNextTime.text = Core.Data.stringManager.getString (7136);
            isFree = true;
            this.BtnCtrl();
            CancelInvoke("TimeLblCtrl");
            return;
        }
		int l = 0;
		string output = "";
		output +=(timeLeft/3600).ToString("d2");
		l = (int)timeLeft % 3600;
		output += ":" + (l / 60).ToString("d2");
		l = (int)l % 60;
        output += ":" + l.ToString("d2");
		leaveNextTime.text = output;
     
	}

	public void ActivityDateCtrl(){
		DateTime tStartDate =DateHelper.UnixTimeStampToDateTime (startDate,true);
		DateTime tEndDate =DateHelper.UnixTimeStampToDateTime (endDate,true);
        lbl_Title_ActivityTime.text = string.Format(Core.Data.stringManager.getString(7308), tStartDate.Month, tStartDate.Day, tStartDate.Hour.ToString("d2") + ":" + tStartDate.Minute.ToString("d2")) +"-"+ string.Format(Core.Data.stringManager.getString(7308),tEndDate.Month,tEndDate.Day,tEndDate.Hour.ToString("D2")+":"+tEndDate.Minute.ToString("D2"));  
    }


    void OnLookTheMonsterInfo(){
        if (bestMonster != null) {
            Monster tMonster = new Monster ();
            tMonster.RTData = new RuntimeMonster ();
			tMonster.InitConfig ();
			tMonster.RTData.curLevel = 1;
			tMonster.RTData.curExp = 1;
			tMonster.config =Core.Data.monManager.getMonsterByNum (bestMonster.ID);
			tMonster.config.ID = bestMonster.ID;
			tMonster.RTData.Attribute = MonsterAttribute.FIRE;
			tMonster.pid = bestMonster.ID;
			tMonster.num = bestMonster.ID;
			tMonster.config.atk = bestMonster.atk;
			tMonster.config.def = bestMonster.def;
			tMonster.BTData = new BattleMonster((int)tMonster.config.atk,tMonster.enhanceAttack,(int)tMonster.config.def,tMonster.enhanceDefend);
            MonsterInfoUI.OpenUI (tMonster, ShowFatePanelController.FateInPanelType.isInTeamModifyPanel,false);
        }
    }
}