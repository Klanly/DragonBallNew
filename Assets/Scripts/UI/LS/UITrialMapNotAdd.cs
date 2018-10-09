using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UITrialMapNotAdd : RUIMonoBehaviour 
{
	
	public UILabel _CurrentDoungen;
	public UILabel _StarNum;
	public UILabel _Surplus;

	public UILabel[] _AddAttrArray;
	public UILabel[] _StarArray;
	public UILabel[] _Labelname;

	List<int> _index = new List<int>();

	List<string> Marklist = new List<string>();
	

	public void OnShow(NewFinalTrialAddBufferInfo res)
	{
		if(res == null)
		{
			ConsoleEx.DebugLog("NewFinalTrialFightResponse is null");
			return;
		}
		_CurrentDoungen.text = FinalTrialMgr.GetInstance()._FinalTrialData.CurDungeon.ToString();
		_StarNum.text = res.totalScore.ToString();
		_Surplus.text = (res.totalScore - res.costScore).ToString();

		_Labelname[0].text = GetNameStr(res.bufferList[0].name);
		_Labelname[1].text = GetNameStr(res.bufferList[1].name);
		_Labelname[2].text = GetNameStr(res.bufferList[2].name);

		_StarArray[0].text = res.bufferList[0].needStar.ToString();
		_StarArray[1].text = res.bufferList[1].needStar.ToString();
		_StarArray[2].text = res.bufferList[2].needStar.ToString();

		_AddAttrArray[0].text = Marklist[0] + res.bufferList[0].buffer.ToString() + "%";
		_AddAttrArray[1].text = Marklist[1] + res.bufferList[1].buffer.ToString()+ "%";
		_AddAttrArray[2].text = Marklist[2] + res.bufferList[2].buffer.ToString()+ "%";
	}

	//0ak 1df 2sf 3eak 4edf 5esf
	string GetNameStr(string str)
	{
		switch(str)
		{
			case "ak":
				_index.Add(1);
				Marklist.Add("+");
				return Core.Data.stringManager.getString(25001);
			case "df":
				_index.Add(2);
				Marklist.Add("+");
				return Core.Data.stringManager.getString(25044);
			case "sf":
				_index.Add(3);
				Marklist.Add("+");
				return Core.Data.stringManager.getString(25046);
			case "eak":
				_index.Add(4);
				Marklist.Add("-");
				return Core.Data.stringManager.getString(25000);
			case "edf":
				_index.Add(5);
				Marklist.Add("-");
				return Core.Data.stringManager.getString(25043);
			case "esf":
				_index.Add(6);
				Marklist.Add("-");
				return Core.Data.stringManager.getString(25045);
		}
		return null;
	}

	void BtnClick1()
	{
		if(!CheckStar(0))
		{
			SQYAlertViewMove.CreateAlertViewMove( Core.Data.stringManager.getString(20080));
			return;
		}
//		if(FinalTrialMgr.GetInstance().NowDougoenType == FinalTrialDougoenType.FinalTrialDougoenType_AwardAndAddattr)
//			FinalTrialMgr.GetInstance().NowDougoenType = FinalTrialDougoenType.FinalTrialDougoenType_Award;
//		FinalTrialMgr.GetInstance().NewFinalTrialMapRequest(0);
		Destroy(gameObject);
	}

	void BtnClick2()
	{
		if(!CheckStar(1))
		{
			SQYAlertViewMove.CreateAlertViewMove( Core.Data.stringManager.getString(20080));
			return;
		}
//		if(FinalTrialMgr.GetInstance().NowDougoenType == FinalTrialDougoenType.FinalTrialDougoenType_AwardAndAddattr)
//			FinalTrialMgr.GetInstance().NowDougoenType = FinalTrialDougoenType.FinalTrialDougoenType_Award;
//		FinalTrialMgr.GetInstance().NewFinalTrialMapRequest(1);
		Destroy(gameObject);
	}

	void BtnClick3()
	{
		if(!CheckStar(2))
		{
			SQYAlertViewMove.CreateAlertViewMove( Core.Data.stringManager.getString(20080));
			return;
		}
//		if(FinalTrialMgr.GetInstance().NowDougoenType == FinalTrialDougoenType.FinalTrialDougoenType_AwardAndAddattr)
//			FinalTrialMgr.GetInstance().NowDougoenType = FinalTrialDougoenType.FinalTrialDougoenType_Award;
//		FinalTrialMgr.GetInstance().NewFinalTrialMapRequest(2);
		Destroy(gameObject);
	}

	bool CheckStar(int index)
	{
		if(FinalTrialMgr.GetInstance()._buffer.bufferList[index].needStar > (FinalTrialMgr.GetInstance()._buffer.totalScore - FinalTrialMgr.GetInstance()._buffer.costScore))return false;
		else
		{
			return true;
		}
	}

	void BackOnclick()
	{
//		FinalTrialMgr.GetInstance().NewFinalTrialMapNotAddRequest();
		Destroy(gameObject);
	}

	void OnDestroy()
	{
		_CurrentDoungen = null;
		_StarNum = null;
		_Surplus = null;
		_AddAttrArray[0] = null;
		_AddAttrArray[1] = null;
		_AddAttrArray[2] = null;
		_StarArray[0] = null;
		_StarArray[1] = null;
		_StarArray[2] = null;
		_AddAttrArray[0] = null;
		_AddAttrArray[1] = null;
		_AddAttrArray[2] = null;
	}


}
