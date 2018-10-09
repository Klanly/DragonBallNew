using UnityEngine;
using System.Collections;

public class FateTipItem : MonoBehaviour 
{
	public UILabel m_txtFate;

	private FateData m_data;

	public void InitUI(FateData data)
	{
		m_data = data;
		if (m_data != null)
		{
			m_txtFate.text = data.name;
		}
		else
		{
			m_txtFate.text = "";
		}
		RED.SetActive (true, this.gameObject);
	}

	void OnPress(bool pressed)
	{
		if (m_data == null)
			return;

		if (pressed)
			MonFragUI.mInstance.ShowTipUI (m_data.name, m_data.description, this.transform);
		else
			MonFragUI.mInstance.HideTipUI ();
	}
}
