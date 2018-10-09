using UnityEngine;
using System.Collections;

public class BtnChallengeEffect : MonoBehaviour {

	public TweenRotation LS;
	public TweenRotation RS;
	public UISprite fire ;

	const float intervalTime = 0.3f;
	
	void Start () 
	{

	}
	
	void OnEnable()
	{
		Play(true);
	}
	
	void OnDisable()
	{
		Play(false);
	}
	
	void ResetPos()
	{
		LS.transform.localRotation = Quaternion.Euler(new Vector3(0,0,0));
		RS.transform.localRotation = Quaternion.Euler(new Vector3(0,180f,0));
	}
	
	
	
	void Play(bool Value)
	{
		LS.enabled = Value;
		RS.enabled = Value;
		if(!Value)
		{
	     	ResetPos();
		}
		
	}
	
	
	void ShowFire()
	{
		fire.enabled = true;
		Invoke("HideFire",Time.deltaTime*3);
	}
	
	void HideFire()
	{
		fire.enabled = false;
	}
	
	
	bool forwardOrReverse = true;
	int count = 0;
	public void OnFinished()
	{
		forwardOrReverse = !forwardOrReverse;
		if(!forwardOrReverse)
		        Invoke("AutoFanZhan",Time.deltaTime);	
		else
		{
			ShowFire();
			count++;
		    if(count > 1)
			{
			    Invoke("AutoFanZhan",3f);
				count = 0;
			}
			else
			    Invoke("AutoFanZhan",Time.deltaTime);	
		}
	}
	
	public void AutoFanZhan()
	{
		if(!this.enabled)
		{
			ResetPos();
			return;
		}
		
		if(!forwardOrReverse)
		{
		    LS.PlayReverse();
			RS.PlayReverse();
		}
		else
		{
		    LS.PlayForward();
			RS.PlayForward();
		}
	}
	
	
	
	
	
}
