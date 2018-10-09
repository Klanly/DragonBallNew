using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class JCPVEPlotMap : MonoBehaviour {

	public UISprite Spr_Map;
	public UISprite Spr_MapPath;
	public GameObject MapGrid;
	public UISprite mask;
	
	public List<JCPVEPlotFloor> bulidings = new List<JCPVEPlotFloor>();
		
	public void SetMap(NewChapter chapterData)
	{
		if(chapterData ==  null)
		{
			gameObject.SetActive(false);
			return;
		}

	   if(!gameObject.activeSelf) gameObject.SetActive(true);
		int MapID = (chapterData.config.ID - 30100)/100;
		gameObject.name = MapID.ToString();
		
		NewChapterData configdata = chapterData.config;
		//JCPVEPlotController.Instance.Lab_Title.text = configdata.name;
		Spr_Map.spriteName = configdata.mapID;
		Spr_MapPath.spriteName = configdata.pathMapID;
		Spr_MapPath.MakePixelPerfect();
		Spr_MapPath.transform.localScale = new Vector3(2.048081f,2.048081f,1f);
		mask.color = chapterData.color;
		int[] floorID =  configdata.floorID;
		
	    Spr_MapPath.transform.localPosition = chapterData.pathPosition;
		
		
#region 建筑物
		if(bulidings.Count < floorID.Length)
		{
			int cha = floorID.Length - bulidings.Count;
			Object prefab = PrefabLoader.loadFromPack("JC/JCPVEPlotBuilding");
			for(int i=0;i<cha;i++)
			{			
				if(prefab != null)
				{
					GameObject buliding = Instantiate(prefab) as GameObject;
					buliding.transform.parent = MapGrid.transform;						
					buliding.transform.localScale = Vector3.one;
					JCPVEPlotFloor script = buliding.GetComponent<JCPVEPlotFloor>();
					bulidings.Add(script);
				}
			}
		}
		if(floorID.Length > 0)
		{
		    Dictionary<int,NewFloor> FloorList = Core.Data.newDungeonsManager.FloorList;
			int i=0;
			for(;i<floorID.Length;i++)
			{
				NewFloor floorData = null;
				if( FloorList.TryGetValue(floorID[i],out floorData) )
				{
					bulidings[i].name = floorID[i].ToString();
					if(bulidings[i] != null)
					{
						bulidings[i].SetData(floorData);
						if(!JCPVEPlotController.Instance.Dic_Floors.ContainsKey(floorID[i]))
						    JCPVEPlotController.Instance.Dic_Floors.Add(floorID[i],bulidings[i]);
						else
							JCPVEPlotController.Instance.Dic_Floors[ floorID[i] ] = bulidings[i];
					}
				}
			}
			for(;i<bulidings.Count;i++)
				bulidings[i].SetData(null);
		}
#endregion
		
#region 一切没有解锁的或不能访问的状态一律隐藏		
		if(!JCPVEPlotController.Instance.isUnlockMap( (chapterData.config.ID - 30100)/100,false ) )
		{
			gameObject.SetActive(false);
		}
		if(MapID > 0)
		{
			//如果之前一关没有通关
			if(!JCPVEPlotController.Instance.isPassCurMap(MapID - 1))
			{
				gameObject.SetActive(false);
			}
		}
#endregion		
		
	}
}
