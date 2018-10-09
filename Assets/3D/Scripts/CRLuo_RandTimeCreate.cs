using UnityEngine;
using System.Collections;

public class CRLuo_RandTimeCreate : MonoBehaviour
{

	//粒子集合
	public GameObject[] myFX;

	int FX_ID = 0;
	
	//顺序播放效果
	public bool OutOfOrder;

	//间隔时间
	public float IntervalTime1 = 1f;

	public bool RandTime;

	//随机间隔2
	public float IntervalTime2 = 1f;

	//开启旋转
	public bool RandRot_X = false;
	public bool RandRot_Y = false;
	public bool RandRot_Z = false;

	Vector3 NewRot;

	// Use this for initialization
	void Start () {
		RandFX();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void RandFX()
	{
		if (myFX.Length > 0)
		{
			 float temp_X = 0f;
			 float temp_Y = 0f;
			 float temp_Z = 0f;
			if (RandRot_X)
			{
				temp_X = Random.Range(0, 360);
			}
			if (RandRot_Y)
			{
				temp_Y = Random.Range(0, 360);
			}
			if (RandRot_Z)
			{
				temp_Z = Random.Range(0, 360);
			}

			NewRot = new Vector3(temp_X, temp_Y, temp_Z);

			if (RandRot_X || RandRot_Y || RandRot_Z)
			{
				EmptyLoad.CreateObj(myFX[FX_ID], this.gameObject.transform.position, Quaternion.Euler(NewRot));
			}
			else
			{
				EmptyLoad.CreateObj(myFX[FX_ID], this.gameObject.transform.position, this.gameObject.transform.rotation);
			}

			if (myFX.Length > 1)
			{
				if (!OutOfOrder)
				{
					FX_ID++;
					if (FX_ID >= myFX.Length)
					{
						FX_ID = 0;
					}


				}
				else
				{
					FX_ID = Random.Range(0, myFX.Length);


				}
			}


			if (RandTime)
			{
				Invoke("RandFX", Random.Range(IntervalTime1,IntervalTime2));
			}
			else
			{
				Invoke("RandFX", IntervalTime1);
			
			}

		}
	}
}
