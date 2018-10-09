using UnityEngine;
using System.Collections;

public class RobUI : MonoBehaviour 
{
	public UIButton m_btnOK;
	public UIButton m_btnCancel;
	public UILabel m_txtContent;

	//金币
	public UILabel m_txtFullMoney;
	public UILabel m_txtLeftMoney;

	//钻石
	public UILabel m_txtFullStone;
	public UILabel m_txtLeftStone;

	private static RobUI _instance;
	public static RobUI mInstance
	{
		get
		{
			return _instance;
		}
	}

	void Awake()
	{
		_instance = this;
	}

	void Start()
	{
		m_btnOK.TextID = 20068;
		m_btnCancel.TextID = 5216;
	}

	public static void OpenUI(string strContent, int fullMoney, int leftMoney, int fullStone, int leftStone)
	{
		if (_instance == null)
		{
			Object prefab = PrefabLoader.loadFromPack ("ZQ/RobUI");
			GameObject obj = Instantiate (prefab) as GameObject;
			RED.AddChild (obj, DBUIController.mDBUIInstance._TopRoot);
		}
		_instance.InitUI (strContent, fullMoney, leftMoney, fullStone, leftStone);
		_instance.SetShow (true);
	}


	public void SetShow(bool bShow)
	{
		RED.SetActive (bShow, this.gameObject);
	}


	private void InitUI(string strContent, int fullMoney, int leftMoney, int fullStone, int leftStone)
	{
		m_txtContent.text = strContent;

		m_txtFullMoney.text = fullMoney.ToString();
		m_txtLeftMoney.text = leftMoney.ToString ();

		m_txtFullStone.text = fullStone.ToString ();
		m_txtLeftStone.text = leftStone.ToString ();
	}

	void OnBtnOK()
	{
		DBUIController.mDBUIInstance.HiddenFor3D_UI(true);
		MailBox.OpenUI (0, MailBoxShowTab.BattleMsg);
		OnBtnCancel ();
	}

	void OnBtnCancel()
	{
		_instance = null;
		Destroy (this.gameObject);
	}
}
