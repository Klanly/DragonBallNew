using UnityEngine;
using System.Collections;

public class CRLuo_ShowStage : MonoBehaviour {

	public string CharactorID ="0";
	public GameObject StageOBJ;
	public Camera CameraOBJ;
	[System.NonSerialized]
	public GameObject Temp_OBJ;

	public float RotSheepScale = 1f;
	public float BackWaitTime = 5f;
	public float BackTime = 1f;
	public Vector3 Default_Rot = new Vector3(0, 0, 0);

	public float CameraAdd = 2;

	Vector3 NowRotation;
	int CharacterAminNo;
	public bool NewOBJ = false;
	Vector2 Touch_O;
    CRLuo_PlayAnim_FX CRLuo;
	public bool Try_key;
    //最近一次按下的时间
    private float lastBtnDown;
    private const float AUTO_TIME = 10.0f;
    public float angel = 50.0f;
    public float time = 0.2f;
	public int MaxAnim = 4;
    //宠物的属性
    private MonsterAttribute attri;
    //宠物的缘分是否都配上
    private bool mAllFated;

	private float cameraPosz;

	void Awake()
	{
        lastBtnDown = Time.realtimeSinceStartup;
	}

	void Start()
	{
		cameraPosz = CameraOBJ.transform.localPosition.z;
	}

    public float GetModelHeight(){
        if (CRLuo != null)
        {
            Transform tTrans = CRLuo.gameObject.transform.FindChild("Bip001");
            return tTrans.localPosition.y;
        }
        else
        {
            return 0;
        }
    }

	// Update is called once per frame
	void Update()
	{

		if (Temp_OBJ != null && NewOBJ) 
		{
//            float a = CRLuo.mainMeshRender.renderer.bounds.center.y
//                + CRLuo.mainMeshRender.renderer.bounds.extents.y + CRLuo.OneShow_Height;

			CameraOBJ.transform.localPosition = new Vector3 (CRLuo.ScaleplateOffset.x, CRLuo.mainMeshRender.renderer.bounds.center.y+CRLuo.ScaleplateOffset.y, cameraPosz + CRLuo.ScaleplateOffset.z);
//            a = a * Screen.height / Screen.width + CameraAdd;
//			float b = Mathf.Abs( CameraOBJ.transform.localPosition.z);
//
//			float TempFOV = (Mathf.Atan (a / b) * 180 / Mathf.PI * 2);
//			CameraOBJ.fieldOfView = TempFOV;
			NewOBJ = false;
		}

		if (Input.GetMouseButtonDown(0))
		{
			MiniItween.DeleteType(StageOBJ, MiniItween.Type.Rotate);

			Touch_O = Input.mousePosition;

			NowRotation = StageOBJ.transform.rotation.eulerAngles;

			if (Temp_OBJ != null)
			{
                if(CRLuo.GetCurAnim() == "Idle") {
                    if(AW.Utils.UnityUtils.inScreenRect(CRLuo.mainMeshRender, CameraOBJ, Input.mousePosition )) {
                        lastBtnDown = Time.realtimeSinceStartup;
                        playAnim();
                    }
                }

			}
        } else if (Input.GetMouseButton(0)) {

			MiniItween.DeleteType(StageOBJ, MiniItween.Type.Rotate);
			
			Vector2 NewPos = Input.mousePosition;

			float NowLong = NewPos.x - Touch_O.x;
			
			NowLong *= RotSheepScale;

			StageOBJ.transform.rotation = Quaternion.Euler(new Vector3(0, NowRotation.y - NowLong, 0));
		}

        if((Time.realtimeSinceStartup - lastBtnDown) >= AUTO_TIME)  {
            lastBtnDown = Time.realtimeSinceStartup;
            playAnim();
        }

	}


    private void playAnim () {
        switch (CharacterAminNo)
        {
        case 0:
			CRLuo.HandleTypeAnim(CRLuoAnim_Main.Type.Free2);
            break;
        case 1:
			CRLuo.HandleTypeAnim(CRLuoAnim_Main.Type.Free1);
            break;
        case 2:
            CRLuo.HandleTypeAnim(CRLuoAnim_Main.Type.Attack);
            break;
        case 3:
            CRLuo.HandleTypeAnim(CRLuoAnim_Main.Type.Skill);
            break;
        case 4:
            CRLuo.HandleTypeAnim(CRLuoAnim_Main.Type.GroupSkill);
            break;
        case 5:
            CRLuo.HandleTypeAnim(CRLuoAnim_Main.Type.OverSkill_0);
            break;
        }

		CharacterAminNo = ( ++CharacterAminNo ) % MaxAnim;
	}
	
	public void DeleteSelf()
	{
		if (CRLuo != null)
		{
			Destroy (CRLuo.gameObject);
			CRLuo = null;
		}
		Destroy (this.gameObject);
	}


    public void Rotate(bool right) {

        if(right) {
            Quaternion to = Quaternion.Euler(new Vector3(0, NowRotation.y + angel, 0));
            MiniItween.RotateTo(StageOBJ, to , time);
        } else {
            Quaternion to = Quaternion.Euler(new Vector3(0, NowRotation.y - angel, 0));
            MiniItween.RotateTo(StageOBJ, to , time);
        }
    }


    public void ShowCharactor(int ID, MonsterAttribute attri = MonsterAttribute.DEFAULT_NO, bool allFated = false)
	{
       //NewOBJ = true;
        this.attri = attri;
        this.mAllFated = allFated;

        string prefabName = "pb" + ID;
        #if SPLIT_MODEL
		Object temp = ModelLoader.get3DModel(ID);
		if(temp != null)
		{
			AssetTask aTask = new AssetTask("pb" + ID, typeof(Object), FeatureShow);
			aTask.AppendCommonParam(ID, temp);
			FeatureShow(aTask);
		}
		else
		{
			if(Core.Data.sourceManager.IsModelExist(ID))
			{
				AssetTask aTask = new AssetTask(prefabName, typeof(Object), FeatureShow);
				aTask.AppendCommonParam(ID, null, AssetTask.loadType.Only_loadlocal);
				//再通过WWW加载
				aTask.DispatchToRealHandler();
			}
			else
			{
				Object tempDefault = PrefabLoader.loadFromUnPack("CRLuo/pbXXX", false, false);
				AssetTask task = new AssetTask(prefabName, typeof(Object), null);
				task.AppendCommonParam(ID, tempDefault);
				FeatureShow(task);
			}
		}
        #else
		Object temp = ModelLoader.get3DModel(ID);
		AssetTask aTask = new AssetTask(prefabName, typeof(Object), FeatureShow);
        aTask.AppendCommonParam(ID, temp);
        FeatureShow(aTask);
        #endif

	}

    private void FeatureShow(AssetTask task) {
        NewOBJ = true;
        Object temp = task.Obj;

        if (Temp_OBJ != null) {
            Destroy(Temp_OBJ);
            Temp_OBJ = null;
        }
            
        if (temp != null) {
            Temp_OBJ = Instantiate(temp, StageOBJ.transform.position, Quaternion.Euler(new Vector3(0,180,0))) as GameObject;
		} else {
			temp = PrefabLoader.loadFromUnPack("CRLuo/pbXXX", false, false);
			Temp_OBJ = Instantiate(temp, StageOBJ.transform.position, Quaternion.Euler(new Vector3(0,180,0))) as GameObject;
		}

        StageOBJ.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));

        Temp_OBJ.transform.parent = StageOBJ.transform;

        CRLuo = Temp_OBJ.GetComponent<CRLuo_PlayAnim_FX>();
        CRLuo.CameraKey_Attack = false;
        CRLuo.CameraKey_GroupSkill = false;
        CRLuo.CameraKey_OverSkill = false;
        CRLuo.CameraKey_Skill = false;

        if(attri == MonsterAttribute.ALL) {
            CRLuo.AddGoldenGlow();
        }

        CRLuo.BodyFX_ON_OFF(this.mAllFated);

    }

	#if DEBUG
	void OnGUI()
	{
		if (Try_key)
		{
			float width = 100;
			int buttonCount = 9;
			float buttonHeight = Screen.height / buttonCount;

			CharactorID = GUI.TextField(new Rect(0, (int)(0 * buttonHeight), width, Screen.height / buttonCount), CharactorID);
			if (GUI.Button(new Rect(width, (int)(0 * buttonHeight), width, Screen.height / buttonCount / 2), "+"))
			{

				CharactorID = "" + (int.Parse(CharactorID) + 1);

				Object temp_R = ModelLoader.get3DModel(int.Parse(CharactorID));

				if (temp_R != null)
				{
                    ShowCharactor(int.Parse(CharactorID));
				}

				else
				{
					for (int i = int.Parse(CharactorID); i < 10999; i++)
					{
						CharactorID = "" + i;

						temp_R = ModelLoader.get3DModel(i);

						if (temp_R != null)
						{
							ShowCharactor(i);
							break;
						}
					}
				}

			}

			if (GUI.Button(new Rect(width, (int)(1 * buttonHeight) / 2, width, Screen.height / buttonCount / 2), "-"))
			{
				if (int.Parse(CharactorID) > 0)
				{
					CharactorID = "" + (int.Parse(CharactorID) - 1);

					Object temp_R = ModelLoader.get3DModel(int.Parse(CharactorID));

					if (temp_R != null)
					{
						ShowCharactor(int.Parse(CharactorID));
					}

					else
					{
						for (int i = int.Parse(CharactorID); i > 0; i--)
						{
							CharactorID = "" + i;

							temp_R = ModelLoader.get3DModel(i);

							if (temp_R != null)
							{
								ShowCharactor(i);
								break;
							}
						}
					}
				}
			}


			if (GUI.Button(new Rect(0, (int)(1 * buttonHeight), width, Screen.height / buttonCount), "Show"))
			{
				ShowCharactor(int.Parse(CharactorID));
			}

		}
	}
	#endif
	
	public static CRLuo_ShowStage CreateShowStage()
	{
		Object obj = PrefabLoader.loadFromPack("CRLuo/System/ShowStage",false);
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			CRLuo_ShowStage ss = go.GetComponent<CRLuo_ShowStage>();
			return ss;
		}
		return null;
	}

	public static CRLuo_ShowStage CreateRoleShowStage(){
		Object obj = PrefabLoader.loadFromPack("CRLuo/System/ShowStage",false);
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			CRLuo_ShowStage ss = go.GetComponent<CRLuo_ShowStage>();
			ss.CameraOBJ.nearClipPlane = 1.0f;
			return ss;
		}
		return null;
	}

}










