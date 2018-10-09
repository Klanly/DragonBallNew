using UnityEngine;
using System.Collections;

public class TeamMonsterCell : MonoBehaviour 
{
	public UISprite m_spHead;
	public UISprite m_spProp;
	public UISprite m_spBg;
	public UILabel m_txtLv;
	public UILabel m_txtTip;
	public UISprite m_spSel;
    public GameObject voidPosEffect;
	public GameObject m_EmptyEquip;
//	private const string ADD_SP = "headAdd";
//	private const string LOCK_SP = "head0";
	
	public Monster m_monster;
	public int m_nPos;

	public void InitUI(int pos)
	{
		m_nPos = pos;
		MonsterTeam curTeam = Core.Data.playerManager.RTData.curTeam;

		if(IsLock(pos))
		{
			RED.SetActive(false, m_spProp.gameObject, m_txtLv.gameObject,m_EmptyEquip.gameObject);
			AtlasMgr.mInstance.SetHeadSprite(m_spHead, AtlasMgr.HEAD_LOCK);
			m_spHead.width = 54;
			m_spHead.height = 61;

			string strTip = "";
			if (pos != 1)
			{
				strTip = Core.Data.stringManager.getString (5091);
				strTip = string.Format (strTip, Core.Data.playerManager.GetMonSlotUnLockLvl (pos + 1));
			}
			else
			{
				strTip = Core.Data.stringManager.getString (9111);
				strTip = string.Format (strTip, RED.GetChineseNum (4));
			}
			m_txtTip.text = strTip;
		}
		else
		{
			m_monster = curTeam.getMember(pos);
			if(m_monster != null)
			{
				RED.SetActive(true, m_spBg.gameObject, m_spProp.gameObject, m_txtLv.gameObject);
				AtlasMgr.mInstance.SetHeadSprite(m_spHead,m_monster.num.ToString());
				m_spBg.spriteName = "star" + ((int)(m_monster.RTData.m_nAttr)).ToString();
				m_spProp.spriteName = "Attribute_" + ((int)(m_monster.RTData.Attribute)).ToString();
				m_txtLv.text = "Lv" + m_monster.RTData.curLevel.ToString();



                if (voidPosEffect != null) 
				{
                    RED.SetActive (false,voidPosEffect);
                }

				int count = 0;
				for (short i = 0; i < 2; i++)
				{
					Equipment equip = Core.Data.playerManager.RTData.curTeam.getEquip (pos, i);
					if (equip == null)
					{
						if(Core.Data.EquipManager.GetValidEquipCount(i, SplitType.Split_If_InCurTeam) > 0)
						{
							count++;
						}
					}
				}
				RED.SetActive (count > 0, m_EmptyEquip);
				m_spHead.MakePixelPerfect ();
			}
			else
			{
				RED.SetActive(false, m_spBg.gameObject, m_spProp.gameObject, m_txtLv.gameObject);
                //AtlasMgr.mInstance.SetHeadSprite(m_spHead, AtlasMgr.HEAD_ADD);
                if (voidPosEffect != null)
				{

					RED.SetActive (true,voidPosEffect);
					int count = Core.Data.monManager.GetValidMonCount(SplitType.Split_If_InCurTeam);
					RED.SetActive(count > 0, m_EmptyEquip.gameObject);
                }
				m_spHead.width = 100;
				m_spHead.height = 100;
			}

			m_txtTip.text = "";
		}
	}

	public static bool IsLock(int pos)
	{
		if(pos == 1)
		{
			return !TeamUI.secondPosUnLock;
		}
		else
		{
			UserLevelInfo curLevelInfo =  Core.Data.playerManager.curConfig;
		
			if(pos >= curLevelInfo.petSlot)
			{
				return true;
			}
		}
		return false;
	}

	public void SetSelected(bool bSel)
	{
		RED.SetActive (bSel, m_spSel.gameObject);
	}
	
	public void OnClick()
	{
		if(TeamModifyUI.mInstance != null)
		{
			TeamModifyUI.DestroyUI();
		}

		if (UIWXLActivityMainController.Instance != null)
				UIWXLActivityMainController.Instance.OnBtnClick ();

//		SQYUIManager.getInstance().opIndex = m_nPos;

		m_monster = Core.Data.playerManager.RTData.curTeam.getMember (m_nPos);
		if(m_monster == null)
		{
			if (!IsLock (m_nPos)) {
				SQYUIManager.getInstance ().opMonster = null;
				DBUIController.mDBUIInstance.SetViewState (RUIType.EMViewState.S_Bag, RUIType.EMBoxType.CHANGE);
				Core.Data.temper.infoPetAtk = 0;
				Core.Data.temper.infoPetDef = 0;

				//当前选中 monster
				Core.Data.temper.curShowMonster = null;

				SQYUIManager.getInstance().opIndex = m_nPos;
				if (TeamUI.mInstance != null) {
					TeamUI.mInstance.SetShow (false);
				}
			}
//			else {
//				SQYUIManager.getInstance ().opIndex = 0;
//			}
		}
		else
		{
			TeamUI.mInstance.ClickTeamMonster(this);
			DBUIController.mDBUIInstance.HiddenFor3D_UI (false);
			Core.Data.temper.curShowMonster = m_monster;
			Core.Data.temper.curSelectEquip = null;
			Core.Data.temper.bagSelectMonster = null;
			SQYUIManager.getInstance().opIndex = m_nPos;
		}
	}
}
