using UnityEngine;
using System.Collections;

public class QuickChatCell : MonoBehaviour 
{
	UILabel m_txtContent;
	string m_strText;
	void Start()
	{
		if (m_txtContent == null)
		{
			m_txtContent = GetComponent<UILabel>();
		}
		m_txtContent.text = m_strText;
	}


	public void SetText(string strText)
	{
		m_strText = strText;
	}

	void OnClick()
	{
		UIMessageMain.Instance.AppendText(m_strText);
	}

}
