using UnityEngine;
using System.Collections;

public class CRLuo_Anim_TFplay : MonoBehaviour {
	public bool Key {get;set;}
//	public string TruePlay;
//	public string FalsePlay;
	public GameObject Sign;
	public GameObject GanTanHao;

	// Use this for initialization
	void Start ()
	{
	    Key = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Time.frameCount % 3 == 0) {
			if (MailReveicer.Instance == null || MailReveicer.Instance.mailState == MailState.None) {
				Key = false;
				//Debug.LogError("["+Key.ToString() +"]     "+ MailReveicer.Instance.mailState.ToString());
				if (Sign.activeSelf)
					Sign.SetActive (false);
			} else {
				Key = true;
				//Debug.LogWarning("["+Key.ToString() +"]     "+ MailReveicer.Instance.mailState.ToString());
				if (!Sign.activeSelf)
					Sign.SetActive (true);
			}
		
			animation.enabled = Key;
			if (GanTanHao.activeSelf != Key)
				GanTanHao.SetActive (Key);
		}

	}
}
