using UnityEngine;
using System.Collections;

public class CRLuo_Rot_Inversion : MonoBehaviour {
	public GameObject InputOBJ;

	public bool X;
	public bool Y;
	public bool Z;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Quaternion Temp = InputOBJ.transform.localRotation;
		if(X)
			Temp.x = - Temp.x;
		if (Y)
			Temp.y = -Temp.y;

		if (Z)
			Temp.z = - Temp.z;
		this.gameObject.transform.localRotation = Temp;
	
	}
}
