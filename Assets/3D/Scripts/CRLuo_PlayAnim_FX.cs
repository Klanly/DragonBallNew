using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CRLuo_PlayAnim_FX : MonoBehaviour
{
	public int StarLevel;
	public string _ = "按键测试按钮123456789";
	public bool Try_Key;
	//角色ID
	[System.NonSerialized]
	public int OBJScreen_ID;
	//左站位标记
	[System.NonSerialized]
	public bool Pos_L = true;

	//左站位标记
	[System.NonSerialized]
	public bool SaveLife = true;

	private Camera _MyCamera_L;
	public Camera MyCamera_L
	{
		get
		{
			if (_MyCamera_L == null)
			{
				//从所有子物体中寻找摄像机L物体
				Camera[] cameras = this.transform.GetComponentsInChildren<Camera>();
				foreach (Camera aCamera in cameras)
				{
					if (aCamera.name == "Camera_L")
					{
						_MyCamera_L = aCamera;
						break;
					}
				}
				if (_MyCamera_L == null)
				{
					Debug.LogWarning("左摄像机不存在");	
				}
				
			}

			return _MyCamera_L;
		}
	}


	private Camera _MyCamera_R;
	public Camera MyCamera_R
	{
		get
		{
			if (_MyCamera_R == null)
			{
				//从所有子物体中寻找摄像R物体
				Camera[] cameras = this.transform.GetComponentsInChildren<Camera>();
				foreach (Camera aCamera in cameras)
				{
					if (aCamera.name == "Camera_R")
					{
						_MyCamera_R = aCamera;
						break;
					}
				}
				if (_MyCamera_R == null)
				{
					Debug.LogWarning("左摄像机不存在");
				}

			}

			return _MyCamera_R;
		}
	}
	public string ____ = "摄像机有效动作开关";
	public bool CameraKey_Attack = false;
	public bool CameraKey_Skill = false;
	public bool CameraKey_GroupSkill = false;
	public bool CameraKey_OverSkill = true;
	public string _____ = "终结技循环次数";
	public int OverSkillNUM = 0;
	//自身ID号
	[System.NonSerialized]
	public int i_Num = 0;
	public string ______ = "自身模型网格";
	public GameObject mainMeshRender;
	public string _______ = "自身骨骼中心";
	public GameObject RootBone;

	//敌人连接
	[System.NonSerialized]
	public CRLuo_PlayAnim_FX RivalOBJ;
	[System.NonSerialized]
	public bool Show_RivalAmin_Key = true;
	//被打时接受对方控制标记
	[System.NonSerialized]
	public bool Injure_Key = false;

	//public string ________ = "派别";
	//public enum teamType { None, Fire, Water, Wood, Light, Dark };
	//public teamType Team_Type;

	public string ________ = "特效总开关";
	//特效总开关
	public bool FX_Body_key = false;

	public string _________ = "生命特征特效";
	//生命特效对象数组
	public ParticleSystem [] myBodyFX;

	public string __________ = "随身特效开关";
	//生命呼吸特效
	public Anim_Fx_ONOFF[] myFx_OnOff;

	public string ___________ = "行为烘托特效";
	//生命呼吸特效
	public FxLifeAdvanced [] myFxLifeAdvanced;

	public string ____________ = "攻击效果特效";

	//动画特效类数组
	public AttackFX [] myAttackFX;

	public string _____________ = "刀光拖尾特效";
	//刀光拖尾
	public TrailController [] trailController;


	public string ______________ = "敌人挨打动作";
	//刀光拖尾
	public RivalAmin[] myRivalAmin;

	public string _______________ = "角色位移控制";
	//物体移动
	public MoveGo [] myMove;

	public string ________________ = "摄像机震动特效";
	//摄像机震动
	public GameObject CameraFx_IN;
	public CamearFX [] myCamearFX;

	public string _________________ = "子弹时间特效";
	//时间变速
	public TimeGo [] myTimeFX;
	public string ____________________ = "隐身瞬移特效";
	public HideFx[] myHideFX;

	[System.NonSerialized]
	public float NowTimeSheep;


	//当前动画状态名
	private string str_CurAnim = "";
	
	public string GetCurAnim(){
		return str_CurAnim;
	}

	public string __________________ = "角色标尺";
	//绘制参考线参数
	public bool Scaleplate_Key = true;
	public float Scaleplate_Height = 5f;
	public float Scaleplate_Wide = 4f;
	public float Scaleplate_Long = 4f;
	public bool ScaleplateShowBox = false;
	public Vector3 ScaleplateOffset;
	public float OneShow_Height = 0f;
	public bool ShowBlood_Key = false;
	public float ShowBloodLong = 1;

	public string ___________________ = "死亡特效";
	//灵魂
	public GameObject prefab_Soul;

	public CRLuoAnim_Main.Type NowAnimType;

	//添加碰撞体
	public void GenerateCollider()
	{
		BoxCollider boxColldier = this.gameObject.AddComponent<BoxCollider>();
		boxColldier.center = new Vector3(-ScaleplateOffset.x / this.transform.localScale.x, -ScaleplateOffset.y / this.transform.localScale.y, -ScaleplateOffset.z / this.transform.localScale.z) +
			Vector3.up * Scaleplate_Height / 2 / this.transform.localScale.y;
		boxColldier.size = new Vector3(Scaleplate_Wide / 2 / this.transform.localScale.x, Scaleplate_Height / 2 / this.transform.localScale.y, Scaleplate_Long / 2 / this.transform.localScale.z);
	}

	//人物标尺绘制
	void OnDrawGizmos() {
		if(Scaleplate_Key){
			Gizmos.color = new Color(1, 0, 1);
			Gizmos.DrawLine(this.transform.position + ScaleplateOffset, this.transform.position + ScaleplateOffset + Vector3.up * Scaleplate_Height);
			Gizmos.DrawLine(this.transform.position + ScaleplateOffset + Vector3.forward * Scaleplate_Long * 0.5f, this.transform.position + ScaleplateOffset - Vector3.forward * Scaleplate_Long * 0.5f);
			Gizmos.DrawLine(this.transform.position + ScaleplateOffset + Vector3.right * Scaleplate_Wide * 0.5f, this.transform.position + ScaleplateOffset - Vector3.right * Scaleplate_Wide * 0.5f);
			Gizmos.DrawLine(this.transform.position + ScaleplateOffset + Vector3.forward * Scaleplate_Long * 0.5f + Vector3.up * Scaleplate_Height, this.transform.position + ScaleplateOffset - Vector3.forward * Scaleplate_Long * 0.5f + Vector3.up * Scaleplate_Height);
			Gizmos.DrawLine(this.transform.position + ScaleplateOffset + Vector3.right * Scaleplate_Wide * 0.5f + Vector3.up * Scaleplate_Height, this.transform.position + ScaleplateOffset - Vector3.right * Scaleplate_Wide * 0.5f + Vector3.up * Scaleplate_Height);
			if (ScaleplateShowBox)
			{
				Gizmos.DrawLine(this.transform.position + ScaleplateOffset - Vector3.forward * Scaleplate_Long * 0.5f, this.transform.position + ScaleplateOffset - Vector3.forward * Scaleplate_Long * 0.5f + Vector3.up * Scaleplate_Height);
				Gizmos.DrawLine(this.transform.position + ScaleplateOffset + Vector3.forward * Scaleplate_Long * 0.5f, this.transform.position + ScaleplateOffset + Vector3.forward * Scaleplate_Long * 0.5f + Vector3.up * Scaleplate_Height);
				Gizmos.DrawLine(this.transform.position + ScaleplateOffset - Vector3.right * Scaleplate_Wide * 0.5f, this.transform.position + ScaleplateOffset - Vector3.right * Scaleplate_Wide * 0.5f + Vector3.up * Scaleplate_Height);
				Gizmos.DrawLine(this.transform.position + ScaleplateOffset + Vector3.right * Scaleplate_Wide * 0.5f, this.transform.position + ScaleplateOffset + Vector3.right * Scaleplate_Wide * 0.5f + Vector3.up * Scaleplate_Height);
			}
		}
		if(ShowBlood_Key){
			Gizmos.color = new Color(0,1,0);
			Gizmos.DrawCube( this.transform.position + Vector3.down * 0.05f + ScaleplateOffset ,new Vector3(ShowBloodLong,0.1f,0.001f) );
		}
	}

	bool BianShen = false;
	// 程序预运行部分
	void Start()
	{
		FX_Body_key = false;
		
		MyCamera_L.gameObject.SetActive(false);
		MyCamera_R.gameObject.SetActive(false);

        if (myAttackFX != null && myAttackFX.Length != 0)
		{
			//循环枚举 类组的各个内容 上限为类组的长度
			for (int i = 0; i < myAttackFX.Length; i++)
			{
				//判断当前类组i中眠Type 是否与传入的动作名一致 并且 特效对象不为空  并且绑定对象不为空
				if (myAttackFX[i].FXtype == CRLuoAnim_Main.Type.Attack )
				{
					if (myAttackFX[i].myFxElement != null && myAttackFX[i].myFxElement.Length != 0) 
					{
						
						for (int j = 0; j < myAttackFX[i].myFxElement.Length; j++)
						{
							if (myAttackFX[i].myFxElement[j].Prefab_FX != null && myAttackFX[i].myFxElement[j].ON_OFF && myAttackFX[i].myFxElement[j].SendRival)
							{
								BianShen = true;
							}
						}
					}
				}
			}
		}
		if (BanCameraController.Instance != null)
		{
			BanCameraController.Instance.AddRole(this);
		}

		if (BanCameraControlle_XY.Instance != null)
		{
			BanCameraControlle_XY.Instance.AddRole(this);
		}
		//创建刀光
		TrailInit();
	

		#region 上台下台材质球效果初始化
		//创建新材质球
		//Material material = new Material(Shader.Find("Unlit/REDCommonModelDouble"));

		if (targetShader == null)
		{
			//如果在名称提示中有(Clone)
			if (this.gameObject.name.EndsWith("(Clone)"))
			{
				this.gameObject.name = this.gameObject.name.Substring(0, this.gameObject.name.Length - 7);
			}

			Debug.LogWarning(this.gameObject.name + "没有赋予闪白材质");
		}
		Material material = new Material(targetShader);


		//载入白色图片
		Texture2D texture = (Texture2D)Resources.Load("Pictures/white");

		//把图片赋予材质球贴图属性
		material.mainTexture = texture;





		//}
		#endregion
		if (UpShowKey)
		{

			//
			SkinnedMeshRenderer[] renders = GetComponentsInChildren<SkinnedMeshRenderer>();

			//
			foreach (SkinnedMeshRenderer aMeshRenderer in renders)
			{
				foreach (Material aMaterial in aMeshRenderer.materials)
				{
					//把材质球加入原始材质目录
					list_Exist.Add(aMaterial);
				}

				Material[] result = AddNewToExist(aMeshRenderer.materials, material);

				aMeshRenderer.materials = result;

				//把白色贴图材质球加入遮盖材质目录
				list.Add(result[result.Length - 1]);
			}

			//让自身变无
			foreach (Material aMater in list_Exist)
			{
				aMater.SetFloat("_Cutoff", 1);

			}

			//让白片也变无
			foreach (Material aMater in list)
			{
				aMater.SetColor("_Color", new Color(1, 1, 1, 0));
			}

			StartCoroutine(DelayStart());
			Invoke("Fx_main_Botton", F_ShowTime + i_brotherCount * F_GenerateGap);
			//让影子不显示
			try
			{
				ShadowObj.renderer.enabled = false;
			}
			catch (System.Exception e)
			{
				Debug.LogError("ShadowObj Name:" + ShadowObj.name + "\n" + e.ToString());
			}

		}


		



	}

	//有几个哥哥
	[System.NonSerialized]
	public int i_brotherCount;

	//生成的间隙
	public const float F_GenerateGap = 0.5f;

	[System.NonSerialized]
	public bool b_StandAlone = false;

	//登场开关
	[System.NonSerialized]
	public bool UpShowKey = false;

	public void MaterialReturnToExist()
	{
		SkinnedMeshRenderer[] SkinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach (SkinnedMeshRenderer aSkinnedMeshRenderer in SkinnedMeshRenderers)
		{
			aSkinnedMeshRenderer.materials = MaterialRemoveLast(aSkinnedMeshRenderer.materials);
		}
	}

	public void MaterialAddWhite()
	{
		Material material = new Material(targetShader);
		Texture2D texture = (Texture2D)Resources.Load("Pictures/white");
		material.mainTexture = texture;
		SkinnedMeshRenderer[] renders = GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach (SkinnedMeshRenderer aMeshRenderer in renders)
		{
			Material[] result = AddNewToExist(aMeshRenderer.materials, material);
			aMeshRenderer.materials = result;
			list.Add(result[result.Length - 1]);
		}
		foreach (Material aMater in list)
		{
			aMater.SetColor("_Color", new Color(1, 1, 1, 0));
		}
	}

	//延时登台
	IEnumerator DelayStart()
	{
		yield return new WaitForSeconds(i_brotherCount * F_GenerateGap);
		//登台效果
		UpStage();

		if (Application.loadedLevelName == "Game" && b_StandAlone == false)
		{
			Invoke("BanRandomFree", 0.4f);
		}

	}


	void BanRandomFree(){
		//随机播放动作
		if(Random.Range(0,2) == 0){
			HandleTypeAnim(CRLuoAnim_Main.types[0]);
		}else{
			HandleTypeAnim(CRLuoAnim_Main.types[1]);
		}
	}



	int OverSkill_Remaining;

	//每帧调用命令
	void Update()
	{


		if (!animation.isPlaying && !Injure_Key)
		{
			if (str_CurAnim == "OverSkill_0")
			{
                HandleTypeAnim(CRLuoAnim_Main.Type.OverSkill_1);
			}
			else if (str_CurAnim == "OverSkill_1" && OverSkillNUM != 0)
			{
				OverSkillNUM--;
				HandleTypeAnim(CRLuoAnim_Main.Type.OverSkill_1);
			}
			else if (str_CurAnim == "OverSkill_1" && OverSkillNUM == 0)
			{
				HandleTypeAnim(CRLuoAnim_Main.Type.OverSkill_2);
			}

			//如果不是死亡（最后一个动作为死亡）和结束状态
            else if (str_CurAnim != CRLuoAnim_Main.types[CRLuoAnim_Main.types.Length - 1].ToString()
			         && str_CurAnim != CRLuoAnim_Main.Type.Injure_GA.ToString() && str_CurAnim != "Over")
			{
				//调用默认动作
				HandleTypeAnim(CRLuoAnim_Main.types[0]);
			}
            else if(str_CurAnim == CRLuoAnim_Main.types[CRLuoAnim_Main.types.Length - 1].ToString()
			        || str_CurAnim == CRLuoAnim_Main.Type.Injure_GA.ToString() )
			{
				if (SaveLife)
				{
					HandleTypeAnim(CRLuoAnim_Main.Type.StandUp);
				} else {
                    Die();
				}

			}

			else if (str_CurAnim != "Over")
			{

				//建立当前子物体名称列表
				Transform[] children = transform.GetComponentsInChildren<Transform>();

				//判断生命特征数组是否为空
				if (myFxLifeAdvanced != null && myFxLifeAdvanced.Length > 0)
				{
					//逐个提取生命特征变量
					foreach (FxLifeAdvanced aFxLifeAdvanced in myFxLifeAdvanced)
					{


						foreach (UseToType aUseToType in aFxLifeAdvanced.LifeUse)
						{

							//判断当前组中生命特征特效非空
							if (aFxLifeAdvanced.Life_FX != null && aUseToType.UseType == CRLuoAnim_Main.types[CRLuoAnim_Main.types.LongLength - 1])
							{
								//在子属性列表中查提取名称
								foreach (Transform aTransform in children)
								{
									//如果搜索到的名称中包含（特效名称）
									if (aTransform.name.StartsWith(aFxLifeAdvanced.Life_FX.name))
									{
										//设置特效父物体为空
										aTransform.parent = null;
									}
								}
							}
						}
					}
				}

				if (myAttackFX != null && myAttackFX.Length > 0)
				{
					//提取个个特效组中的元素
					foreach (AttackFX aAttackFX in myAttackFX)
					{
						//判断当前特效是否为死亡特效
						if (aAttackFX.FXtype == CRLuoAnim_Main.types[CRLuoAnim_Main.types.LongLength - 1])
						{
							if (aAttackFX.myFxElement != null && aAttackFX.myFxElement.Length > 0)
							{
								//在子特效包中逐鎏崛热							
								foreach (FxElement aFxElement in aAttackFX.myFxElement)
								{
									if (aFxElement.Prefab_FX != null)
									{
										//在子物体名称中逐个提取
										foreach (Transform aTransform in children)
										{
											//如果提取模型.名称.存在（攻击子特效.匦?名称）
											if (aTransform.name.StartsWith(aFxElement.Prefab_FX.name))
											{
												//设置当前特效为空
												aTransform.parent = null;
											}
										}
									}
								}
							}
						}
					}
				}





			}
		}

        #region 总测试开关
		if (Try_Key)
		{


			//		绻??键
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				//调用默认动作函数
				HandleTypeAnim(CRLuoAnim_Main.types[1]);
			}
			//如果按 2 键
			else if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				//调用攻击动作函数
				HandleTypeAnim(CRLuoAnim_Main.types[2]);
			}
			//如果按 3 键
			else if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				//调用技能动作函数
				HandleTypeAnim(CRLuoAnim_Main.types[3]);
			}
			//		如?尺4 键
			else if (Input.GetKeyDown(KeyCode.Alpha4))
			{
				//调用受伤动作函数
				HandleTypeAnim(CRLuoAnim_Main.types[4]);
			}
			//如果按 5 键
			else if (Input.GetKeyDown(KeyCode.Alpha5))
			{
				//调用自由动作函数
                HandleTypeAnim(CRLuoAnim_Main.types[5]);
			}
			//如果按 5 键
			else if (Input.GetKeyDown(KeyCode.Alpha6))
			{
				//调用自由2动作函数
				HandleTypeAnim(CRLuoAnim_Main.types[6]);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha7))
			{
				//调用触屏动作函数
				HandleTypeAnim(CRLuoAnim_Main.types[7]);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha8))
			{
				//调用死亡动作函数
				HandleTypeAnim(CRLuoAnim_Main.types[8]);
			}
			else if (Input.GetKeyDown(KeyCode.X))
			{
				//特效开关
				if (FX_Body_key)
				{
					FX_Body_key = false;
				}
				else
				{
					FX_Body_key = true;
				}
				BodyFX_ON_OFF(FX_Body_key);

			}

		}
        #endregion
		//上台下台标记，如果不为0说明在上台或下台的过程中
		if (fadeParam != 0)
		{
			//登台显示
			StageShow();
		}
	}


	//返回眼睛的高度
	public Vector3 GetEyePos() {
		return this.transform.position + Scaleplate_Height * Vector3.up;
	}


	//物体销毁运行部分
	void OnDestroy()
	{
		if (BanCameraController.Instance != null)
		{
			BanCameraController.Instance.RemoveRole(this);
		}

		if (BanCameraControlle_XY.Instance != null)
		{
			BanCameraControlle_XY.Instance.RemoveRole(this);
		}
			
		//added by zhangqiang to release assets
		Resources.UnloadUnusedAssets();
	}


	//定义开启生命特效程序集（接收布尔型变量）
	public void BodyFX_ON_OFF(bool Key)
	{
		//判断生命特效数据数组是否为空或长度为0
		if (myBodyFX.Length != 0)
		{
			//循环枚举特效数据，上限为数组长度
			for (int i = 0; i < myBodyFX.Length; i++)
			{
				//判断特效内容是
				if (myBodyFX[i] != null)
				{
					myBodyFX[i].enableEmission = Key;
				}
			}

		}
	}

	private bool b_NeedDie = false;

	void Create_TeleportFX()
	{
		GameObject aObject = Resources.Load("UnPack/CRLuo/FX_Teleport_Move") as GameObject;

		GameObject temp_Screen = EmptyLoad.CreateObj(aObject, RootBone.transform.position, RootBone.transform.rotation);

		temp_Screen.transform.parent = RootBone.transform;
	}
	void Create_RushFX(bool Key)
	{
		if (Key)
		{
			GameObject aObject = Resources.Load("UnPack/CRLuo/Somke_Dust_GoRush") as GameObject;

			EmptyLoad.CreateObj(aObject, this.gameObject.transform.position, this.gameObject.transform.rotation);
		}
		else
		{
			GameObject aObject = Resources.Load("UnPack/CRLuo/Somke_Dust_BackRush") as GameObject;

			EmptyLoad.CreateObj(aObject, this.gameObject.transform.position, this.gameObject.transform.rotation);

		}
	}

	/// <summary>
	/// 单独处理对决逻辑
	/// </summary>
	public void HandleFightVs() {
		if(RivalOBJ == null) return;

		//先播放攻击动画
		animation.CrossFade("Attack");
		RivalOBJ.animation.CrossFade("Attack");

		slowXuLi();
		Invoke("slowJump", 0.5F);
		Invoke("Rush", 0.8F);
		Invoke("Stop", 1.06F);
		Invoke("normal", 2.06F);
		Invoke("Injure", 2.26F);
	}

	void slowXuLi() {
		float slow = 0.2F;
		foreach (AnimationState state in animation) {
			state.speed = slow;
		}

		foreach (AnimationState state in RivalOBJ.animation) {
			state.speed = slow;
		}
	}

	void Rush() {
		foreach (AnimationState state in animation) {
			state.speed = 1F;
		}

		foreach (AnimationState state in RivalOBJ.animation) {
			state.speed = 1F;
		}
	}

	void slowJump() {
		float slow = 1F;
		foreach (AnimationState state in animation) {
			state.speed = slow;
		}

		foreach (AnimationState state in RivalOBJ.animation) {
			state.speed = slow;
		}
	}

	void Stop() {
		foreach (AnimationState state in animation) {
			state.speed = 0F;
		}

		foreach (AnimationState state in RivalOBJ.animation) {
			state.speed = 0F;
		}
	}

	void normal() {
		foreach (AnimationState state in animation) {
			state.speed = 1F;
		}

		foreach (AnimationState state in RivalOBJ.animation) {
			state.speed = 1F;
		}
	}

	void Injure() {
		RivalOBJ.animation.CrossFade("Injure_G0");
		StartCoroutine(RivalOBJ.CreateSounds(CRLuoAnim_Main.Type.Injure_Fly_Go));
		RivalOBJ.str_CurAnim = "Injure_GA";

	}

	//定义攻击特效函数
	public void HandleTypeAnim(CRLuoAnim_Main.Type animType, CRLuoAnim_Main.Type aliasType = CRLuoAnim_Main.Type.None)
	{
		TrailsCheck(animType);
		//调用模型attack攻击动作
		animation.CrossFade(animType.ToString());
		//设置当前动作名为attack（攻击动作）
		if(aliasType == CRLuoAnim_Main.Type.Injure_GA)
			str_CurAnim = aliasType.ToString();
		else 
			str_CurAnim = animType.ToString();

		NowAnimType = animType;
		if (Show_RivalAmin_Key) {
			StartCoroutine ("CreateRivalAnim", animType);
		}
		if (animType == CRLuoAnim_Main.Type.Idle)
		{
			if (Pos_L)
			{
				MyCamera_L.gameObject.SetActive(false);
			}
			else
			{
				MyCamera_R.gameObject.SetActive(false);
			}


			if (RivalOBJ != null)
			{
				RivalOBJ.Injure_Key = false;
			}

			foreach(ParticleSystem myFX in myBodyFX) {
				if(myFX!=null)
				{

					myFX.gameObject.layer = this.gameObject.layer;
				}
			}
			foreach(Anim_Fx_ONOFF myFXOnOff in myFx_OnOff) {
				if(myFXOnOff.FX_obj!=null)
				{
						
					myFXOnOff.FX_obj.gameObject.layer = this.gameObject.layer;
				}
			}



		}else if (CRLuoAnim_Main.Type.BackRush == animType)
		{
			//aaaaaaaa
			Create_TeleportFX();
			Create_RushFX(true);
		}
		else if (CRLuoAnim_Main.Type.GoRush == animType)
		{
			Create_TeleportFX();
			Create_RushFX(false);
		}
		
		if (!Injure_Key)
		{
			if (animType == CRLuoAnim_Main.Type.Attack && CameraKey_Attack)
			{
				if (Pos_L)
				{
					MyCamera_L.gameObject.SetActive(true);
				}
				else
				{
					MyCamera_R.gameObject.SetActive(true);
				}


			}


			if (animType == CRLuoAnim_Main.Type.Skill && CameraKey_Skill)
			{
				if (Pos_L)
				{
					MyCamera_L.gameObject.SetActive(true);
				}
				else
				{
					MyCamera_R.gameObject.SetActive(true);
				}

			}



			if (animType == CRLuoAnim_Main.Type.GroupSkill && CameraKey_GroupSkill)
			{
				if (Pos_L)
				{
					MyCamera_L.gameObject.SetActive(true);
				}
				else
				{
					MyCamera_R.gameObject.SetActive(true);
				}

			}

			if (animType == CRLuoAnim_Main.Type.OverSkill_0 && CameraKey_OverSkill)
			{
				if (Pos_L)
				{
					MyCamera_L.gameObject.SetActive(true);
				}
				else
				{
					MyCamera_R.gameObject.SetActive(true);
				}

			}



			StartCoroutine("CreateCamearFx", animType);

		}
			
		if (!Injure_Key || str_CurAnim != "Attack" || BianShen)
		{
			//从myAttackFX[]攻击特效类（数组）中提取创建 攻击（attack） 的特效
			StartCoroutine("CreateFx", animType);
			StartCoroutine("CreateHide", animType);
		}
			
		StartCoroutine("CreateLifeFx", animType);
		StartCoroutine("AnimFx_ONOFF", animType);
		//挨打不排除的特效
		StartCoroutine("CreateTime", animType);
		StartCoroutine("CreateMove", animType);
		StartCoroutine("CreateSounds", animType);
	}

	public void DelayDestroy(float time){
		StartCoroutine(DestroyIE(time));
	}

	IEnumerator DestroyIE(float time){
		yield return new WaitForSeconds(time);
		Destroy(this.gameObject);
	}

	//随身特效开关
	IEnumerator AnimFx_ONOFF(CRLuoAnim_Main.Type Type)
	{
		if (myFx_OnOff != null && myFx_OnOff.Length != 0)
		{
			for (int i = 0; i < myFx_OnOff.Length; i++)
			{
				if (myFx_OnOff[i].FX_obj != null)
				{
					if (myFx_OnOff[i].AnimFx_Use != null && myFx_OnOff[i].AnimFx_Use.Length != 0)
					{
						for (int j = 0; j < myFx_OnOff[i].AnimFx_Use.Length; j++)
						{
							if (myFx_OnOff[i].AnimFx_Use[j].TimeLong > 0)
							{
								if (myFx_OnOff[i].AnimFx_Use[j].UseType == Type)
								{
									//程序在此等待 WaitForSeconds(等待秒数） 游戏界面仍然运动。
									yield return new WaitForSeconds(myFx_OnOff[i].AnimFx_Use[j].StartTime);
									myFx_OnOff[i].FX_obj.SetActiveRecursively(true);
									yield return new WaitForSeconds(myFx_OnOff[i].AnimFx_Use[j].TimeLong);
									myFx_OnOff[i].FX_obj.SetActiveRecursively(false);

								}
							}
						}
					}
				
				
				}


			}

		}



	}


	//攻击特效  定义提取创建特效 的  有参函数 （类 变量）
	IEnumerator CreateFx(CRLuoAnim_Main.Type Type)
	{
		//----
		//判断当前   特效类数组不为空 并且 数组长度不为0
		if (myAttackFX != null && myAttackFX.Length != 0)
		{
			//循环枚举 类组的各个内容 上限为类组的长度
			for (int i = 0; i < myAttackFX.Length; i++)
			{
				//判断当前类组i中的 Type 是否与传入的动作名一致 并且 特效对象不为空  并且     绑定对象不为空
				if (myAttackFX[i].FXtype == Type && myAttackFX[i].go_TargetBones != null)
				{
					if (myAttackFX[i].myFxElement != null && myAttackFX[i].myFxElement.Length != 0) 
					{
					
					
					for (int j = 0; j < myAttackFX[i].myFxElement.Length; j++)
					{
						if (myAttackFX[i].myFxElement[j].Prefab_FX != null && myAttackFX[i].myFxElement[j].ON_OFF)
						{
							//程序在此等待 WaitForSeconds(等待秒数） 游戏界面仍然运动。
							yield return new WaitForSeconds(myAttackFX[i].myFxElement[j].FXtime);

							//创建粒子物体，temp继承GameObject嗍粜裕ㄈ系?ㄟ  实例化特效。

							GameObject temp;

							if (myAttackFX[i].myFxElement[j].UseNoParent)
							{
								
								Vector3 localOffset = myAttackFX[i].v3_FXPos + myAttackFX[i].myFxElement[j].v3_FXPos_offset + myAttackFX[i].go_TargetBones.transform.position - this.transform.position;

								Vector3 localRotation = this.transform.localRotation.eulerAngles + myAttackFX[i].v3_FXRotation + myAttackFX[i].myFxElement[j].v3_FXRotation_offset;

								temp = EmptyLoad.CreateObj(
									myAttackFX[i].myFxElement[j].Prefab_FX,
									this.transform.TransformPoint(localOffset),
									Quaternion.Euler(localRotation)
								);

							}
							else {
								temp = EmptyLoad.CreateObj(myAttackFX[i].myFxElement[j].Prefab_FX, myAttackFX[i].go_TargetBones.transform.position + myAttackFX[i].v3_FXPos + myAttackFX[i].myFxElement[j].v3_FXPos_offset, myAttackFX[i].myFxElement[j].Prefab_FX.transform.localRotation);
								//temp临时对象 继承绑定物体位置
								temp.transform.parent = myAttackFX[i].go_TargetBones.transform;
							}
							if (myAttackFX[i].myFxElement[j].QiGong){
								BanCurveLine aBanCurveLine = temp.GetComponent<BanCurveLine>();
								if( aBanCurveLine != null && RivalOBJ != null )  aBanCurveLine.target = RivalOBJ.RootBone;
								//aBanCurveLine.f_Time = 
							}

							//调整特效旋转
							//temp.transform.localRotation = Quaternion.Euler(
							//                                                 myAttackFX[i].go_TargetBones.transform.localRotation.x+myAttackFX[i].v3_FXRotation.x + myAttackFX[i].myFxElement[j].v3_FXRotation_offset.x,
							//                                                 myAttackFX[i].go_TargetBones.transform.localRotation.y + myAttackFX[i].v3_FXRotation.y + myAttackFX[i].myFxElement[j].v3_FXRotation_offset.y,
							//                                                 myAttackFX[i].go_TargetBones.transform.localRotation.z + myAttackFX[i].v3_FXRotation.z + myAttackFX[i].myFxElement[j].v3_FXRotation_offset.z
							//                                                 );

							temp.layer = this.gameObject.layer;
							if (myAttackFX[i].myFxElement[j].DeleteFx)
							{

								temp.GetComponent<CRLuo_DeleteOBJ>().Delete1_Time = myAttackFX[i].myFxElement[j].DeleteTime;

							}
							if (myAttackFX[i].myFxElement[j].SendRival)
							{
								
								temp.GetComponent<CRLuo_ShowPlayAnim>().Rival = RivalOBJ;
								temp.GetComponent<CRLuo_ShowPlayAnim>().Master = this;
								
							}
						}
					}

				}
				}


			}
		}

	}

	IEnumerator CreateRivalAnim(CRLuoAnim_Main.Type Type)
	{
		if (myRivalAmin != null && myRivalAmin.Length != 0)
		{
			//循环枚举 类组的各个内容 上限为类组的长度
			for (int i = 0; i < myRivalAmin.Length; i++)
			{
				//判断当前类组i中的 Type 是否与传入的动作名一致 并且 特效对象不为空  并且     绑定对象不为空
				if (myRivalAmin[i].FXtype == Type && RivalOBJ != null)
				{
					if (myRivalAmin[i].myRivalAminElement != null && myRivalAmin[i].myRivalAminElement.Length != 0)
					{
						for (int j = 0; j < myRivalAmin[i].myRivalAminElement.Length; j++)
						{

							if (myRivalAmin[i].myRivalAminElement[j].RivalInjuredAnimName != RivalInjuredAnim.Type.None && myRivalAmin[i].myRivalAminElement[j].ON_OFF)
							{


								//程序在此等待 WaitForSeconds(等待秒数） 游戏界面仍然运动。
								yield return new WaitForSeconds(myRivalAmin[i].myRivalAminElement[j].Time_Wait);

								if(RivalOBJ == null) continue;

								RivalOBJ.Injure_Key = true;

								if (myRivalAmin[i].myRivalAminElement[j].RivalInjuredAnimName == RivalInjuredAnim.Type.RandAnim)
								{
									switch (Random.Range(0,3))
									{

										case 0:
											RivalOBJ.HandleTypeAnim(CRLuoAnim_Main.Type.Injure_0);
											break;
										case 1:
											RivalOBJ.HandleTypeAnim(CRLuoAnim_Main.Type.Injure_1);
											break;
										case 2:
											RivalOBJ.HandleTypeAnim(CRLuoAnim_Main.Type.Injure_2);
											break;

									}
								}
								else
								{
									switch (myRivalAmin[i].myRivalAminElement[j].RivalInjuredAnimName)
									{
										case RivalInjuredAnim.Type.GoRus:
											RivalOBJ.HandleTypeAnim(CRLuoAnim_Main.Type.GoRush);
											break;
										case RivalInjuredAnim.Type.BackRush:
											RivalOBJ.HandleTypeAnim(CRLuoAnim_Main.Type.BackRush);
											break;
										case RivalInjuredAnim.Type.Attack:
											RivalOBJ.HandleTypeAnim(CRLuoAnim_Main.Type.Attack);
											break;

										case RivalInjuredAnim.Type.Defend:
											RivalOBJ.HandleTypeAnim(CRLuoAnim_Main.Type.Defend);
											break;

										case RivalInjuredAnim.Type.Injure_0: 
											RivalOBJ.HandleTypeAnim(CRLuoAnim_Main.Type.Injure_0);
											break;
										case RivalInjuredAnim.Type.Injure_1:
											RivalOBJ.HandleTypeAnim(CRLuoAnim_Main.Type.Injure_1);
											break;
										case RivalInjuredAnim.Type.Injure_2:
											RivalOBJ.HandleTypeAnim(CRLuoAnim_Main.Type.Injure_2);
											break;
										case RivalInjuredAnim.Type.Injure_Fly_Down:
											RivalOBJ.HandleTypeAnim(CRLuoAnim_Main.Type.Injure_Fly_Down);
											break;
										case RivalInjuredAnim.Type.Injure_Fly_Up:
											RivalOBJ.HandleTypeAnim(CRLuoAnim_Main.Type.Injure_Fly_Up);
											break;

										case RivalInjuredAnim.Type.Injure_Fly_Go:
											RivalOBJ.HandleTypeAnim(CRLuoAnim_Main.Type.Injure_Fly_Go);
											break;

										case RivalInjuredAnim.Type.Injure_G0:
										case RivalInjuredAnim.Type.Injure_GA: 
										RivalOBJ.HandleTypeAnim(CRLuoAnim_Main.Type.Injure_G0, CRLuoAnim_Main.Type.Injure_GA);
											break;
										case RivalInjuredAnim.Type.Injure_G1:
											RivalOBJ.HandleTypeAnim(CRLuoAnim_Main.Type.Injure_G1);
											break;
										case RivalInjuredAnim.Type.Injure_G2:
											RivalOBJ.HandleTypeAnim(CRLuoAnim_Main.Type.Injure_G2);
											break;
										case RivalInjuredAnim.Type.StandUp:
											RivalOBJ.HandleTypeAnim(CRLuoAnim_Main.Type.StandUp);
											break;

									}
								}



								if (myRivalAmin[i].myRivalAminElement[j].Time_Continue != 0)
								{
									MiniItween.DeleteType(RivalOBJ.gameObject, MiniItween.Type.Move);
									Vector3 MovePos = this.transform.TransformPoint(myRivalAmin[i].myRivalAminElement[j].v3_RivalGoPos);

									MiniItween.MoveTo(RivalOBJ.gameObject, MovePos , myRivalAmin[i].myRivalAminElement[j].Time_Continue,myRivalAmin[i].myRivalAminElement[j].PosRotType, false);

									MiniItween.DeleteType(RivalOBJ.gameObject, MiniItween.Type.Rotate);
									Vector3 RotateRot = myRivalAmin[i].myRivalAminElement[j].v3_RivalGoRot + RivalOBJ.gameObject.transform.rotation.eulerAngles;

									MiniItween.RotateTo(RivalOBJ.gameObject, RotateRot, myRivalAmin[i].myRivalAminElement[j].Time_Continue, myRivalAmin[i].myRivalAminElement[j].PosRotType, true);
								}
								

							}
						}

					}

				}
			}

		}

	}
	

	//生命特效
	IEnumerator CreateLifeFx(CRLuoAnim_Main.Type Type)
	{
		//----
		//判断当前   特效类数组不为空 并且 数组长度不为0
		if (myFxLifeAdvanced != null && myFxLifeAdvanced.Length != 0)
		{
			//循环枚举 类组的各瞿谌苓上限为类组的长度
			for (int i = 0; i < myFxLifeAdvanced.Length; i++)
			{
				//判断当前类组i中的 Type 是否与传入的动作名一致 并且 特效对象不为空  并且     绑定对象不为空
				if (myFxLifeAdvanced[i].go_TargetBones != null && myFxLifeAdvanced[i].Life_FX != null && myFxLifeAdvanced[i].ON_OFF)
				{
					if (myFxLifeAdvanced[i].LifeUse != null && myFxLifeAdvanced[i].LifeUse.Length != 0)
					{
						for (int j = 0; j < myFxLifeAdvanced[i].LifeUse.Length; j++)
						{

							if (myFxLifeAdvanced[i].LifeUse[j].UseType == Type)
							{


									//绦蛟诖说却?WaitForSeconds(等待秒数） 游戏界面仍然运动。
									yield return new WaitForSeconds(myFxLifeAdvanced[i].LifeUse[j].StartTime);

									//创建粒子物体，temp继承GameObject类属性（认爹）   实例化特效。
									GameObject temp = EmptyLoad.CreateObj(myFxLifeAdvanced[i].Life_FX);

									//temp临时对象 继承绑定物体位置
									temp.transform.parent = myFxLifeAdvanced[i].go_TargetBones.transform;

									temp.transform.localPosition = myFxLifeAdvanced[i].v3_FXPos;

									temp.transform.localRotation = Quaternion.Euler(myFxLifeAdvanced[i].v3_FXRotation.x, myFxLifeAdvanced[i].v3_FXRotation.y, myFxLifeAdvanced[i].v3_FXRotation.z);
									temp.layer = this.gameObject.layer;
							}
						}

					}

				}
			}

		}
	}


	//摄像机震动效果
	IEnumerator CreateCamearFx(CRLuoAnim_Main.Type Type)
	{

		if (CameraFx_IN != null && myCamearFX != null && myCamearFX.Length != 0)
		{
			for (int i = 0; i < myCamearFX.Length; i++)
			{
				if ( myCamearFX[i].CamearFx_Key && myCamearFX[i].CamearFx_Time != 0 && (myCamearFX[i].Use_X || myCamearFX[i].Use_Y || myCamearFX[i].Use_Z))
				{

					Vector3 shake_V3 = new Vector3(0, 0, 0);
					if (myCamearFX[i].Use_X)
					{
						shake_V3 += new Vector3(myCamearFX[i].Scale, 0, 0);
					}
					if (myCamearFX[i].Use_Y)
					{
						shake_V3 += new Vector3(0, myCamearFX[i].Scale, 0);
					}
					if (myCamearFX[i].Use_Z)
					{
						shake_V3 += new Vector3(0, 0, myCamearFX[i].Scale);
					}
					if (myCamearFX[i].CamearUse != null && myCamearFX[i].CamearUse.Length != 0)
					for (int j = 0; j < myCamearFX[i].CamearUse.Length; j++)
					{
						if (myCamearFX[i].CamearUse[j].UseType == Type)
						{
							yield return new WaitForSeconds(myCamearFX[i].CamearUse[j].StartTime);
							MiniItween itween = MiniItween.Shake(CameraFx_IN, shake_V3, myCamearFX[i].CamearFx_Time, MiniItween.EasingType.EaseInOutQuart);
							itween.b_handleScaleTime = true;
						}

					}
				}
			}
		}
	}

	//物体移位效果
	IEnumerator CreateMove(CRLuoAnim_Main.Type Type)
	{
		//判断当前   特Ю嗍?椴晃?赃⑶堰数组长度不为0
		if (myMove != null && myMove.Length != 0)
		{
			//循访毒剡类组的各个内容 上限为类组的长度
			for (int i = 0; i < myMove.Length; i++)
			{
				//判断当前类组i中的 Type 是否与传入的动作名一致 并且 特效对象不为空  并且     绑定对象不为空

				//try {
				if (myMove[i].ActionType == Type && myMove[i].Move_Key)
				{

					//MiniItween.MoveTo(this.gameObject, myAttackFX[i].MoveTo_Pos + this.gameObject.transform.position, myAttackFX[i].Move_Time, MiniItween.EasingType.AnimationCurve, true).myExtraData
					//      = new ForMiniItween.ExtraData(myAttackFX[i].curve);



					//MiniItween.RotateTo(this.gameObject, myAttackFX[i].MoveTo_Rot + this.gameObject.transform.eulerAngles, myAttackFX[i].Move_Time, MiniItween.EasingType.AnimationCurve, true).myExtraData
					//      = new ForMiniItween.ExtraData(myAttackFX[i].curve);


					if (myMove[i].Move_Time > 0)
					{
						yield return new WaitForSeconds(myMove[i].StartTime);

						if (myMove[i].MoveTo_Pos != Vector3.zero)
						{

							MiniItween.MoveTo(this.gameObject, this.transform.TransformPoint(myMove[i].MoveTo_Pos), myMove[i].Move_Time, myMove[i].MoveType, true);

							//MiniItween itween = MiniItween.MoveTo(this.gameObject, myMove[i].MoveTo_Pos + this.gameObject.transform.position, myMove[i].Move_Time, myMove[i].MoveType, true);

						}
						if (myMove[i].MoveTo_Rot != Vector3.zero)
						{
							MiniItween.RotateTo(this.gameObject, this.transform.TransformDirection(myMove[i].MoveTo_Rot), myMove[i].Move_Time, myMove[i].MoveType, true);
							//MiniItween itween = MiniItween.RotateTo(this.gameObject, myMove[i].MoveTo_Rot + this.gameObject.transform.eulerAngles, myMove[i].Move_Time, myMove[i].MoveType, true);
						}
					}


					//      }
					//}catch(System.Exception e){
					//      Debug.Log(i+" "+myMove.Length);
				}



			}
		}
	}

	//摄像机慢速效果
	IEnumerator CreateTime(CRLuoAnim_Main.Type Type)
	{

		
		//判断当前   特效类数组不为空 并且 数组长度不为0
		if (myTimeFX != null && myTimeFX.Length != 0)
		{
			//循环枚举 类组的各个内容 上限为类组的长度
			for (int i = 0; i < myTimeFX.Length; i++)
			{
				//判断当前类组i中的 Type 是否与传入的动作名一致 并且 特效对象不为空  并且     绑定对象不为空

				if (myTimeFX[i].ActionType == Type && myTimeFX[i].Time_Key)
				{

					if (myTimeFX[i].Time_Long > 0 && myTimeFX[i].curve != null)
					{
							
						//Debug.LogWarning("---mandongzuo---");
							
						yield return new WaitForSeconds(myTimeFX[i].Start_Time);

						MiniItween.TimeScale(myTimeFX[i].curve.curve, myTimeFX[i].Time_Long);
						
						Invoke("BakeTimeSheep",myTimeFX[i].Time_Long+0.5f );
						
					}

				}



			}
		}
	}

	IEnumerator CreateHide(CRLuoAnim_Main.Type Type)
	{
		//判断当前   特效类数组不为空 并且 数组长度不为0
		if (myHideFX != null && myHideFX.Length != 0)
		{
			//循环枚举 类组的各个内容 上限为类组的长度
			for (int i = 0; i < myHideFX.Length; i++)
			{
				//判断当前类组i中的 Type 是否与传入的动作名一致 并且 特效对象晃?捱并且     绑定对象不为空

				if (myHideFX[i].Hide_Type == Type && myHideFX[i].Hide_Key)
				{

					if (mainMeshRender != null && ShadowObj!=null)
					{
						yield return new WaitForSeconds(myHideFX[i].StartTime);

						mainMeshRender.renderer.enabled = false;
						ShadowObj.renderer.enabled = false;

						yield return new WaitForSeconds(myHideFX[i].TimeLong);

						mainMeshRender.renderer.enabled = true;
						ShadowObj.renderer.enabled = true;


					}

				}



			}
		}
	}


	void BakeTimeSheep()
	{
		 Time.timeScale = NowTimeSheep ;
		
	}
	#region 刀馔衔鄙
	void TrailsCheck(CRLuoAnim_Main.Type animType)
	{
		if (trailController != null)
		{

			for (int i = 0; i < trailController.Length; i++)
			{
				trailController[i].Check(animType);
			}
		}
	}

	//第一次调用，以后不用 
	void TrailInit() { 
		if(trailController != null){
			for (int i = 0; i < trailController.Length;i++ )
			{
				trailController[i].Init();
			}
		}
	}

    //死亡
    public void Die() {
        //如果是死亡状态，调用退场效果
        DownStage();
        //删除当前物体时间为显示时间的两倍
        Destroy(this.gameObject, F_ShowTime * 2);
        //设置状态为退场标记
        str_CurAnim = "Over";

        if (prefab_Soul != null)
        {
            SkinnedMeshRenderer[] renders = GetComponentsInChildren<SkinnedMeshRenderer>();
            if (renders != null && renders.Length > 0)
            {
                //Vector3 targetPos = renders[0].bounds.center + Vector3.down * renders[0].bounds.extents.y;
                GameObject obj = EmptyLoad.CreateObj(prefab_Soul, RootBone.transform.position, Quaternion.identity);
                obj.layer = LayerMask.NameToLayer("Default");
            }
            prefab_Soul = null;
        }
    }

	#endregion

	#region 上台下台程序集
	//Ｐ驼叁泾昙牵?无材质变化 1、上场闪白阶段   2、上场闪白褪去显示颜色  3、下场涟诌4、闪白褪去物体消
    private int fadeParam = 0;
	//当前透明度
	private float f_CurAlpha;
	//开始透明度
	private float f_FromAlpha;
	//目标透明度
	private float f_TargetAlpha;
	//开始时间
	private float f_FromTime;
	//变化过程时间
	public float F_ShowTime = 0.5f;
	//变白材质列表
	[System.NonSerialized]
	public List<Material> list = new List<Material>();
	//原始材质列表
	[System.NonSerialized]
	public List<Material> list_Exist = new List<Material>();



	//自动搜索阴影变量存在标记
	private GameObject _ShadowObj = null;
	
	//阴影对象
	public GameObject ShadowObj{
		get{
			if (_ShadowObj == null)
			{
				//从一级子物体中寻找Shadow物体
				Transform aTrans = this.transform.FindChild("Shadow");
				if (aTrans != null)
				{
					_ShadowObj = aTrans.gameObject;
				}
				else
				{
					Debug.LogWarning("物体不存在阴影对象");
				}	
			}
			return _ShadowObj;
		}	
	}



	//材质球连接(变量为誉柯级寥犺
	private Shader _targetShader;
	public Shader targetShader
	{
		get
		{
			if (_targetShader == null)
			{
				_targetShader = Resources.Load ("Unpack/CRLuo/REDCommonModelDouble") as Shader;
			}
			return _targetShader;
		}
	}

	
    Material[] AddNewToExist(Material[] exist, Material newMaterial)
	{
		Material[] result = new Material[exist.Length + 1];
		for (int i = 0; i < exist.Length; i++)
		{
			result[i] = exist[i];
		}
		result[exist.Length] = newMaterial;
		return result;
	}

	//材质球去掉最后一个
	Material[] MaterialRemoveLast(Material[] exist)
	{
		List<Material> list_Temp = new List<Material>();
		list_Temp.Add(exist[0]);
		for (int i = 1; i < exist.Length - 1; i++)
		{
			list_Temp.Add(exist[i]);
		}
		return list_Temp.ToArray();
	}

	//材质除最后一个，全去掉
	List<Material> MaterialOnlyLast(List<Material> exist)
	{
		Debug.LogError("come here");
		List<Material> list_Temp = new List<Material>();
		list_Temp.Add(exist[exist.Count - 1]);
		return list_Temp;
	}



	//上台主命令
	public void UpStage()
	{
		if (ShadowObj == null)
		{
			//如果在名称提示杏乡Clone)
			if (this.gameObject.name.EndsWith("(Clone)"))
			{
				//如果有克隆名称减少名称字符长度
				this.gameObject.name = this.gameObject.name.Substring(0, this.gameObject.name.Length - 7);
			}

			Debug.LogWarning(this.gameObject.name + "阴影没有连接");
		}
		ShadowObj.renderer.enabled = true;
		f_CurAlpha = 0;
		f_FromAlpha = 0;
		f_TargetAlpha = 1;
		f_FromTime = Time.time;
		//上台变化标记
		fadeParam = 1;

	}
	//下台主命令
	public void DownStage()
	{
		//增加白色材质球
		MaterialAddWhite();
		f_CurAlpha = 0;
		f_FromAlpha = 0;
		f_TargetAlpha = 1;
		f_FromTime = Time.time;
		//下台变化标记
		fadeParam = 3;
	}

	public void AddGoldenGlow(){
		Object aObject = Resources.Load("UnPack/CRLuo/GoldenGlow");
		List<Material> list = new List<Material>();
		foreach(Material aMaterial in mainMeshRender.renderer.materials){
			list.Add(aMaterial);
		}
		list.Add(Instantiate(aObject) as Material);
		//list.Insert(0,Instantiate(aObject) as Material);
		mainMeshRender.renderer.materials = list.ToArray();
	}

	//人物显示与消失主命令
	void StageShow()
	{
		if (fadeParam == 1)
		{
			if (f_CurAlpha != f_TargetAlpha)
			{
				//透明度变化
				f_CurAlpha = Mathf.Lerp(f_FromAlpha, f_TargetAlpha, (Time.time - f_FromTime) / F_ShowTime);
				//监测当前透明度与目标透明度差距
				if (Mathf.Abs(f_CurAlpha - f_TargetAlpha) < 0.01f)
				{
					//设置无差距
					f_CurAlpha = f_TargetAlpha;
					//这里代表完成了一次目标，开始现身
					fadeParam = 2;
					//在list_exist 中 便利设置原始材质球显示
					foreach (Material aMater in list_Exist)
					{
						//设置材质球通道限制 0
						aMater.SetFloat("_Cutoff", 0.5f);
					}
					//当前透明度
					f_CurAlpha = 1;
					//开始透明度
					f_FromAlpha = 1;
					//目标透明度
					f_TargetAlpha = 0;
					//开始时间
					f_FromTime = Time.time;
				}
				//每一帧设置白色材质球的透明度
				foreach (Material aMater in list)
				{
					aMater.SetColor("_Color", new Color(1, 1, 1, f_CurAlpha));
					if (ShadowObj != null)
					{
						ShadowObj.GetComponent<ShadowAnim>().ShowOpacityKey = f_CurAlpha;
					}
				}
			}
		}
		else if (fadeParam == 2)
		{
			if (f_CurAlpha != f_TargetAlpha)
			{
				f_CurAlpha = Mathf.Lerp(f_FromAlpha, f_TargetAlpha, (Time.time - f_FromTime) / F_ShowTime);
				if (Mathf.Abs(f_CurAlpha - f_TargetAlpha) < 0.01f)
				{
					f_CurAlpha = f_TargetAlpha;
					//这里代表完成了二次目标
					fadeParam = 0;
					//材质球返回正常
					MaterialReturnToExist();

				}
				foreach (Material aMater in list)
				{
					aMater.SetColor("_Color", new Color(1, 1, 1, f_CurAlpha));
				}
			}
		}
		else if (fadeParam == 3)
		{
			if (f_CurAlpha != f_TargetAlpha)
			{
				f_CurAlpha = Mathf.Lerp(f_FromAlpha, f_TargetAlpha, (Time.time - f_FromTime) / F_ShowTime);
				if (Mathf.Abs(f_CurAlpha - f_TargetAlpha) < 0.01f)
				{

					f_CurAlpha = f_TargetAlpha;
					//这里代表完成了一文勘樘
					fadeParam = 4;

					foreach (Material aMater in list_Exist)
					{

						aMater.SetFloat("_Cutoff", 1);

					}

					//list_Exist = MaterialOnlyLast(list_Exist);
					Material [] materials = new Material[1];
					materials[0] = mainMeshRender.renderer.materials[mainMeshRender.renderer.materials.Length - 1];
					mainMeshRender.renderer.materials = materials;

					//当前透明度
					f_CurAlpha = 1;
					//开始透明度
					f_FromAlpha = 1;
					//目标透明度
					f_TargetAlpha = 0;
					//开始时间
					f_FromTime = Time.time;
				}

				if (f_CurAlpha > 0.6f)
				{
//					if (prefab_Soul != null)
//					{
//						SkinnedMeshRenderer[] renders = GetComponentsInChildren<SkinnedMeshRenderer>();
//						if (renders != null && renders.Length > 0)
//						{
//							Vector3 targetPos = renders[0].bounds.center + Vector3.down * renders[0].bounds.extents.y;
//							GameObject obj = EmptyLoad.CreateObj(prefab_Soul, RootBone.transform.position, Quaternion.identity);
  //                          obj.layer = LayerMask.NameToLayer("Default");
	//					}
	//					prefab_Soul = null;
	//				}
				}

				foreach (Material aMater in list)
				{
					aMater.SetColor("_Color", new Color(1, 1, 1, f_CurAlpha));
				}
			}
		}
		else if (fadeParam == 4)
		{
			if (f_CurAlpha != f_TargetAlpha)
			{
				f_CurAlpha = Mathf.Lerp(f_FromAlpha, f_TargetAlpha, (Time.time - f_FromTime) / F_ShowTime);
				if (Mathf.Abs(f_CurAlpha - f_TargetAlpha) < 0.01f)
				{
					f_CurAlpha = f_TargetAlpha;
					//这里代表完成了二次目标
					fadeParam = 0;

				}
				foreach (Material aMater in list)
				{
					aMater.SetColor("_Color", new Color(1, 1, 1, f_CurAlpha));
					ShadowObj.GetComponent<ShadowAnim>().ShowOpacityKey = f_CurAlpha;
				}
			}
		}
	}




	#endregion

	public string _____________________________________________ = "角色音效";
	//播放音效
	public SoundsFx [] MySoundsFx;

	IEnumerator CreateSounds(CRLuoAnim_Main.Type Type)
	{
		//判断当前   特效类数组不为空 并且 数组长度不为0
		if (MySoundsFx != null && MySoundsFx.Length != 0)
		{
			//循环枚举 类组的各个内容 上限为类组的长度
			for (int i = 0; i < MySoundsFx.Length; i++)
			{
				//判断当前类组i中的 Type 是否与传氲亩?髅?恢吝并且 特效韵蟛晃?赃 并且     绑定对象不为空
				if (MySoundsFx[i].Sound_Type == Type && MySoundsFx[i].Use_Key)
				{
					//判断当前   特效类数组不为空 并且 数组长度不为0
					if (MySoundsFx[i].MySounds != null && MySoundsFx[i].MySounds .Length != 0)
					{
						//循环枚举 类组的各个内容 上限为类组的长度
						for (int j = 0; j < MySoundsFx[i].MySounds.Length; j++)
						{
							//判断当前类组i中的 Type 是否与传入的动作名一致 并且 特效对象不为空  并且     绑定对象不为空
							
							if (MySoundsFx[i].MySounds[j].Sound_Key)
							{
								if(MySoundsFx[i].MySounds[j].StartTime > 0)
									yield return new WaitForSeconds(MySoundsFx[i].MySounds[j].StartTime);

                                if(Core.Data != null && Core.Data.soundManager != null)
								Core.Data.soundManager.SoundFxPlay(MySoundsFx[i].MySounds[j].sounds);

				}

			}
					}}}
		}
	}




}

[System.Serializable]

//定义攻击特效类 
public class AttackFX
{
	//攻击特效

	// 定义 FXtype 为 Type 类型 
	public CRLuoAnim_Main.Type FXtype;


	//
    public GameObject go_TargetBones;

	//定义绑定位置位置偏移
	public Vector3 v3_FXPos;

	//定义绑定位置渲染偏移
	public Vector3 v3_FXRotation;


	public FxElement[] myFxElement;

}

[System.Serializable]
public class FxElement
{
	//攻击特效元素

	public bool ON_OFF = true;
	public bool UseNoParent = false;

	//定义物体特效对象变量
	public GameObject Prefab_FX;

	//定义攻击延迟之间变量
	public float FXtime;

	//ㄒ灏蠖ㄎ恢梦恢闷?派	
	public Vector3 v3_FXPos_offset;

	//定义绑定位置渲染偏移
	public Vector3 v3_FXRotation_offset;

	public bool QiGong = false;
	public bool DeleteFx;
	public float DeleteTime;
	public bool SendRival;
}

//敌人挨打动作 
[System.Serializable]
public class RivalAmin
{
	//攻击特效

	// 定义 FXtype 为 Type 类型 
	public CRLuoAnim_Main.Type FXtype;

	public RivalAminElement[] myRivalAminElement;

}

[System.Serializable]
public class RivalAminElement
{
	//攻击特效元素

	public bool ON_OFF = true;
	//定义物体特效对象变量
	public RivalInjuredAnim.Type RivalInjuredAnimName;

	public float Time_Wait;

	public float Time_Continue;
	public MiniItween.EasingType PosRotType;

	//移动到位置
	public Vector3 v3_RivalGoPos;

	//旋转到位置
	public Vector3 v3_RivalGoRot;
}


[System.Serializable]
public class FxLifeAdvanced
{
    public GameObject Life_FX;

	public bool ON_OFF = true;


	
	//定义特效绑定骨骼位置
	public GameObject go_TargetBones;

	//定义绑定位置位置偏移
	public Vector3 v3_FXPos;

	//定义绑定位置渲染偏移
	public Vector3 v3_FXRotation;

	public UseToType[] LifeUse;




}

[System.Serializable]
public class UseToType
{
	//生命呼吸特效应用类型

	// 定义 FXtype 为 Type 类型 
	public CRLuoAnim_Main.Type UseType;

	public float StartTime;
}


[System.Serializable]
public class Anim_Fx_ONOFF
{
	//特效开关

	//定义物体特效对象变量
	public GameObject FX_obj;

	public bool ON_OFF = true;

	public Fx_ONOFF_UseToType[] AnimFx_Use;




}

[System.Serializable]
public class Fx_ONOFF_UseToType
{
	// 定义 FXtype 为 Type 类型 
	public CRLuoAnim_Main.Type UseType;

	public float StartTime;

	public float TimeLong;
}




[System.Serializable]
public class MoveGo
{

	//物体移动

	public CRLuoAnim_Main.Type ActionType;

	public bool Move_Key;

	public Vector3 MoveTo_Pos;

	public Vector3 MoveTo_Rot;

	public MiniItween.EasingType MoveType;

	public float StartTime;

	public float Move_Time;



}

[System.Serializable]
public class TimeGo
{

	//镜头变速
	public CRLuoAnim_Main.Type ActionType;

	public bool Time_Key;

	public Curve curve;

	public float Time_Long;

	public float Start_Time;

}


[System.Serializable]
public class CamearFX
{

	//摄像机震动;

	public bool CamearFx_Key;

	public float CamearFx_Time;

	public float Scale = 1;

	public bool Use_X;

	public bool Use_Y;

	public bool Use_Z;

	public UseToType[] CamearUse;


}




[System.Serializable]
public class TrailController
{
	//这个尾巴的对象
	public MeleeWeaponTrail trail;

	// 颜色
	public Color[] color;

	//public Vector2 v2_Tiling = new Vector2(1,1);

	//public Material material;

	public TrailControllerSwitcher[] animsOfUsingThisTrail;

	public void Check(CRLuoAnim_Main.Type animType)
	{
		TrailControllerSwitcher tCS = null;
		for (int i = 0; i < animsOfUsingThisTrail.Length; i++)
		{
			if (animsOfUsingThisTrail[i].animType == animType)
			{
				tCS = animsOfUsingThisTrail[i];
				break;
			}
		}
		if (tCS != null)
		{
			trail.StopAllCoroutines();
			if (tCS.b_Enable)
			{
				if (tCS.f_StartTime > 0)
				{
					trail.StartCoroutine(Start(tCS.f_StartTime, tCS));
				}
				else
				{
					SetTrail(tCS);
				}
				if (tCS.f_EndTime > 0)
				{
					trail.StartCoroutine(End(tCS.f_EndTime));
				}
			}
			else
			{
				SetTrail(tCS);
			}
		}
		else
		{
			trail.Emit = false;
		}
	}

	public void Init() {
		trail.SetColor(color);
		trail.trailController = this;
	}

	public void SetTrail(TrailControllerSwitcher tCS)
	{
		trail.Emit = true;
		trail.SetLifeTime(tCS.leftTime);
		trail.SetColor(color);
		trail.SetSize(tCS.size);
	}

	public IEnumerator Start(float delayTime, TrailControllerSwitcher tCS)
	{
		yield return new WaitForSeconds(delayTime);
		SetTrail(tCS);
	}

	public IEnumerator End(float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		trail.Emit = false;
	}

}


[System.Serializable]
public class TrailControllerSwitcher
{
	public CRLuoAnim_Main.Type animType;
	public bool b_Enable;
	public float f_StartTime = 0;
	public float f_EndTime = 0;
	// 长度
	public float leftTime;

	// 宽度
	public float[] size;
}


[System.Serializable]
public class SoundsFx
{
	public CRLuoAnim_Main.Type Sound_Type;
	public bool Use_Key;
	public SoundsFx_Element [] MySounds;
	//public float f_EndTime = 0;
}
[System.Serializable]
public class SoundsFx_Element
{
	public bool Sound_Key;
	public SoundFx sounds;
	public float StartTime = 0;
	//public float f_EndTime = 0;
}

[System.Serializable]
public class HideFx
{
	public CRLuoAnim_Main.Type Hide_Type;
	public bool Hide_Key;
	public float StartTime = 0;
	public float TimeLong = 1;
}


//自定义动作类型（0：空  1：呼吸  X:死亡）
public class CRLuoAnim_Main {
    public enum Type { None, Idle, GoRush, BackRush, Free1, Free2, Attack, Skill, GroupSkill, OverSkill_0, OverSkill_1, OverSkill_2, Injure_0, Injure_1, Injure_2, Injure_Fly_Up, Injure_Fly_Down, Injure_G0, Injure_G1, Injure_G2, Defend, Show,StandUp, Injure_Fly_Go, PowerSkill, Injure_GA};
	public static CRLuoAnim_Main.Type[] types = new CRLuoAnim_Main.Type[] { Type.Idle, Type.Show, Type.Free1, Type.Free2, Type.Attack, Type.Skill, Type.GroupSkill, Type.OverSkill_0, Type.Injure_0, Type.Injure_1, Type.Injure_2, Type.Defend, Type.StandUp, Type.Injure_GA, Type.Injure_Fly_Go };
}

public class RivalInjuredAnim
{
	public enum Type { None, GoRus, BackRush, Attack, Defend, RandAnim, Injure_0, Injure_1, Injure_2, Injure_Fly_Up, Injure_Fly_Down, Injure_G0, Injure_G1, Injure_G2, Injure_Fly_Go, StandUp, Injure_GA };
}
