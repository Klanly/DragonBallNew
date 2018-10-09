using UnityEngine;
using System.Collections;

public class CRLuo_GoMiniItween : MonoBehaviour {

	public Vector3 Go_Pot;
	public float Go_StartTime;
	public float Go_Time;
	public MiniItween.EasingType Type;
	public float DeleteTime;

	// Use this for initialization
	void Start () {
		Invoke("GOGO", Go_StartTime);

		Destroy(this.gameObject, DeleteTime);

	}
	void GOGO() 
	{
		MiniItween.MoveTo(this.gameObject, Go_Pot, Go_Time,Type,true);
	}
}
