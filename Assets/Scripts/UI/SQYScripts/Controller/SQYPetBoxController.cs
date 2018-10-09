using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RUIType;

namespace RUIType
{
	public enum EMBoxType
	{
		NONE = 0,
		//查看包袱
		LOOK_Charator,
		//换人物
		CHANGE,
		//出售人物
		SELL_Charator,
		//强化人物
		QiangHua,
		//添加攻击装备
		Equip_ADD_ATK ,
		//添加防守防备
		Equip_ADD_DEF ,
		//换装备
		Equipment_SWAP_ATK ,
		Equipment_SWAP_DEF ,

		//强化atk装备
		Equip_QH_ATK ,
		//强化def装备
		Equip_QH_DEF ,

		//查看包裹的装备
		LOOK_Equipment,
		//查看道具
		LOOK_Props ,
		//查看宝石
		LOOK_Gem ,
		//查看武者碎片
		LOOK_MonFrag,
		//查看武器碎片
		LOOK_AtkFrag,
		//查看防具碎片
		LOOK_DefFrag,
		//卖装备
		SELL_Equiement ,

		//合成真忍者
		HECHENG_ZHENREN_MAIN,
		HECHENG_ZHENREN_SUB ,
		//合成神忍者
		HECHENG_SHENREN_MAIN ,
		HECHENG_SHENREN_SUB ,
		//真忍合神忍
		ZHENREN_HE_SHENREN_MAIN ,
		ZHENREN_HE_SHENREN_SUB ,
		//属性变换
		ATTR_SWAP ,
		//潜力训练
		QIANLI_XUNLIAN ,
		
		//宝石合成
		GEM_HECHENG_MAIN,
		GEM_HECHENG_SUB,

		SELL_GEM,
		//镶嵌界面选择装备
		SELECT_EQUIPMENT_INLAY,
		//镶嵌界面选择宝石
		SELECT_GEM_INLAY,
		//重铸界面选择装备
		SELECT_EQUIPMENT_RECAST,

		//分解宠物
		DECOMPOSE_MONSTER,
		//宠物升级
		EVOLVE_MONSTER,
	}
}
/// <summary>
/// PetBoxCtrl 分为三部分,当前部分主要做一些逻辑,
/// SQYPetBox_Service 主要接受一些IO和其他部分的操作
/// SQYPetBox_View主要就是一个UI的更新
/// </summary>
public partial class SQYPetBoxController : MDBaseViewController
{
	/// <summary>
	/// 当前背包的类型
	/// </summary>
	[HideInInspector]
	public EMBoxType _boxType = EMBoxType.LOOK_Charator;

	/// <summary>
	/// 当前背包的元素类型:人物,装备,道具等
	/// </summary>
	EMItemType _itemType = EMItemType.NONE;

	/// <summary>
	/// 当前选择的所有的背包单元
	/// </summary>
	List<SQYNodeForBI> szSelectCharator = new List<SQYNodeForBI> ();

	//所有的物件
	Dictionary<int, SQYNodeForBI> dicAllBoxItem = new Dictionary<int, SQYNodeForBI>();

	Dictionary<int, SQYLinePetView> dicAllStarLine = new Dictionary<int, SQYLinePetView>();

	/// <summary>
	/// 每一个星级单元的 高度
	/// </summary>
	const float LineHeight = -420f;

	private readonly int[] EXP_MONSTERS = {19999, 19998};
	private readonly int[] EXP_EQUIPS = {40999, 40998, 45999, 45998};

	private float m_scrollVal = 0;

    public StarMove star  ; // yangcg

	public EMBoxType boxType
	{
		get
		{
			return _boxType;
		}
	}


	private static SQYPetBoxController _mInstace;
	public static SQYPetBoxController mInstance
	{
		get 
		{
			return _mInstace;
		}
	}

	private bool m_bShow = false;
	public bool IsShow
	{
		get
		{
			return m_bShow;
		}
	}

	bool bInitView = false;
	void initView ()
	{
		if (bInitView)
			return;
		bInitView = true;
	}

	bool resetAscending(EMBoxType bt)
	{
		switch(bt)
		{
		case EMBoxType.LOOK_Charator:
		case EMBoxType.DECOMPOSE_MONSTER:
		case EMBoxType.EVOLVE_MONSTER:
		case EMBoxType.CHANGE:
		case EMBoxType.Equip_ADD_ATK:
		case EMBoxType.Equip_ADD_DEF:
		case EMBoxType.Equipment_SWAP_ATK:
		case EMBoxType.Equipment_SWAP_DEF:
		case EMBoxType.LOOK_Equipment:
		case EMBoxType.LOOK_Props:
		case EMBoxType.LOOK_Gem:
		case EMBoxType.HECHENG_SHENREN_MAIN:
		case EMBoxType.HECHENG_SHENREN_SUB:
		case EMBoxType.HECHENG_ZHENREN_MAIN:
		case EMBoxType.HECHENG_ZHENREN_SUB:
		case EMBoxType.ZHENREN_HE_SHENREN_MAIN:
		case EMBoxType.ZHENREN_HE_SHENREN_SUB:
		case EMBoxType.ATTR_SWAP:
		case EMBoxType.QIANLI_XUNLIAN:
		case EMBoxType.SELECT_EQUIPMENT_INLAY:
		case EMBoxType.SELECT_EQUIPMENT_RECAST:
		case EMBoxType.SELECT_GEM_INLAY:
		case EMBoxType.LOOK_AtkFrag:
		case EMBoxType.LOOK_DefFrag:
		case EMBoxType.LOOK_MonFrag:
			return false;
		}
		return true;
	}
	EMItemType resetItemType(EMBoxType bt)
	{
		EMItemType iType = EMItemType.NONE;
		switch (bt) 
		{
		case EMBoxType.LOOK_Charator:
		case EMBoxType.DECOMPOSE_MONSTER:
		case EMBoxType.EVOLVE_MONSTER:
		case EMBoxType.CHANGE:
		case EMBoxType.SELL_Charator:
		case EMBoxType.QiangHua:
		case EMBoxType.HECHENG_ZHENREN_MAIN:
		case EMBoxType.HECHENG_ZHENREN_SUB:
		case EMBoxType.ZHENREN_HE_SHENREN_MAIN:
		case EMBoxType.ZHENREN_HE_SHENREN_SUB:
		case EMBoxType.HECHENG_SHENREN_SUB:
		case EMBoxType.HECHENG_SHENREN_MAIN:
		case EMBoxType.ATTR_SWAP:
		case EMBoxType.QIANLI_XUNLIAN:
			iType = EMItemType.Charator;
			break;

		case EMBoxType.LOOK_Equipment:
		case EMBoxType.Equip_ADD_ATK:
		case EMBoxType.Equip_ADD_DEF:
		case EMBoxType.SELL_Equiement:
		case EMBoxType.Equip_QH_ATK:
		case EMBoxType.Equip_QH_DEF:
		case EMBoxType.Equipment_SWAP_ATK:
		case EMBoxType.Equipment_SWAP_DEF:
		case EMBoxType.SELECT_EQUIPMENT_INLAY:
		case EMBoxType.SELECT_EQUIPMENT_RECAST:
			iType = EMItemType.Equipment;
			break;

		case EMBoxType.LOOK_Props:
			iType = EMItemType.Props;
			break;

		case EMBoxType.LOOK_Gem:
		case EMBoxType.SELL_GEM:
		case EMBoxType.GEM_HECHENG_MAIN:
		case EMBoxType.GEM_HECHENG_SUB:
		case EMBoxType.SELECT_GEM_INLAY:
			iType = EMItemType.Gem;
			break;
		case EMBoxType.LOOK_AtkFrag:
			iType = EMItemType.AtkFrag;
			break;
		case EMBoxType.LOOK_DefFrag:
			iType = EMItemType.DefFrag;
			break;
		case EMBoxType.LOOK_MonFrag:
			iType = EMItemType.MonFrag;
			break;
		}
		return iType;
	}
	SplitType resetSplitType(EMBoxType bt)
	{
		SplitType st = SplitType.None;
		switch(bt)
		{
		case EMBoxType.QiangHua:
		case EMBoxType.Equip_QH_ATK:
		case EMBoxType.Equip_QH_DEF:
		case EMBoxType.SELL_Equiement:
		case EMBoxType.SELL_Charator:
		case EMBoxType.SELL_GEM:
		case EMBoxType.GEM_HECHENG_MAIN:
		case EMBoxType.GEM_HECHENG_SUB:
		case EMBoxType.SELECT_GEM_INLAY:
		case EMBoxType.DECOMPOSE_MONSTER:
			st = SplitType.Split_If_InTeam;
			break;
		case EMBoxType.CHANGE:
		case EMBoxType.Equip_ADD_ATK:
		case EMBoxType.Equip_ADD_DEF:
		case EMBoxType.Equipment_SWAP_ATK:
		case EMBoxType.Equipment_SWAP_DEF:
			st = SplitType.Split_If_InCurTeam;
			break;
		}
		return st;
	}
	List<object> allStarData(short star)
	{
		List<object> aline = new List<object>();

		SplitType stype = resetSplitType(_boxType);

		switch(_itemType)
		{
		case EMItemType.Charator:
			{
				List<Monster> szMonsters = null;
				if (_boxType == EMBoxType.HECHENG_ZHENREN_MAIN)
				{
					szMonsters = Core.Data.monManager.GetHeChengMonByStage (star, RuntimeMonster.NORMAL_MONSTER);	
				}
				else if (_boxType == EMBoxType.HECHENG_ZHENREN_SUB)
				{
					szMonsters = Core.Data.monManager.GetZhenRenHeChSubMon (star);
				}
				else if (_boxType == EMBoxType.ZHENREN_HE_SHENREN_MAIN)
				{
					szMonsters = Core.Data.monManager.GetHeChengMonByStage (star, RuntimeMonster.ZHEN_MONSTER);
				}
				else if (_boxType == EMBoxType.ZHENREN_HE_SHENREN_SUB)
				{
					szMonsters = Core.Data.monManager.GetZhenRenHeShenRenSub (star);
				}
				else if (_boxType == EMBoxType.HECHENG_SHENREN_MAIN)
				{
					szMonsters = Core.Data.monManager.GetHeChengMonByStage (star, RuntimeMonster.NORMAL_MONSTER);
				}
				else if (_boxType == EMBoxType.HECHENG_SHENREN_SUB)
				{
					int index = TrainingRoomUI.mInstance.m_hechengUI.m_nTempIndex;
					Monster mon = TrainingRoomUI.mInstance.m_hechengUI.m_subData[index];
					if( mon == null)
					{
						szMonsters = Core.Data.monManager.GetShenRenHeChSubMon (star);
					}
					else
					{
						szMonsters = Core.Data.monManager.GetHechengMon(star, mon.num, mon.RTData.Attribute);
						if(szMonsters != null && szMonsters.Count > 0)
						{
							for(int m = 0; m < szMonsters.Count; m++)
							{
								if(szMonsters[m].pid == mon.pid)
									szMonsters.Remove(szMonsters[m]);
							}
						}
					}
				}
				else if (_boxType == EMBoxType.ATTR_SWAP)
				{
					szMonsters = Core.Data.monManager.GetMonListByStage (star, RuntimeMonster.NORMAL_MONSTER);
					szMonsters.AddRange (Core.Data.monManager.GetMonListByStage (star, RuntimeMonster.ZHEN_MONSTER));
					szMonsters = Core.Data.monManager.SortMonList (szMonsters);
				}
				else if (_boxType == EMBoxType.QIANLI_XUNLIAN)
				{
					List<Monster> tempList = Core.Data.monManager.getMonsterListByStar (star);
					szMonsters = new List<Monster>();
					foreach(Monster mon in tempList)
					{
						if(!Core.Data.monManager.IsExpMon(mon.num))
						{
							szMonsters.Add(mon);
						}
					}
				}
				else if(_boxType == EMBoxType.DECOMPOSE_MONSTER)
				{
					if(star >= 3)
					{
						szMonsters = new List<Monster>();
						List<Monster> decompList = Core.Data.monManager.getMonsterListByStar (star, stype);
						for (int i = 0; i < decompList.Count; i++)
						{
							if (!Core.Data.monManager.IsExpMon (decompList [i].num))
							{
								szMonsters.Add (decompList [i]);
							}
						}
					}
				}
				else if(_boxType == EMBoxType.EVOLVE_MONSTER)
				{
					szMonsters = Core.Data.monManager.getEvolveMonListByStar (star);
				}
				else if(_boxType == EMBoxType.SELL_Charator)
				{
					if(star <= 4)
					{
						szMonsters = Core.Data.monManager.getMonsterListByStar (star, stype);
					}
				}
				else if(_boxType == EMBoxType.CHANGE)
				{
					if(Core.Data.guideManger.isGuiding && ZQReceiver.m_curGuide == EventTypeDefine.Click_AddRole3)
					{
						if(star <= 3)
						{
							szMonsters = Core.Data.monManager.getMonsterListByStar (star, stype);
						}
					}
					else
					{
						szMonsters = Core.Data.monManager.getMonsterListByStar (star, stype);
					}
				}
				else
				{
					szMonsters = Core.Data.monManager.getMonsterListByStar (star, stype);
				}

				if(_boxType == EMBoxType.QiangHua)
				{
					if(szMonsters.Contains(SQYUIManager.getInstance().opMonster))
					{
						szMonsters.Remove(SQYUIManager.getInstance().opMonster);
					}
					szMonsters = SortExpMonsters(szMonsters);
				}
				
				if(szMonsters != null)
					aline.AddRange(szMonsters.ToArray());
			}
			break;
		case EMItemType.Equipment:
			{
				List<Equipment> szEquipments = null;
				if (_boxType == EMBoxType.Equip_ADD_ATK || _boxType == EMBoxType.Equipment_SWAP_ATK)
				{
					szEquipments = Core.Data.EquipManager.getAtkEquipByStar (star, stype);
				}
				else if (_boxType == EMBoxType.Equipment_SWAP_DEF)
				{
					szEquipments = Core.Data.EquipManager.getDefEquipByStar (star, stype);
				}
				else if(_boxType == EMBoxType.Equip_ADD_DEF)
				{
					if(Core.Data.guideManger.isGuiding && ZQReceiver.m_curGuide == EventTypeDefine.Click_FangJU)
					{
						if(star <= 3)
						{
							szEquipments = Core.Data.EquipManager.getDefEquipByStar (star, stype);
						}
					}
					else
					{
						szEquipments = Core.Data.EquipManager.getDefEquipByStar (star, stype);
					}
				}
				else if (_boxType == EMBoxType.SELECT_EQUIPMENT_RECAST || _boxType == EMBoxType.SELECT_EQUIPMENT_INLAY)
				{
					szEquipments = Core.Data.EquipManager.getEquipListByStar (star, stype , true);
				}
				else if(_boxType == EMBoxType.Equip_QH_ATK)
				{
					szEquipments = Core.Data.EquipManager.GetEquipByStar(star, 0, stype);
				}
				else if(_boxType == EMBoxType.Equip_QH_DEF)
				{
					szEquipments = Core.Data.EquipManager.GetEquipByStar(star, 1, stype);
				}
				else if(_boxType == EMBoxType.SELL_Equiement)
				{
					szEquipments = Core.Data.EquipManager.GetEquipByStar(star, -1, stype);
				}
				else
				{
					szEquipments = Core.Data.EquipManager.getEquipListByStar (star, stype);
				}

				if(_boxType == EMBoxType.Equip_QH_ATK || _boxType == EMBoxType.Equip_QH_DEF)
				{
					if(szEquipments.Contains(SQYUIManager.getInstance().targetEquip))
					{
						szEquipments.Remove(SQYUIManager.getInstance().targetEquip);
					}
					szEquipments = SortExpEquips(szEquipments);
				}

				if(szEquipments != null && szEquipments.Count > 0)
				{
					aline.AddRange(szEquipments.ToArray());
				}
			}
			break;
		case EMItemType.Props:
			{
				List<Item> szItems = Core.Data.itemManager.GetItemByStar(star);
				aline.AddRange(szItems.ToArray());
			}
			break;
		case EMItemType.Gem:
			{
//			    if(_boxType == EMBoxType.SELECT_GEM_INLAY)
//				{
//				    List<Gems> szGems = Core.Data.gemsManager.GetGemsByStarAndAllLastSelected(star);
//					aline.AddRange (szGems.ToArray ());
//				}
//			    else
				if(_boxType == EMBoxType.GEM_HECHENG_SUB)
			    {
					List<Gems> szGems = Core.Data.gemsManager.GetGemByFirstSelectedAndStar(star);
					aline.AddRange (szGems.ToArray ());
			    }
			    else if(_boxType == EMBoxType.GEM_HECHENG_MAIN)
				{
					List<Gems> szGems = Core.Data.gemsManager.GetGemsByStar(star, stype,10);
					aline.AddRange (szGems.ToArray ());
				}
				else if(_boxType == EMBoxType.SELL_GEM)
				{
					if(star <= 3)
					{
						List<Gems> szGems = Core.Data.gemsManager.GetGemsByStar(star, stype);
						aline.AddRange (szGems.ToArray ());
					}
				}
				else
				{
					List<Gems> szGems = Core.Data.gemsManager.GetGemsByStar(star, stype);
					aline.AddRange (szGems.ToArray ());
				}
			}
			break;
		case EMItemType.AtkFrag:
			{
				List<Soul> monFrag = Core.Data.soulManager.GetAtkFragByStar (star);
				aline.AddRange(monFrag.ToArray());
			}
			break;
		case EMItemType.DefFrag:
			{
				List<Soul> monFrag = Core.Data.soulManager.GetDefFragByStar (star);
				aline.AddRange(monFrag.ToArray());
			}
			break;
		case EMItemType.MonFrag:
			{
				List<Soul> monFrag = Core.Data.soulManager.GetMonFragByStar (star);
				aline.AddRange(monFrag.ToArray());
			}
			break;
		}
		return aline;
	}
	
	private List<Monster> SortExpMonsters(List<Monster> srcList)
	{
		if(srcList == null || srcList.Count == 0)
		{
			return srcList;
		}

		List<Monster> list = new List<Monster>();
		List<int> expList = new List<int>();
		expList.AddRange(EXP_MONSTERS);
		for(int i = 0; i < srcList.Count; i++)
		{
			if(expList.Contains(srcList[i].num))
			{
				list.Add(srcList[i]);
			}
		}

		for(int i = 0; i < srcList.Count; i++)
		{
			if(!expList.Contains(srcList[i].num))
			{
				list.Add(srcList[i]);
			}
		}

		return list;
	}

	private List<Equipment> SortExpEquips(List<Equipment> srcList)
	{
		if(srcList == null || srcList.Count == 0)
		{
			return srcList;
		}
		
		List<Equipment> list = new List<Equipment>();
		List<int> expList = new List<int>();
		expList.AddRange(EXP_EQUIPS);
		for(int i = 0; i < srcList.Count; i++)
		{
			if(expList.Contains(srcList[i].Num))
			{
				list.Add(srcList[i]);
			}
		}
		
		for(int i = 0; i < srcList.Count; i++)
		{
			if(!expList.Contains(srcList[i].Num))
			{
				list.Add(srcList[i]);
			}
		}
		
		return list;		
	}



	void ClearItemNewState(EMItemType itemType)
	{
		switch (itemType)
		{
			case EMItemType.Charator:
				List<Monster> monList = Core.Data.monManager.GetNewMonList ();
				if (monList != null && monList.Count > 0)
				{
					m_bNeedSave = true;
					foreach (Monster mon in monList)
					{
						BagOfStatus status = new BagOfStatus ();
						status.status = BagOfStatus.STATUS_NORMAL;

						mon.isNew = false;
						status.pid = mon.pid;
						status.num = mon.num;
						Core.Data.AccountMgr.setStatus (status);
					}
				}
				break;
			case EMItemType.Equipment:
				List<Equipment> equipList = Core.Data.EquipManager.GetNewEquip ();
				if (equipList != null && equipList.Count > 0)
				{
					m_bNeedSave = true;
					foreach (Equipment equip in equipList)
					{
						BagOfStatus status = new BagOfStatus ();
						status.status = BagOfStatus.STATUS_NORMAL;

						equip.isNew = false;
						status.pid = equip.RtEquip.id;
						status.num = equip.RtEquip.num;
						Core.Data.AccountMgr.setStatus (status);
					}
				}
				break;
			case EMItemType.Gem:
				List<Gems> gemList = Core.Data.gemsManager.GetNewGem ();
				if (gemList != null && gemList.Count > 0)
				{
					m_bNeedSave = true;
					foreach (Gems gem in gemList)
					{
						BagOfStatus status = new BagOfStatus ();
						status.status = BagOfStatus.STATUS_NORMAL;

						gem.isNew = false;
						status.pid = gem.id;
						status.num = gem.configData.ID;
						Core.Data.AccountMgr.setStatus (status);
					}
				}
				break;
			case EMItemType.Props:
				List<Item> itemList = Core.Data.itemManager.GetNewItem ();
				if (itemList != null && itemList.Count > 0)
				{
					m_bNeedSave = true;
					foreach (Item item in itemList)
					{
						BagOfStatus status = new BagOfStatus ();
						status.status = BagOfStatus.STATUS_NORMAL;

						item.isNew = false;
						status.pid = item.RtData.id;
						status.num = item.configData.ID;
						Core.Data.AccountMgr.setStatus (status);
					}
				}
				break;
//			case EMItemType.Soul:
//				List<Soul> soulList = Core.Data.soulManager.GetNewSoul ();
//				if (soulList != null && soulList.Count > 0)
//				{
//					m_bNeedSave = true;
//					foreach (Soul soul in soulList)
//					{
//						BagOfStatus status = new BagOfStatus ();
//						status.status = BagOfStatus.STATUS_NORMAL;
//
//						soul.isNew = false;
//						status.pid = soul.m_RTData.id;
//						status.num = soul.m_config.ID;
//						Core.Data.AccountMgr.setStatus (status);
//					}
//				}
//				break;
		}

		if (m_bNeedSave)
		{
			Core.Data.AccountMgr.save ();
			UpdateBtnTips ();
			SQYMainController.mInstance.UpdateBagTip ();
			m_bNeedSave = false;
		}
	}
		
	public void SetPetBoxType (EMBoxType bt)
	{
		if (bt != _boxType)
		{
			ClearItemNewState (resetItemType (_boxType));
		}

		m_bagItemOprtUI.SetShow (false);
		UpdateBtnTips ();
		while(sv_BagPackage.transform.childCount > 0)
		{
			Transform tf = sv_BagPackage.transform.GetChild(0);
			tf.parent = null;
			Destroy(tf.gameObject);
		}

		dicAllBoxItem.Clear ();
		dicAllStarLine.Clear ();

		_boxType = bt;
		RED.SetActive (false, go_CoinPanel, m_objDecompose);
		szSelectCharator.Clear ();

		_itemType = resetItemType(_boxType);//设置背包元素的类型
		freshViewWithBoxType();//根据当前的背包类型 刷新UI
		bool bAscending = resetAscending(_boxType);//是否升序(1-5)

		SplitType stype = resetSplitType(_boxType);//根据背包类型 设置筛选类型

		float tempy = 0;
		int idx = 0;

		for (int i = 1; i <7; i++) 
		{//便利所以的星级
			if (bAscending)
				idx = i;
			else
				idx = 7 - i;
			
			List<object> aline = allStarData((short)idx);//取得当前背包元素类型的所有数据
		
			if (aline.Count > 0) 
			{
				SQYLinePetView lpv = null;
				lpv = SQYLinePetView.CreateLinePetView ();
				lpv.name = "star" + idx.ToString();

				lpv.ACT_SelectOneCharator += this.selectOneCharator;
				lpv.ACT_SelectMoreCharators += this.selectMoreCharator;
				lpv.GetComponent<UIDragScrollView> ().scrollView = sv_BagPackage;
	
				//关键在这个函数里面
				lpv.freshLinePetView (aline, idx, _boxType, stype,_itemType);//根据各种类型 刷新当前星级View的UI

				RED.AddChild (lpv.gameObject, sv_BagPackage.gameObject);//加到ScrollView中

				for(int n = 0; n < lpv.szCurNodeBI.Count; n++)
				{
					dicAllBoxItem.Add(lpv.szCurNodeBI[n]._boxItem.pid,  lpv.szCurNodeBI[n]);
				}

				dicAllStarLine.Add (idx, lpv);
				
				Vector3 v3 = lpv.transform.localPosition;
				v3.y = tempy;
				v3.x = 0;
				lpv.transform.localPosition = v3;//设位置

				if (lpv.MoreThanHalf) { // 重新计算下次的位置
					tempy += LineHeight;
				} else {
					tempy += LineHeight / 2 - 50;
				}
			} 
		}
		sv_BagPackage.ResetPosition();//scrollview 重新归位

		if(_boxType == EMBoxType.QiangHua)
		{
			CheckStrengthMon();
		}
		else if(_boxType == EMBoxType.Equip_QH_ATK)
		{
			CheckStrengthEquip(0);
		}
		else if(_boxType == EMBoxType.Equip_QH_DEF)
		{
			CheckStrengthEquip(1);
		}
	}


	void CheckStrengthMon()
	{
		List<Monster> list = new List<Monster>();
		for(short i = 1; i <= 5; i++)
		{
			List<Monster> tempList = Core.Data.monManager.getMonsterListByStar(i, SplitType.Split_If_InTeam);
			list.AddRange(tempList.ToArray());
		}
		if(list.Count == 0)
		{
			OnBtnShop();
		}
	}

	void CheckStrengthEquip(int type)
	{
		List<Equipment> list = new List<Equipment>();
		for(short i = 1; i <= 5; i++)
		{
			List<Equipment> tempList = null;
			if(type == 0)
			{
				tempList = Core.Data.EquipManager.getAtkEquipByStar(i, SplitType.Split_If_InTeam);
			}
			else if(type == 1)
			{
				tempList = Core.Data.EquipManager.getDefEquipByStar(i, SplitType.Split_If_InTeam);
			}
			 
			list.AddRange(tempList.ToArray());
		}
		if(list.Count == 0)
		{
			OnBtnShop();
		}
	}
	
	void SendSellMonsterMsg()
	{
		List<int> charNode = new List<int>();
		foreach(SQYNodeForBI node in szSelectCharator)
		{
			Monster monster = node._boxItem.curData as Monster;
			charNode.Add(monster.pid);
		}

		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.SELL_MONSTER, new  SellMonsterParam(Core.Data.playerManager.PlayerID, charNode.ToArray()));

		task.ErrorOccured += testHttpResp_Error;
		task.afterCompleted += testHttpResp_UI;

		task.DispatchToRealHandler();

		ComLoading.Open ();
	}
	public void SellCharator ()
	{
		if(szSelectCharator.Count == 0)
		{
			return;
		}

		int star = 0;
		foreach(SQYNodeForBI node in szSelectCharator)
		{
			Monster monster = node._boxItem.curData as Monster;
			if (monster.Star > star)
			{
				star = monster.Star;
			}
		}

		if (star >= 3)
		{
			string strContent = Core.Data.stringManager.getString (5154);
			strContent = string.Format (strContent, Core.Data.stringManager.getString(5013), star,Core.Data.stringManager.getString(6), Core.Data.stringManager.getString(5013), Core.Data.stringManager.getString(5008));
			UIInformation.GetInstance ().SetInformation (strContent, Core.Data.stringManager.getString(5030), SendSellMonsterMsg, null);
		}
		else
		{
			SendSellMonsterMsg ();
		}
	}


	public void DecomposeMonster()
	{
		string strText = "";
		int costCoin = GetDecomposeCoin ();

		strText = Core.Data.stringManager.getString (5164);
		strText = string.Format(strText, costCoin, GetBattleSoulCnt(), Core.Data.stringManager.getString(5166));

		UIInformation.GetInstance().SetInformation(strText, Core.Data.stringManager.getString(5030), SendDecomposeMonMsg);
	}

	void SendDecomposeMonMsg()
	{
		if(szSelectCharator.Count == 0)
		{
			return;
		}

		if (GetDecomposeCoin () > Core.Data.playerManager.RTData.curCoin)
		{
            JCRestoreEnergyMsg.OpenUI(ItemManager.COIN_PACKAGE,ItemManager.COIN_BOX,2);

			//SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(35000));
			return;
		}

		List<int> charNode = new List<int>();
		foreach(SQYNodeForBI node in szSelectCharator)
		{
			Monster monster = node._boxItem.curData as Monster;
			charNode.Add(monster.pid);
		}
		
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.DECOMPOSE_MONSTER, new  DecomposelMonsterParam(Core.Data.playerManager.PlayerID, charNode.ToArray()));
		
		task.ErrorOccured += testHttpResp_Error;
		task.afterCompleted += testHttpResp_UI;
		
		task.DispatchToRealHandler();
		
		ComLoading.Open ();
	}


	void SendSellEquipMsg()
	{
		List<int> charNode = new List<int>();
		foreach(SQYNodeForBI node in szSelectCharator)
		{
			Equipment equip = node._boxItem.curData as Equipment;
			charNode.Add(equip.RtEquip.id);
		}

		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.SELL_EQUIPMENT, new  SellEquipParam(Core.Data.playerManager.PlayerID, charNode.ToArray()));

		task.ErrorOccured += testHttpResp_Error;
		task.afterCompleted += testHttpResp_UI;

		task.DispatchToRealHandler();

		ComLoading.Open ();
	}

	public void SellEquipment()
	{
		if(szSelectCharator.Count == 0)
		{
			return;
		}

		int star = 0;
		foreach(SQYNodeForBI node in szSelectCharator)
		{
			Equipment equip = node._boxItem.curData as Equipment;
			if (equip.ConfigEquip.star > star)
			{
				star = equip.ConfigEquip.star;
			}
		}

		if (star >= 3)
		{
			string strContent = Core.Data.stringManager.getString (5154);
			strContent = string.Format (strContent, Core.Data.stringManager.getString(5014), star,Core.Data.stringManager.getString(6), Core.Data.stringManager.getString(5014), Core.Data.stringManager.getString(5008));
			UIInformation.GetInstance ().SetInformation (strContent, Core.Data.stringManager.getString(5030), SendSellEquipMsg, null);
		}
		else
		{
			SendSellEquipMsg ();
		}

	}


	public void SellGem()
	{
		if(szSelectCharator.Count == 0)
		{
			return;
		}
		
		List<int> charNode = new List<int>();
		foreach(SQYNodeForBI node in szSelectCharator)
		{
			Gems gem = node._boxItem.curData as Gems;
			charNode.Add(gem.id);
		}
		
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.SELL_GEM, new  SellEquipParam(Core.Data.playerManager.PlayerID, charNode.ToArray()));
		
		task.ErrorOccured += testHttpResp_Error;
		task.afterCompleted += testHttpResp_UI;

		task.DispatchToRealHandler();

		ComLoading.Open ();
	}

	void QiangHua ()
	{
		if(szSelectCharator.Count == 0)
		{
			return;
		}

		if(Core.Data.playerManager.RTData.curCoin < GetStrengthCoin())
		{
            JCRestoreEnergyMsg.OpenUI(ItemManager.COIN_PACKAGE,ItemManager.COIN_BOX,2);

			//SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(35000));
			return;
		}

		int star = 0;
		foreach(SQYNodeForBI node in szSelectCharator)
		{
			Monster monster = node._boxItem.curData as Monster;
			if (monster.Star > star && monster.num != 19999)
			{
				star = monster.Star;
			}
		}

		if (star >= 3)
		{
			string strContent = Core.Data.stringManager.getString (5154);
			strContent = string.Format (strContent, Core.Data.stringManager.getString(5013), star,Core.Data.stringManager.getString(6), Core.Data.stringManager.getString(5013), Core.Data.stringManager.getString(5009));
			UIInformation.GetInstance ().SetInformation (strContent, Core.Data.stringManager.getString(5030), SendStrengthMonMsg, null);
		}
		else
		{
			SendStrengthMonMsg ();
		}
	}


	void SendStrengthMonMsg()
	{
		List<Monster> charNode = new List<Monster>();
		List<int> listMon = new List<int>();
		foreach(SQYNodeForBI node in szSelectCharator)
		{
			Monster monster = node._boxItem.curData as Monster;
			charNode.Add(monster);
		}

		int count = charNode.Count;
		if(SQYUIManager.getInstance().opMonster.RTData.curLevel == 59)
		{
			int totalExp = 0;
			int finalLv = 0;

			for(int i = 0; i < charNode.Count; i++)
			{
				totalExp += GetStrengthMonExp (charNode[i]);
				finalLv = GetStrengthMonFinalLv (totalExp);
				
				if(finalLv == 60)
				{
					count = i + 1;
					break;
				}
			}
		}

		for(int i = 0; i < count; i++)
		{
			listMon.Add(charNode[i].pid);
		}


		//记录合成前  宠物信息
		Core.Data.temper.preMonsterData = new Monster();
		Core.Data.temper.preMonsterData.config = SQYUIManager.getInstance().opMonster.config;
		Core.Data.temper.preMonsterData.num = SQYUIManager.getInstance().opMonster.num;
		Core.Data.temper.preMonsterData.pid = SQYUIManager.getInstance().opMonster.pid;

		Core.Data.temper.preMonsterData.RTData = new RuntimeMonster();
		Core.Data.temper.preMonsterData.RTData.addStar = SQYUIManager.getInstance().opMonster.RTData.addStar;

		Core.Data.temper.preMonsterData.BTData = new BattleMonster(SQYUIManager.getInstance().opMonster.baseAttack,SQYUIManager.getInstance().opMonster.enhanceAttack,SQYUIManager.getInstance().opMonster.baseDefend,SQYUIManager.getInstance().opMonster.enhanceDefend);
		Core.Data.temper.preMonsterData.RTData.curLevel = SQYUIManager.getInstance().opMonster.RTData.curLevel;
		Core.Data.temper.preMonsterData.RTData.curExp = SQYUIManager.getInstance().opMonster.RTData.curExp;
		Core.Data.temper.preMonsterData.InitConfig();

		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.STRENGTHEN_MONSTER, new StrengthenParam(Core.Data.playerManager.PlayerID, SQYUIManager.getInstance().opMonster.pid, listMon.ToArray()));

		task.ErrorOccured += testHttpResp_Error;
		task.afterCompleted += testHttpResp_UI;

		task.DispatchToRealHandler();
		ComLoading.Open ();
	}

	
	void StrengthEquip ()
	{
		if(szSelectCharator.Count == 0)
		{
			return;
		}

		if(Core.Data.playerManager.RTData.curCoin < GetStrengthCoin())
		{
            JCRestoreEnergyMsg.OpenUI(ItemManager.COIN_PACKAGE,ItemManager.COIN_BOX,2);

			//SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(35000));
			return;
		}
		
		int star = 0;
		foreach(SQYNodeForBI node in szSelectCharator)
		{
			Equipment equip = node._boxItem.curData as Equipment;
			if (equip.ConfigEquip.star > star && equip.Num != 40998 &&
				equip.Num != 40999 && equip.Num != 45999 && equip.Num != 45998)
			{
				star = equip.ConfigEquip.star;
			}
		}

		if (star >= 3)
		{
			string strContent = Core.Data.stringManager.getString (5154);
			strContent = string.Format (strContent, Core.Data.stringManager.getString(5014), star,Core.Data.stringManager.getString(6), Core.Data.stringManager.getString(5014), Core.Data.stringManager.getString(5009));
			UIInformation.GetInstance ().SetInformation (strContent, Core.Data.stringManager.getString(5030), SendStrengthEquipMsg, null);
		}
		else
		{
			SendStrengthEquipMsg ();
		}
	}

	void SendStrengthEquipMsg()
	{
		List<int> charNode = new List<int>();
		foreach(SQYNodeForBI node in szSelectCharator)
		{
			Equipment equip = node._boxItem.curData as Equipment;
			charNode.Add(equip.RtEquip.id);
		}

		Core.Data.temper.preEquipData = new Equipment();
        Core.Data.temper.preEquipData.ConfigEquip = new EquipData ();
        Core.Data.temper.preEquipData.ConfigEquip.def =  SQYUIManager.getInstance().targetEquip.ConfigEquip.def;
        Core.Data.temper.preEquipData.ConfigEquip.atk =  SQYUIManager.getInstance().targetEquip.ConfigEquip.atk;
        Core.Data.temper.preEquipData.ConfigEquip.atkGrowth = SQYUIManager.getInstance ().targetEquip.ConfigEquip.atkGrowth;
        Core.Data.temper.preEquipData.ConfigEquip.defGrowth = SQYUIManager.getInstance ().targetEquip.ConfigEquip.defGrowth;
        Core.Data.temper.preEquipData.RtEquip = new EquipInfo ();
        Core.Data.temper.preEquipData.ConfigEquip.star = SQYUIManager.getInstance ().targetEquip.ConfigEquip.star;
        Core.Data.temper.preEquipData.ConfigEquip.name = SQYUIManager.getInstance ().targetEquip.ConfigEquip.name;
        Core.Data.temper.preEquipData.RtEquip.lv = SQYUIManager.getInstance ().targetEquip.RtEquip.lv;
        Core.Data.temper.preEquipData.RtEquip.exp = SQYUIManager.getInstance ().targetEquip.RtEquip.exp;



        Debug.Log (" origin  temperData lv = " + Core.Data.temper.preEquipData.RtEquip.lv + " exp = " +  Core.Data.temper.preEquipData.RtEquip.exp );
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.STRENGTHEN_EQUIPMENT, new StrengthEquipParam(Core.Data.playerManager.PlayerID, SQYUIManager.getInstance().targetEquip.RtEquip.id, charNode.ToArray(), 
		                                                                                SQYUIManager.getInstance().targetEquip.ConfigEquip.type + 1, SQYUIManager.getInstance().targetEquip.Num));

		task.ErrorOccured += testHttpResp_Error;
		task.afterCompleted += testHttpResp_UI;

		task.DispatchToRealHandler();

		ComLoading.Open ();
	}

	void AddEquip()
	{
		if(szSelectCharator.Count == 0)
		{
			return;
		}

		Equipment equip = szSelectCharator[0]._boxItem.curData as Equipment;
		if(Core.Data.EquipManager.IsExpEquip(equip.Num))
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(5179));
			return;
		}

		ChangeEquipmentParam param = new ChangeEquipmentParam();

		param.gid = Core.Data.playerManager.PlayerID;
		param.tmid = Core.Data.playerManager.RTData.curTeamId;
		param.pos = (short)(SQYUIManager.getInstance ().opIndex + 1);
//		param.pos = (short)(TeamUI.mInstance.mSelectIndex + 1);
		param.seqid =0;
		param.teqid = equip.RtEquip.id;


		//选择 兵器
		Core.Data.temper.curSelectEquip = equip;

		ChanggeEquip(param);
	}

	public void UnEquip()
	{
		if(szSelectCharator.Count == 0)
		{
			return;
		}
		
		ChangeEquipmentParam param = new ChangeEquipmentParam();
		param.gid = Core.Data.playerManager.PlayerID;
		param.tmid = Core.Data.playerManager.RTData.curTeam.TeamId;
		param.seqid = SQYUIManager.getInstance().targetEquip.RtEquip.id;
		param.teqid = 0;
		param.pos = (short)(Core.Data.playerManager.RTData.curTeam.GetEquipPosByEquipID(param.seqid) + 1);

		ChanggeEquip(param);
	}

	void SwapEquip()
	{
		if(szSelectCharator.Count == 0)
		{
			return;
		}
		
		ChangeEquipmentParam param = new ChangeEquipmentParam();
		Equipment targetEquip = szSelectCharator[0]._boxItem.curData as Equipment;

		if(targetEquip.Num == 40999 || targetEquip.Num == 40998 || targetEquip.Num == 45999 || targetEquip.Num == 45998)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(5179));
			return;
		}

		param.gid = Core.Data.playerManager.PlayerID;
		param.tmid = Core.Data.playerManager.RTData.curTeam.TeamId;
		param.seqid = SQYUIManager.getInstance().targetEquip.RtEquip.id;
		param.teqid = targetEquip.RtEquip.id;
		param.pos = (short)(SQYUIManager.getInstance ().opIndex + 1);

		//选择的装备
		Core.Data.temper.curSelectEquip = targetEquip;
		ChanggeEquip(param);
	}

	void ChanggeEquip(ChangeEquipmentParam param)
	{
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		task.AppendCommonParam(RequestType.CHANGE_EQUIPMENT, param);
		
		task.ErrorOccured += testHttpResp_Error;
		task.afterCompleted += testHttpResp_UI;

		//then you should dispatch to a real handler
		task.DispatchToRealHandler ();

		ComLoading.Open ();
	}

	public void UseProp()
	{
		if (szSelectCharator.Count != 1)
			return;
		HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);

		UsePropParam param = new UsePropParam();
		param.gid = Core.Data.playerManager.PlayerID;
		if (_boxType == EMBoxType.LOOK_Props)
		{
            Item prop = szSelectCharator[0]._boxItem.curData as Item;

			if (prop.configData.type == (int)ItemType.JingLi)
			{
				if (Core.Data.playerManager.RTData.curJingLi >= 999)
				{
					//5214
					string strText = Core.Data.stringManager.getString (5176);
					strText = string.Format (strText, Core.Data.stringManager.getString (5038));
					SQYAlertViewMove.CreateAlertViewMove (strText);
					return;
				}

				VipInfoData vipInfo = Core.Data.vipManager.GetVipInfoData (Core.Data.playerManager.curVipLv);
				if (vipInfo != null)
				{
					if (Core.Data.playerManager.dayStatus.jlUse >= vipInfo.energyItemLimit)
					{
						SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (5157));
						return;
					}
					else
					{
						Core.Data.playerManager.dayStatus.jlUse++;
					}
				}
			}
			else if (prop.configData.type == (int)ItemType.TiLi)
			{
				if (Core.Data.playerManager.RTData.curTili >= 99)
				{
					string strText = Core.Data.stringManager.getString (5176);
					strText = string.Format (strText, Core.Data.stringManager.getString (5039));
					SQYAlertViewMove.CreateAlertViewMove (strText);
					return;
				}

				VipInfoData vipInfo = Core.Data.vipManager.GetVipInfoData (Core.Data.playerManager.curVipLv);
				if (vipInfo != null)
				{
					if (Core.Data.playerManager.dayStatus.tlUse >= vipInfo.staminaItemLimit)
					{
						SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (5158));
						return;
					}
					else
					{
						Core.Data.playerManager.dayStatus.tlUse++;
					}
				}
			}
			else if (prop.configData.type == (int)ItemType.Key)
			{
				//打开宝箱
				viewWillHidden ();
				//WXLAcitvityFactory.CreatActivity (ActivityItemType.TreasureBoxItem, DBUIController.mDBUIInstance._bottomRoot);
				TreasureBoxController tBCtrl = TreasureBoxController.CreatTreasureBoxCtr ();
				tBCtrl.gameObject.SetActive (true);
				return;
			}
			else if (prop.configData.ID == ItemManager.CHAOSHENSHUI)
			{
				viewWillHidden ();
				TrainingRoomUI.OpenUI (ENUM_TRAIN_TYPE.QianLiXunLian, null, TraingUICallback);
				return;
			}
			else if (prop.configData.ID == ItemManager.WUXINGWAN)
			{
				viewWillHidden ();
				TrainingRoomUI.OpenUI (ENUM_TRAIN_TYPE.AttrSwap, null, TraingUICallback);
				return;
			}

			param.propid = prop.RtData.id;
			param.propNum = prop.RtData.num;
			param.nm = 1;
		}
		else if (_boxType == EMBoxType.LOOK_AtkFrag || _boxType == EMBoxType.LOOK_DefFrag || _boxType == EMBoxType.LOOK_MonFrag)
		{
			Soul soul = szSelectCharator[0]._boxItem.curData as Soul;
			param.propid = soul.m_RTData.id;
			param.propNum = soul.m_RTData.num;
			param.nm = soul.m_config.quantity;
		}
			
		task.AppendCommonParam(RequestType.USE_PROP, param);

		task.ErrorOccured += testHttpResp_Error;
		task.afterCompleted += testHttpResp_UI;
	
		//then you should dispatch to a real handler
		task.DispatchToRealHandler ();

		m_scrollVal = sv_BagPackage.verticalScrollBar.value;
//		if ((param.propNum <= 110026 && param.propNum >= 110024))
//		{
			ComLoading.Open();
//		}
	}


	void TraingUICallback()
	{
		viewWillShow ();
		SetPetBoxType (EMBoxType.LOOK_Props);
		UpdateBtnSprite ();
		UpdateBtnEnable ();
	}

	public void SoulHeCheng()
	{
		if (szSelectCharator.Count != 1)
			return;
		HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);

		SoulHeChenParam param = new SoulHeChenParam();
		Soul soul = szSelectCharator [0]._boxItem.curData as Soul;
		param.gid = Core.Data.playerManager.PlayerID;
		param.chipId = soul.m_RTData.id;

		task.AppendCommonParam(RequestType.SOULHECHENG, param);

		task.ErrorOccured += testHttpResp_Error;
		task.afterCompleted += testHttpResp_UI;

		//then you should dispatch to a real handler
		task.DispatchToRealHandler ();
		ComLoading.Open ();
	}

	public void ChangeTeam ()
	{
		if (szSelectCharator.Count != 1)
			return;

		Monster mon = szSelectCharator [0]._boxItem.curData as Monster;
		if(mon.num == 19999 || mon.num == 19998)
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(5178));
			return;
		}

		//同一类型的人不让上阵
		if (!Core.Data.guideManger.isGuiding) {
			if (CheckSameNumMon (mon) == false) {
				SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getNetworkErrorString (4020));
				return;
			}
		}

		//记录 选择的 宠物
		Core.Data.temper.bagSelectMonster = mon;
		
		HttpTask task = new HttpTask (ThreadType.MainThread, TaskResponse.Default_Response);
		
		string spid = Core.Data.playerManager.PlayerID;
		int srcID = 0;
		if (SQYUIManager.getInstance ().opMonster != null) {
			srcID = SQYUIManager.getInstance ().opMonster.pid;
		}
		int tarID = mon.pid;
		int pos = SQYUIManager.getInstance ().opIndex + 1;
		int teamID = Core.Data.playerManager.RTData.curTeamId;  //SQYUIManager.getInstance ().opTeam.TeamId;
		
		task.AppendCommonParam (RequestType.CHANGE_TEAM_MEMBER, new ChangeTeamMemberParam (spid, srcID, tarID, pos, teamID));

		task.ErrorOccured += testHttpResp_Error;
		task.afterCompleted += testHttpResp_UI;

		task.DispatchToRealHandler ();

		ComLoading.Open ();
	}

	bool CheckSameNumMon (Monster selectMom){
		if (selectMom != null) {

			MonsterTeam tMteam = Core.Data.playerManager.RTData.getTeam (Core.Data.playerManager.RTData.curTeamId);
			//一样的替换
			if (SQYUIManager.getInstance ().opMonster != null) {
				if (selectMom.num == SQYUIManager.getInstance ().opMonster.num) {
					//如果有重的 且相等
					foreach (Monster tMon  in tMteam.TeamMember) {
						if (tMon != null && SQYUIManager.getInstance () != null) {
							if (tMon.num == SQYUIManager.getInstance ().opMonster.num) {
								if (tMteam.GetMonsterPos (SQYUIManager.getInstance ().opMonster.pid) != tMteam.GetMonsterPos (tMon.pid)) {
									return false;
								}
							}
						}
						return true;
					}				
					return false;
				} else {
					foreach (Monster tMon  in tMteam.TeamMember) {
						if (tMon != null) {
							if (tMon.num == selectMom.num) {
								return false;
							}
						}
					}
					return true;
				}
			} else {
				foreach (Monster tMon  in tMteam.TeamMember ) {
					if (tMon != null) {
						if (tMon.num == selectMom.num) {
								return false;
							}
						}
					}
					return true;
				}				
				return false;
		}
		return false;

	}

	#region 网络返回

	void testHttpResp_UI (BaseHttpRequest request, BaseResponse response)
	{
		ComLoading.Close ();
		if (response.status != BaseResponse.ERROR) 
		{
			HttpRequest rq = request as HttpRequest;
			switch(rq.Type)
			{
				case RequestType.SELL_MONSTER:
					CheckMonsterOperateType ();
					SellMonsterResponse resp = response as SellMonsterResponse;
					SQYAlertViewMove.CreateAlertViewMove(string.Format(Core.Data.stringManager.getString(5153), resp.data));
					DBUIController.mDBUIInstance.RefreshUserInfo();
					break;
			case RequestType.CHANGE_TEAM_MEMBER:
					ChangeTeamMemberParam param = rq.ParamMem as ChangeTeamMemberParam;
					Monster chgMon = Core.Data.monManager.getMonsterById (param.droleid);
					TeamUI.mInstance.SetShow (true);
					TeamUI.mInstance.RefreshMonster (chgMon);
					DBUIController.mDBUIInstance._mainViewCtl.RefreshUserInfo ();
					viewWillHidden ();
					SQYMainController.mInstance.UpdateTeamTip ();
					
				//added by zhangqiang to modify guide
				if(Core.Data.guideManger.isGuiding)
						Core.Data.guideManger.AutoRUN();
					break;

				case RequestType.STRENGTHEN_MONSTER:
				{
					Monster mon = Core.Data.monManager.getMonsterById (SQYUIManager.getInstance ().opMonster.pid);
					LevelUpMsgBox.OpenUI (mon, ConfigDataType.Monster);
//					if(TeamUI.mInstance != null && TeamUI.mInstance.IsShow)
//					{
//						TeamUI.mInstance.curMonster = mon;
//					}
					
					CheckMonsterOperateType ();
					DBUIController.mDBUIInstance.RefreshUserInfo();
					SQYMainController.mInstance.UpdateTeamTip ();
					break;
				}
				case RequestType.SELL_GEM:
					SetPetBoxType (_boxType);
					DBUIController.mDBUIInstance.RefreshUserInfo ();
					string strText = Core.Data.stringManager.getString (5153);
					SellEquipResponse sellresp = response as SellEquipResponse;
					strText = string.Format (strText, sellresp.data);
					SQYAlertViewMove.CreateAlertViewMove (strText);
					break;
				case RequestType.SELL_EQUIPMENT:
					CheckEquipOperateType ();
					SellEquipResponse equpResp = response as SellEquipResponse;
					SQYAlertViewMove.CreateAlertViewMove(string.Format(Core.Data.stringManager.getString(5153), equpResp.data));
					DBUIController.mDBUIInstance.RefreshUserInfo();
					SQYMainController.mInstance.UpdateTeamTip ();
					break;
                case RequestType.STRENGTHEN_EQUIPMENT:
                    StrengthEquipParam equipParam = rq.ParamMem as StrengthEquipParam;
                    Equipment equip = Core.Data.EquipManager.getEquipment (equipParam.seqid);
                    LevelUpMsgBox.OpenUI (equip, ConfigDataType.Equip);
                    Core.Data.soundManager.SoundFxPlay (SoundFx.FX_Strengthen_Weapon);
                    DBUIController.mDBUIInstance.RefreshUserInfoWithoutShow ();
					if (TeamUI.mInstance == null || (TeamUI.mInstance != null && !TeamUI.mInstance.IsShow))
					{
                        SetPetBoxType (EMBoxType.LOOK_Equipment);
						DBUIController.mDBUIInstance.RefreshUserInfo();
                    } 
					else 
					{
//                      DBUIController.mDBUIInstance.SetViewState (EMViewState.S_Team_NoSelect);
						DBUIController.mDBUIInstance.RefreshUserInfo();

						UIMiniPlayerController.Instance.SetActive(false);
                    }
					SQYMainController.mInstance.UpdateTeamTip ();
					break;
                case RequestType.CHANGE_EQUIPMENT:
                    ChangeEquipmentParam chgparam = rq.ParamMem as ChangeEquipmentParam;
                    if (chgparam.teqid == 0)
                    {
                        SetPetBoxType(_boxType);
                    }
                    else
                    {
                        TeamUI.mInstance.SetShow(true);
                        DBUIController.mDBUIInstance._mainViewCtl.RefreshUserInfo();

                        viewWillHidden();
                    }
                    UIMiniPlayerController.Instance.SetActive(false);

					TeamUI.mInstance.FreshCurTeam ();
					SQYMainController.mInstance.UpdateTeamTip ();

				//added by zhangqiang to modify guide
				if(Core.Data.guideManger.isGuiding)
					Core.Data.guideManger.AutoRUN();

					break;
				case RequestType.USE_PROP:
					UsePropSuc (request,response);
					break;
				case RequestType.SOULHECHENG:
					SoulHeChengSuc (request,response);
					break;
				case RequestType.DECOMPOSE_MONSTER:
					DecomposeMonSuc (request, response);
					break;
			}
		} 
		else 
		{
			SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(30000 + response.errorCode));
		}
	}


	void CheckMonsterOperateType()
	{
		int count = 0;
		for (short i = 1; i <= 5; i++)
		{
			List<Monster> list = Core.Data.monManager.getMonsterListByStar (i, SplitType.Split_If_InTeam);
			if (list != null)
			{
				count += list.Count;
			}
		}
		if (count > 0)
		{
			SetPetBoxType (_boxType);
		}
		else
		{
			SetPetBoxType (EMBoxType.LOOK_Charator);
		}
	}

	void CheckEquipOperateType()
	{
		int count = 0;
		for (short i = 1; i <= 5; i++)
		{
			List<Equipment> list = Core.Data.EquipManager.getAtkEquipByStar (i, SplitType.Split_If_InTeam);
			if (list != null)
			{
				count += list.Count;
			}
		}
		if (count > 0)
		{
			SetPetBoxType (_boxType);
		}
		else
		{
			SetPetBoxType (EMBoxType.LOOK_Equipment);
		}
	}


	void testHttpResp_Error (BaseHttpRequest request, string error)
	{
		ComLoading.Close();
		ConsoleEx.DebugLog ("---- Http Resp - Error has ocurred." + error);
		SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(36000));
	}

	#endregion


	void SoulHeChengSuc(BaseHttpRequest req, BaseResponse response)
	{
//		SetPetBoxType (EMBoxType.LOOK_Soul);
		SetPetBoxType (_boxType);
		SoulHeChenResponse resp = response as SoulHeChenResponse;
		GetRewardSucUI.OpenUI (resp.data, Core.Data.stringManager.getString(5096));

		if(resp.data[0].getCurType() == ConfigDataType.Monster)
		{
			Monster mon = resp.data[0].toMonster(Core.Data.monManager);
			if(mon.Star == 5)
			{
				Core.Data.taskManager.AutoShowPromptWord();
			}
		}
	}

	void DecomposeMonSuc(BaseHttpRequest req, BaseResponse response)
	{
		SetPetBoxType (EMBoxType.DECOMPOSE_MONSTER);
		DecomposeMonsterResponse resp = response as DecomposeMonsterResponse;
		//ui显示
		GetRewardSucUI.OpenUI (resp.data.p, Core.Data.stringManager.getString(5047));
		DBUIController.mDBUIInstance.RefreshUserInfo ();
		SQYMainController.mInstance.UpdateTeamTip ();
	}

	void UsePropSuc(BaseHttpRequest req,  BaseResponse response)
	{
		HttpRequest request = req as HttpRequest;
		UsePropParam param = request.ParamMem as UsePropParam;
		SetPetBoxType (EMBoxType.LOOK_Props);

		szSelectCharator.Clear();
		SQYNodeForBI bi = GetBagItem (param.propid);
		if (bi != null)
		{
			selectOneCharator (bi);
		}
			
		UsePropResponse resp = response as UsePropResponse;
		if(resp.data == null)
		{
			return;
		}

		ItemOfReward[] rewards = resp.data.p;
		SQYMainController.mInstance.UpdateBagTip ();

		if (rewards != null)
		{
			if (rewards.Length == 0)
			{
				RED.LogWarning ("get nothing!");
				return;
			}

			ItemData itemData = Core.Data.itemManager.getItemData(param.propNum);
			if(itemData != null && itemData.type == (short)ItemType.Egg)
			{
				if(rewards.Length == 1 && rewards[0].getCurType() == ConfigDataType.Monster)
				{
					CradSystemFx.GetInstance ().SetCardSinglePanel (rewards,350,  true, true, false);
				}
				else
				{
					GetRewardSucUI.OpenUI (rewards, Core.Data.stringManager.getString (5097));
				}
			}
			else
			{
				GetRewardSucUI.OpenUI (rewards, Core.Data.stringManager.getString (5097));
			}
		}
		else
		{

            //5214
            string strText = Core.Data.stringManager.getString (5214);
			if (resp.data.coin > 0)
			{

                strText =   string.Format(strText, resp.data.coin , Core.Data.stringManager.getString (5037));
				//strText += resp.data.coin + Core.Data.stringManager.getString (5037) + " ";
			}
			if (resp.data.eny > 0)
			{
				
                strText =   string.Format(strText, resp.data.eny , Core.Data.stringManager.getString (5038));

                //strText += resp.data.eny + Core.Data.stringManager.getString (5038) + " ";
			}
			if (resp.data.pwr > 0)
			{

                strText =   string.Format(strText, resp.data.pwr , Core.Data.stringManager.getString (5039));


				//strText += resp.data.pwr + Core.Data.stringManager.getString (5039);
			}
			if(resp.data.stone > 0)
			{


                strText =   string.Format(strText, resp.data.stone , Core.Data.stringManager.getString (5070));

				//strText += resp.data.stone + Core.Data.stringManager.getString (5070);

				string strName = "";
				ItemData item = Core.Data.itemManager.getItemData(param.propNum);
				if(item != null)
				{
					strName = item.name;
				}
				
				Core.Data.ActivityManager.OnUsedItem(strName , 1);
			}
			
			SQYAlertViewMove.CreateAlertViewMove (strText);

			DBUIController.mDBUIInstance.RefreshUserInfo ();
		}

		sv_BagPackage.verticalScrollBar.value = m_scrollVal;
		SQYMainController.mInstance.UpdateTeamTip ();
	}

	#region override

	public override void viewWillShow ()
	{
		RED.SetActive (true, this.gameObject);
		//DBUIController.mDBUIInstance._playerViewCtl.SetActive(true);
		base.viewWillShow ();
	}
	

	public override void viewDidShow ()
	{
		base.viewDidShow ();

		sv_BagPackage.Scroll(0.03f);
		m_bShow = true;
	}

	public override void viewWillHidden ()
	{
		base.viewWillHidden ();
	}

	public override void viewDidHidden ()
	{
		base.viewDidHidden ();
		m_bShow= false;
		RED.SetActive (false, this.gameObject);
	}

	#endregion

	public static SQYPetBoxController CreatePetBoxView ()
	{
		Object obj = PrefabLoader.loadFromPack ("SQY/pbSQYPetBoxController");
		if (obj != null) {
			GameObject go = Instantiate (obj) as GameObject;
			SQYPetBoxController pc = go.GetComponent<SQYPetBoxController> ();
			_mInstace = pc;
			pc.initView ();

			DBUIController.mDBUIInstance.RefreshUserInfo();
			RED.AddChild(go, DBUIController.mDBUIInstance._bottomRoot);
			go.transform.localPosition = new Vector3(-1140,0,0);
			return pc;
		}
		return null;
	}


	public SQYNodeForBI GetBagItem(int itemId)
	{
		if (dicAllBoxItem.ContainsKey (itemId))
		{
			return dicAllBoxItem [itemId];
		}
		return null;
	}

	public SQYNodeForBI GetBagItemByStarAndPos(int star, int pos)
	{
		if(dicAllStarLine.ContainsKey(star))
		{
			return dicAllStarLine[star].szCurNodeBI[pos];
		
		}
		return null;
	}
		
	//新手引导，调整筋斗云的位置
	public void GuideSortJinDouYun()
	{
		List<Equipment> srcList = Core.Data.EquipManager.getEquipListByStar (3, SplitType.Split_If_InCurTeam);
		List<object> newList = new List<object> ();

		foreach (Equipment equip in srcList)
		{
			if (equip.Num == 45108 && equip.RtEquip.id == -10)
			{
				newList.Add (equip as object);
				srcList.Remove (equip);
				break;
			}
		}
		newList.AddRange (srcList.ToArray ());
		dicAllStarLine [3].freshLinePetView (newList, 3, EMBoxType.Equip_ADD_DEF, SplitType.Split_If_InCurTeam, EMItemType.Equipment);
	}

	//新手引导，调整龟仙人位置
	public void GuideSortGuiXianRen()
	{
		List<Monster> srcList = Core.Data.monManager.getMonsterListByStar (3, SplitType.Split_If_InCurTeam);
		List<object> newList = new List<object> ();

		foreach (Monster mon in srcList)
		{
			if (mon.num == 10140 && mon.pid == -11)
			{
				newList.Add (mon as object);
				srcList.Remove (mon);
				break;
			}
		}

		newList.AddRange (srcList.ToArray ());
		dicAllStarLine [3].freshLinePetView (newList, 3, EMBoxType.CHANGE, SplitType.Split_If_InCurTeam, EMItemType.Charator);
	}
}
