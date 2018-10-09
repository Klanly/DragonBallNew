using System;
using System.Collections.Generic;


public class ActionOnReceiveSockResponse {

	public readonly static Dictionary<RequestType, Action<BaseSocketRequest, BaseResponse>> ACTION_LIST = new Dictionary<RequestType, Action<BaseSocketRequest, BaseResponse>>() {
		 
									
		{	RequestType.SOCK_ADDPOWER,       (r,t) => { Core.Data.playerManager.AddPowerUpdate(t);} },
		{	RequestType.SOCK_ATTACKBOSS,     (r,t) => { Core.Data.playerManager.AttackBossUpdate(t);} },
		{	RequestType.SOCK_BUYLOTTERY,     (r,t) => { Core.Data.playerManager.BuyLottery(t);Core.Data.monManager.AddMonsterInFestival(t); } },
		{	RequestType.SOCK_HONORBUYITEM,   (r,t) => {Core.Data.ActivityManager.AddHonorItem(t);}},
		//	{	RequestType.SOCK_BUYLOTTERY,   (r,t) => { Core.Data.playerManager.BuyLottery(t); } },

	};

	public static Action<BaseSocketRequest, BaseResponse> getAction(SocketTask task) {
		if(task != null) {
			SocketRequest request = task.request as SocketRequest;
			if(request != null && ACTION_LIST.ContainsKey(request.Type))
				return ACTION_LIST[request.Type];
			else
				return null;
		} 
		else 
			return null;
	}
}


