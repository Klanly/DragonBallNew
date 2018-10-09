using UnityEngine;
using System.Collections;

public class LockFB : MonoBehaviour 
{
	public UILabel m_txtTip;

	public void SetFBType(int type)
	{
		string strText = Core.Data.stringManager.getString (9098);
		strText = string.Format (strText, Core.Data.stringManager.getString (9098 + type));
		m_txtTip.text = strText;
	}
}
