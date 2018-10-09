using UnityEngine;
using System.Collections;

public class UITaoBaoController : RUIMonoBehaviour {

	private static UITaoBaoController instance;
	public static UITaoBaoController Instance
	{
		get
		{
			return instance;
		}
	}
	public GameObject winObj;
	public UIInput input_Box;

	public static UITaoBaoController CreateTaoBaoPanel (ActivityItemType type, GameObject tObj)
	{
		UnityEngine.Object obj = WXLLoadPrefab.GetPrefab (WXLPrefabsName.UITaoBaoPanel);
		if (obj != null) {
			GameObject go = Instantiate (obj) as GameObject;
			UITaoBaoController fc = go.GetComponent<UITaoBaoController> ();
			Transform goTrans = go.transform;
			go.transform.parent = tObj.transform;
			go.transform.localPosition = Vector3.zero;
			goTrans.localScale = Vector3.one;

            RED.TweenShowDialog(go);
			return fc;
		}
		return null;		
	}

	void Awake(){
        instance = this;
		winObj.SetActive (false);

	}


	public void OnEnterTaoBao(){
        //		WWW www = new WWW ("http:/www.taobao.com");
		Application.OpenURL ("http:/www.taobao.com");
	}



	public void OnYesBtn(){
		if( string.IsNullOrEmpty(input_Box .value) == false)
			ActivityNetController.GetInstance ().TaobaoRequest (input_Box.value);
	}

	public void OnNoBtn(){
		winObj.SetActive (false);
	}

	public void OnGetCDKey(){
		winObj.SetActive (true);
		//ActivityNetController.GetInstance ().TaobaoRequest ("11111111111111111c");
	
	}


	public void OnBack(){
        Destroy(gameObject);
        //gameObject.SetActive (false);
	}
}
