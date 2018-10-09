using UnityEngine;
using System.Collections;

public class SQYBIGem : SQYBoxItem 
{
	UILabel lab_Name;
	UISprite sp_equiped;
	UISprite sp_Head;
	Gems curGem = null;


	protected override void initBoxItem ()
	{
		if(bInitBox)return;
		base.initBoxItem ();

		if(allLabels.Length > 0)
			lab_Name = allLabels[0];
			
		if(allSprites.Length>2)
			sp_Head = allSprites[2];
		if(allSprites.Length>3)
			sp_equiped = allSprites[3];
	}

	public override void freshBoxItemWithData (object obj)
	{
		base.freshBoxItemWithData(obj);
		curGem = obj as Gems;
		curItemType = RUIType.EMItemType.Gem;
		if(curGem != null)
		{
			lab_Name.text = curGem.configData.name;
			sp_Head.spriteName  = curGem.configData.anime2D;
			sp_Head.MakePixelPerfect();
			RED.SetActive(curGem.equipped, sp_equiped.gameObject);
			this.name = curGem.id.ToString();
		}
	}
	public static SQYBIGem CreateBIGem()
	{
		UnityEngine.Object obj = PrefabLoader.loadFromPack("SQY/pbSQYBIGem", true, true);
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			SQYBIGem cv = go.GetComponent<SQYBIGem>();
			cv.initBoxItem();
			return cv;
		}
		return null;
	}
}
