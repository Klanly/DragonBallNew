using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//副本弱引导
public class FBWeekGuide : MonoBehaviour {

	
	void OnEnable () 
	{
	     if(Core.Data.playerManager.Lv < 10 )
		{
			if(!Core.Data.guideManger.isGuiding)
				GuideHand.SetActive(true);
			else
				GuideHand.SetActive(false);
		}
		else
		{
			if(_guideHand!= null)
				Destroy(_guideHand);
		}
	}
	
	private GameObject _guideHand;
	private GameObject GuideHand
	{
		get
		{
			if(_guideHand == null)
			{
		        Object prefab = PrefabLoader.loadFromPack("JC/JianTou");
				_guideHand = Instantiate(prefab) as GameObject;
				_guideHand.transform.parent = transform;
				_guideHand.transform.localScale = Vector3.one;
				_guideHand.transform.localPosition = new Vector3(0,100f,0);
				_guideHand.transform.localEulerAngles = new Vector3(0,0,90f);				
			}
			return _guideHand;
		}
	}
}
