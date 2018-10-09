using UnityEngine;
using System.Collections;

public class CRLuo_Shake : MonoBehaviour {

	public Vector3 Shake;
	public float StaretTime;
	public float ShakeTime;
	public bool Loop;
	public float LoopTime;


	// Use this for initialization
	void Start()
	{

		if (!Loop)
		{
			Invoke("ShakeFX", StaretTime);
		}
		else
		{
			InvokeRepeating("ShakeFX", StaretTime, LoopTime);
		}
	}

	void ShakeFX()
	{

		MiniItween.Shake(this.gameObject, Shake, ShakeTime,MiniItween.EasingType.EaseOutQuad);
	}
}
