using UnityEngine;
using System.Collections;

public class SQYChapterController : MDBaseViewController {


	
	public static System.Action OnExitBackCall;
	private static SQYChapterController _instance;
	public static SQYChapterController Instance
	{
			get{return _instance;}
	}
	
	void OnEnable()
	{
		 UIMiniPlayerController.Instance.freshPlayerInfoView ();
		DBUIController.mDBUIInstance.RefreshUserInfo ();
	}
	
	
	void Start()
	{
		
	}
	
	public UIScrollView _view;
	
	
	void Awake(){
			_instance = this;
	}
	
	public void OnBtnBack()
	{
		gameObject.SetActive (false);
        DBUIController.mDBUIInstance.ShowFor2D_UI (false);
		if(OnExitBackCall != null)
		{
			if(!Core.Data.guideManger.isGuiding)
			OnExitBackCall();
			OnExitBackCall = null;
		}
	}

	void OnBtnAddCoin()
	{
	}
//    void OnEnable(){
//        Debug.Log("sqy chapter  enable");
//        UIMiniPlayerController.Instance.SetActive (true);
//    }

	void OnBtnAddJewel()
	{
	}

//	public void OnBtnChapter()
//	{
//		DBUIController.mDBUIInstance.ViewToCityFloor();
//	}
	
	public static SQYChapterController CreateChapterView()
	{
		UnityEngine.Object obj = PrefabLoader.loadFromPack("SQY/pbSQYChapterController");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			SQYChapterController cc = go.GetComponent<SQYChapterController>();
			return cc;
		}
		return null;
	}
	
	float[] silderPos = new float[]{0,0.372f,0.745f,1f};
	//显示到第N章
	public void SetChapterAtIndex(int index = -1)
	{
		//if(index< 0) index = Core.Data.dungeonsManager.c
		
		int xiabiao=(int)index/4;
		if(xiabiao>=0 && xiabiao < silderPos.Length)
			StartCoroutine(SetChapterAt(xiabiao));
			
		
		
		
	}
	
	IEnumerator SetChapterAt(int xiabiao)
	{
		yield return new WaitForEndOfFrame();
		Debug.Log("_view.horizontalScrollBar="+_view.horizontalScrollBar.value);
		_view.horizontalScrollBar.value = silderPos[xiabiao];
	}
	
}
