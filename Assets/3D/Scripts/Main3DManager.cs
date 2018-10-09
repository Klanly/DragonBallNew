using UnityEngine;
using System.Collections;

public class Main3DManager : MonoBehaviour {

	public CRLuo_PlayAnim_FX Man_L;
	public CRLuo_PlayAnim_FX Man_R;

	CRLuo_PlayAnim_FX Man_temp;

	GameObject Man_GameObj_L;
	GameObject Man_GameObj_R;
	//ban
	public bool KillYou = false;
	public float Fly_Time = 0.5f;
	public float DoubleKill_Time = 1f;
	public Camera Main3DCamera;

	public float Long_Show = 3;
	public float Long_Near = 1;
	public float Long_Far  = 5;

	public float VS_Near   = -0.6f;

	//对决位置
	public float Vs_Pos    = 4;

	#region 新版本对决
	// -- 第一版的对决数据 -- 开启战斗
	public float Factor1_Time = 0.2f;
	public float Factor2_Time = 0.6f;
	public float Factor3_Time = 0.3f;

	// -- 第二版的对决数据 
	public float F_XuLi_Time  = 0.5F;
	public float F_Jump_Time  = 0.3F;
	public float F_Rush1_Time = 0.26F;
	public float F_Fight_Time = 0.6F;
	public float F_Rush2_Time = 0.3F;
	//起跳位移
	public float StartUp_Dis = 1f;

	public MiniItween.EasingType FadeIn;
	#endregion

	[HideInInspector]
	public GameObject Default_Charactor {
		get {
			return PrefabLoader.loadFromUnPack("CRLuo/pbXXX", false, true) as GameObject;
		}
	}

	[HideInInspector]
	public GameObject Default_Screen {
		get {
			return PrefabLoader.loadFromPack("CRLuo/Screen/pbScreen_XXX", false) as GameObject;
		}
	}

    public GameObject temp_Screen;

	public int Screen_ID;

	public BanCameraController BanCamera;

	public float OverSkillAddTime = 3f;
	public int OverSkillAddScale =10 ;

	/// <summary>
	/// 是否要开启点击屏幕的记录
	/// </summary>
	public bool OverSkill_Key = false;

	/// <summary>
	/// 单纯的UI控制
	/// </summary>
	public bool OverSkill_KeyUI = false;

	/// <summary>
	/// OverSkill1阶段统计的次数
	/// </summary>
	[HideInInspector]
	public int OverSkill_Add = 0;

	/// <summary>
	/// 是否是左边，如果是左边才记录OverSkill1的阶段统计
	/// </summary>
	private bool superLeft = false;

    //这里使用的是战斗的临时数据
    private TemporyData BattleInfo;
    //通知的回调
    public System.Action<int> NotifyCombo;

	// Use this for initialization
	void Start () {
		if (Man_L != null)
		{
			Man_GameObj_L = Man_L.gameObject;
			Man_L.gameObject.transform.position = new Vector3(0, 0, -Long_Show);
			Man_L.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
		}
		if (Man_R != null)
		{
			Man_GameObj_R = Man_R.gameObject;
			Man_R.gameObject.transform.position = new Vector3(0, 0, Long_Show);
			Man_R.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
		}

        if(Core.Data != null) BattleInfo = Core.Data.temper;
	}
	
	// Update is called once per frame
	void Update () {
		bool click = Input.GetMouseButtonDown(0);
		if (OverSkill_Key) {
			if (click) {
				if(BattleInfo == null || !BattleInfo.hasLiquidate) {
					if(TimeMgr.getInstance().WarPause == false)  //暂停的时候不能点击
						OverSkill_Add ++;
				} 
			}
		}

		if(OverSkill_KeyUI) {
			if(click) Core.Data.soundManager.BtnPlay(ButtonType.Confirm);
		}

        if(Man_L != null && Man_R != null && Man_L.NowAnimType == CRLuoAnim_Main.Type.Idle && Man_R.NowAnimType == CRLuoAnim_Main.Type.Idle) {
            Screen_Brightness_ONOFF(true);
        }

	}

	// --------- 强制关屏幕 --------

	private bool forcedscreenoff = false;  // true off screen

	public bool ForcedScreenOff {
		set {
			forcedscreenoff = value;
			if(temp_Screen == null) return;
			Screen_ON_OFF(!forcedscreenoff);
		}
		get {
			return forcedscreenoff;
		}
	}

	public float Screen_OFF_Brightness = 0.2f;
	bool Now_OffOn = true;
	void Screen_Brightness_ONOFF(bool Key) {
		if(ForcedScreenOff) return;

		if(Now_OffOn != Key)
			Screen_ON_OFF(Key);
	}

	void Screen_ON_OFF(bool Key){
		if (temp_Screen != null)
		{
			if (Key )
			{
				MeshRenderer[] Myrenderer = temp_Screen.GetComponentsInChildren<MeshRenderer>();
				foreach (MeshRenderer aMeshRenderer in Myrenderer)
				{
					foreach (Material aMaterial in aMeshRenderer.materials)
					{
						aMaterial.SetColor("_Color", new Color(1, 1, 1, 1));
					}
				}
			}
			else
			{

				MeshRenderer[] Myrenderer = temp_Screen.GetComponentsInChildren<MeshRenderer>();
				foreach (MeshRenderer aMeshRenderer in Myrenderer)
				{
					foreach (Material aMaterial in aMeshRenderer.materials)
					{
						aMaterial.SetColor("_Color", new Color(Screen_OFF_Brightness, Screen_OFF_Brightness, Screen_OFF_Brightness, 1));
					}
				}
			}
			Now_OffOn = Key;

			ParticleSystem [] MyPart = temp_Screen.GetComponentsInChildren<ParticleSystem>();
			foreach (ParticleSystem aMyPart in MyPart)
			{
				aMyPart.renderer.enabled = Key;
			}

		}
	}


    public void ShowScreen(int ID)
	{
		if (temp_Screen != null)
			Destroy(temp_Screen);
		
		Object temp = PrefabLoader.loadFromPack("CRLuo/Screen/pbScreen_" + ID, false);

		Vector3 NewPos = gameObject.transform.position;
		if(ID != 1)
			NewPos += new Vector3(6.0f, 0.0f, 0.0f);

		if (temp != null)
			temp_Screen = (GameObject)GameObject.Instantiate(temp, NewPos, this.gameObject.transform.rotation);
		else
			temp_Screen = (GameObject)GameObject.Instantiate(Default_Screen, NewPos, this.gameObject.transform.rotation);

		Screen_Brightness_ONOFF (true);
		Now_OffOn = true;
	}

	/// <summary>
	/// 这个是用来设定怒气技的连击阶段， 这个代码是（没有OverSkill1的阶段的), 目前版本不再使用
	/// </summary>
	/// <param name="clickCount">Click count.</param>
    void OverSkillEnd(int clickCount) {
        Man_temp.OverSkillNUM = clickCount;
		if(NotifyCombo != null) NotifyCombo(Man_temp.OverSkillNUM);

		OverSkill_Key = false;
		Invoke("changeUI", 3f);
	}

	/// <summary>
	/// 这个是用来设定，Overskill1阶段的连续技
	/// </summary>
	void OverSkillEnd1(bool left) {
		superLeft = left;

		#if !LOCAL_AUTO
		if(BattleInfo == null || !BattleInfo.hasLiquidate) {
			if(superLeft) {
				Man_temp.OverSkillNUM = OverSkill_Add / OverSkillAddScale;
				if(NotifyCombo != null) NotifyCombo(Man_temp.OverSkillNUM);
			}
		}
		#endif

		OverSkill_Key = false;
		OverSkill_Add = 0;
		Invoke("changeUI", 3f);
	}

	/// <summary>
	/// 包装函数
	/// </summary>
	IEnumerator OverSkillEndWrap(bool left, float delayTime) {
		yield return new WaitForSeconds(delayTime);
		OverSkillEnd1(left);
	}

	void changeUI() {
		OverSkill_KeyUI = false;
	}


	void VSAction(int id_L, int id_R) 
	{
		Screen_Brightness_ONOFF (true);
		Show_LAction(id_L);
		Show_RAction(id_R);
	}

    public CRLuo_PlayAnim_FX Show_LAction(int id_L)
	{
		Screen_Brightness_ONOFF (true);
		if (Man_GameObj_L != null)
			Destroy(Man_GameObj_L);


        string prefabName = "pb" + id_L;
        Man_L = null;

        #if SPLIT_MODEL
        Object temp_L = ModelLoader.get3DModel(id_L);
//		Debug.LogError (" temp  L ==" + temp_L );
		if(temp_L != null){
			AssetTask task = new AssetTask(prefabName, typeof(Object), null);
			task.AppendCommonParam(id_L, temp_L);
			FeatureLeft(task);
		}else{
//			Debug.LogError (" temp  L is null  == " + Core.Data.sourceManager.IsModelExist(id_L) );
            if(Core.Data.sourceManager.IsModelExist(id_L)){
				AssetTask aTask = new AssetTask(prefabName, typeof(Object), FeatureLeft);
				aTask.AppendCommonParam(id_L, null, AssetTask.loadType.Only_loadlocal);
				//再通过WWW加载
				aTask.DispatchToRealHandler();
			}else{
				Object tempDefault = PrefabLoader.loadFromUnPack("CRLuo/pbXXX", false, true);
				AssetTask task = new AssetTask(prefabName, typeof(Object), null);
				task.AppendCommonParam(id_L, tempDefault);
				FeatureLeft(task);
			}
		}

        #else

        Object temp_L = ModelLoader.get3DModel(id_L);
        AssetTask task = new AssetTask(prefabName, typeof(Object), null);
        task.AppendCommonParam(id_L, temp_L);
        FeatureLeft(task);
       
        #endif

		return Man_L;
	}

    public CRLuo_PlayAnim_FX Show_RAction(int id_R)
    {       
        Screen_Brightness_ONOFF (true);
        if (Man_GameObj_R != null)
            Destroy(Man_GameObj_R);

        string prefabName = "pb" + id_R;
        Man_R = null;

        #if SPLIT_MODEL
        Object temp_R = ModelLoader.get3DModel(id_R);
//		Debug.Log("  load  model   ==  " + temp_R);
		if(temp_R != null){
			AssetTask task = new AssetTask(prefabName, typeof(Object), null);
			task.AppendCommonParam(id_R, temp_R);
			FeatureRight(task);
		}else{
//			Debug.Log("  temp R==  null " + Core.Data.sourceManager.IsModelExist(id_R) );
            if(Core.Data.sourceManager.IsModelExist(id_R)){
				AssetTask aTask = new AssetTask(prefabName, typeof(Object), FeatureRight);
				aTask.AppendCommonParam(id_R, null, AssetTask.loadType.Only_loadlocal);
				//再通过WWW加载
				aTask.DispatchToRealHandler();
			}else{
				Object tempDefault = PrefabLoader.loadFromUnPack("CRLuo/pbXXX", false, true);
				AssetTask task = new AssetTask(prefabName, typeof(Object), null);
				task.AppendCommonParam(id_R, tempDefault);
				FeatureRight(task);
			}
		}
		#else
		Object temp_R = ModelLoader.get3DModel(id_R);
		AssetTask task = new AssetTask(prefabName, typeof(Object), null);
        task.AppendCommonParam(id_R, temp_R);
        FeatureRight(task);

        #endif

        return Man_R;
    }

    private void FeatureLeft (AssetTask task) {
        Object temp_L = task.Obj;
        if (temp_L != null) {
            Man_GameObj_L = (GameObject)GameObject.Instantiate(temp_L, new Vector3(0, 0, -Long_Show), Quaternion.Euler(new Vector3(0, 0, 0)));
		} else {
			Man_GameObj_L = (GameObject)GameObject.Instantiate(Default_Charactor, new Vector3(0, 0, -Long_Show), Quaternion.Euler(new Vector3(0, 0, 0)));
		}

        Man_L = Man_GameObj_L.GetComponent<CRLuo_PlayAnim_FX>();

        Man_L.MyCamera_L.cullingMask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("3D"));
        Man_L.MyCamera_L.depth = -1;

        Man_L.MyCamera_R.cullingMask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("3D"));
        Man_L.MyCamera_R.depth = -1;

        Man_L.OBJScreen_ID = task.arg1;

        Man_L.Pos_L = true;

        Man_L.HandleTypeAnim(CRLuoAnim_Main.Type.Show);

        MiniItween.MoveTo(Man_GameObj_L, new Vector3(0, 0, -Long_Show), Fly_Time);

        Man_L.Injure_Key = false;

        if (Man_R != null)
        {
            GoShowPos(false);
            Man_R.Injure_Key = false;
        }
    }

    private void FeatureRight (AssetTask task) {
        Object temp_R = task.Obj;

        if (temp_R != null) {
            Man_GameObj_R = (GameObject)GameObject.Instantiate(temp_R, new Vector3(0, 0, Long_Show), Quaternion.Euler(new Vector3(0, 180, 0)));
		} else {
			Man_GameObj_R = (GameObject)GameObject.Instantiate(Default_Charactor, new Vector3(0, 0, Long_Show), Quaternion.Euler(new Vector3(0, 180, 0)));
		}

        Man_R = Man_GameObj_R.GetComponent<CRLuo_PlayAnim_FX>();

        Man_R.MyCamera_L.cullingMask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("3D"));

        Man_R.MyCamera_L.depth = -1;

        Man_R.MyCamera_R.cullingMask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("3D"));
        Man_R.MyCamera_R.depth = -1;

        Man_R.OBJScreen_ID = task.arg1;

        Man_R.Pos_L = false;

        Man_R.Injure_Key = false;

        Man_R.HandleTypeAnim(CRLuoAnim_Main.Type.Show);

        MiniItween.MoveTo(Man_GameObj_R, new Vector3(0, 0, Long_Show), Fly_Time);

        if (Man_L != null)
        {
            GoShowPos( true);
            Man_L.Injure_Key = false;
        }
    }

	/// <summary>
	/// 这个方法已不再使用
	/// </summary>
	public void setCombo(int count, bool left){
		if(left) {
			if(Man_L != null) {
				Man_L.OverSkillNUM = count;
			}
		} else {
			if(Man_R != null) {
				Man_R.OverSkillNUM = count;
			}
		}
        
    }

	public void Free1_Ban(bool isLeft){
		StartCoroutine(Free1(isLeft));
	}

    public void Die(bool left) {
        CRLuo_PlayAnim_FX fx = left ? Man_L : Man_R;
        if(fx != null) {
            fx.Die();
        }
    }

	IEnumerator Free1(bool LiftObj)
	{

		Screen_Brightness_ONOFF (true);
		if (LiftObj)
		{
			if (Man_L != null)
			{
				float temp_Z = Man_GameObj_L.transform.position.z;

				if (temp_Z < -Long_Show)
				{
					Man_L.HandleTypeAnim(CRLuoAnim_Main.Type.GoRush);
				}
				else if (temp_Z > -Long_Show)
				{
					Man_L.HandleTypeAnim(CRLuoAnim_Main.Type.BackRush);
				}

				MiniItween.MoveTo(Man_GameObj_L, new Vector3(0, 0, -Long_Show), Fly_Time);
				yield return new WaitForSeconds(Fly_Time);

				Man_L.Injure_Key = false;
				Man_L.HandleTypeAnim(CRLuoAnim_Main.Type.Free1);
			}
		}
		else
		{
			if (Man_R != null)
			{
				float temp_Z = Man_GameObj_R.transform.position.z;

				if (temp_Z > Long_Show)
				{
					Man_R.HandleTypeAnim(CRLuoAnim_Main.Type.GoRush);
				}
				else if (temp_Z < Long_Show)
				{
					Man_R.HandleTypeAnim(CRLuoAnim_Main.Type.BackRush);
				}

				MiniItween.MoveTo(Man_GameObj_R, new Vector3(0, 0, Long_Show), Fly_Time);
				yield return new WaitForSeconds(Fly_Time);
				Man_R.Injure_Key = false;
				Man_R.HandleTypeAnim(CRLuoAnim_Main.Type.Free1);
			}
		}
	}

	void GoShowPos(bool LiftObj)
	{


		if (LiftObj)
		{
			if (Man_L != null)
			{
				float temp_Z = Man_GameObj_L.transform.position.z;

				if (temp_Z < -Long_Show)
				{
					Man_L.HandleTypeAnim(CRLuoAnim_Main.Type.GoRush);
				}
				else if (temp_Z > -Long_Show)
				{
					Man_L.HandleTypeAnim(CRLuoAnim_Main.Type.BackRush);
				}

				MiniItween.MoveTo(Man_GameObj_L, new Vector3(0, 0, -Long_Show), Fly_Time);

			}
		}
		else
		{
			if (Man_R != null)
			{
				float temp_Z = Man_GameObj_R.transform.position.z;

				if (temp_Z > Long_Show)
				{
					Man_R.HandleTypeAnim(CRLuoAnim_Main.Type.GoRush);
				}
				else if (temp_Z < Long_Show)
				{
					Man_R.HandleTypeAnim(CRLuoAnim_Main.Type.BackRush);
				}

				MiniItween.MoveTo(Man_GameObj_R, new Vector3(0, 0, Long_Show), Fly_Time);

			}
		}
	}

	/// <summary>
	/// 属性克制的挑衅
	/// </summary>
	public void Conflict_Provoke(bool left) {
		Screen_Brightness_ONOFF (true);
		GameObject Enhanced = Instantiate(PrefabLoader.loadFromUnPack("Ban/FX_KeZhi", false)) as GameObject;
		GameObject Dehanced = Instantiate(PrefabLoader.loadFromUnPack("Ban/FX_BeiKeZhi", false)) as GameObject;

		if (left) {
			if (Man_L != null) {
				Man_L.Injure_Key = false;
				Man_L.HandleTypeAnim(CRLuoAnim_Main.Type.Free2);
				RED.AddChild(Enhanced, Man_L.gameObject, new Vector3(0f, -0.16f, 0f));
				if (Man_R != null) {
					if(Man_R != null) {
						Man_R.Injure_Key = false;
						Man_R.HandleTypeAnim(CRLuoAnim_Main.Type.Injure_2);
						RED.AddChild(Dehanced, Man_R.gameObject, Vector3.zero);
					}
				}
			}
		} else {
			if (Man_R != null) {
				Man_R.Injure_Key = false;
				Man_R.HandleTypeAnim(CRLuoAnim_Main.Type.Free2);
				RED.AddChild(Enhanced, Man_R.gameObject, new Vector3(0f, -0.16f, 0f));
				if (Man_L != null) {
					if(Man_L != null) {
						Man_L.Injure_Key = false;
						Man_L.HandleTypeAnim(CRLuoAnim_Main.Type.Injure_2);
						RED.AddChild(Dehanced, Man_L.gameObject, Vector3.zero);
					}
				}
			}
		}
	}


    public void Free2_Ban(bool isLeft){
		StartCoroutine(Free2(isLeft));
	}

	IEnumerator Free2(bool LiftObj)
	{
		Screen_Brightness_ONOFF (true);
		if (LiftObj)
		{
			if (Man_L != null)
			{
				Man_L.Injure_Key = false;
				Man_L.HandleTypeAnim(CRLuoAnim_Main.Type.Free2);
				if (Man_R != null)
				{
					yield return new WaitForSeconds(2f);
					if(Man_R != null){
						Man_R.Injure_Key = false;
						Man_R.HandleTypeAnim(CRLuoAnim_Main.Type.Injure_2);
					}
				}
			}

		}
		else
		{
			if (Man_R != null)
			{
				Man_R.Injure_Key = false;
				Man_R.HandleTypeAnim(CRLuoAnim_Main.Type.Free2);
				if (Man_L != null)
				{
					yield return new WaitForSeconds(2f);
					if(Man_L != null){
						Man_L.Injure_Key = false;
						Man_L.HandleTypeAnim(CRLuoAnim_Main.Type.Injure_2);
					}
				}
			}

		}
	}

    //那一边Win
    public void Win(bool LeftObjWin) {
        Screen_Brightness_ONOFF (true);
        if (LeftObjWin) {
            if (Man_L != null) {
                Man_L.Injure_Key = false;
                Man_L.HandleTypeAnim(CRLuoAnim_Main.Type.Free2);
            }
        } else {
            if (Man_R != null) {
                Man_R.Injure_Key = false;
                Man_R.HandleTypeAnim(CRLuoAnim_Main.Type.Free2);
            }
        }
    }

	/// <summary>
	/// 对攻动画
	/// </summary>
	/// <param name="isLeft">If set to <c>true</c> is left.</param>
	public void AttackAction_Ban(bool isLeft){
		#if VS
		StartCoroutine(AttackAction(isLeft));
		#else
		StartCoroutine(Attack_VsEx(isLeft));
		#endif
	}

	IEnumerator AttackAction(bool LiftObj)
	{
		Screen_Brightness_ONOFF (true);
		if (Man_L != null && Man_R != null)
		{
			Man_L.HandleTypeAnim(CRLuoAnim_Main.Type.GoRush);
			Man_R.HandleTypeAnim(CRLuoAnim_Main.Type.GoRush);
			MiniItween.MoveTo(Man_GameObj_L, new Vector3(0, 0, -Long_Near), Fly_Time);
			MiniItween.MoveTo(Man_GameObj_R, new Vector3(0, 0, Long_Near), Fly_Time);
			yield return new WaitForSeconds(Fly_Time);

            //强制到特定的位置
            Man_GameObj_L.transform.localPosition = new Vector3(0, 0, -Long_Near);
            Man_GameObj_R.transform.localPosition = new Vector3(0, 0, Long_Near);

			if (LiftObj)
			{
				Man_L.RivalOBJ = Man_R;
				Man_R.RivalOBJ = null;
				Man_R.SaveLife = !KillYou;
				Man_L.Injure_Key = false;
				Man_L.HandleTypeAnim(CRLuoAnim_Main.Type.Attack);
			}
			else
			{
				Man_R.RivalOBJ = Man_L;
				Man_L.RivalOBJ = null;
				Man_L.SaveLife = !KillYou;
				Man_R.Injure_Key = false;
				Man_R.HandleTypeAnim(CRLuoAnim_Main.Type.Attack);
			}
		}
	}


	/// <summary>
	/// 阿飞的进化版，精简版的-对决
	/// </summary>
	/// <returns>The vs.</returns>
	/// <param name="leftObj">If set to <c>true</c> left object.</param>
	IEnumerator Attack_Vs (bool leftObj) {
		if (Man_L != null && Man_R != null) {
			Screen_Brightness_ONOFF (true);
			
			if (leftObj) {
				Man_L.RivalOBJ = Man_R;
				Man_R.RivalOBJ = null;
				Man_R.SaveLife = !KillYou;
				Man_L.Injure_Key = false;
				Man_L.HandleTypeAnim(CRLuoAnim_Main.Type.Attack);
			} else {
				Man_R.RivalOBJ = Man_L;
				Man_L.RivalOBJ = null;
				Man_L.SaveLife = !KillYou;
				Man_R.Injure_Key = false;
				Man_R.HandleTypeAnim(CRLuoAnim_Main.Type.Attack);
			}
						
			//蓄力位置
			yield return new WaitForSeconds(Factor1_Time);
			
			//起跳位置
			float Left_Z = Man_GameObj_L.transform.position.z, Right_Z = Man_GameObj_R.transform.position.z;
			MiniItween.MoveTo(Man_GameObj_L, new Vector3(0, 0, Left_Z + StartUp_Dis), Factor2_Time);
			MiniItween.MoveTo(Man_GameObj_R, new Vector3(0, 0, Right_Z - StartUp_Dis), Factor2_Time);
			yield return new WaitForSeconds(Factor2_Time);
			
			//前冲位移
			MiniItween.MoveTo(Man_GameObj_L, new Vector3(0, 0, Long_Far), Factor3_Time, FadeIn);
			MiniItween.MoveTo(Man_GameObj_R, new Vector3(0, 0, -Long_Far), Factor3_Time, FadeIn);
            yield return new WaitForSeconds(Factor3_Time);
		}
	}
    //go back to standard postion
	public void RePosition () {
		if (Man_L != null && Man_R != null) {
			Man_GameObj_L.transform.position = new Vector3(0, 0, -3);
			Man_GameObj_R.transform.position = new Vector3(0, 0, 3);
		}
	}

	//go back to standard postion
	public void RePositionEx () {
		if (Man_L != null && Man_R != null) {
			Man_GameObj_L.transform.position = new Vector3(0, 0, -5);
			Man_GameObj_R.transform.position = new Vector3(0, 0, 5);
		}
	}

	/// <summary>
	/// 阿飞进化版的第二次改动对决
	/// </summary>

	IEnumerator Attack_VsEx(bool leftObj) {
		if (Man_L != null && Man_R != null) {
			Screen_Brightness_ONOFF (true);

			if (leftObj) {
				Man_L.RivalOBJ = Man_R;
				Man_R.RivalOBJ = null;
				Man_R.SaveLife = !KillYou;
				Man_L.Injure_Key = false;
				Man_L.HandleFightVs();
			} else {
				Man_R.RivalOBJ = Man_L;
				Man_L.RivalOBJ = null;
				Man_L.SaveLife = !KillYou;
				Man_R.Injure_Key = false;
				Man_R.HandleFightVs();
			}

			//蓄力位置
			yield return new WaitForSeconds(F_XuLi_Time);

			//起跳位置
			Core.Data.soundManager.SoundFxPlay(SoundFx.FX_JumpWar);
			float Left_Z = Man_GameObj_L.transform.position.z, Right_Z = Man_GameObj_R.transform.position.z;
			MiniItween.MoveTo(Man_GameObj_L, new Vector3(0, 0, Left_Z + StartUp_Dis), F_Jump_Time);
			MiniItween.MoveTo(Man_GameObj_R, new Vector3(0, 0, Right_Z - StartUp_Dis), F_Jump_Time);
			yield return new WaitForSeconds(F_Jump_Time);

			Object obj = PrefabLoader.loadFromUnPack("Ban/FightVsEff", false, true);
			Instantiate(obj, new Vector3(0f, 0.72f, 0f), Quaternion.identity);
			Core.Data.soundManager.SoundFxPlay(SoundFx.FX_Vs);

			//前冲位移段1
			MiniItween.MoveTo(Man_GameObj_L, new Vector3(0, 0, VS_Near), F_Rush1_Time, FadeIn);
			MiniItween.MoveTo(Man_GameObj_R, new Vector3(0, 0, -VS_Near), F_Rush1_Time, FadeIn);
			yield return new WaitForSeconds(F_Rush1_Time);

			//对决暂停的时间
			yield return new WaitForSeconds(F_Fight_Time);

			//前冲位移段2
			MiniItween.MoveTo(Man_GameObj_L, new Vector3(0, 0, Long_Far), F_Rush2_Time, FadeIn);
			MiniItween.MoveTo(Man_GameObj_R, new Vector3(0, 0, -Long_Far), F_Rush2_Time, FadeIn);
			yield return new WaitForSeconds(F_Rush2_Time);
		}
	}

	public void Draw_Ban(){
		StartCoroutine(DoubleKill());
	}
	IEnumerator Draw()
	{
		Screen_Brightness_ONOFF (true);
		if (Man_L != null && Man_R != null)
		{
			Man_L.HandleTypeAnim(CRLuoAnim_Main.Type.GoRush);
			Man_R.HandleTypeAnim(CRLuoAnim_Main.Type.GoRush);
			MiniItween.MoveTo(Man_GameObj_L, new Vector3(0, 0, -Long_Near), Fly_Time);
			MiniItween.MoveTo(Man_GameObj_R, new Vector3(0, 0, Long_Near), Fly_Time);

			yield return new WaitForSeconds(Fly_Time);

			Man_L.RivalOBJ = null;
			Man_L.SaveLife = !KillYou;
			Man_L.Injure_Key = false;
			Man_R.RivalOBJ = null;
			Man_R.SaveLife = !KillYou;
			Man_R.Injure_Key = false;

			Man_L.HandleTypeAnim(CRLuoAnim_Main.Type.Attack);
			Man_R.HandleTypeAnim(CRLuoAnim_Main.Type.Attack);

			yield return new WaitForSeconds(DoubleKill_Time);

			Man_L.HandleTypeAnim(CRLuoAnim_Main.Type.BackRush);
			Man_R.HandleTypeAnim(CRLuoAnim_Main.Type.BackRush);

			MiniItween.MoveTo(Man_GameObj_L, new Vector3(0, 0, -Long_Far), Fly_Time);
			MiniItween.MoveTo(Man_GameObj_R, new Vector3(0, 0, Long_Far), Fly_Time);
		}

	}

	public void DoubleKill_Ban(){
		StartCoroutine(DoubleKill());
	}

	IEnumerator DoubleKill()
	{
		Screen_Brightness_ONOFF (false);
		if (Man_L != null && Man_R != null)
		{
			Man_L.HandleTypeAnim(CRLuoAnim_Main.Type.GoRush);
			Man_R.HandleTypeAnim(CRLuoAnim_Main.Type.GoRush);
			MiniItween.MoveTo(Man_GameObj_L, new Vector3(0, 0, -Long_Near), Fly_Time);
			MiniItween.MoveTo(Man_GameObj_R, new Vector3(0, 0, Long_Near), Fly_Time);

			yield return new WaitForSeconds(Fly_Time);

			Man_L.RivalOBJ = null;
			Man_L.SaveLife = false;
			Man_L.Injure_Key = false;
			Man_R.RivalOBJ = null;
			Man_R.SaveLife = false;
			Man_R.Injure_Key = false;

			Man_L.HandleTypeAnim(CRLuoAnim_Main.Type.Attack);
			Man_R.HandleTypeAnim(CRLuoAnim_Main.Type.Attack);


            //蓄力位置
            yield return new WaitForSeconds(Factor1_Time);

            //起跳位置
            float Left_Z = Man_GameObj_L.transform.position.z, Right_Z = Man_GameObj_R.transform.position.z;
            MiniItween.MoveTo(Man_GameObj_L, new Vector3(0, 0, Left_Z + StartUp_Dis), Factor2_Time);
            MiniItween.MoveTo(Man_GameObj_R, new Vector3(0, 0, Right_Z - StartUp_Dis), Factor2_Time);
            yield return new WaitForSeconds(Factor2_Time);

            Man_L.HandleTypeAnim(CRLuoAnim_Main.Type.Injure_Fly_Go);
            Man_R.HandleTypeAnim(CRLuoAnim_Main.Type.Injure_Fly_Go);

            //前冲位移
            MiniItween.MoveTo(Man_GameObj_L, new Vector3(0, 0, Long_Far), Factor3_Time, FadeIn);
            MiniItween.MoveTo(Man_GameObj_R, new Vector3(0, 0, -Long_Far), Factor3_Time, FadeIn);
            yield return new WaitForSeconds(Factor3_Time);

			//yield return new WaitForSeconds(DoubleKill_Time);
		}

	}

	public void SkillAction_Ban(bool lift){
        StartCoroutine(SkillAction(lift));
	}
	public void GroupSkill_Ban(bool lift)
	{
        StartCoroutine (GroupSkillAction (lift));
	}
    public void ChangeAttribute_Ban(bool left)
    {
        StartCoroutine (ChangeAttribute (left));
    }

    IEnumerator ChangeAttribute(bool left)
    {
        if (Man_L != null && Man_R != null)
        {
            Man_L.HandleTypeAnim(CRLuoAnim_Main.Type.BackRush);
            Man_R.HandleTypeAnim(CRLuoAnim_Main.Type.BackRush);
            MiniItween.MoveTo(Man_GameObj_L, new Vector3(0, 0, -Long_Far), Fly_Time);
            MiniItween.MoveTo(Man_GameObj_R, new Vector3(0, 0, Long_Far), Fly_Time);
            yield return new WaitForSeconds(Fly_Time);

            if (left)
            {
                Man_L.AddGoldenGlow ();
            }
            else
            {
                Man_R.AddGoldenGlow ();
            }
        }
    }

	IEnumerator SkillAction(bool LiftObj)
	{
		Screen_Brightness_ONOFF (false);
		if (Man_L != null && Man_R != null) {
			Man_L.HandleTypeAnim(CRLuoAnim_Main.Type.BackRush);
			Man_R.HandleTypeAnim(CRLuoAnim_Main.Type.BackRush);
			MiniItween.MoveTo(Man_GameObj_L, new Vector3(0, 0, -Long_Far), Fly_Time);
			MiniItween.MoveTo(Man_GameObj_R, new Vector3(0, 0, Long_Far), Fly_Time);
			yield return new WaitForSeconds(Fly_Time);

			if (LiftObj) {
				Man_L.RivalOBJ = Man_R;
				Man_R.RivalOBJ = null;
				Man_R.SaveLife = !KillYou;
				Man_L.Injure_Key = false;
				Man_L.HandleTypeAnim(CRLuoAnim_Main.Type.Skill);
			} else {
				Man_R.RivalOBJ = Man_L;
				Man_L.RivalOBJ = null;
				Man_L.SaveLife = !KillYou;
				Man_R.Injure_Key = false;
				Man_R.HandleTypeAnim(CRLuoAnim_Main.Type.Skill);
			}
		}
	}
	public void PowerSkillAction_Ban(bool lift){
		StartCoroutine(PowerSkill(lift));
	}

	IEnumerator PowerSkill(bool LiftObj)
	{		
		Screen_Brightness_ONOFF (false);
		if (LiftObj)
		{
			if (Man_L != null)
			{
				float temp_Z = Man_GameObj_L.transform.position.z;

				if (temp_Z < -Long_Show)
				{
					Man_L.HandleTypeAnim(CRLuoAnim_Main.Type.GoRush);
				}
				else if (temp_Z > -Long_Show)
				{
					Man_L.HandleTypeAnim(CRLuoAnim_Main.Type.BackRush);
				}

				MiniItween.MoveTo(Man_GameObj_L, new Vector3(0, 0, -Long_Show), Fly_Time);
				yield return new WaitForSeconds(Fly_Time);

				Man_L.Injure_Key = false;
				Man_L.HandleTypeAnim(CRLuoAnim_Main.Type.PowerSkill);
			}
		}
		else
		{
			if (Man_R != null)
			{
				float temp_Z = Man_GameObj_R.transform.position.z;

				if (temp_Z > Long_Show)
				{
					Man_R.HandleTypeAnim(CRLuoAnim_Main.Type.GoRush);
				}
				else if (temp_Z < Long_Show)
				{
					Man_R.HandleTypeAnim(CRLuoAnim_Main.Type.BackRush);
				}

				MiniItween.MoveTo(Man_GameObj_R, new Vector3(0, 0, Long_Show), Fly_Time);
				yield return new WaitForSeconds(Fly_Time);
				Man_R.Injure_Key = false;
				Man_R.HandleTypeAnim(CRLuoAnim_Main.Type.PowerSkill);
			}
		}
	}

	IEnumerator GroupSkillAction(bool LiftObj)
	{
		Screen_Brightness_ONOFF (false);
		if (Man_L != null && Man_R != null)
		{
			Man_L.HandleTypeAnim(CRLuoAnim_Main.Type.BackRush);
			Man_R.HandleTypeAnim(CRLuoAnim_Main.Type.BackRush);
			MiniItween.MoveTo(Man_GameObj_L, new Vector3(0, 0, -Long_Far), Fly_Time);
			MiniItween.MoveTo(Man_GameObj_R, new Vector3(0, 0, Long_Far), Fly_Time);
			yield return new WaitForSeconds(Fly_Time);

			if (LiftObj)
			{

				Man_L.RivalOBJ = Man_R;
				Man_R.RivalOBJ = null;
				Man_R.SaveLife = !KillYou;
				Man_L.Injure_Key = false;
				Man_L.HandleTypeAnim(CRLuoAnim_Main.Type.GroupSkill);
			}
			else
			{
				Man_R.RivalOBJ = Man_L;
				Man_L.RivalOBJ = null;
				Man_L.SaveLife = !KillYou;
				Man_R.Injure_Key = false;
				Man_R.HandleTypeAnim(CRLuoAnim_Main.Type.GroupSkill);
			}
		}
	}

	//angry skill -ban
    public void OverSkillAction_Ban(bool isLeft, int count = 0) {
        StartCoroutine(OverSkillAction(isLeft,count));
	}

    IEnumerator OverSkillAction(bool LiftObj, int count) {
		Screen_Brightness_ONOFF (false);
		if (Man_L != null && Man_R != null)
		{
			OverSkill_Key = true;
			OverSkill_KeyUI = true;
			///
			/// --- 这个是不记录Overskill1阶段连击数的 , 目前不再使用 ---
            //AsyncTask.QueueOnMainThread( () => {OverSkillEnd(count);}, OverSkillAddTime);

			/// 
			/// --- 这个是用来记录下OverSkill1阶段的连击数 ---
			StartCoroutine(OverSkillEndWrap(LiftObj, OverSkillAddTime) );

			Man_L.HandleTypeAnim(CRLuoAnim_Main.Type.GoRush);
			Man_R.HandleTypeAnim(CRLuoAnim_Main.Type.GoRush);
			MiniItween.MoveTo(Man_GameObj_L, new Vector3(0, 0, -Long_Near), Fly_Time);
			MiniItween.MoveTo(Man_GameObj_R, new Vector3(0, 0, Long_Near), Fly_Time);
			yield return new WaitForSeconds(Fly_Time);

			if (LiftObj)
			{
				Man_L.RivalOBJ = Man_R;
				Man_R.RivalOBJ = null;
				Man_R.SaveLife = !KillYou;
				Man_L.Injure_Key = false;
				Man_L.HandleTypeAnim(CRLuoAnim_Main.Type.OverSkill_0);

				Man_temp = Man_L;
			}
			else
			{
				Man_R.RivalOBJ = Man_L;
				Man_L.RivalOBJ = null;
				Man_L.SaveLife = !KillYou;
				Man_R.Injure_Key = false;
				Man_R.HandleTypeAnim(CRLuoAnim_Main.Type.OverSkill_0);
				
				Man_temp = Man_R;
			}
		}
	}

    /// <summary>
    /// 自爆技能
    /// </summary>
    public IEnumerator ExploreSelf(bool leftObj, int count = 0) {
        Screen_Brightness_ONOFF (false);
        if (Man_L != null && Man_R != null)
        {
            OverSkill_Key = true;
			OverSkill_KeyUI = true;
			///
			/// ----- 不记录Overskill1阶段的连击数，目前不再使用 ----
            //AsyncTask.QueueOnMainThread( () => {OverSkillEnd(count);}, OverSkillAddTime);

			///
			/// ----- 记录Overskill1的连击数
			StartCoroutine(OverSkillEndWrap(leftObj, OverSkillAddTime) );

            Man_L.HandleTypeAnim(CRLuoAnim_Main.Type.GoRush);
            Man_R.HandleTypeAnim(CRLuoAnim_Main.Type.GoRush);
            MiniItween.MoveTo(Man_GameObj_L, new Vector3(0, 0, -Long_Near), Fly_Time);
            MiniItween.MoveTo(Man_GameObj_R, new Vector3(0, 0, Long_Near), Fly_Time);
            yield return new WaitForSeconds(Fly_Time);

            if (leftObj)
            {
                Man_R.RivalOBJ = null;
                Man_R.SaveLife = true;
                Man_L.Injure_Key = false;
                Man_L.SaveLife = false;
                Man_L.HandleTypeAnim(CRLuoAnim_Main.Type.OverSkill_0);

                Man_temp = Man_L;
            }
            else
            {
                Man_L.RivalOBJ = null;
                Man_L.SaveLife = !KillYou;
                Man_R.Injure_Key = false;
                Man_R.HandleTypeAnim(CRLuoAnim_Main.Type.OverSkill_0);
                Man_R.SaveLife = false;
                Man_temp = Man_R;
            }
        }
    }

	public void FlyDown(bool leftObj, bool dead) {
		CRLuo_PlayAnim_FX fx = leftObj ? Man_L : Man_R;
		fx.SaveLife = !dead;

		fx.HandleTypeAnim(CRLuoAnim_Main.Type.Injure_Fly_Go);
	}

    /// <summary>
    /// 自爆群体技能 
    /// </summary>
    public IEnumerator GroupExploreSelf(bool leftObj) {
        Screen_Brightness_ONOFF (false);
        if (Man_L != null && Man_R != null)
        {
            Man_L.HandleTypeAnim(CRLuoAnim_Main.Type.BackRush);
            Man_R.HandleTypeAnim(CRLuoAnim_Main.Type.BackRush);
            MiniItween.MoveTo(Man_GameObj_L, new Vector3(0, 0, -Long_Far), Fly_Time);
            MiniItween.MoveTo(Man_GameObj_R, new Vector3(0, 0, Long_Far), Fly_Time);
            yield return new WaitForSeconds(Fly_Time);

            if (leftObj)
            {
				Man_R.RivalOBJ = Man_L;
				Man_R.SaveLife = !KillYou;
                Man_L.Injure_Key = false;
                Man_L.SaveLife = false;
                Man_L.HandleTypeAnim(CRLuoAnim_Main.Type.GroupSkill);
            }
            else
            {
				Man_L.RivalOBJ = Man_R;
                Man_L.SaveLife = !KillYou;
                Man_R.Injure_Key = false;
                Man_R.SaveLife = false;
                Man_R.HandleTypeAnim(CRLuoAnim_Main.Type.GroupSkill);
            }
        }
    }


	public string str_ID_L = "0";
	public string str_ID_R = "0";

	public bool showButtons = true;

	#if DEBUG
	void OnGUI()
	{
		if(!showButtons){
			return;
		}

		float width = 100;
		int buttonCount = 9;
		float buttonHeight = Screen.height / buttonCount;


		if (GUI.Button(new Rect(Screen.width / 2 - width*3, (int)(0 * buttonHeight), width, Screen.height / buttonCount), "VS"))
		{

			VSAction(int.Parse(str_ID_L), int.Parse(str_ID_R));
			BanCamera.GoDefault_Key = true;
		}
		
		if (KillYou)
		{
			if (GUI.Button(new Rect(Screen.width / 2 - width*2, (int)(0 * buttonHeight), width, Screen.height / buttonCount), "Kill"))
			{

				KillYou = !KillYou;
			}
		}
		else
		{
			if (GUI.Button(new Rect(Screen.width/ 2 - width*2, (int)(0 * buttonHeight), width, Screen.height / buttonCount), "Save"))
			{
				KillYou = !KillYou;
			}
		}

		if (GUI.Button(new Rect(Screen.width / 2 - width, (int)(0 * buttonHeight), width, Screen.height / buttonCount), "Draw"))
		{
			StartCoroutine("Draw");			
		}


		if (GUI.Button(new Rect(Screen.width / 2, (int)(0 * buttonHeight), width, Screen.height / buttonCount), "DoubleKill"))
		{
			StartCoroutine("DoubleKill");			
		}

		if (GUI.Button(new Rect(Screen.width / 2 + width, (int)(0 * buttonHeight), width, Screen.height / buttonCount), "LastScreen"))
		{
			if (Screen_ID > 0)
			{
				ShowScreen(--Screen_ID);
			}
		}

		if (GUI.Button(new Rect(Screen.width / 2 + width * 2, (int)(0 * buttonHeight), width, Screen.height / buttonCount), "NextScreen"))
		{
			ShowScreen(++Screen_ID);
		}




		str_ID_L = GUI.TextField(new Rect(0, (int)(0 * buttonHeight), width, Screen.height / buttonCount), str_ID_L);

		if (GUI.Button(new Rect( width ,(int)(0 * buttonHeight), width, Screen.height / buttonCount / 2), "+"))
		{

			str_ID_L = "" + (int.Parse(str_ID_L) + 1);

			Object temp_L = ModelLoader.get3DModel(int.Parse(str_ID_L));

			if (temp_L != null)
			{
				Show_LAction(int.Parse(str_ID_L));
			}

			else
			{
				for (int i = int.Parse(str_ID_L); i < 19999; i++)
				{
					str_ID_L = "" + i;

					temp_L = ModelLoader.get3DModel(i);

					if (temp_L != null)
					{
						Show_LAction(i);
						break;
					}
				}
			}
		}

		if (GUI.Button(new Rect( width, (int)(1 * buttonHeight) / 2, width, Screen.height / buttonCount / 2), "-"))
		{
			if (int.Parse(str_ID_L) > 0)
			{



				str_ID_L = "" + (int.Parse(str_ID_L) - 1);

				Object temp_L = ModelLoader.get3DModel(int.Parse(str_ID_L));

				if (temp_L != null)
				{
					Show_LAction(int.Parse(str_ID_L));
				}

				else
				{
					for (int i = int.Parse(str_ID_L); i > 0; i--)
					{
						str_ID_L = "" + i;

						temp_L = ModelLoader.get3DModel(i);

						if (temp_L != null)
						{
							Show_LAction(i);
							break;
						}
					}
				}
			}
		}


		if (GUI.Button(new Rect(0, (int)(1 * buttonHeight), width, Screen.height / buttonCount), "Show") )
		{
			Show_LAction(int.Parse(str_ID_L));
			
		}

		if (GUI.Button(new Rect(0, (int)(2 * buttonHeight), width, Screen.height / buttonCount), "Free1"))
		{

			StartCoroutine("Free1", true);

		}
		if (GUI.Button(new Rect(0, (int)(3 * buttonHeight), width, Screen.height / buttonCount), "Free2"))
		{

			StartCoroutine("Free2", true);
		}

		if (GUI.Button(new Rect(0, (int)(4 * buttonHeight), width, Screen.height / buttonCount), "Attack"))
		{

			AttackAction_Ban(true);
			
		}
		if (GUI.Button(new Rect(0, (int)(5 * buttonHeight), width, Screen.height / buttonCount), "Skill"))
		{

			StartCoroutine("SkillAction", true);
		}

		if (GUI.Button(new Rect(0, (int)(6 * buttonHeight), width, Screen.height / buttonCount), "PowerSkill"))
		{

			StartCoroutine("PowerSkill", true);
		}

		if (GUI.Button(new Rect(0, (int)(7 * buttonHeight), width, Screen.height / buttonCount), "GroupSkill"))
		{

			StartCoroutine("GroupSkillAction", true);
		}
		if (GUI.Button(new Rect(0, (int)(8 * buttonHeight), width, Screen.height / buttonCount), "OverSkill"))
		{

			OverSkillAction_Ban(true);
		}

		//----------------------------------------------------------------------


		str_ID_R = GUI.TextField(new Rect(Screen.width - width, (int)(0 * buttonHeight), width, Screen.height / buttonCount), str_ID_R);

		if (GUI.Button(new Rect(Screen.width - width*2, (int)(0 * buttonHeight), width, Screen.height / buttonCount/2), "+"))
		{



			str_ID_R = ""+(int.Parse(str_ID_R) + 1);

			Object temp_R = ModelLoader.get3DModel(int.Parse(str_ID_R));

			if (temp_R != null)
			{
				Show_RAction(int.Parse(str_ID_R));
			}

			else
			{
				for (int i = int.Parse(str_ID_R); i <19999; i++)
				{
					str_ID_R = ""+i;

					temp_R = ModelLoader.get3DModel(i);

					if (temp_R != null)
					{
						Show_RAction(i);
						break;
					}
				}
			}


		}

		if (GUI.Button(new Rect(Screen.width - width * 2, (int)(1 * buttonHeight)/2, width, Screen.height / buttonCount / 2), "-"))
		{
			if (int.Parse(str_ID_R) > 0)
			{



				str_ID_R = "" + (int.Parse(str_ID_R) - 1);

				Object temp_R = ModelLoader.get3DModel(int.Parse(str_ID_R));

				if (temp_R != null)
				{
					Show_RAction(int.Parse(str_ID_R));
				}

				else
				{
					for (int i = int.Parse(str_ID_R); i > 0; i--)
					{
						str_ID_R = "" + i;

						temp_R = ModelLoader.get3DModel(i);

						if (temp_R != null)
						{
							Show_RAction(i);
							break;
						}
					}
				}
			}
		}


		if (GUI.Button(new Rect(Screen.width - width, (int)(1 * buttonHeight), width, Screen.height / buttonCount), "Show"))
		{

			Show_RAction(int.Parse(str_ID_R));
		}
		if (GUI.Button(new Rect(Screen.width - width, (int)(2 * buttonHeight), width, Screen.height / buttonCount), "Free1"))
		{

			StartCoroutine("Free1", false);
		}
		if (GUI.Button(new Rect(Screen.width - width, (int)(3 * buttonHeight), width, Screen.height / buttonCount), "Free2"))
		{

			StartCoroutine("Free2", false);
		}
		if (GUI.Button(new Rect(Screen.width - width, (int)(4 * buttonHeight), width, Screen.height / buttonCount), "Attack"))
		{

			AttackAction_Ban(false);
		}
		if (GUI.Button(new Rect(Screen.width - width, (int)(5 * buttonHeight), width, Screen.height / buttonCount), "Skill"))
		{

			StartCoroutine("SkillAction", false);
		}
		if (GUI.Button(new Rect(Screen.width - width, (int)(6 * buttonHeight), width, Screen.height / buttonCount), "PowerSkill"))
		{

			StartCoroutine("PowerSkill", false);
		}
		if (GUI.Button(new Rect(Screen.width - width, (int)(7 * buttonHeight), width, Screen.height / buttonCount), "GroupSkill"))
		{

			StartCoroutine("GroupSkillAction", false);
		}
		if (GUI.Button(new Rect(Screen.width - width, (int)(8 * buttonHeight), width, Screen.height / buttonCount), "OverSkill"))
		{

			OverSkillAction_Ban(false);
		}




		if (GUI.Button(new Rect(Screen.width/ 2 - 50, Screen.height/6f*5f, 100, Screen.height / 6), "ON"))
		{
			//Screen_Brightness_ONOFF (true);
			RePositionEx();
		}


		if (GUI.Button(new Rect(Screen.width/ 2 + 50, Screen.height/6f*5f, 100, Screen.height / 6), "OFF"))
		{
			Screen_Brightness_ONOFF (false);
		}



	}
	#endif

}
