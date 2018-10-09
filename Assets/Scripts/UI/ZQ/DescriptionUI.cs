using UnityEngine;
using System.Collections;
using System.Text;

public class DescriptionUI : MonoBehaviour 
{
	public UILabel m_txtTitle;						//	
	public UILabel m_txtContent;					//

	public void SetText(string strTitle, string[] strText)
	{
		m_txtTitle.text = strTitle;
		StringBuilder sb = new StringBuilder ();
		for (int i = 0; i < strText.Length; i++)
		{
			sb.Append (strText [i]);
			sb.Append ("\n");
		}
		m_txtContent.text = sb.ToString ();
		RED.SetActive (true, this.gameObject);
	}

	void OnBtnExit()
	{
		RED.SetActive (false, this.gameObject);
	}
}
