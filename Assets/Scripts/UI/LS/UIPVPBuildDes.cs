using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIPVPBuildDes : RUIMonoBehaviour 
{
	public UILabel mCurDungeon;
	public UILabel mDes;
	public UILabel mName;
	public UILabel mAtkOrDefValue;
	public UISprite AtkOrDefIcon;
	public UISprite[] mItem;
	public List<JCPVEPlotDesMonsterHead> mMonsterHeadList;

	private int mID;
	private int m_Members;


	public void OnShow(NewMapFinalTrial m_NewMapFinalTrial, string m_Name, ItemOfReward[] _member)
	{
		for(int i=0; i<mItem.Length; i++)
		{
			mItem[i].gameObject.SetActive(false);
		}
		if(_member.Length <= 5 && _member.Length >= 3)
		{
			for(int j=0; j<_member.Length; j++)
			{
				mMonsterHeadList[j].gameObject.SetActive(true);
				mMonsterHeadList[j].SetData(_member[j]);
            }
		}


		mID = m_NewMapFinalTrial.Data.ID;
		m_Members = m_NewMapFinalTrial.Data.members;
		mCurDungeon.SafeText(m_NewMapFinalTrial.Data.name);
		mDes.SafeText(m_NewMapFinalTrial.Data.Des);
		mName.SafeText(m_Name);
		if(FinalTrialMgr.GetInstance().NowEnum == TrialEnum.TrialType_ShaLuChoose)
		{
			mAtkOrDefValue.SafeText(Core.Data.playerManager.RTData.curTeam.teamAttack.ToString());
			AtkOrDefIcon.spriteName = "common-0008";
		}
		else if(FinalTrialMgr.GetInstance().NowEnum == TrialEnum.TrialType_PuWuChoose)
		{
			mAtkOrDefValue.SafeText(Core.Data.playerManager.RTData.curTeam.teamDefend.ToString());
			AtkOrDefIcon.spriteName = "common-0010";
		}

		for(int i=0; i<m_NewMapFinalTrial.Data.Reward.Count; i++)
		{
			mItem[i].gameObject.SetActive(true);

			ConfigDataType datatype = DataCore.getDataType(m_NewMapFinalTrial.Data.Reward[i][0]);
			if (datatype == ConfigDataType.Monster)
			{
				AtlasMgr.mInstance.SetHeadSprite (mItem[i], m_NewMapFinalTrial.Data.Reward[i][0].ToString ());
				continue;
			}
			else if (datatype == ConfigDataType.Equip)
			{
				mItem[i].atlas = AtlasMgr.mInstance.equipAtlas;
			}
			else if (datatype == ConfigDataType.Gems)
			{
				mItem[i].atlas = AtlasMgr.mInstance.commonAtlas;
			}
			else if (datatype == ConfigDataType.Item)
			{
				mItem[i].atlas = AtlasMgr.mInstance.itemAtlas;
			}
			else if(datatype == ConfigDataType.Frag)
			{
				int id = (m_NewMapFinalTrial.Data.Reward[i][0]%150000)+10000;
				if (DataCore.getDataType(id) == ConfigDataType.Monster)
				{
					AtlasMgr.mInstance.SetHeadSprite (mItem[i], id.ToString ());
					continue;
				}
				else if (DataCore.getDataType(id) == ConfigDataType.Item)
				{
					mItem[i].atlas = AtlasMgr.mInstance.itemAtlas;
				}
				
			}

			mItem[i].spriteName = m_NewMapFinalTrial.Data.Reward[i][0].ToString();
		}

	}

	void BeginBattle()
	{
		if(FinalTrialMgr.GetInstance().NowEnum == TrialEnum.TrialType_ShaLuChoose)
		{
			FightRoleSelectPanel.OpenUI(m_Members,SelectFightPanelType.SHALU_BATTLE,  0).CallBack_Fight = (int[] array,int teamID) => 
			{
				if(array.Length == 0)
				{
					SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(9092));
				}
				else
				{
					FinalTrialMgr.GetInstance().NewFinalTrialCurTeamRequest(mID, array, teamID);
				}

			};
		}
		else if(FinalTrialMgr.GetInstance().NowEnum == TrialEnum.TrialType_PuWuChoose)
		{
			FightRoleSelectPanel.OpenUI(m_Members,SelectFightPanelType.SHALU_BATTLE,  1).CallBack_Fight = (int[] array,int teamID) => 
			{
				if(array.Length == 0)
				{
					SQYAlertViewMove.CreateAlertViewMove(Core.Data.stringManager.getString(9092));
				}
				else
				{
					FinalTrialMgr.GetInstance().NewFinalTrialCurTeamRequest(mID, array, teamID);
				}
			};
		}

//		FinalTrialMgr.GetInstance().NewFinalTrialCurTeamRequest(mID);
	}

	void BackOnclick()
	{
		gameObject.SetActive(false);
	}

	static public UIPVPBuildDes CreatePanel()
	{
		Object obj = PrefabLoader.loadFromPack("LS/pbLSPVPPlotDes");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			if(go != null)
			{
				RED.AddChild(go, DBUIController.mDBUIInstance._bottomRoot);
				UIPVPBuildDes script = go.GetComponent<UIPVPBuildDes>();
				return script;
			}
		}
		return null;
	}
}
