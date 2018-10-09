using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class ActionOnReceiveCMSHttpResponse{

	public readonly static Dictionary<RequestType, Action<BaseSocketRequest, BaseResponse>> ACTION_LIST = new Dictionary<RequestType, Action<BaseSocketRequest, BaseResponse>>()
	{					
		//{	RequestType.SOCK_ADDPOWER,       (r,t) => { Core.Data.playerManager.AddPowerUpdate(t);} },
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
