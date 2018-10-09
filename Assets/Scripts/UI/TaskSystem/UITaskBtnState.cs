using UnityEngine;
using System.Collections;

public class UITaskBtnState : MonoBehaviour {

	public UISprite sgin;
	
	public static UITaskBtnState _this;
	
	public GameObject JianTou;
	void Awake()
	{
	    _this = this;
	}
	
	
	void OnEnable()
	{
		_this = this;
		refresh();
	}
	
	public void refresh()
	{
		sgin.enabled = Core.Data.taskManager.isHaveTaskComplete;
		if(sgin.enabled && !Core.Data.guideManger.isGuiding && Core.Data.playerManager.Lv < Core.Data.guideManger.TaskSystemWeekGuideTiggerLevel)
		{
			if(JianTou != null && !JianTou.gameObject.activeSelf)
			JianTou.SetActive(true);
		}
		else
		{
			if(JianTou != null && JianTou.activeSelf)
			JianTou.SetActive(false);
		}
	}
	
	public static void Refresh()
	{
		if(_this!=null)
			_this.refresh();

		if (SQYMainController.mInstance != null)
			SQYMainController.mInstance.UpdateTopTip ();
	}

}
