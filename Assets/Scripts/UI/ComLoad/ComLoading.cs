using UnityEngine;
using System.Collections;
/// <summary>
/// 用法说明，直接调用 ComLoading.Open();显示load 画面。ComLoading.Close();-结束loading
/// </summary>
public class ComLoading : MonoBehaviour 
{
	public float _delaytime = 0.5f;
	public UILabel _txtDesp;
	public UISprite LoadBack ;
	public GameObject Content;

	private float maskAlpha = 0.5f;

	private static ComLoading _instance;
	public static ComLoading mInstance
	{
		get 
		{
			return _instance;
		}
	}

	public static void Open(string strText = "",float maskAlpha = 0.5f)
	{
		if (_instance == null)
		{
			Object pro = PrefabLoader.loadFromPack ("Comload/pbComLoading");
			GameObject prefab = GameObject.Instantiate (pro) as GameObject;

			GameObject parent = null;
			if(DBUIController.mDBUIInstance != null) {
				parent = DBUIController.mDBUIInstance._TopRoot;
			} else {
				Behaviour uic = UICamera.currentCamera;
				if(uic == null) { //空的情况下，可以再次去战斗场景里面去相机
					uic = BattleCamera.Instance.Camera1;
				}

				if(uic == null) return;
				parent = uic.gameObject;
			}

			RED.AddChild (prefab, parent);
            if(Core.SM.CurScenesName == SceneName.GAME_BATTLE)
                prefab.layer = LayerMask.NameToLayer ("UI");
            else
    			prefab.layer = LayerMask.NameToLayer ("UITop");
			prefab.name = "ComloadingUI";

			_instance = prefab.GetComponent<ComLoading>();

		} 
		else 
		{
			RED.SetActive (true, _instance.gameObject);
		}
        if(_instance != null)
		{
			if(strText =="") strText = Core.Data.stringManager.getString(9125);
        	_instance._txtDesp.text = strText;
			_instance.Init();
			_instance.maskAlpha = maskAlpha;
		}
	}
	
	private void Init()
	{
		if(_instance!=null && _instance.gameObject.activeSelf)
		{
		    Invoke("ShowContent",0.3f);
		}
	}
	
	private void ShowContent()
	{
		if(_instance!=null && _instance.gameObject.activeSelf)
		{
			LoadBack.color = new Color(1f,1f,1f,this.maskAlpha);
			Content.SetActive(true);
		}
	}
	
	public static void Close()
	{
		if (_instance != null) 
		{
			RED.SetActive (false, _instance.gameObject);
			_instance._txtDesp.text = "";
			_instance.LoadBack.color = new Color(1f,1f,1f,0);
			_instance.Content.SetActive(false);
			_instance.CancelInvoke("ShowContent");
		}
	}
}
