using UnityEngine;
using System.Collections;

public class GameCheatUI : MonoBehaviour 
{
	public UIInput m_inputPwd;
	public GameObject m_objInput;


	private readonly string PWD = "Who is your daddy";
	public static void OpenUI()
	{
		Object prefab = PrefabLoader.loadFromPack ("ZQ/GameCheatUI");
		if (prefab != null)
		{
			GameObject obj = Instantiate (prefab) as GameObject;
			RED.AddChild (obj, DBUIController.mDBUIInstance._TopRoot);
		}
	}

	void OnBtnOK()
	{
		if (!string.Equals (PWD, m_inputPwd.value))
		{
			SQYAlertViewMove.CreateAlertViewMove ("密码不对");
		}
	}

	void OnBtnCancel()
	{
		Destroy (this.gameObject);
	}

	void OnBtnStone()
	{
		SendCheatMsg (2);
	}

	void OnBtnCoin()
	{
		SendCheatMsg (1);
	}

	void OnBtnEquip()
	{
		SendCheatMsg (3);
	}

	void OnBtnMonster()
	{
		SendCheatMsg (4);
	}


	void SendCheatMsg(int type)
	{

	}


}
