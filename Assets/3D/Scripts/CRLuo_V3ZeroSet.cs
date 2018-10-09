using UnityEngine;
using System.Collections;

public class CRLuo_V3ZeroSet : MonoBehaviour {

	public bool Set_X;
	public bool Set_Y;
	public bool Set_Z;
	// Use this for initialization
	void Start () {
		Vector3 v3_Temp = this.gameObject.transform.position;

		if (Set_X)
		{
			v3_Temp.x = 0;
		}
		if (Set_Y)
		{
			v3_Temp.y = 0;
		}
		if (Set_Z)
		{
			v3_Temp.z = 0;
		}

		this.gameObject.transform.position = v3_Temp;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
