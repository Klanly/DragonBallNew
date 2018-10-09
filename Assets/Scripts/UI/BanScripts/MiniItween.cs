//主要是针对itween在Time.scaleTime == 0的时候不能使用而编写完成
//现在完成了除曲线以外的其他功能 并且加入了一些的新的东西 （如MeshTo）
//Static和OtherStatic里面的方法是你可以调用的
//你可以自己设置DelayTime和最后结束时的委托Delegate
//大部分曲线公式来自于itween
//作者：Ban 于 2011年11月28日
//联系方式 ：邮箱bandingyue@163.com     QQ：1410539068
//最后，你的反馈是我更新的最大动力
using UnityEngine;
using System.Collections;
using ForMiniItween;

public class MiniItween : MonoBehaviour {

	#region Private

	public delegate void DelegateFunc();
	public delegate void DelegateFuncWithObject(GameObject go);

	private delegate float DelegateEasingFunc(float start, float end, float value);

	private event DelegateEasingFunc myDelegateEasingFunc;

	private void HandleEasing(EasingType aEasingType) {
		myDelegateEasingFunc = null;
		switch (aEasingType) {
			case EasingType.Normal:
				myDelegateEasingFunc += Normal;
				break;
			case EasingType.Bounce:
				myDelegateEasingFunc += Bounce;
				break;
			case EasingType.Bounce2:
				myDelegateEasingFunc += Bounce2;
				break;
			case EasingType.Bounce3:
				myDelegateEasingFunc += Bounce3;
				break;
			case EasingType.EaseOutQuint:
				myDelegateEasingFunc += EaseOutQuint;
				break;
			case EasingType.EaseOutSine:
				myDelegateEasingFunc += EaseOutSine;
				break;
			case EasingType.EaseInOutCirc:
				myDelegateEasingFunc += EaseInOutCirc;
				break;
			case EasingType.EaseInOutExpo:
				myDelegateEasingFunc += EaseInOutExpo;
				break;
			case EasingType.Linear:
				myDelegateEasingFunc += Linear;
				break;
			case EasingType.Clerp:
				myDelegateEasingFunc += Clerp;
				break;
			case EasingType.Spring:
				myDelegateEasingFunc += Spring;
				break;
			case EasingType.EaseInQuad:
				myDelegateEasingFunc += EaseInQuad;
				break;
			case EasingType.EaseOutQuad:
				myDelegateEasingFunc += EaseOutQuad;
				break;
			case EasingType.EaseInOutQuad:
				myDelegateEasingFunc += EaseInOutQuad;
				break;
			case EasingType.EaseInCubic:
				myDelegateEasingFunc += EaseInCubic;
				break;
			case EasingType.EaseOutCubic:
				myDelegateEasingFunc += EaseOutCubic;
				break;
			case EasingType.EaseInOutCubic:
				myDelegateEasingFunc += EaseInOutCubic;
				break;
			case EasingType.EaseInQuart:
				myDelegateEasingFunc += EaseInQuart;
				break;
			case EasingType.EaseOutQuart:
				myDelegateEasingFunc += EaseOutQuart;
				break;
			case EasingType.EaseInOutQuart:
				myDelegateEasingFunc += EaseInOutQuart;
				break;
			case EasingType.EaseInQuint:
				myDelegateEasingFunc += EaseInQuint;
				break;
			case EasingType.EaseInOutQuint:
				myDelegateEasingFunc += EaseInOutQuint;
				break;
			case EasingType.EaseInSine:
				myDelegateEasingFunc += EaseInSine;
				break;
			case EasingType.EaseInOutSine:
				myDelegateEasingFunc += EaseInOutSine;
				break;
			case EasingType.EaseInExpo:
				myDelegateEasingFunc += EaseInExpo;
				break;
			case EasingType.EaseOutExpo:
				myDelegateEasingFunc += EaseOutExpo;
				break;
			case EasingType.EaseInCirc:
				myDelegateEasingFunc += EaseInCirc;
				break;
			case EasingType.EaseOutCirc:
				myDelegateEasingFunc += EaseOutCirc;
				break;
			case EasingType.EaseInBack:
				myDelegateEasingFunc += EaseInBack;
				break;
			case EasingType.EaseOutBack:
				myDelegateEasingFunc += EaseOutBack;
				break;
			case EasingType.EaseInOutBack:
				myDelegateEasingFunc += EaseInOutBack;
				break;
			case EasingType.AnimationCurve:
				myDelegateEasingFunc += AnimationCurve;
				break;
			case EasingType.Pow:
				myDelegateEasingFunc += Pow;
				break;
			case EasingType.AnimationCurve2:
				myDelegateEasingFunc += AnimationCurve2;
				break;
		}
	}

	V4 ReturnValue(float value) {
		return new V4(startV4.x + (endV4.x - startV4.x) * value, startV4.y + (endV4.y - startV4.y) * value,
			startV4.z + (endV4.z - startV4.z) * value, startV4.a + (endV4.a - startV4.a) * value);
	}

	//已经逝去的时间
	private float passedTime = 0;

	#endregion

	#region Public

	/// <summary>
	/// 最后完成的时候想要完成啥代码就在这里加入吧
	/// </summary>
	public event DelegateFunc myDelegateFunc;
	public event DelegateFuncWithObject myDelegateFuncWithObject;
    public System.Action FinishedAnim;

	//是否使用世界坐标
	[System.NonSerialized]
	public bool b_useWorld = false;

	//是否解决ScaleTime
	[System.NonSerialized]
	public bool b_handleScaleTime = false;//当为true的时候 scale 为 0 则不运转

	[System.NonSerialized]
	public float startTime;

	[System.NonSerialized]
	public float duringTime;

	[System.NonSerialized]
	public V4 startV4;

	[System.NonSerialized]
	public V4 endV4;

	[System.NonSerialized]
	public float f_DelayTime = 0;

	//额外数据
	public ExtraData myExtraData = null;

	//返回剩余时间
	public float ReturnLeftTime() {
		return duringTime - passedTime;
	}

	#endregion

	#region OTHER STATIC FUNCTION
	public static void TrunOverMesh(GameObject targetGO) {
		Mesh mesh = targetGO.GetComponent<MeshFilter>().sharedMesh;
		int[] triangles = mesh.triangles;
		for (int i = 0; i < triangles.Length; i += 3) {
			int temp = triangles[i + 1];
			triangles[i + 1] = triangles[i + 2];
			triangles[i + 2] = temp;
		}
		mesh.triangles = triangles;
	}

	/// <summary>
	/// 默认使用sharedMesh
	/// </summary>
	public static void SetVertexColor(GameObject target,V4 v4_To) {
		VertexColor color = new VertexColor(target.GetComponent<MeshFilter>(),true);
		color.SetColor(v4_To);
		//try {
		//      VertexColor color = new VertexColor(target.GetComponent<MeshFilter>().sharedMesh);
		//      color.SetColor(v4_To);
		//}catch(System.Exception e){
		//      Debug.Log(e);
		//      Debug.Log(target.name);
		//}
	}

	/// <summary>
	/// 是否使用sharedMesh
	/// </summary>
	public static void SetVertexColor(GameObject target, V4 v4_To,bool b_UseShared) {
		VertexColor color = new VertexColor(target.GetComponent<MeshFilter>(), b_UseShared);
		color.SetColor(v4_To);
	}

	#endregion

	#region STATIC
//	static MiniItween MoveTo(GameObject target, V4 v4, float time, EasingType aEasingType, bool b_useWorld) {
//		MiniItween script = target.AddComponent<MiniItween>();
//		script.b_useWorld = b_useWorld;
//		script.myType = Type.Move;
//		script.myEasingType = aEasingType;
//		script.startTime = Time.realtimeSinceStartup;
//		script.duringTime = time;
//		script.endV4 = v4;
//		if (b_useWorld) {
//			script.startV4 = new V4(script.transform.position);
//		} else {
//			script.startV4 = new V4(script.transform.localPosition);
//		}
//		return script;
//	}

	public static GameObject miniItween_TimeScaleItem;

	/// <summary>
	/// 可以实现慢动作或者快进
	/// </summary>
	public static void TimeScale(AnimationCurve curve,float time){

		if(miniItween_TimeScaleItem != null){
			GameObject.Destroy(miniItween_TimeScaleItem);
			miniItween_TimeScaleItem = null;
		}

		miniItween_TimeScaleItem = new GameObject("MiniItween_TimeScaleItem");

		MiniItween itween = miniItween_TimeScaleItem.AddComponent<MiniItween>();

		itween.b_handleScaleTime = false;

		itween.myType = Type.TimeScale;

		itween.myEasingType = EasingType.AnimationCurve2;

		itween.myExtraData = new ExtraData(curve);

		itween.startTime = Time.realtimeSinceStartup;

		itween.duringTime = time;	

	}

	/// <summary>
	/// 删除某种类型
	/// </summary>
	public static void DeleteType(GameObject target, MiniItween.Type deleteType) {
		MiniItween itween = target.GetComponent<MiniItween>();
		if (itween != null) {
			if (itween.myType == deleteType) {
				Destroy(itween);
			}
		}
	}

	#region RotateAround

	public static MiniItween RotateAround(GameObject target,Vector3 point, Vector3 axis, float angle, float time) {
		return RotateAround(target, point, axis, angle, time, EasingType.Normal);
	}

	public static MiniItween RotateAround(GameObject target, Vector3 point, Vector3 axis, float angle, float time, MiniItween.EasingType aEasingType) {
		return RotateAround(target, point, axis, angle, time, aEasingType,true);
	}

	public static MiniItween RotateAround(GameObject target, Vector3 point, Vector3 axis, float angle, float time, MiniItween.EasingType aEasingType,bool rotateSelf) {
		MiniItween script = target.AddComponent<MiniItween>();
		script.myType = Type.RotateAround;
		script.myEasingType = aEasingType;
		script.startTime = Time.realtimeSinceStartup;
		script.duringTime = time;
		script.myExtraData = new ExtraData(new RotateAroundInfo(target.transform.position, target.transform.rotation, rotateSelf, point, angle, axis));
		return script;
	}

	/// <summary>
	/// 这个旋转轴默认为   Vector3.back
	/// 确定目标位置和旋转弧度直接计算出旋转点
	/// 比较方便些
	/// </summary>
	/// <param name="target"></param>
	/// <param name="toPos"></param>
	/// <param name="angle"></param>
	/// <param name="time"></param>
	/// <param name="aEasingType"></param>
	/// <param name="rotateSelf"></param>
	/// <param name="prefer">1表示不变，2表示变化，3另一种变化，4表示完全随机</param>
	/// <returns></returns>
	public static MiniItween RotateAround(GameObject target, Vector3 toPos, float angle, float time, MiniItween.EasingType aEasingType, bool rotateSelf,int prefer) {
		MiniItween script = target.AddComponent<MiniItween>();
		script.myType = Type.RotateAround;
		script.myEasingType = aEasingType;
		script.startTime = Time.realtimeSinceStartup;
		script.duringTime = time;

		Vector3 v3_FromTo = new Vector3(toPos.x - target.transform.position.x,toPos.y - target.transform.position.y,0);
		Vector3 v3_Verti = Vector3.Cross(v3_FromTo,Vector3.forward);
		v3_Verti.Normalize();

		angle = Mathf.Abs(angle);

		switch(prefer){
			case 1:
				break;
			case 2:
				if (v3_Verti.x < 0) {
					angle = -angle;
				}
				break;
			case 3:
				if (v3_Verti.y > 0) {
					v3_Verti = -v3_Verti;
					angle = -angle;
				}
				break;
			case 4:
				if (Random.Range(0, 2) == 0) {
					angle = -angle;
				}
				break;
			default:
				Debug.Log("就不能看看说明吗，啊");
				break;
		}

		//这便是旋转点
		Vector3 point = (target.transform.position + toPos) / 2/*中点*/ + Vector3.Distance(target.transform.position, toPos) / 2 * Mathf.Tan( (180 - angle)/2*Mathf.PI/180 )*v3_Verti;

		//显示中心点
		//GameObject tempPoint = GameObject.CreatePrimitive(PrimitiveType.Cube);
		//tempPoint.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
		//tempPoint.transform.position = point;

		script.myExtraData = new ExtraData(new RotateAroundInfo(target.transform.position, target.transform.rotation, rotateSelf,point , angle, Vector3.back));
		return script;
	}

	#endregion

	#region MeshAnim

	/// <summary>
	/// 这个方法用于已经有MeshAnim
	/// </summary>
	public static MiniItween MeshTo(GameObject aGameObject, V4 V4_To, float time, EasingType aEasingType) {
		//首先检测是否有MeshAnim
		MeshAnim meshAnim = aGameObject.GetComponent<MeshAnim>();
		if (meshAnim == null) {
			Debug.LogError("It does not have a Mesh");
			return null;
		}
		//下面生成Miniitween
		MiniItween script = aGameObject.AddComponent<MiniItween>();
		script.myExtraData = new ExtraData(meshAnim);
		script.myType = Type.MeshAnim;
		script.myEasingType = aEasingType;
		script.startTime = Time.realtimeSinceStartup;
		script.duringTime = time;
		script.startV4 = new V4(meshAnim.curClip);
		script.endV4 = V4_To;
		return script;
	}

	/// <summary>
	/// 这个方法用于已经有MeshAnim
	/// </summary>
	public static MiniItween MeshFromTo(GameObject aGameObject,V4 V4_From, V4 V4_To, float time, EasingType aEasingType) {
		//首先检测是否有MeshAnim
		MeshAnim meshAnim = aGameObject.GetComponent<MeshAnim>();
		if (meshAnim == null) {
			Debug.LogError("It does not have a Mesh");
			return null;
		}
		meshAnim.SetClip(V4_From);
		//下面生成Miniitween
		MiniItween script = aGameObject.AddComponent<MiniItween>();
		script.myExtraData = new ExtraData(meshAnim);
		script.myType = Type.MeshAnim;
		script.myEasingType = aEasingType;
		script.startTime = Time.realtimeSinceStartup;
		script.duringTime = time;
		script.startV4 = V4_From;
		script.endV4 = V4_To;
		return script;
	}

	/// <summary>
	/// 这个方法用于之前没有MeshAnim
	/// </summary>
	public static MiniItween MeshTo(GameObject aGameObject, V4 V4_To, float time, EasingType aEasingType, int[] vertexIndex, bool IsXY) {
		return MeshFromTo(aGameObject, new V4(0, 1, 1, 0), V4_To, time, aEasingType, vertexIndex, IsXY);
	}

	/// <summary>
	/// 这个方法用于之前没有MeshAnim
	/// </summary>
	public static MiniItween MeshFromTo(GameObject aGameObject,V4 V4_From, V4 V4_To, float time, EasingType aEasingType, int[] vertexIndex, bool IsXY) {
		//判断是否已经有了
		MeshAnim meshAnim = aGameObject.GetComponent<MeshAnim>();
		if (meshAnim != null) {
			Debug.LogError("It already has a MeshAnim with it");
			return null;
		}
		//生成MeshAnim
		meshAnim = aGameObject.AddComponent<MeshAnim>();
		meshAnim.Init(vertexIndex);
		if (IsXY) {
			meshAnim.myXYZ = MeshAnim.XYZ.XY;
		} else {
			meshAnim.myXYZ = MeshAnim.XYZ.XZ;
		}
		meshAnim.SetClip(V4_From);
		//下面生成MeshAnim
		MiniItween script = aGameObject.AddComponent<MiniItween>();
		script.myExtraData = new ExtraData(meshAnim);
		script.myType = Type.MeshAnim;
		script.myEasingType = aEasingType;
		script.startTime = Time.realtimeSinceStartup;
		script.duringTime = time;
		script.startV4 = V4_From;
		script.endV4 = V4_To;
		return script;
	}

	#endregion

	#region Shake

	/// <summary>
	/// Vector3代表振幅
	/// </summary>
	/// <param name="target"></param>
	/// <param name="v3"></param>
	/// <param name="time"></param>
	/// <param name="aEasingType"></param>
	/// <returns></returns>
	public static MiniItween Shake(GameObject target, Vector3 v3, float time,EasingType aEasingType, bool b_useWorld = true) {
		//首先检测
		MiniItween.DeleteType(target, MiniItween.Type.Shake);
		//接下来这样
		MiniItween script = target.AddComponent<MiniItween>();
		script.b_useWorld = b_useWorld;
		script.myType = Type.Shake;
		script.myEasingType = aEasingType;
		script.startTime = Time.realtimeSinceStartup;
		script.duringTime = time;
		if(b_useWorld)
		{
			script.startV4 = new V4(script.transform.position);
		}
		else
		{
			script.startV4 = new V4(script.transform.localPosition);
		}
		script.endV4 = new V4(v3);
		return script;
	}

	/// <summary>
	/// 这是最后一个参数确定了起始点的方法
	/// </summary>
	/// <param name="target"></param>
	/// <param name="v3"></param>
	/// <param name="time"></param>
	/// <param name="aEasingType"></param>
	/// <param name="?"></param>
	/// <returns></returns>
	public static MiniItween Shake(GameObject target, Vector3 v3, float time, EasingType aEasingType,Vector3 v3_start, bool b_useWorld) {
		//首先检测
		MiniItween.DeleteType(target, MiniItween.Type.Shake);
		//接下来这样
		MiniItween script = target.AddComponent<MiniItween>();
		script.b_useWorld = b_useWorld;
		script.myType = Type.Shake;
		script.myEasingType = aEasingType;
		script.startTime = Time.realtimeSinceStartup;
		script.duringTime = time;
		script.startV4 = new V4(v3_start);
		script.endV4 = new V4(v3);
		return script;
	}

	public static MiniItween Shake(GameObject target,V4 v4,float time,EasingType aEasingType, bool b_useWorld){
		//首先检测
		MiniItween.DeleteType(target, MiniItween.Type.Shake);
		//接下来这样
		MiniItween script = target.AddComponent<MiniItween>();

		script.myType = Type.Shake;
		script.myEasingType = aEasingType;
		script.startTime = Time.realtimeSinceStartup;
		script.duringTime = time;

		if(b_useWorld)
		{
			script.startV4 = new V4(script.transform.position);
		}
		else
		{
			script.startV4 = new V4(script.transform.localPosition);
		}
		script.endV4 = v4;
		return script;
	}

	#endregion

	#region RotateTo

	public static MiniItween RotateTo(GameObject target, Vector3 v3, float time) {
		return RotateTo(target,Quaternion.Euler(v3),time,EasingType.Normal,false);
	}

	public static MiniItween RotateTo(GameObject target, Quaternion rotation, float time) {
		return RotateTo(target,rotation,time,EasingType.Normal,false);
	}

	public static MiniItween RotateTo(GameObject target,Vector3 v3,float time,bool b_useWorld) {
		return RotateTo(target, Quaternion.Euler(v3), time, EasingType.Normal, b_useWorld);
	}

	public static MiniItween RotateTo(GameObject target, Quaternion rotation, float time, bool b_useWorld) {
		return RotateTo(target, rotation, time, EasingType.Normal, b_useWorld);
	}

	public static MiniItween RotateTo(GameObject target, Vector3 v3, float time, EasingType aEasingType) {
		return RotateTo(target, Quaternion.Euler(v3), time, aEasingType, false);
	}

	public static MiniItween RotateTo(GameObject target, Quaternion rotation, float time, EasingType aEasingType) {
		return RotateTo(target,rotation,time,aEasingType,false);
	}

	public static MiniItween RotateTo(GameObject target, Vector3 v3, float time, EasingType aEasingType, bool b_useWorld) {
		return RotateTo(target,Quaternion.Euler(v3),time,aEasingType,b_useWorld );
	}

	public static MiniItween RotateTo(GameObject target, Quaternion rotation, float time, EasingType aEasingType, bool b_useWorld) {
		MiniItween script = target.AddComponent<MiniItween>();
		script.myType = Type.Rotate;
		script.myEasingType = aEasingType;
		script.startTime = Time.realtimeSinceStartup;
		script.duringTime = time;
		script.endV4 = new V4(rotation);
		script.b_useWorld = b_useWorld;
		if (b_useWorld) {
			script.startV4 = new V4(target.transform.rotation);
		} else {
			script.startV4 = new V4(target.transform.localRotation);
		}
		return script;
	}

	#endregion

	#region MoveTo

	public static MiniItween MoveTo(GameObject target, Vector3 v3, float time) {
		return MoveTo(target, v3, time, EasingType.Normal, false);
	}

	public static MiniItween MoveTo(GameObject target, Vector3 v3, float time, bool b_useWorld) {
		return MoveTo(target, v3, time, EasingType.Normal, b_useWorld);
	}

	public static MiniItween MoveTo(GameObject target, Vector3 v3, float time, EasingType aEasingType) {
		return MoveTo(target, v3, time, aEasingType, false);
	}

	public static MiniItween MoveTo(GameObject target, Vector3 v3, float time, EasingType aEasingType, bool b_useWorld) {
		return MoveTo(target, new V4(v3), time, aEasingType, b_useWorld);
	}

	static MiniItween MoveTo(GameObject target, V4 v4, float time, EasingType aEasingType, bool b_useWorld) {
		MiniItween script = target.AddComponent<MiniItween>();
		script.b_useWorld = b_useWorld;
		script.myType = Type.Move;
		script.myEasingType = aEasingType;
		script.startTime = Time.realtimeSinceStartup;
		script.duringTime = time;
		script.endV4 = v4;
		if (b_useWorld) {
			script.startV4 = new V4(script.transform.position);
		} else {
			script.startV4 = new V4(script.transform.localPosition);
		}
		return script;
	}

	#endregion

	#region MoveArcTo

	public static MiniItween MoveArcTo(GameObject target,Vector3 v3,float time,EasingType aEasingType,AnimationCurve aAnimationCurve,bool useWorld){
		return MoveArcTo(target,new V4(v3.x,v3.y,v3.z,1),time,aEasingType,aAnimationCurve,useWorld);
	}

	public static MiniItween MoveArcTo(GameObject target,V4 v4,float time,EasingType aEasingType,AnimationCurve aAnimationCurve,bool useWorld){

		GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = target.transform.position;

		GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = v4.Vector3Value();

		MiniItween itween = target.AddComponent<MiniItween>();
		itween.myType = Type.MoveArc;
		itween.myEasingType = aEasingType;
		itween.startTime = Time.realtimeSinceStartup;
		itween.duringTime = time;
		itween.endV4 = v4;
		itween.b_useWorld = useWorld;
		if(useWorld){
			itween.startV4 = new V4(target.transform.position);
		}else{
			itween.startV4 = new V4(target.transform.localPosition);
		}
		itween.myExtraData = new ExtraData(aAnimationCurve);
		itween.myExtraData.v3_Temp = (itween.startV4.Vector3Value() - v4.Vector3Value()).normalized;
		itween.myExtraData.v3_Temp = Vector3.Cross( itween.myExtraData.v3_Temp,Vector3.back );
		return itween;
	}

	#endregion

	#region ScaleTo

	public static MiniItween ScaleTo(GameObject target, Vector3 v3, float time) {
		return ScaleTo(target,v3,time,EasingType.Normal);
	}

	public static MiniItween ScaleTo(GameObject target, Vector3 v3, float time, EasingType aEasingType) {
		return ScaleTo(target, new V4(v3), time, aEasingType);
	}

	static MiniItween ScaleTo(GameObject target, V4 v4, float time, EasingType aEasingType) {
		MiniItween script = target.AddComponent<MiniItween>();
		script.myType = Type.Scale;
		script.myEasingType = aEasingType;
		script.startTime = Time.realtimeSinceStartup;
		script.duringTime = time;
		script.endV4 = v4;
		script.startV4 = new V4(script.transform.localScale);
		return script;
	}

	#endregion

	#region ColorTo

	public static MiniItween ColorFromTo(GameObject target, V4 v4_From,V4 v4_To, float time, MiniItween.EasingType aEasingType, Type type = Type.Color) {
		//Debug.Log("Warnning");
		MiniItween script = target.AddComponent<MiniItween>();
		script.myType = type;
		script.myEasingType = aEasingType;
		script.startTime = Time.realtimeSinceStartup;
		script.duringTime = time;
		script.endV4 = v4_To;
		script.startV4 = v4_From;
		if(type == Type.Color)
		{
			script.renderer.material.color = v4_From.ColorValue();
		}
		else if(type == Type.ColorWidget)
		{
			UIWidget widget = target.GetComponent<UIWidget>();

			widget.color = v4_From.ColorValue();
		}
		return script;
	}

	public static MiniItween ColorTo(GameObject target, V4 v4, float time, Type type = Type.Color) {
//		Debug.Log("Warnning");
		MiniItween script = target.AddComponent<MiniItween>();
		script.myType = type;
		script.myEasingType = EasingType.Normal;
		script.startTime = Time.realtimeSinceStartup;
		script.duringTime = time;
		script.endV4 = v4;
		if(type == Type.Color)
		{
			script.startV4 = new V4(script.renderer.material.color.r, script.renderer.material.color.g,
				script.renderer.material.color.b, script.renderer.material.color.a);
		}
		else if(type == Type.ColorWidget)
		{
			UIWidget widget = target.GetComponent<UIWidget>();

			script.startV4 = new V4(widget.color.r, widget.color.g, widget.color.b, widget.color.a);
		}
		return script;
	}

	public static MiniItween ColorTo(GameObject target, V4 v4, float time, MiniItween.EasingType aEasingType, Type type = Type.Color) {
//		Debug.Log("Warnning");
		MiniItween script = target.AddComponent<MiniItween>();
		script.myType = type;
		script.myEasingType = aEasingType;
		script.startTime = Time.realtimeSinceStartup;
		script.duringTime = time;
		script.endV4 = v4;

		if(type == Type.Color)
		{
			script.startV4 = new V4(script.renderer.material.color.r, script.renderer.material.color.g,
				script.renderer.material.color.b, script.renderer.material.color.a);
		}
		else if(type == Type.ColorWidget)
		{
			UIWidget widget = target.GetComponent<UIWidget>();

			script.startV4 = new V4(widget.color.r, widget.color.g, widget.color.b, widget.color.a);
		}
		return script;
	}

	#endregion

	#region VertexColor

	/// <summary>
	/// 默认情况下使用sharedMesh 
	/// </summary>
	public static MiniItween VertexColorTo(GameObject target, V4 v4, float time, MiniItween.EasingType aEasingType) {
		return VertexColorTo(target, v4, time, aEasingType, true);
	}

	//等待一帧
	public static IEnumerator VertexColorToWaitAFrame(GameObject target, V4 v4, float time, MiniItween.EasingType aEasingType) {
		yield return null;
		VertexColorTo(target, v4, time, aEasingType, true);
	}

	public static MiniItween VertexColorTo(GameObject target, V4 v4, float time, MiniItween.EasingType aEasingType,bool UseShared) {
		MiniItween script = target.AddComponent<MiniItween>();
		script.myExtraData = new ExtraData(new VertexColor(target.GetComponent<MeshFilter>(), UseShared));
		script.myType = Type.VertexColor;
		script.myEasingType = aEasingType;
		script.startTime = Time.realtimeSinceStartup;
		script.duringTime = time;
		script.endV4 = v4;
		Color fromColor = script.myExtraData.vertexColor.GetColor();
		script.startV4 = new V4(fromColor.r, fromColor.g, fromColor.b, fromColor.a);
		//Debug.Log("RGBA"+fromColor.r+" "+fromColor.g+" "+fromColor.b+" "+fromColor.a);
		return script;
	}

	/// <summary>
	/// 默认情况下使用sharedMesh
	/// </summary>
	public static MiniItween VertexColorFromTo(GameObject target,V4 v4_From, V4 v4_To, float time, MiniItween.EasingType aEasingType) {
		return VertexColorFromTo(target, v4_From, v4_To, time, aEasingType,true);
	}

	//等待一帧
	public static IEnumerator VertexColorFromToWaitAFrame(GameObject target, V4 v4_From, V4 v4_To, float time, MiniItween.EasingType aEasingType) {
		yield return null;
		VertexColorFromTo(target, v4_From, v4_To, time, aEasingType, true);
	}

	public static MiniItween VertexColorFromTo(GameObject target, V4 v4_From, V4 v4_To, float time, MiniItween.EasingType aEasingType, bool UseShared) {
		MiniItween script = target.AddComponent<MiniItween>();
		script.myExtraData = new ExtraData( new VertexColor(target.GetComponent<MeshFilter>(),UseShared) );
		script.myExtraData.vertexColor.SetColor(v4_From);
		script.myType = Type.VertexColor;
		script.myEasingType = aEasingType;
		script.startTime = Time.realtimeSinceStartup;
		script.duringTime = time;
		script.endV4 = v4_To;
		script.startV4 = v4_From;
		return script;
	}

	#endregion

	#region V4Delegate

	public static MiniItween V4DelegateFromTo(GameObject target, V4 v4_From,V4 v4_To, float time, MiniItween.EasingType aEasingType,ExtraData.V4Delegate V4Delegate) {
		MiniItween script = target.AddComponent<MiniItween>();
		script.myExtraData = new ExtraData(V4Delegate);
		script.myExtraData.HandleV4Delegate(v4_From);
		script.myType = Type.V4Delegate;
		script.myEasingType = aEasingType;
		script.startTime = Time.realtimeSinceStartup;
		script.duringTime = time;
		script.endV4 = v4_To;
		script.startV4 = v4_From;
		return script;
	}

	#endregion

	#endregion

	#region 曲线公式

	private float EaseOutExpo(float start, float end, float value) {
		end -= start;
		return end * (-Mathf.Pow(2, -10 * value / 1) + 1) + start;
	}

	private float EaseInSine(float start, float end, float value) {
		end -= start;
		return -end * Mathf.Cos(value / 1 * (Mathf.PI / 2)) + end + start;
	}

	private float Normal(float start, float end, float value) {
		return value * (end - start) + start;
	}

	public static float Bounce(float start, float end, float value) {
		value /= 1f;
		end -= start;
		if (value < (1 / 2.75f)) {
			return end * (7.5625f * value * value) + start;
		} else if (value < (2 / 2.75f)) {
			value -= (1.5f / 2.75f);
			return end * (7.5625f * (value) * value + .75f) + start;
		} else if (value < (2.5 / 2.75)) {
			value -= (2.25f / 2.75f);
			return end * (7.5625f * (value) * value + .9375f) + start;
		} else {
			value -= (2.625f / 2.75f);
			return end * (7.5625f * (value) * value + .984375f) + start;
		}
	}

	public static float Bounce2(float start,float end,float value) {
		value /= 1f;
		end -= start;
		float firstPt = 0.5f;
		float secondPt = 0.7f;
		float thirdPt = 0.9f;
		float fourthPt = 1f;
		
		//float firstReach = 0.9f;
		//float secondReach = 0.95f;
		//float thirdReach = 0.98f;
		float firstReach = 0.85f;
		float secondReach = 0.9f;
		float thirdReach = 0.95f;
		//float fourthReach = 1f;

		if (value < firstPt) {
			return end * (value * value / (firstPt * firstPt))+start;
		} else if (value < secondPt) {
			float a = (1 - firstReach) / ((secondPt / 2 - firstPt / 2) * (secondPt / 2 - firstPt / 2));
			return end * (a * (value - (firstPt + secondPt) / 2) * (value - (firstPt + secondPt) / 2) + firstReach) + start;
		} else if (value < thirdPt) {
			float a = (1 - secondReach) / ((thirdPt / 2 - secondPt / 2) * (thirdPt / 2 - secondPt / 2));
			return end * (a * (value - (secondPt + thirdPt) / 2) * (value - (secondPt + thirdPt) / 2) + secondReach) + start;
		} else if (value < fourthPt) {
			float a = (1 - thirdReach) / ((fourthPt / 2 - thirdPt / 2) * (fourthPt / 2 - thirdPt / 2));
			return end * (a * (value - (thirdPt + fourthPt) / 2) * (value - (thirdPt + fourthPt) / 2) + thirdReach) + start;
		} else {
			return end * 1 + start;
		}
	}

	public static float Bounce3(float start, float end, float value) {
		float weight = 0.05f;
		//float weight2 = 0.01f;
		float firstPt = 0.6f;

		if (value < firstPt) {
			return Mathf.Sin((value - firstPt / 2) * Mathf.PI * 1 / firstPt) / 2 + 0.5f;
		} else {//if (value < 0.8f) {
			return (Mathf.Sin((value - (firstPt - (1 - firstPt) / 2)) * Mathf.PI * 2 / (1 - firstPt) - Mathf.PI / 2) / 2 + 0.5f)
				* weight + (1 - weight);
		}
		//else {
		//      return (Mathf.Sin(value * Mathf.PI * 2 / 0.2f + Mathf.PI / 2) / 2 + 0.5f) * weight2 + (1 - weight2);
		//}
		//return 1;
	}

	private float EaseOutQuint(float start, float end, float value) {
		value--;
		end -= start;
		return end * (value * value * value * value * value + 1) + start;
	}

	private float EaseOutSine(float start, float end, float value) {
		end -= start;
		return end * Mathf.Sin(value / 1 * (Mathf.PI / 2)) + start;
	}

	private float EaseInOutExpo(float start, float end, float value) {
		value /= .5f;
		end -= start;
		if (value < 1) return end / 2 * Mathf.Pow(2, 10 * (value - 1)) + start;
		value--;
		return end / 2 * (-Mathf.Pow(2, -10 * value) + 2) + start;
	}

	private float Linear(float start, float end, float value) {
		return Mathf.Lerp(start, end, value);
	}

	private float Clerp(float start, float end, float value) {
		float min = 0.0f;
		float max = 360.0f;
		float half = Mathf.Abs((max - min) / 2.0f);
		float retval = 0.0f;
		float diff = 0.0f;

		if ((end - start) < -half) {
			diff = ((max - start) + end) * value;
			retval = start + diff;
		} else if ((end - start) > half) {
			diff = -((max - end) + start) * value;
			retval = start + diff;
		} else retval = start + (end - start) * value;
		return retval;
	}

	private float Spring(float start, float end, float value) {
		value = Mathf.Clamp01(value);
		value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
		return start + (end - start) * value;
	}

	private float EaseInQuad(float start, float end, float value) {
		end -= start;
		return end * value * value + start;
	}

	private float EaseOutQuad(float start, float end, float value) {
		end -= start;
		return -end * value * (value - 2) + start;
	}

	private float EaseInOutQuad(float start, float end, float value) {
		value /= .5f;
		end -= start;
		if (value < 1) return end / 2 * value * value + start;
		value--;
		return -end / 2 * (value * (value - 2) - 1) + start;
	}

	private float EaseInCubic(float start, float end, float value) {
		end -= start;
		return end * value * value * value + start;
	}

	private float EaseOutCubic(float start, float end, float value) {
		value--;
		end -= start;
		return end * (value * value * value + 1) + start;
	}

	private float EaseInOutCubic(float start, float end, float value) {
		value /= .5f;
		end -= start;
		if (value < 1) return end / 2 * value * value * value + start;
		value -= 2;
		return end / 2 * (value * value * value + 2) + start;
	}

	private float EaseInQuart(float start, float end, float value) {
		end -= start;
		return end * value * value * value * value + start;
	}

	private float EaseOutQuart(float start, float end, float value) {
		value--;
		end -= start;
		return -end * (value * value * value * value - 1) + start;
	}

	public static float EaseInOutQuart(float start, float end, float value) {
		value /= .5f;
		end -= start;
		if (value < 1) return end / 2 * value * value * value * value + start;
		value -= 2;
		return -end / 2 * (value * value * value * value - 2) + start;
	}

	private float EaseInQuint(float start, float end, float value) {
		end -= start;
		return end * value * value * value * value * value + start;
	}

	private float EaseInOutQuint(float start, float end, float value) {
		value /= .5f;
		end -= start;
		if (value < 1) return end / 2 * value * value * value * value * value + start;
		value -= 2;
		return end / 2 * (value * value * value * value * value + 2) + start;
	}

	private float EaseInOutSine(float start, float end, float value) {
		end -= start;
		return -end / 2 * (Mathf.Cos(Mathf.PI * value / 1) - 1) + start;
	}

	private float EaseInExpo(float start, float end, float value) {
		end -= start;
		return end * Mathf.Pow(2, 10 * (value / 1 - 1)) + start;
	}

	private float EaseInCirc(float start, float end, float value) {
		end -= start;
		return -end * (Mathf.Sqrt(1 - value * value) - 1) + start;
	}

	private float EaseOutCirc(float start, float end, float value) {
		value--;
		end -= start;
		return end * Mathf.Sqrt(1 - value * value) + start;
	}

	private float EaseInOutCirc(float start, float end, float value) {
		value /= .5f;
		end -= start;
		if (value < 1) return -end / 2 * (Mathf.Sqrt(1 - value * value) - 1) + start;
		value -= 2;
		return end / 2 * (Mathf.Sqrt(1 - value * value) + 1) + start;
	}

	private float EaseInBack(float start, float end, float value) {
		end -= start;
		value /= 1;
		float s = 1.70158f;
		return end * (value) * value * ((s + 1) * value - s) + start;
	}

	public static float EaseOutBack(float start, float end, float value) {
		float s = 1.70158f;
		end -= start;
		value = (value / 1) - 1;
		return end * ((value) * value * ((s + 1) * value + s) + 1) + start;
	}

	private float EaseInOutBack(float start, float end, float value) {
		float s = 1.70158f;
		end -= start;
		value /= .5f;
		if ((value) < 1) {
			s *= (1.525f);
			return end / 2 * (value * value * (((s) + 1) * value - s)) + start;
		}
		value -= 2;
		s *= (1.525f);
		return end / 2 * ((value) * value * (((s) + 1) * value + s) + 2) + start;
	}

	public float AnimationCurve(float start,float end,float value) {
		end -= start;

		value = this.myExtraData.animationCurve.Evaluate(value);
		return end * value + start;
	}

	//Just for TimeScale
	public float AnimationCurve2(float start,float end,float value){
		if(value >= 1){
			Destroy(this.gameObject,0.5f);
		}
		return this.myExtraData.animationCurve.Evaluate(value);
	}

	public float Pow(float start, float end, float value) {
		end -= start;
		value = value * value;
		return end * value + start;
	}

	#endregion
	
	#region EasingType

	public enum EasingType { Normal, Bounce,Bounce2,Bounce3, EaseOutQuint, EaseOutSine, EaseInOutCirc, EaseInOutExpo,
	Linear, Clerp, Spring, EaseInQuad, EaseOutQuad, EaseInOutQuad, EaseInCubic, EaseOutCubic, EaseInOutCubic, EaseInQuart, EaseOutQuart,
	EaseInOutQuart, EaseInQuint, EaseInOutQuint, EaseInSine, EaseInOutSine, EaseInExpo, EaseOutExpo,
	EaseInCirc, EaseOutCirc, EaseInBack, EaseOutBack, EaseInOutBack,AnimationCurve,Pow,AnimationCurve2};
		
	[HideInInspector]
	public EasingType myEasingType;

	#endregion

	#region Type

	public enum Type { Move, Scale, Color, ColorWidget, Rotate, Shake, MeshAnim, RotateAround, VertexColor , TimeScale, V4Delegate , MoveArc };

	public Type myType{
		set{
			_myType = value;
			myTypeFunc = null;
			switch(value){
				case Type.Move:
					myTypeFunc += MoveFunc;
					break;
				case Type.Scale:
					myTypeFunc += ScaleFunc;
					break;
				case Type.Color:
					myTypeFunc += ColorFunc;
				    break;
				case Type.ColorWidget:
					myTypeFunc += ColorWidgetFunc;
					break;
				case Type.Rotate:
					myTypeFunc += RotateFunc;
					break;
				case Type.Shake:
					myTypeFunc += ShakeFunc;
					break;
				case Type.MeshAnim:
					myTypeFunc += MeshAnimFunc;
					break;
				case Type.RotateAround:
					myTypeFunc += RotateAroundFunc;
					break;
				case Type.VertexColor:
					myTypeFunc += VertexColorFunc;
					break;
				case Type.TimeScale:
					myTypeFunc += TimeScaleFunc;
					break;
				case Type.V4Delegate:
					myTypeFunc += V4DelegateFunc;
					break;
				case Type.MoveArc:
					myTypeFunc += MoveArcFunc;
					break;
			}
		}
		get{
			return _myType;
		}
	}

	private Type _myType;

	public delegate void TypeFunc(float value);

	public event TypeFunc myTypeFunc;

	void MoveFunc(float value) {
		if (b_useWorld) {
			this.transform.position = ReturnValue(value).Vector3Value();
		} else {
			this.transform.localPosition = ReturnValue(value).Vector3Value();
		}
	}

	void MoveArcFunc(float value){
		if(b_useWorld){
			this.transform.position = ReturnValue(value).Vector3Value();
			Debug.LogWarning("myExtraData.v3_Temp:"+myExtraData.v3_Temp+"Evaluate:"+myExtraData.animationCurve.Evaluate(value)+"endV4.a:"+endV4.a);
//			this.transform.position = ReturnValue(value).Vector3Value() + 
//				myExtraData.v3_Temp * myExtraData.animationCurve.Evaluate(value) * endV4.a;
//			GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = this.transform.position;
		}else{
//			this.transform.localPosition = ReturnValue(value).Vector3Value() + 
//				myExtraData.v3_Temp * myExtraData.animationCurve.Evaluate(value) * endV4.a;
		}
	}

	void ScaleFunc(float value) {
		this.transform.localScale = ReturnValue(value).Vector3Value();
	}

	void ColorFunc(float value) {
		this.renderer.material.color = ReturnValue(value).ColorValue();
	}

	void ColorWidgetFunc(float value) {
//		this.renderer.material.color = ReturnValue(value).ColorValue();
		UIWidget widget = this.GetComponent<UIWidget>();
		widget.color = ReturnValue(value).ColorValue();;
	}

	void RotateFunc(float value) {
		if (b_useWorld) {
			this.transform.rotation = ReturnValue(value).RotationValue();
		} else {
			this.transform.localRotation = ReturnValue(value).RotationValue();
		}
	}

	void ShakeFunc(float value) {
		float temp = value;
		temp = 1 - temp;
		Vector3 temp2 = endV4.Vector3Value() * temp;
		if(Time.realtimeSinceStartup >= startV4.a +endV4.a ){
			if(this.b_useWorld)
			{
				this.transform.position = startV4.Vector3Value() - temp2 + new Vector3(temp2.x * Random.Range(0f, 2f), 
					temp2.y * Random.Range(0f, 2f), temp2.z * Random.Range(0f, 2f));
			}
			else
			{
				this.transform.localPosition = startV4.Vector3Value() - temp2 + new Vector3(temp2.x * Random.Range(0f, 2f), 
					temp2.y * Random.Range(0f, 2f), temp2.z * Random.Range(0f, 2f));
			}
			startV4.a = Time.realtimeSinceStartup + endV4.a;
		}
	}

	void MeshAnimFunc(float value) {
		V4 temp3 = ReturnValue(value);
		myExtraData.meshAnim.SetClip(temp3.x, temp3.y, temp3.z, temp3.a);
	}

	void RotateAroundFunc(float value) {
		float curAngle = myExtraData.rotateAroundInfo.f_Angle * value;
		this.transform.rotation = myExtraData.rotateAroundInfo.rotation_Old;
		this.transform.position = myExtraData.rotateAroundInfo.v3_OldPos;
		this.transform.RotateAround(myExtraData.rotateAroundInfo.v3_Point, myExtraData.rotateAroundInfo.v3_Axis, curAngle);
		if (!myExtraData.rotateAroundInfo.b_RotateSelf) {
			this.transform.rotation = myExtraData.rotateAroundInfo.rotation_Old;
		}
	}

	void VertexColorFunc(float value) {
		myExtraData.vertexColor.SetColor(ReturnValue(value));
	}

	void TimeScaleFunc(float value){
		Time.timeScale = value;
	}

	void V4DelegateFunc(float value){
		myExtraData.HandleV4Delegate(ReturnValue(value));
	}

	#endregion

	void Start() {
		startTime += f_DelayTime;
		HandleEasing(myEasingType);
	}

	void Update () {
		if(b_handleScaleTime){
			if(f_DelayTime > 0){
				f_DelayTime -= Time.deltaTime;
				return;
			}
			passedTime += Time.deltaTime;
		}else{
			if(f_DelayTime > 0){
				f_DelayTime -= Time.deltaTime;
				return;
			}
			passedTime = Time.realtimeSinceStartup - (startTime + f_DelayTime);
		}

		float value = (passedTime / duringTime);

		if (value >= 1) {
			myTypeFunc(myDelegateEasingFunc(0, 1, 1));
			if (myDelegateFunc != null) {
				myDelegateFunc();
			}
			if(myDelegateFuncWithObject!=null)
			{
				myDelegateFuncWithObject(this.gameObject);
			}
            if(FinishedAnim != null) FinishedAnim();
			Destroy(this);
		} else {
			myTypeFunc(myDelegateEasingFunc(0, 1, value));
		}

	}

}

public class V4 {
	public float x;
	public float y;
	public float z;
	public float a;

	public V4(){
		this.x = 0;
		this.y = 0;
		this.z = 0;
		this.a = 0;
	}

	public V4(Quaternion rotation) {
		this.x = rotation.x;
		this.y = rotation.y;
		this.z = rotation.z;
		this.a = rotation.w;
	}

	public V4(float x,float y,float z) {
		this.x = x;
		this.y = y;
		this.z = z;
		this.a = 0;
	}

	public V4(Vector3 v3) {
		this.x = v3.x;
		this.y = v3.y;
		this.z = v3.z;
		this.a = 0;
	}

	public V4(Vector3 v3,float a) {
		this.x = v3.x;
		this.y = v3.y;
		this.z = v3.z;
		this.a = a;
	}

	public V4(float x,float y,float z,float a) {
		this.x = x;
		this.y = y;
		this.z = z;
		this.a = a;
	}

	public V4(V4 v4) {
		this.x = v4.x;
		this.y = v4.y;
		this.z = v4.z;
		this.a = v4.a;
	}

	public V4(Color color) {
		this.x = color.r;
		this.y = color.g;
		this.z = color.b;
		this.a = color.a;
	}

	public static V4 operator +(V4 v4_1, V4 v4_2) {
		return new V4(v4_1.x + v4_2.x, v4_1.y + v4_2.y, v4_1.z + v4_2.z, v4_1.a + v4_2.a);
	}

	public static V4 operator +(V4 v4_1, float f) {
		return new V4(v4_1.x + f, v4_1.y + f, v4_1.z + f, v4_1.a + f);
	}

	public static V4 operator -(V4 v4_1, V4 v4_2) {
		return new V4(v4_1.x - v4_2.x, v4_1.y - v4_2.y, v4_1.z - v4_2.z, v4_1.a - v4_2.a);
	}

	public static V4 operator -(V4 v4_1, float f) {
		return new V4(v4_1.x - f, v4_1.y - f, v4_1.z - f, v4_1.a - f);
	}

	public static V4 operator *(V4 v4_1, V4 v4_2) {
		return new V4(v4_1.x * v4_2.x, v4_1.y * v4_2.y, v4_1.z * v4_2.z, v4_1.a * v4_2.a);
	}

	public static V4 operator *(V4 v4_1, float f) {
		return new V4(v4_1.x * f, v4_1.y * f, v4_1.z * f, v4_1.a * f);
	}

	public static V4 operator /(V4 v4_1, V4 v4_2) {
		return new V4(v4_1.x / v4_2.x, v4_1.y / v4_2.y, v4_1.z / v4_2.z, v4_1.a / v4_2.a);
	}

	public static V4 operator /(V4 v4_1, float f) {
		return new V4(v4_1.x / f, v4_1.y / f, v4_1.z / f, v4_1.a / f);
	} 

	//返回颜色
	public Color ColorValue(){
		return new Color(x,y,z,a);
	}

	//返回Vector3
	public Vector3 Vector3Value(){
		return new Vector3(x,y,z);
	}

	//返回
	public Quaternion RotationValue() {
		return new Quaternion(x,y,z,a);
	}

}

namespace ForMiniItween{

	public class MeshAnim : MonoBehaviour {

		//这里请自定义你的顶点类型。
		/// <summary>
		/// 这个是我的Plane的顶点顺序
		/// </summary>
		//public static int[] vertexIndexType_1 = { 1, 3, 2, 0 };
		public static int[] vertexIndexType_1 = { 3, 1,0, 2};

		/// <summary>
		/// 这个Ezgui生成的Plane的顶点的顺序
		/// </summary>
//		public static int[] vertexIndexType_2 = { 1, 3, 2, 0 };
		public static int[] vertexIndexType_2 = { 3,2,1,0 };

		//又是一种顶点顺序
		public static int [] vectexIndexType_3 = {1,3,2,0};

		//SMSprite的定点顺序
		public static int [] vectexIndexType_4 = {1,3,2,0};

		[System.NonSerialized]
		public int[] vertexIndex;//本次的定点顺序

		[System.NonSerialized]
		public MeshFilter myMeshFilter;

		[System.NonSerialized]
		public XYZ myXYZ = XYZ.XY;//默认状态下为XY平面

		[System.NonSerialized]
		public Vector2[] v2_Vt;//顶点贴图坐标（初始的数据）

		[System.NonSerialized]
		public Vector3[] v3_V;//顶点位置

		[System.NonSerialized]
		public V4 curClip;

		public enum XYZ { XY, XZ };

		/// <summary>
		/// 请注意XYZ的设置
		/// </summary>
		/// <param name="myMeshFilter"></param>
		/// <param name="vertexIndex"></param>
		public MeshAnim(int[] vertexIndex) {
			Init(vertexIndex);
		}

		//生成之后必须先Init
		public void Init(int[] vertexIndex) {
			curClip = new V4(0, 1, 1, 0);//当前的裁切信息 左右上下
			this.myMeshFilter = GetComponent<MeshFilter>();
			if (this.myMeshFilter != null) {
				Mesh mesh = myMeshFilter.mesh;
				if (mesh.vertexCount == 4) {
					this.vertexIndex = vertexIndex;
					v2_Vt = mesh.uv;
					v3_V = mesh.vertices;
				} else {
					Debug.LogError("Vertex count is not 4");
				}
			} else {
				Debug.LogError("Can not find MeshFilter");
			}
		}

		/// <summary>
		/// 左右上下来填参数
		/// </summary>
		public void SetClip(V4 v4) {
			SetClip(v4.x, v4.y, v4.z, v4.a);
		}

		/// <summary>
		/// 左右上下来填参数
		/// </summary>
		public void SetClip(float left, float right, float up, float down) {
			Mesh mesh = myMeshFilter.mesh;
			float leftPos, rightPos, upPos, downPos;
			if (myXYZ == XYZ.XY) {
				//顶点位置
				leftPos = (v3_V[vertexIndex[0]].x - v3_V[vertexIndex[3]].x) * left + v3_V[vertexIndex[3]].x;
				rightPos = (v3_V[vertexIndex[0]].x - v3_V[vertexIndex[3]].x) * right + v3_V[vertexIndex[3]].x;
				upPos = (v3_V[vertexIndex[0]].y - v3_V[vertexIndex[1]].y) * up + v3_V[vertexIndex[1]].y;
				downPos = (v3_V[vertexIndex[0]].y - v3_V[vertexIndex[1]].y) * down + v3_V[vertexIndex[1]].y;
				Vector3[] v3_Temp = mesh.vertices;
				v3_Temp[vertexIndex[0]] = new Vector3(rightPos, upPos, 0);
				v3_Temp[vertexIndex[1]] = new Vector3(rightPos, downPos, 0);
				v3_Temp[vertexIndex[2]] = new Vector3(leftPos, downPos, 0);
				v3_Temp[vertexIndex[3]] = new Vector3(leftPos, upPos, 0);
				mesh.vertices = v3_Temp;
			} else {
				//顶点位置
				leftPos = (v3_V[vertexIndex[0]].x - v3_V[vertexIndex[3]].x) * left + v3_V[vertexIndex[3]].x;
				rightPos = (v3_V[vertexIndex[0]].x - v3_V[vertexIndex[3]].x) * right + v3_V[vertexIndex[3]].x;
				upPos = (v3_V[vertexIndex[0]].z - v3_V[vertexIndex[1]].z) * up + v3_V[vertexIndex[1]].z;
				downPos = (v3_V[vertexIndex[0]].z - v3_V[vertexIndex[1]].z) * down + v3_V[vertexIndex[1]].z;
				Vector3[] v3_Temp = mesh.vertices;
				v3_Temp[vertexIndex[0]] = new Vector3(rightPos, 0, upPos);
				v3_Temp[vertexIndex[1]] = new Vector3(rightPos, 0, downPos);
				v3_Temp[vertexIndex[2]] = new Vector3(leftPos, 0, downPos);
				v3_Temp[vertexIndex[3]] = new Vector3(leftPos, 0, upPos);
				mesh.vertices = v3_Temp;
			}
			//顶点Uv
			leftPos = (v2_Vt[vertexIndex[0]].x - v2_Vt[vertexIndex[3]].x) * left + v2_Vt[vertexIndex[3]].x;
			rightPos = (v2_Vt[vertexIndex[0]].x - v2_Vt[vertexIndex[3]].x) * right + v2_Vt[vertexIndex[3]].x;
			upPos = (v2_Vt[vertexIndex[0]].y - v2_Vt[vertexIndex[1]].y) * up + v2_Vt[vertexIndex[1]].y;
			downPos = (v2_Vt[vertexIndex[0]].y - v2_Vt[vertexIndex[1]].y) * down + v2_Vt[vertexIndex[1]].y;
			Vector2[] v2_Temp = mesh.uv;
			v2_Temp[vertexIndex[0]] = new Vector2(rightPos, upPos);
			v2_Temp[vertexIndex[1]] = new Vector2(rightPos, downPos);
			v2_Temp[vertexIndex[2]] = new Vector2(leftPos, downPos);
			v2_Temp[vertexIndex[3]] = new Vector2(leftPos, upPos);
			mesh.uv = v2_Temp;
			//记录裁切信息
			curClip.x = left;
			curClip.y = right;
			curClip.z = up;
			curClip.a = down;
		}

	}

	//绕点旋转的参数
	public class RotateAroundInfo {
		public Vector3 v3_OldPos;
		public Quaternion rotation_Old;
		public bool b_RotateSelf;
		public Vector3 v3_Point;
		public float f_Angle;
		public Vector3 v3_Axis;

		public RotateAroundInfo(Vector3 oldPos, Quaternion rotation, bool rotateSelf, Vector3 point, float angle, Vector3 axis) {
			v3_OldPos = oldPos;
			rotation_Old = rotation;
			b_RotateSelf = rotateSelf;
			v3_Point = point;
			this.f_Angle = angle;
			this.v3_Axis = axis;
		}

	}

	//顶点颜色
	public class VertexColor {

		public MeshFilter meshFilter = null;

		public bool useShared = false;

		public VertexColor(MeshFilter meshFilter,bool useShared) {
			this.meshFilter = meshFilter;
			this.useShared = useShared;
		}

		/// <summary>
		/// 读取第一个顶点的颜色
		/// </summary>
		public Color GetColor() {
			if (useShared) {
				return meshFilter.sharedMesh.colors[0];
			} else {
				return meshFilter.mesh.colors[0];
			}
		}

		public void SetColor(V4 v4) {
			if (useShared) {
				Color[] colors = new Color[meshFilter.sharedMesh.vertexCount];
				Color color = v4.ColorValue();
				for (int i = 0; i < meshFilter.sharedMesh.vertexCount;i++ ) {
					colors[i] = color;
				}
				meshFilter.sharedMesh.colors = colors;

			} else {
				Color[] colors = new Color[meshFilter.mesh.vertexCount];
				Color color = v4.ColorValue();
				for (int i = 0; i < meshFilter.mesh.vertexCount; i++) {
					colors[i] = color;
				}
				meshFilter.mesh.colors = colors;
			}
		}

	}

	//额外数据
	public class ExtraData {
		
		public MeshAnim meshAnim = null;
		public RotateAroundInfo rotateAroundInfo = null;
		public VertexColor vertexColor = null;
		public AnimationCurve animationCurve = null;

		public delegate void V4Delegate(V4 v4);
		public event V4Delegate myV4Delegate;

		public Vector3 v3_Temp;

		public void HandleV4Delegate(V4 v4){
			myV4Delegate(v4);
		}

		public ExtraData(MeshAnim meshAnim) {
			this.meshAnim = meshAnim;
		}

		public ExtraData(RotateAroundInfo rotateAroundInfo) {
			this.rotateAroundInfo = rotateAroundInfo;
		}

		public ExtraData(VertexColor vertexColor) {
			this.vertexColor = vertexColor;
		}

		/// <summary>
		/// 切记AnimationCurve必须目标指向从0到1
		/// </summary>
		public ExtraData(AnimationCurve animationCurve) {
			this.animationCurve = animationCurve;
		}

		public ExtraData(V4Delegate v4Delegate) { 
			myV4Delegate = null;
			myV4Delegate += v4Delegate;
		}

	}

}

