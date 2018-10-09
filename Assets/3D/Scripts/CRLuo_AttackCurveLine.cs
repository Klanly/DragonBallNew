using UnityEngine;
using System.Collections;

public class CRLuo_AttackCurveLine : MonoBehaviour
{

	//左右弧线偏移
	public Vector2 TrackRandomOffset_LR = new Vector2(-45f, 45f);
	//上下弧线偏移
	public Vector2 TrackRandomOffset_UD = new Vector2(-45f, 45f);
	//落点偏移
	public Vector3 v3_TargetRandomOffset = new Vector3(0.2f,0.2f,0.2f);
	//碰撞特效
	public GameObject FX_Boo;
	//攻击目标
	public GameObject target;

	//最终攻击目标
	private Vector3 finalPos;

	//过程时间
	public float f_Time = 3;
	//击中目标点的微调
	public float f_BeiShu = 5;

	//删除延迟
	public float DeleteSelfTime = 0.5f;

	//默认速度上限
	private float f_MaxSpeed = 50;
	//默认加速度
	private float f_AddSpeed = 50;
	//当前速度
	private Vector3 v3_Speed;

	void Start () {

		//设置随机偏移范围
		v3_TargetRandomOffset = new Vector3(
			Random.Range(v3_TargetRandomOffset.x, -v3_TargetRandomOffset.x),
			Random.Range(v3_TargetRandomOffset.y, -v3_TargetRandomOffset.y),
			Random.Range(v3_TargetRandomOffset.z, -v3_TargetRandomOffset.z)
		);

		if (target != null)
		{
			finalPos = target.transform.position + v3_TargetRandomOffset;
		}
		else
		{
			finalPos = v3_TargetRandomOffset;
		}


		//计算最大速度
		f_MaxSpeed = Vector3.Distance(finalPos,this.transform.position)/f_Time;
		//计算加速度
		f_AddSpeed = f_MaxSpeed * f_BeiShu;
		//横向偏移角度
		float angleX = Random.Range(TrackRandomOffset_UD.x, TrackRandomOffset_UD.y);
		//纵向偏移角度
		float angleY = Random.Range(TrackRandomOffset_LR.x, TrackRandomOffset_LR.y);
		//Z为前进方向
		float angleZ = 0;
		//设置初始角度四位数
		Quaternion rotation = Quaternion.Euler(angleX, angleY, angleZ);

		//求两点之间的单位向量（以this.transform.position为0点指向finalPos的单位1线段长度的XYZ坐标）
		Vector3 dir = (finalPos - this.transform.position).normalized;
		//最终角度 = 旋转向量*偏转单位向量得到正确的偏移后的角度
		dir = rotation * dir;
		//最终速度 = 初始角度*速度
		v3_Speed = dir * f_MaxSpeed;

		//地面烟尘特效生成
		if (Ground_FX != null)
		{
			//创建烟尘
			temp_Ground_FX = EmptyLoad.CreateObj(Ground_FX, new Vector3(this.transform.position.x, 0, this.transform.position.x), Quaternion.identity);
			//首先关闭显示
			PartSystem_ONOFF(true);

		}

	}

	private bool b_WillDie = false;

	//private bool b_InUse = false;

	void Update () {

		//烟尘特效是否存在
		if (Ground_FX != null)
		{
			//设置烟尘地面位置匹配
			temp_Ground_FX.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);
			//如果高度小于2显示
			if (this.transform.position.y < 2)
			{
				PartSystem_ONOFF(true);
			}
			else
			{
				PartSystem_ONOFF(false);
			}
		
		}

		//if (Input.GetKeyDown(KeyCode.A))
		//{
		//      b_InUse = true;
		//}

		//if (!b_InUse)
		//{
		//      return;
		//}

		//如果当前对象消失为真
		if(b_WillDie){
			//跳出程序，不继续运行
			return;
		}

		//当前位置 = 矢量速度*时间累计
		this.transform.position += v3_Speed * Time.deltaTime;
		//目标矢量 = 目标位置 - 当前位置
		Vector3 target_Minus_This = finalPos - this.transform.position;
		//如果距离速度与当前目标矢量距离 小于 0 说明到达攻击目标
		if( Vector3.Dot(v3_Speed,target_Minus_This) < 0 ){

			//准备删除当前物体
			Destroy(this.gameObject, DeleteSelfTime);
			//如果爆炸特效存在
			if(FX_Boo != null){
				//创建爆炸特效 创建特效（特效，当前位置，与父级或场景旋转对齐）
				EmptyLoad.CreateObj(FX_Boo,this.transform.position,Quaternion.identity);
			}
			//设置所有当前物体下的粒子关闭
			foreach(ParticleSystem aParticleSystem in GetComponentsInChildren<ParticleSystem>()){
				aParticleSystem.enableEmission = false;
			}

			foreach (MeshRenderer aRenderer in GetComponentsInChildren<MeshRenderer>())
			{
				aRenderer.enabled = false;
			}
			//当前对象消失为真
			b_WillDie = true;
			//跳出当前程序，屏蔽后方程序
			return;
		}

		//标准化当前矢量方向
		Vector3 dir = Vector3.Normalize(target_Minus_This);
		//加速度结果 = 标准化方向*加速度
		v3_Speed += dir * f_AddSpeed * Time.deltaTime;
		//如果当前速度平方（快速变成1维长度数据） 大于最大速度平方
		if (Vector3.SqrMagnitude(v3_Speed) > f_MaxSpeed * f_MaxSpeed)
		{
			//当前速度 = 矢量标准化单位*速度上限
			v3_Speed = Vector3.Normalize(v3_Speed) * f_MaxSpeed;
		}
	}

	//地面特效连接
	public GameObject Ground_FX;
	//地面特效临时存储变量
	GameObject temp_Ground_FX;
	//当物体删除时运行
	void OnDestroy()
	{
		//关闭所有特效
		PartSystem_ONOFF(false);
		//删除地面特效时间
		Destroy(temp_Ground_FX,1f);
	}

	//地面粒子开关集
	void PartSystem_ONOFF(bool Key)
	{
		//设置地面粒子显示开关
		if (temp_Ground_FX != null)
		{
			ParticleSystem[] PartTemp = temp_Ground_FX.GetComponentsInChildren<ParticleSystem>();
			foreach (ParticleSystem aPartTemp in PartTemp)
			{
				aPartTemp.enableEmission = Key;
			}


		}
	}
}
