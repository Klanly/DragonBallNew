using UnityEngine;
using System.Collections;

public class CRLuo_RandRotate : MonoBehaviour {

	public bool X;
	public bool Y;
	public bool Z;

	// Use this for initialization
	void Start () {
		Quaternion Temp = this.gameObject.transform.localRotation;
		if (X)
			Temp.x = Random.Range (0, 360);
		if (Y)
			Temp.y = Random.Range (0, 360);
		if (Z)
			Temp.z = Random.Range (0, 360);
		this.gameObject.transform.localRotation = Temp;
	
	}
}
