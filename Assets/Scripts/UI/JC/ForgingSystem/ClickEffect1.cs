using UnityEngine;
using System.Collections;

public class ClickEffect1 : MonoBehaviour {
	
	public UISprite Selected = null;
	public UISprite DragBall = null;
	void OnPress (bool isPressed)
	{
		Selected.enabled=isPressed;
//		if(DragBall!=null)
//		{
//			if(isPressed)
//				DragBall.spriteName = "common-1005";
//			else
//				DragBall.spriteName = "common-1006";
//		}
	}
}
