using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonListener : MonoBehaviour {

	public UISprite SpeedUp;
	public UILabel  SpeedTips;
	public UISprite AutoBat;
	//每次加速的量
    private float deltaSpeed = 0.5f;

	///
	/// 战斗的初始速度 
	/// 
	[HideInInspector]
	#if VS
	public const float BaseSpeed = 1.2F;
	#else
	public const float BaseSpeed = 1.3F;
	#endif


	private int curLoop = 0;

	/// 
	/// 是否开放VIP等级
	/// 
	private bool OpenSpeedUp = false;

	void Awake() {
		SpeedTips.gameObject.SetActive(false);
		SetVipSpeedUp();
		SetVipAuto();
	}

	#region 战斗加速功能

	void SpeedUpClick() {

		if(OpenSpeedUp) {
			//当前的速度
			float curFactor = 1.0f;
			if(curLoop == 0) {
				curFactor = BaseSpeed + deltaSpeed;
				curLoop   = 1;
			} else {
				curFactor = BaseSpeed;
				curLoop   = 0;
			}

			if(curLoop == 0) SpeedUp.GetComponent<UIButton>().normalSprite= "zhandoujiasu";
			else SpeedUp.GetComponent<UIButton>().normalSprite = "zhandoujiasu2";

			//设定基线
			TimeMgr.getInstance().setBaseLine(curFactor);

			StartCoroutine(FadeIn());
		} else {
			StartCoroutine(VipOpenSpeedUp());
		}
	}

	void SetVipSpeedUp() {
		int vipLv = 0; 

		PlayerManager playerMgr = Core.Data.playerManager;
		VipManager vipMgr       = Core.Data.vipManager;

		if(playerMgr != null && vipMgr != null) {
			vipLv = playerMgr.RTData.curVipLevel;
			VipInfoData vipdata = vipMgr.GetVipInfoData(vipLv);
			if(vipdata != null) {
				OpenSpeedUp = vipdata.speedup == (short)1;
			} else {
				OpenSpeedUp = false;
			}
		}
	}

	/// 
	/// 目前是vip 1级开启加速功能
	/// 
	IEnumerator VipOpenSpeedUp() {
		StopCoroutine("FadeOut");
		SpeedTips.gameObject.SetActive(true);
		SpeedTips.text  = Core.Data.stringManager.getString(50);
		SpeedTips.color = new Color(1f, 1f, 1f, 0f);
		TweenColor.Begin(SpeedTips.gameObject, 0.1f, Color.white);
		yield return new WaitForSeconds(1f);
		StartCoroutine("FadeOut");
	}

	/// <summary>
	/// 按钮事件
	/// </summary>
	public void OnAddSpeedButtonClick() {
		//播放新手引导的时候，要不能操控
		if(Core.Data.guideManger.isGuiding)
			return;

		SpeedUpClick();
	}

	/// <summary>
	/// 战斗结算的时候保存速度的状态
	/// </summary>
	public void SaveSpeedUp() {
		AccountConfigManager accMgr = Core.Data.AccountMgr;
		if(curLoop == 0) accMgr.UserConfig.SpeedUp = 0;
		else accMgr.UserConfig.SpeedUp = 1;

		accMgr.save();
	}

	IEnumerator FadeIn() {
		StopCoroutine("FadeOut");
		SpeedTips.gameObject.SetActive(true);
		int wordId      = curLoop == 1 ? 46 : 47;
		SpeedTips.text  = Core.Data.stringManager.getString(wordId);
		SpeedTips.color = new Color(1f, 1f, 1f, 0f);
		TweenColor.Begin(SpeedTips.gameObject, 0.1f, Color.white);
		yield return new WaitForSeconds(1f);
		StartCoroutine("FadeOut");
	}

	IEnumerator FadeOut() {
		TweenColor.Begin(SpeedTips.gameObject, 0.1f, new Color(1f, 1f, 1f, 0f));
		yield return new WaitForSeconds(0.1f);
		SpeedTips.gameObject.SetActive(false);
	}

	//重置
	public void ResetSpeedUp() {
		curLoop = 0;
		SpeedUp.GetComponent<UIButton>().normalSprite = "zhandoujiasu";
		//设定基线
		TimeMgr.getInstance().setBaseLine(1F);
	}

	#endregion

	/// 
	/// 两个同时保存, 得要根据战斗类型类判定
	/// 
	public void SpeedUpAndAutoSave() {
		TemporyData temp = Core.Data.temper;

		if(temp.currentBattleType == TemporyData.BattleType.BossBattle || temp.currentBattleType == TemporyData.BattleType.FinalTrialBuou 
			|| temp.currentBattleType == TemporyData.BattleType.FinalTrialShalu) {

			AccountConfigManager accMgr = Core.Data.AccountMgr;

			if(curLoopAuto == 0) accMgr.UserConfig.AutoBat = 0;
			else accMgr.UserConfig.AutoBat = 1;

			if(curLoop == 0) accMgr.UserConfig.SpeedUp = 0;
			else accMgr.UserConfig.SpeedUp = 1;

			accMgr.save();
		}
	}

	#region 自动战斗

	private int curLoopAuto = 0;

	/// 
	/// 判定vip资格
	/// 
	private bool isAutoBattle = false;

	void AutoClick() {
		if(isAutoBattle) {
			if(curLoopAuto == 0) {
				curLoopAuto   = 1;
			} else {
				curLoopAuto   = 0;
			}

			if(curLoopAuto == 1) {
				AutoBat.spriteName = "zidong";
                AutoBat.GetComponent<UIButton>().normalSprite = "zidong"; // yangcg
			} else {
				AutoBat.spriteName = "shoudong";
                AutoBat.GetComponent<UIButton>().normalSprite = "shoudong";// yangcg
			}
			AccountConfigManager accMgr = Core.Data.AccountMgr;
			if(curLoopAuto == 0) accMgr.UserConfig.AutoBat = 0;
			else accMgr.UserConfig.AutoBat = 1;

			StartCoroutine(FadeInAuto());
		} else {
			StartCoroutine(VipOpenAuto());
		}
	}

	/// 
	/// 自动战斗的按钮事件
	/// 
	public void OnAutoButtonClick() {
		//播放新手引导的时候，要不能操控
		if(Core.Data.guideManger.isGuiding)
			return;
		AutoClick();
	}

	public void SaveAuto() {
		AccountConfigManager accMgr = Core.Data.AccountMgr;

		if(curLoopAuto == 0) accMgr.UserConfig.AutoBat = 0;
		else accMgr.UserConfig.AutoBat = 1;

		accMgr.save();
	}

	void SetVipAuto() {
		int vipLv = 0; 

		PlayerManager playerMgr = Core.Data.playerManager;
		VipManager vipMgr       = Core.Data.vipManager;

		if(playerMgr != null && vipMgr != null) {
			vipLv = playerMgr.RTData.curVipLevel;
			VipInfoData vipdata = vipMgr.GetVipInfoData(vipLv);
			if(vipdata != null) {
				isAutoBattle = true;//vipdata.speedup == (short)1;
			} else {
				isAutoBattle = false;
			}
		}
	}

	/// <summary>
	/// 目前是自动战斗不受VIP限定
	/// </summary>
	IEnumerator VipOpenAuto() {
		StopCoroutine("FadeOut");
		SpeedTips.gameObject.SetActive(true);
		SpeedTips.text  = Core.Data.stringManager.getString(51);
		SpeedTips.color = new Color(1f, 1f, 1f, 0f);
		TweenColor.Begin(SpeedTips.gameObject, 0.1f, Color.white);
		yield return new WaitForSeconds(1f);
		StartCoroutine("FadeOut");
	}


	IEnumerator FadeInAuto() {
		StopCoroutine("FadeOut");
		SpeedTips.gameObject.SetActive(true);
		int wordId      = curLoopAuto == 1 ? 52 : 53;
		SpeedTips.text  = Core.Data.stringManager.getString(wordId);
		SpeedTips.color = new Color(1f, 1f, 1f, 0f);
		TweenColor.Begin(SpeedTips.gameObject, 0.1f, Color.white);
		yield return new WaitForSeconds(1f);
		StartCoroutine("FadeOut");
	}
	#endregion

	/// 
	/// 跳过战斗
	/// 
	public void OnSkipButtonClick()
	{
		Core.Data.soundManager.BtnPlay(ButtonType.Confirm);
        //播放新手引导的时候，要不能操控
        if(Core.Data.guideManger.isGuiding)
            return;

		if(!BanBattleManager.Instance.skipBtn.CanSkip)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(5182), UICamera.currentCamera.gameObject);
			return;
		}

		///
		/// ---- 必须不在调用协程 -----
		///
		BanBattleProcess.Instance.CancelInvoke("UpdateHpIfOverSkillAnim");

		//如果可以跳过，则应该让神龙的信息重置
		BanBattleManager.Instance.AoyiMgr.init();

        Core.Data.temper.SkipBattle = true;
		//跳过战斗的播放动画
		BanBattleProcess.Instance.skip = true;

		//added by zhangqiang 
		//发送gps组队战斗结束的消息
		if(Core.Data.temper.currentBattleType == TemporyData.BattleType.GPSWar)
		{
			GPSRewardUI.OpenUI ();
		}
		else
		{
        //add by wxl  增加赌博 结果展示过程；
//        if (Core.Data.temper.gambleTypeId != -1) {
//            BanBattleManager.Instance.ShowGambleResult ();
//        } else {
            BanBattleManager.Instance.ShowCalculate();
			if(!BanBattleManager.Instance.battleWin) 
			{
				if(Core.Data.AccountMgr.UserConfig.guidefirst == 0) 
				{
					if(Core.Data.guideManger.TriggerHideGuide(1000000)) 
					{
						Core.Data.AccountMgr.UserConfig.guidefirst = 1;
						Core.Data.AccountMgr.save();
					}
				}
			}
        }
	}

    /// <summary>
    /// 放弃战斗
    /// </summary>
    public void OnGiveUpClick() {
        string content = Core.Data.stringManager.getString(28);
        string btnText = Core.Data.stringManager.getString(5030);
        //string btnText = Core.Data.stringManager.getString(17);
        UIInformation.GetInstance().SetInformation(content, btnText, BackToGameUI);
    }

    void BackToGameUI() {
        Core.Data.temper.SkipBattle = true;

        Core.SM.beforeLoadLevel(SceneName.GAME_BATTLE, SceneName.MAINUI);
        AsyncLoadScene.m_Instance.LoadScene(SceneName.MAINUI);
        FinalTrialMgr.GetInstance().jumpTo = TrialEnum.None;
        BattleToUIInfo.From = RUIType.EMViewState.MainView;
    }

}
