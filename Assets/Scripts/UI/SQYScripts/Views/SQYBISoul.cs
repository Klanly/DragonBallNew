using UnityEngine;
using System.Collections;

public class SQYBISoul : SQYBoxItem
{
	UILabel m_txtName;
	UILabel m_txtCnt;
	UISprite m_spHead;
	UISprite m_spAttr;
	UISprite m_spMask;

	public Soul m_data;

	protected override void initBoxItem ()
	{
		if(bInitBox)return;
		base.initBoxItem ();

		m_txtName = allLabels [0];
		m_txtCnt = allLabels [1];
		m_spHead = allSprites [0];
		m_spAttr = allSprites [1];
		m_spMask = allSprites [2];
	}


	public override void freshBoxItemWithData (object obj)
	{
		base.freshBoxItemWithData(obj);
		m_data = obj as Soul;
		if (m_data.m_config.type == (int)ItemType.Monster_Frage)
		{
			curItemType = RUIType.EMItemType.MonFrag;
		}
		else if (m_data.m_config.type == (int)ItemType.Equip_Frage)
		{
			EquipData equip = Core.Data.EquipManager.getEquipConfig (m_data.m_config.updateId);
			if (equip != null)
			{
				if(equip.type == 0)
					curItemType = RUIType.EMItemType.AtkFrag;
				else
					curItemType = RUIType.EMItemType.DefFrag;
			}
		}
		
		if(m_data != null)
		{
			m_txtName.text = m_data.m_config.name;
			if (m_data.m_RTData.count < m_data.m_config.quantity)
			{
				m_txtCnt.text = "[ff0000]" + m_data.m_RTData.count + "[-][00ff00]/" + m_data.m_config.quantity + "[-]";
			}
			else
			{
				m_txtCnt.text = "[00ff00]" + m_data.m_RTData.count + "/" + m_data.m_config.quantity + "[-]";
			}


			if (m_data.m_config.type == (int)(ItemType.Monster_Frage))
			{
				AtlasMgr.mInstance.SetHeadSprite (m_spHead, m_data.m_config.updateId.ToString ());
				m_spAttr.spriteName = "bag-0003";
			}
			else
			{
				m_spHead.atlas = AtlasMgr.mInstance.equipAtlas;
				m_spHead.spriteName = m_data.m_config.updateId.ToString ();
				m_spAttr.spriteName = "sui";
			}
	
			RED.SetActive (m_data.m_RTData.count == 0, m_spMask.gameObject);
			this.name = m_data.m_RTData.id.ToString();
			m_spHead.MakePixelPerfect();
			m_spAttr.MakePixelPerfect ();
		}
	}


	public static SQYBISoul CreateBISoul()
	{
		UnityEngine.Object obj = PrefabLoader.loadFromPack("SQY/pbSQYBISoul", true, true);
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			SQYBISoul cv = go.GetComponent<SQYBISoul>();
			cv.initBoxItem();
			return cv;
		}
		return null;
	}

}
