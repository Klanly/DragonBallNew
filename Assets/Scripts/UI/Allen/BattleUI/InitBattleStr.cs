using UnityEngine;
using System.Collections;

public class InitBattleStr : MonoBehaviour {
	public UIButton btnJump;


	//是否可以点击
	public bool CanClick
	{
		get
		{
//			bool val = false;
//			switch(Core.Data.temper.currentBattleType)
//			{
//				case TemporyData.BattleType.BossBattle:
//				case TemporyData.BattleType.PVPVideo:
//					val = true;
//					break;
//				default:
//					val = false;
//					break;
//			}
//
//			return val;
			return true;
		}	
	}

	//能点击，是否可以跳过
	public bool CanSkip
	{
		get
		{
			bool val = true;
//			#if !DEBUG
			if(Core.Data.guideManger.isGuiding) {
				val = false;
			}
				
//			if (Core.Data.playerManager.RTData.curVipLevel >= 3)
//			{
//				return true;
//			}暂时去点VIP限制战斗跳过
            //战斗跳过功能限制改成等级1级 yangchenguang 可能会影响到新手引导
			if (Core.Data.playerManager.RTData.curLevel >= 1)
			{
				return true;
			}

			if (Core.Data.temper.currentBattleType == TemporyData.BattleType.PVPVideo) {
				return true;
			}

            TemporyData.BattleType type = Core.Data.temper.currentBattleType;
			BattleSequence sequence = Core.Data.temper.warBattle;
			if(sequence != null) 
			{
                if(type == TemporyData.BattleType.BossBattle) {
					if(sequence.battleData.isfirst == 1)
                    {
                        if(Core.Data.temper.hasLiquidate)
                            val = true;
                        else
                            val = false;
                    } 
                } else {
                    if(Core.Data.temper.hasLiquidate)
                        val = true;
                    else
                        val = false;
                }
				
			}
//			#endif
			return val;
		}
	}

	void Start() 
	{
		btnJump.TextID = 2001;

		if(CanClick)
		{
			setEnableBtn(CanSkip);
		}
		else
		{
			btnJump.isEnabled = false;
		}

	}

    public void setEnableBtn(bool isEnable) 
	{
		btnJump.UpdateColor(isEnable);
    }
}
