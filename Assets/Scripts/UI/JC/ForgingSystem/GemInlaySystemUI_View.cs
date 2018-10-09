using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GemInlaySystemUI_View : MonoBehaviour {
	
	
	public List<GemHoleViewInfo> all_holes=new List<GemHoleViewInfo>();
	Dictionary<int,List<int>> Dic_Holes = null;
	public System.Action<GameObject> ButtonClick;
	public UISprite Spr_Equipment;

	public ParticleSystem[] arryParticle;

	/*当前装备上的所有孔
	 * */
	List<GemHoleViewInfo> AllholesInCurEquipment=null;
	public UILabel Lab_InlayDescribe;
	public FrogingTopUI frogingTopUI;
	public UILabel Lab_EquipEffect;
	public UILabel Lab_Add;

	#region 装备详细信息
	public UILabel Lab_EquipmentName;
	public UILabel Lab_EquipmentAttact;
	public UILabel Lab_EquipmentDef;
	public UILabel Lab_SkillTrigger;
	public UILabel Lab_EquipmentAttrib;
	#endregion
	
	public void Init()
	{
	    Dic_Holes = new Dictionary<int, List<int>>();
		Dic_Holes.Add(1,new List<int>(){0});
		Dic_Holes.Add(2,new List<int>(){0,5});
		Dic_Holes.Add(3,new List<int>(){2,4,6});
		Dic_Holes.Add(4,new List<int>(){1,2,3,4});
		#region 组件名字
		//Lab_InlayDescribe.text=TEXT(9003)+TEXT(9004);
		
		Lab_Add.text = TEXT(5006);
		#endregion
	}
	
	
	void OnEnable()
	{
		ClearEquipInfo();
		//宝石镶嵌
		frogingTopUI.SetTitle("bsxt_bsxq");
		frogingTopUI.SetDes(TEXT (9017));
	}
	
	public string TEXT(int num_text)
	{
		return	Core.Data.stringManager.getString(num_text);
	}
	
	
	/*显示装备上的孔
	 * */
	public void ShowHoles (EquipSlot[] slots) 
	{
	    foreach(GemHoleViewInfo go in all_holes)
		{
		   if(go.gameObject.activeSelf)go.gameObject.SetActive(false);
		}
		int hole_count=slots.Length;
		AllholesInCurEquipment=new List<GemHoleViewInfo>();
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
				AllholesInCurEquipment[hole_index].isHaveGem=true;
			}
			else
			{
				AllholesInCurEquipment[hole_index].SetGem(null);
				AllholesInCurEquipment[hole_index].isHaveGem=false;
			}
		}
		
		
	}
	
    void Click_GemHole(GameObject button)
	{
		if(ButtonClick!=null)
			ButtonClick(button);
	}
	
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
		
	
	/*在某个孔里镶嵌宝石
	 * */
	public void SetHoleGem(int Solt_Index,string GemSpriteName)
	{
		if(Solt_Index>=0 && Solt_Index<all_holes.Count)
		{
			all_holes[Solt_Index].SetGem(GemSpriteName);

			//如果洞洞镶嵌宝石，激活了隐藏属性
			//需要判断是否激活隐藏属性
			if(FrogingSystem.ForgingRoomUI.Instance.InlaySystem.IsEquipHasEffect())
			{
				for(int i = 0; i < arryParticle.Length; i++)
				{
					arryParticle[i].Play();
				}
			}
		}
	}
	
	/*获得当前孔是第几个孔 -1说明该孔不存在
	 * */
	public int GetHoleId(string HoleName)
	{
		int index=-1;
		for(int i=0;i<AllholesInCurEquipment.Count;i++)
		{
			if(AllholesInCurEquipment[i].gameObject.name==HoleName)
			{
				index=i;
				break;
			}
		}
		return index;
	}

	
	/*清空显示面板
	 * */
	public void ClearGemInlayViewPanel()
	{
		foreach(GemHoleViewInfo gemview in all_holes)
		{
			gemview.SetGem(null);
			gemview.isHaveGem=false;
			gemview.gameObject.SetActive(false);
		}
		SetSelectedEquipment(null);
		ClearEquipInfo();
	}
	
	public void ClearEquipInfo()
	{
		Lab_EquipmentName.text = "";
		Lab_EquipmentAttrib.text ="[ffe265]"+TEXT(9053)+"[-]";
		Lab_EquipmentAttact.text ="[ffe265]"+TEXT(5103)+":[-]";
		Lab_EquipmentDef.text ="[ffe265]"+TEXT(5104)+":[-]";
		Lab_SkillTrigger.text ="[ffe265]"+TEXT(9009)+":[-]";

		Lab_EquipEffect.text = "[2e9fff]"+ TEXT(9016)+":[-]" ;

	}
	
	
	
	public string  GetEquipEffectDes(Equipment eqi)
	{
	    int[] effect = eqi.ConfigEquip.effect;
		string result = "";
		if(effect.Length>0)
		{
			for(int i=0;i<effect.Length;i++)
			{
				if(effect[i]>0)
				{
					switch(i)
					{
					case 0:
						result = TEXT(9016) +":   "+TEXT(9010)+effect[i];
						break;
					case 1:
						result = TEXT(9016) +":   "+TEXT(9011)+effect[i];
						break;
					case 2:
						result = TEXT(9016) +":   "+TEXT(9009)+effect[i]+"%";
						break;
					} 
				}
			}
		}
		return result;
	}
	
	
	
	public void ShowEquipmentInfo(Equipment eqi)
	{
		
		Lab_EquipmentName.text = "[ffe265]"+eqi.name+"[-]";
		Lab_EquipmentAttrib.text ="[ffe265]"+TEXT(9053)+"[-]";
		
		int addAttact = 0;
		int addDef = 0;
		float addSkillTrigger = 0;
		foreach (EquipSlot s in eqi.RtEquip.slot)
		{
			if(s.mGem != null)
			{
				addAttact += s.mGem.configData.atk;
				addDef += s.mGem.configData.def;
				addSkillTrigger += s.mGem.configData.skillEffect;
			}
		}
		
		Lab_EquipmentAttact.text ="[ffe265]"+TEXT(5103)+":[-]      "+ eqi.ConfigEquip.atk.ToString() +"+  [8cff79]"+addAttact.ToString()+"[-]";
		Lab_EquipmentDef.text ="[ffe265]"+TEXT(5104)+":[-]      " + eqi.ConfigEquip.def.ToString() +"+  [8cff79]"+addDef.ToString()+"[-]";
		//Debug.LogError("ShowEquipmentInfo");
		Lab_SkillTrigger.text ="[ffe265]"+TEXT(9009)+":[-]   0"  +"+  [8cff79]"+addSkillTrigger.ToString()+"%[-]";
		
		bool isHave = Core.Data.EquipManager.isHaveEffect(eqi);
		Lab_EquipEffect.text =isHave ? "[2e9fff]"+ GetEquipEffectDes(eqi)+"[-]  [00ff00]("+TEXT(9054)+")[-]"      :        "[2e9fff]"+GetEquipEffectDes(eqi)+"[-]  [ff0000]("+TEXT(9055)+")[-]";
	}
	

}
