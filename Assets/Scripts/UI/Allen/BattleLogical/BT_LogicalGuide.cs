using UnityEngine;
using System.Collections;

/// <summary>
/// 战斗的核心逻辑，这个partial 类用来判定新手引导
/// </summary>
namespace AW.Battle {
	public partial class BT_Logical 
	{
		
		/// <summary>
		/// 是特殊的新手引导吗？ --------- 已经不再使用,服务器来保存这个值
		/// </summary>
		/// winOrLose 战斗的输赢, 1 左边赢 0 右边赢
		bool Assignment(int winOrLose) {
			bool isSpecial = false;

			TemporyData temp = Core.Data.temper;
			AccountConfigManager AccMgr = Core.Data.AccountMgr;

			ClientBattleParam param = temp.clientReqParam;
			if(temp.currentBattleType == TemporyData.BattleType.BossBattle && param != null) {
				int floorId = param.doorId;
				if(floorId == 60104) { //60104副本第一次打

					if(!TeamUI.secondPosUnLock)
					{		
						AccMgr.UserConfig.SpecialGuideID = 1000;
						AccMgr.save();
					}
				}

				if(floorId == 60109) { //60109副本第一次打，必须失败
					isSpecial = AccMgr.UserConfig.FB_60109 == 0;

					if(isSpecial) {
						AccMgr.UserConfig.FB_60109 = 1;
						AccMgr.UserConfig.SpecialGuideID = isSpecial ? 2000 : 0;
						AccMgr.save();
					}
				}

				//是技能副本吗
				if(floorId / 100 == 301) {
					isSpecial = temp.warBattle.battleData.isfirst == 1;

					if(isSpecial) {
						AccMgr.UserConfig.SpecialGuideID = isSpecial ? 3000 : 0;
						AccMgr.save();
					}
				}

			}

			return isSpecial;
		}
	}
}

