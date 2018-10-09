using UnityEngine;
using System.Collections;

public static class RUIExtend {
	public static void textID (this UILabel label,int id)
	{
		if(label != null)
		{
			if(Core.Data != null && Core.Data.stringManager != null)
			{
				label.text = Core.Data.stringManager.getString(id);
			}
		}
	}
	public static void SafeText(this UILabel label,string text)
	{
		if(label != null)
		{
			label.text = text;
		}
	}
}
