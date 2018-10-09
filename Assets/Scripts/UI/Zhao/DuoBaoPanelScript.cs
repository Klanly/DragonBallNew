using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class DuoBaoPanelScript : RUIMonoBehaviour {

	// Use this for initialization

	#region 天下第一
	public UILabel tianXiaDiYiRankLabel;
	public UILabel tianXiaDiYiLabel1;
//	public UILabel getZhanGongLabel;
	public UISprite tianXiaDiYiSpritObj;

	public GameObject tianXiaDiYiContentRoot;
	public GameObject tianXiaDiYiLockRoot;
	public UILabel tianXiaDiYiLockLabel;


	public UISprite tianXiaDiYiContentBackgroundSprite;
	public UISprite tianXiaDiYiMiniContentBackgroundSprite;
	public UISprite tianXiaDiYiTitleBackgroudSprite;
	#endregion

	#region 抢夺金币
	public GameObject qiangDuoContentRoot;
	public GameObject qiangDuoLockRoot;
	public UILabel qiangDuoLockLabel;
	public UISprite qiangDuoSpritObj;
	public UISprite qiangDuoContentBackgroundSprite;
	public UISprite qiangDuoMiniContentBackgroundSprite;
	public UISprite qiangDuoTitleBackgroudSprite;

	public UILabel qiangDuoGold;
	public UILabel qiangduoLabel1;
	public UILabel qiangduoLabel2;
	public UILabel SurplusQiangduo;
    #endregion

	#region 沙鲁的游戏

	public GameObject shaluContentRoot;
	public GameObject shaluLockRoot;
	public UILabel shaluLockLabel;
	public UISprite shaluSpritObj;
	public UISprite shaluContentBackgroundSprite;
	public UISprite shaluMiniContentBackgroundSprite;
	public UISprite shaluTitleBackgroudSprite;
	public UILabel RemainNumShalu;
	public UILabel OldFloorShalu;
	public UILabel TopFloorShalu;

	public UILabel mDragonTitle1;
	public UILabel mDragonTitle2;
	public UILabel RemainSummonTime;
	public UILabel TruceTime;
	public UISprite[] DragonSkillIcon;

	#endregion

	#region 布欧的游戏

	public GameObject buouContentRoot;
	public GameObject buouLockRoot;
	public UILabel buouLockLabel;
	public UISprite buouSpritObj;
	public UISprite buouContentBackgroundSprite;
	public UISprite buouMiniContentBackgroundSprite;
	public UISprite buouTitleBackgroudSprite;

	public UILabel OldFloorBuou;
	public UILabel RemainNumBuou;
	public UILabel TopFloorBuou;

	public UILabel ShaluBuouTitle1;
	public UILabel ShaluBuouTitle2;

	#endregion


    #region 神龙 提示

    public UISprite dragonTip;
    private int countingTime=0;
    bool countBool = true;
    public UILabel timeTitle;
    public Transform aoyiParent;
//    private Vector3 starPos =new Vector3(90 ,-270,0) ;
    #endregion
	private static DuoBaoPanelScript _instance;
	public static DuoBaoPanelScript Instance
	{
		get{return _instance;}
	}
	void Awake(){
		_instance = this;
	}

	void Start()
    {

		#if SPLIT_MODEL
        if(FinalTrialMgr.GetInstance().m_nDownLoadTipCnt == 0 && !Core.Data.guideManger.isGuiding && Core.Data.playerManager.RTData.downloadReawrd == 0)
		{
            List<SouceData> list = Core.Data.sourceManager.GetUpdateModes();
            if(list != null || list.Count > 0)
            {
                UIDownloadPacksWindow.OpenDownLoadUI(null);
            }
            FinalTrialMgr.GetInstance().m_nDownLoadTipCnt++;
		}
		#endif
        this.RefreshTip();

		tianXiaDiYiLabel1.SafeText(Core.Data.stringManager.getString(25157));
		qiangduoLabel1.SafeText(Core.Data.stringManager.getString(25155));
		qiangduoLabel2.SafeText(Core.Data.stringManager.getString(6015));
    
	}

    public void RefreshTip(){
        dragonTip.gameObject.SetActive(false);
        if (Core.Data.dragonManager.callDragonSucceed[(int)DragonManager.DragonType.EarthDragon] ||
            Core.Data.dragonManager.callDragonSucceed[(int)DragonManager.DragonType.NMKXDragon])
        {
            dragonTip.gameObject.SetActive(true);
        }

        this.StartRepeating();
    }

	void QiangduoRank()
	{
		UIChallengeRank.OpenUI(2);
	}

	void TianXiaRank()
	{
		UIChallengeRank.OpenUI(1);
	}

	void TianXiaDuihuan()
	{
		FinalTrialMgr.GetInstance().OpenDuihuanRequest(DuihuanBtnCallback, DuihuanFromType.Type_Duobao);
	}

	void QiangduoDuihuan()
	{
		FinalTrialMgr.GetInstance().OpenDuihuanGoldRequest(DuihuanBtnCallback);
    }
    
	void DuihuanBtnCallback()
	{
		return;
	}

	public void QiangduoIntroduction()
	{
		UIRobIntroduct.OpenUI(false);
	}



	void OnBtnWuDaoDaHui()
	{

	}

	void OnBtnQiangDuoZhan()
	{
																																																		
	}

	void OnBtnShaLu()
	{

	}

    public void OnBtnQuit(){
        Destroy(gameObject);
		FinalTrialMgr.GetInstance().jumpTo = TrialEnum.None;
        DBUIController.mDBUIInstance.ShowFor2D_UI();
		DBUIController.mDBUIInstance.RefreshUserInfo ();
    }

    void OnBtnBuOu() {
		hide();
	}

	public void hide()
	{
		FinalTrialMgr.GetInstance().jumpTo = TrialEnum.None;
		gameObject.SetActive (false);
		DBUIController.mDBUIInstance.ShowFor2D_UI();
	}

	//奥义设置
	public void SetDragonskillIcon(List<AoYi> m_SpriteNameArray)
	{

        List<AoYi> tAoyiList = new List<AoYi>();
        for (int i = 0; i < m_SpriteNameArray.Count; i++)
        {
            if (m_SpriteNameArray[i] != null)
                tAoyiList.Add(m_SpriteNameArray[i]);
            else
                tAoyiList.Add(null);
        }
				//   Debug.Log(" aoyi  list  count " + tAoyiList.Count);
        for(int i=0; i< DragonSkillIcon.Length; i++)
        {
            foreach (AoYi tA in tAoyiList)
            {
                if (tA != null)
                {
                    if (i == tA.Pos)
                    {
                        DragonSkillIcon[i].gameObject.SetActive(true);
                        DragonSkillIcon[i].spriteName = tA.Num.ToString();
                        break;
                    }
                }
                else
                {
                    DragonSkillIcon[i].gameObject.SetActive(false);
                }
            }
		}


        //aoyiParent.transform.localPosition = starPos + Vector3.left* 20* tNo;

//		for(int j=0; j<m_SpriteNameArray.Count; j++)
//		{
//			m_index = m_SpriteNameArray[j].Pos;
//			if(m_index > -1 && m_index < 5)
//			{
//				DragonSkillIcon[m_index].gameObject.SetActive(true);
//				DragonSkillIcon[m_index].spriteName = m_SpriteNameArray[j].ID.ToString();
//			}
//		}

	}

	//召唤倒计时
	public void SetDragonSummonTime()
	{
        countingTime++;
        long tDragonTypeTime = 0;
        if (countingTime > 5)
        {
            countBool = !countBool;
            countingTime = 0;
        }

        if (countBool == true)
        {
			if (Core.Data.dragonManager.callDragonSucceed [(int)DragonManager.DragonType.EarthDragon]) {
				RemainSummonTime.SafeText (Core.Data.stringManager.getString (25145));
			}
			else {
				tDragonTypeTime = Core.Data.dragonManager.callEarthDragonTime;
				DateTime d = new DateTime();
				d = d.AddSeconds(tDragonTypeTime);
				string tTime = string.Format("{0:D2}:{1:D2}", d.Minute, d.Second);
				RemainSummonTime.SafeText(tTime);
			}
            timeTitle.text = Core.Data.stringManager.getString(25146);
        }
        else
        {
			//Debug.Log ( " NMKXDragon == " + Core.Data.dragonManager.callDragonSucceed[(int)DragonManager.DragonType.NMKXDragon] );
			if (Core.Data.dragonManager.callDragonSucceed [(int)DragonManager.DragonType.NMKXDragon]) {
				RemainSummonTime.SafeText (Core.Data.stringManager.getString (25145));
			}
			else {
				tDragonTypeTime = Core.Data.dragonManager.callNMKXDragonTime;
				DateTime d = new DateTime();
				d = d.AddSeconds(tDragonTypeTime);
				string tTime = string.Format("{0:D2}:{1:D2}", d.Minute, d.Second);
				RemainSummonTime.SafeText(tTime);
			}
            timeTitle.text = Core.Data.stringManager.getString(25149);
        }
       
		aoyiParent.GetComponent<UIGrid> ().repositionNow = true;
	}

	//免战倒计时
	public void SetTruceTime()
	{
        string m_time = null;
		if (Core.Data.dragonManager.mianZhanTime <= 1)
        {
            m_time = null;
        }
		else 
        {
            DateTime d = new DateTime();
            d = d.AddSeconds(Core.Data.dragonManager.mianZhanTime);
			m_time = string.Format( "{0:D2}:{1:D2}:{2:D2}", d.Hour,d.Minute, d.Second);
        }
        if (string.IsNullOrEmpty(m_time))
        {
            TruceTime.SafeText(Core.Data.stringManager.getString(25148));
        }
        else
        {
            TruceTime.SafeText(m_time);
        }
	}

    void StartRepeating(){
        CancelInvoke("SetDragonSummonTime");
        CancelInvoke("SetTruceTime");
        countingTime = 0;
        InvokeRepeating("SetDragonSummonTime",0,1);
        InvokeRepeating("SetTruceTime",0,1);

        SetDragonskillIcon(Core.Data.dragonManager.usedToList());
    }

	public void show()
	{
		DBUIController.mDBUIInstance.HiddenFor3D_UI();
		this.setContent(tianXiaDiYiContentRoot, tianXiaDiYiLockRoot, tianXiaDiYiSpritObj, "challenge-0004", "challenge-0005",
			tianXiaDiYiContentBackgroundSprite, tianXiaDiYiMiniContentBackgroundSprite, tianXiaDiYiTitleBackgroudSprite, 
			tianXiaDiYiLockLabel, 10,
			Core.Data.playerManager.RTData.curLevel < 10);

		this.setContent(shaluContentRoot, shaluLockRoot, shaluSpritObj, "challenge-0008", "challenge-0002",
		                shaluContentBackgroundSprite, shaluMiniContentBackgroundSprite, shaluTitleBackgroudSprite, 
		                shaluLockLabel, 15,
		                Core.Data.playerManager.RTData.curLevel < 15);
		
		this.setContent(buouContentRoot, buouLockRoot, buouSpritObj, "challenge-0003", "challenge-0001",
		                buouContentBackgroundSprite, buouMiniContentBackgroundSprite, buouTitleBackgroudSprite, 
		                buouLockLabel, 20,
		                Core.Data.playerManager.RTData.curLevel < 20);

		this.setContent(qiangDuoContentRoot, qiangDuoLockRoot, qiangDuoSpritObj, "challenge-0007", "challenge-0006",
			qiangDuoContentBackgroundSprite, qiangDuoMiniContentBackgroundSprite, qiangDuoTitleBackgroudSprite, 
			qiangDuoLockLabel, 20,
			Core.Data.playerManager.RTData.curLevel < 20);



        this.RefreshTip();
		
		SetActive(true);
	}

	void setContent(GameObject contentRoot, GameObject lockRoot, 
		UISprite coverSprit, string coverLockSpritName, string coverUnLockSpritName,
		UISprite contentBackgroundSprite, UISprite miniContentBackgroundSprite ,UISprite titleBackgroudSprite, 
		UILabel lockLabel, int lockLv,
		bool isLock)
	{
		contentRoot.SetActive(!isLock);
		lockRoot.SetActive(isLock);
		if(isLock)
		{
			coverSprit.color = Color.black;
//			coverSprit.spriteName = coverLockSpritName;
			contentBackgroundSprite.spriteName = "common-0036";
			titleBackgroudSprite.spriteName = "common-0015";
			miniContentBackgroundSprite.spriteName = "common-0034";
			lockLabel.text = Core.Data.stringManager.getString(6027).Replace("#", lockLv.ToString());

		}
		else
		{
			if(contentRoot == shaluContentRoot )
			{
				if(FinalTrialMgr.GetInstance().IsInFloorBuou)
				{
					coverSprit.color = Color.black;
//					coverSprit.spriteName = coverLockSpritName;
				}
				else
				{
//					coverSprit.spriteName = coverUnLockSpritName;
					coverSprit.color = Color.white;
				}
				mDragonTitle1.SafeText(Core.Data.stringManager.getString(25146));
				mDragonTitle2.SafeText(Core.Data.stringManager.getString(25147));
//				List<AoYi> m_TempAoyi = Core.Data.dragonManager.getAllAoYi();
//				List<AoYi> AoyiArray = new List<AoYi>();
//				foreach(AoYi data in m_TempAoyi)
//				{
//					if(data.Pos>0)
//					{
//						AoyiArray.Add(data);
//					}
//				}
                SetDragonskillIcon(Core.Data.dragonManager.usedToList());
			}
			else if(contentRoot == buouLockRoot )
			{
				if(FinalTrialMgr.GetInstance().IsInFloorShalu)
				{
					coverSprit.color = Color.black;
//					coverSprit.spriteName = coverLockSpritName;
				}
				else
				{
//					coverSprit.spriteName = coverUnLockSpritName;
					coverSprit.color = Color.white;
				}
			}
			else
			{
//				coverSprit.spriteName = coverUnLockSpritName;
				coverSprit.color = Color.white;
				if(contentRoot == qiangDuoContentRoot)
				{
					if(FinalTrialMgr.GetInstance().allPVPRobData!=null && FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus!= null && FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus.robs != null 
					   && FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus.robs.count - FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus.robs.passCount >= 0)
					{
						SurplusQiangduo.SafeText(Core.Data.stringManager.getString(25173) + (FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus.robs.count - FinalTrialMgr.GetInstance().allPVPRobData.pvpStatus.robs.passCount).ToString());
					}
				}
			}
			contentBackgroundSprite.spriteName = "common-0035";
			titleBackgroudSprite.spriteName = "common-0018";
			miniContentBackgroundSprite.spriteName = "bag-0001";
			lockLabel.text = "";
		}
	}

	public void SetShaluBuouData(GetDuoBaoLoginInfoResponse mResponse)
	{
		if((mResponse.data.atkStatus.totalCout-mResponse.data.atkStatus.yetCount) <= 0) 
		{
//			RemainNumShalu.text = "0";
		}
		else 
		{
//			RemainNumShalu.text = (mResponse.data.atkStatus.totalCout-mResponse.data.atkStatus.yetCount).ToString();
        }
        if((mResponse.data.defStatus.totalCout-mResponse.data.defStatus.yetCount) <= 0)
		{
			RemainNumBuou.text = "0";
        }
		else 
		{
			RemainNumBuou.text = (mResponse.data.defStatus.totalCout-mResponse.data.defStatus.yetCount).ToString();
		}

		ShaluBuouTitle1.SafeText(Core.Data.stringManager.getString(25150));
		ShaluBuouTitle2.SafeText(Core.Data.stringManager.getString(25151));

		if(mResponse.data.defStatus.todayBest <= 0 ) OldFloorBuou.text = "0";
		else OldFloorBuou.text = mResponse.data.defStatus.todayBest.ToString();
//		if(mResponse.data.atkStatus.todayBest <= 0 ) OldFloorShalu.text = "0";
//		else OldFloorShalu.text = mResponse.data.atkStatus.todayBest.ToString();
		if(mResponse.data.defStatus.historyBest <= 0 ) TopFloorBuou.text = "0";
		else TopFloorBuou.text = mResponse.data.defStatus.historyBest.ToString();
//		if(mResponse.data.atkStatus.historyBest <= 0 ) TopFloorShalu.text = "0";
//		else TopFloorShalu.text = mResponse.data.atkStatus.historyBest.ToString();

	}

	public static DuoBaoPanelScript CreateQiangDuoPanel()
	{
		UnityEngine.Object obj = PrefabLoader.loadFromPack("GX/pbDuoBaoPanel");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			DuoBaoPanelScript cc = go.GetComponent<DuoBaoPanelScript>();
			return cc;
		}
		return null;
	}
}
