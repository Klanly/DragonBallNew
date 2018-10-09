using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeblockingBuildMgr : Manager
{
//	List<DeblockingBuildData> mConfigList;
	public override bool loadFromConfig()
	{
		return base.readFromLocalConfigFile<DeblockingBuildData>(ConfigType.DeblockingBuild, mDeblockingBuildDataDic);
	}

	//存到字典  key是表中的num 代表等级以及关卡ID
	Dictionary<int, DeblockingBuildData> mDeblockingBuildDataDic;

	public int mFloorRecord;

	public DeblockingBuildMgr()
	{
//		mConfigList = new List<DeblockingBuildData>();
		mDeblockingBuildDataDic = new Dictionary<int, DeblockingBuildData>();
	}

	void Analysis()
	{
//		mDeblockingBuildDataDic = new Dictionary<int, DeblockingBuildData>();
//		foreach(DeblockingBuildData data in mConfigList)
//		{
//			mDeblockingBuildDataDic.Add(data.num, data);
//		}
	}

	public override void fullfillByNetwork(BaseResponse response)
	{
		if(response != null && response.status != BaseResponse.ERROR)
		{
			LoginResponse resp = response as LoginResponse;
			if(resp != null)
			{
				Analysis();
			}
		}
	}

	#region 升级解锁弹窗
	
	int UnLockCount = 0;
	List<string> mCurDeblockingName = new List<string>();
	List<string> mCurDeblockingIcon = new List<string>();
	List<string> mCurDeblockingAtlas = new List<string>();
	public void OpenLevelUpUnlock(bool m_IsUnlockLevelUp)
	{
		if(Core.Data.guideManger.isGuiding)return;
		foreach(DeblockingBuildData data in mDeblockingBuildDataDic.Values)
		{
			if(!m_IsUnlockLevelUp && mFloorRecord >= Core.Data.newDungeonsManager.lastFloorId)break;
			if(m_IsUnlockLevelUp)
			{
				if(data.type == 0 && data.num == Core.Data.playerManager.Lv)
				{
					for(int i=0; i<data.name.Length; i++)
					{
						mCurDeblockingName.Add(data.name[i]);
						mCurDeblockingIcon.Add(data.icon[i]);
						mCurDeblockingAtlas.Add(data.deblockingType);
					}
				}
			}
			if(mFloorRecord < Core.Data.newDungeonsManager.lastFloorId)
			{
				if(data.type == 1 && data.num == Core.Data.newDungeonsManager.lastFloorId)
				{
					for(int i=0; i<data.name.Length; i++)
					{
						mCurDeblockingName.Add(data.name[i]);
						mCurDeblockingIcon.Add(data.icon[i]);
						mCurDeblockingAtlas.Add(data.deblockingType);
					}
				}
			}

		}
		if(mFloorRecord != Core.Data.newDungeonsManager.lastFloorId)
		{
			mFloorRecord = Core.Data.newDungeonsManager.lastFloorId;
		}




		if(mCurDeblockingName.Count != mCurDeblockingIcon.Count)
		{
			RED.LogWarning("Excel has error");
			return;
		}

		BeginUnlock();
	}
	
	public void BeginUnlock()
	{
		if(UnLockCount < mCurDeblockingName.Count)
		{
			UIDragonBallBuildUnlock.OpenUI(mCurDeblockingName[UnLockCount], mCurDeblockingIcon[UnLockCount], mCurDeblockingAtlas[UnLockCount]);
			UnLockCount++;
			return;
		}
		UnLockCount = 0;
		mCurDeblockingName.Clear();
		mCurDeblockingIcon.Clear();
		mCurDeblockingAtlas.Clear();
	}
	
	#endregion

}
