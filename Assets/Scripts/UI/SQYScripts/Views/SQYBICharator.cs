using UnityEngine;
using System.Collections;
using RUIType;
public class SQYBICharator : SQYBoxItem {


	Monster curMonster = null;

	UILabel lab_Level;
	UILabel lab_Name;

	UISprite sp_InTeam;
	UISprite sp_Attribut;
	UISprite sp_Star;
	UISprite sp_Head;
	UISprite sp_stage;


	protected override void initBoxItem ()
	{
		if(bInitBox)return;
		base.initBoxItem ();

		if(allLabels.Length>0)lab_Level = allLabels[0];
		if(allLabels.Length>1)lab_Name = allLabels[1];
		if(allSprites.Length>0)sp_Attribut = allSprites[0];
		if(allSprites.Length>1)sp_InTeam = allSprites[1];
		if(allSprites.Length>2)sp_Star = allSprites[2];
		if(allSprites.Length>3)sp_Head = allSprites[3];
		if(allSprites.Length>4)sp_stage = allSprites[4];
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
		curItemType = EMItemType.Charator;
		curMonster = obj as Monster;

		RED.SetActive (false, sp_InTeam.gameObject);
		if(curMonster == null)
		{
			lab_Level.text = "";
			sp_Attribut.enabled = false;
			sp_Star.enabled = false;
		}
		else
		{
			this.gameObject.name = curMonster.pid.ToString();

			if(SQYPetBoxController.mInstance._boxType != EMBoxType.CHANGE)
			{
				RED.SetActive(curMonster.inTeam, sp_InTeam.gameObject);
			}
			
			sp_Star.enabled = true;
			sp_Attribut.enabled = true;
			
			lab_Name.text = curMonster.config.name;

			AtlasMgr.mInstance.SetHeadSprite(sp_Head, curMonster.num.ToString());

			sp_Head.MakePixelPerfect();
			
			lab_Level.text = "Lv" + curMonster.RTData.curLevel.ToString();

			if(curMonster.num == 19999 || curMonster.num == 19998)
			{
				sp_Attribut.spriteName = "common-1055";
				sp_Star.spriteName = "star6";
			}
			else
			{
				sp_Attribut.spriteName = "Attribute_"+((int)curMonster.RTData.Attribute).ToString();
				sp_Star.spriteName = "star" + curMonster.RTData.m_nAttr.ToString();
			}

			sp_Attribut.MakePixelPerfect();



			this.name = curMonster.pid.ToString();

			if(curMonster.RTData.m_nStage == 1)
			{
				RED.SetActive(false, sp_stage.gameObject);
			}
			else
			{
				RED.SetActive(true, sp_stage.gameObject);
				sp_stage.spriteName = "stage_" + curMonster.RTData.m_nStage.ToString();
			}
		}
	}
	
	protected override void dealloc ()
	{
		lab_Level = null;
		lab_Name = null;
		sp_Attribut = null;
		sp_Star = null;
		sp_InTeam = null;
		sp_Head = null;

		base.dealloc ();
	}
	void OnDestroy()
	{
		dealloc();
	}
	public static SQYBICharator CreateBICharator()
	{
		UnityEngine.Object obj = PrefabLoader.loadFromPack("SQY/pbSQYBICharator", true, true);
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			SQYBICharator cv = go.GetComponent<SQYBICharator>();
			cv.initBoxItem();
			return cv;
		}
		return null;
	}
}
