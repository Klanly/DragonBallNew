using UnityEngine;
using System.Collections;
using System;
// yangcg tui song 2014/06/24
public class IOSLocalPush : MonoBehaviour {

	PlayerManager player = null; 

	#region singleton
	private static GameObject sGameObj;

	public static IOSLocalPush getInstance() {
		if (sGameObj == null) {
			sGameObj = new GameObject();
			sGameObj.name = "IOSLocalPush";
			//keep alive
			DontDestroyOnLoad(sGameObj);

			// add child to gobal 
			var gobal = GameObject.FindGameObjectWithTag("Gobal");
			if(gobal != null) sGameObj.transform.parent = gobal.transform;

			return (IOSLocalPush) sGameObj.AddComponent(typeof(IOSLocalPush));
		}
		
		return (IOSLocalPush) sGameObj.GetComponent(typeof(IOSLocalPush));
	}
	#endregion

	public void notifyLoggedin() {
		player = Core.Data.playerManager;
	}

#if UNITY_IPHONE
	/// <summary>
	/// Notifications the message.
	/// </summary>
	/// <param name="str">String.</param>
	/// <param name="hours">Hours.</param>
	/// <param name="isRepeatDay">If set to <c>true</c> is repeat day.</param>
	public static void NotificationMessage(string str , int hours , bool isRepeatDay)
	{

		int year = System.DateTime.Now.Year ;
		int month = System.DateTime.Now.Month; 
		int days = System.DateTime.Now.Day; 
		System.DateTime nowTime = new System.DateTime (year ,month,days ,hours , 0,0); // System.DateTime
		NotificationMessage(str ,nowTime ,isRepeatDay);

	}
	/// <summary>
	/// Notifications the message.
	/// </summary>
	/// <param name="str">推送内容</param>
	/// <param name="nowDatatime">推送的时间</param>
	/// <param name="isRepeatDay">是否是定点</param>
	public static void NotificationMessage(string str , System.DateTime nowDatatime , bool isRepeatDay)
	{
		if (nowDatatime > System.DateTime.Now)
		{

			LocalNotification localNotification = new LocalNotification();
			localNotification.fireDate =nowDatatime;   
			localNotification.alertBody = str;
			localNotification.applicationIconBadgeNumber =localNotification.applicationIconBadgeNumber + 1 ;

			//localNotification.repeatInterval
			//localNotification.repeatInterval = CalendarUnit.Second;
			localNotification.hasAction = true;
			if (isRepeatDay) 
			{
				//是否每天定期循环
				localNotification.repeatCalendar = CalendarIdentifier.ChineseCalendar;
				localNotification.repeatInterval = CalendarUnit.Day;

			}
			localNotification.soundName = LocalNotification.defaultSoundName;
			NotificationServices.ScheduleLocalNotification(localNotification);
		}
	}
	void Awake()
	{
		//清空所有本地消息
		CleanNotification ();
	}
	void OnApplicationPause(bool paused)
	{

		//程序进入后台时
		if(paused)
		{
			setTStimes(1) ; 
			//Debug.Log("setTStimes(1)");
			setTStimes(2) ;
			//Debug.Log("setTStimes(2)");
			setTStimes(3) ; 
			//Debug.Log("setTStimes(3)");
			//10秒后发送
			//NotificationMessage("YANGCG : 10秒后发送",System.DateTime.Now.AddSeconds(10),false);
			//每天中午12点推送
			//NotificationMessage("YANGCG : 每天中午12点推送",12,true);
			
		}
		else
		{
			//程序从后台进入前台时
			CleanNotification();

		}

	}

	//清空所有本地消息
	void CleanNotification()
	{

		LocalNotification l = new LocalNotification ();

		l.applicationIconBadgeNumber = -1;

		NotificationServices.PresentLocalNotificationNow (l);

		NotificationServices.CancelAllLocalNotifications ();

		NotificationServices.ClearLocalNotifications ();

	}
	//开始计时推送
	void setTStimes(int value )
	{
		switch(value)
		{
		case 1: // 体力

			TLPush(); 

			break ;
		case 2: // 精力 5minutes  1;


			JLPush() ;
		
			break ; 
		case 3: //
			//每天中午12点推送
			NotificationMessage("吃拉面的时间到啦，快来开餐吧",12,true);
			//每天下午18点推送
			NotificationMessage("吃拉面的时间到啦，快来开餐吧",18,true);
			break ; 
		}
	}
	//体力推送
	private void TLPush()
	{
		int TLDiff = player.totalTili - player.curTiLi ; 
		//Debug.Log ("推送内容 体力 ==" + TLDiff);
		if (TLDiff > 0 )
		{
//			if (DBUIController.mDBUIInstance == null )
//			{
//				//Debug.LogError("DBUIController.mDBUIInstance == null") ; 
//			}
//			if (DBUIController.mDBUIInstance._playerViewCtl == null ) 
//			{
//				//Debug.LogError("DBUIController.mDBUIInstance._playerViewCtl  == null") ; 
//			}
			if(DBUIController.mDBUIInstance == null || DBUIController.mDBUIInstance._playerViewCtl == null) return;
			TimeSpan powerTime = DBUIController.mDBUIInstance._playerViewCtl.powerTime ; 
			int TLhours   = powerTime.Hours   ; 
			int TLMinutes = powerTime.Minutes ; 
			int TLSeconds = powerTime.Seconds ; 
			int secondsDiff 	= (TLDiff-1) * 300; 
			int tatolSeconds 	= TLhours*3600  + TLMinutes*60 + TLSeconds  + secondsDiff; 

			int dayHours = System.DateTime.Now.AddSeconds(tatolSeconds).Hour;
			if (dayHours> 9 || dayHours < 22)
			{
				NotificationMessage("小伙伴们已经饥渴难耐，快带他们去冒险吧",System.DateTime.Now.AddSeconds(tatolSeconds),false);
			}
		}

	}
	//精力推送
	private void JLPush()
	{
		int JLDiff =player.totalJingli  - player.curJingLi   ; 
		//Debug.Log ("推送内容 精力  == " + JLDiff);
		if (JLDiff > 0)
		{
			if (DBUIController.mDBUIInstance == null )
			{
			//	Debug.LogError("DBUIController.mDBUIInstance == null") ; 
			}
			if (DBUIController.mDBUIInstance._playerViewCtl == null ) 
			{
			//	Debug.LogError("DBUIController.mDBUIInstance._playerViewCtl  == null") ; 
			}

			TimeSpan energyTime = SQYPlayerController.energyTime;//DBUIController.mDBUIInstance._playerViewCtl.energyTime ; 
			int JLhours 		= energyTime.Hours ; 
			int JLMinutes 		= energyTime.Minutes ; 
			int JLSeconds 		= energyTime.Seconds ;
			int secondsDiff 	= (JLDiff-1) * 300; 
			int tatolSeconds 	= JLhours*3600  + JLMinutes*60 + JLSeconds  + secondsDiff; 

			int dayHours = System.DateTime.Now.AddSeconds(tatolSeconds).Hour;
			if (dayHours > 9 || dayHours < 22)
			{
				NotificationMessage("小伙伴们已经饥渴难耐，快待他们去冒险吧",System.DateTime.Now.AddSeconds(tatolSeconds),false);
			}
		}
	}

#endif

#if UNITY_ANDROID

#endif

}
