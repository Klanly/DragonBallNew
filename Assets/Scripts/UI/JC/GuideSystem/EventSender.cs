using UnityEngine;
using System.Collections;

public class EventSender 
{
    public delegate void OnEvent(EventTypeDefine p_event, object p_param = null);
    public static OnEvent onEvent;
    public static void SendEvent(EventTypeDefine p_event, object p_param = null)
    {
        if (onEvent != null)
        {
			Core.Data.guideManger.CanClickGuideUI = false;
            onEvent(p_event, p_param);
        }
		
		
		
    }
	
	
}
