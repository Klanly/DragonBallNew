using UnityEngine;
using System.Collections;

public class LoginInitOBB : MonoBehaviour {
	public UILabel lblContent;
	public UILabel lblBtn;
	public UILabel lblDesp;

	public UISlider slider;
	public UIButton BtnDownLoad;

	string expPath = null;
	string mainPath = null;

	private bool downloadStarted = false;
	private string uri;
	WWW www;


	void Start(){

		#if Google && UNITY_ANDROID &&   !UNITY_EDITOR
	
		if (!DownloadOBBController.RunningOnAndroid() || Core.Data.usrManager.isDownOBB ==1)
		{
			Destroy (gameObject);
			return;
		}else{
			gameObject.SetActive(true);
		}
		 expPath = DownloadOBBController.GetExpansionFilePath ();
		NGUIDebug.Log(" show  in  start  : " + expPath);
		if (expPath == null) {
			lblContent.text = "External storage is not available!";
			BtnDownLoad.isEnabled = false;
			return;
		} else {
			lblContent.text = "the path is available!! ";
			BtnDownLoad.isEnabled = true;
			StartCoroutine(loadLevel());
		}
		#elif UNITY_EDITOR
			Destroy(gameObject);
		#endif
	}

	#if Google && UNITY_ANDROID &&   !UNITY_EDITOR
	public void OnDownload(){
		StartCoroutine(loadLevel());
		NGUIDebug.Log(" click  btn ");	
	}



	public void FindOBBFilePath(){
		string mainPath = DownloadOBBController.GetMainOBBPath (expPath);
		string patchPath = DownloadOBBController.GetPatchOBBPath (expPath);

		NGUIDebug.Log( "Main = ..." + (mainPath == null ? " NOT AVAILABLE" : mainPath.Substring (expPath.Length)));
		NGUIDebug.Log( "Patch = ..." + (patchPath == null ? " NOT AVAILABLE" : patchPath.Substring (expPath.Length)));

		if (mainPath == null || patchPath == null)
			DownloadOBBController.FetchOBB ();
	}

	void Update (){
		if (www != null) {
			if (!www.isDone) {
				lblDesp.text = "Loading data ... " + (www.progress * 100).ToString ("f2") + "%";
				slider.value = www.progress;
			}
		}
	}
		
	IEnumerator loadLevel(){
		do{
			yield return new WaitForSeconds(0.5f);
			mainPath = DownloadOBBController.GetMainOBBPath(expPath); 
		}
		while(mainPath == null);


		NGUIDebug.Log(" load  level   find path 	  " + mainPath);
		lblDesp.gameObject.SetActive(true);
		slider.gameObject.SetActive(true);
		slider.value = 0;
		BtnDownLoad.gameObject.SetActive(false);

		if(downloadStarted == false){
			downloadStarted = true;
			uri = "file://" + mainPath;
			www = WWW.LoadFromCacheOrDownload(uri,0);
			NGUIDebug.Log(" load  level in  { }   " + www.error);
			yield return www;
			if(www.error == null){
				Application.LoadLevel (0);
				Core.Data.usrManager.isDownOBB =1;
			}
		}
	}

	#endif
}
