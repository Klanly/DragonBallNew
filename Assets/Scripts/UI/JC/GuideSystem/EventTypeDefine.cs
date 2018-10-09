using UnityEngine;
using System.Collections;

public enum EventTypeDefine 
{
	GoToFate_WuKong = -1,                        //返回小悟空缘配齐的界面
	
	Open_RecruitingRoom =1,                      //打开招募屋
	Click_RecruitingRoom_NewFighters =2 ,        //点击新武者
	Click_RecruitingRoom_MartialArtist =3,       //点击武术家
	Click_RecruitingRoom_SuperPowers =4,         //点击超能者
	Click_RecruitingRoom_Redeemer =5,            //点击救世主
	Exit_RecruitingRoom = 6,                                 //退出招募屋
	Open_BattleArray = 7,                                    //点击阵容按钮，展开列表
	Click_BattleArray_Add =8,								 //点击阵容的＋号，进入背包
	Click_FourStarHero = 9,									 //选择4星英雄
	Click_GoToBattle = 10,									 //点击上阵
	Click_FB = 11,											 //点击副本
	Click_FirstChapter = 12,								 //选择第一大章
	Click_RunButton = 13,									 //点击执行
	Click_ReceiveButton = 14,							     //点击收取
	Click_GoToBossFight = 15,							     //跳转BOSS战
	Click_RunBossFight = 16,							     //点击执行，进入BOSS战
	Play_FightAnimation = 17,								 //播放战斗
			
						
	Exit_MainFB = 18,                                    //点击返回，从副本返回主界面
	OpenTreasureChest = 19,								 //高亮宝箱区域，选择一个宝箱
	ExitFightingScene = 20,							     //点击关闭战斗
	Exit_FBtoMainScene = 21,						     //点击返回，一键从副本返回游戏主界面
	
	Click_Mokey = 22,								//点击阵容小悟空
	Click_Atk_Equip = 23,							//点击装备武器，进入背包装备栏
	Click_Gold_Cudgel = 24,							//点击选择金箍棒
	Click_EquipBtn = 25,							//点击装备按钮，返回人物信息界面
	Click_Badake = 26,								//点击巴达克
	Click_StrengthInTeam = 27,						//点击强化，进入背包
	Click_First_SubMon = 28,						//点击第一张1星卡
	Click_Second_SubMon = 29,						//点击第二张1星卡
	Click_Third_SubMon = 30,						//点击第三张1星卡
	Click_StrengthInBag = 31,						//点击强化，返回阵容
	Click_TeamToMainUI = 32,						//点击返回，返回主界面
	Click_CreateProdeceBuild = 36,					//点击建造产出建筑
	Click_BuildGetMoney = 37,						//点击经济建筑收钱
	Click_EnterProduceBuild = 38,					//点击进入经济建筑
	Click_ProduceBuildGetNow = 39,					//经济建筑马上收钱



	//Click_EnterXunLianWu = 54,						//进入训练屋
	//Click_QianliXunLian = 55,						    //点击潜力训练
	//Click_QianliXunLian_MainCard = 56,				//点击主卡
	//Click_SelMainCardInBag = 57,					    //潜力训练在背包选择主卡
	//Click_OKInBag = 58,								//点击确定，选择巴达克，返回潜力训练

	Click_GetFouStarMonCard = 63,						 //招募收取四星宠物卡片
	Click_CloseMonsterLevelUpBox = 64,                   //关闭宠物升级框
	Click_BagToZhenRong = 65,                            //返回阵容
	
	Click_TeamToMainUI2 = 66,                           //点击返回,从副本主界面返回主界面，并重置3D场景位置
	Click_CreateBulid = 67,                             //点击确定创建建筑
	Click_GetBuildCoin = 68,                            // 点击建筑的确定收取金币
	
	
	Click_Activity = 40,                                 //点击活动
	Click_ActivityGiftBag = 41,                          //点击等级礼包
	Click_GetActivityGiftBag = 42,                       //点击领取
	
	Click_DuoBaoButton = 44,                              //点击夺宝
	Click_MartialConference = 45,                         //点击武道大会
	Click_Exchange_DuoBao = 46,                           //点击兑换，切换兑换页
	Click_GetChaoShenShui = 47,                           //兑换一个超圣水
	Click_BackToDuoBao = 48,                              //点击返回，返回夺宝界面
	Click_DuoBaoBackToMain =49,                           //点击返回，返回主界面
	
	Click_EnterXunLianWu = 50,                            //点击进入训练屋
	Click_QianliXunLian = 51,                             //点击进入潜力训练
	Click_QianliXunLian_MainCard = 52,                    //点击选择主卡，进入背包
	Click_SelMainCardInBag = 53,                          //点击选择巴达克
	Click_OKInBag = 54,                                   //点击确定，选择巴达克，返回潜力训练
	
	
	
	Click_AnyWhereToMainScene = 56,               //点击任意地方后，强制返回主界面
	Click_ShenLong = 57,                                     //点击神龙
	Click_SixStarBall = 58,                                    //点击6号龙珠
	Click_RobSixStarBall =59,                               //固定只推送一个机器人，点击抢夺按钮
	Click_BackToShengLongMain = 69,                //返回龙珠主界面
	Click_CallOfDragon = 61,                                 //点击神龙合成按钮
	Click_ChooseAoYi = 62,                                   //选择奥义
	Click_YesAtDragonUI = 70,                              //召唤神龙以后点击确定
    Click_XiaoWuKongVBiKe = 71,                   //小悟空对战比克大魔王，属性克制
	Click_XiaoWuKong_OS_4 = 200,                  //小悟空怒气技能（教学）狂揍-释放4倍
	Click_XiaoWuKong_OS_4_2 = 207,                //小悟空怒气技能（教学）狂揍-释放4倍
	Click_XiaoWuKong_OS_4_3 = 208,                //小悟空怒气技能（教学）狂揍-释放4倍
	Click_XiaoWuKong_OS_4_4 = 209,                //小悟空怒气技能（教学）狂揍-释放4倍

	Click_WuKong2_Anger   = 201,                  //孙悟空2继承小悟空20怒气
	Click_WuKong2_OS_4    = 202,                  //孙悟空2释放终结技能
	Click_WuKong3_OS_4    = 203,                  //孙悟空3释放终结技能
	Click_BILUSI_V_WuKong3= 204,                  //比鲁斯
	Click_WuKong3_NO_ANGER= 205,                  //没有怒气值不能释放主动技能
	Click_BuErMa_Show     = 206,                  //布尔玛出现
	Click_WuKong3_Vs_BiLu = 210,                  //孙悟空3对决比鲁斯
	Click_XiaoWuKong_Lv1  = 211,                  //小悟空第一关的释放技能

	Click_AddNewRole = 72,                        //点击添加新人物      
	Click_FightWithFulisa = 73,                   //跳转打弗利萨
	Click_ExitLevelFiveReward= 74,                //退出五级奖励界面
	Click_HideUIGuide = 75,                       //隐藏新手引导
	Click_FangJU = 76,                            //点击装备防具，进入背包装备栏
	Click_JinDouYun = 77,                    //点击选择筋斗云
	Click_EquJinDouYun = 78,               //点击装备按钮，返回人物信息界面
	Click_AddRole3 = 79,                       //点击阵容的＋号，进入背包3号位
	Click_SelectGuiXianRen = 80,          //选择龟仙人
	Click_UpGuiXianRen = 81,               //点击上阵龟仙人
	Click_EquipAoYi = 83,                       //点击穿戴奥义
	Click_SelectedFirstAoYi = 84,           //选择第一个奥义
	Click_SureEquipAoYi = 85,               //点击确定穿戴这个奥义
	Click_LookZuHeSkill = 86,                 //点击查看组合技能
	Click_SevenRewardButton = 87,          //点击七日礼包
	Click_GetSevenReward = 88,               //点击领取七日礼包
	Click_CloseSevenReward = 89,            //点击关闭七日礼包
	Click_JumpToStrengthening  = 90,                                       //点击失败副本跳转强化按钮
	Click_RunTiaoZhanBtn =91,                  //点击挑战(天下第一)按钮
	Click_SigninButton = 92,                       //点击等级礼包
	Click_GetSigninRewardButton = 93,     //点击领取第一天的奖励(签到)
	Click_CloseSignin = 94,                        //点击关闭签到
	
	
	#region 3级引导
	Add_ChongFengQiangToBag = 95,           //对话(添加一个冲锋枪到背包)
	Click_BagBtn = 96,                  //点击背包
	Click_FristGrid = 97,                                      //点击金箍棒(背包的第一个)
	Click_QiangHuaEquipBtn = 98,                //点击装备强化按钮
	Click_SelectFristToEat = 99,          //选择要吃的武器(背包第一个)
	Click_QiangHuaBtn = 100,                   //点击强化按钮
	#endregion

	#region 5级新增引导
	Add_GetLv1RedGem = 101,            //获得一个1级蓝宝石
	Click_OpenFrogingSystem = 102,      //打开装备锻造屋
	Click_BackTo3DMain =104,             //点击返回，返回主界面(并设置主界面位置)
	Click_CloseCombinationSkillPanel = 105,    //关闭组合技能显示板
	Click_OpenGemMosaic= 103,         //打开宝石镶嵌界面
	Click_AddEquipment = 106,             //点击添加按钮
	Click_LeftGemSlot = 109,                //点击左边的宝石孔
	
	#region LS
	Click_Shop = 133,                     //点击商城
	Click_BuyFiveStarEgg = 134,       //点击五星扭蛋
	Click_SureBuyFiveStarEgg = 135,     //点击确定购买五星扭蛋
	Click_ExitShop = 136,     //退出商城
	#endregion

	#region ZQ
	Click_SelectFristEquip = 107,           //点击背包的第一个装备(金箍棒)
	Click_SureJinGuBang = 108,          //点击确定按钮(确定选择金箍棒)
	Click_SelectFristGem = 110,          //点击背包的第一个宝石(一级红宝石)
	Click_SureRedGem = 111,            //点击确定按钮(确定选择一级红宝石)
	
	#endregion
	
	Click_MainLineTask = 112,       //任务弱引导(点击了主线任务按钮)
	Click_TaskGoBtn = 113,           //任务弱引导(点击了Go按钮)
	
	#endregion
	
	
	#region 每日奖励
	Click_DayRewardButton = 114,           //点击每日奖励(new)
	Click_SevenRewardUI = 115,             //领取了七日礼包(new)
	Click_CloseDayRewardPanel = 118,   //关闭了每日奖励(new)
	#endregion
	
    Click_ExitQiangHuaBag = 119,      //退出背包强化界面
	
	Click_ShowSunAndBuErMa = 120,   //显示太阳和布尔玛

	#region 10.9
	Add_GetJinGuBang = 121,           //布尔玛送金箍棒
	Click_WuZhe = 122,         //点击武者按钮
	Click_JuQingFB = 123,   //点击剧情副本
	Click_FirstGuanKa = 124,     //点击第一关
	Click_FightDesButton = 125,       //点击描述战斗按钮
	Click_FightButton = 126,    //点击战斗按钮
	Click_QiangHuaToBag = 127,      //强化界面返回背包界面
	Click_BagToMainScene = 128,     //背包界面返回主界面
	Click_SecondGuanKa = 129,     //点击第二关
	Click_ExitPlotFB = 130,      //退出剧情副本
	Click_ThirdGuaKa = 131,    //点击第三关
	Click_FourthGuaKa = 132,    //点击第四关
	Sliding_Texture = 137 ,     //手势滑动漫画
	Click_CloseTexture = 138,     //关闭副本漫画
	UnLock_God_And_SecondPos  =139,   //开启第二槽位以及天神招募屋
	Click_SelectSecondRoleAtFightPanel = 140,     //在战斗选择界面选择第二个人
	Click_SelectFristRoleAtFightPanel = 141,     //在战斗选择界面选择第一个人
	
	
	Click_OpenMenuAtFB = 142,    //点击打开副本界面侧边的菜单栏
	Click_MenuAtFB_Squad = 143,    //点击打开副本界面侧边的菜单栏
	Click_SkillButton = 144,           //点击升级技能按扭
	
	Click_OpenTaskPage1 = 145,     //第一次打开任务面板
	Click_CloseTaskPage = 146,    //关闭任务面板
	Click_GetTaskRewardButton = 147, //领取奖励按钮
	Click_OpenTaskPage2 = 148,     //第二次打开任务面板
	
	Click_SuperCard = 149,     //点击超级武者卡牌
	Click_BuyOneCard = 150,   //点购买一个
	Click_ReturnToSquad = 151,   //强化界面返回阵容界面
	Click_ExitSquad = 152,    //退出阵容界面
	Click_JinGuBang = 153,  //点击金箍棒
	Click_JuQingFB_Special = 154,  //重要: 点击剧情副本，但这里是特殊处理<因为这里会影响下一步到底是点第四关还是点第五关(默认是第四关)>
	Click_FifthGuaKa = 155,    //点击第五关
	Click_ReturnHome = 156,   //点击返回家园
	#endregion



	Add_GetJinDouYun = 157,//新版引导




}
