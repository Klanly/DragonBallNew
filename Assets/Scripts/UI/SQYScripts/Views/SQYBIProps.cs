using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SQYBIProps : SQYBoxItem
{
	UILabel lab_name = null;
	UISprite sp_head=null;
	UILabel lal_count = null;

	Item curItem = null;

	protected override void initBoxItem ()
	{
		if(bInitBox)return;
		base.initBoxItem ();

		lab_name = allLabels[0];
		lal_count = allLabels[1];

		sp_head = allSprites[2];
	}

	
	public override void freshBoxItemWithData (object obj)
	{
		base.freshBoxItemWithData(obj);
		curItem = obj as Item;
		curItemType = RUIType.EMItemType.Props;
		if(curItem != null)
		{
			this.gameObject.name = curItem.RtData.id.ToString();
			lab_name.text = curItem.configData.name;


            lal_count.text = ItemNumLogic.setItemNum(curItem.RtData.count , lal_count , lal_count.gameObject.transform.parent.gameObject.GetComponent<UISprite>());//yangcg
//			sp_head.spriteName = curItem.RtData.num.ToString ();
			sp_head.spriteName = curItem.configData.iconID.ToString ();
		}
	}
	
	
	public static SQYBIProps CreateBIProps()
	{

		UnityEngine.Object obj = PrefabLoader.loadFromPack("SQY/pbSQYBIProps", true, true);
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			SQYBIProps cv = go.GetComponent<SQYBIProps>();
			cv.initBoxItem();
			return cv;
		}
		return null;
	}
	
}
