using System;
using UnityEngine;

public class NewBattleListener : EventReceiver {
    public Action<int> Open;
    public Action Exit;

    protected override void OnEvent(EventTypeDefine p_e,object p_param) {
		ConsoleEx.DebugLog("EventType = " + p_e.ToString(), ConsoleEx.YELLOW);
		BanBattleManager battleMgr = BanBattleManager.Instance;
		BanSideInfo      attside   = battleMgr != null ? battleMgr.attackSideInfo : null;
		GuideManager     guideMgr  = Core.Data.guideManger;
#if NewGuide
		NewUIGuide guideUI   =  NewUIGuide.Instance;
#else
		UIGuide          guideUI   = UIGuide.Instance;
#endif
		TemporyData      temp      = Core.Data.temper;

        switch(p_e)
        {
        //打开第一个宝箱
        case EventTypeDefine.OpenTreasureChest:
            if(Open != null) {
                GuideData data = p_param as GuideData;
                if(data != null)
                {
                    Open(data.MultiIndex);
					Core.Data.guideManger.DelayAutoRun(1.5f);
                }
              
            }
            break;
        //退出战斗
        case EventTypeDefine.ExitFightingScene:
            if(Exit != null) {

                AsyncTask.QueueOnMainThread(
                    () => {                        
                      Exit();
					  #region Add by jc 如果没有升级并且已经是最后一步引导了
						if( Core.Data.playerManager.Lv <= temp.mPreLevel && temp.mPreLevel > 0 && !guideMgr.isLastOfCurGuide)
						{
							guideUI.HideGuide();
							Core.Data.guideManger.DelayAutoRun(2.5f);
						}
					  #endregion
                    }
                    , 0.5f
                );

            }
            break;
		case EventTypeDefine.Click_XiaoWuKongVBiKe:               //显示双方队伍
		case EventTypeDefine.Click_WuKong2_Anger:                 //孙悟空2继承小悟空20怒气
		case EventTypeDefine.Click_BILUSI_V_WuKong3:              //比鲁斯
		case EventTypeDefine.Click_WuKong3_NO_ANGER:              //没有怒气值不能释放主动技能
		case EventTypeDefine.Click_WuKong3_Vs_BiLu:               //孙悟空3对决比鲁斯
			guideUI.HideGuide();
			Time.timeScale = 1.0f;
			break;
#region 怒气技的起始阶段
		case EventTypeDefine.Click_XiaoWuKong_OS_4:{
				Time.timeScale = 1.0f;
				SkillData sd = Core.Data.skillManager.getSkillDataConfig(25037); 
				attside.PlayerAngryWord.FeatureWarShow(1, sd.name,      ()=> { Time.timeScale = 0f;} );
			}
			break;
		case EventTypeDefine.Click_WuKong2_OS_4:{
				Time.timeScale = 1.0f;
				SkillData sd = Core.Data.skillManager.getSkillDataConfig(25008); 
				attside.PlayerAngryWord.FeatureWarShow(1, sd.name,      ()=> { Time.timeScale = 0f;} );
			}
			break;
		case EventTypeDefine.Click_WuKong3_OS_4:{
				Time.timeScale = 1.0f;
				SkillData sd = Core.Data.skillManager.getSkillDataConfig(25058); 
				attside.PlayerAngryWord.FeatureWarShow(1, sd.name,      ()=> { Time.timeScale = 0f;} );
			}
			break;
#endregion

		case EventTypeDefine.Click_XiaoWuKong_OS_4_2:
			Time.timeScale = 1.0f;
			attside.PlayerAngryWord.FeatureWarShow(2, string.Empty, ()=> { Time.timeScale = 0f;} );
			break;
		case EventTypeDefine.Click_XiaoWuKong_OS_4_3:
			Time.timeScale = 1.0f;
			attside.PlayerAngryWord.FeatureWarShow(3, string.Empty, ()=> { Time.timeScale = 0f;} );
			break;
		case EventTypeDefine.Click_XiaoWuKong_OS_4_4:
			Time.timeScale = 1.0f;
			guideUI.HideGuide();
			AsyncTask.RemoveAllDelayedWork(); 
			attside.PlayerAngryWord.FeatureWarShow(4, string.Empty, ()=> { battleMgr.AngryUI(false, null, -1);});

			long now = Core.TimerEng.curTime;
			TimerTask task = new TimerTask(now, now + 2, 1, ThreadType.MainThread);
			task.onEventEnd = (t) => { 
				Time.timeScale = 1.0f; 
				AsyncTask.RemoveAllDelayedWork(); 
				battleMgr.AngryUI(false, null, -1);
			};
			task.DispatchToRealHandler();
			break;
		case EventTypeDefine.Click_XiaoWuKong_Lv1: {
				Time.timeScale = 1.0f;
				guideUI.HideGuide();
				AsyncTask.RemoveAllDelayedWork();

				attside.PlayerAngryWord.FeatureWarShow(1, string.Empty, ()=> { battleMgr.AngryUI(false, null, -1);});
				attside.angrySlot.curAP = 0;
				battleMgr.Root.Camera1.eventReceiverMask = LayerMask.NameToLayer("Everything");
			}
			break;
		//调用武者强化
		case EventTypeDefine.Click_JumpToStrengthening:
			if(Exit != null) {
				UIGuide.Instance.HideGuide();

				Core.Data.guideManger.DelayAutoRun(2f);
				// --- 武者强化 ---
				FinalTrialMgr.GetInstance().jumpTo = TrialEnum.None;
				BattleToUIInfo.From = RUIType.EMViewState.S_Team_NoSelect;
				Exit();
			}

			break;
        }


    }
}