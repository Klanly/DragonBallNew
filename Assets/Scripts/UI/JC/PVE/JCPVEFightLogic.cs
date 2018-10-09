using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class JCPVEFightLogic  {

	private static JCPVEFightLogic  _Instance;
	
	NewFloor floordata = null;
	
	public static JCPVEFightLogic Instance
	{
		get
		{
			if(_Instance == null)
				_Instance = new JCPVEFightLogic();
			return _Instance;
		}
	}
	

    public void Fight(int floorID,int[] array = null,int teamID =1) 
    {
		ComLoading.Open();
		
		//赋值floordata
		Core.Data.newDungeonsManager.FloorList.TryGetValue(floorID, out floordata);

		//added by zhangqiang ao rember level
		if(Core.Data.playerManager.RTData.curTeam.validateMember == 0)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(5031));
			ComLoading.Close();
			return;
		}
		
		Core.Data.temper.mPreLevel = Core.Data.playerManager.RTData.curLevel;
		Core.Data.temper.mPreVipLv = Core.Data.playerManager.RTData.curVipLevel;

        HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		
		//int flag =0 ;
		//检测该关卡是否是这一章节的最后一个关卡
		//bool isLastFloorOfChapter = false;

		task.AppendCommonParam(RequestType.NEW_PVE_BOSSBATTLE, new ClientBattleParam(Core.Data.playerManager.PlayerID, floorID, 0, 0, 0, 0,Core.Data.guideManger.isGuiding ? 1 : 0 ,array,teamID));

        task.ErrorOccured += HttpResp_Error;
        task.afterCompleted += BattleResponseFUC;
        task.DispatchToRealHandler();
    }
	
	
	
	/*点击BOSS关卡<执行>按钮后服务器返回的数据
	 * */
    void BattleResponseFUC (BaseHttpRequest request, BaseResponse response) 
    {
        ComLoading.Close();
        if(response != null)
        {
            TemporyData temp = Core.Data.temper;

            if(response.status!=BaseResponse.ERROR)
            {
                BattleResponse r = response as BattleResponse;

				ClientBattleResponse resp = response as ClientBattleResponse;

				if(r != null) {
					if(r != null && r.data != null && r.data.reward != null && r.data.sync != null) Core.Data.playerManager.RTData.curVipLevel = r.data.sync.vip;

					r.data.battleData.rsty = null;
					r.data.battleData.rsmg = null;
                    temp.warBattle = r.data;

                    temp.currentBattleType = TemporyData.BattleType.BossBattle;

					HttpRequest req = request as HttpRequest;
					BaseRequestParam param = req.ParamMem;
					//BattleResponse res = response as BattleResponse;
					BattleParam bp = param as BattleParam;
					FloorData floorD =	Core.Data.dungeonsManager.getFloorData(bp.doorId);
					if(r.data.battleData.iswin  == 1){
						if(floorD != null)
							Core.Data.ActivityManager.OnMissionComplete(floorD.name);
					}else {
						if(floorD != null)
							Core.Data.ActivityManager.OnMissionFail(floorD.name);
					}
					if(bp.flag == 1){
						//add by wxl 
						Core.Data.ActivityManager.OnPurchaseVirtualCurrency(ActivityManager.ChapterType,1,10);
					}
				} 

				if(resp != null) { 
					
                    temp.currentBattleType = TemporyData.BattleType.BossBattle;
                    temp.clientDataResp = resp;

					#if LOCAL_AUTO
					temp.Open_StepMode = false;
					#else
					temp.Open_StepMode = true;
					#endif
                    temp.Open_LocalWarMode = true;
					
					HttpRequest req = request as HttpRequest;
					if(req != null) {
						ClientBattleParam param = req.ParamMem as ClientBattleParam;
						if(param != null)
                            temp.clientReqParam = param;
					}

				}

				Core.Data.deblockingBuildMgr.mFloorRecord = Core.Data.newDungeonsManager.lastFloorId;

                //跳转至Ban 的场景
                JumpToBattleView();
            }
            else
            {
				if(response.errorCode == 4002)
				JCRestoreEnergyMsg.OpenUI(110015,110016);
				else
                ErrorDeal(response.errorCode);
            }
        }
    }
	
	void JumpToBattleView() 
    {
		Core.Data.newDungeonsManager.curFightingFloor = floordata;
        BattleToUIInfo.From = RUIType.EMViewState.S_CityFloor;

		Core.Data.temper.CitySence = floordata.config.Scence;
		Core.SM.beforeLoadLevel(Application.loadedLevelName, SceneName.GAME_BATTLE);
        AsyncLoadScene.m_Instance.LoadScene(SceneName.GAME_BATTLE);
    }
	
	
	void HttpResp_Error (BaseHttpRequest request, string error) 
    {
        ComLoading.Close();
        ShowMag("Error["+error+"]");
    }

    public void ErrorDeal(int errorID)
    {
       ShowMag(Core.Data.stringManager.getNetworkErrorString(errorID));
    }
	
	void ShowMag(string msg, bool cancel = false)
    {
        UIInformation.GetInstance ().SetInformation (msg, Core.Data.stringManager.getString (5030),null);
	}
	
	
}
