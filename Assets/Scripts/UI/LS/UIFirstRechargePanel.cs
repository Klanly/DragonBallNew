using UnityEngine;
using System.Collections;
using System;

public class UIFirstRechargePanel : RUIMonoBehaviour
{
	static UIFirstRechargePanel mInstance;
	public static UIFirstRechargePanel GetInstance()
	{
		return mInstance;
	}

	public UILabel[] _TitleArray;
	public UILabel _NeedMoney;
	public UILabel _MonsterName;
	public UILabel btn_name;
	public RewardCell[] _Reward;
	public UILabel _RemainTimeTitle;
	public UILabel _RamainTime;

	private long m_time;

	public static void OpenUI()
	{
		UnityEngine.Object obj = PrefabLoader.loadFromPack("LS/pbLSFirstRechargePanel");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			if(go != null)
			{
				RED.AddChild (go, DBUIController.mDBUIInstance._TopRoot);
				mInstance = go.GetComponent<UIFirstRechargePanel>();
			}
		}
	}

	void Start()
	{
		SetDetail();
		SetTime();
	}

	void SetDetail()
	{
		_TitleArray[0].text = Core.Data.stringManager.getString(25133);
		_TitleArray[1].text = Core.Data.stringManager.getString(25134);
		_TitleArray[2].text = Core.Data.stringManager.getString(25135);
		_TitleArray[3].text = Core.Data.stringManager.getString(25136);

		_NeedMoney.text = "1998";
		_MonsterName.text = Core.Data.stringManager.getString(25137);

		if(Core.Data.ActivityManager.firstIOR == null)return;
		if(Core.Data.ActivityManager.firstIOR.Count != 4)return;
		if(_Reward.Length != 4)return;
		for(int i=0; i<Core.Data.ActivityManager.firstIOR.Count; i++)
		{
			_Reward[i].InitUI(Core.Data.ActivityManager.firstIOR[i]);
		}


		this.ShowBtnlabel ();
	}

	void SetTime()
	{
		m_time = Core.Data.playerManager.FirstPurchaseReward;
		if(m_time > 0)
		{
			_RemainTimeTitle.gameObject.SetActive(true);
			_RamainTime.gameObject.SetActive(true);
			InvokeRepeating("RepeatTimeBegin", 0f, 1f);
		}
		else
		{
			_RemainTimeTitle.gameObject.SetActive(false);
			_RamainTime.gameObject.SetActive(false);
		}
	}
	
	void RepeatTimeBegin()
	{
		int l = 0;
		string output = "";
		output += (m_time/3600).ToString("d2");
		l = (int)m_time % 3600;
		output += ":" + (l / 60).ToString ("d2");
		l = (int)l % 60;
        output+=":"+l.ToString("d2");

		_RamainTime.SafeText(output);

		m_time--;
		if(m_time < 0)
		{
			CancelTime();
		}
	}
	
	void CancelTime()
    {
        CancelInvoke("RepeatTimeBegin");
        _RemainTimeTitle.gameObject.SetActive(false);
        _RamainTime.gameObject.SetActive(false);
    }
    
    void Rechcage_OnClick()
	{
		int canGain = Core.Data.rechargeDataMgr._canGainFirstAward;
		if (canGain == 1) 
		{
			ActivityNetController.GetInstance ().GetFirstChargeGiftRequest ();
		} 
		else if(canGain == 0 || canGain==2 )
		{
			UIDragonMallMgr.GetInstance().mUIDragonRechargeMain.SetActive(true);
		}
		
	}

	void Back_OnClick()
	{
		Core.Data.rechargeDataMgr.CloseAll();
	}

	public void Close()
	{
		Destroy(gameObject);
	}

	void OnDestroy()
	{
		mInstance = null;
	}

	public void ShowBtnlabel()
	{
		int canGain = Core.Data.rechargeDataMgr._canGainFirstAward;
		if (canGain == 1) {//已充值  未领取    // 马上领取
			btn_name.text = Core.Data.stringManager.getString (7407);
		} else if (canGain == 2) {//已充值  已领取
			RED.SetActive (false, btn_name.transform.parent.gameObject);
		} else if (canGain == 0){  //未充值
			//马上充值
			btn_name.text = Core.Data.stringManager.getString (7406);
		}
	}
	
}
