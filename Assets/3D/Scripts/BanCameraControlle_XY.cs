using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BanCameraControlle_XY : MonoBehaviour
{

	//----CRLuo罗-----
	public Vector3 Default_Pos = new Vector3(7,1,3);
	public Vector3 Default_Rot = new Vector3( 0,270,0);
	public bool GoDefault_Key;
	//----CRLuo罗-----
	//摄像机最底位置
	public float CameraMinHeight = 1f;

	//高度范围增加
	public float CameraFOVAddHeight = 7f;
	public float CameraFOVAddHeight_LongKey = 5f;

	private List<GameObject> go_Objects = new List<GameObject>();

	//摄像机到人物的X轴向距离
	public float distance = 10;

	//角度平滑
	public float f_PingHua_Big = 10;
	public float f_PingHua_Small = 5;

	//位移平滑
    public float f_PingHua_Pos_Big = 5;
	public float f_PingHua_Pos_Small = 2;

	//摄像机角度对应的最小人物距离
	public float minDistance = 2;

	//人物距离左右边界的间隙
	public float f_Gap = 0.5f;
	//近距离晃动开关
	public bool MainShowKey = false;

	//bool b_Shake = false;

    private Camera MainCamera;
	//定义全局存在标记
	public static BanCameraControlle_XY Instance;

	public void AddRole(CRLuo_PlayAnim_FX aCRLuo_PlayAnim_FX)
	{
        go_Objects.Add(aCRLuo_PlayAnim_FX.mainMeshRender);
	}

	//移除角色网格队列
	public void RemoveRole(CRLuo_PlayAnim_FX aCRLuo_PlayAnim_FX)
	{
        go_Objects.Remove(aCRLuo_PlayAnim_FX.mainMeshRender);
	}

	//在所以程序之前运行
	void Awake() {
		//给存在标记赋值
		Instance = this;

        MainCamera = GetComponent<Camera>();
	}

	//开始运行
	void Start()
	{
		//开启默认摄像机标记
		GoDefault_Key = true;
	}

	//每一帧运行
	void Update()
	{
		//如果没有角色或开启复位运行
		if (go_Objects.Count == 0 || GoDefault_Key)
		{
			//位置复位
			GoDefault();
		}
		else
		{
			//否则定义模型网格左右上下的位置极限

			//左边为Z最大
			float left = float.MaxValue;
			//右边为Z最小
			float right = float.MinValue;
			//上面为Y最小
			float up = float.MinValue;
			//下面为Y最大
			float down = float.MaxValue;

			//遍历所有网格
			foreach (GameObject aGo in go_Objects)
			{
				//如果左边 大于  当前网格模型中心坐标.z - 当前网格宽度.z 
                if (left > aGo.renderer.bounds.center.x - aGo.renderer.bounds.extents.x)
				{
					//左边记录z最小值
                    left = aGo.renderer.bounds.center.x - aGo.renderer.bounds.extents.x;
				}

				//如果左边 小于  当前网格模型中心坐标.z + 当前网格宽度.z 
                if (right < aGo.renderer.bounds.center.x + aGo.renderer.bounds.extents.x)
				{
					//右边记录z最大值
                    right = aGo.renderer.bounds.center.x + aGo.renderer.bounds.extents.x;
				}

				//如果上边 大于  当前网格模型中心坐标.y + 当前网格高度.y 
                if (up < aGo.renderer.bounds.center.y + aGo.renderer.bounds.extents.y)
				{
					//上边记录y最大值
                    up = aGo.renderer.bounds.center.y + aGo.renderer.bounds.extents.y;
				}

				//如果上边 大于  当前网格模型中心坐标.y + 当前网格高度.y 
                if (down > aGo.renderer.bounds.center.y - aGo.renderer.bounds.extents.y)
                {
					//下边记录y最小值
                    down = aGo.renderer.bounds.center.y - aGo.renderer.bounds.extents.y;
				}

			}


			//-------------------Fov-------------------
			//定义最近距离标记
			bool b_InMinDistance;
			//模型宽度范围（a保存对角直边长）
			float a = (right - left) / 2;


			//如果a小于最金距离
			if (a < minDistance / 2)
			{
				//a固定为最近距离
				a = minDistance / 2;
				//开启最近距离标记
				b_InMinDistance = true;
            } else {
				//否则关闭最近距离标记
				b_InMinDistance = false;
			}

			if (a > CameraFOVAddHeight_LongKey)
			{
				float h = (up - down) / 2 + CameraFOVAddHeight;

				if (a < h)
				{
					a = h;
				}
			}

			//模型宽度范围 加上 人物距离左右边界的间隙
			a += f_Gap;

			//计算当前的画面比
			a = a * Screen.height / Screen.width;

			//记录摄像机到角色距离 b保存临角直边长
			float b = distance;

			//摄像机广角 = tan(对边比临边)*（π*180）弧度转角度*2倍夹角
			float targetFov = (Mathf.Atan(a / b) * 180 / Mathf.PI * 2);

            if (b_InMinDistance) {
                //进入极限距离
                MainCamera.fieldOfView = Mathf.Lerp(MainCamera.fieldOfView, targetFov, Time.deltaTime * f_PingHua_Small);
            } else {
                //如果不于极限
                //如果新焦距大于当前焦距
                if (targetFov > MainCamera.fieldOfView) {
                    //摄像机使用拉远速度改变焦距
                    MainCamera.fieldOfView = Mathf.Lerp(MainCamera.fieldOfView, targetFov, Time.deltaTime * f_PingHua_Big);
                } else {
                    //摄像机使用拉近速度改变焦距
                    MainCamera.fieldOfView = Mathf.Lerp(MainCamera.fieldOfView, targetFov, Time.deltaTime * f_PingHua_Small);
                }
            }
			

			//-------------------Pos-------------------

			//计算摄像机新位置（默认距离，上下中间值，左右中间值）
			Vector3 targetPos = new Vector3((left + right) / 2, (up + down) / 2, distance);
    		//如果新定位高度小于最小高度
            if (targetPos.y < CameraMinHeight) {
				//定位为最小高度
				targetPos.y = CameraMinHeight;
			}

			//如果进入极限距离
            if (b_InMinDistance) {
                //使用慢速位移
                this.transform.position = Vector3.Lerp(this.transform.position, targetPos, Time.deltaTime * f_PingHua_Pos_Small);
            } else {
				//使用快速位移
                this.transform.position = Vector3.Lerp(this.transform.position, targetPos, Time.deltaTime * f_PingHua_Pos_Big);

                if( Mathf.Abs(targetPos.z) <= 1.2f) {
                    transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
                }

			}
		}
	}



	//设置默认位置程序
    void GoDefault() {
		this.gameObject.transform.position = Default_Pos;
		this.gameObject.transform.rotation = Quaternion.Euler(Default_Rot);
        GoDefault_Key = false;
	}

//	void OnGUI() {
//
//		if (MainShowKey)
//		{
//
//			if (b_Shake)
//			{
//				if (GUI.Button(new Rect((Screen.width - 100) / 2, Screen.height / 6f * 5f, 100, Screen.height / 6), "no shake"))
//				{
//					b_Shake = !b_Shake;
//				}
//			}
//			else
//			{
//				if (GUI.Button(new Rect((Screen.width - 100) / 2, Screen.height / 6f * 5f, 100, Screen.height / 6), "shake"))
//				{
//					b_Shake = !b_Shake;
//				}
//			}
//		}
//		
//	}

}
