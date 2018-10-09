using UnityEngine;
using System.Collections;

public class GuideTask : MonoBehaviour {
	
	//string ButtonName = "Start talk";

	
	void Start ()
	{

	}
	
	void OnGUI()
	{

	    if(GUI.Button(new Rect(400,50,100,50),"XXXXXXXXX"))
		{
//			//1411719534    1411765219
//			JCTimerData data = JCTimerDic.Instance.CreateTimer( 0,1411719534 , 1411765219,1);
//			data.onEvent =(TimerTask t) =>
//			{
//				Debug.LogError("sssssssssssss");
//			};
			//recordDayChanged(Core.Data.playerManager.RTData.systemTime);
			//Debug.Log(Core.Data.guideManger.isGuiding.ToString());
			//DBUIController.mDBUIInstance._PVERoot.ResetPVESystem();
			Core.Data.AccountMgr.UserConfig.SpecialGuideID = 3000;
			Core.Data.guideManger.Init();
		}
		
		
	}

	
	public void recordDayChanged(long sysTime) 
	{
		//long LeftOfDayEnd = DateHelper.getLeftTimingAtDayChanged(sysTime);
		
		long now = Core.TimerEng.curTime;
		TimerTask task = new TimerTask(now+1, now + 10, 1, ThreadType.MainThread);
		
		//Debug.LogError("now="+now.ToString() +  "    "+(now + LeftOfDayEnd).ToString() );
		
		//System.Action<TimerTask > OnChange = OnDayChanged ;
		
		task.onEventBegin += OnDayChangedA;
		task.onEvent =  OnDayChanged;
		
		task.onEventEnd = OnDayChangedB;
		task.DispatchToRealHandler();
	}
	
	
	void OnDayChangedA(TimerTask t)
	{
		Debug.LogError("AAAAAAAAAAAAA");
	}
	
	void OnDayChangedB(TimerTask t)
	{
		Debug.LogError("BBBBBBBBBBBBBB");
	}
	
	void OnDayChanged(TimerTask t)
	{
		Debug.LogError("sssssssssssss");
	}
	
}
