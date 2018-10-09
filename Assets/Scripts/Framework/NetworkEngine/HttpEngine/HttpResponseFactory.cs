using System;
using fastJSON;

public class HttpResponseFactory {

	public static void createResponse(HttpTask task, string json) {
		ConsoleEx.Write( task.relation.respType.ToString() + " is coming back : => " + json);
		BaseResponse response = null;
		if(!string.IsNullOrEmpty(json) && task != null) {
			try {
				response = JSON.Instance.ToObject(json, task.relation.respType) as BaseResponse;

				if(response != null) 
				{
					response.handleResponse();
					//store in the task
					task.response = response;

					//added by zhangqiang to sync coin and stone
					CheckCoinAndStone(response);
				}
			} catch(Exception ex) {
				ConsoleEx.DebugLog(ex.ToString());
				task.errorInfo = ex.ToString();
			} 
		}
	}

	static void CheckCoinAndStone(BaseResponse response)
	{
		if(response.status == BaseResponse.ERROR && 
			response.errorCode == 5000 || response.errorCode == 5006)
		{
			HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
			task.AppendCommonParam(RequestType.SYNC_MONEY, new SyncMoneyParam(Core.Data.playerManager.PlayerID));

			//then you should dispatch to a real handler
			task.DispatchToRealHandler ();
		}
	}
}
