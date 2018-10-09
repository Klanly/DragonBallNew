using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ZhaoMuUI : MonoBehaviour 
{
	private static ZhaoMuUI _mInstance;
	public static ZhaoMuUI mInstance
	{
		get 
		{
			return _mInstance;
		}
	}
		
	public ZhaoMuSubUI m_zhaomuSubUI;
	public UILabel[] m_labTime;
	public System.Action closeCallBack;

	public static void OpenUI(System.Action callBack = null)
	{
		if (_mInstance == null)
		{
			Object prefab = PrefabLoader.loadFromPack ("ZQ/ZhaoMuUI");
			if (prefab != null)
			{
				GameObject obj = Instantiate (prefab) as GameObject;
				RED.AddChild (obj, DBUIController.mDBUIInstance._bottomRoot);
				_mInstance.closeCallBack = callBack;
			}
		}
		else
		{
			mInstance.SetShow (true);
			_mInstance.closeCallBack = callBack;
		}
	}



	public static void DestroyUI()
	{
		if (_mInstance != null)
		{
			Destroy (_mInstance.gameObject);
			_mInstance = null;
		}
	}
		
	void Awake()
	{
		_mInstance = this;
	}

	void Start()
	{
		m_zhaomuSubUI.SetShow (false);

		if (IsInvoking ("UpdateTime")) {
			CancelInvoke ("UpdateTime");
		}

		InvokeRepeating ("UpdateTime", 0.0f, 1.0f);
	}

	void UpdateTime()
	{
		ZhaoMuStateData[] data = new ZhaoMuStateData[2];

		data[0] = Core.Data.zhaomuMgr.GetZhaomuState (m_zhaomuSubUI.ZHAOMU_BASE + 1);
		data[1] = Core.Data.zhaomuMgr.GetZhaomuState (m_zhaomuSubUI.ZHAOMU_BASE + 3);

		for (int i = 0; i < data.Length; i++)
		{

			string strFreeCnt = Core.Data.stringManager.getString (5243);
			
			if (data [i] != null)
			{
				//免费
				if (Core.Data.zhaomuMgr.IsZhaomuFree(data[i].pron))
				{
					strFreeCnt = string.Format (strFreeCnt, data [i].freecount, data [i].totalcount);
					m_labTime[i].text = strFreeCnt;
				}
				else
				{
					if (data [i].freecount <= 0)
					{
						m_labTime [i].text = Core.Data.stringManager.getString (5249);
					}
					else
					{
						string strTime = Core.Data.stringManager.getString (5244);
						m_labTime [i].text = string.Format (strTime, m_zhaomuSubUI.GetTimeFormat (data[i].coolTime));
					}
				}
			}
			else
			{
				m_labTime[i].text = "";
			}
		}
	}
		
	public void SetShow(bool bShow)
	{
		RED.SetActive (bShow, this.gameObject);
        RED.SetActive (bShow,UIMiniPlayerController.Instance.gameObject);
       
	}

	public void OnClickZhaomu(int type)
	{
		m_zhaomuSubUI.OpenUI (type);
	}


	void OnClickMainZhaomu(GameObject obj)
	{
		int index = int.Parse (obj.name);
		OnClickZhaomu(index);
	}
		
	public void OnClickExit()
	{
		if (closeCallBack != null) {
			closeCallBack ();
			closeCallBack = null;
		} else {
			DBUIController.mDBUIInstance.ShowFor2D_UI ();
		}

		Destroy (this.gameObject); 
		_mInstance = null;
	}

	void DownLoadFinish(AssetTask task)
	{

	}
}
