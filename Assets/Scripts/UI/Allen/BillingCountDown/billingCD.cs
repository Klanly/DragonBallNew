using UnityEngine;
using System.Collections;
using System;

public class billingCD : MonoBehaviour {
	private UILabel txtCD;
	private long howMany;

	private const int ONEDAY = 86400;
	private const int ONEHOUR = 3600;
	private const int ONEMINI  = 60;
	private const int MINISECOND = 1000;

	private bool hasPurchase = false;

	public static billingCD BillCountDown;
	// Use this for initialization
	void Start () {
		BillCountDown = this;
		txtCD = gameObject.GetComponent<UILabel>();
	}

	void init() {

		///
		/// status == -1 代表网络后至
		///

		int status = Core.Data.temper.PurStatus;

		if(status != -1) {
			NetworkCallBack(status);
		} 
	}

	void OnDisable() {
		CancelInvoke();
	}

	void OnEnable() {
		Invoke("init", 1f);
	}

	/// 
	/// 1  已充值  未领取
	/// 2  已充值  已领取
	/// 0  未充值 

	public void NetworkCallBack(int status) {
		ConsoleEx.DebugLog("status = " + status, ConsoleEx.YELLOW);
		if(status == 0) {
			PlayerManager player = Core.Data.playerManager;
			howMany = player == null ? 0 : player.FirstPurchaseReward;
			if(txtCD != null && howMany > 0) {
				txtCD.gameObject.SetActive(true);
				CancelInvoke();
				InvokeRepeating("CountDown", 0f ,1f);
			} else {
				hideUI();
			}
		} else if(status == 1) {
			//已充值  未领取
			txtCD.gameObject.SetActive(false);
			CancelInvoke();
		} else {
			CancelInvoke();
		}

	}

	void hideUI () {
		if(Core.Data.rechargeDataMgr != null) {
			Core.Data.rechargeDataMgr._canGainFirstAward = 2;
			if(SQYMainController.mInstance != null)
				SQYMainController.mInstance.ArrangeLeftBtnPos();
		}
	}

	void CountDown() {
		howMany -= 1;
		if(howMany > 0) {
			txtCD.text = format(howMany);
		} else {
			hideUI();
			CancelInvoke();
		}
	}

	/// 
	/// 转化为 HH:MM:SS的时间格式
	/// 
	string format(long unix) {
		int tick = (int)unix;
		int day  = tick / ONEDAY;
		int hour = (tick % ONEDAY) / ONEHOUR;
		int min  = ((tick % ONEDAY) % ONEHOUR) / ONEMINI;
		int sec  = tick % ONEMINI;

		if(day > 0) hour = day * 24 + hour;

		string formatted = string.Format("{0:D2}:{1:D2}:{2:D2}", hour, min, sec);

		return formatted;
	}

}
