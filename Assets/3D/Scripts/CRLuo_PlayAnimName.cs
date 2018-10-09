using UnityEngine;
using System.Collections;

public class CRLuo_PlayAnimName : MonoBehaviour {
	public string PlayName;
	public float PlayTime;
	// Use this for initialization
	void Start () {
		Invoke ("PlayGo", PlayTime);
	}
	

	void PlayGo()
	{
		animation.CrossFade(PlayName);

	}
}
