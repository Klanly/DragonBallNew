using UnityEngine;
using System;
using fastJSON;
using System.Collections;


/// <summary>
/// 原来是用来测试战斗退出后再次战斗的逻辑
/// 
/// 现在还用来测试注销功能
/// </summary>
public class QuickJumpToBattle : MonoBehaviour {

	string battleSequence = @"{""status"":1,""act"":103,""data"":{""sync"":{""eny"":219,""pwr"":""10"",""stone"":""9982361"",""rank"":-1,""ep"":105,""lv"":""62"",""vip"":""3""},""battleData"":{""isfirst"":0,""ak"":841,""df"":5877,""rsmg"":""rVZNc9owEP0vOpOMVtaH4VQm0wOnHppLJ5ODC4IycW3GFmkzTP57JWHQyuCP0NxYSd63b\/c9iQPJjHnU2W8yO5Cs2FRvD4UhMzohxi8+Hch2RWYslZJPSLG3S0CB2\/1dVe50Zd7ITExIrl91brfske8v29z+fGIAQj1PSHZeEFRSu7DcV3NjMSBJbZZXvShW+q+HzPJ8nRlt8ej75AgMKlUyADOBgfkZOBExMpMTBpTSFj5LEL6iCcaHGN8erMpcL2wgFJeOsXnbaV8aWen1cM9oKBsULhtCv+KG0QRaBYNCBcuU4YLZ1YYh1DRqFguoMm6WEhgVd0hSDJgMAQLHgMkZkMd4MG2z5JjlNJIFHwBldDpOEkp2sHR6xohiiCZPOtQf95Uq1pY\/5pmKaJpyEFVeRxUtDfXwjA2nhjrLOvTDW52FpAMxTSKHpZ0OO5sLrLnshTQvf2xdbmUt7JJbvzVLNqhNZvY1md2B2\/i53yyKdem23r8c3MfZ8kVXPqf9TBe1Plql2XjAt09z4AEbrLkP587XpyzxwqkA2oP\/tVj+siD3IM4oxyV672x5UY24rEaoacBiV7Dq\/XrdMLXHKhMyrbdFlofwNA9Uu+jJx8bm4wz6M4ZpQP80\/F3c4u+zx9OA\/5tGaxTwgTrGzCHuG0yjvrnw1LeE2qOVXpavugpElmVtAs9dpdGW9cymbHZqZzTnGv+gNfG3nbdsEz0eX6ojIspSm2YWJzqcQg8hwIR80YGQD89CoHCDtPoy0luElVwOtCEYDdQ\/awM2h08WVmcdY4TleK1W\/pu7KWqSlCnWBsPacP9+kDaSSBkwRhlsrDI8tzBHH37oyplXlf\/XhPiG7NK9ziG7D0\/ZBW\/7qNjneWSl40JwU3PguqH8e4MdJaK2Mdw2GGuoZwynboXrmpLq182nTOnPtihcPidgEo5eQP8D"",""rsty"":""i9Y11DHQMdIxBUIQbWJgiMQGiZsYmANJ81gA"",""iswin"":2},""reward"":{""bep"":96,""bco"":3600,""eep"":0,""eco"":0,""np"":null,""p"":null},""ext"":{""zg"":0,""coin"":0,""p"":null},""doorStatus"":{""free"":0,""vipfree"":-1}}}";

    void StartTest() {

        try {
            BattleResponse response = JSON.Instance.ToObject(battleSequence, typeof(BattleResponse)) as BattleResponse;

            if(response != null) {
                response.handleResponse();
            }

            Core.Data.temper.warBattle = response.data;
            Core.Data.temper.currentBattleType = TemporyData.BattleType.BossBattle;

            Core.SM.beforeLoadLevel(Application.loadedLevelName, SceneName.GAME_BATTLE);
            AsyncLoadScene.m_Instance.LoadScene(SceneName.GAME_BATTLE);

        } catch(Exception ex) {
            ConsoleEx.DebugLog(ex.ToString());
        } 

    }

	void Unregister() {
		if(Core.SM.CurScenesName == SceneName.MAINUI) {
			Core.SM.beforeLoadLevel(Application.loadedLevelName, SceneName.LOGIN_SCENE);
			AsyncLoadScene.m_Instance.LoadScene(SceneName.LOGIN_SCENE);

			Core.SM.OnUnregister();
		}
	}

    void OnGUI () {
        if(GUI.Button(new Rect(100,100, 100,100), "Unregister")) {
			Unregister();
        }
    }

}
