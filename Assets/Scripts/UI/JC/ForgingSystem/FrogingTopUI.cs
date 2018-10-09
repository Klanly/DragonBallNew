using UnityEngine;
using System.Collections;


public class FrogingTopUI : MonoBehaviour 
{
    public UILabel Spr_Title;
	
	public UILabel Lab_Des;
	
	public GameObject bg;
	
	public void SetTitle(string text)
	{


//        {"ID":90017,"txt":"装备锻造屋"}
//        {"ID":90018,"txt":"宝石镶嵌"}
//        {"ID":90019,"txt":"宝石合成"}
//        {"ID":90020,"txt":"宝石插槽"}


        string str ;
        if ("bsxt_bsxq" == text )
        {
            str =  Core.Data.stringManager.getString(90018);
            Spr_Title.text = str;

        }else if ("bsxt_zbdzw" == text)
        {
            str =  Core.Data.stringManager.getString(90017);
            Spr_Title.text = str;
        }else if ("bsxt_zbcz" == text)
        {
            str =  Core.Data.stringManager.getString(90020);
            Spr_Title.text = str;
        }else if ("bsxt_bshc"== text) 
        {
            str =  Core.Data.stringManager.getString(90019);
            Spr_Title.text = str;
        }




		Spr_Title.MakePixelPerfect();
	}
	
	public void SetDes(string text)
	{
		Lab_Des.text = text;
	}
	
	public bool ShowBg
	{
		set
		{
			if(bg.activeSelf != value)
				bg.SetActive(value);
		}
	}
}
	
