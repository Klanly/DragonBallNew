using UnityEngine;
using System.Collections;

public class DragonBankController : MonoBehaviour {

	private static DragonBankController instance;
	public static DragonBankController Instance
	{
		get
		{
			return instance;
		}
	}

	public UILabel lblSaveMoneyNum;
	public UILabel lblIncreaseRateNum;
	public UILabel lblLeftDaysNum;
	public UILabel lblTotalGetMoneyNum;
	public UILabel lblSaveMoneyTitle;
	public UILabel lblIncreaseRateTitle;
	public UILabel lblLeftDaysTitle;
	public UILabel lblTotalGetMoneyTitle;
	public UILabel lblDesp;

	public GameObject bankTag;
	public JCBuyNumberWidgetForBank numWidget;
	readonly int unitMoney = 1000;
	private BankData myBankData = null;
	public UIButton btnSave;
	public UIButton btnReceive;

	public static DragonBankController CreatDragonBankController(){
		UnityEngine.Object obj = WXLLoadPrefab.GetPrefab(WXLPrefabsName.UIDragonBankPanel);
		if (obj != null) {
			GameObject go = Instantiate(obj) as GameObject;
			DragonBankController fc = go.GetComponent<DragonBankController>();
			Transform goTrans = go.transform;
			go.transform.parent = DBUIController.mDBUIInstance._bottomRoot.transform;
			go.transform.localPosition = Vector3.zero;
			goTrans.localScale = Vector3.one;
			return fc;
		}
		return null;
	}

	void Awake(){
		instance = this;
		ActivityNetController.GetInstance ().SendDragonBankMsg ();
	}


	void Start(){
		lblDesp.text = Core.Data.stringManager.getString (7515);
		bankTag.SetActive (false);
	}

	public void InitData(BankData tData){
		myBankData = tData;
		if (myBankData != null) {

			lblSaveMoneyTitle.text = Core.Data.stringManager.getString(7511);
			lblIncreaseRateTitle.text = Core.Data.stringManager.getString(7512);
			lblLeftDaysTitle.text = Core.Data.stringManager.getString(7513);
			lblTotalGetMoneyTitle.text = Core.Data.stringManager.getString(7514);

			lblSaveMoneyNum.text = myBankData.stone.ToString ();
			lblIncreaseRateNum.text = myBankData.profit * 100 + "%";
			lblLeftDaysNum.text = myBankData.surplusTime + Core.Data.stringManager.getString(25183);
			lblTotalGetMoneyNum.text = myBankData.profit_stone.ToString();
		}
	}

	public void OpenSaveMoney(){
		bankTag.gameObject.SetActive (true);
		numWidget.NumIndex = 0;
		VipInfoData tdata = Core.Data.vipManager.GetVipInfoData(Core.Data.playerManager.curVipLv);
		if (Core.Data.playerManager.Stone >= tdata.bankmax) {
			numWidget.Maxnum = tdata.bankmax /unitMoney;
			numWidget.uislider.numberOfSteps = tdata.bankmax / unitMoney + 1;
		}
		else {
			numWidget.Maxnum = Core.Data.playerManager.Stone / unitMoney;
			numWidget.uislider.numberOfSteps = tdata.bankmax / unitMoney + 1;
		}
		numWidget._unitprice = unitMoney;
		if (Core.Data.playerManager.Stone >= unitMoney) {
			btnSave.isEnabled = true;
		}
		else {
			btnSave.isEnabled = false;
		}
	}

	public void CloseSaveTag(){
		bankTag.gameObject.SetActive (false);
		numWidget.NumIndex = 0;
	}
		

	public void SaveMoney(){
		if (myBankData != null && numWidget != null) {
			if (myBankData.stone == 0) {
				if (numWidget.thisValue * unitMoney > 0) {
					if (numWidget.thisValue * unitMoney < Core.Data.playerManager.Stone) {
						Debug.Log ("cur save money === " + numWidget.thisValue * unitMoney);
						ActivityNetController.GetInstance ().SaveMoneyInBank (numWidget.thisValue * unitMoney);
					}
					CloseSaveTag ();
				} else {
					SQYAlertViewMove.CreateAlertViewMove (string.Format (Core.Data.stringManager.getString (7517), unitMoney));
				}
			} else if(myBankData.stone >0) {
				SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (7520));
			}
		}
	}

	public void ReceiveMoney(){
		if (myBankData != null) {
			if (myBankData.stone == 0) {
				SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (7516));
			} else if (myBankData.stone > 0) {
				if (myBankData.surplusTime > 0 && myBankData.profit_stone > 0)
					UIInformation.GetInstance ().SetInformation (Core.Data.stringManager.getString (7519), Core.Data.stringManager.getString (5030), ForceReceive);
				else
					ForceReceive ();
			}
		}
	}

	void ForceReceive(){
		ActivityNetController.GetInstance ().ReceiveMoneyInBank ();
	}

	public void BtnBack(){
		DBUIController.mDBUIInstance.ShowFor2D_UI ();
		Destroy (gameObject);
	}



}
