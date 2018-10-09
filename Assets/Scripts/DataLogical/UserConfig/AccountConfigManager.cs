/*
 * Created By Allen Wu. All rights reserved.
 */

using System;
using System.Collections.Generic;

public class BagOfStatus {
	//服务器唯一ID
	public int pid;
	//状态
	public short status;
	//配置表Num
	public int num;

	//初次加入背包
	public const short STATUS_NEW = 1;
	//正常状态
	public const short STATUS_NORMAL = 0;
	//删除状态
	public const short STATUS_DELETE = 2;

	public BagOfStatus () { }

	public BagOfStatus (int pid, int num, short status) { 
		this.pid = pid;
		this.status = status;
		this.num = num;
	}
}

public class AccountConfigData : DataObject
{
    //小悟空缘是否第一次配齐   1 - 已经配齐过  0 没有配齐过 (这个是没用的，但是我觉得悟空应该在这个地方保存）
    public short wukongfate;
    //每日首次  登陆奖励 和 公告
    public long time;
	//8级新手引导 (第一次打BOSS失败时触发) 1 -- 已经触发过， 0 没有触发过
	public short guidefirst;
	//战斗是否有加速的信息，1 代表加速， 0 没有加速
	public short SpeedUp;
	//是否自动战斗，1 代表自动， 0 没有自动
	public short AutoBat;
	//16关，全的克制引导
	public short ShengWuZhe;

	/// 
	/// ----------  下面两个值已经不再使用 ------------
	/// 

	//弱引导(任务)    0 没有做过  1做过
	public short weekGuide_Task;
	
	//弱引导(大转盘)    0 没有做过  1做过
	public short weekGuide_Wheel;

	/// <summary>
	/// --------- 下面两个值为特殊的副本引导 ---------
	/// </summary>
	public short FB_60104;

	public short FB_60109;
	
	//背包里面所有的物品要加上一个是否是新的状态
	//文件存储的时候，是一个数组，数组里面是一个服务器唯一ID+一个short型的状态
	public BagOfStatus[] bagstatus;

	//抢夺金币  抢夺龙珠 
	public short BattleWin;
	
    #region 特殊条件引导ID add by jc
	public int SpecialGuideID = 0;
	//如果升级没有弹窗这个值将会有值(防止引导并发)
	public int mPreLevel = -1;
	#endregion
	
	
	
	//add more...

    public AccountConfigData() { }

	/// <summary>
	/// 添加默认的值
	/// </summary>
	public void DefaultValue() {
        wukongfate = 0;
        time = 0;
		bagstatus = null;
		BattleWin = 0;

		FB_60104 = (short)0;
		FB_60109 = (short)0;
		SpecialGuideID = 0;
		SpeedUp  = 0;
		AutoBat  = 0;
		ShengWuZhe = 0;
	}
}

/// <summary>
/// 账户的信息
/// 下载漫画，
/// </summary>
public class AccountConfigManager : Manager {
	public const bool AES_ENCRYPT = false;

    private AccountConfigData accInfo;
	private DataPersistManager persist;

	//key = pid，这个只设置normal状态
	private Dictionary<int, BagOfStatus> mDicBagStatus = null;
	//key = num，这个只设置new的状态, value是有New状态的数量
	private Dictionary<ConfigDataType, int> mDicBagStatus2 = null;

    public AccountConfigData UserConfig {
        get { return accInfo; } 
        set { accInfo = value; }
	}

	#region ignore it
    //检测每日第一次登陆  
    public bool checkFirstLogin(long curTime) {
        bool first = false;
        if(accInfo != null) {

            //这个账号没有使用过
            if(accInfo.time == 0) {
                first = true;
            } else {
                try {
                    first = DateHelper.isPassedOneDay(accInfo.time, curTime);
                } catch(Exception ex) {
                    ConsoleEx.DebugLog(ex.ToString());
                    first = false;
                } 
            }
        }

        return first;
    }

    public void setLoginTime(long systime){

        if(accInfo != null) {
            accInfo.time = systime;
            save();
        }
    }

	public override bool loadFromConfig () {
		return true;
	}

	#endregion

    public AccountConfigManager() {
        accInfo = null;
		persist = Core.DPM;

        accInfo = persist.ReadFromLocalFileSystem(DataType.ACCOUNT_CONFIG, AES_ENCRYPT) as AccountConfigData;
        if(accInfo == null) {
            accInfo = new AccountConfigData();
            accInfo.DefaultValue();
		}

        accInfo.mType = DataType.ACCOUNT_CONFIG;
	}

	#region Bag of Status

	//分析本地记录
	public void analyseBag() {
		mDicBagStatus = new Dictionary<int, BagOfStatus>();
		mDicBagStatus2 = new Dictionary<ConfigDataType, int>();

		if(accInfo != null && accInfo.bagstatus != null) {
			foreach(BagOfStatus status in accInfo.bagstatus) {
				if(status != null) {
					mDicBagStatus[status.pid] = status;
				}
			}
		}
	}
	//增加某个类型的New的状态
	public void analyseBag(ConfigDataType type, int pid){
		//New status 的数量RefreshCount
		int count = 0;
		if(!mDicBagStatus2.TryGetValue(type, out count)) {
			count = 0;
		}

		if(getStatus(pid) == BagOfStatus.STATUS_NEW) { //如果是New状态
			count ++;
		}
		mDicBagStatus2[type] = count;
	}

	//删除某个类型的New的状态
	public void removeBag(ConfigDataType type, int pid) {
		//New status 的数量
		int count = 0;
		if(!mDicBagStatus2.TryGetValue(type, out count)) {
			count = 0;
		}

		if(getStatus(pid) == BagOfStatus.STATUS_NEW) { //如果是New状态
			count --;
			if(count < 0) count = 0;
		}
		mDicBagStatus2[type] = count;
	}

	//删除某个类型的New的状态
	private void removeBag(BagOfStatus status) {
		if(status != null) {
			ConfigDataType type = DataCore.getDataType(status.num);
			removeBag(type, status.pid);
		}
	}

	//设置ConfigDataType的New Status
	public void clearBagStatus(ConfigDataType type) {
		mDicBagStatus2[type] = 0;
	}

	//根据类型获取New数量
	public int getStatusByType(ConfigDataType type) {
		int count = 0;
		if(!mDicBagStatus2.TryGetValue(type, out count)) {
			count = 0;
		}
		return count;
	}

	//根据服务器ID获取一个状态
	public short getStatus(int pid) {
		short status = BagOfStatus.STATUS_NEW;
		BagOfStatus bagStatus = null;

		if(mDicBagStatus.TryGetValue(pid, out bagStatus)) {
			status = bagStatus.status;
		} 
		return status;
	}

	//设定一个状态
	public void setStatus(BagOfStatus newStatus) {
		if(newStatus != null) {
			switch(newStatus.status) {
			case BagOfStatus.STATUS_DELETE: 
				removeBag(newStatus);
				if(mDicBagStatus.ContainsKey(newStatus.pid)) {
					mDicBagStatus.Remove(newStatus.pid);
				}
				break;
			case BagOfStatus.STATUS_NORMAL:
				removeBag(newStatus);
				mDicBagStatus[newStatus.pid] = newStatus;
				break;
			case BagOfStatus.STATUS_NEW:
				break;
			}
		}
	}

	//设定为新的状态
	public void setNewStatus(int pid, int num) {
		setStatus(new BagOfStatus(pid, num, BagOfStatus.STATUS_NEW));
	}

	#endregion

	public void save() {
        if (accInfo != null) {
			accInfo.bagstatus = new BagOfStatus[mDicBagStatus.Count];
			mDicBagStatus.Values.CopyTo(accInfo.bagstatus, 0);
            persist.WriteToLocalFileSystem(accInfo, AES_ENCRYPT);
		}
	}

	public void SaveFinalTrialQiangduoQueue()
	{
		if(accInfo != null){
			//accInfo.qiangduoRoleQueue = FinalTrialMgr.GetInstance().QiangduoRoleQueue;
		}
    }

	public void SaveFinalTrial()
	{
		if(accInfo != null)
		{
			persist.WriteToLocalFileSystem(accInfo, AES_ENCRYPT);
        }
    }

	public void SetBattleResult(bool _key)
	{
		if(accInfo != null) {
			accInfo.BattleWin = _key ? (short)1 : (short)0;
		}
	}

	public void ResetBattleResult()
	{
		if(accInfo != null) {
			accInfo.BattleWin = 0;
		}
	}
    
}
