using UnityEngine;
using System.Collections;

public class UITrialAlternative : MonoBehaviour
{
	public UILabel mShaluLab;
	public UILabel mBuouLab;

	public UILabel m_BtnName1;
	public UILabel m_BtnName2;

	public UILabel m_Stone1;
	public UILabel m_Stone2;

	public UIButton m_ShaluBtn;
	public UIButton m_BuouBtn;

	private int m_ShaluStone;
	private int m_BuouStone;

	VipInfoData _data;

	private FinalTrialData m_TrialData{
		get{
			return FinalTrialMgr.GetInstance()._FinalTrialData;
		}
	}

	public void OnShow(GetFinalTrialStateInfo m_data)
	{
		mShaluLab.SafeText(m_data.shalu.fCount.ToString() + "/" + m_data.shalu.count.ToString());
		mBuouLab.SafeText(m_data.buou.fCount.ToString() + "/" + m_data.buou.count.ToString());

		m_BtnName1.SafeText(Core.Data.stringManager.getString(5142));
		m_BtnName2.SafeText(Core.Data.stringManager.getString(5142));

		if( m_data.shalu.fCount == 0 && FinalTrialMgr.GetInstance()._FinalTrialData.CurDungeon < 15)m_ShaluBtn.isEnabled = false;
		else m_ShaluBtn.isEnabled = true;

		if(m_data.buou.fCount == 0 && FinalTrialMgr.GetInstance()._FinalTrialData.CurDungeon < 15)m_BuouBtn.isEnabled = false;
		else m_BuouBtn.isEnabled = true;

		m_Stone1.SafeText(m_data.shalu.needStone.ToString());
		m_Stone2.SafeText(m_data.buou.needStone.ToString());
		m_ShaluStone = m_data.shalu.needStone;
		m_BuouStone = m_data.buou.needStone;

		_data = Core.Data.vipManager.GetVipInfoData(Core.Data.playerManager.curVipLv);
	}

	void Btn_Callback(GameObject m_btn)
	{
		if(m_btn.name == "BackBtn")
		{

		}
		else if(m_btn.name == "ShaluIcon")
		{
			FinalTrialMgr.GetInstance().OpenNewMap(1);
		}
		else if(m_btn.name == "BuouIcon")
		{
			FinalTrialMgr.GetInstance().OpenNewMap(2);
		}
		Close ();
	}

	void Click_Shalu()
	{
		if(_data != null)
		{
			if(FinalTrialMgr.GetInstance()._FinalTrialData.CurDungeon < 15 && _data.expeditionreset <=  0)
			{
				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(20059));
				return;
			}
		}
		if(Core.Data.playerManager.RTData.curStone < m_ShaluStone)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(7310));
			return;
		}
		UIInformation.GetInstance().SetInformation(Core.Data.stringManager.getString(7357), Core.Data.stringManager.getString (5030), ShaluBuyTimeCallback);

	}

	void Click_Buou()
	{
		if(_data != null)
		{
			if(FinalTrialMgr.GetInstance()._FinalTrialData.CurDungeon < 15 && _data.expeditionreset <=  0)
			{
				SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(20059));
				return;
			}
		}
		if(Core.Data.playerManager.RTData.curStone < m_BuouStone)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(7310));
			return;
		}
		UIInformation.GetInstance().SetInformation(Core.Data.stringManager.getString(7357), Core.Data.stringManager.getString (5030), BuouBuyTimeCallback);
	}

	void ShaluBuyTimeCallback()
	{
		FinalTrialMgr.GetInstance().ResetFinalTrialValueRequest(1);
	}

	void BuouBuyTimeCallback()
	{
		FinalTrialMgr.GetInstance().ResetFinalTrialValueRequest(2);
	}

	public void Close()
	{
		Destroy (gameObject);
		FinalTrialMgr.GetInstance ()._UITrialAlternative = null;
	}
	
	static public UITrialAlternative CreatePanel()
	{
		Object obj = PrefabLoader.loadFromPack("LS/pbLSTrialAlternative");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			if(go != null)
			{
				RED.AddChild(go, DBUIController.mDBUIInstance._bottomRoot);
				UITrialAlternative script = go.GetComponent<UITrialAlternative>();
				return script;
			}
		}
		return null;
	}
}
