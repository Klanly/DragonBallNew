using UnityEngine;
using System.Collections;
using System;

public enum MDAlertBehaviour{
	
	WILL_SHOW,
	DID_SHOW,
	
	WILL_HIDDEN,
	DID_HIDDEN,
	
	CLICK_OK,
	CLICK_CANCEL,
	CLICK_BACKGROUND,
	
}
public class MDBaseAlertView : RUIMonoBehaviour {
	public int alertID;
	public Action<MDAlertBehaviour,MDBaseAlertView> alertViewBehavriour;
	
	
	public virtual void showView()
	{
		if(alertViewBehavriour!=null)
		{
			alertViewBehavriour(MDAlertBehaviour.WILL_SHOW,this);
		}
	}
	public virtual void showViewEnd()
	{
		if(alertViewBehavriour!=null)
		{
			alertViewBehavriour(MDAlertBehaviour.DID_SHOW,this);
		}
	}
	public virtual void hiddenView()
	{
		if(alertViewBehavriour!=null)
		{
			alertViewBehavriour(MDAlertBehaviour.WILL_HIDDEN,this);
		}
	}
	public virtual void hiddenViewEnd()
	{
		if(alertViewBehavriour!=null)
		{
			alertViewBehavriour(MDAlertBehaviour.DID_HIDDEN,this);
		}
	}
	public virtual void OnBtnCancel()
	{
		if(alertViewBehavriour!=null)
		{
			alertViewBehavriour(MDAlertBehaviour.CLICK_CANCEL,this);
		}
	}
	public virtual void OnBtnOk()
	{
		if(alertViewBehavriour!=null)
		{
			alertViewBehavriour(MDAlertBehaviour.CLICK_OK,this);
		}
	}
	public virtual void OnBtnBackGround()
	{
		if(alertViewBehavriour!=null)
		{
			alertViewBehavriour(MDAlertBehaviour.CLICK_BACKGROUND,this);
		}
	}
}