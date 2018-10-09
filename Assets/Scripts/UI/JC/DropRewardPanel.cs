using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DropRewardPanel : MonoBehaviour {
	
	
	public UITable uiTable;	
	List<DropElement> Elements = new List<DropElement>();
	private int ChapterID;
	//{"ID":20001,"txt":"第#章"}
	public UILabel Title;
	
	List<string> Nums = new List<string>(){"一","二","三","四","五","六","七","八","九","十","十一","十二","十三","十四","十五"};
	
	void Start () 
	{
	    
	}
	
	//初始化
	void Init()
	{
		
		Chapter chapterData = null;		
		DungeonsManager dm = Core.Data.dungeonsManager;

		
		if(dm.ChapterList.TryGetValue(this.ChapterID,out chapterData))
		{
			int[] cityID = chapterData.config.cityID;
			int cityCount = cityID.Length;
			CreateElement(cityCount);
			//赋值
			for(int i=0 ;i< cityCount ;i++)
			{
				City city = null;
				if(dm.CityList.TryGetValue(cityID[i], out city))
				{
					int[] floorID = city.config.floorID ;
					if(floorID.Length > 0)
					{
						Floor floor = null;
				        if(dm.FloorList.TryGetValue(floorID[floorID.Length-1] , out floor))
						{
							Elements[i].gameObject.SetActive(true);
							Elements[i].SetData(floor,city);
						}
					}
				}
			}
			
		}
		uiTable.repositionNow = true;
	}
	
	
	void CreateElement(int count)
	{
		Object prefab = PrefabLoader.loadFromPack("JC/DropElement");
		if(prefab == null) return;
		for(int i=0 ;i< count ;i++)
		{
			GameObject obj = Instantiate(prefab) as GameObject;
			RED.AddChild(obj, uiTable.gameObject);		
			Elements.Add(obj.GetComponent<DropElement>());
			obj.SetActive(false);
		}
	}
	
	static DropRewardPanel _this;
	public static void OpenUI(int ChapterID)
	{
		Object prefab = PrefabLoader.loadFromPack("JC/DropRewardPanel");
		if(prefab != null)
		{
			GameObject obj = Instantiate(prefab) as GameObject;
			RED.AddChild(obj, DBUIController.mDBUIInstance._TopRoot);
			_this = obj.GetComponent<DropRewardPanel>();
			_this.ChapterID = ChapterID;
					//30100
		      int numIndex = (_this.ChapterID - 30000)/100 -1;
		       if(numIndex>=0 && numIndex<_this.Nums.Count) 
				_this.Title.text = Core.Data.stringManager.getString(20001).Replace("#",  _this.Nums[numIndex] );
			
		}
	}	
	
     
	void Close()
	{
		Destroy(gameObject);
		Resources.UnloadUnusedAssets();
	}
	
	public static void DestroySelf()
	{
		if(_this != null)
		{
			_this.Close();
			_this = null;
		}
	}
	
}
