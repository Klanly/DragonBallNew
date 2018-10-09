using UnityEngine;
using System.Collections;

public abstract class EventReceiver 
{
    public EventReceiver()
    {
        Registe();
    }

    void Registe()
    {
        EventSender.onEvent += OnEvent;
    }

    protected abstract void OnEvent(EventTypeDefine p_e,object p_param);
}