using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ALaLei : MonoBehaviour {

	public UISprite face;
	public GameObject all;
	public UISpriteAnimation mouthAnima;
	public UISprite hat;    // 40,76
	List<System.Action> ChangeAnima = new List<System.Action>();
	bool PlayChange = false;
	int ChangeIndex = 0;
	static ALaLei _Instance;
	public GameObject say;
	public UISprite Spr_talkback;
	public UILabel Lab_talk;
//	string talkContent="";
	void Start ()
	{
		 ChangeAnima.Add(Change0);
		 ChangeAnima.Add(Change1);
		 ChangeAnima.Add(Change2);
		 ChangeAnima.Add(Change3);
	}
	
	
	public static ALaLei  Instance
	{
		get
		{
			if(_Instance == null)
			{
				GameObject obj = PrefabLoader.loadFromPack("JC/ALaLei")as GameObject ;
				
				if(obj !=null)
				{
					_Instance = obj.GetComponent<ALaLei>();
					NGUITools.AddChild(DBUIController.mDBUIInstance._bottomRoot,  obj);
					//GameObject go = NGUITools.AddChild(GuideTask.ppp.gameObject,  obj);
					_Instance.all.transform.localPosition = new Vector3(-640f,-450f,0);
				}
			}
			else
			{
				Debug.Log(" RUN  Instance !!!!! ");
				_Instance.gameObject.SetActive(true);
			}
			return _Instance;
		}
	}
	
	
	
	public void Play (string text) 
	{
		SetTalk(text);
		all.GetComponent<TweenPosition>().enabled = true;
	}
//	
//	void Play2()
//	{
//		Debug.Log("----------Play------------");		
//		TweenPosition TP = all.GetComponent<TweenPosition>();//TweenPosition.Begin(all,0.1f,new Vector3(-504f,-234f,0));
//		TP.from = new Vector3(-640f,-450f,0);
//		TP.to = new Vector3(-504f,-234f,0);
//		TP.onFinished.Clear();
//		TP.onFinished.Add(new EventDelegate(this,"Finished"));
//		TP.Reset();
//		TP.PlayForward();
//		if(TP.enabled = false)TP.enabled = true;
//	}
	
	float timecount = 0f;
	void Update()
	{
		if(timecount<0.05f)
		{
			timecount+=Time.deltaTime;
			return;
		}
		else
		{
			timecount = 0;
		}
			
		
		if(PlayChange)
		{	
			System.Action action =ChangeAnima[ChangeIndex];
			if(action != null )action();
			
			ChangeIndex++;
			if(ChangeIndex>3)
			{
				PlayChange = false;
				ChangeIndex = 0;
			}
		}
		
		
	}
	
	void Finished()
	{
		PlayChange = true;
	}
		
	void Change0()
	{
		face.spriteName = "Symbol 5";
		face.MakePixelPerfect();
		face.transform.localPosition = new Vector3(8f,14f,0);
		hat.transform.localPosition = new Vector3(40f,86f,0);
		hat.transform.localRotation  =  Quaternion.Euler(0,0,2f);	
	}
	
	void Change1()
	{
		Vector3 temppos = all.transform.localPosition;
		temppos.x -= 5f;
		temppos.y -= 5f;
		all.transform.localPosition = temppos;
	}
	
	void Change2()
	{
		Vector3 temppos = all.transform.localPosition;
		temppos.x -= 5f;
		temppos.y -= 5f;
		all.transform.localPosition = temppos;
		face.spriteName = "Symbol 6";
		face.MakePixelPerfect();
		face.transform.localPosition = Vector3.zero;
		hat.transform.localPosition = new Vector3(37f,70f,0);
		hat.transform.localRotation  =  Quaternion.Euler(0,0,-5f);	
	}
	
	void Change3()
	{
		hat.transform.localPosition = new Vector3(40f,76f,0);
		hat.transform.localRotation  =  Quaternion.Euler(0,0,-5f);	
		mouthAnima.enabled = true;		
		//Invoke("Change4",2f);
		TweenScale TC=say.GetComponent<TweenScale>();
		TweenScale TS=say.GetComponent<TweenScale>();
		TC.enabled = true;
		TS.enabled = true;
		Invoke("Change3_2",1f);
	}
	/*对话框收回
	 * */
	void Change3_2()
	{
		TweenScale TC=say.GetComponent<TweenScale>();
		TweenScale TS=say.GetComponent<TweenScale>();
		TC.onFinished.Clear();
		TC.onFinished.Add(new EventDelegate(this,"ReverseScaleFinished"));		
		TC.PlayReverse();
		TS.PlayReverse();

	}
	
	void ReverseScaleFinished()
	{
		Invoke("Change4",0.2f);
		//Change4();
	}
	
	/*收回
	 * */
	void Change4()
	{
		mouthAnima.enabled=false;
		UISprite mouth = mouthAnima.GetComponent<UISprite>();
		mouth.spriteName = "Mouth 3";
		mouth.MakePixelPerfect();
		TweenPosition TP = all.GetComponent<TweenPosition>();//TweenPosition.Begin(all,0.1f,new Vector3(-640f,-450f,0));
		TP.onFinished.Clear();
		TP.onFinished.Add(new EventDelegate(this,"Change5"));
		TP.PlayReverse();
	}
	
	void Change5()
	{
		Destroy(gameObject);
		_Instance = null;
	}
	
	void SetTalk(string text)
	{
		int length = text.Length;
		 Spr_talkback.width = 55+(length-1)*Lab_talk.fontSize;
		Lab_talk.text = text;
	}
	
}
