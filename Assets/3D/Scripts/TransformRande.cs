using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TransformRande : MonoBehaviour
{


	#region 定义当前物体的位置、旋转、缩放存储变量
	[System.NonSerialized]
	public Vector3 ThisOBJ_Pos;
	[System.NonSerialized]
	public Vector3 ThisOBJ_Rot;
	[System.NonSerialized]
	public Vector3 ThisOBJ_Sca;
	#endregion

	#region 创建程序主要变量
	//创建对象总数
	public float OBJSum = 0;
	//创建物体数组
	public GameObject[] CreatOBJ;
	//位移开关
	public bool Pos_Key = false;
	public float Pos1_X;
	public float Pos2_X;
	public float Pos1_Y;
	public float Pos2_Y;
	public float Pos1_Z;
	public float Pos2_Z;

	public bool Ring_Key = false;
	public float R1 = 1;
	public bool R_rand_Key = false;
	public float R2 = 1;




	public bool Rot_Key = false;
	public float Rot1_X;
	public float Rot2_X;
	public float Rot1_Y;
	public float Rot2_Y;
	public float Rot1_Z;
	public float Rot2_Z;

	public bool Sca_Key = false;
	public float Sca1_X = 1f;
	public float Sca2_X = 1f;
	public float Sca1_Y = 1f;
	public float Sca2_Y = 1f;
	public float Sca1_Z = 1f;
	public float Sca2_Z = 1f;
	public bool Sca_Olny_X_Key = false;

	public bool CreatrTime_Key = false;
	public float Cre_Time1;
	public float Cre_Time2;

	public bool CreatrGroup_Key = false;
	#endregion

	// Use this for initialization
	void Start () {

		for (int i = 0; i < OBJSum; i++)
		{
			if (CreatrTime_Key)
			{
				Invoke("RandeCreate", Random.Range(Cre_Time1, Cre_Time2));
			}
			else
			{
				RandeCreate();
			}
		}

	
	}
	

	void RandeCreate()
	{
		Vector3 temp_Pos = Vector3.zero;
		ThisOBJ_Pos = this.gameObject.transform.position;
		if (Pos_Key && !Ring_Key)
		{
			temp_Pos = new Vector3(Random.Range(Pos1_X, Pos2_X) + ThisOBJ_Pos.x, Random.Range(Pos1_Y, Pos2_Y) + ThisOBJ_Pos.y, Random.Range(Pos1_Z, Pos2_Z) + ThisOBJ_Pos.z);
		}
		else if (Ring_Key)
		{
			float myR = R1;
			if(R_rand_Key)
			{
				myR = Random.Range(R1, R2);
			}
			float myA = Random.Range(0, 360);

			temp_Pos = new Vector3(myR * Mathf.Cos(myA) + ThisOBJ_Pos.x, Random.Range(Pos1_Y, Pos2_Y) + ThisOBJ_Pos.y, myR * Mathf.Sin(myA) + ThisOBJ_Pos.z);
		}
		else
		{
			temp_Pos = ThisOBJ_Pos;
		}

		Vector3 temp_Rot = Vector3.zero;
		ThisOBJ_Rot = this.gameObject.transform.rotation.eulerAngles;
		if (Rot_Key)
		{
			temp_Rot = new Vector3(Random.Range(Rot1_X, Rot2_X) + ThisOBJ_Rot.x, Random.Range(Rot1_Y, Rot2_Y) + ThisOBJ_Rot.y, Random.Range(Rot1_Z, Rot2_Z) + ThisOBJ_Rot.z);
		}
		else
		{
			temp_Rot = ThisOBJ_Rot;
		}


		Vector3 temp_Sca = new Vector3(1,1,1);
		ThisOBJ_Sca = this.gameObject.transform.localScale;
		if (Sca_Key)
		{
			temp_Sca = new Vector3(Random.Range(Sca1_X, Sca2_X) * ThisOBJ_Sca.x, Random.Range(Sca1_Y, Sca2_Y) * ThisOBJ_Sca.y, Random.Range(Sca1_Z, Sca2_Z) * ThisOBJ_Sca.z);
		}
		else
		{
			temp_Sca = ThisOBJ_Sca;
		}
		if (Sca_Olny_X_Key)
		{
			temp_Sca = new Vector3(temp_Sca.x, temp_Sca.x, temp_Sca.x);
		
		}

		if (CreatOBJ.Length > 0)
		{
			int temp_ID = Random.Range(0, CreatOBJ.Length);

			if (CreatOBJ[temp_ID] != null)
			{
				GameObject tempOBJ = (GameObject)Instantiate(CreatOBJ[temp_ID], temp_Pos, Quaternion.Euler(temp_Rot));

				tempOBJ.transform.localScale = temp_Sca;

				if (CreatrGroup_Key)
				{
					tempOBJ.transform.parent = this.gameObject.transform;
				}
			}
		
		}



	}
}
