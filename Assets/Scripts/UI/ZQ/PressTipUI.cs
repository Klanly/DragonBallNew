using UnityEngine;
using System.Collections;

public class PressTipUI : MonoBehaviour 
{
	public UILabel m_txtName;
	public UILabel m_txtDesp;
	public TweenScale m_root;

	void Awake()
	{
	}

	public void OpenUI(string strName, string strDesp, Transform tfparent)
	{
		this.transform.parent = tfparent;
		this.transform.localPosition = Vector3.up * 75;
		ShowUI ();
		m_txtName.text = strName;
		m_txtDesp.text = strDesp;
	}

	void SetShow(bool bshow)
	{
		RED.SetActive (bshow, this.gameObject);
	}

	public void HideUI()
	{
		m_root.delay = 0;
		m_root.duration = 0.25f;
		m_root.from =  Vector3.one;
		m_root.to = new Vector3(0.01f,0.01f,0.01f);
		m_root.onFinished.Clear();
		m_root.onFinished.Add(new EventDelegate(this,"HidePanel"));
		m_root.ResetToBeginning();
		m_root.PlayForward();
	}

	void HidePanel()
	{
		this.transform.parent = MonFragUI.mInstance.transform;
		RED.SetActive (false, this.gameObject);
	}

	void ShowUI()
	{
		SetShow (true);
		m_root.delay = 0;
		m_root.duration = 0.25f;
		m_root.from =  new Vector3(0.01f,0.01f,0.01f);
		m_root.to = Vector3.one;
		m_root.onFinished.Clear();
		m_root.ResetToBeginning();
		m_root.PlayForward();
	}
}
