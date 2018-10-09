using UnityEngine;
using System.Collections;

public class CRLuo_Set_Ground : MonoBehaviour
{
	

	// Use this for initialization
	void Start () {

		Vector3 tempPos = this.gameObject.transform.position;

		tempPos.y = 0;
		this.gameObject.transform.position = tempPos;
	
	
	}
}
