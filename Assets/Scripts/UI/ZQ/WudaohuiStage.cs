using UnityEngine;
using System.Collections;

public class WudaohuiStage : MonoBehaviour 
{
	//武道会站台
	public TextMesh[] m_txtName;
	[System.NonSerialized]
	public GameObject Temp_OBJ;
	CRLuo_PlayAnim_FX CRLuo;
	//宠物的属性
	private MonsterAttribute attri;
	//宠物的缘分是否都配上
	private bool mAllFated;

	private readonly float MIN_SIZE = 1.08f;
	private readonly float MAX_SIZE = 3.49f;

	void Start()
	{
		ShowFirst ();
	}
		
	public void ShowFirst()
	{
		GetChallengeRank data = FinalTrialMgr.GetInstance ()._FinalTrialData.m_GetChallengeRank;
		if (data == null)
		{
			FinalTrialMgr.GetInstance ().SendFirstRansMsg ();
			return;
		}

		MonsterData config = Core.Data.monManager.getMonsterByNum (data.firstRole.roleNum);
		if (config == null)
		{	
			RED.LogWarning("final trial manager monster can not find  " + config.ID);
			return;
		}

		for (int i = 0; i < m_txtName.Length; i++)
		{
			m_txtName[i].text = data.name;
		}
		ShowCharactor (data.firstRole.roleNum, (MonsterAttribute)data.firstRole.roleAttr, (data.firstRole.roleLots == 1));
	}


	public float GetModelHeight(){
		if (CRLuo != null)
		{
			Transform tTrans = CRLuo.gameObject.transform.FindChild("Bip001");
			return tTrans.localPosition.y;
		}
		return 0;
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
		
	public void ShowCharactor(int ID, MonsterAttribute attri = MonsterAttribute.DEFAULT_NO, bool allFated = false)
	{
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
		Object temp = task.Obj;

		if (Temp_OBJ != null) {
			Destroy(Temp_OBJ);
			Temp_OBJ = null;
		}

		if (temp != null) {
			Temp_OBJ = Instantiate(temp, transform.position, Quaternion.Euler(new Vector3(0,180,0))) as GameObject;
		} else {
			temp = PrefabLoader.loadFromUnPack("CRLuo/pbXXX", false, false);
			Temp_OBJ = Instantiate(temp, transform.position, Quaternion.Euler(new Vector3(0,180,0))) as GameObject;
		}

		Temp_OBJ.transform.parent = transform;

		SkinnedMeshRenderer mesh = Temp_OBJ.GetComponentInChildren<SkinnedMeshRenderer>();
		if (mesh != null)
		{
			if (mesh.bounds.extents.y > MAX_SIZE)
			{
//				RED.LogWarning ("大模型  " + mesh.bounds.extents.y);
				Temp_OBJ.transform.localScale = mesh.bounds.extents.y / MAX_SIZE * Vector3.one;
			}
			else if (mesh.bounds.extents.y < MIN_SIZE)
			{
//				RED.LogWarning ("小模型  " + mesh.bounds.extents.y);
				Temp_OBJ.transform.localScale = MIN_SIZE / mesh.bounds.extents.y * Vector3.one;
			}
			else
			{
//				RED.LogWarning ("正常模型  " + mesh.bounds.extents.y);
				Temp_OBJ.transform.localScale = Vector3.one;
			}
		}
		else
		{
			RED.LogWarning ("mesh is null ");
		}

		ShadowAnim shadow = Temp_OBJ.GetComponentInChildren<ShadowAnim>();
		if (shadow != null)
		{
			shadow.enabled = false;
			shadow.transform.localPosition = Vector3.zero;
		}

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
}
