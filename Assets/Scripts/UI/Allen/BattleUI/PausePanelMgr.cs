using UnityEngine;
using System.Collections;

public class PausePanelMgr : MonoBehaviour {

	public UISprite SoundOn;
	public UISprite SoundOff;
	public UILabel  lblOnOff;

	/// <summary>
	/// 主要初始化声音的显示
	/// </summary>
	public void init() {
		UserConfigManager User = Core.Data.usrManager; 
		if(User.UserConfig.mute == 0) { //打开声音的状态
			SoundOn.gameObject.SetActive(true);
			SoundOff.gameObject.SetActive(false);

			lblOnOff.text = Core.Data.stringManager.getString(30);
		} else {
			SoundOn.gameObject.SetActive(false);
			SoundOff.gameObject.SetActive(true);

			lblOnOff.text = Core.Data.stringManager.getString(31);
		}
	}

	/// <summary>
	/// 继续战斗
	/// </summary>
	public void OnContinue() {
		gameObject.SetActive(false);
		///战斗暂停已结束
		TimeMgr.getInstance().WarPause = false;
		TimeMgr.getInstance().revertToBaseLine();
	}

	/// <summary>
	/// 放弃战斗
	/// </summary>
	private bool hasClick = false;
	public void OnGiveUp() {
		if(hasClick) return;
		hasClick = true;
		/// 
		/// 跳过战斗的播放动画
		/// 
		Core.Data.temper.SkipBattle = true;
		BanBattleProcess.Instance.skip = true;
		///
		/// 保存战斗的速度状态
		///
		BanBattleManager battleMgr = BanBattleManager.Instance;
		battleMgr.SpeedOrSkipMgr.SpeedUpAndAutoSave();
		battleMgr.attackSideInfo.CancelInvoke();
		///
		///战斗暂停已结束
		/// 
		TimeMgr.getInstance().WarPause = false;
		TimeMgr.getInstance().setBaseLine(1f);
		StopAllCoroutines();
		AsyncTask.RemoveAllDelayedWork();
		SettleBossBattle();
		updatePlayerLv();
		
		Core.SM.beforeLoadLevel(SceneName.GAME_BATTLE, SceneName.MAINUI);
		AsyncLoadScene.m_Instance.LoadScene(SceneName.MAINUI);
		Core.Data.temper.WarErrorCode = -1;
	}

	/// <summary>
	/// 更新用户等级
	/// </summary>
	void updatePlayerLv() {
		TemporyData.BattleType curbattle = Core.Data.temper.currentBattleType;
		//更新用户等级
		if(curbattle == TemporyData.BattleType.BossBattle || 
			curbattle == TemporyData.BattleType.QiangDuoDragonBallBattle ||
			curbattle == TemporyData.BattleType.QiangDuoGoldBattle || 
			curbattle == TemporyData.BattleType.Revenge || 
			curbattle == TemporyData.BattleType.TianXiaDiYiBattle ||
			curbattle == TemporyData.BattleType.SuDiBattle ) {
			//设定用户的等级
			if (Core.Data.temper.AfterBattleLv > Core.Data.playerManager.RTData.curLevel) {
				Core.Data.playerManager.SetCurUserLevel (Core.Data.temper.AfterBattleLv);
			}
		}
	}

	/// <summary>
	/// 声音开关
	/// </summary>
	public void OnChangeSound() {
		Core.Data.soundManager.SwitchSound(true);

		init();
	}


	#region 提过网络请求

	/// <summary>
	/// 如果战斗还在继续，则需要向网络发起请求,告知服务器客户端失败
	/// </summary>
	void SettleBossBattle() {
		bool WarIsOnGoing = BanBattleManager.Instance.War.WarIsOnGoing;

		TemporyData temp  = Core.Data.temper;
		temp.GiveUpBattle = true;

		if(WarIsOnGoing) {
			ClientBattleCheckParam param = null;
			RequestType ReqType = RequestType.SETTLE_BOSSBATTLE;

			HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Igonre_Response);
			if(temp.currentBattleType == TemporyData.BattleType.BossBattle)
				param = new ClientBattleCheckParam (temp.clientReqParam, null, null, string.Empty, null, null);
			else if(temp.currentBattleType == TemporyData.BattleType.FinalTrialShalu || 
				temp.currentBattleType == TemporyData.BattleType.FinalTrialBuou) {
				param = new ClientBTShaBuParam (temp.shaluBuOuParam, null, null, string.Empty, null, null);
				ReqType = RequestType.SETTLE_SHABU;
			}

			param.sequence.LeftWin = 0;
			task.AppendCommonParam(ReqType, param);

			//then you should dispatch to a real handler
			task.DispatchToRealHandler();
		}

	}


	#endregion

}
