using UnityEngine;
using System.Collections;

public class CRLuo_LookAtOld : MonoBehaviour {

	Vector3 OldPos;
	Vector3 OldPos1;
	
	// Update is called once per frame
	void Update () {

		this.gameObject.transform.LookAt(OldPos1);

		Vector3 temp = this.gameObject.transform.position;
		if (temp.z - OldPos1.z < 0)
		{
			Quaternion temp_Q = this.gameObject.transform.rotation;

			temp_Q.x = -temp_Q.x;

			this.gameObject.transform.rotation = temp_Q;
		}
	}

	void LastUpdate()
	{
		OldPos = this.gameObject.transform.position;
		OldPos1 = OldPos;
	}

}
