using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class JCPVETimerManager : MonoBehaviour 
{	
	
	[HideInInspector]
	public bool isExpFBCoding = false;
	[HideInInspector]
	public bool isGemFBCoding = false;
	[HideInInspector]
	public bool isSkillFBCoding = false;
	[HideInInspector]
	public bool isFightSoulFBCoding = false;
	//经验副本冷却倒计时
	public System.Action<long> ExpFBCoding;
	//宝石副本冷却倒计时
	public System.Action<long> GemFBCoding;
	//技能副本倒计时
	public System.Action<long> SkillFBCoding;
	//战魂副本倒计时
	public System.Action<long> FightSoulFBCoding;
	//程序是否正在运行
	private bool isApplicationPause = false;
	
	//唤醒程序
	void OnApplicationPause(bool pauseStatus) 
	{	
		isApplicationPause = pauseStatus;
		if(!pauseStatus)
		{
			AutoOpenPVESystemTimer();
			if(Core.Data.temper.ExpDJS == 0)
			{
				isExpFBCoding = false;
				if(ExpFBCoding != null)
				   ExpFBCoding(0);
			}
			if(Core.Data.temper.GemDJS == 0)
			{
				isGemFBCoding = false; 
				if(GemFBCoding != null)
				   GemFBCoding(0);
			}
			if(Core.Data.temper.SkillDJS == 0)
			{
				isSkillFBCoding = false;
				if(SkillFBCoding != null)
				{
				   SkillFBCoding(0);
				}
			}
			if(Core.Data.temper.FightSoulDJS == 0)
			{
				isFightSoulFBCoding = false;
				if(FightSoulFBCoding != null)
				   FightSoulFBCoding(0);
			}
		}
	}
	
	private static JCPVETimerManager _Instance;
	public static JCPVETimerManager Instance
	{
		get
		{
			if(_Instance == null)
			{
				GameObject JCPVETimerObj = new GameObject();
				JCPVETimerObj.name = "JCPVETimerManager";
				JCPVETimerObj.transform.parent = GameObject.Find("Gobal").transform;
				DontDestroyOnLoad(JCPVETimerObj);
				_Instance = JCPVETimerObj.AddComponent<JCPVETimerManager>();
				
			}
			return _Instance;
		}
	}
	
	//经验副本倒计时任务
	TimerTask Exp_Task = null;
	//宝石副本倒计时任务
	TimerTask Gem_Task = null;
	
	//立即消毁经验副本<用于12点的时候立即销毁>
	public void DeleteExpTaskImmediately()
	{
		if(Exp_Task != null)
		{
			Core.TimerEng.deleteTask(Exp_Task);
			Core.Data.temper.ExpDJS = 0;
			isExpFBCoding = false;
			if(ExpFBCoding != null)
			   ExpFBCoding(0);
		}
	}
	
	//立即消毁宝石副本<用于12点的时候立即销毁>
	public void DeleteGemTaskImmediately()
	{
		if(Gem_Task != null)
		{
			Core.TimerEng.deleteTask(Gem_Task);
			Core.Data.temper.GemDJS = 0;
			isGemFBCoding = false; 
			if(GemFBCoding != null)
			   GemFBCoding(0);
		}
	}
	
	
	//自动开启PVE系统所有计时器
	public void AutoOpenPVESystemTimer()
	{
		ExplorDoors explorDoors = Core.Data.newDungeonsManager.explorDoors;	   
	    if(explorDoors != null)
		{
		    //JCPVETimerManager jctm = JCPVETimerManager.Instance;		
			
		     if(explorDoors.exp!= null)
			{
				if(Core.Data.temper.ExpDJS == 0 && explorDoors.exp.passCount < explorDoors.exp.count)
		    	   CreateTimerExp(explorDoors.exp.startTime,explorDoors.exp.endTime,1);
			}
	        else
				   DeleteExpTaskImmediately();
			
		    if(explorDoors.gems!= null )	
			{
				if(Core.Data.temper.GemDJS == 0 && explorDoors.gems.passCount < explorDoors.gems.count)
			        CreateTimerGem(explorDoors.gems.startTime,explorDoors.gems.endTime,1);
			}
			else
				    DeleteGemTaskImmediately();
			
			if(explorDoors.skill!= null &&  Core.Data.temper.SkillDJS == 0 && explorDoors.skill.passCount < explorDoors.skill.count)					
				 CreateTimerSkill(explorDoors.skill.startTime,explorDoors.skill.endTime,1);
			
			if(explorDoors.souls != null && Core.Data.temper.FightSoulDJS == 0 && explorDoors.souls.passCount < explorDoors.souls.count)					
				 CreateTimerFightSoul(explorDoors.souls.startTime,explorDoors.souls.endTime,1);
		}		
	}
	
	/*经验副本计时*/
	public void CreateTimerExp(long startTime,long endTime,int interval) 
	{
		long now = Core.TimerEng.curTime;
		//时间已到
		if(startTime >= endTime || now >=endTime)
		{
			if(ExpFBCoding != null)
			ExpFBCoding(0);
			return;
		}	
		
		Exp_Task = new TimerTask(now, endTime, interval, ThreadType.MainThread);
		Exp_Task.onEvent =  (TimerTask t) =>
		{
			Core.Data.temper.ExpDJS = t.leftTime;
			if(!isApplicationPause)
			{
				AsyncTask.QueueOnMainThread( () =>
				{		
					isExpFBCoding = true;
					if(ExpFBCoding != null)
					ExpFBCoding(t.leftTime);				
				} );
			}
		};		
		Exp_Task.onEventEnd =  (TimerTask t) =>
		{
			Core.Data.temper.ExpDJS = 0;
			if(!isApplicationPause)
			{
				AsyncTask.QueueOnMainThread( () =>
				{	
					isExpFBCoding = false;
					if(ExpFBCoding != null)
					ExpFBCoding(0);
				} );
			}
		};
		Exp_Task.DispatchToRealHandler();
	}
	
	/*宝石副本计时*/
    public void CreateTimerGem(long startTime,long endTime,int interval) 
	{
		long now = Core.TimerEng.curTime;
		//时间已到
		if(startTime >= endTime || now > endTime)
		{
			if(GemFBCoding != null)
			GemFBCoding(0);
			return;
		}

		Gem_Task = new TimerTask(now, endTime, interval, ThreadType.MainThread);
		Gem_Task.onEvent =  (TimerTask t) =>
		{
		    Core.Data.temper.GemDJS = t.leftTime;
			if(!isApplicationPause)
			{
				AsyncTask.QueueOnMainThread( () =>
				{
					isGemFBCoding = true;
					if(GemFBCoding != null)
					GemFBCoding(t.leftTime);
				});
			}
		};
		Gem_Task.onEventEnd =  (TimerTask t) =>
		{
			Core.Data.temper.GemDJS = 0;
			if(!isApplicationPause)
			{
				AsyncTask.QueueOnMainThread( () =>
				{
					isGemFBCoding = false; 
					if(GemFBCoding != null)
					GemFBCoding(0);
				});
			}
		};
		Gem_Task.DispatchToRealHandler();
	}
	
	/*技能副本计时*/
	public void CreateTimerSkill(long startTime,long endTime,int interval) 
	{
		long now = Core.TimerEng.curTime;
		//时间已到
		if(startTime >= endTime || now >=endTime)
		{
			if(SkillFBCoding != null)
			{
				isSkillFBCoding = false;				
			    SkillFBCoding(0);
			}
			return;
		}

		TimerTask task = new TimerTask(now, endTime, interval, ThreadType.MainThread);
		
		isSkillFBCoding = true;
		task.onEvent =  (TimerTask t) =>
		{
			Core.Data.temper.SkillDJS = t.leftTime;
			if(!isApplicationPause)
			{				
				AsyncTask.QueueOnMainThread( () =>
				{					
					if(SkillFBCoding != null)
					{
					    SkillFBCoding(t.leftTime);
					}
				} );
			}
		};		
		task.onEventEnd =  (TimerTask t) =>
		{			
			Core.Data.temper.SkillDJS = 0;
			if(!isApplicationPause)
			{	
				AsyncTask.QueueOnMainThread( () =>
				{
					isSkillFBCoding = false;
					if(SkillFBCoding != null)
					SkillFBCoding(0);
				});
			}
		};

		task.DispatchToRealHandler();
	}
	
	/*战魂副本计时*/
	public void CreateTimerFightSoul(long startTime,long endTime,int interval) 
	{
		long now = Core.TimerEng.curTime;
		//时间已到
		if(startTime >= endTime || now >= endTime)
		{
			if(FightSoulFBCoding != null)
			{
				isFightSoulFBCoding = false;	
			    FightSoulFBCoding(0);
			}
			return;
		}
		
		TimerTask task = new TimerTask(now, endTime, interval, ThreadType.MainThread);

		isFightSoulFBCoding = true;
		task.onEvent =  (TimerTask t) =>
		{
			if(!isApplicationPause)
			{
				Core.Data.temper.FightSoulDJS =  t.leftTime;
				AsyncTask.QueueOnMainThread( () =>
				{
					if(FightSoulFBCoding != null)
					FightSoulFBCoding(t.leftTime);
				} );
			}
		};		
		task.onEventEnd =  (TimerTask t) =>
		{
			Core.Data.temper.FightSoulDJS = 0;
			if(!isApplicationPause)
			{	
				AsyncTask.QueueOnMainThread( () =>
				{
					isFightSoulFBCoding = false;
					if(FightSoulFBCoding != null)
					FightSoulFBCoding(0);
				} );
			}
		};
		task.DispatchToRealHandler();
	}
	
}


