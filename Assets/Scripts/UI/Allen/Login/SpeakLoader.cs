using UnityEngine;
using System.Collections;

public class SpeakLoader : MonoBehaviour {

	public GameObject broadCast;
	public UILabel    title;
	public UILabel    content;

	public GameObject btnSpeaker;
	/// 
	/// 自动显示
	/// 
	public void autoShow(string strTitle, string strContent) {
		if(string.IsNullOrEmpty(strTitle) || string.IsNullOrEmpty(strContent)) {
			return;
		}
		btnSpeaker.SetActive(true);
		title.text   = strTitle;
		content.text = strContent;

		BoxCollider bc = content.GetComponent<BoxCollider>();
		bc.center = new Vector3(450, -content.height / 2, 0);
		bc.size   = new Vector3(900, content.height, 0);

		ShowBroadCast();
	}

	/// 
	/// 展示一个公告信息
	/// 
	public void ShowBroadCast() {
		broadCast.SetActive(true);
		broadCast.transform.localScale = Vector3.zero;
		MiniItween.ScaleTo(broadCast, Vector3.one, 0.15f);
		AsyncTask.QueueOnMainThread (() =>{
			content.enabled = false;
			content.enabled = true;
			content.MakePixelPerfect();
		},0.3f);
		Invoke ("MakePerfect",0.5f);
	}
	void MakePerfect(){
		content.MakePixelPerfect();
	}

	/// 
	/// 隐藏一个公告信息
	/// 
	public void HideBroadCast() {
		MiniItween.ScaleTo(broadCast, Vector3.zero, 0.2f).FinishedAnim = () => {
			broadCast.SetActive(false);
		};
	}

}
