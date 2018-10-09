using UnityEngine;
using System.Collections;

public class FightBuildingInBattle : MonoBehaviour {

	// ---- UI ----
	public UILabel AtkNum;
	public UILabel DefNum;
	public UILabel RateNum;
	public GameObject Go_Bg;

	private bool mOpenBuilding;
	private int Atk;
	private int Def;
	private int Rate;

	void Start() {
		TemporyData Temp = Core.Data.temper;
		if(Temp.currentBattleType == TemporyData.BattleType.FightWithFulisa) {
			gameObject.SetActive(false);
			return;
		}

		fightBuildingInfo();
		if(!mOpenBuilding) {
			gameObject.SetActive(false);
		} else {
			Go_Bg.SetActive(false);
		}
	}

	void fightBuildingInfo() {
		BuildingManager buildMgr = Core.Data.BuildingManager;
		Building fightBuilding = buildMgr.GetBuildFromBagByNum(BaseBuildingData.BUILD_BATTLE);

		mOpenBuilding = fightBuilding == null ? false : fightBuilding.RTData.dur > 0;

		if(mOpenBuilding) {
			Atk = (int)fightBuilding.config.GetAtk;
			Def = (int)fightBuilding.config.GetDef;
			Rate= (int)fightBuilding.config.GetRate;

			AtkNum.text = "+" + Atk.ToString() + "%";
			DefNum.text = "+" + Def.ToString() + "%";
			RateNum.text= "+" + Rate.ToString()+ "%";
		}
	}

	void OnHold() {
		Go_Bg.SetActive(true);
	}

	void OnRelease() {
		Go_Bg.SetActive(false);
	}
}
