using UnityEngine;
using System.Collections;

public class CRLuo_AttackNUM_FX : MonoBehaviour {
	public bool Start_Key;
	public int Update_Num;
	public float Update_StartTime;
	public float Update_LongTime;
	public bool End_Key;

    private BattleHitMgr MainSystem;
    private float NextTime;

    void Start() {
        if (BattleHitMgr.Instance != null) {
            MainSystem = BattleHitMgr.Instance;

            if (Start_Key) {
                AddNum();
            }

            if (Update_Num == 1 ) {
                Invoke("AddNum", Update_StartTime);
            } else if (Update_Num > 2) {
                NextTime = Update_LongTime / (Update_Num - 1);
                Invoke("NextAddNum", Update_StartTime);
            }

        } else {
			//删除脚本
			Destroy(this);
		}

	}

	//物体销毁运行部分
    void OnDestroy() {
        if (End_Key) {
			AddNum();
		}
	}

    void AddNum() {
        MainSystem.ShowCount();
		MainSystem.showCoin();
	}

    void NextAddNum() {
		AddNum();
		Update_Num -= 1;
        if (Update_Num > 0) {
			Invoke("NextAddNum", NextTime);
		}
	}
}
