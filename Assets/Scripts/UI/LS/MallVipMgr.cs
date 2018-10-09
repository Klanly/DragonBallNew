using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VipManager :  Manager
{
    private List<VipGiftData> VipGiftDataListConfig ;
    private List<VipInfoData> VipInfoDataListConfig;
	//private List<UserHeadData> UserHeadListConfig;
    private List<BuyEnergy> BuyEnergyDataConfig;

    public bool IsFirstLogin_Vip = true;

	public VipStatus vipStatus = null;

	public VipManager()
    {
        VipGiftDataListConfig = new List<VipGiftData>();

		// 第一个 为普通非VIP用户 数据
        VipInfoDataListConfig = new List<VipInfoData>();
		//UserHeadListConfig = new List<UserHeadData>();

        BuyEnergyDataConfig = new List<BuyEnergy>();

    }

	public void fullfillByNetwork(BaseResponse response) {
		if (response != null && response.status != BaseResponse.ERROR) {
			LoginResponse resp = response as LoginResponse;
			if (resp != null && resp.data != null && resp.data.vipInfo != null) {
				vipStatus = resp.data.vipInfo;
			}
		}
	}

    public override bool loadFromConfig () {
        return base.readFromLocalConfigFile<VipGiftData> (ConfigType.VipGift, VipGiftDataListConfig) |
			      base.readFromLocalConfigFile<VipInfoData> (ConfigType.VipInfo, VipInfoDataListConfig)
            |base.readFromLocalConfigFile<BuyEnergy> (ConfigType.BuyEnergy, BuyEnergyDataConfig);
				//| base.readFromLocalConfigFile<UserHeadData>(ConfigType.UserHead, UserHeadListConfig);
    }

//	public List<UserHeadData> HeadInfoList{
//		get{
//			return UserHeadListConfig;
//		}
//	}
    
    public List<VipGiftData> GetVipGiftDataListConfig()
    {
        return VipGiftDataListConfig;
    }
    public List<BuyEnergy> GetBuyEnergyDataConfig()
    {
        return BuyEnergyDataConfig;
    }
    public List<VipInfoData> GetVipInfoDataListConfig()
    {
        return VipInfoDataListConfig;
    }

    public VipGiftData GetVipGiftData(int mIndex)
    {
        if(VipGiftDataListConfig.Count != 0)
        {
            if(mIndex <=  VipGiftDataListConfig.Count)
            {
                return VipGiftDataListConfig[mIndex-1];
            }
        }
        return null;
    }

	public VipInfoData GetVipInfoData(int vipLv)
	{
		if(this.VipInfoDataListConfig.Count != 0)
		{
			foreach(VipInfoData data in VipInfoDataListConfig)
			{
				if(vipLv == data.vipLv)return data;
			}
		}
		return null;
	}

//	public UserHeadData GetUserHeadData(int id)
//	{
//		if(this.UserHeadListConfig.Count != 0)
//		{
//			foreach(UserHeadData item in UserHeadListConfig)
//			{
//				if(item.id == id)return item;
//			}
//		}
//		return null;
//	}

	//得到自己当前的免费招募次数
	public int GetFreeZhaoMuTime()
	{
		if (Core.Data.playerManager.RTData.curVipLevel == 0)
		{
			return 0;
		}
		else
		{
			for (int i = 0; i < VipInfoDataListConfig.Count; i++)
			{
				if (VipInfoDataListConfig [i].vipLv == Core.Data.playerManager.RTData.curVipLevel)
				{
					return VipInfoDataListConfig [i].freeSuperHR;
				}
			}
		}
		return 0;
	}


	//获取连续扫荡VIP解锁等级
	int _ContinuousSweepingVIPLevel = -1;
	public int GetUnLockContinuousSweepingVIPLevel()
	{
		if(_ContinuousSweepingVIPLevel > 0)
			return _ContinuousSweepingVIPLevel;
		foreach(VipInfoData vipData in VipInfoDataListConfig)
		{
			if(vipData.raidunlock > 0)
			{
				//Debug.LogError(vipData.raidunlock.ToString());
				_ContinuousSweepingVIPLevel = vipData.vipLv;
				return _ContinuousSweepingVIPLevel;
			}
		}
		return _ContinuousSweepingVIPLevel;
	}

	//1：技能副本；2：战魂副本；3：经验副本；4：宝石副本  
	//通过当前可以购买的次数，获取下一个VIP等级
	public VipInfoData GetNextVipLevelBuyActiveFB(int PVE_Type,int Cur_BuyCount)
	{
		VipInfoData data = null;
		bool result = false;
		foreach(VipInfoData vipData in VipInfoDataListConfig)
		{
			switch(PVE_Type)
			{
			case 1:
				result = vipData.specialdoor1 > Cur_BuyCount;
				break;
			case 2:
				result = vipData.specialdoor2 > Cur_BuyCount;
				break;
			case 3:
				result = vipData.specialdoor3 > Cur_BuyCount;
				break;
			case 4:
				result = vipData.specialdoor4 > Cur_BuyCount;
				break;
			}
			if(result)
			{
				data = vipData;
				return data;
			}
		}
		return data;
	}

	public int MaxVipLevel
	{
		get
		{
			return VipInfoDataListConfig.Count -1;
		}
	}
	
}
