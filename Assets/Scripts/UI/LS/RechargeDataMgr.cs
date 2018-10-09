using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class RechargeDataMgr : Manager
{

	private List<RechargeData> RechargeDataConfig ;

    public PayCountData payData =null;

	public PayCountResponse res;
	
	public bool _IsOpenFirstScene = false;
	
	public int NPCId;
	
	//0是没有状态 1是首充 2为不是首充
	public int _RechageStatus = 0;

	// 1 success
	public int _canGainFirstAward = -1;
	
	public RechargeDataMgr()
	{
		RechargeDataConfig = new List<RechargeData>();

	}

	int maxcount;
	public int Maxcount
	{
		get{
			return RechargeDataConfig.Count;
		}
	}

	public delegate void RechargeCallback();
	public RechargeCallback _RechargeCallback;

	public delegate void RechargeRequestCallback();
	public RechargeRequestCallback _RechargeRequestCallback;

	bool IsFirstRequest = true;

	/// 
	/// 注销功能
	/// 
	public void Unregister() {
		IsFirstRequest = true;
//		_RechageStatus = 0;
		Core.Data.vipManager.IsFirstLogin_Vip = true;
		_canGainFirstAward = -1;
	}
    
	public override bool loadFromConfig () 
	{
		_canGainFirstAward = -1;
		return base.readFromLocalConfigFile<RechargeData> (ConfigType.RechargeInfo, RechargeDataConfig);
	}

	public RechargeData GetRechargeData(int id)
	{
		for(int i=0; i<RechargeDataConfig.Count; i++)
		{
			if(RechargeDataConfig[i].ID == id)
			{
				return RechargeDataConfig[i];
			}
		}
		return null;
	}

	public List<RechargeData> GetShowDataList()
	{
		List<RechargeData> m_Temp = new List<RechargeData>();
		for(int i=0; i<RechargeDataConfig.Count; i++)
		{
			if(RechargeDataConfig[i].Sell == 1)m_Temp.Add(RechargeDataConfig[i]);
		}
		return m_Temp;
	}

	public void SendHttpRequest()
	{
		if(!IsFirstRequest)
		{
			ComLoading.Open();
		}

		GetPayCntParam param = new GetPayCntParam(Core.Data.playerManager.PlayerID);
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		
		task.AppendCommonParam(RequestType.RECHARGECOUNT, param);
		
		task.afterCompleted += testHttpResp_UI;
		
		task.DispatchToRealHandler ();
	}
	
	void testHttpResp_UI(BaseHttpRequest request, BaseResponse response)
	{

		if(response != null && response.status != BaseResponse.ERROR)
		{
			res = response as PayCountResponse;
			Core.Data.rechargeDataMgr.payData = res.data;
//			CheckFirstRecharge();
			if (UIFirstRechargePanel.GetInstance () != null) {
				UIFirstRechargePanel.GetInstance().ShowBtnlabel ();
			}
			if(IsFirstRequest)
			{
				IsFirstRequest = false;
				return;
			}
			else 
			{
				OpenRechargeUI();
			}
//			if(GetGiftPanelController.Instance != null)
//			{
//				GetGiftPanelController.Instance.BtnBack();
//			}
		}
		else if(response != null && response.status == BaseResponse.ERROR)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getNetworkErrorString(response.errorCode));
//			_RechageStatus = 0;
		}
		ComLoading.Close();
	}

	void OpenRechargeUI()
	{
		if(!_IsOpenFirstScene)
		{		
			if(UIDragonMallMgr.GetInstance()._UIDragonRechargeMain == null)
			{
				//首充  开关
				if (LuaTest.Instance.OpenFirstCharge == true) {

					if (_canGainFirstAward == 2) {
						UIDragonMallMgr.GetInstance ().mUIDragonRechargeMain.SetActive (true);
					} else if (_canGainFirstAward == 1 || _canGainFirstAward == 0) {
						if (DBUIController.mDBUIInstance._UIDragonMallMain != null) {
							DBUIController.mDBUIInstance._UIDragonMallMain.SetActive (false);
						}
						DBUIController.mDBUIInstance.HiddenFor3D_UI (false);
						UIFirstRechargePanel.OpenUI ();
						UIFirstRecharge.OpenUI ();
						_IsOpenFirstScene = true;
					}
				} else {
					UIDragonMallMgr.GetInstance ().mUIDragonRechargeMain.SetActive (true);
				} 

			}
			else
			{

				UIDragonMallMgr.GetInstance().mUIDragonRechargeMain.CreateCell(res);
			}
			
		}
		else
		{
			if(UIDragonMallMgr.GetInstance()._UIDragonRechargeMain == null)
			{
				UIDragonMallMgr.GetInstance().mUIDragonRechargeMain.SetActive(true);
			}
			else
			{
				UIDragonMallMgr.GetInstance().mUIDragonRechargeMain.CreateCell(res);
			}
		}
	}
	
//	void CheckFirstRecharge()
//	{
//		if(res != null && res.data != null && res.data.buyCounts != null)
//		{
//			if(res.data.buyCounts.Count == 0)
//			{
//				_RechageStatus = 1;
//				return;
//			}
//		}
//		_RechageStatus = 2;
//	}
	
	public void CloseAll()
	{
		UIFirstRecharge.GetInstance().Close();
		UIFirstRechargePanel.GetInstance().Close();
		if(_RechargeCallback != null)
		{
			_RechargeCallback();
		    _RechargeCallback = null;
			DBUIController.mDBUIInstance.HiddenFor3D_UI();
        }
		else
		{
			DBUIController.mDBUIInstance.ShowFor2D_UI();
        }
        
        _IsOpenFirstScene = false;
	}
}
