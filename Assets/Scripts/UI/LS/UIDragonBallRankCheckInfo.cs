using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIDragonBallRankCheckInfo : RUIMonoBehaviour 
{
	
	public UILabel[] mItemName;
	public UILabel[] mItemNum;
	public UISprite[] mItemIcon;
	public UISprite[] mItemCircle;
	public UISprite[] mbg;


	public UILabel[] SkillLabel;
	public UILabel[] ConsistSkillLabel;
	public UILabel LevelLabel;
	public UILabel mName;
	public UISprite mIconAttr;
	public UILabel mAttack;
	public UILabel mDefense;

	public UISprite[] mStar1;
	public UISprite[] mStar2;

	public StarsUI m_stars;
	public StarsUI m_extStars;

	public GameObject mRoot;

	public RankRoleIcon _RankRoleIcon = null;

	private int m_teamindex;
	private int m_equipteamindex;

	EquipInfo[] _EquipInfo = null;
//	EquipTeam _EquipTeam = null;
	MonsterTeamInfo teaminfo = null;
	MonsterTeamInfo teaminfoTemp = null;
	MonsterTeam m_MonTeam = null;

	List<EquipInfo> _EquipInfoList = new List<EquipInfo>();
	List<MonsterInfo> _MonsterInfoList = new List<MonsterInfo>();
	int Capacity = 0;

	UIGrid mGrid;
	UIScrollView mView;

	int starnum1, starnum2;
	string str1, str2;

	Vector3 m1 = new Vector3(-440.0f,-46.0f,0.0f);

	float mm = 9.5f;

	public void OnShow(GetNewFinalTrialRankCheckInfoResponse mresponse)
	{
		m_teamindex = 0;
		if(mGrid == null || mView == null)
		{
			FindGrid();
        }
		if(mresponse == null || mresponse.data == null)return;
		int index = 0;


		if(mresponse.data.detail != null)
		{
			_EquipInfo = mresponse.data.detail.eqip;
			if(mresponse.data.currTeam > 0)
			{
				foreach(MonsterTeamInfo monsterteaminfo in mresponse.data.detail.monteam)
				{
					if(monsterteaminfo.teamid == mresponse.data.currTeam)
					{
						break;
					}
					m_teamindex++;
				}
			} 
			for(int z=0; z<mresponse.data.detail.monteam.Length; z++)
			{
				if(mresponse.data.detail.monteam[z].teamid == mresponse.data.currTeam)
				{
					teaminfo = mresponse.data.detail.monteam[z];
					Capacity = Core.Data.playerManager.GetUserLvlInfo(mresponse.data.lv).petSlot;
				}
			}
		}

		m_equipteamindex = -1;
		if(mresponse.data.detail != null && mresponse.data.detail.equip != null)
		{
			for(int i=0; i<mresponse.data.detail.equip.Length; i++)
			{
				if(mresponse.data.detail.equip[i].id == mresponse.data.currTeam)
				{
					m_equipteamindex = i;
					mresponse.data.detail.equip[m_equipteamindex].HumanReadable();
					break;
				}
			}
		}


		
		GameObject obj1 = PrefabLoader.loadFromPack("LS/pbLSRankRole") as GameObject;

		SortedDictionary<string, int> dic = new SortedDictionary<string, int>();
		foreach(string key in teaminfo.mon.Keys)
		{
			dic.Add(key,teaminfo.mon[key]);
		}
        
		teaminfoTemp = new MonsterTeamInfo();
		teaminfoTemp.teamid = teaminfo.teamid;
		teaminfoTemp.mon = new Dictionary<string, int>();
		foreach(string keys in dic.Keys)
		{
			for(int i=0; i<mresponse.data.detail.monster.Length; i++)
			{
				int id = 0;
				if(dic.TryGetValue(keys,out id))
				{
					if(mresponse.data.detail.monster[i].id == id)
					{
						_MonsterInfoList.Add(mresponse.data.detail.monster[i]);
						teaminfoTemp.mon.Add(keys,mresponse.data.detail.monster[i].num);
						if(obj1 != null)
						{
							GameObject go = NGUITools.AddChild (mGrid.gameObject, obj1);

	//						go.gameObject.name = (1000 + i).ToString();
							RankRoleIcon mm = go.gameObject.GetComponent<RankRoleIcon>();
							DragonBallRankMgr.GetInstance().mRankRoleIcon.Add (mm);
							mm.gameObject.SetActive(true);

							Monster mon = mresponse.data.detail.monster[i].toMonster(Core.Data.monManager);

							if(mresponse.data.detail.equip != null && m_equipteamindex >= 0)mm.InitUI(mon, index, mresponse.data.detail.equip[m_equipteamindex].EquipIdList[index], mresponse.data.detail.monteam);	
							else 
							{
								mm.InitUI(mon, index, new int[]{}, mresponse.data.detail.monteam);
							}
							break;
	                    }
	                    
					}
				}
			}
			index++;

		}

		_RankRoleIcon = DragonBallRankMgr.GetInstance().mRankRoleIcon[0];
		_RankRoleIcon.SetChoose(true);
		DragonBallRankMgr.GetInstance()._NowChooseIndex = 0;
        
		SetChoose();

		//自己组装monsterteam 用于打开缘界面做准备
		SetMonsterTeam();

		mGrid.Reposition();
		mView.ResetPosition();
	}

	public void SetChoose()
	{
		_EquipInfoList.Clear();
		SetDetail(_RankRoleIcon.EquipData);

		Vector3 pos = m_stars.transform.localPosition;
		if (_RankRoleIcon.Data.Star == 1)
		{
			float val = (m_stars.m_listStars.Length - 1) / 2.0f;
			pos.x = val * m_stars.m_Width * -1;
			m_stars.m_pos = ENUM_POS.left;
		}
		else
		{
			pos.x = 0;
			m_stars.m_pos = ENUM_POS.middle;
		}
		m_stars.transform.localPosition = pos;
		m_extStars.transform.localPosition = pos;
		RED.SetActive (_RankRoleIcon.Data.Star == 1, m_extStars.gameObject);
		m_stars.SetStar (_RankRoleIcon.Data.Star);
		m_extStars.SetStar (6);
		
		m_stars.gameObject.transform.localPosition = new Vector3(m_stars.gameObject.transform.localPosition.x-300.0f, m_stars.gameObject.transform.localPosition.y, m_stars.gameObject.transform.localPosition.z);
		m_extStars.gameObject.transform.localPosition = new Vector3(m_extStars.gameObject.transform.localPosition.x-300.0f, m_extStars.gameObject.transform.localPosition.y, m_extStars.gameObject.transform.localPosition.z);
		
		mName.text = _RankRoleIcon.Data.config.name;
		LevelLabel.text = _RankRoleIcon.Data.RTData.curLevel.ToString();
		int attr = (int)(_RankRoleIcon.Data.RTData.Attribute); 
		mIconAttr.spriteName = "Attribute_" + attr.ToString();
		mAttack.text = _RankRoleIcon.Data.getAttack.ToString();
		mDefense.text = _RankRoleIcon.Data.getDefend.ToString();

		for(int j=0; j<SkillLabel.Length; j++)
		{
			SkillLabel[j].SafeText("");
			SkillLabel[j].gameObject.SetActive(false);
		}

		for(int i=0; i<_RankRoleIcon.Data.config.skill.Length; i++)
		{
		    SkillLabel[i].gameObject.SetActive(true);
			SkillData skill = Core.Data.skillManager.getSkillDataConfig( _RankRoleIcon.Data.config.skill[i]);
			if(skill != null)SkillLabel[i].text = Core.Data.skillManager.getSkillDataConfig( _RankRoleIcon.Data.config.skill[i]).name;
			
		}

		for(int j=0; j<ConsistSkillLabel.Length; j++)
		{
			ConsistSkillLabel[j].text = "";
		}

		List<FateData> fateList = _RankRoleIcon.Data.getMyFate(Core.Data.fateManager);
		int value = 0;
		int count = fateList.Count;

		for (int i = 0; i < count; i++)
		{
			
			ConsistSkillLabel[i].text = fateList[i].name;
			if(checkMyFate(fateList[i], teaminfo, Capacity))
			{
				value ++;
				ConsistSkillLabel[i].color = new Color (1f,227f/255,43f/255);
			}
			else
			{
				ConsistSkillLabel[i].color = Color.white;
			}
		}

		mItemIcon[0].MakePixelPerfect();
		mItemIcon[1].MakePixelPerfect();


	}

	public void SetDetail(int[] data)
	{
		for(int i=0; i<mItemIcon.Length; i++)
		{
			mItemIcon[0].spriteName = "equip0";
			mItemIcon[1].spriteName = "equip1";
			mItemNum[i].gameObject.SetActive(false);
			mItemName[i].gameObject.SetActive(false);
//			mbg[i].gameObject.SetActive(false);
//			mItemCircle[i].gameObject.SetActive(false);
        }

		for(int st=0; st<mStar1.Length; st++)
		{
			mStar1[st].gameObject.SetActive(false);
			mStar2[st].gameObject.SetActive(false);
		}

        if(data == null)return;
		if(data.Length == 0 )return;
		else
		{
			for(int z=0; z<data.Length; z++)
			{
				for(int j=0; j<_EquipInfo.Length; j++)
				{
					if(data[z] == _EquipInfo[j].id)	
					{
						_EquipInfoList.Add(_EquipInfo[j]);
                        SetEquipDetail(_EquipInfo[j]);
                        
                    }
				}
			}
		}
	}

	void SetEquipDetail(EquipInfo _info)
	{
		EquipData edata = Core.Data.EquipManager.getEquipConfig(_info.num);
        
		if(edata.type == 0)
		{
			mItemIcon[0].gameObject.SetActive(true);
			mItemNum[0].gameObject.SetActive(true);
			mItemName[0].gameObject.SetActive(true);
			mbg[0].gameObject.SetActive(true);
			mItemCircle[0].gameObject.SetActive(true);

			if(edata != null)
			{
				mItemName[0].text = edata.name;
				mItemNum[0].text = "";
				mItemIcon[0].spriteName = _info.num.ToString();
			}
			
			int mCount = 0;
			while(mCount < edata.star)
			{
				if(mStar1[mCount] != null)mStar1[mCount].gameObject.SetActive(true);
				mStar1[mCount].transform.localPosition = new Vector3(m1.x+(mCount+1)*2*mm,m1.y,m1.z);
				mCount++;
			}
			for(int z=0; z<mCount; z++)
			{
				mStar1[z].transform.localPosition = new Vector3(mStar1[z].transform.localPosition.x-(mCount+1)*mm,m1.y,m1.z); ;
            }
            for(;mCount<5; mCount++)
            {
                if(mStar1[mCount] != null)mStar1[mCount].gameObject.SetActive(false);
            }
        }
        else
        {
			mItemIcon[1].gameObject.SetActive(true);
			mItemNum[1].gameObject.SetActive(true);
			mItemName[1].gameObject.SetActive(true);
			mbg[1].gameObject.SetActive(true);
			mItemCircle[1].gameObject.SetActive(true);
			
			if(edata != null)
			{
				mItemName[1].text = edata.name;
				mItemNum[1].text = "";
				mItemIcon[1].spriteName = _info.num.ToString();
			}
			
			
			int mCount = 0;
			while(mCount < edata.star)
			{
				if(mStar2[mCount] != null)mStar2[mCount].gameObject.SetActive(true);
				mStar2[mCount].transform.localPosition = new Vector3(m1.x+(mCount+1)*2*mm,m1.y,m1.z);
				mCount++;
			}
			for(int z=0; z<mCount; z++)
			{
				mStar2[z].transform.localPosition = new Vector3(mStar2[z].transform.localPosition.x-(mCount+1)*mm,m1.y,m1.z); ;
            }
            for(;mCount<5; mCount++)
            {
                if(mStar2[mCount] != null)mStar2[mCount].gameObject.SetActive(false);
            }
        }
    }
    
    // Use this for initialization
    void FindGrid ()
    {
        mGrid = mRoot.gameObject.GetComponentInChildren<UIGrid>();
		mView = mRoot.gameObject.GetComponent<UIScrollView>();
	}
	

	void Back_OnClick()
	{
		gameObject.SetActive(false);
		DestroyAll();
	}

	void DestroyAll()
	{
		_EquipInfoList.Clear();
		_MonsterInfoList.Clear();
        DragonBallRankMgr.GetInstance().DestoryChild();
		Destroy(gameObject);
	}

	void GetObject( int pid, string _str, int _starnum)
	{
		ConfigDataType type = DataCore.getDataType(pid);
		switch(type)
		{
			case ConfigDataType.Monster:
				_str = Core.Data.monManager.getMonsterByNum(pid).name;
				_starnum = Core.Data.monManager.getMonsterByNum(pid).star;
				break;
			case ConfigDataType.Item:
				_starnum = Core.Data.itemManager.getItemData(pid).star;
				_str = Core.Data.itemManager.getItemData(pid).name;
				break;
			case ConfigDataType.Equip:
				_starnum = Core.Data.EquipManager.getEquipConfig(pid).star;
				_str = Core.Data.EquipManager.getEquipConfig(pid).name;
				break;
			case ConfigDataType.Gems:
				_starnum = Core.Data.gemsManager.getGemData(pid).star;
				_str = Core.Data.gemsManager.getGemData(pid).name;
				break;
			default:
				RED.LogError(" not found  : " +  pid);
				break;
		}
	}

	bool checkMyFate(FateData faDa, MonsterTeamInfo mTeam, int capacity)
	{
		bool check = true;
		if(faDa != null && mTeam != null) 
		{
			int count = faDa.CountOfCondition;
			for(int i = 0; i < count; ++ i)
			{
				int[] condition = faDa.MyFate(i);
				if(condition != null)
				{
					ConfigDataType conData = (ConfigDataType)condition[FateData.Type_Pos];
					int ID = condition[FateData.Item_ID_Pos];
					int amount = condition[FateData.Item_Count_Pos];
					
					switch(conData)
					{
						case ConfigDataType.HugeBeast:
							check = false;
							break;
						case ConfigDataType.Gender:
							check = false;
							break;
						case ConfigDataType.Monster:
							check = checkMonster(ID, amount, mTeam, capacity) && check;
							break;
	                    case ConfigDataType.Star:
							check = false;
							break;
	                    case ConfigDataType.Equip:
							check = checkEquip(ID, amount, capacity) && check;
                        break;
                    }
                    
                }
            }
            
		}
		
		return check;
	}

	private bool checkEquip(int EquipNum, int count, int capacity) {
		int requiredCount = 0;

		for(int pos = 0; pos < _EquipInfoList.Count; ++ pos) 
		{
			if(_EquipInfoList[pos].num == EquipNum) 
			{
				requiredCount ++;
			}
        }
        
        return requiredCount >= count;
		
	}

	private bool checkMonster(int MonNum, int count, MonsterTeamInfo team, int capacity) {
		
		int totalCount = 0;

		for(int i=0; i<_MonsterInfoList.Count; i++)
		{
			if(MonNum == _MonsterInfoList[i].num)
			{
				totalCount ++;
			}
		}
        
        return totalCount >= count; 
	}

	private void Skill_OnClick(GameObject obj)
	{
//		switch(obj.name)
//		{
//			case "skillbtn1":
//				SkillPanelScript.ShowpbSkillPanel (DBUIController.mDBUIInstance._TopRoot, _RankRoleIcon.skilllist[0]);
//				break;
//			case "skillbtn2":
//				SkillPanelScript.ShowpbSkillPanel (DBUIController.mDBUIInstance._TopRoot, _RankRoleIcon.skilllist[1]);
//				break;
//			case "skillbtn3":
//				SkillPanelScript.ShowpbSkillPanel (DBUIController.mDBUIInstance._TopRoot, _RankRoleIcon.skilllist[2]);
//				break;
//		}
	}

	private void SetMonsterTeam()
	{
		GetNewFinalTrialRankCheckInfoResponse res = DragonBallRankMgr.GetInstance().m_response;
		if(res.data.detail != null && res.data.detail.monteam != null && res.data.detail.monteam .Length != 0 && res.data.detail.monteam[m_teamindex].mon != null)
		{
			m_MonTeam = new MonsterTeam(1, res.data.detail.monteam[m_teamindex].mon.Count);
		}
		else
		{
			m_MonTeam = new MonsterTeam(1, 1);
		}
		foreach(string keys in res.data.detail.monteam[m_teamindex].mon.Keys)
		{
			int id = 0;
			if(res.data.detail.monteam[m_teamindex].mon.TryGetValue(keys, out id))
			{
				for(int i=0; i<res.data.detail.monster.Length; i++)
				{
					if(id == res.data.detail.monster[i].id)
					{
						Monster mon = res.data.detail.monster[i].toMonster(Core.Data.monManager);
						int pos = int.Parse (keys);
						pos = pos % 100;
						pos -= 1;
						m_MonTeam.setMember(mon, pos);
						EquipInfo info = null;
						if(res.data.detail.equip != null && res.data.detail.equip.Length != 0)
						{
							if(m_equipteamindex >= 0)
							{
								if(res.data.detail.equip[m_equipteamindex].EquipIdList[pos] != null)
								{
									foreach(int _id in res.data.detail.equip[m_equipteamindex].EquipIdList[pos])
									{
										for(int t=0; t<res.data.detail.eqip.Length; t++)
										{
											if(res.data.detail.eqip[t].id == _id)
											{
												info = res.data.detail.eqip[t];
												EquipData data = Core.Data.EquipManager.getEquipConfig(info.num);
												Equipment ment = new Equipment(info, data, Core.Data.gemsManager);
												m_MonTeam.setEquip(ment, pos);
											}
										}
									}
								}
							}
						}
						break;
					}
				}
			}
		}
	}

	private void ConsistSkill_OnClick(GameObject obj)
	{
		SetMonsterTeam();
		CombinationSkillPanelScript.ShowpbCombinationSkillPanel(DBUIController.mDBUIInstance._TopRoot, _RankRoleIcon.Data,false, m_MonTeam);
	}

	private void Equipment_OnClick(GameObject obj)
	{

	}

}
