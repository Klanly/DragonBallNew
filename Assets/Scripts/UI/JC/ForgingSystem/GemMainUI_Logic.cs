using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FrogingSystem;

//宝石系统逻辑层
public class GemMainUI_Logic : MonoBehaviour 
{

	public void Start()
	{
		//注册监听
		gameObject.GetComponent<GemMainUI_View>().ButtonClick+=ChoseChildSystem;
	}
	
	public void ChoseChildSystem(GameObject Btn)
	{
		switch(Btn.name)
		{
		case "Btn_Gem_Synthetic":		
			ForgingRoomUI.Instance.GoTo(ForgingPage.Forging_Synthetic);
			break;
		case "Btn_Gem_Mosaic":			
			ForgingRoomUI.Instance.GoTo(ForgingPage.Forging_Mosaic);
			break;
		case "Btn_Gem_Recasting":
			ForgingRoomUI.Instance.GoTo(ForgingPage.Forging_Recasting);
			break;
		}
	}


	

}
