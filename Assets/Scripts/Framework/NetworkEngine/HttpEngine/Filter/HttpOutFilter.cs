using System;

public class HttpOutFilter : IFilter {

	//请求
	public RequestType request = RequestType.None;

    /// <summary>
    /// 检测Http 是否应该 真的发送出去 Out
    /// </summary>
    public bool check () {
        bool keepReserve = false;
		keepReserve = Core.Data.guideManger.isGuiding && !Core.Data.guideManger.isAskServer && Core.Data.guideManger.LastTaskID != 100000;
        //永远放行
		if(Core.Data.temper.LetGoACT == HttpRequestFactory.ACTION_SETTLE_BATTLE_BOSS || request == RequestType.CURRENT_USERSATAE || request == RequestType.GET_AWARDACTIVITY || request == RequestType.GET_FIRSTCHARGESTATE || request == RequestType.GET_MONTHSTATE || request == RequestType.GET_TIANXIADIYI_RANKSINGLE) 
			keepReserve = false;
		if(request == RequestType.VIPSHOPINFO||request == RequestType.ACTIVITYLIMITTIME) keepReserve = false;
        //永远拦截 
        if(request == RequestType.FIGHT_FULISA )
			keepReserve = true;

        return keepReserve;
    }

    //处理本地数据
    public string handleLocalTask(HttpTask task) {
        return GuideLocalData.HandleLocalHttpTask(task);
    }

}
