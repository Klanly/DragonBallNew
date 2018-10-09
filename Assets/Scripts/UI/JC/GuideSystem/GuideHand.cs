using UnityEngine;
using System.Collections;

public class GuideHand : MonoBehaviour {

	// Use this for initialization
	public Transform Target;
	public TweenPosition handpos;
	public TweenPosition selfMove;
	public TweenScale selfScale;
	//参考距离 ReferenceDistance 像素 1秒 跑完
	const float ReferenceDistance =1500f;
	
	void Start () {
	
	}
	

	public void SetDir(int dir,Vector3 _from ,Vector3 _to )
	{
		if(_from == _to ) return;
		
		selfScale.enabled = false;
		selfMove.enabled = false;
		CancelInvoke("SetSelfMove");
		if(!gameObject.activeSelf)
			gameObject.SetActive(true);
		selfMove.transform.localRotation = Quaternion.Euler(0,0,-30f);
		selfMove.duration = 0.5f;
		selfMove.style = UITweener.Style.PingPong;
		selfMove.to = new Vector3(0,20f,0);
	     switch(dir)
		{
		case 0:		
			Target.transform.localRotation = Quaternion.Euler(0,0,180f);
			Target.transform.localPosition = new Vector3(0,60f,0);
			break;
		case 1:			
			Target.transform.localRotation = Quaternion.Euler(0,0,0);
			Target.transform.localPosition = new Vector3(5f,-60f,0);
			break;
		case 2:			
			Target.transform.localRotation = Quaternion.Euler(0,0,330f);
			Target.transform.localPosition = new Vector3(-20f,-60f,0);
			break;
		case 3:
			Target.transform.localRotation = Quaternion.Euler(0,0,30);
			Target.transform.localPosition = new Vector3(50f,-50f,0);
			break;
		case 5:   //左(无倾斜)
			Target.transform.localRotation = Quaternion.Euler(0,0,270f);
			Target.transform.localPosition = new Vector3(-80f,-0,0);
			break;
		case 4:
			selfMove.transform.localRotation = Quaternion.Euler(0,0,0);
			Target.transform.localRotation = Quaternion.Euler(0,0,0);
			selfMove.to = new Vector3(0,120f,0);
			selfMove.duration = 1.2f;
		    selfMove.style = UITweener.Style.Loop;
			break;
		}
		
		if(_from != _to)
		{
			float distance = Vector3.Distance(_from,_to);			
			handpos.from = _from;
			handpos.to = _to;
			float durationTime = 1f*(distance/ReferenceDistance);
			handpos.duration = durationTime;
			handpos.ResetToBeginning ();
			handpos.PlayForward();
			Invoke("SetSelfMove",durationTime);
		}
		
		
	}
	
	void SetSelfMove()
	{
		selfMove.enabled = true;
		selfScale.enabled = true;
	}
	
	
	//设置滑动手势
	void SetSlidingGestures()
	{
		
	}
	
}
