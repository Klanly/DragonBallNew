using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CRLuo_PlayAnim_FX : MonoBehaviour
{
	public int StarLevel;
	public string _ = "�������԰�ť123456789";
	public bool Try_Key;
	//��ɫID
	[System.NonSerialized]
	public int OBJScreen_ID;
	//��վλ���
	[System.NonSerialized]
	public bool Pos_L = true;

	//��վλ���
	[System.NonSerialized]
	public bool SaveLife = true;

	private Camera _MyCamera_L;
	public Camera MyCamera_L
	{
		get
		{
			if (_MyCamera_L == null)
			{
				//��������������Ѱ�������L����
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
					Debug.LogWarning("�������������");	
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
				//��������������Ѱ������R����
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
					Debug.LogWarning("�������������");
				}

			}

			return _MyCamera_R;
		}
	}
	public string ____ = "�������Ч��������";
	public bool CameraKey_Attack = false;
	public bool CameraKey_Skill = false;
	public bool CameraKey_GroupSkill = false;
	public bool CameraKey_OverSkill = true;
	public string _____ = "�սἼѭ������";
	public int OverSkillNUM = 0;
	//����ID��
	[System.NonSerialized]
	public int i_Num = 0;
	public string ______ = "����ģ������";
	public GameObject mainMeshRender;
	public string _______ = "�����������";
	public GameObject RootBone;

	//��������
	[System.NonSerialized]
	public CRLuo_PlayAnim_FX RivalOBJ;
	[System.NonSerialized]
	public bool Show_RivalAmin_Key = true;
	//����ʱ���ܶԷ����Ʊ��
	[System.NonSerialized]
	public bool Injure_Key = false;

	//public string ________ = "�ɱ�";
	//public enum teamType { None, Fire, Water, Wood, Light, Dark };
	//public teamType Team_Type;

	public string ________ = "��Ч�ܿ���";
	//��Ч�ܿ���
	public bool FX_Body_key = false;

	public string _________ = "����������Ч";
	//������Ч��������
	public ParticleSystem [] myBodyFX;

	public string __________ = "������Ч����";
	//����������Ч
	public Anim_Fx_ONOFF[] myFx_OnOff;

	public string ___________ = "��Ϊ������Ч";
	//����������Ч
	public FxLifeAdvanced [] myFxLifeAdvanced;

	public string ____________ = "����Ч����Ч";

	//������Ч������
	public AttackFX [] myAttackFX;

	public string _____________ = "������β��Ч";
	//������β
	public TrailController [] trailController;


	public string ______________ = "���˰�����";
	//������β
	public RivalAmin[] myRivalAmin;

	public string _______________ = "��ɫλ�ƿ���";
	//�����ƶ�
	public MoveGo [] myMove;

	public string ________________ = "���������Ч";
	//�������
	public GameObject CameraFx_IN;
	public CamearFX [] myCamearFX;

	public string _________________ = "�ӵ�ʱ����Ч";
	//ʱ�����
	public TimeGo [] myTimeFX;
	public string ____________________ = "����˲����Ч";
	public HideFx[] myHideFX;

	[System.NonSerialized]
	public float NowTimeSheep;


	//��ǰ����״̬��
	private string str_CurAnim = "";
	
	public string GetCurAnim(){
		return str_CurAnim;
	}

	public string __________________ = "��ɫ���";
	//���Ʋο��߲���
	public bool Scaleplate_Key = true;
	public float Scaleplate_Height = 5f;
	public float Scaleplate_Wide = 4f;
	public float Scaleplate_Long = 4f;
	public bool ScaleplateShowBox = false;
	public Vector3 ScaleplateOffset;
	public float OneShow_Height = 0f;
	public bool ShowBlood_Key = false;
	public float ShowBloodLong = 1;

	public string ___________________ = "������Ч";
	//���
	public GameObject prefab_Soul;

	public CRLuoAnim_Main.Type NowAnimType;

	//�����ײ��
	public void GenerateCollider()
	{
		BoxCollider boxColldier = this.gameObject.AddComponent<BoxCollider>();
		boxColldier.center = new Vector3(-ScaleplateOffset.x / this.transform.localScale.x, -ScaleplateOffset.y / this.transform.localScale.y, -ScaleplateOffset.z / this.transform.localScale.z) +
			Vector3.up * Scaleplate_Height / 2 / this.transform.localScale.y;
		boxColldier.size = new Vector3(Scaleplate_Wide / 2 / this.transform.localScale.x, Scaleplate_Height / 2 / this.transform.localScale.y, Scaleplate_Long / 2 / this.transform.localScale.z);
	}

	//�����߻���
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
	// ����Ԥ���в���
	void Start()
	{
		FX_Body_key = false;
		
		MyCamera_L.gameObject.SetActive(false);
		MyCamera_R.gameObject.SetActive(false);

        if (myAttackFX != null && myAttackFX.Length != 0)
		{
			//ѭ��ö�� ����ĸ������� ����Ϊ����ĳ���
			for (int i = 0; i < myAttackFX.Length; i++)
			{
				//�жϵ�ǰ����i����Type �Ƿ��봫��Ķ�����һ�� ���� ��Ч����Ϊ��  ���Ұ󶨶���Ϊ��
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
		//��������
		TrailInit();
	

		#region ��̨��̨������Ч����ʼ��
		//�����²�����
		//Material material = new Material(Shader.Find("Unlit/REDCommonModelDouble"));

		if (targetShader == null)
		{
			//�����������ʾ����(Clone)
			if (this.gameObject.name.EndsWith("(Clone)"))
			{
				this.gameObject.name = this.gameObject.name.Substring(0, this.gameObject.name.Length - 7);
			}

			Debug.LogWarning(this.gameObject.name + "û�и������ײ���");
		}
		Material material = new Material(targetShader);


		//�����ɫͼƬ
		Texture2D texture = (Texture2D)Resources.Load("Pictures/white");

		//��ͼƬ�����������ͼ����
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
					//�Ѳ��������ԭʼ����Ŀ¼
					list_Exist.Add(aMaterial);
				}

				Material[] result = AddNewToExist(aMeshRenderer.materials, material);

				aMeshRenderer.materials = result;

				//�Ѱ�ɫ��ͼ����������ڸǲ���Ŀ¼
				list.Add(result[result.Length - 1]);
			}

			//���������
			foreach (Material aMater in list_Exist)
			{
				aMater.SetFloat("_Cutoff", 1);

			}

			//�ð�ƬҲ����
			foreach (Material aMater in list)
			{
				aMater.SetColor("_Color", new Color(1, 1, 1, 0));
			}

			StartCoroutine(DelayStart());
			Invoke("Fx_main_Botton", F_ShowTime + i_brotherCount * F_GenerateGap);
			//��Ӱ�Ӳ���ʾ
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

	//�м������
	[System.NonSerialized]
	public int i_brotherCount;

	//���ɵļ�϶
	public const float F_GenerateGap = 0.5f;

	[System.NonSerialized]
	public bool b_StandAlone = false;

	//�ǳ�����
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

	//��ʱ��̨
	IEnumerator DelayStart()
	{
		yield return new WaitForSeconds(i_brotherCount * F_GenerateGap);
		//��̨Ч��
		UpStage();

		if (Application.loadedLevelName == "Game" && b_StandAlone == false)
		{
			Invoke("BanRandomFree", 0.4f);
		}

	}


	void BanRandomFree(){
		//������Ŷ���
		if(Random.Range(0,2) == 0){
			HandleTypeAnim(CRLuoAnim_Main.types[0]);
		}else{
			HandleTypeAnim(CRLuoAnim_Main.types[1]);
		}
	}



	int OverSkill_Remaining;

	//ÿ֡��������
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

			//����������������һ������Ϊ�������ͽ���״̬
            else if (str_CurAnim != CRLuoAnim_Main.types[CRLuoAnim_Main.types.Length - 1].ToString()
			         && str_CurAnim != CRLuoAnim_Main.Type.Injure_GA.ToString() && str_CurAnim != "Over")
			{
				//����Ĭ�϶���
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

				//������ǰ�����������б�
				Transform[] children = transform.GetComponentsInChildren<Transform>();

				//�ж��������������Ƿ�Ϊ��
				if (myFxLifeAdvanced != null && myFxLifeAdvanced.Length > 0)
				{
					//�����ȡ������������
					foreach (FxLifeAdvanced aFxLifeAdvanced in myFxLifeAdvanced)
					{


						foreach (UseToType aUseToType in aFxLifeAdvanced.LifeUse)
						{

							//�жϵ�ǰ��������������Ч�ǿ�
							if (aFxLifeAdvanced.Life_FX != null && aUseToType.UseType == CRLuoAnim_Main.types[CRLuoAnim_Main.types.LongLength - 1])
							{
								//���������б��в���ȡ����
								foreach (Transform aTransform in children)
								{
									//����������������а�������Ч���ƣ�
									if (aTransform.name.StartsWith(aFxLifeAdvanced.Life_FX.name))
									{
										//������Ч������Ϊ��
										aTransform.parent = null;
									}
								}
							}
						}
					}
				}

				if (myAttackFX != null && myAttackFX.Length > 0)
				{
					//��ȡ������Ч���е�Ԫ��
					foreach (AttackFX aAttackFX in myAttackFX)
					{
						//�жϵ�ǰ��Ч�Ƿ�Ϊ������Ч
						if (aAttackFX.FXtype == CRLuoAnim_Main.types[CRLuoAnim_Main.types.LongLength - 1])
						{
							if (aAttackFX.myFxElement != null && aAttackFX.myFxElement.Length > 0)
							{
								//������Ч������������							
								foreach (FxElement aFxElement in aAttackFX.myFxElement)
								{
									if (aFxElement.Prefab_FX != null)
									{
										//�������������������ȡ
										foreach (Transform aTransform in children)
										{
											//�����ȡģ��.����.���ڣ���������Ч.��?���ƣ�
											if (aTransform.name.StartsWith(aFxElement.Prefab_FX.name))
											{
												//���õ�ǰ��ЧΪ��
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

        #region �ܲ��Կ���
		if (Try_Key)
		{


			//		�??��
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				//����Ĭ�϶�������
				HandleTypeAnim(CRLuoAnim_Main.types[1]);
			}
			//����� 2 ��
			else if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				//���ù�����������
				HandleTypeAnim(CRLuoAnim_Main.types[2]);
			}
			//����� 3 ��
			else if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				//���ü��ܶ�������
				HandleTypeAnim(CRLuoAnim_Main.types[3]);
			}
			//		��?��4 ��
			else if (Input.GetKeyDown(KeyCode.Alpha4))
			{
				//�������˶�������
				HandleTypeAnim(CRLuoAnim_Main.types[4]);
			}
			//����� 5 ��
			else if (Input.GetKeyDown(KeyCode.Alpha5))
			{
				//�������ɶ�������
                HandleTypeAnim(CRLuoAnim_Main.types[5]);
			}
			//����� 5 ��
			else if (Input.GetKeyDown(KeyCode.Alpha6))
			{
				//��������2��������
				HandleTypeAnim(CRLuoAnim_Main.types[6]);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha7))
			{
				//���ô�����������
				HandleTypeAnim(CRLuoAnim_Main.types[7]);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha8))
			{
				//����������������
				HandleTypeAnim(CRLuoAnim_Main.types[8]);
			}
			else if (Input.GetKeyDown(KeyCode.X))
			{
				//��Ч����
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
		//��̨��̨��ǣ������Ϊ0˵������̨����̨�Ĺ�����
		if (fadeParam != 0)
		{
			//��̨��ʾ
			StageShow();
		}
	}


	//�����۾��ĸ߶�
	public Vector3 GetEyePos() {
		return this.transform.position + Scaleplate_Height * Vector3.up;
	}


	//�����������в���
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


	//���忪��������Ч���򼯣����ղ����ͱ�����
	public void BodyFX_ON_OFF(bool Key)
	{
		//�ж�������Ч���������Ƿ�Ϊ�ջ򳤶�Ϊ0
		if (myBodyFX.Length != 0)
		{
			//ѭ��ö����Ч���ݣ�����Ϊ���鳤��
			for (int i = 0; i < myBodyFX.Length; i++)
			{
				//�ж���Ч������
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
	/// ��������Ծ��߼�
	/// </summary>
	public void HandleFightVs() {
		if(RivalOBJ == null) return;

		//�Ȳ��Ź�������
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

	//���幥����Ч����
	public void HandleTypeAnim(CRLuoAnim_Main.Type animType, CRLuoAnim_Main.Type aliasType = CRLuoAnim_Main.Type.None)
	{
		TrailsCheck(animType);
		//����ģ��attack��������
		animation.CrossFade(animType.ToString());
		//���õ�ǰ������Ϊattack������������
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
			//��myAttackFX[]������Ч�ࣨ���飩����ȡ���� ������attack�� ����Ч
			StartCoroutine("CreateFx", animType);
			StartCoroutine("CreateHide", animType);
		}
			
		StartCoroutine("CreateLifeFx", animType);
		StartCoroutine("AnimFx_ONOFF", animType);
		//�����ų�����Ч
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

	//������Ч����
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
									//�����ڴ˵ȴ� WaitForSeconds(�ȴ������� ��Ϸ������Ȼ�˶���
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


	//������Ч  ������ȡ������Ч ��  �вκ��� ���� ������
	IEnumerator CreateFx(CRLuoAnim_Main.Type Type)
	{
		//----
		//�жϵ�ǰ   ��Ч�����鲻Ϊ�� ���� ���鳤�Ȳ�Ϊ0
		if (myAttackFX != null && myAttackFX.Length != 0)
		{
			//ѭ��ö�� ����ĸ������� ����Ϊ����ĳ���
			for (int i = 0; i < myAttackFX.Length; i++)
			{
				//�жϵ�ǰ����i�е� Type �Ƿ��봫��Ķ�����һ�� ���� ��Ч����Ϊ��  ����     �󶨶���Ϊ��
				if (myAttackFX[i].FXtype == Type && myAttackFX[i].go_TargetBones != null)
				{
					if (myAttackFX[i].myFxElement != null && myAttackFX[i].myFxElement.Length != 0) 
					{
					
					
					for (int j = 0; j < myAttackFX[i].myFxElement.Length; j++)
					{
						if (myAttackFX[i].myFxElement[j].Prefab_FX != null && myAttackFX[i].myFxElement[j].ON_OFF)
						{
							//�����ڴ˵ȴ� WaitForSeconds(�ȴ������� ��Ϸ������Ȼ�˶���
							yield return new WaitForSeconds(myAttackFX[i].myFxElement[j].FXtime);

							//�����������壬temp�̳�GameObject����ԣ��ϵ?��  ʵ������Ч��

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
								//temp��ʱ���� �̳а�����λ��
								temp.transform.parent = myAttackFX[i].go_TargetBones.transform;
							}
							if (myAttackFX[i].myFxElement[j].QiGong){
								BanCurveLine aBanCurveLine = temp.GetComponent<BanCurveLine>();
								if( aBanCurveLine != null && RivalOBJ != null )  aBanCurveLine.target = RivalOBJ.RootBone;
								//aBanCurveLine.f_Time = 
							}

							//������Ч��ת
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
			//ѭ��ö�� ����ĸ������� ����Ϊ����ĳ���
			for (int i = 0; i < myRivalAmin.Length; i++)
			{
				//�жϵ�ǰ����i�е� Type �Ƿ��봫��Ķ�����һ�� ���� ��Ч����Ϊ��  ����     �󶨶���Ϊ��
				if (myRivalAmin[i].FXtype == Type && RivalOBJ != null)
				{
					if (myRivalAmin[i].myRivalAminElement != null && myRivalAmin[i].myRivalAminElement.Length != 0)
					{
						for (int j = 0; j < myRivalAmin[i].myRivalAminElement.Length; j++)
						{

							if (myRivalAmin[i].myRivalAminElement[j].RivalInjuredAnimName != RivalInjuredAnim.Type.None && myRivalAmin[i].myRivalAminElement[j].ON_OFF)
							{


								//�����ڴ˵ȴ� WaitForSeconds(�ȴ������� ��Ϸ������Ȼ�˶���
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
	

	//������Ч
	IEnumerator CreateLifeFx(CRLuoAnim_Main.Type Type)
	{
		//----
		//�жϵ�ǰ   ��Ч�����鲻Ϊ�� ���� ���鳤�Ȳ�Ϊ0
		if (myFxLifeAdvanced != null && myFxLifeAdvanced.Length != 0)
		{
			//ѭ��ö�� ����ĸ�����������Ϊ����ĳ���
			for (int i = 0; i < myFxLifeAdvanced.Length; i++)
			{
				//�жϵ�ǰ����i�е� Type �Ƿ��봫��Ķ�����һ�� ���� ��Ч����Ϊ��  ����     �󶨶���Ϊ��
				if (myFxLifeAdvanced[i].go_TargetBones != null && myFxLifeAdvanced[i].Life_FX != null && myFxLifeAdvanced[i].ON_OFF)
				{
					if (myFxLifeAdvanced[i].LifeUse != null && myFxLifeAdvanced[i].LifeUse.Length != 0)
					{
						for (int j = 0; j < myFxLifeAdvanced[i].LifeUse.Length; j++)
						{

							if (myFxLifeAdvanced[i].LifeUse[j].UseType == Type)
							{


									//����ڴ˵ȴ?WaitForSeconds(�ȴ������� ��Ϸ������Ȼ�˶���
									yield return new WaitForSeconds(myFxLifeAdvanced[i].LifeUse[j].StartTime);

									//�����������壬temp�̳�GameObject�����ԣ��ϵ���   ʵ������Ч��
									GameObject temp = EmptyLoad.CreateObj(myFxLifeAdvanced[i].Life_FX);

									//temp��ʱ���� �̳а�����λ��
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


	//�������Ч��
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

	//������λЧ��
	IEnumerator CreateMove(CRLuoAnim_Main.Type Type)
	{
		//�жϵ�ǰ   �ا���?鲻�?�ߢ������鳤�Ȳ�Ϊ0
		if (myMove != null && myMove.Length != 0)
		{
			//ѭ�ö�������ĸ������� ����Ϊ����ĳ���
			for (int i = 0; i < myMove.Length; i++)
			{
				//�жϵ�ǰ����i�е� Type �Ƿ��봫��Ķ�����һ�� ���� ��Ч����Ϊ��  ����     �󶨶���Ϊ��

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

	//���������Ч��
	IEnumerator CreateTime(CRLuoAnim_Main.Type Type)
	{

		
		//�жϵ�ǰ   ��Ч�����鲻Ϊ�� ���� ���鳤�Ȳ�Ϊ0
		if (myTimeFX != null && myTimeFX.Length != 0)
		{
			//ѭ��ö�� ����ĸ������� ����Ϊ����ĳ���
			for (int i = 0; i < myTimeFX.Length; i++)
			{
				//�жϵ�ǰ����i�е� Type �Ƿ��봫��Ķ�����һ�� ���� ��Ч����Ϊ��  ����     �󶨶���Ϊ��

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
		//�жϵ�ǰ   ��Ч�����鲻Ϊ�� ���� ���鳤�Ȳ�Ϊ0
		if (myHideFX != null && myHideFX.Length != 0)
		{
			//ѭ��ö�� ����ĸ������� ����Ϊ����ĳ���
			for (int i = 0; i < myHideFX.Length; i++)
			{
				//�жϵ�ǰ����i�е� Type �Ƿ��봫��Ķ�����һ�� ���� ��Ч�����?�߲���     �󶨶���Ϊ��

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
	#region �����α�
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

	//��һ�ε��ã��Ժ��� 
	void TrailInit() { 
		if(trailController != null){
			for (int i = 0; i < trailController.Length;i++ )
			{
				trailController[i].Init();
			}
		}
	}

    //����
    public void Die() {
        //���������״̬�������˳�Ч��
        DownStage();
        //ɾ����ǰ����ʱ��Ϊ��ʾʱ�������
        Destroy(this.gameObject, F_ShowTime * 2);
        //����״̬Ϊ�˳����
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

	#region ��̨��̨����
	//���������ǣ?�޲��ʱ仯 1���ϳ����׽׶�   2���ϳ�������ȥ��ʾ��ɫ  3���³�����4��������ȥ������
    private int fadeParam = 0;
	//��ǰ͸����
	private float f_CurAlpha;
	//��ʼ͸����
	private float f_FromAlpha;
	//Ŀ��͸����
	private float f_TargetAlpha;
	//��ʼʱ��
	private float f_FromTime;
	//�仯����ʱ��
	public float F_ShowTime = 0.5f;
	//��ײ����б�
	[System.NonSerialized]
	public List<Material> list = new List<Material>();
	//ԭʼ�����б�
	[System.NonSerialized]
	public List<Material> list_Exist = new List<Material>();



	//�Զ�������Ӱ�������ڱ��
	private GameObject _ShadowObj = null;
	
	//��Ӱ����
	public GameObject ShadowObj{
		get{
			if (_ShadowObj == null)
			{
				//��һ����������Ѱ��Shadow����
				Transform aTrans = this.transform.FindChild("Shadow");
				if (aTrans != null)
				{
					_ShadowObj = aTrans.gameObject;
				}
				else
				{
					Debug.LogWarning("���岻������Ӱ����");
				}	
			}
			return _ShadowObj;
		}	
	}



	//����������(����Ϊ���¼��Ƞ�
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

	//������ȥ�����һ��
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

	//���ʳ����һ����ȫȥ��
	List<Material> MaterialOnlyLast(List<Material> exist)
	{
		Debug.LogError("come here");
		List<Material> list_Temp = new List<Material>();
		list_Temp.Add(exist[exist.Count - 1]);
		return list_Temp;
	}



	//��̨������
	public void UpStage()
	{
		if (ShadowObj == null)
		{
			//�����������ʾ����Clone)
			if (this.gameObject.name.EndsWith("(Clone)"))
			{
				//����п�¡���Ƽ��������ַ�����
				this.gameObject.name = this.gameObject.name.Substring(0, this.gameObject.name.Length - 7);
			}

			Debug.LogWarning(this.gameObject.name + "��Ӱû������");
		}
		ShadowObj.renderer.enabled = true;
		f_CurAlpha = 0;
		f_FromAlpha = 0;
		f_TargetAlpha = 1;
		f_FromTime = Time.time;
		//��̨�仯���
		fadeParam = 1;

	}
	//��̨������
	public void DownStage()
	{
		//���Ӱ�ɫ������
		MaterialAddWhite();
		f_CurAlpha = 0;
		f_FromAlpha = 0;
		f_TargetAlpha = 1;
		f_FromTime = Time.time;
		//��̨�仯���
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

	//������ʾ����ʧ������
	void StageShow()
	{
		if (fadeParam == 1)
		{
			if (f_CurAlpha != f_TargetAlpha)
			{
				//͸���ȱ仯
				f_CurAlpha = Mathf.Lerp(f_FromAlpha, f_TargetAlpha, (Time.time - f_FromTime) / F_ShowTime);
				//��⵱ǰ͸������Ŀ��͸���Ȳ��
				if (Mathf.Abs(f_CurAlpha - f_TargetAlpha) < 0.01f)
				{
					//�����޲��
					f_CurAlpha = f_TargetAlpha;
					//������������һ��Ŀ�꣬��ʼ����
					fadeParam = 2;
					//��list_exist �� ��������ԭʼ��������ʾ
					foreach (Material aMater in list_Exist)
					{
						//���ò�����ͨ������ 0
						aMater.SetFloat("_Cutoff", 0.5f);
					}
					//��ǰ͸����
					f_CurAlpha = 1;
					//��ʼ͸����
					f_FromAlpha = 1;
					//Ŀ��͸����
					f_TargetAlpha = 0;
					//��ʼʱ��
					f_FromTime = Time.time;
				}
				//ÿһ֡���ð�ɫ�������͸����
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
					//�����������˶���Ŀ��
					fadeParam = 0;
					//�����򷵻�����
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
					//������������һ�Ŀ���
					fadeParam = 4;

					foreach (Material aMater in list_Exist)
					{

						aMater.SetFloat("_Cutoff", 1);

					}

					//list_Exist = MaterialOnlyLast(list_Exist);
					Material [] materials = new Material[1];
					materials[0] = mainMeshRender.renderer.materials[mainMeshRender.renderer.materials.Length - 1];
					mainMeshRender.renderer.materials = materials;

					//��ǰ͸����
					f_CurAlpha = 1;
					//��ʼ͸����
					f_FromAlpha = 1;
					//Ŀ��͸����
					f_TargetAlpha = 0;
					//��ʼʱ��
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
					//�����������˶���Ŀ��
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

	public string _____________________________________________ = "��ɫ��Ч";
	//������Ч
	public SoundsFx [] MySoundsFx;

	IEnumerator CreateSounds(CRLuoAnim_Main.Type Type)
	{
		//�жϵ�ǰ   ��Ч�����鲻Ϊ�� ���� ���鳤�Ȳ�Ϊ0
		if (MySoundsFx != null && MySoundsFx.Length != 0)
		{
			//ѭ��ö�� ����ĸ������� ����Ϊ����ĳ���
			for (int i = 0; i < MySoundsFx.Length; i++)
			{
				//�жϵ�ǰ����i�е� Type �Ƿ��봫�Ķ?��?���߲��� ��Ч����?�� ����     �󶨶���Ϊ��
				if (MySoundsFx[i].Sound_Type == Type && MySoundsFx[i].Use_Key)
				{
					//�жϵ�ǰ   ��Ч�����鲻Ϊ�� ���� ���鳤�Ȳ�Ϊ0
					if (MySoundsFx[i].MySounds != null && MySoundsFx[i].MySounds .Length != 0)
					{
						//ѭ��ö�� ����ĸ������� ����Ϊ����ĳ���
						for (int j = 0; j < MySoundsFx[i].MySounds.Length; j++)
						{
							//�жϵ�ǰ����i�е� Type �Ƿ��봫��Ķ�����һ�� ���� ��Ч����Ϊ��  ����     �󶨶���Ϊ��
							
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

//���幥����Ч�� 
public class AttackFX
{
	//������Ч

	// ���� FXtype Ϊ Type ���� 
	public CRLuoAnim_Main.Type FXtype;


	//
    public GameObject go_TargetBones;

	//�����λ��λ��ƫ��
	public Vector3 v3_FXPos;

	//�����λ����Ⱦƫ��
	public Vector3 v3_FXRotation;


	public FxElement[] myFxElement;

}

[System.Serializable]
public class FxElement
{
	//������ЧԪ��

	public bool ON_OFF = true;
	public bool UseNoParent = false;

	//����������Ч�������
	public GameObject Prefab_FX;

	//���幥���ӳ�֮�����
	public float FXtime;

	//����λ��λ���?��	
	public Vector3 v3_FXPos_offset;

	//�����λ����Ⱦƫ��
	public Vector3 v3_FXRotation_offset;

	public bool QiGong = false;
	public bool DeleteFx;
	public float DeleteTime;
	public bool SendRival;
}

//���˰����� 
[System.Serializable]
public class RivalAmin
{
	//������Ч

	// ���� FXtype Ϊ Type ���� 
	public CRLuoAnim_Main.Type FXtype;

	public RivalAminElement[] myRivalAminElement;

}

[System.Serializable]
public class RivalAminElement
{
	//������ЧԪ��

	public bool ON_OFF = true;
	//����������Ч�������
	public RivalInjuredAnim.Type RivalInjuredAnimName;

	public float Time_Wait;

	public float Time_Continue;
	public MiniItween.EasingType PosRotType;

	//�ƶ���λ��
	public Vector3 v3_RivalGoPos;

	//��ת��λ��
	public Vector3 v3_RivalGoRot;
}


[System.Serializable]
public class FxLifeAdvanced
{
    public GameObject Life_FX;

	public bool ON_OFF = true;


	
	//������Ч�󶨹���λ��
	public GameObject go_TargetBones;

	//�����λ��λ��ƫ��
	public Vector3 v3_FXPos;

	//�����λ����Ⱦƫ��
	public Vector3 v3_FXRotation;

	public UseToType[] LifeUse;




}

[System.Serializable]
public class UseToType
{
	//����������ЧӦ������

	// ���� FXtype Ϊ Type ���� 
	public CRLuoAnim_Main.Type UseType;

	public float StartTime;
}


[System.Serializable]
public class Anim_Fx_ONOFF
{
	//��Ч����

	//����������Ч�������
	public GameObject FX_obj;

	public bool ON_OFF = true;

	public Fx_ONOFF_UseToType[] AnimFx_Use;




}

[System.Serializable]
public class Fx_ONOFF_UseToType
{
	// ���� FXtype Ϊ Type ���� 
	public CRLuoAnim_Main.Type UseType;

	public float StartTime;

	public float TimeLong;
}




[System.Serializable]
public class MoveGo
{

	//�����ƶ�

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

	//��ͷ����
	public CRLuoAnim_Main.Type ActionType;

	public bool Time_Key;

	public Curve curve;

	public float Time_Long;

	public float Start_Time;

}


[System.Serializable]
public class CamearFX
{

	//�������;

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
	//���β�͵Ķ���
	public MeleeWeaponTrail trail;

	// ��ɫ
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
	// ����
	public float leftTime;

	// ���
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


//�Զ��嶯�����ͣ�0����  1������  X:������
public class CRLuoAnim_Main {
    public enum Type { None, Idle, GoRush, BackRush, Free1, Free2, Attack, Skill, GroupSkill, OverSkill_0, OverSkill_1, OverSkill_2, Injure_0, Injure_1, Injure_2, Injure_Fly_Up, Injure_Fly_Down, Injure_G0, Injure_G1, Injure_G2, Defend, Show,StandUp, Injure_Fly_Go, PowerSkill, Injure_GA};
	public static CRLuoAnim_Main.Type[] types = new CRLuoAnim_Main.Type[] { Type.Idle, Type.Show, Type.Free1, Type.Free2, Type.Attack, Type.Skill, Type.GroupSkill, Type.OverSkill_0, Type.Injure_0, Type.Injure_1, Type.Injure_2, Type.Defend, Type.StandUp, Type.Injure_GA, Type.Injure_Fly_Go };
}

public class RivalInjuredAnim
{
	public enum Type { None, GoRus, BackRush, Attack, Defend, RandAnim, Injure_0, Injure_1, Injure_2, Injure_Fly_Up, Injure_Fly_Down, Injure_G0, Injure_G1, Injure_G2, Injure_Fly_Go, StandUp, Injure_GA };
}
