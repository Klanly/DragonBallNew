using UnityEngine;
using System.Collections;

public class UITest : MonoBehaviour 
{

	void OnGUI() 
	{
		if (GUI.Button(new Rect(10, 10, 250, 100), "Http Task- On MainThread")) {
			//testHttpTask_OnMainThread();
            appendText();
		}
		 
//		if(GUI.Button(new Rect(10, 120, 250, 100), "Http Task- On Background Thread"))
//			testHttpTask_OnBackGround();

		if(GUI.Button(new Rect(10, 230, 250, 100), "Internal Http Task - Close Http Engine"))
			testHttpTask_InternalCommand();

		if(GUI.Button(new Rect(10, 340, 250, 100), "Customized Int32"))
			test_MyInt();


		if(GUI.Button(new Rect(260, 10, 250, 100), "Timer Task"))
			testTimer();

		if(GUI.Button(new Rect(260, 120, 250, 100), "Timer Task2"))
			testTimer();

		if(GUI.Button(new Rect(260, 230, 250, 100), "Download Resources.."))
			test_DownloadResource();

        if(GUI.Button(new Rect(260, 340, 250, 100), "Filte Bad words."))
            Test_BadWord();
	}


    void appendText() {
        SourceManager.SaveToLocalDiskAppend("this is a test.");
    }

    #region rich text
    void RichText() {
        GUIStyle style = new GUIStyle ();
        style.richText = true;
        GUILayout.Label("<size=30>Some <color=yellow>RICH</color> text</size>",style);
    }
    #endregion

	Int32Fog testValue;
	void test_MyInt () {
		testValue = 12345;

		int add = testValue + 12345;

		int minus = testValue - 1000;

		int mul = testValue * 3;

		int dividual = testValue / 2;

		ConsoleEx.DebugLog("testValue " + testValue);

		ConsoleEx.DebugLog("add = " + add + " \t minus = " + minus + "\t mul = " + mul + "\t dividual = " + dividual);
	}

	#region Http Task
	/// <summary>
	/// Test the http task & sample
	/// </summary>
//	void testHttpTask_OnMainThread () {
//
//		HttpTask task = new HttpTask(ThreadType.MainThread, TaskResponse.Default_Response);
//		//HttpTask task = new HttpTask(ThreadType.MainThread); is the same as above line.
//
//		task.AppendCommonParam(RequestType.GET_PARTITION_SERVER, new PartitionServerParam(1,Native.mInstace.m_thridParty.GetAccountData()) );
//
//		Debug.Log("task request = " + JsonFx.Json.JsonWriter.Serialize(task) );
//
//		task.taskCompeleted += testHttpResp_Mainthread;
//		task.ErrorOccured += testHttpResp_Error;
//		task.afterCompleted += testHttpResp_UI;
//
//		//then you should dispatch to a real handler
//		task.DispatchToRealHandler();
//
//	}

//	void testHttpTask_OnBackGround() {
//		HttpTask task = new HttpTask(ThreadType.BackGround, TaskResponse.Default_Response);
//		//HttpTask task = new HttpTask(ThreadType.BackGround); is the same as above line.
//
//		task.AppendCommonParam(RequestType.GET_PARTITION_SERVER, new PartitionServerParam(1,Native.mInstace.m_thridParty.GetAccountData()) );
//
//		Debug.Log("task request = " + JsonFx.Json.JsonWriter.Serialize(task) );
//
//		task.taskCompeleted += () => { ConsoleEx.DebugLog(" --- Http Resp - running in the background, ### Lambda Expression ###"); };
//		task.ErrorOccured += testHttpResp_Error;
//
//		//then you should dispatch to a real handler
//		task.DispatchToRealHandler();
//	}

	void testHttpTask_InternalCommand() {
		HttpTask task = new HttpTask(ThreadType.BackGround);
		task.AppendCmdParam(InternalRequestType.SHUT_DOWN);

		Debug.Log("task request = " + JsonFx.Json.JsonWriter.Serialize(task) );

		//then you should dispatch to a real handler
		task.DispatchToRealHandler();
	}


	void testHttpResp_Background() {
		ConsoleEx.DebugLog(" --- Http Resp - running in the background ---");
	}


	void testHttpResp_Mainthread() {
		ConsoleEx.DebugLog(" --- Http Resp - running in the main thread ---");
	}


	void testHttpResp_UI (BaseHttpRequest request, BaseResponse response) {
		ConsoleEx.DebugLog(" --- Http Resp - running in the main thread, UI purpose --");
	}


	void testHttpResp_Error (BaseHttpRequest request, string error) {
		ConsoleEx.DebugLog("---- Http Resp - Error has ocurred." + error);
	}

	#endregion

	#region Timer Task

	void testTimer() {
		//We must tell Timer the system unix time stamp, so right now we simulate it for test purpose.
		Core.TimerEng.OnLogin(DateHelper.LocalDateTimeToUnixTimeStamp(System.DateTime.UtcNow));

		//-------- 
		TimerTask task = new TimerTask(DateHelper.LocalDateTimeToUnixTimeStamp(System.DateTime.UtcNow), DateHelper.LocalDateTimeToUnixTimeStamp(System.DateTime.UtcNow) + 10, 1);

		//All this methods is running in the background
		task.onEventBegin += eventBegin;
		task.onEventEnd += eventEnd;

		// equals to task.onEvent += frequent;
		task.onEvent += (TimerTask t) => {
			Debug.Log("Timer Engine : = on event frequent. => " + t.leftTime + " 111");
		} ;


		task.DispatchToRealHandler();

		testTimer1();

	}

	void testTimer1()
	{
		//-------- 

//		Core.TimerEng.OnLogin(1007);
		TimerTask task = new TimerTask(DateHelper.LocalDateTimeToUnixTimeStamp(System.DateTime.UtcNow) + 7, DateHelper.LocalDateTimeToUnixTimeStamp(System.DateTime.UtcNow) + 11, 1);

		//All this methods is running in the background
		task.onEventBegin += eventBegin;
		task.onEventEnd += eventEnd;

		// equals to task.onEvent += frequent;
		task.onEvent += (TimerTask t) => {
			Debug.Log("Timer Engine : = on event frequent. => " + t.leftTime + " 22");
		} ;


		task.DispatchToRealHandler();
	}


	void eventBegin (TimerTask task) {
		Debug.Log("Timer Engine : = on event begin.");
	}

	void eventEnd (TimerTask task) {
		Debug.Log("Timer Engine : = on event end.");
	}

	void frequent(TimerTask task) {
		Debug.Log("Timer Engine : = on event frequent. => " + task.leftTime);
	}

	#endregion 


	#region 下载资源

	void test_DownloadResource () {
		HttpDownloadTask task = new HttpDownloadTask(ThreadType.MainThread, TaskResponse.Igonre_Response);
		string url = @"http://zhangyuntao.com.cn/phpMyAdmin-4.1.0-all-languages.zip";
		string fn = "license.zip";
		string path = DeviceInfo.PersistRootPath;
		string md5 = "test md5";
		long size = 103234;
		task.AppendDownloadParam(url, fn, path, md5, size);

		task.DownloadStart += (string durl) => { ConsoleEx.DebugLog("Download starts and url = " + durl); };
		 
		task.taskCompeleted += () => { ConsoleEx.DebugLog("Download success."); };
		task.afterCompleted += testDownloadResp;
		task.ErrorOccured += (s,t) => { ConsoleEx.DebugLog("Download failure.");};

		task.Report += (cur,total) => {ConsoleEx.DebugLog("current Bytes = " + cur + ", total Bytes = " + total);};

		task.DispatchToRealHandler();
	}

	void testDownloadResp (BaseHttpRequest request, BaseResponse response) {
		ConsoleEx.DebugLog("Download success UI.");
	}

	#endregion
	
    #region 关键字过滤

    void Test_BadWord() {
        bool reuslt = false;
        reuslt = SensitiveFilterManager.getInstance().check("$");
        Debug.Log("result = " + reuslt);
    }

    #endregion

}
