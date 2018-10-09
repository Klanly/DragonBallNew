using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BagItemOprtUI : MonoBehaviour 
{
	public UIButton m_btnDecompose;			//分解
	public UIButton m_btnStrengthen;		//强化
	public UIButton m_btnSell;				//出售
	public UIButton m_btnDetial;			//详情

	private UISprite m_spDecompose;
	private UISprite m_spStrengthen;
	private UISprite m_spSell;
	private UISprite m_spDetial;


	private const string SP_SELL = "beibao_msz_cs";				//出售
	private const string SP_DECOMPOSE = "beibao_msz_fj";		//分解
	private const string SP_HECHENG = "beibao_msz_hc";			//合成
	private const string SP_QIANGHUA = "beibao_msz_qh";			//强化
	private const string SP_USE = "beibao_msz_sy";				//使用
	private const string SP_DETIAL = "beibao_msz_xq";			//详情
	private const string SP_XIANGQIAN = "beibao_msz_xq1";		//镶嵌
	private const string SP_UNEQUIP = "beibao_msz_xx";			//卸下
	private const string SP_COLLECT = "shouji";			//收集

	//选中的背包项
	private SQYBoxItem m_selBagItem;

	private Vector3[] m_targetPos = new Vector3[4] {new Vector3(0, 50, 0), new Vector3(55,-20,0), new Vector3(0, -90, 0), new Vector3(-55, -20, 0)};
	private List<GameObject> m_btns = new List<GameObject>();

	const int limitLv =15 ;

	void PlayAnim()
	{
		if (m_btns.Count == 0)
		{
			m_btns.Add (m_btnDecompose.gameObject);
			m_btns.Add (m_btnStrengthen.gameObject);
			m_btns.Add (m_btnSell.gameObject);
			m_btns.Add (m_btnDetial.gameObject);
		}

		for (int i = 0; i < m_btns.Count; i++)
		{
			m_btns [i].transform.localPosition = Vector3.zero;
		}

		for (int i = 0; i < m_btns.Count; i++)
		{
			TweenPosition.Begin (m_btns [i], 0.1f, m_targetPos [i]);
		}
	}


	void InitBtnSprite()
	{
		Transform content = null;
		if (m_spDecompose == null)
		{
			content = m_btnDecompose.transform.FindChild ("content");
			m_spDecompose = content.GetComponent<UISprite> ();
		}

		if (m_spDetial == null)
		{
			content = m_btnDetial.transform.FindChild ("content");
			m_spDetial = content.GetComponent<UISprite>();
		}

		if (m_spSell == null)
		{
			content = m_btnSell.transform.FindChild ("content");
			m_spSell = content.GetComponent<UISprite>();
		}

		if (m_spStrengthen == null)
		{
			content = m_btnStrengthen.transform.FindChild ("content");
			m_spStrengthen = content.GetComponent<UISprite>();
		}
	}

	public void OpenUI(SQYBoxItem  itemParent)
	{
		RED.AddChild (this.gameObject, itemParent.transform.parent.gameObject);
		m_selBagItem = itemParent;
		InitBtnSprite ();
		SetShow (true);
		switch (itemParent.curItemType)
		{
			case RUIType.EMItemType.Charator:
				InitMonsterUI ();
				break;
			case RUIType.EMItemType.Equipment:
				InitEquipUI ();
				break;
			case RUIType.EMItemType.Gem:
				InitGemUI ();
				break;
			case RUIType.EMItemType.Props:
				InitPropUI ();
				break;
//			case RUIType.EMItemType.Soul:
			case RUIType.EMItemType.AtkFrag:
			case RUIType.EMItemType.DefFrag:
			case RUIType.EMItemType.MonFrag:
				InitSoulUI ();
				break;
		}

		PlayAnim ();
	}

	public void SetShow(bool bShow)
	{
		RED.SetActive (bShow, this.gameObject);
		if (!bShow)
		{
			this.transform.parent = SQYPetBoxController.mInstance.transform;
		}
	}

	void InitMonsterUI()
	{
		Monster mon = m_selBagItem.curData as Monster;
		RED.SetActive (true, m_btnDecompose.gameObject, m_btnStrengthen.gameObject, m_btnSell.gameObject, m_btnDetial.gameObject);

		m_spDecompose.spriteName = SP_DECOMPOSE;
		m_spStrengthen.spriteName = SP_QIANGHUA;
		m_spSell.spriteName = SP_COLLECT;
		m_spDetial.spriteName = SP_DETIAL;

		m_btnDecompose.isEnabled = !mon.inTeam && mon.Star >= 3 && !Core.Data.monManager.IsExpMon(mon.num);
		m_btnStrengthen.isEnabled = !Core.Data.monManager.IsExpMon(mon.config.ID);
//		m_btnSell.isEnabled = !mon.inTeam && mon.config.star <= 4;
		m_btnSell.isEnabled = true;
		m_btnDetial.isEnabled = true;
	}

	void InitEquipUI()
	{
		Equipment equip = m_selBagItem.curData as Equipment;
		RED.SetActive (true, m_btnDecompose.gameObject, m_btnStrengthen.gameObject, m_btnSell.gameObject, m_btnDetial.gameObject);

		m_spDecompose.spriteName = SP_XIANGQIAN;
		m_spStrengthen.spriteName = SP_QIANGHUA;
	
		m_spSell.spriteName = SP_COLLECT;
		m_spDetial.spriteName = SP_DETIAL;

		m_btnDecompose.isEnabled = true;
		m_btnStrengthen.isEnabled = !Core.Data.EquipManager.IsExpEquip(equip.ConfigEquip.ID);
//		m_btnSell.isEnabled = (!equip.equipped && equip.RtEquip.EquipedGemCount == 0);
		m_btnSell.isEnabled = true;
		m_btnDetial.isEnabled = true;
	}

	void InitGemUI()
	{
		Gems gem = m_selBagItem.curData as Gems;
		RED.SetActive (true, m_btnDecompose.gameObject, m_btnSell.gameObject);
		RED.SetActive (false, m_btnStrengthen.gameObject, m_btnDetial.gameObject);

		if (gem.equipped || gem.configData.star >= 4)
		{
			m_btnSell.isEnabled = false;
		}
		else
		{
			m_btnSell.isEnabled = true;
		}

		m_btnDecompose.isEnabled = !gem.equipped;
		m_spDecompose.spriteName = SP_HECHENG;
		m_spSell.spriteName = SP_SELL;
	}

	void InitSoulUI()
	{
		Soul soul = m_selBagItem.curData as Soul;
		RED.SetActive (false, m_btnSell.gameObject);
		RED.SetActive (true, m_btnStrengthen.gameObject, m_btnDetial.gameObject, m_btnDecompose.gameObject);

		m_btnDecompose.isEnabled = soul.m_RTData.count >= soul.m_config.quantity;
		m_spDecompose.spriteName = SP_HECHENG;
		m_spStrengthen.spriteName = SP_COLLECT;
		m_spDetial.spriteName = SP_DETIAL;
	}

	void InitPropUI()
	{
		Item item = m_selBagItem.curData as Item;
		RED.SetActive (true, m_btnDecompose.gameObject);
		RED.SetActive (false, m_btnStrengthen.gameObject, m_btnDetial.gameObject, m_btnSell.gameObject);

		m_btnDecompose.isEnabled = item.configData.CanUse ();
		m_spDecompose.spriteName = SP_USE;
	}


	//分解
	void OnClickDecompose()
	{
		switch (m_selBagItem.curItemType)
		{
			case RUIType.EMItemType.Charator:
				SQYPetBoxController.mInstance.DecomposeMonster ();
				break;
			case RUIType.EMItemType.Equipment:
				OpenFroginRoomUI ();
				break;
			case RUIType.EMItemType.Gem:
				OpenGemHechengUI ();
				break;
			case RUIType.EMItemType.Props:
				SQYPetBoxController.mInstance.UseProp ();
				break;
//			case RUIType.EMItemType.Soul:
			case RUIType.EMItemType.AtkFrag:
			case RUIType.EMItemType.DefFrag:
			case RUIType.EMItemType.MonFrag:
				SQYPetBoxController.mInstance.SoulHeCheng();
				break;
		}
	}

	//强化
	public void OnClickStrengthen()
	{
		switch (m_selBagItem.curItemType)
		{
			case RUIType.EMItemType.Charator:
				SQYUIManager.getInstance ().opMonster = m_selBagItem.curData as Monster;
				SQYPetBoxController.mInstance.SetPetBoxType (RUIType.EMBoxType.QiangHua);
				SQYPetBoxController.enterStrengthIndex = 1;
				break;
			case RUIType.EMItemType.Equipment:
				Equipment equip = m_selBagItem.curData as Equipment;
				SQYUIManager.getInstance ().targetEquip = equip;
				if (equip.ConfigEquip.type == 0)
				{
					SQYPetBoxController.mInstance.SetPetBoxType (RUIType.EMBoxType.Equip_QH_ATK);
				}
				else
				{
					SQYPetBoxController.mInstance.SetPetBoxType (RUIType.EMBoxType.Equip_QH_DEF);
				}
				break;
			case RUIType.EMItemType.AtkFrag:
			case RUIType.EMItemType.DefFrag:
			case RUIType.EMItemType.MonFrag:
				OpenFatePanel ();
				break;
		}
	}

	//出售
	void OnClickSell()
	{
		switch (m_selBagItem.curItemType)
		{
			case RUIType.EMItemType.Charator:
			OpenFatePanel ();
//				SQYPetBoxController.mInstance.SellCharator ();
				break;
		case RUIType.EMItemType.Equipment:
			OpenFatePanel ();
//				Equipment equip = m_selBagItem.curData as Equipment;
//				SQYUIManager.getInstance ().targetEquip = equip;
//				if (equip.equipped)
//				{
//					SQYPetBoxController.mInstance.UnEquip ();
//				}
//				else
//				{
//					SQYPetBoxController.mInstance.SellEquipment ();
//				}
				break;
			case RUIType.EMItemType.Gem:
				SQYPetBoxController.mInstance.SellGem ();
				break;
//			case RUIType.EMItemType.Soul:
		}
	}

	//详情
	void OnClickDetial()
	{
		switch (m_selBagItem.curItemType)
		{
			case RUIType.EMItemType.Equipment:
				Equipment equip = m_selBagItem.curData as Equipment;
				JCEquipmentDesInfoUI.OpenUI (equip);
				break;
			case RUIType.EMItemType.Charator:
				Monster monster = m_selBagItem.curData as Monster;
				MonsterInfoUI.OpenUI(monster,ShowFatePanelController.FateInPanelType.isInMonsterInfoPanel);
				break;
			case RUIType.EMItemType.AtkFrag:
			case RUIType.EMItemType.DefFrag:
			case RUIType.EMItemType.MonFrag:
				OpenFragInfoUI ();
				break;
		}
	}


	void hideBag()
	{
		DBUIController.mDBUIInstance.SetViewState(RUIType.EMViewState.H_Bag);
	}

	void ActiveBag()
	{
		DBUIController.mDBUIInstance.SetViewState(RUIType.EMViewState.S_Bag, RUIType.EMBoxType.LOOK_Equipment);
	}

	void OpenFroginRoomUI()
	{
		int limitLv = Core.Data.BuildingManager.GetBuildUnlockLevel (BaseBuildingData.BUILD_YELIAN, 1);

		if (Core.Data.playerManager.RTData.curLevel < limitLv)
		{
			string strText = Core.Data.stringManager.getString (6054);
			strText = strText.Replace ("#", limitLv.ToString());
			SQYAlertViewMove.CreateAlertViewMove (strText);
			return;
		}

		FrogingSystem.ForgingRoomUI.OpenUI(ExitFroging);
		FrogingSystem.ForgingRoomUI.Instance.GoTo(FrogingSystem.ForgingPage.Forging_Mosaic);

		Equipment equip = m_selBagItem.curData as Equipment;
		if(equip != null)
			FrogingSystem.ForgingRoomUI.Instance.InlaySystem.SelectEquipment(equip);


		DBUIController.mDBUIInstance.SetViewState (RUIType.EMViewState.H_Bag);
	}


	//打开宝石合成UI
	void OpenGemHechengUI()
	{
		BaseBuildingData tBuildData =	Core.Data.BuildingManager.GetBuildFromBagByNum (BaseBuildingData.BUILD_YELIAN).config;
		if (tBuildData != null) {
			if (Core.Data.playerManager.Lv < tBuildData.limitLevel) {

				SQYAlertViewMove.CreateAlertViewMove (string.Format (Core.Data.stringManager.getString (7320), tBuildData.limitLevel.ToString ()) + tBuildData.name);
				return;
			}
		} else {
			SQYAlertViewMove.CreateAlertViewMove ("can't find building config ");
			return;
		}
		ComLoading.Open ();
		Gems gem = m_selBagItem.curData as Gems;
		FrogingSystem.ForgingRoomUI.OpenUI(()=>
		{
			//关闭镶嵌宝石界面
			FrogingSystem.ForgingRoomUI.Instance.DestoryForgingRoomUI();
			DBUIController.mDBUIInstance.SetViewState (RUIType.EMViewState.S_Bag);			
			SQYPetBoxController.mInstance.SetPetBoxType(RUIType.EMBoxType.LOOK_Gem);
		});
		DBUIController.mDBUIInstance.SetViewState (RUIType.EMViewState.H_Bag);
		FrogingSystem.ForgingRoomUI.Instance.GoToAndSetDia(FrogingSystem.ForgingPage.Forging_Synthetic,gem);
	}
	
	
	
	
	
	void ExitFroging()
	{
		FrogingSystem.ForgingRoomUI.Instance.DestoryForgingRoomUI();
		Core.Data.playerManager.RTData.curTeam.upgradeMember();
		DBUIController.mDBUIInstance._mainViewCtl.RefreshUserInfo();

		DBUIController.mDBUIInstance.SetViewState (RUIType.EMViewState.S_Bag, RUIType.EMBoxType.LOOK_Equipment);
	}

	/// <summary>
	/// 强化装备
	/// </summary>
	void OnClickStrengthEquip()
	{
		Equipment equip = m_selBagItem.curData as Equipment;
		List<Equipment> list = Core.Data.EquipManager.GetEquipList (equip.ConfigEquip.type, SplitType.Split_If_InTeam);
		if (list == null || list.Count == 0)
		{
			if(LuaTest.Instance != null && !LuaTest.Instance.ConvenientBuy){;}
			else UIInformation.GetInstance ().SetInformation (Core.Data.stringManager.getString(5098), Core.Data.stringManager.getString(5030), OpenShopUI);
			return;
		}

		if (equip.RtEquip.lv >= 60)
		{
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(5150));
			return;
		}

		if(equip.ConfigEquip.type == 0)
		{
			DBUIController.mDBUIInstance.SetViewState(RUIType.EMViewState.S_Bag,RUIType.EMBoxType.Equip_QH_ATK);
		}
		else if(equip.ConfigEquip.type == 1)
		{
			DBUIController.mDBUIInstance.SetViewState(RUIType.EMViewState.S_Bag,RUIType.EMBoxType.Equip_QH_DEF);
		}

		if(TeamUI.mInstance != null)
		{
			TeamUI.mInstance.CloseUI ();
		}
	}

	void OpenShopUI()
	{
		UIDragonMallMgr.GetInstance ().OpenUI (ShopItemType.Egg, null);
		if(SQYPetBoxController.mInstance != null && SQYPetBoxController.mInstance.IsShow)
		{
			DBUIController.mDBUIInstance.SetViewState(RUIType.EMViewState.H_Bag);
		}

		if(TeamUI.mInstance != null)
		{
			TeamUI.mInstance.CloseUI ();
		}
	}

	//  wxl  change  出售 改成 收集（出处）
	void OpenFatePanel(){
		if (m_selBagItem.pid != 0) 
		{
			if (m_selBagItem.curItemType == RUIType.EMItemType.Charator)
			{
				Monster tMonster = m_selBagItem.curData as Monster;
				if (tMonster != null) 
				{
					ShowFatePanelController.CreatShowFatePanel (tMonster.config.ID, ShowFatePanelController.FateInPanelType.isInBagPanel, null);
				}
			} 
			else if (m_selBagItem.curItemType == RUIType.EMItemType.Equipment) 
			{
				Equipment tEquip = m_selBagItem.curData as Equipment;
				if (tEquip != null) 
				{
					ShowFatePanelController.CreatShowFatePanel (tEquip.ConfigEquip.ID, ShowFatePanelController.FateInPanelType.isInBagPanel, null);
				}
			}
			else if(m_selBagItem.curItemType == RUIType.EMItemType.AtkFrag 
				|| m_selBagItem.curItemType == RUIType.EMItemType.DefFrag 
				|| m_selBagItem.curItemType == RUIType.EMItemType.MonFrag) 
				{
				Soul tSoul = m_selBagItem.curData as Soul;

				if (tSoul != null) 
				{
					if (tSoul.m_config.type == (int)ItemType.Monster_Frage)
					{
						ShowFatePanelController.CreatShowFatePanel (tSoul.m_config.updateId, ShowFatePanelController.FateInPanelType.isInBagPanel, null);
					} 
					else if(tSoul.m_config.type == (int)ItemType.Equip_Frage)
					{
						ShowFatePanelController.CreatShowFatePanel (tSoul.m_config.updateId, ShowFatePanelController.FateInPanelType.isInBagPanel, null);
					}
				} 
			}
		}
	}

	void OpenFragInfoUI()
	{
		Soul soul = m_selBagItem.curData as Soul;
		if (soul != null)
		{
			if (soul.m_config.type == (int)ItemType.Monster_Frage)
			{
				MonsterData mon = Core.Data.monManager.getMonsterByNum (soul.m_config.updateId);
				if (mon != null)
				{
					MonFragUI.OpenUI (mon);
				}
				else
				{
					RED.LogWarning (soul.m_config.ID + "  not find monster by monster frag :: " + soul.m_config.updateId);
				}
			}
			else if (soul.m_config.type == (int)ItemType.Equip_Frage)
			{
				EquipData equip = Core.Data.EquipManager.getEquipConfig (soul.m_config.updateId);
				if (equip != null)
				{
					YcgEquipInfo.openUI (equip);
				}
				else
				{
					RED.LogWarning (soul.m_config.ID + "  not find equip by equip frag :: " + soul.m_config.updateId);
				}
			}
		}
	}


}
