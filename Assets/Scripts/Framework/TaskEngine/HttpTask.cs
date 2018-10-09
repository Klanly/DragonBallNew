using System;
using System.Collections;

public class HttpTask : BaseTaskAbstract {

	/* The code will be excute after we get response
	 */ 
	public Action<BaseHttpRequest, BaseResponse> afterCompleted;

	/* When Error Occured, it will notify user
	 */ 
	public Action<BaseHttpRequest, string> ErrorOccured;

	//this will be fullfilld when create instance
	public BaseHttpRequest request;
	//this will be fullfilled after http response is coming.
	public BaseResponse response;
	//we should know the relationship between Request with BaseBuildingData
	public RelationShipReqAndResp relation;
	//we will store the error Info if Json Parser has ouccerd
	public string errorInfo;

    //发出去的Filter
    public HttpOutFilter OutFilter;
    //尚未使用HttpInFilter
    public HttpInFilter InFilter;

	public HttpTask (ThreadType threadType, TaskResponse respType = TaskResponse.Default_Response) {
		this.type = TaskType.HttpTask;
		this.threadType = threadType;
		this.respType = respType;
        this.OutFilter = new HttpOutFilter();
	}

	/*
	 * This is for creating Common Http Task
	 */ 
	public void AppendCommonParam (RequestType requestType, BaseRequestParam param) {
		this.request = HttpRequestFactory.createHttpRequestInstance(requestType, param);
		this.relation = HttpRequestFactory.getRelationShip(requestType);
		this.OutFilter.request = requestType;
	}

	/*
	 * This is for creating command in the http client
	 */
	public void AppendCmdParam (InternalRequestType requestType) {
		this.request = new InternalRequest(requestType);
	}

	/*
	 * This is for creating third party task
	 */
	public void AppendThirdParam(ThirdPartyRequestType requestType, string param) {
		this.request = new ThirdPartyHttpRequest(requestType, param);
	}

	/// <summary>
	/// 异步方法，必须等待回调
	/// </summary>

	public override void DispatchToRealHandler() 
	{
//		if (this.OutFilter.check()== true) {
		if(this.check()){
//			ConsoleEx.Write ("  request  local  type ===  " + OutFilter.request ,"yellow");
			//先检测本地有没有数据，如果有的话，直接返回，不走网络
			//DispatchImmediatly();
			System.Threading.ThreadPool.QueueUserWorkItem (DispatchImmediatly);
		} else {
			//网络的
//			ConsoleEx.Write ("  request net type ===  " + OutFilter.request ,"yellow");
			Core.NetEng.httpEngine.sendHttpTask (this);
		}
	}
//	// <summary>
//	/// out filter   一样的check
//	/// </summary>//
//
	public bool check () {

		bool keepReserve = false;
		keepReserve = Core.Data.guideManger.isGuiding  && Core.Data.guideManger.LastTaskID != 100000;
		//永远放行
		if(OutFilter.request == RequestType.SETTLE_BOSSBATTLE || OutFilter.request == RequestType.CURRENT_USERSATAE || OutFilter.request == RequestType.GET_AWARDACTIVITY || OutFilter.request == RequestType.GET_FIRSTCHARGESTATE || OutFilter.request == RequestType.GET_MONTHSTATE || OutFilter.request == RequestType.GET_TIANXIADIYI_RANKSINGLE) 
			keepReserve = false;
		if(OutFilter.request == RequestType.VIPSHOPINFO || OutFilter.request == RequestType.ACTIVITYLIMITTIME || OutFilter.request == RequestType.MESSAGE_INFORMATION) keepReserve = false;
		//永远拦截 
		if(OutFilter.request == RequestType.FIGHT_FULISA )
			keepReserve = true;

		return keepReserve;
	}

	/// <summary>
	/// 同步的方法，会block当前线程
	/// </summary>
	public void DispatchImmediatly(System.Object obj) {

		Core.NetEng.httpEngine.RunImmediatly(this);
	}

	public virtual void handleErrorOcurr() {
		if(ErrorOccured != null ) {
			if(!string.IsNullOrEmpty(errorInfo))
				ErrorOccured(request, errorInfo);
			else {
				if(response != null) {
					ExceptionResponse ex = response as ExceptionResponse;
					if(ex != null) 
						ErrorOccured(request, ex.HttpError);
				}
			}
				
		} 
	}

	public override void handleBackGroundCompleted() {
		base.handleBackGroundCompleted();

		if(afterCompleted != null) {
			try {		
				afterCompleted(request, response);
			} catch (Exception ex){
				ConsoleEx.DebugLog(ex.ToString());
			}
		}

	}

	public override void handleMainThreadCompleted() {
		#region Task Add by jc
		if(response.task != null && response.task.Length>0)
			Core.Data.taskManager.Complete(response.task, (request as HttpRequest).Act);
		#endregion

		base.handleMainThreadCompleted();
		if(afterCompleted != null)
			afterCompleted(request, response);
	}


	/*	
     * This is internal routine called after task is completed
     * this lives in the background thread.
     * 
     * I will define doWork.
     */ 
    public virtual void handleCompletedInternal(Action<BaseHttpRequest, BaseResponse> doWork, Action doSync) {
		if(doWork != null) {
			doWork(request, response);
		}

        //在内部处理的时候，也考虑和服务器同步信息
        if(response.sync != null && doSync != null) {
            doSync();
        }
	}

    #region 取走ErrorInfo，以便重试的时候能设置为空置
    public string takeErrorOver {
        get {
            string error = errorInfo;
            errorInfo = string.Empty;
            return error;
        }
    }
    #endregion
}
