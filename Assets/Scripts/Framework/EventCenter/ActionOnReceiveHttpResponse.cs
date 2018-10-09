using System;
using System.Collections.Generic;

/// <summary>
/// All the internal work should be defined here when http's response is coming.
/// </summary>
public class ActionOnReceiveHttpResponse {

	public readonly static Dictionary<RequestType, Action<BaseHttpRequest, BaseResponse>> ACTION_LIST = new Dictionary<RequestType, Action<BaseHttpRequest, BaseResponse>>() {
		{ 
			RequestType.LOGIN_GAME,         (r,t) => { Core.Data.AccountMgr.analyseBag(); Sync_Operation(t); }
		},
		{ 
			RequestType.THIRD_PARTY_LOGIN,  (r,t) => { Core.Data.AccountMgr.analyseBag(); Sync_Operation(t); }
		},

		{
			RequestType.CHANGE_TEAM_MEMBER, (r,t) => { Core.Data.playerManager.ChangeTeamMember(r, t, Core.Data.monManager); } 
		},

		{
			RequestType.CHANGE_EQUIPMENT,   (r,t) => { Core.Data.playerManager.ChangeTeamEquip(r, t, Core.Data.EquipManager); }
		},

		{
			RequestType.PVE_BATTLE,         (r,t) => { Core.Data.dungeonsManager.OnFinishFloor(r,t); Core.Data.monManager.addItem(t); Core.Data.EquipManager.addItem(t); 
                                                        Core.Data.itemManager.addItem(t); Core.Data.playerManager.addItem2(r,t); 
														Core.Data.soulManager.addItem(t); }
		},

		{
			RequestType.SETTLE_BOSSBATTLE, (r,t) => {  Core.Data.monManager.addItem(t); Core.Data.EquipManager.addItem(t); 
														Core.Data.itemManager.addItem(t); Core.Data.playerManager.addItem2(r,t); 
														Core.Data.soulManager.addItem(t); Core.Data.gemsManager.addItem(t);}
		},

		{
			RequestType.SETTLE_SHABU,      (r,t) => { Core.Data.itemManager.addItem(t as ClientBTShaBuResponse); }
		},

		{
			RequestType.SYNC_PVE,      (r,t) => { Core.Data.newDungeonsManager.SyncPveData(t); }
		},

		{
			RequestType.RESET_FLOOR,      (r,t) => { Core.Data.newDungeonsManager.ResetFloor(r, t); }
		},

		{
			RequestType.SYNC_MONEY,      (r,t) => { Core.Data.playerManager.SyncMoney(t); }
		},

		{
			RequestType.DECOMPOSE_MONSTER,   (r,t) => { Core.Data.itemManager.DecomposeMonster(t); Core.Data.monManager.DecomposeMonster(r, t); Core.Data.playerManager.DecomposeMon(t);}
		},

		{
			RequestType.EVOLVE_MONSTER,      (r,t) => {Core.Data.monManager.EvolveMonster(t); Core.Data.itemManager.EvolveMonster(r, t); Core.Data.playerManager.EvolveMonster(t);}
		},

		{
			RequestType.SM_PVE_BATTLE,      (r,t) => { Core.Data.dungeonsManager.OnFinishFloor(r,t); Core.Data.monManager.addItem(t); Core.Data.EquipManager.addItem(t); 
                                                        Core.Data.itemManager.addItem(t); Core.Data.playerManager.addItem2(r,t); 
														Core.Data.soulManager.addItem(t);}
		},
		{
			RequestType.SELL_MONSTER,       (r,t) => { Core.Data.monManager.sellMonster(r,t); Core.Data.playerManager.SellMonster(t);}
		},

		{
			RequestType.STRENGTHEN_MONSTER, (r,t) => { Core.Data.monManager.StrengthenMonster(r, t); Core.Data.playerManager.StrengthenMonster(t);}
		},

		{
			RequestType.STRENGTHEN_EQUIPMENT, (r,t) => { Core.Data.EquipManager.StrengthEquip(r, t); Core.Data.playerManager.StrengthEquip(t);}
		},

		{
			RequestType.SELL_EQUIPMENT,      (r,t) => { Core.Data.EquipManager.SellEquip(r, t); Core.Data.playerManager.SellEquip(t);}
		},
		{
			RequestType.SOULHECHENG,         (r,t) => { Core.Data.monManager.SoulHeCheng(t); Core.Data.soulManager.SoulHeCheng(r, t);  Core.Data.EquipManager.SoulHeCheng(t); }
		},
		{
            RequestType.SELL_GEM,            (r,t) => { Core.Data.gemsManager.SellGem(r, t); Core.Data.playerManager.SellEquip(t);}
		},
		{
			RequestType.USE_PROP,            (r,t) => {Core.Data.itemManager.UseItem(r, t); Core.Data.playerManager.UseProp(r,t);  Core.Data.monManager.UseProp(t);  
												Core.Data.gemsManager.UsePropSuc(t);	Core.Data.EquipManager.UserProp(t); Core.Data.soulManager.UseProp(t);}
		},
		{
			RequestType.SWAP_TEAM,           (r,t) => {Core.Data.playerManager.SwapTeam(r,t); }
		},
		{
			RequestType.SWAP_MONSTER_POS,    (r,t) => {Core.Data.playerManager.SwapMonsterPos(r,t); }
		},

        {
			RequestType.BUY_ITEM,            (r,t) => {Core.Data.playerManager.BuyItem(t); Core.Data.itemManager.addItemshop(t);  Core.Data.EquipManager.AddShopItem(t); Core.Data.gemsManager.BuyItemSuc(t);
												Core.Data.soulManager.BuyItemSuc(t); Core.Data.monManager.BuyItemSuc(t);
		}
        },
		{
			RequestType.ZHANGONG_BUY_ITEM,     (r,t) => {Core.Data.itemManager.addItemshop(t); Core.Data.EquipManager.AddShopItem(t); Core.Data.gemsManager.BuyItemSuc(t);Core.Data.soulManager.BuyItemSuc(t);
				Core.Data.monManager.BuyItemSuc(t);}
		},
		{
			RequestType.QIANGDUO_GOLD_BUY_ITEM,  (r,t) => {Core.Data.playerManager.BuyItem(t); Core.Data.itemManager.addItemshop(t); Core.Data.EquipManager.AddShopItem(t); Core.Data.gemsManager.BuyItemSuc(t);
				Core.Data.soulManager.BuyItemSuc(t);Core.Data.monManager.BuyItemSuc(t);}
		},
		{
			RequestType.TIANXIADIYI_BATTLE,  (r,t) => {Core.Data.playerManager.fightReward(r,t);}
		},
		{
			RequestType.QIANGDUO_GOLD_BATTLE,  (r,t) => {Core.Data.playerManager.fightReward(r, t);}
		},
		{
			RequestType.QIANGDUO_DRAGONBALL_BATTLE,  (r,t) => {Core.Data.playerManager.fightReward(r,t);}
		},
		{
			RequestType.SU_DI_BATTLE,  (r,t) => {Core.Data.playerManager.fightReward(r,t);}
		},
//		{
//			RequestType.BUILD_CREATED,  (r,t) => { Core.Data.BuildingManager.BuildCreate(r,t);}
//		},
		{
			RequestType.BUILD_GET,  (r,t) => { Core.Data.BuildingManager.BuildGet(r,t);}
		},
		{
			RequestType.BATTLE_BUILD_OPEN,  (r,t) => { Core.Data.BuildingManager.BattleBuildOpen(r,t);}
		},
		{
			RequestType.BUILD_UPGRADE,  (r,t) => { Core.Data.BuildingManager.BuildUpgrade(r,t);}
		},
		{
			RequestType.ZHAOMU,  (r,t) => {Core.Data.playerManager.ZhaoMu(t);	 Core.Data.monManager.ZhaoMuMonster(t);  Core.Data.EquipManager.ZhaomuEquip(t);  Core.Data.zhaomuMgr.OnZhaomuMsg(r, t);
				Core.Data.itemManager.Zhaomu(t); Core.Data.soulManager.Zhaomu(t); Core.Data.gemsManager.Zhaomu(t);}
		},
		{
			RequestType.HECHENG,  (r,t) => { Core.Data.monManager.HeChengMonster(r, t);}
		},
		{
			RequestType.ATTR_SWAP,  (r,t) => { Core.Data.monManager.AttrSwap(r, t); Core.Data.playerManager.AttrSwap(t); Core.Data.itemManager.AttrSwap(r,t);}
		},
		{
			RequestType.QIANLIXUNLIAN,  (r,t) => { Core.Data.monManager.QianLiXunLian(r, t); Core.Data.playerManager.QianLiXunLian(t); Core.Data.itemManager.QianLiXunLian(t);}
		},
		{
			RequestType.QIANLIRESET,  (r,t) => { Core.Data.monManager.QianLiReset(r, t); Core.Data.playerManager.QianLiXunLian(t);}
		},
		{
			RequestType.SKILL_UPGRADE, (r, t) => {Core.Data.monManager.MonSkillUpgrade(r, t);  Core.Data.itemManager.SkillUpgrade(t);}
		},
		{
			RequestType.GET_TIANXIADIYI_RANKSINGLE, (r, t) => {FinalTrialMgr.GetInstance().SingleRankAfterComplete(r, t);}
		},
		{
			RequestType.HAVEDINNER,  (r,t) => { Core.Data.playerManager.AddJingli(t);}
		},
		{  
            RequestType.SIGNDAY,           (r,t) => {Core.Data.playerManager.BuQianUpdateStone(t);}
		},
		{
            RequestType.CHANGE_USERINFO,   (r,t) => { Core.Data.playerManager.ChangeUserInfo(r, t);}
		},
		{
            RequestType.GEM_SYNTHETIC,     (r,t) => { Core.Data.gemsManager.SyntheticGem(r, t); }
		},
        {
            RequestType.BUY_MIANZHAN_TIME, (r,t) => { Core.Data.playerManager.BuyMianZhanPai(t);}
        },
		{
            RequestType.GEM_EXCHANGE,      (r,t) => { Core.Data.gemsManager.ExchangeGem(r, t); }
		},
        {  
            RequestType.LEVELGIFT_REQUEST, (r,t) => {Core.Data.itemManager.LevelUpDateItem(t);Core.Data.monManager.addItem(t); Core.Data.EquipManager.addItem(t);Core.Data.soulManager.addItem(t);Core.Data.gemsManager.AddLevelGem(t);}
        },
		{
            RequestType.GEM_RECASTING,     (r,t) => {Core.Data.EquipManager.EquipmentGemSlotRecast(r,t);}
		},

		{
            RequestType.SEVENDAYREWARD_BUY,   (r,t) => {Core.Data.itemManager.addItemshop(t); Core.Data.monManager.addItem(t);Core.Data.gemsManager.addItem(t);Core.Data.soulManager.addItem(t);Core.Data.EquipManager.addItem(t);}
		},

		{
			RequestType.SECRETSHOP_BUY,       (r,t) => {Core.Data.playerManager.BuyItem(t); Core.Data.itemManager.addItemshop(t);  Core.Data.EquipManager.AddShopItem(t);
				Core.Data.soulManager.addItem(t);Core.Data.gemsManager.BuyItemSuc(t);Core.Data.monManager.addItem(t);}
		},
        {
            RequestType.OPENTREASUREBOX,      (r,t) => {Core.Data.itemManager.UseKeyItem(r,t);}
        },
		{
			RequestType.NEW_FINALTRIAL_BATTLE,      (r,t) => {Core.Data.monManager.addItem(t); Core.Data.itemManager.addItem(t); Core.Data.EquipManager.addItem(t);  
				Core.Data.playerManager.BuyItem(t);Core.Data.soulManager.addItem(t);}
		},
		{
			RequestType.GUAGUALE,      (r,t) => {Core.Data.playerManager.BuyItem(t); }
		},
		{
			RequestType.GETVIPGIFT,      (r,t) => {Core.Data.itemManager.addItem(t); }
        },
		{
			RequestType.SECRETSHOP_BUYSOULHERO,       (r,t) => {Core.Data.playerManager.BuyItem(t); Core.Data.itemManager.addItemshop(t);  Core.Data.EquipManager.AddShopItem(t);
				Core.Data.soulManager.addItem(t);}
        },
        {
            RequestType.BUY_ENERGY,          (r,t) => {Core.Data.playerManager.UseBuyEnergy(r,t);}

        },
		{
			RequestType.ACTIVATION_CODE,            (r,t) => {Core.Data.playerManager.BuyItem(t); Core.Data.itemManager.addItemshop(t);  Core.Data.EquipManager.AddShopItem(t); Core.Data.gemsManager.UsePropSuc(t);
				Core.Data.soulManager.UseProp(t); Core.Data.monManager.UseProp(t);				
			}
		},
		{
			RequestType.GET_MONTHGIFT,          (r,t) => {Core.Data.itemManager.addItemshop(t);}

		},
		{
			RequestType.GET_FIRSTCHARGEGIFT,    	(r,t) => {Core.Data.itemManager.addItemshop(t);}

		},
		{
			RequestType.GETVIPLEVELREWARD,    	(r,t) => {Core.Data.itemManager.addItemshop(t); Core.Data.EquipManager.AddShopItem(t); Core.Data.gemsManager.BuyItemSuc(t);
				Core.Data.soulManager.BuyItemSuc(t); Core.Data.monManager.BuyItemSuc(t);}
			
		},
		{
			RequestType.GETACTIVITYLIMITTIME,    	(r,t) => {Core.Data.itemManager.addItemshop(t); Core.Data.EquipManager.AddShopItem(t); Core.Data.gemsManager.BuyItemSuc(t);
				Core.Data.soulManager.BuyItemSuc(t); Core.Data.monManager.BuyItemSuc(t);}
			
		},
		{
			RequestType.RESETFINALTRIALVALUE,    	(r,t) => {Core.Data.playerManager.BuyItem(t);}
			
		},
		{
			RequestType.ACTIVESHOPBUYITEM,            (r,t) => {Core.Data.playerManager.BuyItem(t); Core.Data.itemManager.addItemshop(t);  Core.Data.EquipManager.AddShopItem(t); Core.Data.gemsManager.BuyItemSuc(t);
				Core.Data.soulManager.BuyItemSuc(t); Core.Data.monManager.BuyItemSuc(t);
			}
		},
		{
			RequestType.REFRESH_ZHANGONG_BUY_ITEM,            (r,t) => {Core.Data.playerManager.BuyItem(t); 
			}
		},

	};

	public static Action<BaseHttpRequest, BaseResponse> getAction(HttpTask task) {
		if(task != null) {
			HttpRequest request = task.request as HttpRequest;
			if(request != null && ACTION_LIST.ContainsKey(request.Type))
				return ACTION_LIST[request.Type];
			else
				return null;
		} 
		else 
			return null;
	}


    //------------------------------------------------------------------------------------
    //-------------------------------- SYNC Operation ------------------------------------
    //------------------------------------------------------------------------------------
    private static void Sync_Operation(BaseResponse t){
        Core.Data.monManager.fullfillByNetwork(t); 
        Core.Data.gemsManager.fullfillByNetwork(t);
        Core.Data.EquipManager.fullfillByNetwork(t);
        Core.Data.playerManager.fullfillByNetwork(t, Core.Data.monManager, Core.Data.EquipManager);  
        Core.Data.itemManager.fullfillByNetwork(t);
        Core.Data.dungeonsManager.fullfillByNetwork(t);
        Core.Data.dragonManager.fullfillByNetwork(t);
        Core.Data.BuildingManager.fullfillByNetwork(t);
        Core.Data.soulManager.fullfillByNetwork(t);
        Core.Data.ActivityManager.fullfillByNetwork(t);
		Core.Data.guideManger.fullfillByNetwork(t);
        Core.Data.noticManager.fullfillByNetwork (t);
		Core.Data.newDungeonsManager.fullfillByNetwork(t);
		Core.Data.deblockingBuildMgr.fullfillByNetwork(t);
		Core.Data.battleTeamMgr.fullfillByNetwork(t);
		Core.Data.vipManager.fullfillByNetwork(t);
		Core.Data.FinalTrialDataMgr.fullfillByNetwork(t);
		LuaTest.fullfillByNetwork(t);
    }


    public static Action Sync_Operation(HttpTask task) {

        if(task != null && task.response.sync != null) {
            LoginResponse magicResp = new LoginResponse();
            magicResp.data = task.response.sync;
            magicResp.status = (short)0;
            magicResp.errorCode = 0;
			magicResp.handleResponse();
            return () => { Sync_Operation(magicResp); };
        } else {
            return null;
        }

    }

}
