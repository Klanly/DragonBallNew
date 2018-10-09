using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
//vip 功能界面控制脚本
public class UIDragonVipTequan : RUIMonoBehaviour {
	
    public UILabel mCurLevel;
	public UILabel mNextLevel;
	public UILabel mExp;
	public UILabel mMoney;
	public UISlider mExcBar;
    public UILabel mlevelStaticNum;
    public UILabel TitleLv ; //当前等级

	UICenterOnChild mCenter;
	UIScrollView mScrollView;

	public GameObject mGrid;
	public UIVipGiftCell[] m_Temp2;	//scrollview换的对象

	private readonly float m_Width = 1000f;

	GameObject _Lastobj = null;

	int Vipindex;
	int CurVipindex;

	//scrollview换的位置
	Vector3[] m_Temp = new Vector3[3];

	//当前显示的cell
	UIVipGiftCell m_Choose = null;

	UIDragonMallMgr mMallMgr{
		get{
			return UIDragonMallMgr.GetInstance();
		}
	}
	

	void Start()
	{
		SetInit();
	}

	void SetInit()
	{
		Transform trans = mGrid.transform;
		for(int i=0; i<trans.childCount; i++)
		{
			m_Temp[i] = trans.GetChild(i).localPosition;
		}
	}

    public void SetDetail()
	{
		CurVipindex = Core.Data.playerManager.curVipLv;
		Vipindex = Core.Data.playerManager.curVipLv;
		if (Vipindex <= 0 )
        {
			Vipindex = 1;
        }

		if(mCenter == null)mCenter = mGrid.GetComponent<UICenterOnChild>();
		if(mScrollView == null)mScrollView = mGrid.transform.parent.gameObject.GetComponent<UIScrollView>();

        List<VipInfoData> _templist = Core.Data.vipManager.GetVipInfoDataListConfig();
       
		TitleLv.SafeText(Vipindex.ToString());
		mCurLevel.SafeText( CurVipindex.ToString());//当前玩家的等级
		if(CurVipindex >= 15)mNextLevel.SafeText("");
		else
		{ 
			mNextLevel.SafeText((CurVipindex+1).ToString());
		}

		VipInfoData _datanext;
		VipInfoData _datacur;
		_datanext = Core.Data.vipManager.GetVipInfoData(CurVipindex+1);
		_datacur = Core.Data.vipManager.GetVipInfoData(CurVipindex);
		if(_datanext == null && _datacur == null)
		{
			mMoney.SafeText("");
			mExp.SafeText("-/-");
		}
		else if(_datanext == null && _datacur != null)
		{
			mMoney.SafeText("");
            mExp.SafeText((_datacur.rmb *10).ToString() +  "/-");  // *10  =钻石了
        }
		else if(_datanext != null && _datacur == null)
		{
			RED.LogError(CurVipindex.ToString() + "VIP's vipinfo is null");
			mExcBar.value = 100f;
		}
		else
		{
			if(_datanext != null)
			{
				mMoney.SafeText((mMallMgr.m_CurLevelMoney*10).ToString());
				mExp.SafeText((mMallMgr.m_TotalMoney*10).ToString()+ "/" + (_datanext.rmb*10).ToString());			
				float inner = mMallMgr.m_TotalMoney;
				float inner2 = _datanext.rmb;
				mExcBar.value = inner/inner2;
			}
			else
			{
				mMoney.SafeText("0");
				mExp.SafeText((mMallMgr.m_TotalMoney*10).ToString() + "/-");
				mExcBar.value = 100f;
			}

        }
        
	
        setVipLvStone(_templist);
		SetChildInfo();
    }

	void SetChildInfo()
	{
		m_Temp2[0].SetDetail(Core.Data.vipManager.GetVipInfoData(Vipindex-1));
		m_Temp2[1].SetDetail(Core.Data.vipManager.GetVipInfoData(Vipindex));
		m_Temp2[2].SetDetail(Core.Data.vipManager.GetVipInfoData(Vipindex+1));
		m_Temp2[0].SetPanel(0f);
		m_Temp2[2].SetPanel(0f);
	}

    private void setVipLvStone(List<VipInfoData> _templist )
    {
        VipInfoData vipInfoData ;
        int freeLv  ;
        int maxVip = _templist.Count-1 ;
		CurVipindex +=1 ;
		if (CurVipindex >= maxVip)
        {
            vipInfoData = _templist[maxVip] ;
            freeLv = vipInfoData.freeLevel ;
            if (freeLv != 0)
            {

            }else
            {
                mlevelStaticNum.text = Core.Data.stringManager.getString(6127);
            }
        }else
        {
			vipInfoData = _templist[CurVipindex] ;
            freeLv = vipInfoData.freeLevel ;
            if (freeLv != 0)
            {

            }else
            {
                mlevelStaticNum.text  = Core.Data.stringManager.getString(6127);
            }

        }

    }
  
    public  void DestroyUI()
    {
        Destroy(this.gameObject);
      
    }
	void SendRequest()
	{
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.GETVIPGIFT, new GetVipGiftParam(int.Parse(Core.Data.playerManager.PlayerID)));
		
		task.afterCompleted = GetComplete;
		
		task.DispatchToRealHandler ();
	}

	void GetComplete(BaseHttpRequest request, BaseResponse response)
	{
		if(response != null && response.status != BaseResponse.ERROR)
		{
			GetVipGiftResponse res = response as GetVipGiftResponse;
			GetRewardSucUI.OpenUI(res.data.award,Core.Data.stringManager.getString(5047));
			//   Core.Data.ActivityManager.SetDailyGiftState(ActivityManager.vipLibaoType,"2");
			Core.Data.ActivityManager.SetActState(ActivityManager.vipLibaoType,"2");
            UIWXLActivityMainController.Instance.Refresh ();
		}

	}



	void TestRemoveItemRight()
	{
		m_Choose = m_Temp2[2];
		m_Temp2[0].SetDetail(Core.Data.vipManager.GetVipInfoData(Vipindex+1));
		mCenter.onFinished = DelegatePanelSet;
		mCenter.CenterOn(m_Temp2[2].gameObject.transform);

		m_Temp[0] = new Vector3(m_Temp[2].x+m_Width, m_Temp[2].y, m_Temp[2].z);
		m_Temp2[0].gameObject.transform.localPosition = m_Temp[0];
		m_Temp2[1].SetPanel(0f);
		Vector3 _temp = m_Temp[0];
		m_Temp[0] = m_Temp[1];
		m_Temp[1] = m_Temp[2];
		m_Temp[2] = _temp;

		UIVipGiftCell _Temp = m_Temp2[0];
		m_Temp2[0] = m_Temp2[1];
		m_Temp2[1] = m_Temp2[2];
		m_Temp2[2] = _Temp;
	}

	void TestRemoveItemLeft()
	{
		m_Choose = m_Temp2[0];

		m_Temp2[2].SetDetail(Core.Data.vipManager.GetVipInfoData(Vipindex-1));

		mCenter.onFinished = DelegatePanelSet;
		mCenter.CenterOn(m_Temp2[0].transform);

		m_Temp[2] = new Vector3(m_Temp[0].x-m_Width, m_Temp[0].y, m_Temp[0].z);
		m_Temp2[2].gameObject.transform.localPosition = m_Temp[2];
		m_Temp2[1].SetPanel(0f);
		Vector3 _temp = m_Temp[2];
		m_Temp[2] = m_Temp[1];
		m_Temp[1] = m_Temp[0];
		m_Temp[0] = _temp;

		UIVipGiftCell _Temp = m_Temp2[2];
		m_Temp2[2] = m_Temp2[1];
		m_Temp2[1] = m_Temp2[0];
		m_Temp2[0] = _Temp;
	}

	void Right_OnClick()
	{
		if(Vipindex >=15)return;
		Vipindex++;
		TestRemoveItemRight();
		TitleLv.SafeText((Vipindex).ToString());
	}
    
    void Left_OnClick()
	{
		if(Vipindex <= 1)return;
		Vipindex--;
		TestRemoveItemLeft();
		TitleLv.SafeText((Vipindex).ToString());
       
	}
	

	void DelegatePanelSet()
	{
		m_Choose.SetPanel(1f);
	}

    void Back_OnClick()
    {
		Destroy(gameObject);
    }




	void Recharge_OnClick()
	{
		UIDragonMallMgr.GetInstance().SetRechargeMainPanelActive();
		if(UIWXLActivityMainController.Instance != null)
		{
			if(Core.Data.rechargeDataMgr._canGainFirstAward == 1 ||Core.Data.rechargeDataMgr._canGainFirstAward == 0 )
			{
				UIWXLActivityMainController.Instance.OnBtnClick();
			}
		}
        DestroyUI();
	}



	void OnDestroy()
	{
		mCurLevel = null;
		mNextLevel = null;
		mExp = null;
		mMoney = null;
		mExcBar = null;
        TitleLv = null ;
		mCenter = null;
		mScrollView = null;	
		mGrid = null;
        _Lastobj = null;
	}
}