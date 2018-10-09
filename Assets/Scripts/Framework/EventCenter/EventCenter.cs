using System;
using System.Collections.Generic;


/// <summary>
/// Message type. 这个类型实际上是处理默认的处理网络请求的类型。
/// </summary>
public enum MsgType {
	HTTP_EXCEPTION,
	HTTP_SESSION_TIMEOUT,
	SOCK_EXCEPTION,
    SOCK_COMMONE_EXCEPTION,
}

public class EventCenter {
	//过期
	public static readonly int SessionTimeOut = 2107;
	public static readonly int SessionUnMatched = 2108;



	private Dictionary<MsgType, Action<BaseTaskAbstract>> exchange = null;

	public EventCenter() { 
		exchange = new Dictionary<MsgType, Action<BaseTaskAbstract>>();
	}

	/// <summary>
	/// Registers the default handler. 实际上是处理默认的处理网络请求的类型
	/// </summary>
	/// <param name="kind">Kind.</param>
	/// <param name="method">Method.</param>
	public void RegisterDefaultHandler (MsgType kind, Action<BaseTaskAbstract> method) {
		if(method != null) {
            if (exchange.ContainsKey (kind) && exchange.ContainsValue (method)) {
                return;
            } else
                exchange.Add (kind, method);
               
		}
	}

	Action<BaseTaskAbstract> DefaultSessionExce {
		get {
			Action<BaseTaskAbstract> task = null;
			if(!exchange.TryGetValue(MsgType.HTTP_SESSION_TIMEOUT, out task)){
				task = null;
			}

			return task;
		}
	}

	Action<BaseTaskAbstract> DefaultHttpExce {
		get {
			Action<BaseTaskAbstract> task = null;
			if(!exchange.TryGetValue(MsgType.HTTP_EXCEPTION, out task)){
				task = null;
			}

			return task;
		}
	}

	Action<BaseTaskAbstract> DefaultSockExce {
		get {
			Action<BaseTaskAbstract> task = null;
			if(!exchange.TryGetValue(MsgType.SOCK_EXCEPTION, out task)){
				task = null;
			}
			return task;
		}
	}

    Action<BaseTaskAbstract> CommonSockExce {
        get {
            Action<BaseTaskAbstract> task = null;
            if(!exchange.TryGetValue(MsgType.SOCK_COMMONE_EXCEPTION, out task)){
                task = null;
            }
            return task;
        }
    }


	// ********************************  HTTP CALLBACK ****************************
	/*
	 * Running in the background thread
	 */ 
    void Http_OnReceive(HttpTask task)
    {
		GetResponseErrorCode(task);
		if (task != null && string.IsNullOrEmpty(task.errorInfo) )
		{
			///
			/// ----------- 分开try catch的原因是：有可能处理HttpResponse时报出异常 ， 异常之后无法正确的处理回调，-----------
			///
			try {
				//所有内部的处理，包括转换为合理的格式
				task.handleCompletedInternal(ActionOnReceiveHttpResponse.getAction(task), ActionOnReceiveHttpResponse.Sync_Operation(task) );
			} catch(Exception ex) {
				ConsoleEx.DebugLog(ex.ToString());
			} 
			
			
			///
			/// ------------- try catch保证：回调异常了之后，不影响Http线程 ------
			///
			try {
				if(task.threadType == ThreadType.MainThread) {
					AsyncTask.QueueOnMainThread( () => { task.handleMainThreadCompleted(); } );
				} else {
					task.handleBackGroundCompleted();
				}
			} catch(Exception ex) {
				ConsoleEx.DebugLog(ex.ToString());
			}
			//session error 
			HandleSessionEx (task);
			
		} else {
			//Json error.
			Http_OnException(task);
		}


    }

	//session id 不一致 情况
	void HandleSessionEx(HttpTask task){

		if (task != null && task.response != null) {
			if (task.response.errorCode == SessionTimeOut || task.response.errorCode == SessionUnMatched) {
				try {
					AsyncTask.QueueOnMainThread (() => {
						DefaultSessionExce (task);
					});
				} catch (Exception ex) {
					ConsoleEx.DebugLog (ex.ToString ());
				}
			}
		}
	}

	/*
	 * Running in the main thread
	 */
	void Http_OnException(HttpTask task) {
        ConsoleEx.DebugLog(" --> Event Center Recevied Http Exception." + task.errorInfo);
        if(!string.IsNullOrEmpty(task.errorInfo)) {
            if(Core.Data != null && Core.Data.stringManager != null) {
                string errInfoChinese = Core.Data.stringManager.getString(17);
                task.errorInfo = errInfoChinese;
            }
        }

		if(task != null) {
			//we should send following code to the main thread
			AsyncTask.QueueOnMainThread( () => { task.handleErrorOcurr(); } );

			//Default handler if ErrorOccured is null
			if(DefaultHttpExce != null && task.ErrorOccured == null)
				AsyncTask.QueueOnMainThread( () => { DefaultHttpExce(task);} );
		}

    }

	public void RegisterToHttpEngine (HttpThread httpEngine) {
		if(httpEngine != null) {
			httpEngine.Http_OnException = Http_OnException;
			httpEngine.Http_OnReceive = Http_OnReceive;
		}
	}
	

	// ******************************** SOCKET CALLBACK ****************************
	// ***************************************************************************** 
	void Sock_OnReceive(SocketTask task) {
		if (task != null && string.IsNullOrEmpty(task.errorInfo) )
		{
			///
			/// ----------- 分开try catch的原因是：有可能处理HttpResponse时报出异常 ， 异常之后无法正确的处理回调，-----------
			///
			try {
				//所有内部的处理，包括转换为合理的格式
				task.handleCompletedInternal(ActionOnReceiveSockResponse.getAction(task));
			} catch(Exception ex) {
				ConsoleEx.DebugLog(ex.ToString());
			}

			///
			/// ------------- try catch保证：回调异常了之后，不影响Http线程 ------
			///
			try {

				if(task.threadType == ThreadType.MainThread) {
					AsyncTask.QueueOnMainThread( () => { task.handleMainThreadCompleted(); } );
				} else {
					task.handleBackGroundCompleted();
				}
			} catch (Exception ex) {
				ConsoleEx.DebugLog(ex.ToString());
			}

		} else {
			//Json error.
			Sock_OnException(task);
		}
	}

	void Sock_OnException(SocketTask task) {
		ConsoleEx.DebugLog(" --> Event Center Recevied Socket Exception");

		if(task != null) {
			//we should send following code to the main thread
			AsyncTask.QueueOnMainThread( () => { task.handleErrorOcurr(); } );

			//Default handler if ErrorOccured is null
			if(DefaultSockExce != null && task.ErrorOccured == null)
				AsyncTask.QueueOnMainThread( () => { DefaultSockExce(task);} );
		}

	}

	void Sock_OnCommonExcption(string error) {
		ConsoleEx.DebugLog(" --> Event Center Recevied Common Socket Exception. " + error);
		//Default handler if ErrorOccured is null
		if(DefaultSockExce != null) {
			SocketTask sTask = new SocketTask(ThreadType.MainThread);
			sTask.errorInfo = error;
			AsyncTask.QueueOnMainThread( () => { DefaultSockExce(sTask);} );
		}

        if(CommonSockExce != null) {
            SocketTask sTask = new SocketTask(ThreadType.MainThread);
            sTask.errorInfo = error;
            AsyncTask.QueueOnMainThread( () => { CommonSockExce(sTask);} );
        }

	}
		
	public void RegisterToSockEngine (SocketEngine sockEngine) {
		if(sockEngine != null) {
			sockEngine.Socket_OnException = Sock_OnException;
			sockEngine.Socket_OnReceive = Sock_OnReceive;
			sockEngine.Socket_CommException = Sock_OnCommonExcption;
		}
	}

	void GetResponseErrorCode(HttpTask _task)
	{
		if(_task != null)
		{
			if(_task.response != null)
			{
				if(_task.response.errorCode == 2109)
				{
					MessageMgr.GetInstance().Upgrape = _task.response.msg.url;
					AsyncTask.QueueOnMainThread(() => {UIVersinUpgrade.OpenUI();});
				}
			}
		}
	}

	// ******************************** TIMER CALLBACK ****************************
}
