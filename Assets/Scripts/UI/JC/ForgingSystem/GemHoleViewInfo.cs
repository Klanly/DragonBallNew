using UnityEngine;
using System.Collections;

public class GemHoleViewInfo : MonoBehaviour {

	public UISprite  Spr_HoleColor;
	public UISprite  Spr_HoleGem;
	public GameObject effect;
	public UILabel lbl_lv;

	public void SetHoleColor(short color)
	{
//		Spr_HoleColor.spriteName="solt"+color.ToString();
		if(Spr_HoleColor!= null && Spr_HoleColor.transform.parent.GetComponent<UIButton>() != null)
			Spr_HoleColor.transform.parent.GetComponent<UIButton>().normalSprite = "solt"+color.ToString();
		else 
			Spr_HoleColor.spriteName="solt"+color.ToString();
	}
	
	public void SetGem(string GemSpriteName)
	{
		if(GemSpriteName==null)
		{
			Spr_HoleGem.enabled=false;
			isHaveGem=false;
			//Debug.Log(gameObject.name+"  ->  "+isHaveGem.ToString());
		}
		else
		{
			if(!Spr_HoleGem.enabled)Spr_HoleGem.enabled=true;
			Spr_HoleGem.spriteName=GemSpriteName;
			Spr_HoleGem.MakePixelPerfect();
			isHaveGem=true;
		}

		RED.SetActive (!isHaveGem, effect);
	}

	public void SetGemLv(int lv){
		if (lbl_lv != null) {
			if (lv == 0)
				lbl_lv.gameObject.SetActive (false);
			else {
				lbl_lv.gameObject.SetActive (true);
				lbl_lv.text = "Lv" + lv;
			}
		}
	}

	public bool isHaveGem{get;set;}
}
