using UnityEngine;
using System.Collections;

public class UIMailAndBattleMain : MonoBehaviour 
{

	public UIMailAndBattleInfo _UIMailAndBattleInfo;


	public GameObject PanelType1;
	public GameObject root1;
	public GameObject mscrollview1;
	
	public UISprite mSprite;
	public UISprite[] mBg;
	public UIButton[] mBtn;
	public UISprite[] mWarning;
	public UIScrollView mUIScrollView1;
	public UIGrid mUIGrid1;
	
	private static UIMailAndBattleMain _instance;
	public static UIMailAndBattleMain Instance
	{
		get{return _instance;}
	}
	
	void Awake(){
		_instance = this;
	}
	
	void Start()
	{
		DBUIController.mDBUIInstance.HiddenFor3D_UI();
		
	}

	public static void OpenUI()
	{
		UnityEngine.Object obj = PrefabLoader.loadFromPack("LS/pbLSMailAndBattleRoot");
		if(obj != null)
		{
			GameObject go = RUIMonoBehaviour.Instantiate(obj) as GameObject;
			_instance = go.GetComponent<UIMailAndBattleMain>();
			RED.AddChild(go.gameObject,DBUIController.mDBUIInstance._bottomRoot);
		}
	}

	
}
