using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum BossComeType{
	isAttacking,
	isFinish,
}
public class UIActMonsterComeController : RUIMonoBehaviour
{
	private static UIActMonsterComeController instance;
	public static UIActMonsterComeController Instance
	{
		get
		{
			return instance;
		}
	}
	public UILabel lbl_Right_AtkLeftTime;	
	public UILabel lbl_Right_LeftTimes;
	public UILabel lbl_Right_Buff;
	public UILabel lbl_Left_BloodNum;
	public UILabel lbl_LeftMonsterName;
	public UILabel lbl_Mid_actTimeTitle;
	public UILabel lbl_NextTimeNum;
	public UILabel lbl_HurtNum;
	public UILabel lbl_MyPointNum;
    public UILabel lbl_CurPoint;
    public UIButton showRankBtn;
	UIActMonsterComeController monsterCtrl;
	public List<GameObject> rankList;

	public UIPanel beforeAtkBoss;
	public UIPanel atkingBoss;
	public GameObject panel_mid_Atking;
	public GameObject panel_mid_Finish;
	public BossComeType curPanelType;
    public bool isNormalAtk = false;
	private const int leftTime = 45;
	public GameObject Btn_Atk_Free;
	public GameObject Btn_Atk_Dia;
	public static int TimeLeft = 0;
	public List<UserAtkBossInfo> hurtDataList;
	public List<GameObject> honorGiftList;
	public GameObject tipObj;
	[HideInInspector]
	public int curMyPointNum;
    [HideInInspector]
    public bool isRankState = true;

    public UIButton btnAddPowerDia;
    public UIButton btnAddPowerCoin;

    CRLuo_ShowStage mShowOne = null;
    int previouNum = 0;
    MonsterAttribute previousAttr = MonsterAttribute.DEFAULT_NO;
    [HideInInspector]
    public  UserAtkBossInfo lastKiller =null;
    private  List<WorldBossRewardData> rewardDataList;
    public List<WorldBossRewardItem> rewardObjList;


    public bool isShowHonor = false;
    public UILabel lblShowHonor;
    public UIButton btnShowHonor;

	void Awake(){
		instance = this;
		hurtDataList = new List<UserAtkBossInfo> ();
	}
        
	void Start(){
        ActivityNetController.GetInstance().LoginBoss (BackReconnect);
		DBUIController.mDBUIInstance.HiddenFor3D_UI();
		if(ActivityNetController.GetInstance().TempAtkData!= null)
	        this.UpdateList (ActivityNetController.GetInstance().TempAtkData);
        rewardDataList = new List<WorldBossRewardData> ();
        rewardDataList = Core.Data.ActivityManager.GetWorldBossRewardDataList ();

    }

    void InitNormalRewardRank(){
        for (int i = 0; i < rewardObjList.Count; i++) {
            if (i < rewardDataList.Count) {
                rewardObjList [i].SetItemValue ((object)rewardDataList [i]);
            } else {
                rewardObjList [i].gameObject.SetActive (false);
            }
        }       
    }


	public void init(SockLoginBossData data){
        ComLoading.Close ();
		InvokeRepeating ("TimeLblCtrl", 0, 1);
        string tState = Core.Data.ActivityManager.GetActivityStateData(ActivityManager.monsterType);

        show3DModel(data.bossId,MonsterAttribute.ALL,true);
		//是否开启
        if (tState == "1") {
			curPanelType = BossComeType.isAttacking;


		} else {
			curPanelType = BossComeType.isFinish;
            InitNormalRewardRank ();
		}
		//是否第一次打
		TimeLeft = data.reliveTime;
		if (TimeLeft == 0)
			isNormalAtk = true;
		else
			{
			isNormalAtk = false;
			AtkTimer ();
		}	


		OnAttack ();
		ActivityManager.addPecent = data.add;
		ActivityManager.buyLeftTimes = data.buyTimes;

        if (data.buyTimes > 0) {
            btnAddPowerDia.isEnabled = true;
            btnAddPowerCoin.isEnabled = true;
        } else {
            btnAddPowerDia.isEnabled = false;
            btnAddPowerCoin.isEnabled = false;
        }
		RefreshBoss (data);
		this.Refresh ();
	}
        
	public static UIActMonsterComeController CreateMonsterPanel (ActivityItemType type, GameObject tObj)
	{
		UnityEngine.Object obj = WXLLoadPrefab.GetPrefab (WXLPrefabsName.UIMonsterComePanel);
		if (obj != null) {
			GameObject go = Instantiate (obj) as GameObject;
			UIActMonsterComeController fc = go.GetComponent<UIActMonsterComeController> ();
			Transform goTrans = go.transform;
            go.transform.parent = DBUIController.mDBUIInstance._bottomRoot.transform;
			go.transform.localPosition = Vector3.zero;
			goTrans.localScale = Vector3.one;
			return fc;
		}
		return null;		
	}



	/// <summary>
	/// 攻击间隔
	/// </summary>
	void AtkTimer(){
		InvokeRepeating ("TimeCount",0,1);
	}

	void TimeCount(){
		if(TimeLeft>0)
		TimeLeft--;
		lbl_Right_AtkLeftTime.text = TimeLeft.ToString ();
		if (TimeLeft == 0) {
			isNormalAtk = true;
			OnAttack ();
			CancelInvoke ("TimeCount"); 
		} else {
			isNormalAtk = false;
		}
	}

    //断线重联回调
    void BackReconnect(){
        del3DModel ();
        DBUIController.mDBUIInstance.ShowFor2D_UI ();
		if (gameObject != null) {
			Destroy (gameObject);
		}
    }

	public void On_Back ()
    {
        del3DModel ();
		DBUIController.mDBUIInstance.RefreshUserInfoWithoutShow ();
        DBUIController.mDBUIInstance.ShowFor2D_UI ();
		if (UIWXLActivityMainController.Instance!= null)
        {

			if (UIWXLActivityMainController.Instance.gameObject.activeSelf == false)
				UIWXLActivityMainController.Instance.SetActive(true);
        }
        else
        {
			UIWXLActivityMainController.CreateActivityMainPanel (DBUIController.mDBUIInstance._TopRoot);
        }
        UIWXLActivityMainController.Instance.Refresh ();
   
        ActivityNetController.GetInstance ().LogOutBoss ();

        Destroy (gameObject);
		instance = null;

	}
        
	/// <summary>
	/// 攻击 方法
	/// </summary>
	void Btn_Attack_Dia(){
		//花钱 攻击
		ActivityNetController.GetInstance ().sendAttackBoss (1);
	}

	public void On_AttackBoss_Free ()
	{
		if(isNormalAtk == true){
			// 免费 攻击
			ActivityNetController.GetInstance().sendAttackBoss (0);
			isNormalAtk = false;
		}
	}

	/// <summary>
	/// 钻石加成 20%  type 1 钻石类型
	/// </summary>
	void On_PluePropertyT(){
		ActivityNetController.GetInstance ().Addpower (1);
	}
	void AtkShowTip(){
		tipObj.SetActive (true);
	}
	public void CancelShowTip(){
		tipObj.SetActive (false);
	}

	/// <summary>
	/// 金币§加成 5%
	/// </summary>
	void On_PlusPropertyF(){
		ActivityNetController.GetInstance ().Addpower (0);
	}
	/// <summary>
	/// Shows the rank.
	/// </summary>
	public void ShowRank(){
        showRankBtn.isEnabled = false;
        RankWindowCtrl.CreatRankWindowCtrl ();
        showRankBtn.isEnabled = true;
	}



	/// <summary>
	/// Refresh panel  进入房间  需要 进行刷新
	/// </summary>
	public  void Refresh ()
	{
		switch(curPanelType) {
        case BossComeType.isFinish:
            lbl_Mid_actTimeTitle.text = Core.Data.stringManager.getString (7101);
            panel_mid_Atking.SetActive (false);
            panel_mid_Finish.SetActive (true);

            atkingBoss.gameObject.SetActive (false);
            beforeAtkBoss.gameObject.SetActive (true);
            btnShowHonor.gameObject.SetActive (false);
			RefreshHonorItemList ();
			break;
        case BossComeType.isAttacking:
            this.AttackBuffAndTimes ();
            lbl_Mid_actTimeTitle.text = Core.Data.stringManager.getString (7125);
			atkingBoss.gameObject.SetActive (true);
			beforeAtkBoss.gameObject.SetActive (false);
            btnShowHonor.gameObject.SetActive (true);
			panel_mid_Atking.SetActive (true);
			panel_mid_Finish.SetActive (false);
			break;
		default:
			break;
		}


	}
	/// <summary>
	/// 刷新boss
	/// </summary>
	public void RefreshBoss(SockLoginBossData data ){
        if (data.buyTimes < 1) {
            btnAddPowerDia.isEnabled = false;
            btnAddPowerCoin.isEnabled = false;
        } else {
            btnAddPowerDia.isEnabled = true;
            btnAddPowerCoin.isEnabled = true;
        }

		lbl_Left_BloodNum.text = data.bossHp.ToString();
		lbl_LeftMonsterName.text = Core.Data.monManager.getMonsterByNum (data.bossId).name;
		lbl_Right_LeftTimes.text = data.buyTimes.ToString ();
		lbl_Right_Buff.text = Core.Data.stringManager.getString (7152) + ActivityManager.addPecent + "%";

	}


	public void AttackBuffAndTimes (){
        if (curPanelType == BossComeType.isAttacking) {

            if (ActivityManager.buyLeftTimes > 0) {
                btnAddPowerDia.isEnabled = true;
                btnAddPowerCoin.isEnabled = true;
            } else {
                btnAddPowerDia.isEnabled = false;
                btnAddPowerCoin.isEnabled = false;
            }

			lbl_Right_LeftTimes.text = ActivityManager.buyLeftTimes.ToString ();
			lbl_Right_Buff.text = Core.Data.stringManager.getString (7152) + ActivityManager.addPecent + "%";
			lbl_Right_LeftTimes.text = ActivityManager.buyLeftTimes.ToString ();
		}
	}
	/// <summary>
	/// 刷新当前列表. 战斗时时列表
	/// </summary>
	/// <param name="strList">String list.</param>
	public void UpdateList(SockBossAtkListData data ){
        curMyPointNum = data.userPoint;
		if (data.bossCurHp == 0) 
			curPanelType = BossComeType.isFinish;
		
		if (curPanelType == BossComeType.isAttacking) {
			lbl_Left_BloodNum.text = data.bossCurHp.ToString ();
			this.RefreshList (data.attStrList);
			lbl_HurtNum.text = data.attCur.ToString ();	
            Core.Data.temper.WorldBoss_Att = data.bossCurHp;


		} else {
			
			lbl_MyPointNum.text = curMyPointNum.ToString ();
		
			hurtDataList.Clear ();
			//排行榜8个人
			for (int i = 0; i < 8; i++) {
				if (i < data.attStrList.Count - 1) {
					hurtDataList.Add (data.attStrList [i]);
				} else {
					hurtDataList.Add (null);
				}
			}

            if (data.isKill == 1)
                isRankState = true;
            else
                isRankState = false;
		}
	}

	


	public void RefreshList(List<UserAtkBossInfo> hurtBossList){
		switch (curPanelType) {
		case BossComeType.isAttacking:

			if (gameObject != null)
				for (int i = 0; i < rankList.Count; i++) {
					if (i < hurtBossList.Count) {

						rankList [i].GetComponent<ActHurtRankItem> ().SetItemValue (hurtBossList [i]);
						rankList [i].GetComponent<ActHurtRankItem> ().rank = i + 1;
					}
					else {
//						rankList [i].gameObject.SetActive (false);
//						if (i == hurtBossList.Count - 1 ) {
//							rankList [i].gameObject.SetActive (false);
//						} else {
							rankList [i].GetComponent<ActHurtRankItem> ().SetItemValue (null);
//						}
					}
				}
				else
					hurtDataList = hurtBossList;
			break;
		default:
			break;
		}
	}
	/// <summary>
	/// 攻击 按钮  立即攻击(花钱)   攻击（免费）  
	/// </summary>
	public void OnAttack(){
		if (isNormalAtk == false) {
			Btn_Atk_Free.SetActive (false);
			Btn_Atk_Dia.SetActive (true);
		} else {
			Btn_Atk_Dia.SetActive (false);
			Btn_Atk_Free.SetActive (true);
		}
	}

	public void TimeLblCtrl (){
		if( Core.Data.ActivityManager.GetActivityStateData(ActivityManager.monsterType)=="1")
            curPanelType = BossComeType.isAttacking;
            
		else
            curPanelType = BossComeType.isFinish;
            
        switch (curPanelType) {
        case BossComeType.isFinish:
            lbl_Mid_actTimeTitle.text = Core.Data.stringManager.getString (7101);
            panel_mid_Atking.SetActive (false);
            panel_mid_Finish.SetActive (true);


            atkingBoss.gameObject.SetActive (false);
            beforeAtkBoss.gameObject.SetActive (true);
            btnShowHonor.gameObject.SetActive (false);

            RefreshHonorItemList ();
            isShowHonor = false;
//            isShowHonor = true;
//            this.ShowHonorListBtn ();
            lbl_CurPoint.text  = Core.Data.stringManager.getString(7105);
            break;
        case BossComeType.isAttacking:
            this.AttackBuffAndTimes ();
            lbl_Mid_actTimeTitle.text = Core.Data.stringManager.getString (7125);

            atkingBoss.gameObject.SetActive (true);
            btnShowHonor.gameObject.SetActive (true);
            if (isShowHonor == true) {
                beforeAtkBoss.gameObject.SetActive (true);
            } else {
                beforeAtkBoss.gameObject.SetActive (false);
            }
            panel_mid_Atking.SetActive (true);
            panel_mid_Finish.SetActive (false);
            break;
        }


		long timeT =  ActivityNetController.GetInstance ().MonsterRespectTime ();
		int l = 0;
		string output = "";
		output += (timeT/3600).ToString("d2");
		l = (int)timeT % 3600;
		output += ":" + (l / 60).ToString ("d2");
		l = (int)l % 60;
		output+=":"+l.ToString("d2");
		lbl_NextTimeNum.text = output;
	}
		


	/// <summary>
	/// 刷新兑换列表
	/// </summary>
	public void RefreshHonorItemList(){

		List<HonorItemData> thorList = Core.Data.ActivityManager.GetHonorItemList ();
		for (int i = 0; i < honorGiftList.Count; i++) {
			//要兑换道具的 配置表
			if (i < thorList.Count && i <honorGiftList.Count)
				honorGiftList [i].GetComponent<ActGiftItem> ().SetItemValue ((object)thorList[i]);
			else
				honorGiftList [i].gameObject.SetActive (false);
		}
	}

	/// <summary>
	/// 收到商品展示  
	/// </summary>
    public void ShowHonorItem(ItemOfReward[] iOR,int cost){
		curMyPointNum -= cost;
		lbl_MyPointNum.text = curMyPointNum.ToString();
        lbl_CurPoint.text = Core.Data.stringManager.getString (7109) + ":[FFD700]"+curMyPointNum.ToString()+"[-]" ;
        GetRewardSucUI.OpenUI(iOR,Core.Data.stringManager.getString(5047));
        this.RefreshHonorItemList ();
	}


    void show3DModel(int num, MonsterAttribute attri, bool AllFated)
    {
        if(mShowOne == null)
        {
            mShowOne = CreateMonsterComeShowStage();
            mShowOne.Try_key = false;
        }
        RED.SetActive(true, mShowOne.gameObject);
        if(previouNum != num || attri != previousAttr)
        {
            mShowOne.ShowCharactor(num, attri, AllFated);
            previouNum = num;
            previousAttr = attri;
        }
    }

    public void del3DModel()
    {
        if(mShowOne != null)
        {
            previouNum = 0;
            previousAttr = MonsterAttribute.DEFAULT_NO;
            mShowOne.DeleteSelf();
            mShowOne = null;
        }
    }

    public static CRLuo_ShowStage CreateMonsterComeShowStage()
    {
        UnityEngine.Object obj = PrefabLoader.loadFromPack("CRLuo/System/ShowMonsterStage",false);
        if(obj != null)
        {
            GameObject go = Instantiate(obj) as GameObject;
            CRLuo_ShowStage ss = go.GetComponent<CRLuo_ShowStage>();
            return ss;
        }
        return null;
    }

    /// <summary>
    /// 展示 荣誉 按钮
    /// </summary>
    public void ShowHonorListBtn(){
        isShowHonor = !isShowHonor;
        this.HonorState ();
    }

    void HonorState(){
        if (isShowHonor == true) {  
            beforeAtkBoss.gameObject.SetActive (true);
            RefreshHonorItemList ();
            lblShowHonor.text  = Core.Data.stringManager.getString(7373) ;
            lbl_CurPoint.text = Core.Data.stringManager.getString (7109) + ":[FFD700]"+curMyPointNum.ToString()+"[-]" ;
        } else {
            beforeAtkBoss.gameObject.SetActive (false);
            atkingBoss.gameObject.SetActive (true);
            lblShowHonor.text  =Core.Data.stringManager.getString(7372) ;
            lbl_CurPoint.text  = Core.Data.stringManager.getString(7105);
        }

        btnShowHonor.gameObject.SetActive (true);
    }
        
		
}
