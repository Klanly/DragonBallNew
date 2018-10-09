using UnityEngine;
using System.Collections;

public class UIRobIntroduct : RUIMonoBehaviour 
{

	static UIRobIntroduct _Instance;
	public static UIRobIntroduct GetInstance()
	{
		return _Instance;
	}

	public UILabel _Title1;
	public UILabel _Title2;
	public UILabel _Content;
	
	public UILabel _Title3;
	public UILabel _Title4;
	public UILabel _ContentTianxia;
	public GameObject m_Grab;
	public GameObject m_Tianxia;

	bool m_isTianxia;

	public static void OpenUI(bool _isTianxia)
	{
		if(_Instance == null)
		{
			Object obj = PrefabLoader.loadFromPack("LS/pbLSRobIntroduct");
			if(obj != null)
			{
				GameObject go = Instantiate (obj) as GameObject;
				if(go != null)
				{
                    _Instance = go.GetComponent<UIRobIntroduct>();
					RED.AddChild(go, DBUIController.mDBUIInstance._TopRoot);
					_Instance.m_isTianxia = _isTianxia;
                }
            }
		}
	}

	void Start()
	{
		_Title1.text = Core.Data.stringManager.getString(25102);
		_Title2.text = Core.Data.stringManager.getString(25103);
		_Content.text = Core.Data.stringManager.getString(25104);

		_Title3.text = Core.Data.stringManager.getString(25102);
		_Title4.text = Core.Data.stringManager.getString(25162);
		_ContentTianxia.text = Core.Data.stringManager.getString(25161);

		if(m_isTianxia)
		{
			m_Tianxia.gameObject.SetActive(true);
			m_Grab.gameObject.SetActive(false);
		}
		else
		{
			m_Tianxia.gameObject.SetActive(false);
			m_Grab.gameObject.SetActive(true);
		}
	}

	void Back_OnClick()
	{
		this.dealloc();
		if(_Instance != null)_Instance = null;
	}


}
