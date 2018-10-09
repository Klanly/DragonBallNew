using System;
using System.Collections.Generic;

/*
 * 	①. 宠物扭蛋获得5星时（轮播1）
	②. 装备扭蛋获得5星时（轮播1）
	③. 宠物进化至6星时（轮播1）
	④. 开宝箱获得钻石卡时（轮播1）
	⑤. 首次通关全PVE副本时（轮播1）
	⑥. 获得PVP排行榜前3任意位置时（轮播1）
	⑦. 最后击杀世界BOSS时（轮播1）
	⑧. 后台设定滚动公告（起始时间、结束时间、间隔时间）
	⑨. 小提示小帮主（自动间隔轮播-读表10分钟轮播1次）
	⑩. 5星和6星宠物融合为全属性时（轮播1）
 */ 
public enum MovableNoticeType {
	EGG_5Star   = 0x1,
	EQUIP_5Star = 0x2,
	MON_GRADE_6Star = 0x3,
	GEMS_WHEN_OPEN_CHEST = 0x4,
	PVE_COMPLETE= 0x5,
	PVP_TOP3    = 0x6,
	PVE_WORLDBOSS = 0x7,
	MAKE_NO_SENSE = 0x8,
	TIPS        = 0x9,
	PROPERTY_ALL = 0xA,
}

//从服务器读取的信息
public class NoticeDataConfig {
	public int type;
	public string Tips;
}


public class NoticeLocal : DataObject {
	//要存储的本地数据
	public NoticeDataConfig[] tips;

	//TODO : add more

	public NoticeLocal() { }

	//默认的数据
	public void defaultValue() {
		tips = null;
	}

}


/// <summary>
/// 主界面的公告条
/// 
/// -- 这是一个会存储本地的数据，本地数据和账号的服务器相关
/// </summary>
public class MovableNoticeManager : Manager {

	//5分钟 300秒
	private const long FIVE_MINITE = 300;

	private const bool AES_ENCRYPT = false;

	//当前的次数
	private int sentTimes = 0;
	//最大的重试次数
	private const int MAX_RETRY_TIMES = 3;
	//缓存的数据 -- 从服务器上取得
	private List<NoticeDataConfig> cached;
	//本地的缓存数据
	private NoticeLocal cachedlocal;

	//当前读取的位置信息
	private int curIndex = 0;
	private int maxValidate = 0;

	//当下载完成后，调用的东西
	private Action onFinished;

	private DataPersistManager persist;

	public MovableNoticeManager() {
		cached = new List<NoticeDataConfig>();
		sentTimes = 0;
		curIndex = 0;

		persist = Core.DPM;
	} 

	//检测有效的信息
	public bool checkValidate() {
		return maxValidate > 0;
	}

	#region Override & Ignore

	public override void fullfillByNetwork (BaseResponse response) {

	}

	#endregion

	//从本地配置文件里加载
	public override bool loadFromConfig() {
		cachedlocal = persist.ReadFromLocalFileSystem(DataType.TIPS_CONFIG, AES_ENCRYPT) as NoticeLocal;
		if(cachedlocal == null) {
			cachedlocal = new NoticeLocal();
		}
		cachedlocal.mType = DataType.TIPS_CONFIG;

		return true;
	}

	//服务器上读取到了，写到文件里面,覆盖或追加写入
	private void saveTofile() {
		cachedlocal = new NoticeLocal();
		cachedlocal.tips = cached.ToArray();
		cachedlocal.mType = DataType.TIPS_CONFIG;

		if(maxValidate > 30)
			persist.WriteToLocalFileSystem(cachedlocal, AES_ENCRYPT);
		else 
			persist.AppendToLocalFileSystem(cachedlocal, AES_ENCRYPT);
	}

	public NoticeDataConfig getCurrentNotice() {
		if(maxValidate == 0) return null;

		curIndex = (++ curIndex) % maxValidate;

		if(curIndex >= cached.Count) {
			return null;
		}

		return cached[curIndex];
	}


	//从服务器上加载数据
	public void loadFromNetwork(Action onFinish) {
		
		if(!Core.Data.guideManger.isGuiding)
		{
			sentTimes ++ ;
			HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
	
			task.AppendCommonParam(RequestType.MOVABLE_NOTICE, new MovableNoticeParam( Core.Data.playerManager.PlayerID ) );
	
			task.afterCompleted = HttpComingBack;
			task.ErrorOccured   = HttpError;
	
			//then you should dispatch to a real handler
			task.DispatchToRealHandler();
	
			onFinished = onFinish;
		}
	}

	void HttpComingBack(BaseHttpRequest request, BaseResponse response) {
		if(response != null && response.status != BaseResponse.ERROR) {
			MovableNoticeResponse movResp = response as MovableNoticeResponse;

			if(movResp != null && movResp.data != null) {
				cached.Clear();
				maxValidate = 0;

				foreach(NoticeDataConfig config in movResp.data) {
					if(config != null) {
						cached.Add(config);
						maxValidate ++ ;
					}
				}

				if(maxValidate > 0) {
					//存储到本地
					saveTofile();
				} else {
					//从本地读取
					loadFromConfig();
				}

				if(onFinished != null) {
					onFinished();
				}

			} else {
				ConsoleEx.DebugLog("Get Movable notice : " + Core.Data.stringManager.getString(response.errorCode));
			}

			//开始计时，5分钟后继续更新
			Core.Data.temper.setNoticeReady(true);
			long curSysTime = Core.TimerEng.curTime;
			TimerTask getNoticeTask = new TimerTask(curSysTime, curSysTime + FIVE_MINITE, 1, ThreadType.BackGround);
			getNoticeTask.onEventEnd = Core.Data.temper.onNetworkCallBack;
			getNoticeTask.DispatchToRealHandler();
		}
	}

	void HttpError(BaseHttpRequest request, string error) {
		ConsoleEx.DebugLog("Get Movable notice : " + error);

		if(sentTimes <= MAX_RETRY_TIMES) {
			loadFromNetwork(onFinished);
		}
	}

}
