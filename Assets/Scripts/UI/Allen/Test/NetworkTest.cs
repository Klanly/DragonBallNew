using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SuperSocket.ClientEngine;

public class NetworkTest : MonoBehaviour {

	private static NetworkTest instance;
	public static NetworkTest Instance
	{
		get
		{
			return instance;
		}
	}

	void OnGUI() {

		if(GUI.Button(new Rect(10,10,100,100), "get Battle Info")) {
			sendBattleRequest();
		}

		if(GUI.Button(new Rect(10, 210, 100, 100), "Socket Connect.")) {
			sendTcpRequest();
		}

		if(GUI.Button(new Rect(10, 320, 200, 200), "Allen Socket Connect.")) {
				sendAllenTcpOpen();
		}

		if(GUI.Button(new Rect(10, 520, 100, 100), "Allen Socket DisConnect.")) {
				SendAllenTcpClose();
		}
	}
	void Awake(){
        instance = this;
	}
	#region Http Operation
	void sendBattleRequest() {
		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
		//HttpTask task = new HttpTask(ThreadType.MainThread); is the same as above line.

//		Core.Data.temper.mPreLevel = Core.Data.playerManager.RTData.curLevel;
//		Core.Data.temper.mPreVipLv = Core.Data.playerManager.RTData.curVipLevel;

		task.AppendCommonParam(RequestType.PVE_BATTLE, new BattleParam("692793",60105,50010,40001));

		task.ErrorOccured += HttpResp_Error;
		task.afterCompleted += HttpResp_UI;

		//then you should dispatch to a real handler
		task.DispatchToRealHandler();
	}

	void HttpResp_UI (BaseHttpRequest request, BaseResponse response) {
		ConsoleEx.DebugLog(" --- Http Resp - running in the main thread, UI purpose --" + response.GetType().ToString());
		ConsoleEx.DebugLog("Battle Response " + JsonFx.Json.JsonWriter.Serialize(response));


	}


	void HttpResp_Error (BaseHttpRequest request, string error) {
		ConsoleEx.DebugLog("---- Http Resp - Error has ocurred." + error);

	}
	#endregion

	#region Socket Operation


	public void sendTcpRequest() {

		DataHandler handler = new DataHandler();
		NonBlockingConnection conn = new NonBlockingConnection("192.168.1.55", 8001, handler);

		System.Threading.Thread.Sleep(1000);

		for(int i=0; i<10; i++)
		{
			conn.Write("madongfang\r\n");
		}
	}

	class DataHandler : ISystemHandler {
		// 创建连接
		public bool OnConnect
		(NonBlockingConnection conn)
		{
			ConsoleEx.DebugLog("Connect to the server  in  test!");
			return false;
		}

		// 接收数据
		public bool OnData(NonBlockingConnection conn)
		{
			ConsoleEx.DebugLog("Receive data from the server !!");
			IList<string> json = conn.ReadStrings();
			ConsoleEx.DebugLog("The list size is " + json.Count);
			foreach(string s in json)
			{
				ConsoleEx.DebugLog(s + "==========");
			}
			return true;
		}

		// 断开连接
		public bool OnDisconnect(NonBlockingConnection conn)
		{
			ConsoleEx.DebugLog("DisConnect from the server !!");
			return true;
		}

		// 发生异常
		public bool OnException(NonBlockingConnection conn, Exception e)
		{
			ConsoleEx.DebugLog("Exception occur, the exception is " + e.ToString());
			return true;
		}
	}


	#endregion


	#region Socket Operation More

	private void sendAllenTcpOpen () {
		SocketTask task = new SocketTask(ThreadType.MainThread, TaskResponse.Default_Response);

        task.AppendCommonParam(RequestType.SOCK_LOGIN, new SockLoginParam("656679",null,0));

		task.ErrorOccured = SocketResp_Error;
		task.afterCompleted = SocketRespUI;

		task.DispatchToRealHandler();
	}

	void SocketRespUI (BaseSocketRequest request, BaseResponse response) {
		ConsoleEx.DebugLog(" --- Http Resp - running in the main thread, UI purpose --" + response.GetType().ToString());
		ConsoleEx.DebugLog("Socket Login Response " + JsonFx.Json.JsonWriter.Serialize(response));
	}


	void SocketResp_Error (BaseSocketRequest request, string error) {
		ConsoleEx.DebugLog("---- Socket Resp - Error has ocurred." + error);
	}


	private void SendAllenTcpClose() {
		SocketTask task = new SocketTask(ThreadType.MainThread, TaskResponse.Default_Response);

		task.AppendCmdParam(InternalRequestType.SHUT_DOWN);

		task.DispatchToRealHandler();
	}

	#endregion


}
