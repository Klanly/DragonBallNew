using UnityEngine;
using System.Collections;

public class CRLuo_TimeCreate : MonoBehaviour {

	public GameObject FXOBJ;
	public float CreateTime;
	public float OffsetTime;
	public bool LoopCreate;

	public float AllTime;
	public bool Parent_key;
	// Use this for initialization
	void Start () {

		if (!LoopCreate)
		{
			Invoke("CreateFX", CreateTime + OffsetTime);
		}
		else
		{
			InvokeRepeating("CreateFX", CreateTime + OffsetTime, AllTime);
		}
	}

	public void ReplayTimeCreate()
	{
		Start();
	}
	
	// Update is called once per frame

	void CreateFX()
	{
		GameObject temp = (GameObject)Instantiate(FXOBJ, this.gameObject.transform.position, this.gameObject.transform.rotation);
		if (Parent_key)
		{
			temp.transform.parent = this.gameObject.transform;
		}
	}
}
