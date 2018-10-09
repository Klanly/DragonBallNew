using UnityEngine;
using System.Collections;

public class RadarItem : MonoBehaviour 
{
	public UISprite m_spIcon;
	public UISprite m_spLock;
	public UILabel m_txtLock;


	void Start()
	{
		int id = int.Parse(this.name) + 1;
		GPSWarInfo info = Core.Data.gpsWarManager.GetGPSWarInfo(id);
		if(Core.Data.playerManager.RTData.curLevel >= info.Unlock_Lv)
		{
			m_spIcon.color = Color.white;
			RED.SetActive(false, m_txtLock.gameObject, m_spLock.gameObject);
		}
		else
		{
			m_spIcon.color = Color.black;
			RED.SetActive(true, m_spLock.gameObject, m_txtLock.gameObject);
			string strText = Core.Data.stringManager.getString(7320);
			strText = string.Format(strText, info.Unlock_Lv);
			m_txtLock.text = strText;
		}
	}

}
