using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GameMovie : MonoBehaviour 
{
    public GameObject PianTou;
	public TweenAlpha mask;
	public UILabel Label ;
	string LabContent ;
	int index=0;
	public TweenAlpha labAlpha;
	private GameCG CG;
	private GameObject CG3D;
	public UILabel Lab_Subtitles;
	void Start () 
	{	
		LabContent = Core.Data.stringManager.getString(9023);
#if CG2D
		GameObject CG2D = Instantiate( PrefabLoader.loadFromPack("JC/CG2D") ) as GameObject;
		RED.AddChild(CG2D,gameObject);
		CG = CG2D.GetComponent<GameCG>();
		CG.Play().OnFinished = () =>
		{	 
		   Play3DAnimation();
		};
#else
		Lab_Subtitles.text = "";
		CG3D = Instantiate( PrefabLoader.loadFromPack("JC/CG3D") ) as GameObject;
		RED.AddChild(CG3D,null);
		InitCG3DSubtitles();
		Invoke("PianTou0Finished",53f);
#endif		

		
		
	}
	
	void PianTou0Finished()
	{
		Core.Data.soundManager.BGMPlay(SceneBGM.BGM_GAMEUI);
		//Core.Data.soundManager.BGMStop();
		
		CG3D.SetActive(false);
		Play3DAnimation();
	}
	
	
	void Play3DAnimation()
	{
		PianTou.SetActive(true);
		Invoke("HideMask",17f);
	}
	
	void HideMask()
	{
		mask.enabled = true;
		Invoke("PlayLabAmimation",3.5f);
	}
	
	void GoToGameUIScene()
	{
		Core.SM.beforeLoadLevel(SceneName.GameMovie, SceneName.MAINUI);
		Application.LoadLevel(SceneName.MAINUI);
	}
	
	void PlayLabAmimation()
	{
		Core.Data.soundManager.BGMPlay(SceneBGM.BGM_14YEAR);
		InvokeRepeating("LabAmimation",0.3f,0.2f);
	}
	
	void LabAmimation()
	{
		if(index < LabContent.Length)
		{
		   Label.text = LabContent.Substring(0,index);
		   index++;
		}
		else
		{
		   Label.text = LabContent;
		   CancelInvoke("LabAmimation");
		   labAlpha.enabled = true;
			Core.Data.soundManager.BGMStop();
		   Invoke("GoToGameUIScene",2f);
		}
	}
	
	
	//------------------------------字幕---------------------------------
	public class SubtitlesData
	{
		public string str;
		public float startTime;
		public float continueTime;
		public SubtitlesData(){}
		public SubtitlesData(string _str,float _startTime,float _continueTime)
		{
			str = _str;
			startTime = _startTime;
			continueTime = _continueTime;
		}
	}
	
	public List<SubtitlesData> list_SubtitlesData = new List<SubtitlesData>();

	//运行3DCG的字幕
	IEnumerator RunCG3DSubtitles(SubtitlesData data)
	{
		yield return new WaitForSeconds( data.startTime );
		Lab_Subtitles.text = data.str;
		yield return new WaitForSeconds( data.continueTime );
		Lab_Subtitles.text = "";
	}
	
	//初始化3D字幕
	void InitCG3DSubtitles()
	{
		StringManager	 sm = Core.Data.stringManager;
		if(sm != null)
		{
			list_SubtitlesData.Add(new SubtitlesData(sm.getString(9024),0,10.4f ));
			list_SubtitlesData.Add(new SubtitlesData(sm.getString(9025),11.3f,5.6f ));
			list_SubtitlesData.Add(new SubtitlesData(sm.getString(9032),17f,1.7f ));
			list_SubtitlesData.Add(new SubtitlesData(sm.getString(9033),19f,0.64f ));
			list_SubtitlesData.Add(new SubtitlesData(sm.getString(9034),20f,4.3f ));
			list_SubtitlesData.Add(new SubtitlesData(sm.getString(9035),25f,2f ));
			list_SubtitlesData.Add(new SubtitlesData(sm.getString(9036),27.5f,1.7f ));
			
			list_SubtitlesData.Add(new SubtitlesData(sm.getString(9037),32f,5f ));
			list_SubtitlesData.Add(new SubtitlesData(sm.getString(9038),38f,0.6f ));
			list_SubtitlesData.Add(new SubtitlesData(sm.getString(9039),39f,0.8f ));
			list_SubtitlesData.Add(new SubtitlesData(sm.getString(9026),40f,10.2f ));
			
			StartCoroutine( PlaySoundEffect(SoundFx.FX_prolog7,40f) );
				
			foreach(SubtitlesData data in list_SubtitlesData)
			StartCoroutine(RunCG3DSubtitles(data));
		}
	}
	
	
	IEnumerator PlaySoundEffect(SoundFx fx,float time)
	{
		yield return new WaitForSeconds(time);
		Core.Data.soundManager.SoundFxPlay(fx);
	}	
}
