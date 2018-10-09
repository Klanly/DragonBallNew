using UnityEngine;
using System.Collections;

public class BanTemp : MonoBehaviour {

	public bool b_Use = true;

	public int count = 9;

	public int button_Height = 100;

	public CRLuo_PlayAnim_FX [] playAnims;

	void OnGUI(){

		float width = (float)Screen.width/count;

		if(b_Use){
			for(int i = 0;i<count;i++){
				if(GUI.Button( new Rect(width*i,0,width,button_Height),""+i )){
					foreach(CRLuo_PlayAnim_FX aCRLuo_PlayAnim_FX in playAnims){
						aCRLuo_PlayAnim_FX.HandleTypeAnim(CRLuoAnim_Main.types[i]);
					}
				}
			}
		}

		if(GUI.Button( new Rect(0,Screen.height - button_Height,button_Height,button_Height),"switch")){
			b_Use = !b_Use;
		}

	}

	void Update(){

	}

	public void OnButtonClick(){

	}

}
