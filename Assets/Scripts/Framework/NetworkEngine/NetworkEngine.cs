using System;
using System.Net;

public class NetworkEngine : ICore {
    //Http client
    public HttpThread httpEngine;
    //Socket client
	public SocketEngine SockEngine;

    public NetworkEngine() {
        httpEngine = HttpThread.getInstance();
		SockEngine = SocketEngine.getInstance();
    }

    void ICore.Dispose()
    {
        if (httpEngine != null)
        {
            HttpTask shutdownTask = new HttpTask(ThreadType.BackGround, TaskResponse.Default_Response);
            shutdownTask.AppendCmdParam(InternalRequestType.SHUT_DOWN);
            httpEngine.sendHttpTask(shutdownTask);
        }
        // Socket is still empty
		if(SockEngine != null) {
			SocketTask shutdownTask = new SocketTask(ThreadType.BackGround, TaskResponse.Default_Response);
			shutdownTask.AppendCmdParam(InternalRequestType.SHUT_DOWN);
			SockEngine.sendSocketTask(shutdownTask);
		}
    }

    void ICore.Reset()
    {
        throw new NotImplementedException();
    }


	/// <summary>
	/// Raises the login event. 我会在这里控制底层的HttpThread 和Socket Engine的问题
	/// </summary>
	/// <param name="obj">Object.</param>
	void ICore.OnLogin(Object obj) {
		NewworkObj netObj = obj as NewworkObj;

		ICore httpIc = httpEngine as ICore;
		httpIc.OnLogin(netObj.dpm);
		// Socket is still empty
		SockEngine.OnLogin(netObj.dep);
    }
}

public class NewworkObj {
	public DataPersistManager dpm;
	public DnsEndPoint dep;

	public NewworkObj(DataPersistManager dataPm, DnsEndPoint point) {
		dpm = dataPm;
		dep = point;
	}
}