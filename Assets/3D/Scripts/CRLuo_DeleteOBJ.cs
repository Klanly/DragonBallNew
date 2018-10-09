using UnityEngine;
using System.Collections;

public class CRLuo_DeleteOBJ : MonoBehaviour
{

	public string _ = "-=<定时删除物体对象程序>=-";
	public string __ = "删除时间";
	public float Delete1_Time = 5f;
	public string ___ = "随机时间";
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


