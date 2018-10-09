using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
/// <summary>
/// 用于一组动画缩放的展示
/// </summary>
public class HenryPlayEffectTool : MonoBehaviour {
	ArrayList trlist = new ArrayList();
	public int durationTotalTime=2; 
	public float durationEveryTime=0.1f;
	public Vector3 maxScale=new Vector3(1.4f,1.4f,1.0f);
	public Vector3 originalityScale=new Vector3(1,1,1);

	void Awake(){
		foreach (Transform child in transform)
		{
			trlist.Add(child.gameObject); 
			child.localScale=new Vector3(0.0f,0.0f,0f);

		}    
		this.Invoke("playEffectStart",0.3f);
	}

	int playIndex;
	//bool isaddCall;
	//开始播缩放处理
	void playEffectStart(){
		if(playIndex==0){

			foreach (GameObject child in trlist)
			{
				child.transform.localScale=new Vector3(0.0f,0.0f,0f);
			}

		}

		GameObject obj=(GameObject)trlist[playIndex];
		TweenScale.Begin(obj, durationEveryTime, maxScale);

		Invoke("reducefun",durationEveryTime);

	}
	void reducefun(){
		GameObject obj = (GameObject)trlist[playIndex];

		TweenScale.Begin(obj, 0.05f, originalityScale);
		Invoke("reseatComplete",0.05f);
	}
	//复位完成
	void reseatComplete(){

		playIndex++;
		if (playIndex == trlist.Count)
		{
			playIndex = 0;
			Invoke("playEffectStart", durationTotalTime);
		}
		else
		{
			playEffectStart();
		}
	}

	void Destroy(){
		this.Destroy();
		foreach (GameObject child in trlist)
		{
			Destroy(child);
		}
	}
}
