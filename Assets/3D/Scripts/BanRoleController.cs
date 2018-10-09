using UnityEngine;
using System.Collections;

public class BanRoleController : MonoBehaviour {

	public bool isLeft = true;

	public int width = 100;

	public CRLuo_PlayAnim_FX playAnimFX;

	void OnGUI() {
		//if (isLeft)
		//{

			

		//      //
		//}
		//else { 
			
		//}

		int startX;
		if (isLeft)
		{
			startX = 0;
		}
		else {
			startX = Screen.width - width;
		}

		int buttonCount = 9;

		float buttonHeight = Screen.height/buttonCount;

		for (int i = 0; i < buttonCount; i++)
		{
			if (GUI.Button(new Rect(startX, (int)(i * buttonHeight), width, Screen.height / buttonCount), ""+i))
			{
				if (playAnimFX != null)
				{
					playAnimFX.HandleTypeAnim(CRLuoAnim_Main.types[i]);
				}
			}
		}

	}

}
