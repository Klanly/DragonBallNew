using UnityEngine;
using System.Collections;

public class CRLuo_RandScale : MonoBehaviour {
	public float Scale_Min;
	public float Scale_Max;
	// Use this for initialization
	void Start () {
		float temp = Random.Range (Scale_Min, Scale_Max);
		this.gameObject.transform.localScale =new Vector3(temp,temp,temp);
	
	}

}
