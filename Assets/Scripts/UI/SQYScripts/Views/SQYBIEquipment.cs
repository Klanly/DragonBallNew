using UnityEngine;
using System.Collections;
using RUIType;
public class SQYBIEquipment : SQYBoxItem {


	UILabel lab_Level;
	UILabel lab_Name;

	UISprite sp_InTeam;
	UISprite sp_Exp;

	UISprite sp_Head;

	Equipment curEquipment=null;

	protected override void initBoxItem ()
	{
		if(bInitBox)return;
		base.initBoxItem ();

		if(allLabels.Length>0)lab_Level = allLabels[0];
		if(allLabels.Length>1)lab_Name = allLabels[1];

		if(allSprites.Length>0)sp_Exp = allSprites[1];
		if(allSprites.Length>1)sp_InTeam = allSprites[0];
		if(allSprites.Length>3)sp_Head = allSprites[3];
	}

	void Awake()
	{
		szTwPos = group_Select.GetComponentsInChildren<TweenPosition>();
		for(int i=0;i<szTwPos.Length;i++)
		{
			Destroy(szTwPos[i]);
			szTwPos[i] = null;
		}
	}

	public override void freshBoxItemWithData (object obj)
	{
		base.freshBoxItemWithData(obj);
		curItemType = EMItemType.Equipment;
		curEquipment = obj as Equipment;

		RED.SetActive (curEquipment.equipped, sp_InTeam.gameObject);

		if(curEquipment != null)
		{
			this.gameObject.name = curEquipment.RtEquip.id.ToString();
			lab_Name.text = curEquipment.ConfigEquip.name;
			lab_Level.text = "LV."+curEquipment.RtEquip.lv.ToString();
			sp_Head.spriteName = curEquipment.ConfigEquip.ID.ToString();

			if (SQYPetBoxController.mInstance._boxType == EMBoxType.Equip_ADD_ATK
				|| SQYPetBoxController.mInstance._boxType == EMBoxType.Equip_ADD_DEF
				|| SQYPetBoxController.mInstance.boxType == EMBoxType.Equipment_SWAP_ATK
				|| SQYPetBoxController.mInstance.boxType == EMBoxType.Equipment_SWAP_DEF)
			{
				RED.SetActive (false, sp_InTeam.gameObject);
			}
		
			if(curEquipment.Num == 40999 || curEquipment.Num == 40998 || curEquipment.Num == 45999 || curEquipment.Num == 45998)
			{
				RED.SetActive(true, sp_Exp.gameObject);
			}
			else
			{
				RED.SetActive(false, sp_Exp.gameObject);
			}
		}
		this.name = curEquipment.RtEquip.id.ToString();
	}

	protected override void dealloc ()
	{
		lab_Level = null;
		lab_Name = null;
		sp_InTeam = null;
		sp_Head = null;

		base.dealloc ();
	}
	void OnDestroy()
	{
		dealloc();
	}

	public static SQYBIEquipment CreateBIEquipment()
	{
		UnityEngine.Object obj = PrefabLoader.loadFromPack("SQY/pbSQYBIEquipment", true, true);
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			SQYBIEquipment cv = go.GetComponent<SQYBIEquipment>();
			cv.initBoxItem();
			return cv;
		}
		return null;
	}
	
}
