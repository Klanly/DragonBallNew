using UnityEngine;
using System.Collections;

public class CRLuo_ShowANDelCharactor : MonoBehaviour {

	public int CharactorID;
	public float ShowTime;
	public float DeleteTime;
	
	public GameObject Default_Charactor;
	
	public GameObject goModel;
	public Camera Mycamear;
	public GameObject [] CardGounp;
	public float ShowCardTime;
	public ParticleSystem m_Smoke;
	public ParticleSystem m_Flash;
	public CRLuo_RedD_AlphaAnim m_RedD_AlphaAnim;
	public CRLuo_TimeCreate m_TimeCreate;

	public int StarNum = -1;

	private GameObject Man_GameObj;
	private GameObject Main_Egg;
	// Use this for initialization
	void Start () {
		Invoke ("CreateModel", ShowTime);
		Invoke ("DeleteModel", DeleteTime);
		Invoke ("CreateCard", ShowCardTime);
	}

	public void ReplayCRLuo()
	{
		if(goModel != null)Destroy(goModel);
		if(Default_Charactor != null)Destroy(goModel);
		if(Man_GameObj != null)Destroy(Man_GameObj);
		Object temp_L = PrefabLoader.loadFromPack("LS/pbLSEgg");
		if(temp_L != null)
		{
			Main_Egg = Instantiate(temp_L) as GameObject;
			RED.AddChild(Main_Egg, this.gameObject);
		}
		Object temp_B = PrefabLoader.loadFromPack("LS/Light_Cross_Boo_Egg");
		if(temp_L != null)
		{
			GameObject go = Instantiate(temp_B) as GameObject;
			go.transform.localPosition = new Vector3(0f,0.54f,0f);
			RED.AddChild(go, this.gameObject);
        }
        Start();
		m_RedD_AlphaAnim.ReplayAlphaAnim();
		m_TimeCreate.ReplayTimeCreate();
		m_Smoke.Stop();
		m_Smoke.Play();
		m_Flash.Stop();
		m_Flash.Play();
	}

	public void DeleteModel() {
		if(goModel != null) {
			goModel.layer = LayerMask.NameToLayer("Show");

			foreach(Transform child in goModel.transform) {
				if(child != null) {
					child.gameObject.layer = LayerMask.NameToLayer("Show");
				}
			}

		}
	}

	void CreateCard()
	{
		if (goModel != null) {
			int CardID;
			if(StarNum != -1)
			{
				CardID = StarNum;
			}
			else
			{
				CardID = goModel.GetComponent<CRLuo_PlayAnim_FX> ().StarLevel;
			}

			if(CardID>0)
			{
				CardID--;
			}
			else 
			{
				CardID = EggCardSingle.GetInstance().starnum - 1;
			}

				Man_GameObj = (GameObject)GameObject.Instantiate(CardGounp[CardID],this.gameObject.transform.position, this.gameObject.transform.rotation);
				Man_GameObj.transform.parent = this.gameObject.transform;
				
//			    Card3DBg _Script = CardGounp[CardID].GetComponent<Card3DBg>();
//				if(_Script != null)
//				{
//					_Script.SetMonStar(EggCardSingle.GetInstance().starnum);
//				}
//				Transform cardframe = gameObject.transform.FindChild("Plane01");
//			    Transform cardbg = gameObject.transform.FindChild("Plane02");
//			    Transform cardbtm = gameObject.transform.FindChild("Plane03");
//				if(cardframe != null && cardbg != null && cardbtm != null)
//				{
//					cardframe.gameObject.renderer.material.mainTexture = AtlasMgr.mInstance.Get3DCardFrameTexture (EggCardSingle.GetInstance().starnum);
//				    cardbg.gameObject.renderer.material.mainTexture = AtlasMgr.mInstance.Get3DCardBgTexture (EggCardSingle.GetInstance().starnum);
//				    cardbtm.gameObject.renderer.material.mainTexture = AtlasMgr.mInstance.Get3DCardBtmTexture (EggCardSingle.GetInstance().starnum);
//				}
			}
			ResetPos ();

		}
	public void CreateModel()
	{
		int id_L = CharactorID;

		string prefabName = "pb" + id_L;
		#if SPLIT_MODEL
		Object temp_L = ModelLoader.get3DModel(id_L);
		if(temp_L != null){
			AssetTask task = new AssetTask(prefabName, typeof(Object), null);
			task.AppendCommonParam(id_L, temp_L);
			FeatureLeft(task);
		}else{
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

	}

	void ResetPos(){
		if (CharactorID > 0) {
			Vector3 tPos = Mycamear.transform.localPosition;
			if (CharactorID == 10182 || CharactorID == 10187 || CharactorID == 10202||CharactorID == 10120 || CharactorID == 10115 || CharactorID == 10153) {
				Mycamear.transform.localPosition = new Vector3 (tPos.x,1.4f,tPos.z);
				Mycamear.fieldOfView = 34f;
			}
			if (CharactorID == 10202) {
				Mycamear.transform.localPosition = new Vector3 (tPos.x,1.4f,tPos.z);
				Mycamear.fieldOfView = 36f;
			}
			if (CharactorID == 10214) {
				Mycamear.transform.localPosition = new Vector3 (tPos.x,1.5f,tPos.z);
				Mycamear.fieldOfView = 50f;
			}
			if (CharactorID == 10141 || CharactorID==19985) {
				Mycamear.transform.localPosition = new Vector3 (tPos.x,1.4f,tPos.z);
				Mycamear.fieldOfView = 40;
			}


		}
	}

	private void FeatureLeft (AssetTask task) 
	{
		Object temp_L = task.Obj;
		
		if (goModel != null) {
			Destroy(goModel);
			goModel = null;
		}

		if (temp_L != null) {
			goModel = (GameObject)GameObject.Instantiate(temp_L, gameObject.transform.position , Quaternion.Euler(new Vector3(0, 0, 0)));
		} else {
			goModel = (GameObject)GameObject.Instantiate(Default_Charactor, gameObject.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
		}
	}


}
