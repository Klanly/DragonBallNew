using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentShowCellScript : MonoBehaviour {
	
	/// <summary>
	/// 装备名称
	/// </summary>
	public UILabel NameLabel;

	/// <summary>
	/// 装备图标
	/// </summary>
	public UISprite EquipmentICON;

	/// <summary>
	/// 装备单元的索引
	/// </summary>
	public int Index;

    public StarsUI starObj;

	public UILabel m_txtTip;

	public Equipment MyCurrentEquipment; 

	public UILabel m_txtLv;

	public GameObject m_emptyTip;

	void Start()
	{
		if(Index == 0)
		{
			m_txtTip.text = Core.Data.stringManager.getString(5144);
		}
		else if(Index == 1)
		{
			m_txtTip.text = Core.Data.stringManager.getString(5145);
		}
	}

	public void OnClick()
	{
		if (null!= MyCurrentEquipment) 
		{
            //            Debug.Log (" get atk = " + MyCurrentEquipment.getAttack + " atk = " + MyCurrentEquipment.ConfigEquip.atk);
            Core.Data.temper.equipAtk = MyCurrentEquipment.getAttack;
            Core.Data.temper.equipDef = MyCurrentEquipment.getDefend;
			EquipmentPanelScript.OpenUI(MyCurrentEquipment, true, HideZhengRong,ActiveZhengRong);			
		}
		else
		{
            Core.Data.temper.equipAtk = 0;
            Core.Data.temper.equipDef = 0;
			List<Equipment> list = Core.Data.EquipManager.GetEquipList (Index, SplitType.Split_If_InCurTeam);
			if (list == null || list.Count == 0)
			{
				SQYAlertViewMove.CreateAlertViewMove (Core.Data.stringManager.getString(5237));
				return;
			}

			if(Index == 0)
            {
				DBUIController.mDBUIInstance.SetViewState(RUIType.EMViewState.S_Bag, RUIType.EMBoxType.Equip_ADD_ATK);
            }
			else
			{
				DBUIController.mDBUIInstance.SetViewState(RUIType.EMViewState.S_Bag, RUIType.EMBoxType.Equip_ADD_DEF);
			}

			HideZhengRong();
		}
	}
	
	
	void HideZhengRong()
	{
		TeamUI.mInstance.SetShow (false);
	}
	
	void ActiveZhengRong()
	{
		ShowTeamInfo ();
	}
	
	
	void OpenShopUI()
	{
		UIDragonMallMgr.GetInstance().OpenUI(ShopItemType.Egg, ShowTeamInfo);
		HideZhengRong ();
	}
		
	void ShowTeamInfo()
	{
		//		DBUIController.mDBUIInstance.SetViewState(RUIType.EMViewState.S_Team_NoSelect);
		TeamUI.mInstance.SetShow (true);
	}

	public void Show(Equipment e,int index)
	{
		Index = index;
		if (e == null)
		{
			NameLabel.gameObject.SetActive (false);
			EquipmentICON.spriteName = "equip" + Index.ToString();
			EquipmentICON.width = 60;
			EquipmentICON.height = 60;
			MyCurrentEquipment = null;
            if (starObj != null)
            {
                starObj.gameObject.SetActive(false);
              
            }
			m_txtLv.text = "";

			int count = Core.Data.EquipManager.GetValidEquipCount (index, SplitType.Split_If_InCurTeam);
			RED.SetActive(count > 0, m_emptyTip);
			RED.SetActive(true, m_txtTip.gameObject);
			return;
		}

		MyCurrentEquipment = e;
		EquipmentICON.width = 80;
		EquipmentICON.height = 80;
		NameLabel.gameObject.SetActive (true);
		EquipmentICON.spriteName = MyCurrentEquipment.ConfigEquip.ID.ToString();
		NameLabel.text = e.name;
		m_txtLv.text = "Lv" + e.RtEquip.lv.ToString ();
        if (starObj != null)
        {
            starObj.gameObject.SetActive(true);
            starObj.SetStar(e.ConfigEquip.star);
        }
		RED.SetActive(false, m_txtTip.gameObject, m_emptyTip);
	}
}
