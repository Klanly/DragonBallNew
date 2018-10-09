using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class UIGuide : MonoBehaviour {
	
	public static UIGuide _Instance;
    public enum ClickType
	{
		AnyArea = 0,
		SpecifiedArea =1,
	}
	ClickType curClickType;
	
	public GameObject Mask;
	public BuErMaAnimation bRMAnima;
	public BuErMaAnimation xiaoWukongAnima;
	public GuideHand hand;
	public GuideMask uiMaskScale = null;

	//布尔玛1的对话框位置，因为战斗的对决在正中心，所以必须挪开
	public GameObject go_BuMa1_Dia;
	public GameObject go_BuMa1;

	//布尔玛4
	public GameObject go_BuMa4;
	public GameObject go_BuMa4_Dia;


	UIRoot root = null;
	
	public List<GameObject> LIST_ROLE = new List<GameObject>();
	public List<GameObject> LIST_MASK = new List<GameObject>();
	public List<UILabel> LIST_Dialogue = new List<UILabel>();
	
	public List<GameObject> Guide2Layers;
	
	public UICamera GuideCamera;
	public BoxCollider AnyAreaClickCollider;
	public BoxCollider SpecifiedAreaClickCollider;

	//引导点击事件
//	public bool GuideTouchEnable
//	{
//		set
//		{
//			AnyAreaClickCollider.enabled = SpecifiedAreaClickCollider.enabled = value;
//		}
//		get
//		{
//			return AnyAreaClickCollider.enabled;
//		}
//	}
		
	void Start () 
	{
		Core.Data.temper.SetGameTouch(false);
		
	   //屏幕适应
		root=transform.parent .GetComponent<UIRoot>();
		if(root != null)
			root.manualHeight = (int) (1136f/(640f/(float)Screen.height*(float)Screen.width)*640f);
		
		
		foreach(GameObject g in Guide2Layers)
		{
			g.layer = LayerMask.NameToLayer("Guide2");
		}
	}
	
	
	public static UIGuide Instance
	{
		get
		{
			if(_Instance == null)
			{
				Object prefab = PrefabLoader.loadFromPack("JC/UI Root_guide");
				if(prefab != null)
				{
					GameObject obj = Instantiate(prefab) as GameObject;		
					
					if(obj != null)
					{
						obj.transform.localPosition = Vector3.zero;
						obj.transform.localScale = Vector3.one;
						
						_Instance = obj.GetComponentInChildren<UIGuide>();
						//obj.transform.parent = DBUIController.mDBUIInstance._bottomRoot.transform.parent;
						//Debug.Log(DBUIController.mDBUIInstance.transform.name);
						_Instance.Invoke("ShowCamera",0.2f);
						DontDestroyOnLoad(_Instance.transform.parent.gameObject);
					}
				}
			}
			else
			{

			}
			return _Instance;
		}
	}
	
	//延迟显示摄像机,是为了在做适配完以后才显示
	void ShowCamera()
	{
		_Instance.GuideCamera.gameObject.SetActive(true);
		_Instance.AnyAreaClickCollider.enabled = true;
	}

	void AnyAreaClick(GameObject btn)
	{	
		if( !Core.Data.guideManger.CanClickGuideUI ) return;
		//Debug.LogError("AnyAreaClick");
		if(curClickType == ClickType.AnyArea)
		{
			GuideData task=Core.Data.guideManger.CurTask;
			if(task != null && task.ID > 0)
			{	
				//GuideTouchEnable = false;
			    EventSender.SendEvent( (EventTypeDefine)task.TaskID ,task);				
				Core.Data.guideManger.AutoSendMsgAtLastTask();
			}
			if(task.AutoNext==0)
			    Core.Data.guideManger.AutoRUN();
			else
			{
				//如果是缘引导触发
				if(task!=null && task.ID == -1)
				{
					//GuideTouchEnable = false;
					EventSender.SendEvent( (EventTypeDefine)task.TaskID ,task);
					DestoryGuide();
				}
			}
		}		
	}
	
	void SpecifiedAreaClick(GameObject btn)
	{
		if( !Core.Data.guideManger.CanClickGuideUI ) return;
		//Debug.LogError("SpecifiedAreaClick");
		if(curClickType == ClickType.SpecifiedArea)
		{
			GuideData task=Core.Data.guideManger.CurTask;	
			if(task != null && task.ID > 0)
			{

			    task.MultiIndex = btn.name == "GuideMask" ? 0:System.Convert.ToInt32(btn.name);			
				//GuideTouchEnable = false;
			    EventSender.SendEvent( (EventTypeDefine)task.TaskID ,task);	
				
				 Core.Data.guideManger.AutoSendMsgAtLastTask();
			}
			if(task != null && task.AutoNext==0)
			{
//				if(Core.Data.guideManger.LastTaskID == 300017)
//					Invoke("AfterAutoRun",0.5f);
//				else
 			     Core.Data.guideManger.AutoRUN();
			}
			
		}
		else
		{
			AnyAreaClick(btn);
		}
	}
	
	
		
	
	
	void AfterAutoRun()
	{
		Core.Data.guideManger.AutoRUN();
	}
	
	
	//设置遮罩类型
	void SetMaskType(int id)
	{
		for(int i =0 ;i<LIST_MASK.Count ;i++)
		{
			if(i == id)
			{
				if(!LIST_MASK[i].activeSelf) LIST_MASK[i].SetActive(true);
			}
			else
			{
				if(LIST_MASK[i].activeSelf) LIST_MASK[i].SetActive(false);
			}
			
		}
	}
	
	//设置说话人物类型
	void SetRoleType(GuideData data)
	{
		int id = data.RoleID;
		if(data.Dialogue == "null") id = -1;
			
		for(int i =0 ; i<LIST_ROLE.Count; i++)
		{
			if(i == id )
			{
				if(!LIST_ROLE[i].activeSelf) LIST_ROLE[i].SetActive(true);
				LIST_Dialogue[i].text = "　　" + data.Dialogue;
			}
			else
			{
				if(LIST_ROLE[i].activeSelf) LIST_ROLE[i].SetActive(false);
			}			
		}
	}
	
	
	public void SetUI(GuideData data)
	{
		Core.Data.guideManger.CanClickGuideUI = true;
		//GuideTouchEnable = true;
		ActivityNetController.GetInstance().SendCurrentUserState(data.ID);
		/*圆孔遮罩*/
		if(!transform.parent.gameObject.activeSelf)
		{
			transform.parent.gameObject.SetActive(true);
		}
		
		SetMaskType(data.MaskType);
		
		SetRoleType(data);
		
		if(data.Sound > 0) {
			if(data.ID == 100001)
				StartCoroutine( PlaySound(data.Sound,1) ); 
			else
                StartCoroutine( PlaySound(data.Sound) ); 
		}
		
		
		uiMaskScale.SetScale(data.MaskType,data.RenderMask,data.ZoomX,data.ZoomY,data.Multi);
		curClickType = (ClickType)data.Operation;
		///
		///  用于适配战斗的UI
		/// 
		float MaskY = data.MaskY;
		Vector3 ArrowPos = Vector3.zero;
		if(data.ArrowDir == 4)
		{ 
			ArrowPos = new Vector3(data.MaskX,data.MaskY,0);
			data.MaskX = 0;
			data.MaskY = -1000; 
		}
		
		Mask.transform.localPosition = new Vector3(data.MaskX,data.MaskY,0);

		if(data.Dialogue == "null" && MaskY > -600 && data.Multi != null && data.Multi.Count == 0)
		{
			if(data.ArrowDir != 4)
		       hand.SetDir(data.ArrowDir,hand.transform.localPosition,Mask.transform.localPosition);
			else
			   hand.SetDir(data.ArrowDir,hand.transform.localPosition,ArrowPos);
		}
		else 
		{
				hand.gameObject.SetActive(false);
		}
		
		//自动适配全屏
		AutoFixAllSceen(data);

	}



	//矩形适用
	void autoFitScreenRect(GuideData data) {
		if(data.MaskY > 0)
			Mask.transform.localPosition = new Vector3(data.MaskX,data.MaskY + (root.manualHeight -640f) * 0.5f,0f);
		else
			Mask.transform.localPosition = new Vector3(data.MaskX,data.MaskY - (root.manualHeight -640f) * 0.5f,0f);
	}

	//矩形适用
	//4号布尔玛和对话框的出现位置
	void autoFitScreenRect1() {
		//布尔玛4的位置
		float X = go_BuMa4.transform.localPosition.x;
		float Y = 32F;
		go_BuMa4.transform.localPosition = new Vector3(X, Y - (root.manualHeight - 640f) * 0.5f,0f);

		//对话框的位置
		X = go_BuMa4_Dia.transform.localPosition.x;
		Y = 192F;
		go_BuMa4_Dia.transform.localPosition = new Vector3(X, Y - (root.manualHeight - 640f) * 0.5f,0f);
	}

	//1号布尔玛的出现位置,适应屏幕的大小
	void BuErMaFitScreen() {
		if(Core.SM.CurScenesName == SceneName.GAME_BATTLE) {
			go_BuMa1.transform.localPosition = new Vector3(0f, 33f, 0f);

			float X = go_BuMa1.transform.localPosition.x;
			float Y = go_BuMa1.transform.localPosition.y;
		if(go_BuMa1 != null && root != null)
			go_BuMa1.transform.localPosition = new Vector3(X, Y - (root.manualHeight - 640f) * 0.5f,0f);
		}
	}
	
	//防止播放声音回调错乱
	int PlaysondCount = 0;
	IEnumerator PlaySound(int sound,int time =0)
	{
		yield return new WaitForSeconds(time);		
		Core.Data.soundManager.GuideFxPlay( (SoundFx)sound, SoundFxPlayFinished);
		PlaysondCount++;
		if(bRMAnima.gameObject.activeSelf)
		    bRMAnima.PlayMouth(true);
		if(xiaoWukongAnima.gameObject.activeSelf)
	    	xiaoWukongAnima.PlayMouth(true);
	}
	
	void SoundFxPlayFinished()
	{
		PlaysondCount--;
		if(_Instance != null && PlaysondCount == 0)
		{
			if(bRMAnima != null)
			{
				if(bRMAnima.gameObject.activeSelf)
				    bRMAnima.PlayMouth(false);
				if(xiaoWukongAnima.gameObject.activeSelf)
				    xiaoWukongAnima.PlayMouth(false);
			}
		}
	}
	
	public void ShowGuide()
	{
		if(!transform.parent.gameObject.activeSelf)
			transform.parent.gameObject.SetActive(true);
	}
	
	
	void OnEnable()
	{
		Core.Data.temper.SetGameTouch(false);
	}
	
	public void HideGuide()
	{
		Core.Data.temper.SetGameTouch(false);
		if(_Instance != null && transform!=null && transform.parent.gameObject.activeSelf)
			transform.parent.gameObject.SetActive(false);
	}
	
	//完成遮蔽
	public void CompleteShelter()
	{
		Mask.transform.localPosition = new Vector3(0,-1000f,0);
	}
	
	
	public void DestoryGuide()
	{
		CancelInvoke("RunNextGuide");
		Core.Data.temper.SetGameTouch(true);
		if(transform != null && transform.parent != null)
		 Destroy(transform.parent.gameObject);
	     _Instance = null;	
	}


	public static void SafeDestroy()
	{
		Core.Data.temper.SetGameTouch(true);
		if(_Instance!= null)
			_Instance.DestoryGuide();
	}

	
	//延迟x秒后执行下一个引导
	bool isPause = false;
	System.Action _action = null;
	public void DelayAutoRun(float second,bool pause = false, System.Action callback = null)
	{
		//Debug.Log(Core.Data.guideManger.LastTaskID.ToString());
		isPause = pause;
		_action = callback;
		if(this != null)
		Invoke("RunNextGuide",second);
	}
	
	void RunNextGuide()
	{
		Debug.Log(Core.Data.guideManger.LastTaskID.ToString());
		Core.Data.guideManger.AutoRUN();
		float timescale = isPause ? 0f : 1f;
		if(Time.timeScale != timescale ) Time.timeScale = timescale;
		if(_action != null ) _action();
	}
	
	void OnDestroy()
	{
		CancelInvoke("RunNextGuide");
	}
	
    //自动适配全屏
	void AutoFixAllSceen(GuideData data)
	{
		float y = 0;
		if(data.AdapterToSceen > 0)
		{
			
			
			float h = 1136f /(float) Screen.width * (float) Screen.height;
			y =  - Mathf.Abs((640f - h))/2;
			hand.gameObject.SetActive(true);
			
			//Mask.transform.localPosition = new Vector3(data.MaskX, data.MaskY * root.manualHeight / 640f  /*+ Mathf.Abs(y)*/, 0f);
			hand.SetDir(3,hand.transform.localPosition,Mask.transform.localPosition);
			hand.selfMove.enabled = true;
			
			
			
		}
		this.transform.localPosition = new Vector3(0,y,0);
	}
	



}
