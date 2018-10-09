using UnityEngine;
using System.Collections;

public class CRLuo_Transform_Add : MonoBehaviour
{
	public string _ = "-=<物体位移、旋转、缩放、修改累加程序>=-";
	public string __ = "位移累加";
	public bool Move_Add_Key;
	public Vector3 Move_Add;
	public Vector3 Move_SpeedUp;
	public string ___ = "旋转累加";
	public bool Rotation_Add_Key;
	public Vector3 Rotation_Add;
	public Vector3 Rotation_SpeedUp;
	public string ____ = "缩放累加";
	public bool Scale_Add_Key;
	public Vector3 Scale_Add;
	public Vector3 Scale_SpeedUp;

	public string _____ = "时间控制";
	public bool Time_Add_Key = false;
	public float StartTime = 0;
	public float EndTime = 3;
	public string ______ = "自动销毁";
	public bool AutoDelete ;
	void Start () {
		//如果有时间控制
		if(Time_Add_Key){
			//如果开启自动销毁
			if (AutoDelete)
			{
				//计时执行删除
				Invoke("Del", EndTime);
			}
			else
			{
				//计时执行停止
				Invoke("Stop", EndTime);
			}
		}
	}

	//自定义删除函数
	void Del(){
		//删除当前对象
		Destroy(this.gameObject);
	}
	//自定义停止函数
	void Stop()
	{
		//删除这段代码
		Destroy(this);
	}
	void Update () {

		//是否有时间控制
		if (Time_Add_Key)
		{
			//如果没有到开始时间
			if(StartTime > 0){
				//开始时间减每一帧间隙时间
				StartTime -= Time.deltaTime;
				//这一帧从这里直接跳出当前程序
				return;
			}
		}

		//如果开启位移累加
		if (Move_Add_Key)
		{
			Move_SpeedUp += Move_SpeedUp * Time.deltaTime;
			Move_Add += Move_SpeedUp * Time.deltaTime;

			//位移修改为 = 修改值*帧间隙时间
			this.transform.Translate(Move_Add * Time.deltaTime);
		}

		//如果开启旋转累加
		if (Rotation_Add_Key)
		{
			Rotation_SpeedUp += Rotation_SpeedUp * Time.deltaTime;
			Rotation_Add += Rotation_SpeedUp * Time.deltaTime;
			//位移修改为 = 修改值*帧间隙时间
			this.transform.Rotate(Rotation_Add * Time.deltaTime);
		}

		//如果开启缩放累加
		if (Scale_Add_Key)
		{
			Scale_SpeedUp += Scale_SpeedUp * Time.deltaTime;
			Scale_Add += Scale_SpeedUp * Time.deltaTime;
			//位移修改为 = 修改值*帧间隙时间
			this.transform.localScale += Scale_Add * Time.deltaTime;
		}

	}
}
