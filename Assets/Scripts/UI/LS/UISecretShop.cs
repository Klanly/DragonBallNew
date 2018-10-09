using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UISecretShop : RUIMonoBehaviour {

	SecretShopMgr _Mgr{
		get{
			return SecretShopMgr.GetInstance();
		}
	}

    public List<UISecretShopCell> UISecretShopCellList = new List<UISecretShopCell> ();
	public UILabel mTime;

	public GameObject shopone;
	public GameObject shoptwo;
	public GameObject Zejuan;

	List<SecretShopDataStruct> VipList = new List<SecretShopDataStruct>();
	List<SecretShopDataStruct> LvList = new List<SecretShopDataStruct>();
	List<SecretShopDataStruct> NormalList = new List<SecretShopDataStruct>();

    List<SecretShopDataStruct> allList = new List<SecretShopDataStruct>();

	public SoulRewardPanel _SoulRewardPanel;

	public List<UISecretSoulHero> SoulHeroList = new List<UISecretSoulHero>();
    public GameObject itemRoot;
	public GameObject itemRootBaBa;
	public UIGrid m_BabaGrid;
	public UIGrid m_SoulGrid;
	public UILabel m_BaBaRefreshBtn;
	public UILabel m_ZeJuanRefreshBtn;

	public UILabel m_BuySoulBtn1;
	public UILabel m_BuySoulBtn2;

	public UILabel m_CurJifenLab;
	public UILabel m_CurJifenNum;
    
    long _time;
	
    int costMoney;
	int costMoneyType;
	int surplusRefresh;

	public void OnShow(SecretShopResponse mresponse, int shoptype)
	{
		if(mresponse.data == null)return;
		if(mresponse.data.Items == null)return;
        allList.Clear();
        VipList.Clear();
        NormalList.Clear();
        LvList.Clear();
		costMoney = _Mgr._RefreshMoney;
		costMoneyType = _Mgr._RefreshMoneyType;
        if(shoptype == 1)
		{
			if(m_CurJifenLab != null)m_CurJifenLab.SafeText(Core.Data.stringManager.getString(6023));
			if(m_CurJifenNum != null)m_CurJifenNum.SafeText(mresponse.data.jifen.ToString());
            if(mresponse.data != null && m_BaBaRefreshBtn != null)
			{
				surplusRefresh = mresponse.data.surplusRefreshTimes;
				m_BaBaRefreshBtn.SafeText(Core.Data.stringManager.getString(6004) + "("  + (mresponse.data.refreshMaxTimes - mresponse.data.surplusRefreshTimes).ToString() + "/" + mresponse.data.refreshMaxTimes.ToString() + ")");
			}
			for(int i=0; i<mresponse.data.Items.Length; i++)
			{
				if(mresponse.data.Items[i].type == 1)
				{
					VipList.Add(mresponse.data.Items[i]);
				}
				else if(mresponse.data.Items[i].type == 2)
				{
					LvList.Add(mresponse.data.Items[i]);
				}
                else
                {
                    NormalList.Add(mresponse.data.Items[i]);
                }
                allList.Add (mresponse.data.Items[i]);
            }
		}
		else
        {   //则卷售货屋
			if(mresponse.data != null && m_ZeJuanRefreshBtn != null)
			{
				surplusRefresh = mresponse.data.surplusRefreshTimes;
				m_ZeJuanRefreshBtn.SafeText(Core.Data.stringManager.getString(6004) + "("  + (mresponse.data.refreshMaxTimes - mresponse.data.surplusRefreshTimes).ToString() + "/" + mresponse.data.refreshMaxTimes.ToString() + ")");
			}
			for(int i=0; i<8; i++)
			{
				if(mresponse.data.Items[i].type == 1)
				{
					VipList.Add(mresponse.data.Items[i]);
				}
				else if(mresponse.data.Items[i].type == 2)
				{
					LvList.Add(mresponse.data.Items[i]);
				}
                else
                {
                    NormalList.Add(mresponse.data.Items[i]);
                }
                allList.Add (mresponse.data.Items[i]);
            }

			for(int j=0; j<SoulHeroList.Count; j++)
			{
				SoulHeroList[j].SetDetail(mresponse.data.souls[j], 0);
				SoulHeroList[j].gameObject.SetActive(true);
			}
			m_BuySoulBtn1.SafeText(_Mgr._purchaseSoulStone.ToString() + " " + Core.Data.stringManager.getString(6057));
			m_BuySoulBtn2.SafeText(_Mgr._purchaseSoulStone.ToString() + " " + Core.Data.stringManager.getString(6057));
		}
//0 no 1 vip 2 lv

        if (UISecretShopCellList == null || UISecretShopCellList.Count ==0)
        {
            for (int i = 0; i < allList.Count; i++)
            {
                InitItem();
            }
        }
				//        Debug.Log(UISecretShopCellList.Count + " count ttttttt");

        for(int i=0;i<allList.Count;i++){
            if(allList[i] != null && UISecretShopCellList [i] != null)
                UISecretShopCellList [i].OnShow (allList[i]);
		}
		if(shoptype == 1)m_BabaGrid.Reposition();
		else m_SoulGrid.Reposition();

		TimerMgr(mresponse.data.Flush);

        DBUIController.mDBUIInstance.RefreshUserInfo();
	}

	public void Back_OnClick()
	{
		VipList.Clear();
		LvList.Clear();
		NormalList.Clear();
        allList.Clear ();
		SoulHeroList.Clear();

		SecretShopMgr.GetInstance().m_data = null;
		SecretShopMgr.GetInstance().CloseSecretTime();
		SecretShopMgr.GetInstance().Clear();

        Destroy (gameObject);
        //gameObject.SetActive(false);
		DBUIController.mDBUIInstance.ShowFor2D_UI();

	}
		
	void Start()
	{
		_SoulRewardPanel.gameObject.SetActive(false);
		if(SecretShopMgr.GetInstance()._ShopType == 1)
		{
			shopone.gameObject.SetActive(true);
			shoptwo.gameObject.SetActive(false);
			Zejuan.gameObject.SetActive(false);
		}
		else
		{
			shopone.gameObject.SetActive(false);
			shoptwo.gameObject.SetActive(true);
			Zejuan.gameObject.SetActive(true);
			foreach(UISecretSoulHero script in SoulHeroList)
			{
				script.gameObject.SetActive(false);
			}
		}
	}

	void Detail_OnClick()
	{
//		SecretShopMgr.GetInstance().SetSecretShopTag(true, SecretShopType.SecretShopType_Dec);
	}

	public void ClearChoose()
	{
		foreach(UISecretShopCell item in UISecretShopCellList)
		{
			item.mChoose.gameObject.SetActive(false);
		}
	}
		
    //初始化 item
    public void InitItem(){
        UnityEngine.Object obj = WXLLoadPrefab.GetPrefab (WXLPrefabsName.UISecretShopCellItem);
        if (obj != null) {
            GameObject go = Instantiate (obj) as GameObject;
            UISecretShopCell fc = go.GetComponent<UISecretShopCell> ();
            Transform goTrans = go.transform;
			if(SecretShopMgr.GetInstance()._ShopType == 2)go.transform.parent = itemRoot.transform;
			else go.transform.parent = itemRootBaBa.transform;
            goTrans.localScale = Vector3.one;
            go.transform.localPosition  = Vector3.zero ;
            UISecretShopCellList.Add (fc);
            //   fc.Init(id);
            //return fc;
        }
    }

    public void RefreshShopBtn(){
		if(surplusRefresh <= 0)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(25172));
			return;
		}

		string tStr = "";
		if(costMoneyType == 0)tStr = string.Format(Core.Data.stringManager.getString(7394),costMoney.ToString(), Core.Data.stringManager.getString(5070));
		else if(costMoneyType == 1)tStr = string.Format(Core.Data.stringManager.getString(7394),costMoney.ToString(), Core.Data.stringManager.getString(5037));
        UIInformation.GetInstance ().SetInformation (tStr,Core.Data.stringManager.getString (5030),RefreshRequest,null);
    }

    public  void RefreshRequest()
	{
        Debug.Log(" refresh =" + SecretShopMgr.GetInstance()._ShopType  );
        if(SecretShopMgr.GetInstance()._ShopType != 0)
		{
			if(costMoneyType == 0)
			{			
				if(Core.Data.playerManager.RTData.curStone < costMoney)
				{
					SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(35006));
					return;
				}
			}
			else if(costMoneyType == 1)
			{			
				if(Core.Data.playerManager.RTData.curCoin < costMoney)
				{
					SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(35000));
					return;
				}
			}
            SecretShopMgr.GetInstance().RefreshSecretShop();
        }
    }


	void BuySoul_OnClick()
	{
		SecretShopMgr.GetInstance().SecretSoulHeroRequest();
	}

	void TimerMgr(long[] secrettime)
	{
		if(secrettime == null )return;
		if(secrettime.Length == 0)return;

		DateTime dtstart = new DateTime(1970,1,1,0,0,0,0, DateTimeKind.Utc);
		DateTime mydata = dtstart.AddSeconds(secrettime[0]).ToLocalTime();
//		DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
//		long lTime = long.Parse(secrettime[0].ToString() + "0000000");
//		TimeSpan toNow = new TimeSpan(lTime);
		
//        int l = 0;
//        long T = 0;
//		string output = "";
//		T = secrettime[0]%(3600*24);
//		output += (T/3600).ToString("d2");
//		l = (int)T % 3600;
//		output += ":" +(l / 60).ToString ("d2");
//		l = (int)l % 60;
//		output+= ":" +l.ToString("d2");

		mTime.SafeText(mydata.Hour.ToString("d2") + ":" + mydata.Minute.ToString("d2") + ":" + mydata.Second.ToString("d2"));
	}
    
}
