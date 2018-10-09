using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//PVP地图UI管理器
public class PVPMapController : MonoBehaviour {
	
	public UIGrid uiGrid;
	List<JCPVEPlotMap> List_Maps = new List<JCPVEPlotMap>();
	
	private UICenterOnChild uicenter;
	public UIScrollView _uiscrollview;
	
    //private int CurMapIndex = 0;
	
	public static JCPVEPlotController Instance;
	
	//关闭按钮回调
	public System.Action<string> Exit;
	
	Dictionary<int,NewFloor> FloorList = null;
	
	public UILabel Lab_Title;
	
	void Start ()
	{
		FloorList = Core.Data.newDungeonsManager.FloorList;
		
	    CreateMap();

	}
	
	
	public static JCPVEPlotController OpenUI()
	{
		if(Instance == null)
		{
			Object prefab = PrefabLoader.loadFromPack("JC/PVPMapController");
			if(prefab != null)
			{
				GameObject obj = Instantiate(prefab) as GameObject;
				RED.AddChild(obj, DBUIController.mDBUIInstance._bottomRoot);
				Instance = obj.GetComponent<JCPVEPlotController>();				
			}
		}
		return Instance;
	}

	
	//创建地图
	void CreateMap ()
	{
		Object prefab = PrefabLoader.loadFromPack("JC/JCPVEPlotMap");
		if( prefab != null)
		{
			Dictionary<int, NewChapter> list_chapter = Core.Data.newDungeonsManager.ChapterList;
			int i = -1;
			foreach(NewChapter data in list_chapter.Values)
			{
				i++;
	            GameObject map = Instantiate(prefab) as GameObject;
				map.transform.parent = uiGrid.transform;
				map.transform.localPosition = new Vector3(uiGrid.cellWidth*i,0,0);
				map.transform.localScale = Vector3.one;
				map.name = i.ToString();
				JCPVEPlotMap jcmap = map.GetComponent<JCPVEPlotMap>();
				if(jcmap!=null)
				{
					jcmap.SetMap(data);
				    List_Maps.Add(jcmap);
				}
				map.SetActive(false);
			}
		}
	}
	
	
	void OnBtnClick(GameObject btn)
	{
		OnBtnClick(btn.name);
	}
	
	void OnBtnClick(string btnName)
	{
		switch(btnName)
		{
		case "Btn_Close":
		        DestoryMe();
			break;
		}
	}
	
	//销毁自身<释放内存>
	void DestoryMe()
	{
		Instance = null;
		if(Exit != null) Exit(gameObject.name);
		Destroy(gameObject);
	}
	
	//建筑物被点击了
	void OnBuildingClick(GameObject btn)
	{
		int buildingID = 0;
		int.TryParse(btn.name,out buildingID);
		NewFloor floorData = null;
		if(FloorList.TryGetValue(buildingID,out floorData))
		{
			JCPVEPlotDes.OpenUI(floorData);
		}
	}
	
	
}
