using UnityEngine;
using System.Collections;

public class BuildScene : MonoBehaviour 
{
	private static BuildScene m_Instance;
	public static BuildScene mInstance
	{
		get
		{
			return m_Instance;
		}
	}

	public const int BUILD_LAYER = 10;
	//场景主相机
	public Camera m_camera;

	//太阳移动
	public CRLuo_MiniItweenFromTo sunCamera
	{
		get
		{
			if (_sunCamera == null)
			{
				if(m_camera != null)
				_sunCamera = m_camera.GetComponent<CRLuo_MiniItweenFromTo>();
			}
			return _sunCamera;
		}
	}

	private CRLuo_MiniItweenFromTo _sunCamera;
	
	public bool ZhaoMuUnlock
	{
		get
		{
			return Core.Data.newDungeonsManager.lastFloorId >= 60104;
		}
	}


	//所有建筑
	public BuildItem[] m_arryBuilds;

	//天下第一武道会
	public WudaohuiStage m_wudaohui;

    //特殊镜头
    public GameObject SpecailPointView;
    //特殊镜头挂载点
    public GameObject SpecailPoint;


	private GameObject m_QShop;

	private Object m_preUnCreate;
	public Object prefabUnCreate
	{
		get
		{
			if(m_preUnCreate == null)
			{
				m_preUnCreate = PrefabLoader.loadFromPack("");
				if(m_preUnCreate == null)
				{
					RED.LogError ("uncreated build prefab  is null");
				}
			}
			return m_preUnCreate;
		}
	}

	private Vector3 startPos;
	private Vector3 endPos;
	private GameObject m_selObj;

	void Awake()
	{
		m_Instance = this;
	}

    void Start()    
	{
		JC_TouchRoll.TouchState = TouchCtrl;
	}
		
	public void UpdateBuildById(int buildID)
	{
		for (int i = 0; i < m_arryBuilds.Length; i++)
		{
			if (buildID == m_arryBuilds [i].buildID)
			{
				m_arryBuilds [i].UpdateBuildState ();
				return;
			}
		}
	}
	
	public void UpdateBuildByNum(int num)
	{
		for (int i = 0; i < m_arryBuilds.Length; i++)
		{
			if (num == m_arryBuilds [i].buildNum)
			{
				m_arryBuilds [i].UpdateBuildState ();
				return;
			}
		}
	}

    public void UpdateAllBuilds()
	{
		for (int i = 0; i < m_arryBuilds.Length; i++)
		{
			m_arryBuilds [i].UpdateBuildState ();
		}
	}

	public void CheckAllBuildAnim()
	{
		for (int i = 0; i < m_arryBuilds.Length; i++)
		{
			m_arryBuilds [i].CheckAnimPlaying ();
		}
	}

	public void GetProductionSuc(int buildID)
	{
		for (int i = 0; i < m_arryBuilds.Length; i++)
		{
			if (buildID == m_arryBuilds [i].buildID)
			{
				m_arryBuilds [i].GetProductionSuc();
				return;
			}
		}
	}

	public void BuildUpgradeSuc(int buildId)
	{
		for (int i = 0; i < m_arryBuilds.Length; i++)
		{
			if (buildId == m_arryBuilds [i].buildID)
			{
				m_arryBuilds [i].BuildUpgradeSuc();
				return;
			}
		}
	}

	public void BuildOpenUpdate(int buildId){
		for (int i = 0; i < m_arryBuilds.Length; i++)
		{
			if (buildId == m_arryBuilds [i].buildID)
			{
				m_arryBuilds [i].UpdateOpenProduction();
				return;
			}
		}
	}

	public void SetShow(bool bShow)
	{
		RED.SetActive(bShow, this.gameObject);
		//检测一下 
		if (bShow == true) {
			CheckFragBuilding ();
		}
	}

	public void CheckFragBuilding(){
		if (m_arryBuilds.Length >= 11) {
			bool isShark = Core.Data.soulManager.IsHaveFullFrag ();
			if (m_arryBuilds [11] != null) {
				if (m_arryBuilds [11].m_Lighting == null) {
					m_arryBuilds [11].CheckLighting ();
				} 
				if (m_arryBuilds [11].m_Lighting != null) {
					m_arryBuilds [11].m_Lighting.SetActive (isShark);
				}
			}
		}
	}


	//更新武道会
	public void UpdateWudaohui()
	{
		m_wudaohui.ShowFirst ();
	}

	void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			m_selObj = null;
			RaycastHit hit;
			if(UICamera.Raycast(Input.mousePosition))
				return;

			startPos = Input.mousePosition;


			Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out hit))
			{
				if(hit.collider.gameObject.layer == BUILD_LAYER)
				{
					hit.collider.gameObject.SendMessage("OnTouchDown");
					m_selObj = hit.collider.gameObject;
				}
			}
		}
		else if(Input.GetMouseButton(0))
		{
			RaycastHit hit;
			if(UICamera.Raycast(Input.mousePosition))
				return;
		}
		else if(Input.GetMouseButtonUp(0))
		{
			RaycastHit hit;
			if(UICamera.Raycast(Input.mousePosition))
				return;

			Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out hit))
			{
				if(hit.collider.gameObject.layer == BUILD_LAYER && hit.collider.gameObject == m_selObj && m_selObj != null)
				{
					hit.collider.gameObject.SendMessage("OnTouchUp");
				}
			}
			else if(m_selObj != null && m_selObj.layer == BUILD_LAYER)
			{
				m_selObj.collider.gameObject.SendMessage("OnTouchUp");
			}
			
			if( !IsDraging() )
			{
				ray = m_camera.ScreenPointToRay(Input.mousePosition);
				if(Physics.Raycast(ray, out hit))
				{
					if(hit.collider.gameObject.layer == BUILD_LAYER)
					{
						hit.collider.gameObject.SendMessage("OnClick");
					}
				}
			}
		}
	}

	private bool IsDraging()
	{
		endPos = Input.mousePosition;
	
		float disX = Mathf.Abs(startPos.x - endPos.x);
		float disY = Mathf.Abs(startPos.y - endPos.y);

//		float time = Screen.width / 1136;
//
//		disX /= time;
//		disY /= time;

		float delta = 0;
		#if UNITY_EDITOR
		delta = 15;
		#else
		delta = 100;
		#endif

		if(disX > delta  || disY > delta)
		{
//			RED.LogWarning("disX  ::  " + disX);
//			RED.LogWarning("disY  ::  " + disY);
//			RED.LogWarning("scale ::  " + time);
//			RED.LogWarning("delata :: " + delta);
//			RED.LogWarning("startX :: " + startPos.x);
//			RED.LogWarning("endPos :: " + endPos.x);

			return true;
		}

		return false;
	}

	private void TouchCtrl(AllenTouchState state)
	{
		if(state == AllenTouchState.Continue && JC_TouchRoll.isMoving)
		{
			DBUIController.mDBUIInstance.WillHideMainUI();
		}
		else if(state == AllenTouchState.Up)
		{
			DBUIController.mDBUIInstance.WillShowMainUI();
		}
	}

	public BuildItem GetBuildItemByNum(int num)
	{
		for (int i = 0; i < m_arryBuilds.Length; i++)
		{
			if (num == m_arryBuilds [i].buildNum)
			{
				return m_arryBuilds [i];
			}
		}
		return null;
	}

    #region 特殊的镜头展示

    public void showSpecialViewPoint() {
        GameObject pointV = Instantiate(SpecailPointView) as GameObject;
        RED.AddChild(pointV, SpecailPoint);
    }

    #endregion

	void UpdateWudaohui(string strName, int pNum)
	{

	}


//	void OnGUI(){
//		if (GUI.Button(new Rect(100, 100, 100, 100), " text "))
//		{
//			m_arryBuilds [8].ShowEffect ();
//			//showMoneyTip.InitInfo (tInt);
//		}
//	}
}
