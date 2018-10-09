using UnityEngine;
using System.Collections;

public class MovieCG : MonoBehaviour {
	public CG_Anim cgAnim;

	private const float DELAY_TIME = 0.05f;
	private const string CG_MOVIE = "CG.mp4";
	private const float DELAY_TIME1 = 11f;
	public GameObject CG_Bike;
	// Use this for initialization
	void Start ()
	{
#if NewGuide
		CG_Bike.SetActive(false);
		NewJCCreateRole.OpenUI ().OnCreateSucess = () =>
		{
			//---------------------------------------
			//---------------片头-----------------
			//---------------------------------------
			if(!CG_Bike.activeSelf) 
			    CG_Bike.SetActive(true);
			play();
		};
#else
		play();
#endif


	}

	void play() {
		#if !UNITY_EDITOR
		Handheld.PlayFullScreenMovie(CG_MOVIE, Color.black, FullScreenMovieControlMode.CancelOnInput);
		#endif
		//Unity doesn't freeze immediately sometimes
		Invoke("OnMovieEnd", DELAY_TIME);
	}

	//Unity doesn't freeze immediately sometimes
	void OnMovieEnd(){
		StartCoroutine(cgAnim.PlayAnim());
		Core.Data.soundManager.BGMPlay(SceneBGM.BGM_CG_BiKe);
		Invoke("SendBattleRequest", DELAY_TIME1);
	}

	void SendBattleRequest() {
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.FIGHT_FULISA, new BattleParam(Core.Data.playerManager.PlayerID, -1, 0, 0));
		task.afterCompleted = BattleResponseFUC;
		task.DispatchToRealHandler();
	}

	void JumpToBattleView() {
		BattleToUIInfo.From = RUIType.EMViewState.MainView;
		Core.Data.temper.CitySence = 4;
		Core.SM.beforeLoadLevel(Application.loadedLevelName, SceneName.GAME_BATTLE);
		AsyncLoadScene.m_Instance.LoadScene(SceneName.GAME_BATTLE);
	}

	void BattleResponseFUC (BaseHttpRequest request, BaseResponse response) {
		if(response != null) {
			if(response.status!=BaseResponse.ERROR) {
				BattleResponse r = response as BattleResponse;
				if(r != null && r.data != null && r.data.reward != null) Core.Data.playerManager.RTData.curVipLevel = r.data.sync.vip;

				r.data.battleData.rsty = null;
				r.data.battleData.rsmg = null;
				Core.Data.temper.warBattle = r.data;
				Core.Data.temper.currentBattleType = TemporyData.BattleType.FightWithFulisa;
				//跳转至Ban 的场景
				JumpToBattleView();
			} else {
				RED.LogError (response.errorCode.ToString());
			}
		}
	}

}