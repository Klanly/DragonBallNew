using UnityEngine;
using System.Collections;

public class CRLuo_TouchRoll : MonoBehaviour {

	public Vector3 Default_Rot;
	public float L_Max = 20;
	public float R_Max = 20;
	public float U_Max = 20;
	public float D_Max = 20;
	public float BakeDistance = 3;
	public float BakeTime = 1;

	public float MoveSpeed =1;
	Vector2 Old_Pos_V2;
	Vector2 New_Pos_V2;
	Vector3 Old_Pos_V3;
	Vector3 Now_Pos_V3;
	bool OpenTouch;
	bool TouchInput;

	// Use this for initialization
	void Start () {
		this.gameObject.transform.rotation = Quaternion.Euler(Default_Rot);
		OpenTouch = true;
		TouchInput = true;
	}
	
	// Update is called once per frame
	void Update () {

		if (OpenTouch)
		{
			Now_Pos_V3 = this.gameObject.transform.localRotation.eulerAngles;
			if (Now_Pos_V3.z - Default_Rot.z < -L_Max || Now_Pos_V3.z - Default_Rot.z > R_Max || Now_Pos_V3.x - Default_Rot.x < -U_Max || Now_Pos_V3.x - Default_Rot.x > D_Max)
			{
				OpenTouch = false;
				TouchInput = false;
				Return_Rot();
				Invoke("Touch_OK", BakeTime);
				return;

			}

			if (Input.GetMouseButtonDown(0))
			{
				TouchInput = true;
			
				//结束旋转任务
				MiniItween.DeleteType(this.gameObject, MiniItween.Type.Rotate);

				Old_Pos_V2 = Input.mousePosition;
				Old_Pos_V3 = this.gameObject.transform.localRotation.eulerAngles;
			}
			else if (Input.GetMouseButton(0) && TouchInput)
			{
				Vector2 tempMousePos = Input.mousePosition;
				//if (tempMousePos != New_Pos_V2)
				//{
				Vector3 Temp_Rot = Vector3.zero;
				Temp_Rot.x = (tempMousePos.y - Old_Pos_V2.y) * Time.deltaTime * MoveSpeed + Old_Pos_V3.x;
				Temp_Rot.y = 0;
				Temp_Rot.z = -(tempMousePos.x - Old_Pos_V2.x) * Time.deltaTime * MoveSpeed + Old_Pos_V3.z;

				this.gameObject.transform.localRotation = Quaternion.Euler(Temp_Rot);
				//}
			}
			else if (Input.GetMouseButtonUp(0))
			{
				TouchInput = true;
			}
		}
	
	}

	void Return_Rot()
	{
		Vector3 Go_pos;
		if (Now_Pos_V3.z - Default_Rot.z < -L_Max)
		{
			Go_pos =Now_Pos_V3 ;
			Go_pos.z += BakeDistance;
			//结束旋转任务
			MiniItween.DeleteType(this.gameObject, MiniItween.Type.Rotate);
			MiniItween.RotateTo(this.gameObject, Go_pos,BakeTime,MiniItween.EasingType.EaseInOutCubic);
		}
		else if (Now_Pos_V3.z - Default_Rot.z > R_Max)
		{
			Go_pos = Now_Pos_V3;
			Go_pos.z -= BakeDistance;
			//结束旋转任务
			MiniItween.DeleteType(this.gameObject, MiniItween.Type.Rotate);
			MiniItween.RotateTo(this.gameObject, Go_pos, BakeTime, MiniItween.EasingType.EaseInOutCubic);
		}
		else if (Now_Pos_V3.x - Default_Rot.x < -U_Max)
		{
			Go_pos = Now_Pos_V3;
			Go_pos.x += BakeDistance;
			//结束旋转任务
			MiniItween.DeleteType(this.gameObject, MiniItween.Type.Rotate);
			MiniItween.RotateTo(this.gameObject, Go_pos, BakeTime, MiniItween.EasingType.EaseInOutCubic);
		
		}
		else if (Now_Pos_V3.x - Default_Rot.x > D_Max)
		{
			Go_pos = Now_Pos_V3;
			Go_pos.x -= BakeDistance;
			//结束旋转任务
			MiniItween.DeleteType(this.gameObject, MiniItween.Type.Rotate);
			MiniItween.RotateTo(this.gameObject, Go_pos, BakeTime, MiniItween.EasingType.EaseInOutCubic);

		}
	
	}

	void Touch_OK()
	{
		OpenTouch = true;
	}
}
