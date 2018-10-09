using UnityEngine;
using System.Collections;

public class BuErMaAnimation : MonoBehaviour {

	public UISprite Spr_Eye;
	public UISprite Spr_Mouth;
	private UISpriteAnimation Anim_Eye;
	public UISpriteAnimation Anim_Mouth;
	private bool PlayingEye{get;set;}
	private bool PlayingMouth{get;set;}
	/*眨眼次数
	 * */
	private int PlayEyeCount;
	private int PlayEyeIndex;
	
	public string DefaultMouth = "mouth-10080001";
	public string DefaultEyes = "eye-10090001";
	
	void Start () 
	{
	    Anim_Eye = Spr_Eye.GetComponent<UISpriteAnimation>();
		//Anim_Mouth = Spr_Mouth.GetComponent<UISpriteAnimation>();
		PlayingEye = false;
		PlayingMouth = false;
		PlayEyeCount = 0;
		PlayEyeIndex = 0;
		RandomPlayEye();
	}
	
	
    void RandomPlayEye()
	{
		PlayEye();
		int playEyeTime = Random.Range(1,4);
		Invoke("RandomPlayEye",playEyeTime);
	}
	
	
	public void PlayEye(int playEyeCount = 1)
	{
		PlayEyeCount = playEyeCount;
		PlayingEye = true;
		Anim_Eye.enabled = true;
	}
	
	public void PlayMouth(bool StartOrStop)
	{
//		Debug.Log("StartOrStop="+StartOrStop);
		PlayingMouth = StartOrStop;
		if(PlayingMouth)
		Anim_Mouth.enabled = true;
	}
	
	void Update () 
	{
	     if(PlayingEye)
		{
			if(Spr_Eye.spriteName == DefaultEyes)
			{
				if(PlayEyeIndex >= PlayEyeCount)
				{
					PlayingEye = false;
				    Anim_Eye.enabled = false;
					PlayEyeIndex = 0;
					PlayEyeCount = 0;
				}
			}
			else
				PlayEyeIndex++;
		}
		
		if(!PlayingMouth)
		{
			if(Spr_Mouth.spriteName == DefaultMouth)
			{
				Anim_Mouth.enabled = false;
				PlayingMouth = false;				
			}
		}
	}
}
