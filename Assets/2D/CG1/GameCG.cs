using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GameCG : MonoBehaviour {
   
	public System.Action OnFinished;
	
	public List<GameObject> Pages = new List<GameObject>();
	public UISprite Background;
	#region Page1
	public TweenAlpha CGMask;
	public TweenScale Page1_Back2;
	string Str_Page1;
	
	public UILabel Lab_Page1;
	#endregion
	
	#region Page2
	public TweenPosition Page2_Back;
	public UILabel Lab_Page2;
	#endregion
	
	#region Page3
	public TweenPosition Page3_Left;
	public TweenPosition Page3_Right;
	public TweenAlpha Page3_BLeft;
	public GameObject Page3_LeftBall;
	public TweenAlpha Page3_BRight;
	public GameObject Page3_Skill;
	#endregion
	int index = 0;
	float CGMaskSpeed = 1.5f;
	
	#region Page4
	public UISprite Page4_TLeft;
	public TweenAlpha BTeft;
	
	public GameObject Go;
	
	public List<UISprite> Hong = new List<UISprite>();
	public TweenPosition Page4_TRight1;
	public TweenPosition Page4_TRight2;
	public GameObject Page4_TRight3;
	public GameObject Page4_Last;
	public GameObject Page4_Last2;
	public List<UISprite> HaHa = new List<UISprite>();
	#endregion
	
	#region Page5
	public UISprite Page5_Left;
	public TweenPosition Page5_fly;
	public GameObject Page5_Right1;
	public GameObject Page5_Right2;
	public GameObject Page5_Right3;
	public GameObject Page5_Ball;
	#endregion
	
	#region Page6
	public UISprite Page6_star;
	public UISprite Page6_fly;
	public UISprite Page6_XingQiu;
	public UILabel  Page6_label;
	#endregion
	
	#region Page7
	public UISprite Page7_fly;
	public UISprite Page7_bomb;
	public UISprite Page7_back;
	public UISprite Page7_gangdang;
	#endregion

	void OnGUI()
	{
		if(GUI.Button(new Rect(400,130,100,50),"PAGE7"))
		{
			Play();
		}
	}
	
	
	void Start () 
	{
		UnityEngine.Camera camera= NGUITools.FindCameraForLayer(gameObject.layer);
		if(camera!=null)
		camera.clearFlags = CameraClearFlags.Skybox;
		
	    Str_Page1 = Core.Data.stringManager.getString(9024);
		//Debug.Log("Str_Page1="+Str_Page1);
	}
	
	public GameCG Play()
	{
		Invoke("PlayPage1",0.2f);
		return this;
	}
	
	public void PlayPage1()
	{
		
		Pages[0].SetActive(true);
		CGMask.enabled = true;
		Invoke("Page1_2",CGMaskSpeed);
	}
	
	void Page1_2()
	{
		Page1_Back2.enabled = true;		
		Core.Data.soundManager.SoundFxPlay(SoundFx.FX_prolog0);
		InvokeRepeating("Page1_WordEffect",0.13f,0.16f);
	}
	
	void Page1_WordEffect()
	{
		if(index < Str_Page1.Length)
		{
		   Lab_Page1.text = Str_Page1.Substring(0,index);
		   index++;
		}
		else
		{
			Lab_Page1.text = Str_Page1;
		    CancelInvoke("Page1_WordEffect");
			Invoke("Page1_3",2.5f);
		}
	}
	
	void Page1_3()
	{
		CGMask.PlayReverse();
		Invoke("FinishPage1",CGMaskSpeed);
	}
	
	void FinishPage1()
	{
         Pages[0].SetActive(false);
	     
		 Pages[1].SetActive(true);
		 Lab_Page2.text = Core.Data.stringManager.getString(9025);
		
         CGMask.PlayForward();
		 Invoke("PlayPage2",CGMaskSpeed);
	}
	
	//------------------Page2--------------------
	void PlayPage2()
	{
		Core.Data.soundManager.SoundFxPlay(SoundFx.FX_prolog1);
		Page2_Back.enabled = true;
		Invoke("Page2_2",4.5f);
	}
	
	void Page2_2()
	{
		CGMask.PlayReverse();
		Invoke("FinishPage2",CGMaskSpeed);
	}
	
	void FinishPage2()
	{
		 CGMask.PlayForward();
		 Pages[1].SetActive(false);
		 Background.enabled = false;
		 Pages[2].SetActive(true);
		 Invoke( "PlayPage3",1f);
	}
	
	//------------------Page3--------------------
	void PlayPage3()
	{
		Core.Data.soundManager.SoundFxPlay(SoundFx.FX_prolog2);
		Page3_Left.enabled = true;
		Page3_Right.enabled = true;
		Invoke("Page3_2",1.5f);
		Invoke("PlayPage3_2_2",0.2f);
		Invoke("Page3_1",1);
	}
	
	void Page3_1()
	{
		Core.Data.soundManager.SoundFxPlay(SoundFx.FX_prolog3);
	}

	
	void Page3_2()
	{

		Page3_BLeft.enabled = true;
		Invoke("Page3_3",1f);
	}
	
	void PlayPage3_2_2()
	{
		Vector3 pos1= Page3_Left.transform.localPosition ;
		pos1.x -=10f;
		Page3_Left.transform.localPosition = pos1;
		Vector3 pos2= Page3_Right.transform.localPosition ;
		pos2.x +=10f;
		Page3_Right.transform.localPosition = pos2;
	}
	
	
	
	void Page3_3()
	{
		Core.Data.soundManager.SoundFxPlay(SoundFx.FX_ZanBo1);
		Page3_LeftBall.SetActive(true);
		Invoke("Page3_4",2f);
	}
	
	void Page3_4()
	{		
		Page3_BRight.enabled = true;
		Invoke("Page3_5",1f);
	}
	
	void Page3_5()
	{
		Core.Data.soundManager.SoundFxPlay(SoundFx.FX_ZanBo3);
		Page3_Skill.SetActive(true);
		Invoke("Page3_6",2f);
	}
	
	void Page3_6()
	{
		CGMask.PlayReverse();
		Invoke("FinishPage3",CGMaskSpeed);
	}
	
	void FinishPage3()
	{
		Pages[2].SetActive(false);
		CGMask.PlayForward();
		Pages[3].SetActive(true);
		//Invoke("PlayPage4",CGMaskSpeed);
		PlayPage4();
	}
	
	//------------------Page4--------------------
	float Page4X;
	float Page4Y;
	int HongIndex = 0;
	int HaHaIndex = 0;
	void PlayPage4()
	{
		Page4_TLeft.gameObject.SetActive(true);
		Invoke("PlayPage4_2",0.8f);
	}
	
	void PlayPage4_2()
	{
		Core.Data.soundManager.SoundFxPlay(SoundFx.FX_prolog4);
		Invoke("PlayPage4_2_1",4f);
	}
	
	void PlayPage4_2_1()
	{
		BTeft.enabled = true;
		Invoke("PlayPage4_3",1.2f);
		Invoke("PlayPage4_2_2",0.9f);
	}
	
	
	
	void PlayPage4_2_2()
	{
		Go.SetActive(true);
	}
	
	void PlayPage4_3()
	{
		Core.Data.soundManager.SoundFxPlay(SoundFx.FX_BaoZha2);
		MiniItween.Shake(BTeft.gameObject,new Vector3(10f,10f,0),1.5f,MiniItween.EasingType.EaseInCubic,false);
		InvokeRepeating("PlayPage4_4",0.4f,0.2f);
	}
	
	void PlayPage4_4()
	{
		Hong[HongIndex].enabled = true;
		HongIndex++;
		if(HongIndex>2)
		{
			CancelInvoke("PlayPage4_4");
			Invoke("PlayPage4_5",0.5f);
		}
	}
	
	void PlayPage4_5()
	{
		Page4_TRight1.enabled = true;
		Invoke("PlayPage4_6",0.5f);
	}
	
	void PlayPage4_6()
	{
		Core.Data.soundManager.SoundFxPlay(SoundFx.FX_prolog5);
		Page4_TRight2.enabled=true;
		Invoke("PlayPage4_7",0.5f);
	}
	
	void PlayPage4_7()
	{
		Page4_TRight3.SetActive(true);
		Invoke("PlayPage4_8",0.5f);
	}
	
	void PlayPage4_8()
	{
		Page4_Last.SetActive(true);
		Invoke("PlayPage4_9",0.2f);
	}
	
	void PlayPage4_9()
	{
		Page4_Last2.SetActive(true);
		Core.Data.soundManager.SoundFxPlay(SoundFx.FX_prolog6);
		InvokeRepeating("PlayPage4_10",0.6f,0.22f);
	}
	
	void PlayPage4_10()
	{
		Debug.Log(HaHaIndex);
		HaHa[HaHaIndex].enabled = true;
		HaHaIndex++;
		if(HaHaIndex>3)
		{
			CancelInvoke("PlayPage4_10");
			CGMask.PlayReverse();
			Invoke("FinishPage4",CGMaskSpeed);
		}
	}
	
	void FinishPage4()
	{
		Pages[3].SetActive(false);
		CGMask.PlayForward();
		Pages[4].SetActive(true);
		Invoke("PlayPage5",CGMaskSpeed);
	}
	
   //------------------Page4--------------------
	
	void PlayPage5()
	{
		Core.Data.soundManager.SoundFxPlay(SoundFx.FX_BaoZha4);
		MiniItween.Shake(Page5_Left.gameObject,new Vector3(5f,5f,0),2f,MiniItween.EasingType.EaseInCubic,false);
		Page5_fly.enabled = true;
		Invoke("PlayPage5_2",2f);
	}
	
	void PlayPage5_2()
	{
		Page5_Right1.SetActive(true);
		Invoke("PlayPage5_3",0.5f);
	}
	
	void PlayPage5_3()
	{
		Page5_Right2.SetActive(true);
		Invoke("PlayPage5_4",0.5f);
	}
	
	void PlayPage5_4()
	{
		Page5_Right3.SetActive(true);
		Invoke("PlayPage5_5",0.5f);
	}
	
	void PlayPage5_5()
	{
		Page5_Ball.SetActive(true);
		Invoke("PlayPage5_6",1.2f);
	}
	
	void PlayPage5_6()
	{
		CGMask.PlayReverse();
		
		Invoke("FinishPage5",CGMaskSpeed);
	}
	
	void FinishPage5()
	{
//		if(OnFinished!= null)
//			OnFinished();
//		CGMask.PlayForward();
		Pages[4].SetActive(false);
		Background.enabled = true;
		Pages[5].SetActive(true);
		CGMask.PlayForward();
		PlayPage6();
	}
	
	
	void PlayPage6()
	{
		
		Page6_label.text = Core.Data.stringManager.getString(9026);
		Core.Data.soundManager.SoundFxPlay( SoundFx.FX_prolog7);
		Page6_XingQiu.gameObject.SetActive(true);
		Page6_fly.GetComponent<TweenScale>().PlayForward();
		Page6_fly.GetComponent<TweenPosition>().PlayForward();
		Invoke("PlayPage6_2",7f);
		Invoke("PlayPage6_3",10f);
	}
	
	void PlayPage6_2()
	{
		Page6_fly.GetComponent<TweenAlpha>().PlayForward();
	}
	
	void PlayPage6_3()
	{
		Page6_star.gameObject.SetActive(true);
		Invoke("PlayPage6_4",0.4f);
	}
	
	void PlayPage6_4()
	{
		Page6_star.GetComponent<TweenAlpha>().PlayReverse();
     	Page6_star.GetComponent<TweenScale>().PlayReverse();
		Invoke("PlayPage6_5",0.4f);
	}
	
	void PlayPage6_5()
	{
		Page6_star.gameObject.SetActive(false);
		Invoke("PlayPage6_6",1f);
	}
	
	void PlayPage6_6()
	{
		TweenAlpha temp= Page6_XingQiu.GetComponent<TweenAlpha>();
		temp.duration = 1.5f;
		temp.PlayReverse();
		Page6_label.GetComponent<TweenAlpha>().PlayForward();
		Page6_label.transform.parent.GetComponent<TweenAlpha>().PlayForward();
		Pages[6].SetActive(true);
//		CGMask.PlayReverse();
//		Invoke("FinishPage6",CGMaskSpeed);
		
		Invoke("FinishPage6",2f);
	}
	
	void FinishPage6()
	{
		PlayPage7();
//	     Pages[6].SetActive(false);
//		 CGMask.PlayForward();
	}
	
	void PlayPage7()
	{
		Page7_fly.gameObject.SetActive(true);
		Invoke("PlayPage7_2",0.3f);
	}
	
	void PlayPage7_2()
	{
		Page7_fly.gameObject.SetActive(false);
		Page7_bomb.gameObject.SetActive(true);
		Page7_gangdang.enabled = true;
		Core.Data.soundManager.SoundFxPlay(SoundFx.FX_BaoZha1);
		MiniItween.Shake(Page7_back.gameObject,new Vector3(5f,5f,0),2f,MiniItween.EasingType.EaseInCubic,false);
		Invoke("PlayPage7_3",2f);
	}
	
	void PlayPage7_3()
	{
		CGMask.GetComponent<UISprite>().spriteName="Symbol 54";
		CGMask.PlayReverse();
		Invoke("FinishPage7",CGMaskSpeed);
	}
	
	void FinishPage7()
	{
		UnityEngine.Camera camera= NGUITools.FindCameraForLayer(gameObject.layer);
		if(camera!=null)
		camera.clearFlags = CameraClearFlags.Depth;
		
		Core.Data.soundManager.BGMPlay(SceneBGM.BGM_GAMEUI);
		 //Core.Data.soundManager.BGMStop();
		Background.gameObject.SetActive(false);
		Pages[6].SetActive(false);
		CGMask.PlayForward();
		if(OnFinished!=null)
			OnFinished();
	}
	
}
