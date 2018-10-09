using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZQReceiver : EventReceiver
{
	
	private const int JINGUBANG = 40110;
	private const int CHONGFENGQIANG = 40301;
	public ZQReceiver()
	{
		
	}

	public static EventTypeDefine m_curGuide;

	protected override void OnEvent(EventTypeDefine p_e,object p_param)
	{
		m_curGuide = p_e;
		switch(p_e)
		{
			case EventTypeDefine.Open_RecruitingRoom:															//打开招募
				ZhaoMuUI.OpenUI ();
				DBUIController.mDBUIInstance.HiddenFor3D_UI ();
				break;
			case EventTypeDefine.Click_RecruitingRoom_NewFighters:												//招募1		
//				ZhaoMuUI.mInstance.ZhaoMuMon (ZhaoMuUI.mInstance.m_btnZhaoMu[0], true);
				break;
			case EventTypeDefine.Click_RecruitingRoom_MartialArtist:											//招募2
//				ZhaoMuUI.mInstance.ZhaoMuMon (ZhaoMuUI.mInstance.m_btnZhaoMu[1], true);
				break;
			case EventTypeDefine.Click_RecruitingRoom_SuperPowers:												//招募3
//				ZhaoMuUI.mInstance.ZhaoMuMon (ZhaoMuUI.mInstance.m_btnZhaoMu[2], true);
				break;
			case EventTypeDefine.Click_RecruitingRoom_Redeemer:													//招募4
//				ZhaoMuUI.mInstance.ZhaoMuMon (ZhaoMuUI.mInstance.m_btnZhaoMu[3], true);
				break;
			case EventTypeDefine.Exit_RecruitingRoom:
				ZhaoMuUI.mInstance.OnClickExit ();
				
				break;
			case EventTypeDefine.Open_BattleArray:
				SQYMainController.mInstance.OnBtnZhenRong();
				break;
			case EventTypeDefine.Click_BattleArray_Add:															//点击队伍第二个位置
			case EventTypeDefine.Click_Badake:
				TeamMonsterCell cell = TeamUI.mInstance.GetMonCellByPos(1);
				if(cell != null)
				{
					cell.OnClick();
				}
				break;
			case EventTypeDefine.Click_FourStarHero:
				SQYNodeForBI bi = SQYPetBoxController.mInstance.GetBagItem (-1);
				if (bi != null)
				{
					SQYPetBoxController.mInstance.selectOneCharator (bi);
				}
				break;
			case EventTypeDefine.Click_GoToBattle:
				SQYPetBoxController.mInstance.OnBtnOK ();
				break;
			case EventTypeDefine.Click_Mokey:																				//点击阵容小悟空
				TeamMonsterCell monkey = TeamUI.mInstance.GetMonCellByPos(0);
				if(monkey != null)
				{
					monkey.OnClick();
				}
				break;
			case EventTypeDefine.Click_Atk_Equip:																			
				EquipmentTableManager.Instance.m_atkEquip.OnClick ();
				break;
			case EventTypeDefine.Click_Gold_Cudgel:																			//点击选择金箍棒
				List<Equipment> list = Core.Data.EquipManager.GetAllEquipByNum(JINGUBANG);
				if(list != null && list.Count > 0)
				{
						SQYNodeForBI gold = SQYPetBoxController.mInstance.GetBagItem (list[0].RtEquip.id);
						if (gold != null)
						{
							SQYPetBoxController.mInstance.selectOneCharator (gold);
						}
				}
				break;
			case EventTypeDefine.Click_EquipBtn:
			case EventTypeDefine.Click_StrengthInBag:
				SQYPetBoxController.mInstance.OnBtnOK();
			    Core.Data.guideManger.HideGuide();
				break;
			case EventTypeDefine.Click_StrengthInTeam:																	//阵容点击强化
//				for(int i = 0; i < 3; i++)
//				{
//					RuntimeMonster rtData = new RuntimeMonster();
//					rtData.addStar = 0;
//					rtData.Attribute = (MonsterAttribute)(i + 1);
//					rtData.curLevel = 15;
//					rtData.curExp = 20;
//					
//					MonsterData monData = Core.Data.monManager.getMonsterByNum(10175);
//					Monster mon = new Monster(-5 - i, 10175, rtData, monData);
//					Core.Data.monManager.AddMonter(mon);
//				}

				TeamUI.mInstance.m_teamView.OnBtnQiangHua ();
				break;
			case EventTypeDefine.Click_First_SubMon:																	//强化子卡1																
				SQYNodeForBI first = SQYPetBoxController.mInstance.GetBagItemByStarAndPos (1, 0);
				if (first != null)
				{
					SQYPetBoxController.mInstance.selectOneCharator (first);
				}
				break;
			case EventTypeDefine.Click_Second_SubMon:																	//强化子卡2
				SQYNodeForBI second = SQYPetBoxController.mInstance.GetBagItemByStarAndPos (1, 1);				
				if (second != null)
				{
					SQYPetBoxController.mInstance.selectOneCharator (second);
				}
				break;
			case EventTypeDefine.Click_Third_SubMon:																	//强化子卡3
				SQYNodeForBI third = SQYPetBoxController.mInstance.GetBagItemByStarAndPos (1, 2);
				if (third != null)
				{
					SQYPetBoxController.mInstance.selectOneCharator (third);
				}
				break;
			
			case EventTypeDefine.Click_GetFouStarMonCard:																//招募获取四星宠物卡
//				ZhaoMuUI.mInstance.OnClickMain();
				break;
			case EventTypeDefine.Click_TeamToMainUI:	
				TeamUI.mInstance.OnBtnTeamViewWitnIndex (SQYTeamInfoView.BTN_BACK);
				break;
			case EventTypeDefine.Click_CreateProdeceBuild:						
				
				BuildItem build = BuildScene.mInstance.GetBuildItemByNum (830001);
				build.ClickBuild();
				break;
			case EventTypeDefine.Click_BuildGetMoney:	

				BuildItem bd = BuildScene.mInstance.GetBuildItemByNum (830001);
				bd.OnClickGet ();
				break;
			case EventTypeDefine.Click_EnterProduceBuild:																// 进入经济建筑
				BuildItem bds = BuildScene.mInstance.GetBuildItemByNum (830001);
				bds. ClickBuild();
				break;
			case EventTypeDefine.Click_ProduceBuildGetNow:																//马上收钱
				BuildLvlUpUI.mInstance.OnBtnClickOK();
				break;

			case EventTypeDefine.Click_EnterXunLianWu:																	//进入训练屋
				TrainingRoomUI.OpenUI ();
				DBUIController.mDBUIInstance.HiddenFor3D_UI ();
				break;
			case EventTypeDefine.Click_QianliXunLian:																	//进入潜力训练
				TrainingRoomUI.mInstance.OnClickTypes (TrainingRoomUI.mInstance.m_types[1]);							
				break;
			case EventTypeDefine.Click_QianliXunLian_MainCard:															//潜力训练选择主卡
				TrainingRoomUI.mInstance.m_qianLiUI.OnClickMain();
				break;
			case EventTypeDefine.Click_SelMainCardInBag:																//潜力训练背包中选择主卡
				for (short i = 5; i >= 0; i--)
				{
					List<Monster> jingubangList = Core.Data.monManager.getMonsterListByStar (i, SplitType.None);
					if (jingubangList != null && jingubangList.Count > 0)
					{
						SQYNodeForBI mon = SQYPetBoxController.mInstance.GetBagItem(jingubangList[0].pid);
						SQYPetBoxController.mInstance.selectOneCharator (mon);
						break;
					}
				}
				break;		
			case EventTypeDefine.Click_OKInBag:																			//点击确定，选择巴达克，返回潜力训练																			
				SQYPetBoxController.mInstance.OnBtnOK ();
				break;
			case EventTypeDefine.Click_CloseMonsterLevelUpBox:
			    LevelUpMsgBox.Instance.OnClose();
				break;
			case EventTypeDefine.Click_BagToZhenRong:
			case EventTypeDefine.Click_QiangHuaToBag:      //强化界面返回背包界面
			case EventTypeDefine.Click_BagToMainScene:     //背包界面返回主界面
				SQYPetBoxController.mInstance.OnBtnBack();
				break;
			case EventTypeDefine.Click_CreateBulid:
//				bd = BuildScene.mInstance.GetBuildItemByNum (830001);
//				bd.SendCreateBuildMsg ();
//				UIInformation.GetInstance ().mUIMallOldMan.OnClickExit ();
			    break;
			case EventTypeDefine.Click_GetBuildCoin:
				 UIInformation.GetInstance().mUIMallOldMan.OnClickOK();
				break;
			case EventTypeDefine.Click_AddNewRole:
				cell = TeamUI.mInstance.GetMonCellByPos(2);
				if(cell != null)
				{
					cell.OnClick();
				}
				break;
			case EventTypeDefine.GoToFate_WuKong:
				MonsterTeam curTeam = Core.Data.playerManager.RTData.curTeam;
				List<Monster> listMon = curTeam.GetMonByNum (PlayerInfo.DEFAULT_HEAD);
				for (int i = 0; i < listMon.Count; i++)
				{
					if (curTeam.IsAllFataActive (listMon [i]))
					{
						int pos = curTeam.GetMonsterPos (listMon [i].pid);
						if (TeamUI.mInstance != null)
						{
							cell = TeamUI.mInstance.GetMonCellByPos(pos);
							if(cell != null)
							{
								cell.OnClick();
							}
							break;
						}
					}
				}
				break;
			case EventTypeDefine.Click_FangJU:														//点击防具，进入背包，重新排序，把筋斗云放在第一位
				TeamUI.mInstance.SetShow (false);
				DBUIController.mDBUIInstance.SetViewState (RUIType.EMViewState.S_Bag, RUIType.EMBoxType.Equip_ADD_DEF);
				SQYPetBoxController.mInstance.GuideSortJinDouYun ();
				break;
			case EventTypeDefine.Click_JinDouYun:													//点击选择筋斗云
				bi = SQYPetBoxController.mInstance.GetBagItem (-10);
				if (bi != null)
				{
					SQYPetBoxController.mInstance.selectOneCharator (bi);
				}
				break;
			case EventTypeDefine.Click_EquJinDouYun:												//点击装备按钮
				SQYPetBoxController.mInstance.OnBtnOK ();
				break;
			
			case EventTypeDefine.Click_AddRole3:                      //点击阵容的＋号，进入背包3号位
				cell = TeamUI.mInstance.GetMonCellByPos (2);
				if (cell != null)
				{
					cell.OnClick ();
				}
				SQYPetBoxController.mInstance.GuideSortGuiXianRen ();
				break;
			case EventTypeDefine.Click_SelectGuiXianRen:          //选择龟仙人
				bi = SQYPetBoxController.mInstance.GetBagItem (-11);
				if (bi != null)
				{
					SQYPetBoxController.mInstance.selectOneCharator (bi);
				}
				break;
			case EventTypeDefine.Click_UpGuiXianRen:               //点击上阵龟仙人
				SQYPetBoxController.mInstance.OnBtnOK ();
				break;
		case EventTypeDefine.Click_LookZuHeSkill:
			TeamUI.mInstance.m_teamView.OnBtnSkillView();
			    break;

				//三级新手引导，添加冲锋枪到背包
			case EventTypeDefine.Add_ChongFengQiangToBag:
			    
			    ItemOfReward[] reward = new ItemOfReward[]{new ItemOfReward(),new ItemOfReward()};

				reward[0].ppid = -22;
				reward[0].pid = 40103;
				reward[0].lv = 1;
				reward[0].num = 1;
			    
			    reward[1].ppid = -20;
				reward[1].pid = 40103;
				reward[1].lv = 1;
				reward[1].num = 1;
			
				Core.Data.EquipManager.AddEquip (reward);
				break;
			case EventTypeDefine.Click_BagBtn:									//打开背包，看武器
				DBUIController.mDBUIInstance.SetViewState (RUIType.EMViewState.S_Bag, RUIType.EMBoxType.LOOK_Equipment);
				break;
			case EventTypeDefine.Click_FristGrid:								//点击选择金箍棒
				List<Equipment> equips = Core.Data.EquipManager.GetAllEquipByNum (JINGUBANG);
				SQYNodeForBI node = SQYPetBoxController.mInstance.GetBagItem(equips[0].RtEquip.id);
				SQYPetBoxController.mInstance.selectOneCharator (node);
				break;
			case EventTypeDefine.Click_QiangHuaEquipBtn:						//点击强化
				EquipmentPanelScript.mInstance.OnStrengBtnClick();
				break;
			case EventTypeDefine.Click_SelectFristToEat:							//吃掉冲锋枪
				equips = Core.Data.EquipManager.GetAllEquipByNum (CHONGFENGQIANG);
				node = SQYPetBoxController.mInstance.GetBagItem(equips[0].RtEquip.id);
				SQYPetBoxController.mInstance.selectOneCharator (node);
				break;
			case EventTypeDefine.Click_QiangHuaBtn:									//点击强化
			    Core.Data.guideManger.HideGuide ();
				SQYPetBoxController.mInstance.OnBtnOK ();
				break;

				//五级新手引导
			case EventTypeDefine.Click_SelectFristEquip:           //点击背包的第一个装备(筋斗云)
				equips = Core.Data.EquipManager.GetAllEquipByNum (45108);
				node = SQYPetBoxController.mInstance.GetBagItem(equips[0].RtEquip.id);
				SQYPetBoxController.mInstance.selectOneCharator (node);
				break;
			case EventTypeDefine.Click_SureJinGuBang:          //点击确定按钮(确定选择金箍棒)
				SQYPetBoxController.mInstance.OnBtnOK ();
				break;
			case EventTypeDefine.Click_SelectFristGem:          //点击背包的第一个宝石(一级红宝石)
				node = SQYPetBoxController.mInstance.GetBagItem(-21);
				SQYPetBoxController.mInstance.selectOneCharator (node);
				break;
			case EventTypeDefine.Click_SureRedGem:            //点击确定按钮(确定选择一级红宝石)
				SQYPetBoxController.mInstance.OnBtnOK ();
				break;

			case EventTypeDefine.Click_ExitQiangHuaBag:
				DBUIController.mDBUIInstance.SetViewState(RUIType.EMViewState.H_Bag);
				DBUIController.mDBUIInstance.ShowFor2D_UI();
				break;

			case EventTypeDefine.Click_WuZhe:
				SQYMainController.mInstance.OnBtnMonster ();
				break;

			case EventTypeDefine.UnLock_God_And_SecondPos:
				Core.Data.BuildingManager.ZhaoMuUnlock = true;
				Core.Data.BuildingManager.UpdateZhaoMu ();
				break;

			case EventTypeDefine.Click_OpenMenuAtFB:						//点击topmenuui的top按钮
				TopMenuUI.mInstance.OnBtnTop ();
				break;

			case EventTypeDefine.Click_MenuAtFB_Squad:						//打开阵容
				TopMenuUI.mInstance.OnBtnTeam ();
				break;

			case EventTypeDefine.Click_SkillButton:							//技能升级
				TeamUI.mInstance.m_teamView.OnBtnSkillUp ();
				break;

			case EventTypeDefine.Click_SuperCard:							//点击超级武者卡牌
				ZhaoMuUI.mInstance.OnClickZhaomu(2);
				break;

			case EventTypeDefine.Click_BuyOneCard:							//点购买一个
				ZhaoMuUI.mInstance.m_zhaomuSubUI.OnClickSubZhao (1);
			    Core.Data.guideManger.HideGuide ();
				Core.Data.temper.SetGameTouch (true);
				ZhaoMuUI.mInstance.m_zhaomuSubUI.OnClickExit ();
				break;

			case EventTypeDefine.Click_JinGuBang:							
				EquipmentTableManager.Instance.m_atkEquip.OnClick();
				break;

			case EventTypeDefine.Click_ReturnToSquad:
				SQYPetBoxController.mInstance.OnBtnBack ();
				break;
			case EventTypeDefine.Click_ExitSquad:
				TeamUI.mInstance.OnBtnTeamViewWitnIndex (SQYTeamInfoView.BTN_BACK);
				break;
		case EventTypeDefine.Click_ReturnHome:
			Building buildData	= Core.Data.BuildingManager.GetBuildFromBagByNum (830001);
			if (buildData != null) {
				Debug.Log ("  return home    ");
				buildData.RTData.openType = 1;
				buildData.RTData.dur = 0;
				buildData.fTime = new System.DateTime ();

			}					

			Debug.Log ("   return nnnnnnn   " + buildData.RTData.openType  );
			    TopMenuUI.mInstance.OnBtnHome();
				break;
		}
	}
}
