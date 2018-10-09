using UnityEngine;
using System.Collections;

public class DuoBaoShowCellScript : RUIMonoBehaviour {

    public TrialEnum mTrialEnum;
	public UILabel _Name;
    
    void OnClick()
    {
        if(mTrialEnum == TrialEnum.TrialType_ShaluAndBuou || mTrialEnum == TrialEnum.TrialType_TianXiaDiYi || mTrialEnum == TrialEnum.TrialType_QiangDuoGold)
        {
			FinalTrialMgr.GetInstance().CreateScript(mTrialEnum, QiangduoEnum.QiangduoEnum_List);
        }
		else if (mTrialEnum == TrialEnum.TrialType_QiangDuoDragonBall)
		{
            if (Core.Data.playerManager.Lv < 15)
            {
                SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(37021),null);
                return;
            }
			UIShenLongManager.setShenLongManagerRoot (DBUIController.mDBUIInstance._bottomRoot);
		}
		else
        {
            SQYAlertViewMove.CreateAlertViewMove("This fuction is not open");
        }
    }

	void Start()
	{
		if(mTrialEnum == TrialEnum.TrialType_TianXiaDiYi)_Name.SafeText(Core.Data.stringManager.getString(6092));
		else if(mTrialEnum == TrialEnum.TrialType_QiangDuoDragonBall)_Name.SafeText(Core.Data.stringManager.getString(6045));
		else if(mTrialEnum == TrialEnum.TrialType_ShaluAndBuou)_Name.SafeText(Core.Data.stringManager.getString(25139));
		else if(mTrialEnum == TrialEnum.TrialType_QiangDuoGold)_Name.SafeText(Core.Data.stringManager.getString(25140));
	}

}
