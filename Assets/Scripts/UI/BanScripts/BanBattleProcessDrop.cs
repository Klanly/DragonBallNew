using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class BanBattleManager : MonoBehaviourEx {

	//会掉落的所有物品，这里做UI展示
	private DropItem[] dropItemArr;
	//boss掉落的物品
	private List<DropItem> BossDropArr;

	//当前的掉落龙珠索引
	private int curDragonBall;
	//可掉落的龙珠总数
	private int totalDragonBall;

	//分析掉落物品信息
	void analyzeDropItem() {
		curDragonBall = 0;

		if(dropItemArr == null) {

			TemporyData temp = Core.Data.temper;
			if(temp.clientDataResp != null && temp.clientDataResp.data != null && temp.clientDataResp.data.preAward != null) {
				dropItemArr = temp.clientDataResp.data.preAward;

				BossDropArr = new List<DropItem>();

				foreach(DropItem item in dropItemArr) {
					if(item != null) {
						bool isBall = DataCore.getDataType(item.pid) == ConfigDataType.Frag;
						isBall = isBall && item.pid <= 150014;

						if(isBall) {
							totalDragonBall += item.num;
						} else {
							BossDropArr.Add(item);
						}
					}
				}

			} else {
				totalDragonBall = 0;
			}
		}
	}

	//掉落龙珠
	//参数Count影响掉落
	public bool dropOneBall(int Count) {
		bool hasUsed = false;
		if(curDragonBall < totalDragonBall) {
			hasUsed = possbilityDrop(Count);
			if(hasUsed) curDragonBall ++;
		}
		return hasUsed;
	}

	//掉落的概率一半一半
	bool possbilityDrop(int count) {
		bool drop = false;
		float value = Random.value * count;
		drop = value >= 20f;
		return drop;
	}

	/// <summary>
	/// 获取boss掉落龙珠的个数
	/// </summary>
	public int getBossDropBall {
		get {
			int count = totalDragonBall - curDragonBall;
			if(count < 0) count = 0;
			return count;
		}
	}

	/// <summary>
	/// 获取Boss掉落的其他物品个数
	/// </summary>
	public int getBossOtherDrop {
		get {
			if(BossDropArr == null) {
				return 0;
			} else {
				return BossDropArr.Count;
			}
		}
	}
}
