using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RUIType;
public partial class SQYPetBoxController {
	/// <summary>
	/// 选择了一大波
	/// </summary>
	/// <param name="all">If set to <c>true</c> all.</param>
	/// <param name="szNode">Size node.</param>
	void selectMoreCharator (bool all,List<SQYNodeForBI> szNode)
	{

		switch(_boxType)
		{
			case EMBoxType.SELL_Charator:
			case EMBoxType.SELL_Equiement:
			case EMBoxType.SELL_GEM:
			case EMBoxType.DECOMPOSE_MONSTER:
			case EMBoxType.QiangHua:
			case EMBoxType.Equip_QH_ATK:
			case EMBoxType.Equip_QH_DEF:
				for (int i = 0; i < szNode.Count; i++)
				{
					szNode [i]._boxItem.bSelect = all;
					if (all)
					{
						if (!szSelectCharator.Contains (szNode [i]))
						{
							szSelectCharator.Add (szNode [i]);
						}
					}
					else
					{
						if (szSelectCharator.Contains (szNode [i]))
						{
							szSelectCharator.Remove (szNode [i]);
						}
					}
				}

				break;
		}
		if(_boxType == EMBoxType.SELL_Charator || _boxType == EMBoxType.SELL_Equiement || _boxType == EMBoxType.SELL_GEM 
			|| _boxType == EMBoxType.QiangHua || _boxType == EMBoxType.Equip_QH_ATK || _boxType == EMBoxType.Equip_QH_DEF || _boxType == EMBoxType.DECOMPOSE_MONSTER)
		{
			UpdateSellMoney();
		}
		btn_Ok.isEnabled = szSelectCharator.Count > 0;
		CheckBagItemNewState ();
	}
	/// <summary>
	/// 选择了一个
	/// </summary>
	/// <param name="node">Node.</param>
	public void selectOneCharator (SQYNodeForBI node)
	{

		if (_boxType == EMBoxType.LOOK_Charator || _boxType == EMBoxType.LOOK_Equipment || _boxType == EMBoxType.LOOK_Gem || _boxType == EMBoxType.LOOK_Props 
			|| _boxType == EMBoxType.LOOK_AtkFrag || _boxType == EMBoxType.LOOK_DefFrag || _boxType == EMBoxType.LOOK_MonFrag)
		{
			m_bagItemOprtUI.OpenUI (node._boxItem);
		}
        //  Debug.Log (" type = " + _boxType);
		switch (_boxType) 
        {
		case EMBoxType.HECHENG_SHENREN_MAIN:
		case EMBoxType.HECHENG_ZHENREN_MAIN:
			{
				Monster curM = node._boxItem.curData as Monster;

				if(szSelectCharator.Count>0)
				{

					if (curM != null && curM.config.ID != 19999) {

						if (szSelectCharator [0] != node) {
							if (szSelectCharator [0] != null) {
								szSelectCharator [0]._boxItem.bSelect = false;
							}

							if (!node._boxItem.bSelect) {
								szSelectCharator [0] = node;
								node._boxItem.bSelect = true;
							}
						}
					} 

				}
				else
				{
					if (curM != null && curM.config.ID != 19999) {
						szSelectCharator.Add (node);
						node._boxItem.bSelect = true;
					}
				}
			}
			break;
		case EMBoxType.CHANGE:
		case EMBoxType.Equip_ADD_ATK:
		case EMBoxType.Equip_ADD_DEF:
		case EMBoxType.Equipment_SWAP_ATK:
		case EMBoxType.Equipment_SWAP_DEF:
		case EMBoxType.LOOK_Charator:
		case EMBoxType.EVOLVE_MONSTER:
		case EMBoxType.LOOK_Equipment:
		case EMBoxType.LOOK_Gem:
		case EMBoxType.LOOK_Props:
		case EMBoxType.LOOK_AtkFrag:
		case EMBoxType.LOOK_DefFrag:
		case EMBoxType.LOOK_MonFrag:
		case EMBoxType.HECHENG_ZHENREN_SUB:
		case EMBoxType.ZHENREN_HE_SHENREN_MAIN:
		case EMBoxType.ZHENREN_HE_SHENREN_SUB:
		case EMBoxType.HECHENG_SHENREN_SUB:
		case EMBoxType.ATTR_SWAP:
		case EMBoxType.QIANLI_XUNLIAN:
		case EMBoxType.GEM_HECHENG_MAIN:
		case EMBoxType.GEM_HECHENG_SUB:
		case EMBoxType.SELECT_EQUIPMENT_INLAY:
		case EMBoxType.SELECT_GEM_INLAY:
		case EMBoxType.SELECT_EQUIPMENT_RECAST:
			{
				if(szSelectCharator.Count>0)
				{
					if(szSelectCharator[0] != node)
					{
						if(szSelectCharator[0] != null)
						{
							szSelectCharator[0]._boxItem.bSelect = false;
						}
						
						if (!node._boxItem.bSelect) 
						{
							szSelectCharator[0] = node;
							node._boxItem.bSelect = true;
						}
					}
				}
				else
				{
					szSelectCharator.Add(node);
					node._boxItem.bSelect = true;
				}
			}
			break;

		case EMBoxType.QiangHua:
		case EMBoxType.Equip_QH_ATK:
		case EMBoxType.Equip_QH_DEF:
		case EMBoxType.SELL_Charator:
		case EMBoxType.SELL_Equiement:
		case EMBoxType.SELL_GEM:
		case EMBoxType.DECOMPOSE_MONSTER:
			{
				node._boxItem.bSelect = !node._boxItem.bSelect;
				if(szSelectCharator.Contains(node))
				{
					szSelectCharator.Remove(node);
				}
				if(node._boxItem.bSelect)
				{
					if (_boxType == EMBoxType.QiangHua)
					{
						int totalExp = GetStrengthMonTotalExp ();
						int finalLv = GetStrengthMonFinalLv(totalExp);
						if(finalLv < 60)
						{
							szSelectCharator.Add(node);
						}
						else
						{
							node._boxItem.bSelect  = false;

							if (node._boxItem.star == 6)
							{
								SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (5159));
							}
							else
							{
								SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (5139));
								//UIInformation.GetInstance ().SetInformation (Core.Data.stringManager.getString (5139), Core.Data.stringManager.getString (5030), OpenMonEvoveUI);
							}
						}
					}
					else if (_boxType == EMBoxType.Equip_QH_ATK || _boxType == EMBoxType.Equip_QH_DEF)
					{
						int finalLv = GetStrengthEquipFinalLv();
						if(finalLv < 60)
						{
							szSelectCharator.Add(node);
						}
						else
						{
							node._boxItem.bSelect  = false;
						}
					}
					else
					{
						szSelectCharator.Add(node);
					}
				}
			}
			break;
		}
        if (_boxType == EMBoxType.CHANGE||_boxType==EMBoxType.LOOK_Charator ||_boxType== EMBoxType.LOOK_Equipment || _boxType==EMBoxType.LOOK_Gem ||
            _boxType==EMBoxType.LOOK_Props || _boxType == EMBoxType.Equip_ADD_ATK|| _boxType==EMBoxType.Equip_ADD_DEF ||
            _boxType == EMBoxType.Equipment_SWAP_ATK || _boxType ==  EMBoxType.Equipment_SWAP_DEF|| _boxType == EMBoxType.SELECT_GEM_INLAY||
			_boxType == EMBoxType.SELECT_EQUIPMENT_INLAY || _boxType == EMBoxType.SELECT_EQUIPMENT_RECAST ||_boxType == EMBoxType.LOOK_AtkFrag ||
			_boxType == EMBoxType.LOOK_DefFrag || _boxType == EMBoxType.LOOK_MonFrag)
		{
            lblStrength.text = "";
			UpdateSimpleDetail();
		}

		btn_Ok.isEnabled = szSelectCharator.Count > 0;
		if(_boxType == EMBoxType.SELL_Charator || _boxType == EMBoxType.SELL_Equiement || _boxType == EMBoxType.SELL_GEM 
						|| _boxType == EMBoxType.QiangHua || _boxType == EMBoxType.Equip_QH_ATK || _boxType == EMBoxType.Equip_QH_DEF || _boxType == EMBoxType.DECOMPOSE_MONSTER)
		{
			UpdateSellMoney();
		}
		CheckBagItemNewState ();
	}

	//检测背包道具的new状态
	void CheckBagItemNewState()
	{
		BagOfStatus status = new BagOfStatus ();
		status.status = BagOfStatus.STATUS_NORMAL;
		switch (_itemType)
		{
			case EMItemType.Charator:
				foreach (SQYNodeForBI bi in szSelectCharator)
				{
					Monster mon = bi._boxItem.curData as Monster;

					if (mon.isNew)
					{
						m_bNeedSave = true;
						mon.isNew = false;
						status.pid = mon.pid;
						status.num = mon.num;
						Core.Data.AccountMgr.setStatus (status);
					}
				}
				break;
			case EMItemType.Equipment:
				foreach (SQYNodeForBI bi in szSelectCharator)
				{
					Equipment equip = bi._boxItem.curData as Equipment;
					if (equip.isNew)
					{
						m_bNeedSave = true;
						equip.isNew = false;
						status.pid = equip.RtEquip.id;
						status.num = equip.RtEquip.num;
						Core.Data.AccountMgr.setStatus (status);
					}
				}
				break;
			case EMItemType.Gem:
				foreach (SQYNodeForBI bi in szSelectCharator)
				{
					Gems gem = bi._boxItem.curData as Gems;
					if (gem.isNew)
					{
						m_bNeedSave = true;
						gem.isNew = false;
						status.pid = gem.id;
						status.num = gem.configData.ID;
						Core.Data.AccountMgr.setStatus (status);
					}
				}
				break;
			case EMItemType.Props:
				foreach (SQYNodeForBI bi in szSelectCharator)
				{
					Item item = bi._boxItem.curData as Item;
					if (item.isNew)
					{
						m_bNeedSave = true;
						item.isNew = false;
						status.pid = item.RtData.id;
						status.num = item.configData.ID;
						Core.Data.AccountMgr.setStatus (status);
					}
				}
				break;
//			case EMItemType.Soul:
//				foreach (SQYNodeForBI bi in szSelectCharator)
//				{
//					Soul soul = bi._boxItem.curData as Soul;
//					if (soul.isNew)
//					{
//						m_bNeedSave = true;
//						soul.isNew = false;
//						status.pid = soul.m_RTData.id;
//						status.num = soul.m_config.ID;
//						Core.Data.AccountMgr.setStatus (status);
//					}
//				}
//				break;
		}
				

		UpdateBtnTips ();
		SQYMainController.mInstance.UpdateBagTip ();
	}

	void OpenMonEvoveUI()
	{
		DBUIController.mDBUIInstance.SetViewState(RUIType.EMViewState.HIDE_TEAM_VIEW);
		DBUIController.mDBUIInstance.HiddenFor3D_UI();
		TrainingRoomUI.OpenUI (ENUM_TRAIN_TYPE.MonsterEvolve);
	}



	void OnBtnCharator ()
	{
		if(TrainingRoomUI.mInstance != null || FrogingSystem.ForgingRoomUI.Instance != null)
		{
			return;
		}

		if(_boxType == EMBoxType.LOOK_Equipment || _boxType == EMBoxType.LOOK_Gem || _boxType == EMBoxType.LOOK_Props )   //|| _boxType == EMBoxType.LOOK_Soul)
		{
            lblStrength.text = "";
			SetPetBoxType(EMBoxType.LOOK_Charator);
		}
	}

	void OnBtnEquipment ()
	{
		if(TrainingRoomUI.mInstance != null || FrogingSystem.ForgingRoomUI.Instance != null)
		{
			return;
		}

		if(_boxType == EMBoxType.LOOK_Charator || _boxType == EMBoxType.LOOK_Gem || _boxType == EMBoxType.LOOK_Props) //|| _boxType == EMBoxType.LOOK_Soul)
		{
            lblStrength.text = "";
			SetPetBoxType(EMBoxType.LOOK_Equipment);
		}
	}

	void OnBtnProps ()
	{
		if(TrainingRoomUI.mInstance != null || FrogingSystem.ForgingRoomUI.Instance != null)
		{
			return;
		}

		if(_boxType == EMBoxType.LOOK_Charator || _boxType == EMBoxType.LOOK_Gem || _boxType == EMBoxType.LOOK_Equipment)// || _boxType == EMBoxType.LOOK_Soul)
		{
            lblStrength.text = "";
			SetPetBoxType(EMBoxType.LOOK_Props);
		}
	}

	void OnBtnGem()
	{
		if(TrainingRoomUI.mInstance != null || FrogingSystem.ForgingRoomUI.Instance != null)
		{
			return;
		}

		if(_boxType == EMBoxType.LOOK_Charator || _boxType == EMBoxType.LOOK_Props || _boxType == EMBoxType.LOOK_Equipment)// || _boxType == EMBoxType.LOOK_Soul)
		{
            lblStrength.text = "";
			SetPetBoxType(EMBoxType.LOOK_Gem);
		}
	}
	void OnBtnMonFrag()
	{
		if(TrainingRoomUI.mInstance != null || FrogingSystem.ForgingRoomUI.Instance != null)
		{
			return;
		}

//		if(_boxType == EMBoxType.LOOK_Charator || _boxType == EMBoxType.LOOK_Props || _boxType == EMBoxType.LOOK_Equipment || _boxType == EMBoxType.LOOK_Gem)
		if(_boxType == EMBoxType.LOOK_AtkFrag || _boxType == EMBoxType.LOOK_DefFrag)// || _boxType == EMBoxType.LOOK_Equipment || _boxType == EMBoxType.LOOK_Gem)
		{
            lblStrength.text = "";
			SetPetBoxType(EMBoxType.LOOK_MonFrag);
		}
	}

	void OnBtnAtkFrag()
	{
		if(TrainingRoomUI.mInstance != null || FrogingSystem.ForgingRoomUI.Instance != null)
		{
			return;
		}

		//		if(_boxType == EMBoxType.LOOK_Charator || _boxType == EMBoxType.LOOK_Props || _boxType == EMBoxType.LOOK_Equipment || _boxType == EMBoxType.LOOK_Gem)
		if(_boxType == EMBoxType.LOOK_MonFrag || _boxType == EMBoxType.LOOK_DefFrag)// || _boxType == EMBoxType.LOOK_Equipment || _boxType == EMBoxType.LOOK_Gem)
		{
			lblStrength.text = "";
			SetPetBoxType(EMBoxType.LOOK_AtkFrag);
		}
	}

	void OnBtnDefFrag()
	{
		if(TrainingRoomUI.mInstance != null || FrogingSystem.ForgingRoomUI.Instance != null)
		{
			return;
		}

		//		if(_boxType == EMBoxType.LOOK_Charator || _boxType == EMBoxType.LOOK_Props || _boxType == EMBoxType.LOOK_Equipment || _boxType == EMBoxType.LOOK_Gem)
		if(_boxType == EMBoxType.LOOK_AtkFrag || _boxType == EMBoxType.LOOK_MonFrag)// || _boxType == EMBoxType.LOOK_Equipment || _boxType == EMBoxType.LOOK_Gem)
		{
			lblStrength.text = "";
			SetPetBoxType(EMBoxType.LOOK_DefFrag);
		}
	}


	void OnBtnDecompose()
	{
		if (_boxType == EMBoxType.LOOK_Charator)
		{
			int count = 0;
			for (short i = 3; i < 7; i++)
			{
				List<Monster> list = Core.Data.monManager.getMonsterListByStar (i, SplitType.Split_If_InTeam);
				if (list != null)
				{
					count += list.Count;
				}
			}

			if (count == 0)
			{
				SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(5169));
				return;
			}
			SetPetBoxType (EMBoxType.DECOMPOSE_MONSTER);
		}
		else if (_boxType == EMBoxType.LOOK_Props)
		{
			viewWillHidden ();
			UIDragonMallMgr.GetInstance().OpenUI(ShopItemType.Item, OpenShopCallback);
		}
	}

	void OpenShopCallback()
	{
		UIMiniPlayerController.Instance.SetActive (true);
		viewWillShow ();
	}

	/// <summary>
	/// 点击了返回按钮
	/// </summary>
	public void OnBtnBack ()
	{
       
		switch(_boxType)
		{
		case EMBoxType.CHANGE:
			if(TeamUI.mInstance != null)
			{
				TeamUI.mInstance.SetShow(true);
				SQYUIManager.getInstance ().opIndex = TeamUI.mInstance.mSelectIndex;
                UIMiniPlayerController.Instance.gameObject.SetActive(false);
			}
			else
			{
				DBUIController.mDBUIInstance.ShowFor2D_UI();
			}
			viewWillHidden();
			break;
            case EMBoxType.QiangHua:
                if (SQYPetBoxController.enterStrengthIndex == 2)
                {
                    viewWillHidden();
					TeamUI.mInstance.SetShow(true);
                    UIMiniPlayerController.Instance.SetActive(false);
					TeamUI.mInstance.RefreshMonster (SQYUIManager.getInstance().opMonster);
                }
                else if (SQYPetBoxController.enterStrengthIndex == 1)
                {
                    SetPetBoxType(EMBoxType.LOOK_Charator);
                }

            ClearStar();

			break;

            case EMBoxType.Equip_ADD_ATK:
            case EMBoxType.Equip_ADD_DEF:
                viewWillHidden();
                UIMiniPlayerController.Instance.SetActive(false);
			TeamUI.mInstance.SetShow (true);
			break;

		case EMBoxType.SELL_Charator :
			SetPetBoxType(EMBoxType.LOOK_Charator);
			break;

		case EMBoxType.SELL_GEM:
			SetPetBoxType(EMBoxType.LOOK_Gem);
			break;

		case EMBoxType.SELL_Equiement:
			SetPetBoxType(EMBoxType.LOOK_Equipment);
			break;
		case EMBoxType.Equip_QH_ATK:
		case EMBoxType.Equip_QH_DEF:
		case EMBoxType.Equipment_SWAP_ATK:
		case EMBoxType.Equipment_SWAP_DEF:
			if (TeamUI.mInstance != null)
			{
				TeamUI.mInstance.SetShow (true);
				TeamUI.mInstance.FreshCurTeam ();
				UIMiniPlayerController.Instance.SetActive(false);
				viewWillHidden ();
			}
			else
			{
				SetPetBoxType (EMBoxType.LOOK_Equipment);
			}
			break;

		case EMBoxType.GEM_HECHENG_MAIN:
		case EMBoxType.GEM_HECHENG_SUB:
			FrogingSystem.ForgingRoomUI.Instance.Visible=true;
			FrogingSystem.ForgingRoomUI.Instance.SyntheticSystem.ClearLastSelected();
			viewWillHidden();
			break;
		case EMBoxType.SELECT_EQUIPMENT_INLAY:
			FrogingSystem.ForgingRoomUI.Instance.Visible=true;
			viewWillHidden();
			break;
		case EMBoxType.SELECT_GEM_INLAY:
			FrogingSystem.ForgingRoomUI.Instance.Visible=true;
			viewWillHidden();
			break;
		case EMBoxType.SELECT_EQUIPMENT_RECAST:
			FrogingSystem.ForgingRoomUI.Instance.Visible=true;
			viewWillHidden();
			break;
		case EMBoxType.HECHENG_SHENREN_MAIN:
		case EMBoxType.HECHENG_SHENREN_SUB:
		case EMBoxType.HECHENG_ZHENREN_MAIN:
		case EMBoxType.HECHENG_ZHENREN_SUB:
		case EMBoxType.ZHENREN_HE_SHENREN_MAIN:
		case EMBoxType.ZHENREN_HE_SHENREN_SUB:
            viewWillHidden ();
            TrainingRoomUI.mInstance.SetShow (true);
			TrainingRoomUI.mInstance.m_hechengUI.RepositionCard ();

            TrainingRoomUI.mInstance.m_hechengUI.initXX();
            break;
		case EMBoxType.ATTR_SWAP:
            viewWillHidden ();
            TrainingRoomUI.mInstance.SetShow (true);

            break;
		case EMBoxType.QIANLI_XUNLIAN:
			viewWillHidden ();
			TrainingRoomUI.mInstance.SetShow (true);
            TrainingRoomUI.mInstance.m_qianLiUI.initXX();
			break;
		case EMBoxType.LOOK_Equipment:
		case EMBoxType.LOOK_Gem:
		case EMBoxType.LOOK_Props:
		case EMBoxType.LOOK_Charator:

            if (star != null )
            {
                star.gameObject.SetActive(false);
                star.ClearS();
            }

			viewWillHidden ();
			DBUIController.mDBUIInstance.ShowFor2D_UI();
			break;
		case EMBoxType.LOOK_AtkFrag:
		case EMBoxType.LOOK_DefFrag:
		case EMBoxType.LOOK_MonFrag:
			if (TopMenuUI.mInstance != null) 
			{
				DBUIController.mDBUIInstance.HiddenFor3D_UI ();
				DBUIController.mDBUIInstance._PVERoot.SetActive (true);
				if (FinalTrialMgr.GetInstance ()._PvpShaluBuouRoot != null) {
					FinalTrialMgr.GetInstance ()._PvpShaluBuouRoot.SetActive (true);
				}

				TopMenuUI.OpenUI ();

				if (FightRoleSelectPanel.Instance != null)
					UIMiniPlayerController.Instance.SetActive (false);
			}
			else
			{
				DBUIController.mDBUIInstance.ShowFor2D_UI ();
			}
			viewWillHidden ();
            break;

		case EMBoxType.DECOMPOSE_MONSTER:
			if (TrainingRoomUI.mInstance != null)
			{
				TrainingRoomUI.mInstance.SetShow (true);
				TrainingRoomUI.mInstance.m_monEvolveUI.FreshUI();
				viewWillHidden ();
			}
			else
			{
				SetPetBoxType (EMBoxType.LOOK_Charator);
			}
			break;
		case EMBoxType.EVOLVE_MONSTER:
			viewWillHidden ();
			TrainingRoomUI.mInstance.SetShow (true);
			TrainingRoomUI.mInstance.m_monEvolveUI.FreshUI();
			break;
		default:
			viewWillHidden();
			DBUIController.mDBUIInstance.ShowFor2D_UI();
			break;
		}
				
		ClearItemNewState (resetItemType (_boxType));
	}
	/// <summary>
	/// 点击了确定按钮
	/// </summary>
	public void OnBtnOK ()
	{
		switch (_boxType)
		{
		case EMBoxType.CHANGE:
			ChangeTeam ();
			break;

		case EMBoxType.LOOK_Charator:
			SetPetBoxType (EMBoxType.SELL_Charator);
			break;

		case EMBoxType.SELL_Charator:
			SellCharator ();
			break;

		case EMBoxType.SELL_Equiement:
			SellEquipment();
			break;

		case EMBoxType.SELL_GEM:
			SellGem();
			break;

		case EMBoxType.QiangHua:
			QiangHua ();
			break;

		case EMBoxType.LOOK_Equipment:
			SetPetBoxType (EMBoxType.SELL_Equiement);
			break;

		case EMBoxType.LOOK_Gem:
			SetPetBoxType (EMBoxType.SELL_GEM);
			break;

		case EMBoxType.Equip_QH_ATK:
		case EMBoxType.Equip_QH_DEF:
			StrengthEquip();
			break;

		case EMBoxType.Equip_ADD_ATK:
		case EMBoxType.Equip_ADD_DEF:
			AddEquip();
			break;

		case EMBoxType.Equipment_SWAP_ATK:
		case EMBoxType.Equipment_SWAP_DEF:
			SwapEquip ();
			break;

		case EMBoxType.LOOK_Props:
			UseProp ();
			break;
//		case EMBoxType.LOOK_Soul:
		case EMBoxType.LOOK_AtkFrag:
		case EMBoxType.LOOK_DefFrag:
		case EMBoxType.LOOK_MonFrag:
			SoulHeCheng ();
			break;
		case EMBoxType.HECHENG_ZHENREN_MAIN:
		case EMBoxType.HECHENG_ZHENREN_SUB:
		case EMBoxType.ZHENREN_HE_SHENREN_MAIN:
		case EMBoxType.ZHENREN_HE_SHENREN_SUB:
		case EMBoxType.HECHENG_SHENREN_MAIN:
		case EMBoxType.HECHENG_SHENREN_SUB:
			if (szSelectCharator.Count != 0  && szSelectCharator[0] != null)
			{
               
				Monster mon = szSelectCharator [0]._boxItem.curData as Monster;
                   
                    if (_boxType == EMBoxType.HECHENG_SHENREN_SUB
					    || _boxType == EMBoxType.HECHENG_ZHENREN_SUB
					    || _boxType == EMBoxType.ZHENREN_HE_SHENREN_SUB)
                    {   
                        if (mon.inTeam)
                        {
                            SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(5099));
                            return;
                        }
                    } 
               
               
				TrainingRoomUI.mInstance.SetShow (true);
                TrainingRoomUI.mInstance.m_hechengUI.SetData(mon);
                viewWillHidden();

                TrainingRoomUI.mInstance.m_hechengUI.initXX();
			}
			break;
		case EMBoxType.ATTR_SWAP:
			if (szSelectCharator.Count != 0  && szSelectCharator[0] != null)
			{
				Monster mon = szSelectCharator [0]._boxItem.curData as Monster;
				TrainingRoomUI.mInstance.SetShow (true);
				TrainingRoomUI.mInstance.m_attrSwapUI.SetData(mon);
				viewWillHidden ();
			}
			break;
		case EMBoxType.QIANLI_XUNLIAN:



			if (szSelectCharator.Count != 0  && szSelectCharator[0] != null)
			{
              

             
				Monster mon = szSelectCharator [0]._boxItem.curData as Monster;
				TrainingRoomUI.mInstance.SetShow (true);
				TrainingRoomUI.mInstance.m_qianLiUI.SetData(mon);
				viewWillHidden ();
                TrainingRoomUI.mInstance.m_qianLiUI.initXX();


             
               

			}
			break;
		case EMBoxType.GEM_HECHENG_MAIN:
		case EMBoxType.GEM_HECHENG_SUB:
			if (szSelectCharator.Count != 0  && szSelectCharator[0] != null)
			{
               
				Gems gemdata = szSelectCharator [0]._boxItem.curData as Gems;			    			
				FrogingSystem.ForgingRoomUI.Instance.SyntheticSystem.SelectGem(gemdata);
				FrogingSystem.ForgingRoomUI.Instance.Visible=true;
				viewWillHidden ();
			}
			break;
		case EMBoxType.SELECT_EQUIPMENT_INLAY:
		    if (szSelectCharator.Count != 0  && szSelectCharator[0] != null)
			{
				FrogingSystem.ForgingRoomUI.Instance.Visible=true;
				Equipment equip = szSelectCharator [0]._equipment.curData as Equipment;
				FrogingSystem.ForgingRoomUI.Instance.InlaySystem.SelectEquipment(equip);
				viewWillHidden ();
			}
			break;
		case EMBoxType.SELECT_GEM_INLAY:
			if (szSelectCharator.Count != 0  && szSelectCharator[0] != null)
			{
				FrogingSystem.ForgingRoomUI.Instance.Visible=true;
				Gems gemdata = szSelectCharator [0]._boxItem.curData as Gems;
			    FrogingSystem.ForgingRoomUI.Instance.InlaySystem.SelectGem(gemdata);				
				viewWillHidden ();
			}
			break;
		case EMBoxType.SELECT_EQUIPMENT_RECAST:
			if (szSelectCharator.Count != 0  && szSelectCharator[0] != null)
			{
				FrogingSystem.ForgingRoomUI.Instance.Visible=true;
				Equipment equdata = szSelectCharator [0]._equipment.curData as Equipment;
			    FrogingSystem.ForgingRoomUI.Instance.RecastingSystem.SelectEquipment(equdata);
				viewWillHidden ();
			}
		    break;
		case EMBoxType.EVOLVE_MONSTER:
			if (szSelectCharator.Count != 0  && szSelectCharator[0] != null)
			{
				Monster mon = szSelectCharator [0]._boxItem.curData as Monster;
				if (mon.RTData.curLevel < 60)
				{
					SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (5127));
					return;
				}

				TrainingRoomUI.mInstance.SetShow (true);
				TrainingRoomUI.mInstance.m_monEvolveUI.SetSelData(mon);
				viewWillHidden();
			}
			break;
		case EMBoxType.DECOMPOSE_MONSTER:
			DecomposeMonster ();
			break;

		}
	}

	void OnBtnSell()
	{
		switch (_boxType)
		{
			case EMBoxType.LOOK_Charator:
				SetPetBoxType (EMBoxType.SELL_Charator);
				break;
			
			case EMBoxType.LOOK_Equipment:
				SetPetBoxType (EMBoxType.SELL_Equiement);
				break;

			case EMBoxType.LOOK_Gem:
				SetPetBoxType (EMBoxType.SELL_GEM);
				break;
		}
	}

	void OnBtnShop()
	{
		if(!Core.Data.guideManger.isGuiding)
		{
			if (_boxType == EMBoxType.QiangHua)
			{
				JCRestoreEnergyMsg.OpenUI (ItemManager.SILVER_EXP_PIG, ItemManager.SILVER_PIG_PACKAGE, 4);
            }
            else if (_boxType == EMBoxType.Equip_QH_ATK )
            {

            JCRestoreEnergyMsg.OpenUI (ItemManager.SILVER_GUN_ONE, ItemManager.SILVER_GUN, 5);

            }
			else if ( _boxType == EMBoxType.Equip_QH_DEF)
			{
            	JCRestoreEnergyMsg.OpenUI (ItemManager.SILVER_HUWAN_ONE, ItemManager.SILVER_HUWAN, 3);
			}
		}
	}

	public void ShowBag()
	{
		viewWillShow ();
        UIMiniPlayerController.Instance.HideFunc();
		SetPetBoxType(_boxType);
	}
		
    void StrengthBtn()
	{
        if(szSelectCharator.Count == 0 || szSelectCharator.Count >1)
        {
            return;
        }
			
        Monster monster = szSelectCharator[0]._boxItem.curData as Monster;
		if (monster.RTData.curLevel >= 60)
		{
			SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString (5139));
			return;
		}
			
        SQYUIManager.getInstance ().opMonster = monster;
        DBUIController.mDBUIInstance.SetViewState (RUIType.EMViewState.S_Bag, RUIType.EMBoxType.QiangHua);
        enterStrengthIndex = 1;
        //  ShowUI (false);
    }

	public void PageChange()
	{
		szSelectCharator.Clear ();
		m_bagItemOprtUI.SetShow(false);
	}
}
