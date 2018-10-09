using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CRLuo_InfiniteScreen : MonoBehaviour {

	public float U_Pos;
	public float V_Pos;
    
	public GameObject ScreenOBJ;
    public GameObject CameraOBJ;

    public float P_Add = 3;
    public float RotAddSpeed = 0.1f;

    public float RotUpSpeed = 5;

    public bool Invert;

    [System.NonSerialized]
    public Quaternion Default_Rot;

    [System.NonSerialized]
    public float RotNowSpeed = 0f;

	float Old_U_Pos;
	float Old_V_Pos;


	// Use this for initialization
	void Start () {

		CreateScreen();
	
	}

	// Update is called once per frame
    void Update()
    {
	    V_Pos += 0.3f;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ClearSpeed();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            ClearSpeed();

        }



        if (Input.GetKey(KeyCode.Q))
        {

            CameraRotL();
        }
        else if (Input.GetKey(KeyCode.E))
        {
            CameraRotR();

        }




        if (Input.GetKey(KeyCode.W))
        {
            V_Pos -= P_Add * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            V_Pos += P_Add * Time.deltaTime;
        }


        if (Input.GetKey(KeyCode.A))
        {
            U_Pos += P_Add * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            U_Pos -= P_Add * Time.deltaTime;
        }




        if (U_Pos != 0 || V_Pos != 0)
        {

            SetPos();

        }
    }

	void SetPos()
	{

		for (int i = 0; i < list_Screen.Count; i++)
		{
			list_Screen[i].gameObject.transform.position += new Vector3(U_Pos, 0, V_Pos);

		}
		U_Pos = 0 ;
		V_Pos = 0 ;
	
	}


    void ClearSpeed()
    {
        RotNowSpeed = 0;
    }

    public void CameraRotL()
    {
        if (RotNowSpeed < RotUpSpeed)
        {
            RotNowSpeed += RotAddSpeed;
        }

        float i;
        if (Invert)
        {
            i = -1;
        }
        else
        {
            i = 1;
        };

        CameraOBJ.transform.Rotate(Vector3.up * RotNowSpeed * Time.deltaTime * i);
    }
    public void CameraRotR()
    {
        if (RotNowSpeed < RotUpSpeed)
        {
            RotNowSpeed += RotAddSpeed;
        }

        float i;
        if (Invert)
        {
            i = -1;
        }
        else
        {
            i = 1;
        };

        CameraOBJ.transform.Rotate(Vector3.up * (-RotNowSpeed) * Time.deltaTime * i);
    }







	public List<CRLuo_InfiniteScreenCallipers> list_Screen = new List<CRLuo_InfiniteScreenCallipers>();

	void CreateScreen()
	{
		if (ScreenOBJ != null && list_Screen.Count == 0)
		{

            CRLuo_InfiniteScreenCallipers temp1 = ((GameObject)GameObject.Instantiate(ScreenOBJ)).GetComponent<CRLuo_InfiniteScreenCallipers>();
			temp1.ID_U = 0;
			temp1.ID_V = 0;
			//给台子数樘砑拥谝桓鎏ㄗ姨
			list_Screen.Add(temp1);
            temp1.gameObject.transform.parent = this.gameObject.transform;
			//设置当前物体的空间初始位置
		temp1.transform.position = GetPos(new Vector3(-temp1.U_Callipers*0.5f, temp1.UVOffset.y, -temp1.V_Callipers*0.5f) - temp1.UVOffset);


            CRLuo_InfiniteScreenCallipers temp2 = ((GameObject)GameObject.Instantiate(ScreenOBJ)).GetComponent<CRLuo_InfiniteScreenCallipers>();
			temp2.ID_U = 1;
			temp2.ID_V = 0;
			//给台子数组添加第一个台子
			list_Screen.Add(temp2);
            temp2.gameObject.transform.parent = this.gameObject.transform;
		temp2.transform.position = GetPos(new Vector3(temp2.U_Callipers * 0.5f, temp2.UVOffset.y, -temp2.V_Callipers * 0.5f) - temp2.UVOffset);

            CRLuo_InfiniteScreenCallipers temp3 = ((GameObject)GameObject.Instantiate(ScreenOBJ)).GetComponent<CRLuo_InfiniteScreenCallipers>();
			temp3.ID_U = 0;
			temp3.ID_V = 1;
			//给台子数组添加第一个台子
			list_Screen.Add(temp3);
            temp3.gameObject.transform.parent = this.gameObject.transform;
		temp3.transform.position = GetPos(new Vector3(-temp3.U_Callipers * 0.5f, temp3.UVOffset.y, temp3.V_Callipers * 0.5f) - temp3.UVOffset);

            CRLuo_InfiniteScreenCallipers temp4 = ((GameObject)GameObject.Instantiate(ScreenOBJ)).GetComponent<CRLuo_InfiniteScreenCallipers>();
			temp4.ID_U = 1;
			temp4.ID_V = 1;
			//给台子数组添加第一个台子
			list_Screen.Add(temp4);
            temp4.gameObject.transform.parent = this.gameObject.transform;
		temp4.transform.position = GetPos(new Vector3(temp4.U_Callipers * 0.5f, temp4.UVOffset.y, temp4.V_Callipers * 0.5f) - temp4.UVOffset);

		}

		else
		{
			Debug.Log("当前无" + ScreenOBJ + "战斗场景");

		}




	}



	//与当前物体的相对坐标转换为与当前物体的世界坐标
	Vector3 GetPos(float x, float y, float z)
	{
		return this.transform.position + new Vector3(x, y, z);
	}

	Vector3 GetPos(Vector3 v3)
	{
		return this.transform.position + v3;
	}


}
