using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RUIType;

public class JCEquipmentDesInfoUI : RUIMonoBehaviour {
	/// <summary>
	/// 装备名字
	/// </summary>
	public UILabel EquipNameLabel;
	/// <summary>
	/// 装备描述
	/// </summary>
	public UILabel DesLabel;
	/// <summary>
	/// 界面控制根节点
	/// </summary>
	public GameObject PanelRoot;
	/// <summary>
	/// 装备的图标
	/// </summary>
	public UISprite EquipmentICON;
	public StarsUI EquipmentStars;
	
	public List<GemRecastingHoleInfo> GemHole = new List<GemRecastingHoleInfo>();

	/// <summary>
	/// 缘分人物名称
	/// </summary>
	public UILabel[] FateNamelabelArray;
	public UISprite[] m_arryMonsterHead;
	public StarsUI[] m_arryStars;

	public UILabel Lab_Atk;
	public UILabel Lab_Def;
	public UILabel Lab_EquipEffect;
	
	private Equipment mEquipData;
	private bool m_bShowChange;
	private static JCEquipmentDesInfoUI _mInstance;
	public static JCEquipmentDesInfoUI mInstance
	{
		get
		{
			return _mInstance;
		}
	}


	void Awake()
	{
		_mInstance = this;
		this.transform.parent = DBUIController.mDBUIInstance._TopRoot.transform;
	}


	/// <summary>
	/// 关闭按钮
	/// </summary>
	public void OnXBtnClick()
	{
		TweenScale tween = PanelRoot.GetComponent<TweenScale>();
		tween.delay = 0;
		tween.duration = 0.25f;
		tween.from =  Vector3.one;
		tween.to = new Vector3(0.01f,0.01f,0.01f);
		tween.onFinished.Clear();
		tween.onFinished.Add(new EventDelegate(this,"DestroyPanel"));
//		tween.Reset();
		tween.ResetToBeginning ();
		tween.PlayForward();
	}
	/// <summary>
	/// Destroies the panel.
	/// </summary>
	void DestroyPanel()
	{
		Destroy(gameObject);
	}


	/// <summary>
	/// 初始化界面信息
	/// </summary>
	public void InitPanel(Equipment o)
	{
		mEquipData = o;
		EquipNameLabel.text = o.name;
		DesLabel.text = o.ConfigEquip.description;

		EquipmentICON.spriteName = mEquipData.ConfigEquip.ID.ToString ();
		EquipmentStars.SetStar(mEquipData.ConfigEquip.star);
			
		//RED.SetActive(mEquipData.equipped, m_spEquiped.gameObject);

		List<MonsterData> monsterlist = Core.Data.fateManager.getMonsterFateByEquipNum (Core.Data.monManager, o.ConfigEquip.ID);
		Debug.Log ("o.ConfigEquip.description"+monsterlist.Count);
		int count = monsterlist.Count;
		if(count > 4)
		{
			count = 4;
		}
		int i = 0;

		//  need to change
		for (; i < count; i++) {
			RED.SetActive(true, m_arryMonsterHead[i].transform.parent.gameObject);
			FateNamelabelArray [i].text = monsterlist [i].name; 
			AtlasMgr.mInstance.SetHeadSprite(m_arryMonsterHead[i],monsterlist[i].ID.ToString());
		    m_arryStars[i].SetStar(monsterlist[i].star);
		}

		for (; i < 4; i++) {
			RED.SetActive(false, m_arryMonsterHead[i].transform.parent.gameObject);
		}
		
		
		/*宝石孔和宝石
		 * */
		if(o.RtEquip != null)
		{
			i =0;
			foreach(EquipSlot  slot in  o.RtEquip.slot)
			{
				if(!GemHole[i].gameObject.activeSelf)
						GemHole[i].gameObject.SetActive(true);
				
				GemHole[i].SetHoleColor(slot.color);
                if (slot.mGem != null)
                {
                    GemHole[i].SetGem(slot.mGem.configData.anime2D);
					GemHole[i].SetLv(slot.mGem.configData.level);
                }
                else
                {
                    GemHole[i].SetLv(0);
                }
				i++;
			}
		}
		else
		{
			//直接读的配表
			int mosaic = o.ConfigEquip.Mosaic;
			Debug.Log("mosaic="+mosaic.ToString());
				
				
			for(int k=0;k<GemHole.Count;k++)
			{
				if(k < mosaic)
				{
				     if(!GemHole[k].gameObject.activeSelf)
						GemHole[k].gameObject.SetActive(true);
					    GemHole[k].SetSlotUnkown();
				}
				else
				{
					if(GemHole[k].gameObject.activeSelf)
						GemHole[k].gameObject.SetActive(false);
				}
			}
		}
		
		/*攻击力和防御力
		 * */	
		Lab_Atk.text = o.ConfigEquip.atk.ToString();
		Lab_Def.text = o.ConfigEquip.def.ToString();
		
		/*显示装备隐藏属性
	     * */
		
		bool isHave = Core.Data.EquipManager.isHaveEffect(o);
		Lab_EquipEffect.text =isHave ? "[2e9fff]"+ GetEquipEffectDes(o)+"[-]  [00ff00]("+TEXT(9054)+")[-]"      :        "[2e9fff]"+GetEquipEffectDes(o)+"[-]  [ff0000]("+TEXT(9055)+")[-]";
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
	
	public void SetEquipEffect(string text)
	{
		Lab_EquipEffect.text=text;
	}
	
	public string TEXT(int num_text)
	{
		return	Core.Data.stringManager.getString(num_text);
	}
	
	
	
	
	
	
	
	/// <summary>
	/// Showpbs the equipment.
	/// </summary>
	/// <returns>The equipment.</returns>
	/// <param name="root">Root.</param>
	public static JCEquipmentDesInfoUI OpenUI(Equipment o){

		if(mInstance == null)
		{
			GameObject obj = PrefabLoader.loadFromPack("JC/JCEquipmentDesInfoUI")as GameObject ;
			if(obj !=null)
			{
					NGUITools.AddChild(DBUIController.mDBUIInstance._TopRoot,  obj);
			}
		}
		else
		{
			mInstance.SetActive(true);
		}
		mInstance.InitPanel(o);
		return mInstance;
	}

	public override void SetActive(bool bActive)
	{
		NGUITools.SetActive(this.gameObject, bActive);
	}

}
