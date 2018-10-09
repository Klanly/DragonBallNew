using UnityEngine;
using System.Collections;

public class RollTaskState : MonoBehaviour {

		
	public GameObject JianTou;
	
	public static RollTaskState Instance;
	
	void Awake()
	{
		Instance = this;
	}
	
	void OnEnable () 
	{
		//弱引导(任务)触发条件
		if(Core.Data.AccountMgr.UserConfig.weekGuide_Wheel == 0 &&  Core.Data.playerManager.RTData.curLevel >= 10  && !Core.Data.guideManger.isGuiding)
		{
			IsWeekGuides = true;
		}
		else
		{
			if(IsWeekGuides)
			IsWeekGuides = false;
		}
		
	}
	

	
	bool isWeekGuide = false;
	public bool IsWeekGuides
	{
		get
		{
			return isWeekGuide;
		}
		set
		{
			isWeekGuide = value;
			JianTou.SetActive(isWeekGuide);
		}
	}
}
