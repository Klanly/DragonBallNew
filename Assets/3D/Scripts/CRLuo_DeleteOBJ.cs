using UnityEngine;
using System.Collections;

public class CRLuo_DeleteOBJ : MonoBehaviour
{

	public string _ = "-=<��ʱɾ������������>=-";
	public string __ = "ɾ��ʱ��";
	public float Delete1_Time = 5f;
	public string ___ = "���ʱ��";
	public bool Del_Rand_Key = false;
	public float Delete2_Time = 10f;
	// Use this for initialization



	void Start()
	{
		if (Del_Rand_Key)
		{
			Destroy(this.gameObject, Random.Range(Delete1_Time, Delete2_Time));
		}
		else
		{
			Destroy(this.gameObject, Delete1_Time);
		}
	}

}


