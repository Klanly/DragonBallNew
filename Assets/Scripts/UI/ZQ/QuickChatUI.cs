using UnityEngine;
using System.Collections;

public class QuickChatUI : MonoBehaviour 
{
	public UIGrid m_grid;
	private bool m_bShow = true;


	public static QuickChatUI OpenUI()
	{
		Object prefab = PrefabLoader.loadFromPack ("ZQ/QuickChatUI");
		if (prefab != null)
		{
			GameObject obj = Instantiate (prefab) as GameObject;
			RED.AddChild (obj, UIMessageMain.Instance.gameObject);
			QuickChatUI ui = obj.GetComponent<QuickChatUI>();
			obj.transform.localPosition = new Vector3(41f,0f,0f);
			return ui;
		}
		return null;
	}

	void Start()
	{
		InitUI ();
	}

	void InitUI()
	{
		for (int i = 0; i < 10; i++)
		{
			string strText = Core.Data.stringManager.getString (5217 + i);
			CreateQuickChatCell (strText);
		}
	}

	void CreateQuickChatCell(string strText)
	{

		Object prefab = PrefabLoader.loadFromPack ("ZQ/QuickChatCell");
		if (prefab != null)
		{
			GameObject obj = Instantiate (prefab) as GameObject;
			RED.AddChild (obj, m_grid.gameObject);
			QuickChatCell cell = obj.GetComponent<QuickChatCell>();
			cell.SetText (strText);
		}
	}

	public void SetShow(bool bShow)
	{
		RED.SetActive (bShow, this.gameObject);
		m_bShow = bShow;
	}

	public bool IsShow
	{
		get
		{
			return m_bShow;
		}
	}
}
