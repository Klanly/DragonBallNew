using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GemRecastingSystemUI_View : MonoBehaviour {
	
	public System.Action<GameObject> ButtonClick;
	public List<GemRecastingHoleInfo> all_holes=new List<GemRecastingHoleInfo>();
	Dictionary<int,List<int>> Dic_Holes = null;
	public UISprite Spr_Equipment;
	//public UILabel Lab_Describe;
	//public UILabel Lab_Recast;
	/*当前装备上的所有孔
	 * */
	List<GemRecastingHoleInfo> AllholesInCurEquipment=null;
	public FrogingTopUI frogingTopUI;
	public UILabel Lab_stone;
	public UILabel Lab_Add;
	void Start () 
	{
	    Dic_Holes = new Dictionary<int, List<int>>();
		Dic_Holes.Add(1,new List<int>(){0});
		Dic_Holes.Add(2,new List<int>(){0,5});
		Dic_Holes.Add(3,new List<int>(){2,4,6});
		Dic_Holes.Add(4,new List<int>(){1,2,3,4});
		#region 组件名字
//		Lab_Describe.text=TEXT(9005)+TEXT(9004);
//		Lab_Recast.text=TEXT(9005);
 		Lab_Add.text = TEXT(5006);
		
		#endregion
	}
	
	void OnEnable()
	{
		frogingTopUI.SetTitle("bsxt_zbcz");
		frogingTopUI.SetDes(TEXT(9018));
	}
	
	public string TEXT(int num_text)
	{
		return	Core.Data.stringManager.getString(num_text);
	}
	
	public void GemRecastingSystemClick(GameObject btn)
	{
		if(ButtonClick!=null)
			ButtonClick(btn);
	}
	
	/*显示装备上的孔
	 * */
	public void ShowHoles (EquipSlot[] slots) 
	{
	    foreach(GemRecastingHoleInfo go in all_holes)
		{
		   if(go.gameObject.activeSelf)go.gameObject.SetActive(false);
		}
		int hole_count=slots.Length;
		AllholesInCurEquipment=new List<GemRecastingHoleInfo>();
		if(Dic_Holes.ContainsKey(hole_count))
		{
			List<int> temphole= Dic_Holes[hole_count];
			foreach(int index in temphole)
			{
				all_holes[index].gameObject.SetActive(true);
				AllholesInCurEquipment.Add(all_holes[index]);
			}
				
		}
		for(int hole_index=0;hole_index<slots.Length;hole_index++)
		{
			EquipSlot slot=slots[hole_index];	
			AllholesInCurEquipment[hole_index].SetHoleColor(slot.color);
			/*如果孔里面有宝石
			 * */
			if(slot.id>0)
			{
				AllholesInCurEquipment[hole_index].SetGem(slot.mGem.configData.anime2D);
                AllholesInCurEquipment[hole_index].SetLv(slot.mGem.configData.level);
				AllholesInCurEquipment[hole_index].isHaveGem=true;
			}
			else
			{
				AllholesInCurEquipment[hole_index].SetGem(null);
				AllholesInCurEquipment[hole_index].isHaveGem=false;
                AllholesInCurEquipment[hole_index].SetLv(0);
			}
		}
	}
	
	/*显示选中的装备
	 * */
	public void SetSelectedEquipment(string EquipmentSpriteName)
	{
		if(EquipmentSpriteName == null)
			Spr_Equipment.enabled=false;
		else
		{
			if(!Spr_Equipment.enabled)Spr_Equipment.enabled=true;
			Spr_Equipment.spriteName=EquipmentSpriteName;
			Spr_Equipment.MakePixelPerfect();
		}
		
	}
	
	/*获得装备上锁定的格子数组
	 * */
	public int[] GetLockAarry()
	{
		List<int> list=new List<int>();
		for(int i=0;i<AllholesInCurEquipment.Count;i++)
		{
			if(AllholesInCurEquipment[i].isLock)
				list.Add(i);
		}
		return list.ToArray();
	}
	
	/*清空显示面板
	 * */
	public void ClearGemInlayViewPanel()
	{
		foreach(GemRecastingHoleInfo gemview in all_holes)
		{
			gemview.SetGem(null);
			gemview.isHaveGem=false;
			if(gemview.isLock)
			gemview.AutoLockOrUnLock();
			gemview.gameObject.SetActive(false);
		}
		SetSelectedEquipment(null);
		SetNeedStone(0);
	}
	
	public void SetNeedStone(int stone)
	{
		Lab_stone.text = stone.ToString();
	}
	
}
