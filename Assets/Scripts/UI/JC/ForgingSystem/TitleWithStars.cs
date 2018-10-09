using UnityEngine;
using System.Collections;

public class TitleWithStars : MonoBehaviour {

	public UILabel Lab_Title;
	public UISprite L_Star;
	public UISprite R_Star;
	public int LStarOffsetX=-3;
	public int RStarOffsetX=3;
	public void SetTitle(string text)
	{
		float halffontlength=(Lab_Title.fontSize*(text.Length)+(text.Length-1)*Lab_Title.spacingX)/2;
		Lab_Title.text = text;
		L_Star.transform.localPosition = new Vector3(-halffontlength+LStarOffsetX,0,0);
		R_Star.transform.localPosition = new Vector3(halffontlength+RStarOffsetX,0,0);
	}
	
	
	public string text
	{
		set
		{
			SetTitle(value);
		}
		get
		{
			return Lab_Title.text;
		}
	}
}
