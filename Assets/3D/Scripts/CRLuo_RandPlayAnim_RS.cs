using UnityEngine;
using System.Collections;

public class CRLuo_RandPlayAnim_RS : MonoBehaviour {

	public string[] AnimName;
	public float RandMinTime=1f;
	public float RandMaxTime=5f;

	public bool Rotate_X;
	public bool Rotate_Y;
	public bool Rotate_Z;

	public float Scale_Min;
	public float Scale_Max;
	// Use this for initialization
	void Start () {
		if (AnimName == null) {
			Debug.Log(this.gameObject.name + "Not Anim !!!!");
			Destroy(this);
		}
		if (AnimName.Length > 1) {
			Invoke ("RandPlayGO", Random.Range (RandMinTime, RandMaxTime));
		}
	}
	void Update()
	{

		if (!animation.isPlaying)
		{
			animation.CrossFade(AnimName[0]);
		}
	}
	void RandPlayGO()
	{
		Quaternion Temp = this.gameObject.transform.localRotation;
		if (Rotate_X)
			Temp.x = Random.Range (0, 360);
		if (Rotate_Y)
			Temp.y = Random.Range (0, 360);
		if (Rotate_Z)
			Temp.z = Random.Range (0, 360);
		this.gameObject.transform.localRotation = Temp;


		animation.CrossFade(AnimName[Random.Range(1, AnimName.Length)]);

		Invoke("RandPlayGO", Random.Range(RandMinTime, RandMaxTime));

	}
}
