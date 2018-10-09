using UnityEngine;
using System.Collections;
public enum VC_STATE{
		NONE,
		WILL_SHOW,
		DID_SHOW,
		WILL_HIDDEN,
		DID_HIDDEN,
}

public class MDBaseViewController : RUIMonoBehaviour {
	
	[HideInInspector]
	public VC_STATE state = VC_STATE.NONE;
	
	public System.Action<VC_STATE,MDBaseViewController> viewControllerBehaviour;
	
	[HideInInspector]
	public int vcID=0;
	
//	const string TPNAME = "BaseViewAnimation";
	const int VCFlag = 1;
	public virtual void viewWillShow()
	{
		if (!this.gameObject.activeSelf)
		{
			RED.SetActive (true, this.gameObject);
		}
		state = VC_STATE.WILL_SHOW;
//		TweenPosition tp = gameObject.AddComponent<TweenPosition>();
//		tp.mComponentFlag = VCFlag;
//		tp.method = UITweener.Method.EaseIn;
//		tp.to = new Vector3(0,0,0);
//		tp.from = new Vector3(-1141f,0,0);
//		tp.duration =0.3f;
//		tp.PlayForward();
//		tp.onFinished.Clear();
//

		transform.localPosition = new Vector3(0,0,0);
		if(viewControllerBehaviour != null)
		{
			viewControllerBehaviour(VC_STATE.WILL_SHOW,this);
		}
//
//		tp.onFinished.Add(new EventDelegate(this,"viewDidShow"));
		StartCoroutine(IEviewDidShow());
	}
	IEnumerator IEviewDidShow()
	{
		yield return new WaitForEndOfFrame();
		viewDidShow();
	}
	public virtual void viewDidShow(){
		state = VC_STATE.DID_SHOW;
//		TweenPosition[] tp = this.GetComponents<TweenPosition>();
//		for(int i=0;i<tp.Length;i++)
//		{
//			if(tp[i].mComponentFlag==VCFlag)
//			{
//				Destroy(tp[i]);
//				break;
//			}
//		}
		
		if(viewControllerBehaviour != null)
		{
			viewControllerBehaviour(VC_STATE.DID_SHOW,this);
		}
	}
	public virtual void viewWillHidden()
	{
		if (!this.gameObject.activeInHierarchy)
		{
			return;
		}
		state =VC_STATE.WILL_HIDDEN;
//		TweenPosition tp = gameObject.AddComponent<TweenPosition>();
//		tp.mComponentFlag = VCFlag;
//		tp.method = UITweener.Method.EaseIn;
//		tp.from = new Vector3(0,0,0);;
//		tp.to = new Vector3(-1141f,0,0);
//		tp.duration =0.3f;
//		tp.PlayForward();
//		tp.onFinished.Clear();
//		tp.onFinished.Add(new EventDelegate(this,"viewDidHidden"));
		transform.localPosition = new Vector3(-1141f,0,0);
		if(viewControllerBehaviour != null)
		{
			viewControllerBehaviour(VC_STATE.WILL_HIDDEN,this);
		}
		StartCoroutine(IEviewDidHidden());
	}
	IEnumerator IEviewDidHidden()
	{
		yield return new WaitForEndOfFrame();
		viewDidHidden();
	}
	public virtual void viewDidHidden(){
		state = VC_STATE.DID_HIDDEN;

//		TweenPosition[] tp = this.GetComponents<TweenPosition>();
//		for(int i=0;i<tp.Length;i++)
//		{
//			if(tp[i].mComponentFlag == VCFlag)
//			{
//				GameObject.Destroy(tp[i]);
//				break;
//			}
//		}
		if(viewControllerBehaviour != null)
		{
			viewControllerBehaviour(VC_STATE.DID_HIDDEN,this);
		}
	}
	
	
}

